using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils.Menu;
using System;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;

using UniquePro.Entities.Patient;

namespace UNIQUE.OEM
{
    public partial class Form_REQUEST_NEWORDER : Form
    {

        private string Patnum;
        private string Patname;
        private string PatTitle;
        private string PatBirthdate;
        private string PatSex;
        private string PatAge;
        private string PatDiagnostic;
        private string PatComment;
        private string PatAltnumber;
        private string FormStatus;

        // for modification
        private string StrAccessnum;
        private string StrDoctor;
        private string StrLocation;
        private string StrReqdatetime;
        private string StrColletdatetime;
        private string StrIPDOPD;
        private string StrVip;
        private string StrHosnumber;


        SqlConnection conn;

        System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");

        System.Globalization.CultureInfo cultureinfo_TH = new System.Globalization.CultureInfo("th-TH");

        System.Globalization.CultureInfo cultureinfo_ENG = new System.Globalization.CultureInfo("en-US");

        string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));
        string StrDateTime_Modification_Requests = DateTime.Now.ToString("HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));
        //
        TreeListNode childNode;
        TreeListNode rootNode;

        // Hemo
        object val01;   //TESTCODE
        object val02;   //TESTNAME
        object val03;   //SPECIMENCODE
        object val04;   //SPECIMENNAME
        object val14;   //TESTID
        // Culture
        object val05;   //TESTCODE
        object val06;   //TESTNAME
        object val07;   //SPECIMENCODE
        object val08;   //SPECIMENNAME
        object val15;   //TESTID

        // Gram
        object val09;   //TESTCODE
        object val10;   //TESTNAME
        object val11;   //SPECIMENCODE
        object val12;   //SPECIMENNAME
        object val13;   //PROTOCOL CODE
        object val16;   //TESTID



        // Implement if Have User Login
        // **************************
        string StrUserLogin = "SYS";
        // **************************

        // String This Patient
        string Str_RequestAccess = "";                                  // Request Accessnumber
        string Str_patientPatNumber = "";                               // Patient number
        string Str_patientID = "";                                      // Patient ID of This request
        string Str_patientTitle = "";                                   // Patient Title
        string Str_patientName = "";                                    // Patient Name
        string Str_patientBirthdate = "";                               // Patient Birth Date
        string Str_patientSex = "";                                     // Patient Sex
        string Str_patientAge = "";                                     // Patient Age
        string Str_patientDiagnostic = "";                              // Patient Diagnostic of request
        string Str_patientComment = "";                                 // Patient Comment of request
        string Str_Req_Doctor = "";                                     // Doctor request
        string Str_Req_Location = "";                                   // Location at request
        string Str_Req_specimen = "";                                   // Specimen of Request
        string Str_Req_Urgent = "";                                     // Request Urgent ? S(Stat),A(ASAP),R(Routine)
        string Str_Req_IPDOPD = "";                                     // Request IPD or OPD Select
        int requestID;                                                  // Make RequestID to insert to SP_TESTS.

        // Check if user select Receive specimen when Create Request
        Boolean Check_Receive_specimen;
        string StrVIP = "";
        string StrSecret = "";

        //  Treelist Menu select
        bool DelGram = false;
        bool UnDelGram = false;
        bool DelCulture = false;
        bool DelHemo = false;

        // Datgrid view
        int rowindex = 0;
        bool DelRowDatagrid = false;

        // Counter Add test code
        int AddGram = 0;
        int AddCulture = 0;
        int AddHemo = 0;

        // For Creation
        public Form_REQUEST_NEWORDER(string lblPatnumber, string lblPatTitle, string lblPatname, string lblPatBirthdate, string lblPatSex, string lblPatAge, string lblPatDiagnostic, string Strformstatus)
        {

            InitializeComponent();
            this.Patnum = lblPatnumber;
            this.PatTitle = lblPatTitle;
            this.Patname = lblPatname;
            this.PatBirthdate = lblPatBirthdate;
            this.PatSex = lblPatSex;
            this.PatAge = lblPatAge;
            this.PatDiagnostic = lblPatDiagnostic;
            this.FormStatus = Strformstatus;

        }
        // For Modification
        public Form_REQUEST_NEWORDER(string lblPatnumber, string lblPatTitle, string lblPatname, string lblPatBirthdate, string lblPatSex, string lblPatAge, string lblPatDiagnostic, string Strformstatus, string lblAccessnumber)
        {

            InitializeComponent();
            this.Patnum = lblPatnumber;
            this.PatTitle = lblPatTitle;
            this.Patname = lblPatname;
            this.PatBirthdate = lblPatBirthdate;
            this.PatSex = lblPatSex;
            this.PatAge = lblPatAge;
            this.PatDiagnostic = lblPatDiagnostic;
            this.FormStatus = Strformstatus;

            this.StrAccessnum = lblAccessnumber;

        }

        public Form_REQUEST_NEWORDER(PatientM objPatientM)
        {
            InitializeComponent();
            this.Patnum = objPatientM.PatientNo;
            this.PatTitle = objPatientM.Title1;
            this.Patname = objPatientM.PatientName;
            this.PatBirthdate = objPatientM.BirthDate;
            this.PatSex = objPatientM.Sex;
            this.PatAge = objPatientM.Age;
            this.PatDiagnostic = "";
            this.PatComment = objPatientM.Comment;
            this.PatAltnumber = objPatientM.PatAltnumber;
            this.FormStatus = "Creation";
        }



        private void Form_REQUEST_NEWORDER_Load(object sender, EventArgs e)
        {
            conn = new Connection_ORM().Connect();
            // Status from requests
            lblReqstatusform.Text = FormStatus;

            if (lblReqstatusform.Text == "Creation")
            {
                labelControl_patnum.Text = Patnum;
                labelControl_tltle.Text = PatTitle;
                labelControl_name.Text = Patname;
                labelControl_Birthdate.Text = PatBirthdate;
                labelControl_sex.Text = PatSex;
                labelControl_Age.Text = PatAge;
                labelControl_diag.Text = PatDiagnostic;
                memoEdit_Comments.Text = PatComment;
                textEdit_Altnumber.Text = PatAltnumber;

                // Request select IPD or OPD
                if (radioGroup1.SelectedIndex == 0)
                {
                    Str_Req_IPDOPD = "0";
                }

                // Request Urgent select
                if (radioGroup2.SelectedIndex == 0)
                {
                    Str_Req_Urgent = "0";
                }

                Generate_Accessnumber();
                // Left panel Query
                queryDoctor();
                queryLocation();
                querySpecimen();

                // Right Panel Query
                query_code_GRAM();
                query_code_Culture();
                query_code_Hemoculture();
                CreateColumns(treeList_Gram, null);
                CreateColumns(treeList_Hemo, null);
                CreateColumns(treeList_Culture, null);

                //GetSpecimen();

                // Set Combo Date time
                // Date time Filter
                CultureInfo us = new CultureInfo("en-US");
                dateTimePicker_Req_RequestDate.Value.Date.ToString("yyyy-MM-dd", us);
                timeEdit_Req_RequestTime.Time.ToString("HH:mm:ss");
                dateTimePicker_Req_CollectionDate.Value.Date.ToString("yyyy-MM-dd", us);
                timeEdit_Req_CollectionTime.Time.ToString("HH:mm:ss");

                string strDateTime = DateTime.Now.ToString("HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));


                dateTimePicker_Req_RequestDate.Value = DateTime.Now;
                dateTimePicker_Req_CollectionDate.Text = strDateTime;
                timeEdit_Req_RequestTime.Time = DateTime.Now;
                timeEdit_Req_CollectionTime.Time = DateTime.Now;
            }
            else if (lblReqstatusform.Text == "Modification")
            {
                labelControl_patnum.Text = Patnum;
                labelControl_tltle.Text = PatTitle;
                labelControl_name.Text = Patname;
                labelControl_Birthdate.Text = PatBirthdate;
                labelControl_sex.Text = PatSex;
                labelControl_Age.Text = PatAge;
                labelControl_diag.Text = PatDiagnostic;
                memoEdit_Comments.Text = PatComment;
                textEdit_Altnumber.Text = PatAltnumber;

                // Request select IPD or OPD
                if (radioGroup1.SelectedIndex == 0)
                {
                    Str_Req_IPDOPD = "0";
                }

                // Request Urgent select
                if (radioGroup2.SelectedIndex == 0)
                {
                    Str_Req_Urgent = "0";
                }

                // Left panel Query
                queryDoctor();
                queryLocation();
                querySpecimen();

                // Right Panel Query
                query_code_GRAM();
                query_code_Culture();
                query_code_Hemoculture();
                CreateColumns(treeList_Gram, null);
                CreateColumns(treeList_Hemo, null);
                CreateColumns(treeList_Culture, null);

                // Set Combo Date time
                // Date time Filter

                radioGroup3.Visible = false;
                timeEdit_Req_CollectionTime.Enabled = false;
                dateTimePicker_Req_RequestDate.Enabled = false;
                timeEdit_Req_RequestTime.Enabled = false;

                // Start process detail
                textEdit_accessnumber.Text = StrAccessnum;                  // Goto Query LNum: Mod1001
                Modification_Detail_request(StrAccessnum);
                timer_for_Modification.Enabled = true;


            }
        }

        private void Generate_Accessnumber()
        {
            if (textEdit_accessnumber.Text == "")
            {
                int numLength = Query_Accessnumber_LENGTH();

                int runningNumber = new Nullable<int>(getRunningNumber()).GetValueOrDefault();
                int padLeftZero = Convert.ToInt32(numLength - Convert.ToInt32(runningNumber.ToString().Length));
                string newNumber = runningNumber.ToString().PadLeft(padLeftZero + 1, '0');

                string strYear2Acc = DateTime.Now.ToString("yy", CultureInfo.CreateSpecificCulture("hu-HU"));
                textEdit_accessnumber.Text = strYear2Acc.ToString() + newNumber;

                // Old config setting all cultureinfo to english format
                //
                //DateTime dt = DateTime.Now;
                //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                //textEdit_accessnumber.Text = dt.ToString("yy") + newNumber;
                // Insert new accessnumber to database
                //
                insertNewRunningNumber(newNumber);
            }
            else //edit
            {

            }
        }
        private int getRunningNumber()
        {
            int number = 0;
            string sql = "SELECT COUNTERNUMBER FROM MANAGECOUNTER  ORDER BY COUNTERNUMBER DESC";
            SqlCommand cmd = new SqlCommand(sql, conn);
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            cmd.ExecuteNonQuery();
            adap.Fill(ds, "reqnumber");
            if (ds.Tables["reqnumber"].Rows.Count > 0)
            {
                number = Int32.Parse(ds.Tables["reqnumber"].Rows[0][0].ToString()) + 1;
            }
            return number;
        }

        private void insertNewRunningNumber(string newNumber)
        {
            string sql = "UPDATE MANAGECOUNTER SET COUNTERNUMBER=@COUNTERNUMBER  ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@COUNTERNUMBER", SqlDbType.VarChar).Value = newNumber;
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            cmd.ExecuteNonQuery();
        }

        private int Query_Accessnumber_LENGTH()
        {
            int result = 0;
            try
            {
                string sql_LENGTH = @"SELECT ACCESS_LENGTH FROM DICT_SYSTEM_MB_CONFIG ";
                SqlCommand cmd = new SqlCommand(sql_LENGTH, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_LENGTH");
                cmd.ExecuteReader();
                if (ds.Tables["sql_LENGTH"].Rows.Count > 0)
                {
                    result = Convert.ToInt32(ds.Tables[0].Rows[0]["ACCESS_LENGTH"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: Q1010 :Query Accessnumber Length in system_MB_config ?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        private void queryDoctor()
        {
            try
            {
                string sql_combo = @"SELECT DOCCODE,DOCNAME FROM DICT_DOCTORS ";

                Writedatalog.WriteLog("Combo " + sql_combo);
                SqlCommand cmd = new SqlCommand(sql_combo, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_result");
                cmd.ExecuteReader();

                Writedatalog.WriteLog("Combo " + sql_combo);

                if (ds.Tables["sql_result"].Rows.Count > 0)
                {
                    //comboBoxEdit_doctor.DisplayMember = "DOCCODE";
                    //comboBoxEdit_doctor.ValueMember = "DOCCODE";
                    //DataViewManager dvm = new DataViewManager(ds);
                    gridLookUpEdit_Doctor.Properties.DataSource = ds.Tables["sql_result"];
                    gridLookUpEdit_Doctor.Properties.ValueMember = "DOCCODE";
                    gridLookUpEdit_Doctor.Properties.DisplayMember = "DOCCODE";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: Q1001 :Combo_Search_critiria Select data to Combo ?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void queryLocation()
        {
            try
            {
                string sql_combo = @"SELECT LOCCODE,LOCNAME FROM DICT_LOCATIONS ";

                Writedatalog.WriteLog(strDateTime + " Combo " + sql_combo);
                SqlCommand cmd = new SqlCommand(sql_combo, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_result");
                cmd.ExecuteReader();

                Writedatalog.WriteLog("Combo " + sql_combo);

                if (ds.Tables["sql_result"].Rows.Count > 0)
                {
                    gridLookUpEdit_Location.Properties.DataSource = ds.Tables["sql_result"];
                    gridLookUpEdit_Location.Properties.ValueMember = "LOCCODE";
                    gridLookUpEdit_Location.Properties.DisplayMember = "LOCCODE";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: Q1002 :Combo_Search_critiria Select data to Combo ?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void querySpecimen()
        {
            try
            {
                string sql_combo = @"SELECT COLLMATERIALCODE,COLLMATERIALTEXT,COLLMATERIALID FROM DICT_COLL_MATERIALS";

                Writedatalog.WriteLog(strDateTime + " Combo " + sql_combo);
                SqlCommand cmd = new SqlCommand(sql_combo, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_result");
                cmd.ExecuteReader();

                Writedatalog.WriteLog("Combo " + sql_combo);

                if (ds.Tables["sql_result"].Rows.Count > 0)
                {
                    gridLookUpEdit_Specimen.Properties.DataSource = ds.Tables["sql_result"];
                    gridLookUpEdit2.Properties.DataSource = ds.Tables["sql_result"];
                    gridLookUpEdit2.Properties.DisplayMember = "COLLMATERIALTEXT";
                    gridLookUpEdit_Collec_Culture.Properties.DataSource = ds.Tables["sql_result"];
                    gridLookUpEdit_Collec_Culture.Properties.DisplayMember = "COLLMATERIALTEXT";
                    gridLookUpEdit3.Properties.DataSource = ds.Tables["sql_result"];
                    gridLookUpEdit3.Properties.DisplayMember = "COLLMATERIALTEXT";

                    gridLookUpEdit_Specimen.Properties.DisplayMember = "COLLMATERIALCODE";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: Q1003 :Combo_Search_critiria Select data to Combo ?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void query_code_GRAM()
        {
            try
            {
                string sql_combo = @"SELECT 
 DICT_TESTS.TESTCODE
,DICT_TESTS.TESTNAME 
,DICT_TESTS.TESTID
,DICT_MB_PROTOCOLS.PROTOCOLCODE

FROM DICT_TESTS 
LEFT OUTER JOIN DICT_MB_PROTOCOLS ON DICT_TESTS.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID
WHERE DICT_TESTS.TESTS_TAB='0'";

                Writedatalog.WriteLog(strDateTime + " Combo " + sql_combo);
                SqlCommand cmd = new SqlCommand(sql_combo, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_result");
                cmd.ExecuteReader();

                Writedatalog.WriteLog("Combo " + sql_combo);

                if (ds.Tables["sql_result"].Rows.Count > 0)
                {
                    gridLookUpEdit4.Properties.DataSource = ds.Tables["sql_result"];
                    gridLookUpEdit4.Properties.DisplayMember = "TESTCODE";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: Q1101 :Combo_Search_TESTCODE Gramstain ?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void query_code_Culture()
        {
            try
            {
                string sql_combo = @"SELECT DICT_TESTS.TESTCODE
,DICT_TESTS.TESTNAME  ,DICT_TESTS.TESTID 
,DICT_MB_PROTOCOLS.PROTOCOLCODE
FROM DICT_TESTS 

LEFT OUTER JOIN DICT_MB_PROTOCOLS ON DICT_TESTS.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID
WHERE DICT_TESTS.TESTS_TAB='1'";

                Writedatalog.WriteLog(strDateTime + " Combo " + sql_combo);
                SqlCommand cmd = new SqlCommand(sql_combo, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_result");
                cmd.ExecuteReader();

                Writedatalog.WriteLog("Combo " + sql_combo);

                if (ds.Tables["sql_result"].Rows.Count > 0)
                {
                    gridLookUpEdit_Culture.Properties.DataSource = ds.Tables["sql_result"];
                    gridLookUpEdit_Culture.Properties.DisplayMember = "TESTNAME";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: QCOMBO_L370 :query_code_Culture Culture?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void query_code_Hemoculture()
        {
            try
            {
                string sql_combo = @"SELECT DICT_TESTS.TESTCODE
,DICT_TESTS.TESTNAME  ,DICT_TESTS.TESTID
,DICT_MB_PROTOCOLS.PROTOCOLCODE
FROM DICT_TESTS 

LEFT OUTER JOIN DICT_MB_PROTOCOLS ON DICT_TESTS.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID
WHERE DICT_TESTS.TESTS_TAB='2'";

                Writedatalog.WriteLog(strDateTime + " Combo " + sql_combo);
                SqlCommand cmd = new SqlCommand(sql_combo, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_result");
                cmd.ExecuteReader();

                Writedatalog.WriteLog("Combo " + sql_combo);

                if (ds.Tables["sql_result"].Rows.Count > 0)
                {
                    gridLookUpEdit_Hemo_Single_code.Properties.DataSource = ds.Tables["sql_result"];
                    gridLookUpEdit_Hemo_Single_code.Properties.DisplayMember = "TESTNAME";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: Q1104 :Combo_Search_TESTCODE Hemoculture?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Hemoculture
        private void dropDownButton3_Click(object sender, EventArgs e)
        {
            // Set dummy Count number Code insert in Treelist and Gridview1
            AddHemo++;
            string Str_TESTCODE = "";
            string Str_TESTNAME = "";
            string Str_PROTOCOLCODE = "";

            string Str_SPECIMENNAME = "";
            string Str_SPECIMENCODE = "";


            if (lblReqstatusform.Text == "Creation")
            {
                // REMARK
                // 0 : TextEdit3 = Blank   number per code
                // 1 :  = Test code 
                // 2 :  = Name Code
                // 3 :  = Specimen name
                // 4 :  = Specimen code
                // 5 :  = Protocol code
                // 6 = Request ID
                // 7 = Request ID for Modification
                // 8 = Status for Modification

                if (gridLookUpEdit_Hemo_Single_code.Text != "")
                {
                    if (gridLookUpEdit_Hemo_Single_code.EditValue != null)
                    {
                        DataRowView dv = gridLookUpEdit_Hemo_Single_code.Properties.GetRowByKeyValue(gridLookUpEdit_Hemo_Single_code.EditValue) as DataRowView;
                        Str_TESTCODE = dv["TESTCODE"].ToString();
                        Str_TESTNAME = dv["TESTNAME"].ToString();
                        Str_PROTOCOLCODE = dv["PROTOCOLCODE"].ToString();
                    }

                    TreeListNode parentForRootNodes = null;
                    if (gridLookUpEdit2.Text != "")
                    {
                        if (gridLookUpEdit2.EditValue != null)
                        {
                            DataRowView dv = gridLookUpEdit2.Properties.GetRowByKeyValue(gridLookUpEdit2.EditValue) as DataRowView;
                            Str_SPECIMENNAME = dv["COLLMATERIALTEXT"].ToString();
                            Str_SPECIMENCODE = dv["COLLMATERIALCODE"].ToString();
                        }


                        childNode = treeList_Hemo.AppendNode(new object[] {
                            "",                             // 0
                            textEdit3.Text,                 // 1
                            Str_TESTCODE,                   // 2
                            Str_TESTNAME,                   // 3
                            Str_SPECIMENNAME,               // 4
                            Str_SPECIMENCODE,               // 5
                            Str_PROTOCOLCODE,               // 6
                            "",                             // 7
                            ""                              // 8

                        }, parentForRootNodes);
                    }
                    else
                    {
                        childNode = treeList_Hemo.AppendNode(new object[] {
                            "",                             // 0
                            textEdit3.Text,                 // 1
                            Str_TESTCODE,                   // 2
                            Str_TESTNAME,                   // 3
                            "",                             // 4
                            "",                             // 5
                            Str_PROTOCOLCODE,               // 6
                            "",                             // 7
                            ""                              // 8

                        }, parentForRootNodes);
                    }
                    treeList_Hemo.EndUnboundLoad();
                    treeList_Hemo.ExpandAll();
                }
            }
            else if (lblReqstatusform.Text == "Modification")
            {
                // Grid Add New code 
                try
                {
                    if (gridLookUpEdit_Hemo_Single_code.Text != "")
                    {
                        if (gridLookUpEdit_Hemo_Single_code.EditValue != null)
                        {
                            DataRowView dv = gridLookUpEdit_Hemo_Single_code.Properties.GetRowByKeyValue(gridLookUpEdit_Hemo_Single_code.EditValue) as DataRowView;
                            Str_TESTCODE = dv["TESTCODE"].ToString();
                            Str_TESTNAME = dv["TESTNAME"].ToString();
                            Str_PROTOCOLCODE = dv["PROTOCOLCODE"].ToString();
                        }

                        TreeListNode parentForRootNodes = null;
                        if (gridLookUpEdit2.Text != "")
                        {
                            if (gridLookUpEdit2.EditValue != null)
                            {
                                DataRowView dv = gridLookUpEdit2.Properties.GetRowByKeyValue(gridLookUpEdit2.EditValue) as DataRowView;
                                Str_SPECIMENNAME = dv["COLLMATERIALTEXT"].ToString();
                                Str_SPECIMENCODE = dv["COLLMATERIALCODE"].ToString();
                            }

                            childNode = treeList_Hemo.AppendNode(new object[] {
                                "",                         // 0
                                textEdit3.Text,             // 1
                                Str_TESTCODE,               // 2
                                Str_TESTNAME,               // 3
                                Str_SPECIMENNAME,           // 4
                                Str_SPECIMENCODE,           // 5
                                Str_PROTOCOLCODE,           // 6
                                AddHemo.ToString(),         // 7
                                "New"                       // 8

                            }, parentForRootNodes);
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = Str_TESTCODE;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Str_TESTNAME;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = "New";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = Properties.Resources.addfile_16x16;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = AddHemo.ToString();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value = Str_SPECIMENNAME;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[7].Value = Str_SPECIMENCODE;

                            dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.LightGreen;

                        }
                        else
                        {
                            childNode = treeList_Hemo.AppendNode(new object[] {
                                "",                         // 0
                                textEdit3.Text,             // 1
                                Str_TESTCODE,               // 2
                                Str_TESTNAME,               // 3
                                "",                         // 4
                                "",                         // 5
                                Str_PROTOCOLCODE,           // 6
                                AddHemo.ToString(),         // 7
                                "New"                       // 8

                            }, parentForRootNodes);
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = Str_TESTCODE;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Str_TESTNAME;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = "New";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = Properties.Resources.addfile_16x16;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = AddHemo.ToString();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[7].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.LightGreen;

                        }
                        treeList_Hemo.EndUnboundLoad();
                        treeList_Hemo.ExpandAll();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        // Culture
        private void dropDownButton_Culture_Click(object sender, EventArgs e)
        {
            // Set dummy Count number Code insert in Treelist and Gridview1
            AddCulture++;
            string Str_TESTCODE = "";
            string Str_TESTNAME = "";
            string Str_PROTOCOLCODE = "";

            string Str_SPECIMENNAME = "";
            string Str_SPECIMENCODE = "";


            if (lblReqstatusform.Text == "Creation")
            {
                // REMARK
                // 0 : TextEdit1 = Blank   number per code
                // 1 :  = Test code 
                // 2 :  = Name Code
                // 3 :  = Specimen name
                // 4 :  = Specimen code
                // 5 :  = Protocol code
                // 6 = Request ID
                // 7 = Request ID for Modification
                // 8 = Status for Modification

                if (gridLookUpEdit_Culture.Text != "")
                {
                    if (gridLookUpEdit_Culture.EditValue != null)
                    {
                        DataRowView dv = gridLookUpEdit_Culture.Properties.GetRowByKeyValue(gridLookUpEdit_Culture.EditValue) as DataRowView;
                        Str_TESTCODE = dv["TESTCODE"].ToString();
                        Str_TESTNAME = dv["TESTNAME"].ToString();
                        Str_PROTOCOLCODE = dv["PROTOCOLCODE"].ToString();
                    }

                    TreeListNode parentForRootNodes = null;
                    if (gridLookUpEdit_Collec_Culture.Text != "")
                    {
                        if (gridLookUpEdit_Collec_Culture.EditValue != null)
                        {
                            DataRowView dv = gridLookUpEdit_Collec_Culture.Properties.GetRowByKeyValue(gridLookUpEdit_Collec_Culture.EditValue) as DataRowView;
                            Str_SPECIMENNAME = dv["COLLMATERIALTEXT"].ToString();
                            Str_SPECIMENCODE = dv["COLLMATERIALCODE"].ToString();
                        }

                        childNode = treeList_Culture.AppendNode(new object[] {
                            "",                             // 0
                            textEdit4.Text,                 // 1
                            Str_TESTCODE,                   // 2
                            Str_TESTNAME,                   // 3
                            Str_SPECIMENNAME,               // 4
                            Str_SPECIMENCODE,               // 5
                            Str_PROTOCOLCODE,               // 6
                            "",                             // 7
                            ""                              // 8
                        }, parentForRootNodes);
                    }
                    else
                    {
                        childNode = treeList_Culture.AppendNode(new object[] {
                            "",                             // 0
                            textEdit4.Text,                 // 1
                            Str_TESTCODE,                   // 2
                            Str_TESTNAME,                   // 3
                            "",                             // 4
                            "",                             // 5
                            Str_PROTOCOLCODE,               // 6
                            "",                             // 7
                            ""                              // 8
                        }, parentForRootNodes);
                    }
                    treeList_Culture.EndUnboundLoad();
                    treeList_Culture.ExpandAll();
                }
            }
            else if (lblReqstatusform.Text == "Modification")
            {
                // Grid Add New code 
                try
                {
                    if (gridLookUpEdit_Culture.Text != "")
                    {
                        if (gridLookUpEdit_Culture.EditValue != null)
                        {
                            DataRowView dv = gridLookUpEdit_Culture.Properties.GetRowByKeyValue(gridLookUpEdit_Culture.EditValue) as DataRowView;
                            Str_TESTCODE = dv["TESTCODE"].ToString();
                            Str_TESTNAME = dv["TESTNAME"].ToString();
                            Str_PROTOCOLCODE = dv["PROTOCOLCODE"].ToString();
                        }

                        TreeListNode parentForRootNodes = null;
                        if (gridLookUpEdit_Collec_Culture.Text != "")
                        {
                            if (gridLookUpEdit_Collec_Culture.EditValue != null)
                            {
                                DataRowView dv = gridLookUpEdit_Collec_Culture.Properties.GetRowByKeyValue(gridLookUpEdit_Collec_Culture.EditValue) as DataRowView;
                                Str_SPECIMENNAME = dv["COLLMATERIALTEXT"].ToString();
                                Str_SPECIMENCODE = dv["COLLMATERIALCODE"].ToString();
                            }

                            childNode = treeList_Culture.AppendNode(new object[] {
                                "",                         // 0
                                textEdit4.Text,             // 1
                                Str_TESTCODE,               // 2
                                Str_TESTNAME,               // 3
                                Str_SPECIMENNAME,           // 4
                                Str_SPECIMENCODE,           // 5
                                Str_PROTOCOLCODE,           // 6
                                AddCulture.ToString(),      // 7
                                "New"                       // 8

                            }, parentForRootNodes);
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = Str_TESTCODE;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Str_TESTNAME;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = "New";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = Properties.Resources.addfile_16x16;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = AddCulture.ToString();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value = Str_SPECIMENNAME;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[7].Value = Str_SPECIMENCODE;

                            dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.LightGreen;

                        }
                        else
                        {
                            childNode = treeList_Culture.AppendNode(new object[] {
                                "",                         // 0
                                textEdit4.Text,             // 1
                                Str_TESTCODE,               // 2
                                Str_TESTNAME,               // 3
                                "",                         // 4
                                "",                         // 5
                                Str_PROTOCOLCODE,           // 6
                                AddCulture.ToString(),      // 7
                                "New"                       // 8

                            }, parentForRootNodes);
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = Str_TESTCODE;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Str_TESTNAME;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = "New";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = Properties.Resources.addfile_16x16;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = AddCulture.ToString();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[7].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.LightGreen;

                        }
                        treeList_Culture.EndUnboundLoad();
                        treeList_Culture.ExpandAll();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        // GRAM
        private void dropDownButton6_Click(object sender, EventArgs e)
        {
            try
            {
                // Set dummy Count number Code insert in Treelist and Gridview1
                AddGram++;
                string Str_TESTCODE = "";
                string Str_TESTNAME = "";
                string Str_PROTOCOLCODE = "";

                string Str_SPECIMENNAME = "";
                string Str_SPECIMENCODE = "";

                if (lblReqstatusform.Text == "Creation")
                {
                    // REMARK
                    // 0 : TextEdit1 = Blank   number per code
                    // 1 :  = Test code 
                    // 2 :  = Name Code
                    // 3 :  = Specimen name
                    // 4 :  = Specimen code
                    // 5 :  = Protocol code
                    // 6 = Request ID
                    // 7 = Request ID for Modification
                    // 8 = Status for Modification

                    if (gridLookUpEdit4.Text != "")
                    {
                        if (gridLookUpEdit4.EditValue != null)
                        {
                            DataRowView dv = gridLookUpEdit4.Properties.GetRowByKeyValue(gridLookUpEdit4.EditValue) as DataRowView;
                            Str_TESTCODE = dv["TESTCODE"].ToString();
                            Str_TESTNAME = dv["TESTNAME"].ToString();
                            Str_PROTOCOLCODE = dv["PROTOCOLCODE"].ToString();
                        }

                        TreeListNode parentForRootNodes = null;
                        if (gridLookUpEdit3.Text != "")
                        {
                            if (gridLookUpEdit3.EditValue != null)
                            {
                                DataRowView dv = gridLookUpEdit3.Properties.GetRowByKeyValue(gridLookUpEdit3.EditValue) as DataRowView;
                                Str_SPECIMENNAME = dv["COLLMATERIALTEXT"].ToString();
                                Str_SPECIMENCODE = dv["COLLMATERIALCODE"].ToString();
                            }

                            childNode = treeList_Gram.AppendNode(new object[] {
                            "",                             // 0
                            textEdit1.Text,                 // 1
                            Str_TESTCODE,                   // 2
                            Str_TESTNAME,                   // 3
                            Str_SPECIMENNAME,               // 4
                            Str_SPECIMENCODE,               // 5
                            Str_PROTOCOLCODE,               // 6
                            "",                             // 7
                            ""                              // 8

                        }, parentForRootNodes);
                        }
                        else
                        {
                            childNode = treeList_Gram.AppendNode(new object[] {
                            "",                             // 0
                            textEdit1.Text,                 // 1
                            Str_TESTCODE,                   // 2
                            Str_TESTNAME,                   // 3
                            "",                             // 4
                            "",                             // 5
                            Str_PROTOCOLCODE,               // 6
                            "",                             // 7
                            ""                              // 8
                        }, parentForRootNodes);
                        }
                        treeList_Gram.EndUnboundLoad();
                        treeList_Gram.ExpandAll();
                    }
                }
                else if (lblReqstatusform.Text == "Modification")
                {
                    // Grid Add New code 
                    if (gridLookUpEdit4.Text != "")
                    {
                        if (gridLookUpEdit4.EditValue != null)
                        {
                            DataRowView dv = gridLookUpEdit4.Properties.GetRowByKeyValue(gridLookUpEdit4.EditValue) as DataRowView;
                            Str_TESTCODE = dv["TESTCODE"].ToString();
                            Str_TESTNAME = dv["TESTNAME"].ToString();
                            Str_PROTOCOLCODE = dv["PROTOCOLCODE"].ToString();
                        }

                        TreeListNode parentForRootNodes = null;
                        if (gridLookUpEdit3.Text != "")
                        {
                            if (gridLookUpEdit3.EditValue != null)
                            {
                                DataRowView dv = gridLookUpEdit3.Properties.GetRowByKeyValue(gridLookUpEdit3.EditValue) as DataRowView;
                                Str_SPECIMENNAME = dv["COLLMATERIALTEXT"].ToString();
                                Str_SPECIMENCODE = dv["COLLMATERIALCODE"].ToString();
                            }

                            childNode = treeList_Gram.AppendNode(new object[] {
                                "",                         // 0
                                textEdit1.Text,             // 1
                                Str_TESTCODE,               // 2
                                Str_TESTNAME,               // 3
                                Str_SPECIMENNAME,           // 4
                                Str_SPECIMENCODE,           // 5
                                Str_PROTOCOLCODE,           // 6
                                AddGram.ToString(),         // 7
                                "New"                       // 8

                            }, parentForRootNodes);
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = Str_TESTCODE;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Str_TESTNAME;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = "New";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = Properties.Resources.addfile_16x16;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = AddGram.ToString();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value = Str_SPECIMENNAME;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[7].Value = Str_SPECIMENCODE;

                            dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.LightGreen;

                        }
                        else
                        {
                            childNode = treeList_Gram.AppendNode(new object[] {
                                "",                         // 0
                                textEdit1.Text,             // 1
                                Str_TESTCODE,               // 2
                                Str_TESTNAME,               // 3
                                "",                         // 4
                                "",                         // 5
                                Str_PROTOCOLCODE,           // 6
                                AddGram.ToString(),         // 7
                                "New"                       // 8

                            }, parentForRootNodes);
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = Str_TESTCODE;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Str_TESTNAME;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = "New";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3].Value = Properties.Resources.addfile_16x16;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4].Value = AddGram.ToString();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[6].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[7].Value = "";
                            dataGridView1.Rows[dataGridView1.RowCount - 1].DefaultCellStyle.BackColor = Color.LightGreen;

                        }
                        treeList_Gram.EndUnboundLoad();
                        treeList_Gram.ExpandAll();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CreateColumns(TreeList tl, DataSet ds)
        {
            RepositoryItemComboBox riCombo = new RepositoryItemComboBox();
            //riCombo.Items.AddRange(new string[] { "Specimen1", "Specimen2", "Specimen3", "Specimen4", "Specimen5", "Specimen6", "Specimen7" });

            // Create three columns.
            tl.BeginUpdate();
            tl.Columns.Add();

            TreeListColumn col1 = tl.Columns.Add();
            col1.Caption = "Amout";
            col1.VisibleIndex = 1;
            col1.Width = 80;
            col1.OptionsColumn.AllowEdit = true;
            TreeListColumn col2 = tl.Columns.Add();
            col2.Caption = "<i>Test</i><b>Code</b>";
            col2.VisibleIndex = 2;
            col2.Width = 80;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            col2.OptionsColumn.AllowEdit = false;
            TreeListColumn col3 = tl.Columns.Add();
            col3.Caption = "<i>Test</i><b>Name</b>";
            col3.VisibleIndex = 3;
            col3.Width = 190;
            col3.OptionsColumn.AllowEdit = false;
            TreeListColumn col4 = tl.Columns.Add();
            col4.Caption = "Specimen name";
            col4.VisibleIndex = 4;
            col4.Width = 190;
            col4.OptionsColumn.AllowEdit = false;
            //col4.ColumnEdit = riCombo;
            TreeListColumn col5 = tl.Columns.Add();
            col5.Caption = "SpmCode";
            col5.VisibleIndex = 5;
            col5.Width = 90;
            col5.OptionsColumn.AllowEdit = false;
            TreeListColumn col6 = tl.Columns.Add();
            col6.Caption = "Protocol";
            col6.VisibleIndex = 6;
            col6.Width = 50;
            col6.OptionsColumn.AllowEdit = false;
            // 7. ช่องนี้ ใช้เพื่อเปรียบเทีย Request ID ตั้งแต่ตอน Request ครั้งแรก และ เพื่อ เทียบ ตอน แก้ไข Request
            TreeListColumn col7 = tl.Columns.Add();
            col7.Caption = "REQTESTID";
            col7.VisibleIndex = 7;
            col7.Width = 50;
            col7.Visible = false;
            col7.OptionsColumn.AllowEdit = false;
            // 8. ช่องนี้ ใช้สำหรับ การเพิ่ม testcode และการ Modification Request นอกนั้นไม่ใช้งาน เป็นค่าว่าง ๆ
            TreeListColumn col8 = tl.Columns.Add();
            col8.Caption = "MODSTATUS";
            col8.VisibleIndex = 8;
            col8.Width = 10;
            col8.Visible = false;
            col8.OptionsColumn.AllowEdit = false;

            //col5.ColumnEdit = riCombo;

            tl.EndUpdate();
        }

        // Tool ForDelete Node
        private void treeList_Gram_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            // Check if a node's indicator cell is clicked.
            TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(e.Point);
            TreeListNode node = null;
            if (hitInfo.HitInfoType == HitInfoType.RowIndicator)
            {
                node = hitInfo.Node;
            }
            if (node == null) return;
            // Create the Delete Node command.
            //DXMenuItem menuItem = new DXMenuItem("Delete Node", new EventHandler(deleteNodeMenuItemClick));
            //menuItem.Tag = node;
            //e.Menu.Items.Add(menuItem);

        }
        private void treeList2_PopupMenuShowing(object sender, DevExpress.XtraTreeList.PopupMenuShowingEventArgs e)
        {
            // Check if a node's indicator cell is clicked.
            TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(e.Point);
            TreeListNode node = null;
            if (hitInfo.HitInfoType == HitInfoType.RowIndicator)
            {
                node = hitInfo.Node;
            }
            if (node == null) return;
            // Create the Delete Node command.
            //DXMenuItem menuItem = new DXMenuItem("Delete Node", new EventHandler(deleteNodeMenuItemClick));
            //menuItem.Tag = node;
            //e.Menu.Items.Add(menuItem);
        }
        private void treeList_Culture_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            // Check if a node's indicator cell is clicked.
            TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(e.Point);
            TreeListNode node = null;
            if (hitInfo.HitInfoType == HitInfoType.RowIndicator)
            {
                node = hitInfo.Node;
            }
            if (node == null) return;
            // Create the Delete Node command.
            //DXMenuItem menuItem = new DXMenuItem("Delete Node", new EventHandler(deleteNodeMenuItemClick));
            //menuItem.Tag = node;
            //e.Menu.Items.Add(menuItem);
        }
        private void treeList_Gram_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                TreeList treeList = sender as TreeList;
                TreeListHitInfo info = treeList.CalcHitInfo(e.Location);
                if (info.Node != null)
                {
                    treeList_Gram.FocusedNode = info.Node;
                    if (treeList_Gram.Nodes.Count > 0)
                    {
                        this.contextMenuStrip_delete.Show(this.treeList_Gram, e.Location);
                        DelGram = true;
                        UnDelGram = true;
                        contextMenuStrip_delete.Show(Cursor.Position);
                    }
                }
            }
        }

        private void treeList_Culture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                TreeList treeList = sender as TreeList;
                TreeListHitInfo info = treeList.CalcHitInfo(e.Location);
                if (info.Node != null)
                {
                    treeList_Culture.FocusedNode = info.Node;

                    if (treeList_Culture.Nodes.Count > 0)
                    {
                        this.contextMenuStrip_delete.Show(this.treeList_Culture, e.Location);
                        DelCulture = true;
                        contextMenuStrip_delete.Show(Cursor.Position);
                    }
                }
            }
        }

        private void treeList_Hemo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                TreeList treeList = sender as TreeList;
                TreeListHitInfo info = treeList.CalcHitInfo(e.Location);
                if (info.Node != null)
                {
                    treeList_Hemo.FocusedNode = info.Node;

                    if (treeList_Hemo.Nodes.Count > 0)
                    {
                        this.contextMenuStrip_delete.Show(this.treeList_Hemo, e.Location);
                        DelHemo = true;
                        contextMenuStrip_delete.Show(Cursor.Position);
                    }
                }
            }
        }

        private void deleteNodeMenuItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null) return;
            TreeListNode node = item.Tag as TreeListNode;
            if (node == null) return;
            node.TreeList.DeleteNode(node);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DelGram == true)   // Click delete
                {
                    string CellREQTESTID = "";
                    string CellREQModify = "";
                    CellREQTESTID = treeList_Gram.FocusedNode.GetValue(7).ToString();       // GET REQUESTID
                    TreeListColumn ColModi = treeList_Gram.Columns[8];
                    CellREQModify = treeList_Gram.FocusedNode.GetDisplayText(ColModi);       // GET Status for Modify

                    if (lblReqstatusform.Text == "Creation")
                    {
                        TreeListNode node = treeList_Gram.FocusedNode;
                        if (node.ParentNode != null)
                            node.ParentNode.Nodes.Remove(node);
                        else
                            treeList_Gram.Nodes.Remove(node);
                    }
                    else if (lblReqstatusform.Text == "Modification")
                    {
                        if (CellREQModify == "New")
                        {
                            TreeListNode node = treeList_Gram.FocusedNode;
                            if (node.ParentNode != null)
                                node.ParentNode.Nodes.Remove(node);
                            else
                                treeList_Gram.Nodes.Remove(node);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    string Cell4 = row.Cells[4].Value.ToString();

                                    if (Cell4 == CellREQTESTID)
                                    {
                                        if (row.Cells[2].Value.ToString() == "New")
                                        {
                                            dataGridView1.Rows.Remove(row);
                                        }
                                        else
                                        {
                                            row.Cells[3].Value = Properties.Resources.cancel_16x16;
                                            row.Cells[5].Value = "ลบ";
                                            row.DefaultCellStyle.BackColor = Color.LightPink;
                                            dataGridView1.Refresh();
                                        }
                                    }
                                }
                            }
                        }
                        else if (CellREQModify == "")
                        {
                            TreeListNode node = treeList_Gram.FocusedNode;
                            node.CheckState = CheckState.Unchecked;
                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    string Cell4 = row.Cells[4].Value.ToString();

                                    if (Cell4 == CellREQTESTID)
                                    {
                                            row.Cells[3].Value = Properties.Resources.cancel_16x16;
                                            row.Cells[5].Value = "ลบ";
                                            row.DefaultCellStyle.BackColor = Color.LightPink;
                                            dataGridView1.Refresh();
                                    }
                                }
                            }
                        }
                        DelGram = false;
                    }
                }
                else if (DelCulture == true)
                {
                    string CellREQTESTID = "";
                    string CellREQModify = "";
                    CellREQTESTID = treeList_Culture.FocusedNode.GetValue(7).ToString();       // GET REQUESTID
                    TreeListColumn ColModi = treeList_Culture.Columns[8];
                    CellREQModify = treeList_Culture.FocusedNode.GetDisplayText(ColModi);       // GET Status for Modify

                    if (lblReqstatusform.Text == "Creation")
                    {
                        TreeListNode node = treeList_Culture.FocusedNode;
                        if (node.ParentNode != null)
                            node.ParentNode.Nodes.Remove(node);
                        else
                            treeList_Culture.Nodes.Remove(node);
                    }
                    else if (lblReqstatusform.Text == "Modification")
                    {
                        if (CellREQModify == "New")
                        {
                            TreeListNode node = treeList_Culture.FocusedNode;
                            if (node.ParentNode != null)
                                node.ParentNode.Nodes.Remove(node);
                            else
                                treeList_Culture.Nodes.Remove(node);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    string Cell4 = row.Cells[4].Value.ToString();

                                    if (Cell4 == CellREQTESTID)
                                    {
                                        if (row.Cells[2].Value.ToString() == "New")
                                        {
                                            dataGridView1.Rows.Remove(row);
                                        }
                                        else
                                        {
                                            row.Cells[3].Value = Properties.Resources.cancel_16x16;
                                            row.Cells[5].Value = "ลบ";
                                            row.DefaultCellStyle.BackColor = Color.LightPink;
                                            dataGridView1.Refresh();
                                        }
                                    }
                                }
                            }
                        }
                        else if (CellREQModify == "")
                        {
                            TreeListNode node = treeList_Culture.FocusedNode;
                            node.CheckState = CheckState.Unchecked;
                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    string Cell4 = row.Cells[4].Value.ToString();

                                    if (Cell4 == CellREQTESTID)
                                    {
                                        row.Cells[3].Value = Properties.Resources.cancel_16x16;
                                        row.Cells[5].Value = "ลบ";
                                        row.DefaultCellStyle.BackColor = Color.LightPink;
                                        dataGridView1.Refresh();
                                    }
                                }
                            }
                        }
                        DelCulture = false;
                    }
                }
                else if (DelHemo == true)
                {

                    string CellREQTESTID = "";
                    string CellREQModify = "";
                    CellREQTESTID = treeList_Hemo.FocusedNode.GetValue(7).ToString();       // GET REQUESTID
                    TreeListColumn ColModi = treeList_Hemo.Columns[8];
                    CellREQModify = treeList_Hemo.FocusedNode.GetDisplayText(ColModi);       // GET Status for Modify

                    if (lblReqstatusform.Text == "Creation")
                    {
                        TreeListNode node = treeList_Hemo.FocusedNode;
                        if (node.ParentNode != null)
                            node.ParentNode.Nodes.Remove(node);
                        else
                            treeList_Hemo.Nodes.Remove(node);
                    }
                    else if (lblReqstatusform.Text == "Modification")
                    {
                        if (CellREQModify == "New")
                        {
                            TreeListNode node = treeList_Hemo.FocusedNode;
                            if (node.ParentNode != null)
                                node.ParentNode.Nodes.Remove(node);
                            else
                                treeList_Hemo.Nodes.Remove(node);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    string Cell4 = row.Cells[4].Value.ToString();

                                    if (Cell4 == CellREQTESTID)
                                    {
                                        if (row.Cells[2].Value.ToString() == "New")
                                        {
                                            dataGridView1.Rows.Remove(row);
                                        }
                                        else
                                        {
                                            row.Cells[3].Value = Properties.Resources.cancel_16x16;
                                            row.Cells[5].Value = "ลบ";
                                            row.DefaultCellStyle.BackColor = Color.LightPink;
                                            dataGridView1.Refresh();
                                        }
                                    }
                                }
                            }
                        }
                        else if (CellREQModify == "")
                        {
                            TreeListNode node = treeList_Hemo.FocusedNode;
                            node.CheckState = CheckState.Unchecked;
                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    string Cell4 = row.Cells[4].Value.ToString();

                                    if (Cell4 == CellREQTESTID)
                                    {
                                        row.Cells[3].Value = Properties.Resources.cancel_16x16;
                                        row.Cells[5].Value = "ลบ";
                                        row.DefaultCellStyle.BackColor = Color.LightPink;
                                        dataGridView1.Refresh();
                                    }
                                }
                            }
                        }
                        DelHemo = false;
                    }
                }
                else if (DelRowDatagrid == true)
                {
                    if (!this.dataGridView1.Rows[this.rowindex].IsNewRow)
                    {
                        //this.dataGridView1.Rows.RemoveAt(this.rowindex);
                        // Change status to canecl picture

                        // Set REQUESTIT to Varaible
                        string Strcell_RequestID = dataGridView1.Rows[this.rowindex].Cells[4].Value.ToString();

                        dataGridView1.Rows[this.rowindex].Cells[3].Value = Properties.Resources.cancel_16x16;
                        dataGridView1.Rows[this.rowindex].Cells[5].Value = "ลบ";
                        dataGridView1.Rows[this.rowindex].DefaultCellStyle.BackColor = Color.LightPink;
                        dataGridView1.Refresh();

                        // Search in Tree list and change value checkbox
                        for (int i = 0; i < treeList_Gram.AllNodesCount; i++)
                        {
                            string StrRequestID = treeList_Gram.Nodes[i][7].ToString();                                           // Get Request ID 
                            if (StrRequestID == Strcell_RequestID)
                            {
                                treeList_Gram.Nodes[i].CheckState = CheckState.Unchecked;
                            }
                        }
                        for (int i = 0; i < treeList_Culture.AllNodesCount; i++)
                        {
                            string StrRequestID = treeList_Culture.Nodes[i][7].ToString();                                        // Get Request ID 
                            if (StrRequestID == Strcell_RequestID)
                            {
                                treeList_Culture.Nodes[i].CheckState = CheckState.Unchecked;
                            }
                        }
                        for (int i = 0; i < treeList_Hemo.AllNodesCount; i++)
                        {
                            string StrRequestID = treeList_Hemo.Nodes[i][7].ToString();                                           // Get Request ID 
                            if (StrRequestID == Strcell_RequestID)
                            {
                                treeList_Hemo.Nodes[i].CheckState = CheckState.Unchecked;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UndeleteMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (UnDelGram == true)   // Click delete
                {
                    if (lblReqstatusform.Text == "Modification")
                    {
                        if (!this.dataGridView1.Rows[this.rowindex].IsNewRow)
                        {
                            //this.dataGridView1.Rows.RemoveAt(this.rowindex);
                            // Change status to canecl picture
                            // Set REQUESTIT to Varaible
                            string Strcell_RequestID = dataGridView1.Rows[this.rowindex].Cells[4].Value.ToString();

                            dataGridView1.Rows[this.rowindex].Cells[3].Value = Properties.Resources.accept_icon16x16;
                            dataGridView1.Rows[this.rowindex].Cells[5].Value = "";
                            dataGridView1.Rows[this.rowindex].DefaultCellStyle.BackColor = Color.White;
                            dataGridView1.Refresh();

                            // Search in Tree list and change value checkbox
                            for (int i = 0; i < treeList_Gram.AllNodesCount; i++)
                            {
                                string StrRequestID = treeList_Gram.Nodes[i][7].ToString();                                           // Get Request ID 
                                if (StrRequestID == Strcell_RequestID)
                                {
                                    treeList_Gram.Nodes[i].CheckState = CheckState.Checked;
                                }
                            }
                            for (int i = 0; i < treeList_Culture.AllNodesCount; i++)
                            {
                                string StrRequestID = treeList_Culture.Nodes[i][7].ToString();                                        // Get Request ID 
                                if (StrRequestID == Strcell_RequestID)
                                {
                                    treeList_Culture.Nodes[i].CheckState = CheckState.Checked;
                                }
                            }
                            for (int i = 0; i < treeList_Hemo.AllNodesCount; i++)
                            {
                                string StrRequestID = treeList_Hemo.Nodes[i][7].ToString();                                           // Get Request ID 
                                if (StrRequestID == Strcell_RequestID)
                                {
                                    treeList_Hemo.Nodes[i].CheckState = CheckState.Checked;
                                }
                            }

                        }
                        UnDelGram = false;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView1.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip_delete.Show(this.dataGridView1, e.Location);
                        DelRowDatagrid = true;
                        contextMenuStrip_delete.Show(Cursor.Position);
                    }
                }
            }
        }
        // End Tool For Delete Node

        // GRAM 
        private void gridLookUpEdit4_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = gridLookUpEdit4.Properties.View as DevExpress.XtraGrid.Views.Grid.GridView;
            val09 = view.GetRowCellValue(view.FocusedRowHandle, "TESTCODE");
            val10 = view.GetRowCellValue(view.FocusedRowHandle, "TESTNAME");
            val13 = view.GetRowCellValue(view.FocusedRowHandle, "PROTOCOLCODE");
        }
        private void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = gridLookUpEdit_Hemo_Single_code.Properties.View as DevExpress.XtraGrid.Views.Grid.GridView;
            val01 = view.GetRowCellValue(view.FocusedRowHandle, "TESTCODE");
            val02 = view.GetRowCellValue(view.FocusedRowHandle, "TESTNAME");
        }
        private void gridLookUpEdit_Culture_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = gridLookUpEdit_Culture.Properties.View as DevExpress.XtraGrid.Views.Grid.GridView;
            val05 = view.GetRowCellValue(view.FocusedRowHandle, "TESTCODE");
            val06 = view.GetRowCellValue(view.FocusedRowHandle, "TESTNAME");
        }

        private void gridLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = gridLookUpEdit2.Properties.View as DevExpress.XtraGrid.Views.Grid.GridView;
            val03 = view.GetRowCellValue(view.FocusedRowHandle, "COLLMATERIALTEXT");
            val04 = view.GetRowCellValue(view.FocusedRowHandle, "COLLMATERIALCODE");
        }
        private void gridLookUpEdit_Collec_Culture_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = gridLookUpEdit_Collec_Culture.Properties.View as DevExpress.XtraGrid.Views.Grid.GridView;
            val07 = view.GetRowCellValue(view.FocusedRowHandle, "COLLMATERIALTEXT");
            val08 = view.GetRowCellValue(view.FocusedRowHandle, "COLLMATERIALCODE");
        }
        private void gridLookUpEdit3_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = gridLookUpEdit3.Properties.View as DevExpress.XtraGrid.Views.Grid.GridView;
            val11 = view.GetRowCellValue(view.FocusedRowHandle, "COLLMATERIALTEXT");
            val12 = view.GetRowCellValue(view.FocusedRowHandle, "COLLMATERIALCODE");
        }

        private void dropDownButton9_Click(object sender, EventArgs e)
        {
            gridLookUpEdit_Hemo_Single_code.Text = "";
            gridLookUpEdit2.Text = "";
        }

        private void dropDownButton10_Click(object sender, EventArgs e)
        {
            gridLookUpEdit_Culture.Text = "";
            gridLookUpEdit_Collec_Culture.Text = "";
        }
        private void dropDownButton11_Click(object sender, EventArgs e)
        {
            gridLookUpEdit4.Text = "";
            gridLookUpEdit3.Text = "";
        }

        private void dropDownButton5_Click(object sender, EventArgs e)
        {
            if (lblReqstatusform.Text == "Creation")
            {
                checkBox4.Checked = false;
                treeList_Gram.ClearNodes();
            }
            else if (lblReqstatusform.Text == "Modification")
            {


            }
        }

        private void dropDownButton4_Click(object sender, EventArgs e)
        {
            if (lblReqstatusform.Text == "Creation")
            {

                checkBox_For_treelist2.Checked = false;
                treeList_Hemo.ClearNodes();
            }
            else if (lblReqstatusform.Text == "Modification")
            {

            }
        }
            private void dropDownButton1_Click(object sender, EventArgs e)
        {
            if (lblReqstatusform.Text == "Creation")
            {
                checkBox6.Checked = false;
                treeList_Culture.ClearNodes();
            }
            else if (lblReqstatusform.Text == "Modification")
            {

            }
        }

            private void checkBox_For_treelist2_CheckedChanged(object sender, EventArgs e)
        {
            if (lblReqstatusform.Text == "Creation")
            {
                if (treeList_Hemo.AllNodesCount > 0)
                {
                    if (checkBox_For_treelist2.Checked == true)
                    {
                        foreach (var node in treeList_Hemo.Nodes.OfType<TreeListNode>())
                        {
                            treeList_Hemo.SetNodeCheckState(node, CheckState.Checked);
                        }
                    }
                    else
                    {
                        foreach (var node in treeList_Hemo.Nodes.OfType<TreeListNode>())
                        {
                            treeList_Hemo.SetNodeCheckState(node, CheckState.Unchecked);
                        }
                    }
                }
                else
                {
                    checkBox_For_treelist2.Checked = false;
                }
            }
            //else if (lblReqstatusform.Text == "Modification")
            //{
            //    if (checkBox_For_treelist2.Checked == true)
            //    {
            //        foreach (var node in treeList_Hemo.Nodes.OfType<TreeListNode>())
            //        {
            //            int i = Convert.ToInt32(node);
            //            treeList_Hemo.SetNodeCheckState(node, CheckState.Checked);

            //            string CellREQTESTID = treeList_Gram.Nodes[i].GetDisplayText(6);

            //            foreach (DataGridViewRow row in dataGridView1.Rows)
            //            {
            //                string Cell4 = row.Cells[4].Value.ToString();

            //                if (Cell4 == CellREQTESTID)
            //                {
            //                    row.Cells[3].Value = Properties.Resources.close_16x16;
            //                    dataGridView1.Refresh();
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        foreach (var node in treeList_Hemo.Nodes.OfType<TreeListNode>())
            //        {
            //            treeList_Hemo.SetNodeCheckState(node, CheckState.Unchecked);
            //        }
            //    }
            //}

        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (treeList_Culture.AllNodesCount > 0)
            {
                if (checkBox6.Checked == true)
                {
                    foreach (var node in treeList_Culture.Nodes.OfType<TreeListNode>())
                    {
                        treeList_Culture.SetNodeCheckState(node, CheckState.Checked);
                    }
                }
                else
                {
                    foreach (var node in treeList_Culture.Nodes.OfType<TreeListNode>())
                    {
                        treeList_Culture.SetNodeCheckState(node, CheckState.Unchecked);
                    }
                }
            }
            else
            {
                checkBox6.Checked = false;
            }

        }

        // TAB GRAM
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (lblReqstatusform.Text == "Creation")
            {

                if (treeList_Gram.AllNodesCount > 0)
                {
                    if (checkBox4.Checked == true)
                    {
                        foreach (var node in treeList_Gram.Nodes.OfType<TreeListNode>())
                        {
                            treeList_Gram.SetNodeCheckState(node, CheckState.Checked);
                        }
                    }
                    else
                    {
                        foreach (var node in treeList_Gram.Nodes.OfType<TreeListNode>())
                        {
                            treeList_Gram.SetNodeCheckState(node, CheckState.Unchecked);
                        }
                    }
                }
                else
                {
                    checkBox4.Checked = false;
                }
            }
            else if (lblReqstatusform.Text == "Modification")
            {
                try
                {
                    if (checkBox4.Checked == true)
                    {
                        foreach (var node in treeList_Gram.Nodes.OfType<TreeListNode>())
                        {
                            int i = 0;
                            treeList_Gram.SetNodeCheckState(node, CheckState.Checked);

                            string CellREQTESTID = treeList_Gram.Nodes[i].GetDisplayText(6);

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                string Cell4 = row.Cells[4].Value.ToString();

                                if (Cell4 == CellREQTESTID)
                                {
                                    row.Cells[3].Value = Properties.Resources.accept_icon16x16;
                                    dataGridView1.Refresh();
                                }
                            }
                            i++;
                        }
                    }
                    else
                    {
                        foreach (var node in treeList_Gram.Nodes.OfType<TreeListNode>())
                        {
                            treeList_Gram.SetNodeCheckState(node, CheckState.Unchecked);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void checkBox_Hemo_single_CheckedChanged(object sender, EventArgs e)
        {
            // Autoselect Single test & profile
            if (checkBox_Hemo_single.Checked == true)
            {
                checkBox_Hemo_Profile.Checked = false;
                gridLookUpEdit_Hemo_Single_code.Visible = true;
                gridLookUpEdit_Hemo_profile.Visible = false;
            }
            else if (checkBox_Hemo_single.Checked == false)
            {
                checkBox_Hemo_Profile.Checked = true;
                gridLookUpEdit_Hemo_Single_code.Visible = false;
                gridLookUpEdit_Hemo_profile.Visible = true;
            }
        }

        private void checkBox_Hemo_Profile_CheckedChanged(object sender, EventArgs e)
        {
            // Autoselect Single test & profile
            if (checkBox_Hemo_Profile.Checked == true)
            {
                checkBox_Hemo_single.Checked = false;
                gridLookUpEdit_Hemo_profile.Visible = true;
                gridLookUpEdit_Hemo_Single_code.Visible = false;

            }
            else if (checkBox_Hemo_Profile.Checked == false)
            {
                checkBox_Hemo_single.Checked = true;
                gridLookUpEdit_Hemo_profile.Visible = false;
                gridLookUpEdit_Hemo_Single_code.Visible = true;

            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            // Autoselect Single test & profile
            if (checkBox5.Checked == true)
            {
                checkBox1.Checked = false;
                gridLookUpEdit_Culture.Visible = true;
                gridLookUpEdit1.Visible = false;
            }
            else if (checkBox5.Checked == false)
            {
                checkBox1.Checked = true;
                gridLookUpEdit_Culture.Visible = false;
                gridLookUpEdit1.Visible = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Autoselect Single test & profile
            if (checkBox1.Checked == true)
            {
                checkBox5.Checked = false;
                gridLookUpEdit1.Visible = true;
                gridLookUpEdit_Culture.Visible = false;

            }
            else if (checkBox1.Checked == false)
            {
                checkBox5.Checked = true;
                gridLookUpEdit1.Visible = false;
                gridLookUpEdit_Culture.Visible = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            // Autoselect Single test & profile
            if (checkBox3.Checked == true)
            {
                checkBox2.Checked = false;
                gridLookUpEdit4.Visible = true;
                gridLookUpEdit7.Visible = false;
            }
            else if (checkBox3.Checked == false)
            {
                checkBox2.Checked = true;
                gridLookUpEdit4.Visible = false;
                gridLookUpEdit7.Visible = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            // Autoselect Single test & profile
            if (checkBox2.Checked == true)
            {
                checkBox3.Checked = false;
                gridLookUpEdit7.Visible = true;
                gridLookUpEdit4.Visible = false;

            }
            else if (checkBox2.Checked == false)
            {
                checkBox3.Checked = true;
                gridLookUpEdit7.Visible = false;
                gridLookUpEdit4.Visible = true;
            }
        }

        private void Loop_save_Gram(string StrRequestDT, string StrCollectionDT)
        {
            int TESTORDER = 1;
            int StrspecimenID = 0;
            if (treeList_Gram.AllNodesCount > 0)
            {
                try
                {
                    for (int i = 0; i < treeList_Gram.AllNodesCount; i++)
                    {
                        if (treeList_Gram.Nodes[i].CheckState == CheckState.Checked)
                        {
                            string Protocal_LENGTH = "";
                            int IntAmout = Convert.ToInt32(treeList_Gram.Nodes[i].GetValue(1).ToString());
                            string StrTestcode = treeList_Gram.Nodes[i][2].ToString();                                           // Test code 
                            string StrTestname = treeList_Gram.Nodes[i][3].ToString();                                           // Test Name
                            string StrSpecimenname = treeList_Gram.Nodes[i][4].ToString();                                       // Specimen name
                            string StrSpecimencode = treeList_Gram.Nodes[i][5].ToString();                                       // Specimen Code
                            string StrCollected = strDateTime;                                                                   // DT Receive Create request

                            if (StrSpecimencode != "")
                            {
                                StrspecimenID = Query_specimenID(StrSpecimencode);
                            }

                            if (IntAmout > 1)
                            {
                                for (int I = 0; I < IntAmout; I++)
                                {
                                    // ############Start Get Protocol && Number Running of Protocol
                                    //
                                    // Query Protocol ID from StrTestcode
                                    string[] Chk_ProtocolID = Query_PROTOCOL(StrTestcode);
                                    // Query Length From Chk_ProtocolID;
                                    Protocal_LENGTH = Chk_Length();
                                    // Get Number Protocal from Chk_ProtocolID
                                    int RUNNINGPROTOCOL = new Nullable<int>(GETNUMBERPROTOCOL(Chk_ProtocolID[0])).GetValueOrDefault();
                                    // Get LENGTH - NUMBER ---> Example
                                    // Number LENGTH = 8
                                    // Number Protocal now = 1
                                    // Equal = [8 - 1 = 7]
                                    int PADLEFTZERO = Convert.ToInt32(Protocal_LENGTH) - (Convert.ToInt32(RUNNINGPROTOCOL.ToString().Length));
                                    // Set Newnumber of Protocol = 123 + 1 = 00000123
                                    string NEWPROTOCAL = RUNNINGPROTOCOL.ToString().PadLeft(PADLEFTZERO + 1, '0');
                                    int Lengcount = NEWPROTOCAL.Length;
                                    int NowLengthProtocol = Convert.ToInt32(Protocal_LENGTH);
                                    if (Lengcount > NowLengthProtocol)
                                    {
                                        Update_NEW_LENGTH(Lengcount, Chk_ProtocolID[0]);
                                        Writedatalog.WriteLog(strDateTime + " Log:REQHEMO3003 Update Length in PROTOCOL_NUMBER : Protocol Length count = " + Lengcount + " and Old Protocol LENGTH = " + NowLengthProtocol);
                                    }
                                    // Update Protocol Number in DATABASE and General number
                                    Update_New_PROTOCOL_NUMBER(NEWPROTOCAL, Chk_ProtocolID[0]);
                                    string NewProtocol_num = Chk_ProtocolID[1] + NEWPROTOCAL;

                                    // ใช้ Boolean เช็ค ว่า จะรอรับ หรือ รับเลย
                                    // แตกต่างกันที่ Database table ที่ จะ Insert 
                                    // ถ้า รับเลย จะนำเข้าที่ REQUESTS,REQ_TESTS,MB_REQUESTS,SUBREQMB_STAIN
                                    // ถ้า รอรับ จะนำเข้าที่ SP_REQUESTS,SP_TESTS เพื่อรอรับ Specimen ต่อไปกรณีที่ Speimen ยังไม่มา สามารถเลือกรับได้ใน แต่ละ Request ใน เมนู Specimen receive
                                    //
                                    // 
                                    if (Check_Receive_specimen == false)      // false
                                    {
                                        // ###########End Query Running && protocol number.
                                        // ###########
                                        // ###########Start Loop Insert if Amount > 1
                                        // Insert Table REQUESTS Step 2
                                        // Table refer REQUESTS
                                        // Table refer REQ_TESTS
                                        //
                                        Insert_Into_REQUESTS_Step2(Str_patientID, Str_RequestAccess, StrRequestDT, StrspecimenID, StrCollectionDT, Chk_ProtocolID[2], StrUserLogin);
                                        // Step ถัดมา คือ Table 2 Table MB_REQUESTS , SUBREQMG_STAINS
                                        // Step 1. Insert MB_REQUESTS with Protocal number
                                        // 
                                        // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                                        // 
                                        Insert_Into_MB_REQUESTS(Str_RequestAccess, Chk_ProtocolID[2], Chk_ProtocolID[0], NewProtocol_num, StrRequestDT, StrUserLogin);
                                    }
                                    else if (Check_Receive_specimen == true)   // False
                                    {
                                        // ###########End Query Running && protocol number.
                                        // ###########
                                        // ###########Start Loop Insert if Amount > 1
                                        // Insert Table SP_REQUESTS
                                        // Table refer SP_TESTS
                                        //
                                        Insert_Into_SP_REQUESTS_Step2(TESTORDER, Str_patientID, Str_RequestAccess, StrTestcode, StrSpecimencode, StrSpecimenname, StrTestname, StrRequestDT, StrCollectionDT, Chk_ProtocolID[1], StrUserLogin, NewProtocol_num);
                                        TESTORDER++;
                                        //MessageBox.Show(strDateTime + "  Log: REQHEMO1107 :Step Insert SP_REQUESTS", "Error Please check Amout of Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3002:Loop Total Amout Multiple code   : [" + I + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3002:List string  {Amout}             : [" + IntAmout + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3002:Query Patient ID                 : [" + Str_patientID + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3002:Query Protocol ID                : [" + Chk_ProtocolID[0] + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3002:Query Protocol Protocol CODE     : [" + Chk_ProtocolID[1] + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3002:Query Protocol TEST ID           : [" + Chk_ProtocolID[2] + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3002:Query Protocol Length            : [" + Protocal_LENGTH + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log GRAM 1002.0:Loop save in Test CODE GRAM  : [" + StrTestcode + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log GRAM 1002.1:Loop save in Test Name GRAM  : [" + StrTestname + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log GRAM 1002.2:Loop save in Specimen name   : [" + StrSpecimenname + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log GRAM 1002.3:Loop save in Specimen Code   : [" + StrSpecimencode + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log GRAM 1002.0:Loop save in New Protocol No.: [" + NewProtocol_num + "]");
                                }
                            }
                            else if (IntAmout == 1)
                            {
                                // ############Start Get Protocol && Number Running of Protocol
                                //
                                // Query Protocol ID from StrTestcode
                                string[] Chk_ProtocolID = Query_PROTOCOL(StrTestcode);
                                Writedatalog.WriteLog(strDateTime + " Log:L1007  " + Chk_ProtocolID[0]);
                                Writedatalog.WriteLog(strDateTime + " Log:L1008  " + Chk_ProtocolID[1]);
                                Writedatalog.WriteLog(strDateTime + " Log:L1009  " + Chk_ProtocolID[2]);

                                // Query ค้นหาความยาวของ Number Protocol ที่ใช้;
                                Protocal_LENGTH = Chk_Length();
                                Writedatalog.WriteLog(strDateTime + " Log:L1013  " + Protocal_LENGTH);

                                // Get Number Protocal from Chk_ProtocolID
                                int RUNNINGPROTOCOL = new Nullable<int>(GETNUMBERPROTOCOL(Chk_ProtocolID[0])).GetValueOrDefault();
                                Writedatalog.WriteLog(strDateTime + " Log:L1017  " + RUNNINGPROTOCOL);

                                // Get LENGTH - NUMBER ---> Example
                                // Number LENGTH = 8
                                // Number Protocal now = 1
                                // Equal = [8 - 1 = 7]
                                int PADLEFTZERO = Convert.ToInt32(Protocal_LENGTH) - (Convert.ToInt32(RUNNINGPROTOCOL.ToString().Length));
                                Writedatalog.WriteLog(strDateTime + " Log:L1024  " + PADLEFTZERO);

                                // Set Newnumber of Protocol = 123 + 1 = 00000123
                                string NEWPROTOCAL = RUNNINGPROTOCOL.ToString().PadLeft(PADLEFTZERO + 1, '0');
                                Writedatalog.WriteLog(strDateTime + " Log:L1028  " + NEWPROTOCAL);

                                int Lengcount = NEWPROTOCAL.Length;
                                Writedatalog.WriteLog(strDateTime + " Log:L1031  " + Lengcount);

                                int NowLengthProtocol = Convert.ToInt32(Protocal_LENGTH);
                                Writedatalog.WriteLog(strDateTime + " Log:L1034  " + NowLengthProtocol);

                                if (Lengcount > NowLengthProtocol)
                                {
                                    Update_NEW_LENGTH(Lengcount, Chk_ProtocolID[0]);
                                    Writedatalog.WriteLog(strDateTime + " Log:REQGRAM3003 Update Length in PROTOCOL_NUMBER : Protocol Length count = " + Lengcount + " and Old Protocol LENGTH = " + NowLengthProtocol);
                                }
                                // Update Protocol Number in DATABASE and General number
                                Update_New_PROTOCOL_NUMBER(NEWPROTOCAL, Chk_ProtocolID[0]);
                                string NewProtocol_num = Chk_ProtocolID[1] + NEWPROTOCAL;

                                // ใช้ Boolean เช็ค ว่า จะรอรับ หรือ รับเลย
                                // แตกต่างกันที่ Database table ที่ จะ Insert 
                                // ถ้า รับเลย จะนำเข้าที่ REQUESTS,REQ_TESTS,MB_REQUESTS,SUBREQMB_STAIN
                                // ถ้า รอรับ จะนำเข้าที่ SP_REQUESTS,SP_TESTS เพื่อรอรับ Specimen ต่อไปกรณีที่ Speimen ยังไม่มา สามารถเลือกรับได้ใน แต่ละ Request ใน เมนู Specimen receive
                                //
                                // 
                                if (Check_Receive_specimen == false)      // false หรือ รับทันที
                                {
                                    // ###########End Query Running && protocol number.
                                    // ###########
                                    // ###########Start Loop Insert if Amount > 1
                                    // Insert Table REQUESTS Step 2
                                    // Table refer REQUESTS
                                    // Table refer REQ_TESTS
                                    //
                                    Insert_Into_REQUESTS_Step2(Str_patientID, Str_RequestAccess, StrRequestDT, StrspecimenID, StrCollectionDT, Chk_ProtocolID[2], StrUserLogin);
                                    // Step ถัดมา คือ Table 2 Table MB_REQUESTS , SUBREQMG_STAINS
                                    // Step 1. Insert MB_REQUESTS with Protocal number
                                    // 
                                    // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                                    // 
                                    Insert_Into_MB_REQUESTS(Str_RequestAccess, Chk_ProtocolID[2], Chk_ProtocolID[0], NewProtocol_num, StrRequestDT, StrUserLogin);
                                }
                                else if (Check_Receive_specimen == true)   // true หรือ รอรับ
                                {
                                    // ###########End Query Running && protocol number.
                                    // ###########
                                    // ###########Start Loop Insert if Amount > 1
                                    // Insert Table SP_REQUESTS
                                    // Table refer SP_TESTS
                                    //
                                    Writedatalog.WriteLog(strDateTime + "  Log:S3001:Request Start Step SP_REQUESTS Step2: [" + IntAmout + "]");                 // 1

                                    Insert_Into_SP_REQUESTS_Step2(TESTORDER, Str_patientID, Str_RequestAccess, StrTestcode, StrSpecimencode, StrSpecimenname, StrTestname, StrRequestDT, StrCollectionDT, Chk_ProtocolID[1], StrUserLogin, NewProtocol_num);
                                    TESTORDER++;

                                    //MessageBox.Show(strDateTime + "  Log: REQHEMO1107 :Step Insert SP_REQUESTS", "Error Please check Amout of Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                // Insert Table REQUESTS Step 2
                                //Insert_Into_REQUESTS_Step2(StrPATID_REQ, Str_RequestAccess, Get_Date_Request + Get_Time_Request, Chk_ProtocolID[2]);
                                // Step 1. Insert MB_REQUESTS with Protocal number
                                // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                                //Insert_Into_MB_REQUESTS(StrLabID, StrProtocolID, NewProtocol_num);                                                            // Example

                                Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3001:Request Single code              : [" + IntAmout + "]");                 // 1
                                Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3002:Query Protocol Length            : [" + Protocal_LENGTH + "]");          // 8
                                Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3003:Query Patient ID                 : [" + Str_patientID + "]");            // 24
                                Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3004:Query Protocol ID                : [" + Chk_ProtocolID[0] + "]");        // 5
                                Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3005:Query Protocol Protocol CODE     : [" + Chk_ProtocolID[1] + "]");        // P01
                                Writedatalog.WriteLog(strDateTime + "  Log:REQGRAM3006:Query Protocol TEST ID           : [" + Chk_ProtocolID[2] + "]");        // 1013

                                Writedatalog.WriteLog(strDateTime + "  Log:Log GRAM 1002.0:Loop save in Test CODE GRAM  : [" + StrTestcode + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log GRAM 1002.1:Loop save in Test Name GRAM  : [" + StrTestname + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log GRAM 1002.2:Loop save in Specimen name   : [" + StrSpecimenname + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log GRAM 1002.3:Loop save in Specimen Code   : [" + StrSpecimencode + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log GRAM 1002.0:Loop save in New Protocol No.: [" + NewProtocol_num + "]");
                            }
                            //else
                            //{
                            //    MessageBox.Show(strDateTime + "  Log: REQHEMO1107 :Loop Hemo?", "Error Please check Amout of Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //}
                        }
                    }
                    Writedatalog.WriteLog(strDateTime + "  Log:LGRAM1002:Loop End in Treelist GRAM   : [" + strDateTime + "]");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(strDateTime + "  Log: Q1107 :Loop Hemo?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Loop_save_Culture(string StrRequestDT, string StrCollectionDT)
        {
            int TESTORDER = 1;
            int StrspecimenID = 0;
            if (treeList_Culture.AllNodesCount > 0)
            {
                try
                {
                    for (int i = 0; i < treeList_Culture.AllNodesCount; i++)
                    {
                        if (treeList_Culture.Nodes[i].CheckState == CheckState.Checked)
                        {
                            string Protocal_LENGTH = "";
                            int IntAmout = Convert.ToInt32(treeList_Culture.Nodes[i].GetValue(1).ToString());
                            string StrTestcode = treeList_Culture.Nodes[i][2].ToString();                                           // Test code 
                            string StrTestname = treeList_Culture.Nodes[i][3].ToString();                                           // Test Name
                            string StrSpecimenname = treeList_Culture.Nodes[i][4].ToString();                                       // Specimen name
                            string StrSpecimencode = treeList_Culture.Nodes[i][5].ToString();                                       // Specimen Code
                            string StrCollected = strDateTime;                                                                   // DT Receive Create request

                            if (StrSpecimencode != "")
                            {
                                StrspecimenID = Query_specimenID(StrSpecimencode);
                            }

                            if (IntAmout > 1)
                            {
                                for (int I = 0; I < IntAmout; I++)
                                {
                                    // ############Start Get Protocol && Number Running of Protocol
                                    //
                                    // Query Protocol ID from StrTestcode
                                    string[] Chk_ProtocolID = Query_PROTOCOL(StrTestcode);
                                    // Query Length From Chk_ProtocolID;
                                    Protocal_LENGTH = Chk_Length();
                                    // Get Number Protocal from Chk_ProtocolID
                                    int RUNNINGPROTOCOL = new Nullable<int>(GETNUMBERPROTOCOL(Chk_ProtocolID[0])).GetValueOrDefault();
                                    // Get LENGTH - NUMBER ---> Example
                                    // Number LENGTH = 8
                                    // Number Protocal now = 1
                                    // Equal = [8 - 1 = 7]
                                    int PADLEFTZERO = Convert.ToInt32(Protocal_LENGTH) - (Convert.ToInt32(RUNNINGPROTOCOL.ToString().Length));
                                    // Set Newnumber of Protocol = 123 + 1 = 00000123
                                    string NEWPROTOCAL = RUNNINGPROTOCOL.ToString().PadLeft(PADLEFTZERO + 1, '0');
                                    int Lengcount = NEWPROTOCAL.Length;
                                    int NowLengthProtocol = Convert.ToInt32(Protocal_LENGTH);
                                    if (Lengcount > NowLengthProtocol)
                                    {
                                        Update_NEW_LENGTH(Lengcount, Chk_ProtocolID[0]);
                                        Writedatalog.WriteLog(strDateTime + " Log:REQCulture3003 Update Length in PROTOCOL_NUMBER : Protocol Length count = " + Lengcount + " and Old Protocol LENGTH = " + NowLengthProtocol);
                                    }
                                    // Update Protocol Number in DATABASE and General number
                                    Update_New_PROTOCOL_NUMBER(NEWPROTOCAL, Chk_ProtocolID[0]);
                                    string NewProtocol_num = Chk_ProtocolID[1] + NEWPROTOCAL;

                                    // ใช้ Boolean เช็ค ว่า จะรอรับ หรือ รับเลย
                                    // แตกต่างกันที่ Database table ที่ จะ Insert 
                                    // ถ้า รับเลย จะนำเข้าที่ REQUESTS,REQ_TESTS,MB_REQUESTS,SUBREQMB_STAIN
                                    // ถ้า รอรับ จะนำเข้าที่ SP_REQUESTS,SP_TESTS เพื่อรอรับ Specimen ต่อไปกรณีที่ Speimen ยังไม่มา สามารถเลือกรับได้ใน แต่ละ Request ใน เมนู Specimen receive
                                    //
                                    // 
                                    if (Check_Receive_specimen == false)      // false
                                    {
                                        // ###########End Query Running && protocol number.
                                        // ###########
                                        // ###########Start Loop Insert if Amount > 1
                                        // Insert Table REQUESTS Step 2
                                        // Table refer REQUESTS
                                        // Table refer REQ_TESTS
                                        //
                                        Insert_Into_REQUESTS_Step2(Str_patientID, Str_RequestAccess, StrRequestDT, StrspecimenID, StrCollectionDT, Chk_ProtocolID[2], StrUserLogin);
                                        // Step ถัดมา คือ Table 2 Table MB_REQUESTS , SUBREQMG_STAINS
                                        // Step 1. Insert MB_REQUESTS with Protocal number
                                        // 
                                        // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                                        // 
                                        Insert_Into_MB_REQUESTS(Str_RequestAccess, Chk_ProtocolID[2] ,Chk_ProtocolID[0], NewProtocol_num, StrRequestDT, StrUserLogin);
                                    }
                                    else if (Check_Receive_specimen == true)   // False
                                    {
                                        // ###########End Query Running && protocol number.
                                        // ###########
                                        // ###########Start Loop Insert if Amount > 1
                                        // Insert Table SP_REQUESTS
                                        // Table refer SP_TESTS
                                        //
                                        Insert_Into_SP_REQUESTS_Step2(TESTORDER, Str_patientID, Str_RequestAccess, StrTestcode, StrSpecimencode, StrSpecimenname, StrTestname, StrRequestDT, StrCollectionDT, Chk_ProtocolID[1], StrUserLogin, NewProtocol_num);
                                        TESTORDER++;
                                        //MessageBox.Show(strDateTime + "  Log: REQHEMO1107 :Step Insert SP_REQUESTS", "Error Please check Amout of Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3001:Loop Total Amout Multiple code   : [" + I + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3001:List string  {Amout}             : [" + IntAmout + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3001:Query Patient ID                 : [" + Str_patientID + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3001:Query Protocol ID                : [" + Chk_ProtocolID[0] + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3001:Query Protocol Protocol CODE     : [" + Chk_ProtocolID[1] + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3001:Query Protocol TEST ID           : [" + Chk_ProtocolID[2] + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3002:Query Protocol Length            : [" + Protocal_LENGTH + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log Culture 1002.0:Loop save in Test CODE Culture  : [" + StrTestcode + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log Culture 1002.1:Loop save in Test Name Culture  : [" + StrTestname + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log Culture 1002.2:Loop save in Specimen name   : [" + StrSpecimenname + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log Culture 1002.3:Loop save in Specimen Code   : [" + StrSpecimencode + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log Culture 1002.0:Loop save in New Protocol No.: [" + NewProtocol_num + "]");
                                }
                            }
                            else if (IntAmout == 1)
                            {
                                // ############Start Get Protocol && Number Running of Protocol
                                //
                                // Query Protocol ID from StrTestcode
                                string[] Chk_ProtocolID = Query_PROTOCOL(StrTestcode);
                                Writedatalog.WriteLog(strDateTime + " Log:L1007  " + Chk_ProtocolID[0]);
                                Writedatalog.WriteLog(strDateTime + " Log:L1008  " + Chk_ProtocolID[1]);
                                Writedatalog.WriteLog(strDateTime + " Log:L1009  " + Chk_ProtocolID[2]);

                                // Query ค้นหาความยาวของ Number Protocol ที่ใช้;
                                Protocal_LENGTH = Chk_Length();
                                Writedatalog.WriteLog(strDateTime + " Log:L1013  " + Protocal_LENGTH);

                                // Get Number Protocal from Chk_ProtocolID
                                int RUNNINGPROTOCOL = new Nullable<int>(GETNUMBERPROTOCOL(Chk_ProtocolID[0])).GetValueOrDefault();
                                Writedatalog.WriteLog(strDateTime + " Log:L1017  " + RUNNINGPROTOCOL);

                                // Get LENGTH - NUMBER ---> Example
                                // Number LENGTH = 8
                                // Number Protocal now = 1
                                // Equal = [8 - 1 = 7]
                                int PADLEFTZERO = Convert.ToInt32(Protocal_LENGTH) - (Convert.ToInt32(RUNNINGPROTOCOL.ToString().Length));
                                Writedatalog.WriteLog(strDateTime + " Log:L1024  " + PADLEFTZERO);

                                // Set Newnumber of Protocol = 123 + 1 = 00000123
                                string NEWPROTOCAL = RUNNINGPROTOCOL.ToString().PadLeft(PADLEFTZERO + 1, '0');
                                Writedatalog.WriteLog(strDateTime + " Log:L1028  " + NEWPROTOCAL);

                                int Lengcount = NEWPROTOCAL.Length;
                                Writedatalog.WriteLog(strDateTime + " Log:L1031  " + Lengcount);

                                int NowLengthProtocol = Convert.ToInt32(Protocal_LENGTH);
                                Writedatalog.WriteLog(strDateTime + " Log:L1034  " + NowLengthProtocol);

                                if (Lengcount > NowLengthProtocol)
                                {
                                    Update_NEW_LENGTH(Lengcount, Chk_ProtocolID[0]);
                                    Writedatalog.WriteLog(strDateTime + " Log:REQHEMO3003 Update Length in PROTOCOL_NUMBER : Protocol Length count = " + Lengcount + " and Old Protocol LENGTH = " + NowLengthProtocol);
                                }
                                // Update Protocol Number in DATABASE and General number
                                Update_New_PROTOCOL_NUMBER(NEWPROTOCAL, Chk_ProtocolID[0]);
                                string NewProtocol_num = Chk_ProtocolID[1] + NEWPROTOCAL;

                                // ใช้ Boolean เช็ค ว่า จะรอรับ หรือ รับเลย
                                // แตกต่างกันที่ Database table ที่ จะ Insert 
                                // ถ้า รับเลย จะนำเข้าที่ REQUESTS,REQ_TESTS,MB_REQUESTS,SUBREQMB_STAIN
                                // ถ้า รอรับ จะนำเข้าที่ SP_REQUESTS,SP_TESTS เพื่อรอรับ Specimen ต่อไปกรณีที่ Speimen ยังไม่มา สามารถเลือกรับได้ใน แต่ละ Request ใน เมนู Specimen receive
                                //
                                // 
                                if (Check_Receive_specimen == false)      // false หรือ รับทันที
                                {
                                    // ###########End Query Running && protocol number.
                                    // ###########
                                    // ###########Start Loop Insert if Amount > 1
                                    // Insert Table REQUESTS Step 2
                                    // Table refer REQUESTS
                                    // Table refer REQ_TESTS
                                    //
                                    Insert_Into_REQUESTS_Step2(Str_patientID, Str_RequestAccess, StrRequestDT, StrspecimenID, StrCollectionDT, Chk_ProtocolID[2], StrUserLogin);
                                    // Step ถัดมา คือ Table 2 Table MB_REQUESTS , SUBREQMG_STAINS
                                    // Step 1. Insert MB_REQUESTS with Protocal number
                                    // 
                                    // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                                    // 
                                    Insert_Into_MB_REQUESTS(Str_RequestAccess, Chk_ProtocolID[2] , Chk_ProtocolID[0], NewProtocol_num, StrRequestDT, StrUserLogin);
                                }
                                else if (Check_Receive_specimen == true)   // true หรือ รอรับ
                                {
                                    // ###########End Query Running && protocol number.
                                    // ###########
                                    // ###########Start Loop Insert if Amount > 1
                                    // Insert Table SP_REQUESTS
                                    // Table refer SP_TESTS
                                    //
                                    Writedatalog.WriteLog(strDateTime + "  Log:S3001:Request Start Step SP_REQUESTS Step2: [" + IntAmout + "]");                 // 1

                                    Insert_Into_SP_REQUESTS_Step2(TESTORDER, Str_patientID, Str_RequestAccess, StrTestcode, StrSpecimencode, StrSpecimenname, StrTestname, StrRequestDT, StrCollectionDT, Chk_ProtocolID[1], StrUserLogin, NewProtocol_num);
                                    TESTORDER++;

                                    //MessageBox.Show(strDateTime + "  Log: REQHEMO1107 :Step Insert SP_REQUESTS", "Error Please check Amout of Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                // Insert Table REQUESTS Step 2
                                //Insert_Into_REQUESTS_Step2(StrPATID_REQ, Str_RequestAccess, Get_Date_Request + Get_Time_Request, Chk_ProtocolID[2]);
                                // Step 1. Insert MB_REQUESTS with Protocal number
                                // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                                //Insert_Into_MB_REQUESTS(StrLabID, StrProtocolID, NewProtocol_num);                                                            // Example

                                Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3001:Request Single code              : [" + IntAmout + "]");                 // 1
                                Writedatalog.WriteLog(strDateTime + "  Log:REQCultureO3002:Query Protocol Length            : [" + Protocal_LENGTH + "]");          // 8
                                Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3003:Query Patient ID                 : [" + Str_patientID + "]");            // 24
                                Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3004:Query Protocol ID                : [" + Chk_ProtocolID[0] + "]");        // 5
                                Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3005:Query Protocol Protocol CODE     : [" + Chk_ProtocolID[1] + "]");        // P01
                                Writedatalog.WriteLog(strDateTime + "  Log:REQCulture3006:Query Protocol TEST ID           : [" + Chk_ProtocolID[2] + "]");        // 1013

                                Writedatalog.WriteLog(strDateTime + "  Log:Log Culture 1002.0:Loop save in Test CODE Culture  : [" + StrTestcode + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log Culture 1002.1:Loop save in Test Name Culture  : [" + StrTestname + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log Culture 1002.2:Loop save in Specimen name   : [" + StrSpecimenname + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log Culture 1002.3:Loop save in Specimen Code   : [" + StrSpecimencode + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log Culture 1002.0:Loop save in New Protocol No.: [" + NewProtocol_num + "]");
                            }
                            //else
                            //{
                            //    MessageBox.Show(strDateTime + "  Log: REQHEMO1107 :Loop Hemo?", "Error Please check Amout of Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //}
                        }
                    }
                    Writedatalog.WriteLog(strDateTime + "  Log:LCulture1002:Loop End in Treelist Culture   : [" + strDateTime + "]");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(strDateTime + "  Log: Q1107 :Loop Hemo?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Loop_save_Hemo(string StrRequestDT, string StrCollectionDT)
        {
            int TESTORDER = 1;
            int StrspecimenID = 0;
            if (treeList_Hemo.AllNodesCount > 0)
            {
                try
                {
                    for (int i = 0; i < treeList_Hemo.AllNodesCount; i++)
                    {
                        if (treeList_Hemo.Nodes[i].CheckState == CheckState.Checked)
                        {
                            string Protocal_LENGTH = "";
                            int IntAmout = Convert.ToInt32(treeList_Hemo.Nodes[i].GetValue(1).ToString());
                            string StrTestcode = treeList_Hemo.Nodes[i][2].ToString();                                           // Test code 
                            string StrTestname = treeList_Hemo.Nodes[i][3].ToString();                                           // Test Name
                            string StrSpecimenname = treeList_Hemo.Nodes[i][4].ToString();                                       // Specimen name
                            string StrSpecimencode = treeList_Hemo.Nodes[i][5].ToString();                                       // Specimen Code
                            string StrCollected = strDateTime;                                                                   // DT Receive Create request

                            if (StrSpecimencode != "")
                            {
                                StrspecimenID = Query_specimenID(StrSpecimencode);
                            }

                            if (IntAmout > 1)
                            {
                                for (int I = 0; I < IntAmout; I++)
                                {
                                    // ############Start Get Protocol && Number Running of Protocol
                                    //
                                    // Query Protocol ID from StrTestcode
                                    string[] Chk_ProtocolID = Query_PROTOCOL(StrTestcode);
                                    // Query Length From Chk_ProtocolID;
                                    Protocal_LENGTH = Chk_Length();
                                    // Get Number Protocal from Chk_ProtocolID
                                    int RUNNINGPROTOCOL = new Nullable<int>(GETNUMBERPROTOCOL(Chk_ProtocolID[0])).GetValueOrDefault();
                                    // Get LENGTH - NUMBER ---> Example
                                    // Number LENGTH = 8
                                    // Number Protocal now = 1
                                    // Equal = [8 - 1 = 7]
                                    int PADLEFTZERO = Convert.ToInt32(Protocal_LENGTH) - (Convert.ToInt32(RUNNINGPROTOCOL.ToString().Length));
                                    // Set Newnumber of Protocol = 123 + 1 = 00000123
                                    string NEWPROTOCAL = RUNNINGPROTOCOL.ToString().PadLeft(PADLEFTZERO + 1, '0');
                                    int Lengcount = NEWPROTOCAL.Length;
                                    int NowLengthProtocol = Convert.ToInt32(Protocal_LENGTH);
                                    if (Lengcount > NowLengthProtocol)
                                    {
                                        Update_NEW_LENGTH(Lengcount, Chk_ProtocolID[0]);
                                        Writedatalog.WriteLog(strDateTime + " Log:REQHEMO3003 Update Length in PROTOCOL_NUMBER : Protocol Length count = " + Lengcount + " and Old Protocol LENGTH = " + NowLengthProtocol);
                                    }
                                    // Update Protocol Number in DATABASE and General number
                                    Update_New_PROTOCOL_NUMBER(NEWPROTOCAL, Chk_ProtocolID[0]);
                                    string NewProtocol_num = Chk_ProtocolID[1] + NEWPROTOCAL;

                                    // ใช้ Boolean เช็ค ว่า จะรอรับ หรือ รับเลย
                                    // แตกต่างกันที่ Database table ที่ จะ Insert 
                                    // ถ้า รับเลย จะนำเข้าที่ REQUESTS,REQ_TESTS,MB_REQUESTS,SUBREQMB_STAIN
                                    // ถ้า รอรับ จะนำเข้าที่ SP_REQUESTS,SP_TESTS เพื่อรอรับ Specimen ต่อไปกรณีที่ Speimen ยังไม่มา สามารถเลือกรับได้ใน แต่ละ Request ใน เมนู Specimen receive
                                    //
                                    // 
                                    if (Check_Receive_specimen == false)      // false
                                    {
                                        // ###########End Query Running && protocol number.
                                        // ###########
                                        // ###########Start Loop Insert if Amount > 1
                                        // Insert Table REQUESTS Step 2
                                        // Table refer REQUESTS
                                        // Table refer REQ_TESTS
                                        //
                                        Insert_Into_REQUESTS_Step2(Str_patientID, Str_RequestAccess, StrRequestDT, StrspecimenID, StrCollectionDT, Chk_ProtocolID[2], StrUserLogin);
                                        // Step ถัดมา คือ Table 2 Table MB_REQUESTS , SUBREQMG_STAINS
                                        // Step 1. Insert MB_REQUESTS with Protocal number
                                        // 
                                        // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                                        // 
                                        Insert_Into_MB_REQUESTS(Str_RequestAccess, Chk_ProtocolID[2] , Chk_ProtocolID[0], NewProtocol_num, StrRequestDT, StrUserLogin);
                                    }
                                    else if (Check_Receive_specimen == true)   // False
                                    {
                                        // ###########End Query Running && protocol number.
                                        // ###########
                                        // ###########Start Loop Insert if Amount > 1
                                        // Insert Table SP_REQUESTS
                                        // Table refer SP_TESTS
                                        //
                                        Insert_Into_SP_REQUESTS_Step2(TESTORDER, Str_patientID, Str_RequestAccess, StrTestcode, StrSpecimencode, StrSpecimenname, StrTestname, StrRequestDT, StrCollectionDT, Chk_ProtocolID[1], StrUserLogin, NewProtocol_num);
                                        TESTORDER++;
                                        //MessageBox.Show(strDateTime + "  Log: REQHEMO1107 :Step Insert SP_REQUESTS", "Error Please check Amout of Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3001:Loop Total Amout Multiple code   : [" + I + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3001:List string  {Amout}             : [" + IntAmout + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3001:Query Patient ID                 : [" + Str_patientID + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3001:Query Protocol ID                : [" + Chk_ProtocolID[0] + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3001:Query Protocol Protocol CODE     : [" + Chk_ProtocolID[1] + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3001:Query Protocol TEST ID           : [" + Chk_ProtocolID[2] + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3002:Query Protocol Length            : [" + Protocal_LENGTH + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log HEMO 1002.0:Loop save in Test CODE HEMO  : [" + StrTestcode + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log HEMO 1002.1:Loop save in Test Name HEMO  : [" + StrTestname + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log HEMO 1002.2:Loop save in Specimen name   : [" + StrSpecimenname + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log HEMO 1002.3:Loop save in Specimen Code   : [" + StrSpecimencode + "]");
                                    Writedatalog.WriteLog(strDateTime + "  Log:Log HEMO 1002.0:Loop save in New Protocol No.: [" + NewProtocol_num + "]");
                                }
                            }
                            else if (IntAmout == 1)
                            {
                                // ############Start Get Protocol && Number Running of Protocol
                                //
                                // Query Protocol ID from StrTestcode
                                string[] Chk_ProtocolID = Query_PROTOCOL(StrTestcode);
                                Writedatalog.WriteLog(strDateTime + " Log:L1007  " + Chk_ProtocolID[0]);
                                Writedatalog.WriteLog(strDateTime + " Log:L1008  " + Chk_ProtocolID[1]);
                                Writedatalog.WriteLog(strDateTime + " Log:L1009  " + Chk_ProtocolID[2]);

                                // Query ค้นหาความยาวของ Number Protocol ที่ใช้;
                                Protocal_LENGTH = Chk_Length();
                                Writedatalog.WriteLog(strDateTime + " Log:L1013  " + Protocal_LENGTH);

                                // Get Number Protocal from Chk_ProtocolID
                                int RUNNINGPROTOCOL = new Nullable<int>(GETNUMBERPROTOCOL(Chk_ProtocolID[0])).GetValueOrDefault();
                                Writedatalog.WriteLog(strDateTime + " Log:L1017  " + RUNNINGPROTOCOL);

                                // Get LENGTH - NUMBER ---> Example
                                // Number LENGTH = 8
                                // Number Protocal now = 1
                                // Equal = [8 - 1 = 7]
                                int PADLEFTZERO = Convert.ToInt32(Protocal_LENGTH) - (Convert.ToInt32(RUNNINGPROTOCOL.ToString().Length));
                                Writedatalog.WriteLog(strDateTime + " Log:L1024  " + PADLEFTZERO);

                                // Set Newnumber of Protocol = 123 + 1 = 00000123
                                string NEWPROTOCAL = RUNNINGPROTOCOL.ToString().PadLeft(PADLEFTZERO + 1, '0');
                                Writedatalog.WriteLog(strDateTime + " Log:L1028  " + NEWPROTOCAL);

                                int Lengcount = NEWPROTOCAL.Length;
                                Writedatalog.WriteLog(strDateTime + " Log:L1031  " + Lengcount);

                                int NowLengthProtocol = Convert.ToInt32(Protocal_LENGTH);
                                Writedatalog.WriteLog(strDateTime + " Log:L1034  " + NowLengthProtocol);

                                if (Lengcount > NowLengthProtocol)
                                {
                                    Update_NEW_LENGTH(Lengcount, Chk_ProtocolID[0]);
                                    Writedatalog.WriteLog(strDateTime + " Log:REQHEMO3003 Update Length in PROTOCOL_NUMBER : Protocol Length count = " + Lengcount + " and Old Protocol LENGTH = " + NowLengthProtocol);
                                }
                                // Update Protocol Number in DATABASE and General number
                                Update_New_PROTOCOL_NUMBER(NEWPROTOCAL, Chk_ProtocolID[0]);
                                string NewProtocol_num = Chk_ProtocolID[1] + NEWPROTOCAL;

                                // ใช้ Boolean เช็ค ว่า จะรอรับ หรือ รับเลย
                                // แตกต่างกันที่ Database table ที่ จะ Insert 
                                // ถ้า รับเลย จะนำเข้าที่ REQUESTS,REQ_TESTS,MB_REQUESTS,SUBREQMB_STAIN
                                // ถ้า รอรับ จะนำเข้าที่ SP_REQUESTS,SP_TESTS เพื่อรอรับ Specimen ต่อไปกรณีที่ Speimen ยังไม่มา สามารถเลือกรับได้ใน แต่ละ Request ใน เมนู Specimen receive
                                //
                                // 
                                if (Check_Receive_specimen == false)      // false หรือ รับทันที
                                {
                                    // ###########End Query Running && protocol number.
                                    // ###########
                                    // ###########Start Loop Insert if Amount > 1
                                    // Insert Table REQUESTS Step 2
                                    // Table refer REQUESTS
                                    // Table refer REQ_TESTS
                                    //
                                    Insert_Into_REQUESTS_Step2(Str_patientID, Str_RequestAccess, StrRequestDT, StrspecimenID, StrCollectionDT, Chk_ProtocolID[2], StrUserLogin);
                                    // Step ถัดมา คือ Table 2 Table MB_REQUESTS , SUBREQMG_STAINS
                                    // Step 1. Insert MB_REQUESTS with Protocal number
                                    // 
                                    // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                                    // 
                                    Insert_Into_MB_REQUESTS(Str_RequestAccess, Chk_ProtocolID[2] , Chk_ProtocolID[0], NewProtocol_num, StrRequestDT, StrUserLogin);
                                }
                                else if (Check_Receive_specimen == true)   // true หรือ รอรับ
                                {
                                    // ###########End Query Running && protocol number.
                                    // ###########
                                    // ###########Start Loop Insert if Amount > 1
                                    // Insert Table SP_REQUESTS
                                    // Table refer SP_TESTS
                                    //
                                    Writedatalog.WriteLog(strDateTime + "  Log:S3001:Request Start Step SP_REQUESTS Step2: [" + IntAmout + "]");                 // 1

                                    Insert_Into_SP_REQUESTS_Step2(TESTORDER, Str_patientID, Str_RequestAccess, StrTestcode, StrSpecimencode, StrSpecimenname, StrTestname, StrRequestDT, StrCollectionDT, Chk_ProtocolID[1], StrUserLogin, NewProtocol_num);
                                    TESTORDER++;

                                    //MessageBox.Show(strDateTime + "  Log: REQHEMO1107 :Step Insert SP_REQUESTS", "Error Please check Amout of Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                // Insert Table REQUESTS Step 2
                                //Insert_Into_REQUESTS_Step2(StrPATID_REQ, Str_RequestAccess, Get_Date_Request + Get_Time_Request, Chk_ProtocolID[2]);
                                // Step 1. Insert MB_REQUESTS with Protocal number
                                // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                                //Insert_Into_MB_REQUESTS(StrLabID, StrProtocolID, NewProtocol_num);                                                            // Example

                                Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3001:Request Single code              : [" + IntAmout + "]");                 // 1
                                Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3002:Query Protocol Length            : [" + Protocal_LENGTH + "]");          // 8
                                Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3003:Query Patient ID                 : [" + Str_patientID + "]");            // 24
                                Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3004:Query Protocol ID                : [" + Chk_ProtocolID[0] + "]");        // 5
                                Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3005:Query Protocol Protocol CODE     : [" + Chk_ProtocolID[1] + "]");        // P01
                                Writedatalog.WriteLog(strDateTime + "  Log:REQHEMO3006:Query Protocol TEST ID           : [" + Chk_ProtocolID[2] + "]");        // 1013

                                Writedatalog.WriteLog(strDateTime + "  Log:Log HEMO 1002.0:Loop save in Test CODE HEMO  : [" + StrTestcode + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log HEMO 1002.1:Loop save in Test Name HEMO  : [" + StrTestname + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log HEMO 1002.2:Loop save in Specimen name   : [" + StrSpecimenname + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log HEMO 1002.3:Loop save in Specimen Code   : [" + StrSpecimencode + "]");
                                Writedatalog.WriteLog(strDateTime + "  Log:Log HEMO 1002.0:Loop save in New Protocol No.: [" + NewProtocol_num + "]");
                            }
                            //else
                            //{
                            //    MessageBox.Show(strDateTime + "  Log: REQHEMO1107 :Loop Hemo?", "Error Please check Amout of Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //}
                        }
                    }
                    Writedatalog.WriteLog(strDateTime + "  Log:LGRAM1002:Loop End in Treelist Hemoculture   : [" + strDateTime + "]");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(strDateTime + "  Log: Q1107 :Loop Hemo?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Insert_Into_REQUESTS_Step1(string StrPatnumber, string StrReqNumber, string StrLocation, string StrDoctor, string StrRequestDT, string StrCollectionDT, string StrReqComment)
        {
            //Str_patientID = Query_PatientID(StrPatnumber);

            // ถ้ารอรับ Specimen Insert request to Table REQUESTS
            // รอรับ = true
            // รับทันที = false
            if (radioGroup3.SelectedIndex == 0)
            {
                // รอรับ
                Check_Receive_specimen = true;
                try
                {
                    string sql_Insert = @"INSERT INTO SP_REQUESTS (SP_ACCESSNUMBER,PATNUM,TRANSMISSIONSTATUS,SP_LOCCODE,SP_DOCCODE,SP_SPECIMENCODE,SP_IPDOROPD,COLLECTIONDATE,REQURGENT,REQDATE,LOGUSERID,LOGDATE,USERFIELD1,SECRETRESULT)
                                            VALUES ('" + StrReqNumber + "','" + StrPatnumber + "','" + 0 + "','" + StrLocation + "','" +
StrDoctor + "','" + Str_Req_specimen + "','" + Str_Req_IPDOPD + "','" + StrCollectionDT + "','" + Str_Req_Urgent + "','" + StrRequestDT + "','" + "REQMANUAL" + "','" + strDateTime + "','" + "SYS" + "','" + StrSecret + "')";
                    Writedatalog.WriteLog(strDateTime + "  Log:Save Request 3002.1:Save Request to SP_REQUESTS      : [" + sql_Insert + "]");
                    SqlCommand cmd = new SqlCommand(sql_Insert, conn);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error :SAVEREQ1002 Insert Data to SP_REQUESTS \r\nDetail : " + ex.Message, "Inform Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (radioGroup3.SelectedIndex == 1)
            {
                // รับทันที
                Check_Receive_specimen = false;
                try
                {
                    string REQ_createtionDate = DateTime.Now.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));
                    string sql_Insert = @"INSERT INTO REQUESTS (PATID, ACCESSNUMBER, REQCREATIONDATE, REQSTATUS,REQ_IPDOROPD, STATUSDATE, REQDATE, COLLECTIONDATE,SPECIMENCODE, URGENT, COMMENT,SECRETRESULT, LASTUPDATE, RECEIVEDDATE ,REQDOCTOR ,REQLOCATION, LOGUSERID, LOGDATE) 
                                         VALUES ('" + Str_patientID + "','" + StrReqNumber + "','" + REQ_createtionDate + "','" + "10" + "','" + Str_Req_IPDOPD + "','" + strDateTime + "','" + StrRequestDT + "','" + StrCollectionDT + "','" + Str_Req_specimen + "','" +
                                             Str_Req_Urgent + "','" + StrReqComment + "','" + StrSecret + "','" + strDateTime + "','" + strDateTime + "','" + StrDoctor + "','" + StrLocation + "','" + "REQMANUAL" + "','" + strDateTime + "'); SELECT SCOPE_IDENTITY()";
                    SqlCommand cmd = new SqlCommand(sql_Insert, conn);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    requestID = Convert.ToInt32(cmd.ExecuteScalar());
                    Writedatalog.WriteLog(strDateTime + "  Log:Save Request 3002.0:Save Request to REQUESTS         : [" + StrReqNumber + "]");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error :REQENTRYI1001 Insert Data to REQUESTS \r\nDetail : " + ex.Message, "Inform Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // Table REQUESTS
        private void Insert_Into_REQUESTS_Step2(string Str_patientID, string Str_RequestAccess, string StrRequestDT, int StrspecimenID, string StrCollectionDT, string StrTESTID, string StrUserLogin)
        {
            string StrREQSTATUS = Query_REQSTATUS_REQUESTS(Str_RequestAccess);
            try
            {
                // เช็ค Status ใน Table REQUESTS ถ้า เป็น 1 คือมี Request แล้ว ให้ Insert ได้ แต่ถ้ามี ค่า เป็น 0 จะไม่ทำอะไร 
                // จำเป็น ต้องมี Request บันทึกใน Table REQUEST ก่อนเสมอ เพื่อป้องกัน การส่ง Request test เข้าไปโดยที่ไม่มี Request หลัก
                if (StrREQSTATUS != "10")
                {
                    // Do Notthing
                    // ไม่ต้องทำอะไร จำเป็นต้องมี Request ใน Table REQUESTS ก่อน
                }
                // เริ่ม Insert โดย ค้นหา REQUEST ID จาก Table REQUEST มาเพื่อนำไปบันทึกใน Table SP_TESTS
                else if (StrREQSTATUS == "10")
                {
                    // เริ่มค้นหา
                    // ค้นหา RequestID ที่ เพิ่มไป Insert ไปใน Step 1 ว่าใช้ Request ID อะไร
                    string StrREQUESTID = Query_REQUESTID_REQUESTS(Str_RequestAccess);
                    Writedatalog.WriteLog(strDateTime + "  Log:L5001  :Insert_Into_REQUESTS_Step2 Query_REQUESTID_REQUESTS       : [" + StrREQUESTID + "]");
                    // เริ่ม Insert data โดยนำ Request ID และ Test ID ที่ค้นหาได้จากก่อนหน้านี้มา Insert ใน Table SP_TESTS
                    Insert_Into_REQ_TESTS(StrREQUESTID, StrTESTID, StrRequestDT, StrspecimenID, StrUserLogin);
                    Writedatalog.WriteLog(strDateTime + "  Log:L5001.1:Insert_Into_REQUESTS_Step2 Insert_Into_REQ_TESTS          : [" + StrREQUESTID + " " + StrTESTID + "]");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1011 Insert Data to REQUESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Table SP_REQUESTS
        private void Insert_Into_SP_REQUESTS_Step2(int TESTORDER, string Str_patientID, string Str_RequestAccess, string StrTestcode, string StrSpecimencode, string StrSpecimenname, string StrTestname, string StrRequestDT, string StrCollectionDT, string StrProtocolCODE, string StrUserLogin, string NewProtocol_num)
        {
            string StrTRANSMISSIONSTATUS = Query_REQSTATUS_SP_REQUESTS(Str_RequestAccess);
            try
            {
                // เช็ค Status ใน Table SP_REQUESTS ถ้า เป็น 1 คือมี Request แล้ว ให้ Insert ได้ แต่ถ้ามี ค่า เป็น 0 จะไม่ทำอะไร 
                // จำเป็น ต้องมี Request บันทึกใน Table SP_REQUEST ก่อนเสมอ เพื่อป้องกัน การส่ง Request test เข้าไปโดยที่ไม่มี Request หลัก
                if (StrTRANSMISSIONSTATUS != "0")
                {
                    // Do Notthing
                    // ไม่ต้องทำอะไร จำเป็นต้องมี Request ใน Table SP_REQUESTS ก่อน
                }
                // เริ่ม Insert โดย ค้นหา SP_REQUEST ID จาก Table SP_REQUEST มาเพื่อนำไปบันทึกใน Table SP_TESTS
                else if (StrTRANSMISSIONSTATUS == "0")
                {
                    // เริ่มค้นหา
                    // ค้นหา RequestID ที่ เพิ่มไป Insert ไปใน Step 1 ว่าใช้ Request ID อะไร

                    //Writedatalog.WriteLog(strDateTime + "  Log:L5001  :Insert_Into_REQUESTS_Step2 Query_REQUESTID_REQUESTS       : [" + StrREQUESTID + "]");
                    // เริ่ม Insert data โดยนำ Request ID และ Test ID ที่ค้นหาได้จากก่อนหน้านี้มา Insert ใน Table SP_TESTS
                    Insert_Into_SP_TESTS(TESTORDER, StrTestcode, StrTestname, StrSpecimencode, StrSpecimenname, StrProtocolCODE, StrRequestDT, StrUserLogin, NewProtocol_num);
                    Writedatalog.WriteLog(strDateTime + "  Log:L5001.1:Insert_Into_REQUESTS_Step2 Insert_Into_REQ_TESTS          : [" + TESTORDER + " " + StrProtocolCODE + NewProtocol_num + "]");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1011 Insert Data to REQUESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Table REQUESTS --> REQSTATUS
        private string Query_REQSTATUS_REQUESTS(string Str_RequestAccess)
        {
            string result = "";
            try
            {
                string sql_Query_REQSTATUS = @"SELECT REQSTATUS FROM REQUESTS WHERE ACCESSNUMBER='" + Str_RequestAccess + "'";

                SqlCommand cmd = new SqlCommand(sql_Query_REQSTATUS, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adap.Fill(ds, "REQSTATUS_REQUESTS");
                if (ds.Tables["REQSTATUS_REQUESTS"].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["REQSTATUS"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1004 Query_REQSTATUS_REQUESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        // Table SP_REQUESTS --> TRANSMISSIONSTATUS
        private string Query_REQSTATUS_SP_REQUESTS(string Str_RequestAccess)
        {
            string result = "";
            try
            {
                string sql_Query_REQSTATUS = @"SELECT TRANSMISSIONSTATUS FROM SP_REQUESTS WHERE SP_ACCESSNUMBER='" + Str_RequestAccess + "'";

                SqlCommand cmd = new SqlCommand(sql_Query_REQSTATUS, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adap.Fill(ds, "TRANSMISSIONSTATUS_REQUESTS");
                if (ds.Tables["TRANSMISSIONSTATUS_REQUESTS"].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["TRANSMISSIONSTATUS"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :Q1005 Query_TRANSMISSIONSTATUS_SP_REQUESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        // Table REQUESTS --> REQUESTSID
        private string Query_REQUESTID_REQUESTS(string Str_RequestAccess)
        {
            string result = "";
            try
            {
                string sql_RequestID = @"SELECT REQUESTID FROM REQUESTS WHERE ACCESSNUMBER='" + Str_RequestAccess + "'";
                SqlCommand cmd = new SqlCommand(sql_RequestID, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_Query_RequestID");
                cmd.ExecuteReader();
                if (ds.Tables["sql_Query_RequestID"].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["REQUESTID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1012 Query_REQUESTID_REQUESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }
        // Table REQ_TESTS
        private void Insert_Into_REQ_TESTS(string StrREQUESTID, string REQUESTS_TESTID, string StrRequestDT, int StrspecimenID, string StrUserLogin)
        {
            try
            {
                string sql_REQ_TESTS = @"INSERT INTO REQ_TESTS (TESTID,REQUESTID,COLLMATERIALID,LASTUPDATE,LOGUSERID,LOGDATE) 
VALUES ('" + REQUESTS_TESTID + "','" + StrREQUESTID + "','" + StrspecimenID + "','" + StrRequestDT + "','" + StrUserLogin + "','" + strDateTime + "')";

                SqlCommand cmd = new SqlCommand(sql_REQ_TESTS, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                Writedatalog.WriteLog(strDateTime + "  Log:L5001.1:Insert_Into_REQ_TESTS                     : [" + REQUESTS_TESTID + " " + StrREQUESTID + "]");
                Writedatalog.WriteLog(strDateTime + "  Log:L5001.1:Insert_Into_REQ_TESTS SQL statement       : [" + sql_REQ_TESTS + "]");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Insert_Into_REQ_TESTS ! \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }
        // Table SP_TESTS
        private void Insert_Into_SP_TESTS(int TESTORDER, string StrTestcode, string StrSpecimencode, string StrSpecimenname, string StrTestname, string StrProtocolCODE, string StrRequestDT, string StrUserLogin, string NewProtocol_num)
        {
            try
            {
                string sql_SP_TESTS = @"INSERT INTO SP_TESTS (SP_TESTORDER,SP_ACCESSNUMBER,SP_TESTCODE,SP_TESTNAME,SP_SPECIMENCODE,SP_SPECIMENNAME,SP_TESTSTATUS,LOGUSERID,LOGDATE,SP_STNCODE)
VALUES ('" + TESTORDER + "','" + Str_RequestAccess + "','" + StrTestcode + "','" + StrSpecimencode + "','" + StrSpecimenname + "','" + StrTestname + "','" + 0 + "','" + "REQMANUAL" + "','" + StrRequestDT + "','" + StrProtocolCODE + "')";

                Writedatalog.WriteLog(strDateTime + "  Log:L5002.2:Insert_Into_REQ_TESTS SQL statement       : [" + sql_SP_TESTS + "]");
                SqlCommand cmd = new SqlCommand(sql_SP_TESTS, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                Writedatalog.WriteLog(strDateTime + "  Log:L5002.1:Insert_Into_REQ_TESTS                     : [" + StrProtocolCODE + " " + Str_RequestAccess + NewProtocol_num + "]");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Insert_Into_SP_TESTS ! \r\nDetail : " + ex.Message, "Inform Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Insert_Into_MB_REQUESTS(string Str_RequestAccess, string StrTestID ,string StrProtocolID, string NewProtocol_num, string StrRequestDT, string StrUserlogin)
        {
            int MB_REQUESTID;
            //Boolean Req_specimen_status = true;
            // Str_RequestAccess = Accessnumber ที่ได้รับจากหน้า Request entry
            // StrProtocolID = Protocol ID คือ รหัส ของ Protocol นั้น ๆ 
            // NewProtocol_num = Protocol Number ที่ ถูกสร้างมา จากการดึง Query ตัวอย่างเช่น P0100000001
            try
            {
                // Step 1. Insert MB_REQUESTS with Protocal number นำเข้า ใน MB_REQUESTS โดยเพิ่ม รหัส Protocol Number เช่น P0100000001
                //
                string StrREQUESTID = Query_REQUESTID_REQUESTS(Str_RequestAccess);
                Writedatalog.WriteLog(strDateTime + " Insert MB_REQUESTS " + StrREQUESTID);

                string sql_Insert_MB_REQUESTS = @"INSERT INTO MB_REQUESTS (PROTOCOLID,TESTID,REQUESTID,MBREQNUMBER,SUBREQUESTCREDATE,COLLECTIONDATE,RECEIVEDDATE,LOGUSERID,LOGDATE,MBREQEUSTSTATUS)
                        VALUES ('" + StrProtocolID + "','" + StrTestID + "','" + StrREQUESTID + "','" + NewProtocol_num + "','" + StrRequestDT + "','" + strDateTime + "','" + StrRequestDT + "','" + StrUserlogin + "','" + strDateTime + "','" + "1" + "'); SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(sql_Insert_MB_REQUESTS, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                MB_REQUESTID = Convert.ToInt32(cmd.ExecuteScalar());
                Writedatalog.WriteLog(strDateTime + " Insert MB_REQUESTS ==================================== " + sql_Insert_MB_REQUESTS);
                // Insert Stain if Protocol found Stain in Dababase
                // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                Query_Stain_Protocol_and_Insert(MB_REQUESTID, StrProtocolID, StrRequestDT, StrUserlogin);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1014 Insert_Into_MB_REQUESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void Query_Stain_Protocol_and_Insert(int MB_REQUESTID, string StrProtocolID, string StrRequestDT, string StrUserlogin)
        {
            try
            {
                string sql_Query_Stain = @"SELECT DICT_MB_PROTOCOLS.PROTOCOLID,DICT_MB_PROTOCOLS.PROTOCOLCODE 
,DICT_MB_STAINS.MBSTAINID,MBSTAINCODE,DICT_MB_STAINS.STAINNAME    
FROM DICT_MB_PROTOCOLS 
LEFT OUTER JOIN  DICT_MB_PROTOCOL_STAINS ON DICT_MB_PROTOCOLS.PROTOCOLID = DICT_MB_PROTOCOL_STAINS.PROTOCOLID
LEFT OUTER JOIN  DICT_MB_STAINS ON DICT_MB_PROTOCOL_STAINS.MBSTAINID = DICT_MB_STAINS.MBSTAINID
WHERE DICT_MB_PROTOCOLS.PROTOCOLID = '" + StrProtocolID + "'";

                Writedatalog.WriteLog(strDateTime + " Query in " + "[" + StrProtocolID + "] STAINS ==================================== " + sql_Query_Stain);

                SqlCommand cmd = new SqlCommand(sql_Query_Stain, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adap.Fill(ds, "STAIN_QUERY");
                if (ds.Tables["STAIN_QUERY"].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables["STAIN_QUERY"].Rows.Count; i++)
                    {
                        if (ds.Tables["STAIN_QUERY"].Rows[i]["MBSTAINID"].ToString() != "")
                        {
                            string Str_StrainID = ds.Tables["STAIN_QUERY"].Rows[i]["MBSTAINID"].ToString();
                            string sql_stain = @"INSERT INTO SUBREQMB_STAINS (MBSTAINID,SUBREQUESTID,CREATEUSER,CREATIONDATE,LOGUSERID,LOGDATE) 
                                                    VALUES ('" + Str_StrainID + "','" + MB_REQUESTID + "','" + StrUserlogin + "','" + StrRequestDT + "','" + StrUserlogin + "','" + StrRequestDT + "' )";
                            Writedatalog.WriteLog(strDateTime + " Insert Str_StrainID SUBREQMB_STAINS ==================================== " + sql_stain);

                            SqlCommand cmd_stain = new SqlCommand(sql_stain, conn);
                            SqlDataAdapter ada = new SqlDataAdapter(cmd_stain);
                            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                            cmd_stain.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1013 Query_Stain_Protocol_and_Insert SUBREQMB_STAINS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string[] Query_PROTOCOL(string StrTestcode)
        {
            string[] result = new string[3];
            try
            {
                string sql_PorotocolID = @"SELECT 
 DICT_TESTS.PROTOCOLID
,DICT_MB_PROTOCOLS.PROTOCOLCODE
,DICT_TESTS.TESTCODE
,DICT_TESTS.TESTNAME
,DICT_TESTS.PROTOCOLID
,DICT_TESTS.COLLMATERIALID
,DICT_TESTS.TESTID
 FROM DICT_TESTS
 LEFT OUTER JOIN DICT_MB_PROTOCOLS ON DICT_TESTS.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
 WHERE DICT_TESTS.TESTCODE='" + StrTestcode + "'";

                Writedatalog.WriteLog(strDateTime + "  Log:QueryProtocol_10001:Query Protocol ID? " + sql_PorotocolID);

                SqlCommand cmd = new SqlCommand(sql_PorotocolID, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_Query_IDPROTOCOL");
                cmd.ExecuteReader();

                if (ds.Tables["sql_Query_IDPROTOCOL"].Rows.Count > 0)
                {
                    result[0] = ds.Tables[0].Rows[0]["PROTOCOLID"].ToString();
                    result[1] = ds.Tables[0].Rows[0]["PROTOCOLCODE"].ToString();
                    result[2] = ds.Tables[0].Rows[0]["TESTID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :Q1007 Query_PROTOCOL \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        private string Chk_Length()
        {
            string result = "";
            try
            {
                string sql_LENGTH = @"SELECT PROTOCOL_LENGTH FROM DICT_SYSTEM_MB_CONFIG";
                SqlCommand cmd = new SqlCommand(sql_LENGTH, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_LENGTH");
                cmd.ExecuteReader();
                if (ds.Tables["sql_LENGTH"].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["PROTOCOL_LENGTH"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :L1475 string Chk_Length() \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        private string Query_PatientID(string Str_PATNUMBER)
        {
            string result = "";
            try
            {
                string sql_PATID = @"SELECT PATID,PATNUMBER FROM PATIENTS WHERE PATNUMBER='" + Str_PATNUMBER + "'";
                SqlCommand cmd = new SqlCommand(sql_PATID, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_Query_PatID");
                cmd.ExecuteReader();
                if (ds.Tables["sql_Query_PatID"].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["PATID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :REQENTRYQ1013 Query_PATID \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        private int Query_specimenID(string StrSpecimencode)
        {
            int result = 0;
            try
            {
                string sql_LENGTH = @"SELECT COLLMATERIALID FROM DICT_COLL_MATERIALS WHERE COLLMATERIALCODE='" + StrSpecimencode + "' ";
                SqlCommand cmd = new SqlCommand(sql_LENGTH, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_specimencode");
                cmd.ExecuteReader();
                if (ds.Tables["sql_specimencode"].Rows.Count > 0)
                {
                    result = Convert.ToInt32(ds.Tables[0].Rows[0]["COLLMATERIALID"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error SpecimenID From DICT_COLL_MATERIALS \r\nDetail : " + ex.Message, "Inform Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        private int GETNUMBERPROTOCOL(string StrProtocolID)
        {
            Writedatalog.WriteLog(strDateTime + "  Log:REQENTRYL4001  :GETNUMBERPROTOCOL GETNUMBERPROTOCOL : [" + StrProtocolID + "]");
            int LENGTH_FIRST = 0;
            int COUNTPROTOCOL = 0;
            try
            {
                string sql = "SELECT COUNTERNUMBER FROM PROTOCOL_COUNTERS WHERE PROTOCOLID ='" + StrProtocolID + "' ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds.Clear();
                cmd.ExecuteNonQuery();
                adp.Fill(ds, "COMPTURE");

                if (ds.Tables["COMPTURE"].Rows.Count > 0)
                {
                    COUNTPROTOCOL = Int32.Parse(ds.Tables["COMPTURE"].Rows[0][0].ToString()) + 1;
                    Writedatalog.WriteLog(strDateTime + "  Log:L4001.1:GETNUMBERPROTOCOL COMPTURE           : [" + ds.Tables["COMPTURE"].Rows.Count + "]");
                    Writedatalog.WriteLog(strDateTime + "  Log:L4001.2:GETNUMBERPROTOCOL COUNTPROTOCOL + 1  : [" + COUNTPROTOCOL + "]");
                }
                else
                {
                    LENGTH_FIRST = Query_Protocol_Start();
                    Writedatalog.WriteLog(strDateTime + "  Log Q4002.3 Length of protocol ############   " + "[" + LENGTH_FIRST + "]");
                    // change 
                    if (LENGTH_FIRST > 0)
                    {

                        string fmt = new String('0', LENGTH_FIRST - 1) + 1;

                        // !!!!ต้องปรับ ให้ Number counternumber ให้ Run ตาม Length
                        string sql_Insert_NEW_PROID = @"INSERT INTO PROTOCOL_COUNTERS(PROTOCOLID, COUNTERNUMBER, LENGTH, CREATEDATE, LOGUSER, LOGDATE)
                                                        VALUES('" + StrProtocolID + "','" + fmt + "','" + LENGTH_FIRST + "','" + strDateTime + "','" + "SYS" + "','" + strDateTime + "')";
                        Writedatalog.WriteLog(strDateTime + "  Log:L4001.3:Send_process NewProtocol_num" + "[" + sql_Insert_NEW_PROID + "]");
                        SqlCommand cmd_insert_PROTOCOL_COUNTER = new SqlCommand(sql_Insert_NEW_PROID, conn);
                        if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                        cmd_insert_PROTOCOL_COUNTER.ExecuteNonQuery();
                        COUNTPROTOCOL = 1;
                    }
                    else
                    {
                        MessageBox.Show("Error : REQENTRY1009.1 Please Contact to Administrator -- Error Not Found PROTOCOL NUMBER IN CONFIG ! \r\nDetail : ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :REQENTRY1009 Query Get Number of Protocol ! \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            return COUNTPROTOCOL;

        }
        private int Query_Protocol_Start()
        {
            int result = 0;
            try
            {
                string sql_LENGTH = @"SELECT PROTOCOL_LENGTH FROM DICT_SYSTEM_MB_CONFIG ";
                SqlCommand cmd = new SqlCommand(sql_LENGTH, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_LENGTH");
                cmd.ExecuteReader();
                if (ds.Tables["sql_LENGTH"].Rows.Count > 0)
                {
                    result = Convert.ToInt32(ds.Tables[0].Rows[0]["PROTOCOL_LENGTH"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Query_Protocol_Start DICT_SYSTEM_MB_CONFIG \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        private void Update_NEW_LENGTH(int NewLength, string StrProtocolID)
        {
            try
            {
                string sql_New_Length = @"UPDATE PROTOCOL_COUNTERS set LENGTH ='" + NewLength + "' WHERE PROTOCOLID='" + StrProtocolID + "'";
                SqlCommand cmd = new SqlCommand(sql_New_Length, conn);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Update LENGTH \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Update_New_PROTOCOL_NUMBER(string RUNNINGPROTOCOL, string StrProtocolID)
        {
            try
            {
                string sql_New_Length = @"UPDATE PROTOCOL_COUNTERS set COUNTERNUMBER ='" + RUNNINGPROTOCOL + "' WHERE PROTOCOLID='" + StrProtocolID + "'";
                SqlCommand cmd = new SqlCommand(sql_New_Length, conn);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :REQENTRY1010 Update Ptococol NewNUMBER \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void radioGroup3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup3.SelectedIndex == 0)
            {
                dateTimePicker_Req_CollectionDate.Enabled = false;
                timeEdit_Req_CollectionTime.Enabled = false;
            }
            else if (radioGroup3.SelectedIndex == 1)
            {
                dateTimePicker_Req_CollectionDate.Enabled = true;
                timeEdit_Req_CollectionTime.Enabled = true;
            }
        }
        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup2.SelectedIndex == 0)
            {
                Str_Req_Urgent = "0";
            }
            else if (radioGroup2.SelectedIndex == 1)
            {
                Str_Req_Urgent = "1";
            }
        }
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                Str_Req_IPDOPD = "0";
            }
            else if (radioGroup1.SelectedIndex == 1)
            {
                Str_Req_IPDOPD = "1";
            }

        }
        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (lblReqstatusform.Text == "Creation")
                {
                    Str_RequestAccess = textEdit_accessnumber.Text;                              // Request Accessnumber
                    Str_patientPatNumber = labelControl_patnum.Text;                             // Patient number
                    Str_patientTitle = labelControl_tltle.Text;                                  // Patient Title
                    Str_patientName = labelControl_name.Text;                                    // Patient Name
                    Str_patientBirthdate = labelControl_Birthdate.Text;                          // Patient Birth Date
                    Str_patientSex = labelControl_sex.Text;                                      // Patient Sex
                    Str_patientAge = labelControl_Age.Text;                                      // Patient Age
                    Str_patientDiagnostic = labelControl_diag.Text;                              // Patient Diagnostic of request
                    Str_patientComment = memoEdit_Comments.Text;                                 // Patient Comment of request
                    Str_Req_Doctor = gridLookUpEdit_Doctor.Text;                                 // Doctor request
                    Str_Req_Location = gridLookUpEdit_Location.Text;                             // Location at request
                    Str_Req_specimen = gridLookUpEdit_Specimen.Text;                             // Specimen of Request

                    Str_patientID = Query_PatientID(Str_patientPatNumber);


                    bool Gram = false;
                    bool Culture = false;
                    bool Hemo = false;

                    if (Str_patientID != "")
                    {
                        // Get data from date time Left panel
                        //
                        CultureInfo us = new CultureInfo("en-US");
                        string Get_Date_Request = dateTimePicker_Req_RequestDate.Value.Date.ToString("yyyy-MM-dd", us);
                        string Get_Time_Request = timeEdit_Req_RequestTime.Time.ToString("HH:mm:ss", us);
                        string Get_Date_Collection = dateTimePicker_Req_CollectionDate.Value.Date.ToString("yyyy-MM-dd", us);
                        string Get_Time_Collection = timeEdit_Req_CollectionTime.Time.ToString("HH:mm:ss", us);

                        string Str_Request_DT = Get_Date_Request + " " + Get_Time_Request;
                        string Str_Collect_DT = Get_Date_Collection + " " + Get_Time_Collection;

                        if (treeList_Hemo.AllNodesCount > 0 || treeList_Gram.AllNodesCount > 0 || treeList_Culture.AllNodesCount > 0)
                        {
                            // Loop check in Treelist select items
                            int Incheck_Gram = 0;
                            int Incheck_Culture = 0;
                            int Incheck_Hemo = 0;


                            if (treeList_Gram.AllNodesCount > 0)
                            {

                                for (int i = 0; i < treeList_Gram.AllNodesCount; i++)
                                {
                                    if (treeList_Gram.Nodes[i].CheckState == CheckState.Checked)
                                    {
                                        Gram = true;
                                        Incheck_Gram++;
                                    }
                                }
                            }
                            if (treeList_Culture.AllNodesCount > 0)
                            {
                                for (int i = 0; i < treeList_Culture.AllNodesCount; i++)
                                {
                                    if (treeList_Culture.Nodes[i].CheckState == CheckState.Checked)
                                    {
                                        Culture = true;
                                        Incheck_Culture++;
                                    }
                                }
                            }
                            if (treeList_Hemo.AllNodesCount > 0)
                            {
                                for (int i = 0; i < treeList_Hemo.AllNodesCount; i++)
                                {
                                    if (treeList_Hemo.Nodes[i].CheckState == CheckState.Checked)
                                    {
                                        Hemo = true;
                                        Incheck_Hemo++;
                                    }
                                }
                            }
                            // End Loop check in treelist select items

                            // Start process save if select items
                            //
                            if (Incheck_Gram > 0 || Incheck_Culture > 0 || Incheck_Hemo > 0)
                            {
                                Insert_Into_REQUESTS_Step1(Str_patientPatNumber, Str_RequestAccess, Str_Req_Location, Str_Req_Doctor, Str_Request_DT, Str_Collect_DT, Str_patientComment);
                                MessageBox.Show(strDateTime + "  Log: You want to save request?", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (Incheck_Gram > 0)
                                {
                                    Loop_save_Gram(Str_Request_DT, Str_Collect_DT);
                                }
                                if (Incheck_Culture > 0)
                                {
                                    Loop_save_Culture(Str_Request_DT, Str_Collect_DT);
                                }
                                if (Incheck_Hemo > 0)
                                {
                                    Loop_save_Hemo(Str_Request_DT, Str_Collect_DT);
                                }
                                this.Close();
                            }
                            if ((Gram == true) && (Culture == true) && (Hemo == true))
                            {
                                // Not thing.
                            }
                            else if ((Gram == false) && (Culture == true) && (Hemo == true))
                            {
                                // Not thing.
                            }
                            else if ((Gram == false) && (Culture == false) && (Hemo == true))
                            {
                                // Not thing.
                            }
                            else if ((Gram == true) && (Culture == false) && (Hemo == false))
                            {
                                // Not thing.
                            }
                            else if ((Gram == true) && (Culture == false) && (Hemo == true))
                            {
                                // Not thing.
                            }
                            else
                            {
                                MessageBox.Show(strDateTime + "  Log: Please select Item", "Inform you", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show(strDateTime + "  Log: Please select list for save requests" + Str_patientID, "Inform you", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show(strDateTime + "  Log: Not found data in this request", "Inform you", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (lblReqstatusform.Text == "Modification")
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chkSecret_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSecret.Checked == true)
            {
                StrSecret = "1";
            }
            else
            {
                StrSecret = "0";
            }
        }

        // Query Mod1001
        private void Modification_Detail_request(string StrAccessnumber)
        {
            string result = "";
            try
            {
                string sql_detail_req = @"SELECT REQUESTS.REQUESTID
,REQUESTS.PATID
,REQUESTS.ACCESSNUMBER
,REQUESTS.REQCREATIONDATE
,REQUESTS.SPECIMENCODE
,REQUESTS.CATEGORY
,REQUESTS.HOSTORDERNUMBER
,REQUESTS.REQSTATUS
,REQUESTS.REQ_IPDOROPD
,REQUESTS.STATUSDATE
,REQUESTS.REQDATE
,REQUESTS.COLLECTIONDATE
,REQUESTS.URGENT
,REQUESTS.COMMENT
,REQUESTS.SECRETRESULT
,REQUESTS.LASTUPDATE
,REQUESTS.RECEIVEDDATE
,REQUESTS.EXTERNALORDERNUMBER
,REQUESTS.REQDOCTOR
,REQUESTS.REQLOCATION
,REQUESTS.LOGUSERID
,REQUESTS.LOGDATE
 FROM REQUESTS

 WHERE REQUESTS.ACCESSNUMBER='" + StrAccessnumber + "'";

                SqlCommand cmd = new SqlCommand(sql_detail_req, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_Query_req1");
                cmd.ExecuteReader();

                if (ds.Tables["sql_Query_req1"].Rows.Count > 0)
                {

                    // IPD & OPD
                    if (ds.Tables[0].Rows[0]["REQ_IPDOROPD"].ToString() == "0")
                    {
                        
                        radioGroup1.SelectedIndex = 0;
                    }
                    else
                    {
                        radioGroup1.SelectedIndex = 1;
                    }
                    // Urgent request
                    if (ds.Tables[0].Rows[0]["URGENT"].ToString() == "0")
                    {

                        radioGroup2.SelectedIndex = 0;
                    }
                    else
                    {
                        radioGroup2.SelectedIndex = 1;
                    }
                    // Secret Result
                    if (ds.Tables[0].Rows[0]["SECRETRESULT"].ToString() == "1")
                    {

                        chkSecret.Checked = true;
                    }
                    else
                    {
                        chkSecret.Checked = false;
                    }
                    // DOCTOR & LOCATION
                    if (ds.Tables[0].Rows[0]["REQDOCTOR"].ToString() !="")
                    {
                    gridLookUpEdit_Doctor.EditValue = ds.Tables[0].Rows[0]["REQDOCTOR"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["REQLOCATION"].ToString() != "")
                    {
                        gridLookUpEdit_Location.EditValue = ds.Tables[0].Rows[0]["REQLOCATION"].ToString();
                    }

                    // Query Mod1002
                    Query_REQ_TESTS_modification(StrAccessnumber);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :Mod1001 Query_Mod_Requests \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Query Mod1002
        private void Query_REQ_TESTS_modification(string StrAccessnumber)
        {
            try
            {
                string sql = @"SELECT REQ_TESTS.REQTESTID
,DICT_TESTS.TESTCODE
,DICT_TESTS.TESTNAME
,DICT_COLL_MATERIALS.COLLMATERIALTEXT AS 'SPECIMEN_NAME'
,DICT_COLL_MATERIALS.COLLMATERIALCODE AS 'SPECIMEN_CODE'
,DICT_TESTS.TESTS_TAB
,REQ_TESTS.CHAPID
,REQ_TESTS.LABOID
,REQ_TESTS.TESTID
,REQ_TESTS.REQUESTID
,REQ_TESTS.COLLMATERIALID
,REQ_TESTS.STNID
,REQ_TESTS.DEPTH
,REQ_TESTS.NOTPRINTABLE
,REQ_TESTS.VALIDATIONCAUSE
,REQ_TESTS.VALIDATIONINITIALS
,REQ_TESTS.VALIDATIONSTATUS
,REQ_TESTS.RESVALUE
,REQ_TESTS.RESUPDDATE
,REQ_TESTS.RESSTATUS
,REQ_TESTS.LASTUPDATE
,REQ_TESTS.UPDSTATUS
,REQ_TESTS.ORDERPLACERNUMBER
,REQ_TESTS.ORDERFILLERNUMBER
,REQ_TESTS.SECRETRESULT
,REQ_TESTS.LOGUSERID
,REQ_TESTS.LOGDATE
,TECHNICALINITIALS
 FROM REQ_TESTS
 LEFT OUTER JOIN DICT_TESTS ON REQ_TESTS.TESTID = DICT_TESTS.TESTID
 LEFT OUTER JOIN REQUESTS ON REQUESTS.REQUESTID = REQ_TESTS.REQUESTID
 LEFT OUTER JOIN DICT_COLL_MATERIALS ON REQ_TESTS.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID
 WHERE REQUESTS.ACCESSNUMBER='" + StrAccessnumber + "' ORDER BY REQ_TESTS.REQUESTID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "result");
                cmd.ExecuteReader();

                string StrTest_Tab = "";
                int CheckNodeTAB1 = 0;
                int CheckNodeTAB2 = 0;
                int CheckNodeTAB3 = 0;

                if (ds.Tables["result"].Rows.Count > 0)
                {

                    //treeList_Gram.BeginUnboundLoad();
                    //treeList_Gram.ClearNodes();
                    //treeList_Culture.BeginUnboundLoad();
                    //treeList_Culture.ClearNodes();
                    //treeList_Hemo.BeginUnboundLoad();
                    //treeList_Hemo.ClearNodes();

                    TreeListNode parentForRootNodes = null;
                    // Start check TESTS_TAB 0-9
                    // NOW config only 0-2
                    // 0 = GRAM STAIN
                    // 1 = AROBE CULTURE
                    // 2 = HEMO CULTURE

                    for (int i = 0; i < ds.Tables["result"].Rows.Count; i++)
                    {
                        string StrStatus = ds.Tables["result"].Rows[i]["RESSTATUS"].ToString();

                        if (StrStatus != "1" || StrStatus != "2" || StrStatus != "3" || StrStatus != "4")
                        {
                            StrStatus = "NO";
                        }
                        else
                        {
                            StrStatus = "Yes";
                        }

                        // Grid ALL TESTS
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = (ds.Tables["result"].Rows[i]["TESTCODE"]).ToString();
                        dataGridView1.Rows[i].Cells[1].Value = (ds.Tables["result"].Rows[i]["TESTNAME"]).ToString();
                        dataGridView1.Rows[i].Cells[2].Value = StrStatus;
                        dataGridView1.Rows[i].Cells[3].Value = Properties.Resources.accept_icon16x16;
                        dataGridView1.Rows[i].Cells[4].Value = (ds.Tables["result"].Rows[i]["REQTESTID"]).ToString();

                        StrTest_Tab = ds.Tables["result"].Rows[i]["TESTS_TAB"].ToString();

                        if (StrTest_Tab =="0")
                        {
                            childNode = treeList_Gram.AppendNode(new object[] {
                        "",
                        "1",
                        ds.Tables["result"].Rows[i]["TESTCODE"].ToString(),
                        ds.Tables["result"].Rows[i]["TESTNAME"].ToString(),
                        ds.Tables["result"].Rows[i]["SPECIMEN_NAME"].ToString(),
                        ds.Tables["result"].Rows[i]["SPECIMEN_CODE"].ToString(),
                        "",
                        ds.Tables["result"].Rows[i]["REQTESTID"].ToString()}, parentForRootNodes);
                            treeList_Gram.EndUnboundLoad();
                            treeList_Gram.ExpandAll();

                            treeList_Gram.Nodes[CheckNodeTAB1].CheckState = CheckState.Checked;

                            CheckNodeTAB1++;

                        }
                        else if (StrTest_Tab == "1")
                        {
                            childNode = treeList_Culture.AppendNode(new object[] {
                        "",
                        "1",
                        ds.Tables["result"].Rows[i]["TESTCODE"].ToString(),
                        ds.Tables["result"].Rows[i]["TESTNAME"].ToString(),
                        ds.Tables["result"].Rows[i]["SPECIMEN_NAME"].ToString(),
                        ds.Tables["result"].Rows[i]["SPECIMEN_CODE"].ToString(),
                        "",
                        ds.Tables["result"].Rows[i]["REQTESTID"].ToString()}, parentForRootNodes);
                            treeList_Culture.EndUnboundLoad();
                            treeList_Culture.ExpandAll();

                            treeList_Culture.Nodes[CheckNodeTAB2].CheckState = CheckState.Checked;

                            CheckNodeTAB2++;

                        }
                        else if (StrTest_Tab == "2")
                        {
                            childNode = treeList_Hemo.AppendNode(new object[] {
                        "",
                        "1",
                        ds.Tables["result"].Rows[i]["TESTCODE"].ToString(),
                        ds.Tables["result"].Rows[i]["TESTNAME"].ToString(),
                        ds.Tables["result"].Rows[i]["SPECIMEN_NAME"].ToString(),
                        ds.Tables["result"].Rows[i]["SPECIMEN_CODE"].ToString(),
                        "",
                        ds.Tables["result"].Rows[i]["REQTESTID"].ToString()}, parentForRootNodes);
                            treeList_Hemo.EndUnboundLoad();
                            treeList_Hemo.ExpandAll();

                            treeList_Hemo.Nodes[CheckNodeTAB3].CheckState = CheckState.Checked;

                            CheckNodeTAB3++;
                        }
                    }

                }
                }
            catch (Exception ex)
            {
                MessageBox.Show("Error Mod1002 Query_REQ_TESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dateTimePicker_Req_RequestDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void timer_for_Modification_Tick(object sender, EventArgs e)
        {
            timeEdit_modTime.Text = DateTime.Now.ToString(StrDateTime_Modification_Requests, cultureinfo);
        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void memoEdit_Comments_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void treeList_Gram_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            if (e.State == CheckState.Indeterminate) e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
            treeList_Gram.FocusedNode = e.Node;
            if (e.State.ToString() == "Unchecked")
            {
                string CellREQTESTID = treeList_Gram.FocusedNode.GetDisplayText(7);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string Cell4 = row.Cells[4].Value.ToString();

                    if (Cell4 == CellREQTESTID)
                    {
                        row.Cells[3].Value = Properties.Resources.cancel_16x16;
                        row.Cells[5].Value = "ลบ";
                        row.DefaultCellStyle.BackColor = Color.LightPink;

                        dataGridView1.Refresh();
                    }
                }
            }
            else if (e.State.ToString() == "Checked")
           {
                string CellREQTESTID = treeList_Gram.FocusedNode.GetDisplayText(7);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string Cell4 = row.Cells[4].Value.ToString();

                    if (Cell4 == CellREQTESTID)
                    {
                        row.Cells[3].Value = Properties.Resources.accept_icon16x16;
                        row.Cells[5].Value = "";
                        row.DefaultCellStyle.BackColor = Color.White;

                        dataGridView1.Refresh();
                    }
                }
            }
        }

        private void treeList_Culture_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            if (e.State == CheckState.Indeterminate) e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
            treeList_Culture.FocusedNode = e.Node;
            if (e.State.ToString() == "Unchecked")
            {
                string CellREQTESTID = treeList_Culture.FocusedNode.GetDisplayText(7);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string Cell4 = row.Cells[4].Value.ToString();

                    if (Cell4 == CellREQTESTID)
                    {
                        row.Cells[3].Value = Properties.Resources.cancel_16x16;
                        row.Cells[5].Value = "ลบ";
                        row.DefaultCellStyle.BackColor = Color.LightPink;

                        dataGridView1.Refresh();
                    }
                }
            }
            else if (e.State.ToString() == "Checked")
            {
                string CellREQTESTID = treeList_Culture.FocusedNode.GetDisplayText(7);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string Cell4 = row.Cells[4].Value.ToString();

                    if (Cell4 == CellREQTESTID)
                    {
                        row.Cells[3].Value = Properties.Resources.accept_icon16x16;
                        row.Cells[5].Value = "";
                        row.DefaultCellStyle.BackColor = Color.White;

                        dataGridView1.Refresh();
                    }
                }
            }
        }

        private void treeList_Hemo_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            if (e.State == CheckState.Indeterminate) e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
            treeList_Hemo.FocusedNode = e.Node;
            if (e.State.ToString() == "Unchecked")
            {
                string CellREQTESTID = treeList_Hemo.FocusedNode.GetDisplayText(7);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string Cell4 = row.Cells[4].Value.ToString();

                    if (Cell4 == CellREQTESTID)
                    {
                        row.Cells[3].Value = Properties.Resources.cancel_16x16;
                        row.Cells[5].Value = "ลบ";
                        row.DefaultCellStyle.BackColor = Color.LightPink;

                        dataGridView1.Refresh();
                    }
                }
            }
            else if (e.State.ToString() == "Checked")
            {
                string CellREQTESTID = treeList_Hemo.FocusedNode.GetDisplayText(7);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string Cell4 = row.Cells[4].Value.ToString();

                    if (Cell4 == CellREQTESTID)
                    {
                        row.Cells[3].Value = Properties.Resources.accept_icon16x16;
                        row.Cells[5].Value = "";
                        row.DefaultCellStyle.BackColor = Color.White;

                        dataGridView1.Refresh();
                    }
                }

            }

        }
    }
}

