namespace Amss.Boilerplate.Persistence.Impl.Configuration.Conventions
{
    using FluentNHibernate.Conventions;

    internal static class ConventionExtender
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "convention", Justification = "As designed")]
        public static string CleanTableName(this IConvention convention, string tableName)
        {
            return tableName.Replace("Entity", string.Empty);
        }
    }
}