using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinxAPI.Entity
{
   public static class LogEntity
    {
        public static SqlConnection abrir()
        {
            string strcon = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Vini\source\repos\LinxAPI\DBase\LinxAPI.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            return con;
        }
    }
}
