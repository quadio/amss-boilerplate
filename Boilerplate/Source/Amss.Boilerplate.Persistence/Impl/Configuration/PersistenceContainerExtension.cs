namespace Amss.Boilerplate.Persistence.Impl.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Common.Transactions;
    using Amss.Boilerplate.Common.Unity;
    using Amss.Boilerplate.Data.Common;
    using Amss.Boilerplate.Persistence.Impl.Commands;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
    using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
    using Microsoft.Practices.Unity;

    using NHibernate;

    using Unity.AutoRegistration;

    public class PersistenceContainerExtension : UnityContainerExtension
    {
        #region Fields

        public const string DeletePolicy = "Persistence.Delete";

        private readonly FlushMode flushMode;

        private readonly string connectionName;

        #endregion

        #region Constructors

        [InjectionConstructor]
        public PersistenceContainerExtension() : this(false, null)
        {
        }

        public PersistenceContainerExtension(bool batch, string connectionName)
        {
            // auto allows speed up mass create / update operations
            // but cause auto database update before each count operation,
            // so in auto mode we have problem with validation:
            // change value - validate - validate cause count() operation - auto flush occured - database constraint
            // that is why in application server we need auto mode
            this.flushMode = batch ? FlushMode.Auto : FlushMode.Commit;
            this.connectionName = connectionName;
        }

        #endregion

        #region Methods

        protected override void Initialize()
        {
            this.Container
                .RegisterType<DatabaseConfigurator, MsSql2005DatabaseConfigurator>("System.Data.SqlClient")
                .RegisterType<DatabaseConfigurator>(new InjectionFactory(this.CreateDatabaseConfigurator))
                .RegisterType<ISessionFactory>(
                    new ContainerControlledLifetimeManager(),
                    new InjectionFactory(CreateSessionFactory))
                .RegisterType<ISession>(new UnitOfWorkLifetimeManager(), new InjectionFactory(this.CreateSession))
                .RegisterType<Func<ISession>>(
                    new ContainerControlledLifetimeManager(), 
                    new InjectionFactory(
                        c =>
                        {
                            Func<ISession> func = () => c.Resolve<ISession>();
                            return func;
                        }))
                .RegisterType(typeof(IRepository), typeof(Repository), new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>();

            LoggerProvider.SetLoggersFactory(new NHibernate.Logging.CommonLogging.CommonLoggingLoggerFactory());

            this.RegisterCommands();

            this.ConfigureExceptionHandling();
        }

        private static ISessionFactory CreateSessionFactory(IUnityContainer container)
        {
            var configurator = container.Resolve<DatabaseConfigurator>();
            var sessionFactory = configurator.CreateSessionFactory();
            return sessionFactory;
        }

        private DatabaseConfigurator CreateDatabaseConfigurator(IUnityContainer container)
        {
            var currentConnectionName = this.connectionName;
            var configurationSource = ConfigurationSourceFactory.Create();
            if (string.IsNullOrEmpty(currentConnectionName))
            {
                currentConnectionName = DatabaseConfigurator.DefaultDatabaseConnectionName;
                var settings = DatabaseSettings.GetDatabaseSettings(configurationSource);
                if (settings != null)
                {
                    currentConnectionName = settings.DefaultDatabase;
                }
            }

            var section = (ConnectionStringsSection)configurationSource.GetSection("connectionStrings");
            Contract.Assert(section != null);
            var css = section.ConnectionStrings[currentConnectionName];
            Contract.Assert(css != null);
            Contract.Assert(!string.IsNullOrEmpty(css.ProviderName));
            var configurator = container.Resolve<DatabaseConfigurator>(css.ProviderName);
            configurator.ConnectionString = css.ConnectionString;
            return configurator;
        }
        
        private ISession CreateSession(IUnityContainer container)
        {
            var sessionFactory = container.Resolve<ISessionFactory>();
            var session = sessionFactory.OpenSession();
            Contract.Assert(session != null);
            session.FlushMode = this.flushMode;
            return session;
        }

        private void RegisterCommands()
        {
            this
                .ConfigureSelfAutoRegistration(typeof(ISpecification<>))
                .Include(
                    t => t.IsClass && !t.IsGenericType && !t.IsAbstract && t.Name.EndsWith(WellKnownAppParts.Command),
                    Then.Register().As(t => this.CommandToContract(t)))
                .Include(
                    t => t.IsClass && !t.IsGenericType && !t.IsAbstract && t.ImplementsOpenGeneric(typeof(ISpecification<>)),
                    this.RegisterSpecification)
                .ApplyAutoRegistration();
        }

        private Type[] CommandToContract(Type commandType)
        {
            var query = from t in commandType.GetInterfaces()
                        where t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IQueryRepositoryCommand<,>)
                        select t;
            var result = query.ToArray();
            return result;
        }

        private void RegisterSpecification(Type specificationType, IUnityContainer container)
        {
            var resulType =
                specificationType.GetInterfaces()
                .Single(
                    t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ISpecification<>))
                .GetGenericArguments()
                .Single();
            var queryDataType = specificationType;
            var interfaceType = typeof(IQueryRepositoryCommand<,>).MakeGenericType(queryDataType, resulType);
            var commandType = typeof(SpecificationCommand<>).MakeGenericType(resulType);
            container.RegisterType(interfaceType, commandType);
        }

        private void ConfigureExceptionHandling()
        {
            var configurationSource = new DictionaryConfigurationSource(); 

            var builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                   .GivenPolicyWithName(DeletePolicy)
                   .ForExceptionType<Exception>()
                        .ThenNotifyRethrow()
                   .ForExceptionType<NHibernate.Exceptions.ConstraintViolationException>()
                        .WrapWith<DeleteConstraintException>()
                        .UsingMessage("Cannot delete object.")
                        .ThenThrowNewException();
            builder.UpdateConfigurationWithReplace(configurationSource);

            var configurator = new UnityContainerConfigurator(this.Container);
            EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
        }

        #endregion
    }
}