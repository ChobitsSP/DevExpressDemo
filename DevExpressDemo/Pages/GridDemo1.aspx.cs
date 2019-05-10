using DevExpress.Export;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.XtraPrinting;
using DevExpressDemo.DbModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace DevExpressDemo.Pages
{
    public partial class GridDemo1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxGridView1.InitSettings();
            ASPxGridView1.InitEditForm();
            ASPxGridView1.RowInserting += ASPxGridView1_RowInserting;
            ASPxGridView1.RowDeleting += ASPxGridView1_RowDeleting;
            ASPxGridView1.RowUpdating += ASPxGridView1_RowUpdating;
            ASPxGridView1.RowValidating += ASPxGridView1_RowValidating;
        }

        protected void ASPxGridView1_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            var grid = (ASPxGridView)sender;
            var item = e.NewValues.ToEntity<Table1>();

            item.CreateTime = DateTime.Now;
            item.FileUrl = grid.GetEditText("FileUrl", "ASPxTextBox1");

            using (var db = new Entities())
            {
                db.Table1.Add(item);
                db.SaveChanges();
            }

            e.Cancel = true;
            grid.CancelEdit();
        }

        private void ASPxGridView1_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            var grid = (ASPxGridView)sender;
            var item = e.NewValues.ToEntity<Table1>();

            item.FileUrl = grid.GetEditText("FileUrl", "ASPxTextBox1");

            using (var db = new Entities())
            {
                var old = db.Table1.Find(e.Keys[0]);

                old.Name = item.Name;
                old.IsDel = item.IsDel;
                old.FileUrl = item.FileUrl;

                db.SaveChanges();
            }

            e.Cancel = true;
            grid.CancelEdit();
        }

        protected void ASPxGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            var grid = (ASPxGridView)sender;

            using (var db = new Entities())
            {
                var old = db.Table1.Find(e.Keys[0]);
                db.Table1.Remove(old);
                db.SaveChanges();
            }

            e.Cancel = true;
            grid.CancelEdit();
        }

        protected void ASPxGridView1_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            //var grid = (sender as ASPxGridView);
            //var FileUrl = grid.GetEditText("FileUrl", "ASPxTextBox1");
            //if (string.IsNullOrEmpty(FileUrl))
            //{
            //    e.Errors.Add(grid.Columns["FileUrl"], "FileUrl required");
            //    e.RowError = "FileUrl required";
            //    return;
            //}
        }

        protected void Export_ServerClick(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.WriteXlsxToResponse("table1.xlsx", new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }
    }
}