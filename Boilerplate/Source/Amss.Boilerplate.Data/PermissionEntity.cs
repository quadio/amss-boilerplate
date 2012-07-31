namespace Amss.Boilerplate.Data
{
    public class PermissionEntity : BaseEntity
    {
        public virtual AccessRight Name { get; set; }

        public virtual RoleEntity Role { get; set; }
    }
}