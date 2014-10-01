using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class UserTransfer
    {
        private AccountAccess account;
        private PersonAccess person;
        private CampusAccess campus;
        private DepartmentAccess dempartment;

        [DataMember]
        public AccountAccess Account
        {
            get { return account; }
            set { account = value; }
        }

        [DataMember]
        public PersonAccess Person
        {
            get { return person; }
            set { person = value; }
        }

        [DataMember]
        public CampusAccess Campus
        {
            get { return campus; }
            set { campus = value; }
        }

        [DataMember]
        public DepartmentAccess Dempartment
        {
            get { return dempartment; }
            set { dempartment = value; }
        }
    }
}
