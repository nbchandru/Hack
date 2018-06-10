using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using magHack.core.Model;
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
            var loginSuccessful = Helper.Login(loginModel, connectionString);
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
                                signUpSuccessfull = Helper.SignUpStatusObject(persister, signUpModel, updatePersonCommand, "CafeUser", updatetabelCommand);
                                jsonResult.Data = new { signUpSuccessfull };
                                return jsonResult;
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
                                signUpSuccessfull = Helper.SignUpStatusObject(persister, signUpModel, updatePersonCommand, "Customer", updatetabelCommand);
                                jsonResult.Data = new { signUpSuccessfull };
                                return jsonResult;
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
                                signUpSuccessfull = Helper.SignUpStatusObject(persister, signUpModel, "CafeUser", "CafeteriaManager", updatetabelCommand);
                                jsonResult.Data = new { signUpSuccessfull };
                                return jsonResult;
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

        [HttpPost]
        public ActionResult AddIngredients(InventoryUpdateModel inventoryUpdateModel)
        {
            var jsonResult = new JsonResult();
            var loginSuccessful = false;
            if (Session["loggedIn"] != null || Helper.Login(inventoryUpdateModel.LoginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                if (loginSuccessful)
                {
                    var connectionString = Constants.ODBCString;
                    using (var dbConnection = new ODBConnection(connectionString))
                    using (var persister = new ODBCPersister(dbConnection))
                    {
                        var count = int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkCafeteriaManagerVendorID"), inventoryUpdateModel.InventoryModel.CafeteriaVendorID)).ToString());
                        if (count > 0)
                        {
                            // if it exists
                            var itemExists = int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkStoreHouseInventoryItem"), inventoryUpdateModel.InventoryModel.IngredientID)).ToString()) > 0;
                            if (itemExists)
                            {

                                //update updateStoreHouseInventory
                                persister.ExecuteNonQueryCmd("StoreHouseInventory", string.Format(Helper.GetQueryValue("updateStoreHouseInventory"),
                                    inventoryUpdateModel.InventoryModel.IngredientID,
                                    inventoryUpdateModel.InventoryModel.IngredientName,
                                    inventoryUpdateModel.InventoryModel.IngredientQuantity,
                                    inventoryUpdateModel.InventoryModel.IngredientQuantityUnit,
                                    "instock",
                                    inventoryUpdateModel.InventoryModel.IngredientID));
                            }
                            else
                            {
                                persister.ExecuteNonQueryCmd("StoreHouseInventory", string.Format(Helper.GetQueryValue("insertStoreHouseInventory"),
                                    inventoryUpdateModel.InventoryModel.IngredientID,
                                    inventoryUpdateModel.InventoryModel.IngredientName,
                                    inventoryUpdateModel.InventoryModel.IngredientQuantity,
                                    inventoryUpdateModel.InventoryModel.IngredientQuantityUnit,
                                    "instock",
                                    inventoryUpdateModel.InventoryModel.CafeteriaVendorID));
                            }
                        }
                        else
                        {
                            var msg = "You are not previliged to complete this action";
                            jsonResult.Data = new { msg };

                        }
                    }
                }
                else
                {
                    jsonResult.Data = new { loginSuccessful };
                }
            }
            else
            {
                var msg = "Unable to Login";
                jsonResult.Data = new { msg };
            }

            return jsonResult;
        }

        [HttpPost]
        public ActionResult AddOrDeleteCafe(CafeUpdateModel cafeupdateModel)
        {
            var jsonResult = new JsonResult();
            var loginSuccessful = false;
            if (Session["loggedIn"] != null || Helper.Login(cafeupdateModel.LoginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                var connectionString = Constants.ODBCString;
                using (var dbConnection = new ODBConnection(connectionString))
                using (var persister = new ODBCPersister(dbConnection))
                {
                    var count = int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkCafeteriaManager"), cafeupdateModel.CafeteriaManagerID)).ToString());
                    if (count > 0)
                    {
                        // if it exists
                        var itemExists = int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkCafe"), cafeupdateModel.Cafe.CafeName)).ToString()) > 0;

                        if (itemExists)
                        {

                            //update updateStoreHouseInventory
                            persister.ExecuteNonQueryCmd("Cafe", string.Format(Helper.GetQueryValue("updateCafe"),
                                cafeupdateModel.Cafe.CafeName,
                                cafeupdateModel.Cafe.CafeName,
                                cafeupdateModel.Delete ? "inactive" : "active",
                                cafeupdateModel.Cafe.CafeteriaVendorID,
                                cafeupdateModel.Delete ? cafeupdateModel.Cafe.CafeName : cafeupdateModel.NewName));
                        }
                        else
                        {
                            persister.ExecuteNonQueryCmd("Cafe", string.Format(Helper.GetQueryValue("insertCafe"),
                               cafeupdateModel.Cafe.CafeName,
                               cafeupdateModel.Cafe.CafeName,
                               "active",
                                cafeupdateModel.Cafe.CafeteriaVendorID));
                        }
                    }
                    else
                    {
                        var msg = "You are not previliged to complete this action";
                        jsonResult.Data = new { msg };

                    }
                }
            }
            else
            {
                jsonResult.Data = new { loginSuccessful };
            }
            return jsonResult;
        }

        [HttpPost]
        public ActionResult AddOrDeleteCafeMenue(CafeMenueInsertModel cafeMenuInsertModel)
        {
            var jsonResult = new JsonResult();
            var loginSuccessful = false;
            if (Session["loggedIn"] != null || Helper.Login(cafeMenuInsertModel.LoginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                var connectionString = Constants.ODBCString;
                using (var dbConnection = new ODBConnection(connectionString))
                using (var persister = new ODBCPersister(dbConnection))
                {
                    var count = int.Parse(persister.ExecuteScalar(string.Format(Helper.GetQueryValue("checkCafeteriaManager"), cafeMenuInsertModel.LoginDetails.UserName)).ToString());
                    if (count > 0)
                    {
                        persister.ExecuteNonQueryCmd("Cafe", string.Format(Helper.GetQueryValue("insertCafeMenue"),
                            cafeMenuInsertModel.CafeMenue.CafeID,
                            cafeMenuInsertModel.CafeMenue.CafeMenueName));
                    }
                    else
                    {
                        var msg = "You are not previliged to complete this action";
                        jsonResult.Data = new { msg };

                    }
                }
            }
            else
            {
                jsonResult.Data = new { loginSuccessful };
            }
            return jsonResult;
        }


        [HttpPost]
        public ActionResult GetOrderDetailsFromCafe(CafeUpdateModel cafeupdateModel)
        {
            var jsonResult = new JsonResult();
            var loginSuccessful = false;
            var queryString = string.Format(Helper.GetQueryValue("cafeAllCustomerOrders"), cafeupdateModel.Cafe.CafeID);
            if (Session["loggedIn"] != null || Helper.Login(cafeupdateModel.LoginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                var connectionString = Constants.ODBCString;
                var data = new List<Dictionary<string, string>>();
                using (var dbConnection = new ODBConnection(connectionString))
                using (var dataSet = new DBDataSet(dbConnection, queryString))
                {
                    var cols = dataSet.ColumnNames;
                    var dic = new Dictionary<string, string>();
                    while (dataSet.MoveNext())
                    {
                        foreach (var col in cols)
                        {
                            dic[col] = dataSet.GetValue(col);
                        }
                        data.Add(dic);
                    }
                }
                jsonResult.Data = data;
            }
            else
            {
                jsonResult.Data = new { loginSuccessful };
            }
            return jsonResult;
        }
    }
}
