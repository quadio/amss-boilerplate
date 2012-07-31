namespace Amss.Boilerplate.Persistence.Impl.Mappings
{
    using System.Collections.Generic;

    using Amss.Boilerplate.Data;

    using FluentNHibernate;
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;

    public class UserMapping : IAutoMappingOverride<UserEntity>
    {
        public void Override(AutoMapping<UserEntity> mapping)
        {
            mapping.Table("[User]");
            mapping
                .HasMany(Reveal.Member<UserEntity, IEnumerable<UserPasswordCredentialEntity>>("userPasswordCredentials"))
                .LazyLoad()
                .Cascade.AllDeleteOrphan()
                .Fetch.Join() // use this to avoid running separate query because we always need it
                .Inverse();
            mapping.HasManyToMany(Reveal.Member<UserEntity, IEnumerable<PartnerEntity>>("partners"))
                .LazyLoad().Table("PartnerUser");
            mapping.IgnoreProperty(i => i.Partner);
            mapping.IgnoreProperty(i => i.UserPasswordCredential);
        }
    }
}