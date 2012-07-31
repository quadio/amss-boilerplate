namespace Amss.Boilerplate.Web.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Runtime.Caching;

    using Amss.Boilerplate.Business.Exceptions;
    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Initialization.Configuration;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using Microsoft.Practices.Unity;

    public class WebContainerExtension : InitializationContainerExtension
    {
        #region Fields

        public const string DefaultPolicy = "Communication.Web";

        public const string ValidationPolicy = "Communication.Web.Validation";

        #endregion

        #region Methods

        protected override void Initialize()
        {
            base.Initialize();

            this.Container.RegisterInstance<ObjectCache>(MemoryCache.Default);
            this.ConfigureExceptionHandling();
        }

        private void ConfigureExceptionHandling()
        {
            var configurationSource = new DictionaryConfigurationSource();
            var builder = new ConfigurationSourceBuilder();
            builder
                .ConfigureExceptionHandling()
                .GivenPolicyWithName(ValidationPolicy)
                    .ForExceptionType<DeleteConstraintException>()
                        .HandleCustom<BusinessValidationHandler>()
                        .ThenThrowNewException()
                    .ForExceptionType<BusinessValidationException>()
                        .ThenNotifyRethrow()
                    .ForExceptionType<BusinessException>()
                        .HandleCustom<BusinessValidationHandler>()
                        .ThenThrowNewException()
                .GivenPolicyWithName(DefaultPolicy)
                    .ForExceptionType<Exception>()
                        .LogToCategory("General")
                            .WithSeverity(TraceEventType.Critical)
                            .UsingExceptionFormatter<TextExceptionFormatter>()
                        .WrapWith<Exception>()
                            .UsingMessage("An error has occurred while processing request. Please contact your administrator for more information. [Error ID: {handlingInstanceID}]")
                        .ThenThrowNewException();
            builder.UpdateConfigurationWithReplace(configurationSource);

            var configurator = new UnityContainerConfigurator(this.Container);
            EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
        }

        #endregion

        #region Nested types
        // ReSharper disable UnusedParameter.Local
        internal class BusinessValidationHandler : IExceptionHandler
        {
            public BusinessValidationHandler(NameValueCollection collection)
            {
            }

            public Exception HandleException(Exception exception, Guid handlingInstanceId)
            {
                var result = new BusinessValidationException(exception.Message, exception);
                return result;
            }
        }
        // ReSharper restore UnusedParameter.Local
        #endregion
    }
}
