using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class ModuleAccessDetail
    {
        //moduleId, accountId, trainingAccess, quizTransfe
        private Int32 moduleId;
        //private string moduleName;
        private List<TrainingAccess> training = new List<TrainingAccess>();
        private List<QuizAccess> quiz = new List<QuizAccess>();

        [DataMember]
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        //[DataMember]
        //public string ModuleName
        //{
        //    get { return moduleName; }
        //    set { moduleName = value; }
        //}

        [DataMember]
        public List<TrainingAccess> Training
        {
            get { return training; }
            set { training = value; }
        }

        [DataMember]
        public List<QuizAccess> Quiz
        {
            get { return quiz; }
            set { quiz = value; }
        }
    }
}
