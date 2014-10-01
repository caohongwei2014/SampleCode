using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class ExceptionAccess
    {
        //exceptionId INTEGER  NOT NULL,  
        //  accountId INTEGER NOT NULL,
        //  moduleId INTEGER NOT NULL,
        //  reason VARCHAR(255),
        //  creationDate DATETIME,
        //  expiryDate DATETIME,
        private Int32 exceptionId;
        private Int32 accountId;
        private Int32 moduleId;
        private string reason;
        private DateTime creationDate;
        private DateTime expiryDate;

        [DataMember]
        public Int32 ExceptionId
        {
            get { return exceptionId; }
            set { exceptionId = value; }
        }

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
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        [DataMember]
        public DateTime ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }
    }
}
