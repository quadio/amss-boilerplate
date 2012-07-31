namespace Amss.Boilerplate.Common.Security
{
    using System.Security.Principal;

    public interface IApplicationPrincipal : IPrincipal
    {
        new IApplicationIdentity Identity { get; }

        bool HasPermission(string permission);
    }
}