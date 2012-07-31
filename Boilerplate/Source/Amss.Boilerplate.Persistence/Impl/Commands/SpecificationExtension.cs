namespace Amss.Boilerplate.Persistence.Impl.Commands
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Amss.Boilerplate.Data.Common;

    using NHibernate;
    using NHibernate.Linq;

    internal static class SpecificationExtension
    {
        public static IQueryable<T> Page<T>(this ISession session, ISpecification<T> queryData)
        {
            Contract.Assert(session != null);
            var query = session
                .Query<T>()
                .Page(queryData);
            return query;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> query, ISpecification<T> queryData)
        {
            Contract.Assert(query != null);
            Contract.Assert(queryData != null);
            var result = query;
            if (queryData.PageSize > 0)
            {
                result = query
                    .Skip(queryData.PageIndex * queryData.PageSize)
                    .Take(queryData.PageSize);
            }

            return result;
        }

        public static IQueryable<T> Order<T>(this IQueryable<T> query, ISpecification<T> queryData)
        {
            Contract.Assert(query != null);
            var result = queryData.Order(query);
            return result;
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> query, ISpecification<T> queryData)
        {
            Contract.Assert(query != null);
            Contract.Assert(queryData != null);
            var result = query.Where(queryData.IsSatisfiedBy());
            return result;
        }

        public static int Count<T>(this ISession session, ISpecification<T> queryData)
        {
            Contract.Assert(session != null);
            var result = session
                .Query<T>()
                .Count(queryData);
            return result;
        }

        public static int Count<T>(this IQueryable<T> query, ISpecification<T> queryData)
        {
            Contract.Assert(query != null);
            Contract.Assert(queryData != null);
            var result = query.Count(queryData.IsSatisfiedBy());
            return result;
        }
    }
}