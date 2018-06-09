using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace magHack.Models
{
    [Serializable]
    public class SignUpModel
    {
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string NewPassword{ get; set; }
        public string ConfirmPassword{ get; set; }

        public string Email { get; set; }

        public string PhoneNumber{ get; set; }

        public bool ValidateUserName()
        {
            return Regex.IsMatch(UserName, @"^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase);
        }
        public bool ValidateEmail()
        {
            return Regex.IsMatch(Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public bool ValidatePassword()
        {
            if (ConfirmPassword == NewPassword)
            {
                return Regex.IsMatch(ConfirmPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$");
            }

            return false;
        }
    }//^[a-zA-Z0-9]+$

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserType
    {
        CUSTOMER,
        VENDOR,
        CAFE_USER
    }
}