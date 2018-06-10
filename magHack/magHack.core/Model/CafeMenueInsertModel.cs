using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    [Serializable]
    public class CafeMenueInsertModel : IModel
    {
        public CafeMenueModel CafeMenue{ get; set; }
        public LoginModel LoginDetails { get; set; }
    }
}
