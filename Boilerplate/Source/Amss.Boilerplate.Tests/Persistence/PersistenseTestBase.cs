namespace Amss.Boilerplate.Tests.Persistence
{
    using Amss.Boilerplate.Common;
    using Amss.Boilerplate.Common.Transactions;
    using Amss.Boilerplate.Tests.Common;

    using Microsoft.Practices.ServiceLocation;

    using NHibernate;

    using NUnit.Framework;

    using QuickGenerate.Primitives;

    public abstract class PersistenseTestBase : TestBase
    {
        #region Constants and Fields

        protected readonly StringGenerator ShortStringGenerator = new StringGenerator(5, 20);

        private Transaction transaction;

        private UnitOfWork unitOfWork;

        #endregion

        #region Public Properties

        public ISession Session { get; set; }

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public virtual void SetUp()
        {
            this.unitOfWork = new UnitOfWork();
            this.transaction = new Transaction();
            this.Session = ServiceLocator.Current.GetInstance<ISession>();
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.transaction.Dispose();
            this.unitOfWork.Dispose();
        }

        #endregion
    }
}