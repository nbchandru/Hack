using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace magHack.core.Model
{
    [Serializable]
    public class FoodItemModel : IModel
    {
        public string FoodItemID { get; set; }
        public string FoodItemName { get; set; }

        public FoodItemType FoodItemType { get; set; }

        public string FoodItemDescription { get; set; }

        public string FoodItemCost { get; set; }

        public string FoodItemStatus { get; set; }

        public string CafeMenuID { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum FoodItemType
    {
        Veg,
        NonVeg
    }
}
