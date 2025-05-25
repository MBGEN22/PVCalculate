using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV_Calculate.DAL
{
    internal class DAL
    {
        //link  connection 
        public SqlConnection sqlConnection;
        public DAL()
        {
            //
            string mode = Properties.Settings.Default.mode;
            if (mode == "SQL")
            {
                sqlConnection = new SqlConnection(@"Server=" + Properties.Settings.Default.server + "; Database=" + Properties.Settings.Default.dataBase +
                    "; Integrated Security=false ; User ID =" + Properties.Settings.Default.ID + ";Password=" + Properties.Settings.Default.PASS + "");
            }
            else if (mode == "Local")
            {
                sqlConnection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + @"|DataDirectory|\db_uc_projet.mdf" + "; Integrated Security = True");
            }
            else
            {
                sqlConnection = new SqlConnection(@"Server=" + Properties.Settings.Default.server + "; Database=" + Properties.Settings.Default.dataBase + "; Integrated Security=true");
            }
        }

        //Methode Open Connection 
        public void Open()
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
        }

        //methoode Close Connection 

        public void Close()
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        //methode read data from dataBase 
        public DataTable SelectData(string stored_procedure, SqlParameter[] param)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = stored_procedure;
                sqlCmd.Connection = sqlConnection;
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        sqlCmd.Parameters.Add(param[i]);
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                //frm_server server = new frm_server();
                //var st = MessageBox.Show(ex.Message);
                //server.ShowDialog();
                return null;
            }
        }



        //Methode to insert , Update , delete data from Database 
        public void ExecuteCommand(string Stored_Produced, SqlParameter[] param)
        {
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = Stored_Produced;
            sqlcmd.Connection = sqlConnection;

            if (param != null)
            {
                for (int i = 0; i < param.Length; i++)
                {
                    sqlcmd.Parameters.Add(param[i]);
                }
            }
        }
    }
}
