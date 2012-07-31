namespace Amss.Boilerplate.Data
{
    using System.Collections.Generic;

    public class RoleEntity : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual ICollection<PermissionEntity> Permissions { get; set; } 
    }
}