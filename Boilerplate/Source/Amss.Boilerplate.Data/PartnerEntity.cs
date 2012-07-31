namespace Amss.Boilerplate.Data
{
    using System.Collections.Generic;

    public class PartnerEntity : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual bool Disabled { get; set; }

        public virtual ICollection<UserEntity> Users { get; set; } 
    }
}