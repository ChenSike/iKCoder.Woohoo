using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using WooHoo.Configs;
using WooHoo.Filter;
using Dapper;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfAllProItemImgsLstController : WHControllerBase
    {
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action(int proid,string titleimg = "0")
        {
            Orm.Orm_conf_all_proitems_imgs orm_Conf_All_Proitems_Imgs = new Orm.Orm_conf_all_proitems_imgs();
            orm_Conf_All_Proitems_Imgs.proid = proid;
            if (titleimg == "1")
                orm_Conf_All_Proitems_Imgs.titleimg = "1";
            else
                orm_Conf_All_Proitems_Imgs.titleimg = "0";
            string query = "select * from conf_all_proitems_imgs where proid=@proid and titleimg=@titleimg";
            List<Orm.Orm_conf_all_proitems_imgs> orm_Conf_All_Proitems_Imgs_Lst = dbConnection.Query<Orm.Orm_conf_all_proitems_imgs>(query, orm_Conf_All_Proitems_Imgs).ToList();
            if (orm_Conf_All_Proitems_Imgs_Lst.Count > 0)
                return Json(orm_Conf_All_Proitems_Imgs_Lst);
            else
            {
                Conf_ResponseMessage conf_ResponseMessageObj = new Conf_ResponseMessage();
                conf_ResponseMessageObj.code = "411";
                conf_ResponseMessageObj.status = "Error";
                conf_ResponseMessageObj.message = "No Data";
                HttpContext.Response.StatusCode = 411;
                return Json(conf_ResponseMessageObj);
            }
        }
    }
}