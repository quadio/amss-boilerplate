namespace Amss.Boilerplate.Web.Areas.Main
{
    using System.Web.Mvc;

    using Amss.Boilerplate.Web.Common;

    public class MainAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Main"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(RouteHelper.Root, string.Empty, new { area = "Main", controller = "Home", action = "Index" });
            context.MapRoute(RouteHelper.Logon, "Login", new { area = "Main", controller = "Home", action = "Login" });
            context.MapRoute(RouteHelper.About, "About", new { area = "Main", controller = "Home", action = "About" });
            context.MapRoute(RouteHelper.Contact, "Contact", new { area = "Main", controller = "Home", action = "Contact" });

            context.MapRoute(
                "Main_default",
                "Main/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}
