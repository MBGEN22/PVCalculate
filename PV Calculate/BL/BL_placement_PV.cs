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
        public DataTable get_liste_centrale()
        {
            DAL.DAL DAL = new DAL.DAL();
            DataTable Dt = new DataTable();
            Dt = DAL.SelectData("get_liste_centrale", null);
            DAL.Close();
            return Dt;
        }
        public DataTable get_technique_tb()
        {
            DAL.DAL DAL = new DAL.DAL();
            DataTable Dt = new DataTable();
            Dt = DAL.SelectData("get_technique_tb", null);
            DAL.Close();
            return Dt;
        }
        public void InsertParametresTechniques(
            float ChargeElementaire,
            float ConstanteBoltzmann,
            float TempReference,
            float IrradianceReference,
            int NbCellulesSerie,
            float Isc,
            float Voc,
            float Impp,
            float Vmpp,
            float Pmp,
            float CoeffTemperature,
            float ResistanceSerie,
            float ResistanceParallele,
            float FacteurIdealite,
            int NbPanneauxSerie,
            int NbChainesParallele,
            float NOCT
)
        {
            var daoo = new DAL.DAL(); // تأكد أن كائن DAL يحتوي على sqlConnection مُهيّأ
            using (daoo.sqlConnection)
            {
                daoo.sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand("InsertParametresTechniques", daoo.sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ChargeElementaire", ChargeElementaire);
                    cmd.Parameters.AddWithValue("@ConstanteBoltzmann", ConstanteBoltzmann);
                    cmd.Parameters.AddWithValue("@TempReference", TempReference);
                    cmd.Parameters.AddWithValue("@IrradianceReference", IrradianceReference);
                    cmd.Parameters.AddWithValue("@NbCellulesSerie", NbCellulesSerie);
                    cmd.Parameters.AddWithValue("@Isc", Isc);
                    cmd.Parameters.AddWithValue("@Voc", Voc);
                    cmd.Parameters.AddWithValue("@Impp", Impp);
                    cmd.Parameters.AddWithValue("@Vmpp", Vmpp);
                    cmd.Parameters.AddWithValue("@Pmp", Pmp);
                    cmd.Parameters.AddWithValue("@CoeffTemperature", CoeffTemperature);
                    cmd.Parameters.AddWithValue("@ResistanceSerie", ResistanceSerie);
                    cmd.Parameters.AddWithValue("@ResistanceParallele", ResistanceParallele);
                    cmd.Parameters.AddWithValue("@FacteurIdealite", FacteurIdealite);
                    cmd.Parameters.AddWithValue("@NbPanneauxSerie", NbPanneauxSerie);
                    cmd.Parameters.AddWithValue("@NbChainesParallele", NbChainesParallele);
                    cmd.Parameters.AddWithValue("@NOCT", NOCT);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void INSERT_PLACEMENT(
            string NAME,
            float Latitude,
            float Longitude,
            float puissance,
            int? technique_id,
            string id_bus
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
                    cmd.Parameters.AddWithValue("@puissance", puissance);
                    cmd.Parameters.AddWithValue("@technique_id", (object)technique_id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id_bus", (object)id_bus ?? DBNull.Value);

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
        public void delete_technique(
            int para_id
        )
        {
            daoo = new DAL.DAL();
            using (daoo.sqlConnection)
            {
                daoo.sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand("delete_technique", daoo.sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@para_id", para_id); 

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
