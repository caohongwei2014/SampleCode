using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class EmployeeReportTransfer
    {
        //name: (first name + last name)
        //moduleList:(name, taking time)
        private Int32 personId;
        private string employeeName;
        private List<ModuleDetailTransfer> moduleList = new List<ModuleDetailTransfer>();

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
        public List<ModuleDetailTransfer> ModuleList
        {
            get { return moduleList; }
            set { moduleList = value; }
        }
    }
}
