using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Data;
using System.Data.SqlClient;

namespace PV_Calculate.BL
{
    internal class BL_Combo
    {
        public DataTable cmb_technique()
        {
            DAL.DAL DAL = new DAL.DAL();
            DataTable Dt = new DataTable();
            Dt = DAL.SelectData("cmb_technique", null);
            DAL.Close();
            return Dt;
        }
        public DataTable cmb_bus()
        {
            DAL.DAL DAL = new DAL.DAL();
            DataTable Dt = new DataTable();
            Dt = DAL.SelectData("cmb_bus", null);
            DAL.Close();
            return Dt;
        }
    }
}
