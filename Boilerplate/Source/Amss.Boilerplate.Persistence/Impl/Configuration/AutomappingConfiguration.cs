namespace Amss.Boilerplate.Persistence.Impl.Configuration
{
    using System;

    using Amss.Boilerplate.Data;

    using FluentNHibernate.Automapping;

    internal class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return typeof(IEntity).IsAssignableFrom(type);
        }

        public override bool AbstractClassIsLayerSupertype(Type type)
        {
            return type == typeof(IEntity) || type == typeof(BaseEntity);
        }
    }
}