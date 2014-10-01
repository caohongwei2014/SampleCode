using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingServiceLibrary;

namespace TrainingRegistrationForConestoga.Controllers
{
    public class QuizController : Controller
    {
        //
        // GET: /Quiz/

        public ActionResult Index()
        {
            List<QuizTransfer> quizList = new List<QuizTransfer>();
            QuizService quizService = new QuizService();
            quizList = quizService.GetQuiz();
            Session["quizList"] = quizList;
            //ViewBag.quizList = quizList;
            return View();
        }

        public ActionResult Create()
        {
            ModuleService moduleService = new ModuleService();
            List<ModuleAccess> moduleList = moduleService.GetModuleData();
            ViewBag.moduleId = new SelectList(moduleList, "moduleId", "moduleName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuizAccess quiz)
        {
            QuizTransfer quizModule = new QuizTransfer();
            QuizAccess quizDe = new QuizAccess();
            try
            {
                if (Request.Form.Get("moduleId") != null)
                {
                    quiz.ModuleId = Convert.ToInt32(Request.Form.Get("moduleId"));
                }
                quizDe = quiz;
                quizModule.QuizDescription = quizDe;
                Session.Add("quiz", quizModule);
                return RedirectToAction("question");
            }
            catch (Exception ex)
            {
                //get the innermost exception
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                ModelState.AddModelError("", "error on insert: " + ex.GetBaseException().Message);
                return View();
            }


        }

        public ActionResult question()
        {
            SelectListItem bl = new SelectListItem() { Text = "TrueOrFalse", Value = "bool" };
            SelectListItem mul = new SelectListItem() { Text = "Multiselect ", Value = "mul" };

            ViewBag.questionType = new SelectList(new SelectListItem[] { bl, mul }, "Value", "Text");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult question(QuestionAnswerAccess question)
        {
            QuizTransfer quizModule = new QuizTransfer();
            QuestionTransfer mulQuestion = new QuestionTransfer();
            //List<QuestionAnswerAccess> boolQuestionList = new List<QuestionAnswerAccess>();
            //List<QuestionTransfer> mulQuestionList = new List<QuestionTransfer>();

            try
            {
                quizModule = (QuizTransfer)Session["quiz"];
                if (Request.Form.Get("questionType") != null)
                {
                    question.Type = Request.Form.Get("questionType");
                }

                if (Request.Form.Get("answer") != null)
                {
                    question.Answer = Request.Form.Get("answer");
                }
                //questionDe = question;
                if (question.Type == "bool")
                {
                    quizModule.BoolQuestion.Add(question);
                    Session.Add("quiz", quizModule);
                    return RedirectToAction("question");
                }
                else
                {
                    mulQuestion.QuestionAnswer = question;
                    quizModule.MulSelQuestion.Add(mulQuestion);
                    Session.Add("quiz", quizModule);
                    return RedirectToAction("SelectionItem");
                }
            }
            catch (Exception ex)
            {
                //get the innermost exception
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                ModelState.AddModelError("", "error on add quiz: " + ex.GetBaseException().Message);
                Session.Add("quiz", "");
                return View("Index");
            }
        }

        public ActionResult SelectionItem()
        {
            QuizTransfer quizModule = new QuizTransfer();
            List<QuestionSelectItem> itemList = new List<QuestionSelectItem>();

            quizModule = (QuizTransfer)Session["quiz"];
            itemList = quizModule.MulSelQuestion.Last().SelectionItems;
            Session["mulQuestionDes"] = quizModule.MulSelQuestion.Last().QuestionAnswer.QuestionDescription;
            Session["mulQuestionAnswer"] = quizModule.MulSelQuestion.Last().QuestionAnswer.Answer;
            ViewBag.itemList = itemList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectionItem(QuestionSelectItem item)
        {
            QuizTransfer quizModule = new QuizTransfer();
            int number = 0;
            quizModule = (QuizTransfer)Session["quiz"];
            quizModule.MulSelQuestion.Last().SelectionItems.Add(item);
            number = quizModule.MulSelQuestion.Last().ItemsNumber;
            number++;
            quizModule.MulSelQuestion.Last().ItemsNumber = number;
            Session.Add("quiz", quizModule);
            return RedirectToAction("SelectionItem");
        }


        /// <summary>
        /// When customer click finish button, add this quiz to database, then display all quiz
        /// </summary>
        /// <returns></returns>
        public ActionResult AddQuiz()
        {
            IQuizService quizService = new QuizService();
            QuizTransfer quizModule = new QuizTransfer();
            quizModule = (QuizTransfer)Session["quiz"];
            quizModule.BoolQuestionNumber = quizModule.BoolQuestion.Count;
            quizModule.MulSelQuestionNumber = quizModule.MulSelQuestion.Count;
            quizService.AddQuiz(quizModule);

            TempData["message"] = "Quiz " + quizModule.QuizDescription.QuizName + " has been added successfully.";
            return RedirectToAction("Index");
        }

        public ActionResult Cancel()
        {

            return View("Index");
        }

        /// <summary>
        /// Create a view to display the quiz that customers want to modify
        /// </summary>
        /// <returns></returns>
        public ActionResult Update(int quizId, int moduleId)
        {
            List<QuizTransfer> quizList = (List<QuizTransfer>)Session["quizList"];
            QuizTransfer quiz = new QuizTransfer();
            ModuleService moduleService = new ModuleService();
            List<ModuleAccess> moduleList = moduleService.GetModuleData();
            foreach (var item in quizList)
            {
                if (quizId == item.QuizDescription.QuizId)
                {
                    quiz = item;
                    Session["quiz"] = quiz;
                    break;
                }
            }

            ViewBag.moduleId = new SelectList(moduleList, "moduleId", "moduleName", moduleId);
            return View(quiz);
        }

        /// <summary>
        /// Save data that customers change into database
        /// </summary>
        /// <param name="quiz"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(QuizTransfer quiz)
        {
            IQuizService quizService = new QuizService();
            QuizTransfer quiz1 = (QuizTransfer)Session["quiz"];

            //modify quiz
            if (Request.Form.Get("quizName") != null)
            {
                quiz1.QuizDescription.QuizName = Request.Form.Get("quizName");
            }

            if (Request.Form.Get("quizDescription") != null)
            {
                quiz1.QuizDescription.Description = Request.Form.Get("quizDescription");
            }

            if (Request.Form.Get("passingGrade") != null)
            {
                quiz1.QuizDescription.PassingGrade = Convert.ToInt32(Request.Form.Get("passingGrade"));
            }

            if (Request.Form.Get("moduleId") != null)
            {
                quiz1.QuizDescription.ModuleId = Convert.ToInt32(Request.Form.Get("moduleId"));
            }

            //modify bool question
            for (int i = 0; i < quiz1.BoolQuestionNumber; i++)
            {
                if (Request.Form.Get(i + "boolQuestionDescription") != null)
                {
                    quiz1.BoolQuestion.ElementAt(i).QuestionDescription = Request.Form.Get(i + "boolQuestionDescription");
                }
                if (Request.Form.Get(i + "boolAnswer") != null)
                {
                    quiz1.BoolQuestion.ElementAt(i).Answer = Request.Form.Get(i + "boolAnswer");
                }
            }

            //modify multiselect question
            for (int j = 0; j < quiz1.MulSelQuestionNumber; j++)
            {
                if (Request.Form.Get(j + "mulSelQuestionDescription") != null)
                {
                    quiz1.MulSelQuestion.ElementAt(j).QuestionAnswer.QuestionDescription = Request.Form.Get(j + "mulSelQuestionDescription");
                }
                if (Request.Form.Get(j + "mulSelQuestionAnswer") != null)
                {
                    quiz1.MulSelQuestion.ElementAt(j).QuestionAnswer.Answer = Request.Form.Get(j + "mulSelQuestionAnswer");
                }

                //modify items of multiselect question
                for (int k = 0; k < quiz1.MulSelQuestion.ElementAt(j).ItemsNumber; k++)
                {
                    if (Request.Form.Get(j.ToString() + k.ToString() + "choiceNumber") != null)
                    {
                        quiz1.MulSelQuestion.ElementAt(j).SelectionItems.ElementAt(k).ChoiceSequenceNumber = Request.Form.Get(j.ToString() + k.ToString() + "choiceNumber");
                    }

                    if (Request.Form.Get(j.ToString() + k.ToString() + "choiceDescription") != null)
                    {
                        quiz1.MulSelQuestion.ElementAt(j).SelectionItems.ElementAt(k).ChoiceDescription = Request.Form.Get(j.ToString() + k.ToString() + "choiceDescription");
                    }
                }
            }
            quizService.UpdateQuiz(quiz1);
            TempData["message"] = "Quiz " + quiz1.QuizDescription.QuizName + " has been updated successfully.";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int quizId, int moduleId)
        {
            List<QuizTransfer> quizList = (List<QuizTransfer>)Session["quizList"];
            QuizTransfer quiz = new QuizTransfer();
            ModuleService moduleService = new ModuleService();
            List<ModuleAccess> moduleList = moduleService.GetModuleData();
            foreach (var item in quizList)
            {
                if (quizId == item.QuizDescription.QuizId)
                {
                    quiz = item;
                    Session["quiz"] = quiz;
                    break;
                }
            }

            ViewBag.moduleId = new SelectList(moduleList, "moduleId", "moduleName", moduleId);
            return View(quiz);
        }

        public ActionResult DeleteQuiz(int id)
        {
            IQuizService quizService = new QuizService();
            List<int> idList = new List<int>();
            idList.Add(id);
            quizService.Delete(idList);
            TempData["message"] = "Quiz " + ((QuizTransfer)Session["quiz"]).QuizDescription.QuizName + " has been deleted successfully.";
            return RedirectToAction("Index");
        }

    }
}
