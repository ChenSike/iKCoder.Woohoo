using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;
using WooHoo.Configs;
using WooHoo.Filter;
using WooHoo.Orm;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfAllProItemPriceController : WHControllerBase
    {
        /// <summary>
        /// 获取具体产品价格信息
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        [HttpGet]
        [Filter_ConnectDB]
        public ActionResult Action(int proid,string modell1,string modell2)
        {
            Orm.Orm_conf_all_proitems_price orm_Conf_All_Proitems_Price = new Orm.Orm_conf_all_proitems_price();
            orm_Conf_All_Proitems_Price.proid = proid;
            string query = "select * from conf_all_proitems_price where proid=@proid and modell1='" + modell1 + "' and modell2='" + modell2 + "'";
            orm_Conf_All_Proitems_Price = dbConnection.Query<Orm_conf_all_proitems_price>(query).SingleOrDefault();
            if (orm_Conf_All_Proitems_Price != null)
                return Json(orm_Conf_All_Proitems_Price);
            else
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "424";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "No Data";
                HttpContext.Response.StatusCode = 424;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}