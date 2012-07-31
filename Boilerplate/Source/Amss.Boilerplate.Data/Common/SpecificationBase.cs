namespace Amss.Boilerplate.Data.Common
{
    using System.Linq;

    using LinqSpecs;

    public abstract class SpecificationBase<T> : Specification<T>, ISpecification<T>
    {
        #region Public Properties

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual IQueryable<T> Order(IQueryable<T> query)
        {
            return query;
        }

        #endregion
    }
}