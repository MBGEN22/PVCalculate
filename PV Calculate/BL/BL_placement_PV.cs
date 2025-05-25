using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV_Calculate.BL
{
    internal class BL_placement_PV
    {
        DAL.DAL daoo;

        public DataTable get_central_with_parame()
        {
            DAL.DAL DAL = new DAL.DAL();
            DataTable Dt = new DataTable();
            Dt = DAL.SelectData("get_central_with_parame", null);
            DAL.Close();
            return Dt;
        }

        public void INSERT_PLACEMENT(
            string NAME,
            float Latitude,
            float Longitude
        )
        {
            daoo = new DAL.DAL();
            using (daoo.sqlConnection)
            {
                daoo.sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT_PLACEMENT", daoo.sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NAME", NAME);
                    cmd.Parameters.AddWithValue("@Latitude", Latitude);
                    cmd.Parameters.AddWithValue("@Longitude", Longitude); 
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool DELETE_PLACEMENT_BY_COORDS(double latitude, double longitude)
        {
            daoo = new DAL.DAL();
            int rowsAffected = 0;
            using (daoo.sqlConnection)
            {
                daoo.sqlConnection.Open();
                string query = "DELETE FROM TB_PLACEMENT_CENTRALE_PV WHERE ABS(Latitude - @lat) < 0.0001 AND ABS(Longitude - @lng) < 0.0001";
                SqlCommand cmd = new SqlCommand(query, daoo.sqlConnection);
                cmd.Parameters.AddWithValue("@lat", latitude);
                cmd.Parameters.AddWithValue("@lng", longitude);
                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected > 0; // true إذا حذفنا على الأقل سجل واحد
        }

    }
}
