namespace Amss.Boilerplate.Api.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Net;

    using Amss.Boilerplate.Business.Exceptions;
    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Initialization.Configuration;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using Microsoft.Practices.Unity;

    using ServiceStack.CacheAccess;
    using ServiceStack.CacheAccess.Providers;
    using ServiceStack.Common.Web;
    using ServiceStack.FluentValidation.Results;

    public class ApiContainerExtension : InitializationContainerExtension
    {
        #region Fields

        public const string DefaultPolicy = "Communication.SsApi";

        #endregion

        #region Methods

        protected override void Initialize()
        {
            base.Initialize();
            this.ConfigureExceptionHandling();

            this.Container.RegisterInstance<ICacheClient>(new MemoryCacheClient());
            new ApiAppHost(this.Container).Init();
        }

        private void ConfigureExceptionHandling()
        {
            var configurationSource = new DictionaryConfigurationSource();
            var builder = new ConfigurationSourceBuilder();
            builder
                .ConfigureExceptionHandling()
                .GivenPolicyWithName(DefaultPolicy)
                    .ForExceptionType<DeleteConstraintException>()
                        .HandleCustom<HttpErrorExceptionHandler>()
                        .ThenThrowNewException()
                    .ForExceptionType<BusinessValidationException>()
                        .HandleCustom<BusinessValidationExceptionHandler>()
                        .ThenThrowNewException()
                    .ForExceptionType<BusinessException>()
                        .HandleCustom<HttpErrorExceptionHandler>()
                        .ThenThrowNewException()
                    .ForExceptionType<Exception>()
                        .LogToCategory("General")
                            .WithSeverity(TraceEventType.Critical)
                            .UsingExceptionFormatter<TextExceptionFormatter>()
                        .HandleCustom(
                            typeof(HttpErrorExceptionHandler),
                            new NameValueCollection
                                {
                                    { HttpErrorExceptionHandler.StatusCodeKey, HttpStatusCode.InternalServerError.ToString("G") },
                                    { HttpErrorExceptionHandler.MessageKey, "An error has occurred while consuming this service. Please contact your administrator for more information." },
                                    { HttpErrorExceptionHandler.AppendHandlingIdKey, bool.TrueString }
                                })
                        .ThenThrowNewException();
            builder.UpdateConfigurationWithReplace(configurationSource);

            var configurator = new UnityContainerConfigurator(this.Container);
            EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
        }

        #endregion

        #region Nested types

        // ReSharper disable ClassNeverInstantiated.Local
        // ReSharper disable UnusedParameter.Local
        private class BusinessValidationExceptionHandler : IExceptionHandler
        {
            public BusinessValidationExceptionHandler(NameValueCollection collection)
            {
            }

            public Exception HandleException(Exception exception, Guid handlingInstanceId)
            {
                var e = exception as BusinessValidationException;
                if (e != null)
                {
                    var list = from error in e.Errors ?? new ValidationFailureInfo[0]
                               select new ValidationFailure(
                                   error.PropertyName,
                                   error.ErrorMessage,
                                   error.ErrorCode,
                                   error.AttemptedValue);
                    exception = new ServiceStack.FluentValidation.ValidationException(list);
                }

                return exception;
            }
        }

        private class HttpErrorExceptionHandler : IExceptionHandler
        {
            // ReSharper disable MemberCanBePrivate.Local
            protected internal const string StatusCodeKey = "StatusCode";

            protected internal const string MessageKey = "Message";

            protected internal const string ErrorCodeKey = "ErrorCode";

            protected internal const string AppendHandlingIdKey = "AppendHandlingId";
            
            // ReSharper restore MemberCanBePrivate.Local
            private readonly HttpStatusCode statusCode = HttpStatusCode.Conflict;

            private readonly string message;

            private readonly string errorCode;

            private readonly bool appendHandlingId;
            
            public HttpErrorExceptionHandler(NameValueCollection collection)
            {
                Contract.Assert(collection != null);
                if (collection.AllKeys.Contains(StatusCodeKey))
                {
                    this.statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), collection[StatusCodeKey]);
                }

                if (collection.AllKeys.Contains(MessageKey))
                {
                    this.message = collection[MessageKey];
                }

                if (collection.AllKeys.Contains(ErrorCodeKey))
                {
                    this.errorCode = collection[ErrorCodeKey];
                }

                if (collection.AllKeys.Contains(AppendHandlingIdKey))
                {
                    this.appendHandlingId = bool.Parse(collection[AppendHandlingIdKey]);
                }
            }

            public Exception HandleException(Exception exception, Guid handlingInstanceId)
            {
                var error = this.FormatErrorMessage(exception, handlingInstanceId);

                var result = new HttpError(error, exception)
                    {
                        StatusCode = this.statusCode
                    };
                if (!string.IsNullOrEmpty(this.errorCode))
                {
                    result.ErrorCode = this.errorCode;
                }

                return result;
            }

            private string FormatErrorMessage(Exception exception, Guid handlingInstanceId)
            {
                var pattern = this.message ?? exception.Message;

                if (this.appendHandlingId)
                {
                    pattern = string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}. [Error ID: {{handlingInstanceID}}]",
                        pattern.Trim(' ', '.'));
                }

                var error = ExceptionUtility.FormatExceptionMessage(pattern, handlingInstanceId);
                return error;
            }
        }

        // ReSharper restore UnusedParameter.Local
        // ReSharper restore ClassNeverInstantiated.Local
        #endregion
    }
}
