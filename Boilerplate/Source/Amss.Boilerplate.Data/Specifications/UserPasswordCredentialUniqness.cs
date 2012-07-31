namespace Amss.Boilerplate.Data.Specifications
{
    using System;
    using System.Linq.Expressions;

    using Amss.Boilerplate.Data.Common;

    public class UserPasswordCredentialUniqness : SpecificationInstanceBase<UserPasswordCredentialEntity>
    {
        public override Expression<Func<UserPasswordCredentialEntity, bool>> IsSatisfiedBy()
        {
            return m => m.Id != this.Instance.Id && m.Login == this.Instance.Login;
        }
    }
}