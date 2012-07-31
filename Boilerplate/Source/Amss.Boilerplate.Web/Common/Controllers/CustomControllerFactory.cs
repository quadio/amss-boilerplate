namespace Amss.Boilerplate.Web.Common.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Amss.Boilerplate.Web.Areas.Main.Controllers;

    using global::Common.Logging;

    internal class CustomControllerFactory : DefaultControllerFactory
    {
        #region Constants and Fields

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #endregion

        #region Methods

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            IController controller;
            try
            {
                controller = base.GetControllerInstance(requestContext, controllerType);
            }
            catch (HttpException exc)
            {
                var httpCode = exc.GetHttpCode();
                if (httpCode == 404)
                {
                    Log.Error("Cannot instantiate mvc controller.", exc);

                    controller = base.GetControllerInstance(requestContext, typeof(ErrorController));

                    requestContext.RouteData.Values.Clear();

                    RouteHelper.InitErrorRoute(httpCode, requestContext.RouteData);
                }
                else
                {
                    throw;
                }
            }

            return controller;
        }

        #endregion
    }
}