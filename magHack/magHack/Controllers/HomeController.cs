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
using System.Threading;


namespace magHack.Controllers
{
    public class HomeController : Controller
    {
        private Helper m_helper = new Helper();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        public ActionResult SignIn(LoginModel loginModel)
        {
            var connectionString = Constants.ODBCString;
            var loginSuccessful = m_helper.Login(loginModel, connectionString);
            var jsonResult = new JsonResult();
            jsonResult.Data = new { loginSuccessful };
            return jsonResult;
        }

        private List<string> CheckSignUpType(SignUpModel signUpModel, string querykey, ODBCPersister persister)
        {
            List<string> validationLogs = new List<string>();
            // if email already exists
            var checkEmailQuery = int.Parse(persister.ExecuteScalar(String.Format(m_helper.GetQueryValue(querykey), signUpModel.UserID)).ToString());
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
                    var updatePersonCommand = String.Format(m_helper.GetQueryValue("insertPersonShort"), signUpModel.UserID, signUpModel.UserID);
                    switch (signUpModel.Type)
                    {
                        case UserType.CAFE_USER:
                            validationLogs = CheckSignUpType(signUpModel, "checkCafeUser", persister);
                            if (validationLogs.Count == 0)
                            {
                                var updatetabelCommand = String.Format(m_helper.GetQueryValue("insertCafeUser"), signUpModel.UserID, signUpModel.NewPassword, "active", signUpModel.CafeID);
                                signUpSuccessfull = m_helper.SignUpStatusObject(persister, signUpModel, updatePersonCommand, "CafeUser", updatetabelCommand);
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
                                var updatetabelCommand = String.Format(m_helper.GetQueryValue("insertCustomerShort"), signUpModel.UserID, signUpModel.NewPassword);
                                signUpSuccessfull = m_helper.SignUpStatusObject(persister, signUpModel, updatePersonCommand, "Customer", updatetabelCommand);
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
                                var updatetabelCommand = String.Format(m_helper.GetQueryValue("insertCafeteriaManager"), signUpModel.UserID, signUpModel.NewPassword,"active", signUpModel.VendorID);
                                signUpSuccessfull = m_helper.SignUpStatusObject(persister, signUpModel, "CafeUser", "CafeteriaManager", updatetabelCommand);
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
            if (Session["loggedIn"] != null || m_helper.Login(inventoryUpdateModel.LoginDetails, Constants.ODBCString))
            {
                var connectionString = Constants.ODBCString;
                using (var dbConnection = new ODBConnection(connectionString))
                using (var persister = new ODBCPersister(dbConnection))
                {
                    var count = int.Parse(persister.ExecuteScalar(string.Format(m_helper.GetQueryValue("checkCafeteriaManagerVendorID"), inventoryUpdateModel.InventoryModel.CafeteriaVendorID)).ToString());
                    if (count > 0)
                    {
                        // if it exists
                        var itemExists = int.Parse(persister.ExecuteScalar(string.Format(m_helper.GetQueryValue("checkStoreHouseInventoryItem"), inventoryUpdateModel.InventoryModel.IngredientID)).ToString()) > 0;
                        if (itemExists)
                        {

                            //update updateStoreHouseInventory
                            persister.ExecuteNonQueryCmd("StoreHouseInventory", string.Format(m_helper.GetQueryValue("updateStoreHouseInventory"),
                                inventoryUpdateModel.InventoryModel.IngredientID,
                                inventoryUpdateModel.InventoryModel.IngredientName,
                                inventoryUpdateModel.InventoryModel.IngredientQuantity,
                                inventoryUpdateModel.InventoryModel.IngredientQuantityUnit,
                                "instock",
                                inventoryUpdateModel.InventoryModel.IngredientID));
                        }
                        else
                        {
                            persister.ExecuteNonQueryCmd("StoreHouseInventory", string.Format(m_helper.GetQueryValue("insertStoreHouseInventory"),
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
                var msg = "Unable to Login";
                jsonResult.Data = new { msg };
            }

            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetMenuForCafeID(LoginModel loginDetails, string cafeID)
        {
            var jsonResult = new JsonResult();
            var loginSuccessful = false;
            var queryString = string.Format(m_helper.GetQueryValue("cafeMenueItems"), cafeID);
            if (Session["loggedIn"] != null || m_helper.Login(loginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                var Data = m_helper.GetODBCData(Constants.ODBCString, queryString);
                var count = Data.Count;
                jsonResult.Data = new { Data, count};
            }
            else
            {
                jsonResult.Data = new { loginSuccessful };
            }
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetCustomerOrdersForCafe(LoginModel loginDetails, string cafeID)
        {
            var jsonResult = new JsonResult();
            var loginSuccessful = false;
            var queryString = string.Format(m_helper.GetQueryValue("cafeAllCustomerOrdersFromCafe"), cafeID);
            if (Session["loggedIn"] != null || m_helper.Login(loginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                var Data = m_helper.GetODBCData(Constants.ODBCString, queryString);
                var count = Data.Count;
                jsonResult.Data = new { Data, count };
            }
            else
            {
                jsonResult.Data = new { loginSuccessful };
            }
            return jsonResult;
        }

        #region cafeRelated

        [HttpPost]
        public ActionResult InsertRecipe(RecipeUpdateModel recipeUpdate)
        {
            var jsonResult = new JsonResult();
            var loginSuccessful = false;
            if (Session["loggedIn"] != null || m_helper.Login(recipeUpdate.LoginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                var connectionString = Constants.ODBCString;
                using (var dbConnection = new ODBConnection(connectionString))
                using (var persister = new ODBCPersister(dbConnection))
                {
                    var count = int.Parse(persister.ExecuteScalar(string.Format(m_helper.GetQueryValue("checkCafeUser"), recipeUpdate.LoginDetails.UserName)).ToString());
                    if (count > 0)
                    {
                        // if it exists
                        var itemExists = int.Parse(persister.ExecuteScalar(string.Format(m_helper.GetQueryValue("checkCafe"), recipeUpdate.CafeID, "active")).ToString()) > 0;
                        if (itemExists)
                        {

                            //update updateStoreHouseInventory
                            persister.ExecuteNonQueryCmd("FoodItem", string.Format(m_helper.GetQueryValue("updateFoodItem"),
                                recipeUpdate.Food.FoodItemID,
                                recipeUpdate.Food.FoodItemName,
                                recipeUpdate.Food.FoodItemType,
                                recipeUpdate.Food.FoodItemDescription,
                                recipeUpdate.Food.FoodItemCost,
                                recipeUpdate.Food.FoodItemStatus,
                                recipeUpdate.Food.CafeMenuID));

                            persister.ExecuteScalar(string.Format(m_helper.GetQueryValue("updateRecipe"), recipeUpdate.Recipe.FoodItemIngredientQuantity, recipeUpdate.Recipe.FoodItemID));
                        }
                        else
                        {
                            persister.ExecuteNonQueryCmd("FoodItem", string.Format(m_helper.GetQueryValue("insertFoodItem"),
                                recipeUpdate.Food.FoodItemID,
                                recipeUpdate.Food.FoodItemName,
                                recipeUpdate.Food.FoodItemType,
                                recipeUpdate.Food.FoodItemDescription,
                                recipeUpdate.Food.FoodItemCost,
                                recipeUpdate.Food.FoodItemStatus,
                                recipeUpdate.Food.CafeMenuID));

                            persister.ExecuteScalar(string.Format(m_helper.GetQueryValue("insertRecipe"), 
                                recipeUpdate.Recipe.FoodItemID,
                                recipeUpdate.Recipe.FoodItemIngredientQuantity,
                                recipeUpdate.Recipe.FoodItemIngredientQuantityUnit,
                                recipeUpdate.Recipe.IngredientID));
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
        public ActionResult AddOrDeleteCafe(CafeUpdateModel cafeupdateModel)
        {
            var jsonResult = new JsonResult();
            var loginSuccessful = false;
            if (Session["loggedIn"] != null || m_helper.Login(cafeupdateModel.LoginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                var connectionString = Constants.ODBCString;
                using (var dbConnection = new ODBConnection(connectionString))
                using (var persister = new ODBCPersister(dbConnection))
                {
                    var count = int.Parse(persister.ExecuteScalar(string.Format(m_helper.GetQueryValue("checkCafeteriaManager"), cafeupdateModel.CafeteriaManagerID)).ToString());
                    if (count > 0)
                    {
                        // if it exists
                        var itemExists = int.Parse(persister.ExecuteScalar(string.Format(m_helper.GetQueryValue("checkCafe"), cafeupdateModel.Cafe.CafeName)).ToString()) > 0;

                        if (itemExists)
                        {

                            //update updateStoreHouseInventory
                            persister.ExecuteNonQueryCmd("Cafe", string.Format(m_helper.GetQueryValue("updateCafe"),
                                cafeupdateModel.Cafe.CafeName,
                                cafeupdateModel.Cafe.CafeName,
                                cafeupdateModel.Delete ? "inactive" : "active",
                                cafeupdateModel.Cafe.CafeteriaVendorID,
                                cafeupdateModel.Delete ? cafeupdateModel.Cafe.CafeName : cafeupdateModel.NewName));
                        }
                        else
                        {
                            persister.ExecuteNonQueryCmd("Cafe", string.Format(m_helper.GetQueryValue("insertCafe"),
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
        public ActionResult AddCafeMenue(CafeMenueInsertModel cafeMenuInsertModel)
        {
            var jsonResult = new JsonResult();
            var loginSuccessful = false;
            if (Session["loggedIn"] != null || m_helper.Login(cafeMenuInsertModel.LoginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                var connectionString = Constants.ODBCString;
                using (var dbConnection = new ODBConnection(connectionString))
                using (var persister = new ODBCPersister(dbConnection))
                {
                    var count = int.Parse(persister.ExecuteScalar(string.Format(m_helper.GetQueryValue("checkCafeteriaManager"), cafeMenuInsertModel.LoginDetails.UserName)).ToString());
                    if (count > 0)
                    {
                        persister.ExecuteNonQueryCmd("Cafe", string.Format(m_helper.GetQueryValue("insertCafeMenue"),
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
            var queryString = string.Format(m_helper.GetQueryValue(cafeupdateModel.Cafe.CafeID == null ? "cafeAllCustomerOrdersFromCafe" :"cafeAllCustomerOrders"), cafeupdateModel.Cafe.CafeID);
            if (Session["loggedIn"] != null || m_helper.Login(cafeupdateModel.LoginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                var Data = m_helper.GetODBCData(Constants.ODBCString, queryString);
                jsonResult.Data = Data;
            }
            else
            {
                jsonResult.Data = new { loginSuccessful };
            }
            return jsonResult;
        }


        [HttpPost]
        public ActionResult GetCafeByStatus(CafeUpdateModel cafeupdateModel)
        {
            var jsonResult = new JsonResult();
            var loginSuccessful = false;
            var queryString = string.Format(m_helper.GetQueryValue("allcafes"), cafeupdateModel.Cafe.Status);
            if (Session["loggedIn"] != null || m_helper.Login(cafeupdateModel.LoginDetails, Constants.ODBCString))
            {
                loginSuccessful = true;
                var Data = m_helper.GetODBCData(Constants.ODBCString, queryString).GroupBy(x => x["CAFEID"]).ToDictionary(x => x.Key, x => x.ToList().Count);
                jsonResult.Data = Data;
            }
            else
            {
                jsonResult.Data = new { loginSuccessful };
            }
            return jsonResult;
        }
        #endregion

        [HttpPost]
        public ActionResult GetOrderDetails(LoginModel loginDetails, List<string> orderList, string cafeID, string cafeMenuID)
        {
            var jsonResult = new JsonResult();
            var orderMap = orderList.ToDictionary(x => x.Split('-')[0], x => int.Parse(x.Split('-')[1]));
            
            if (Session["loggedIn"] != null || m_helper.Login(loginDetails, Constants.ODBCString))
            {

                using (var dbConnection = new ODBConnection(Constants.ODBCString))
                using (var persister = new ODBCPersister(dbConnection))
                {
                    float sum = 0.0f;
                    foreach (var entry in orderMap)
                    {
                        var data = m_helper.GetODBCData(Constants.ODBCString, string.Format(m_helper.GetQueryValue("foodDetails"), entry.Key))[0];
                        sum += float.Parse(data["fooditemcost"]);
                        sum = sum * float.Parse(entry.Key + ""); 
                    }
                    var orderID = Guid.NewGuid();
                    var queryString = string.Format(m_helper.GetQueryValue("registerOrder"), orderID, "orderplaced", sum, cafeID, loginDetails.UserName);
                    persister.ExecuteNonQueryCmd("CustomerOrder", queryString);

                    foreach (var entry in orderMap)
                    {
                        queryString = string.Format(m_helper.GetQueryValue("addFoodToORder"), orderID, entry.Key, entry.Value,"orderplaced", loginDetails.UserName);
                        persister.ExecuteNonQueryCmd("CustomerOrder", queryString);
                    }
                    queryString = string.Format(m_helper.GetQueryValue("inventoryCheck"), orderID);
                    var Data = m_helper.GetODBCData(Constants.ODBCString, queryString);

                    foreach (var entry in Data)
                    {
                        if (float.Parse(entry["totalIngredientQuantityRequired"]) > float.Parse(entry["IngredientQuantity"]))
                        {

                            var msg = "rejected";
                            jsonResult.Data = msg;//updateOrderStatus
                            queryString = string.Format(m_helper.GetQueryValue("updateOrderStatus"), "rejected",orderID);
                            persister.ExecuteNonQueryCmd("CustomerOrder", queryString);
                            return jsonResult;
                        }
                    }

                    foreach (var entry in Data)
                    {
                        var qty = float.Parse(entry["IngredientQuantity"]) - float.Parse(entry["totalIngredientQuantityRequired"]);
                        var status = qty > 0 ? "availabel" : "out of stock";
                        queryString = string.Format(m_helper.GetQueryValue("decrimentStore"), qty, entry["IngredientStatus"], entry["IngredientID"]);
                        persister.ExecuteNonQueryCmd("StoreHouseInventory", queryString);
                    }
                    persister.ExecuteNonQueryCmd("FoodItem", m_helper.GetQueryValue("marFoodItemNA"));

                    DateTime time = DateTime.Now;
                    int count = 60;
                    while (count > 0)
                    {
                        string status = "";
                        if ((DateTime.Now - time).Seconds < 5)
                        {
                            status = "orderplaced";
                        }
                        if ((DateTime.Now - time).Seconds < 12)
                        {
                            status = "processing";
                        }
                        if ((DateTime.Now - time).Seconds < 16)
                        {
                            status = "out for delivery ";
                        }
                        queryString = string.Format(m_helper.GetQueryValue("updateOrderStatus"), status, orderID);
                        persister.ExecuteNonQueryCmd("CustomerOrder", queryString);
                        Thread.Sleep(1000);
                        count--;
                    }
                }
            }
            jsonResult.Data = "successful :)";
            return jsonResult;
        }
    }
}
