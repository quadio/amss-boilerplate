namespace Amss.Boilerplate.Business.Impl.Validation
{
    using Amss.Boilerplate.Data;

    using FluentValidation;

    internal class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            this.RuleFor(i => i)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(1, MetadataInfo.StringNormal)
                .WithName("Password");
        }
    }
}