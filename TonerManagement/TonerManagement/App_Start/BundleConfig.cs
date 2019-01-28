using System.Web.Optimization;

namespace TonerManagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //jqwidgets bundle
            bundles.Add(new ScriptBundle("~/bundles/jqwidgets").Include(
                "~/Scripts/jqxcore.js",
                "~/Scripts/jqxdata.js",
                "~/Scripts/jqxgrid.js",
                "~/Scripts/jqxgrid.selection.js",
                "~/Scripts/jqxgrid.pager.js",
                "~/Scripts/jqxlistbox.js",
                "~/Scripts/jqxbuttons.js",
                "~/Scripts/jqxscrollbar.js",
                "~/Scripts/jqxdatatable.js",
                "~/Scripts/jqxtreegrid.js",
                "~/Scripts/jqxmenu.js",
                "~/Scripts/jqxcalendar.js",
                "~/Scripts/jqxgrid.sort.js",
                "~/Scripts/jqxgrid.filter.js",
                "~/Scripts/jqxdatetimeinput.js",
                "~/Scripts/jqxdropdownlist.js",
                "~/Scripts/jqxslider.js",
                "~/Scripts/jqxeditor.js",
                "~/Scripts/jqxinput.js",
                "~/Scripts/jqxdraw.js",
                "~/Scripts/jqxchart.core.js",
                "~/Scripts/jqxchart.rangeselector.js",
                "~/Scripts/jqxtree.js",
                "~/Scripts/globalize.js",
                "~/Scripts/jqxbulletchart.js",
                "~/Scripts/jqxcheckbox.js",
                "~/Scripts/jqxradiobutton.js",
                "~/Scripts/jqxvalidator.js",
                "~/Scripts/jqxpanel.js",
                "~/Scripts/jqxpasswordinput.js",
                "~/Scripts/jqxnumberinput.js",
                "~/Scripts/jqxcombobox.js",
                "~/Scripts/jqxform.js",
                "~/Scripts/jqxgrid.columnsresize.js",
                "~/Scripts/webcomponents-lite.min.js"
            ));
        }
    }
}
