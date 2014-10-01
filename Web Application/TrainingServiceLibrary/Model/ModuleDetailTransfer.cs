using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class ModuleDetailTransfer
    {
        private Int32 moduleId;
        private string moduleName;
        private DateTime trainingDate;

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
        public DateTime TrainingDate
        {
            get { return trainingDate; }
            set { trainingDate = value; }
        }
    }
}
