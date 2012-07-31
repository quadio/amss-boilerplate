namespace Amss.Boilerplate.Web.Common.Controllers
{
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    public class Controller<TManager> : Controller
        where TManager : class
    {
        [Dependency]
        public TManager Manager { get; set; }
    }
}