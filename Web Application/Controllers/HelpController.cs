using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingRegistrationForConestoga.Controllers
{
    public class HelpController : Controller
    {
        //
        // GET: /Help/

        public FileStreamResult Administrator()
        {           
            
            FileStream fs = new FileStream(Server.MapPath(@"~\HTMLhelpfiles\TrainingModuleRegistrationHelpFile.html"), FileMode.Open, FileAccess.Read);
            return File(fs, "text/html");
            //return View();
        }

    }
}
