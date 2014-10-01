using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class QuizTransfer
    {
        private Int32 mulSelQuestionNumber;
        private List<QuestionTransfer> mulSelQuestion = new List<QuestionTransfer>();
        private Int32 boolQuestionNumber;
        private List<QuestionAnswerAccess> boolQuestion = new List<QuestionAnswerAccess>();
        private QuizAccess quizDescription;

        [DataMember]
        public Int32 MulSelQuestionNumber
        {
            get { return mulSelQuestionNumber; }
            set { mulSelQuestionNumber = value; }
        }

        [DataMember]
        public List<QuestionTransfer> MulSelQuestion
        {
            get { return mulSelQuestion; }
            set { mulSelQuestion = value; }
        }

        [DataMember]
        public Int32 BoolQuestionNumber
        {
            get { return boolQuestionNumber; }
            set { boolQuestionNumber = value; }
        }

        [DataMember]
        public List<QuestionAnswerAccess> BoolQuestion
        {
            get { return boolQuestion; }
            set { boolQuestion = value; }
        }

        [DataMember]
        public QuizAccess QuizDescription
        {
            get { return quizDescription; }
            set { quizDescription = value; }
        }
    }
}
