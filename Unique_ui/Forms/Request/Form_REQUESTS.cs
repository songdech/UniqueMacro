using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors;

using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using UNIQUE.PatientFld;
using UniquePro.Controller;
using UniquePro.Entities.Patient;
using UniquePro.Entities.OEM;
using UNIQUE.Result;
using UNIQUE.Forms.Request;

namespace UNIQUE.OEM
{
    public partial class Form_REQUESTS : Form
    {

        System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");
        string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));
        private static string Version = "Requests ";
        System.Globalization.CultureInfo cultureinfo_ENG = new System.Globalization.CultureInfo("en-US");

        SqlConnection conn;

        // Patient 

        string Str_DateOfBirth = "";        //Format full DOB
        string Str_DOB = "";                //Format DD-MM-YYYY

        int HN_LENGTH;
        int LN_LENGTH;
        string Psex = "";
        string strBirthDate = "";

        // Audit
        string ATR_Access = "";
        string ATR_StepType = "";
        string ATR_UserLogin = "";
        string ATR_UserID = "";
        string ATR_IPAddress = "";

        PatientController objPatient;
        OEMController objOrderEntry;

        private ConfigurationController objConfiguration = new ConfigurationController();

        PatientM objPatientM = null;

        private DataTable dtReq = null;


        TreeListNode childNode;
        TreeListNode rootNode;

        Boolean Get_check_patient_find = false;

        bool Treelist_with_search = false;

        // Query Critiria Search data
        string Filter_Critiria = "";
        // Filter between select Request infomation by Boolean

        //* Edit By Songdech S.
        //* Edit Date: 17/03/2021
        //* Add parameter for switch forms
        public Boolean Form_Search_TestResult { get; set; }

        public Form_REQUESTS()
        {
            Form_Search_TestResult = false;
            InitializeComponent();
        }

        private void Form_REQUESTS_Load(object sender, EventArgs e)
        {
            Clearinformationall();
            conn = new Connection_ORM().Connect();
            Chk_Length();
            this.Text = Version;
            comboBox1.SelectedIndex = 0;
            textEdit_search.Select();

            // Load Query Doctor && Location
            Combo_Search_critiria_Location();
            Combo_Search_critiria_Doctor();

            grdView.Visible = false;
            treeList1.Visible = false;

            if (Form_Search_TestResult == true)
            {
                grdView.Visible = true;
                this.Text = "Search Test Result";
            }
            else
            {
                treeList1.Visible = true;
                this.Text = "Search Request Order";
            }

            //groupControl1.Location = new Point(0, 57);
            //groupControl1.Size = new Size(924, 84);
            //groupControl2.Location = new Point(0, 147);
            //groupControl2.Size = new Size(924, 201);
            //groupControl3.Location = new Point(0, 354);
            //groupControl3.Size = new Size(924, 367);
            //treeList1.Location = new Point(5, 25);
            //treeList1.Size = new Size(914, 339);

        }
        private void searchDataInformations()
        {
            if (textEdit_search.Text != "")
            {
                // clearText();
                // ACCESSNUMBER
            }
        }

        private void Insert_Audit_TESTS()
        {
            try
            {
                    string REQ_createtionDate = DateTime.Now.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));
                    string sql_Insert = @"INSERT INTO AUDIT_TESTS (ATR_ACCESSNUMBER, ATR_CREATIONDATE, STEPDATE, STEPTYPE, INITUSER, ADDRESSEE, USERID, USERSESSION, ATR_ORDER) 
                                         VALUES ('" + ATR_Access + "','" + strDateTime + "','" + strDateTime + "','" + ATR_StepType + "','" + ATR_UserLogin + "','" + ATR_IPAddress + "','" + ATR_UserID + "','" +
                                             "REQUESTS" + "','" + "" + "')";
                    SqlCommand cmd = new SqlCommand(sql_Insert, conn);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "Error :L111 AUDIT: Insert_Audit_TESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreateColumns(TreeList tl, DataSet ds)
        {
            //  CreateColumns_patient_Accessnumber_search && CreateColumns
            // 0 = PATID 
            // 1 = Accessnumber 
            // 2 = Request Date 
            // 3 = Collection Date 
            // 4 = Receive Date 
            // 5 = Status 
            // 6 = User 
            // 7 = REQUESTID 
            // 8 = PATNUMBER
            // 9 = NAME 
            // 10 = LASTNAME 
            // 11 = AGEY 
            // 12 = AGEM 
            // 13 = SEX 
            // 14 = ALTNUMBER 
            // 15 = HOSTORDERNUMBER 
            // 16 = REQDOCTOR 
            // 17 = REQLOCATION 
            // 18 = COMMENT
            // 19 = TITLE1 
            // 20 = TITLE1
            // 21 = BIRTHDATE

            // Create three columns.
            tl.BeginUpdate();
            tl.Columns.Add();
            tl.Columns[0].Caption = "<i><b>PatID</i></b>";
            tl.Columns[0].VisibleIndex = 0;
            tl.Columns[0].Width = 80;
            tl.Columns[0].Visible = false;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.RowHeight = 23;
            tl.Columns.Add();

            tl.Columns[1].Caption = "<i>Request</i><b>Accessnumber</b>";
            tl.Columns[1].VisibleIndex = 1;
            tl.Columns[1].Width = 150;
            tl.Columns[1].Visible = true;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.Columns.Add();

            tl.Columns[2].Caption = "Request Date";
            tl.Columns[2].VisibleIndex = 2;
            tl.Columns[2].Width = 200;
            tl.Columns[2].Visible = true;
            tl.Columns.Add();

            tl.Columns[3].Caption = "Collection Date";
            tl.Columns[3].VisibleIndex = 3;
            tl.Columns[3].Width = 200;
            tl.Columns[3].Visible = true;
            tl.Columns.Add();

            tl.Columns[4].Caption = "Receive Date";
            tl.Columns[4].VisibleIndex = 4;
            tl.Columns[4].Width = 200;
            tl.Columns[4].Visible = true;
            tl.Columns.Add();

            tl.Columns[5].Caption = "Status";
            tl.Columns[5].VisibleIndex = 5;
            tl.Columns[5].Width = 50;
            tl.Columns[5].Visible = true;
            tl.Columns.Add();

            tl.Columns[6].Caption = "User";
            tl.Columns[6].VisibleIndex = 6;
            tl.Columns[6].Width = 90;
            tl.Columns[6].Visible = true;
            tl.Columns.Add();

            tl.Columns[7].Caption = "REQUESTID";
            tl.Columns[7].VisibleIndex = 7;
            tl.Columns[7].Width = 50;
            tl.Columns[7].Visible = false;
            tl.Columns.Add();

            tl.Columns[8].Caption = "PATNUMBER";
            tl.Columns[8].VisibleIndex = 8;
            tl.Columns[8].Width = 10;
            tl.Columns[8].Visible = false;
            tl.Columns.Add();

            tl.Columns[9].Caption = "NAME";
            tl.Columns[9].VisibleIndex = 9;
            tl.Columns[9].Width = 10;
            tl.Columns[9].Visible = false;
            tl.Columns.Add();

            tl.Columns[10].Caption = "LASTNAME";
            tl.Columns[10].VisibleIndex = 10;
            tl.Columns[10].Width = 10;
            tl.Columns[10].Visible = false;
            tl.Columns.Add();

            tl.Columns[11].Caption = "AGEY";
            tl.Columns[11].VisibleIndex = 11;
            tl.Columns[11].Width = 10;
            tl.Columns[11].Visible = false;
            tl.Columns.Add();

            tl.Columns[12].Caption = "AGEM";
            tl.Columns[12].VisibleIndex = 12;
            tl.Columns[12].Width = 10;
            tl.Columns[12].Visible = false;
            tl.Columns.Add();

            tl.Columns[13].Caption = "SEX";
            tl.Columns[13].VisibleIndex = 13;
            tl.Columns[13].Width = 10;
            tl.Columns[13].Visible = false;
            tl.Columns.Add();

            tl.Columns[14].Caption = "ALTNUMBER";
            tl.Columns[14].VisibleIndex = 14;
            tl.Columns[14].Width = 10;
            tl.Columns[14].Visible = false;
            tl.Columns.Add();

            tl.Columns[15].Caption = "HOSTORDERNUMBER";
            tl.Columns[15].VisibleIndex = 15;
            tl.Columns[15].Width = 10;
            tl.Columns[15].Visible = false;
            tl.Columns.Add();

            tl.Columns[16].Caption = "REQDOCTOR";
            tl.Columns[16].VisibleIndex = 16;
            tl.Columns[16].Width = 10;
            tl.Columns[16].Visible = false;
            tl.Columns.Add();

            tl.Columns[17].Caption = "REQLOCATION";
            tl.Columns[17].VisibleIndex = 17;
            tl.Columns[17].Width = 10;
            tl.Columns[17].Visible = false;
            tl.Columns.Add();

            tl.Columns[18].Caption = "COMMENT";
            tl.Columns[18].VisibleIndex = 18;
            tl.Columns[18].Width = 10;
            tl.Columns[18].Visible = false;
            tl.Columns.Add();

            tl.Columns[19].Caption = "TITLE1";
            tl.Columns[19].VisibleIndex = 19;
            tl.Columns[19].Width = 10;
            tl.Columns[19].Visible = false;
            tl.Columns.Add();

            tl.Columns[20].Caption = "TITLE2";
            tl.Columns[20].VisibleIndex = 20;
            tl.Columns[20].Width = 10;
            tl.Columns[20].Visible = false;
            tl.Columns.Add();

            tl.Columns[21].Caption = "BIRTHDATE";
            tl.Columns[21].VisibleIndex = 21;
            tl.Columns[21].Width = 10;
            tl.Columns[21].Visible = false;
            tl.Columns.Add();

            tl.EndUpdate();
        }
        private void CreateColumns_patient_Accessnumber_search(TreeList tl, DataSet ds)
        {
            // 0 = PATID 1 = Accessnumber 2 = Request Date 3 = Collection Date 4 = Receive Date 5 = Status 6 = User 7 = REQUESTID 8 = PATNUMBER
            // 9 = NAME 10 = LASTNAME 11 = AGEY 12 = AGEM 13 = SEX 14 = ALTNUMBER 15 = HOSTORDERNUMBER 16 = REQDOCTOR 17 = REQLOCATION 18 = COMMENT
            // 19 = TITLE1 20 = TITLE1
            // 22 = IPDOPD
            // 23 = URGENT
            // 24 = SECRETRESULT;

            // Create three columns.
            tl.BeginUpdate();
            tl.Columns.Add();
            tl.Columns[0].Caption = "<i><b>PatID</i></b>";
            tl.Columns[0].VisibleIndex = 0;
            tl.Columns[0].Width = 80;
            tl.Columns[0].Visible = false;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.RowHeight = 23;
            tl.Columns.Add();

            tl.Columns[1].Caption = "<i>Request</i><b>Accessnumber</b>";
            tl.Columns[1].VisibleIndex = 1;
            tl.Columns[1].Width = 150;
            tl.Columns[1].Visible = true;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.Columns.Add();

            tl.Columns[2].Caption = "Request Date";
            tl.Columns[2].VisibleIndex = 2;
            tl.Columns[2].Width = 200;
            tl.Columns[2].Visible = true;
            tl.Columns.Add();

            tl.Columns[3].Caption = "Collection Date";
            tl.Columns[3].VisibleIndex = 3;
            tl.Columns[3].Width = 200;
            tl.Columns[3].Visible = true;
            tl.Columns.Add();

            tl.Columns[4].Caption = "Receive Date";
            tl.Columns[4].VisibleIndex = 4;
            tl.Columns[4].Width = 200;
            tl.Columns[4].Visible = true;
            tl.Columns.Add();

            tl.Columns[5].Caption = "Status";
            tl.Columns[5].VisibleIndex = 5;
            tl.Columns[5].Width = 50;
            tl.Columns[5].Visible = true;
            tl.Columns.Add();

            tl.Columns[6].Caption = "User";
            tl.Columns[6].VisibleIndex = 6;
            tl.Columns[6].Width = 90;
            tl.Columns[6].Visible = true;
            tl.Columns.Add();

            tl.Columns[7].Caption = "REQUESTID";
            tl.Columns[7].VisibleIndex = 7;
            tl.Columns[7].Width = 50;
            tl.Columns[7].Visible = false;
            tl.Columns.Add();

            tl.Columns[8].Caption = "PATNUMBER";
            tl.Columns[8].VisibleIndex = 8;
            tl.Columns[8].Width = 10;
            tl.Columns[8].Visible = false;
            tl.Columns.Add();

            tl.Columns[9].Caption = "NAME";
            tl.Columns[9].VisibleIndex = 9;
            tl.Columns[9].Width = 10;
            tl.Columns[9].Visible = false;
            tl.Columns.Add();

            tl.Columns[10].Caption = "LASTNAME";
            tl.Columns[10].VisibleIndex = 10;
            tl.Columns[10].Width = 10;
            tl.Columns[10].Visible = false;
            tl.Columns.Add();

            tl.Columns[11].Caption = "AGEY";
            tl.Columns[11].VisibleIndex = 11;
            tl.Columns[11].Width = 10;
            tl.Columns[11].Visible = false;
            tl.Columns.Add();

            tl.Columns[12].Caption = "AGEM";
            tl.Columns[12].VisibleIndex = 12;
            tl.Columns[12].Width = 10;
            tl.Columns[12].Visible = false;
            tl.Columns.Add();

            tl.Columns[13].Caption = "SEX";
            tl.Columns[13].VisibleIndex = 13;
            tl.Columns[13].Width = 10;
            tl.Columns[13].Visible = false;
            tl.Columns.Add();

            tl.Columns[14].Caption = "ALTNUMBER";
            tl.Columns[14].VisibleIndex = 14;
            tl.Columns[14].Width = 10;
            tl.Columns[14].Visible = false;
            tl.Columns.Add();

            tl.Columns[15].Caption = "HOSTORDERNUMBER";
            tl.Columns[15].VisibleIndex = 15;
            tl.Columns[15].Width = 10;
            tl.Columns[15].Visible = false;
            tl.Columns.Add();

            tl.Columns[16].Caption = "REQDOCTOR";
            tl.Columns[16].VisibleIndex = 16;
            tl.Columns[16].Width = 10;
            tl.Columns[16].Visible = false;
            tl.Columns.Add();

            tl.Columns[17].Caption = "REQLOCATION";
            tl.Columns[17].VisibleIndex = 17;
            tl.Columns[17].Width = 10;
            tl.Columns[17].Visible = false;
            tl.Columns.Add();

            tl.Columns[18].Caption = "COMMENT";
            tl.Columns[18].VisibleIndex = 18;
            tl.Columns[18].Width = 10;
            tl.Columns[18].Visible = false;
            tl.Columns.Add();

            tl.Columns[19].Caption = "TITLE1";
            tl.Columns[19].VisibleIndex = 19;
            tl.Columns[19].Width = 10;
            tl.Columns[19].Visible = false;
            tl.Columns.Add();

            tl.Columns[20].Caption = "TITLE2";
            tl.Columns[20].VisibleIndex = 20;
            tl.Columns[20].Width = 10;
            tl.Columns[20].Visible = false;
            tl.Columns.Add();

            tl.Columns[21].Caption = "BIRTHDATE";
            tl.Columns[21].VisibleIndex = 21;
            tl.Columns[21].Width = 10;
            tl.Columns[21].Visible = false;
            tl.Columns.Add();

            tl.Columns[22].Caption = "IPDOPD";
            tl.Columns[22].VisibleIndex = 22;
            tl.Columns[22].Width = 10;
            tl.Columns[22].Visible = false;
            tl.Columns.Add();

            tl.Columns[23].Caption = "URGENT";
            tl.Columns[23].VisibleIndex = 23;
            tl.Columns[23].Width = 10;
            tl.Columns[23].Visible = false;
            tl.Columns.Add();

            tl.Columns[24].Caption = "SECRETRESULT";
            tl.Columns[24].VisibleIndex = 24;
            tl.Columns[24].Width = 10;
            tl.Columns[24].Visible = false;
            tl.Columns.Add();


            tl.EndUpdate();
        }

        private void CreateColumns_patientSearch(TreeList tl, DataSet ds)
        {
            // Create three columns.
            //  CreateColumns_patientSearch
            // 0 = PATID 
            // 1 = PATNUMBER 
            // 2 = NAME 
            // 3 = LASTNAME 
            // 4 = REQDOCTOR 
            // 5 = REQLOCATION 
            // 6 = COMMENT 
            // 7 = BIRTHDATE 
            // 8 = SEX
            // 9 = AGEY 
            // 10 = AGEM 
            // 11 = TITLE1 
            // 12 = TITLE2 

            tl.BeginUpdate();
            tl.Columns.Add();
            tl.Columns[0].Caption = "<i><b>PatID</i></b>";
            tl.Columns[0].VisibleIndex = 0;
            tl.Columns[0].Width = 80;
            tl.Columns[0].Visible = false;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.RowHeight = 23;
            tl.Columns.Add();

            tl.Columns[1].Caption = "<i>Request</i><b>PATNUMBER</b>";
            tl.Columns[1].VisibleIndex = 1;
            tl.Columns[1].Width = 150;
            tl.Columns[1].Visible = true;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.Columns.Add();

            tl.Columns[2].Caption = "Name";
            tl.Columns[2].VisibleIndex = 2;
            tl.Columns[2].Width = 200;
            tl.Columns[2].Visible = true;
            tl.Columns.Add();

            tl.Columns[3].Caption = "Lastname";
            tl.Columns[3].VisibleIndex = 3;
            tl.Columns[3].Width = 200;
            tl.Columns[3].Visible = true;
            tl.Columns.Add();

            tl.Columns[4].Caption = "Refer Doctor";
            tl.Columns[4].VisibleIndex = 4;
            tl.Columns[4].Width = 200;
            tl.Columns[4].Visible = true;
            tl.Columns.Add();

            tl.Columns[5].Caption = "Refer Location";
            tl.Columns[5].VisibleIndex = 5;
            tl.Columns[5].Width = 50;
            tl.Columns[5].Visible = true;
            tl.Columns.Add();

            tl.Columns[6].Caption = "Comment";
            tl.Columns[6].VisibleIndex = 6;
            tl.Columns[6].Width = 90;
            tl.Columns[6].Visible = true;
            tl.Columns.Add();

            tl.Columns[7].Caption = "BIRTHDATE";
            tl.Columns[7].VisibleIndex = 7;
            tl.Columns[7].Width = 90;
            tl.Columns[7].Visible = false;
            tl.Columns.Add();

            tl.Columns[8].Caption = "SEX";
            tl.Columns[8].VisibleIndex = 8;
            tl.Columns[8].Width = 90;
            tl.Columns[8].Visible = false;
            tl.Columns.Add();

            tl.Columns[9].Caption = "AGEY";
            tl.Columns[9].VisibleIndex = 9;
            tl.Columns[9].Width = 90;
            tl.Columns[9].Visible = false;
            tl.Columns.Add();

            tl.Columns[10].Caption = "AGEM";
            tl.Columns[10].VisibleIndex = 10;
            tl.Columns[10].Width = 90;
            tl.Columns[10].Visible = false;
            tl.Columns.Add();

            tl.Columns[11].Caption = "TITLE1";
            tl.Columns[11].VisibleIndex = 11;
            tl.Columns[11].Width = 90;
            tl.Columns[11].Visible = false;
            tl.Columns.Add();

            tl.Columns[12].Caption = "TITLE2";
            tl.Columns[12].VisibleIndex = 12;
            tl.Columns[12].Width = 90;
            tl.Columns[12].Visible = false;
            tl.Columns.Add();

            tl.EndUpdate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // PATNUMBER
            if (comboBox1.SelectedIndex == 0)
            {
                Get_check_patient_find = false;
                Clearinformation_combo1();
                textEdit_search.Properties.MaxLength = HN_LENGTH;
                textEdit_search.Text = "";
                textEdit_search.Select();
                label1.Visible = true;
                label1.Text = HN_LENGTH + " digit";

                textEdit_Search_text.Enabled = true;

            }
            // SAMPLE NUMBER
            else if (comboBox1.SelectedIndex == 1)
            {
                Get_check_patient_find = false;
                Clearinformation_combo1();
                textEdit_search.Properties.MaxLength = LN_LENGTH;
                label1.Text = LN_LENGTH + " digit";
                textEdit_search.Text = "";
                textEdit_search.Select();

                // Clear Treelist
                treeList1.ClearNodes();
                treeList1.Columns.Clear();

                textEdit_Search_text.Enabled = false;

            }
            // 
            else if (comboBox1.SelectedIndex == 2)
            {
                Clearinformation_combo1();
                textEdit_search.Properties.MaxLength = 11;
                textEdit_search.Text = "";
                textEdit_search.Select();
                label1.Text = "Search Protocol";

                textEdit_Search_text.Enabled = false;


            }
            else if (comboBox1.SelectedIndex == 3)
            {
                Clearinformation_combo1();
                textEdit_search.Properties.MaxLength = 20;
                textEdit_search.Text = "";
                textEdit_search.Select();
                label1.Text = "Search Hostpital number";

                textEdit_Search_text.Enabled = false;


            }

            else
            {
                MessageBox.Show("Please select Source first");
            }

        }

        private void textEdit_search_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 0)
            //    {
            //        // Search Patient..
            //        treeList1.ClearNodes();
            //        treeList1.Columns.Clear();
            //        Process_FindPatients();
            //    }
            //    else if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 1)
            //    {
            //        // Search Accessnumber..
            //        treeList1.ClearNodes();
            //        treeList1.Columns.Clear();
            //        Process_FindAccessnumber();
            //    }
            //    else if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 2)
            //    {
            //        // Search Protocol
            //        treeList1.ClearNodes();
            //        treeList1.Columns.Clear();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please select source first");
            //    }
            //}
        }
        private void dropDownButton_SearchPAT_Click(object sender, EventArgs e)
        {
            if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 0)
            {
                // Search Patient..
                // Set boolean true if select find Patient and Accessnumber
                Get_check_patient_find = false;
                // Start Process
                if (Form_Search_TestResult == false)
                {
                    Process_FindPatients();
                }
                else
                {
                    Process_FindPatientsWithTest();
                }
                
            }
            else if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 1)
            {
                // Search Accessnumber..
                // Set boolean false if select search by Accessnumber
                Get_check_patient_find = false;
                // Start Process
                /*Edit By Songdech S.
                 * Edit Date: 17/03/21
                 * Description: add new function for search request test
                 */
                if (Form_Search_TestResult == true)
                {
                    Process_FindAccessnumber_WithTestResult();
                }
                else
                {
                    Process_FindAccessnumber();
                }
                    
                
            }
            else if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 2)
            {
                // Search Protocol
            }
            else
            {
                MessageBox.Show("Please select source first");
            }
        }

        private void Process_FindPatients()
        {
            string Strsex = "";

            try
            {

                string sql_Query_Patient = @"SELECT 
 PATID
,PATNUMBER
,NAME
,LASTNAME
,ADDRESS1,ADDRESS2,CITY
,STATE,POSTALCODE,COUNTRY
,PATIENTS.BIRTHDATE
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as AGEY 
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as AGEM
,SEX,TELEPHON,TELEPHON2,FAX,EMAIL,INCOMMINGDATE,ROOMNUMBER
,REFDOCTOR,REFLOCATION,LASTUPDTESTDATE,BG_ABO,BG_RHESUS,BG_PHENOTYPES
,BG_KELL,DEATHDATE,SECRETRESULT,VIP,DOCID,PATCREATIONDATE,STARTVALIDDATE
,ENDVALIDDATE,CREATEBY,LOGUSERID,LOGDATE,COMMENT
 FROM PATIENTS WHERE PATNUMBER='" + textEdit_search.Text + "' ";

                Writedatalog.WriteLog(strDateTime + " Query Patient " + "[" + "" + "] Query ==================================== " + sql_Query_Patient);

                SqlCommand cmd = new SqlCommand(sql_Query_Patient, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adap.Fill(ds, "Query_patient");

                if (ds.Tables["Query_patient"].Rows.Count > 0)
                {
                    string StrCOMMENTS = ds.Tables["Query_patient"].Rows[0]["COMMENT"].ToString();
                    memoEdit_Comments.Text = StrCOMMENTS;
                    if (ds.Tables[0].Rows[0]["PATNUMBER"].ToString() != "")
                    { txtPatnum.Text = ds.Tables[0].Rows[0]["PATNUMBER"].ToString().TrimStart('0'); }
                    if (ds.Tables["Query_patient"].Rows[0]["SEX"].ToString() == "1")
                    { Strsex = "ชาย" + " /"; }
                    else if (ds.Tables["Query_patient"].Rows[0]["SEX"].ToString() == "2")
                    { Strsex = "หญิง" + " /"; }
                    else
                    { Strsex = "ไม่ระบุ" + " /"; }
                    labelControl_name.Text = ds.Tables[0].Rows[0]["NAME"].ToString() + "  " + ds.Tables["Query_patient"].Rows[0]["LASTNAME"].ToString();
                    labelControl_sex.Text = Strsex;
                    //DateTime DT = Convert.ToDateTime(ds.Tables[0].Rows[0]["REQDATE"].ToString());
                    //string StrReqDate = DT.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));
                    if (ds.Tables[0].Rows[0]["BIRTHDATE"].ToString() != "")
                    {
                        //Str_DOB = ((DateTime)(ds.Tables["Query_patient"].Rows[0]["BIRTHDATE"])).ToString("dd-MM-yyyy", cultureinfo_ENG);
                        Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(ds.Tables["Query_patient"].Rows[0]["BIRTHDATE"].ToString()));

                        // Start Date of Birth in calcu
                        if (Str_DOB == null || Str_DOB == "")
                        {
                            Str_DateOfBirth = "-";
                        }
                        else
                        {
                            string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                            int Str_Year = Convert.ToInt32(Array_DOB[2]);
                            int Str_Month = Convert.ToInt32(Array_DOB[1]);
                            int Str_Day = Convert.ToInt32(Array_DOB[0]);

                            DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                            DateTime ToDate = DateTime.Now;
                            DateDifference dDiff = new DateDifference(myDate, ToDate);

                            Str_DateOfBirth = dDiff.ToString();
                        }
                        // END Date of Birth in calcu

                        DateTime bDt = Convert.ToDateTime(ds.Tables[0].Rows[0]["BIRTHDATE"].ToString());
                        strBirthDate = bDt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));

                        labelControl_Birthdate.Text = Str_DOB;
                        labelControl_Age.Text = Str_DateOfBirth;
                    }
                    // Active Order entry with patient search
                    Process_FindAccessnumber_with_patient();
                }
                else
                {
                    // Search by like in Form_patientLists
                    //
                    PatientController objPatient = new PatientController();
                    DataTable dt = null;

                    PatientM objPatientM = new PatientM();

                    try
                    {
                        if (textEdit_search.Text != "")
                        {
                            objPatientM.PatientNo = "%" + textEdit_search.Text + "%";
                            dt = objPatient.GetPatientNumber_(objPatientM);

                            if (dt.Rows.Count != 0)
                            {
                                Form_PatientLists FM = new Form_PatientLists(objPatientM.PatientNo);
                                FM.ShowDialog();

                                textEdit_search.Text = ControlParameter.PatientM.PatientNo.TrimStart('0');
                                string Str_patnumber = ControlParameter.PatientM.PatientNo;

                                Process_FindPatients_Form_PatientLists(Str_patnumber);

                            }
                            else
                            {

                                DialogResult yes = MessageBox.Show("Not Found patient do you want Insert Patient ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (yes == DialogResult.Yes)
                                {
                                    frmPatientRegister FM = new frmPatientRegister("New");
                                    FM.ShowDialog();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "Error :L737 Query_Patient and_Insert SUBREQMB_STAINS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void Process_FindPatients_Form_PatientLists(string Str_Patnumber)
        {
            string Strsex = "";

            try
            {

                string sql_Query_Patient = @"SELECT 
 PATID
,PATNUMBER
,NAME
,LASTNAME
,ADDRESS1,ADDRESS2,CITY
,STATE,POSTALCODE,COUNTRY
,PATIENTS.BIRTHDATE
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as AGEY 
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as AGEM
,SEX,TELEPHON,TELEPHON2,FAX,EMAIL,INCOMMINGDATE,ROOMNUMBER
,REFDOCTOR,REFLOCATION,LASTUPDTESTDATE,BG_ABO,BG_RHESUS,BG_PHENOTYPES
,BG_KELL,DEATHDATE,SECRETRESULT,VIP,DOCID,PATCREATIONDATE,STARTVALIDDATE
,ENDVALIDDATE,CREATEBY,LOGUSERID,LOGDATE,COMMENT
 FROM PATIENTS WHERE PATNUMBER='" + Str_Patnumber + "' ";

                SqlCommand cmd = new SqlCommand(sql_Query_Patient, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adap.Fill(ds, "Query_patient");

                if (ds.Tables["Query_patient"].Rows.Count > 0)
                {
                    string StrCOMMENTS = ds.Tables["Query_patient"].Rows[0]["COMMENT"].ToString();
                    memoEdit_Comments.Text = StrCOMMENTS;
                    if (ds.Tables[0].Rows[0]["PATNUMBER"].ToString() != "")
                    { txtPatnum.Text = ds.Tables[0].Rows[0]["PATNUMBER"].ToString().TrimStart('0'); }
                    if (ds.Tables["Query_patient"].Rows[0]["SEX"].ToString() == "1")
                    { Strsex = "ชาย" + " /"; }
                    else if (ds.Tables["Query_patient"].Rows[0]["SEX"].ToString() == "2")
                    { Strsex = "หญิง" + " /"; }
                    else
                    { Strsex = "ไม่ระบุ" + " /"; }
                    labelControl_name.Text = ds.Tables[0].Rows[0]["NAME"].ToString() + "  " + ds.Tables["Query_patient"].Rows[0]["LASTNAME"].ToString();
                    labelControl_sex.Text = Strsex;
                    if (ds.Tables[0].Rows[0]["BIRTHDATE"].ToString() != "")
                    {
                        Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(ds.Tables["Query_patient"].Rows[0]["BIRTHDATE"].ToString()));

                        // Start Date of Birth in calcu
                        if (Str_DOB == null || Str_DOB == "")
                        {
                            Str_DateOfBirth = "-";
                        }
                        else
                        {
                            string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                            int Str_Year = Convert.ToInt32(Array_DOB[2]);
                            int Str_Month = Convert.ToInt32(Array_DOB[1]);
                            int Str_Day = Convert.ToInt32(Array_DOB[0]);

                            DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                            DateTime ToDate = DateTime.Now;
                            DateDifference dDiff = new DateDifference(myDate, ToDate);

                            Str_DateOfBirth = dDiff.ToString();
                        }
                        // END Date of Birth in calcu

                        DateTime bDt = Convert.ToDateTime(ds.Tables[0].Rows[0]["BIRTHDATE"].ToString());
                        strBirthDate = bDt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));

                        labelControl_Birthdate.Text = Str_DOB;
                        labelControl_Age.Text = Str_DateOfBirth;
                    }
                    // Active Order entry with patient search
                    Process_FindAccessnumber_with_patient();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "Error :L738 Query_Patient From Patient Lists \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Process_FindAccessnumber()
        {
            string Strsex = "";
            string Str_Req_status = "";
            try
            {
                string sql_accessnumber_search = "";
                sql_accessnumber_search = @"SELECT REQUESTS.REQUESTID,REQUESTS.PATID,REQUESTS.ACCESSNUMBER
,PATIENTS.PATNUMBER
,PATIENTS.TITLE1
,PATIENTS.TITLE2
,PATIENTS.NAME,PATIENTS.LASTNAME
,PATIENTS.BIRTHDATE
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as AGEY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as AGEM
,PATIENTS.SEX
,PATIENTS.ALTNUMBER
,REQUESTS.HOSTORDERNUMBER
,REQUESTS.REQDOCTOR
,REQUESTS.REQLOCATION
,REQUESTS.REQCREATIONDATE,REQUESTS.REQSTATUS,REQUESTS.STATUSDATE
,REQUESTS.REQDATE,REQUESTS.COLLECTIONDATE,REQUESTS.COMMENT,REQUESTS.LASTUPDATE
,REQUESTS.RECEIVEDDATE,REQUESTS.LOGUSERID,REQUESTS.LOGDATE
FROM REQUESTS
LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
WHERE REQUESTS.ACCESSNUMBER='" + textEdit_search.Text + "' AND REQUESTS.REQSTATUS =10";

                Writedatalog.WriteLog(strDateTime + " Query Accessnumber " + "[" + "" + "] Query Accessnumber================ " + sql_accessnumber_search);

                SqlCommand cmd = new SqlCommand(sql_accessnumber_search, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adap.Fill(ds, "Query_accessnumber");

                CreateColumns(treeList1, ds);
                treeList1.BeginUnboundLoad();
                treeList1.ClearNodes();

                TreeListNode parentForRootNodes = null;

                /*
                 * 
            //  CreateColumns_patient_Accessnumber_search && CreateColumns
            // 0 = PATID 
            // 1 = Accessnumber 
            // 2 = Request Date 
            // 3 = Collection Date 
            // 4 = Receive Date 
            // 5 = Status 
            // 6 = User 
            // 7 = REQUESTID 
            // 8 = PATNUMBER
            // 9 = NAME 
            // 10 = LASTNAME 
            // 11 = AGEY 
            // 12 = AGEM 
            // 13 = SEX 
            // 14 = ALTNUMBER 
            // 15 = HOSTORDERNUMBER 
            // 16 = REQDOCTOR 
            // 17 = REQLOCATION 
            // 18 = COMMENT
            // 19 = TITLE1 
            // 20 = TITLE1
            // 21 = BIRTHDATE
                 */
                if (ds.Tables["Query_accessnumber"].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables["Query_accessnumber"].Rows.Count; i++)
                    {

                        if (ds.Tables["Query_accessnumber"].Rows[i]["REQSTATUS"].ToString() == "10")
                        { Str_Req_status = "Open"; }
                        else if (ds.Tables["Query_accessnumber"].Rows[i]["REQSTATUS"].ToString() == "11")
                        { Str_Req_status = "Close"; }

                        string StrCOMMENTS = ds.Tables["Query_accessnumber"].Rows[i]["COMMENT"].ToString();
                        memoEdit_Comments.Text = StrCOMMENTS;

                        childNode = treeList1.AppendNode(new object[] {
                        ds.Tables["Query_accessnumber"].Rows[i]["PATID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["ACCESSNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQDATE"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["COLLECTIONDATE"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["RECEIVEDDATE"].ToString(),
                        Str_Req_status,
                        ds.Tables["Query_accessnumber"].Rows[i]["LOGUSERID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQUESTID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["PATNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["NAME"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["LASTNAME"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["AGEY"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["AGEM"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["SEX"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["ALTNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["HOSTORDERNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQDOCTOR"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQLOCATION"].ToString(),
                        StrCOMMENTS,
                        ds.Tables["Query_accessnumber"].Rows[i]["TITLE1"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["TITLE2"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["BIRTHDATE"].ToString()
                        }, parentForRootNodes);

                        if (ds.Tables[0].Rows[0]["PATNUMBER"].ToString() != "")
                        { txtPatnum.Text = ds.Tables[0].Rows[0]["PATNUMBER"].ToString().TrimStart('0'); }
                        if (ds.Tables["Query_accessnumber"].Rows[0]["SEX"].ToString() == "1")
                        { Strsex = "ชาย"; }
                        else if (ds.Tables["Query_accessnumber"].Rows[0]["SEX"].ToString() == "2")
                        { Strsex = "หญิง"; }
                        else
                        { Strsex = "ไม่ระบุ"; }

                        labelControl_name.Text = ds.Tables[0].Rows[0]["NAME"].ToString() + " " + ds.Tables["Query_accessnumber"].Rows[0]["LASTNAME"].ToString();
                        labelControl_sex.Text = Strsex;
                        lblHostnum.Text = ds.Tables[0].Rows[0]["HOSTORDERNUMBER"].ToString();

                        DateTime DT = Convert.ToDateTime(ds.Tables[0].Rows[0]["REQDATE"].ToString());
                        string StrReqDate = DT.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));

                        if (ds.Tables[0].Rows[0]["BIRTHDATE"].ToString() != "")
                        {
                            //Str_DOB = ((DateTime)(ds.Tables["Query_accessnumber"].Rows[0]["BIRTHDATE"])).ToString("dd-MM-yyyy", cultureinfo_ENG);
                            Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(ds.Tables["Query_accessnumber"].Rows[0]["BIRTHDATE"].ToString()));

                            // Start Date of Birth in calcu
                            if (Str_DOB == null || Str_DOB == "")
                            {
                                Str_DateOfBirth = "-";
                            }
                            else
                            {
                                string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                                int Str_Year = Convert.ToInt32(Array_DOB[2]);
                                int Str_Month = Convert.ToInt32(Array_DOB[1]);
                                int Str_Day = Convert.ToInt32(Array_DOB[0]);

                                DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                                DateTime ToDate = DateTime.Now;
                                DateDifference dDiff = new DateDifference(myDate, ToDate);

                                Str_DateOfBirth = dDiff.ToString();
                            }
                            // END Date of Birth in calcu

                            labelControl_Birthdate.Text = Str_DOB;

                            labelControl_Age.Text = Str_DateOfBirth;
                        }

                        treeList1.EndUnboundLoad();
                        treeList1.ExpandAll();
                    }

                }
                else
                {
                    //DialogResult yes = MessageBox.Show("Not Found Accessnumber ?", "", MessageBoxButtons.OK);
                    MessageBox.Show("Not Found Accessnumber ?", "", MessageBoxButtons.OK);
                    //if (yes == DialogResult.Yes)
                    //{
                    //    //OEM.Form_PATIENT_INSERT FM_insert = new OEM.Form_PATIENT_INSERT();
                    //    //FM_insert.ShowDialog();
                    //}

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "Error :L892  Query_accessnumber \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Process_FindPatientsWithTest()
        {
            //textEdit_search.Text
            string Strsex = "";
            string Str_Req_status = "";
            string bdate = "";

            DataTable dtView;
            DataRow dr;
            PatientController objPatient = new PatientController();

            try
            {

                dtReq = objPatient.GetAccessNumberData("", textEdit_search.Text , "");

                //CreateColumns(treeList1, dt);
                //treeList1.BeginUnboundLoad();
                //treeList1.ClearNodes();

                //TreeListNode parentForRootNodes = null;
                dtView = new DataTable();

                dtView.Columns.Add("ACCESSNUMBER");
                dtView.Columns.Add("SPECIMENCODE");
                dtView.Columns.Add("REQDATE");
                dtView.Columns.Add("MBREQNUMBER");
                dtView.Columns.Add("PROTOCOL");
                dtView.Columns.Add("COLLECTIONDATE");
                dtView.Columns.Add("RECEIVEDDATE");
                dtView.Columns.Add("REQSTATUS");
                dtView.Columns.Add("LOGUSERID");
                dtView.Columns.Add("COMMENT");

                if (dtReq.Rows.Count > 0)
                {
                    for (int i = 0; i < dtReq.Rows.Count; i++)
                    {
                        dr = dtView.NewRow();
                        dr["ACCESSNUMBER"] = dtReq.Rows[i]["ACCESSNUMBER"].ToString();
                        dr["SPECIMENCODE"] = dtReq.Rows[i]["SPECIMENCODE"].ToString();
                        dr["MBREQNUMBER"] = dtReq.Rows[i]["MBREQNUMBER"].ToString();
                        dr["PROTOCOL"] = dtReq.Rows[i]["PROTOCOLTEXT"].ToString();
                        dr["REQDATE"] = dtReq.Rows[i]["REQDATE"].ToString();
                        dr["COLLECTIONDATE"] = dtReq.Rows[i]["COLLECTIONDATE"].ToString();
                        dr["RECEIVEDDATE"] = dtReq.Rows[i]["RECEIVEDDATE"].ToString();
                        dr["REQSTATUS"] = dtReq.Rows[i]["REQSTATUS"].ToString();
                        dr["LOGUSERID"] = dtReq.Rows[i]["LOGUSERID"].ToString();
                        dr["COMMENT"] = dtReq.Rows[i]["COMMENT"].ToString();

                        dtView.Rows.Add(dr);

                        dr = null;

                    }

                    grdView.DataSource = dtView;


                    if (dtReq.Rows[0]["PATNUMBER"].ToString() != "")
                    {
                        txtPatnum.Text = dtReq.Rows[0]["PATNUMBER"].ToString().TrimStart('0');
                        objPatientM = objPatient.GetPatientSearchObject(dtReq.Rows[0]["PATNUMBER"].ToString());
                    }
                    if (dtReq.Rows[0]["SEX"].ToString() == "1")
                    { Strsex = "ชาย"; }
                    else if (dtReq.Rows[0]["SEX"].ToString() == "2")
                    { Strsex = "หญิง"; }
                    else
                    { Strsex = "ไม่ระบุ"; }

                    if (dtReq.Rows[0]["REQSTATUS"].ToString() == "1")
                    { Str_Req_status = "Open"; }
                    else if (dtReq.Rows[0]["REQSTATUS"].ToString() == "2")
                    { Str_Req_status = "Close"; }

                    labelControl_name.Text = dtReq.Rows[0]["NAME"].ToString() + " " + dtReq.Rows[0]["LASTNAME"].ToString();
                    labelControl_sex.Text = Strsex;
                    lblHostnum.Text = dtReq.Rows[0]["HOSTORDERNUMBER"].ToString();

                    DateTime DT = Convert.ToDateTime(dtReq.Rows[0]["REQDATE"].ToString());
                    string StrReqDate = DT.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));

                    if (dtReq.Rows[0]["BIRTHDATE"].ToString() != "")
                    {
                        DateTime bDt = Convert.ToDateTime(dtReq.Rows[0]["BIRTHDATE"].ToString());
                        strBirthDate = bDt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));
                        bdate = strBirthDate;

                        //Str_DOB = ((DateTime)(dtReq.Rows[0]["BIRTHDATE"])).ToString("dd-MM-yyyy", cultureinfo_ENG);
                        Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(dtReq.Rows[0]["BIRTHDATE"].ToString()));

                        // Start Date of Birth in calcu
                        if (Str_DOB == null || Str_DOB == "")
                        {
                            Str_DateOfBirth = "-";
                        }
                        else
                        {
                            string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                            int Str_Year = Convert.ToInt32(Array_DOB[2]);
                            int Str_Month = Convert.ToInt32(Array_DOB[1]);
                            int Str_Day = Convert.ToInt32(Array_DOB[0]);

                            DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                            DateTime ToDate = DateTime.Now;
                            DateDifference dDiff = new DateDifference(myDate, ToDate);

                            Str_DateOfBirth = dDiff.ToString();
                        }
                        // END Date of Birth in calcu

                        labelControl_Birthdate.Text = Str_DOB;

                        labelControl_Age.Text = Str_DateOfBirth;

                        objPatientM.LongAge = labelControl_Age.Text;
                        objPatientM.BirthDate = dtReq.Rows[0]["BIRTHDATE"].ToString();

                        if (dtReq.Rows[0]["REQ_IPDOROPD"].ToString() == "0")
                        {
                            objPatientM.IPDOPDDesc = "IPD";
                            objPatientM.IPDOPDStatus = "0";
                        }
                        else if (dtReq.Rows[0]["REQ_IPDOROPD"].ToString() == "1")
                        {
                            objPatientM.IPDOPDDesc = "OPD";
                            objPatientM.IPDOPDStatus = "1";
                        }
                        else
                        {
                            objPatientM.IPDOPDDesc = "-";
                            objPatientM.IPDOPDStatus = "";
                        }

                    }
                }
                else
                {
                    // Search by like in Form_patientLists
                    //
                    DataTable dt = null;

                    PatientM objPatientM = new PatientM();

                    try
                    {
                        if (textEdit_search.Text != "")
                        {
                            objPatientM.PatientNo = "%" + textEdit_search.Text + "%";
                            dt = objPatient.GetPatientNumber_(objPatientM);

                            if (dt.Rows.Count != 0)
                            {
                                Form_PatientLists FM = new Form_PatientLists(objPatientM.PatientNo);
                                FM.ShowDialog();

                                textEdit_search.Text = ControlParameter.PatientM.PatientNo.TrimStart('0');
                                string Str_patnumber = ControlParameter.PatientM.PatientNo;

                                Process_FindPatientsWithTest();

                                //Process_FindAccessnumber_WithTestResult();
                                //Process_FindPatients_Form_PatientLists(Str_patnumber);

                            }
                            else
                            {

                                DialogResult yes = MessageBox.Show("Not Found patient do you want Insert Patient ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (yes == DialogResult.Yes)
                                {
                                    frmPatientRegister FM = new frmPatientRegister("New");
                                    FM.ShowDialog();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :Req:1003 Query_accessnumber \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Process_FindAccessnumber_WithTestResult()
        {
            string Strsex = "";
            string Str_Req_status = "";
            string bdate = "";


            DataTable dtView;
            DataRow dr;
            PatientController objPatient = new PatientController();

            try
            {
                
                dtReq = objPatient.GetAccessNumberData(textEdit_search.Text,"", "");

                //CreateColumns(treeList1, dt);
                //treeList1.BeginUnboundLoad();
                //treeList1.ClearNodes();

                //TreeListNode parentForRootNodes = null;
                dtView = new DataTable();

                dtView.Columns.Add("ACCESSNUMBER");
                dtView.Columns.Add("SPECIMENCODE");
                dtView.Columns.Add("REQDATE");
                dtView.Columns.Add("MBREQNUMBER");
                dtView.Columns.Add("PROTOCOL");
                dtView.Columns.Add("COLLECTIONDATE");
                dtView.Columns.Add("RECEIVEDDATE");
                dtView.Columns.Add("REQSTATUS");
                dtView.Columns.Add("LOGUSERID");
                dtView.Columns.Add("COMMENT");

                if (dtReq.Rows.Count > 0)
                {
                    for (int i = 0; i < dtReq.Rows.Count; i++)
                    {
                        dr = dtView.NewRow();
                        dr["ACCESSNUMBER"] = dtReq.Rows[i]["ACCESSNUMBER"].ToString();
                        dr["SPECIMENCODE"] = dtReq.Rows[i]["SPECIMENCODE"].ToString();
                        dr["MBREQNUMBER"] = dtReq.Rows[i]["MBREQNUMBER"].ToString();
                        dr["PROTOCOL"] = dtReq.Rows[i]["PROTOCOLTEXT"].ToString();
                        dr["REQDATE"] = dtReq.Rows[i]["REQDATE"].ToString();
                        dr["COLLECTIONDATE"] = dtReq.Rows[i]["COLLECTIONDATE"].ToString();
                        dr["RECEIVEDDATE"] = dtReq.Rows[i]["RECEIVEDDATE"].ToString();
                        dr["REQSTATUS"] = dtReq.Rows[i]["REQSTATUS"].ToString();
                        dr["LOGUSERID"] = dtReq.Rows[i]["LOGUSERID"].ToString();
                        dr["COMMENT"] = dtReq.Rows[i]["COMMENT"].ToString();

                        dtView.Rows.Add(dr);

                        dr = null;

                    }

                    grdView.DataSource = dtView;


                    if (dtReq.Rows[0]["PATNUMBER"].ToString() != "")
                    {
                        txtPatnum.Text = dtReq.Rows[0]["PATNUMBER"].ToString().TrimStart('0');
                        objPatientM = objPatient.GetPatientSearchObject(dtReq.Rows[0]["PATNUMBER"].ToString());
                    }
                    if (dtReq.Rows[0]["SEX"].ToString() == "1")
                    { Strsex = "ชาย"; }
                    else if (dtReq.Rows[0]["SEX"].ToString() == "2")
                    { Strsex = "หญิง"; }
                    else
                    { Strsex = "ไม่ระบุ"; }

                    if (dtReq.Rows[0]["REQSTATUS"].ToString() == "1")
                    { Str_Req_status = "Open"; }
                    else if (dtReq.Rows[0]["REQSTATUS"].ToString() == "2")
                    { Str_Req_status = "Close"; }

                    labelControl_name.Text = dtReq.Rows[0]["NAME"].ToString() + " " + dtReq.Rows[0]["LASTNAME"].ToString();
                    labelControl_sex.Text = Strsex;
                    lblHostnum.Text = dtReq.Rows[0]["HOSTORDERNUMBER"].ToString();

                    DateTime DT = Convert.ToDateTime(dtReq.Rows[0]["REQDATE"].ToString());
                    string StrReqDate = DT.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));

                    if (dtReq.Rows[0]["BIRTHDATE"].ToString() != "")
                    {
                        DateTime bDt = Convert.ToDateTime(dtReq.Rows[0]["BIRTHDATE"].ToString());
                        strBirthDate = bDt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));
                        bdate = strBirthDate;

                        //Str_DOB = ((DateTime)(dtReq.Rows[0]["BIRTHDATE"])).ToString("dd-MM-yyyy", cultureinfo_ENG);
                        Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(dtReq.Rows[0]["BIRTHDATE"].ToString()));

                        // Start Date of Birth in calcu
                        if (Str_DOB == null || Str_DOB == "")
                        {
                            Str_DateOfBirth = "-";
                        }
                        else
                        {
                            string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                            int Str_Year = Convert.ToInt32(Array_DOB[2]);
                            int Str_Month = Convert.ToInt32(Array_DOB[1]);
                            int Str_Day = Convert.ToInt32(Array_DOB[0]);

                            DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                            DateTime ToDate = DateTime.Now;
                            DateDifference dDiff = new DateDifference(myDate, ToDate);

                            Str_DateOfBirth = dDiff.ToString();
                        }
                        // END Date of Birth in calcu


                        labelControl_Birthdate.Text = Str_DOB;

                        labelControl_Age.Text = Str_DateOfBirth;

                        objPatientM.LongAge = labelControl_Age.Text;
                        objPatientM.BirthDate  = dtReq.Rows[0]["BIRTHDATE"].ToString();  

                        if (dtReq.Rows[0]["REQ_IPDOROPD"].ToString() == "0")
                        {
                            objPatientM.IPDOPDDesc = "IPD";
                            objPatientM.IPDOPDStatus = "0";
                        }
                        else if (dtReq.Rows[0]["REQ_IPDOROPD"].ToString() == "1")
                        {
                            objPatientM.IPDOPDDesc = "OPD";
                            objPatientM.IPDOPDStatus = "1";
                        }
                        else
                        {
                            objPatientM.IPDOPDDesc = "-";
                            objPatientM.IPDOPDStatus = "";
                        }

                    }
                }
                else
                {
                    DialogResult yes = MessageBox.Show("Not Found patient do you want Insert Patient ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (yes == DialogResult.Yes)
                    {
                        frmPatientRegister FM = new frmPatientRegister("New");
                        FM.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :Req:1003 Query_accessnumber \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Process_FindAccessnumber_with_patient()
        {
            string Str_Req_status = "";

            try
            {
                if (txtPatnum.Text !="")
                { 
                string sql_accessnumber_search = "";
                sql_accessnumber_search = @"SELECT REQUESTS.REQUESTID,REQUESTS.PATID,REQUESTS.ACCESSNUMBER
                        ,PATIENTS.PATNUMBER
                        ,PATIENTS.NAME,PATIENTS.LASTNAME
                        ,PATIENTS.TITLE1,PATIENTS.TITLE2
                        ,PATIENTS.BIRTHDATE
                        ,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as AGEY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as AGEM
                        ,PATIENTS.SEX
                        ,PATIENTS.ALTNUMBER
                        ,REQUESTS.HOSTORDERNUMBER
                        ,REQUESTS.REQDOCTOR
                        ,REQUESTS.REQLOCATION
                        ,REQUESTS.REQ_IPDOROPD
                        ,REQUESTS.URGENT
                        ,REQUESTS.SECRETRESULT
                        ,DICT_DOCTORS.DOCNAME
                        ,DICT_LOCATIONS.LOCNAME
                        ,REQUESTS.REQCREATIONDATE,REQUESTS.REQSTATUS,REQUESTS.STATUSDATE
                        ,REQUESTS.REQDATE,REQUESTS.COLLECTIONDATE,REQUESTS.COMMENT,REQUESTS.LASTUPDATE
                        ,REQUESTS.RECEIVEDDATE,REQUESTS.LOGUSERID,REQUESTS.LOGDATE
                        FROM REQUESTS
                        LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
                        LEFT OUTER JOIN DICT_DOCTORS ON REQUESTS.REQDOCTOR = DICT_DOCTORS.DOCCODE
                        LEFT OUTER JOIN DICT_LOCATIONS ON REQUESTS.REQLOCATION = DICT_LOCATIONS.LOCCODE
                        WHERE PATIENTS.PATNUMBER='" + txtPatnum.Text + "' ";

                Writedatalog.WriteLog(strDateTime + " Query Accessnumber " + "[" + "" + "] Query Accessnumber================ " + sql_accessnumber_search);

                SqlCommand cmd = new SqlCommand(sql_accessnumber_search, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adap.Fill(ds, "Query_accessnumber");
                    if (ds.Tables["Query_accessnumber"].Rows.Count > 0)
                    {
                        CreateColumns_patient_Accessnumber_search(treeList1, ds);
                        treeList1.BeginUnboundLoad();
                        treeList1.ClearNodes();
                        TreeListNode parentForRootNodes = null;

                        //  CreateColumns_patient_Accessnumber_search && CreateColumns
                        // 0 = PATID 
                        // 1 = Accessnumber 
                        // 2 = Request Date 
                        // 3 = Collection Date 
                        // 4 = Receive Date 
                        // 5 = Status 
                        // 6 = User 
                        // 7 = REQUESTID 
                        // 8 = PATNUMBER
                        // 9 = NAME 
                        // 10 = LASTNAME 
                        // 11 = AGEY 
                        // 12 = AGEM 
                        // 13 = SEX 
                        // 14 = ALTNUMBER 
                        // 15 = HOSTORDERNUMBER 
                        // 16 = REQDOCTOR 
                        // 17 = REQLOCATION 
                        // 18 = COMMENT
                        // 19 = TITLE1 
                        // 20 = TITLE2
                        // 21 = BIRTHDATE
                        // 22 = IPDOPD
                        // 23 = URGENT
                        // 24 = SECRETRESULT

                        for (int i = 0; i < ds.Tables["Query_accessnumber"].Rows.Count; i++)
                        {
                            if (ds.Tables["Query_accessnumber"].Rows[i]["REQSTATUS"].ToString() == "10")
                            { Str_Req_status = "Open"; }
                            else if (ds.Tables["Query_accessnumber"].Rows[i]["REQSTATUS"].ToString() == "11")
                            { Str_Req_status = "Close"; }

                            string StrCOMMENTS = ds.Tables["Query_accessnumber"].Rows[i]["COMMENT"].ToString();
                            memoEdit_Comments.Text = StrCOMMENTS;


                        childNode = treeList1.AppendNode(new object[] {
                        ds.Tables["Query_accessnumber"].Rows[i]["PATID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["ACCESSNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQDATE"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["COLLECTIONDATE"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["RECEIVEDDATE"].ToString(),
                        Str_Req_status,
                        ds.Tables["Query_accessnumber"].Rows[i]["LOGUSERID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQUESTID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["PATNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["NAME"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["LASTNAME"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["AGEY"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["AGEM"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["SEX"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["ALTNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["HOSTORDERNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["DOCNAME"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["LOCNAME"].ToString(),
                        StrCOMMENTS,
                        ds.Tables["Query_accessnumber"].Rows[i]["TITLE1"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["TITLE2"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["BIRTHDATE"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQ_IPDOROPD"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["URGENT"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["SECRETRESULT"].ToString()

                        }  , parentForRootNodes);


                            if (ds.Tables[0].Rows[0]["BIRTHDATE"].ToString() != "")
                            {
                                //Str_DOB = ((DateTime)(ds.Tables[0].Rows[0]["BIRTHDATE"])).ToString("dd-MM-yyyy", cultureinfo_ENG);
                                Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(ds.Tables[0].Rows[0]["BIRTHDATE"].ToString()));

                                // Start Date of Birth in calcu
                                if (Str_DOB == null || Str_DOB == "")
                                {
                                    Str_DateOfBirth = "-";
                                }
                                else
                                {
                                    string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                                    int Str_Year = Convert.ToInt32(Array_DOB[2]);
                                    int Str_Month = Convert.ToInt32(Array_DOB[1]);
                                    int Str_Day = Convert.ToInt32(Array_DOB[0]);

                                    DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                                    DateTime ToDate = DateTime.Now;
                                    DateDifference dDiff = new DateDifference(myDate, ToDate);

                                    Str_DateOfBirth = dDiff.ToString();
                                }
                                // END Date of Birth in calcu

                                labelControl_Birthdate.Text = Str_DOB;

                                labelControl_Age.Text = Str_DateOfBirth;
                            }

                            // *************
                            // ************* Set text from 1 to OPEN and 2 to CLOSE
                            //
                            //if (ds.Tables["Query_accessnumber"].Rows[0]["REQSTATUS"].ToString() == "1")
                            //{ Str_Req_status = "Open"; }
                            //else if (ds.Tables["Query_accessnumber"].Rows[0]["REQSTATUS"].ToString() == "2")
                            //{ Str_Req_status = "Close"; }
                            //DateTime DT = Convert.ToDateTime(ds.Tables[0].Rows[0]["REQDATE"].ToString());
                            //string StrReqDate = DT.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));
                            treeList1.EndUnboundLoad();
                            treeList1.ExpandAll();
                        }
                            //string cellaccessnumber = treeList1.FocusedNode.GetDisplayText(1);
                            //string cellRequestDate = treeList1.FocusedNode.GetDisplayText(2);
                            //string cellCollectDate = treeList1.FocusedNode.GetDisplayText(3);
                            //string cellDoctor = treeList1.FocusedNode.GetDisplayText(16);
                            //string cellLocation = treeList1.FocusedNode.GetDisplayText(17);
                            //string cellCommentReq = treeList1.FocusedNode.GetDisplayText(18);

                            //lblAccessnumber.Text = cellaccessnumber;
                            //lblCollected.Text = cellCollectDate;
                            //lblRequested.Text = cellRequestDate;
                            //lblDoctor.Text = cellDoctor;
                            //lblLocation.Text = cellLocation;
                            //label_IPDOPD.Text = "IPDOPD";
                            //lblConfidential.Text = "confidental";
                            //OrderCOMMENT.Text = cellCommentReq;
                    }
                    else
                    {
                        treeList1.ClearNodes();

                        DialogResult yes = MessageBox.Show("Not Found Accessnumber you want create request Order ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (yes == DialogResult.Yes)
                        {
                            Form_REQUEST_NEWORDER FM_NEWORDER = new Form_REQUEST_NEWORDER(txtPatnum.Text, labelControl_tltle.Text, labelControl_name.Text, labelControl_Birthdate.Text, labelControl_sex.Text, labelControl_Age.Text, labelControl_diag.Text, "Creation");
                            FM_NEWORDER.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Error : L992 Query Process_FindAccessnumber_with_patient \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Chk_Length()
        {
            try
            {
                string sql_LENGTH = @"SELECT HN_LENGTH,ACCESS_LENGTH FROM DICT_SYSTEM_MB_CONFIG";
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
                    HN_LENGTH = Convert.ToInt32(ds.Tables["sql_LENGTH"].Rows[0]["HN_LENGTH"].ToString());
                    LN_LENGTH = Convert.ToInt32(ds.Tables["sql_LENGTH"].Rows[0]["ACCESS_LENGTH"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Error :L1018 Chk_Length \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clearinformationall()
        {
            // Date time Filter
            CultureInfo us = new CultureInfo("en-US");
            dateTimePicker_start.Value.Date.ToString("yyyy-MM-dd", us);
            timeEdit_start.Time.ToString("HH:mm:ss");
            dateTimePicker_finish.Value.Date.ToString("yyyy-MM-dd", us);
            timeEdit_finish.Time.ToString("HH:mm:ss");

            dateTimePicker_start.Value = DateTime.Now;
            dateTimePicker_finish.Value = DateTime.Now;
            timeEdit_start.Time = DateTime.Now;
            timeEdit_finish.Time = DateTime.Now;
            // Date time Filter End clean

            //checkButton_Search.Checked = false;
            textEdit_search.Text = "";
            textEdit_search.Select();

            //textEdit_search.s
            treeList1.ClearNodes();
            treeList1.Columns.Clear();
            txtPatnum.Text = "";
            labelControl_name.Text = "";
            labelControl_Birthdate.Text = "";
            labelControl_diag.Text = "";
            labelControl_sex.Text = "";
            labelControl_altnum.Text = "";
            labelControl_Age.Text = "";
            lblHostnum.Text = "";
            memoEdit_Comments.Text = "";
            labelControl_tltle.Text = "";
            textEdit_Search_text.Text = "";
            //
            //groupControl1.Location = new Point(0, 57);
            //groupControl1.Size = new Size(924, 84);
            //groupControl2.Location = new Point(0, 147);
            //groupControl2.Size = new Size(924, 201);
            //groupControl3.Location = new Point(0, 354);
            //groupControl3.Size = new Size(924, 367);
            //treeList1.Location = new Point(5, 25);
            //treeList1.Size = new Size(914, 339);
            //
            textEdit_search.Enabled = true;
            dropDownButton_SearchPAT.Enabled = true;
            dateTimePicker_start.Enabled = false;
            timeEdit_start.Enabled = false;
            dateTimePicker_finish.Enabled = false;
            timeEdit_finish.Enabled = false;
            dropDownButton_Start_search.Enabled = false;
            radioGroup1.Enabled = false;
            comboBox_Search_DOCTOR.Enabled = false;
            comboBox_Search_LOCATION.Enabled = false;
        }

        private void Clearinformation_combo1()
        {
            // Date time Filter
            CultureInfo us = new CultureInfo("en-US");
            dateTimePicker_start.Value.Date.ToString("yyyy-MM-dd", us);
            timeEdit_start.Time.ToString("HH:mm:ss");
            dateTimePicker_finish.Value.Date.ToString("yyyy-MM-dd", us);
            timeEdit_finish.Time.ToString("HH:mm:ss");

            dateTimePicker_start.Value = DateTime.Now;
            dateTimePicker_finish.Value = DateTime.Now;
            timeEdit_start.Time = DateTime.Now;
            timeEdit_finish.Time = DateTime.Now;
            // Date time Filter End clean

            checkButton_Search.Checked = false;
            textEdit_search.Text = "";
            textEdit_search.Select();

            //textEdit_search.s
            treeList1.ClearNodes();
            treeList1.Columns.Clear();
            txtPatnum.Text = "";
            labelControl_name.Text = "";
            labelControl_Birthdate.Text = "";
            labelControl_diag.Text = "";
            labelControl_sex.Text = "";
            labelControl_altnum.Text = "";
            labelControl_Age.Text = "";
            lblHostnum.Text = "";
            memoEdit_Comments.Text = "";
            labelControl_tltle.Text = "";
            //
            //groupControl1.Location = new Point(0, 57);
            //groupControl1.Size = new Size(924, 84);
            //groupControl2.Location = new Point(0, 147);
            //groupControl2.Size = new Size(924, 201);
            //groupControl3.Location = new Point(0, 354);
            //groupControl3.Size = new Size(924, 367);
            //treeList1.Location = new Point(5, 25);
            //treeList1.Size = new Size(914, 339);
            //
            //textEdit_search.Enabled = true;
            //dropDownButton_SearchPAT.Enabled = true;
            //dateTimePicker_start.Enabled = false;
            //timeEdit_start.Enabled = false;
            //dateTimePicker_finish.Enabled = false;
            //timeEdit_finish.Enabled = false;
            //dropDownButton_Start_search.Enabled = false;
            //radioGroup1.Enabled = false;
            //comboBox_Search_DOCTOR.Enabled = false;
            //comboBox_Search_LOCATION.Enabled = false;

        }


        private void dropDownButton_search_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Get_check_patient_find = true;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                Get_check_patient_find = false;
            }
            textEdit_Search_text.Text = "";
            Process_Search_start();
        }
        private void Combo_Search_critiria_Location()
        {
            try
            {
                string sql_combo = @"SELECT LOCCODE,LOCNAME FROM DICT_LOCATIONS ";
                SqlCommand cmd = new SqlCommand(sql_combo, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_result");
                cmd.ExecuteReader();
                if (ds.Tables["sql_result"].Rows.Count > 0)
                {
                    comboBox_Search_LOCATION.DisplayMember = "LOCCODE";
                    comboBox_Search_LOCATION.ValueMember = "LOCCODE";
                    comboBox_Search_LOCATION.DataSource = ds.Tables["sql_result"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: L1169 :Combo_Search_critiria Select data to Combo ?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Combo_Search_critiria_Doctor()
        {
            try
            {
                string sql_combo = @"SELECT DOCCODE,DOCNAME FROM DICT_DOCTORS ";
                SqlCommand cmd = new SqlCommand(sql_combo, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "sql_result");
                cmd.ExecuteReader();
                if (ds.Tables["sql_result"].Rows.Count > 0)
                {
                    comboBox_Search_DOCTOR.DisplayMember = "DOCCODE";
                    comboBox_Search_DOCTOR.ValueMember = "DOCCODE";
                    comboBox_Search_DOCTOR.DataSource = ds.Tables["sql_result"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: L1195 :Combo_Search_critiria Select data to Combo ?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkButton_Search_Click(object sender, EventArgs e)
        {
            Clearinformationall();
            Combo_Search_critiria_Location();
            Combo_Search_critiria_Doctor();
            textEdit_search.Text = "";

            if (checkButton_Search.Checked == true)
            {
                // Expand group
                //groupControl1.Location = new Point(0, 57);
                //groupControl1.Size = new Size(924, 84);
                //groupControl2.Location = new Point(0, 147);
                //groupControl2.Size = new Size(924, 201);
                //groupControl3.Location = new Point(0, 354);
                //groupControl3.Size = new Size(924, 367);
                //treeList1.Location = new Point(5, 25);
                //treeList1.Size = new Size(914, 339);
                //
                textEdit_search.Enabled = true;
                textEdit_Search_text.Enabled = false;

                dropDownButton_SearchPAT.Enabled = true;
                dateTimePicker_start.Enabled = false;
                timeEdit_start.Enabled = false;
                dateTimePicker_finish.Enabled = false;
                timeEdit_finish.Enabled = false;
                dropDownButton_Start_search.Enabled = false;
                radioGroup1.Enabled = false;
                comboBox_Search_DOCTOR.Enabled = false;
                comboBox_Search_LOCATION.Enabled = false;
                dropDownButton2.Enabled = false;
            }
            else
            {
                //groupControl1.Location = new Point(0, 57);
                //groupControl1.Size = new Size(924, 182);
                //groupControl2.Location = new Point(0, 245);
                //groupControl2.Size = new Size(924, 200);
                //groupControl3.Location = new Point(0, 451);
                //groupControl3.Size = new Size(924, 270);
                //treeList1.Location = new Point(5, 24);
                //treeList1.Size = new Size(914, 243);

                dropDownButton_SearchPAT.Enabled = false;
                textEdit_search.Enabled = false;
                if (comboBox1.SelectedIndex == 0)
                {
                    textEdit_Search_text.Enabled = true;
                }
                else
                {
                    textEdit_Search_text.Enabled = false;
                }

                dateTimePicker_start.Enabled = true;
                timeEdit_start.Enabled = true;
                dateTimePicker_finish.Enabled = true;
                timeEdit_finish.Enabled = true;
                dropDownButton_Start_search.Enabled = true;
                radioGroup1.Enabled = true;
                //comboBox_Search_DOCTOR.Enabled = true;
                //comboBox_Search_LOCATION.Enabled = true;
                dropDownButton2.Enabled = true;
            }
        }

        private void Process_Search_start()
        {
            int count = 0;
            string get_data = "";
            string Str_Req_status = "";
            // Get date and time
            CultureInfo us = new CultureInfo("en-US");
            string get_date_start = dateTimePicker_start.Value.Date.ToString("yyyy-MM-dd", us);
            string get_time_start = timeEdit_start.Time.ToString("HH:mm:ss");
            string get_date_finish = dateTimePicker_finish.Value.Date.ToString("yyyy-MM-dd", us);
            string get_time_finish = timeEdit_finish.Time.ToString("HH:mm:ss");
            // Set start - finish dateTime for search 
            string start_date_time = get_date_start + " " + get_time_start;
            string finish_date_time = get_date_finish + " " + get_time_finish;
            // Check parameter input filter
            string filter_combo_0 = "";

            if (radioGroup1.SelectedIndex == 0 && comboBox1.SelectedIndex == 0)
            {
                // Patient select number && Receive in radio
                Filter_Critiria = @"AND FORMAT(PATIENTS.PATCREATIONDATE, 'yyyy-MM-dd HH:mm:ss') >= '" + start_date_time + @"'
AND FORMAT(PATIENTS.PATCREATIONDATE, 'yyyy-MM-dd HH:mm:ss') <= '" + finish_date_time + @"'
ORDER BY PATIENTS.PATCREATIONDATE ASC";
            }
            if (radioGroup1.SelectedIndex == 1 && comboBox1.SelectedIndex == 0)
            {
                // Patient select number && Date requests in radio
                Filter_Critiria = @"AND FORMAT(PATIENTS.LASTUPDTESTDATE, 'yyyy-MM-dd HH:mm:ss') >= '" + start_date_time + @"'
AND FORMAT(PATIENTS.LASTUPDTESTDATE, 'yyyy-MM-dd HH:mm:ss') <= '" + finish_date_time + @"'
ORDER BY PATIENTS.LASTUPDTESTDATE ASC";
            }
            if (radioGroup1.SelectedIndex == 0 && comboBox1.SelectedIndex == 1)
            {
                // Patient select number && Date requests in radio
                Filter_Critiria = @"AND FORMAT(REQUESTS.RECEIVEDDATE, 'yyyy-MM-dd HH:mm:ss') >= '" + start_date_time + @"'
AND FORMAT(REQUESTS.RECEIVEDDATE, 'yyyy-MM-dd HH:mm:ss') <= '" + finish_date_time + @"'
ORDER BY REQUESTS.RECEIVEDDATE ASC";
            }
            if (radioGroup1.SelectedIndex == 1 && comboBox1.SelectedIndex == 1)
            {
                // Patient select number && Date requests in radio
                Filter_Critiria = @"AND FORMAT(REQUESTS.REQDATE, 'yyyy-MM-dd HH:mm:ss') >= '" + start_date_time + @"'
AND FORMAT(REQUESTS.REQDATE, 'yyyy-MM-dd HH:mm:ss') <= '" + finish_date_time + @"'
ORDER BY REQUESTS.REQDATE ASC";
            }
            if (radioGroup1.SelectedIndex == 2 && comboBox1.SelectedIndex == 0)
            {
                // Patient select number && Doctor Refer
                Filter_Critiria = @"AND PATIENTS.REFDOCTOR = '" + comboBox_Search_DOCTOR.Text + "' ORDER BY PATIENTS.PATCREATIONDATE ASC";
            }
            if (radioGroup1.SelectedIndex == 2 && comboBox1.SelectedIndex == 1)
            {
                // Patient select number && Doctor Refer
                Filter_Critiria = @"AND REQUESTS.REQDOCTOR = '" + comboBox_Search_DOCTOR.Text + "' ORDER BY REQDATE ASC";
            }
            if (radioGroup1.SelectedIndex == 3 && comboBox1.SelectedIndex == 1)
            {
                // Patient select number && Doctor Refer
                Filter_Critiria = @"AND REQUESTS.REQLOCATION = '" + comboBox_Search_LOCATION.Text + "' ORDER BY REQDATE ASC";
            }

            if (radioGroup1.SelectedIndex == 3 && comboBox1.SelectedIndex == 0)
            {
                // Patient select number && Location Refer
                Filter_Critiria = @"AND PATIENTS.REFLOCATION = '" + comboBox_Search_LOCATION.Text + "' ORDER BY PATIENTS.REFLOCATION ASC";
            }
            if (radioGroup1.SelectedIndex == 4 && comboBox1.SelectedIndex == 0)
            {
                // Patient select number && All
                Filter_Critiria = @"";
            }
            if (radioGroup1.SelectedIndex == 4 && comboBox1.SelectedIndex == 1)
            {
                // Patient select number && All
                Filter_Critiria = @"";
            }
            // if select patient
            if (comboBox1.SelectedIndex == 0)
            {
                count = 0;
                get_data = "Select Patient Filter";
                filter_combo_0 = @"SELECT PATIENTS.PATID
,PATIENTS.PATNUMBER,PATIENTS.TITLE1,PATIENTS.TITLE2,PATIENTS.NAME,PATIENTS.LASTNAME,PATIENTS.ADDRESS1,PATIENTS.ADDRESS2,PATIENTS.CITY
,PATIENTS.STATE,PATIENTS.POSTALCODE,PATIENTS.COUNTRY,PATIENTS.BIRTHDATE
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate())) / 12 as AGEY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate())) % 12 as AGEM
,PATIENTS.SEX,PATIENTS.TELEPHON,PATIENTS.TELEPHON2,PATIENTS.FAX,PATIENTS.EMAIL,PATIENTS.INCOMMINGDATE,PATIENTS.ROOMNUMBER
,PATIENTS.REFDOCTOR,PATIENTS.REFLOCATION,PATIENTS.LASTUPDTESTDATE,PATIENTS.BG_ABO,PATIENTS.BG_RHESUS,PATIENTS.BG_PHENOTYPES
,PATIENTS.BG_KELL,PATIENTS.DEATHDATE,PATIENTS.SECRETRESULT,PATIENTS.VIP,PATIENTS.DOCID,PATIENTS.PATCREATIONDATE,PATIENTS.STARTVALIDDATE
,PATIENTS.ENDVALIDDATE,PATIENTS.CREATEBY,PATIENTS.LOGUSERID,PATIENTS.LOGDATE,PATIENTS.COMMENT
FROM PATIENTS WHERE PATIENTS.PATNUMBER is not null" + @"
" + Filter_Critiria +" ";

                Writedatalog.WriteLog(strDateTime + "  Log:L1396 :Send_process Check filter_combo ?         " + get_data + "        : " + "\r\n" + filter_combo_0);
                SqlCommand cmd = new SqlCommand(filter_combo_0, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adap.Fill(ds, "Query_patient");

                if (ds.Tables["Query_patient"].Rows.Count > 0)
                {
                    CreateColumns_patientSearch(treeList1, ds);
                    treeList1.BeginUnboundLoad();
                    treeList1.ClearNodes();
                    TreeListNode parentForRootNodes = null;

                    for (int i = 0; i < ds.Tables["Query_patient"].Rows.Count; i++)
                    {
                        count++;
                        childNode = treeList1.AppendNode(new object[] {
                        ds.Tables["Query_patient"].Rows[i]["PATID"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["PATNUMBER"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["NAME"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["LASTNAME"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["REFDOCTOR"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["REFLOCATION"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["COMMENT"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["BIRTHDATE"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["SEX"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["AGEY"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["AGEM"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["TITLE1"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["TITLE2"].ToString() }, parentForRootNodes);

                        treeList1.EndUnboundLoad();
                        treeList1.ExpandAll();
                    }
                    label4.Text = "Total Line " + count.ToString();

                    MessageBox.Show("Finish Query data", "", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Not Found data?", "", MessageBoxButtons.OK);
                }
            }
            // if select Accessnumber for search
            if (comboBox1.SelectedIndex == 1)
            {
                count = 0;
                get_data = "Select Accessnumber Filter";
                filter_combo_0 = @"SELECT REQUESTS.REQUESTID,REQUESTS.PATID,REQUESTS.ACCESSNUMBER,PATIENTS.PATNUMBER,PATIENTS.NAME,PATIENTS.LASTNAME,PATIENTS.BIRTHDATE
,PATIENTS.TITLE1,PATIENTS.TITLE2
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as AGEY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as AGEM
,PATIENTS.SEX,PATIENTS.ALTNUMBER,REQUESTS.HOSTORDERNUMBER,REQUESTS.REQDOCTOR,REQUESTS.REQLOCATION
,REQUESTS.REQCREATIONDATE,REQUESTS.REQSTATUS,REQUESTS.STATUSDATE
,REQUESTS.REQDATE,REQUESTS.COLLECTIONDATE,REQUESTS.COMMENT,REQUESTS.LASTUPDATE
,REQUESTS.RECEIVEDDATE,REQUESTS.LOGUSERID,REQUESTS.LOGDATE
FROM REQUESTS
LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
WHERE REQUESTS.ACCESSNUMBER is not null" + @"
" + Filter_Critiria +" ";

                Writedatalog.WriteLog(strDateTime + "  Log:L1448 :Send_process Check filter_combo ?         " + get_data + "        : " + "\r\n" + filter_combo_0);

                SqlCommand cmd = new SqlCommand(filter_combo_0, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adap.Fill(ds, "Query_accessnumber");

                if (ds.Tables["Query_accessnumber"].Rows.Count > 0)
                {
                    
                    CreateColumns(treeList1, ds);
                    treeList1.BeginUnboundLoad();
                    treeList1.ClearNodes();
                    TreeListNode parentForRootNodes = null;

                    //  CreateColumns_patient_Accessnumber_search && CreateColumns
                    // 0 = PATID 
                    // 1 = Accessnumber 
                    // 2 = Request Date 
                    // 3 = Collection Date 
                    // 4 = Receive Date 
                    // 5 = Status 
                    // 6 = User 
                    // 7 = REQUESTID 
                    // 8 = PATNUMBER
                    // 9 = NAME 
                    // 10 = LASTNAME 
                    // 11 = AGEY 
                    // 12 = AGEM 
                    // 13 = SEX 
                    // 14 = ALTNUMBER 
                    // 15 = HOSTORDERNUMBER 
                    // 16 = REQDOCTOR 
                    // 17 = REQLOCATION 
                    // 18 = COMMENT
                    // 19 = TITLE1 
                    // 20 = TITLE2
                    // 21 = BIRTHDATE

                    for (int i = 0; i < ds.Tables["Query_accessnumber"].Rows.Count; i++)
                    {
                        count++;

                        if (ds.Tables["Query_accessnumber"].Rows[i]["REQSTATUS"].ToString() == "10")
                        { Str_Req_status = "Open"; }
                        else if (ds.Tables["Query_accessnumber"].Rows[i]["REQSTATUS"].ToString() == "11")
                        { Str_Req_status = "Close"; }


                        childNode = treeList1.AppendNode(new object[] {
                        ds.Tables["Query_accessnumber"].Rows[i]["PATID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["ACCESSNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQDATE"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["COLLECTIONDATE"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["RECEIVEDDATE"].ToString(),
                        Str_Req_status,
                        ds.Tables["Query_accessnumber"].Rows[i]["LOGUSERID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQUESTID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["PATNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["NAME"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["LASTNAME"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["AGEY"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["AGEM"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["SEX"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["ALTNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["HOSTORDERNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQDOCTOR"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQLOCATION"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["COMMENT"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["TITLE1"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["TITLE2"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["BIRTHDATE"].ToString()

                        }, parentForRootNodes);
                        memoEdit_Comments.Text = ds.Tables["Query_accessnumber"].Rows[i]["COMMENT"].ToString();
                        treeList1.EndUnboundLoad();
                        treeList1.ExpandAll();
                    }
                    label4.Text = "Total Line " + count.ToString();
                    MessageBox.Show("Finish Query data", "", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Not Found data?", "", MessageBoxButtons.OK);
                }
            }
            // if select Protocol number
            if (comboBox1.SelectedIndex == 2)
            {
                count = 0;
                get_data = "Select Accessnumber Filter";
                filter_combo_0 = @"SELECT REQUESTS.REQUESTID,REQUESTS.PATID,REQUESTS.ACCESSNUMBER,PATIENTS.PATNUMBER,PATIENTS.NAME,PATIENTS.LASTNAME,PATIENTS.BIRTHDATE
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as AGEY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as AGEM
,PATIENTS.SEX,PATIENTS.ALTNUMBER,REQUESTS.HOSTORDERNUMBER,REQUESTS.REQDOCTOR,REQUESTS.REQLOCATION
,REQUESTS.REQCREATIONDATE,REQUESTS.REQSTATUS,REQUESTS.STATUSDATE
,REQUESTS.REQDATE,REQUESTS.COLLECTIONDATE,REQUESTS.COMMENT,REQUESTS.LASTUPDATE
,REQUESTS.RECEIVEDDATE,REQUESTS.LOGUSERID,REQUESTS.LOGDATE
FROM REQUESTS
LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
WHERE REQUESTS.ACCESSNUMBER is not null" + @"
" + Filter_Critiria + " ";

                Writedatalog.WriteLog(strDateTime + "  Log:Q1001 :Send_process Check filter_combo ?         " + get_data + "        : " + "\r\n" + filter_combo_0);

                SqlCommand cmd = new SqlCommand(filter_combo_0, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adap.Fill(ds, "Query_accessnumber");

                if (ds.Tables["Query_accessnumber"].Rows.Count > 0)
                {
                    CreateColumns(treeList1, ds);
                    treeList1.BeginUnboundLoad();
                    treeList1.ClearNodes();
                    TreeListNode parentForRootNodes = null;

                    for (int i = 0; i < ds.Tables["Query_accessnumber"].Rows.Count; i++)
                    {
                        if (ds.Tables["Query_accessnumber"].Rows[i]["REQSTATUS"].ToString() == "10")
                        { Str_Req_status = "Open"; }
                        else if (ds.Tables["Query_accessnumber"].Rows[i]["REQSTATUS"].ToString() == "11")
                        { Str_Req_status = "Close"; }


                        childNode = treeList1.AppendNode(new object[] {
                        ds.Tables["Query_accessnumber"].Rows[i]["PATID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["ACCESSNUMBER"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQDATE"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["COLLECTIONDATE"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["RECEIVEDDATE"].ToString(),
                        Str_Req_status,
                        ds.Tables["Query_accessnumber"].Rows[i]["LOGUSERID"].ToString(),
                        ds.Tables["Query_accessnumber"].Rows[i]["REQUESTID"].ToString() }, parentForRootNodes);

                        memoEdit_Comments.Text = ds.Tables["Query_accessnumber"].Rows[i]["COMMENT"].ToString();

                        treeList1.EndUnboundLoad();
                        treeList1.ExpandAll();
                    }
                    label4.Text = "Total Line " + count.ToString();
                    MessageBox.Show("Finish Query data", "", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Not Found data?", "", MessageBoxButtons.OK);
                }
            }
        }
        private void treeList1_DoubleClick(object sender, EventArgs e)
        {
            Treelist_with_search = true;

            Treelist_select_item();
        }

        private void Treelist_select_item()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                if (Get_check_patient_find == true)
                {
                    //  CreateColumns_patientSearch
                    // 0 = PATID 
                    // 1 = PATNUMBER 
                    // 2 = NAME 
                    // 3 = LASTNAME 
                    // 4 = REQDOCTOR 
                    // 5 = REQLOCATION 
                    // 6 = COMMENT 
                    // 7 = BIRTHDATE 
                    // 8 = SEX
                    // 9 = AGEY 
                    // 10 = AGEM 
                    // 11 = TITLE1 
                    // 12 = TITLE2 
                    string cellpatnum = treeList1.FocusedNode.GetDisplayText(1);
                    string cellname = treeList1.FocusedNode.GetDisplayText(2);
                    string celllastname = treeList1.FocusedNode.GetDisplayText(3);
                    string cellDoctor = treeList1.FocusedNode.GetDisplayText(4);
                    string cellLocation = treeList1.FocusedNode.GetDisplayText(5);
                    string cellComment = treeList1.FocusedNode.GetDisplayText(6);
                    string cellBirthdate = treeList1.FocusedNode.GetDisplayText(7);
                    string cellsex = treeList1.FocusedNode.GetDisplayText(8);
                    string cellAgey = treeList1.FocusedNode.GetDisplayText(9);
                    string cellAgem = treeList1.FocusedNode.GetDisplayText(10);
                    string cellTitle1 = treeList1.FocusedNode.GetDisplayText(11);
                    string cellTitle2 = treeList1.FocusedNode.GetDisplayText(12);

                    txtPatnum.Text = cellpatnum;
                    labelControl_name.Text = cellname + "  " + celllastname;
                    labelControl_tltle.Text = cellTitle1;

                    Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(cellBirthdate), cultureinfo_ENG);

                    // Start Date of Birth in calcu
                    if (Str_DOB == null || Str_DOB == "")
                    {
                        Str_DateOfBirth = "-";
                    }
                    else
                    {
                        string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                        int Str_Year = Convert.ToInt32(Array_DOB[2]);
                        int Str_Month = Convert.ToInt32(Array_DOB[1]);
                        int Str_Day = Convert.ToInt32(Array_DOB[0]);

                        DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                        DateTime ToDate = DateTime.Now;
                        DateDifference dDiff = new DateDifference(myDate, ToDate);

                        Str_DateOfBirth = dDiff.ToString();
                    }
                    // END Date of Birth in calcu

                    labelControl_Birthdate.Text = Str_DOB;
                    labelControl_Age.Text = Str_DateOfBirth;

                    if (cellsex == "1")
                    { labelControl_sex.Text = "ชาย"; }
                    else if (cellsex == "2")
                    { labelControl_sex.Text = "หญิง"; }
                    else
                    { labelControl_sex.Text = "ไม่ระบุ"; }

                    textEdit_search.Text = cellpatnum;
                    xtraTabControl_request_info.SelectedTabPage = Search1;

                    Process_FindPatients();
                }
                else
                {
                    // If select Patient
                    try
                    {
                        //  CreateColumns_patient_Accessnumber_search
                        // 0 = PATID 
                        // 1 = Accessnumber 
                        // 2 = Request Date 
                        // 3 = Collection Date 
                        // 4 = Receive Date 
                        // 5 = Status 
                        // 6 = User 
                        // 7 = REQUESTID 
                        // 8 = PATNUMBER
                        // 9 = NAME 
                        // 10 = LASTNAME 
                        // 11 = AGEY 
                        // 12 = AGEM 
                        // 13 = SEX 
                        // 14 = ALTNUMBER 
                        // 15 = HOSTORDERNUMBER 
                        // 16 = REQDOCTOR 
                        // 17 = REQLOCATION 
                        // 18 = COMMENT
                        // 19 = TITLE1 
                        // 20 = TITLE2
                        // 21 = BIRTHDATE

                        // AS send Accessnumber
                        string cellaccessnumber = treeList1.FocusedNode.GetDisplayText(1);
                        string cellRequestDate = treeList1.FocusedNode.GetDisplayText(2);
                        string cellCollectDate = treeList1.FocusedNode.GetDisplayText(3);
                        string cellRequestID = treeList1.FocusedNode.GetDisplayText(7);
                        string cellpatnum = treeList1.FocusedNode.GetDisplayText(8);
                        string cellname = treeList1.FocusedNode.GetDisplayText(9);
                        string celllastname = treeList1.FocusedNode.GetDisplayText(10);
                        string cellAgey = treeList1.FocusedNode.GetDisplayText(11);
                        string cellAgem = treeList1.FocusedNode.GetDisplayText(12);
                        string cellsex = treeList1.FocusedNode.GetDisplayText(13);
                        string cellDoctor = treeList1.FocusedNode.GetDisplayText(16);
                        string cellLocation = treeList1.FocusedNode.GetDisplayText(17);
                        string cellCommentReq = treeList1.FocusedNode.GetDisplayText(18);
                        string cellTitle1 = treeList1.FocusedNode.GetDisplayText(19);
                        string cellTitle2 = treeList1.FocusedNode.GetDisplayText(20);
                        string cellBirthdate = treeList1.FocusedNode.GetDisplayText(21);

                        string cellIPDOPD = treeList1.FocusedNode.GetDisplayText(22);
                        string cellURGENT = treeList1.FocusedNode.GetDisplayText(23);
                        string cellSecret = treeList1.FocusedNode.GetDisplayText(24);

                        lblAccessnumber.Text = cellaccessnumber;
                        lblCollected.Text = cellCollectDate;
                        lblRequested.Text = cellRequestDate;
                        lblDoctor.Text = cellDoctor;
                        lblLocation.Text = cellLocation;
                        OrderCOMMENT.Text = cellCommentReq;
                        //txtPatnum.Text = cellpatnum;
                        //labelControl_name.Text = cellname + "  " + celllastname;
                        //labelControl_tltle.Text = cellTitle1;

                        Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(cellBirthdate), cultureinfo_ENG);

                        // Start Date of Birth in calcu
                        if (Str_DOB == null || Str_DOB == "")
                        {
                            Str_DateOfBirth = "-";
                        }
                        else
                        {
                            string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                            int Str_Year = Convert.ToInt32(Array_DOB[2]);
                            int Str_Month = Convert.ToInt32(Array_DOB[1]);
                            int Str_Day = Convert.ToInt32(Array_DOB[0]);

                            DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                            DateTime ToDate = DateTime.Now;
                            DateDifference dDiff = new DateDifference(myDate, ToDate);

                            Str_DateOfBirth = dDiff.ToString();
                        }
                        // END Date of Birth in calcu

                        labelControl_Birthdate.Text = Str_DOB;
                        labelControl_Age.Text = Str_DateOfBirth;

                        if (cellsex == "1")
                        { labelControl_sex.Text = "ชาย"; }
                        else if (cellsex == "2")
                        { labelControl_sex.Text = "หญิง"; }
                        else
                        { labelControl_sex.Text = "ไม่ระบุ"; }
                        textEdit_search.Text = cellpatnum;

                        if (cellIPDOPD == "1")
                        {
                            label_IPDOPD.Text = "OPD";
                        }
                        else if (cellIPDOPD == "0")
                        {
                            label_IPDOPD.Text = "IPD";
                        }
                        //
                        if (cellURGENT == "1")
                        {
                            pictureBox1.Visible = true;
                            label_Rectube_Urgent.Text = "S";
                        }
                        else if (cellURGENT == "0")
                        {
                            pictureBox1.Visible = false;
                            label_Rectube_Urgent.Text = "R";
                        }

                        if (cellSecret == "1")
                        {
                            pictureBox2.Visible = true;
                            lblConfidential.Text = "Yes";
                        }
                        else if (cellSecret == "0")
                        {
                            pictureBox2.Visible = false;
                            lblConfidential.Text = "No";
                        }
                        else
                        {
                            pictureBox2.Visible = false;
                            lblConfidential.Text = "-";
                        }


                        if (treeList1.AllNodesCount > 0)
                        {
                            Form_REQUESTS_ITEM_ fm = new Form_REQUESTS_ITEM_(cellaccessnumber, cellRequestID);
                            fm.ShowDialog();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(strDateTime + "  Log: L1679 : Treelist_select_item?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            // if Select Accessnumber
            else if (comboBox1.SelectedIndex == 1)
            {

                Writedatalog.WriteLog(strDateTime + "Test system Log-------------------------->>>>>>>>>>>>>>>>>>>>> " + Get_check_patient_find );

                if (Get_check_patient_find == false)
                {
                    /*
                     *             //  CreateColumns_patient_Accessnumber_search && CreateColumns
                // 0 = PATID 
                // 1 = Accessnumber 
                // 2 = Request Date 
                // 3 = Collection Date 
                // 4 = Receive Date 
                // 5 = Status 
                // 6 = User 
                // 7 = REQUESTID 
                // 8 = PATNUMBER
                // 9 = NAME 
                // 10 = LASTNAME 
                // 11 = AGEY 
                // 12 = AGEM 
                // 13 = SEX 
                // 14 = ALTNUMBER 
                // 15 = HOSTORDERNUMBER 
                // 16 = REQDOCTOR 
                // 17 = REQLOCATION 
                // 18 = COMMENT
                // 19 = TITLE1 
                // 20 = TITLE1
                // 21 = BIRTHDATE

                     * */
                    try
                    {
                        string cellaccessnumber = treeList1.FocusedNode.GetDisplayText(1);
                        string cellRequestID = treeList1.FocusedNode.GetDisplayText(7);
                        string cellpatnum = treeList1.FocusedNode.GetDisplayText(8);
                        string cellname = treeList1.FocusedNode.GetDisplayText(9);
                        string celllastname = treeList1.FocusedNode.GetDisplayText(10);
                        string cellAgey = treeList1.FocusedNode.GetDisplayText(11);
                        string cellAgem = treeList1.FocusedNode.GetDisplayText(12);
                        string cellsex = treeList1.FocusedNode.GetDisplayText(13);
                        string cellTitle1 = treeList1.FocusedNode.GetDisplayText(19);
                        string cellTitle2 = treeList1.FocusedNode.GetDisplayText(20);
                        string cellBirthDate = treeList1.FocusedNode.GetDisplayText(21);

                        txtPatnum.Text = cellpatnum;
                        labelControl_name.Text = cellname + "  " + celllastname;
                        labelControl_tltle.Text = cellTitle1;

                        Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(cellBirthDate), cultureinfo_ENG);

                        // Start Date of Birth in calcu
                        if (Str_DOB == null || Str_DOB == "")
                        {
                            Str_DateOfBirth = "-";
                        }
                        else
                        {
                            string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                            int Str_Year = Convert.ToInt32(Array_DOB[2]);
                            int Str_Month = Convert.ToInt32(Array_DOB[1]);
                            int Str_Day = Convert.ToInt32(Array_DOB[0]);

                            DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                            DateTime ToDate = DateTime.Now;
                            DateDifference dDiff = new DateDifference(myDate, ToDate);

                            Str_DateOfBirth = dDiff.ToString();
                        }
                        // END Date of Birth in calcu


                        labelControl_Birthdate.Text = Str_DOB;
                        labelControl_Age.Text = Str_DateOfBirth;

                        ATR_Access = cellaccessnumber;
                        Insert_Audit_TESTS();


                        if (treeList1.AllNodesCount > 0)
                        {
                            Form_REQUESTS_ITEM_ fm = new Form_REQUESTS_ITEM_(cellaccessnumber, cellRequestID);
                            fm.ShowDialog();
                        }

                        if (cellsex == "1")
                        { labelControl_sex.Text = "ชาย"; }
                        else if (cellsex == "2")
                        { labelControl_sex.Text = "หญิง"; }
                        else
                        { labelControl_sex.Text = "ไม่ระบุ"; }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(strDateTime + "  Log: L1794 : Treelist_select_item Accessnumber ?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void treeList1_Click(object sender, EventArgs e)
        {
            try
            {

                if (xtraTabControl_request_info.SelectedTabPage == Search2)
                {
                    //  CreateColumns__search
                    // 0 = PATID 
                    // 1 = Accessnumber 
                    // 2 = Request Date 
                    // 3 = Collection Date 
                    // 4 = Receive Date 
                    // 5 = Status 
                    // 6 = User 
                    // 7 = REQUESTID 
                    // 8 = PATNUMBER
                    // 9 = NAME 
                    // 10 = LASTNAME 
                    // 11 = AGEY 
                    // 12 = AGEM 
                    // 13 = SEX 
                    // 14 = ALTNUMBER 
                    // 15 = HOSTORDERNUMBER 
                    // 16 = REQDOCTOR 
                    // 17 = REQLOCATION 
                    // 18 = COMMENT
                    // 19 = TITLE1 
                    // 20 = TITLE2
                    // 21 = BIRTHDATE

                    // AS send Accessnumber
                    string cellpatnum = treeList1.FocusedNode.GetDisplayText(1);
                    string cellname = treeList1.FocusedNode.GetDisplayText(2);
                    string celllastname = treeList1.FocusedNode.GetDisplayText(3);
                    string cellAgey = treeList1.FocusedNode.GetDisplayText(9);
                    string cellAgem = treeList1.FocusedNode.GetDisplayText(10);
                    string cellsex = treeList1.FocusedNode.GetDisplayText(8);
                    string cellTitle1 = treeList1.FocusedNode.GetDisplayText(11);
                    string cellTitle2 = treeList1.FocusedNode.GetDisplayText(12);
                    string cellBirthdate = treeList1.FocusedNode.GetDisplayText(7);
                    txtPatnum.Text = cellpatnum;
                    labelControl_name.Text = cellname + "  " + celllastname;
                    labelControl_tltle.Text = cellTitle1;

                    Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(cellBirthdate), cultureinfo_ENG);

                    // Start Date of Birth in calcu
                    if (Str_DOB == null || Str_DOB == "")
                    {
                        Str_DateOfBirth = "-";
                    }
                    else
                    {
                        string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                        int Str_Year = Convert.ToInt32(Array_DOB[2]);
                        int Str_Month = Convert.ToInt32(Array_DOB[1]);
                        int Str_Day = Convert.ToInt32(Array_DOB[0]);

                        DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                        DateTime ToDate = DateTime.Now;
                        DateDifference dDiff = new DateDifference(myDate, ToDate);

                        Str_DateOfBirth = dDiff.ToString();
                    }
                    // END Date of Birth in calcu


                    labelControl_Birthdate.Text = Str_DOB;
                    labelControl_Age.Text = Str_DateOfBirth;

                    if (cellsex == "1")
                    { labelControl_sex.Text = "ชาย" + " /"; }
                    else if (cellsex == "2")
                    { labelControl_sex.Text = "หญิง" + " /"; }
                    else
                    { labelControl_sex.Text = "ไม่ระบุ" + " /"; }

                    textEdit_search.Text = cellpatnum;

                }
                else if (xtraTabControl_request_info.SelectedTabPage == Search1)
                {
                    // 0 = PATID 
                    // 1 = Accessnumber 
                    // 2 = Request Date 
                    // 3 = Collection Date 
                    // 4 = Receive Date 
                    // 5 = Status 
                    // 6 = User 
                    // 7 = REQUESTID 
                    // 8 = PATNUMBER
                    // 9 = NAME 
                    // 10 = LASTNAME 
                    // 11 = AGEY 
                    // 12 = AGEM 
                    // 13 = SEX 
                    // 14 = ALTNUMBER 
                    // 15 = HOSTORDERNUMBER 
                    // 16 = REQDOCTOR 
                    // 17 = REQLOCATION 
                    // 18 = COMMENT
                    // 19 = TITLE1 
                    // 20 = TITLE2
                    // 21 = BIRTHDATE
                    // 22 = IPDOPD
                    // 23 = URGENT
                    // 24 = SECRETRESULT

                    // AS send Accessnumber
                    string cellaccessnumber = treeList1.FocusedNode.GetDisplayText(1);
                    string cellRequestDate = treeList1.FocusedNode.GetDisplayText(2);
                    string cellCollectDate = treeList1.FocusedNode.GetDisplayText(3);
                    string cellRequestID = treeList1.FocusedNode.GetDisplayText(7);
                    string cellDoctor = treeList1.FocusedNode.GetDisplayText(16);
                    string cellLocation = treeList1.FocusedNode.GetDisplayText(17);
                    string cellCommentReq = treeList1.FocusedNode.GetDisplayText(18);
                    string cellIPDOPD = treeList1.FocusedNode.GetDisplayText(22);
                    string cellURGENT = treeList1.FocusedNode.GetDisplayText(23);
                    string cellSecret = treeList1.FocusedNode.GetDisplayText(24);

                    lblAccessnumber.Text = cellaccessnumber;
                    lblCollected.Text = cellCollectDate;
                    lblRequested.Text = cellRequestDate;
                    lblDoctor.Text = cellDoctor;
                    lblLocation.Text = cellLocation;

                    if (cellIPDOPD == "1")
                    {
                        label_IPDOPD.Text = "OPD" ;
                    }
                    else if (cellIPDOPD == "0")
                    {
                        label_IPDOPD.Text = "IPD";
                    }
                    //
                    if (cellURGENT == "1")
                    {
                        pictureBox1.Visible = true;
                        label_Rectube_Urgent.Text = "S";
                    }
                    else if (cellURGENT == "0")
                    {
                        pictureBox1.Visible = false;
                        label_Rectube_Urgent.Text = "R";
                    }

                    if (cellSecret == "1")
                    {
                        pictureBox2.Visible = true;
                        lblConfidential.Text = "Yes";
                    }
                    else if (cellSecret == "0")
                    {
                        pictureBox2.Visible = false;
                        lblConfidential.Text = "No";
                    }
                    else
                    {
                        pictureBox2.Visible = false;
                        lblConfidential.Text = "-";
                    }


                    OrderCOMMENT.Text = cellCommentReq;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strDateTime + "  Log: L1680 : Treelist_select_by_Search?" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //if (treeList1.Nodes[].CheckState == CheckState.Checked)
            //{
            //    dropDownButton_Barcode_print.Enabled = true;
            //}
            //if (treeList1.FocusedNode.CheckState == CheckState.Checked)
            //{
            //    dropDownButton_Barcode_print.Enabled = true;

            //}
            //else 
            //{
            //    dropDownButton_Barcode_print.Enabled = true;
            //}

        }

        private void treeList1_MouseLeave(object sender, EventArgs e)
        {
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioGroup edit = sender as RadioGroup;
            if (edit.SelectedIndex == 0)
            {
                ClearPatientInfomation();
                treeList1.ClearNodes();
                treeList1.Columns.Clear();

                dateTimePicker_start.Enabled = true;
                timeEdit_start.Enabled = true;
                dateTimePicker_finish.Enabled = true;
                timeEdit_finish.Enabled = true;

                comboBox_Search_DOCTOR.Enabled = false;
                comboBox_Search_LOCATION.Enabled = false;

            }
            if (edit.SelectedIndex == 1)
            {
                ClearPatientInfomation();

                treeList1.ClearNodes();
                treeList1.Columns.Clear();

                dateTimePicker_start.Enabled = true;
                timeEdit_start.Enabled = true;
                dateTimePicker_finish.Enabled = true;
                timeEdit_finish.Enabled = true;

                comboBox_Search_DOCTOR.Enabled = false;
                comboBox_Search_LOCATION.Enabled = false;

            }
            if (edit.SelectedIndex == 2)
            {
                ClearPatientInfomation();

                treeList1.ClearNodes();
                treeList1.Columns.Clear();

                dateTimePicker_start.Enabled = false;
                timeEdit_start.Enabled = false;
                dateTimePicker_finish.Enabled = false;
                timeEdit_finish.Enabled = false;

                comboBox_Search_DOCTOR.Enabled = true;
                comboBox_Search_LOCATION.Enabled = false;

            }
            if (edit.SelectedIndex == 3)
            {
                ClearPatientInfomation();

                treeList1.ClearNodes();
                treeList1.Columns.Clear();

                dateTimePicker_start.Enabled = false;
                timeEdit_start.Enabled = false;
                dateTimePicker_finish.Enabled = false;
                timeEdit_finish.Enabled = false;

                comboBox_Search_DOCTOR.Enabled = false;
                comboBox_Search_LOCATION.Enabled = true;


            }
            if (edit.SelectedIndex == 4)
            {
                ClearPatientInfomation();

                dateTimePicker_start.Enabled = false;
                timeEdit_start.Enabled = false;
                dateTimePicker_finish.Enabled = false;
                timeEdit_finish.Enabled = false;

                comboBox_Search_DOCTOR.Enabled = false;
                comboBox_Search_LOCATION.Enabled = false;
            }
        }
        private void ClearPatientInfomation()
        {
            txtPatnum.Text = "";
            labelControl_name.Text = "";
            labelControl_Birthdate.Text = "";
            labelControl_diag.Text = "";
            labelControl_sex.Text = "";
            labelControl_altnum.Text = "";
            labelControl_Age.Text = "";
            lblHostnum.Text = "";
            memoEdit_Comments.Text = "";
            labelControl_tltle.Text = "";
        }

        private void checkButton_Search_MouseMove(object sender, MouseEventArgs e)
        {
            textEdit_search.Select();
        }

        private void textEdit_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 0)
                {
                    // Search Patient..
                    // Set boolean true if select find Patient and Accessnumber
                    Get_check_patient_find = false;
                    // Start Process
                    if (Form_Search_TestResult == false)
                    {
                        //Process_FindPatientsWithTest();
                        Process_FindPatients();
                    }
                    else
                    {
                        Process_FindPatientsWithTest();
                    }

                }
                else if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 1)
                {
                    // Search Accessnumber..
                    // Set boolean false if select search by Accessnumber
                    Get_check_patient_find = false;
                    // Start Process
                    /*Edit By Songdech S.
                     * Edit Date: 17/03/21
                     * Description: add new function for search request test
                     */
                    if (Form_Search_TestResult == true)
                    {
                        Process_FindAccessnumber_WithTestResult();
                    }
                    else
                    {
                        Process_FindAccessnumber();
                    }


                }
                else if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 2)
                {
                    // Search Protocol
                }
                else
                {
                    MessageBox.Show("Please select source first");
                }
            }
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            //{
            //    e.Handled = true;
            //}
            //// only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }

        private void dropDownButton2_Click(object sender, EventArgs e)
        {
            textEdit_Search_text.Text = "";
            treeList1.ClearNodes();
            treeList1.Columns.Clear();
            CultureInfo us = new CultureInfo("en-US");
            dateTimePicker_start.Value.Date.ToString("yyyy-MM-dd", us);
            timeEdit_start.Time.ToString("HH:mm:ss");
            dateTimePicker_finish.Value.Date.ToString("yyyy-MM-dd", us);
            timeEdit_finish.Time.ToString("HH:mm:ss");

            dateTimePicker_start.Value = DateTime.Now;
            dateTimePicker_finish.Value = DateTime.Now;
            timeEdit_start.Time = DateTime.Now;
            timeEdit_finish.Time = DateTime.Now;

            radioGroup1.SelectedIndex = 0;
        }

        private void textEdit_Search_text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Get_check_patient_find = true;
                Search_by_text();
            }
        }

        private void dropDownButton_search_text_Click(object sender, EventArgs e)
        {
            Search_by_text();
        }

        private void Search_by_text()
        {
            if (textEdit_Search_text.Text != "" && comboBox1.SelectedIndex == 0)
            {
                int count;
                string get_data = "";
                string filter_combo_0 = "";
                if (comboBox1.SelectedIndex == 0)
                {
                    count = 0;
                    get_data = "Select Patient Filter";
                    filter_combo_0 = @"SELECT PATIENTS.PATID
,PATIENTS.PATNUMBER,PATIENTS.TITLE1,PATIENTS.TITLE2,PATIENTS.NAME,PATIENTS.LASTNAME,PATIENTS.ADDRESS1,PATIENTS.ADDRESS2,PATIENTS.CITY
,PATIENTS.STATE,PATIENTS.POSTALCODE,PATIENTS.COUNTRY,PATIENTS.BIRTHDATE
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate())) / 12 as AGEY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate())) % 12 as AGEM
,PATIENTS.SEX,PATIENTS.TELEPHON,PATIENTS.TELEPHON2,PATIENTS.FAX,PATIENTS.EMAIL,PATIENTS.INCOMMINGDATE,PATIENTS.ROOMNUMBER
,PATIENTS.REFDOCTOR,PATIENTS.REFLOCATION,PATIENTS.LASTUPDTESTDATE,PATIENTS.BG_ABO,PATIENTS.BG_RHESUS,PATIENTS.BG_PHENOTYPES
,PATIENTS.BG_KELL,PATIENTS.DEATHDATE,PATIENTS.SECRETRESULT,PATIENTS.VIP,PATIENTS.DOCID,PATIENTS.PATCREATIONDATE,PATIENTS.STARTVALIDDATE
,PATIENTS.ENDVALIDDATE,PATIENTS.CREATEBY,PATIENTS.LOGUSERID,PATIENTS.LOGDATE,PATIENTS.COMMENT
FROM PATIENTS WHERE PATIENTS.NAME like '" + textEdit_Search_text.Text + "%" + "' ";

                    Writedatalog.WriteLog(strDateTime + "  Log:Q1015 :Search By Text Check filter_combo ?         " + get_data + "        : " + "\r\n" + filter_combo_0);
                    SqlCommand cmd = new SqlCommand(filter_combo_0, conn);
                    SqlDataAdapter adap = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    cmd.ExecuteNonQuery();
                    ds.Clear();
                    adap.Fill(ds, "Query_patient");

                    if (ds.Tables["Query_patient"].Rows.Count > 0)
                    {
                        CreateColumns_patientSearch(treeList1, ds);
                        treeList1.BeginUnboundLoad();
                        treeList1.ClearNodes();
                        TreeListNode parentForRootNodes = null;

                        for (int i = 0; i < ds.Tables["Query_patient"].Rows.Count; i++)
                        {
                            count++;
                            childNode = treeList1.AppendNode(new object[] {
                        ds.Tables["Query_patient"].Rows[i]["PATID"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["PATNUMBER"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["NAME"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["LASTNAME"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["REFDOCTOR"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["REFLOCATION"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["COMMENT"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["BIRTHDATE"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["SEX"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["AGEY"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["AGEM"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["TITLE1"].ToString(),
                        ds.Tables["Query_patient"].Rows[i]["TITLE2"].ToString() }, parentForRootNodes);

                            treeList1.EndUnboundLoad();
                            treeList1.ExpandAll();
                        }
                        label4.Text = "Total Line " + count.ToString();

                        MessageBox.Show("Finish Query data", "", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Not Found data?", "", MessageBoxButtons.OK);
                    }
                }
            }
            else
            {
                MessageBox.Show("Search by text Please insert some Charector ?", "", MessageBoxButtons.OK);
            }
        }

        private void radioGroup1_MouseDown(object sender, MouseEventArgs e)
        {
            textEdit_Search_text.Text = "";
        }

        private void textEdit_Search_text_Click(object sender, EventArgs e)
        {
            dropDownButton_Start_search.Enabled = false;

            dateTimePicker_start.Enabled = false;
            timeEdit_start.Enabled = false;
            dateTimePicker_finish.Enabled = false;
            timeEdit_finish.Enabled = false;

            comboBox_Search_DOCTOR.Enabled = false;
            comboBox_Search_LOCATION.Enabled = false;


        }

        private void radioGroup1_MouseHover(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                dateTimePicker_start.Enabled = true;
                timeEdit_start.Enabled = true;
                dateTimePicker_finish.Enabled = true;
                timeEdit_finish.Enabled = true;

                comboBox_Search_DOCTOR.Enabled = false;
                dropDownButton_Start_search.Enabled = true;

            }

            else if (radioGroup1.SelectedIndex == 1)
            {
                dateTimePicker_start.Enabled = true;
                timeEdit_start.Enabled = true;
                dateTimePicker_finish.Enabled = true;
                timeEdit_finish.Enabled = true;

                comboBox_Search_DOCTOR.Enabled = false;
                dropDownButton_Start_search.Enabled = true;

            }
            else if (radioGroup1.SelectedIndex == 2)
            {
                dateTimePicker_start.Enabled = true;
                timeEdit_start.Enabled = true;
                dateTimePicker_finish.Enabled = true;
                timeEdit_finish.Enabled = true;

                comboBox_Search_DOCTOR.Enabled = true;
                comboBox_Search_LOCATION.Enabled = false;
                dropDownButton_Start_search.Enabled = true;
            }
            else if (radioGroup1.SelectedIndex == 3)
            {
                dateTimePicker_start.Enabled = false;
                timeEdit_start.Enabled = false;
                dateTimePicker_finish.Enabled = false;
                timeEdit_finish.Enabled = false;

                comboBox_Search_DOCTOR.Enabled = false;
                comboBox_Search_LOCATION.Enabled = true;
                dropDownButton_Start_search.Enabled = true;

            }
            else if (radioGroup1.SelectedIndex == 4)
            {
                dateTimePicker_start.Enabled = false;
                timeEdit_start.Enabled = false;
                dateTimePicker_finish.Enabled = false;
                timeEdit_finish.Enabled = false;

                comboBox_Search_DOCTOR.Enabled = false;
                comboBox_Search_LOCATION.Enabled = false;

                dropDownButton_Start_search.Enabled = true;
            }
        }

        private void textEdit_Search_text_KeyPress(object sender, KeyPressEventArgs e)
        {
            dropDownButton_Start_search.Enabled = false;

            dateTimePicker_start.Enabled = false;
            timeEdit_start.Enabled = false;
            dateTimePicker_finish.Enabled = false;
            timeEdit_finish.Enabled = false;

            comboBox_Search_DOCTOR.Enabled = false;
            comboBox_Search_LOCATION.Enabled = false;
        }

        private void xtraTabControl_request_info_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl_request_info.SelectedTabPage == Search2)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    Clearinformationall();
                    Get_check_patient_find = true;
                    Treelist_with_search = false;
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    Clearinformationall();
                    Get_check_patient_find = false;
                    Treelist_with_search = false;
                }


                textEdit_search.Select();
                // Clear text Search
                textEdit_search.Text = "";

                textEdit_search.Enabled = true;
                Clearinformationall();

                dropDownButton_SearchPAT.Enabled = false;
                textEdit_search.Enabled = false;
                if (comboBox1.SelectedIndex == 0)
                {
                    textEdit_Search_text.Enabled = true;
                }
                else
                {
                    textEdit_Search_text.Enabled = false;
                }
                dateTimePicker_start.Enabled = true;
                timeEdit_start.Enabled = true;
                dateTimePicker_finish.Enabled = true;
                timeEdit_finish.Enabled = true;
                dropDownButton_Start_search.Enabled = true;
                radioGroup1.Enabled = true;
                dropDownButton2.Enabled = true;
            }
            else if (xtraTabControl_request_info.SelectedTabPage == Search1)
            {
                Get_check_patient_find = false;
                treeList1.ClearNodes();
                treeList1.Columns.Clear();

                textEdit_search.Enabled = true;
                textEdit_Search_text.Enabled = false;

                dropDownButton_SearchPAT.Enabled = true;
                dateTimePicker_start.Enabled = false;
                timeEdit_start.Enabled = false;
                dateTimePicker_finish.Enabled = false;
                timeEdit_finish.Enabled = false;
                dropDownButton_Start_search.Enabled = false;
                radioGroup1.Enabled = false;
                comboBox_Search_DOCTOR.Enabled = false;
                comboBox_Search_LOCATION.Enabled = false;
                dropDownButton2.Enabled = false;

                radioGroup1.SelectedIndex = 0;
            }
        }

        private void barBtnNewOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtPatnum.Text != "")
            {
                Form_REQUEST_NEWORDER FM_NEWORDER = new Form_REQUEST_NEWORDER(txtPatnum.Text, labelControl_tltle.Text, labelControl_name.Text, labelControl_Birthdate.Text, labelControl_sex.Text, labelControl_Age.Text, labelControl_diag.Text, "Creation");
                FM_NEWORDER.ShowDialog();
            }
        }

        private void barBtnReset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabControl_request_info.SelectedTabPage = Search1;
            Clearinformationall();

        }

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barBtnModify_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtPatnum.Text != "")
            {
                Form_REQUEST_NEWORDER FM_NEWORDER = new Form_REQUEST_NEWORDER(txtPatnum.Text, labelControl_tltle.Text, labelControl_name.Text, labelControl_Birthdate.Text, labelControl_sex.Text, labelControl_Age.Text, labelControl_diag.Text, "Modification", lblAccessnumber.Text);
                FM_NEWORDER.ShowDialog();
            }

        }

        private void checkButton_Search_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                frmStain fm = new frmStain();
                OrderEntryM objRequest = new OrderEntryM();
                OEMController objOrder = new OEMController();

                objRequest.objPatientM = objPatientM;

                objRequest.AccessNumber = gridView1.GetFocusedRowCellValue("ACCESSNUMBER").ToString().Trim();
                objRequest.MBReqNumber = gridView1.GetFocusedRowCellValue("MBREQNUMBER").ToString().Trim();

                dtReq.DefaultView.RowFilter = " ACCESSNUMBER = '" + objRequest.AccessNumber + "' and MBREQNUMBER = '" + objRequest.MBReqNumber + "'";
                dtReq.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                DataTable filtered = dtReq.Clone();

                foreach (DataRowView drv in dtReq.DefaultView)
                {
                    filtered.Rows.Add(drv.Row.ItemArray);
                }

                if (string.IsNullOrEmpty(filtered.Rows[0]["COLLECTIONDATE"].ToString()) == false)
                {
                    objRequest.CollectionDate = Convert.ToDateTime(filtered.Rows[0]["COLLECTIONDATE"].ToString());
                }

                if (string.IsNullOrEmpty(filtered.Rows[0]["STATUSDATE"].ToString()) == false)
                {
                    objRequest.StatusDate = Convert.ToDateTime(filtered.Rows[0]["STATUSDATE"].ToString());
                }

                if (string.IsNullOrEmpty(filtered.Rows[0]["REQDATE"].ToString()) == false)
                {
                    objRequest.ReqDate = Convert.ToDateTime(filtered.Rows[0]["REQDATE"].ToString());
                }

                if (string.IsNullOrEmpty(filtered.Rows[0]["RECEIVEDDATE"].ToString()) == false)
                {
                    objRequest.ReceiveDate = Convert.ToDateTime(filtered.Rows[0]["RECEIVEDDATE"].ToString());
                }

                if (string.IsNullOrEmpty(filtered.Rows[0]["LASTUPDATE"].ToString()) == false)
                {
                    objRequest.LastUpdateDate = Convert.ToDateTime(filtered.Rows[0]["LASTUPDATE"].ToString());
                }

                objRequest.ReqCreationDate = filtered.Rows[0]["REQCREATIONDATE"].ToString();
                objRequest.Comment = filtered.Rows[0]["Comment"].ToString();
                objRequest.HostOrderNumber = filtered.Rows[0]["HOSTORDERNUMBER"].ToString();
                objRequest.ProtocolCode = filtered.Rows[0]["PROTOCOLCODE"].ToString();
                objRequest.ProtocolText = filtered.Rows[0]["PROTOCOLTEXT"].ToString();
                objRequest.ReqDoctor = filtered.Rows[0]["REQDOCTOR"].ToString();
                objRequest.ReqLocation = filtered.Rows[0]["REQLOCATION"].ToString();
                objRequest.ReqStatus = filtered.Rows[0]["REQSTATUS"].ToString();
                objRequest.SpecimentCode = filtered.Rows[0]["SPECIMENCODE"].ToString();
                objRequest.UrgentStatus = filtered.Rows[0]["URGENT"].ToString();
                objRequest.REQUESTID = Convert.ToInt16(filtered.Rows[0]["REQUESTID"].ToString());
                objRequest.MBRequestID = objOrder.GetMBRequestID(objRequest.REQUESTID, objRequest.MBReqNumber);
                objRequest.ProtocolID = Convert.ToInt16(filtered.Rows[0]["PROTOCOLID"].ToString());
                
                fm.OrderEntryObject = objRequest;

                fm.Show();

                this.Close();

                String a = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message, "", MessageBoxButtons.OK);
            }
        }

        private void dropDownButton1_Click(object sender, EventArgs e)
        {
            Clearinformationall();
        }

        private void textEdit_search_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
