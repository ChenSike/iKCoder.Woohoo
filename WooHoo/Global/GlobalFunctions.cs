using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace WooHoo.Global
{

    public class JC_WXUserInfo
    {
        public string session_key
        {
            set;
            get;
        }

        public string openid
        {
            set;
            get;
        }

    }

    public class GlobalFunctions
    {

        public static string AppId = "wx16f305dddbdab429";
        public static string Secret = "57f559c16f1b641062d3620d300b6abc";
        public static string WX_API_Get_OpenID = "https://api.weixin.qq.com/sns/jscode2session?appid={$appid}&secret={$secret}&js_code={$code}&grant_type=authorization_code";

        public static string GetOpenIDFromWX(string code)
        {
            try
            {
                WX_API_Get_OpenID = WX_API_Get_OpenID.Replace("{$appid}", AppId);
                WX_API_Get_OpenID = WX_API_Get_OpenID.Replace("{$secret}", Secret);
                WX_API_Get_OpenID = WX_API_Get_OpenID.Replace("{$code}", code);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(WX_API_Get_OpenID);
                request.Method = "GET";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream ioStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
                string html = sr.ReadToEnd();
                sr.Close();
                ioStream.Close();
                response.Close();

                JC_WXUserInfo jC_WXUserInfoObj = JsonConvert.DeserializeObject<JC_WXUserInfo>(html);
                return jC_WXUserInfoObj.openid;
            }
            catch(Exception err)
            {
                if (!File.Exists("c:\\WX_LOGIN_LOG.txt"))
                    File.Create("c:\\WX_LOGIN_LOG.txt");
                FileStream fs = new FileStream("c:\\WX_LOGIN_LOG.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Time:" + DateTime.Now);
                sw.WriteLine("stack:" + err.StackTrace);
                sw.WriteLine("Err:"+err.Message);
                sw.WriteLine("-----------------");
                sw.Flush();
                fs.Flush();
                sw.Close();
                fs.Close();
                return "";
            }
        }
    }
}
