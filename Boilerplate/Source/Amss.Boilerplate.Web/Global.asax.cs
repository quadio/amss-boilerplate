namespace Amss.Boilerplate.Web
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.Caching;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;

    using Amss.Boilerplate.Business;
    using Amss.Boilerplate.Common;
    using Amss.Boilerplate.Common.Security;
    using Amss.Boilerplate.Web.App_Start;
    using Amss.Boilerplate.Web.Areas.Main.Controllers;
    using Amss.Boilerplate.Web.Common;
    using Amss.Boilerplate.Web.Common.Controllers;
    using Amss.Boilerplate.Web.Common.Data;
    using Amss.Boilerplate.Web.Configuration;

    using global::Common.Logging;

    using Microsoft.Practices.ServiceLocation;

    using Mvc.JQuery.Datatables;

    public class MvcApplication : HttpApplication
    {
        #region Fields

        private const string UnitOfWorkKey = "uow";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #endregion

        #region Methods

        protected void Application_Start()
        {
            Shell.Start<WebContainerExtension>();

            DependencyResolver.SetResolver(ServiceLocator.Current);
            ControllerBuilder.Current.SetControllerFactory(typeof(CustomControllerFactory));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(DataTablesParam), new DataTablesModelBinder());
            MvcHandler.DisableMvcResponseHeader = true;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            this.Context.Items.Add(UnitOfWorkKey, new UnitOfWork());
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            this.Context.User = this.GetPrincipal();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var uow = (UnitOfWork)this.Context.Items[UnitOfWorkKey];
            Contract.Assert(uow != null);
            uow.Dispose();
            this.Context.Items.Remove(UnitOfWorkKey);
        }

        protected void Application_Error()
        {
            var error = Server.GetLastError();
            try
            {
                Log.ErrorFormat(
                    CultureInfo.InvariantCulture, 
                    "Server error has been occured while processing page: {0} ", 
                    error,
                    HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : "unknown");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            this.HandleCustomErrors(error);
        }

        protected void Application_End()
        {
            Shell.Shutdown();
        }

        private IPrincipal GetPrincipal()
        {
            IPrincipal principal = ApplicationPrincipal.Anonymous;
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (!authTicket.Expired)
                {
                    var login = authTicket.Name;
                    var cache = ServiceLocator.Current.GetInstance<ObjectCache>();

                    // TODO: if we going to use login as cache key we should not allow to change login?!
                    var session = cache.Get(login) as PrincipalSession;
                    if (session == null)
                    {
                        var manager = ServiceLocator.Current.GetInstance<IUserManager>();
                        var user = manager.FindByLogin(login);

                        if (user != null && user.UserPasswordCredential != null)
                        {
                            session = user.Convert();
                            cache.Add(
                                login, 
                                session, 
                                new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, 0, 60) });
                        }
                    }

                    if (session != null)
                    {
                        principal = session.Convert();
                    }
                }
            }

            return principal;
        }

        private void HandleCustomErrors(Exception exception)
        {
            var httpException = exception as HttpException;

            if (httpException != null)
            {
                var status = httpException.GetHttpCode();

                if (status == 404 || status == 403)
                {
                    var routeData = new RouteData();

                    RouteHelper.InitErrorRoute(status, routeData);

                    // Clear the error on server.
                    Server.ClearError();

                    // Avoid IIS7 getting in the middle
                    Response.StatusCode = status;
                    Response.TrySkipIisCustomErrors = true;

                    // idially we should get controller throught servicelocator
                    IController errorController = new ErrorController();
                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
        }

        #endregion
    }
}