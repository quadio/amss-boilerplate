namespace Amss.Boilerplate.Business.Impl.Validation
{
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Data.Specifications;
    using Amss.Boilerplate.Persistence;

    using FluentValidation;

    internal class UserPasswordCredentialValidator : AbstractValidator<UserPasswordCredentialEntity>
    {
        public UserPasswordCredentialValidator(IRepository repository)
        {
            this.RuleFor(i => i.Login).NotEmpty().Length(1, MetadataInfo.StringNormal).Uniqness(repository, new UserPasswordCredentialUniqness());
            this.RuleFor(i => i.PasswordHash).NotEmpty().Length(1, 100);
            this.RuleFor(i => i.PasswordSalt).NotEmpty().Length(1, 100);
            this.RuleFor(i => i.User).NotNull();
        }
    }
}