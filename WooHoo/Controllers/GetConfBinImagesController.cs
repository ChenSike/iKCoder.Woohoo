using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using WooHoo.Base;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfBinImagesController : WHControllerBase
    {
        [HttpGet]
        public FileContentResult Action(string imgkey)
        {
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("GetBinImg");
            try
            {
                FileStream fs = new FileStream("images\\" + imgkey, FileMode.Open);
                BinaryReader binaryReader = new BinaryReader(fs);
                byte[] buffer = binaryReader.ReadBytes((int)fs.Length);
                binaryReader.Close();
                fs.Close();
                return File(buffer, "image/png");
            }
            catch (Exception err)
            {
                globalTestingLog.AddRecord("Msg", err.Message);
                globalTestingLog.AddRecord("Stace", err.StackTrace);
                return null;
            }
        }
    }
}