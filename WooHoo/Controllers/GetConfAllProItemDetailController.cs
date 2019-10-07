﻿using System;
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

    public class JC_ProductItemDetail_Store
    {
        public int sales
        {
            set;
            get;
        }

        public int stock
        {
            set;
            get;
        }

        public string outofstock
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
    }

    public class JC_ProductItemDetail
    {

        public string title
        {
            set;
            get;
        }

        public int proid
        {
            set;
            get;
        }

        public string des
        {
            set;
            get;
        }        

        public string type_topics
        {
            set;
            get;
        }

        public string materials
        {
            set;
            get;
        }

        public string area
        {
            set;
            get;
        }

        public List<string> imglist
        {
            set;
            get;
        }
                     

        public Dictionary<string,string> infoimgs
        {
            set;
            get;
        }

        public List<JC_ProductItemDetail_Store> jC_ProductItemDetail_Store
        {
            set;
            get;
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class GetConfAllProItemDetailController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(int id,string modell1,string modell2)
        {
            JC_ProductItemDetail newitem = new JC_ProductItemDetail();
            string query = "select * from conf_all_proitems where id=" + id.ToString();
            Orm.Orm_conf_all_proitems Orm_conf_all_proitem = dbConnection.Query<Orm.Orm_conf_all_proitems>(query).FirstOrDefault();
            if (Orm_conf_all_proitem == null)
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "501";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "Product is not existed.";
                HttpContext.Response.StatusCode = 200;
                return Json(conf_ResponseMessageObj);
            }
            else
            {                
                newitem.proid = Orm_conf_all_proitem.id;
                newitem.title = Orm_conf_all_proitem.title;
                newitem.des = Orm_conf_all_proitem.des;
                newitem.materials = Orm_conf_all_proitem.materials;
                newitem.area = Orm_conf_all_proitem.area;              
                newitem.imglist = new List<string>();
                query = "select * from conf_all_proitems_imgs where proid=" + newitem.proid;
                List<Orm.Orm_conf_all_proitems_imgs> orm_Conf_All_Proitems_Imgs = dbConnection.Query<Orm.Orm_conf_all_proitems_imgs>(query).ToList();
                if (orm_Conf_All_Proitems_Imgs != null)
                {                                          
                   foreach(Orm.Orm_conf_all_proitems_imgs orm_Conf_All_Proitems_Imgs_tmp in orm_Conf_All_Proitems_Imgs)
                    {
                        if(orm_Conf_All_Proitems_Imgs_tmp.titleimg=="0")
                            newitem.imglist.Add(orm_Conf_All_Proitems_Imgs_tmp.imgpath);
                    }
                }
                query = "select * from conf_all_proitems_store where proid=" + newitem.proid;
                //Orm.Orm_conf_all_proitems_store orm_Conf_All_Proitems_Store = dbConnection.Query<Orm.Orm_conf_all_proitems_store>(query).First();
                List<Orm.Orm_conf_all_proitems_store> lstOrmConfallproitemsstore = dbConnection.Query<Orm.Orm_conf_all_proitems_store>(query).ToList();
                if(lstOrmConfallproitemsstore!=null)
                {
                    newitem.jC_ProductItemDetail_Store = new List<JC_ProductItemDetail_Store>();
                    foreach(Orm.Orm_conf_all_proitems_store orm_Conf_All_Proitems_Store in lstOrmConfallproitemsstore)
                    {
                        JC_ProductItemDetail_Store jC_ProductItemDetail_Store_Item = new JC_ProductItemDetail_Store();
                        jC_ProductItemDetail_Store_Item.stock = orm_Conf_All_Proitems_Store.stock;
                        if (orm_Conf_All_Proitems_Store.stock > 0)
                            jC_ProductItemDetail_Store_Item.outofstock = "0";
                        else
                            jC_ProductItemDetail_Store_Item.outofstock = "1";
                        jC_ProductItemDetail_Store_Item.modell1 = orm_Conf_All_Proitems_Store.modell1;
                        jC_ProductItemDetail_Store_Item.modell2 = orm_Conf_All_Proitems_Store.modell2;
                        jC_ProductItemDetail_Store_Item.sales = orm_Conf_All_Proitems_Store.sale;
                        query = "select * from conf_all_proitems_price where proid=" + Orm_conf_all_proitem.id + " and modell1='" + modell1 + "' and modell2='" + modell2 + "'";
                        Orm.Orm_conf_all_proitems_price orm_Conf_All_Proitems_Price = dbConnection.Query<Orm.Orm_conf_all_proitems_price>(query).First();
                        if (orm_Conf_All_Proitems_Price != null)
                        {
                            jC_ProductItemDetail_Store_Item.basicprice = orm_Conf_All_Proitems_Price.basic;
                            jC_ProductItemDetail_Store_Item.price = orm_Conf_All_Proitems_Price.discount > 0 ? orm_Conf_All_Proitems_Price.basic * (orm_Conf_All_Proitems_Price.discount / 100.0) : orm_Conf_All_Proitems_Price.basic;
                        }
                        else
                        {
                            jC_ProductItemDetail_Store_Item.basicprice = 0;
                            jC_ProductItemDetail_Store_Item.price = 0;
                        }
                        newitem.jC_ProductItemDetail_Store.Add(jC_ProductItemDetail_Store_Item);
                    }
                }                               
                newitem.infoimgs = new Dictionary<string, string>();
                query = "select * from conf_all_proitems_imginfo where proid=" + newitem.proid;
                List<Orm.Orm_conf_all_proitems_imginfo> orm_Conf_All_Proitems_Imginfos = dbConnection.Query<Orm.Orm_conf_all_proitems_imginfo>(query).ToList();
                foreach (Orm.Orm_conf_all_proitems_imginfo orm_Conf_All_Proitems_Imginfo_tmp in orm_Conf_All_Proitems_Imginfos)
                {
                    if(!newitem.infoimgs.ContainsKey(orm_Conf_All_Proitems_Imginfo_tmp.index.ToString()))
                    {
                        newitem.infoimgs.Add(orm_Conf_All_Proitems_Imginfo_tmp.index.ToString(), orm_Conf_All_Proitems_Imginfo_tmp.img);
                    }
                }
                return Json(newitem);
            }

        }
    }
}