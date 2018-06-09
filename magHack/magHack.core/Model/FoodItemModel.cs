using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace magHack.core.Model
{
    public class FoodItemModel
    {
        public string FoodItemID { get; set; }
        public string FoodItemName { get; set; }

        public FoodItemType FoodItemType { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum FoodItemType
    {
        Veg,
        NonVeg
    }
}
