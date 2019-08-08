using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using Dapper;
using WooHoo.Filter;
using WooHoo.Orm;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfLstTypesProitemsController : WHControllerBase
    {
        [HttpGet]
        [Filter_ConnectDB]
        public ActionResult Action(int lsttypeid)
        {
            Orm_conf_main_lsttype_products orm_Conf_Main_Lsttype_ProductsObj = new Orm_conf_main_lsttype_products();
            orm_Conf_Main_Lsttype_ProductsObj.lsttypes_id = lsttypeid;
            List<Orm_conf_main_lsttype_products> lstLsttypeProducts = dbConnection.Query<Orm_conf_main_lsttype_products>("select * from conf_main_lsttype_products where lsttypes_id=@lsttypes_id", orm_Conf_Main_Lsttype_ProductsObj).ToList();
            return Json(lstLsttypeProducts);
        }
    }
}
