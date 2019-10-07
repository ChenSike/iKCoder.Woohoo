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
    public class GetConfAllOrderDetailController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string orderid)
        {
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("AllOrderDetail");
            try
            {
                string query = "select * from conf_all_orders where orderid='" + orderid + "'";
                Orm.Orm_conf_all_orders orm_Conf_All_Orders = dbConnection.Query<Orm.Orm_conf_all_orders>(query).FirstOrDefault();
                JC_OrderOutput newItem = new JC_OrderOutput();
                newItem.orderid = orm_Conf_All_Orders.orderid;
                newItem.disorderid = orm_Conf_All_Orders.cdt + "_" + orm_Conf_All_Orders.id;
                newItem.totalprice = orm_Conf_All_Orders.totalprice;
                newItem.shiped = orm_Conf_All_Orders.shipped;
                newItem.payed = orm_Conf_All_Orders.payed;
                newItem.cdt = orm_Conf_All_Orders.cdt;
                newItem.items = new List<JC_OrderOutput_ProItem>();
                newItem.status = orm_Conf_All_Orders.status;
                Orm.Orm_conf_all_address orm_Conf_All_Address = new Orm.Orm_conf_all_address();
                query = "select * from conf_all_address where id=" + orm_Conf_All_Orders.addressid;
                orm_Conf_All_Address = dbConnection.Query<Orm.Orm_conf_all_address>(query).FirstOrDefault();
                if (orm_Conf_All_Address != null)
                {
                    newItem.name = orm_Conf_All_Address.name;
                    newItem.address = orm_Conf_All_Address.address;
                    newItem.phone = orm_Conf_All_Address.phone;
                }
                query = "select * from conf_all_orders_proitems where orderid='" + newItem.orderid + "'";
                List<Orm.Orm_conf_all_orders_proitems> orm_Conf_All_Orders_Proitem_lst = dbConnection.Query<Orm.Orm_conf_all_orders_proitems>(query).ToList();
                foreach (Orm.Orm_conf_all_orders_proitems orm_Conf_All_Orders_Proitems_Tmp in orm_Conf_All_Orders_Proitem_lst)
                {
                    JC_OrderOutput_ProItem newProItem = new JC_OrderOutput_ProItem();
                    newProItem.proid = orm_Conf_All_Orders_Proitems_Tmp.proid;
                    query = "select * from conf_all_proitems where id=" + orm_Conf_All_Orders_Proitems_Tmp.proid;
                    Orm.Orm_conf_all_proitems orm_Conf_All_Proitems = dbConnection.Query<Orm.Orm_conf_all_proitems>(query).FirstOrDefault();
                    newProItem.title = orm_Conf_All_Proitems.title;
                    query = "select * from conf_all_proitems_imgs where id=" + orm_Conf_All_Orders_Proitems_Tmp.proid + " and titleimg='1'";
                    Orm.Orm_conf_all_proitems_imgs orm_conf_all_proitems_imgs = dbConnection.Query<Orm.Orm_conf_all_proitems_imgs>(query).FirstOrDefault();
                    newProItem.img = orm_conf_all_proitems_imgs.imgpath;
                    query = "select * from conf_all_proitems_price where proid=" + orm_Conf_All_Orders_Proitems_Tmp.proid + " and modell1='" + orm_Conf_All_Orders_Proitems_Tmp.modell1 + "' and modell2='" + orm_Conf_All_Orders_Proitems_Tmp.modell2 + "'";
                    Orm.Orm_conf_all_proitems_price orm_Conf_All_Proitems_Price = dbConnection.Query<Orm.Orm_conf_all_proitems_price>(query).FirstOrDefault();
                    newProItem.price = orm_Conf_All_Proitems_Price.discount > 0 ? orm_Conf_All_Proitems_Price.basic * (orm_Conf_All_Proitems_Price.discount / 100.0) : orm_Conf_All_Proitems_Price.basic;
                    newProItem.count = orm_Conf_All_Orders_Proitems_Tmp.count;
                    newProItem.modell1 = orm_Conf_All_Orders_Proitems_Tmp.modell1;
                    newProItem.modell2 = orm_Conf_All_Orders_Proitems_Tmp.modell2;
                    newItem.items.Add(newProItem);
                }
                return Json(newItem);
            }
            catch (Exception err)
            {
                globalTestingLog.AddRecord("stace", err.StackTrace);
                globalTestingLog.AddRecord("msg", err.Message);
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "500";
                conf_ResponseMessageObj.status = "error";
                conf_ResponseMessageObj.message = "Faild to execute";
                HttpContext.Response.StatusCode = 500;
                return Json(conf_ResponseMessageObj);
            }

        }
    }
}