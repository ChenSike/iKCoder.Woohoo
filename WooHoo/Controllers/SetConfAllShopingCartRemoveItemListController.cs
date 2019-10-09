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

    public class JC_ShopingCartItemIDList_Item
    {
        public int id
        {
            set;
            get;
        }

        public string modell1
        {
            set;
            get;
        }

        public string modell2
        {
            set;
            get;
        }

    }

    public class JC_ShopingCartItemIDList
    {
        public string guid
        {
            set;
            get;
        }

        public List<JC_ShopingCartItemIDList_Item> idlist
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
            foreach (JC_ShopingCartItemIDList_Item activeid in jC_ShopingCartItemIDList.idlist)
            {
                Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart = new Orm.Orm_conf_all_shopcart();
                orm_Conf_All_Shopcart.id = activeid.id;
                orm_Conf_All_Shopcart.guid = jC_ShopingCartItemIDList.guid;
                orm_Conf_All_Shopcart.modell1 = activeid.modell1;
                orm_Conf_All_Shopcart.modell2 = activeid.modell2;
                string query = "delete from conf_all_shopcart where id=@id and guid=@guid and modell1=@modell1 and modell2=@modell2";
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