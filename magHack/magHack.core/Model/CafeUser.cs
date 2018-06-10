using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    public class CafeUser : CafeteriaManagerModel, IModel
    {
        public string CafeID { get; set; }
    }
}
