namespace Amss.Boilerplate.Persistence.Impl.Configuration.Conventions
{
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Persistence.Impl.Utilities.Text;

    using FluentNHibernate.Conventions;
    using FluentNHibernate.Conventions.AcceptanceCriteria;
    using FluentNHibernate.Conventions.Inspections;
    using FluentNHibernate.Conventions.Instances;

    internal class TableNameConvention : IClassConvention, IClassConventionAcceptance, IJoinedSubclassConvention, IJoinedSubclassConventionAcceptance
    {
        public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
        {
            criteria.Expect(x => typeof(IEntity).IsAssignableFrom(x.EntityType));
        }

        public void Accept(IAcceptanceCriteria<IJoinedSubclassInspector> criteria)
        {
            criteria.Expect(x => typeof(IEntity).IsAssignableFrom(x.EntityType));
        }

        public void Apply(IClassInstance instance)
        {
            instance.Table(Singularizer.Singularize(this.CleanTableName(instance.EntityType.Name)));
        }

        public void Apply(IJoinedSubclassInstance instance)
        {
            instance.Table(Singularizer.Singularize(this.CleanTableName(instance.EntityType.Name)));
        }
    }
}