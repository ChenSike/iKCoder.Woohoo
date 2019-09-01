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
        public ActionResult Action()
        {
            List<JC_TopicProducts> result = new List<JC_TopicProducts>();
            List<int> topics_ids = new List<int>();
            List<int> types_ids = new List<int>();
            string query = "select * from conf_main_topics where selectedmain=1";
            List<Orm.Orm_conf_main_topics> orm_Conf_Main_Topics = dbConnection.Query<Orm.Orm_conf_main_topics>(query).ToList();
            foreach(Orm.Orm_conf_main_topics orm_Conf_Main_Topics_tmp in orm_Conf_Main_Topics)
            {
                query = "select * from conf_main_topics_products where topicsid=" + orm_Conf_Main_Topics_tmp.id;
                
                if (!topics_ids.Contains(orm_Conf_Main_Topics_tmp.id))
                    topics_ids.Add(orm_Conf_Main_Topics_tmp.id);
            }
            query = "select * from conf_main_lsttypes where selectedmain=1";
            List<Orm.Orm_conf_main_lsttypes> orm_Conf_Main_Lsttypes = dbConnection.Query<Orm.Orm_conf_main_lsttypes>(query).ToList();
            foreach(Orm.Orm_conf_main_lsttypes orm_Conf_Main_Lsttypes_tmp in orm_Conf_Main_Lsttypes)
            {
                if (types_ids.Contains(orm_Conf_Main_Lsttypes_tmp.id))
                    types_ids.Add(orm_Conf_Main_Lsttypes_tmp.id);
            }
            foreach(int tmp_proid in topics_ids)
            {
                JC_TopicProducts newItem = new JC_TopicProducts();
                newItem.proid = tmp_proid;
                result.Add(newItem);
            }
            foreach(int tmp_proid in types_ids)
            {
                JC_TopicProducts newItem = new JC_TopicProducts();
                newItem.proid = tmp_proid;
                result.Add(newItem);
            }
            foreach(JC_TopicProducts tmp_jC_TopicProducts in result)
            {
                query = "select * from conf_all_proitems where id=" + tmp_jC_TopicProducts.proid;
                Orm.Orm_conf_all_proitems orm_Conf_All_Proitems = dbConnection.Query<Orm.Orm_conf_all_proitems>(query).SingleOrDefault();
                tmp_jC_TopicProducts.title = orm_Conf_All_Proitems.title;
                query = "select * from conf_all_proitems_price where proid="+ tmp_jC_TopicProducts.proid;
                Orm.Orm_conf_all_proitems_price orm_Conf_All_Proitems_Price = dbConnection.Query<Orm.Orm_conf_all_proitems_price>(query).SingleOrDefault();
                tmp_jC_TopicProducts.basicprice = orm_Conf_All_Proitems_Price.basic;
                tmp_jC_TopicProducts.price = orm_Conf_All_Proitems_Price.discount > 0 ? orm_Conf_All_Proitems_Price.basic * orm_Conf_All_Proitems_Price.discount : orm_Conf_All_Proitems_Price.basic;
                query = "select * from conf_all_proitems_imgs where proid=" + tmp_jC_TopicProducts.proid;
                Orm.Orm_conf_all_proitems_imgs orm_Conf_All_Proitems_Imgs = dbConnection.Query<Orm.Orm_conf_all_proitems_imgs>(query).SingleOrDefault();
                tmp_jC_TopicProducts.img = orm_Conf_All_Proitems_Imgs.imgpath;
            }
            return Json(result);
        }
    }
}