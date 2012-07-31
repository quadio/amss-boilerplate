namespace Amss.Boilerplate.Initialization.Configuration
{
    using System.Diagnostics;

    using Amss.Boilerplate.Business.Impl.Configuration;
    using Amss.Boilerplate.Persistence.Impl.Configuration;

    using Common.Logging.EntLib;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.Unity;

    public class InitializationContainerExtension : UnityContainerExtension
    {
        #region Methods

        protected override void Initialize()
        {
            this.Container
                .AddNewExtension<PersistenceContainerExtension>()
                .AddNewExtension<BusinessContainerExtension>();
        
            this.ConfigureLogging();
        }

        private void ConfigureLogging()
        {
            var configurationSource = new DictionaryConfigurationSource();
            var builder = new ConfigurationSourceBuilder();
            const string DefaultListenerName = "Default";

            builder.ConfigureLogging()
                .WithOptions
                    .DoNotRevertImpersonation()
                .SpecialSources
                    .LoggingErrorsAndWarningsCategory
                        .SendTo.SharedListenerNamed(DefaultListenerName)
                .SpecialSources
                    .UnprocessedCategory
                        .SendTo.SharedListenerNamed(DefaultListenerName)
                .SpecialSources
                    .AllEventsCategory
                        .SendTo.SharedListenerNamed(DefaultListenerName)
                .LogToCategoryNamed("General")
                    .WithOptions.SetAsDefaultCategory()
                    .SendTo.SharedListenerNamed(DefaultListenerName);
            builder.UpdateConfigurationWithReplace(configurationSource);

            var configurator = new UnityContainerConfigurator(this.Container);
            EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);

            this.Container.RegisterType<TraceListener, CommonLoggingEntlibTraceListener>(
                DefaultListenerName,
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(this.CreateListener));
        }

        private TraceListener CreateListener(IUnityContainer c)
        {
            var formatter = new TextFormatter("{message}{dictionary({key} - {value}{newline})}");
            var data = new CommonLoggingEntlibTraceListenerData(this.GetType().FullName, "{listenerName}.{sourceName}", "Text Formatter");
            var listener = new CommonLoggingEntlibTraceListener(data, formatter);
            return listener;
        }

        #endregion
    }
}
