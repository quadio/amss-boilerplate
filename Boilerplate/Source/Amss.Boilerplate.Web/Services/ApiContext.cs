namespace Amss.Boilerplate.Web.Services
{
    using System;
    using System.Data.Services;
    using System.Linq;
    using System.Linq.Expressions;

    using Amss.Boilerplate.Business;
    using Amss.Boilerplate.Common;
    using Amss.Boilerplate.Data.Specifications;
    using Amss.Boilerplate.Web.Areas.Manage.Models;

    using global::Common.Logging;

    using Microsoft.Practices.Unity;

    public class ApiContext : IDisposable
    {
        #region Constants and Fields

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly UnitOfWork unitOfWork;

        private IQueryable<UserGridModel> users;

        #endregion

        #region Constructors and Destructors

        public ApiContext()
        {
            Log.Debug("Creating ApiContext instance");

            this.unitOfWork = new UnitOfWork();
        }

        #endregion

        #region Public Properties

        [Dependency]
        public IUserManager UserManager { get; set; }

        public IQueryable<UserGridModel> Users
        {
            get
            {
                if (this.users == null)
                {
                    var list = this.UserManager.Query(new UserAll());
                    this.users = (from u in list
                                  select
                                      new UserGridModel
                                          {
                                              Id = (int)u.Id, 
                                              Name = u.Name, 
                                              Email = u.Email, 
                                              /*Login =
                                                  u.UserPasswordCredential != null
                                                      ? u.UserPasswordCredential.Login
                                                      : string.Empty*/
                                          }).AsQueryable();
                }

                return this.users;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            Log.Debug("Disposing ApiContext");

            this.Dispose(true);
        }

        #endregion

        #region Methods

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.unitOfWork != null)
                {
                    this.unitOfWork.Dispose();
                }
            }
        }

        #endregion
    }
}