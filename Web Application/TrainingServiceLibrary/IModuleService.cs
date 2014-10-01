using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TrainingServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IModuleService" in both code and config file together.
    [ServiceContract]
    public interface IModuleService
    {
        [OperationContract]
        bool AddModule(ModuleAccess module, AgreementInfoAccess aggreement);

        [OperationContract]
        List<ModuleAccess> GetModuleData(string key = null);

        [OperationContract]
        bool UpdateModule(ModuleAccess module, AgreementInfoAccess agreement);

        [OperationContract]
        bool DeleteModule(List<Int32> moduleIdList);

        [OperationContract]
        ModuleAccessDetail GetModuleDetailData(int moduleId);

        [OperationContract]
        AgreementInfoAccess ShowAgreement(int moduleId);

        [OperationContract]
        bool TakeModule(AccountModuleAccess accountModule);

        [OperationContract]
        bool AssignModuleToAccount(ExceptionAccess exception);

        [OperationContract]
        ExceptionAccess GetExceptionData(int moduleId);

        [OperationContract]
        List<ModuleAccess> GetModuleDataForUser(string userName, string key);
    }
}
