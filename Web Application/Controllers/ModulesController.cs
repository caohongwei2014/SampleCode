using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingServiceLibrary;
using System.IO;

namespace TrainingRegistrationForConestoga.Controllers
{
    public class ModulesController : Controller
    {
        //List of modules.
        // GET: /Modules/

        public ActionResult Index(string key=null)
        {
            ModuleService moduleService = new ModuleService();
            List<ModuleAccess> modules = new List<ModuleAccess>();
            key = Request.Form["search"];
            modules = moduleService.GetModuleData(key);
            if (key != null && key.Trim() != "")
            {
                modules = moduleService.GetModuleData(key);                
            }
            else
            {
                modules = moduleService.GetModuleData(key);   
            }
            
            return View(modules);
        }
        //Create a new module.
        public ActionResult Create()
        { 
      
            ModuleAccess module= new ModuleAccess();
            AgreementInfoAccess agreement = new AgreementInfoAccess();

            SelectListItem administrator = new SelectListItem() { Text = "Administrator", Value = "Administrator" };
            SelectListItem manager = new SelectListItem() { Text = "Manager", Value = "Manager" };
            SelectListItem regular = new SelectListItem() { Text = "RegularUser", Value = "RegularUser" };
            ViewBag.userType = new SelectList(new SelectListItem[] { administrator, manager, regular }, "Value", "Text", "RegularUser");

            return View();
        }
        //Save module information.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModule()
        {
            var moduleName = Request.Form["ModuleName"];
            var description = Request.Form["Description"];
            var expiryDate = Request.Form["ExpiryDate"];
            var userType = Request.Form["userType"];
            var content = Request.Form["AgreementContent"];

            ModuleService moduleService = new ModuleService();
            ModuleAccess module = new ModuleAccess();
            AgreementInfoAccess agreement = new AgreementInfoAccess();
            module.ModuleName = moduleName;
            module.Description = description;
            if (expiryDate != "")
            {
                module.ExpiryDate = Convert.ToDateTime(expiryDate);
            }
            else {
                module.ExpiryDate = DateTime.Now;
            }
            if (userType == "")
            {
                agreement.UserType = "RegularUser";
            }else
            {
                agreement.UserType = userType;
            }
            agreement.Content = content;
            if (moduleService.AddModule(module, agreement))
            { 
                TempData["message"] = "Module has been created";
                return RedirectToAction("Index");
            }
            TempData["message"] = "Module can not be created, Please check your input again.";
            return View("Create");
        }
        //Show module's detail information.
        public ActionResult Detail(int id=0,string moduleName=null)
        {
            ModuleService moduleService = new ModuleService();
            //AgreementInfoAccess agreement = new AgreementInfoAccess();
            if (id != 0 && moduleName != null)
            {
                ViewBag.moduleName = moduleName;
                ModuleAccessDetail moduleDetail = new ModuleAccessDetail();
                List<TrainingAccess> trainingList = new List<TrainingAccess>();
                List<QuizAccess> quizList = new List<QuizAccess>();
                moduleDetail.Training = trainingList;
                moduleDetail.Quiz = quizList;
                moduleDetail = moduleService.GetModuleDetailData(id);
                ViewBag.moduleDetail = moduleDetail;
                return View();
            }
            TempData["message"] = "Please select a module.";
            return RedirectToAction("Index");
        }
        //Update module's information.
        public ActionResult Update(int id=0,string key=null)
        {
             ModuleService moduleService = new ModuleService();
            
            AgreementInfoAccess agreement = new AgreementInfoAccess();
            ModuleAccess module = new ModuleAccess();

            SelectListItem administrator = new SelectListItem() { Text = "Administrator", Value = "Administrator" };
            SelectListItem manager = new SelectListItem() { Text = "Manager", Value = "Manager" };
            SelectListItem regular = new SelectListItem() { Text = "RegularUser", Value = "RegularUser" };                        
            if (id != 0) 
            {             
             var modules = moduleService.GetModuleData(key);
             foreach (var item in modules) 
             {
                 if (item.ModuleId == id) module = item;
             }
             var agree = moduleService.ShowAgreement(id);
             ViewBag.agreement = agree;
             ViewBag.module = module;
             ViewBag.userType = new SelectList(new SelectListItem[] { administrator, manager, regular }, "Value", "Text", agree.UserType);
             Session.Add("moduleId",id);
            }           
           
            return View();
        }

        //Save updated module.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateModule()
        {            
            ModuleService moduleService = new ModuleService();
            AgreementInfoAccess agreement = new AgreementInfoAccess();
            ModuleAccess module = new ModuleAccess();

            var moduleName = Request.Form["ModuleName"];
            var description = Request.Form["Description"];
            var expiryDate = Request.Form["ExpiryDate"];
            var userType = Request.Form["userType"];
            var content = Request.Form["AgreementContent"];
            var moduleId = Convert.ToInt32(Session["moduleId"]);            
            string key = null;
            var modules = moduleService.GetModuleData(key);

            foreach (var item in modules)
            {
                if (item.ModuleId == moduleId) module = item;
            }
            agreement = moduleService.ShowAgreement(moduleId);
            module.Description = description;
            string oleExpiryDate = module.ExpiryDate.ToShortDateString();
            DateTime newExpiryDate = Convert.ToDateTime(expiryDate);
            if (expiryDate !=oleExpiryDate )
            {
                if ( newExpiryDate.Year> DateTime.Now.Year)
                {
                    module.ExpiryDate = Convert.ToDateTime(expiryDate);
                }
                else if (newExpiryDate.Year == DateTime.Now.Year && newExpiryDate.Month > DateTime.Now.Month)
                {
                    module.ExpiryDate = Convert.ToDateTime(expiryDate);
                }
                else if (newExpiryDate.Year == DateTime.Now.Year && newExpiryDate.Month == DateTime.Now.Month && newExpiryDate.Day > DateTime.Now.Day)
                {
                    module.ExpiryDate = Convert.ToDateTime(expiryDate);
                }
                else 
                {
                    TempData["message"] = "Update failed with Module No."+module.ModuleId+ ", Error: Expiry date should be later than today'date.";
                    return RedirectToAction("Index");
                }
            }

            agreement.UserType = userType;
            agreement.Content = content;
            moduleService.UpdateModule(module, agreement);

            //if (moduleService.UpdateModule(module, agreement))
            //{
            //    return RedirectToAction("Index");
            //}
            return RedirectToAction("Index");
        }
        //Delete a module.
        public ActionResult Delete(int id = 0)
        {
            ModuleService moduleService = new ModuleService();
            if (id == 0)
            {
                TempData["message"] = "Please select an entry.";
                return RedirectToAction("Index");
            }            
            List<int> idList = new List<int>();
            idList.Add(id);
            moduleService.DeleteModule(idList);
            TempData["message"] = "Module has been deleted.";
            return RedirectToAction("Index");
        }

    }
}
