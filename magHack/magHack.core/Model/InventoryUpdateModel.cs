using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using magHack.core.Model;

namespace magHack.core.Model
{
    [Serializable]
    public class InventoryUpdateModel
    {
        public LoginModel LoginDetails { get; set; }
        public StoreHouseInventoryModel InventoryModel { get; set;}
    }
}