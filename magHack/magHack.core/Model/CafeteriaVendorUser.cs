using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    public class CafeteriaVendorUser : PersonModel, IModel
    {
        public string CafeteriaVendorUserPassword { get; set; }
        public string CafeteriaVendorUserStatus { get; set; }
        public string CafeteriaVendorID { get; set; }
    }
}
