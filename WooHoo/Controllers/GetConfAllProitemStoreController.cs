using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;
using WooHoo.Configs;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfAllProitemStoreController : WHControllerBase
    {

        /// <summary>
        /// 获取产品库存信息
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult action(int proid)
        {
            Orm.Orm_conf_all_proitems_store orm_Conf_All_Proitems_Store = new Orm.Orm_conf_all_proitems_store();
            orm_Conf_All_Proitems_Store.proid = proid;
            string query = "select * from conf_all_proitems_store where proid=@proid";
            Orm.Orm_conf_all_proitems_store orm_Conf_All_Proitems_Store_Selected = dbConnection.QuerySingleOrDefault<Orm.Orm_conf_all_proitems_store>(query, orm_Conf_All_Proitems_Store);
            if (orm_Conf_All_Proitems_Store_Selected != null)
            {
                return Json(orm_Conf_All_Proitems_Store_Selected);
            }
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