namespace Amss.Boilerplate.Data.Common
{
    public abstract class SpecificationInstanceBase<T> : SpecificationBase<T>, IInstanceQueryData<T>
    {
        public T Instance { get; set; }
    }
}