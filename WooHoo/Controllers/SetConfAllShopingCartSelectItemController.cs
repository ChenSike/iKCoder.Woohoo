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
        public ActionResult Action(int proid,string guid)
        {
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("ShopingCartUpdate");
            try
            {
                string query = "select * from conf_all_shopcart where proid=" + proid + " and guid='" + guid + "' and selected='1'";
                Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart_selected = dbConnection.Query<Orm.Orm_conf_all_shopcart>(query).FirstOrDefault();
                Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart = new Orm.Orm_conf_all_shopcart();
                orm_Conf_All_Shopcart.guid = guid;
                orm_Conf_All_Shopcart.proid = proid;
                if (orm_Conf_All_Shopcart_selected!=null)
                    orm_Conf_All_Shopcart.selected = "0";
                else
                    orm_Conf_All_Shopcart.selected = "1";
                orm_Conf_All_Shopcart.udt = DateTime.Now.ToString();
                query = "update conf_all_shopcart set selected=@selected where proid=@proid and guid=@guid";
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