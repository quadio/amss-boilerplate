namespace Amss.Boilerplate.Persistence.Impl.Mappings
{
    using Amss.Boilerplate.Data;

    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;

    public class PermissionMapping : IAutoMappingOverride<PermissionEntity>
    {
        public void Override(AutoMapping<PermissionEntity> mapping)
        {
            mapping.References(x => x.Role, "RoleId")
                .Fetch.Join();
        }
    }
}