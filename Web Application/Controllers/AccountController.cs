using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using TrainingRegistrationForConestoga.Filters;
using TrainingRegistrationForConestoga.Models;
using TrainingServiceLibrary;

namespace TrainingRegistrationForConestoga.Controllers
{
   
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
            AccountService accountService = new AccountService();
            UserTransfer user=new UserTransfer();
            AccountAccess account = new AccountAccess();
            DepartmentAccess department = new DepartmentAccess();
            CampusAccess campus = new CampusAccess();
            PersonAccess person = new PersonAccess();
            

        //Login 
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //Verify the user's login information.
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var userName = model.UserName;
            var password = model.Password;
            
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //User logoff
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //Register a user. Collect user's information.
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            AccountService accountService = new AccountService();
            List<DepartmentAccess> departments = new List<DepartmentAccess>();
            departments = accountService.GetDepartmentData();
            ViewBag.department = new SelectList(departments,"departmentId", "name");

            List<CampusAccess> campuses = new List<CampusAccess>();
            campuses = accountService.GetCampusData();
            ViewBag.campus = new SelectList(campuses,"campusId","name");

            SelectListItem administrator = new SelectListItem() { Text = "Administrator", Value = "Administrator" };
            SelectListItem manager = new SelectListItem() { Text = "Manager", Value = "Manager" };
            SelectListItem regular = new SelectListItem() { Text = "RegularUser", Value = "RegularUser" };
            ViewBag.userType = new SelectList(new SelectListItem[] { administrator,manager,regular }, "Value", "Text","RegularUser");

            List<PersonAccess> persons = new List<PersonAccess>();
            persons = accountService.GetPersonData();

  //first name will be changed to full name ?????????
            ViewBag.supervisor = new SelectList(persons,"personId","firstName");

            SelectListItem female = new SelectListItem() { Text = "Female", Value = "F" };
            SelectListItem male = new SelectListItem() { Text = "Male", Value = "M" };
            ViewBag.gender = new SelectList(new SelectListItem[] { female,male }, "Value", "Text");

            return View();
        }

        //Save user's information to database.
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (!Roles.RoleExists("Administrator"))
            {
                Roles.CreateRole("Administrator");

            };
            if (!Roles.RoleExists("Manager"))
            {
                Roles.CreateRole("Manager");
            }
            if (!Roles.RoleExists("RegularUser"))
            {
                Roles.CreateRole("RegularUser");
            }

            user.Account = account;
            user.Campus = campus;
            user.Dempartment = department;
            user.Person = person;
            
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                   // WebSecurity.Login(model.UserName, model.Password);
                   
                    user.Account.UserName=model.UserName;
                    user.Account.Password=model.Password;
                    user.Account.UserType = Request.Form["userType"];
                    var userType = Request.Form["userType"];
                    string[] userName = new string[] {model.UserName };
                    Roles.AddUsersToRole(userName, userType);

                    //user.Account.UserType = "administrator";
                    user.Person.FirstName=Request.Form["firstName"];
                    user.Person.LastName=Request.Form["lastName"];
                    if (Request.Form["gender"] == "F")
                    {
                        user.Person.Gender = 'F';
                    }
                    else if (Request.Form["gender"] == "M")
                    {
                        user.Person.Gender = 'M';
                    }
                    user.Person.Address=Request.Form["address"];
                    user.Person.Email=Request.Form["email"];
                    user.Person.PhoneNumber=Request.Form["phoneNumber"];
                    if (Request.Form["supervisor"]==null ||Request.Form["supervisor"]== "" )
                    {
                        user.Person.SupervisorId = 0;
                    }
                    else
                    {
                        user.Person.SupervisorId = Convert.ToInt32(Request.Form["supervisor"]);
                    }

                    if (Request.Form["department"] == null || Request.Form["department"] == "")
                    {
                        user.Dempartment.DepartmentId = 0;
                    }
                    else
                    {
                        user.Dempartment.DepartmentId = Convert.ToInt32(Request.Form["department"]);
                    }

                    if (Request.Form["campus"] == null || Request.Form["campus"] == "")
                    {
                        user.Campus.CampusId = 0;
                    }
                    else
                    {
                        user.Campus.CampusId = Convert.ToInt32(Request.Form["campus"]);
                    }


                    if (accountService.AddUser(user))
                    {
                        TempData["message"] = "The new user has been created.";
                        return RedirectToAction("Register", "Account");
                    }                                      
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form

            List<DepartmentAccess> departments = new List<DepartmentAccess>();
            departments = accountService.GetDepartmentData();
            ViewBag.department = new SelectList(departments, "departmentId", "name");

            List<CampusAccess> campuses = new List<CampusAccess>();
            campuses = accountService.GetCampusData();
            ViewBag.campus = new SelectList(campuses, "campusId", "name");

            SelectListItem administrator = new SelectListItem() { Text = "Administrator", Value = "Administrator" };
            SelectListItem manager = new SelectListItem() { Text = "Manager", Value = "Manager" };
            SelectListItem regular = new SelectListItem() { Text = "RegularUser", Value = "RegularUser" };
            ViewBag.userType = new SelectList(new SelectListItem[] { administrator, manager, regular }, "Value", "Text", "RegularUser");

            List<PersonAccess> persons = new List<PersonAccess>();
            persons = accountService.GetPersonData();            
            //first name will be changed to full name ?????????
            ViewBag.supervisor = new SelectList(persons, "personId", "firstName");

            SelectListItem female = new SelectListItem() { Text = "Female", Value = "F" };
            SelectListItem male = new SelectListItem() { Text = "Male", Value = "M" };
            ViewBag.gender = new SelectList(new SelectListItem[] { female, male }, "Value", "Text");

            return View(model);
        }

        //List all users.
        public ActionResult ListAccount()
        { 
            List<AccountAccess> accounts = new List<AccountAccess>();
            var accountInfo = accountService.GetUserData();

            var searchInput = Request.Form["search"];
            if (searchInput != null && searchInput.Trim() != "")
            {
                foreach (var item in accountInfo)
                {
                    if(item.Account.UserName.Contains(searchInput))
                        accounts.Add(item.Account);
                }  
            }
            else
            {
                 foreach (var item in accountInfo)
                 {
                    accounts.Add(item.Account);            
                }            
            }
            return View(accounts);
        }

        //Edit user's information.
        public ActionResult Edit(string userName=null)
        {
            user = accountService.GetUserData(userName).Single();            

            List<DepartmentAccess> departments = new List<DepartmentAccess>();
            departments = accountService.GetDepartmentData();
            ViewBag.department = new SelectList(departments, "departmentId", "name",user.Dempartment.DepartmentId);

            List<CampusAccess> campuses = new List<CampusAccess>();
            campuses = accountService.GetCampusData();
            ViewBag.campus = new SelectList(campuses, "campusId", "name",user.Campus.CampusId);

            SelectListItem administrator = new SelectListItem() { Text = "Administrator", Value = "Administrator" };
            SelectListItem manager = new SelectListItem() { Text = "Manager", Value = "Manager" };
            SelectListItem regular = new SelectListItem() { Text = "RegularUser", Value = "RegularUser" };
            ViewBag.userType = new SelectList(new SelectListItem[] { administrator, manager, regular }, "Value", "Text", user.Account.UserType);

            List<PersonAccess> persons = new List<PersonAccess>();
            persons = accountService.GetPersonData();
            //first name will be changed to full name ?????????
            if ( user.Person.SupervisorId == 0) {
                ViewBag.supervisor = new SelectList(persons, "personId", "firstName");
            }
            else
            {
                ViewBag.supervisor = new SelectList(persons, "personId", "firstName", user.Person.SupervisorId);
            }

            SelectListItem female = new SelectListItem() { Text = "Female", Value = "F" };
            SelectListItem male = new SelectListItem() { Text = "Male", Value = "M" };
            ViewBag.gender = new SelectList(new SelectListItem[] { female, male }, "Value", "Text",user.Person.Gender);

            return View(user);
        }

        //Save user's new information.
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            var userName=Request.Form["UserName"];
            AccountService accountService = new AccountService();
            user = accountService.GetUserData(userName).Single(); 
            user.Account.Password = Request.Form["Password"];
            string UserType = Request.Form["userType"];
            user.Account.UserName = Request.Form["UserName"];            

            var userType = Request.Form["userType"];
            string[] userNameArray = new string[] {user.Account.UserName };
            if (UserType != "")
            {
                string[] users = Roles.GetUsersInRole(UserType);
                if (!users.Contains(userName))
                {
                    Roles.RemoveUserFromRole(userName, user.Account.UserType);
                    Roles.AddUsersToRole(userNameArray, userType);
                    user.Account.UserType = UserType;
                }
            }    //Roles.DeleteRole("administrator"); 

            //user.Account.UserType = "administrator";
            user.Person.FirstName = Request.Form["FirstName"];
            user.Person.LastName = Request.Form["LastName"];
            if (Request.Form["gender"] == "F")
            {
                user.Person.Gender = 'F';
            }
            else if (Request.Form["gender"] == "M")
            {
                user.Person.Gender = 'M';
            }
            user.Person.Address = Request.Form["Address"];
            user.Person.Email = Request.Form["Email"];
            user.Person.PhoneNumber = Request.Form["PhoneNumber"];
            if (Request.Form["supervisor"] == null || Request.Form["supervisor"] == "")
            {
                user.Person.SupervisorId = 0;
            }
            else
            {
                user.Person.SupervisorId = Convert.ToInt32(Request.Form["supervisor"]);
            }
            user.Dempartment.DepartmentId = Convert.ToInt32(Request.Form["Department"]);
            user.Campus.CampusId = Convert.ToInt32(Request.Form["Campus"]);

            if (accountService.UpdateUser(user))
            {
                var token = WebSecurity.GeneratePasswordResetToken(user.Account.UserName);
                WebSecurity.ResetPassword(token,user.Account.Password);

                return RedirectToAction("ListAccount");
            }

            List<DepartmentAccess> departments = new List<DepartmentAccess>();
            departments = accountService.GetDepartmentData();
            ViewBag.department = new SelectList(departments, "departmentId", "name");

            List<CampusAccess> campuses = new List<CampusAccess>();
            campuses = accountService.GetCampusData();
            ViewBag.campus = new SelectList(campuses, "campusId", "name");

            SelectListItem administrator = new SelectListItem() { Text = "Administrator", Value = "Administrator" };
            SelectListItem manager = new SelectListItem() { Text = "Manager", Value = "Manager" };
            SelectListItem regular = new SelectListItem() { Text = "RegularUser", Value = "RegularUser" };
            ViewBag.userType = new SelectList(new SelectListItem[] { administrator, manager, regular }, "Value", "Text", "RegularUser");

            List<PersonAccess> persons = new List<PersonAccess>();
            persons = accountService.GetPersonData();
            //first name will be changed to full name ?????????
            ViewBag.supervisor = new SelectList(persons, "personId", "firstName");

            SelectListItem female = new SelectListItem() { Text = "Female", Value = "F" };
            SelectListItem male = new SelectListItem() { Text = "Male", Value = "M" };
            ViewBag.gender = new SelectList(new SelectListItem[] { female, male }, "Value", "Text");

            return View(user);
        }

        //Delete an account.
        public ActionResult DeleteAccount(int id, string userName)
        {
            List<int> idlist = new List<int>();
            idlist.Add(id);

            if (accountService.DeleteUser(idlist))
            {
                var user=Membership.GetUser(userName);
                var roles=Roles.GetAllRoles();
                foreach (var item in roles)
                { 
                    if(Roles.IsUserInRole(userName,item) )
                    {
                        Roles.RemoveUserFromRole(userName, item);
                    }
                
                }
                Membership.DeleteUser(userName,true);   
                
                TempData["message"] = " The account was deleted.";
                return RedirectToAction("ListAccount");
            }
            TempData["message"] = "Delete account error.";
            return RedirectToAction("ListAccount");
        }
        //View all modules
        public ActionResult AllModules()
        {
            List<AdminViewReportTransfer> moduleReportList = new List<AdminViewReportTransfer>();
            List<EmployeeDetailTransfer> employeeDetailList = new List<EmployeeDetailTransfer>();
            moduleReportList = accountService.GetModuleInfo();
            Session.Add("moduleReportList", moduleReportList);
            return View(moduleReportList);
        }
        //View detail information of a module.
        public ActionResult ModuleDetails(int id = -1)
        {
            if (id == -1)
            {
                TempData["message"] = "Please select a module.";
                return RedirectToAction("AllModules");
            }
            if (Session["moduleReportList"] != null)
            {
                List<AdminViewReportTransfer> moduleReportList = new List<AdminViewReportTransfer>();
                List<EmployeeDetailTransfer> employeeDetailList = new List<EmployeeDetailTransfer>();
                moduleReportList = (List<AdminViewReportTransfer>)Session["moduleReportList"];
                employeeDetailList = moduleReportList.ElementAt(id).Detail;
                ViewBag.ModuleName = moduleReportList.ElementAt(id).ModuleName;
                return View(employeeDetailList);
            }
            else {
                return RedirectToAction("AllModules");
            }        
        }

        public ActionResult Exception()
        {
            AccountService accountService = new AccountService();
            ModuleService moduleService = new ModuleService();
            List<UserTransfer> userList = new List<UserTransfer>();
            List<ModuleAccess> moduleList = new List<ModuleAccess>();

            userList = accountService.GetUserData();
            ViewBag.userList = new SelectList(userList, "accountId", "userName");
            moduleList = moduleService.GetModuleData();
            ViewBag.moduleList = new SelectList(moduleList, "moduleId", "moduleName");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
