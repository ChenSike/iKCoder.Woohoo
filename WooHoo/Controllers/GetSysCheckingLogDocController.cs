﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetSysCheckingLogDocController : ControllerBase
    {
        [HttpGet]
        public ActionResult action(string logname)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(logname + ".xml");
            return Content(doc.InnerXml);
        }
    }
}