using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using Dapper;

namespace WooHoo.Controllers
{

    public class JC_ShopCartItem
    {
        public int proid
        {
            set;
            get;
        }

        public int count
        {
            set;
            get;
        }

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

        public double price
        {
            set;
            get;
        }

        public int discount
        {
            set;
            get;
        }

        public double basicprice
        {
            set;
            get;
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class GetConfAllShopingCartController : WHControllerBase
    {
        /// <summary>
        /// 获取购物车信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(string guid)
        {
            Global.GlobalTestingLog globalTestingLog = new Global.GlobalTestingLog("GetShopingCart");
            try
            {
                Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart = new Orm.Orm_conf_all_shopcart();
                orm_Conf_All_Shopcart.guid = guid;
                string query = "select * from conf_all_shopcart where guid=@guid";
                List<Orm.Orm_conf_all_shopcart> lst_orm_Conf_All_Shopcarts = dbConnection.Query<Orm.Orm_conf_all_shopcart>(query, orm_Conf_All_Shopcart).ToList();
                List<JC_ShopCartItem> lst_result = new List<JC_ShopCartItem>();
                foreach (Orm.Orm_conf_all_shopcart orm_Conf_All_Shopcart_tmp_obj in lst_orm_Conf_All_Shopcarts)
                {
                    JC_ShopCartItem tmpItemObj = new JC_ShopCartItem();
                    tmpItemObj.proid = orm_Conf_All_Shopcart_tmp_obj.proid;
                    Orm.Orm_conf_all_proitems orm_Conf_All_Proitems = new Orm.Orm_conf_all_proitems();
                    orm_Conf_All_Proitems.id = orm_Conf_All_Shopcart_tmp_obj.proid;
                    string tmpQuery = "select * from conf_all_proitems where id = @id";
                    Orm.Orm_conf_all_proitems orm_Conf_All_Proitems_Selected = dbConnection.Query<Orm.Orm_conf_all_proitems>(tmpQuery, orm_Conf_All_Proitems).First();
                    tmpItemObj.title = orm_Conf_All_Proitems_Selected.title;
                    Orm.Orm_conf_all_proitems_imgs orm_Conf_All_Proitems_Imgs = new Orm.Orm_conf_all_proitems_imgs();
                    orm_Conf_All_Proitems_Imgs.proid = orm_Conf_All_Shopcart_tmp_obj.proid;
                    tmpQuery = "select * from conf_all_proitems_imgs where proid=@proid and titleimg='1'";
                    Orm.Orm_conf_all_proitems_imgs orm_Conf_All_Proitems_Imgs_Selected = dbConnection.Query<Orm.Orm_conf_all_proitems_imgs>(tmpQuery, orm_Conf_All_Proitems_Imgs).First();
                    tmpItemObj.img = orm_Conf_All_Proitems_Imgs_Selected.imgpath;
                    Orm.Orm_conf_all_proitems_price orm_Conf_All_Proitems_Price = new Orm.Orm_conf_all_proitems_price();
                    orm_Conf_All_Proitems_Price.proid = orm_Conf_All_Shopcart_tmp_obj.proid;
                    tmpQuery = "select * from conf_all_proitems_price where proid=@proid";
                    Orm.Orm_conf_all_proitems_price orm_Conf_All_Proitems_Price_Selected = dbConnection.Query<Orm.Orm_conf_all_proitems_price>(tmpQuery, orm_Conf_All_Proitems_Price).SingleOrDefault();
                    tmpItemObj.basicprice = orm_Conf_All_Proitems_Price_Selected.basic;
                    tmpItemObj.discount = orm_Conf_All_Proitems_Price_Selected.discount;
                    tmpItemObj.price = tmpItemObj.discount > 0 ? tmpItemObj.basicprice * (tmpItemObj.discount / 100) : tmpItemObj.basicprice;
                    lst_result.Add(tmpItemObj);
                }
                return Json(lst_result);
            }
            catch(Exception err)
            {
                globalTestingLog.AddRecord("Stace:", err.StackTrace);
                globalTestingLog.AddRecord("Msg:", err.Message);
                return Content(" ");
            }
        }
    }
}