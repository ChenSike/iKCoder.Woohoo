using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;
using WooHoo.Configs;
using Newtonsoft.Json;

namespace WooHoo.Controllers
{ 

    public class JC_ConfAllOrders
    {
        public string guid
        {
            set;
            get;
        }

        public int addressid
        {
            set;
            get;
        }

        public List<int> shopCartItemidLst
        {
            set;
            get;
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class SetConfAllOrdersNewController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "500";
                conf_ResponseMessageObj.status = "error";
                conf_ResponseMessageObj.message = "faild to execute";
                HttpContext.Response.StatusCode = 500;
                return Json(conf_ResponseMessageObj);
            }
            else
            {
                JC_ConfAllOrders jC_ConfAllOrders = JsonConvert.DeserializeObject(data) as JC_ConfAllOrders;
                string query = "";
                double totalprice = 0.0;
                foreach (int shopingcartitem_id in jC_ConfAllOrders.shopCartItemidLst)
                {
                    query = "select * from conf_all_shopcart where id = " + shopingcartitem_id;
                    Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart_Item = dbConnection.Query<Orm.Orm_conf_all_shopcart>(query).FirstOrDefault();
                    if (orm_Conf_All_Shopcart_Item != null)
                    {
                        query = "select * from conf_all_proitems_store where proid = " + orm_Conf_All_Shopcart_Item.proid;
                        Orm.Orm_conf_all_proitems_store orm_Conf_All_Proitems_Store = dbConnection.Query<Orm.Orm_conf_all_proitems_store>(query).FirstOrDefault();
                        int stock = orm_Conf_All_Proitems_Store.stock;
                        query = "select * from conf_all_shopcart where id=" + shopingcartitem_id;
                        Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart = dbConnection.Query<Orm.Orm_conf_all_shopcart>(query).FirstOrDefault();
                        if (orm_Conf_All_Shopcart.count <= stock)
                        {
                            query = "select * from conf_all_proitems_price where proid=" + orm_Conf_All_Shopcart_Item.proid;
                            Orm.Orm_conf_all_proitems_price orm_Conf_All_Proitems_Price = dbConnection.Query<Orm.Orm_conf_all_proitems_price>(query).FirstOrDefault();
                            double price = orm_Conf_All_Proitems_Price.discount > 0 ? orm_Conf_All_Proitems_Price.basic * (orm_Conf_All_Proitems_Price.discount / 100.0) : orm_Conf_All_Proitems_Price.basic;
                            totalprice = totalprice + price;
                        }
                    }
                }                
                string orderid = Guid.NewGuid().ToString();
                string cdt = DateTime.Now.ToString();
                string returned = "0";
                query = "insert into conf_all_orders(orderid,payed,cdt,returned,addressid,guid,totalprice) values('" + orderid + "','0','" + cdt + "','0','" + jC_ConfAllOrders.addressid + "','" + jC_ConfAllOrders.guid + "'," + totalprice + ")";
                dbConnection.Execute(query);                
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "200";
                conf_ResponseMessageObj.status = "OK";
                conf_ResponseMessageObj.message = "Executed";
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
        }        
    }
}