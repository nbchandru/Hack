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
    public class FoodItemRecipeModel : IModel
    {
        public string FoodItemID { get; set; }
        public float FoodItemIngredientQuantity { get; set; }
        public Unit FoodItemIngredientQuantityUnit { get; set; }
        public string IngredientID { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum Unit
    {
        grams,
        ml
    }
}
