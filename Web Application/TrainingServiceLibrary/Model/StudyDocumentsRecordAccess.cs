using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class StudyDocumentsRecordAccess
    {
        private Int32 studyDocumentRecordId;
        private Int32 documentId;
        private Int32 accountId;
        private string method;
        private DateTime visitingDate;

        [DataMember]
        public Int32 StudyDocumentRecordId
        {
            get { return studyDocumentRecordId; }
            set { studyDocumentRecordId = value; }
        }

        [DataMember]
        public Int32 AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        [DataMember]
        public Int32 DocumentId
        {
            get { return documentId; }
            set { documentId = value; }
        }

        [DataMember]
        public string Method
        {
            get { return method; }
            set { method = value; }
        }

        [DataMember]
        public DateTime VisitingDate
        {
            get { return visitingDate; }
            set { visitingDate = value; }
        }

    }
}
