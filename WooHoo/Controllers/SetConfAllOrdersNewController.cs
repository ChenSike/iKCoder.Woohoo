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

        public List<JC_ConfAllOrders_Item> items
        {
            set;
            get;
        }    

    }

    public class JC_ConfAllOrders_Item
    {
        public int proid
        {
            set;
            get;
        }

        public int shopcartid
        {
            set;
            get;
        }

        public double price
        {
            set;
            get;
        }

        public int count
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
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("SetConfAllOrdersNew");
            try
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
                    JC_ConfAllOrders jC_ConfAllOrders = (JC_ConfAllOrders)JsonConvert.DeserializeObject(data, typeof(JC_ConfAllOrders));
                    string query = "";
                    double totalprice = 0.0;
                    foreach (JC_ConfAllOrders_Item shopingcartitem in jC_ConfAllOrders.items)
                    {
                        totalprice = totalprice + shopingcartitem.price;
                        if (shopingcartitem.shopcartid > 0)
                        {
                            query = "delete from conf_all_shopcart where id=" + shopingcartitem.shopcartid;
                            dbConnection.Execute(query);
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
            catch(Exception err)
            {
                globalTestingLog.AddRecord("stace", err.StackTrace);
                globalTestingLog.AddRecord("msg", err.Message);
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "500";
                conf_ResponseMessageObj.status = "error";
                conf_ResponseMessageObj.message = "User existed.";
                HttpContext.Response.StatusCode = 500;
                return Json(conf_ResponseMessageObj);
            }
        }        
    }
}