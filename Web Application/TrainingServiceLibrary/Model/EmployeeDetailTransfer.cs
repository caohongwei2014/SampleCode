using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class EmployeeDetailTransfer
    {
        //DETAIL:(employee name, department, training Date)
        private Int32 personId;
        private string employeeName;
        private string department;
        private DateTime trainingDate;

        [DataMember]
        public Int32 PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        [DataMember]
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }

        [DataMember]
        public string Department
        {
            get { return department; }
            set { department = value; }
        }

        [DataMember]
        public DateTime TrainingDate
        {
            get { return trainingDate; }
            set { trainingDate = value; }
        }
    }
}
