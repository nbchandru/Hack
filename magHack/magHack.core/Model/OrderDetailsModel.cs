using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    [Serializable]
    public class OrderDetailsModel
    {
        public LoginModel LoginDetails { get; set; }

        public List<string> OrderList { get; set; }

        public string CafeID { get; set; }

        public string CafeMenueID { get; set; }
    }
}
