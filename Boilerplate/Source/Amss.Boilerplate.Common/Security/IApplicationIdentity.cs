namespace Amss.Boilerplate.Common.Security
{
    using System.Security.Principal;

    public interface IApplicationIdentity : IIdentity
    {
        long UserId { get; }

        string Email { get; }

        string DisplayName { get; }
    }
}