namespace Amss.Boilerplate.Data.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Amss.Boilerplate.Data.Common;

    public class UserAll : SpecificationBase<UserEntity>
    {
        private readonly bool withDeleted;

        public UserAll(bool withDeleted = false)
        {
            this.withDeleted = withDeleted;
        }

        public override Expression<Func<UserEntity, bool>> IsSatisfiedBy()
        {
            return m => this.withDeleted || !m.Deleted;
        }

        public override IQueryable<UserEntity> Order(IQueryable<UserEntity> query)
        {
            return query.OrderBy(m => m.Created);
        }
    }
}