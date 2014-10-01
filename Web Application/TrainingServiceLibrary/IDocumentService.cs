using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TrainingServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDocumentService" in both code and config file together.
    [ServiceContract]
    public interface IDocumentService
    {
        [OperationContract]
        bool Add(DocumentAccess document);

        [OperationContract]
        bool Update(DocumentAccess document);

        [OperationContract]
        bool Delete(List<Int32> documentIdList);

        [OperationContract]
        List<DocumentAccess> Search(string key = null);

        [OperationContract]
        bool addToTraining(Int32 documentId, Int32 trainingId);

        [OperationContract]
        bool SaveStudyDocumentRecord(StudyDocumentsRecordAccess studyDocumentRecord);

        [OperationContract]
        List<DocumentAccess> GetDocumentData();

    }
}
