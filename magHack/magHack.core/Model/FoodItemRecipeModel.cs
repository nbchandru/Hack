using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace magHack.core.Model
{
    public class FoodItemRecipeModel
    {
        public string FoodItemID { get; set; }
        public string FoodItemIngredientQuantity { get; set; }
        public Unit FoodItemIngredientQuantityUnit { get; set; }

        public string StoreHouseIngredientID { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum Unit
    {
        grams,
        ml
    }
}
