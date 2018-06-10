using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace magHack.core
{
    public class Helper
    {
        public static string GetQueryValue(string queryName)
        {
            string json = File.ReadAllText(@"C:\hackathon\Queries.json");
            JObject jObj = JObject.Parse(json);
            return jObj.GetValue(queryName).ToString();
        }
    }
}
