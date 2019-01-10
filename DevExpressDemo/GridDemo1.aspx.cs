using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpressDemo.DbModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DevExpressDemo
{
    public partial class GridDemo1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxGridView1.InitSettings();
            ASPxGridView1.InitEditForm();
            ASPxGridView1.RowInserting += ASPxGridView1_RowInserting;
            ASPxGridView1.RowDeleting += ASPxGridView1_RowDeleting;
            ASPxGridView1.RowValidating += ASPxGridView1_RowValidating;
        }

        protected void ASPxGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            var grid = (ASPxGridView)sender;
            var item = e.NewValues.ToEntity<Table1>();

            item.CreateTime = DateTime.Now;
            item.FileUrl = grid.GetEditText("url", "ASPxTextBox1");

            using (var db = new Entities())
            {
                db.Table1.Add(item);
                db.SaveChanges();
            }

            e.Cancel = true;
            grid.CancelEdit();
        }

        protected void ASPxGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            var grid = (ASPxGridView)sender;
            var id = Convert.ToInt32(e.Keys[0]);

            using (var db = new Entities())
            {
                var old = db.Table1.First(t => t.Id == id);
                var filepath = Server.MapPath(old.FileUrl);

                db.Table1.Remove(old);
                db.SaveChanges();
                if (File.Exists(filepath)) File.Delete(filepath);
            }

            e.Cancel = true;
            grid.CancelEdit();
        }

        protected void ASPxUploadControl1_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            //string[] extension = e.UploadedFile.FileName.Split('.');

            //if (extension[1] != "zip")
            //{
            //    e.IsValid = false;
            //    return;
            //}

            if (e.UploadedFile != null)
            {
                var FilePath = FildSave(e.UploadedFile.SaveAs, e.UploadedFile.FileName, @"upload");
                var ClientID = (sender as ASPxUploadControl).ClientID;

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    FilePath,
                    ClientID,
                });

                e.CallbackData = json;
            }
        }

        public static string FildSave(Action<string> SaveAs, string fileName, string folder)
        {
            var rootPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            string fullName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
            string longPath = Path.Combine(rootPath, folder.TrimStart('\\', '/'));
            if (!Directory.Exists(longPath)) Directory.CreateDirectory(longPath);

            string localPath = Path.Combine(longPath, fullName);
            SaveAs(localPath);

            string urlPath = '/' + Path
                .Combine(folder, fullName)
                .Replace('\\', '/')
                .Trim('/');

            return urlPath;
        }

        protected void ASPxGridView1_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            var grid = (sender as DevExpress.Web.ASPxGridView);
            var FileUrl = grid.GetEditText("FileUrl", "ASPxTextBox1");
            if (string.IsNullOrEmpty(FileUrl))
            {
                e.Errors.Add(grid.Columns["FileUrl"], "FileUrl required");
                e.RowError = "FileUrl required";
                return;
            }
        }
    }
}