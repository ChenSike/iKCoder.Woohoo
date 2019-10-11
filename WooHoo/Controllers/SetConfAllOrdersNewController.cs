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
                    string orderid = Guid.NewGuid().ToString();
                    foreach (JC_ConfAllOrders_Item shopingcartitem in jC_ConfAllOrders.items)
                    {
                        totalprice = totalprice + shopingcartitem.price;
                        if (shopingcartitem.shopcartid > 0)
                        {
                            query = "delete from conf_all_shopcart where id=" + shopingcartitem.shopcartid;
                            if (dbConnection.Execute(query) > 0)
                            {
                                query = "insert into conf_all_orders_proitems(proid,orderid,count,modell1,modell2) values(" + shopingcartitem.proid + ",'" + orderid + "'," + shopingcartitem.count + ",'" + shopingcartitem.modell1 + "','" + shopingcartitem.modell2 + "')";
                                dbConnection.Execute(query);
                            }
                        }
                    }                                           
                    string cdt = DateTime.Now.ToString("yyyyMMdd");
                    string returned = "0";
                    query = "insert into conf_all_orders(orderid,payed,cdt,returned,addressid,guid,totalprice,shiped,status) values('" + orderid + "','0','" + cdt + "','0','" + jC_ConfAllOrders.addressid + "','" + jC_ConfAllOrders.guid + "'," + totalprice + ",'0','1')";
                    dbConnection.Execute(query);
                    Orm.Orm_conf_all_address orm_Conf_All_Address = new Orm.Orm_conf_all_address();
                    query = "select * from conf_all_address where id=" + jC_ConfAllOrders.addressid;
                    orm_Conf_All_Address = dbConnection.Query<Orm.Orm_conf_all_address>(query).FirstOrDefault();
                    if (orm_Conf_All_Address != null)
                    {
                        Orm.Orm_conf_all_orders_address orm_conf_all_orders_address = new Orm.Orm_conf_all_orders_address();
                        orm_conf_all_orders_address.guid = orm_Conf_All_Address.guid;
                        orm_conf_all_orders_address.name = orm_Conf_All_Address.name;
                        orm_conf_all_orders_address.country = orm_Conf_All_Address.country;
                        orm_conf_all_orders_address.city = orm_Conf_All_Address.city;
                        orm_conf_all_orders_address.state = orm_Conf_All_Address.state;
                        orm_conf_all_orders_address.district = orm_Conf_All_Address.district;
                        orm_conf_all_orders_address.address = orm_Conf_All_Address.address;
                        orm_conf_all_orders_address.phone = orm_Conf_All_Address.phone;
                        orm_conf_all_orders_address.orderid = orderid;
                        query = "insert into conf_all_orders_address(orderid,guid,name,country,state,city,district,address,phone) values(@orderid,@guid,@name,@country,@state,@city,@district,@address,@phone)";
                        dbConnection.Execute(query, orm_conf_all_orders_address);
                    }
                    Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                    conf_ResponseMessageObj.code = "200";
                    conf_ResponseMessageObj.status = "OK";
                    conf_ResponseMessageObj.message = orderid;
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