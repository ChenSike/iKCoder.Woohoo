using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;
using Newtonsoft.Json;
using WooHoo.Configs;

namespace WooHoo.Controllers
{

    public class JC_ShopingCartItemIDList
    {
        public string guid
        {
            set;
            get;
        }

        public List<int> idlist
        {
            set;
            get;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SetConfAllShopingCartRemoveItemListController : WHControllerBase
    {
        [HttpPost]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string postdata)
        {
            JC_ShopingCartItemIDList jC_ShopingCartItemIDList  = (JC_ShopingCartItemIDList)JsonConvert.DeserializeObject(postdata);
            foreach (int activeid in jC_ShopingCartItemIDList.idlist)
            {
                Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart = new Orm.Orm_conf_all_shopcart();
                orm_Conf_All_Shopcart.id = activeid;
                orm_Conf_All_Shopcart.guid = jC_ShopingCartItemIDList.guid;
                string query = "delete from conf_all_shopcart where id=@id and guid=@guid";
                dbConnection.Execute(query, orm_Conf_All_Shopcart);               
            }
            Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
            conf_ResponseMessageObj.code = "200";
            conf_ResponseMessageObj.status = "ok";
            conf_ResponseMessageObj.message = "Executed";
            HttpContext.Response.StatusCode = 200;
            return Json(conf_ResponseMessageObj);
        }
    }
}