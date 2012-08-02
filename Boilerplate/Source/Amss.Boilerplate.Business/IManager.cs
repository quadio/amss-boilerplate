namespace Amss.Boilerplate.Business
{
    using System.Collections.Generic;
    using System.Linq;

    using Amss.Boilerplate.Data.Common;

    public interface IManager<T>
    {
        T Create(T instance);

        void Update(T instance);

        T Load(long id);

        IEnumerable<T> FindAll(IQueryData<T> queryData);

        IQueryable<T> Query(IQueryData<T> queryData);

        int Count(IQueryData<T> queryData);

        T FindOne(IQueryData<T> queryData);

        void Delete(T instance);
    }
}