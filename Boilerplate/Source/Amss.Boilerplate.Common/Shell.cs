namespace Amss.Boilerplate.Common
{
    using System;
    using System.Globalization;

    using Amss.Boilerplate.Common.Configuration;

    using global::Common.Logging;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    public static class Shell
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(Shell));

        private static readonly object LockObject = new object();

        private static volatile bool isInitialized;

        #endregion

        #region Properties

        internal static IServiceLocator ServiceLocator
        {
            get
            {
                Start();
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
            }
        }

        #endregion

        #region Methods

        public static void Start<T>() where T : UnityContainerExtension, new()
        {
            var initialization = new T();
            Start(initialization);
        }

        public static void Start(UnityContainerExtension initialization = null)
        {
            if (!isInitialized)
            {
                lock (LockObject)
                {
                    if (!isInitialized)
                    {
                        ConfigureUnity(initialization);
                        isInitialized = true;
                    }
                }
            }
        }

        public static void Shutdown()
        {
            if (isInitialized)
            {
                lock (LockObject)
                {
                    if (isInitialized)
                    {
                        DestroyUnity();
                        isInitialized = false;
                    }
                }
            }
        }

        public static void Restart()
        {
            Shutdown();
            Start();
        }

        private static void ConfigureUnity(UnityContainerExtension initialization)
        {
            Logger.InfoFormat("Starting the server [{0}]", typeof(Shell).Assembly.EffectiveVersion());
            try
            {
                var configurationSource = ConfigurationSourceFactory.Create();
                var container = new UnityContainer()
                    .AddExtension(new EnterpriseLibraryCoreExtension(configurationSource))
                    .RegisterInstance(configurationSource);

                var serviceLocator = new UnityServiceLocator(container);
                Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => serviceLocator);
                EnterpriseLibraryContainer.Current = serviceLocator;

                if (initialization != null)
                {
                    container.AddExtension(initialization);
                }

                var section = (UnityConfigurationSection)configurationSource.GetSection("unity");
                if (section != null)
                {
                    section.Configure(container);
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal("Unexpected error while container initialization", ex);
                throw;
            }
        }

        private static void DestroyUnity()
        {
            Logger.Info(string.Format(CultureInfo.InvariantCulture, "Stopping the server [{0}]", typeof(Shell).Assembly.EffectiveVersion()));
            try
            {
                var container = EnterpriseLibraryContainer.Current.GetInstance<IUnityContainer>();
                container.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Fatal("Unexpected error while disposing container", ex);
                throw;
            }
        }

        #endregion
    }
}