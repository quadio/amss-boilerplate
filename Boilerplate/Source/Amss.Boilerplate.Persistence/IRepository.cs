namespace Amss.Boilerplate.Persistence
{
    using System.Linq;

    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Data.Common;

    public interface IRepository
    {
        void Create<T>(T instance) where T : class, IEntity;
        
        T Get<T>(long id) where T : class, IEntity;

        T Load<T>(long id) where T : class, IEntity;

        void Update<T>(T instance) where T : class, IEntity;

        void Delete<T>(T instance) where T : class, IEntity;

        int Count<T>(IQueryData queryData);

        IQueryable<T> Query<T>(IQueryData queryData);

        void Flush(bool force = false);
    }
}