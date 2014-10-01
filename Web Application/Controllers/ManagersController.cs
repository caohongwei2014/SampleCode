using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingServiceLibrary;


namespace TrainingRegistrationForConestoga.Controllers
{
    public class ManagersController : Controller
    {
        //List of all subordinates.
        // GET: /Managers/

        public ActionResult Index()
        {
            AccountService accountService = new AccountService();
            List<EmployeeReportTransfer> employeeReportList = new List<EmployeeReportTransfer>();
            List<UserTransfer> userList = new List<UserTransfer>();
            UserTransfer user = new UserTransfer();

            string userName = User.Identity.Name;
            userName = "John";
            userList = accountService.GetUserData(userName);
            user = userList.Single();
                       
            employeeReportList = accountService.GetEmployeeInfo(user.Person.PersonId);
            Session.Add("employeeList", employeeReportList);
            
            return View(employeeReportList);
        }
        //View detail of a subordinate.
        public ActionResult ViewDetail(string EmployeeName =null)
        {
            List<EmployeeReportTransfer> employeeReportList = new List<EmployeeReportTransfer>();
            List<ModuleDetailTransfer> ModuleList = new List<ModuleDetailTransfer>();
            if (EmployeeName == null)
            {

                TempData["message"] = "Please select a person.";
                return RedirectToAction("Index");
            }
            if (Session["employeeList"] == null)
            {
                return RedirectToAction("Index");
            }
            else {
                employeeReportList =( List < EmployeeReportTransfer >) Session["employeeList"];
                foreach (var item in employeeReportList)
                {
                    if (item.EmployeeName == EmployeeName)
                    {
                        ModuleList = item.ModuleList;
                    }                
                }
            }

            return View(ModuleList);
        }

    }
}
