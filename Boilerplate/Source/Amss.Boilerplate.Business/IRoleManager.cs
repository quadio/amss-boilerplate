namespace Amss.Boilerplate.Business
{
    using Amss.Boilerplate.Data;

    public interface IRoleManager : IManager<RoleEntity>
    {
        RoleEntity Load(string name);
    }
}