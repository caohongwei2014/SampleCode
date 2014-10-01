using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TrainingServiceLibrary;
using WebMatrix.WebData;

namespace TrainingRegistrationForConestoga.Controllers
{
    public class RegularUserController : Controller
    {
        //
        // GET: /RegularUser/

        public ActionResult Index()
        {
            AccountService accountService = new AccountService();
            List<ModuleDetailTransfer> moduleList = new List<ModuleDetailTransfer>();
            string userName = User.Identity.Name;
            moduleList=accountService.GetModuleInfoForUser(userName);
            return View(moduleList);
        }
        public ActionResult Profile()
        {
            AccountService accountService = new AccountService();
            UserTransfer user = new UserTransfer();
            string userName = User.Identity.Name;
            ViewBag.userName = userName;
            user = accountService.GetUserData(userName).Single();

            List<DepartmentAccess> departments = new List<DepartmentAccess>();
            departments = accountService.GetDepartmentData();
            ViewBag.department = new SelectList(departments, "departmentId", "name", user.Dempartment.DepartmentId);

            List<CampusAccess> campuses = new List<CampusAccess>();
            campuses = accountService.GetCampusData();
            ViewBag.campus = new SelectList(campuses, "campusId", "name", user.Campus.CampusId);

            SelectListItem administrator = new SelectListItem() { Text = "Administrator", Value = "Administrator" };
            SelectListItem manager = new SelectListItem() { Text = "Manager", Value = "Manager" };
            SelectListItem regular = new SelectListItem() { Text = "RegularUser", Value = "RegularUser" };
            ViewBag.userType = new SelectList(new SelectListItem[] { administrator, manager, regular }, "Value", "Text", user.Account.UserType);

            List<PersonAccess> persons = new List<PersonAccess>();
            persons = accountService.GetPersonData();
            //first name will be changed to full name ?????????
            if (user.Person.SupervisorId == 0)
            {
                ViewBag.supervisor = new SelectList(persons, "personId", "firstName");
            }
            else
            {
                ViewBag.supervisor = new SelectList(persons, "personId", "firstName", user.Person.SupervisorId);
            }

            SelectListItem female = new SelectListItem() { Text = "Female", Value = "F" };
            SelectListItem male = new SelectListItem() { Text = "Male", Value = "M" };
            ViewBag.gender = new SelectList(new SelectListItem[] { female, male }, "Value", "Text", user.Person.Gender);

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePersonalInfo()
        {
            AccountService accountService = new AccountService();
            UserTransfer user = new UserTransfer();
            string userName = User.Identity.Name;
            ViewBag.userName = userName;
            user = accountService.GetUserData(userName).Single();

            user.Account.Password = Request.Form["Password"];
            //user.Account.UserType = Request.Form["userType"];
            user.Account.UserName = Request.Form["UserName"];

            var userType = Request.Form["userType"];
            string[] userNameArray = new string[] { user.Account.UserName };

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
                WebSecurity.ResetPassword(token, user.Account.Password);

                return RedirectToAction("Success");
            }
            TempData["message"] = "Failed to update your personal information, Please try it later.";
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

        public ActionResult Success()
        {

            return View();
        }

        public ActionResult TakeModule()
        {
            IModuleService moduleService = new ModuleService();
            List<ModuleAccess> moduleList = new List<ModuleAccess>();
            string userName = User.Identity.Name;
            string searchInput;

            if (Request.Form["search"] == null)
            {
                searchInput = null;
            }
            else
            {
                searchInput = Request.Form["search"].ToString();
            }

            moduleList = moduleService.GetModuleDataForUser(userName, searchInput);
            Session["moduleListForRegular"] = moduleList;

            return View(moduleList);
        }

        public ActionResult StudyModule(int id)
        {
            IModuleService moduleService = new ModuleService();
            AgreementInfoAccess agreement = new AgreementInfoAccess();
            agreement = moduleService.ShowAgreement(id);

            return View(agreement);
        }

        public ActionResult ContinueStudy(int id, string type = null)
        {
            IQuizService quizService = new QuizService();
            ITrainingService trainingService = new TrainingService();
            IAccountService accountService = new AccountService();
            IModuleService moduleService = new ModuleService();
            List<TrainingAccess> trainingListForModule = new List<TrainingAccess>();
            List<QuizTransfer> quizListForModule = new List<QuizTransfer>();
            AccountModuleAccess accountModule = new AccountModuleAccess();
            string userName = User.Identity.Name;

            //save data in Account_Module table
            accountModule.AccountId = accountService.GetAccountId(userName);
            accountModule.ModuleId = id;
            accountModule.Status = "pass";
            if (type == null)
            {
                moduleService.TakeModule(accountModule);
            }
            


            //get quiz and training list for the module assigned by customers
            trainingListForModule = trainingService.GetTrainingDataForModule(id);
            quizListForModule = quizService.GetQuizForModule(id);

            Session["trainingListForModule"] = trainingListForModule;
            Session["quizListForModule"] = quizListForModule;
            return View();
        }

        public ActionResult TakeTraining(int id = 0)
        {
            int trainingId = 0;
            ITrainingService trainingService = new TrainingService();
            TrainingTransfer trainingDetail = new TrainingTransfer();
            if (id == 0)
            {
                trainingId = Convert.ToInt32(Session["trainingId"]);
            }
            else
            {
                trainingId = id;
            }
            trainingDetail = trainingService.GeTrainingDetailData(trainingId);
            ViewBag.trainingDetail = trainingDetail;
            Session["trainingId"] = trainingId;

            return View();
        }

        public ActionResult Download(string documentName)
        {
            var path = Path.Combine(Server.MapPath("~/App_Data/File"), documentName);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = documentName;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                TempData["message"] = "The file " + documentName + " can not be found.";
                return RedirectToAction("TakeTraining");
            }
        }

        public ActionResult TakeQuiz(int id)
        {
            List<QuizTransfer> quizListForModule = new List<QuizTransfer>();
            QuizTransfer quizForModule = new QuizTransfer();


            SelectListItem T = new SelectListItem() { Text = "True", Value = "T" };
            SelectListItem F = new SelectListItem() { Text = "False ", Value = "F" };

            ViewBag.trueOrFalse = new SelectList(new SelectListItem[] { T, F }, "Value", "Text");

            quizListForModule = (List<QuizTransfer>)Session["quizListForModule"];
            foreach (QuizTransfer item in quizListForModule)
            {
                if (item.QuizDescription.QuizId == id)
                {
                    quizForModule = item;
                    Session["quizForModule"] = quizForModule;
                    break;
                }
            }
            return View(quizForModule);
        }

        public ActionResult EndTakeQuiz()
        {
            //get membershipTypeName from the form
            //if (Request.Form.Get("membershipTypeName") != null)
            //{
            //    membership.membershipTypeName = Request.Form.Get("membershipTypeName");
            //}
            IQuizService quizService = new QuizService();
            IAccountService accountService = new AccountService();
            QuizRecordAccess quizRecord = new QuizRecordAccess();
            QuizAccountAnswerAccess quizAccountRecord = new QuizAccountAnswerAccess();
            List<string> acccountBoolAnswer = new List<string>();
            List<string> acccountMulAnswer = new List<string>();
            int correctNumber = 0;
            int mark = 0;
            string userName = User.Identity.Name;
            int accountId = accountService.GetAccountId(userName);


            QuizTransfer quiz = (QuizTransfer)Session["quizForModule"];
            //modify bool question
            for (int i = 0; i < quiz.BoolQuestionNumber; i++)
            {

                if (Request.Form.Get("boolQuestion" + i) != null)
                {
                    quizAccountRecord.QuestionAnswerId = quiz.BoolQuestion.ElementAt(i).QuestionAnswerId;
                    quizAccountRecord.AccountId = accountId;
                    quizAccountRecord.Answer = Request.Form.Get("boolQuestion" + i);
                    acccountBoolAnswer.Add(Request.Form.Get("boolQuestion" + i));
                    if (Request.Form.Get("boolQuestion" + i) == quiz.BoolQuestion.ElementAt(i).Answer)
                    {
                        correctNumber++;
                    }
                }
                else
                {
                    acccountBoolAnswer.Add(" ");
                }
            }

            //modify multiselect question
            for (int j = 0; j < quiz.MulSelQuestionNumber; j++)
            {

                if (Request.Form.Get("multiSelectItem" + j) != null)
                {
                    quizAccountRecord.QuestionAnswerId = quiz.MulSelQuestion.ElementAt(j).QuestionAnswer.QuestionAnswerId;
                    quizAccountRecord.AccountId = accountId;
                    quizAccountRecord.Answer = Request.Form.Get("multiSelectItem" + j);
                    acccountMulAnswer.Add(Request.Form.Get("multiSelectItem" + j));
                    if (Request.Form.Get("multiSelectItem" + j) == quiz.MulSelQuestion.ElementAt(j).QuestionAnswer.Answer)
                    {
                        correctNumber++;
                    }
                }
                else
                {
                    acccountMulAnswer.Add(" ");
                }
            }

            //calculate mark
            mark = Convert.ToInt32(Math.Round((double)((float)correctNumber / (float)(quiz.MulSelQuestionNumber + quiz.BoolQuestionNumber)), 2) * 100);
            Session["mark"] = mark;
            Session["BoolAnswer"] = acccountBoolAnswer;
            Session["MulAnswer"] = acccountMulAnswer;
            //save data in quizRecord table
            quizRecord.AccountId = accountId;
            quizRecord.HighestGrade = mark;
            quizRecord.QuizId = quiz.QuizDescription.QuizId;
            quizService.SaveQuizRecord(quizRecord);

            //save data in Quiz_Account_Answer table

            return RedirectToAction("QuizResult");
        }

        public ActionResult QuizResult()
        {

            return View();
        }

        public ActionResult QuizResultContinue(int id = 0)
        {
            return View("ContinueStudy");
        }

    }
}
