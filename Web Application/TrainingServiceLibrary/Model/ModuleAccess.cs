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
    public class ModuleAccess
    {
  //      moduleId INTEGER NOT NULL IDENTITY,
  //name VARCHAR(255),
  //description VARCHAR(255),  
  //expiryDate DATETIME,
  //creationDate DATETIME
        private Int32 moduleId;
        private string moduleName;
        private string description;
        private DateTime expiryDate;
        private DateTime creationDate;

        [DataMember]
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        [DataMember]
        [Display(Name = "Module name")]
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        public DateTime ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

    }
}
