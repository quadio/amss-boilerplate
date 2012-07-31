namespace Amss.Boilerplate.Persistence.Impl
{
    using System;
    using System.Data;
    using System.Diagnostics.Contracts;

    using Amss.Boilerplate.Common.Transactions;

    using NHibernate;
    using NHibernate.Transaction;

    internal class TransactionManager : ITransactionManager
    {
        #region Constructors

        public TransactionManager(Func<ISession> getSession)
        {
            Contract.Assert(getSession != null);
            this.GetSession = getSession;
        }

        #endregion

        #region Properties

        private bool IsActiveTransaction
        {
            get
            {
                var transaction = this.GetSession().Transaction;
                return transaction != null && transaction.IsActive;
            }
        }

        private Func<ISession> GetSession { get; set; }

        #endregion

        #region Methods

        public IDisposable BeginTransaction()
        {
            var session = this.GetSession();
            return !this.IsActiveTransaction
                ? session.BeginTransaction()
                : new NestedTransaction(session);
        }

        public void CommitTransaction(IDisposable transactionToken)
        {
            Contract.Assert(transactionToken != null);
            var transaction = transactionToken as ITransaction;
            Contract.Assert(transaction != null);
            transaction.Commit();
        }

        public void RollbackTransaction(IDisposable transactionToken)
        {
            Contract.Assert(transactionToken != null);
            var transaction = transactionToken as ITransaction;
            Contract.Assert(transaction != null);
            if (!transaction.WasRolledBack)
            {
                transaction.Rollback();
            }
        }

        #endregion

        #region Nested Types

        private class NestedTransaction : ITransaction
        {
            #region Fields

            private readonly ISession session;

            #endregion

            #region Constructors

            public NestedTransaction(ISession session)
            {
                this.session = session;
            }

            #endregion

            #region Properties

            public bool IsActive
            {
                get
                {
                    return !this.WasCommitted
                        && !this.WasRolledBack
                        && this.session.Transaction != null
                        && this.session.Transaction.IsActive;
                }
            }

            public bool WasRolledBack { get; private set; }

            public bool WasCommitted { get; private set; }

            #endregion

            #region Methods

            public void Dispose()
            {
            }

            public void Begin()
            {
                throw new NotSupportedException();
            }

            public void Begin(IsolationLevel isolationLevel)
            {
                throw new NotSupportedException();
            }

            public void Commit()
            {
                this.session.Flush();
                this.WasCommitted = true;
            }

            public void Rollback()
            {
                this.session.Transaction.Rollback();
                this.WasRolledBack = true;
            }

            public void Enlist(IDbCommand command)
            {
                throw new NotSupportedException();
            }

            public void RegisterSynchronization(ISynchronization synchronization)
            {
                throw new NotSupportedException();
            }

            #endregion
        }

        #endregion
    }
}