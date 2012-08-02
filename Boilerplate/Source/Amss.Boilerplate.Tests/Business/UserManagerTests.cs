namespace Amss.Boilerplate.Tests.Business
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    using Amss.Boilerplate.Business;
    using Amss.Boilerplate.Business.Exceptions;
    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Data.Specifications;

    using FluentNHibernate.Testing;

    using NUnit.Framework;

    [TestFixture]
    public class UserManagerTests : BusinessIntegrationTestBase
    {
        [Test]
        public void CreateValidUser()
        {
            var user = this.CreateUser();
            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void CreateUserUsingInvalidData()
        {
            Trace.WriteLine("user test");
            var ex = Assert.Throws<BusinessValidationException>(() => this.CreateUser(
                noDefaultRole: true,
                name: new string('a', MetadataInfo.StringNormal + 1), 
                email: string.Empty));
            Trace.WriteLine(ex.Message);
            Assert.That(ex.Errors.Count(), Is.EqualTo(3));

            Trace.WriteLine("password test");
            ex = Assert.Throws<BusinessValidationException>(() => this.CreateUser(
                password: new string('a', MetadataInfo.StringNormal + 1),
                role: new RoleEntity()));
            Trace.WriteLine(ex.Message);
            Assert.That(ex.Errors.Count(), Is.EqualTo(1));

            Trace.WriteLine("login test");
            ex = Assert.Throws<BusinessValidationException>(() => this.CreateUser(
                login: new string('a', MetadataInfo.StringNormal + 1),
                role: new RoleEntity()));
            Trace.WriteLine(ex.Message);
            Assert.That(ex.Errors.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FindUserByPasswordCredential()
        {
            var login = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            const string Password = "password";
            this.CreateUser(login: login, password: Password);
            var manager = this.Locator.GetInstance<IUserManager>();

            var user = manager.FindByPasswordCredential(login, Password);
            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void Load()
        {
            var user = this.CreateUser();
            this.Session.Evict(user);
            var manager = this.Locator.GetInstance<IUserManager>();
            manager.Load(user.Id);
        }

        [Test]
        public void LoadNonExisting()
        {
            var manager = this.Locator.GetInstance<IUserManager>();
            Assert.Throws<ObjectNotFoundException>(() => manager.Load(int.MaxValue));
        }

        [Test]
        public void Delete()
        {
            var manager = this.Locator.GetInstance<IUserManager>();
            var user = this.CreateUser();
            Assert.That(user.Deleted, Is.False);
            Assert.That(user.UserPasswordCredential, Is.Not.Null);
            this.Session.Flush();

            manager.Delete(user);
            this.Session.Evict(user);
            user = manager.Load(user.Id);
            this.Session.Flush();
            Assert.That(user.Deleted, Is.True);
            Assert.That(user.UserPasswordCredential, Is.Null);
        }

        [Test]
        public void ChangePassword()
        {
            var manager = this.Locator.GetInstance<IUserManager>();
            var user = this.CreateUser();
            // ReSharper disable ConvertToConstant.Local
            var newPassword = "newPassword";
            // ReSharper restore ConvertToConstant.Local
            var login = user.UserPasswordCredential.Login;
            manager.ChangePassword(user, newPassword);
            this.Session.Flush();
            this.Session.Evict(user);

            user = manager.FindByPasswordCredential(login, newPassword);
            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void ChangeDeletedPassword()
        {
            var manager = this.Locator.GetInstance<IUserManager>();
            var user = this.CreateUser();
            this.Session.Flush();
            manager.Delete(user);

            user = manager.Load(user.Id);
            var ex = Assert.Throws<BusinessValidationException>(() => manager.ChangePassword(user, "password"));
            Trace.WriteLine(ex.Message);
        }

        [Test(Description = "If manager returns IQueryable UI may perform request which our data layer can not proceed. If FindAll returns IQueryable - test failed")]
        public void QueryableUiTest()
        {
            var manager = this.Locator.GetInstance<IUserManager>();
            var query = manager.FindAll(new UserAll());

            var queryUi = from u in query
                           select new 
                                   {
                                       u.Name,
                                       Login = u.UserPasswordCredential != null ? u.UserPasswordCredential.Login : string.Empty
                                   };

            foreach (var user in queryUi.Where(u => u.Login.StartsWith("w")))
            {
                Trace.WriteLine("User: " + user.Name + ". Login: " + user.Login);
            }
        }

        [Test]
        public void PagerTest()
        {
            var manager = this.Locator.GetInstance<IUserManager>();
            var query = manager.FindAll(new UserAll { PageIndex = 1, PageSize = 10 });

            var queryUi = from u in query
                           select new
                           {
                               u.Name,
                               Login = u.UserPasswordCredential != null ? u.UserPasswordCredential.Login : string.Empty
                           };

            Assert.That(query, Is.Not.Null);
            Assert.That(query.Any());

            foreach (var user in queryUi)
            {
                Trace.WriteLine("User: " + user.Name + ". Login: " + user.Login);
            }
        }

        [Test]
        public void WhereTest()
        {
            var manager = this.Locator.GetInstance<IUserManager>();
            var query = manager.Query(new UserAll ());

            var mock = query.Where(u => u.Email != null ? u.Email.StartsWith("a") : u.Email.StartsWith("b"));

            Assert.That(mock, Is.Not.Null);
        }

        private UserEntity CreateUser(
            string name = null,
            string email = null,
            RoleEntity role = null,
            string login = null, 
            string password = null,
            bool noDefaultRole = false)
        {
            if (!noDefaultRole)
            {
                role = role ??
                    new PersistenceSpecification<RoleEntity>(this.Session).CheckProperty(
                        c => c.Name, this.ShortStringGenerator.GetRandomValue()).VerifyTheMappings();
            }

            var user = new UserEntity
            {
                Name = name ?? this.NormalStringGenerator.GetRandomValue(),
                Email = email ?? this.NormalStringGenerator.GetRandomValue(),
                Role = role
            };

            login = login ?? this.NormalStringGenerator.GetRandomValue();
            password = password ?? this.NormalStringGenerator.GetRandomValue();
            var manager = this.Locator.GetInstance<IUserManager>();
            manager.Create(user, login, password);
            return user;
        }
    }
}