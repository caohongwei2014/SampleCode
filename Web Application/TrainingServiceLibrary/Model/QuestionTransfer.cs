using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    [DataContract]
    public class QuestionTransfer
    {
        private QuestionAnswerAccess questionAnswer;
        private List<QuestionSelectItem> selectionItems = new List<QuestionSelectItem>();
        private Int32 itemsNumber;

        [DataMember]
        public Int32 ItemsNumber
        {
            get { return itemsNumber; }
            set { itemsNumber = value; }
        }

        [DataMember]
        public List<QuestionSelectItem> SelectionItems
        {
            get { return selectionItems; }
            set { selectionItems = value; }
        }


        [DataMember]
        public QuestionAnswerAccess QuestionAnswer
        {
            get { return questionAnswer; }
            set { questionAnswer = value; }
        }
    }
}
