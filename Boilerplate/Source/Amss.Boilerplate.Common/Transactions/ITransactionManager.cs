namespace Amss.Boilerplate.Common.Transactions
{
    using System;

    public interface ITransactionManager
    {
        #region Public Methods and Operators

        IDisposable BeginTransaction();

        void CommitTransaction(IDisposable transactionToken);

        void RollbackTransaction(IDisposable transactionToken);

        #endregion
    }
}