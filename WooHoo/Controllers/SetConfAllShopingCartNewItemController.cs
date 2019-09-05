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
    public class SetConfAllShopingCartNewItemController : WHControllerBase
    {
        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(int proid,string guid)
        {
            string query = "select * from conf_all_shopcart where proid=" + proid + " and guid='" + guid + "'";
            Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart_Selected = dbConnection.Query<Orm.Orm_conf_all_shopcart>(query).First();
            if (orm_Conf_All_Shopcart_Selected != null)
            {
                Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart = new Orm.Orm_conf_all_shopcart();
                orm_Conf_All_Shopcart.proid = proid;
                orm_Conf_All_Shopcart.guid = guid;
                orm_Conf_All_Shopcart.count = orm_Conf_All_Shopcart_Selected.count + 1;
                orm_Conf_All_Shopcart.udt = DateTime.Now.ToString();
                query = "update conf_all_shopcart set count=@count,udt=@udt where proid=@proid and guid=@guid";
                dbConnection.Execute(query, orm_Conf_All_Shopcart);
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "ok";
                conf_ResponseMessageObj.message = "Executed";
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
            else
            {
                Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart = new Orm.Orm_conf_all_shopcart();
                orm_Conf_All_Shopcart.proid = proid;
                orm_Conf_All_Shopcart.guid = guid;
                orm_Conf_All_Shopcart.count = 1;
                orm_Conf_All_Shopcart.cdt = DateTime.Now.ToString();
                orm_Conf_All_Shopcart.udt = DateTime.Now.ToString();
                query = "insert into conf_all_shopcart(guid,proid,count,cdt,udt) values(@guid,@proid,@count,@cdt,@udt)";
                dbConnection.Execute(query, orm_Conf_All_Shopcart);
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "ok";
                conf_ResponseMessageObj.message = "Executed";
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}