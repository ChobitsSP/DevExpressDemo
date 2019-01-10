using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using Newtonsoft.Json;
using DevExpress.Web.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DevExpressDemo
{
    public static class DevUtils
    {
        /// <summary>
        /// ASPxGridView NewValues to Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValues"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this object newValues) where T : class
        {
            var s = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                // Converters = { new NumberConverter() },
            };
            string json = JsonConvert.SerializeObject(newValues);
            var item = JsonConvert.DeserializeObject<T>(json, s);
            return item;
        }

        public static IEnumerable<KeyValuePair<T, K>> GetItems<T, K>(this IEnumerable<ASPxDataUpdateValues> rows) where K : class
        {
            foreach (var row in rows)
            {
                T key = (T)row.Keys[0];
                K value = row.NewValues.ToEntity<K>();
                yield return new KeyValuePair<T, K>(key, value);
            }
        }

        public static void InitSettings(this ASPxGridView grid)
        {
            grid.SettingsBehavior.ConfirmDelete = true;

            grid.Settings.ShowFilterRow = true;
            grid.Settings.ShowFilterRowMenu = true;
            grid.Settings.ShowHeaderFilterButton = true;
            grid.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;
            grid.Settings.ShowGroupPanel = true;
            grid.Settings.ShowFooter = true;
            grid.Settings.ShowGroupFooter = GridViewGroupFooterMode.VisibleAlways;
            grid.SettingsPager.AlwaysShowPager = true;
            grid.SettingsPager.PageSize = 20;

            grid.SettingsEditing.EditFormColumnCount = 1;
            //grid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            grid.HtmlRowPrepared += DXGridView_HtmlRowPrepared;
            grid.Theme = "Office365";
        }

        public static void InitSettings(this ASPxPivotGrid grid)
        {
            grid.OptionsPager.RowsPerPage = 500;
            grid.Theme = "Office365";
            grid.ClientSideEvents.CellClick = @"
function (s, e) {
  var tr = $(e.HtmlEvent.target).parent();
  tr.parent().find('td').css('background-color', '');
  tr.find('td').css('background-color', '#dff0d8');
}";
        }

        /// <summary>
        /// 设置DXGridView行实现鼠标悬停变色
        /// </summary>
        static void DXGridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                //当鼠标停留时更改背景色
                e.Row.Attributes.Add("onmouseover",
                    "row_bgcolor=this.style.backgroundColor;this.style.backgroundColor='#f5f5f5';");
                //当鼠标移开时还原背景色
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=row_bgcolor;");
            }
        }

        public static void Export(this ASPxPivotGrid ASPxPivotGrid1, ASPxPivotGridExporter ASPxPivotGridExporter1, string fileName, bool saveAs)
        {
            foreach (PivotGridField field in ASPxPivotGrid1.Fields)
            {
                if (field.ValueFormat != null && !string.IsNullOrEmpty(field.ValueFormat.FormatString))
                    field.UseNativeFormat = DefaultBoolean.True;
            }

            ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = true;
            ASPxPivotGridExporter1.OptionsPrint.MergeColumnFieldValues = true;
            ASPxPivotGridExporter1.OptionsPrint.MergeRowFieldValues = true;

            ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = DefaultBoolean.True;
            ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = DefaultBoolean.True;
            ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = DefaultBoolean.True;
            ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = DefaultBoolean.True;

            XlsxExportOptionsEx options;
            options = new XlsxExportOptionsEx()
            {
                ExportType = ExportType.DataAware,
                AllowGrouping = DefaultBoolean.True,
                TextExportMode = TextExportMode.Text,
                AllowFixedColumns = DefaultBoolean.True,
                AllowFixedColumnHeaderPanel = DefaultBoolean.True,
                //RawDataMode = exportRawData.Checked
            };

            ASPxPivotGridExporter1.ExportXlsxToResponse(HttpUtility.UrlEncode(fileName), options, saveAs);
        }

        public static void SetCustomFormat(IFormatProvider format, string fmt, params FormatInfo[] fields)
        {
            foreach (var field in fields)
            {
                field.Format = format;
                field.FormatString = fmt;
                field.FormatType = FormatType.Custom;
            }
        }

        public static T FindEditRowCellTemplateControl<T>(this ASPxGridView grid, string columnName, string id) where T : Control
        {
            Control control = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns[columnName], id);
            return control as T;
        }

        public static string GetEditText(this ASPxGridView grid, string columnName, string id)
        {
            return grid.FindEditRowCellTemplateControl<ASPxTextBox>(columnName, id).Text;
        }

        public class DevPriceFormat : ICustomFormatter, IFormatProvider
        {
            public object GetFormat(Type formatType)
            {
                return (formatType == typeof(ICustomFormatter)) ? this : null;
            }

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (format == null || !format.Trim().StartsWith("P2"))
                {
                    if (arg is IFormattable)
                    {
                        return ((IFormattable)arg).ToString(format, formatProvider);
                    }
                    return arg.ToString();
                }

                decimal value = Convert.ToDecimal(arg);

                //  Here's is where you format your number

                return (value / 100).ToString();
            }
        }

        public static void InitEditForm(this ASPxGridView grid)
        {
            var st = grid.SettingsEditing;
            st.Mode = GridViewEditingMode.PopupEditForm;
            grid.SettingsPopup.EditForm.VerticalAlign = PopupVerticalAlign.WindowCenter;
            grid.SettingsPopup.EditForm.HorizontalAlign = PopupHorizontalAlign.WindowCenter;
            grid.SettingsPopup.EditForm.AllowResize = true;
            grid.HtmlEditFormCreated += ASPxGridView_HtmlEditFormCreated;
        }

        private static void ASPxGridView_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            ASPxPopupControl popup = e.EditForm.NamingContainer as ASPxPopupControl;
            popup.MaxHeight = 600;
            popup.ScrollBars = ScrollBars.Both;
        }
    }
}