using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class TrainingTransfer
    {
        private TrainingAccess training;
        private List<DocumentAccess> documentList = new List<DocumentAccess>();

        [DataMember]
        public TrainingAccess Training
        {
            get { return training; }
            set { training = value; }
        }

        [DataMember]
        public List<DocumentAccess> DocumentList
        {
            get { return documentList; }
            set { documentList = value; }
        }
    }
}
