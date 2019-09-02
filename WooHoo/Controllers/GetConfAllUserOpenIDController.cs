using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;


namespace WooHoo.Controllers
{
    
    public class JC_OpenIDDefined
    {
        public string openid
        {
            set;
            get;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GetConfAllUserOpenIDController : WHControllerBase
    {
        /// <summary>
        /// 获取微信用户OPENID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Action(string code)
        {
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("GetOpenOD");
            JC_OpenIDDefined obj = new JC_OpenIDDefined();
            obj.openid = Global.GlobalFunctions.GetOpenIDFromWX(code);
            globalTestingLog.AddRecord("OPENID", obj.openid);
            return Json(obj);
        }
    }
}