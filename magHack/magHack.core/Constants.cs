using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
namespace magHack.core
{
    public class Constants
    {
        public static string ODBCString
        {
            get
            {
                FileStream fs = new FileStream(@"C:\hackathon\TableInfo.xml", FileMode.Open, FileAccess.Read);
                var xmldoc = new XmlDocument();
                xmldoc.Load(fs);
                XmlNode xmlnode = xmldoc.SelectSingleNode("//connectionstring");
                var odbcConString = xmlnode["odbc"].InnerText;
                fs.Close();
                return odbcConString;
            }
        }

        public static string OLEDBString
        {
            get
            {
                FileStream fs = new FileStream(@"C:\hackathon\TableInfo.xml", FileMode.Open, FileAccess.Read);
                var xmldoc = new XmlDocument();
                xmldoc.Load(fs);
                XmlNode xmlnode = xmldoc.SelectSingleNode("//connectionstring");
                var odbcConString = xmlnode["oledb"].InnerText;
                fs.Close();
                return odbcConString;
            }
        }
    }
}
