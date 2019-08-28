using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using WooHoo.Configs;
using WooHoo.Filter;
using Dapper;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfAllProItemController : WHControllerBase
    {
        /// <summary>
        /// 获取具体产品信息
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(int proid)
        {
            Orm.Orm_conf_all_proitems orm_Conf_All_Proitems = new Orm.Orm_conf_all_proitems();
            orm_Conf_All_Proitems.id = proid;
            string qurey = "select * from conf_all_proitems id=@id";
            orm_Conf_All_Proitems = dbConnection.Query<Orm.Orm_conf_all_proitems>(qurey, orm_Conf_All_Proitems).SingleOrDefault();
            if (orm_Conf_All_Proitems != null)
                return Json(orm_Conf_All_Proitems);
            else
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "411";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "Faild to select";
                HttpContext.Response.StatusCode = 411;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}