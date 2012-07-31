namespace Amss.Boilerplate.Business.Impl.Validation
{
    using System.Linq;

    using Amss.Boilerplate.Business.Exceptions;
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Data.Common;
    using Amss.Boilerplate.Persistence;

    using FluentValidation;

    internal static class ValidationExtension
    {
        public static void DemandValid<T>(this IValidator validator, T instance)
        {
            var result = validator.Validate(instance);
            if (!result.IsValid)
            {
                var failures = from error in result.Errors
                               where error != null
                               select
                                   new ValidationFailureInfo(
                                       error.PropertyName,
                                       error.ErrorMessage,
                                       error.CustomState as string,
                                       error.AttemptedValue,
                                       error.CustomState);
                throw new BusinessValidationException(failures.ToList());
            }
        }

        public static IRuleBuilderOptions<T, TProperty> Uniqness<T, TProperty, TBase>(
                this IRuleBuilderOptions<T, TProperty> ruleBuilder,
                IRepository repository,
                IInstanceQueryData<TBase> queryData)
            where T : class, IEntity
            where TBase : class, IEntity
        {
            var validator = new UniquePropertyValidator<TBase>(repository, queryData);
            ruleBuilder.SetValidator(validator);

            // TODO: use state
            /*validator.CustomStateProvider = t => new ValidationFailureState(ViolationCodeNames.NonUniqueState);*/
            return ruleBuilder;
        }
    }
}