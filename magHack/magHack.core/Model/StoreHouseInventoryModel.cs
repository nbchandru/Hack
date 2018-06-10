using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    [Serializable]
    public class StoreHouseInventoryModel : IModel
    {
        public string IngredientID { get; set; }
        public string IngredientName { get; set; }
        public float IngredientQuantity { get; set; }
        public Unit IngredientQuantityUnit { get; set; }
        public string IngredientStatus { get; set; }
        public string CafeteriaVendorID { get; set; }

    }
}
