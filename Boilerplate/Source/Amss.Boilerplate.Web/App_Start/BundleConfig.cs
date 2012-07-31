namespace Amss.Boilerplate.Web.App_Start
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/site.bootstrap.validate.js",
                "~/Scripts/mvcfoolproof.unobtrusive.min.js"));

            bundles.Add(new StyleBundle("~/Content/css/general").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css/jquery.ui").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"));

            /* jqGrid */
            bundles.Add(new ScriptBundle("~/bundles/iqgrid").Include(
                "~/Scripts/i18n/grid.locale-en.js",
                "~/Scripts/jquery.jqGrid.min.js"));

            bundles.Add(new StyleBundle("~/Content/css/jqgrid").Include(
                "~/Content/jquery.jqGrid/ui.jqgrid.css"));

            /* datatables */
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/jquery.dataTables-1.9.2.js",
                "~/Scripts/jquery.dataTables-1.9.2.paging.js"));

            bundles.Add(new StyleBundle("~/Content/css/datatables").Include(
                 "~/Content/jquery.datatables-1.9.2/css/jquery.dataTables.css",
                 "~/Content/jquery.datatables-1.9.2/css/jquery.dataTables.bootstrapper.css"));
        }
    }
}