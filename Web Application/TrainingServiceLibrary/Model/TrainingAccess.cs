using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class TrainingAccess
    {
      //  trainingId INTEGER  NOT NULL IDENTITY,
      //trainingName VARCHAR(512),
      //description VARCHAR(512),
      //type CHAR(255),  
      //moduleId INTEGER
        private Int32 trainingId;
        private Int32 moduleId;
        private string trainingName;
        private string type;
        private string description;

        [DataMember]
        public Int32 TrainingId
        {
            get { return trainingId; }
            set { trainingId = value; }
        }

        [DataMember]
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        [DataMember]
        public string TrainingName
        {
            get { return trainingName; }
            set { trainingName = value; }
        }

        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
