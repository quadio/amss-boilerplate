namespace Amss.Boilerplate.Common
{
    using System;

    using Amss.Boilerplate.Common.Unity;

    public sealed class UnitOfWork : IDisposable
    {
        #region Constructors

        public UnitOfWork()
        {
            UnitOfWorkLifetimeManager.Enable();
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            UnitOfWorkLifetimeManager.Clear();
            UnitOfWorkLifetimeManager.Disable();
        }

        #endregion
    }
}