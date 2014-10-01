using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class PersonAccess
    {
        private Int32 personId;
        private string firstName;
        private string lastName;
        private char gender;
        private Int32 supervisorId;
        private string address;
        private string phoneNumber;
        private string email;

        [DataMember]
        public Int32 PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [DataMember]
        public char Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        [DataMember]
        public Int32 SupervisorId
        {
            get { return supervisorId; }
            set { supervisorId = value; }
        }

        [DataMember]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        [DataMember]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
    }
}
