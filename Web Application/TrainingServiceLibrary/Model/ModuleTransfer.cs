using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class ModuleTransfer
    {
        //moduleId, accountId, trainingAccess, quizTransfe
        private Int32 moduleId;
        private string moduleName;
        private Int32 accountId;
        private string accountName;
        private List<TrainingTransfer> training = new List<TrainingTransfer>();
        private List<QuizTransfer> quiz = new List<QuizTransfer>();

        [DataMember]
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        [DataMember]
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }

        [DataMember]
        public Int32 AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        [DataMember]
        public string AccountName
        {
            get { return accountName; }
            set { accountName = value; }
        }

        [DataMember]
        public List<TrainingTransfer> Training
        {
            get { return training; }
            set { training = value; }
        }

        [DataMember]
        public List<QuizTransfer> Quiz
        {
            get { return quiz; }
            set { quiz = value; }
        }
    }
}
