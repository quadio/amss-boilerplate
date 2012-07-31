namespace Amss.Boilerplate.Common.Security
{
    using System.Diagnostics.Contracts;
    using System.Security.Principal;
    using System.Threading;

    public class ThreadApplicationPrincipal : IApplicationPrincipal
    {
        #region Public Properties

        public IApplicationIdentity Identity
        {
            get
            {
                return ParentPrincipal.Identity;
            }
        }

        #endregion

        #region Explicit Interface Properties

        IIdentity IPrincipal.Identity
        {
            get
            {
                return this.Identity;
            }
        }

        #endregion

        #region Properties

        private static IApplicationPrincipal ParentPrincipal
        {
            get
            {
                var identity = Thread.CurrentPrincipal as IApplicationPrincipal;
                Contract.Assert(identity != null);
                return identity;
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool HasPermission(string permission)
        {
            return ParentPrincipal.HasPermission(permission);
        }

        public bool IsInRole(string role)
        {
            return ParentPrincipal.IsInRole(role);
        }

        #endregion
    }
}