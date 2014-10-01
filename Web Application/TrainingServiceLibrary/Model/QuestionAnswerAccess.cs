using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class QuestionAnswerAccess
    {
      //  questionAnswerId INTEGER  NOT NULL IDENTITY,
      //quizId INTEGER NOT NULL,
      //questionDescription VARCHAR(255),
      //type CHAR(255),
      //Answer CHAR(255)
        private Int32 questionAnswerId;
        private Int32 quizId;
        private string type;
        private string questionDescription;
        private string answer;

        [DataMember]
        public Int32 QuestionAnswerId
        {
            get { return questionAnswerId; }
            set { questionAnswerId = value; }
        }

        [DataMember]
        public Int32 QuizId
        {
            get { return quizId; }
            set { quizId = value; }
        }

        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        [DataMember]
        [Display(Name = "Question description")]
        public string QuestionDescription
        {
            get { return questionDescription; }
            set { questionDescription = value; }
        }

        [DataMember]
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
    }
}
