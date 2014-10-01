using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class CampusAccess
    {
        //campusId INTEGER  NOT NULL IDENTITY,
        //name CHAR(255),
        private Int32 campusId;
        private string name;

        [DataMember]
        public Int32 CampusId
        {
            get { return campusId; }
            set { campusId = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
