using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PH6_1803.Lib.CoreLib;

namespace PH6_1803.Lib.CoreApiTest.Controllers
{
    [Route("test")]
    [ApiController]
    public class DefaultController : ControllerBase
    {

        IWebHostEnvironment _hostEnvironment;
        public DefaultController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("download")]
        public IActionResult DownLoad()
        {
            System.IO.MemoryStream ms = NpoiLib.NpoiExcelHelper.ExportExcel(GetData(), null);

            //var filePath = Path.Combine(_hostEnvironment.WebRootPath , Guid.NewGuid().ToString("N") + ".xlsx");           
            //var fileBytes = ms.ToArray();//文件流Byte
            //FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            //ms.WriteTo(fs);
            //ms.Close();
            //fs.Close();
            //return Ok(filePath);

            //return File(ms.ToArray(), "application/vnd.ms-excel");

            //var file = new FileStreamResult(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            var file = new FileStreamResult(ms, "application/octet-stream")
            {
                FileDownloadName = Guid.NewGuid().ToString("N") + ".xlsx"
            };
            return file;
        }

        #region txt

        [HttpGet("txt1")]
        /// <summary>
        /// 方式一 输出文本文件
        /// </summary>
        /// <returns></returns>
        public IActionResult DownloadTxt()
        {
            var buffer = Encoding.UTF8.GetBytes("asp.net core download file");
            return File(buffer, "text/plain", "file.txt");
        }


        [HttpGet("txt2")]
        /// <summary>
        /// 方式二 输出文本文件
        /// </summary>
        /// <returns></returns>
        public IActionResult DownloadFile2()
        {
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            streamWriter.Write("{\"content\":\"asp.net core download file\"}");
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "text/plain", "file.json");
        }



        [HttpGet("txt3")]
        /// <summary>
        /// 方式三 输出图片文件
        /// </summary>
        /// <returns></returns>
        public IActionResult DownloadFile3()
        {
            var path = Path.Combine(_hostEnvironment.ContentRootPath, "UploadFile", "netcore5.png");
            var fileExtensionName = Path.GetExtension(path);
            return PhysicalFile(path, "image/png", $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.{fileExtensionName}");
        }

        #endregion



        [HttpGet("upload")]
        /// <summary>
        /// 多文件上传
        /// </summary>
        /// <param name="files">文件上传的name</param>
        /// <returns></returns>
        public IActionResult UploadFile(IList<IFormFile> files)
        {
            if (files != null)
            {
                //处理多文件
                foreach (var file in files)
                {
                    //如果需要对文件处理,可以根据文件扩展名,进行筛选
                    var fileExtensionName = Path.GetExtension(file.FileName).Substring(1);
                    var saveFilePath = Path.Combine(_hostEnvironment.ContentRootPath, "UploadFile", $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.{fileExtensionName}");
                    var stream = new FileStream(saveFilePath, FileMode.Create);
                    //asp.net core对异步支持很好,如果使用异步可以使用file.CopyToAsync方法
                    file.CopyTo(stream);
                }
                return Ok();
            }
            else
            {
                return Ok();
            }
        }

        /// <summary>
        /// 物理文件下载
        /// </summary>
        /// <returns></returns>

        [HttpGet("d4")] 
        public IActionResult GetFileFromWebApi() {

            System.IO.MemoryStream ms = NpoiLib.NpoiExcelHelper.ExportExcel(GetData(), null);

            var FilePath = Path.Combine(_hostEnvironment.WebRootPath, Guid.NewGuid().ToString("N")+".xlsx");
            //var fileBytes = ms.ToArray();//文件流Byte
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            ms.WriteTo(fs);
            ms.Close();
            fs.Close();

            return PhysicalFile(FilePath, "application/octet-stream", $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx");

            //try {
            //    var stream = new FileStream(FilePath, FileMode.Open);
            //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK); 
            //    response.Content = new StreamContent(stream);
            //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream"); 
            //    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") 
            //    { 
            //        FileName = Guid.NewGuid().ToString("N") + ".xlsx"
            //    }; 
            //    return response; 
            //} catch { 
            //    return new HttpResponseMessage(HttpStatusCode.NoContent); 
            //} 
        }


        public DataTable GetData()
        {
            DataTable dt = new DataTable();
            DataColumn _dcID = new DataColumn("id");
            DataColumn _dcName = new DataColumn("name");
            dt.Columns.Add(_dcName);
            dt.Columns.Add(_dcID);

            for (int i = 0; i < 5; i++)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = i;
                dr["name"] = $"name{i}";

                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
