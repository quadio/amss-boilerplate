namespace Amss.Boilerplate.Tests.Persistence
{
    using System;
    using System.Collections;
    using System.Linq.Expressions;

    using Amss.Boilerplate.Data;

    using FluentNHibernate.Testing;

    internal static class PersistenceSpecificationExtensions
    {
        public static PersistenceSpecification<T> CheckProperty<T>(
            this PersistenceSpecification<T> spec, Expression<Func<T, object>> expression, DateTime propertyValue)
        {
            var ticks = propertyValue.Ticks - (propertyValue.Ticks % TimeSpan.TicksPerSecond);
            var newValue = new DateTime(ticks, propertyValue.Kind);
            return spec.CheckProperty(expression, (object)newValue);
        }

        public static PersistenceSpecification<T> CheckEntity<T>(
            this PersistenceSpecification<T> spec, Expression<Func<T, object>> expression, IEntity propertyValue)
        {
            return spec.CheckProperty(expression, propertyValue, new EntityComparer());
        }

        internal class EntityComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                var e1 = x as IEntity;
                var e2 = y as IEntity;
                if (e1 == null || e2 == null)
                {
                    return false;
                }

                if (
                    e1.GetType() == e2.GetType() 
                    || e1.GetType().IsInstanceOfType(e2)
                    || e2.GetType().IsInstanceOfType(e1))
                {
                    return e1.Id == e2.Id;
                }

                return false;
            }

            public int GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}