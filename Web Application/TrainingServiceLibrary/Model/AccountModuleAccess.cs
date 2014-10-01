using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class AccountModuleAccess
    {
      //  accountId INTEGER NOT NULL,
      //moduleId INTEGER NOT NULL,
        //status VARCHAR(16),
        private Int32 accountId;
        private Int32 moduleId;
        private string status;

        [DataMember]
        public Int32 AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        [DataMember]
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }


        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

    }
}
