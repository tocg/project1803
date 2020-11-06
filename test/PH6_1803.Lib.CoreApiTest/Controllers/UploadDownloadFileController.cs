using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PH6_1803.Lib.CoreLib;

namespace PH6_1803.Lib.CoreApiTest.Controllers
{
    [Route("file")]
    [ApiController]
    public class UploadDownloadFileController : ControllerBase
    {
        IWebHostEnvironment _hostEnvironment;
        public UploadDownloadFileController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        #region Upload

        [HttpGet("upload")]
        /// <summary>
        /// 多文件上传
        /// </summary>
        /// <param name="files">文件上传的name  【表单中：enctype="multipart/form-data"】</param>
        /// <returns></returns>
        public IActionResult UploadFile(IList<IFormFile> files)
        {

            //var files = Request.Form.Files;

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

        #endregion


        #region Download

        [HttpGet("d1_txt")]
        /// <summary>
        /// 方式一 输出文本文件
        /// </summary>
        /// <returns></returns>
        public IActionResult DownloadTxt()
        {
            var buffer = Encoding.UTF8.GetBytes("asp.net core download file");
            return File(buffer, "text/plain", "file.txt");
        }


        [HttpGet("d2_json")]
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



        [HttpGet("d3_img")]
        /// <summary>
        /// 方式三 输出图片文件
        /// </summary>
        /// <returns></returns>
        public IActionResult DownloadFile3()
        {
            var path = Path.Combine(_hostEnvironment.ContentRootPath, "UploadFile", "netcore5.png");
            var fileExtensionName = Path.GetExtension(path);
            return PhysicalFile(path, "image/png", $"{DateTime.Now:yyyyMMddhhmmss}.{fileExtensionName}");
        }

        [HttpGet("d4_excel")]
        public IActionResult GetFileFromWebApi()
        {
            //通过类库，将Datatable写入内存流
            System.IO.MemoryStream ms = NpoiLib.NpoiExcelHelper.ExportExcel(GetData(), null);

            //临时路径
            var FilePath = Path.Combine(_hostEnvironment.WebRootPath, Guid.NewGuid().ToString("N") + ".xlsx");
            
            //将内存流写入临时路径的文件
            FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate);
            ms.WriteTo(fs);
            ms.Close();
            fs.Close();

            //下载临时文件
            return PhysicalFile(FilePath, "application/octet-stream", $"{DateTime.Now:yyyyMMddhhmmss}.xlsx");
        }

        DataTable GetData()
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

        #endregion

    }
}
