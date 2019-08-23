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
        [HttpGet]
        [Filter_ConnectDB]
        public ActionResult Action(int proid)
        {
            Orm.Orm_conf_all_proitems_price orm_Conf_All_Proitems_Price = new Orm.Orm_conf_all_proitems_price();
            orm_Conf_All_Proitems_Price.proid = proid;
            string query = "select * from conf_all_proitems_price where proid=@proid";
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