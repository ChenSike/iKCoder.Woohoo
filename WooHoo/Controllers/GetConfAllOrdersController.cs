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

    public class JC_OrderOutput
    {
        public string cdt
        {
            set;
            get;
        }

        public string disorderid
        {
            set;
            get;
        }

        public string orderid
        {
            set;
            get;
        }

        public string payed
        {
            set;
            get;
        }

        public string shiped
        {
            set;
            get;
        }

        public double totalprice
        {
            set;
            get;
        }

        public List<JC_OrderOutput_ProItem> items
        {
            set;
            get;
        }

        public string status
        {
            set;
            get;
        }

        public string address
        {
            set;
            get;
        }

        public string name
        {
            set;
            get;
        }

        public string phone
        {
            set;
            get;
        }

    }

    public class JC_OrderOutput_ProItem
    {
        public string img
        {
            set;
            get;
        }

        public string title
        {
            set;
            get;
        }

        public double price
        {
            set;
            get;
        }

        public string proid
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
    public class GetConfAllOrdersController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string guid)
        {
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("GetConfAllOrders");
            try
            {
                string query = "select * from conf_all_orders where guid='" + guid + "'";
                List<JC_OrderOutput> jC_OrderOutputs = new List<JC_OrderOutput>();
                List<Orm.Orm_conf_all_orders> orm_Conf_All_Orders = dbConnection.Query<Orm.Orm_conf_all_orders>(query).ToList();
                foreach (Orm.Orm_conf_all_orders orm_Conf_All_Orders_tmp in orm_Conf_All_Orders)
                {
                    JC_OrderOutput newItem = new JC_OrderOutput();
                    newItem.orderid = orm_Conf_All_Orders_tmp.orderid;
                    newItem.disorderid = orm_Conf_All_Orders_tmp.cdt + "_" + orm_Conf_All_Orders_tmp.id;
                    newItem.totalprice = orm_Conf_All_Orders_tmp.totalprice;
                    newItem.shiped = orm_Conf_All_Orders_tmp.shipped;
                    newItem.payed = orm_Conf_All_Orders_tmp.payed;
                    newItem.items = new List<JC_OrderOutput_ProItem>();
                    query = "select * from conf_all_orders_proitems where orderid='" + newItem.orderid + "'";
                    List<Orm.Orm_conf_all_orders_proitems> orm_Conf_All_Orders_Proitem_lst = dbConnection.Query<Orm.Orm_conf_all_orders_proitems>(query).ToList();
                    foreach (Orm.Orm_conf_all_orders_proitems orm_Conf_All_Orders_Proitems_Tmp in orm_Conf_All_Orders_Proitem_lst)
                    {
                        JC_OrderOutput_ProItem newProItem = new JC_OrderOutput_ProItem();
                        newProItem.proid = orm_Conf_All_Orders_Proitems_Tmp.proid;
                        query = "select * from conf_all_proitems where id=" + orm_Conf_All_Orders_Proitems_Tmp.proid;
                        Orm.Orm_conf_all_proitems orm_Conf_All_Proitems = dbConnection.Query<Orm.Orm_conf_all_proitems>(query).FirstOrDefault();
                        if (orm_Conf_All_Proitems != null)
                        {
                            newProItem.title = orm_Conf_All_Proitems.title;
                        }
                        query = "select * from conf_all_proitems_imgs where id=" + orm_Conf_All_Orders_Proitems_Tmp.proid + " and titleimg='1'";
                        Orm.Orm_conf_all_proitems_imgs orm_conf_all_proitems_imgs = dbConnection.Query<Orm.Orm_conf_all_proitems_imgs>(query).FirstOrDefault();
                        if (orm_conf_all_proitems_imgs != null)
                        {
                            newProItem.img = orm_conf_all_proitems_imgs.imgpath;
                        }
                        query = "select * from conf_all_proitems_price where proid=" + orm_Conf_All_Orders_Proitems_Tmp.proid;
                        Orm.Orm_conf_all_proitems_price orm_Conf_All_Proitems_Price = dbConnection.Query<Orm.Orm_conf_all_proitems_price>(query).FirstOrDefault();
                        if (orm_Conf_All_Proitems_Price != null)
                        {
                            newProItem.price = orm_Conf_All_Proitems_Price.discount > 0 ? orm_Conf_All_Proitems_Price.basic * (orm_Conf_All_Proitems_Price.discount / 100.0) : orm_Conf_All_Proitems_Price.basic;
                        }
                        newProItem.count = orm_Conf_All_Orders_Proitems_Tmp.count;
                        newItem.items.Add(newProItem);

                    }
                    jC_OrderOutputs.Add(newItem);

                }
                return Json(jC_OrderOutputs);
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