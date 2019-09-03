using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Xml;

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
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<root></root>");
            XmlNode rootNode = doc.SelectSingleNode("/root");
            doc.Save("LOG.xml");
            string TestVAL = "";
            try
            {                
                WX_API_Get_OpenID = WX_API_Get_OpenID.Replace("{$appid}", AppId);
                WX_API_Get_OpenID = WX_API_Get_OpenID.Replace("{$secret}", Secret);
                WX_API_Get_OpenID = WX_API_Get_OpenID.Replace("{$code}", code);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(WX_API_Get_OpenID);
                TestVAL = "url:|" + WX_API_Get_OpenID;
                TestVAL = TestVAL + " code:| " + code;                    
                request.Method = "GET";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream ioStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
                string html = sr.ReadToEnd();
                TestVAL = TestVAL + "| Return:" + html;
                sr.Close();
                ioStream.Close();
                response.Close();
                JC_WXUserInfo jC_WXUserInfoObj = JsonConvert.DeserializeObject<JC_WXUserInfo>(html);
                TestVAL = TestVAL + " | OpenID：" + jC_WXUserInfoObj.openid;
                rootNode.InnerText = TestVAL;
                doc.Save("LOG.xml");
                return jC_WXUserInfoObj.openid;
            }
            catch(Exception err)
            {
                TestVAL = TestVAL + "Stace:" + err.StackTrace;
                TestVAL = TestVAL + "Msg:" + err.Message;
                rootNode.InnerText = TestVAL;
                doc.Save("LOG.xml");
                return "";
            }

        }
    }
}
