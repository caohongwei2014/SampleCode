using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingServiceLibrary;
using System.IO;


namespace TrainingRegistrationForConestoga.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DocumentController : Controller
    {
        TrainingService trainingService = new TrainingService();
        DocumentService documentService = new DocumentService();
        List<DocumentAccess> documents = new List<DocumentAccess>();
        DocumentAccess document = new DocumentAccess();
        List<TrainingAccess> trainings = new List<TrainingAccess>();

        //List all documents with training.
        // GET: /Document/

        public ActionResult Index()
        {
            var searchInput = Request.Form["search"];
            if (searchInput != null && searchInput.Trim() != "")
            {
                documents = documentService.Search(searchInput);
                ViewBag.documents = documents;
                Session.Add("documents", documents);
            }
            else
            {
                documents = documentService.Search();
                ViewBag.documents = documents;
                Session.Add("documents", documents);
            }
            return View();
        }
        //List of documents without training.
        public ActionResult DocumentWithoutTraining()
        {
            documents = documentService.GetDocumentData();
            List<DocumentAccess> docList = new List<DocumentAccess>();
            var searchInput = Request.Form["search"];
            if (searchInput != null && searchInput.Trim() != "")
            {
              foreach (var item in documents)
                {
                    if (item.DocumentName.Contains(searchInput)&& item.TrainingId==0)
                    {
                        docList.Add(item);
                    }                
                }
                ViewBag.documents = docList;
                Session.Add("documents", documents);
            }
            else
            {
                foreach (var item in documents)
                {
                    if (item.TrainingId == 0)
                    {
                        docList.Add(item);
                    }
                }
                ViewBag.documents = docList;
                Session.Add("documents", documents);
            }
            return View();
        }

        //Add new document
        public ActionResult AddDocument()
        {

            trainings = trainingService.GetTrainingData();

            ViewBag.trainings = new SelectList(trainings, "trainingId", "trainingName");

            SelectListItem PDF = new SelectListItem() { Text = "PDF File", Value = "PDF" };
            SelectListItem PPT = new SelectListItem() { Text = "PPT File", Value = "PPT" };
            SelectListItem DOC = new SelectListItem() { Text = "Word", Value = "DOC" };
            SelectListItem EXCEL = new SelectListItem() { Text = "EXCEL", Value = "EXCEL" };

            ViewBag.fileType = new SelectList(new SelectListItem[]{PDF,PPT,DOC,EXCEL},"Value","Text");
            

            return View();
        }
        //Upload the document and save the information.
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var type = Request.Form["fileType"];
            var training=Request.Form["trainings"];

            var description = Request.Form["description"];
            string newName =Request.Form["newFileName"];
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    
                    if (newName != null && newName.Trim() != "")
                    {
                        String[] nameAndType=fileName.Split('.');
                        fileName = newName + "." + nameAndType.Last();
                    }

                    var path = Path.Combine(Server.MapPath("~/App_Data/File"), fileName);
                    FileInfo fileInfo = new FileInfo(path);
                    if (!fileInfo.Exists)
                    {
                        file.SaveAs(path);
                    }
                    else 
                    {
                        TempData["message"] = "There is a file named: '"+fileName+"' has existed. Please change the file name and try to upload again!";
                        return RedirectToAction("AddDocument");
                    }
                    newName = fileName;
                    
                    document.Size = file.ContentLength;
                    document.DocumentName = newName;
                    document.Type = type;
                    if (training == null || training.Trim() == "")
                    {
                        document.TrainingId = 0;
                    }
                    else 
                    {
                        document.TrainingId =Convert.ToInt32(training);
                    }                    
                    document.Description = description;
                    documentService.Add(document);
                }
                ViewBag.Message = "Upload successful";
               return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("Index");
            }
        }
        //Edit document infomation.
        public ActionResult Edit(int id=0)
        {
            if (id == 0) {
                TempData["message"] = "Please select an entry.";
                return RedirectToAction("Index");
            }
            SelectListItem PDF = new SelectListItem() { Text = "PDF File", Value = "PDF" };
            SelectListItem PPT = new SelectListItem() { Text = "PPT File", Value = "PPT" };
            SelectListItem DOC = new SelectListItem() { Text = "Word", Value = "DOC" };
            SelectListItem EXCEL = new SelectListItem() { Text = "EXCEL", Value = "EXCEL" };
                    
            documents =(List<DocumentAccess>)Session["documents"];
            foreach (var item in documents)
            {
                if (item.DocumentId == id) {
                    document = item; 
                    Session.Add("document", document);
                }            
            }

            trainings = trainingService.GetTrainingData();
            ViewBag.trainings = new SelectList(trainings, "trainingId", "trainingName", document.TrainingId);
            ViewBag.fileType = new SelectList(new SelectListItem[] { PDF, PPT, DOC, EXCEL }, "Value", "Text",document.Type);
            
            ViewBag.document = document;
            return View();
        }
        //Save updated document information.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            document = (DocumentAccess)Session["document"];            
            document.Type = Request.Form["fileType"];
            if (Request.Form["trainings"] == "")
            {
                document.TrainingId = 0;
            }
            else { 
                document.TrainingId =Convert.ToInt32( Request.Form["trainings"]);
            }
            document.Description = Request.Form["description"];
            if (documentService.Update(document))
            {
                if (document.TrainingId == 0)
                {
                    return RedirectToAction("DocumentWithoutTraining");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }            
            return View("Edit");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditDocumentWithoutTraining()
        //{
        //    document = (DocumentAccess)Session["document"];
        //    document.Type = Request.Form["fileType"];
        //    if (Request.Form["trainings"] == "")
        //    {
        //        document.TrainingId = 0;
        //    }
        //    else
        //    {
        //        document.TrainingId = Convert.ToInt32(Request.Form["trainings"]);
        //    }
        //    document.Description = Request.Form["description"];
        //    if (documentService.Update(document))
        //    {
        //        return RedirectToAction("DocumentWithoutTraining");
        //    }
        //    return View("Edit");
        //}

        ////Delete a document.
        public ActionResult Delete(int id = 0, string documentName = null)
        {
            if (id == 0)
            {
                TempData["message"] = "Please select an entry.";
                return RedirectToAction("Index");
            }
            documents = (List<DocumentAccess>)Session["documents"];
            foreach (var item in documents)
            {
                if (item.DocumentId == id)
                {
                    document = item;
                    Session.Add("document", document);
                }
            }
            var path = Path.Combine(Server.MapPath("~/App_Data/File"), documentName);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                try
                {
                    file.Delete();
                    List<int> idList = new List<int>();
                    idList.Add(document.DocumentId);
                    documentService.Delete(idList);
                    TempData["message"] = "Document has been deleted";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["message"] = "Failed to delete the document " + documentName + " . Error: " + ex.GetBaseException().Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["message"] = "Can not find the document.";
                return RedirectToAction("Index");
            }

        }
        //Delete a document which doesn't belonge to any training.
        public ActionResult DeleteFromWithoutTrainingList(int id = 0, string documentName=null)
        {
            if (id == 0)
            {
                TempData["message"] = "Please select an entry.";
                return RedirectToAction("Index");
            }
            documents = (List<DocumentAccess>)Session["documents"];
            foreach (var item in documents)
            {
                if (item.DocumentId == id)
                {
                    document = item;
                    Session.Add("document", document);
                }
            }
            var path = Path.Combine(Server.MapPath("~/App_Data/File"), documentName);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                try
                {
                    file.Delete();
                    List<int> idList = new List<int>();
                    idList.Add(document.DocumentId);
                    documentService.Delete(idList);
                    TempData["message"] = "Document has been deleted";
                    return RedirectToAction("DocumentWithoutTraining");
                }
                catch (Exception ex)
                {
                    TempData["message"] = "Failed to delete the document " + documentName + " . Error: " + ex.GetBaseException().Message;
                    return RedirectToAction("DocumentWithoutTraining");
                }
            }
            else
            {
                TempData["message"] = "Can not find the document.";
                return RedirectToAction("DocumentWithoutTraining");
            }   
        }
      //Download document.
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
              TempData["message"] = "The file can not be found.";
              return RedirectToAction("Index");
          } 
       }
          
    }
}
