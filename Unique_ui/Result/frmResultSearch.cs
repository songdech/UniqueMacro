using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Patient;
using UniquePro.Entities.OEM;
using UNIQUE.Result;

namespace UNIQUE.Result
{
    public partial class frmResultSearch : Form
    {

        TreeListNode childNode;
        TreeListNode rootNode;

        String strBirthDate = "";

        string Filter_Critiria = "";

        PatientController objPatient;
        OEMController objOrderEntry;

        PatientM objPatientM = null;

        private DataTable dtReq = null;

        public frmResultSearch()
        {
            InitializeComponent();
            objPatient = new PatientController();
            objOrderEntry = new OEMController();
        }

        private void dropDownButton_SearchPAT_Click(object sender, EventArgs e)
        {
            if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 0)
            {
                // Search Patient..
                Process_FindPatients();
            }
            else if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 1)
            {
                // Search Accessnumber..
                Process_FindAccessnumber();
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

        private void frmResultSearch_Load(object sender, EventArgs e)
        {
            Clearinformationall();
            comboBox1.SelectedIndex = 0;
            textEdit_search.Select();

            
            GetIniatialData();

            //groupControl1.Location = new Point(0, 57);
            //groupControl1.Size = new Size(924, 84);
            //groupControl2.Location = new Point(xPatientInform, yPatientInform);
            //groupControl2.Size = new Size(924, 201);
            //groupControl3.Location = new Point(0, 354);
            //groupControl3.Size = new Size(924, 367);
            //treeList1.Location = new Point(5, 25);
            //treeList1.Size = new Size(914, 339);

        }

        private void GetIniatialData()
        {
            DataTable dt = null;

            try
            {
                // Combo for location 
                dt = ControlParameter.DatasetComboData.Tables["DICT_LOCATIONS"];

                if (dt.Rows.Count > 0)
                {
                    comboBox_Search_LOCATION.DisplayMember = "LOCCODE";
                    comboBox_Search_LOCATION.ValueMember = "LOCCODE";
                    comboBox_Search_LOCATION.DataSource = dt;
                }

                dt = null;

                dt = ControlParameter.DatasetComboData.Tables["DICT_DOCTORS"];

                if (dt.Rows.Count > 0)
                {
                    comboBox_Search_DOCTOR.DisplayMember = "DOCCODE";
                    comboBox_Search_DOCTOR.ValueMember = "DOCCODE";
                    comboBox_Search_DOCTOR.DataSource = dt;
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Clearinformationall()
        {
            // Date time Filter
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
            labelControl7.Text = "";

            //textEdit_search.s
            //treeList1.ClearNodes();
            //treeList1.Columns.Clear();
            grdView.DataSource = null;
            
            labelControl_patnum.Text = "";
            labelControl_name.Text = "";
            labelControl_Birthdate.Text = "";
            labelControl_diag.Text = "";
            labelControl_sex.Text = "";
            labelControl_altnum.Text = "";
            labelControl_Age.Text = "";
            labelControl_hosnum.Text = "";
            memoEdit_Comments.Text = "";
            labelControl_tltle.Text = "";
            //
            //groupControl1.Location = new Point(0, 57);
            //groupControl1.Size = new Size(924, 84);
            //groupControl2.Location = new Point(5, 70);
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
            labelControl7.Text = "";

            //textEdit_search.s
            //treeList1.ClearNodes();
            //treeList1.Columns.Clear();
            labelControl_patnum.Text = "";
            labelControl_name.Text = "";
            labelControl_Birthdate.Text = "";
            labelControl_diag.Text = "";
            labelControl_sex.Text = "";
            labelControl_altnum.Text = "";
            labelControl_Age.Text = "";
            labelControl_hosnum.Text = "";
            memoEdit_Comments.Text = "";
            labelControl_tltle.Text = "";
            //        

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textEdit_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 0)
                {
                    // Search Patient..
                    Process_FindPatients();
                    grdView.DataSource = null;
                    //treeList1.ClearNodes();
                    //treeList1.Columns.Clear();
                    labelControl7.Text = "";
                }
                else if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 1)
                {
                    // Search Accessnumber..
                    Process_FindAccessnumber();
                    grdView.DataSource = null;
                    //treeList1.ClearNodes();
                    //treeList1.Columns.Clear();
                    labelControl7.Text = "";
                }
                else if (textEdit_search.Text != "" && comboBox1.SelectedIndex == 2)
                {
                    // Search Protocol
                    grdView.DataSource = null;
                    //treeList1.ClearNodes();
                    //treeList1.Columns.Clear();
                    labelControl7.Text = "";
                }
                else
                {
                    MessageBox.Show("Please select source first");
                }
            }
        }


        private void Process_FindPatients()
        {
            DataTable dt;

            String Strsex = "";
            String strBirthDate = "";
            String bdate = "";

            try
            {

                dt = objPatient.GetPatientSearch(textEdit_search.Text, "");

                if (dt.Rows.Count > 0)
                {
                    string StrCOMMENTS = dt.Rows[0]["COMMENT"].ToString();

                    memoEdit_Comments.Text = StrCOMMENTS;

                    if (dt.Rows[0]["PATNUMBER"].ToString() != "")
                    { labelControl_patnum.Text = dt.Rows[0]["PATNUMBER"].ToString().TrimStart('0'); }
                    if (dt.Rows[0]["SEX"].ToString() == "1")
                    { Strsex = "ชาย"; }
                    else if (dt.Rows[0]["SEX"].ToString() == "2")
                    { Strsex = "หญิง"; }
                    else
                    { Strsex = "ไม่ระบุ"; }
                    labelControl_name.Text = dt.Rows[0]["NAME"].ToString() + " " + dt.Rows[0]["LASTNAME"].ToString();
                    labelControl_sex.Text = Strsex;
                    if (dt.Rows[0]["BIRTHDATE"].ToString() != "")
                    {
                        DateTime bDt = Convert.ToDateTime(dt.Rows[0]["BIRTHDATE"].ToString());
                        strBirthDate = bDt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));
                        bdate = strBirthDate;
                        labelControl_Birthdate.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dt.Rows[0]["BIRTHDATE"].ToString()));
                        int age;
                        if (Convert.ToInt32(String.Format("{0:yyyy}", Convert.ToDateTime(dt.Rows[0]["BIRTHDATE"].ToString()))) > 2500)
                        {
                            age = DateTime.Now.Year - Convert.ToInt32(String.Format("{0:yyyy}", Convert.ToDateTime(dt.Rows[0]["BIRTHDATE"].ToString())));
                            age = age + 543;
                        }
                        else
                        {
                            age = DateTime.Now.Year - Convert.ToInt32(String.Format("{0:yyyy}", Convert.ToDateTime(dt.Rows[0]["BIRTHDATE"].ToString())));
                        }
                        string AGEY = dt.Rows[0]["AGEY"].ToString();
                        string AGEM = dt.Rows[0]["AGEM"].ToString();
                        labelControl_Age.Text = AGEY + " ปี  " + AGEM + " เดือน";
                    }

                    Process_FindAccessnumber_with_patient();
                }
                else
                {
                    DialogResult yes = MessageBox.Show("Not Found patient do you want Insert Patient ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (yes == DialogResult.Yes)
                    {
                        OEM.Form_PATIENT_INSERT FM_insert = new OEM.Form_PATIENT_INSERT();
                        FM_insert.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :Req:1004 Query_Patient and_Insert SUBREQMB_STAINS \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Process_FindAccessnumber()
        {
            string Strsex = "";
            string Str_Req_status = "";
            string bdate = "";
            
            
            DataTable dtView;
            DataRow dr;

            try
            {

                dtReq = objPatient.GetAccessNumberData(textEdit_search.Text ,"", "");

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
                        labelControl_patnum.Text = dtReq.Rows[0]["PATNUMBER"].ToString().TrimStart('0');
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
                    labelControl_hosnum.Text = dtReq.Rows[0]["HOSTORDERNUMBER"].ToString();

                    DateTime DT = Convert.ToDateTime(dtReq.Rows[0]["REQDATE"].ToString());
                    string StrReqDate = DT.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));

                    if (dtReq.Rows[0]["BIRTHDATE"].ToString() != "")
                    {
                        DateTime bDt = Convert.ToDateTime(dtReq.Rows[0]["BIRTHDATE"].ToString());
                        strBirthDate = bDt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));
                        bdate = strBirthDate;
                        labelControl_Birthdate.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtReq.Rows[0]["BIRTHDATE"].ToString()));
                        int age;
                        if (Convert.ToInt32(String.Format("{0:yyyy}", Convert.ToDateTime(dtReq.Rows[0]["BIRTHDATE"].ToString()))) > 2500)
                        {
                            age = DateTime.Now.Year - Convert.ToInt32(String.Format("{0:yyyy}", Convert.ToDateTime(dtReq.Rows[0]["BIRTHDATE"].ToString())));
                            age = age + 543;
                        }
                        else
                        {
                            age = DateTime.Now.Year - Convert.ToInt32(String.Format("{0:yyyy}", Convert.ToDateTime(dtReq.Rows[0]["BIRTHDATE"].ToString())));
                        }
                        string AGEY = dtReq.Rows[0]["AGEY"].ToString();
                        string AGEM = dtReq.Rows[0]["AGEM"].ToString();
                        labelControl_Age.Text = AGEY + " ปี  " + AGEM + " เดือน";

                        objPatientM.LongAge = labelControl_Age.Text;

                        if (dtReq.Rows[0]["SP_IPDOROPD"].ToString() == "0")
                        {
                            objPatientM.IPDOPDDesc = "IPD";
                            objPatientM.IPDOPDStatus = "0";
                        }
                        else if (dtReq.Rows[0]["SP_IPDOROPD"].ToString() == "1")
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



                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    childNode = treeList1.AppendNode(new object[] {
                    //    dt.Rows[i]["PATID"].ToString(),
                    //    dt.Rows[i]["ACCESSNUMBER"].ToString(),
                    //    dt.Rows[i]["REQDATE"].ToString(),
                    //    dt.Rows[i]["COLLECTIONDATE"].ToString(),
                    //    dt.Rows[i]["RECEIVEDDATE"].ToString(),
                    //    dt.Rows[i]["REQSTATUS"].ToString(),
                    //    dt.Rows[i]["LOGUSERID"].ToString() }, parentForRootNodes);

                    //    string StrCOMMENTS = dt.Rows[i]["COMMENT"].ToString();
                    //    memoEdit_Comments.Text = StrCOMMENTS;

                    //    if (dt.Rows[0]["PATNUMBER"].ToString() != "")
                    //    { labelControl_patnum.Text = dt.Rows[0]["PATNUMBER"].ToString().TrimStart('0'); }
                    //    if (dt.Rows[0]["SEX"].ToString() == "1")
                    //    { Strsex = "ชาย"; }
                    //    else if (dt.Rows[0]["SEX"].ToString() == "2")
                    //    { Strsex = "หญิง"; }
                    //    else
                    //    { Strsex = "ไม่ระบุ"; }

                    //    if (dt.Rows[0]["REQSTATUS"].ToString() == "1")
                    //    { Str_Req_status = "Open"; }
                    //    else if (dt.Rows[0]["REQSTATUS"].ToString() == "2")
                    //    { Str_Req_status = "Close"; }


                    //    labelControl_name.Text = dt.Rows[0]["NAME"].ToString() + " " + dt.Rows[0]["LASTNAME"].ToString();
                    //    labelControl_sex.Text = Strsex;
                    //    labelControl_hosnum.Text = dt.Rows[0]["HOSTORDERNUMBER"].ToString();

                    //    DateTime DT = Convert.ToDateTime(dt.Rows[0]["REQDATE"].ToString());
                    //    string StrReqDate = DT.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));

                    //    if (dt.Rows[0]["BIRTHDATE"].ToString() != "")
                    //    {
                    //        DateTime bDt = Convert.ToDateTime(dt.Rows[0]["BIRTHDATE"].ToString());
                    //        strBirthDate = bDt.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("hu-HU"));
                    //        bdate = strBirthDate;
                    //        labelControl_Birthdate.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dt.Rows[0]["BIRTHDATE"].ToString()));
                    //        int age;
                    //        if (Convert.ToInt32(String.Format("{0:yyyy}", Convert.ToDateTime(dt.Rows[0]["BIRTHDATE"].ToString()))) > 2500)
                    //        {
                    //            age = DateTime.Now.Year - Convert.ToInt32(String.Format("{0:yyyy}", Convert.ToDateTime(dt.Rows[0]["BIRTHDATE"].ToString())));
                    //            age = age + 543;
                    //        }
                    //        else
                    //        {
                    //            age = DateTime.Now.Year - Convert.ToInt32(String.Format("{0:yyyy}", Convert.ToDateTime(dt.Rows[0]["BIRTHDATE"].ToString())));
                    //        }
                    //        string AGEY = dt.Rows[0]["AGEY"].ToString();
                    //        string AGEM = dt.Rows[0]["AGEM"].ToString();
                    //        labelControl_Age.Text = AGEY + " ปี  " + AGEM + " เดือน";
                    //    }

                    //    treeList1.EndUnboundLoad();
                    //    treeList1.ExpandAll();
                    //}

                }
                else
                {
                    DialogResult yes = MessageBox.Show("Not Found Accessnumber ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (yes == DialogResult.Yes)
                    {
                        OEM.Form_PATIENT_INSERT FM_insert = new OEM.Form_PATIENT_INSERT();
                        FM_insert.ShowDialog();
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
            PatientController objPatient = new PatientController();
            DataTable dt;

            try
            {
                if (labelControl_patnum.Text != "")
                {

                    dt = objPatient.GetAccessNumberWithPatient(labelControl_patnum.Text);

                    if (dt.Rows.Count > 0)
                    {
                        //CreateColumns(treeList1, dt);
                        //treeList1.BeginUnboundLoad();
                        //treeList1.ClearNodes();
                        TreeListNode parentForRootNodes = null;
                        //for (int i = 0; i < dt.Rows.Count; i++)
                        //{
                        //   // childNode = treeList1.AppendNode(new object[] {
                        //dt.Rows[i]["PATID"].ToString(),
                        //dt.Rows[i]["ACCESSNUMBER"].ToString(),
                        //dt.Rows[i]["REQDATE"].ToString(),
                        //dt.Rows[i]["COLLECTIONDATE"].ToString(),
                        //dt.Rows[i]["RECEIVEDDATE"].ToString(),
                        //dt.Rows[i]["REQSTATUS"].ToString(),
                        //dt.Rows[i]["LOGUSERID"].ToString() }, parentForRootNodes);
                        //    string StrCOMMENTS = dt.Rows[i]["COMMENT"].ToString();
                        //    memoEdit_Comments.Text = StrCOMMENTS;

                        //    // *************
                        //    // ************* Set text from 1 to OPEN and 2 to CLOSE
                        //    //
                        //    //if (ds.Tables["Query_accessnumber"].Rows[0]["REQSTATUS"].ToString() == "1")
                        //    //{ Str_Req_status = "Open"; }
                        //    //else if (ds.Tables["Query_accessnumber"].Rows[0]["REQSTATUS"].ToString() == "2")
                        //    //{ Str_Req_status = "Close"; }
                        //    DateTime DT = Convert.ToDateTime(dt.Rows[0]["REQDATE"].ToString());
                        //    string StrReqDate = DT.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));
                        //    treeList1.EndUnboundLoad();
                        //    treeList1.ExpandAll();
                        //}
                    }
                    else
                    {
                      //  treeList1.ClearNodes();

                        DialogResult yes = MessageBox.Show("Not Found Accessnumber you want create request Order ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (yes == DialogResult.Yes)
                        {
                            OEM.Form_PATIENT_INSERT FM_insert = new OEM.Form_PATIENT_INSERT();
                            FM_insert.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :Req:1005 Query Process_FindAccessnumber_with_patient \r\nDetail : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void CreateColumns(TreeList tl, DataTable dt)
        {
            // 0 = PATID 1 = Accessnumber 2 = Request Date 3 = Collection Date 4 = Receive Date 5 = Status 6 = User 7 = REQUESTID

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

            //tl.Columns[1].Caption = "<i>Request</i><b>Accessnumber</b>";
            tl.Columns[1].Caption = "<b>Accessnumber</b>";
            tl.Columns[1].VisibleIndex = 1;
            tl.Columns[1].Width = 150;
            tl.Columns[1].Visible = true;
            //tl.OptionsView.AllowHtmlDrawHeaders = true;
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

            tl.EndUpdate();
        }

        private void checkButton_Search_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void checkButton_Search_Click(object sender, EventArgs e)
        {
            textEdit_search.Text = "";

            if (checkButton_Search.Checked == true)
            {
                // Expand group
                //groupControl1.Location = new Point(0, 57);
                //groupControl1.Size = new Size(924, 84);
                //groupControl2.Location = new Point(5, 79);
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
                    dropDownButton_search_text.Enabled = true;
                }
                else
                {
                    textEdit_Search_text.Enabled = false;
                    dropDownButton_search_text.Enabled = false;
                }

                dateTimePicker_start.Enabled = true;
                timeEdit_start.Enabled = true;
                dateTimePicker_finish.Enabled = true;
                timeEdit_finish.Enabled = true;
                dropDownButton_Start_search.Enabled = true;
                radioGroup1.Enabled = true;
                comboBox_Search_DOCTOR.Enabled = true;
                comboBox_Search_LOCATION.Enabled = true;
                dropDownButton2.Enabled = true;
            }
        }

        private void dropDownButton_Start_search_Click(object sender, EventArgs e)
        {
            textEdit_Search_text.Text = "";
            Process_Search_start();
        }

        private void Process_Search_start()
        {
            int count = 0;
            string get_data = "";
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

            DataTable dtPatient;
            DataTable dtAccessNumber;
            DataTable dtRequest;

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

                dtPatient = objPatient.GetPatientSearch("", Filter_Critiria);

                if (dtPatient.Rows.Count > 0)
                {
                    //CreateColumns_patientSearch(treeList1, dtPatient);
                    //treeList1.BeginUnboundLoad();
                    //treeList1.ClearNodes();
                    TreeListNode parentForRootNodes = null;

                    //for (int i = 0; i < dtPatient.Rows.Count; i++)
                    //{
                    //    count++;
                    //    childNode = treeList1.AppendNode(new object[] {
                    //    dtPatient.Rows[i]["PATID"].ToString(),
                    //    dtPatient.Rows[i]["PATNUMBER"].ToString(),
                    //    dtPatient.Rows[i]["NAME"].ToString(),
                    //    dtPatient.Rows[i]["LASTNAME"].ToString(),
                    //    dtPatient.Rows[i]["REFDOCTOR"].ToString(),
                    //    dtPatient.Rows[i]["REFLOCATION"].ToString(),
                    //    dtPatient.Rows[i]["COMMENT"].ToString(),
                    //    dtPatient.Rows[i]["BIRTHDATE"].ToString(),
                    //    dtPatient.Rows[i]["SEX"].ToString(),
                    //    dtPatient.Rows[i]["AGEY"].ToString(),
                    //    dtPatient.Rows[i]["AGEM"].ToString(),
                    //    dtPatient.Rows[i]["TITLE1"].ToString(),
                    //    dtPatient.Rows[i]["TITLE2"].ToString() }, parentForRootNodes);

                    //    treeList1.EndUnboundLoad();
                    //    treeList1.ExpandAll();
                    //}
                    labelControl7.Text = "Total Line " + count.ToString();

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
                dtAccessNumber = objPatient.GetAccessNumberData("","", Filter_Critiria);

                if (dtAccessNumber.Rows.Count > 0)
                {

                    //CreateColumns(treeList1, dtAccessNumber);
                    //treeList1.BeginUnboundLoad();
                    //treeList1.ClearNodes();
                    TreeListNode parentForRootNodes = null;

                    // 0 = PATID 1 = Accessnumber 2 = Request Date 3 = Collection Date 4 = Receive Date 5 = Status 6 = User 7 = REQUESTID

                    //    for (int i = 0; i < dtAccessNumber.Rows.Count; i++)
                    //    {
                    //        count++;
                    //        childNode = treeList1.AppendNode(new object[] {
                    //        dtAccessNumber.Rows[i]["PATID"].ToString(),
                    //        dtAccessNumber.Rows[i]["ACCESSNUMBER"].ToString(),
                    //        dtAccessNumber.Rows[i]["REQDATE"].ToString(),
                    //        dtAccessNumber.Rows[i]["COLLECTIONDATE"].ToString(),
                    //        dtAccessNumber.Rows[i]["RECEIVEDDATE"].ToString(),
                    //        dtAccessNumber.Rows[i]["REQSTATUS"].ToString(),
                    //        dtAccessNumber.Rows[i]["LOGUSERID"].ToString(),
                    //        dtAccessNumber.Rows[i]["REQUESTID"].ToString(),
                    //        dtAccessNumber.Rows[i]["PATNUMBER"].ToString(),
                    //        dtAccessNumber.Rows[i]["NAME"].ToString(),
                    //        dtAccessNumber.Rows[i]["LASTNAME"].ToString(),
                    //        dtAccessNumber.Rows[i]["AGEY"].ToString(),
                    //        dtAccessNumber.Rows[i]["AGEM"].ToString(),
                    //        dtAccessNumber.Rows[i]["SEX"].ToString(),
                    //        dtAccessNumber.Rows[i]["ALTNUMBER"].ToString(),
                    //        dtAccessNumber.Rows[i]["HOSTORDERNUMBER"].ToString(),
                    //        dtAccessNumber.Rows[i]["REQDOCTOR"].ToString(),
                    //        dtAccessNumber.Rows[i]["REQLOCATION"].ToString(),
                    //        dtAccessNumber.Rows[i]["COMMENT"].ToString(),
                    //        dtAccessNumber.Rows[i]["TITLE1"].ToString(),
                    //        dtAccessNumber.Rows[i]["TITLE2"].ToString(),
                    //        dtAccessNumber.Rows[i]["BIRTHDATE"].ToString()

                    //        }, parentForRootNodes);
                    //        memoEdit_Comments.Text = dtAccessNumber.Rows[i]["COMMENT"].ToString();
                    //        treeList1.EndUnboundLoad();
                    //        treeList1.ExpandAll();
                    //    }
                    //    labelControl7.Text = "Total Line " + count.ToString();
                    //    MessageBox.Show("Finish Query data", "", MessageBoxButtons.OK);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Not Found data?", "", MessageBoxButtons.OK);
                    //}
                }
                // if select Protocol number
                if (comboBox1.SelectedIndex == 2)
                {
                    count = 0;
                    get_data = "Select Accessnumber Filter";
                    dtRequest = objOrderEntry.GetRequestData(Filter_Critiria);

                    if (dtRequest.Rows.Count > 0)
                    {
                        //CreateColumns(treeList1, dtRequest);
                        //treeList1.BeginUnboundLoad();
                        //treeList1.ClearNodes();
                        //TreeListNode parentForRootNodes = null;

                        //for (int i = 0; i < dtRequest.Rows.Count; i++)
                        //{
                        //    childNode = treeList1.AppendNode(new object[] {
                        //    dtRequest.Rows[i]["PATID"].ToString(),
                        //    dtRequest.Rows[i]["ACCESSNUMBER"].ToString(),
                        //    dtRequest.Rows[i]["REQDATE"].ToString(),
                        //    dtRequest.Rows[i]["COLLECTIONDATE"].ToString(),
                        //    dtRequest.Rows[i]["RECEIVEDDATE"].ToString(),
                        //    dtRequest.Rows[i]["REQSTATUS"].ToString(),
                        //    dtRequest.Rows[i]["LOGUSERID"].ToString(),
                        //    dtRequest.Rows[i]["REQUESTID"].ToString() }, parentForRootNodes);

                        //    memoEdit_Comments.Text = dtRequest.Rows[i]["COMMENT"].ToString();

                        //    treeList1.EndUnboundLoad();
                        //    treeList1.ExpandAll();
                        //}
                        labelControl7.Text = "Total Line " + count.ToString();
                        MessageBox.Show("Finish Query data", "", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Not Found data?", "", MessageBoxButtons.OK);
                    }
                }

            }

            //private void CreateColumns_patientSearch(TreeList tl, DataTable  dt)
            //{
            //    // Create three columns.
            //    tl.BeginUpdate();
            //    tl.Columns.Add();
            //    tl.Columns[0].Caption = "<i><b>PatID</i></b>";
            //    tl.Columns[0].VisibleIndex = 0;
            //    tl.Columns[0].Width = 80;
            //    tl.Columns[0].Visible = false;
            //    tl.OptionsView.AllowHtmlDrawHeaders = true;
            //    tl.RowHeight = 23;
            //    tl.Columns.Add();

            //    tl.Columns[1].Caption = "<i>Request</i><b>PATNUMBER</b>";
            //    tl.Columns[1].VisibleIndex = 1;
            //    tl.Columns[1].Width = 150;
            //    tl.Columns[1].Visible = true;
            //    tl.OptionsView.AllowHtmlDrawHeaders = true;
            //    tl.Columns.Add();

            //    tl.Columns[2].Caption = "Name";
            //    tl.Columns[2].VisibleIndex = 2;
            //    tl.Columns[2].Width = 200;
            //    tl.Columns[2].Visible = true;
            //    tl.Columns.Add();

            //    tl.Columns[3].Caption = "Lastname";
            //    tl.Columns[3].VisibleIndex = 3;
            //    tl.Columns[3].Width = 200;
            //    tl.Columns[3].Visible = true;
            //    tl.Columns.Add();

            //    tl.Columns[4].Caption = "Refer Doctor";
            //    tl.Columns[4].VisibleIndex = 4;
            //    tl.Columns[4].Width = 200;
            //    tl.Columns[4].Visible = true;
            //    tl.Columns.Add();

            //    tl.Columns[5].Caption = "Refer Location";
            //    tl.Columns[5].VisibleIndex = 5;
            //    tl.Columns[5].Width = 50;
            //    tl.Columns[5].Visible = true;
            //    tl.Columns.Add();

            //    tl.Columns[6].Caption = "Comment";
            //    tl.Columns[6].VisibleIndex = 6;
            //    tl.Columns[6].Width = 90;
            //    tl.Columns[6].Visible = true;
            //    tl.Columns.Add();

            //    tl.Columns[7].Caption = "BIRTHDATE";
            //    tl.Columns[7].VisibleIndex = 7;
            //    tl.Columns[7].Width = 90;
            //    tl.Columns[7].Visible = false;
            //    tl.Columns.Add();

            //    tl.Columns[8].Caption = "SEX";
            //    tl.Columns[8].VisibleIndex = 8;
            //    tl.Columns[8].Width = 90;
            //    tl.Columns[8].Visible = false;
            //    tl.Columns.Add();

            //    tl.Columns[9].Caption = "AGEY";
            //    tl.Columns[9].VisibleIndex = 9;
            //    tl.Columns[9].Width = 90;
            //    tl.Columns[9].Visible = false;
            //    tl.Columns.Add();

            //    tl.Columns[10].Caption = "AGEM";
            //    tl.Columns[10].VisibleIndex = 10;
            //    tl.Columns[10].Width = 90;
            //    tl.Columns[10].Visible = false;
            //    tl.Columns.Add();

            //    tl.Columns[11].Caption = "TITLE1";
            //    tl.Columns[11].VisibleIndex = 11;
            //    tl.Columns[11].Width = 90;
            //    tl.Columns[11].Visible = false;
            //    tl.Columns.Add();

            //    tl.Columns[12].Caption = "TITLE2";
            //    tl.Columns[12].VisibleIndex = 12;
            //    tl.Columns[12].Width = 90;
            //    tl.Columns[12].Visible = false;
            //    tl.Columns.Add();

            //    tl.EndUpdate();

            //}

            //private void treeList1_DoubleClick(object sender, EventArgs e)
            //{
            //    TreeList tree = sender as TreeList;
            //    TreeListHitInfo hi = tree.CalcHitInfo(tree.PointToClient(Control.MousePosition));
            //    if (hi.Node != null)
            //    {
            //        if (hi.HitInfoType == HitInfoType.Cell)
            //        {

            //        }
            //    }            
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

                dtReq.DefaultView.RowFilter = " ACCESSNUMBER = '" + objRequest.AccessNumber  + "' and MBREQNUMBER = '" + objRequest.MBReqNumber + "'";
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
                objRequest.ProtocolCode  = filtered.Rows[0]["PROTOCOLCODE"].ToString();
                objRequest.ProtocolText = filtered.Rows[0]["PROTOCOLTEXT"].ToString();
                objRequest.ReqDoctor = filtered.Rows[0]["REQDOCTOR"].ToString();
                objRequest.ReqLocation = filtered.Rows[0]["REQLOCATION"].ToString();
                objRequest.ReqStatus = filtered.Rows[0]["REQSTATUS"].ToString();
                objRequest.SpecimentCode = filtered.Rows[0]["SPECIMENCODE"].ToString();
                objRequest.UrgentStatus = filtered.Rows[0]["URGENT"].ToString();
                objRequest.REQUESTID  = Convert.ToInt16 (filtered.Rows[0]["REQUESTID"].ToString());
                objRequest.MBRequestID = objOrder.GetMBRequestID(objRequest.REQUESTID, objRequest.MBReqNumber);
                objRequest.ProtocolID = Convert.ToInt16 (filtered.Rows[0]["PROTOCOLID"].ToString());

                //var AccessNumber = gridView1.GetFocusedRowCellValue("ACCESSNUMBER").ToString().Trim();
                fm.OrderEntryObject = objRequest;


                fm.Show();

                this.Close();

                String a = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message , "", MessageBoxButtons.OK);
            }
            
        }

        private void grdView_Click(object sender, EventArgs e)
        {

        }
    }
}

