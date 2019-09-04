using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using WooHoo.Base;

namespace WooHoo.Controllers
{

    public class JC_TopicProducts
    {

        public string title
        {
            set;
            get;
        }

        public string img
        {
            set;
            get;
        }

        public int proid
        {
            set;
            get;
        }

        public double price
        {
            set;
            get;
        }

        public double basicprice
        {
            set;
            get;
        }

        public string type_topics
        {
            set;
            get;
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class GetConfMainSelectedProductsController : WHControllerBase
    {
        /// <summary>
        /// 获取首页的产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(int topicid)
        {
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("MainSelectedProducts");
            try
            {
                List<JC_TopicProducts> result = new List<JC_TopicProducts>();
                string query = "select * from conf_all_proitems where id in ( select proid from conf_main_topics_products where topicsid = " + topicid + ") ";
                List<Orm.Orm_conf_all_proitems> lst_Orm_conf_all_proitems = dbConnection.Query<Orm.Orm_conf_all_proitems>(query).ToList();
                foreach (Orm.Orm_conf_all_proitems orm_Conf_All_Proitems_tmp in lst_Orm_conf_all_proitems)
                {
                    JC_TopicProducts newitem = new JC_TopicProducts();
                    newitem.proid = orm_Conf_All_Proitems_tmp.id;
                    newitem.title = orm_Conf_All_Proitems_tmp.title;
                    query = "select * from conf_all_proitems_price where proid=" + orm_Conf_All_Proitems_tmp.id;
                    Orm.Orm_conf_all_proitems_price orm_Conf_All_Proitems_Price = dbConnection.Query<Orm.Orm_conf_all_proitems_price>(query).SingleOrDefault();
                    if (orm_Conf_All_Proitems_Price != null)
                    {
                        newitem.basicprice = orm_Conf_All_Proitems_Price.basic;
                        newitem.price = orm_Conf_All_Proitems_Price.discount > 0 ? orm_Conf_All_Proitems_Price.basic * (orm_Conf_All_Proitems_Price.discount / 100.0) : orm_Conf_All_Proitems_Price.basic;
                    }
                    else
                    {
                        newitem.basicprice = 0;
                        newitem.price = 0;
                    }
                    query = "select * from conf_all_proitems_imgs where proid=" + newitem.proid;
                    Orm.Orm_conf_all_proitems_imgs orm_Conf_All_Proitems_Imgs = dbConnection.Query<Orm.Orm_conf_all_proitems_imgs>(query).First();
                    if (orm_Conf_All_Proitems_Imgs != null)
                    {
                        newitem.img = orm_Conf_All_Proitems_Imgs.imgpath;
                    }
                    newitem.type_topics = topicid.ToString();
                    result.Add(newitem);
                }
                return Json(result);
            }
            catch(Exception err)
            {
                globalTestingLog.AddRecord("stack:", err.StackTrace);
                globalTestingLog.AddRecord("msg:", err.Message);
                return Content("");
            }
        }
    }
}