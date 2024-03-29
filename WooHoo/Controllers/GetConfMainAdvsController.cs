﻿using System;
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
    /// <summary>
    /// 获取首页TOP ADVS配置
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GetConfMainAdvsController : Base.WHControllerBase
    {
        [Filter.Filter_ConnectDB]
        [HttpGet]
        public ActionResult action()
        {
            List<Orm_conf_main_advs> lstResult = dbConnection.Query<Orm_conf_main_advs>("select * from conf_main_advs").ToList();
            return Json(lstResult);
        }
    }
}