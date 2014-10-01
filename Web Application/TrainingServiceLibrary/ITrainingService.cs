using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TrainingServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITrainingService" in both code and config file together.
    [ServiceContract]
    public interface ITrainingService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        List<TrainingAccess> GetTrainingData();

        [OperationContract]
        bool Add(TrainingAccess training);

        [OperationContract]
        bool Update(TrainingAccess training);

        [OperationContract]
        bool Delete(List<Int32> trainingIdList);

        [OperationContract]
        bool AddToModule(int trainingId, int moduleId);

        [OperationContract]
        TrainingTransfer GeTrainingDetailData(int trainingId);

        [OperationContract]
        List<TrainingAccess> GetTrainingDataForModule(int moduleId);
    }
}
