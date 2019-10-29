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
    public class SetConfAllOrdersRemoveController : WHControllerBase
    {
        
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string orderid)
        {
            string query = "delete from conf_all_orders where orderid='" + orderid + "'";
            dbConnection.Execute(query);
            query = "delete from conf_all_orders_proitems where orderid='" + orderid + "'";
            dbConnection.Execute(query);
            query = "delete from conf_all_orders_address where orderid='" + orderid + "'";
            dbConnection.Execute(query);
            Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
            conf_ResponseMessageObj.code = "200";
            conf_ResponseMessageObj.status = "OK";
            conf_ResponseMessageObj.message = orderid;
            HttpContext.Response.StatusCode = 200;
            return Json(conf_ResponseMessageObj);
        }
        
    }
}