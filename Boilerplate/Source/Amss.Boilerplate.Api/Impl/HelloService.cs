namespace Amss.Boilerplate.Api.Impl
{
    using Amss.Boilerplate.Api.Common;
    using Amss.Boilerplate.Api.Data;
    using Amss.Boilerplate.Business;
    using Amss.Boilerplate.Common.Security;
    using Amss.Boilerplate.Data;

    using Microsoft.Practices.Unity;

    using ServiceStack.Common.Web;
    using ServiceStack.ServiceInterface;

    [Authenticate]
    [RequiredPermission(AccessRightRegistry.Admin)]
    [UnitOfWork]
    internal class HelloService : AppRestServiceBase<Hello>
    {
        #region Properties

        [Dependency]
        public IUserManager UserManager { get; set; }

        [Dependency]
        public IApplicationPrincipal Principal { get; set; }

        #endregion

        #region Methods

        // Get
        public override object OnGet(Hello request)
        {
            var user = this.UserManager.Load(this.Principal.Identity.UserId);

            var name = this.Principal.Identity.DisplayName + " " + user.Email;
            if (string.IsNullOrEmpty(request.Name))
            {
                return new HelloResponse { Result = "Hello from " + name };
            }

            return new HelloResponse { Result = "Hello, " + request.Name + " from " + name };
        }

        // Add
        public override object OnPost(Hello request)
        {
            return new HttpError(System.Net.HttpStatusCode.Conflict, "SomeAddErrorCode");
        }

        // Update
        public override object OnPut(Hello request)
        {
            return new HttpError(System.Net.HttpStatusCode.Conflict, "SomeUpdateErrorCode");
        }

        // Delete
        public override object OnDelete(Hello request)
        {
            throw new HttpError(System.Net.HttpStatusCode.Conflict, "SomeDeleteErrorCode");
        }

        #endregion
    }
}