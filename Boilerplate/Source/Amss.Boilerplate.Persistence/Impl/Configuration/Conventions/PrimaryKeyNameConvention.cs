namespace Amss.Boilerplate.Persistence.Impl.Configuration.Conventions
{
    using Amss.Boilerplate.Persistence.Impl.Utilities.Text;

    using FluentNHibernate.Conventions;
    using FluentNHibernate.Conventions.Instances;

    /// <summary>
    /// PrimaryKeyNameConvention - says that name of every column representing primary key should consist of entity name and “Id” suffix.
    /// </summary>
    internal class PrimaryKeyNameConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            var tableName = this.CleanTableName(instance.EntityType.Name);
            var column = Singularizer.Singularize(tableName) + "Id";
            instance.Column(column);
        }
    }
}