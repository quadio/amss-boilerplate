namespace Amss.Boilerplate.Web.Areas.Main.Controllers
{
    using System.Runtime.Caching;
    using System.Web.Security;

    using Amss.Boilerplate.Business;
    using Amss.Boilerplate.Common.Security;
    using Amss.Boilerplate.Web.Areas.Main.Models;

    using Microsoft.Practices.Unity;

    public class HomeManager
    {
        [Dependency]
        public IUserManager UserManager { get; set; }

        [Dependency]
        public IApplicationPrincipal Principal { get; set; }

        [Dependency]
        public ObjectCache Cache { get; set; }

        public bool Login(LoginModel model)
        {
            var user = this.UserManager.FindByPasswordCredential(model.UserName, model.Password);

            var succeed = user != null;

            if (succeed)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
            }

            return succeed;
        }

        public void Logout()
        {
            this.Cache.Remove(this.Principal.Identity.Name);

            FormsAuthentication.SignOut();
        }
    }
}