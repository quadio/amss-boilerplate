namespace Amss.Boilerplate.Tests.Business
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;

    using Amss.Boilerplate.Business;
    using Amss.Boilerplate.Business.Exceptions;
    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Data.Specifications;

    using NUnit.Framework;

    [TestFixture]
    public class RoleManagerTests : BusinessIntegrationTestBase
    {
        #region Constants and Fields

        #endregion

        #region Public Methods and Operators

        [Test]
        public void Create()
        {
            var manager = this.Locator.GetInstance<IRoleManager>();

            var role = this.CreateTestRole();

            var instance = manager.Create(role);

            Assert.That(instance.Id, Is.GreaterThan(0));
            Assert.That(instance.Permissions, Is.Not.Null);
            Assert.That(instance.Permissions.Count, Is.GreaterThan(0));

            foreach (var permission in instance.Permissions)
            {
                Trace.WriteLine("Permission: " + permission.Name);
            }
        }

        [Test]
        public void Delete()
        {
            var manager = this.Locator.GetInstance<IRoleManager>();
            var instance = manager.Create(this.CreateTestRole());

            this.ClearSession(instance);

            manager.Delete(instance);
        }

        [Test]
        public void FindAll()
        {
            var manager = this.Locator.GetInstance<IRoleManager>();

            var instance = manager.Create(this.CreateTestRole());

            this.ClearSession(instance);

            var roles = manager.FindAll(new RoleAll());
            Assert.That(roles, Is.Not.Null);
            Assert.That(roles.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void Load()
        {
            var manager = this.Locator.GetInstance<IRoleManager>();

            var instance = manager.Create(this.CreateTestRole());
            Assert.That(instance, Is.Not.Null);

            this.ClearSession(instance);

            var role = manager.Load(instance.Id);
            Assert.That(role, Is.Not.Null);

            Assert.That(role.Name, Is.EquivalentTo(instance.Name));
        }

        [Test]
        public void LoadByName()
        {
            var manager = this.Locator.GetInstance<IRoleManager>();

            var instance = manager.Create(this.CreateTestRole());
            Assert.That(instance, Is.Not.Null);

            this.ClearSession(instance);

            var role = manager.Load(instance.Name);
            Assert.That(role, Is.Not.Null);
            Assert.That(role.Name, Is.EquivalentTo(instance.Name));
            Assert.That(role.Id, Is.EqualTo(instance.Id));

            Assert.Throws<ObjectNotFoundException>(() => manager.Load(this.ShortStringGenerator.GetRandomValue()));
        }

        [Test]
        public void NotUniqueRole()
        {
            var manager = this.Locator.GetInstance<IRoleManager>();
            var instance = manager.Create(this.CreateTestRole());

            var ex =
                Assert.Throws<BusinessValidationException>(() => manager.Create(new RoleEntity { Name = instance.Name }));
            Trace.WriteLine(ex.Message);
        }

        [Test]
        public void Update()
        {
            var manager = this.Locator.GetInstance<IRoleManager>();
            var role = this.CreateTestRole();
            var instance = manager.Create(role);

            this.ClearSession(instance);

            instance.Name = this.ShortStringGenerator.GetRandomValue();

            manager.Update(instance);
        }

        #endregion
    }
}