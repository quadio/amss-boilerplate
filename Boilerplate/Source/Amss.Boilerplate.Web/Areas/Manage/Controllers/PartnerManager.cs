namespace Amss.Boilerplate.Web.Areas.Manage.Controllers
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Amss.Boilerplate.Business;
    using Amss.Boilerplate.Common.Transactions;
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Data.Specifications;
    using Amss.Boilerplate.Web.Areas.Manage.Models;

    using Microsoft.Practices.Unity;

    using Mvc.JQuery.Datatables;

    public class PartnerManager
    {
        [Dependency]
        public IPartnerManager Manager { get; set; }

        [Dependency]
        public IUserManager UserManager { get; set; }

        [Dependency]
        public IRoleManager RoleManager { get; set; }

        public PartnerModel GetCreateModel()
        {
            var model = new PartnerModel();

            this.FillPartnerModel(model);

            return model;
        }

        public void FillPartnerModel(PartnerModel model)
        {
        }

        public DataTablesResult<PartnerGridModel> GetPartnersGridModel(DataTablesParam dataTableParam)
        {
            Contract.Assert(dataTableParam != null);
            Contract.Assert(dataTableParam.iDisplayLength != 0);

            var list = this.Manager.FindAll(new PartnerAll(true));

            var query = (from entity in list
                         select new PartnerGridModel
                         {
                             Id = entity.Id,
                             Name = entity.Name,
                             Disabled = entity.Disabled
                         }).AsQueryable();

            return DataTablesResult.Create(query, dataTableParam);
        }

        public long CreatePartner(PartnerModel model)
        {
            var partner = new PartnerEntity { Name = model.Name, Disabled = model.Disabled };

            this.Manager.Create(partner);

            return partner.Id;
        }

        public PartnerModel GetEditModel(long id)
        {
            var partner = this.Manager.Load(id);

            var model = new PartnerModel { PartnerId = partner.Id, Name = partner.Name, Disabled = partner.Disabled };

            this.FillPartnerModel(model);

            return model;
        }

        public long UpdatePartner(PartnerModel model)
        {
            Contract.Assert(model.PartnerId.HasValue);

            var partner = this.Manager.Load(model.PartnerId.Value);

            partner.Name = model.Name;
            partner.Disabled = model.Disabled;

            using (var tx = new Transaction())
            {
                this.Manager.Update(partner);

                tx.Complete();
            }

            return partner.Id;
        }
    }
}