namespace Amss.Boilerplate.Persistence.Impl.Mappings
{
    using Amss.Boilerplate.Data;

    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;

    public class RoleMapping : IAutoMappingOverride<RoleEntity>
    {
        public void Override(AutoMapping<RoleEntity> mapping)
        {
            mapping.HasMany(i => i.Permissions)
                .Inverse()
                .LazyLoad()
                .Cascade.All();
        }
    }
}