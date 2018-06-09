using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    public class CustomerOrderFoodListModel : CustomerOrderModel, IModel
    {
        public string FoodItemID { get; set; }
        public long count { get; set; }
        public FoodItemPreparationStatus Status { get; set; }
        public string CafeMenuID { get; set; }
    }

    public enum FoodItemPreparationStatus
    {
        Placed,
        Processiong,
        OutForDelivery
    }
}
