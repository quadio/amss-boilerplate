namespace Amss.Boilerplate.Common.Transactions
{
    using System;
    using System.Diagnostics.Contracts;

    using Microsoft.Practices.ServiceLocation;

    public class Transaction : IDisposable
    {
        #region Constants and Fields

        private bool disposed;

        private bool isCompleted;

        private IDisposable transaction;

        private ITransactionManager transactionManager;

        #endregion

        #region Constructors and Destructors

        public Transaction()
        {
            this.transactionManager = ServiceLocator.Current.GetInstance<ITransactionManager>();
            this.transaction = this.transactionManager.BeginTransaction();
        }

        #endregion

        #region Public Methods and Operators

        public void Complete()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("Transaction");
            }

            this.transactionManager.CommitTransaction(this.transaction);
            this.isCompleted = true;
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            if (!this.isCompleted)
            {
                this.transactionManager.RollbackTransaction(this.transaction);
            }

            this.transaction.Dispose();

            this.transaction = null;
            this.transactionManager = null;
            this.disposed = true;
        }

        #endregion
    }
}