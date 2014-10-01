using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class DocumentAccess
    {
      //  documentId INTEGER  NOT NULL IDENTITY,
      //name VARCHAR(255),
      //type CHAR(255),
      //uploadDate DATETIME,
      //description VARCHAR(255),
      //size INTEGER,
      //trainingId INTEGER NOT NULL,
        private Int32 documentId;
        private Int32 trainingId;
        private string documentName;
        private string trainingName;
        private string type;
        private DateTime uploadDate;
        private Int32 size;
        private string description;

        [DataMember]
        public Int32 DocumentId
        {
            get { return documentId; }
            set { documentId = value; }
        }

        [DataMember]
        public Int32 TrainingId
        {
            get { return trainingId; }
            set { trainingId = value; }
        }

        [DataMember]
        public string TrainingName
        {
            get { return trainingName; }
            set { trainingName = value; }
        }

        [DataMember]
        public string DocumentName
        {
            get { return documentName; }
            set { documentName = value; }
        }

        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        [DataMember]
        public DateTime UploadDate
        {
            get { return uploadDate; }
            set { uploadDate = value; }
        }

        [DataMember]
        public Int32 Size
        {
            get { return size; }
            set { size = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
