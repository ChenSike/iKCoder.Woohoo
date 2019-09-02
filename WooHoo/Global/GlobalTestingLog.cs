using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace WooHoo.Global
{
    public class GlobalTestingLog
    {
        public XmlDocument doc = new XmlDocument();
        public XmlNode rootNode;
        public string LogName;
        public GlobalTestingLog(string fromLogName)
        {
            doc = new XmlDocument();
            doc.LoadXml("<root></root>");
            doc.Save("test.xml");
            rootNode = doc.SelectSingleNode("/root");
            this.LogName = fromLogName;
        }

        public void AddRecord(string title , string value)
        {
            string recordValue = rootNode.InnerText + " [ title ] : " + value;
            rootNode.InnerText = recordValue;
            doc.Save("test.xml");
        }

    }
}
