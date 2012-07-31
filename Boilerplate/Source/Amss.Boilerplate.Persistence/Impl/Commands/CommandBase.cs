namespace Amss.Boilerplate.Persistence.Impl.Commands
{
    using System.Linq;

    using Amss.Boilerplate.Data.Common;

    using Microsoft.Practices.Unity;

    using NHibernate;

    internal abstract class CommandBase<TQueryData, TResult> : IQueryRepositoryCommand<TQueryData, TResult>
        where TQueryData : IQueryData<TResult>
    {
        #region Public Properties

        [Dependency]
        public ISession Session { get; set; }

        #endregion

        #region Public Methods and Operators

        public abstract IQueryable<TResult> Execute(TQueryData queryData);
        
        public abstract int RowCount(TQueryData queryData);

        #endregion

        #region Explicit Interface Methods

        IQueryable<TResult> IQueryRepositoryCommand<TResult>.Execute(IQueryData queryData)
        {
            return this.Execute((TQueryData)queryData);
        }

        int IQueryRepositoryCommand<TResult>.RowCount(IQueryData queryData)
        {
            return this.RowCount((TQueryData)queryData);
        }

        #endregion
    }
}