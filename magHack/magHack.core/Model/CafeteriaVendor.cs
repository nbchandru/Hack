using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    public class CafeteriaVendor : IModel
    {
        public string CafeteriaVendorID { get; set; }
        public string CafeteriaVendorCompanyName { get; set; }
        public string CafeteriaVendorStatus { get; set; }
    }
}
