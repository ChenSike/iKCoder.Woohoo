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

    [Route("api/[controller]")]
    [ApiController]
    public class GetConfMainLstTypesController : WHControllerBase
    {
        /// <summary>
        /// 获取分类列表，范例为：大型绿植、中型绿植、小型绿植
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Filter.Filter_ConnectDB]
        public ActionResult Action()
        {
            string query = "select * from conf_main_lsttypes";
            List<Orm.Orm_conf_main_lsttypes> orm_Conf_Main_Lsttypes = dbConnection.Query<Orm.Orm_conf_main_lsttypes>(query).ToList();
            return Json(orm_Conf_Main_Lsttypes);
        }
    }
}