using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace magHack.core.Model
{
    [Serializable]
    public class SignUpModel
    {
        public UserType Type { get; set; }
        public string UserID { get; set; }
        public string NewPassword{ get; set; }
        public string ConfirmPassword{ get; set; }

        public string Email { get; set; }

        public string PhoneNumber{ get; set; }

        public string VendorID { get; set; }

        public string CafeID { get; set; }

        public bool ValidateUserID()
        {
            return Regex.IsMatch(UserID, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
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
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserType
    {
        CUSTOMER,
        CafeteriaManager,
        CAFE_USER
    }
}