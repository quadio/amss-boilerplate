namespace Amss.Boilerplate.Data
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class UserEntity : BaseEntity
    {
        private ICollection<UserPasswordCredentialEntity> userPasswordCredentials;

        private ICollection<PartnerEntity> partners;

        public virtual string Name { get; set; }

        public virtual string Email { get; set; }

        public virtual bool Deleted { get; set; }

        public virtual RoleEntity Role { get; set; }

        public virtual PartnerEntity Partner
        {
            get
            {
                return this.partners.SingleOrDefault();
            }

            set
            {
                if (this.partners == null)
                {
                    this.partners = new Collection<PartnerEntity>();
                }

                this.partners.Clear();
                this.partners.Add(value);
            }
        }

        public virtual UserPasswordCredentialEntity UserPasswordCredential
        {
            get
            {
                return this.userPasswordCredentials.SingleOrDefault();
            }

            set
            {
                if (this.userPasswordCredentials == null)
                {
                    this.userPasswordCredentials = new Collection<UserPasswordCredentialEntity>();
                }

                this.userPasswordCredentials.Clear();
                this.userPasswordCredentials.Add(value);
            }
        }
    }
}