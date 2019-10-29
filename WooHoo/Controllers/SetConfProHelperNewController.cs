using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WooHoo.Base;
using WooHoo.Filter;
using System.Data;
using System.Text;
using Dapper;


namespace WooHoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetConfProHelperNewController : Base.WHControllerBase
    {
        [HttpGet]
        [Filter_SysAuthor]
        [Filter_ConnectDB]
        public ActionResult Action(int proid,int pro_img_d_c=2,int pro_img_di_c=13)
        {
            StringBuilder rStr = new StringBuilder();
            string query = "";
            try
            {
                string imgpath_1 = "PRO_" + proid + ".fw.jpg";
                query = "insert into conf_all_proitems_imgs(proid,imgpath,width,height,`index`,titleimg) values(" + proid + ",'" + imgpath_1 + "','0','0',1,'1')";
                dbConnection.Execute(query);
                for (int i = 1; i <= pro_img_d_c; i++)
                {
                    string imgpath = "PRO_" + proid + "D" + i + ".fw.jpg";
                    query = "insert into conf_all_proitems_imgs(proid,imgpath,width,height,`index`,titleimg) values(" + proid + ",'" + imgpath + "','0','0'," + (++i) + ",'1')";
                    dbConnection.Execute(query);
                }
            }
            catch
            {
                rStr.Append("<msg>Faild To Insert Imgs</msg>");
                
            }
            rStr.Append("<msg>Insert Imgs For Pro Sucessfully.</msg>");
            try
            {                
                for (int i = 1; i <= pro_img_di_c; i++)
                {
                    string imgpath = "PRO_" + proid + "DI" + i + ".fw.jpg";
                    query = "insert into conf_all_proitems_imginfo(proid,`index`,img) values(" + proid + "," + i + "," + imgpath + ")";
                    dbConnection.Execute(query);
                }
            }
            catch
            {
                rStr.Append("<msg>Faild To Insert Imgs Detail Info</msg>");

            }
            rStr.Append("<msg>Insert Imgs Detail Info For Pro Sucessfully.</msg>");
            return Content(rStr.ToString());
        }
    }
}