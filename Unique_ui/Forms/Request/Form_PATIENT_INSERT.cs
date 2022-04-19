using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.StyleFormatConditions;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils.Menu;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace UNIQUE.OEM
{
    public partial class Form_PATIENT_INSERT : Form
    {
        System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");
        string format_DT = "dd/MM/yyyy HH:mm:ss";
        string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));

        // Confirm Encryption
        AES128_EncryptAndDecrypt AES_128 = new AES128_EncryptAndDecrypt();
        // Confirm Encryption

        int HN_LENGTH;
        int LN_LENGTH;
        // Patient Info
        string strBirthDate = "";
        string StrPATID_REQ = "";

        string Query_Accnumber = "";
        TreeListNode childNode;
        SqlConnection conn;

        // Dianostic
        public static DataSet ds_result;

        public Form_PATIENT_INSERT()
        {
            InitializeComponent();
        }

        private void Insert_New_counter(int StrIPcounter)
        {
            string sql_insert_Counter = "";
            try
            {
                int Number_Cou = 10000 + StrIPcounter;
                DateTime dt = DateTime.Now;
                string format_NOW = "yyyy-MM-dd HH:mm:ss.fff";
                string StrCouLog = DateTime.Now.ToString(format_NOW, cultureinfo);

                sql_insert_Counter = @"INSERT INTO COUNTER (COUNTERNUMBER,LENGHT,COUNTER,NOTE,LOGDATE) VALUES ('" + StrIPcounter + "','5','" + Number_Cou + "','Create files ORM','" + StrCouLog + "')";

                Writedatalog.WriteLog("Start Insert New Client & counter---->" + "\r\n" + sql_insert_Counter);
                SqlCommand cmd = new SqlCommand(sql_insert_Counter, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error insert COUNTER in Database \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Query_HNLENGTH()
        {
            try
            {
                conn = new Connection_ORM().Connect();
                string sql = @"SELECT * FROM DICT_SYSTEM_MB_CONFIG";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "result");
                SqlDataReader reader = cmd.ExecuteReader();

                Writedatalog.WriteLog_Rectube("Query HN_LENGTH   = " + sql);

                if (ds.Tables["result"].Rows.Count > 0)
                {
                    HN_LENGTH = Convert.ToInt32(ds.Tables["result"].Rows[0]["HN_LENGTH"].ToString());
                    LN_LENGTH = Convert.ToInt32(ds.Tables["result"].Rows[0]["ACCESS_LENGTH"].ToString());
                }
                else
                {
                    Writedatalog.WriteLog_Rectube("Not Found HN_LENGTH,LN_LENGTH " + "[" + HN_LENGTH + "]");
                    MessageBox.Show("Error :Patient_insert NO: 2000 Not Found HN_LENGTH in DATABASE \r\nDetail : ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :Patient_insert NO: 2000 Not Found \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form_PATIENT_INSERT_Load(object sender, EventArgs e)
        {
            Query_HNLENGTH();
            textEdit_PatID.Properties.MaxLength = HN_LENGTH;
        }

        private void dropDownButton_Close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
