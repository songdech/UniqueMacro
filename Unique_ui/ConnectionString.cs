using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniquePro.Entities.Common;

namespace UNIQUE
{ 
    class ConnectionString
    {
        SqlConnection conn;        
        //  IniParser parser;
        string conStr;
  
        public SqlConnection Connect()
        {
            //    parser = new IniParser(@"Setting.ini");
            try
            {
                //string configvalue1 =

                //conStr = "";
                //conStr = @"Data Source=SONGDECH-ISC\SQLEXPRESS ;Initial Catalog=UNIQUE;Persist Security Info=True;User ID=sa;Password=P@ssw0rd";
                //conStr = @"Data Source=uniquedb.ckgq5ecwqhep.ap-southeast-1.rds.amazonaws.com,1433;Initial Catalog=UNIQUE_DB;User ID=admin;Password=admin1234";
                conStr = @"Data Source=" + ConnectStringM.StrDataSource + " ;Initial Catalog=" + ConnectStringM.StrCatalog + ";Persist Security Info=True;User ID=" + ConnectStringM.StrUser + ";Password=" + ConnectStringM.StrPassword;
                conn = new SqlConnection(conStr);
                if (conn.State == System.Data.ConnectionState.Open) conn.Close(); conn.Open();
            }
            catch (SqlException) { }
            return conn;
        }
    }
}
