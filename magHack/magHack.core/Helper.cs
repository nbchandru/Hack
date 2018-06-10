using System;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using magHack.core.Model;
using magHack.core.Persister;
using magHack.core.DataBaseAccess;

namespace magHack.core
{
    public class Helper
    {
        public static string GetQueryValue(string queryName)
        {
            string json = File.ReadAllText(@"C:\hackathon\Queries.json");
            JObject jObj = JObject.Parse(json);
            return jObj.GetValue(queryName).ToString();
        }
        public static bool Login(LoginModel loginModel, string connectionString)
        {
            using (var dbConnection = new ODBConnection(connectionString))
            using (var persister = new ODBCPersister(dbConnection))
            {
                var count = int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkCustomerPassword"), loginModel.UserName, loginModel.Password, "active")).ToString()) +
                    int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkCafeteriaManagerPassword"), loginModel.UserName, loginModel.Password, "active")).ToString()) +
                    int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkCafeUserPassword"), loginModel.UserName, loginModel.Password, "active")).ToString());
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }
        public static bool SignUpStatusObject(ODBCPersister persister, SignUpModel signUpModel, string updatePersonCommand, string tableName, string updatetabelCommand)
        {
            // accept the signUp
            persister.ExecuteNonQueryCmd("Person", updatePersonCommand);
            persister.ExecuteNonQueryCmd(tableName, updatetabelCommand);
            return true;
        }
    }
}
