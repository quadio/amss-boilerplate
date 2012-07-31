namespace Amss.Boilerplate.Web.Common.Data
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Amss.Boilerplate.Common.Security;
    using Amss.Boilerplate.Data;

    internal static class PrincipalSessionExtender
    {
        public static PrincipalSession Convert(this UserEntity user)
        {
            Contract.Assert(user != null);
            Contract.Assert(user.UserPasswordCredential != null);

            var session = new PrincipalSession
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Login = user.UserPasswordCredential.Login,
                    Role = user.Role != null ? new[] { user.Role.Name } : new string[0],
                    Permissions = user.Role != null ? user.Role.Permissions.Select(p => p.Name.ToString("G")).ToArray() : new string[0]
                };

            return session;
        }

        public static IApplicationPrincipal Convert(this PrincipalSession session)
        {
            Contract.Assert(session != null);

            var identity = new ApplicationIdentity(session.UserId, session.Login, session.Name, session.Email);
            var principal = new ApplicationPrincipal(identity, session.Role, session.Permissions);

            return principal;
        }
    }
}