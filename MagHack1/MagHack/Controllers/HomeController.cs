using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MacgHack.Core.DataBaseAccess;
using MacgHack.Core.DataSet;
using MacgHack.Core.Model;
using MagHack.Models;

namespace MagHack.Controllers
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
            var connectionString = "DSN=Bamboo DSN;Uid=MagniFoodSchema;Pwd=MagniFoodSchema;";
            var loginSuccessful = true;
            var queryString = "select p.* from MagniFoodSchema.Customer c ,MagniFoodSchema.Person p where c.PersonID = p.PersonID and CustomerStatus like '%active%' and c.CustomerID like '%customer01%' and c.CustomerPassword like '%abc@123%'";
            using (var dbConnection = new ODBConnection(connectionString))
            using (var dataSet = new DBDataSet(dbConnection, queryString))
            {
                while (dataSet.MoveNext())
                {
                    var a = dataSet.GetRow();
                    if (a.Count == 0)
                    {
                        loginSuccessful = false;
                    }
                }
            }

            return Json(new { loginSuccessful });
        }

        public Action SignUp()
        {
            return null;
        }
    }
}
