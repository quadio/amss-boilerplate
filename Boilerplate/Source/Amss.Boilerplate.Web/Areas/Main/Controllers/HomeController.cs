namespace Amss.Boilerplate.Web.Areas.Main.Controllers
{
    using System.Web;
    using System.Web.Mvc;

    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Web.Areas.Main.Models;
    using Amss.Boilerplate.Web.Common.Controllers;
    using Amss.Boilerplate.Web.Common.Filters;

    public class HomeController : Controller<HomeManager>
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;

            new HttpException(501, "ddd");

            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [BusinessValidationFilter]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                if (this.Manager.Login(model))
                {
                    if (this.Url.IsLocalUrl(returnUrl))
                    {
                        return this.Redirect(returnUrl);
                    }

                    return this.RedirectToAction("Index", "Home");
                }

                this.ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            }

            return this.View(model);
        }

        public ActionResult LogOff()
        {
            this.Manager.Logout();

            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult Index()
        {
            this.ViewBag.Message = "Modify this template to kick-start your ASP.NET MVC application.";
            return this.View();
        }

        [PrincipalRightPermission(AccessRight.Admin, AccessRight.SuperAdmin)]
        public ActionResult About()
        {
            this.ViewBag.Message = "Your app description page.";

            return this.View();
        }

        [PrincipalRightPermission(AccessRight.SuperAdmin)]
        public ActionResult Contact()
        {
            this.ViewBag.Message = "Your contact page.";

            return this.View();
        }
    }
}
