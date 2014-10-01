using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class QuizRecordAccess
    {
      //  quizRecordId INTEGER NOT NULL,
      //accountId INTEGER NOT NULL,
      //attendTimes INTEGER,
      //highGradeTime DATETIME,
      //quizId INTEGER NOT NULL,
      //highestGrade INTEGER,
        private Int32 quizRecordId;
        private Int32 accountId;
        private Int32 quizId;
        private Int32 highestGrade;
        private DateTime attendTimes;
        private DateTime highGradeTime;

        [DataMember]
        public Int32 QuizRecordId
        {
            get { return quizRecordId; }
            set { quizRecordId = value; }
        }

        [DataMember]
        public Int32 AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        [DataMember]
        public Int32 QuizId
        {
            get { return quizId; }
            set { quizId = value; }
        }

        [DataMember]
        public Int32 HighestGrade
        {
            get { return highestGrade; }
            set { highestGrade = value; }
        }

       [DataMember]
        public DateTime AttendTimes
        {
            get { return attendTimes; }
            set { attendTimes = value; }
        }

        [DataMember]
       public DateTime HighGradeTime
        {
            get { return highGradeTime; }
            set { highGradeTime = value; }
        }
    }
}
