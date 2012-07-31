namespace Amss.Boilerplate.Web.Areas.Manage.Controllers
{
    using System.Web.Mvc;

    using Amss.Boilerplate.Web.Areas.Manage.Models;
    using Amss.Boilerplate.Web.Common.Controllers;
    using Amss.Boilerplate.Web.Common.Filters;

    using Mvc.JQuery.Datatables;

    public class PartnerController : Controller<PartnerManager>
    {
        #region List

        public ActionResult List(long? id)
        {
            return this.View();
        }

        [HttpPost]
        public DataTablesResult List(DataTablesParam dataTableParam)
        {
            var model = this.Manager.GetPartnersGridModel(dataTableParam);

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
        public ActionResult Create(PartnerModel model)
        {
            if (this.ModelState.IsValid)
            {
                var id = this.Manager.CreatePartner(model);

                return this.RedirectToAction("List", "Partner", new { id });
            }

            this.Manager.FillPartnerModel(model);

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
        public ActionResult Edit(PartnerModel model)
        {
            if (this.ModelState.IsValid)
            {
                var id = this.Manager.UpdatePartner(model);

                return this.RedirectToAction("List", "Partner", new { id });
            }

            this.Manager.FillPartnerModel(model);

            return this.View(model);
        }

        #endregion Edit
    }
}