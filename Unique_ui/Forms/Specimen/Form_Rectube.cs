using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using UNIQUE.Forms.Report_worksheet;
using UniquePro.Controller;

namespace UNIQUE.Forms.Specimen

{
    public partial class Form_Rectube : Form
    {
        // Step delete
        //////delete from PROTOCOL_COUNTERS where COUNTERID=48
        //////update SP_TESTS set SP_TESTSTATUS = 0

        // ถ้า เคลีย Request ที่รับไปแล้วทั้งหมด
        //delete from REQ_TESTS
        //delete from MB_REQUESTS
        //delete from REQUESTS
        //delete from SUBREQMB_STAINS

        // ถ้า เคลีย Requests ที่ยังไม่รับ หรือรับแล้วบางส่วน
        //delete from SP_TESTS
        //delete from SP_REQUESTS

        private string StrAccessnumber = "";

        private static string Version_Rectube = "Rectube Version [2020-03]";
        // Format DateTime en-US
        System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");
        System.Globalization.CultureInfo cultureinfo_TH = new System.Globalization.CultureInfo("th-TH");
        System.Globalization.CultureInfo cultureinfo_ENG = new System.Globalization.CultureInfo("en-US");

        string format_DT = "dd/MM/yyyy HH:mm:ss";

        string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));


        // Confirm Encryption
        AES128_EncryptAndDecrypt AES_128 = new AES128_EncryptAndDecrypt();
        // Confirm Encryption

        int Length_LN;

        // Patient Info
        string strBirthDate = "";
        string StrPATID_REQ = "";
        // Get IP
        string LocalIP = "";
        string IPAddress;
        //
        // Query Step 2.
        string CODE_ABCDEFG = "";
        //

        // Query Step 3.
        string StrPRE_TESTCODE = "";
        string setting_ORM = "setting_ORM.xml";
        // AS Spy path
        string pathspy = "";
        string Query_Accnumber = "";
        TreeListNode childNode;
        SqlConnection conn;

        // ASTM Create
        int compture;
        int StrIPcounter;
        string bdate = "";
        string Psex = "";
        //
        // Dianostic
        public static DataSet ds_result;
        int Querydiag_status = 0;
        private static string Passunlock;
        private static TextBox text_inputClose;
        //

        // Request IPD or OPD
        string Str_Req_IPDOPD = "";

        // Request Urgent ?
        string Str_Req_Urgent = "";
        // Implement User login
        // *************************Change if config role
        string StrUserlogin = "SYS";
        // *************************

        // Secret result
        string StrSecret = "";

        public Form_Rectube(string StrAccnumber)
        {
            InitializeComponent();
            this.StrAccessnumber = StrAccnumber;
        }
        private void Form_Reqtube_Load(object sender, EventArgs e)
        {
            //clear_information();
            this.Text = Version_Rectube;
            txtAccessNumber.Select();
            label_version.Text = Version_Rectube;
            comboBox_Diag_status.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            GetIPAddress();

            string[] values = IPAddress.Split('.');
            LocalIP = values[3];
            Int32.TryParse(LocalIP, out StrIPcounter);

            try
            {
                // read XML setting 
                DataSet dsXmlFile = new DataSet();
                dsXmlFile.ReadXml(setting_ORM, XmlReadMode.Auto);
                pathspy = dsXmlFile.Tables[0].Rows[0]["pathSPY"].ToString();
                Passunlock = dsXmlFile.Tables[0].Rows[0]["password_close"].ToString();

                Length_LN = Convert.ToInt32(dsXmlFile.Tables[0].Rows[0]["Length_Accnum"].ToString());
            }
            catch (Exception)
            { }

            if (StrAccessnumber !="")
            {
                //
                txtAccessNumber.Text = StrAccessnumber;
                label_STATUSREQ.Text = "CREATION";
                Query_request_ORM(StrAccessnumber);
            }
            else
            {

                // Not thing
            }


        }
        private void button_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button_Clean_Click(object sender, EventArgs e)
        {
            clear_information();
        }
        private void txtAccessNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchDataInformations();
            }
        }
        private void timer_DateTime_Tick(object sender, EventArgs e)
        {
            string format_DT_Now = "yyyy-MM-dd HH:mm:ss";

            lblDATE_time.Text = DateTime.Now.ToString(format_DT_Now, cultureinfo);
        }
        private void SetNodeCheckState(TreeList tl, DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            var checkState = node.CheckState;
            var resCheckState = CheckState.Indeterminate;
            switch (checkState)
            {
                case CheckState.Checked:
                    resCheckState = CheckState.Unchecked;
                    break;
                case CheckState.Unchecked:
                    resCheckState = CheckState.Checked;
                    break;
                case CheckState.Indeterminate:
                    resCheckState = CheckState.Checked;
                    break;
            }
            tl.SetNodeCheckState(node, resCheckState, true);
        }
        private void treeList1_MouseDown(object sender, MouseEventArgs e)
        {
            var tl = sender as TreeList;
            if (tl == null)
            {
                return;
            }
            var hi = tl.CalcHitInfo(e.Location);
            if (hi.HitInfoType == HitInfoType.NodeCheckBox)
            {
                return;
            }
            var node = hi.Node;
            if (node != null)
            {
                SetNodeCheckState(tl, node);
            }
        }
        private void clear_information()
        {
            Query_Accnumber = "-";
            txtAccessNumber.Text = "";
            txtAccessNumber.Select();
            lblAccessnumber.Text = "";
            lblHN.Text = "";
            lblName.Text = "";
            lblBirthDate.Text = "";
            lblage.Text = "";
            lblDoctor.Text = "";
            lblLocation.Text = "";
            lblHISNO.Text = "";
            lblRequested.Text = "";
            lblCollected.Text = "";
            richTextBox_OrderCOMMENT.Text = "";
            label_STATUSREQ.Text = "-";
            checkBox1.Checked = false;
            treeList1.EndUnboundLoad();
            treeList1.ClearNodes();
            button_Save.Enabled = false;
            label_IPDOPD.Text = "-";
            lblConfidential.Text = "-";
            pictureBox2.Visible = false;
            label_specimen_req.Text = "-";
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (treeList1.AllNodesCount > 0)
            {
                if (checkBox1.Checked == true)
                {
                    foreach (var node in treeList1.Nodes.OfType<TreeListNode>())
                    {
                        treeList1.SetNodeCheckState(node, CheckState.Checked);
                    }
                }
                else
                {
                    foreach (var node in treeList1.Nodes.OfType<TreeListNode>())
                    {
                        treeList1.SetNodeCheckState(node, CheckState.Unchecked);
                    }
                }
            }
        }
        private void CreateNodes(TreeList tl, DataSet ds)
        {
            tl.BeginUnboundLoad();
            tl.ClearNodes();

            // Create a root node .
            TreeListNode parentForRootNodes = null;

            for (int i = 0; i < ds.Tables["result_req"].Rows.Count; i++)
            {
                childNode = tl.AppendNode(new object[] {
                    ds.Tables["result_req"].Rows[i]["PRE_TESTCODE"].ToString(),
                    ds.Tables["result_req"].Rows[i]["PRE_TESTNAME"].ToString(),
                    ds.Tables["result_req"].Rows[i]["SPECIMENNAME"].ToString(),
                    ds.Tables["result_req"].Rows[i]["COMMENTS"].ToString(),
                    ds.Tables["result_req"].Rows[i]["PROTOCOLCODE"].ToString(),
                    ds.Tables["result_req"].Rows[i]["PROTOCOLNAME"].ToString(),
                    ds.Tables["result_req"].Rows[i]["PATNUMBER"].ToString(),
                    ds.Tables["result_req"].Rows[i]["NAME"].ToString(),
                    ds.Tables["result_req"].Rows[i]["LASTNAME"].ToString(),
                    ds.Tables["result_req"].Rows[i]["PROTOCOLID"].ToString() },
                    parentForRootNodes);
            }
            tl.EndUnboundLoad();
            tl.ExpandAll();
        }
        private void CreateColumns(TreeList tl, DataSet ds)
        {
            // Create three columns.
            tl.BeginUpdate();
            tl.Columns.Add();
            tl.Columns[0].Caption = "<i><b>Code</i></b>";
            tl.Columns[0].VisibleIndex = 0;
            tl.Columns[0].Width = 100;
            tl.Columns[0].Visible = true;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.RowHeight = 23;
            tl.Columns.Add();

            tl.Columns[1].Caption = "<i>Test</i><b> (TEXT)</b>";
            tl.Columns[1].VisibleIndex = 1;
            tl.Columns[1].Width = 300;
            tl.Columns[1].Visible = true;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.Columns.Add();

            tl.Columns[2].Caption = "Specimen";
            tl.Columns[2].VisibleIndex = 2;
            tl.Columns[2].Width = 250;
            tl.Columns[2].Visible = true;
            tl.Columns.Add();

            tl.Columns[3].Caption = "Comment";
            tl.Columns[3].VisibleIndex = 3;
            tl.Columns[3].Width = 250;
            tl.Columns[3].Visible = true;
            tl.Columns.Add();

            tl.Columns[4].Caption = "P";
            tl.Columns[4].VisibleIndex = 4;
            tl.Columns[4].Width = 50;
            tl.Columns[4].Visible = true;
            tl.Columns.Add();

            tl.Columns[5].Caption = "Protocol";
            tl.Columns[5].VisibleIndex = 5;
            tl.Columns[5].Width = 130;
            tl.Columns[5].Visible = true;
            tl.Columns.Add();

            tl.Columns[6].Caption = "HN";
            tl.Columns[6].VisibleIndex = 6;
            tl.Columns[6].Width = 90;
            tl.Columns[6].Visible = false;
            tl.Columns.Add();

            tl.Columns[7].Caption = "Name";
            tl.Columns[7].VisibleIndex = 7;
            tl.Columns[7].Width = 100;
            tl.Columns[7].Visible = false;
            tl.Columns.Add();

            tl.Columns[8].Caption = "LastName";
            tl.Columns[8].VisibleIndex = 8;
            tl.Columns[8].Width = 100;
            tl.Columns[8].Visible = false;
            tl.Columns.Add();

            tl.Columns[9].Caption = "PROTOCALID";
            tl.Columns[9].VisibleIndex = 8;
            tl.Columns[9].Width = 100;
            tl.Columns[9].Visible = true;
            tl.Columns.Add();

            tl.Columns[10].Caption = "PATID";
            tl.Columns[10].VisibleIndex = 10;
            tl.Columns[10].Width = 10;
            tl.Columns[10].Visible = false;
            tl.Columns.Add();

            tl.Columns[11].Caption = "TESTID";
            tl.Columns[11].VisibleIndex = 11;
            tl.Columns[11].Width = 10;
            tl.Columns[11].Visible = false;
            tl.Columns.Add();

            tl.Columns[12].Caption = "REQDATE";
            tl.Columns[12].VisibleIndex = 12;
            tl.Columns[12].Width = 10;
            tl.Columns[12].Visible = false;
            tl.Columns.Add();

            tl.Columns[13].Caption = "COLLECTIONDATE";
            tl.Columns[13].VisibleIndex = 13;
            tl.Columns[13].Width = 10;
            tl.Columns[13].Visible = false;
            tl.Columns.Add();

            tl.Columns[14].Caption = "LOGUSERID";
            tl.Columns[14].VisibleIndex = 14;
            tl.Columns[14].Width = 10;
            tl.Columns[14].Visible = false;
            tl.Columns.Add();

            tl.Columns[15].Caption = "LOGDATE";
            tl.Columns[15].VisibleIndex = 15;
            tl.Columns[15].Width = 10;
            tl.Columns[15].Visible = false;
            tl.Columns.Add();

            tl.Columns[16].Caption = "SP_TESTSID";
            tl.Columns[16].VisibleIndex = 16;
            tl.Columns[16].Width = 10;
            tl.Columns[16].Visible = false;
            tl.Columns.Add();

            tl.Columns[17].Caption = "SP_SPECIMENCODE";
            tl.Columns[17].VisibleIndex = 17;
            tl.Columns[17].Width = 10;
            tl.Columns[17].Visible = false;
            tl.Columns.Add();

            tl.EndUpdate();
        }
        private void button_Search_Click(object sender, EventArgs e)
        {
            searchDataInformations();
        }
        public string GetIPAddress()
        {

            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                    string[] values = IPAddress.Split('.');
                    LocalIP = values[3];
                    lbl_IPADDRESS.Text = IPAddress;
                }
            }
            return IPAddress;
        }
        public void queryPatients(string Accnumber_query)
        {
            if (txtAccessNumber.Text != "-")
            {
                Query_request_ORM(Accnumber_query);

                if (Query_Accnumber != "")
                {
                    // Do modification
                    label_STATUSREQ.Text = Query_Accnumber;
                    //MessageBox.Show("No Data in database Create new?", "", MessageBoxButtons.OK);
                }
                else
                {
                    // Do Creation
                    label_STATUSREQ.Text = "CREATION";
                }
            }
        }
        private void Insert_New_counter(int StrIPcounter)
        {
            // 6.1 Insert counter if new client
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

                Query_counter(StrIPcounter);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error insert COUNTER in Database \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Query_counter(int IPaddress)
        {
            // 6. Query select counter in COUNTER table
            string sql_compture = "";

            sql_compture = @"SELECT COUNTERID,COUNTERNUMBER,LENGHT,COUNTER,NOTE FROM COUNTER WHERE COUNTERNUMBER='" + IPaddress + "'";

            Writedatalog.WriteLog("Start Query_Counter with IP address ---->" + "\r\n" + sql_compture);
            SqlCommand cmd = new SqlCommand(sql_compture, conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            cmd.ExecuteNonQuery();
            ds.Clear();
            adp.Fill(ds, "result_cou");
            SqlDataReader reader = cmd.ExecuteReader();

            if (ds.Tables["result_cou"].Rows.Count > 0)
            {
                string StrCou = ds.Tables["result_cou"].Rows[0]["COUNTER"].ToString();
                Int32.TryParse(StrCou, out compture);
            }
            else
            {
                // If new client & new Ipaddress
                Insert_New_counter(StrIPcounter);
                //
            }
        }
        private void Update_counter(int compture)
        {
            // 8.Update counter in DB table COUNTER
            //  N'UPDATE COUNTER SET COUNTER =@COUNTER WHERE COUNTERNUMBER =1',N'@COUNTER varchar(6)',@COUNTER='192252'
            string sql_Update_Cou = "";

            sql_Update_Cou = @"exec sp_executesql N'UPDATE COUNTER SET COUNTER = @COUNTER WHERE COUNTERNUMBER = " + StrIPcounter + " ',N'@COUNTER varchar(5)',@COUNTER='" + compture + "'";

            Writedatalog.WriteLog("Start Insert New Client & counter---->" + "\r\n" + sql_Update_Cou);
            SqlCommand cmd = new SqlCommand(sql_Update_Cou, conn);
            //cmd.Parameters.Add("@COUNTER", SqlDbType.VarChar).Value = compture;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            cmd.ExecuteNonQuery();
        }
        private void Query_request_ORM(string Accnumber_query)
        {
            // Step 1. Query data
            string sql_req_ACC = "";
            string Strsex = "";
            StrPATID_REQ = "";
            try
            {
                conn = new Connection_ORM().Connect();
                sql_req_ACC = @"SELECT 
SP_TESTS.SP_ACCESSNUMBER AS SP_TESTS_ACCNUM
,SP_TESTS.SP_TESTSID AS SPTESTSID
,SP_TESTS.SP_TESTCODE
,DICT_TESTS.TESTID
,SP_TESTS.SP_STNCODE
,DICT_MB_PROTOCOLS.PROTOCOLID
,SP_REQUESTS.HOSTORDERNUMBER
,SP_REQUESTS.REQDATE
,SP_REQUESTS.SP_DOCCODE
,SP_REQUESTS.SP_LOCCODE
,SP_TESTS.SP_TESTNAME
,SP_TESTS.SP_TESTCODE
,SP_TESTS.SP_SPECIMENCODE
,SP_TESTS.SP_SPECIMENNAME
,DICT_MB_PROTOCOLS.PROTOCOLTEXT
,PATID,PATNUMBER,NAME,LASTNAME
,ADDRESS1,ADDRESS2,CITY,STATE,POSTALCODE,COUNTRY
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as AGEY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as AGEM,PATIENTS.BIRTHDATE,SEX,TELEPHON
,TELEPHON2,FAX,EMAIL,INCOMMINGDATE,PATIENTS.ROOMNUMBER,REFDOCTOR,REFLOCATION,LASTUPDTESTDATE
,BG_ABO,BG_RHESUS,BG_PHENOTYPES,BG_KELL,DEATHDATE
,SP_REQUESTS.SECRETRESULT
,PATIENTS.VIP
,DOCID,PATCREATIONDATE,STARTVALIDDATE,CREATEBY,COMMENT
,SP_TESTS.SP_TESTSTATUS AS SP_TESTSTATUS
,PATIENTS.PATID
,SP_REQUESTS.SP_SPECIMENCODE AS REQSPECIMEN
,SP_TESTS.SP_RECEIVEDDATE
,SP_REQUESTS.REQDATE
,SP_REQUESTS.COLLECTIONDATE
,SP_REQUESTS.LOGUSERID
,SP_REQUESTS.LOGDATE
,SP_REQUESTS.REQURGENT
,SP_REQUESTS.SP_IPDOROPD
FROM PATIENTS
LEFT OUTER JOIN SP_REQUESTS ON PATIENTS.PATNUMBER = SP_REQUESTS.PATNUM
LEFT OUTER JOIN SP_TESTS ON SP_REQUESTS.SP_ACCESSNUMBER = SP_TESTS.SP_ACCESSNUMBER
LEFT OUTER JOIN DICT_MB_PROTOCOLS ON  SP_TESTS.SP_STNCODE = DICT_MB_PROTOCOLS.PROTOCOLCODE
LEFT OUTER JOIN DICT_TESTS ON DICT_TESTS.TESTCODE = SP_TESTS.SP_TESTCODE
WHERE SP_REQUESTS.SP_ACCESSNUMBER ='" + Accnumber_query + "' AND SP_TESTS.SP_TESTSTATUS = 0";

                Writedatalog.WriteLog_Rectube("Start Query_RECTUBE  Statement---->" + "\r\n" + sql_req_ACC);
                SqlCommand cmd = new SqlCommand(sql_req_ACC, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "result_req");
                SqlDataReader reader = cmd.ExecuteReader();
                CreateColumns(treeList1, ds);
                //CreateNodes(treeList1, ds);
                treeList1.BeginUnboundLoad();
                treeList1.ClearNodes();
                TreeListNode parentForRootNodes = null;
                if (ds.Tables["result_req"].Rows.Count > 0)
                {
                    Query_Accnumber = "CREATION";

                    button_Save.Enabled = true;

                    for (int i = 0; i < ds.Tables["result_req"].Rows.Count; i++)
                    {
                        childNode = treeList1.AppendNode(new object[] {
                        ds.Tables["result_req"].Rows[i]["SP_TESTCODE"].ToString(),
                        ds.Tables["result_req"].Rows[i]["SP_TESTNAME"].ToString(),
                        ds.Tables["result_req"].Rows[i]["SP_SPECIMENNAME"].ToString(),
                        ds.Tables["result_req"].Rows[i]["COMMENT"].ToString(),
                        ds.Tables["result_req"].Rows[i]["SP_STNCODE"].ToString(),
                        ds.Tables["result_req"].Rows[i]["PROTOCOLTEXT"].ToString(),
                        ds.Tables["result_req"].Rows[i]["PATNUMBER"].ToString(),
                        ds.Tables["result_req"].Rows[i]["NAME"].ToString(),
                        ds.Tables["result_req"].Rows[i]["LASTNAME"].ToString() ,
                        ds.Tables["result_req"].Rows[i]["PROTOCOLID"].ToString(),
                        ds.Tables["result_req"].Rows[i]["PATID"].ToString(),
                        ds.Tables["result_req"].Rows[i]["TESTID"].ToString(),
                        ds.Tables["result_req"].Rows[i]["REQDATE"].ToString(),
                        ds.Tables["result_req"].Rows[i]["COLLECTIONDATE"].ToString(),
                        ds.Tables["result_req"].Rows[i]["LOGUSERID"].ToString(),
                        ds.Tables["result_req"].Rows[i]["LOGDATE"].ToString(),
                        ds.Tables["result_req"].Rows[i]["SPTESTSID"].ToString(),
                        ds.Tables["result_req"].Rows[i]["SP_SPECIMENCODE"].ToString()
                        }, parentForRootNodes);

                        string StrCOMMENTS = ds.Tables["result_req"].Rows[i]["COMMENT"].ToString();
                        richTextBox_OrderCOMMENT.Text = StrCOMMENTS;
                        label_specimen_req.Text = ds.Tables["result_req"].Rows[0]["REQSPECIMEN"].ToString();

                        if (ds.Tables["result_req"].Rows[0]["REQURGENT"].ToString() == "0")
                        {
                            label_Rectube_Urgent.Text = "Routine";
                            Str_Req_Urgent = "0";
                            label_Rectube_Urgent.ForeColor = Color.Green;
                        }
                        else if (ds.Tables["result_req"].Rows[0]["REQURGENT"].ToString() == "1")
                        {
                            label_Rectube_Urgent.Text = "Urgent";
                            Str_Req_Urgent = "1";
                            label_Rectube_Urgent.ForeColor = Color.Red;
                        }

                        if (ds.Tables[0].Rows[0]["PATNUMBER"].ToString() != "")
                        { lblHN.Text = ds.Tables[0].Rows[0]["PATNUMBER"].ToString().TrimStart('0'); }
                        if (ds.Tables["result_req"].Rows[0]["SEX"].ToString() == "1")
                        { Strsex = "ชาย"; }
                        else if (ds.Tables["result_req"].Rows[0]["SEX"].ToString() == "2")
                        { Strsex = "หญิง"; }
                        else
                        { Strsex = "ไม่ระบุ"; }

                        lblName.Text = ds.Tables[0].Rows[0]["NAME"].ToString() + " " + ds.Tables["result_req"].Rows[0]["LASTNAME"].ToString() + "  (" + Strsex + ")";
                        lblAccessnumber.Text = ds.Tables[0].Rows[0]["SP_TESTS_ACCNUM"].ToString();
                        lblHISNO.Text = ds.Tables[0].Rows[0]["HOSTORDERNUMBER"].ToString();

                        DateTime DT = Convert.ToDateTime(ds.Tables[0].Rows[0]["REQDATE"].ToString());
                        string StrReqDate = DT.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));

                        lblRequested.Text = StrReqDate;
                        lblDoctor.Text = ds.Tables[0].Rows[0]["SP_DOCCODE"].ToString();
                        lblLocation.Text = ds.Tables[0].Rows[0]["SP_LOCCODE"].ToString();
                        StrPATID_REQ = ds.Tables[0].Rows[0]["PATID"].ToString();

                        if (ds.Tables[0].Rows[0]["BIRTHDATE"].ToString() != "")
                        {
                            DateTime bDt = Convert.ToDateTime(ds.Tables[0].Rows[0]["BIRTHDATE"].ToString());
                            strBirthDate = bDt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));
                            bdate = strBirthDate;
                            lblBirthDate.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ds.Tables[0].Rows[0]["BIRTHDATE"].ToString()));
                            //string Str_DOB = ((DateTime)(ds.Tables[0].Rows[0]["BIRTHDATE"].ToString("dd-MM-yyyy", cultureinfo_ENG));

                            string Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(ds.Tables[0].Rows[0]["BIRTHDATE"].ToString()));
                            string Str_DateOfBirth = "";
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

                            lblage.Text = Str_DateOfBirth;
                        }

                        if (ds.Tables[0].Rows[0]["SP_IPDOROPD"].ToString() == "0")
                        {
                            label_IPDOPD.Text = "IPD";
                            Str_Req_IPDOPD = "0";
                        }
                        else if (ds.Tables[0].Rows[0]["SP_IPDOROPD"].ToString() == "1")
                        {
                            label_IPDOPD.Text = "OPD";
                            Str_Req_IPDOPD = "1";
                        }
                        else
                        {
                            label_IPDOPD.Text = "-";
                            Str_Req_IPDOPD = "";
                        }

                        if (ds.Tables[0].Rows[0]["SECRETRESULT"].ToString() == "1")
                        {
                            lblConfidential.Text = "Yes";
                            pictureBox2.Visible = true;
                            StrSecret = "0";
                        }
                        else if (ds.Tables[0].Rows[0]["SECRETRESULT"].ToString() == "0")
                        {
                            lblConfidential.Text = "-";
                            pictureBox2.Visible = false;
                            StrSecret = "1";
                        }
                        else
                        {
                            lblConfidential.Text = "-";
                            pictureBox2.Visible = false;
                            StrSecret = "0";
                        }

                        treeList1.EndUnboundLoad();
                        treeList1.ExpandAll();
                    }
                }  // END IF ---> (ds.Tables["result_req"].Rows.Count > 0)
                else
                {
                    //MessageBox.Show("Receive Already in Database? \r\nDetail : ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult OK = MessageBox.Show("Receive Already in Database? \r\nDetail : ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (OK == DialogResult.OK)
                    {
                        clear_information();
                    }
                    Query_Accnumber = "-";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1001 Query Request ORM \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Query_CodeABCDEFG_inDB(string StrCODE_ABCDEFG, string PRE_ACCESSNUMBER)
        {
            // 2. If Have Duplicate code Query find Exist code in DB ---> 
            // SELECT PRE_TESTCODE FROM PRE_TESTS WHERE PRE_TESTCODE = 'A0024' AND PRE_ACCESSNUMBER = '" + PRE_ACCESSNUMBER + "' AND PRE_TESTSTATUS = '10' ";
            string sql_ABCDEFG = "";
            sql_ABCDEFG = @"SELECT PRE_TESTCODE FROM PRE_TESTS WHERE PRE_TESTCODE = '" + StrCODE_ABCDEFG + "' AND PRE_ACCESSNUMBER = '" + PRE_ACCESSNUMBER + "' AND PRE_TESTSTATUS = '10' ";
            SqlCommand cmd = new SqlCommand(sql_ABCDEFG, conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            cmd.ExecuteNonQuery();
            ds.Clear();
            adp.Fill(ds, "result");
            SqlDataReader reader = cmd.ExecuteReader();
            //Writedatalog.WriteLog("Start query ABCDEFG&CODE --> " + sql_ABCDEFG);
            if (ds.Tables["result"].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["PRE_TESTCODE"].ToString() != "")
                {
                    CODE_ABCDEFG = ds.Tables[0].Rows[0]["PRE_TESTCODE"].ToString();
                }
            }
        }
        private void Query_FindCODE(string StrCode, string StrAccnum)
        {
            // 3. Query find code
            // SELECT PRE_TESTCODE FROM PRE_TESTS WHERE PRE_TESTCODE = '50024' AND PRE_ACCESSNUMBER = '180018436'
            string sql_Query_Findcode = "";

            sql_Query_Findcode = @"SELECT PRE_TESTCODE FROM PRE_TESTS WHERE PRE_TESTCODE = '" + StrCode + "' AND PRE_ACCESSNUMBER = '" + StrAccnum + "'";

            SqlCommand cmd = new SqlCommand(sql_Query_Findcode, conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            cmd.ExecuteNonQuery();
            ds.Clear();
            adp.Fill(ds, "result");
            SqlDataReader reader = cmd.ExecuteReader();

            Writedatalog.WriteLog_ORM("Start FindCode --> " + "\r\n" + sql_Query_Findcode);

            if (ds.Tables["result"].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["PRE_TESTCODE"].ToString() != "")
                {
                    StrPRE_TESTCODE = ds.Tables[0].Rows[0]["PRE_TESTCODE"].ToString();
                }
            }

        }
        private void Insert_Newcode(string StrNewcode, string StrAccnum, string StrTestTxt)
        {
            // 5. If select A,B,C,D,E,F,G --> insert database to PRE_TESTS

            // N'INSERT INTO PRE_TESTS (PRE_TESTCODE, PRE_ACCESSNUMBER, PRE_TESTORDER, PRE_TESTNAME, PRE_TESTCREDATE, DATA_BASE, 
            // PRE_TESTSTATUS, LOGUSERID, LOGDATE, TEMP, RECEIVEDDATE) 
            // VALUES (@PRE_TESTCODE, @PRE_ACCESSNUMBER, @PRE_TESTORDER, @PRE_TESTNAME, 
            // @PRE_TESTCREDATE, @DATA_BASE, @PRE_TESTSTATUS, @LOGUSERID, @LOGDATE, @TEMP, @RECEIVEDDATE) 
            // ',N'@PRE_TESTCODE varchar(5),@PRE_ACCESSNUMBER varchar(9),@PRE_TESTORDER int,@PRE_TESTNAME varchar(34),@PRE_TESTCREDATE varchar(8),@DATA_BASE varchar(3),@PRE_TESTSTATUS 
            // int,@LOGUSERID varchar(3),@LOGDATE datetime,@TEMP int,@RECEIVEDDATE datetime',@PRE_TESTCODE='A0024',@PRE_ACCESSNUMBER='180018436',@PRE_TESTORDER=0,@PRE_TESTNAME='50024^AFB culture  
            // AFB sensitivity',@PRE_TESTCREDATE='20200403',@DATA_BASE='ORM',@PRE_TESTSTATUS=10,@LOGUSERID='SYS',@LOGDATE='2020-04-03 10:20:09.393',@TEMP=0,@RECEIVEDDATE='2020-04-03 10:20:09.393'

            try
            {
                string format_NOW = "yyyy-MM-dd HH:mm:ss.fff";
                string format_NOW_YYYYMMDD = "yyyyMMdd";
                string strDateNow = DateTime.Now.ToString(format_NOW, cultureinfo);
                string strDateNow_YYYYMMDD = DateTime.Now.ToString(format_NOW_YYYYMMDD, cultureinfo);
                string sql_update_newcode = "";

                sql_update_newcode = @"exec sp_executesql N'INSERT INTO PRE_TESTS (PRE_TESTCODE, PRE_ACCESSNUMBER, PRE_TESTORDER, PRE_TESTNAME, PRE_TESTCREDATE, DATA_BASE, 
            PRE_TESTSTATUS, LOGUSERID, LOGDATE, TEMP, RECEIVEDDATE) 
            VALUES (@PRE_TESTCODE, @PRE_ACCESSNUMBER, @PRE_TESTORDER, @PRE_TESTNAME, 
            @PRE_TESTCREDATE, @DATA_BASE, @PRE_TESTSTATUS, @LOGUSERID, @LOGDATE, @TEMP, @RECEIVEDDATE) 
            ',N'@PRE_TESTCODE varchar(5),@PRE_ACCESSNUMBER varchar(9),@PRE_TESTORDER int,@PRE_TESTNAME varchar(34),@PRE_TESTCREDATE varchar(8),@DATA_BASE varchar(3),@PRE_TESTSTATUS 
            int,@LOGUSERID varchar(3),@LOGDATE datetime,@TEMP int,@RECEIVEDDATE datetime',@PRE_TESTCODE='" + StrNewcode + "',@PRE_ACCESSNUMBER='" + StrAccnum + "',@PRE_TESTORDER=0,@PRE_TESTNAME='" + StrTestTxt + "'" +
            ",@PRE_TESTCREDATE='" + strDateNow_YYYYMMDD + "',@DATA_BASE='ORM',@PRE_TESTSTATUS=10,@LOGUSERID='SYS',@LOGDATE='" + strDateNow + "',@TEMP=0,@RECEIVEDDATE='" + strDateNow + "'";

                Writedatalog.WriteLog("Update New_code---->" + "\r\n" + sql_update_newcode);
                SqlCommand cmd = new SqlCommand(sql_update_newcode, conn);
                //cmd.Parameters.Add("@COUNTER", SqlDbType.VarChar).Value = compture;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error :1003 Insert_Newcode in Database \r\n Detail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button_Save_Click(object sender, EventArgs e)
        {
            int IntCheck = 0;
            if (lblAccessnumber.Text != "")
            {
                for (int i = 0; i < treeList1.AllNodesCount; i++)
                {
                    if (treeList1.Nodes[i].CheckState == CheckState.Checked)
                    {
                        IntCheck++;
                    }
                }
            }
            if (IntCheck > 0)
            {
                Send_Process();
            }
            else
            {
                MessageBox.Show("Please select Source first");
            }
        }
        private void Send_Process()
        {
            string strSysDate = "";
            string Pat_number = lblHN.Text;
            string Accnum = lblAccessnumber.Text;
            string REQDOCTOR = lblDoctor.Text;
            string REQLOCATION = lblLocation.Text;
            string Protocal_LENGTH;
            string REQUESTS_PATID = "";
            string REQUESTS_TESTID = "";

            int Get_specimen_ID = 0;

            // Insert Table REQUESTS
            Insert_Into_REQUESTS_Step1(StrPATID_REQ, Accnum, lblRequested.Text,REQDOCTOR,REQLOCATION);

            //int Lcount = 2;
            if (treeList1.AllNodesCount > 0)
            {
                try
                {
                    if (lblAccessnumber.Text != "-" && lblAccessnumber.Text != "")
                    {
                        DialogResult yes = MessageBox.Show("Confirm", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (yes == DialogResult.Yes)
                        {
                            DateTime dt = DateTime.Now; // Or whatever
                            string format_NOW = "yyyyMMddHHmmss";
                            strSysDate = DateTime.Now.ToString(format_NOW, cultureinfo);

                            for (int i = 0; i < treeList1.AllNodesCount; i++)
                            {
                                //    int obxCount = 0;

                                string StrLabID = lblAccessnumber.Text;
                                string StrDoctor = lblDoctor.Text;
                                string StrLocation = lblLocation.Text;
                                string StrCode = treeList1.Nodes[i].GetValue(0).ToString();
                                string StrFulltext = treeList1.Nodes[i][1].ToString();
                                string StrComment = treeList1.Nodes[i].GetValue(3).ToString();
                                string StrProtocolCODE = treeList1.Nodes[i].GetValue(4).ToString();
                                string StrProtocolID = treeList1.Nodes[i].GetValue(9).ToString();
                                string StrCollected = strSysDate;                                           // DT Receive Specimen now
                                string StrPATID = treeList1.Nodes[i].GetValue(10).ToString();
                                string StrTESTID = treeList1.Nodes[i].GetValue(11).ToString();
                                string StrLOGUSERID = treeList1.Nodes[i].GetValue(14).ToString();           // Username in Receive Specimen
                                string StrSPTESTID = treeList1.Nodes[i].GetValue(16).ToString();
                                string StrSPECIMENCODE = treeList1.Nodes[i].GetValue(17).ToString();

                                if (StrSPECIMENCODE != "")
                                {
                                    Get_specimen_ID = Query_specimenID(StrSPECIMENCODE);
                                }

                                REQUESTS_PATID = StrPATID;
                                REQUESTS_TESTID = StrTESTID;

                                if (treeList1.Nodes[i].CheckState == CheckState.Checked)
                                {
                                    Update_SP_TEST(Accnum, lblDATE_time.Text, StrCode, StrSPTESTID);

                                    Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3000:Send_process Get Protocol ID For create nuber of Protocal : [" + StrProtocolID + "]");
                                    Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3001:Send_process Create Counter From PROTOCALID Equal         : [" + StrProtocolID + "]");

                                    // Check Protocal Number is exist?
                                    // true and false if Find PROTOCOLID in Table PROTOCOL_COUNTERS
                                    Boolean Chk_ProtocolID;
                                    Chk_ProtocolID = Query_PROTOCOL(StrProtocolID);
                                    // If Found Ptotocol in Table
                                    if (Chk_ProtocolID)
                                    {
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3002:Send_process Check protocol number is exist?              : [" + Chk_ProtocolID + "]");
                                        //GetProtocol_number(StrProtocolID);
                                        // Check Length of Protocol number
                                        Protocal_LENGTH = Chk_Length(StrProtocolID);
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3002.1:Send_process Check Protocal_LENGTH?                     : [" + Protocal_LENGTH + "]");
                                        // Get Number Protocal from PROTOCOLID
                                        int RUNNINGPROTOCOL = new Nullable<int>(GETNUMBERPROTOCOL(StrProtocolID)).GetValueOrDefault();
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
                                            Update_NEW_LENGTH(Lengcount, StrProtocolID);
                                            Writedatalog.WriteLog_Rectube(strDateTime + " Update Length in PROTOCOL_NUMBER : Protocol Length count = " + Lengcount + " and Old Protocol LENGTH = " + NowLengthProtocol);
                                        }
                                        // Update Protocol Number in DATABASE and General number
                                        Update_New_PROTOCOL_NUMBER(NEWPROTOCAL, StrProtocolID);
                                        string NewProtocol_num = StrProtocolCODE + NEWPROTOCAL;
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3002.1:Send_process Check RUNNINGPROTOCOL?                     : [" + RUNNINGPROTOCOL + "]");
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3002.2:Send_process Check PADLEFTZERO?                         : [" + PADLEFTZERO + "]");
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3002.3:Send_process Check NEWPROTOCAL?                         : [" + NEWPROTOCAL + "]");
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3002.4:Send_process Check Lengcount?                           : [" + Lengcount + "]");
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3002.5:Send_process Check NowLengthProtocol?                   : [" + NowLengthProtocol + "]");
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3002.5:Send_process Check NewProtocol_num?                     : [" + NewProtocol_num + "]");

                                        // Insert Table REQUESTS Step 2
                                        Insert_Into_REQUESTS_Step2(StrPATID_REQ, Accnum, lblRequested.ToString(), StrTESTID, Get_specimen_ID);
                                        // Step 1. Insert MB_REQUESTS with Protocal number
                                        // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN

                                        Insert_Into_MB_REQUESTS(StrLabID, StrProtocolID, NewProtocol_num, StrTESTID);

                                        // Writedatalog.WriteLog_Rectube(strDateTime + " Find Stain Protocol ID : [" + Stain_with_Protocol + "]");

                                    }
                                    // Check only Ptotocol counter.
                                    // No Protocol Counter in Database Table = PROCOL_COUNTERS.
                                    else
                                    {
                                        int Protocal_LENGTH_First = Query_Protocol_Start();
                                        int RUNNINGPROTOCOL = new Nullable<int>(GETNUMBERPROTOCOL(StrProtocolID)).GetValueOrDefault();
                                        int PADLEFTZERO = Convert.ToInt32(Protocal_LENGTH_First) - (Convert.ToInt32(RUNNINGPROTOCOL.ToString().Length));
                                        string NEWPROTOCAL = RUNNINGPROTOCOL.ToString().PadLeft(PADLEFTZERO + 1, '0');
                                        // Update Protocol Number in DATABASE and General number
                                        Update_New_PROTOCOL_NUMBER(NEWPROTOCAL, StrProtocolID);
                                        string NewProtocol_num = StrProtocolCODE + NEWPROTOCAL;

                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3003:Send_process Check protocol number is exist?              : [" + "False" + "]");
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3003.1:Send_process Check Protocal_LENGTH_First ?              : [" + Protocal_LENGTH_First + "]");
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3003.2:Send_process RUNNINGPROTOCOL                            : [" + RUNNINGPROTOCOL + "]");
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3003.3:Send_process PADLEFTZERO                                : [" + PADLEFTZERO + "]");
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3003.4:Send_process NEWPROTOCAL                                : [" + NEWPROTOCAL + "]");
                                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L3003.4:Send_process NewProtocol_num                            : [" + NewProtocol_num + "]");

                                        // Insert Table REQUESTS Step 2
                                        Insert_Into_REQUESTS_Step2(StrPATID_REQ, Accnum, lblRequested.ToString(), StrTESTID, Get_specimen_ID);
                                        // Step 1. Insert MB_REQUESTS with Protocal number
                                        // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                                        Insert_Into_MB_REQUESTS(StrLabID, StrProtocolID, NewProtocol_num, StrTESTID);
                                    }
                                }
                            }
                        }
                    }

                    Loop_After_Save(Accnum);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error :1016 Please Contact to Administrator -- Error Can't Save files into Production server! \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }
        }
        private void Loop_After_Save(string Accnumber_query)
        {
            string sql_After_Save = "";
            try
            {
                conn = new Connection_ORM().Connect();
                sql_After_Save = @"SELECT 
SP_TESTS.SP_ACCESSNUMBER AS SP_TESTS_ACCNUM
,SP_TESTS.SP_TESTCODE
,SP_TESTS.SP_STNCODE
,SP_REQUESTS.HOSTORDERNUMBER
,SP_REQUESTS.REQDATE
,SP_REQUESTS.SP_DOCCODE
,SP_REQUESTS.SP_LOCCODE
,SP_TESTS.SP_TESTNAME
,SP_TESTS.SP_TESTCODE
,SP_TESTS.SP_SPECIMENCODE
,SP_TESTS.SP_SPECIMENNAME
,DICT_MB_PROTOCOLS.PROTOCOLTEXT
,PATID,PATNUMBER,NAME,LASTNAME
,ADDRESS1,ADDRESS2,CITY,STATE,POSTALCODE,COUNTRY
,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as AGEY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as AGEM,PATIENTS.BIRTHDATE,SEX,TELEPHON
,TELEPHON2,FAX,EMAIL,INCOMMINGDATE,PATIENTS.ROOMNUMBER,REFDOCTOR,REFLOCATION,LASTUPDTESTDATE
,BG_ABO,BG_RHESUS,BG_PHENOTYPES,BG_KELL,DEATHDATE
,SP_REQUESTS.SECRETRESULT
,VIP
,DOCID,PATCREATIONDATE,STARTVALIDDATE,CREATEBY,COMMENT
,SP_TESTS.SP_TESTSTATUS AS SP_TESTSTATUS
FROM PATIENTS
LEFT OUTER JOIN SP_REQUESTS ON PATIENTS.PATNUMBER = SP_REQUESTS.PATNUM
LEFT OUTER JOIN SP_TESTS ON SP_REQUESTS.SP_ACCESSNUMBER = SP_TESTS.SP_ACCESSNUMBER
LEFT OUTER JOIN DICT_MB_PROTOCOLS ON  SP_TESTS.SP_STNCODE = DICT_MB_PROTOCOLS.PROTOCOLCODE
WHERE SP_REQUESTS.SP_ACCESSNUMBER ='" + Accnumber_query + "'  AND SP_TESTS.SP_TESTSTATUS = 0";

                SqlCommand cmd = new SqlCommand(sql_After_Save, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "result_loop");
                SqlDataReader reader = cmd.ExecuteReader();

                if (ds.Tables["result_loop"].Rows.Count > 0)
                {
                    Query_request_ORM(Accnumber_query);
                }
                else
                {
                    // Update SP_REQUEST in TRANSMISSIONSTATUS
                    clear_information();
                    Update_SP_REQUESTS(Accnumber_query);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1015 Loop After save \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                MessageBox.Show("Error :1010 Update Ptococol NewNUMBER \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void Insert_Into_REQUESTS_Step1(string StrPATID, string StrLabID, string StrREQDATE, string REQDOCTOR, string REQLOCATION)
        {
            string StrREQSTATUS = Query_REQSTATUS_REQUESTS(StrLabID);
            int requestID;
            try
            {
                if (StrREQSTATUS != "10")
                {
                    string REQ_createtionDate = DateTime.Now.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));
                    string sql_Insert = @"INSERT INTO REQUESTS (PATID, ACCESSNUMBER, REQCREATIONDATE,SPECIMENCODE, REQSTATUS, REQ_IPDOROPD, STATUSDATE, REQDATE, COLLECTIONDATE, URGENT, COMMENT, SECRETRESULT , LASTUPDATE, RECEIVEDDATE, REQDOCTOR, REQLOCATION, EXTERNALORDERNUMBER, LOGUSERID, LOGDATE) 
                                         VALUES ('" + StrPATID + "','" + StrLabID + "','" + REQ_createtionDate + "','" + label_specimen_req.Text + "','" + "10" + "','" + Str_Req_IPDOPD + "','" + lblDATE_time.Text + "','" + StrREQDATE + "','" + strDateTime + "','" +
                                         Str_Req_Urgent + "','" + richTextBox_OrderCOMMENT.Text + "','" + StrSecret + "','" + strDateTime + "','" + strDateTime + "','" + REQDOCTOR + "','" + REQLOCATION + "','" + "" + "','" + "RECTUBE" + "','" + strDateTime + "'); SELECT SCOPE_IDENTITY()";
                    Writedatalog.WriteLog_Rectube(strDateTime + "Request  " + sql_Insert);
                    SqlCommand cmd = new SqlCommand(sql_Insert, conn);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    requestID = Convert.ToInt32(cmd.ExecuteScalar());
                    Writedatalog.WriteLog_Rectube(strDateTime + " Insert REQUESTS ==================================== " + requestID + "   --> Request Datetime " + StrREQDATE + sql_Insert);
                }
                else if (StrREQSTATUS == "10")
                {
                    // Do Notthing
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1005 Insert Data to REQUESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Insert_Into_REQUESTS_Step2(string StrPATID, string StrLabID, string StrREQDATE, string StrTESTID, int StrSpecimenID)
        {
            string StrREQSTATUS = Query_REQSTATUS_REQUESTS(StrLabID);
            try
            {
                if (StrREQSTATUS != "10")
                {
                    // Do Notthing
                }
                else if (StrREQSTATUS == "10")
                {
                    string StrREQUESTID = Query_REQUESTID_REQUESTS(StrLabID);
                    Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L5001  :Insert_Into_REQUESTS_Step2 Query_REQUESTID_REQUESTS       : [" + StrREQUESTID + "]");
                    // Insert REQ_TESTS
                    Insert_Into_REQ_TESTS(StrREQUESTID, StrTESTID, StrSpecimenID);
                    Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L5001.1:Insert_Into_REQUESTS_Step2 Insert_Into_REQ_TESTS          : [" + StrREQUESTID + " " + StrTESTID + "]");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1011 Insert Data to REQUESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Insert_Into_REQ_TESTS(string StrREQUESTID, string REQUESTS_TESTID, int StrSpecimenID)
        {
            try
            {
                string sql_REQ_TESTS = @"INSERT INTO REQ_TESTS (TESTID,REQUESTID,COLLMATERIALID,LASTUPDATE,LOGUSERID,LOGDATE) VALUES ('" + REQUESTS_TESTID + "','" + StrREQUESTID + "','" + StrSpecimenID + "','" + strDateTime + "','" + StrUserlogin + "','" + strDateTime + "')";

                SqlCommand cmd = new SqlCommand(sql_REQ_TESTS, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();

                Writedatalog.WriteLog_Rectube(strDateTime + " Insert REQ_TESTS ==================================== " + sql_REQ_TESTS);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Insert_Into_REQ_TESTS ! \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }
        private void Insert_Into_MB_REQUESTS(string StrLabID, string StrProtocolID, string NewProtocol_num, string StrTESTID)
        {
            int MB_REQUESTID;
            //Boolean Req_specimen_status = true;
            try
            {
                //Query_MB_REQUESTS_check(StrLabID);
                // Step 1. Insert MB_REQUESTS with Protocal number
                string StrREQUESTID = Query_REQUESTID_REQUESTS(StrLabID);
                string sql_Insert_MB_REQUESTS = @"INSERT INTO MB_REQUESTS (PROTOCOLID,TESTID,REQUESTID,MBREQNUMBER,SUBREQUESTCREDATE,COLLECTIONDATE,RECEIVEDDATE,LOGUSERID,LOGDATE,MBREQEUSTSTATUS)
                        VALUES ('" + StrProtocolID + "','" + StrTESTID + "','" + StrREQUESTID + "','" + NewProtocol_num + "','" + strDateTime + "','" + strDateTime + "','" + strDateTime + "','" + "SYS" + "','" + strDateTime + "','" + "1" + "'); SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(sql_Insert_MB_REQUESTS, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                MB_REQUESTID = Convert.ToInt32(cmd.ExecuteScalar());
                Writedatalog.WriteLog_Rectube(strDateTime + " Insert MB_REQUESTS ==================================== " + sql_Insert_MB_REQUESTS);
                // Insert Stain if Protocol found Stain in Dababase
                // Step 2. Table SUBREQMB_STAINS insert if Protocol with STAIN
                Query_Stain_Protocol_and_Insert(MB_REQUESTID, StrProtocolID);
                // Step Insert to MB_ACTIONS for Status
                Insert_INTO_MB_ACTIONS(MB_REQUESTID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1014.1 Insert_Into_MB_REQUESTS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private int GETNUMBERPROTOCOL(string StrProtocolID)
        {
            Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L4001  :GETNUMBERPROTOCOL GETNUMBERPROTOCOL                       : [" + StrProtocolID + "]");
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
                    Writedatalog.WriteLog_Rectube(strDateTime + "Log:L4001.1:GETNUMBERPROTOCOL COMPTURE                  : [" + ds.Tables["COMPTURE"].Rows.Count + "]");
                    Writedatalog.WriteLog_Rectube(strDateTime + "Log:L4001.2:GETNUMBERPROTOCOL COUNTPROTOCOL + 1         : [" + COUNTPROTOCOL + "]");
                }
                else
                {
                    LENGTH_FIRST = Query_Protocol_Start();

                    Writedatalog.WriteLog_Rectube(strDateTime + "#########################   " + "[" + LENGTH_FIRST + "]");
                    // change 
                    if (LENGTH_FIRST > 0)
                    {
                        string sql_Insert_NEW_PROID = @"INSERT INTO PROTOCOL_COUNTERS(PROTOCOLID, COUNTERNUMBER, LENGTH, CREATEDATE, LOGUSER, LOGDATE)
                                                        VALUES('" + StrProtocolID + "','" + "00000001" + "','" + LENGTH_FIRST + "','" + strDateTime + "','" + "SYS" + "','" + strDateTime + "')";
                        Writedatalog.WriteLog_Rectube(strDateTime + "  Log:L4001.3:Send_process NewProtocol_num" +
                                                                      ":" + sql_Insert_NEW_PROID);
                        SqlCommand cmd_insert_PROTOCOL_COUNTER = new SqlCommand(sql_Insert_NEW_PROID, conn);
                        if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                        cmd_insert_PROTOCOL_COUNTER.ExecuteNonQuery();
                        COUNTPROTOCOL = 1;
                    }
                    else
                    {
                        MessageBox.Show("Error : 1009.1 Please Contact to Administrator -- Error Not Found PROTOCOL NUMBER IN CONFIG ! \r\nDetail : ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1009 Query Get Number of Protocol ! \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            return COUNTPROTOCOL;

        }
        private string Query_REQUESTID_REQUESTS(string StrLabID)
        {
            string result = "";
            try
            {
                string sql_RequestID = @"SELECT REQUESTID FROM REQUESTS WHERE ACCESSNUMBER='" + StrLabID + "'";
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
        private Boolean Query_PROTOCOL(string StrProtocalID)
        {
            Boolean result_ID = true;
            try
            {
                string sql_PorotocolID = @"SELECT COUNTERID,PROTOCOLID,COUNTERNUMBER,LENGTH,CREATEDATE,LOGUSER,LOGDATE FROM PROTOCOL_COUNTERS WHERE PROTOCOLID ='" + StrProtocalID + "'";
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
                    result_ID = true;
                }
                else
                {
                    result_ID = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1007 Query_PROTOCOL \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result_ID;
        }
        private string Query_REQSTATUS_REQUESTS(string StrLabID)
        {
            string result = "";
            try
            {
                string sql_Query_REQSTATUS = @"SELECT REQSTATUS FROM REQUESTS WHERE ACCESSNUMBER='" + StrLabID + "'";

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
        private void Query_Stain_Protocol_and_Insert(int MB_REQUESTID, string StrProtocolID)
        {
            try
            {
                string sql_Query_Stain = @"SELECT DICT_MB_PROTOCOLS.PROTOCOLID,DICT_MB_PROTOCOLS.PROTOCOLCODE 
,DICT_MB_STAINS.MBSTAINID,MBSTAINCODE,DICT_MB_STAINS.STAINNAME    
FROM DICT_MB_PROTOCOLS 
LEFT OUTER JOIN  DICT_MB_PROTOCOL_STAINS ON DICT_MB_PROTOCOLS.PROTOCOLID = DICT_MB_PROTOCOL_STAINS.PROTOCOLID
LEFT OUTER JOIN  DICT_MB_STAINS ON DICT_MB_PROTOCOL_STAINS.MBSTAINID = DICT_MB_STAINS.MBSTAINID
WHERE DICT_MB_PROTOCOLS.PROTOCOLID = '" + StrProtocolID + "'";

                Writedatalog.WriteLog_Rectube(strDateTime + " Query in " + "[" + StrProtocolID + "] STAINS ==================================== " + sql_Query_Stain);

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
                                                    VALUES ('" + Str_StrainID + "','" + MB_REQUESTID + "','" + "SYS" + "','" + strDateTime + "','" + "SYS" + "','" + strDateTime + "' )";
                            Writedatalog.WriteLog_Rectube(strDateTime + " Insert Str_StrainID SUBREQMB_STAINS ==================================== " + sql_stain);

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
        private void Insert_INTO_MB_ACTIONS(int MB_REQUESTID)
        {
            try
            {
                            string sql = @"INSERT INTO MB_ACTIONS (SUBREQUESTID,ACTIONTYPE,ACTIONDATE,ACTIONUSER,LOGUSERID,LOGDATE) 
                                         VALUES ('" + MB_REQUESTID + "','" + "0" + "','" + strDateTime + "','" + "RECTUBE" + "','" + "SYS" + "','" + strDateTime + "' )";
                            SqlCommand cmd = new SqlCommand(sql, conn);
                            SqlDataAdapter ada = new SqlDataAdapter(cmd);
                            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                            cmd.ExecuteNonQuery();

                            Writedatalog.WriteLog_Rectube(strDateTime + " Insert MB_ACTIONS ==================================== " + sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1014.0 Insert MB_ACTIONS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
        private string Chk_Length(string StrProtocolID)
        {
            string result = "";
            try
            {
                string sql_LENGTH = @"SELECT LENGTH FROM PROTOCOL_COUNTERS WHERE PROTOCOLID='" + StrProtocolID + "' ";
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
                    result = ds.Tables[0].Rows[0]["LENGTH"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :1008 Chk_Length \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        private void Update_SP_TEST(string ACCESSNUMBER, string RECEIVEDDATE, string TESTCODE, string StrTESTID)
        {
            // 4. Update status of CODE and Transection into PRE_TESTS 
            // exec sp_executesql N'UPDATE PRE_TESTS SET PRE_TESTSTATUS = 10, RECEIVEDDATE=@RECEIVEDDATE WHERE PRE_TESTS.PRE_ACCESSNUMBER = @ACCESSNUMBER AND PRE_TESTS.PRE_TESTCODE = @TESTCODE  ',N'@ACCESSNUMBER varchar(9),@TESTCODE varchar(5),@RECEIVEDDATE datetime',@ACCESSNUMBER='180018436',@TESTCODE='50024',@RECEIVEDDATE='2020-04-03 09:19:27.923'"
            try
            {
                string format_NOW = "yyyy-MM-dd HH:mm:ss.fff";
                string strDateNow = DateTime.Now.ToString(format_NOW, cultureinfo);

                conn = new Connection_ORM().Connect();
                string sql_Update_SP_TESTS = @"UPDATE SP_TESTS set SP_TESTSTATUS = '" + 10 + "' ,SP_RECEIVEDDATE = '" + RECEIVEDDATE +
                             "' WHERE SP_TESTS.SP_ACCESSNUMBER = '" + ACCESSNUMBER + "' AND SP_TESTS.SP_TESTSID ='" + StrTESTID + "' ";
                SqlCommand cmd = new SqlCommand(sql_Update_SP_TESTS, conn);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                Writedatalog.WriteLog_ORM("Start Update --> " + "\r\n" + sql_Update_SP_TESTS);

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error :1006 Update SP_TESTS in Database \r\n Detail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Update_SP_REQUESTS(string ACCESSNUMBER)
        {
            try
            {
                string format_NOW = "yyyy-MM-dd HH:mm:ss.fff";
                string strDateNow = DateTime.Now.ToString(format_NOW, cultureinfo);

                conn = new Connection_ORM().Connect();
                string sql_Update_SP_REQUESTS = @"UPDATE SP_REQUESTS set TRANSMISSIONSTATUS = '" + 10 + "' WHERE SP_REQUESTS.SP_ACCESSNUMBER = '" + ACCESSNUMBER + "' ";
                SqlCommand cmd = new SqlCommand(sql_Update_SP_REQUESTS, conn);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error :1416 Update SP_REQUESTS in Database \r\n Detail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void searchDataInformations()
        {
            if (txtAccessNumber.Text != "")
            {
                // clearText();
                // ACCESSNUMBER
                if (comboBox1.SelectedIndex == 0)
                {
                    txtAccessNumber.MaxLength = Length_LN;
                    //queryData();
                    string Accnumber_query = txtAccessNumber.Text;
                    queryPatients(Accnumber_query);
                }
                // PATIENT NUMBER
                else if (comboBox1.SelectedIndex == 1)
                {
                    //txtAccessNumber.MaxLength = HNlength;
                    //Form_PatientList fm = new Form_PatientList(txtAccessNumber, lblAccessFrmPatientList);
                    //fm.Show();
                }
                // SAMPLE NUMBER
                else if (comboBox1.SelectedIndex == 2)
                {
                    //lblCheckProtocolNumber.Text = txtAccessNumber.Text;
                    //queryData();
                    string Accnumber_query_SID = "";
                    queryPatients(Accnumber_query_SID);
                }
                else
                {
                    MessageBox.Show("Please select Source first");
                }
            }
        }
        public static DialogResult InputBoxClose(string title)
        {
            // New Form
            Form form = new Form();
            text_inputClose = new TextBox();
            text_inputClose.TextAlign = HorizontalAlignment.Center;
            text_inputClose.MaxLength = 20;
            text_inputClose.PasswordChar = '*';

            form.Text = title;

            Button buttonOk = new Button();
            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.SetBounds(150, 100, 100, 30);
            buttonOk.Font = new Font("Tahoma", 12, FontStyle.Regular);

            Button buttonCancel = new Button();
            buttonCancel.Text = "Close";
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.SetBounds(259, 100, 100, 30);
            buttonCancel.Font = new Font("Tahoma", 12, FontStyle.Regular);

            Label label = new Label();
            label.Text = title;
            label.SetBounds(9, 20, 372, 13);
            label.Font = new Font("Tahoma", 14, FontStyle.Regular);

            text_inputClose.SetBounds(12, 50, 372, 40);
            text_inputClose.Font = new Font("Tahoma", 14, FontStyle.Regular);

            label.AutoSize = true;
            text_inputClose.Anchor = text_inputClose.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 150);
            form.Controls.AddRange(new Control[] { label, text_inputClose, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            return dialogResult;
        }
        private void button_AdminMODE_Click(object sender, EventArgs e)
        {
            // lock
            Boolean status_lock = true;
            while (status_lock)
            {
                if (InputBoxClose("Input password for Admin MODE Diagnostic!") == DialogResult.OK)
                {
                    // get password from text
                    string input_passwd = text_inputClose.Text;
                    try
                    {
                        // Encrypt password
                        input_passwd = AES_128.Encrypt(input_passwd);
                        string passwd_text = Passunlock;

                        // check passwd equals
                        if (input_passwd.Equals(passwd_text))
                        {
                            panel_Diag.Visible = true;
                            panel_Diag.Location = new Point(0, 0);

                            status_lock = false;
                        }
                        else
                        {
                            MessageBox.Show("Bad password", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            status_lock = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    status_lock = false;
                }
            }
        }
        private void button_Diag_exit_Click(object sender, EventArgs e)
        {
            panel_Diag.Visible = false;
            panel_Diag.Location = new Point(1174, 8);
        }
        private void Diag_Query_ORMLOG()
        {
            string sql_ORM_LOG = "";
            try
            {
                conn = new Connection_ORM().Connect();
                sql_ORM_LOG = @"SELECT SP_ACCESSNUMBER,PATNUM,REQDATE,SP_REQUESTS.LOGDATE,USERFIELD1 
,PATIENTS.NAME,PATIENTS.LASTNAME
FROM SP_REQUESTS
LEFT OUTER JOIN PATIENTS ON PATIENTS.PATNUMBER = SP_REQUESTS.PATNUM 
where SP_ACCESSNUMBER='" + textBox_Diag_accessnum.Text + "' order by REQDATE";

                SqlCommand cmd = new SqlCommand(sql_ORM_LOG, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "result_diag");
                SqlDataReader reader = cmd.ExecuteReader();
                if (ds.Tables["result_diag"].Rows.Count > 0)
                {
                    ds_result = ds;

                    for (int i = 0; i < ds.Tables["result_diag"].Rows.Count; i++)
                    {
                        dataGridView_Diag_ORMLOG.Rows.Add();
                        dataGridView_Diag_ORMLOG.Rows[i].Cells[0].Value = (i + 1);
                        dataGridView_Diag_ORMLOG.Rows[i].Cells[1].Value = (ds.Tables["result_diag"].Rows[i]["SP_ACCESSNUMBER"]).ToString();
                        dataGridView_Diag_ORMLOG.Rows[i].Cells[2].Value = (ds.Tables["result_diag"].Rows[i]["PATNUM"]).ToString();
                        dataGridView_Diag_ORMLOG.Rows[i].Cells[3].Value = (ds.Tables["result_diag"].Rows[i]["NAME"]).ToString();
                        dataGridView_Diag_ORMLOG.Rows[i].Cells[4].Value = (ds.Tables["result_diag"].Rows[i]["LASTNAME"]).ToString();
                        dataGridView_Diag_ORMLOG.Rows[i].Cells[5].Value = (ds.Tables["result_diag"].Rows[i]["REQDATE"]).ToString();
                        dataGridView_Diag_ORMLOG.Rows[i].Cells[6].Value = (ds.Tables["result_diag"].Rows[i]["USERFIELD1"]).ToString();
                        dataGridView_Diag_ORMLOG.Rows[i].Cells[7].Value = (ds.Tables["result_diag"].Rows[i]["LOGDATE"]).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Query Diag \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Diag_Query_REQUEST()
        {
            try
            {
                string sql_Diag_QueryRequest = "";
                conn = new Connection_ORM().Connect();
                sql_Diag_QueryRequest = @"SELECT SP_TESTS.SP_ACCESSNUMBER,PATNUM,REQDATE,SP_REQUESTS.LOGDATE,USERFIELD1 
,PATIENTS.NAME,PATIENTS.LASTNAME,SP_REQUESTS.PATNUM
,SP_REQUESTS.SP_DOCCODE,SP_REQUESTS.SP_LOCCODE,SP_REQUESTS.COLLECTIONDATE
,SP_REQUESTS.HOSTORDERNUMBER,SP_REQUESTS.TRANSMISSIONSTATUS
,SP_TESTS.SP_TESTCODE,SP_TESTS.SP_TESTNAME,SP_TESTS.SP_SPECIMENCODE,SP_TESTS.SP_SPECIMENNAME
,SP_TESTS.SP_STNCODE,SP_TESTS.SP_RECEIVEDDATE
FROM SP_REQUESTS
LEFT OUTER JOIN SP_TESTS ON SP_TESTS.SP_ACCESSNUMBER = SP_REQUESTS.SP_ACCESSNUMBER
LEFT OUTER JOIN PATIENTS ON PATIENTS.PATNUMBER = SP_REQUESTS.PATNUM  
WHERE SP_TESTS.SP_ACCESSNUMBER ='" + textBox_Diag_accessnum.Text + "'AND SP_REQUESTS.TRANSMISSIONSTATUS=" + Querydiag_status + "";

                Writedatalog.WriteLog_ORM("Query Diag" + "\r\n" + sql_Diag_QueryRequest);
                SqlCommand cmd = new SqlCommand(sql_Diag_QueryRequest, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "result_diag");
                SqlDataReader reader = cmd.ExecuteReader();
                if (ds.Tables["result_diag"].Rows.Count > 0)
                {
                    ds_result = ds;

                    for (int i = 0; i < ds.Tables["result_diag"].Rows.Count; i++)
                    {
                        dataGridView_Diag_Request.Rows.Add();
                        dataGridView_Diag_Request.Rows[i].Cells[0].Value = (i + 1);
                        dataGridView_Diag_Request.Rows[i].Cells[1].Value = (ds.Tables["result_diag"].Rows[i]["PATNUM"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[2].Value = (ds.Tables["result_diag"].Rows[i]["USERFIELD1"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[3].Value = (ds.Tables["result_diag"].Rows[i]["SP_LOCCODE"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[4].Value = (ds.Tables["result_diag"].Rows[i]["SP_DOCCODE"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[5].Value = (ds.Tables["result_diag"].Rows[i]["REQDATE"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[6].Value = (ds.Tables["result_diag"].Rows[i]["COLLECTIONDATE"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[7].Value = (ds.Tables["result_diag"].Rows[i]["HOSTORDERNUMBER"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[8].Value = (ds.Tables["result_diag"].Rows[i]["TRANSMISSIONSTATUS"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[9].Value = (ds.Tables["result_diag"].Rows[i]["SP_STNCODE"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[10].Value = (ds.Tables["result_diag"].Rows[i]["SP_ACCESSNUMBER"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[11].Value = (ds.Tables["result_diag"].Rows[i]["SP_TESTCODE"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[12].Value = (ds.Tables["result_diag"].Rows[i]["SP_TESTNAME"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[13].Value = (ds.Tables["result_diag"].Rows[i]["SP_SPECIMENCODE"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[14].Value = (ds.Tables["result_diag"].Rows[i]["SP_SPECIMENNAME"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[15].Value = (ds.Tables["result_diag"].Rows[i]["SP_RECEIVEDDATE"]).ToString();
                        dataGridView_Diag_Request.Rows[i].Cells[16].Value = (ds.Tables["result_diag"].Rows[i]["COLLECTIONDATE"]).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Query Diag \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button_Diag_query_Click(object sender, EventArgs e)
        {
            if (textBox_Diag_accessnum.Text != "")
            {
                dataGridView_Diag_Request.Rows.Clear();
                dataGridView_Diag_ORMLOG.Rows.Clear();
                Diag_Query_ORMLOG();
                Diag_Query_REQUEST();
            }
            else
            {
                MessageBox.Show("Please fill? \r\nDetail :Blank in :Accessnumber?? ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void comboBox_Diag_status_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox_Diag_status.SelectedIndex == 0)
            {
                Querydiag_status = 10;
                label23.Text = Querydiag_status.ToString();
            }
            // PATIENT NUMBER
            else if (comboBox_Diag_status.SelectedIndex == 1)
            {
                Querydiag_status = 0;
                label23.Text = Querydiag_status.ToString();
            }
        }
        private void dataGridView_Diag_ORMLOG_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Diag_select_MSH();
        }
        private void Diag_select_MSH()
        {
            try
            {
                string Diag_MSH = "";

                Int32 selectedRowCount = dataGridView_Diag_ORMLOG.Rows.GetRowCount(DataGridViewElementStates.Selected);

                if (selectedRowCount > 0)
                {
                    foreach (DataGridViewRow row in dataGridView_Diag_ORMLOG.SelectedRows)
                    {
                        Diag_MSH = row.Cells[6].Value.ToString();
                        richTextBox_Diag.Text = Diag_MSH;
                    }
                }
            }
            catch (SqlException ex) { MessageBox.Show(ex.Message); }
        }
        private void button_Diag_Clear_Click(object sender, EventArgs e)
        {
            textBox_Diag_accessnum.Text = "";
            richTextBox_Diag.Text = "";
            dataGridView_Diag_Request.Rows.Clear();
            dataGridView_Diag_ORMLOG.Rows.Clear();
        }
        private void teamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Reqtube_About fm = new Form_Reqtube_About();
            fm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form_Worksheet_printing fm_worksheet = new Form_Worksheet_printing();
            fm_worksheet.Show();
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void label_Rectube_Urgent_Click(object sender, EventArgs e)
        {

        }

        private void tabPatInfo_Click(object sender, EventArgs e)
        {

        }
    }
}
