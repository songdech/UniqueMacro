using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using UniquePro.Common;
using UniquePro.Entities.Common;
using UniquePro.Entities.OEM;
using UniquePro.Entities.Patient;
using UniquePro.Controller;
using UNIQUE.Result;


namespace UNIQUE.Forms.Dashboard
{
    public partial class Form_DashboardPR_Routine : Form
    {
        System.Globalization.CultureInfo cultureinfo_ENG = new System.Globalization.CultureInfo("en-US");

        private static string Version = "Prelimnary Result";
        ConnectString objConstr = new ConnectString();
        private String Constr = "";
        TreeListNode childNode;

        PatientController objPatient;
        OEMController objOrderEntry;
        PatientM objPatientM = null;

        public Form_DashboardPR_Routine()
        {
            InitializeComponent();
            objPatient = new PatientController();
            objOrderEntry = new OEMController();
        }

        private void Form_DashboardPR_Routine_Load(object sender, EventArgs e)
        {
            this.Text = Version;
            try
            {
                Constr = objConstr.Connectionstring();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            SearchData_REQUEST_Status();
        }

        private void CreateColums_REQUESTS_Routine(TreeList tl, DataSet ds)
        {
            //  CreateColumns
            // 0 = Patnumber
            // 1 = Accessnumber 
            // 2 = PATID
            // 3 = NAME 
            // 4 = LASTNAME 
            // 5 = REQUEST DATE 
            // 6 = RECEIVE DATE 

            // Create three columns.
            tl.BeginUpdate();
            tl.Columns.Add();

            tl.Columns[0].Caption = "<i><b>Patnumber</i></b>";
            tl.Columns[0].VisibleIndex = 0;
            tl.Columns[0].Width = 180;
            tl.Columns[0].Visible = true;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.RowHeight = 23;
            tl.Columns.Add();

            tl.Columns[1].Caption = "<b>Accessnumber</b>";
            tl.Columns[1].VisibleIndex = 1;
            tl.Columns[1].Width = 180;
            tl.Columns[1].Visible = true;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.Columns.Add();

            tl.Columns[2].Caption = "PATID";
            tl.Columns[2].VisibleIndex = 2;
            tl.Columns[2].Width = 10;
            tl.Columns[2].Visible = false;
            tl.Columns.Add();

            tl.Columns[3].Caption = "NAME";
            tl.Columns[3].VisibleIndex = 3;
            tl.Columns[3].Width = 180;
            tl.Columns[3].Visible = true;
            tl.Columns.Add();

            tl.Columns[4].Caption = "LASTNAME";
            tl.Columns[4].VisibleIndex = 4;
            tl.Columns[4].Width = 180;
            tl.Columns[4].Visible = true;
            tl.Columns.Add();

            tl.Columns[5].Caption = "Request Date";
            tl.Columns[5].VisibleIndex = 5;
            tl.Columns[5].Width = 180;
            tl.Columns[5].Visible = true;
            tl.Columns.Add();

            tl.Columns[6].Caption = "Receive Date";
            tl.Columns[6].VisibleIndex = 6;
            tl.Columns[6].Width = 180;
            tl.Columns[6].Visible = true;
            tl.Columns.Add();


            tl.EndUpdate();
        }
        private void CreateColums_MB_REQUESTS_Routine(TreeList tl, DataSet ds)
        {
            //  CreateColums_SP_TESTS_RC_Routine
            // 0 = Protocol Number
            // 1 = Protocol Text
            // 2 = Specimen name
            // 3 = Specimen code
            // 4 = Protocol Station

            // Create three columns.
            tl.BeginUpdate();
            tl.Columns.Add();

            tl.Columns[0].Caption = "<i><b>Protocol Number</i></b>";
            tl.Columns[0].VisibleIndex = 0;
            tl.Columns[0].Width = 180;
            tl.Columns[0].Visible = true;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.RowHeight = 23;
            tl.Columns.Add();

            tl.Columns[1].Caption = "<b>Text</b>";
            tl.Columns[1].VisibleIndex = 1;
            tl.Columns[1].Width = 180;
            tl.Columns[1].Visible = true;
            tl.OptionsView.AllowHtmlDrawHeaders = true;
            tl.Columns.Add();

            tl.Columns[2].Caption = "Test code";
            tl.Columns[2].VisibleIndex = 2;
            tl.Columns[2].Width = 190;
            tl.Columns[2].Visible = true;
            tl.Columns.Add();

            tl.Columns[3].Caption = "URGENT";
            tl.Columns[3].VisibleIndex = 3;
            tl.Columns[3].Width = 10;
            tl.Columns[3].Visible = false;
            tl.Columns.Add();

            tl.Columns[4].Caption = "REQDOCTOR";
            tl.Columns[4].VisibleIndex = 4;
            tl.Columns[4].Width = 10;
            tl.Columns[4].Visible = false;
            tl.Columns.Add();

            tl.Columns[5].Caption = "REQLOCATION";
            tl.Columns[5].VisibleIndex = 5;
            tl.Columns[5].Width = 10;
            tl.Columns[5].Visible = false;
            tl.Columns.Add();

            tl.Columns[6].Caption = "COMMENT";
            tl.Columns[6].VisibleIndex = 6;
            tl.Columns[6].Width = 10;
            tl.Columns[6].Visible = false;
            tl.Columns.Add();

            tl.Columns[7].Caption = "REQDATE";
            tl.Columns[7].VisibleIndex = 7;
            tl.Columns[7].Width = 10;
            tl.Columns[7].Visible = false;
            tl.Columns.Add();

            tl.Columns[8].Caption = "RECEIVEDDATE";
            tl.Columns[8].VisibleIndex = 8;
            tl.Columns[8].Width = 10;
            tl.Columns[8].Visible = false;
            tl.Columns.Add();

            tl.Columns[9].Caption = "PATNUMBER";
            tl.Columns[9].VisibleIndex = 9;
            tl.Columns[9].Width = 10;
            tl.Columns[9].Visible = false;
            tl.Columns.Add();

            tl.Columns[10].Caption = "NAME";
            tl.Columns[10].VisibleIndex = 10;
            tl.Columns[10].Width = 10;
            tl.Columns[10].Visible = false;
            tl.Columns.Add();

            tl.Columns[11].Caption = "LASTNAME";
            tl.Columns[11].VisibleIndex = 11;
            tl.Columns[11].Width = 10;
            tl.Columns[11].Visible = false;
            tl.Columns.Add();

            tl.Columns[12].Caption = "BIRTHDATE";
            tl.Columns[12].VisibleIndex = 12;
            tl.Columns[12].Width = 10;
            tl.Columns[12].Visible = false;
            tl.Columns.Add();

            tl.Columns[13].Caption = "SEX";
            tl.Columns[13].VisibleIndex = 13;
            tl.Columns[13].Width = 10;
            tl.Columns[13].Visible = false;
            tl.Columns.Add();

            tl.Columns[14].Caption = "AGEY";
            tl.Columns[14].VisibleIndex = 14;
            tl.Columns[14].Width = 10;
            tl.Columns[14].Visible = false;
            tl.Columns.Add();

            tl.Columns[15].Caption = "AGEM";
            tl.Columns[15].VisibleIndex = 15;
            tl.Columns[15].Width = 10;
            tl.Columns[15].Visible = false;
            tl.Columns.Add();

            tl.Columns[16].Caption = "DOCNAME";
            tl.Columns[16].VisibleIndex = 16;
            tl.Columns[16].Width = 10;
            tl.Columns[16].Visible = false;
            tl.Columns.Add();

            tl.Columns[17].Caption = "LOCNAME";
            tl.Columns[17].VisibleIndex = 17;
            tl.Columns[17].Width = 10;
            tl.Columns[17].Visible = false;
            tl.Columns.Add();

            tl.Columns[18].Caption = "TITLE1";
            tl.Columns[18].VisibleIndex = 18;
            tl.Columns[18].Width = 10;
            tl.Columns[18].Visible = false;
            tl.Columns.Add();

            tl.Columns[19].Caption = "TITLE2";
            tl.Columns[19].VisibleIndex = 19;
            tl.Columns[19].Width = 10;
            tl.Columns[19].Visible = false;
            tl.Columns.Add();

            tl.Columns[19].Caption = "REQUESTID";
            tl.Columns[19].VisibleIndex = 20;
            tl.Columns[19].Width = 10;
            tl.Columns[19].Visible = false;
            tl.Columns.Add();

            tl.Columns[19].Caption = "PROTOCOLID";
            tl.Columns[19].VisibleIndex = 21;
            tl.Columns[19].Width = 10;
            tl.Columns[19].Visible = false;
            tl.Columns.Add();

            tl.EndUpdate();
        }
        private void SearchData_REQUEST_Status()
        {
            // Load data from REQUESTS
            // Load data in Result Entry and status = null
            // 

            DBFactory dbFactory = new DBFactory();
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT DISTINCT" +
                " REQUESTS.ACCESSNUMBER, PATIENTS.PATID, PATIENTS.PATNUMBER, PATIENTS.NAME " +
                ",PATIENTS.LASTNAME, REQUESTS.URGENT, REQUESTS.COLLECTIONDATE " +
                ",REQUESTS.REQDATE " +
                " FROM REQUESTS " +
                " LEFT OUTER JOIN PATIENTS ON PATIENTS.PATID = REQUESTS.PATID " +
                " LEFT OUTER JOIN MB_REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID " +
                " WHERE REQUESTS.URGENT = '0'" +
                " AND (MB_REQUESTS.VALIDATIONSTATUS = '3' OR MB_REQUESTS.VALIDATIONSTATUS = '4')");

            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql.ToString(), objConn);
                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                ds.Clear();
                adp.Fill(ds, "result");
                SqlDataReader reader = sqlCmd.ExecuteReader();
                CreateColums_REQUESTS_Routine(treeList1, ds);

                treeList1.BeginUnboundLoad();
                treeList1.ClearNodes();
                TreeListNode parentForRootNodes = null;
                if (ds.Tables["result"].Rows.Count > 0)
                {
                    //  CreateColumns
                    // 0 = Patnumber
                    // 1 = Accessnumber 
                    // 2 = PATID
                    // 3 = NAME 
                    // 4 = LASTNAME 
                    // 5 = REQUEST DATE 
                    // 6 = COLLECTION DATE 

                    for (int i = 0; i < ds.Tables["result"].Rows.Count; i++)
                    {
                        childNode = treeList1.AppendNode(new object[] {
                        ds.Tables["result"].Rows[i]["PATNUMBER"].ToString(),
                        ds.Tables["result"].Rows[i]["ACCESSNUMBER"].ToString(),
                        ds.Tables["result"].Rows[i]["PATID"].ToString(),
                        ds.Tables["result"].Rows[i]["NAME"].ToString(),
                        ds.Tables["result"].Rows[i]["LASTNAME"].ToString(),
                        ds.Tables["result"].Rows[i]["REQDATE"].ToString(),
                        ds.Tables["result"].Rows[i]["COLLECTIONDATE"].ToString()

                        }, parentForRootNodes);

                    }
                    treeList1.EndUnboundLoad();
                    treeList1.ExpandAll();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SearchData_MB_REQUESTS(string StrcellACCESS)
        {   // '" + StrcellACCESS + "'
            DBFactory dbFactory = new DBFactory();
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT " +
                " MB_REQUESTS.MBREQNUMBER" +
                ", MB_REQUESTS.REQUESTID" +
                ", REQUESTS.ACCESSNUMBER" +
                ", DICT_TESTS.TESTCODE" +
                ", DICT_TESTS.TESTNAME, REQUESTS.URGENT, REQUESTS.REQDOCTOR, REQUESTS.REQLOCATION" +
                ", REQUESTS.COMMENT, REQUESTS.REQDATE, REQUESTS.RECEIVEDDATE, DICT_DOCTORS.DOCNAME" +
                ", DICT_LOCATIONS.LOCNAME, PATIENTS.PATNUMBER, PATIENTS.NAME, PATIENTS.LASTNAME" +
                ", PATIENTS.BIRTHDATE, PATIENTS.SEX, PATIENTS.TITLE1, PATIENTS.TITLE2, MB_REQUESTS.MBREQUESTID" +
                ", MB_REQUESTS.PROTOCOLID" +
                " FROM MB_REQUESTS" +
                " LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID" +
                " LEFT OUTER JOIN PATIENTS ON PATIENTS.PATID = REQUESTS.PATID" +
                " LEFT OUTER JOIN DICT_TESTS ON MB_REQUESTS.TESTID = DICT_TESTS.TESTID" +
                " LEFT OUTER JOIN DICT_DOCTORS ON REQUESTS.REQDOCTOR = DICT_DOCTORS.DOCCODE" +
                " LEFT OUTER JOIN DICT_LOCATIONS ON REQUESTS.REQLOCATION = DICT_LOCATIONS.LOCCODE" +
                " WHERE REQUESTS.ACCESSNUMBER = '" + StrcellACCESS + "' " +
                " AND (MB_REQUESTS.VALIDATIONSTATUS='3' OR MB_REQUESTS.VALIDATIONSTATUS='4') ");

            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql.ToString(), objConn);
                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                ds.Clear();
                adp.Fill(ds, "PROTOCOL_LIST");
                SqlDataReader reader = sqlCmd.ExecuteReader();
                CreateColums_MB_REQUESTS_Routine(treeList2, ds);

                treeList2.BeginUnboundLoad();
                treeList2.ClearNodes();
                TreeListNode parentForRootNodes = null;
                if (ds.Tables["PROTOCOL_LIST"].Rows.Count > 0)
                {
                    //  CreateColums_SP_TESTS_RC_Routine
                    // 0 = Protocol CODE
                    // 1 = Protocol Text
                    // 2 = Specimen name
                    // 3 = Specimen code
                    // 4 = Protocol Station

                    for (int i = 0; i < ds.Tables["PROTOCOL_LIST"].Rows.Count; i++)
                    {
                        //string Str_DOB = ((DateTime)(ds.Tables["PROTOCOL_LIST"].Rows[i]["BIRTHDATE"])).ToString("dd-MM-yyyy", cultureinfo_ENG);
                        string Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(ds.Tables["PROTOCOL_LIST"].Rows[i]["BIRTHDATE"])).ToString();

                        childNode = treeList2.AppendNode(new object[] {
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["MBREQNUMBER"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["TESTNAME"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["TESTCODE"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["URGENT"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["REQDOCTOR"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["REQLOCATION"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["COMMENT"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["REQDATE"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["RECEIVEDDATE"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["PATNUMBER"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["NAME"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["LASTNAME"].ToString(),
                        Str_DOB,
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["SEX"].ToString(),
                        "",
                        "",
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["DOCNAME"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["LOCNAME"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["TITLE1"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["TITLE2"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["REQUESTID"].ToString(),
                        ds.Tables["PROTOCOL_LIST"].Rows[i]["PROTOCOLID"].ToString()
                        }, parentForRootNodes);
                    }
                    treeList2.EndUnboundLoad();
                    treeList2.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeList1_Click(object sender, EventArgs e)
        {
            if (treeList1.AllNodesCount > 0)
            {
                //
                string StrcellPATID = treeList1.FocusedNode.GetDisplayText(0);
                string StrcellACCESS = treeList1.FocusedNode.GetDisplayText(1);
                SearchData_MB_REQUESTS(StrcellACCESS);
            }
        }

        private void treeList1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void treeList2_DoubleClick(object sender, EventArgs e)
        {
            Send_Information_FRM_RESULT();
        }

        private void Send_Information_FRM_RESULT()
        {
            try
            {

                frmStain fm = new frmStain();
                OrderEntryM objRequest = new OrderEntryM();
                OEMController objOrder = new OEMController();

                //// PATIENT Diffinition OBJECT
                try
                {
                    string Str_DateOfBirth = "";

                    PatientM objPatientM = new PatientM();
                    // #
                    string StrPatnumber = treeList2.FocusedNode.GetDisplayText(9);
                    // NAME OF PATIENT
                    string StrName = treeList2.FocusedNode.GetDisplayText(10);
                    // LASTNAME OF PATIENT
                    string StrLastName = treeList2.FocusedNode.GetDisplayText(11);
                    // BIRTHDATH OF PATIENT

                    DateTime bDt = Convert.ToDateTime(treeList2.FocusedNode.GetDisplayText(12));
                    //string StrBirthDate = bDt.ToString("dd//MM//yyyy", CultureInfo.CreateSpecificCulture("hu-HU"));

                    string StrBirthDate = treeList2.FocusedNode.GetDisplayText(12);
                    string StrSex = treeList2.FocusedNode.GetDisplayText(13);


                    // Start Date of Birth in calcu
                    if (StrBirthDate == null || StrBirthDate == "")
                    {
                        Str_DateOfBirth = "-";
                    }
                    else
                    {
                        string[] Array_DOB = StrBirthDate.Split("-".ToCharArray());

                        int Str_Year = Convert.ToInt32(Array_DOB[2]);
                        int Str_Month = Convert.ToInt32(Array_DOB[1]);
                        int Str_Day = Convert.ToInt32(Array_DOB[0]);

                        DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                        DateTime ToDate = DateTime.Now;
                        DateDifference dDiff = new DateDifference(myDate, ToDate);

                        Str_DateOfBirth = dDiff.ToString();
                    }
                    // END Date of Birth in calcu

                    objPatientM.PatientNo = StrPatnumber;
                    // TITLE + NAMD + LASTNAME
                    objPatientM.PatientName = "Title " + StrName + " " + StrLastName;

                    objPatientM.BirthDate = StrBirthDate;

                    if (StrSex == "1")
                    { objPatientM.Sex = "ชาย"; }
                    else if (StrSex == "2")
                    { objPatientM.Sex = "หญิง"; }
                    else
                    { objPatientM.Sex = "ไม่ระบุเพศ"; }

                    objPatientM.LongAge = Str_DateOfBirth;
                    objRequest.objPatientM = objPatientM;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


                ////END PATIENT OBJECT
                ///

                // Doctor 
                string StrDoctor = treeList2.FocusedNode.GetDisplayText(16);
                // Location
                string StrLocation = treeList2.FocusedNode.GetDisplayText(17);
                objRequest.MBReqNumber = treeList2.FocusedNode.GetDisplayText(0);
                objRequest.AccessNumber = treeList1.FocusedNode.GetDisplayText(1);
                objRequest.UrgentStatus = treeList2.FocusedNode.GetDisplayText(3);
                objRequest.Comment = treeList2.FocusedNode.GetDisplayText(6);
                objRequest.ReqDate = Convert.ToDateTime(treeList2.FocusedNode.GetDisplayText(7));
                objRequest.ReceiveDate = Convert.ToDateTime(treeList2.FocusedNode.GetDisplayText(8));
                objRequest.ReqDoctor = StrDoctor;
                objRequest.ReqLocation = StrLocation;
                objRequest.REQUESTID = Convert.ToInt16 (treeList2.FocusedNode.GetDisplayText(20));
                objRequest.MBRequestID = objOrder.GetMBRequestID(objRequest.REQUESTID, objRequest.MBReqNumber);
                objRequest.ProtocolID= Convert.ToInt16(treeList2.FocusedNode.GetDisplayText(21));                
                fm.OrderEntryObject = objRequest;
                fm.ShowDialog();
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Function " + ex.Message, "", MessageBoxButtons.OK);
            }
        }
    }
}
