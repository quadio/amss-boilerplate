// ReSharper disable CheckNamespace
namespace Amss.Boilerplate.Data
// ReSharper restore CheckNamespace
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;

    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Data.Common;
    using Amss.Boilerplate.Persistence;

    public static class RepositoryExtension
    {
        #region Methods

        public static int Count<T>(this IRepository repository, IQueryData<T> queryData)
        {
            Contract.Assert(repository != null);
            return repository.Count<T>(queryData);
        }

        public static IEnumerable<T> FindAll<T>(this IRepository repository, IQueryData<T> queryData)
        {
            Contract.Assert(repository != null);
            return repository.FindAll<T>((IQueryData)queryData);
        }

        public static IEnumerable<T> FindAll<T>(this IRepository repository, IQueryData queryData)
        {
            Contract.Assert(queryData != null);
            var query = repository.Query<T>(queryData);
            var result = query.ToList();
            return result;
        }

        public static T LoadOne<T>(this IRepository repository, IQueryData<T> queryData)
            where T : class
        {
            Contract.Assert(repository != null);
            Contract.Assert(queryData != null);
            var instance = repository.FindOne(queryData);
            if (instance == null)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "Cannot load [{0}] using criteria [{1}].",
                    typeof(T),
                    queryData.GetType());
                throw new ObjectNotFoundException(message);
            }

            return instance;
        }

        public static T FindOne<T>(this IRepository repository, IQueryData<T> queryData)
        {
            Contract.Assert(repository != null);
            return repository.FindOne<T>((IQueryData)queryData);
        }

        public static T FindOne<T>(this IRepository repository, IQueryData queryData)
        {
            Contract.Assert(queryData != null);
            var query = repository.Query<T>(queryData);
            var result = query.FirstOrDefault();
            return result;
        }

        public static IQueryable<T> Query<T>(this IRepository repository, IQueryData<T> queryData)
        {
            Contract.Assert(repository != null);
            return repository.Query<T>(queryData);
        }

        #endregion
    }
}