﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace magHack.core.Model
{
    [Serializable]
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginModel() { }
    }
}