using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class QuizAccess
    {
      //  quizId INTEGER  NOT NULL IDENTITY, 
      //quizName VARCHAR(512),
      //description VARCHAR(512),
      //passingGrade INTEGER,
      //moduleId INTEGER, 
        private Int32 quizId;
        private Int32 moduleId;
        private Int32 passingGrade;
        private string quizName;
        private string description;

        [DataMember]
        public Int32 QuizId
        {
            get { return quizId; }
            set { quizId = value; }
        }

        [DataMember]
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        [DataMember]
        [Display(Name = "Passing grade")]
        public Int32 PassingGrade
        {
            get { return passingGrade; }
            set { passingGrade = value; }
        }

        [DataMember]
        [Display(Name = "Quiz name")]
        public string QuizName
        {
            get { return quizName; }
            set { quizName = value; }
        }

        [DataMember]
        [Display(Name = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
