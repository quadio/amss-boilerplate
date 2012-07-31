namespace Amss.Boilerplate.Data.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Amss.Boilerplate.Data.Common;

    public class PartnerAll : SpecificationBase<PartnerEntity>
    {
        private readonly bool withDisabled;

        public PartnerAll(bool withDisabled = false)
        {
            this.withDisabled = withDisabled;
        }

        public override Expression<Func<PartnerEntity, bool>> IsSatisfiedBy()
        {
            return m => this.withDisabled || !m.Disabled;
        }

        public override IQueryable<PartnerEntity> Order(IQueryable<PartnerEntity> query)
        {
            return query.OrderBy(m => m.Created);
        }
    }
}