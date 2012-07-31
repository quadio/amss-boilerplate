namespace Amss.Boilerplate.Persistence.Impl.Mappings
{
    using Amss.Boilerplate.Data;

    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;

    public class PartnerMapping : IAutoMappingOverride<PartnerEntity>
    {
        public void Override(AutoMapping<PartnerEntity> mapping)
        {
            mapping.HasManyToMany(i => i.Users)
                .Table("PartnerUser")
                .ChildKeyColumn("UserId")
                .ParentKeyColumn("PartnerId")
                .LazyLoad();
        }
    }
}