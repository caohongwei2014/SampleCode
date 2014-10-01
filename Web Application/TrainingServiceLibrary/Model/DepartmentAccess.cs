using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class DepartmentAccess
    {
        //departmentId INTEGER  NOT NULL,
        //  name CHAR(255),
        //  description CHAR(255),
        private Int32 departmentId;
        private string name;
        private string description;

        [DataMember]
        public Int32 DepartmentId
        {
            get { return departmentId; }
            set { departmentId = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
