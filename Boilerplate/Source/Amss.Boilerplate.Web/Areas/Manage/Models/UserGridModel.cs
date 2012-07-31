namespace Amss.Boilerplate.Web.Areas.Manage.Models
{
    using System.ComponentModel;
    using System.Data.Services.Common;
    using System.Web.Mvc;

    using Lib.Web.Mvc.JQuery.JqGrid;
    using Lib.Web.Mvc.JQuery.JqGrid.DataAnnotations;

    [DataServiceKey("Id")]
    public class UserGridModel
    {
        [HiddenInput]
        [JqGridColumnSortable(true)]
        [JqGridColumnLayout(Alignment = JqGridAlignments.Center)]
        public long Id { get; set; }

        [DisplayName("User Name")]
        [JqGridColumnSortable(true)]
        [JqGridColumnLayout(Alignment = JqGridAlignments.Center)]
        [JqGridColumnFormatter("'showlink'", BaseLinkUrl = "/Manage/User/Edit/", IdName = "Id")]
        public string Name { get; set; }

        [DisplayName("Login")]
        [JqGridColumnSortable(true)]
        [JqGridColumnLayout(Alignment = JqGridAlignments.Center)]
        public string Login { get; set; }

        [DisplayName("Email")]
        [JqGridColumnSortable(true)]
        [JqGridColumnLayout(Alignment = JqGridAlignments.Center)]
        public string Email { get; set; }
    }
}