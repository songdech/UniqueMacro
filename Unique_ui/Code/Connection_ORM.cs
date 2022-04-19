using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace UNIQUE
{
    class Connection_ORM
    {
        SqlConnection conn;
        // Connection string
        string Strcon;

        private static AES128_EncryptAndDecrypt Aes128 = new AES128_EncryptAndDecrypt();

        // setting file
        string setting_ORM = "Setting_ORM.xml";

        // Connection TDNexlab DATABASE
        public SqlConnection Connect()
        {
            try
            {
                DataSet dsXmlFile = new DataSet();
                dsXmlFile.ReadXml(setting_ORM, XmlReadMode.Auto);

                Strcon = @"Data Source=" + dsXmlFile.Tables[0].Rows[0]["dbserver"].ToString() + ";Initial Catalog=" + dsXmlFile.Tables[0].Rows[0]["dbORM_name"].ToString() + ";Persist Security Info=True;User ID=" + dsXmlFile.Tables[0].Rows[0]["dbORM_user"].ToString() + ";Password=" + Aes128.Decrypt(dsXmlFile.Tables[0].Rows[0]["dbORM_pass"].ToString());
                conn = new SqlConnection(Strcon);

                if (conn.State == System.Data.ConnectionState.Open) conn.Close(); conn.Open();
            }
            catch (SqlException) { MessageBox.Show("Cannot connect to the database ORM", "Warning..", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            return conn;
        }
    }
}
