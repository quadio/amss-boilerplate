namespace Amss.Boilerplate.Persistence.Impl.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;

    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Persistence.Impl.Configuration.Conventions;

    using global::Common.Logging;

    using FluentNHibernate.Automapping;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Diagnostics;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

    using NHibernate;
    using NHibernate.Tool.hbm2ddl;

    internal abstract class DatabaseConfigurator
    {
        #region Constants and Fields

        internal const string DefaultDatabaseConnectionName = "Default";

        private string connectionString;

        #endregion

        #region Public Properties

        public string ConnectionString
        {
            get
            {
                return this.connectionString ?? (this.connectionString = DefaultConnectionString);
            }

            set
            {
                this.connectionString = value;
            }
        }

        #endregion

        #region Properties

        protected static string DefaultConnectionString
        {
            get
            {
                string defaultDatabase = DefaultDatabaseConnectionName;

                var configurationSource = ConfigurationSourceFactory.Create();
                var settings = DatabaseSettings.GetDatabaseSettings(configurationSource);
                if (settings != null)
                {
                    defaultDatabase = settings.DefaultDatabase;
                }

                var section = (ConnectionStringsSection)configurationSource.GetSection("connectionStrings");
                Contract.Assert(section != null);
                var css = section.ConnectionStrings[defaultDatabase];
                Contract.Assert(css != null);
                return css.ConnectionString;
            }
        }

        #endregion

        #region Public Methods and Operators

        public virtual void CreateDatabase()
        {
            var fluentConfiguration = this.CreateProductionSchema();
            var configuration = fluentConfiguration.BuildConfiguration();

            new SchemaExport(configuration).Create(false, true);
        }

        public ISessionFactory CreateSessionFactory()
        {
            var configuration = this.CreateProductionSchema();
            var result = configuration.BuildSessionFactory();
            return result;
        }

        public abstract bool DatabaseExists();

        public void ExportSqlSchema(string file)
        {
            var fluentConfiguration = this.CreateProductionSchema();
            var configuration = fluentConfiguration.BuildConfiguration();

            new SchemaExport(configuration).SetOutputFile(file).Create(true, false);
        }

        #endregion

        #region Methods

        protected abstract IPersistenceConfigurer CreatePersistenceConfigurator();

        protected virtual FluentConfiguration CreateProductionSchema()
        {
            var cfg = new AutomappingConfiguration();
            var configuration = Fluently.Configure()
                .Diagnostics(d => d.RegisterListener(new CommonLoggingDiagnosticListener()).Enable())
                .Database(this.CreatePersistenceConfigurator())
                .Mappings(
                    m => m
                        .AutoMappings.Add(
                        () => AutoMap
                                  .AssemblyOf<BaseEntity>(cfg)
                                  .UseOverridesFromAssemblyOf<TableNameConvention>()
                                  .Conventions
                                  .AddFromAssemblyOf<TableNameConvention>()))
                .Cache(
                    c => c
                        .ProviderClass<NHibernate.Cache.HashtableCacheProvider>()
                        .UseQueryCache())
                .ExposeConfiguration(
                        config =>
                            {
                                config.SetInterceptor(new AuditInterceptor());
                                var type = this.GetSqlExceptionConverterType();
                                if (type != null)
                                {
                                    config.SetProperty(
                                        NHibernate.Cfg.Environment.SqlExceptionConverter, 
                                        type.AssemblyQualifiedName);
                                }
                            });
            return configuration;
        }

        protected virtual Type GetSqlExceptionConverterType()
        {
            return null;
        }

        protected void RuntimeConnectionString(ConnectionStringBuilder connectionStringBuilder)
        {
            connectionStringBuilder.Is(this.ConnectionString);
        }

        #endregion

        #region Nested types

        internal class CommonLoggingDiagnosticListener : IDiagnosticListener
        {
            private readonly IDiagnosticResultsFormatter formatter;

            private readonly ILog logger = LogManager.GetLogger("NHibernate.Automapper");

            public CommonLoggingDiagnosticListener()
                : this(new DefaultOutputFormatter())
            {
            }

            public CommonLoggingDiagnosticListener(IDiagnosticResultsFormatter formatter)
            {
                this.formatter = formatter;
            }

            public void Receive(DiagnosticResults results)
            {
                var output = this.formatter.Format(results);
                this.logger.Debug(output);
            }
        }

        #endregion
    }
}