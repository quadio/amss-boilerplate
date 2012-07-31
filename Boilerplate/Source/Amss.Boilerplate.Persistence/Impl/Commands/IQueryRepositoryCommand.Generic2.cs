namespace Amss.Boilerplate.Persistence.Impl.Commands
{
    using System.Linq;

    using Amss.Boilerplate.Data.Common;

    internal interface IQueryRepositoryCommand<in TQueryData, out TResult> : IQueryRepositoryCommand<TResult>
        where TQueryData : IQueryData
    {
        IQueryable<TResult> Execute(TQueryData queryData);

        int RowCount(TQueryData queryData);
    }
}