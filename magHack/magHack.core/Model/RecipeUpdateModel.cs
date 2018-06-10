using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    [Serializable]
    public class RecipeUpdateModel :IModel
    {
        public LoginModel LoginDetails { get; set; }
        public FoodItemRecipeModel Recipe { get; set; }

        public FoodItemModel Food { get; set; }

        public string CafeID { get; set; }
    }
}
