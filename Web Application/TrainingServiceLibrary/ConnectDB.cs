using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace TrainingServiceLibrary
{
    public class ConnectDB
    {

        SqlConnection con = new SqlConnection(@"Data Source=2A501-J06\SQLEXPRESS;Initial Catalog=TrainingRegistion;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        List<PersonAccess> personList = new List<PersonAccess>();

        public void Connect()
        {
            cmd.Connection = con;
            if ((con != null) && (con.State == ConnectionState.Closed))
            {
                con.Open();
            }            
        }

        public void DeleteData(SqlCommand sqlCmd)
        {
        }

        public void changeData(SqlCommand sqlCmd)
        {
            cmd = sqlCmd;
            Connect();
            cmd.ExecuteNonQuery(); 
            //try
            //{
            //    cmd = sqlCmd;
            //    Connect();
            //    cmd.ExecuteNonQuery(); 
            //}catch(Exception ex)
            //{
            //    Console.WriteLine("Execute sql exception, message is " + ex);
            //}  
        }

        public void Close()
        {

            if ((dr != null) &&(!dr.IsClosed))
            {
                dr.Close();
            }
            //cmd.Clone();
            if (con != null)
            {
                con.Close();
            }
            
        }

        public SqlDataReader searchData(SqlCommand sqlCmd)
        {
            cmd = sqlCmd;
            Connect();
            dr = cmd.ExecuteReader();

            return dr;
        }

        public void addData()
        {
              //@name VARCHAR(255),
              //@type VARCHAR(35),
              //@uploadDate DATETIME,
              //@description VARCHAR(255),
              //@size INTEGER,
              //@trainingId INTEGER
            cmd = new SqlCommand("InsertDocument", con);
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

            name.Value = "Asp.net development";
            type.Value = "pdf";
            uploadDate.Value = "2014-03-28";
            //description.Value = null;
            //size.Value = 1024;
            //trainingId.Value = 1;

            con.Open();
            //cmd.CommandText = "insert into person(personId, firstName, lastName, gender, supervisorId, address, phoneNumber, email)" +
            //                    " values(2, 'Tom', 'Li', 'F', 1, 'London street', '333-333-3333', 'custom@126.com');";
            cmd.ExecuteNonQuery();
            //cmd.Clone();
            if (con != null)
            {
                con.Close();
            }
        }
    }
}
