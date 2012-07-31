namespace Amss.Boilerplate.Api.Configuration
{
    using System;
    using System.Collections.Generic;

    using ServiceStack.ServiceInterface.Auth;

    internal class CustomUserAuthRepository : IUserAuthRepository
    {
        #region Public Methods and Operators

        public string CreateOrMergeAuthSession(IAuthSession authSession, IOAuthTokens tokens)
        {
            throw new NotSupportedException();
        }

        public UserAuth CreateUserAuth(UserAuth newUser, string password)
        {
            throw new NotSupportedException();
        }

        public UserAuth GetUserAuth(string userAuthId)
        {
            throw new NotSupportedException();
        }

        public UserAuth GetUserAuth(IAuthSession authSession, IOAuthTokens tokens)
        {
            throw new NotSupportedException();
        }

        public UserAuth GetUserAuthByUserName(string userNameOrEmail)
        {
            throw new NotSupportedException();
        }

        public List<UserOAuthProvider> GetUserOAuthProviders(string userAuthId)
        {
            return new List<UserOAuthProvider>();
        }

        public void LoadUserAuth(IAuthSession session, IOAuthTokens tokens)
        {
            throw new NotSupportedException();
        }

        public void SaveUserAuth(IAuthSession authSession)
        {
            throw new NotSupportedException();
        }

        public void SaveUserAuth(UserAuth userAuth)
        {
            throw new NotSupportedException();
        }

        public bool TryAuthenticate(string userName, string password, out UserAuth userAuth)
        {
            // todo: implement
            userAuth = new UserAuth
                {
                    Id = 1,
                    UserName = userName,
                    Email = "email",
                    PrimaryEmail = "email",
                    DisplayName = "full name",
                    PasswordHash = "adsd",
                    Salt = "salt",
                    Permissions = new List<string>(),
                    Roles = new List<string>(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

            return true;
        }

        public bool TryAuthenticate(
            Dictionary<string, string> digestHeaders, 
            string privateKey, 
            int nonceTimeOut, 
            string sequence, 
            out UserAuth userAuth)
        {
            throw new NotSupportedException();
        }

        public UserAuth UpdateUserAuth(UserAuth existingUser, UserAuth newUser, string password)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}