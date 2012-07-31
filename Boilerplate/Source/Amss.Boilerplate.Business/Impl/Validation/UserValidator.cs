namespace Amss.Boilerplate.Business.Impl.Validation
{
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Persistence;

    using FluentValidation;

    internal class UserValidator : AbstractValidator<UserEntity>
    {
        public UserValidator(IRepository repository)
        {
            this.RuleFor(i => i.Name).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(1, MetadataInfo.StringNormal);
            this.RuleFor(i => i.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(1, MetadataInfo.StringNormal)
                .SetValidator(new FluentValidation.Validators.EmailValidator());
            this.RuleFor(i => i.Role).NotNull();
            this.RuleFor(i => i.UserPasswordCredential)
                .NotNull()
                .SetValidator(new UserPasswordCredentialValidator(repository));
        }        
    }
}