﻿using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using UniquePro.Common;
using UniquePro.Entities.Common;

namespace UNIQUE.Forms.Dashboard
{
    public partial class Form_DashboardRC_Urgent : Form
    {

        System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");
        string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));
        private static string Version = "Requests Creation Urgent";
        ConnectString objConstr = new ConnectString();
        private String Constr = "";
        TreeListNode childNode;

        public Form_DashboardRC_Urgent()
        {
            InitializeComponent();
        }

        private void Form_DashboardRC_Urgent_Load(object sender, EventArgs e)
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
            SearchData_SP_REQUEST_Status();
        }

        private void CreateColums_SP_REQUESTS_RC_Urgent(TreeList tl, DataSet ds)
        {
            //  CreateColumns
            // 0 = Patnumber
            // 1 = Accessnumber 
            // 2 = PATID
            // 3 = NAME 
            // 4 = LASTNAME 
            // 5 = REQUEST DATE 
            // 6 = COLLECTION DATE 

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
            tl.Columns[5].Width = 130;
            tl.Columns[5].Visible = true;
            tl.Columns.Add();

            tl.Columns[6].Caption = "Collection Date";
            tl.Columns[6].VisibleIndex = 6;
            tl.Columns[6].Width = 130;
            tl.Columns[6].Visible = true;
            tl.Columns.Add();


            tl.EndUpdate();
        }

        private void CreateColums_SP_TESTS_RC_Urgent(TreeList tl, DataSet ds)
        {
            //  CreateColums_SP_TESTS_RC_Routine
            // 0 = Protocol CODE
            // 1 = Protocol Text
            // 2 = Specimen code
            // 3 = Specimen name
            // 4 = Protocol Station

            // Create three columns.
            tl.BeginUpdate();
            tl.Columns.Add();

            tl.Columns[0].Caption = "<i><b>Protocol CODE</i></b>";
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

            tl.Columns[2].Caption = "Specimen name";
            tl.Columns[2].VisibleIndex = 2;
            tl.Columns[2].Width = 190;
            tl.Columns[2].Visible = true;
            tl.Columns.Add();

            tl.Columns[3].Caption = "Specimen code";
            tl.Columns[3].VisibleIndex = 3;
            tl.Columns[3].Width = 180;
            tl.Columns[3].Visible = false;
            tl.Columns.Add();

            tl.Columns[4].Caption = "Protocol Station";
            tl.Columns[4].VisibleIndex = 4;
            tl.Columns[4].Width = 180;
            tl.Columns[4].Visible = false;
            tl.Columns.Add();


            tl.EndUpdate();
        }

        private void SearchData_SP_REQUEST_Status()
        {
            DBFactory dbFactory = new DBFactory();
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            StringBuilder sql = new StringBuilder();
            sql.Append("	SELECT PATIENTS.PATID ");
            sql.Append(", SP_REQUESTS.SP_ACCESSNUMBER");
            sql.Append(", PATIENTS.PATID, PATIENTS.PATNUMBER, PATIENTS.NAME, PATIENTS.LASTNAME");
            sql.Append(", SP_REQUESTS.REQURGENT");
            sql.Append(", SP_REQUESTS.COLLECTIONDATE");
            sql.Append(", SP_REQUESTS.TRANSMISSIONSTATUS");
            sql.Append(", SP_REQUESTS.LOGUSERID");
            sql.Append(", ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate())) / 12 as AGEY, ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate())) % 12 as AGEM");
            sql.Append(", PATIENTS.BIRTHDATE, PATIENTS.SEX");
            sql.Append(", PATIENTS.ALTNUMBER");
            sql.Append(", SP_REQUESTS.HOSTORDERNUMBER");
            sql.Append(", SP_REQUESTS.SP_DOCCODE");
            sql.Append(", SP_REQUESTS.SP_LOCCODE");
            sql.Append(", SP_REQUESTS.REQDATE");
            sql.Append(", SP_REQUESTS.LOGDATE");
            sql.Append(", SP_REQUESTS.SECRETRESULT");
            sql.Append(", PATIENTS.COMMENT");
            sql.Append(", PATIENTS.TITLE1");
            sql.Append(", PATIENTS.TITLE2 ");
            sql.Append(" FROM SP_REQUESTS ");
            sql.Append(" LEFT OUTER JOIN PATIENTS ON PATIENTS.PATNUMBER = SP_REQUESTS.PATNUM ");
            sql.Append(" WHERE SP_REQUESTS.TRANSMISSIONSTATUS = '0' AND SP_REQUESTS.REQURGENT ='1'");
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql.ToString(), objConn);
                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                ds.Clear();
                adp.Fill(ds, "result_RC");
                SqlDataReader reader = sqlCmd.ExecuteReader();
                CreateColums_SP_REQUESTS_RC_Urgent(treeList1, ds);

                treeList1.BeginUnboundLoad();
                treeList1.ClearNodes();
                TreeListNode parentForRootNodes = null;
                if (ds.Tables["result_RC"].Rows.Count > 0)
                {
                    //  CreateColumns
                    // 0 = Patnumber
                    // 1 = Accessnumber 
                    // 2 = PATID
                    // 3 = NAME 
                    // 4 = LASTNAME 
                    // 5 = REQUEST DATE 
                    // 6 = COLLECTION DATE 

                    for (int i = 0; i < ds.Tables["result_RC"].Rows.Count; i++)
                    {
                        childNode = treeList1.AppendNode(new object[] {
                        ds.Tables["result_RC"].Rows[i]["PATNUMBER"].ToString(),
                        ds.Tables["result_RC"].Rows[i]["SP_ACCESSNUMBER"].ToString(),
                        ds.Tables["result_RC"].Rows[i]["PATID"].ToString(),
                        ds.Tables["result_RC"].Rows[i]["NAME"].ToString(),
                        ds.Tables["result_RC"].Rows[i]["REQDATE"].ToString(),
                        ds.Tables["result_RC"].Rows[i]["COLLECTIONDATE"].ToString()

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

        private void SearchData_SP_TESTS_Status(string StrcellACCESS)
        {
            DBFactory dbFactory = new DBFactory();
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            StringBuilder sql = new StringBuilder();
            sql.Append("	SELECT SP_TESTCODE ");
            sql.Append(", SP_TESTNAME ");
            sql.Append(", SP_SPECIMENCODE ");
            sql.Append(", SP_SPECIMENNAME ");
            sql.Append(", SP_STNCODE ");
            sql.Append(" FROM SP_TESTS where SP_ACCESSNUMBER = '" + StrcellACCESS + "' ");
            sql.Append(" AND SP_TESTS.SP_TESTSTATUS = '0' ");

            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql.ToString(), objConn);
                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                ds.Clear();
                adp.Fill(ds, "Code_list");
                SqlDataReader reader = sqlCmd.ExecuteReader();
                CreateColums_SP_TESTS_RC_Urgent(treeList2, ds);

                treeList2.BeginUnboundLoad();
                treeList2.ClearNodes();
                TreeListNode parentForRootNodes = null;
                if (ds.Tables["Code_list"].Rows.Count > 0)
                {
                    //  CreateColums_SP_TESTS_RC_Routine
                    // 0 = Protocol CODE
                    // 1 = Protocol Text
                    // 2 = Specimen name
                    // 3 = Specimen code
                    // 4 = Protocol Station

                    for (int i = 0; i < ds.Tables["Code_list"].Rows.Count; i++)
                    {
                        childNode = treeList2.AppendNode(new object[] {
                        ds.Tables["Code_list"].Rows[i]["SP_TESTCODE"].ToString(),
                        ds.Tables["Code_list"].Rows[i]["SP_TESTNAME"].ToString(),
                        ds.Tables["Code_list"].Rows[i]["SP_SPECIMENNAME"].ToString(),
                        ds.Tables["Code_list"].Rows[i]["SP_SPECIMENCODE"].ToString(),
                        ds.Tables["Code_list"].Rows[i]["SP_STNCODE"].ToString()
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
                SearchData_SP_TESTS_Status(StrcellACCESS);
            }
        }

        private void treeList1_DoubleClick(object sender, EventArgs e)
        {
            if (treeList1.AllNodesCount > 0)
            {
                // 
                string StrcellACCESS = treeList1.FocusedNode.GetDisplayText(1);
                Specimen.Form_Rectube FM_Rectube = new Specimen.Form_Rectube(StrcellACCESS);
                FM_Rectube.ShowDialog();
            }
        }
    }
}
