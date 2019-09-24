using System.Web;
using System.Web.Optimization;

namespace FAN.Admin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/jquery").Include(
                        "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/jquery.cookie.js"));

            #region 主题样式
            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/default/css").Include(
                "~/Scripts/jquery_easyui/themes/default/easyui.css"
                ));
            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/black/css").Include(
                "~/Scripts/jquery_easyui/themes/black/easyui.css"
                ));
            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/Silver/css").Include(
                "~/Scripts/jquery_easyui/themes/Silver/easyui.css"
                ));
            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/gray/css").Include(
                "~/Scripts/jquery_easyui/themes/gray/easyui.css"
                ));
            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/metro/css").Include(
                "~/Scripts/jquery_easyui/themes/metro/easyui.css"
                ));
            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/metro-blue/css").Include(
                "~/Scripts/jquery_easyui/themes/metro-blue/easyui.css"
                ));
            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/metro-gray/css").Include(
                "~/Scripts/jquery_easyui/themes/metro-gray/easyui.css"
                ));
            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/metro-green/css").Include(
                "~/Scripts/jquery_easyui/themes/metro-green/easyui.css"
                ));
            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/metro-orange/css").Include(
                "~/Scripts/jquery_easyui/themes/metro-orange/easyui.css"
                ));
            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/metro-red/css").Include(
                "~/Scripts/jquery_easyui/themes/gray/easyui.css"
                ));

            bundles.Add(new StyleBundle("~/Scripts/jquery_easyui/themes/icon").Include(
                "~/Scripts/jquery_easyui/themes/icon-all.css",
                "~/Scripts/jquery_easyui/themes/icon.css"
                ));
            #endregion

            bundles.Add(new ScriptBundle("~/easyuijs_extensions").Include(
                "~/Scripts/jquery_easyui/locale/easyui-lang-zh_CN.js",
                "~/Scripts/jquery_easyuiExtensions/jquery.jdirk.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.form.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.icons.all.js",
                "~/Scripts/jquery_easyuiExtensions/jquery.toolbar.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.menu.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.panel.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.window.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.dialog.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.datagrid.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.icons.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.theme.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.tabs.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.gridselector.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.linkbutton.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.tree.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.treegrid.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.validatebox.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.combo.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.combobox.js",
                "~/Scripts/jquery_easyuiExtensions/jeasyui.extensions.combotree.js",
                "~/Scripts/jquery_easyuiExtensions/treegrid-dnd.js"
                ));

            bundles.Add(new ScriptBundle("~/easyuijs_extends").Include(
                "~/Scripts/jquery_easyui/extend/tree_search.js",
                 "~/Scripts/jquery_easyui/extend/datagrid_excel.js",
                 "~/Scripts/jquery_easyui/plugins/jquery.combotree.js"
            ));
        }
    }
}
