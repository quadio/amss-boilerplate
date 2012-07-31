namespace Amss.Boilerplate.Data.Common
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface ISpecification<T> : IQueryData<T>
    {
        #region Public Properties

        int PageIndex { get; }

        int PageSize { get; }

        #endregion

        #region Public Methods and Operators

        Expression<Func<T, bool>> IsSatisfiedBy();

        IQueryable<T> Order(IQueryable<T> query);

        #endregion
    }
}