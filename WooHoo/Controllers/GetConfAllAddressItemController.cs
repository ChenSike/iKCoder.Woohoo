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
    public class GetConfAllAddressItemController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(int id)
        {
            string query = "select * from conf_all_address where id=@id";
            Orm.Orm_conf_all_address orm_Conf_All_Address = new Orm.Orm_conf_all_address();
            orm_Conf_All_Address.id = id;
            Orm.Orm_conf_all_address orm_Conf_All_Addresses = dbConnection.Query<Orm.Orm_conf_all_address>(query, orm_Conf_All_Address).FirstOrDefault();
            if(orm_Conf_All_Address!=null)
                return Json(orm_Conf_All_Addresses);
            else
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "500";
                conf_ResponseMessageObj.status = "error";
                conf_ResponseMessageObj.message = "Faild to get address item";
                HttpContext.Response.StatusCode = 500;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}