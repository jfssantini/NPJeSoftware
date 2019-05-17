using System.Web.Optimization;

namespace NPJe.FrontEnd
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Principais
            bundles.Add(new ScriptBundle("~/Js").Include(
                "~/Content/plugins/jQuery/jquery-2.2.3.min.js",
                "~/Content/bootstrap/js/bootstrap.min.js",
                "~/Content/plugins/datatables/jquery.dataTables.min.js",
                "~/Content/plugins/datatables/dataTables.bootstrap.min.js",
                "~/Content/plugins/slimScroll/jquery.slimscroll.min.js",
                "~/Content/plugins/fastclick/fastclick.js",
                "~/Content/dist/js/app.min.js",
                "~/Content/dist/js/demo.js"));

            bundles.Add(new StyleBundle("~/Css").Include(
                "~/Content/bootstrap/css/bootstrap.min.css",
                "~/Content/dist/css/AdminLTE.min.css",
                "~/Content/dist/css/skins/_all-skins.min.css"));
            #endregion

            #region Máscaras
            bundles.Add(new ScriptBundle("~/PluginsMask").Include(
                "~/Content/plugins/input-mask/jquery.inputmask.js",
                "~/Content/plugins/input-mask/jquery.inputmask.extensions.js",
                "~/Content/plugins/input-mask/jquery.inputmask.regex.extensions.js",
                "~/Content/plugins/input-mask/jquery.inputmask.date.extensions.js",
                "~/Content/plugins/input-mask/jquery.inputmask.numeric.extensions.js",
                "~/Content/plugins/input-mask/jquery.inputmask.phone.extensions.js",
                "~/Content/plugins/input-mask/phone-codes/phone-codes.json",
                "~/Content/plugins/input-mask/phone-codes/phone-be.json"));
            #endregion

            #region DatePicker
            bundles.Add(new ScriptBundle("~/PluginsDatePicker").Include(
                "~/Content/plugins/timepicker/bootstrap-timepicker.js",
                "~/Content/plugins/datepicker/bootstrap-datepicker.js",
                "~/Content/plugins/datepicker/locales/bootstrap-datepicker.pt-BR.js"));

            bundles.Add(new StyleBundle("~/PluginsDatePickerCss").Include(
                "~/Content/plugins/timepicker/bootstrap-timepicker.css",
                "~/Content/plugins/datepicker/datepicker3.css"));
            #endregion

            bundles.Add(new ScriptBundle("~/Extras").Include(
                "~/Content/plugins/jquery-maskmoney-master/dist/jquery.maskMoney.js",
                "~/Content/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"));

            bundles.Add(new StyleBundle("~/ExtrasCss").Include(
                "~/Content/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"));


            BundleTable.EnableOptimizations = true;
        }
    }
}
