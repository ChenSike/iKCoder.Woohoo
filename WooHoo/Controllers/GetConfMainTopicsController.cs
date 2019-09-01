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
    public class GetConfMainTopicsController : WHControllerBase
    {
        /// <summary>
        /// 获取首页Topic配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Filter_ConnectDB]
        public ActionResult Action()
        {
            List<Orm.Orm_conf_main_topics> orm_Conf_TopicsObj = dbConnection.Query<Orm_conf_main_topics>("select * from conf_main_topics").ToList();
            return Json(orm_Conf_TopicsObj);
        }
    }
}