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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrainingService" in both code and config file together.
    public class TrainingService : ITrainingService
    {
        public void DoWork()
        {
        }

        public List<TrainingAccess> GetTrainingData()
        {

            List<TrainingAccess> trainingList = new List<TrainingAccess>();

            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from TrainingModule;";
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
                    if (System.DBNull.Value.Equals(dr[4]))
                    {
                        training.ModuleId = 0;
                    }
                    else
                    {
                        training.ModuleId = Convert.ToInt32(dr[4]);
                    }

                    trainingList.Add(training);
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            db.Close();

            return trainingList;
        }

        public bool Add(TrainingAccess training)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand("AddTraining");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter RetVal = cmd.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            SqlParameter trainingName = cmd.Parameters.Add("@trainingName", SqlDbType.VarChar, 50);
            trainingName.Direction = ParameterDirection.Input;

            SqlParameter description = cmd.Parameters.Add("@description", SqlDbType.VarChar, 512);
            description.Direction = ParameterDirection.Input;

            SqlParameter type = cmd.Parameters.Add("@type", SqlDbType.VarChar, 35);
            type.Direction = ParameterDirection.Input;

            SqlParameter moduleId = cmd.Parameters.Add("@moduleId", SqlDbType.Int);
            moduleId.Direction = ParameterDirection.Input;

            if (training.TrainingName != null)
            {
                trainingName.Value = training.TrainingName;
            }
            if (training.Description != null)
            {
                description.Value = training.Description;
            }
            if (training.Type != null)
            {
                type.Value = training.Type;
            }
            if (training.ModuleId != default(int))
            {
                moduleId.Value = training.ModuleId;
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

        public bool Update(TrainingAccess training)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand("UpdateTraining");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter RetVal = cmd.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            SqlParameter trainingId = cmd.Parameters.Add("@trainingId", SqlDbType.Int);
            trainingId.Direction = ParameterDirection.Input;

            SqlParameter trainingName = cmd.Parameters.Add("@trainingName", SqlDbType.VarChar, 50);
            trainingName.Direction = ParameterDirection.Input;

            SqlParameter description = cmd.Parameters.Add("@description", SqlDbType.VarChar, 512);
            description.Direction = ParameterDirection.Input;

            SqlParameter type = cmd.Parameters.Add("@type", SqlDbType.VarChar, 35);
            type.Direction = ParameterDirection.Input;

            SqlParameter moduleId = cmd.Parameters.Add("@moduleId", SqlDbType.Int);
            moduleId.Direction = ParameterDirection.Input;

            if (training.TrainingId != default(int))
            {
                trainingId.Value = training.TrainingId;
            }

            if (training.TrainingName != null)
            {
                trainingName.Value = training.TrainingName;
            }
            if (training.Description != null)
            {
                description.Value = training.Description;
            }
            if (training.Type != null)
            {
                type.Value = training.Type;
            }
            if (training.ModuleId != default(int))
            {
                moduleId.Value = training.ModuleId;
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

        public bool Delete(List<Int32> trainingIdList)
        {
            Int32 number;
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;


            number = trainingIdList.Count;
            for (int i = 0; i < number; i++)
            {
                cmd.CommandText = "UPDATE Document SET trainingId=NULL WHERE trainingId=@trainingIdDocument" + i
                                + ";";
                cmd.Parameters.AddWithValue("@trainingIdDocument" + i, trainingIdList[i]);
                db.changeData(cmd);

                cmd.CommandText = "DELETE FROM TrainingModule WHERE trainingId = @trainingId" + i
                                + ";";
                cmd.Parameters.AddWithValue("@trainingId" + i, trainingIdList[i]);
                db.changeData(cmd);
            }
            db.Close();
            return true;
        }

        public bool AddToModule(int trainingId, int moduleId)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "UPDATE TrainingModule SET moduleId=@moduleId WHERE trainingId=@trainingId;";
            cmd.Parameters.AddWithValue("@moduleId", moduleId);
            cmd.Parameters.AddWithValue("@trainingId", trainingId);
            db.changeData(cmd);

            db.Close();
            return true;
        }

        public TrainingTransfer GeTrainingDetailData(int trainingId)
        {
            TrainingTransfer trainingDetail = new TrainingTransfer();
            TrainingAccess training = new TrainingAccess();
            List<DocumentAccess> documentList = new List<DocumentAccess>();

            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;

            //select data from TrainingModule table according trainingId
            cmd.CommandText = "select * from TrainingModule WHERE trainingId = @trainingId;";
            cmd.Parameters.AddWithValue("@trainingId", trainingId);
            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
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
                    if (System.DBNull.Value.Equals(dr[4]))
                    {
                        training.ModuleId = 0;
                    }
                    else
                    {
                        training.ModuleId = Convert.ToInt32(dr[4]);
                    }
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }

            trainingDetail.Training = training;

            //select document list from database
            cmd.CommandText = "SELECT documentId, name, type, uploadDate, description, size, trainingId FROM Document WHERE trainingId = @trainingDetailId";
            cmd.Parameters.AddWithValue("@trainingDetailId", trainingId);
            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    DocumentAccess document = new DocumentAccess();
                    if (System.DBNull.Value.Equals(dr[0]))
                    {
                        document.DocumentId = 0;
                    }
                    else
                    {
                        document.DocumentId = Convert.ToInt32(dr[0]);
                    }

                    if (System.DBNull.Value.Equals(dr[1]))
                    {
                        document.DocumentName = null;
                    }
                    else
                    {
                        document.DocumentName = Convert.ToString(dr[1]);
                    }

                    if (System.DBNull.Value.Equals(dr[2]))
                    {
                        document.Type = null;
                    }
                    else
                    {
                        document.Type = Convert.ToString(dr[2]);
                    }

                    if (System.DBNull.Value.Equals(dr[3]))
                    {
                        document.UploadDate = default(DateTime);
                    }
                    else
                    {
                        document.UploadDate = Convert.ToDateTime(dr[3]);
                    }

                    if (System.DBNull.Value.Equals(dr[4]))
                    {
                        document.Description = null;
                    }
                    else
                    {
                        document.Description = Convert.ToString(dr[4]);
                    }

                    if (System.DBNull.Value.Equals(dr[5]))
                    {
                        document.Size = 0;
                    }
                    else
                    {
                        document.Size = Convert.ToInt32(dr[5]);
                    }

                    if (System.DBNull.Value.Equals(dr[6]))
                    {
                        document.TrainingId = 0;
                    }
                    else
                    {
                        document.TrainingId = Convert.ToInt32(dr[6]);
                    }

                    documentList.Add(document);
                }
            }
            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }

            db.Close();

            trainingDetail.DocumentList = documentList;

            return trainingDetail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<TrainingAccess> GetTrainingDataForModule(int moduleId)
        {
            List<TrainingAccess> trainingList = new List<TrainingAccess>();

            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM TrainingModule WHERE moduleId = @moduleId;";
            cmd.Parameters.Add("@moduleId", SqlDbType.Int).Value = moduleId;
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
                    if (System.DBNull.Value.Equals(dr[4]))
                    {
                        training.ModuleId = 0;
                    }
                    else
                    {
                        training.ModuleId = Convert.ToInt32(dr[4]);
                    }

                    trainingList.Add(training);
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            db.Close();

            return trainingList;
        }
    }
}
