namespace Amss.Boilerplate.Persistence.Impl.Commands
{
    using System.Linq;

    using Amss.Boilerplate.Data.Common;

    internal interface IQueryRepositoryCommand<out TResult>
    {
        IQueryable<TResult> Execute(IQueryData queryData);

        int RowCount(IQueryData queryData);
    }
}