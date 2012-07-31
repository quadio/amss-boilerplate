namespace Amss.Boilerplate.Common.Security
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Principal;

    public class ApplicationPrincipal : IApplicationPrincipal
    {
        #region Constants and Fields

        public static readonly IApplicationPrincipal Anonymous = new ApplicationPrincipal(new ApplicationIdentity(), new string[0], new string[0]);

        private readonly string[] permissions;

        private readonly string[] roles;

        #endregion

        #region Constructors and Destructors

        public ApplicationPrincipal(IApplicationIdentity identity, string[] roles, string[] permissions)
        {
            Contract.Assert(identity != null);
            Contract.Assert(roles != null);
            Contract.Assert(permissions != null);

            this.Identity = identity;
            this.roles = roles;
            this.permissions = permissions;
        }

        #endregion

        #region Public Properties

        public IApplicationIdentity Identity { get; private set; }

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

        #region Public Methods and Operators

        public bool HasPermission(string permission)
        {
            var hasPermission =
                this.permissions.Any(p => p.Equals(permission, StringComparison.InvariantCultureIgnoreCase));
            return hasPermission;
        }

        public bool IsInRole(string role)
        {
            Contract.Assert(role != null);
            var isInRole = this.roles.Any(r => r.Equals(role, StringComparison.InvariantCultureIgnoreCase));
            return isInRole;
        }

        #endregion
    }
}