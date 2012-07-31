namespace Amss.Boilerplate.Business.Impl
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    using Amss.Boilerplate.Business.Impl.Validation;
    using Amss.Boilerplate.Data;

    internal class PartnerManager : ManagerBase<PartnerEntity>, IPartnerManager
    {
        public override PartnerEntity Create(PartnerEntity partner)
        {
            Contract.Assert(partner != null);

            this.DemandValid<PartnerValidator>(partner);
            return base.Create(partner);
        }

        public override void Delete(PartnerEntity partner)
        {
            Contract.Assert(partner != null);
            this.Repository.Delete(partner);
        }

        public void Disable(PartnerEntity partner)
        {
            Contract.Assert(partner != null);
            Contract.Assert(!partner.Disabled);

            partner.Disabled = true;

            this.Repository.Update(partner);
        }

        public override void Update(PartnerEntity partner)
        {
            Contract.Assert(partner != null);
            this.DemandValid<PartnerValidator>(partner);
            base.Update(partner);
        }

        public PartnerEntity AddUser(PartnerEntity partner, UserEntity user)
        {
            Contract.Assert(partner != null);
            Contract.Assert(partner.Id > 0);
            Contract.Assert(user != null);
            Contract.Assert(user.Id > 0);

            if (partner.Users == null)
            {
                partner.Users = new Collection<UserEntity>
                    {
                        user
                    };
            }

            this.Repository.Update(partner);

            return partner;
        }
    }
}