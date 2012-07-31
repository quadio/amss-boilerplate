namespace Amss.Boilerplate.Web.Common.Filters
{
    using System.Web.Mvc;

    using Amss.Boilerplate.Business.Exceptions;
    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Web.Configuration;

    // we could use Controller.OnException but I havn't found the way how to know what view is being rendered if it's not the same as action
    internal class BusinessValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly string view;

        public BusinessValidationFilterAttribute()
            : this(null)
        {
        }

        public BusinessValidationFilterAttribute(string view)
        {
            this.view = view;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var transformedException = filterContext.Exception.TransformException(WebContainerExtension.ValidationPolicy);
            var exception = transformedException as BusinessValidationException;
            if (exception != null)
            {
                // put error into ViewData for custom handling - it's not used right now
                filterContext.Controller.ViewData.Add("Error", exception.Errors);

                filterContext.Controller.ViewData.ModelState.FillFrom(exception);

                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult
                    {
                        // if view is not defined - use action name by default
                        ViewName = this.view ?? filterContext.RouteData.Values["action"].ToString(),
                        TempData = filterContext.Controller.TempData,
                        ViewData = filterContext.Controller.ViewData
                    };
            }
        }
    }
}