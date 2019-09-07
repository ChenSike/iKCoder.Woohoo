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
    public class SetConfAllShopingCartSelectItemController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(int id,string guid)
        {
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("ShopingCartUpdate");
            try
            {
                Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart = new Orm.Orm_conf_all_shopcart();
                orm_Conf_All_Shopcart.guid = guid;
                orm_Conf_All_Shopcart.id = id;
                orm_Conf_All_Shopcart.selected = "1";
                orm_Conf_All_Shopcart.udt = DateTime.Now.ToString();
                string query = "update conf_all_shopcart set selected=@selected where id=@id and guid=@guid";
                dbConnection.Execute(query, orm_Conf_All_Shopcart);
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "ok";
                conf_ResponseMessageObj.message = "Executed";
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
            catch (Exception err)
            {
                globalTestingLog.AddRecord("stace", err.StackTrace);
                globalTestingLog.AddRecord("msg", err.Message);
                return Content("");
            }
        }
    }
}