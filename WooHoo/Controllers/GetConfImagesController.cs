using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;
using WooHoo.Configs;
using System.IO;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfImagesController : WHControllerBase
    {

        /// <summary>
        /// 获取BASE64格式的图片
        /// </summary>
        /// <param name="imgkey"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Action(string imgkey)
        {
            if (!Directory.Exists("images"))
            {
                Directory.CreateDirectory("images");
                return Content("");
            }
            else
            {
                FileStream fs = new FileStream("\\images\\" + imgkey, FileMode.Open);
                BinaryReader binaryReader = new BinaryReader(fs);
                byte[] buffer = binaryReader.ReadBytes((int)fs.Length);
                string base64 = Convert.ToBase64String(buffer);
                binaryReader.Close();
                fs.Close();
                string[] filenameAttrs = imgkey.Split(".");
                string entendType = filenameAttrs[filenameAttrs.Length - 1];
                string result = "data:image/";
                result = result + entendType + ";base64,";
                return Content(result + base64);
            }
        }
    }
}