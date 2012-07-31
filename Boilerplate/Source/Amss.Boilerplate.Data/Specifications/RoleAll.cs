namespace Amss.Boilerplate.Data.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Amss.Boilerplate.Data.Common;

    public class RoleAll : SpecificationBase<RoleEntity>
    {
        public override Expression<Func<RoleEntity, bool>> IsSatisfiedBy()
        {
            return _ => true;
        }

        public override IQueryable<RoleEntity> Order(IQueryable<RoleEntity> query)
        {
            return query.OrderBy(m => m.Created);
        }
    }
}