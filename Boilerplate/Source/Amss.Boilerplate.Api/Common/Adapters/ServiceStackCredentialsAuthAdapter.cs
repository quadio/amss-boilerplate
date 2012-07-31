namespace Amss.Boilerplate.Api.Common.Adapters
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Amss.Boilerplate.Business;
    using Amss.Boilerplate.Common;

    using Microsoft.Practices.Unity;

    using ServiceStack.ServiceInterface;
    using ServiceStack.ServiceInterface.Auth;

    internal class ServiceStackCredentialsAuthAdapter : CredentialsAuthProvider
    {
        #region Constants and Fields

        private readonly IUnityContainer container;

        #endregion

        #region Constructors and Destructors

        public ServiceStackCredentialsAuthAdapter(IUnityContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public Methods and Operators

        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            var session = authService.GetSession();
            var userAuth = this.FindByPasswordCredential(userName, password);
            if (userAuth != null)
            {
                session.UserName = userAuth.UserName;
                session.Email = userAuth.Email;
                session.DisplayName = userAuth.DisplayName;
                session.Permissions = userAuth.Permissions;
                session.Roles = userAuth.Roles;
                session.IsAuthenticated = true;
                session.UserAuthId = userAuth.RefIdStr;
            }

            return userAuth != null;
        }

        #endregion

        #region Methods

        private UserAuth FindByPasswordCredential(string login, string password)
        {
            UserAuth userAuth = null;
            using (new UnitOfWork())
            {
                var manager = this.container.Resolve<IUserManager>();
                var user = manager.FindByPasswordCredential(login, password);
                if (user != null)
                {
                    userAuth = new UserAuth
                        {
                            RefIdStr = user.Id.ToString(CultureInfo.InvariantCulture), 
                            UserName = login, 
                            Email = user.Email, 
                            DisplayName = user.Name, 
                            Roles = new List<string>(), 
                            Permissions = new List<string>(), 
                        };
                    if (user.Role != null)
                    {
                        userAuth.Roles.Add(user.Role.Name);
                        userAuth.Permissions.AddRange(from p in user.Role.Permissions select p.Name.ToString());
                    }
                }
            }

            return userAuth;
        }

        #endregion
    }
}