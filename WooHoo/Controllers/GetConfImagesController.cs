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
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("GetImg");
            try
            {
                FileStream fs = new FileStream("images\\" + imgkey, FileMode.Open);
                globalTestingLog.AddRecord("Step", "1");
                BinaryReader binaryReader = new BinaryReader(fs);
                globalTestingLog.AddRecord("Step", "2");
                byte[] buffer = binaryReader.ReadBytes((int)fs.Length);
                globalTestingLog.AddRecord("Step", "3");
                string base64 = Convert.ToBase64String(buffer);
                globalTestingLog.AddRecord("Step", "4");
                binaryReader.Close();
                fs.Close();
                string[] filenameAttrs = imgkey.Split(".");
                globalTestingLog.AddRecord("Step", "5");
                string entendType = filenameAttrs[filenameAttrs.Length - 1];
                globalTestingLog.AddRecord("Step", "6");
                string result = "data:image/";
                result = result + entendType + ";base64,";
                return Content(result + base64);
            }
            catch (Exception err)
            {
                globalTestingLog.AddRecord("Msg", err.Message);
                globalTestingLog.AddRecord("Stace", err.StackTrace);
                return Content("");
            }            
        }
    }
}