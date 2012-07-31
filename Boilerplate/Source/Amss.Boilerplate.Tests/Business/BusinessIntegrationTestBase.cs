namespace Amss.Boilerplate.Tests.Business
{
    using Amss.Boilerplate.Common;
    using Amss.Boilerplate.Common.Transactions;
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Tests.Common;

    using Microsoft.Practices.ServiceLocation;

    using NHibernate;

    using NUnit.Framework;

    using QuickGenerate.Primitives;

    public abstract class BusinessIntegrationTestBase : TestBase
    {
        #region Constants and Fields

        protected readonly StringGenerator ShortStringGenerator = new StringGenerator(5, MetadataInfo.StringShort);

        protected readonly StringGenerator NormalStringGenerator = new StringGenerator(5, MetadataInfo.StringNormal);

        private Transaction transaction;

        private UnitOfWork unitOfWork;

        #endregion

        #region Public Properties

        public ISession Session { get; set; }

        public IServiceLocator Locator 
        { 
            get
            {
                return ServiceLocator.Current;
            } 
        }

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

        protected void ClearSession(params object[] instances)
        {
            var session = this.Locator.GetInstance<ISession>();
            session.Flush();
            foreach (var instance in instances)
            {
                session.Evict(instance);
            }
        }

        #endregion
    }
}