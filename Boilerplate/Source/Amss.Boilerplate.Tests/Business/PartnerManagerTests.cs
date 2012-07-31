namespace Amss.Boilerplate.Tests.Business
{
    using System.Diagnostics;
    using System.Linq;

    using Amss.Boilerplate.Business;
    using Amss.Boilerplate.Business.Exceptions;
    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Data.Specifications;

    using NUnit.Framework;

    [TestFixture]
    public class PartnerManagerTests : BusinessIntegrationTestBase
    {
        #region Constants and Fields

        #endregion

        #region Public Methods and Operators

        [Test]
        public void Create()
        {
            var manager = this.Locator.GetInstance<IPartnerManager>();

            var partner = this.CreateTestPartner();

            var instance = manager.Create(partner);

            Assert.That(instance.Id, Is.GreaterThan(0));
        }

        [Test]
        public void Delete()
        {
            var manager = this.Locator.GetInstance<IPartnerManager>();
            var instance = manager.Create(this.CreateTestPartner());

            manager.Delete(instance);

            Assert.Throws<ObjectNotFoundException>(() => manager.Load(instance.Id));
        }

        [Test]
        public void Disable()
        {
            var manager = this.Locator.GetInstance<IPartnerManager>();
            var instance = manager.Create(this.CreateTestPartner());

            this.ClearSession(instance);

            manager.Disable(instance);

            this.ClearSession(instance);

            var i = manager.Load(instance.Id);

            Assert.That(i.Disabled, Is.True);
        }

        [Test]
        public void NotUniquePartner()
        {
            var manager = this.Locator.GetInstance<IPartnerManager>();
            var instance = manager.Create(this.CreateTestPartner());

            var ex =
                Assert.Throws<BusinessValidationException>(
                    () => manager.Create(new PartnerEntity { Name = instance.Name }));
            Trace.WriteLine(ex.Message);
        }

        [Test]
        public void FindAll()
        {
            var manager = this.Locator.GetInstance<IPartnerManager>();

            var instance = manager.Create(this.CreateTestPartner());

            this.ClearSession(instance);

            var partners = manager.FindAll(new PartnerAll());
            Assert.That(partners, Is.Not.Null);
            Assert.That(partners.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void Load()
        {
            var manager = this.Locator.GetInstance<IPartnerManager>();

            var instance = manager.Create(this.CreateTestPartner());
            Assert.That(instance, Is.Not.Null);

            this.ClearSession(instance);

            var partner = manager.Load(instance.Id);
            Assert.That(partner, Is.Not.Null);

            Assert.That(partner.Name, Is.EquivalentTo(instance.Name));
        }

        [Test]
        public void Update()
        {
            var manager = this.Locator.GetInstance<IPartnerManager>();
            var partner = this.CreateTestPartner();
            var instance = manager.Create(partner);

            Assert.That(instance, Is.Not.Null);
            this.ClearSession(instance);

            instance.Name = this.ShortStringGenerator.GetRandomValue();

            manager.Update(instance);

            this.ClearSession(instance);

            var ni = manager.Load(instance.Id);
            Assert.That(instance.Name, Is.EqualTo(ni.Name));
        }

        [Test]
        public void AddUser()
        {
            var manager = this.Locator.GetInstance<IPartnerManager>();
            var userManager = this.Locator.GetInstance<IUserManager>();
            var roleManager = this.Locator.GetInstance<IRoleManager>();
            var partner = this.CreateTestPartner();
            var user = this.CreateTestUser();

            roleManager.Create(user.Role);
            var partnerInstance = manager.Create(partner);
            var userInstance = userManager.Create(user, this.ShortStringGenerator.GetRandomValue(), this.ShortStringGenerator.GetRandomValue());

            Assert.That(partnerInstance, Is.Not.Null);
            Assert.That(userInstance, Is.Not.Null);
            
            manager.AddUser(partnerInstance, userInstance);

            this.ClearSession(partnerInstance, userInstance);

            var p = manager.Load(partner.Id);

            Assert.That(p, Is.Not.Null);
            Assert.That(p.Users, Is.Not.Null);
            Assert.That(p.Users.Count, Is.GreaterThan(0));
        }

        #endregion
    }
}