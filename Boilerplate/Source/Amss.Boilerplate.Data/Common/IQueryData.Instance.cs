namespace Amss.Boilerplate.Data.Common
{
    public interface IInstanceQueryData<T> : IQueryData<T>
    {
        T Instance { get; set; }
    }
}