using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    public class CafeMenueModel : IModel
    {
        public string CafeMenuID { get; set; }
        public string CafeMenueName { get; set; }

        public cafeMenuStatus Status { get; set; }
    }

    public enum cafeMenuStatus
    {
        Active,
        InActive
    }
}
