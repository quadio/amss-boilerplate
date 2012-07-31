namespace Amss.Boilerplate.Persistence.Impl.Configuration
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;

    using FluentNHibernate.Cfg.Db;

    using NHibernate;
    using NHibernate.Exceptions;

    internal class MsSql2005DatabaseConfigurator : DatabaseConfigurator
    {
        public override void CreateDatabase()
        {
            Trace.WriteLine("Database creation not supported");
        }

        public override bool DatabaseExists()
        {
            return true;
        }

        protected override IPersistenceConfigurer CreatePersistenceConfigurator()
        {
            var configuration = MsSqlConfiguration.MsSql2005
                .DefaultSchema("bplt")
                .ConnectionString(this.RuntimeConnectionString);
            return configuration;
        }

        protected override Type GetSqlExceptionConverterType()
        {
            return typeof(MsSqlExceptionConverter);
        }

        protected class MsSqlExceptionConverter : ISQLExceptionConverter
        {
            public Exception Convert(AdoExceptionContextInfo contextInfo)
            {
                Exception result = null;
                var sqle = ADOExceptionHelper.ExtractDbException(contextInfo.SqlException) as SqlException;
                if (sqle != null)
                {
                    switch (sqle.Number)
                    {
                        case 547:
                            result = new ConstraintViolationException(
                                sqle.Message,
                                sqle, 
                                contextInfo.Sql, 
                                null);
                            break;
                        case 208:
                            result = new SQLGrammarException(
                                contextInfo.Message,
                                sqle, 
                                contextInfo.Sql);
                            break;
                        case 3960:
                            result = new StaleObjectStateException(
                                contextInfo.EntityName, 
                                contextInfo.EntityId);
                            break;
                    }
                }

                return result ?? SQLStateConverter.HandledNonSpecificException(
                    contextInfo.SqlException,
                    contextInfo.Message, 
                    contextInfo.Sql);
            }
        }
    }
}