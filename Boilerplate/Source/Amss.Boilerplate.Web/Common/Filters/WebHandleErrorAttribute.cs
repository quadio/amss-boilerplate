namespace Amss.Boilerplate.Web.Common.Filters
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;

    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Web.Configuration;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    internal class WebHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Contract.Assert(filterContext != null);
            if (filterContext.IsChildAction)
            {
                return;
            }

            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            var exception = filterContext.Exception;
            if (!this.ExceptionType.IsInstanceOfType(exception))
            {
                return;
            }

            var handledException = exception.TransformException(WebContainerExtension.DefaultPolicy);

            // avoid showing death screen even for not 500 error in production
            if (new HttpException(null, exception).GetHttpCode() != 500)
            {
                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(handledException, controllerName, actionName);
                filterContext.Result = new ViewResult
                    {
                        // if view is not defined - use action name by default
                        ViewName = "Error",
                        ViewData = new ViewDataDictionary(model)
                    };
                filterContext.ExceptionHandled = true;

                return;
            }

            filterContext.Exception = handledException;

            base.OnException(filterContext);
        }
    }
}