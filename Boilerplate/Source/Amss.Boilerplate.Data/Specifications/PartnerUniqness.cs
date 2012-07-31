namespace Amss.Boilerplate.Data.Specifications
{
    using System;
    using System.Linq.Expressions;

    using Amss.Boilerplate.Data.Common;

    public class PartnerUniqness : SpecificationInstanceBase<PartnerEntity>
    {
        public override Expression<Func<PartnerEntity, bool>> IsSatisfiedBy()
        {
            return m => m.Id != this.Instance.Id && m.Name == this.Instance.Name;
        }
    }
}