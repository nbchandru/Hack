using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using magHack.Models;
using magHack.core.DataBaseAccess;
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
            var connectionString = "Driver={Simba Oracle ODBC Driver};HOST=localhost;PORT=1521;UID=MagniFoodSchema;PWD=MagniFoodSchema;SVC=oraodbc";
            var loginSuccessful = true;
            var queryString = "select p.* from MagniFoodSchema.Customer c ,MagniFoodSchema.Person p where c.PersonID = p.PersonID and CustomerStatus like '%active%' and c.CustomerID like 'person01' and c.CustomerPassword like '%abc@123%'";
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

            var jsonResult = new JsonResult();
            jsonResult.Data = new { loginSuccessful };

            return jsonResult;
        }


        [HttpPost]
        public ActionResult SignUp(SignUpModel signUpModel)
        {
            var connectionString = "";
            var queryString = "";
            var signUpSuccessfull = true;
            var jsonResult = new JsonResult();
            List<string> validationLogs = new List<string>();
            using (var dbConnection = new ODBConnection(connectionString))
            using (var dataSet = new DBDataSet(dbConnection, queryString))
            {
                // if email already exists
                if (true)
                {
                    validationLogs.Add("UserName with the current email aready exists");
                }
                else
                {
                    // userName already exists
                    if (true)
                    {
                        validationLogs.Add("UserName already taken.");
                    }
                }
                if (!signUpModel.ValidateUserName())
                {
                    validationLogs.Add("Illeagal userName.");
                }
                if (!signUpModel.ValidateEmail())
                {
                    validationLogs.Add("Incorrect Email address.");
                }
                if (!signUpModel.ValidatePassword())
                {
                    validationLogs.Add("Password requirement not satisfied.");
                }

                if (validationLogs.Count == 0)
                {
                    // accept the signUp
                    jsonResult.Data = new { signUpSuccessfull };
                }
                else
                {
                    signUpSuccessfull = false;
                    jsonResult.Data = new { signUpSuccessfull , validationLogs};
                }
            }

            return jsonResult;
        }
    }
}
