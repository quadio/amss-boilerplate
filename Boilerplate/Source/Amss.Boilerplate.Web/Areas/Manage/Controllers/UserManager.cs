namespace Amss.Boilerplate.Web.Areas.Manage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using Amss.Boilerplate.Business;
    using Amss.Boilerplate.Common.Transactions;
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Data.Specifications;
    using Amss.Boilerplate.Web.Areas.Manage.Models;

    using Lib.Web.Mvc.JQuery.JqGrid;

    using Microsoft.Practices.Unity;

    using Mvc.JQuery.Datatables;

    public class UserManager
    {
        [Dependency]
        public IUserManager Manager { get; set; }

        [Dependency]
        public IPartnerManager PartnerManager { get; set; }

        [Dependency]
        public IRoleManager RoleManager { get; set; }

        public UserModel GetCreateModel()
        {
            var model = new UserModel();

            this.FillUserModel(model);

            return model;
        }

        public void FillUserModel(UserModel model)
        {
            var partners = this.PartnerManager.FindAll(new PartnerAll());
            var roles = this.RoleManager.FindAll(new RoleAll());

            model.AvailablePartners = partners.ToDictionary(p => p.Id, p => p.Name);
            model.AvailableRoles = roles.ToDictionary(p => p.Id, p => p.Name);
        }

        public JqGridJsonResult GetUsersGridModel(JqGridRequest request)
        {
            var enumerable = this.Manager.FindAll(new UserAll(true));

            var users = new List<UserEntity>(enumerable);

            var totalRecordsCount = users.Count();

            var list = (from u in users
                        select
                            new JqGridRecord<UserGridModel>(
                                u.Id.ToString(CultureInfo.InvariantCulture), 
                                new UserGridModel
                                {
                                    Id = u.Id,
                                    Name = u.Name,
                                    Email = u.Email,
                                    Login = u.UserPasswordCredential != null ? u.UserPasswordCredential.Login : string.Empty,
                                })).ToList();

            var response = new JqGridResponse
                {
                    TotalPagesCount = (int)Math.Ceiling(totalRecordsCount / (float)request.RecordsCount),
                    PageIndex = request.PageIndex,
                    TotalRecordsCount = totalRecordsCount,
                };

            response.Records.AddRange(list);

            return new JqGridJsonResult { Data = response };
        }

        public DataTablesResult GetUsersGridModel(DataTablesParam dataTableParam)
        {
            Contract.Assert(dataTableParam != null);
            Contract.Assert(dataTableParam.iDisplayLength != 0);

            var pageIndex = dataTableParam.iDisplayStart / dataTableParam.iDisplayLength;
            var queryData = new UserAll { PageIndex = pageIndex, PageSize = dataTableParam.iDisplayLength };

            var query = this.Manager.FindAll(queryData);
            var count = this.Manager.Count(queryData);

            var list = (from u in query
                        select new[] 
                                { 
                                    u.Id.ToString(CultureInfo.InvariantCulture),
                                    u.Name,
                                    u.Email,
                                    u.UserPasswordCredential != null ? u.UserPasswordCredential.Login : string.Empty
                                }).OfType<object>().ToArray();

            var result = new DataTablesResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                    Data = new DataTablesData
                        {
                            iTotalRecords = dataTableParam.iDisplayLength,
                            iTotalDisplayRecords = count,
                            sEcho = dataTableParam.sEcho, 
                            aaData = list
                        }
                };

            return result;
        }

        public DataTablesResult<UserGridModel> GetUsersGridModel2(DataTablesParam dataTableParam)
        {
            Contract.Assert(dataTableParam != null);
            Contract.Assert(dataTableParam.iDisplayLength != 0);

            var list = this.Manager.FindAll(new UserAll());

            var query = (from u in list
                         select new UserGridModel
                                 {
                                     Id = u.Id,
                                     Name = u.Name,
                                     Email = u.Email,
                                     Login = u.UserPasswordCredential != null ? u.UserPasswordCredential.Login : string.Empty,
                                 }).AsQueryable();

            return DataTablesResult.Create(query, dataTableParam);
        }

        public long CreateUser(UserModel model)
        {
            PartnerEntity partner = null;
            if (model.PartnerId.HasValue)
            {
                partner = this.PartnerManager.Load(model.PartnerId.Value);
            }

            var role = this.RoleManager.Load(model.RoleId);

            var user = new UserEntity
                {
                    Name = model.Name,
                    Email = model.Email,
                    Partner = partner,
                    Role = role
                };

            this.Manager.Create(user, model.Login, model.Password);

            return user.Id;
        }

        public UserModel GetEditModel(long id)
        {
            var user = this.Manager.Load(id);

            var model = new UserModel
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Login = user.UserPasswordCredential != null ? user.UserPasswordCredential.Login : string.Empty,
                    PartnerId = user.Partner != null ? user.Partner.Id : (long?)null,
                    RoleId = user.Role.Id
                };

            this.FillUserModel(model);

            return model;
        }

        public long EditUser(UserModel model)
        {
            Contract.Assert(model.UserId.HasValue);

            var user = this.Manager.Load(model.UserId.Value);
            var role = this.RoleManager.Load(model.RoleId);

            PartnerEntity partner = null;
            if (model.PartnerId.HasValue)
            {
                partner = this.PartnerManager.Load(model.PartnerId.Value);
            }

            user.Name = model.Name;
            user.Email = model.Email;
            user.UserPasswordCredential.Login = model.Login;
            user.Role = role;
            user.Partner = partner;

            using (var tx = new Transaction())
            {
                this.Manager.Update(user);

                tx.Complete();
            }
            
            return user.Id;
        }
    }
}