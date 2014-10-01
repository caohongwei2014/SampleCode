using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class AgreementInfoAccess
    {
        //userType VARCHAR(255) NOT NULL,
    //  content VARCHAR(2048),
    //  moduleId INTEGER,
        private Int32 moduleId;
        private string userType;
        private string content;

        [DataMember]
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        [DataMember]
        public string UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        [DataMember]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
}
