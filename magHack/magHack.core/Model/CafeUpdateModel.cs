using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    [Serializable]
    public class CafeUpdateModel : IModel
    {
        public CafeModel Cafe { get; set; }

        public string NewName { get; set; }
        public LoginModel LoginDetails { get; set; }

        public string CafeteriaManagerID { get; set; }

        public bool Delete { get; set; }
    }
}
