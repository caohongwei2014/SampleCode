using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class AdminViewReportTransfer
    {
        //moduleList:(name, number of people, DETAIL)
        //DETAIL:(employee name, department, training Date)
        private Int32 moduleId;
        private string moduleName;
        private Int32 numberOfPeople;
        private List<EmployeeDetailTransfer> detail = new List<EmployeeDetailTransfer>();

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
        public Int32 NumberOfPeople
        {
            get { return numberOfPeople; }
            set { numberOfPeople = value; }
        }

        [DataMember]
        public List<EmployeeDetailTransfer> Detail
        {
            get { return detail; }
            set { detail = value; }
        }

    }
}
