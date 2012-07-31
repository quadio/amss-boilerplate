namespace Amss.Boilerplate.Tests.Common
{
    using Amss.Boilerplate.Common;
    using Amss.Boilerplate.Initialization.Configuration;

    using NUnit.Framework;

    public abstract class TestBase
    {
        [TestFixtureSetUp]
        public virtual void Initialize()
        {
            NoCategoryTraceListener.Install();
            Shell.Start<InitializationContainerExtension>();
        }

        [TestFixtureTearDown]
        public virtual void Deinitialize()
        {
            Shell.Shutdown();
        }
    }
}