namespace Amss.Boilerplate.Web.Areas.Manage.Controllers
{
    using System.Web.Mvc;

    using Amss.Boilerplate.Business.Exceptions;
    using Amss.Boilerplate.Web.Areas.Manage.Models;
    using Amss.Boilerplate.Web.Common;
    using Amss.Boilerplate.Web.Common.Controllers;
    using Amss.Boilerplate.Web.Common.Filters;

    using Lib.Web.Mvc.JQuery.JqGrid;

    using Mvc.JQuery.Datatables;

    public class UserController : Controller<UserManager>
    {
        #region List

        public ActionResult List(long? id)
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult List(JqGridRequest request)
        {
            var model = this.Manager.GetUsersGridModel(request);

            return model;
        }

        [HttpPost]
        public DataTablesResult List2(DataTablesParam dataTableParam)
        {
            var model = this.Manager.GetUsersGridModel(dataTableParam);

            return model;
        }

        [HttpPost]
        public DataTablesResult List3(DataTablesParam dataTableParam)
        {
            var model = this.Manager.GetUsersGridModel2(dataTableParam);

            return model;
        }

        #endregion List

        #region Create

        public ActionResult Create()
        {
            var model = this.Manager.GetCreateModel();

            return this.View(model);
        }

        [BusinessValidationFilter]
        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var id = this.Manager.CreateUser(model);

                    return this.RedirectToAction("List", "User", new { id });
                }
                catch (BusinessValidationException exc)
                {
                    this.ModelState.FillFrom(exc);
                }
            }

            this.Manager.FillUserModel(model);

            return this.View(model);
        }

        #endregion Create

        #region Edit

        public ActionResult Edit(long id)
        {
            var model = this.Manager.GetEditModel(id);

            return this.View(model);
        }

        [BusinessValidationFilter]
        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var id = this.Manager.EditUser(model);

                    return this.RedirectToAction("List", "User", new { id });
                }
                catch (BusinessValidationException exc)
                {
                    this.ModelState.FillFrom(exc);
                }
            }

            this.Manager.FillUserModel(model);

            return this.View(model);
        }

        #endregion Edit
        
        /*
         public ActionResult ChangePassword()
                {
                    return this.View();
                }

                [HttpPost]
                public ActionResult ChangePassword(ChangePasswordModel model)
                {
                    if (this.ModelState.IsValid)
                    {

                        // ChangePassword will throw an exception rather
                        // than return false in certain failure scenarios.
                        bool changePasswordSucceeded;
                        try
                        {
                            MembershipUser currentUser = Membership.GetUser(this.User.Identity.Name, userIsOnline: true);
                            changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                        }
                        catch (Exception)
                        {
                            changePasswordSucceeded = false;
                        }

                        if (changePasswordSucceeded)
                        {
                            return this.RedirectToAction("ChangePasswordSuccess");
                        }
                        else
                        {
                            this.ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                        }
                    }

                    // If we got this far, something failed, redisplay form
                    return this.View(model);
                }

                public ActionResult ChangePasswordSuccess()
                {
                    return this.View();
                }
         */
    }
}