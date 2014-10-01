using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TrainingServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ModuleService" in both code and config file together.
    public class ModuleService : IModuleService
    {

        public bool AddModule(ModuleAccess module, AgreementInfoAccess agreement)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand("AddModule");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter RetVal = cmd.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            SqlParameter moduleName = cmd.Parameters.Add("@moduleName", SqlDbType.VarChar, 255);
            moduleName.Direction = ParameterDirection.Input;

            SqlParameter description = cmd.Parameters.Add("@description", SqlDbType.VarChar, 255);
            description.Direction = ParameterDirection.Input;

            SqlParameter expiryDate = cmd.Parameters.Add("@expiryDate", SqlDbType.DateTime);
            expiryDate.Direction = ParameterDirection.Input;

            SqlParameter creationDate = cmd.Parameters.Add("@creationDate", SqlDbType.DateTime);
            creationDate.Direction = ParameterDirection.Input;

            SqlParameter userType = cmd.Parameters.Add("@userType", SqlDbType.VarChar, 255);
            userType.Direction = ParameterDirection.Input;

            SqlParameter content = cmd.Parameters.Add("@content", SqlDbType.VarChar, 2048);
            content.Direction = ParameterDirection.Input;

            if (module.ModuleName != null)
            {
                moduleName.Value = module.ModuleName;
            }
            if (module.Description != null)
            {
                description.Value = module.Description;
            }
            if (module.ExpiryDate != null)
            {
                expiryDate.Value = module.ExpiryDate;
            }

            creationDate.Value = DateTime.Now;

            if (agreement.UserType != null)
            {
                userType.Value = agreement.UserType;
            }
            if (agreement.Content != null)
            {
                content.Value = agreement.Content;
            }

            db.changeData(cmd);
            db.Close();

            if (Int32.Parse(RetVal.Value.ToString()) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateModule(ModuleAccess module, AgreementInfoAccess agreement)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand("UpdateModule");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter RetVal = cmd.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            //@moduleName VARCHAR(255) ,
            //  @description VARCHAR(255) = NULL,  
            //  @expiryDate DATETIME = NULL,
            //  @creationDate DATETIME = null,
            //  @userType VARCHAR(255),
            //  @content VARCHAR(2048) = NULL

            SqlParameter moduleId = cmd.Parameters.Add("@moduleId", SqlDbType.Int);
            moduleId.Direction = ParameterDirection.Input;

            SqlParameter moduleName = cmd.Parameters.Add("@moduleName", SqlDbType.VarChar, 255);
            moduleName.Direction = ParameterDirection.Input;

            SqlParameter description = cmd.Parameters.Add("@description", SqlDbType.VarChar, 255);
            description.Direction = ParameterDirection.Input;

            SqlParameter expiryDate = cmd.Parameters.Add("@expiryDate", SqlDbType.DateTime);
            expiryDate.Direction = ParameterDirection.Input;

            SqlParameter creationDate = cmd.Parameters.Add("@creationDate", SqlDbType.DateTime);
            creationDate.Direction = ParameterDirection.Input;

            SqlParameter userType = cmd.Parameters.Add("@userType", SqlDbType.VarChar, 255);
            userType.Direction = ParameterDirection.Input;

            SqlParameter content = cmd.Parameters.Add("@content", SqlDbType.VarChar, 2048);
            content.Direction = ParameterDirection.Input;

            if (module.ModuleId != 0)
            {
                return false;
            }
            moduleId.Value = module.ModuleId;
            if (module.ModuleName != null)
            {
                moduleName.Value = module.ModuleName;
            }
            if (module.Description != null)
            {
                description.Value = module.Description;
            }
            if (module.ExpiryDate != null)
            {
                expiryDate.Value = module.ExpiryDate;
            }

            creationDate.Value = DateTime.Now;

            if (agreement.UserType != null)
            {
                userType.Value = agreement.UserType;
            }
            if (agreement.Content != null)
            {
                content.Value = agreement.Content;
            }

            db.changeData(cmd);
            db.Close();

            if (Int32.Parse(RetVal.Value.ToString()) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public ModuleAccessDetail GetModuleDetailData(int moduleId)
        {
            ModuleAccessDetail moduleDetail = new ModuleAccessDetail();
            List<TrainingAccess> trainingList = new List<TrainingAccess>();
            List<QuizAccess> quizList = new List<QuizAccess>();
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            //query training table
            cmd.CommandText = "SELECT trainingId, trainingName, description, type FROM TrainingModule WHERE moduleId = @moduleId";
            cmd.Parameters.AddWithValue("@moduleId", moduleId);
            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    TrainingAccess training = new TrainingAccess();
                    if (System.DBNull.Value.Equals(dr[0]))
                    {
                        training.TrainingId = 0;
                    }
                    else
                    {
                        training.TrainingId = Convert.ToInt32(dr[0]);
                    }

                    if (System.DBNull.Value.Equals(dr[1]))
                    {
                        training.TrainingName = null;
                    }
                    else
                    {
                        training.TrainingName = Convert.ToString(dr[1]);
                    }

                    if (System.DBNull.Value.Equals(dr[2]))
                    {
                        training.Description = null;
                    }
                    else
                    {
                        training.Description = Convert.ToString(dr[2]);
                    }

                    if (System.DBNull.Value.Equals(dr[3]))
                    {
                        training.Type = null;
                    }
                    else
                    {
                        training.Type = Convert.ToString(dr[3]);
                    }

                    trainingList.Add(training);
                }
            }
            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            moduleDetail.Training = trainingList;

            //query quiz table
            cmd.CommandText = "SELECT quizId, quizName, description, passingGrade FROM Quiz WHERE moduleId = @moduleIdq";
            cmd.Parameters.AddWithValue("@moduleIdq", moduleId);
            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    QuizAccess quiz = new QuizAccess();
                    if (System.DBNull.Value.Equals(dr[0]))
                    {
                        quiz.QuizId = 0;
                    }
                    else
                    {
                        quiz.QuizId = Convert.ToInt32(dr[0]);
                    }

                    if (System.DBNull.Value.Equals(dr[1]))
                    {
                        quiz.QuizName = null;
                    }
                    else
                    {
                        quiz.QuizName = Convert.ToString(dr[1]);
                    }

                    if (System.DBNull.Value.Equals(dr[2]))
                    {
                        quiz.Description = null;
                    }
                    else
                    {
                        quiz.Description = Convert.ToString(dr[2]);
                    }

                    if (System.DBNull.Value.Equals(dr[3]))
                    {
                        quiz.PassingGrade = 0;
                    }
                    else
                    {
                        quiz.PassingGrade = Convert.ToInt32(dr[3]);
                    }

                    quizList.Add(quiz);
                }
            }

            moduleDetail.Quiz = quizList;

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            db.Close();

            return moduleDetail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<ModuleAccess> GetModuleData(string key = null)
        {
            List<ModuleAccess> moduleList = new List<ModuleAccess>();

            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            if (key == null)
            {
                cmd.CommandText = "SELECT moduleId, moduleName, description, expiryDate, creationDate FROM Module; ";
            }
            else
            {
                cmd.CommandText = "SELECT moduleId, moduleName, description, expiryDate, creationDate FROM Module WHERE moduleName LIKE @moduleName;";
                cmd.Parameters.Add("@moduleName", SqlDbType.VarChar).Value = "%" + key + "%";
            }

            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    ModuleAccess module = new ModuleAccess();
                    if (System.DBNull.Value.Equals(dr[0]))
                    {
                        module.ModuleId = 0;
                    }
                    else
                    {
                        module.ModuleId = Convert.ToInt32(dr[0]);
                    }

                    if (System.DBNull.Value.Equals(dr[1]))
                    {
                        module.ModuleName = null;
                    }
                    else
                    {
                        module.ModuleName = Convert.ToString(dr[1]);
                    }

                    if (System.DBNull.Value.Equals(dr[2]))
                    {
                        module.Description = null;
                    }
                    else
                    {
                        module.Description = Convert.ToString(dr[2]);
                    }

                    if (System.DBNull.Value.Equals(dr[3]))
                    {
                        module.ExpiryDate = default(DateTime);
                    }
                    else
                    {
                        module.ExpiryDate = Convert.ToDateTime(dr[3]);
                    }

                    if (System.DBNull.Value.Equals(dr[3]))
                    {
                        module.CreationDate = default(DateTime);
                    }
                    else
                    {
                        module.CreationDate = Convert.ToDateTime(dr[3]);
                    }

                    moduleList.Add(module);
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            db.Close();
            return moduleList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<ModuleAccess> GetModuleDataForUser(string userName, string key)
        {
            List<ModuleAccess> moduleList = new List<ModuleAccess>();

            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;

            if (key == null)
            {
                cmd.CommandText = "SELECT M.* FROM Module AS M"
                                + " WHERE ((M.expiryDate > GETDATE())"
                                + " OR (M.moduleId IN ("
                                + " SELECT EX.moduleId FROM Exception AS EX JOIN Account AS A ON EX.accountId = A.accountId"
                                + " WHERE A.userName= @userName"
                                + " AND EX.expiryDate > GETDATE())))"
                                + " AND (M.moduleId NOT IN ("
                                + " SELECT AM.moduleId FROM Account_Module AS AM JOIN Account AS A ON A.accountId = AM.accountId"
                                + " WHERE A.userName = @userName1));";
                cmd.Parameters.Add("@userName", SqlDbType.VarChar).Value = userName;
                cmd.Parameters.Add("@userName1", SqlDbType.VarChar).Value = userName;                
            }
            else
            {
                cmd.CommandText = "SELECT M.* FROM Module AS M "
                                + " WHERE ((M.moduleName LIKE @moduleName"
                                + " AND M.expiryDate > GETDATE())"
                                + " OR (M.moduleId IN ("
                                + " SELECT EX.moduleId FROM Exception AS EX JOIN Account AS A ON EX.accountId = A.accountId"
                                + " JOIN Module AS M ON EX.moduleId = M.moduleId"
                                + " WHERE A.userName= @userName"
                                + " AND M.moduleName LIKE @moduleName1"
                                + " AND EX.expiryDate > GETDATE())))"
                                + " AND (M.moduleId NOT IN ("
                                + " SELECT AM.moduleId FROM Account_Module AS AM JOIN Account AS A ON A.accountId = AM.accountId"
                                + " WHERE A.userName = @userName1));";
                cmd.Parameters.Add("@moduleName", SqlDbType.VarChar).Value = "%" + key + "%";
                cmd.Parameters.Add("@userName", SqlDbType.VarChar).Value = userName;
                cmd.Parameters.Add("@moduleName1", SqlDbType.VarChar).Value = "%" + key + "%";
                cmd.Parameters.Add("@userName1", SqlDbType.VarChar).Value = userName; 
            }
           
            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    ModuleAccess module = new ModuleAccess();
                    if (System.DBNull.Value.Equals(dr[0]))
                    {
                        module.ModuleId = 0;
                    }
                    else
                    {
                        module.ModuleId = Convert.ToInt32(dr[0]);
                    }

                    if (System.DBNull.Value.Equals(dr[1]))
                    {
                        module.ModuleName = null;
                    }
                    else
                    {
                        module.ModuleName = Convert.ToString(dr[1]);
                    }

                    if (System.DBNull.Value.Equals(dr[2]))
                    {
                        module.Description = null;
                    }
                    else
                    {
                        module.Description = Convert.ToString(dr[2]);
                    }

                    if (System.DBNull.Value.Equals(dr[3]))
                    {
                        module.ExpiryDate = default(DateTime);
                    }
                    else
                    {
                        module.ExpiryDate = Convert.ToDateTime(dr[3]);
                    }

                    if (System.DBNull.Value.Equals(dr[3]))
                    {
                        module.CreationDate = default(DateTime);
                    }
                    else
                    {
                        module.CreationDate = Convert.ToDateTime(dr[3]);
                    }

                    moduleList.Add(module);
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            db.Close();
            return moduleList;
        }

        public bool DeleteModule(List<Int32> moduleIdList)
        {
            Int32 number;
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;


            number = moduleIdList.Count;
            for (int i = 0; i < number; i++)
            {
                cmd.CommandText = "DELETE FROM Module WHERE moduleId = @moduleId" + i
                                + ";";
                cmd.Parameters.AddWithValue("@moduleId" + i, moduleIdList[i]);
                db.changeData(cmd);
            }
            db.Close();
            return true;
        }

        public AgreementInfoAccess ShowAgreement(int moduleId)
        {
            AgreementInfoAccess agreement = new AgreementInfoAccess();

            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            if (moduleId == 0)
            {
                return agreement;
            }
            else
            {
                cmd.CommandText = "SELECT userType, content, moduleId FROM AgreementInformation WHERE moduleId = @moduleId;";
                cmd.Parameters.Add("@moduleId", SqlDbType.Int).Value = moduleId;
            }

            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (System.DBNull.Value.Equals(dr[0]))
                    {
                        agreement.UserType = null;
                    }
                    else
                    {
                        agreement.UserType = Convert.ToString(dr[0]);
                    }

                    if (System.DBNull.Value.Equals(dr[1]))
                    {
                        agreement.Content = null;
                    }
                    else
                    {
                        agreement.Content = Convert.ToString(dr[1]);
                    }

                    if (System.DBNull.Value.Equals(dr[2]))
                    {
                        agreement.ModuleId = 0;
                    }
                    else
                    {
                        agreement.ModuleId = Convert.ToInt32(dr[2]);
                    }
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            db.Close();
            return agreement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountModule"></param>
        /// <returns></returns>
        public bool TakeModule(AccountModuleAccess accountModule)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;

            DateTime today = DateTime.Now;

            //add a new record data in Account_Module table
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Account_Module(accountId,moduleId,status,visitingDate)VALUES(@accountId, @moduleId, @status, @visitingDate);";
            cmd.Parameters.AddWithValue("@accountId", accountModule.AccountId);
            cmd.Parameters.AddWithValue("@moduleId", accountModule.ModuleId);
            cmd.Parameters.AddWithValue("@status", accountModule.Status);
            cmd.Parameters.AddWithValue("@visitingDate", today);
            db.changeData(cmd);

            db.Close();
            return true;
        }

        /// <summary>
        /// As administrators user, they can assign a module that has expired to a staff
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public bool AssignModuleToAccount(ExceptionAccess exception)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;

            //add a new record data in Account_Module table
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Exception(accountId,moduleId,reason,creationDate,expiryDate)VALUES(@accountId, @moduleId, @reason, @creationDate, expiryDate);";
            cmd.Parameters.AddWithValue("@accountId", exception.AccountId);
            cmd.Parameters.AddWithValue("@moduleId", exception.ModuleId);
            cmd.Parameters.AddWithValue("@reason", exception.Reason);
            cmd.Parameters.AddWithValue("@creationDate", exception.CreationDate);
            cmd.Parameters.AddWithValue("@expiryDate", exception.ExpiryDate);
            db.changeData(cmd);

            db.Close();
            return true;
        }

        /// <summary>
        /// Query Exception table to see if the module is assigned to employees
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public ExceptionAccess GetExceptionData(int moduleId)
        {
            ExceptionAccess exceptionModule = new ExceptionAccess();

            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            if (moduleId == 0)
            {
                return exceptionModule;
            }
            else
            {
                cmd.CommandText = "SELECT * FROM Exception WHERE moduleId = @moduleId;";
                cmd.Parameters.Add("@moduleId", SqlDbType.Int).Value = moduleId;
            }

            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (System.DBNull.Value.Equals(dr[0]))
                    {
                        exceptionModule.ExceptionId = 0;
                    }
                    else
                    {
                        exceptionModule.ExceptionId = Convert.ToInt32(dr[0]);
                    }

                    if (System.DBNull.Value.Equals(dr[1]))
                    {
                        exceptionModule.AccountId = 0;
                    }
                    else
                    {
                        exceptionModule.AccountId = Convert.ToInt32(dr[1]);
                    }

                    if (System.DBNull.Value.Equals(dr[2]))
                    {
                        exceptionModule.ModuleId = 0;
                    }
                    else
                    {
                        exceptionModule.ModuleId = Convert.ToInt32(dr[2]);
                    }

                    if (System.DBNull.Value.Equals(dr[3]))
                    {
                        exceptionModule.Reason = null;
                    }
                    else
                    {
                        exceptionModule.Reason = Convert.ToString(dr[3]);
                    }

                    if (System.DBNull.Value.Equals(dr[4]))
                    {
                        exceptionModule.CreationDate = default(DateTime);
                    }
                    else
                    {
                        exceptionModule.CreationDate = Convert.ToDateTime(dr[4]);
                    }

                    if (System.DBNull.Value.Equals(dr[5]))
                    {
                        exceptionModule.ExpiryDate = default(DateTime);
                    }
                    else
                    {
                        exceptionModule.ExpiryDate = Convert.ToDateTime(dr[5]);
                    }
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            db.Close();
            return exceptionModule;
        }
    }
}
