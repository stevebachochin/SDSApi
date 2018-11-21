using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SDSApi.Models;

namespace SDSAPI.Controllers
{
    public class FileUploadController : ApiController
    {
        //HttpResponseMessage Put(int id, Product item)
        /**
         
         public async Task<IActionResult> PutFiles([FromRoute] int id, [FromBody] Files files)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != files.Fid)
            {
                return BadRequest();
            }
         **/
        [HttpPost]
        [Route("api/UploadFile")]
        public HttpResponseMessage UploadFile()
        {
            string fileName = null;
            var httpRequest = HttpContext.Current.Request;
            var postedFile = httpRequest.Files["File"];
            //Create custom filename
            fileName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/FileLibrary/" + fileName);
            postedFile.SaveAs(filePath);
            //Save to DB
            using (DBModel db = new DBModel())
            {
                SDSApi.Models.File file = new SDSApi.Models.File()
                {
                    Sdsfid = httpRequest["SDSId"],
                    FileName = fileName
                };
                db.Files.Add(file);
                db.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [Route("api/DeleteFile/{fileName}")]
        // public HttpResponseMessage DeleteFile()
        public void Put(string fileName)
        {
            fileName = fileName.Replace("~", ".");
            var fullPath = HttpContext.Current.Server.MapPath("~/FileLibrary/" + fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
