using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class AccountAccess
    {
      //  accountId INTEGER  NOT NULL IDENTITY,
      //userName CHAR(255),
      //password CHAR(255),
      //creationDate DATETIME,
      //lastLoginDate DATETIME,
      //personId INTEGER NOT NULL,
        private Int32 accountId;
        private Int32 personId;
        private string userName;
        private string password;
        private string userType;
        private DateTime creationDate;
        private DateTime lastLoginDate;

        [DataMember]
        public Int32 AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        [DataMember]
        public Int32 PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [DataMember]
        public string UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        [DataMember]
        public DateTime LastLoginDate
        {
            get { return lastLoginDate; }
            set { lastLoginDate = value; }
        }
    }
}
