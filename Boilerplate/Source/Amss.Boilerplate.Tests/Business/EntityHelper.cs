namespace Amss.Boilerplate.Tests.Business
{
    using System.Collections.ObjectModel;

    using Amss.Boilerplate.Data;

    using QuickGenerate.Primitives;

    internal static class EntityHelper
    {
        private static readonly StringGenerator ShortStringGenerator = new StringGenerator(5, 20);

        public static RoleEntity CreateTestRole(this BusinessIntegrationTestBase testBase)
        {
            var role = new RoleEntity { Name = ShortStringGenerator.GetRandomValue(), };
            role.Permissions = new Collection<PermissionEntity>
                {
                    new PermissionEntity
                        {
                            Name = AccessRight.Admin,
                            Role = role
                        }
                };

            return role;
        }

        public static PartnerEntity CreateTestPartner(this BusinessIntegrationTestBase testBase)
        {
            var p = new PartnerEntity { Name = ShortStringGenerator.GetRandomValue() };

            return p;
        }

        public static UserEntity CreateTestUser(this BusinessIntegrationTestBase testBase)
        {
            var u = new UserEntity
                {
                    Name = ShortStringGenerator.GetRandomValue(),
                    Email = ShortStringGenerator.GetRandomValue(),
                };

            var r = CreateTestRole(testBase);
            u.Role = r;

            return u;
        }
    }
}