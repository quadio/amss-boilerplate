namespace Amss.Boilerplate.Web.Areas.Manage.Models
{
    using System.ComponentModel;
    using System.Data.Services.Common;
    using System.Web.Mvc;

    using Lib.Web.Mvc.JQuery.JqGrid;
    using Lib.Web.Mvc.JQuery.JqGrid.DataAnnotations;

    [DataServiceKey("Id")]
    public class PartnerGridModel
    {
        [HiddenInput]
        [JqGridColumnSortable(true)]
        [JqGridColumnLayout(Alignment = JqGridAlignments.Center)]
        public long Id { get; set; }

        [DisplayName("Partner Name")]
        [JqGridColumnSortable(true)]
        [JqGridColumnLayout(Alignment = JqGridAlignments.Center)]
        [JqGridColumnFormatter("'showlink'", BaseLinkUrl = "/Manage/Partner/Edit/", IdName = "Id")]
        public string Name { get; set; }

        [JqGridColumnSortable(false)]
        public bool Disabled { get; set; }
    }
}