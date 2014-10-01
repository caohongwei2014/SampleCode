using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TrainingServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        bool AddUser(UserTransfer user);

        [OperationContract]
        bool UpdateUser(UserTransfer user);

        [OperationContract]
        bool DeleteUser(List<Int32> accountIdList);

        [OperationContract]
        Int32 Login(string userName, string password, out string userLevel);

        [OperationContract]
        List<PersonAccess> GetPersonData();

        [OperationContract]
        List<DepartmentAccess> GetDepartmentData();

        [OperationContract]
        List<CampusAccess> GetCampusData();

        [OperationContract]
        List<UserTransfer> GetUserData(string key = null);

        [OperationContract]
        List<EmployeeReportTransfer> GetEmployeeInfo(int personId);

        [OperationContract]
        List<AdminViewReportTransfer> GetModuleInfo();

        [OperationContract]
        bool changePassword(string userName, string password);

        [OperationContract]
        List<ModuleDetailTransfer> GetModuleInfoForUser(string userName);

        [OperationContract]
        int GetAccountId(string userName);

        // TODO: Add your service operations here
    }
}
