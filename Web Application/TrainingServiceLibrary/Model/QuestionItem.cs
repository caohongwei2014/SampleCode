using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class QuestionSelectItem
    {
        //  mulSelectionId INTEGER  NOT NULL IDENTITY,
        //questionAnswerId INTEGER NOT NULL,
        //choiceDescription VARCHAR(1024),
        //choiceSequenceNumber VARCHAR(255),
        private Int32 mulSelectionId;
        private Int32 questionAnswerId;
        private string choiceSequenceNumber;
        private string choiceDescription;

        [DataMember]
        public Int32 MulSelectionId
        {
            get { return mulSelectionId; }
            set { mulSelectionId = value; }
        }

        [DataMember]
        public Int32 QuestionAnswerId
        {
            get { return questionAnswerId; }
            set { questionAnswerId = value; }
        }


        [DataMember]
        public string ChoiceSequenceNumber
        {
            get { return choiceSequenceNumber; }
            set { choiceSequenceNumber = value; }
        }

        [DataMember]
        public string ChoiceDescription
        {
            get { return choiceDescription; }
            set { choiceDescription = value; }
        }
    }
}
