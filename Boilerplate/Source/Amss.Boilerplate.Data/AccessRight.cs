namespace Amss.Boilerplate.Data
{
    public enum AccessRight
    {
        Admin,
        SuperAdmin,
    }

    public static class AccessRightRegistry
    {
        public const string Admin = "Admin";

        public const string SuperAdmin = "SuperAdmin";
    }
}