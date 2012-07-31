namespace Amss.Boilerplate.Web.Services
{
    using System.Data.Services;
    using System.Data.Services.Common;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;

    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Web.Configuration;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ClientApi : DataService<ApiContext>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("Users", EntitySetRights.AllRead);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }

        protected override void HandleException(HandleExceptionArgs args)
        {
            Contract.Assert(args != null);
            Contract.Assert(args.Exception != null);

            var handledException = args.Exception.TransformException(WebContainerExtension.DefaultPolicy);

            throw handledException;
        }
        
        protected override ApiContext CreateDataSource()
        {
            var context = base.CreateDataSource();

            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            container.BuildUp(context);

            return context;
        }
    }
}
