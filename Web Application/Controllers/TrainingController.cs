using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingServiceLibrary;

namespace TrainingRegistrationForConestoga.Controllers
{
    public class TrainingController : Controller
    {
        //List all trainings.
        // GET: /Training/

        public ActionResult Index()
        {  
            TrainingService trainingService = new TrainingService();
            List<TrainingAccess> trainings = new List<TrainingAccess>();
            List<TrainingAccess> trainingsSelected = new List<TrainingAccess>();
            var searchInput = Request.Form["search"];
            if (searchInput != null && searchInput.Trim() != "")
            {
                trainings = trainingService.GetTrainingData();
                foreach (var item in trainings)
                {
                    if (item.TrainingName.Contains(searchInput))
                    { 
                      trainingsSelected.Add(item);
                    }                  
                }
                ViewBag.trainings = trainingsSelected;
            }
            else
            {
                trainings = trainingService.GetTrainingData();
                ViewBag.trainings = trainings;
            } 
            return View();
        }

        //Create a new training.
        public ActionResult Create()
        {
            TrainingService trainingService = new TrainingService();
            TrainingAccess training = new TrainingAccess();
            ModuleService moduleService = new ModuleService();
            List<ModuleAccess> modules = new List<ModuleAccess>();
            modules= moduleService.GetModuleData();
            ViewBag.modules = new SelectList(modules, "moduleId", "moduleName");

            SelectListItem allStaff = new SelectListItem() { Text = "Open for all", Value = "allStaff" };
            SelectListItem  manager= new SelectListItem() { Text = "Managers only", Value = "manager" };
            SelectListItem newEmployee = new SelectListItem() { Text = "New staff only", Value = "newStaff" };

            ViewBag.Type = new SelectList(new SelectListItem[] { allStaff, manager, newEmployee }, "Value", "Text");

            return View(training);
        }

        //Save new module.
        [HttpPost]
        public ActionResult Create(TrainingAccess training)
        {
            if (Request.Form["modules"] == "")
            {
                training.ModuleId = 0;
            }
            else { 
                training.ModuleId=Convert.ToInt32(Request.Form["modules"]);            
            }

            TrainingService trainingService = new TrainingService();
            TrainingAccess train=training;
            try
            {
                if (trainingService.Add(train))
                {
                    TempData["message"] = "New training has been created.";
                    return RedirectToAction("Index");
                }
                TempData["message"] = "Failed to created new training.";
                return View("Create");
            }
            catch (Exception ex)
            {
                TempData["message"] = "Failed to created new training. Eorror: " + ex.GetBaseException().Message;
                return View("Create");
            }
       }
        //Show detail of a training.
        public ActionResult Detail(int id=0)
        {
            ModuleService moduleService = new ModuleService();
            TrainingService trainingService = new TrainingService();            
            TrainingTransfer training=new TrainingTransfer();            
            if (id != 0) 
            {
                Session.Add("trainingId", id);
                training = trainingService.GeTrainingDetailData(id);
                ViewBag.training = training;

                List<ModuleAccess> modules = new List<ModuleAccess>();
                modules = moduleService.GetModuleData();
                foreach (var item in modules)
                {
                    if (item.ModuleId == training.Training.ModuleId)
                    {
                        ViewBag.moduleName = item.ModuleName;
                    }
                }                
            }
            else if (Session["trainingId"] != null)
            {
                if (Session["trainingId"] != null)
                {
                    id = Convert.ToInt32(Session["trainingId"]);
                }
                else {
                    TempData["message"] = "Please select an training";
                }
                training = trainingService.GeTrainingDetailData(id);
                ViewBag.training = training;
                List<ModuleAccess> modules = new List<ModuleAccess>();
                modules = moduleService.GetModuleData();
                foreach (var item in modules)
                {
                    if (item.ModuleId == training.Training.ModuleId)
                    {
                        ViewBag.moduleName = item.ModuleName;
                    }
                }
            }
            else {
                TempData["message"] = "Please select an entry.";            
            }
            return View();
        }
        //Delete a training.
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                TempData["message"] = "Please select an entry";
                return RedirectToAction("Index");
            }
            TrainingService trainingService = new TrainingService();
            List<int> idList = new List<int>();
            idList.Add(id);
            if (trainingService.Delete(idList))
            {
                TempData["message"] = "The training(s) has been deleted.";
            }
            else
            {
                TempData["message"] = "Failed to delete the traing No." + id;
            }
            return RedirectToAction("Index");
        }
        //Update a training.
        public ActionResult Update(int id = 0)
        {
            TrainingService trainingService = new TrainingService();
            List<TrainingAccess> trainings = new List<TrainingAccess>();
            TrainingAccess training = new TrainingAccess();
            List<ModuleAccess> modules = new List<ModuleAccess>();
            ModuleService moduleService = new ModuleService();
            modules = moduleService.GetModuleData();

            if (id != 0)
            {
                Session.Add("trainingId", id);
                trainings = trainingService.GetTrainingData();
                foreach (var item in trainings)
                {
                    if (item.TrainingId == id)
                    {
                        ViewBag.training = item;
                        training = item;
                        ViewBag.modules = new SelectList(modules, "moduleId", "moduleName", training.ModuleId);

                        SelectListItem allStaff = new SelectListItem() { Text = "Open for all", Value = "allStaff" };
                        SelectListItem manager = new SelectListItem() { Text = "Managers only", Value = "manager" };
                        SelectListItem newEmployee = new SelectListItem() { Text = "New staff only", Value = "newStaff" };
                        ViewBag.Type = new SelectList(new SelectListItem[] { allStaff, manager, newEmployee }, "Value", "Text",item.Type);
                    }
                }
            }
            else if(Session["trainingId"]!=null) { 
                id=Convert.ToInt32(Session["trainingId"]);
                trainings = trainingService.GetTrainingData();
                foreach (var item in trainings)
                {
                    if (item.TrainingId == id)
                    {
                        ViewBag.training = item;
                        training = item;
                        ViewBag.modules = new SelectList(modules, "moduleId", "moduleName", training.ModuleId);

                        SelectListItem allStaff = new SelectListItem() { Text = "Open for all", Value = "allStaff" };
                        SelectListItem manager = new SelectListItem() { Text = "Managers only", Value = "manager" };
                        SelectListItem newEmployee = new SelectListItem() { Text = "New staff only", Value = "newStaff" };
                        ViewBag.Type = new SelectList(new SelectListItem[] { allStaff, manager, newEmployee }, "Value", "Text",item.Type);
                    }
                }
            }else
            {
                TempData["message"] = "Please selete a training.";
            }
            return View(training);        
        }

        //Save updated training.
        [HttpPost]
        public ActionResult Update(TrainingAccess training)
        {
            training.ModuleId = Convert.ToInt32(Request.Form["modules"]);
            training.Type=Request.Form["Type"];
            training.TrainingId = Convert.ToInt32(Request.Form["TrainingId"]);
            TrainingService trainingService = new TrainingService();
            TrainingAccess train = training;
            trainingService.Update(training);
            return RedirectToAction("Index");
        }
    }
}
