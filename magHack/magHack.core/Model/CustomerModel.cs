using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace magHack.core.Model
{
    public class CustomerModel : PersonModel, IModel
    {
        public string Password { get; set; }
        public CustomerType Type { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum CustomerType
    {
        Premium,
        Gold,
        Silver
    }
}
