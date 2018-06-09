using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.Model
{
    public class PersonModel : IModel
    {
        public string PersonID { get; set; }
        public string PersonName { get; set; }
        public string PersonEmailID { get; set; }
        public string PersonMobContactNumber { get; set; }
    }
}
