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
using magHack.core.DataSet;

namespace magHack.core
{
    public class Helper
    {
        public string GetQueryValue(string queryName)
        {
            string json = File.ReadAllText(@"C:\hackathon\Queries.json");
            JObject jObj = JObject.Parse(json);
            return jObj.GetValue(queryName).ToString();
        }
        public bool Login(LoginModel loginModel, string connectionString)
        {
            using (var dbConnection = new ODBConnection(connectionString))
            using (var persister = new ODBCPersister(dbConnection))
            {
                var count = int.Parse(persister.ExecuteScalar(string.Format(GetQueryValue("checkCustomerPassword"), loginModel.UserName, loginModel.Password, "active")).ToString()) +
                    int.Parse(persister.ExecuteScalar(string.Format(GetQueryValue("checkCafeteriaManagerPassword"), loginModel.UserName, loginModel.Password, "active")).ToString()) +
                    int.Parse(persister.ExecuteScalar(string.Format(GetQueryValue("checkCafeUserPassword"), loginModel.UserName, loginModel.Password, "active")).ToString());
                if (count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public List<Dictionary<string, string>> GetODBCData(string connectionString, string queryString)
        {
            var data = new List<Dictionary<string, string>>();
            using (var dbConnection = new ODBConnection(connectionString))
            using (var dataSet = new DBDataSet(dbConnection, queryString))
            {
                var cols = dataSet.ColumnNames;
                
                while (dataSet.MoveNext())
                {
                    var dic = new Dictionary<string, string>();
                    foreach (var col in cols)
                    {
                        dic[col] = dataSet.GetValue(col);
                    }
                    data.Add(dic);
                }
            }

            return data;
        }

        public bool SignUpStatusObject(ODBCPersister persister, SignUpModel signUpModel, string updatePersonCommand, string tableName, string updatetabelCommand)
        {
            // accept the signUp
            persister.ExecuteNonQueryCmd("Person", updatePersonCommand);
            persister.ExecuteNonQueryCmd(tableName, updatetabelCommand);
            return true;
        }
    }
}
