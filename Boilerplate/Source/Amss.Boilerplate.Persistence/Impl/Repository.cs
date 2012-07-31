namespace Amss.Boilerplate.Persistence.Impl
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Data.Common;
    using Amss.Boilerplate.Persistence.Impl.Commands;
    using Amss.Boilerplate.Persistence.Impl.Configuration;

    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    using NHibernate;

    internal class Repository : IRepository
    {
        #region Fields

        private readonly Func<ISession> sessionProducer;

        #endregion

        #region Constructors

        public Repository(Func<ISession> sessionProducer)
        {
            Contract.Assert(sessionProducer != null);
            this.sessionProducer = sessionProducer;
        }

        #endregion

        #region Properties

        [Dependency]
        public IServiceLocator ServiceLocator { get; set; }

        private ISession Session
        {
            get
            {
                return this.sessionProducer();
            }
        }

        #endregion

        #region Methods

        public void Create<T>(T instance) where T : class, IEntity
        {
            this.Session.Save(instance);
        }

        public T Get<T>(long id) where T : class, IEntity
        {
            var instance = this.Session.Load<T>(id);
            return instance;
        }

        public T Load<T>(long id) where T : class, IEntity
        {
            var instance = this.Session.Get<T>(id);
            if (instance == null)
            {
                throw new Common.Exceptions.ObjectNotFoundException(id, typeof(T), null);
            }
            
            return instance;
        }

        public void Update<T>(T instance) where T : class, IEntity
        {
            this.Session.Update(instance);
        }

        public void Delete<T>(T instance) where T : class, IEntity
        {
            // want to explicitly flush here, because do not want suspended throw on transaction commit
            this.Delete(instance, true);
        }

        public int Count<T>(IQueryData queryData)
        {
            Contract.Assert(queryData != null);
            var command = this.ResolveCommand<T>(queryData);
            var count = command.RowCount(queryData);
            return count;
        }

        public IQueryable<T> Query<T>(IQueryData queryData)
        {
            Contract.Assert(queryData != null);
            var command = this.ResolveCommand<T>(queryData);
            var result = command.Execute(queryData);
            return result;
        }

        public void Flush(bool force = false)
        {
            if (force || this.Session.FlushMode == FlushMode.Commit)
            {
                this.Session.Flush();
            }
        }

        private void Delete<T>(T instance, bool flush) where T : class, IEntity
        {
            Contract.Assert(instance != null);
            try
            {
                this.Session.Delete(instance);
                if (flush)
                {
                    this.Flush(true);
                }
            }
            catch (Exception ex)
            {
                var rethrow = ExceptionPolicy.HandleException(ex, PersistenceContainerExtension.DeletePolicy);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private IQueryRepositoryCommand<T> ResolveCommand<T>(IQueryData commandData)
        {
            Contract.Assert(commandData != null);
            var type = typeof(IQueryRepositoryCommand<,>).MakeGenericType(commandData.GetType(), typeof(T));
            var command = (IQueryRepositoryCommand<T>)this.ServiceLocator.GetInstance(type);
            return command;
        }

        #endregion
    }
}