namespace Amss.Boilerplate.Api.Configuration
{
    using System.Globalization;

    using Amss.Boilerplate.Api.Common.Adapters;
    using Amss.Boilerplate.Common.Configuration;

    using Funq;

    using Microsoft.Practices.Unity;

    using ServiceStack.Logging;
    using ServiceStack.ServiceHost;
    using ServiceStack.ServiceInterface;
    using ServiceStack.ServiceInterface.Auth;
    using ServiceStack.WebHost.Endpoints;

    internal class ApiAppHost : AppHostBase
    {
        #region Constants and Fields

        private readonly IUnityContainer unityContainer;

        #endregion

        #region Constructors and Destructors

        public ApiAppHost(IUnityContainer unityContainer)
            : base(
            string.Format(CultureInfo.InvariantCulture, "Amss.Boilerplate Web Services {0}", typeof(ApiAppHost).Assembly.EffectiveVersion()), 
            typeof(ApiAppHost).Assembly)
        {
            this.unityContainer = unityContainer;
            LogManager.LogFactory = new ServiceStackLogFactoryAdapter();
        }

        #endregion

        #region Public Methods and Operators

        public override void Configure(Container container)
        {
            this.ConfigureInternals(container);
            this.ConfigureAuthentication();
        }

        #endregion

        #region Methods

        private void ConfigureInternals(Container container)
        {
            container.Adapter = new ServiceStackContainerAdapter(this.unityContainer);
            this.SetConfig(
                new EndpointHostConfig
                {
                    EnableFeatures = Feature.Metadata | Feature.Json | Feature.Html,
                    AllowJsonpRequests = true,
#if DEBUG
                    DebugMode = true
#endif
                });
        }

        private void ConfigureAuthentication()
        {
            var authProviders = new IAuthProvider[]
                {
                    new ServiceStackCredentialsAuthAdapter(this.unityContainer)
                };
            var authFeature = new AuthFeature(() => new AuthUserSession(), authProviders)
                {
                   IncludeAssignRoleServices = false 
                };
            this.Plugins.Add(authFeature);
        }

        #endregion
    }
}