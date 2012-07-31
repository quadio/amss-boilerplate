namespace Amss.Boilerplate.Business
{
    using Amss.Boilerplate.Data;

    public interface IPartnerManager : IManager<PartnerEntity>
    {
        void Disable(PartnerEntity partner);
        
        PartnerEntity AddUser(PartnerEntity partner, UserEntity user);
    }
}