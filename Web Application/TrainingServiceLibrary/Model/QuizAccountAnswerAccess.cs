using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class QuizAccountAnswerAccess
    {
        private Int32 qAccountAnswerId;
        private Int32 questionAnswerId;
        private Int32 accountId;
        private string answer;
        private DateTime submitDate;

        [DataMember]
        public Int32 QAccountAnswerId
        {
            get { return qAccountAnswerId; }
            set { qAccountAnswerId = value; }
        }

        [DataMember]
        public Int32 QuestionAnswerId
        {
            get { return questionAnswerId; }
            set { questionAnswerId = value; }
        }

        [DataMember]
        public Int32 AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        [DataMember]
        public DateTime SubmitDate
        {
            get { return submitDate; }
            set { submitDate = value; }
        }

        [DataMember]
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
    }
}
