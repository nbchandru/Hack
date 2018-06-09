using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    public class CustomerOrderModel : CustomerModel, IModel
    {
        public string CustomerOrderID { get; set; }
        public string CustomerOrderStatus { get; set; }
        public string CustomerOrderTotal { get; set; }
        public DateTime CustomerOrderTimestamp { get; set; }
    }
}
