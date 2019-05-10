using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DevExpressDemo.Controllers
{
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        /// <summary>
        /// 上传临时存储文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Upload")]
        public async Task<string> Upload()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = HttpContext.Current.Server.MapPath("~/temp_files");
            if (!Directory.Exists(root)) Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);

            // Read the form data.
            await Request.Content.ReadAsMultipartAsync(provider);
            MultipartFileData file = provider.FileData[0];
            // var id = new Guid(provider.FormData["id"]);

            var fileName = file.Headers.ContentDisposition.FileName.Trim('"');
            fileName = Guid.NewGuid() + Path.GetExtension(fileName);
            File.Copy(file.LocalFileName, Path.Combine(root, fileName));

            var PhotoUrl = "/temp_files/" + fileName;
            return PhotoUrl;
        }
    }
}