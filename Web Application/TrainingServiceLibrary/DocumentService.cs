using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace TrainingServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DocumentService" in both code and config file together.
    public class DocumentService : IDocumentService
    {
        public bool Add(DocumentAccess document)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand("InsertDocument");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter RetVal = cmd.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            SqlParameter name = cmd.Parameters.Add("@name", SqlDbType.VarChar, 255);
            name.Direction = ParameterDirection.Input;

            SqlParameter type = cmd.Parameters.Add("@type", SqlDbType.VarChar, 35);
            type.Direction = ParameterDirection.Input;

            SqlParameter uploadDate = cmd.Parameters.Add("@uploadDate", SqlDbType.DateTime);
            uploadDate.Direction = ParameterDirection.Input;

            SqlParameter description = cmd.Parameters.Add("@description", SqlDbType.VarChar, 255);
            description.Direction = ParameterDirection.Input;

            SqlParameter size = cmd.Parameters.Add("@size", SqlDbType.Int);
            size.Direction = ParameterDirection.Input;

            SqlParameter trainingId = cmd.Parameters.Add("@trainingId", SqlDbType.Int);
            trainingId.Direction = ParameterDirection.Input;

            if (document.DocumentName != null)
            {
                name.Value = document.DocumentName;
            }
            if (document.Type != null)
            {
                type.Value = document.Type;
            }

            uploadDate.Value = DateTime.Now;
            
            if (document.Description != null)
            {
                description.Value = document.Description;
            }
            if (document.Size != 0)
            {
                size.Value = document.Size;
            }
            if (document.TrainingId != 0)
            {
                trainingId.Value = document.TrainingId;
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

        public bool Update(DocumentAccess document)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand("UpdateDocument");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter RetVal = cmd.Parameters.Add("RetVal", SqlDbType.Int);
            RetVal.Direction = ParameterDirection.ReturnValue;

            SqlParameter documentId = cmd.Parameters.Add("@documentId", SqlDbType.Int);
            documentId.Direction = ParameterDirection.Input;

            SqlParameter name = cmd.Parameters.Add("@name", SqlDbType.VarChar, 255);
            name.Direction = ParameterDirection.Input;

            SqlParameter type = cmd.Parameters.Add("@type", SqlDbType.VarChar, 35);
            type.Direction = ParameterDirection.Input;

            SqlParameter uploadDate = cmd.Parameters.Add("@uploadDate", SqlDbType.DateTime);
            uploadDate.Direction = ParameterDirection.Input;

            SqlParameter description = cmd.Parameters.Add("@description", SqlDbType.VarChar, 255);
            description.Direction = ParameterDirection.Input;

            SqlParameter size = cmd.Parameters.Add("@size", SqlDbType.Int);
            size.Direction = ParameterDirection.Input;

            SqlParameter trainingId = cmd.Parameters.Add("@trainingId", SqlDbType.Int);
            trainingId.Direction = ParameterDirection.Input;

            if(document.DocumentId == 0)
            {
                return false;
            }
            documentId.Value = document.DocumentId;

            if (document.DocumentName != null)
            {
                name.Value = document.DocumentName;
            }
            if (document.Type != null)
            {
                type.Value = document.Type;
            }

            if (document.UploadDate != null)
            {
                uploadDate.Value = document.UploadDate;
            }
            
            if (document.Description != null)
            {
                description.Value = document.Description;
            }
            if (document.Size != 0)
            {
                size.Value = document.Size;
            }
            if (document.TrainingId != 0)
            {
                trainingId.Value = document.TrainingId;
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

        public bool Delete(List<Int32> documentIdList)
        {
            Int32 number;
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            

            number = documentIdList.Count;
            for (int i = 0; i < number; i++)
            {
                cmd.CommandText = "DELETE FROM Document WHERE documentId = @documentId" + i
                                + ";";
                cmd.Parameters.AddWithValue("@documentId" + i, Int32.Parse(documentIdList[i].ToString()));
                db.changeData(cmd);
            }
            db.Close();
            return true;
        }

        public List<DocumentAccess> Search(string key = null)
        {
            List<DocumentAccess> documentList = new List<DocumentAccess>();

            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            if (key == null)
            {
                cmd.CommandText = "SELECT documentId, doc.name AS documentName, doc.type, uploadDate, doc.description, size, doc.trainingId, trainingName "
                                + "FROM Document AS doc JOIN TrainingModule AS train ON doc.trainingId = train.trainingId;";
            }
            else
            {
                cmd.CommandText = "SELECT documentId, doc.name AS documentName, doc.type, uploadDate, doc.description, size, doc.trainingId, trainingName "
                                + "FROM Document AS doc JOIN TrainingModule AS train ON doc.trainingId = train.trainingId WHERE doc.name LIKE @documentName;";
                cmd.Parameters.Add("@documentName", SqlDbType.VarChar).Value = "%" + key + "%";
            }

            dr = db.searchData(cmd);
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    DocumentAccess document = new DocumentAccess();
                    if (System.DBNull.Value.Equals(dr[0]))
                    {
                        document.DocumentId = 0;
                    }else
                    {
                        document.DocumentId = Convert.ToInt32(dr[0]);
                    }

                    if (System.DBNull.Value.Equals(dr[1]))
                    {
                        document.DocumentName = null;
                    }else
                    {
                        document.DocumentName = Convert.ToString(dr[1]);
                    }

                    if (System.DBNull.Value.Equals(dr[2]))
                    {
                        document.Type = null;
                    }else
                    {
                        document.Type = Convert.ToString(dr[2]);
                    }

                    if (System.DBNull.Value.Equals(dr[3]))
                    {
                        document.UploadDate = new DateTime();
                    }else
                    {
                        document.UploadDate = Convert.ToDateTime(dr[3]);
                    }

                    if (System.DBNull.Value.Equals(dr[4]))
                    {
                        document.Description = null;
                    }else
                    {
                        document.Description = Convert.ToString(dr[4]);
                    }

                    if (System.DBNull.Value.Equals(dr[5]))
                    {
                        document.Size = 0;
                    }else
                    {
                        document.Size = Convert.ToInt32(dr[5]);
                    }

                    if (System.DBNull.Value.Equals(dr[6]))
                    {
                        document.TrainingId = 0;
                    }else
                    {
                        document.TrainingId = Convert.ToInt32(dr[6]);
                    }

                    if (System.DBNull.Value.Equals(dr[7]))
                    {
                        document.TrainingName = null;
                    }else
                    {
                        document.TrainingName = Convert.ToString(dr[7]);
                    }

                    documentList.Add(document);
                }
            }

            if ((dr != null) && (!dr.IsClosed))
            {
                dr.Close();
            }
            db.Close();
            return documentList;
        }

        public bool addToTraining(Int32 documentId, Int32 trainingId)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Document SET trainingId=@trainingId WHERE documentId=@documentId;";
            cmd.Parameters.AddWithValue("@trainingId", trainingId);
            cmd.Parameters.AddWithValue("@documentId", documentId);
            db.changeData(cmd);
            db.Close();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studyDocumentRecord"></param>
        /// <returns></returns>
        public bool SaveStudyDocumentRecord(StudyDocumentsRecordAccess studyDocumentRecord)
        {
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            DateTime today = DateTime.Today;

            //insert data in Study_Document_Record table
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Study_Document_Record(documentId,method,accountId,visitingDate)VALUES(@documentId, @method, @accountId, @visitingDate);";
            cmd.Parameters.AddWithValue("@documentId", studyDocumentRecord.DocumentId);
            cmd.Parameters.AddWithValue("@method", studyDocumentRecord.Method);
            cmd.Parameters.AddWithValue("@accountId", studyDocumentRecord.AccountId);
            cmd.Parameters.AddWithValue("@visitingDate", today);

            db.changeData(cmd);
            db.Close();
            return true;
        }

        public List<DocumentAccess> GetDocumentData()
        {
            List<DocumentAccess> documentList = new List<DocumentAccess>();
            ConnectDB db = new ConnectDB();
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Document;";
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
                        document.UploadDate = new DateTime();
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

            return documentList;
        }
    }
}
