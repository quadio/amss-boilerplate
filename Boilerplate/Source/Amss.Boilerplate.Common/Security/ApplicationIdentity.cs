namespace Amss.Boilerplate.Common.Security
{
    using System.Diagnostics.Contracts;

    public class ApplicationIdentity : IApplicationIdentity
    {
        #region Constructors and Destructors

        public ApplicationIdentity(string type = "application")
        {
            this.AuthenticationType = type;
        }

        public ApplicationIdentity(long userId, string name, string displayName, string email)
            : this(userId, name, displayName, email, "application")
        {
        }

        public ApplicationIdentity(long userId, string name, string displayName, string email, string type)
            : this(type)
        {
            Contract.Assert(userId > 0);
            Contract.Assert(name != null);
            Contract.Assert(displayName != null);
            Contract.Assert(email != null);

            this.UserId = userId;
            this.Name = name;
            this.DisplayName = displayName;
            this.Email = email;
            this.IsAuthenticated = true;
        }

        #endregion

        #region Public Properties

        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public long UserId { get; private set; }

        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public string Email { get; private set; }

        #endregion
    }
}
