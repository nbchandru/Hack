using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using magHack.Models;
using magHack.core.DataBaseAccess;
using magHack.core.Persister;
using magHack.core;
using magHack.core.DataSet;


namespace magHack.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        public ActionResult SignIn(LoginModel loginModel)
        {
            var connectionString = Constants.ODBCString;
            var loginSuccessful = true;
            using (var dbConnection = new ODBConnection(connectionString))
            using (var persister = new ODBCPersister(dbConnection))
            {
                var count = int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkCustomerPassword"), loginModel.UserName, loginModel.Password, "active")).ToString()) +
                    int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkCafeteriaManagerPassword"), loginModel.UserName, loginModel.Password, "active")).ToString()) +
                    int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkCafePassword"), loginModel.UserName, loginModel.Password, "active")).ToString());
                if (count > 0)
                {
                    Session["loggedIn"] = true;
                    loginSuccessful = true;
                }
            }

            var jsonResult = new JsonResult();
            jsonResult.Data = new { loginSuccessful };

            return jsonResult;
        }

        private List<string> CheckSignUpType(SignUpModel signUpModel, string querykey, ODBCPersister persister)
        {
            List<string> validationLogs = new List<string>();
            // if email already exists
            var checkEmailQuery = int.Parse(persister.ExecuteScalar(String.Format(Helper.GetQueryValue(querykey), signUpModel.UserID)).ToString());
            if (checkEmailQuery > 0)
            {
                validationLogs.Add("UserName with the current email aready exists");
            }
            if (!signUpModel.ValidateUserID())
            {
                validationLogs.Add("Illeagal userName.");
            }
            if (!signUpModel.ValidatePassword())
            {
                validationLogs.Add("Password requirement not satisfied.");
            }
            return validationLogs;
        }

        private JsonResult SignUpStatusObject(ODBCPersister persister, SignUpModel signUpModel, string updatePersonCommand, string tableName, string updatetabelCommand)
        {
            // accept the signUp
            persister.ExecuteNonQueryCmd("Person", updatePersonCommand);
            persister.ExecuteNonQueryCmd(tableName, updatetabelCommand);
            var jsonResult = new JsonResult();
            var signUpSuccessfull = true;
            jsonResult.Data = new { signUpSuccessfull };
            return jsonResult;
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel signUpModel)
        {
            var connectionString = Constants.ODBCString;
            var jsonResult = new JsonResult();
            var signUpSuccessfull = true;
            List<string> validationLogs = new List<string>();
            using (var dbConnection = new ODBConnection(connectionString))
            using (var persister = new ODBCPersister(dbConnection))
            {
                if (CheckSignUpType(signUpModel, "checkPersonID", persister).Count > 0)
                {
                    signUpSuccessfull = false;
                    jsonResult.Data = new { signUpSuccessfull, validationLogs };
                }
                else
                {
                    var updatePersonCommand = String.Format(Helper.GetQueryValue("insertPersonShort"), signUpModel.UserID, signUpModel.UserID);
                    switch (signUpModel.Type)
                    {
                        case UserType.CAFE_USER:
                            validationLogs = CheckSignUpType(signUpModel, "checkCafeUser", persister);
                            if (validationLogs.Count == 0)
                            {
                                var updatetabelCommand = String.Format(Helper.GetQueryValue("insertCafeUser"), signUpModel.UserID, signUpModel.NewPassword, "active", signUpModel.CafeID);
                                jsonResult = SignUpStatusObject(persister, signUpModel, updatePersonCommand, "CafeUser", updatetabelCommand);
                            }
                            else
                            {
                                signUpSuccessfull = false;
                                jsonResult.Data = new { signUpSuccessfull, validationLogs };
                            }
                            break;

                        case UserType.CUSTOMER:
                            validationLogs = CheckSignUpType(signUpModel, "checkCustomer", persister);
                            if (validationLogs.Count == 0)
                            {
                                var updatetabelCommand = String.Format(Helper.GetQueryValue("insertCustomerShort"), signUpModel.UserID, signUpModel.NewPassword);
                                jsonResult = SignUpStatusObject(persister, signUpModel, updatePersonCommand, "Customer", updatetabelCommand);
                            }
                            else
                            {
                                signUpSuccessfull = false;
                                jsonResult.Data = new { signUpSuccessfull, validationLogs };
                            }
                            break;

                        case UserType.CafeteriaManager:
                            validationLogs = CheckSignUpType(signUpModel, "checkCafeteriaManager", persister);
                            if (validationLogs.Count == 0)
                            {
                                var updatetabelCommand = String.Format(Helper.GetQueryValue("insertCafeteriaManager"), signUpModel.UserID, signUpModel.NewPassword,"active", signUpModel.VendorID);
                                jsonResult = SignUpStatusObject(persister, signUpModel, "CafeUser", "CafeteriaManager", updatetabelCommand);
                            }
                            else
                            {
                                signUpSuccessfull = false;
                                jsonResult.Data = new { signUpSuccessfull, validationLogs };
                            }
                            break;
                    }
                }
            }

            return jsonResult;
        }
    }
}
