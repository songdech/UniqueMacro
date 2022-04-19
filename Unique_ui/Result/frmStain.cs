using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.BandedGrid;

using UniquePro.Entities.Patient;
using UniquePro.Entities.OEM;
using UniquePro.Entities.Testing;
using UniquePro.Controller;


namespace UNIQUE.Result
{
    public partial class frmStain : Form
    {
        SqlConnection connTDNL;
        SqlConnection conn;
        RepositoryItemComboBox riCombo ;       
        RepositoryItemComboBox riComboObservation;
        List<StainObservationsForDeleteObservation> sListForDel = new List<StainObservationsForDeleteObservation>();
        List<StainObservationsForDeleteRequestStain> sListForDelRequestMBStain = new List<StainObservationsForDeleteRequestStain>();

        private OrderEntryM objOrderEntryM = null;
        private OEMController objOrderEntry = null;
        
        public DataTable dtExpertRule { get; set; }

        private DataTable dtComment;

        private TestResultM objTestResultM  = null;
        private StainTestResultM objTestStainResultM = null;

        private TestController objTestController = null;

        private DataTable dtOraganism;

        private CommonController objCommonControl = null;

        private String SelectRootStain = "";
        private BreakpointController objBreakpointControl;

        public OrderEntryM OrderEntryObject
        {
            get
            {
                return this.objOrderEntryM;
            }
            set
            {
                this.objOrderEntryM = value;
            }
        }

        public frmStain()
         {
             InitializeComponent();
            
             objOrderEntry = new OEMController();
             objTestStainResultM = new StainTestResultM();
             objTestController = new TestController();
             DataTable dt = new DataTable();
            objCommonControl = new CommonController();
            
            objTestStainResultM.CreateBy = ControlParameter.loginID;
            //this.ActiveControl = txtRequestSearch;
            //     roadData(); // DEMO INFORMATION OF STAIN.
        }

        public Action actionAddNewStain { get; set; }

     
        private void roadData()
        {
          //  string sql = "select * from DICT_LOCATIONS";
            string sql = @"SELECT SUBREQMB_STAINS.SUBREQMBSTAINID,SUBREQMB_STAINS.COLONYID,SUBREQMB_STAINS.SUBREQUESTID,SUBREQMB_STAINS.MBSTAINID,SUBREQMB_STAINS.VALREQUESTED,SUBREQMB_STAINS.CONSOLIDATIONSTATUS,SUBREQMB_STAINS.CREATEUSER,SUBREQMB_STAINS.CREATIONDATE,SUBREQMB_STAINS.LOGUSERID,SUBREQMB_STAINS.LOGDATE,SUBREQMB_STAINS.NOTPRINTABLE,DICT_MB_STAINS.MBSTAINID,DICT_MB_STAINS.MBSTAINCODE,DICT_MB_STAINS.MBSTAINCREDATE,DICT_MB_STAINS.SHORTTEXT,DICT_MB_STAINS.FULLTEXT,DICT_MB_STAINS.DICTCONSOSTATUS,DICT_MB_STAINS.LISTESTCODE, DICT_MB_STAINS.FIRSTITEMID,DICT_MB_STAINS.STARTVALIDDATE,DICT_MB_STAINS.ENDVALIDDATE,DICT_MB_STAINS.NOTPRINTABLE, MB_STAINS_OBSERVATIONS.SUBREQMBSTAINID,MB_STAINS_OBSERVATIONS.MORPHOBSERVATION,MB_STAINS_OBSERVATIONS.CREATEUSER,MB_STAINS_OBSERVATIONS.CREATIONDATE,MB_STAINS_OBSERVATIONS.QUANTITATIVERESULT,MB_STAINS_OBSERVATIONS.COMMENTS,MB_STAINS_OBSERVATIONS.RESUPDDATE,MB_STAINS_OBSERVATIONS.LOGUSERID,MB_STAINS_OBSERVATIONS.LOGDATE,MB_STAINS_OBSERVATIONS.MORPHOBSERVATIONID
 FROM SUBREQMB_STAINS LEFT OUTER JOIN DICT_MB_STAINS 
 ON (SUBREQMB_STAINS.MBSTAINID = DICT_MB_STAINS.MBSTAINID )
  LEFT OUTER JOIN MB_STAINS_OBSERVATIONS 
  ON (SUBREQMB_STAINS.SUBREQMBSTAINID = MB_STAINS_OBSERVATIONS.SUBREQMBSTAINID ) 
  WHERE  SUBREQMB_STAINS.COLONYID IS NULL AND SUBREQMB_STAINS.SUBREQUESTID = 2398
  ORDER BY SUBREQMB_STAINS.SUBREQMBSTAINID";
            SqlCommand cmd = new SqlCommand(sql, connTDNL);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
           // dataset_Doctors1 = new DataSet();
            DataSet ds = new DataSet();
            if (connTDNL.State == ConnectionState.Open) connTDNL.Close(); connTDNL.Open();
            ds.Clear();
            adap.Fill(ds);
           // gridControl1.DataSource = ds.Tables[0];
            //gridControl2.DataSource = ds.Tables[0];
          //  vGridControl1.DataSource = ds.Tables[0];
      
            
            //DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
            //newRow["FULLTEXT"] = "Gram Stain";           
            //(gridControl2.DataSource as DataTable).Rows.Add(newRow);

            //DataRow newRow2 = (gridControl2.DataSource as DataTable).NewRow();
            //newRow2["FULLTEXT"] = "GMS Stain";
            //(gridControl2.DataSource as DataTable).Rows.Add(newRow2);

            //gridControl2.RefreshDataSource();
          
        }

        private void loadComboQuantitativeResult()
        {
            if (gridView2.SelectedRowsCount > 0)
            {
                string[] strObservation = { };
                
                List<string> observation = new List<string>();
                List<string> quantitative = new List<string>();
                //colors.Add("Red");
                //colors.Add("Blue");
                //colors.Add("Green");

                int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
                DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(selRows[0]));
                string rootStain = selRow["STAINNAME"].ToString();
                CommonController objCommon = new CommonController();

                DataTable dt = objCommon.GetResultTypeWithStainListSource(rootStain);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        // strObservation[i] = ds.Tables["text"].Rows[i]["CUSRESULTTEXT"].ToString();
                        if (dt.Rows[i]["PROPERTY"].ToString() == "0")  // Observation.
                        {
                            observation.Add(dt.Rows[i]["CUSRESULTTEXT"].ToString());
                        }
                        if (dt.Rows[i]["PROPERTY"].ToString() == "1")
                        {
                            quantitative.Add(dt.Rows[i]["CUSRESULTTEXT"].ToString());  //Stain
                        }
                    }
                }

              
                riComboObservation = new RepositoryItemComboBox();
                    riCombo = new RepositoryItemComboBox();
                    //  riCombo.Items.AddRange(new string[] { "San Francisco", "Monterey", "Toronto", "Boston", "Kuaui", "Singapore", "Tokyo" });

                    riCombo.Items.AddRange(quantitative);
                    riComboObservation.Items.AddRange(observation);
                    //      riCombo.Items.Add(ds.Tables["text"].Rows);

                    //Add the item to the internal repository
                    gridControl2.RepositoryItems.Add(riCombo);
                    gridControl2.RepositoryItems.Add(riComboObservation);
                    //Now you can define the repository item as an in-place column editor
                    colQUANTITATIVERESULT.ColumnEdit = riCombo;
                    colMORPHOBSERVATION.ColumnEdit = riComboObservation;

                    riComboObservation.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    riCombo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
               
            }
        }
    
        private void frmStain_Load(object sender, EventArgs e)
        {
            try
            {

                checkEnableButton();
                InitData();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
        }

        private void InitData()
        {

            ConfigurationController objConfiguration = new ConfigurationController();            
            AutoCompleteStringCollection objOrganismCode = new AutoCompleteStringCollection();
            AutoCompleteStringCollection objOrganismName = new AutoCompleteStringCollection();
            TestResultM objTestResultM = new TestResultM();
            BreakpointController objBreakpointControl = new BreakpointController();
            TestController objTestDAO = new TestController();
            DataTable dt;

             try
            {
                
                if (objOrderEntryM != null)
                {

                    txtPatnum.Text = objOrderEntryM.objPatientM.PatientNo;
                    txtPatnum.ReadOnly = true;

                    labelControl_name.Text = objOrderEntryM.objPatientM.PatientName;
                    // AGE + SEX
                    labelControl_Age.Text =  objOrderEntryM.objPatientM.LongAge + " " + objOrderEntryM.objPatientM.Sex;

                    labelControl_Birthdate.Text = objOrderEntryM.objPatientM.BirthDate.ToString();
                    memoEdit_Comments.Text = objOrderEntryM.objPatientM.IPDOPDDesc;
                    //label_IPDOPD.Text = objOrderEntryM.objPatientM.IPDOPDDesc;

                    objTestResultM.PatientID = objOrderEntryM.objPatientM.PatientID;
                    objTestResultM.PatNo = objOrderEntryM.objPatientM.PatientNo;
                    objTestResultM.RefDoc = objOrderEntryM.ReqDoctor;
                    objTestResultM.RefLocation = objOrderEntryM.ReqLocation;
                    objTestResultM.SpecimenCode = objOrderEntryM.SpecimentCode;
                    objTestResultM.Status = objOrderEntryM.Status;
                    objTestResultM.RequestID = objOrderEntryM.REQUESTID;
                    objTestResultM.CreateBy = ControlParameter.ControlUser.USERNAME;
                    objTestResultM.UpdateBy = ControlParameter.ControlUser.USERNAME;
                    objTestResultM.MBRequestID = objOrderEntryM.MBRequestID;
                    objTestResultM.AccessNumber = objOrderEntryM.AccessNumber;
                    objTestResultM.MBNumber = objOrderEntryM.MBReqNumber;
                    objTestResultM.UrgentStatus = objOrderEntryM.UrgentStatus;
                    objTestResultM.Comment = objOrderEntryM.Comment;
                    objTestResultM.ProtocalID = objOrderEntryM.ProtocolID;

                    dtOraganism = objConfiguration.GetDICTOrganisms();

                    for (int i = 0; i <= dtOraganism.Rows.Count - 1; i++)
                    {
                        objOrganismCode.Add(dtOraganism.Rows[i]["ORGANISMCODE"].ToString());
                        objOrganismName.Add(dtOraganism.Rows[i]["ORGANISMNAME"].ToString());
                    }
                    
                    if (objOrderEntryM.SpecimentCode == "")
                    {
                        label_specimen_req.Text = "N/A";
                    }
                    else
                    {
                        //Label_Control_SpecimenNo.Text = objOrderEntryM.SpecimentCode + "-" + objOrderEntryM.AccessNumber;
                        //lblSubrequst.Text = objOrderEntryM.SpecimentCode + "-" + objOrderEntryM.AccessNumber;
                        label_specimen_req.Text= objOrderEntryM.MBReqNumber;
                    }


                    lblAccessnumber.Text = objOrderEntryM.AccessNumber;
                    label_Rectube_Urgent.Text = "Request";
                    lblDoctor.Text = objOrderEntryM.ReqDoctor;
                    lblLocation.Text = objOrderEntryM.ReqLocation;

                    OrderCOMMENT.Text = objOrderEntryM.Comment;
                    lblAccessnumber.Text = objOrderEntryM.AccessNumber;
                    lblRequested.Text = objOrderEntryM.ReqDate.ToString();

                    lblCollected.Text = objOrderEntryM.ReceiveDate.ToString();
                    lblSpecimen.Text = objOrderEntryM.SpecimentCode;

                    if (objOrderEntryM.UrgentStatus == "1")
                    {
                        label_Rectube_Urgent.Text = "URGENT";
                        pictureBox1.Visible = true;
                    }
                    else
                    {
                        label_Rectube_Urgent.Text = "Routine";
                        pictureBox1.Visible = false;
                    }

                    culturelIndenAndSensitivity1.objTestResult = objTestResultM;
                    culturelIndenAndSensitivity1.BreakpointData =objBreakpointControl.GetAntoBreakpointAll();

                    dtExpertRule = culturelIndenAndSensitivity1.dtExpertDisplay;

                    if (dtExpertRule != null)
                    {
                        if (dtExpertRule.Rows.Count == 0)
                        {
                            pExpertrule.Visible = false;
                        }
                    }
                    else 
                    {
                        pExpertrule.Visible = false;
                    }
                   
                    selectStainInformation();

                    dtComment = objTestDAO.GetDICTComment();

                    cboComment.Properties.Items.Clear();

                    cboComment.Properties.Items.Add("");

                    for (int i = 0; i <= dtComment.Rows.Count - 1; i++) 
                    {
                        cboComment.Properties.Items.Add(dtComment.Rows[i]["CUSRESULTTEXT"].ToString ()); 
                    }

                    DataTable dtTemp = objTestController.GetComment(objOrderEntryM.MBRequestID);

                    if (dtTemp.Rows.Count > 0) 
                    {
                        if (Convert.ToInt64 (dtTemp.Rows[0]["COMMENTCODEID"].ToString()) != -99) 
                        {
                            dtComment.DefaultView.RowFilter = " CUSLISTID = " + Convert.ToInt64(dtTemp.Rows[0]["COMMENTCODEID"].ToString());
                            dtComment.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            if (dtComment.DefaultView.Count > 0) 
                            {
                                cboComment.SelectedItem = dtComment.DefaultView[0]["CUSRESULTTEXT"].ToString();
                                //cboComment.de .Properties.de .GetDisplayText(dtComment.DefaultView[0]["CUSRESULTTEXT"].ToString());
                                //cboComment.Properties.OwnerEdit.SelectedText = dtComment.DefaultView[0]["CUSRESULTTEXT"].ToString();
                            }

                        }
                        txtComment.Text = dtTemp.Rows[0]["COMMENTTEXT"].ToString();
                    }

                    dtTemp = null;

                    //* Load Iden and culture
                    dtExpertRule = culturelIndenAndSensitivity1.dtExpertDisplay;

                    if (dtExpertRule != null)
                    {
                        if (dtExpertRule.Rows.Count == 0)
                        {
                            pExpertrule.Visible = false;
                        }
                        else
                        {
                            GetDisplayExpertRule();
                            pExpertrule.Visible = true;
                        }
                    }
                    else
                    {
                        pExpertrule.Visible = false;
                    }

                    //cboComment.DrawItem

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objOrganismCode = null;
                objOrganismName = null;
                objBreakpointControl = null;
            }
        }

        public void GetDisplayExpertRule()
        {
            int StartNoX = 0;
            int StartNoY = 0;

            int StartDisplayX = 0;
            int StartDisplayY = 0;


            if (dtExpertRule.Rows.Count == 1)
            {
                lblNo.Text = dtExpertRule.Rows[0]["no"].ToString();
                lblDisplay.Text = dtExpertRule.Rows[0]["displaytext"].ToString();
            }
            else
            {
                for (int i = 0; i <= dtExpertRule.Rows.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        lblNo.Text = dtExpertRule.Rows[0]["no"].ToString();
                        lblDisplay.Text = dtExpertRule.Rows[0]["displaytext"].ToString();
                        StartNoX = lblNo.Location.X;
                        StartNoY = lblNo.Location.Y;
                        StartDisplayX = lblDisplay.Location.X;
                        StartDisplayY = lblDisplay.Location.Y;
                    }
                    else
                    {
                        System.Windows.Forms.Label lblNoCreate = new System.Windows.Forms.Label();
                        System.Windows.Forms.Label lblDisplayCreate = new System.Windows.Forms.Label();

                        StartDisplayY = StartDisplayY + 20;

                        lblNoCreate.Size = lblNo.Size;
                        lblNoCreate.Location = new Point(StartNoX, StartDisplayY);
                        lblNoCreate.Text = dtExpertRule.Rows[i]["no"].ToString();
                        lblNoCreate.Font = lblNo.Font;
                        lblNoCreate.ForeColor = lblNo.ForeColor;

                        lblDisplayCreate.Size = lblDisplayCreate.Size;
                        lblDisplayCreate.Location = new Point(StartDisplayX, StartDisplayY);
                        lblDisplayCreate.Font = lblDisplay.Font;
                        lblDisplayCreate.ForeColor = lblDisplay.ForeColor;
                        lblDisplayCreate.Width = lblDisplay.Width;
                        lblDisplayCreate.Text = dtExpertRule.Rows[i]["displaytext"].ToString();
                        lblDisplayCreate.AutoSize = true;

                        //lblNoCreate.Location = StartX +1;
                        pExpertrule.Controls.Add(lblNoCreate);
                        pExpertrule.Controls.Add(lblDisplayCreate);


                    }
                }
            }

            if (dtExpertRule.Rows.Count == 0)
            {
                pExpertrule.Visible = false;
            }
            else 
            {
                pExpertrule.Visible = true;
            }            
            //dtExpertDisplay
        }

        private void checkEnableButton()
        {
            btnAddStain.Enabled = true;
            btnAddObservations.Enabled = true;
            btnSave.Enabled = true;
            btnTechnical.Enabled = true;
            btnPreliminary.Enabled = true;
            btnFinal.Enabled = true;

            //if (gridView2.RowCount <= 0)
            //{
            //    btnAddStain.Enabled = false;
            //    btnAddObservations.Enabled = false;
            //    btnSave.Enabled = false;
            //    btnTechnical.Enabled = false;
            //    btnPreliminary.Enabled = false;
            //    btnFinal.Enabled = false;
            //}
            //else
            //{
            //    btnAddStain.Enabled = true;
            //    btnAddObservations.Enabled = true;
            //    btnSave.Enabled = true;
            //    btnTechnical.Enabled = true;
            //    btnPreliminary.Enabled = true;
            //    btnFinal.Enabled = true;
            //}
        }

        private void aaa()
        {
            //for (int i = 0; i < treeList1.AllNodesCount; i++)
            //{
            //  //  string topologyID = treeList1.Nodes[i].GetValue(0).ToString();
            //}
        }

        private void CreateNodes(TreeList tl)
        {
            tl.BeginUnboundLoad();
            // Create a root node .
            TreeListNode parentForRootNodes = null;
            TreeListNode rootNode = tl.AppendNode(
                new object[] { "Alfreds Futterkiste", "", "" },
                parentForRootNodes);
         
            
            // Create a child of the rootNode
            tl.AppendNode(new object[] { "Suyama, Michael", "Obere Str. 55", "030-0074263" }, rootNode);
            // Creating more nodes
            // ...
            tl.EndUnboundLoad();
   


        }

        private void CreateColumns(TreeList tl)
        {
           // // Create three columns.
           // tl.BeginUpdate();
           // TreeListColumn col1 = tl.Columns.Add();
           // col1.Caption = "Customer";
           // col1.VisibleIndex = 0;
           // TreeListColumn col2 = tl.Columns.Add();
           // col2.Caption = "Location";
           // riCombo = new RepositoryItemComboBox();
           // riCombo.Items.AddRange(new string[] { "San Francisco", "Monterey", "Toronto", "Boston", "Kuaui", "Singapore", "Tokyo" });
           // //Add the item to the internal repository
           // treeList1.RepositoryItems.Add(riCombo);
           // //Now you can define the repository item as an in-place column editor
           // col2.ColumnEdit = riCombo;
            
           // col2.VisibleIndex = 1;
           // TreeListColumn col3 = tl.Columns.Add();
           // col3.Caption = "Phone";
           // col3.VisibleIndex = 2;
           // tl.EndUpdate();
        

           // //if (tl.Nodes.ParentNode.Id > 0)
           // //{
            
          
           //// }
         
        }
        public void discount(ArrayList rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = (rows[i] as DataRowView).Row;
                decimal oldValue = (decimal)row["Price"];
                row["Price"] = oldValue * 0.95m;
            }
        }
        //Returns the child data rows for the given group row 
        public void getChildRows(GridView view, int groupRowHandle, ArrayList childRows)
        {
            if (!view.IsGroupRow(groupRowHandle)) return;
            //Get the number of immediate children 
            int childCount = view.GetChildRowCount(groupRowHandle);
            for (int i = 0; i < childCount; i++)
            {
                //Get the handle of a child row with the required index 
                int childHandle = view.GetChildRowHandle(groupRowHandle, i);
                //If the child is a group row, then add its children to the list 
                if (view.IsGroupRow(childHandle))
                    getChildRows(view, childHandle, childRows);
                else
                {
                    // The child is a data row.  
                    // Add it to the childRows as long as the row wasn't added before 
                    object row = view.GetRow(childHandle);
                    if (!childRows.Contains(row))
                        childRows.Add(row);
                }
            }
        }
        public void AddRow(DevExpress.XtraGrid.Views.Grid.GridView View)
        {

            int currentRow;

            currentRow = View.FocusedRowHandle;

            if (currentRow < 0)
            {

                currentRow = View.GetDataRowHandleByGroupRowHandle(currentRow);

            }

            View.AddNewRow();

            if (View.GroupedColumns.Count == 0)

                return;



            // Initialize group values 

            foreach (GridColumn groupColumn in View.GroupedColumns)
            {

                object value = View.GetRowCellValue(currentRow, groupColumn);

                View.SetRowCellValue(View.FocusedRowHandle, groupColumn, value);

            }

            View.UpdateCurrentRow();

            View.MakeRowVisible(View.FocusedRowHandle, true);

            View.ShowEditor();

        }

        private void tileNavPane1_Click(object sender, EventArgs e)
        {

        }

        private void treeList1_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
        {
           
        //      if(e.Column.FieldName != "Category") {
        //object obj = e.Node.GetValue(0);
        //if(obj != null) {
        //    switch (obj.ToString())
        //    {
        //        case "Location":
                   
        //            e.RepositoryItem = riCombo;

        //            break;
        //        case "Supplier":
        //         //   e.RepositoryItem = repositoryItemComboBox1;
        //            break;
        //        case "Unit Price":
        //            e.RepositoryItem = riCombo;
        //            break;
        //        case "Units in Stock":
        //            e.RepositoryItem = riCombo;
        //            break;
        //        case "Discontinued":
        //            e.RepositoryItem = riCombo;
        //            break;
        //        case "Last Order":
        //            e.RepositoryItem = riCombo;
        //            break;
        //        case "Relevance":
        //            e.RepositoryItem = riCombo;
        //            break;
        //        case "Phone":
        //            e.RepositoryItem = riCombo;
        //            break;
        //    }
        //    }
        //}
        }

        private void gridControl2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmStainList fm = new frmStainList();
            fm.actionAddNewStain = medAddNewStain;
            fm.ShowDialog();
            //DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
            //newRow["FULLTEXT"] = "India Ink";
            ////   newRow["MORPHOBSERVATION"] = "No Acid Fast Bacilli Seen";
            //(gridControl2.DataSource as DataTable).Rows.Add(newRow);
            //gridControl2.RefreshDataSource();
            //openStainList();
        }

        private void AddNewRowStain()
        {
            DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
            newRow["FULLTEXT"] = "India Ink";
            //   newRow["MORPHOBSERVATION"] = "No Acid Fast Bacilli Seen";
            (gridControl2.DataSource as DataTable).Rows.Add(newRow);
            gridControl2.RefreshDataSource();
            openStainList();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            addNewObservations();
        }

        private void addNewObservations()
        {            
          //  GridView view = sender as GridView;
            if (gridView2.SelectedRowsCount > 0)
            {
                int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
                DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(selRows[0]));
                string rootStain = selRow["STAINNAME"].ToString();

                DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
                newRow["STAINNAME"] = rootStain;
                newRow["MORPHOBSERVATION"] = "";
                newRow["SUBREQMBSTAINID"] = selRow["SUBREQMBSTAINID"].ToString();
                newRow["MBSTAINID"] = selRow["MBSTAINID"].ToString();
                newRow["SUBREQUESTID"] = selRow["SUBREQUESTID"].ToString();
                (gridControl2.DataSource as DataTable).Rows.Add(newRow);
                gridControl2.RefreshDataSource();
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void xtraTabPage2_Click(object sender, EventArgs e)
        {
            
        }

        private void xtraTabPage2_Paint(object sender, PaintEventArgs e)
        {
            //var myForm = new Culture_Identification();
            //// myForm.TopLevel = false;
            //myForm.AutoScroll = true;
            /////  panelControl1.Controls.Add(myForm);
            //xtraTabPage2.Controls.Add(myForm);
            //myForm.Show();
        }

//        private void gridView2_RowClick(object sender, RowClickEventArgs e)
//        {
       
//////            GridView view = sender as GridView;
//////            //if (gridView2.SelectedRowsCount > 0)
//////            //{
//////            //    int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
//////            //    DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(selRows[0]));
//////            //    string rootStain = selRow["FULLTEXT"].ToString();

//////            //    DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
//////            //    newRow["FULLTEXT"] = rootStain;
//////            //    newRow["MORPHOBSERVATION"] = "No Acid Fast Bacilli Seen";
//////            //    (gridControl2.DataSource as DataTable).Rows.Add(newRow);
//////            //    gridControl2.RefreshDataSource();

    
//////            //}
//////            if (gridView2.SelectedRowsCount > 0)
//////            {
//////                string[] strObservation = { };

//////                string sql = @"select DICT_CUS_RESULT_LIST.CUSRESULTTEXT from DICT_CUS_RESULT_LIST
//////LEFT OUTER JOIN DICT_CUS_RESULTS ON DICT_CUS_RESULT_LIST.CUSRESULTID = DICT_CUS_RESULTS.CUSRESULTID
//////LEFT OUTER JOIN RESULT_TYPE_PROPERTIES ON DICT_CUS_RESULTS.CUSRESULTID = RESULT_TYPE_PROPERTIES.CUSRESULTID
//////LEFT OUTER JOIN DICT_MB_STAINS ON DICT_MB_STAINS.MBSTAINID = RESULT_TYPE_PROPERTIES.MBSTAINID
//////WHERE RESULT_TYPE_PROPERTIES.PROPERTY = 0
//////AND DICT_MB_STAINS.MBSTAINCODE = 'GS'  ";
//////                SqlCommand cmd = new SqlCommand(sql, conn);
//////                SqlDataAdapter adap = new SqlDataAdapter(cmd);
//////                DataSet ds = new DataSet();
//////                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
//////                ds.Clear();
//////                adap.Fill(ds, "text");
//////                if (ds.Tables["text"].Rows.Count > 0)
//////                {
//////                    for (int i = 0; i < ds.Tables["text"].Rows.Count; i++)
//////                    {
//////                        strObservation[i] = ds.Tables["text"].Rows[i]["CUSRESULTTEXT"].ToString();
//////                    }
//////                }

//////                int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
//////                DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(selRows[0]));
//////                string rootStain = selRow["FULLTEXT"].ToString();

//////                if (rootStain.Contains("GMS"))
//////                {
//////                    riCombo = new RepositoryItemComboBox();
//////                //  riCombo.Items.AddRange(new string[] { "San Francisco", "Monterey", "Toronto", "Boston", "Kuaui", "Singapore", "Tokyo" });

//////                 //   riCombo.Items.AddRange(new string[]{ strObservation.ToString()});
//////                    riCombo.Items.Add( strObservation);
                 
//////                    //Add the item to the internal repository
//////                    gridControl2.RepositoryItems.Add(riCombo);
//////                    //Now you can define the repository item as an in-place column editor
//////                    colQUANTITATIVERESULT.ColumnEdit = riCombo;
//////                }
//////                else
//////                {
//////                    riCombo = new RepositoryItemComboBox();
//////                   // riCombo.Items.AddRange(new string[] { "a", "b", "c" });
                   
                  
//////                    //Add the item to the internal repository
//////                    gridControl2.RepositoryItems.Add(riCombo);
//////                    //Now you can define the repository item as an in-place column editor
//////                    colQUANTITATIVERESULT.ColumnEdit = riCombo;
//////                }
//////            }
//        }

        private void gridView2_MouseUp(object sender, MouseEventArgs e)
        {

        }

        //        private void gridView2_RowCellClick(object sender, RowCellClickEventArgs e)
        //        {
        //            ////int rowHandle = gridView2.FocusedRowHandle;


        //            ////if (e.Button == MouseButtons.Right)
        //            ////{
        ////object abc = gridView2.GetRowCellValue(rowHandle, "FULLTEXT");
        // //   MessageBox.Show(""+"abc");

        //            ////    contextMenuStrip1.Show(this.gridControl2, e.Location);
        //            ////    contextMenuStrip1.Show(Cursor.Position);


        //            ////}
        //        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //GridView view = sender as GridView;
            //view.DeleteRow(view.FocusedRowHandle);
            try
            {
                int rowHandle = gridView2.FocusedRowHandle;
                object subReqMBstainID = gridView2.GetRowCellValue(rowHandle, "SUBREQMBSTAINID");
                object morpho = gridView2.GetRowCellValue(rowHandle, "MORPHOBSERVATION");
                object morphoid = gridView2.GetRowCellValue(rowHandle, "MORPHOBSERVATIONID");

                StainObservationsForDeleteObservation observ = new StainObservationsForDeleteObservation();
                observ.RequestStainID = int.Parse(subReqMBstainID.ToString());
                observ.Morphoobservation = morpho.ToString();

                objTestStainResultM.SUBREQMBSTAINID= int.Parse(subReqMBstainID.ToString());
                objTestStainResultM.MORPHOBSERVATIONID = int.Parse(morphoid.ToString ());

                // Add data from class StainObservations to LIST.

                sListForDel.Add(observ);

                objTestController.DeleteStainStainObservation(objTestStainResultM);

                gridView2.DeleteRow(gridView2.FocusedRowHandle);


            }catch(Exception){
                int rowHandle = gridView2.FocusedRowHandle;     

                int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
                DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(rowHandle));
                  string rootStain = selRow["STAINNAME"].ToString();
                  string stain = selRow["SUBREQMBSTAINID"].ToString();
               //   MessageBox.Show("" + rootStain.ToString() + " stain : "+ stain);
                  StainObservationsForDeleteRequestStain obsDel = new StainObservationsForDeleteRequestStain();
                  obsDel.MbRequestID = int.Parse( stain.ToString());
                  sListForDelRequestMBStain.Add(obsDel);
                  gridView2.DeleteRow(gridView2.FocusedRowHandle);
            }

    
         


            
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridView2.AddNewRow();

           // int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
           // DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(selRows[0]));
           // string rootStain = selRow["FULLTEXT"].ToString();
           // //if (rootStain.Contains("GMS"))
           // //{
           // //GridView view = sender as GridView;
           // //view.SetRowCellValue(e.RowHandle, "Date", DateTime.Now.Date);


           // DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
           // newRow["FULLTEXT"] = rootStain;
           // newRow["MORPHOBSERVATION"] = "No Acid Fast Bacilli Seen";
           // (gridControl2.DataSource as DataTable).Rows.Add(newRow);
           // gridControl2.RefreshDataSource();
           // //}
        
        }

        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e)
        {

            ////MessageBox.Show("changed in gridview2");
            //int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
            //DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(selRows[0]));
            //string rootStain = selRow["STAINNAME"].ToString();
            ////string rootStain = "GMS";

            ////if (rootStain.Contains("GMS"))
            ////{
            ////GridView view = sender as GridView;
            ////view.SetRowCellValue(e.RowHandle, "Date", DateTime.Now.Date);

            //DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
            //newRow["STAINNAME"] = rootStain;
            //newRow["MORPHOBSERVATION"] = "No Acid Fast Bacilli Seen";
            //(gridControl2.DataSource as DataTable).Rows.Add(newRow);
            //gridControl2.RefreshDataSource();
            //}




        }

        private void addStainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openStainList();
        }

        private void openStainList()
        {
            frmStainList fm = new frmStainList();
            fm.actionAddNewStain = medAddNewStain;
            fm.ShowDialog();
        }
        private void medAddNewStain( )
        {
            //int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
            //DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(selRows[0]));
            //string rootStain = selRow["FULLTEXT"].ToString();
            ////if (rootStain.Contains("GMS"))
            ////{
            ////GridView view = sender as GridView;
            ////view.SetRowCellValue(e.RowHandle, "Date", DateTime.Now.Date);            
            //DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
            //newRow["FULLTEXT"] = rootStain;
            //newRow["MORPHOBSERVATION"] = "12344555";
            //(gridControl2.DataSource as DataTable).Rows.Add(newRow);
            //gridControl2.RefreshDataSource();

            if (ControlParameter.strStainControlID != "")
            {
                if (gridView2.RowCount > 0)
                {
                        int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
                    DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(selRows[0]));
                    string rootStain = selRow["STAINNAME"].ToString();

                    DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
                    newRow["STAINNAME"] = ControlParameter.strStainControlName;
                    newRow["SUBREQMBSTAINID"] = 0;
                    newRow["MBSTAINID"] = ControlParameter.strStainControlID;
                    //newRow["SUBREQUESTID"] = selRow["SUBREQUESTID"].ToString();
                    newRow["SUBREQUESTID"] = objOrderEntryM.MBRequestID.ToString();//lblRequestID.Text.Trim();
                    //   newRow["MORPHOBSERVATION"] = "No Acid Fast Bacilli Seen";                   
                    (gridControl2.DataSource as DataTable).Rows.Add(newRow);                                                              
                    gridControl2.RefreshDataSource();

                }
                else
                {// Gridview is null.
                    if (lblRequestID.Text.Trim() != "")
                    {
                        DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
                        newRow["STAINNAME"] = ControlParameter.strStainControlName;
                        newRow["MBSTAINID"] = ControlParameter.strStainControlID;
                        newRow["SUBREQUESTID"] = objOrderEntryM.MBRequestID.ToString ();//lblRequestID.Text.Trim();
                        (gridControl2.DataSource as DataTable).Rows.Add(newRow);
                        gridControl2.RefreshDataSource();
                    }
                }
            }
            else
            {
                // do someting.
            }
        }

       

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //int[] selActionRows = ((GridView)gridControl2.MainView).GetSelectedRows();
            //String sResult = "";


            ////DataRowView selActionRow = (DataRowView)(((GridView)grdDisplay.MainView).GetRow(selActionRows[0]));
            //ArrayList rows = new ArrayList();

            //// Add the selected rows to the list.
            //for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            //{
            //    if (gridView2.GetSelectedRows()[i] >= 0)
            //        rows.Add(gridView2.GetDataRow(gridView2.GetSelectedRows()[i]));
            //}
            //try
            //{
            //    gridView2.BeginUpdate();
            //    for (int i = 0; i < rows.Count; i++)
            //    {
            //        DataRow row = rows[i] as DataRow;
            //        sResult = row["QUANTITATIVERESULT"].ToString();
            //        row["QUANTITATIVERESULT"] = e.Value;
            //    }
                

            //}
            //catch (Exception ex)
            //{
            //    throw  new Exception(ex.Message);
            //}
            //finally 
            //{
            //    gridView2.EndUpdate();
            //}
            //var view = sender as GridView;


        }

        private void gridView2_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {

        }

        private void gridView2_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {

        }

        private void gridView2_RowUpdated(object sender, RowObjectEventArgs e)
        {
           // MessageBox.Show("ex");
         autoNewRowUnderStain();
        //GridView View = gridView2;
        //AddRow(gridView2);
        }

        private void autoNewRowUnderStain()
        {                   
          //  System.Data.DataRow row2 = gridView2.GetDataRow(gridView2.FocusedRowHandle);

         //   MessageBox.Show("row  " + row2["MORPHOBSERVATION"].ToString() );
           // row[0] = "New Value";

            try
            {
                int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
                DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(selRows[0]));
                string rootStain = selRow["STAINNAME"].ToString();

                if (gridView2.RowCount > 0)
                {
                    DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
                    newRow["STAINNAME"] = rootStain;
                    // newRow["STAINNAME"] = rootStain;
                    newRow["MORPHOBSERVATION"] = "";

                    if (selRow["SUBREQMBSTAINID"].ToString() != "")
                    {
                        newRow["SUBREQMBSTAINID"] = int.Parse(selRow["SUBREQMBSTAINID"].ToString());
                    }
                    else
                    {
                        newRow["SUBREQMBSTAINID"] = 0;
                    }
                    newRow["MBSTAINID"] = selRow["MBSTAINID"].ToString();
                    newRow["SUBREQUESTID"] = selRow["SUBREQUESTID"].ToString();
                    (gridControl2.DataSource as DataTable).Rows.Add(newRow);
                    gridControl2.RefreshDataSource();
                }
                else
                {
                    if (lblRequestID.Text.Trim() != "")
                    {
                        DataRow newRow = (gridControl2.DataSource as DataTable).NewRow();
                        newRow["STAINNAME"] = ControlParameter.strStainControlName;
                        newRow["MBSTAINID"] = ControlParameter.strStainControlID;
                        newRow["SUBREQUESTID"] = lblRequestID.Text.Trim();
                        (gridControl2.DataSource as DataTable).Rows.Add(newRow);
                        gridControl2.RefreshDataSource();
                    }
                }

                gridView2.UpdateCurrentRow();

                gridView2.MakeRowVisible(gridView2.FocusedRowHandle, true);

                gridView2.ShowEditor();
            }
            catch { };

         
        }
        //public void AddRow(DevExpress.XtraGrid.Views.Grid.GridView View) 
        //{

        //    int currentRow;

        //    currentRow = View.FocusedRowHandle;

        //    if (currentRow < 0)
        //    {

        //        currentRow = View.GetDataRowHandleByGroupRowHandle(currentRow);

        //    }

        //    View.AddNewRow();



        //    if (View.GroupedColumns.Count == 0)

        //        return;



        //    // Initialize group values 

        //    foreach (GridColumn groupColumn in View.GroupedColumns)
        //    {

        //        object value = View.GetRowCellValue(currentRow, groupColumn);

        //        View.SetRowCellValue(View.FocusedRowHandle, groupColumn, value);

        //    }

        //    View.UpdateCurrentRow();

        //    View.MakeRowVisible(View.FocusedRowHandle, true);

        //    View.ShowEditor();

        private void gridView2_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

         //   MessageBox.Show("TEST");
            loadComboQuantitativeResult();
        }

        private void gridView2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void gridView2_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtRequestSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               // selectRequestInformatons();
            }
        }

        //private void selectRequestInformatons()
        //{
        //    //string sql = @"SELECT distinct  rq.ACCESSNUMBER,rq.EXTERNALORDERNUMBER,submb.RECEIVEDDATE,rq.REQDATE,submb.COLLECTIONDATE,rq.COMMENT,rq.URGENT,submb.MBREQUESTID
        //    //,submb.MBREQNUMBER,dp.PROTOCOLCODE,dp.PROTOCOLTEXT,dp.REPORTFORMAT
        //    //,pt.PATNUMBER,pt.NAME,pt.LASTNAME,pt.BIRTHDATE,pt.ADDRESS1,pt.TELEPHON,pt.SEX,pt.EMAIL,pt.VIP,submb.MBREQEUSTSTATUS FROM REQUESTS rq
        //    //INNER JOIN MB_REQUESTS submb ON rq.REQUESTID = submb.REQUESTID
        //    //INNER JOIN REQ_TESTS rt ON rq.REQUESTID = rt.REQUESTID
        //    //INNER JOIN DICT_TESTS dt ON rt.TESTID = dt.TESTID
        //    //INNER JOIN DICT_MB_PROTOCOLS dp on submb.PROTOCOLID=dp.PROTOCOLID
        //    //LEFT OUTER JOIN PATIENTS pt  on rq.PATID = pt.PATID 
        //    //WHERE submb.MBREQNUMBER  = @MBREQNUMBER ";

        //    string sql = @"SELECT distinct  rq.ACCESSNUMBER,rq.EXTERNALORDERNUMBER,submb.RECEIVEDDATE,rq.REQDATE,submb.COLLECTIONDATE,rq.COMMENT,rq.URGENT,submb.MBREQUESTID
        //    ,submb.MBREQNUMBER,dp.PROTOCOLCODE,dp.PROTOCOLTEXT,dp.REPORTFORMAT
        //    ,pt.PATNUMBER,pt.NAME,pt.LASTNAME,pt.BIRTHDATE,pt.ADDRESS1,pt.TELEPHON,pt.SEX,pt.EMAIL,pt.VIP,submb.MBREQEUSTSTATUS FROM REQUESTS rq
        //    INNER JOIN MB_REQUESTS submb ON rq.REQUESTID = submb.REQUESTID
        //    INNER JOIN REQ_TESTS rt ON rq.REQUESTID = rt.REQUESTID
        //    INNER JOIN DICT_TESTS dt ON rt.TESTID = dt.TESTID
        //    INNER JOIN DICT_MB_PROTOCOLS dp on submb.PROTOCOLID=dp.PROTOCOLID
        //    LEFT OUTER JOIN PATIENTS pt  on rq.PATID = pt.PATID 
        //    WHERE rq.ACCESSNUMBER = @MBREQNUMBER and submb.MBREQNUMBER =  @ReqID";

        //    SqlCommand cmd = new SqlCommand(sql, conn);
        //    cmd.Parameters.Add("@MBREQNUMBER", SqlDbType.VarChar).Value = objOrderEntryM.AccessNumber;
        //    cmd.Parameters.Add("@ReqID", SqlDbType.VarChar).Value = objOrderEntryM.MBReqNumber;

        //    SqlDataAdapter adap = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();
        //    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
        //    ds.Clear();
        //    adap.Fill(ds, "request");
        //    if (ds.Tables["request"].Rows.Count > 0)
        //    {

        //        lblHostnum.DataBindings.Clear();
        //        lblHostnum.DataBindings.Add("TEXT",ds,"request.PATNUMBER");
        //        //   lblName.DataBindings.Clear();
        //        //lblName1.Text = ds.Tables["request"].Rows[0]["NAME"].ToString() + " " + ds.Tables["request"].Rows[0]["LASTNAME"].ToString();
        //        Label_Control_SpecimenNo.
        //        lblSubrequst.Text = ds.Tables["request"].Rows[0]["MBREQNUMBER"].ToString();
        //        lblLocation.Text = "";
        //        lblDoctor.Text = "";
        //        lblReqdate.Text = ds.Tables["request"].Rows[0]["REQDATE"].ToString();
        //        lblReceived.Text = ds.Tables["request"].Rows[0]["RECEIVEDDATE"].ToString();
        //        lblComment.Text = ds.Tables["request"].Rows[0]["COMMENT"].ToString();
        //        lblRequestID.Text = ds.Tables["request"].Rows[0]["MBREQUESTID"].ToString();
        //        try
        //        {
        //            if (ds.Tables["request"].Rows[0]["BIRTHDATE"].ToString() != "")
        //            {
        //                DateTime bDate = Convert.ToDateTime(ds.Tables["request"].Rows[0]["BIRTHDATE"].ToString());
        //                DateTime cDate = DateTime.Now;
        //                AgeCalculate age = new AgeCalculate(bDate, cDate);
        //                ////  Console.WriteLine("It's been {0} years, {1} months, and {2} days since your birthday", age.Years, age.Months, age.Days);
        //                lblAge1.Text = age.Years + " ปี " + age.Months + " เดือน " + age.Days + " วัน ";
        //                lblBirthDate1.Text = bDate.ToString("dd/MM/yyyy");

        //                selectStainInformation();
        //            }
        //        }
        //        catch (Exception)
        //        {

        //        }

        //        string reqStatus = ds.Tables["request"].Rows[0]["MBREQEUSTSTATUS"].ToString();
        //        if (reqStatus == "1")
        //        {
        //            lblStatus.Text = "[RC]";
        //        }

        //        string reportFormat = ds.Tables["request"].Rows[0]["REPORTFORMAT"].ToString();
        //        if (reportFormat == "1")
        //        {
        //            tabDirectExam.PageEnabled = false;
        //            tabIden.PageEnabled = true;
        //        }
        //        if (reportFormat == "2")
        //        {
        //            tabDirectExam.PageEnabled = true;
        //            tabIden.PageEnabled = false;                    
        //        }
        //    }
        //}

        private void selectStainInformation()
        {
            DataTable dt;
            DataRow dr;
            DataTable dtProtocolWithStain; 
            try
            {
                dt = objOrderEntry.GetRequestWithStain(objOrderEntryM.MBReqNumber);

                gridControl2.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    checkEnableButton();
                }
                else
                {
                    dtProtocolWithStain = objTestController.GetProtocalWithStain(Convert.ToInt16 (objOrderEntryM.ProtocolID));

                    if (dtProtocolWithStain.Rows.Count > 0)
                    {
                        objTestStainResultM.MBStainID = Convert.ToInt16 (dtProtocolWithStain.Rows[0]["MBSTAINID"].ToString());
                        objTestStainResultM.SubReqID = objOrderEntryM.MBRequestID;

                        for (int i = 1; i <= 5; i++) 
                        {
                            objTestController.SaveStainDirectExam(objTestStainResultM);
                        }
                        

                        dt = objOrderEntry.GetRequestWithStain(objOrderEntryM.MBReqNumber);

                        gridControl2.DataSource = dt;

                        
                    }                    

                }

                checkEnableButton();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally {
                dt = null;
            }
        }

        private void txtSpecimenSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //selectRequestInformatons();
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            //List<StainObservations> sList = new List<StainObservations>();
            List<StainTestResultM> sList = new List<StainTestResultM>();

            try
              {
                Cursor.Current = Cursors.WaitCursor;

                //tabIden.PageVisible = true;

                //if (xtraTabControl1.SelectedTabPage.Equals(tabIden))
                //{

                //}
                //else
                //{
                //int stainID = 0;
                //int reqMBStainID = 0;
                //int reqMBID = 0;
                //int subreqMbStainID = 0;
                //checkForDeleteStainObservationResult();
                //DataTable dt = ((System.Data.DataView)gridView2.DataSource).Table;

                culturelIndenAndSensitivity1.SaveData();

                for (int i = 0; i < gridView2.RowCount - 1; i++)
                    {
                        int rowHandle = gridView2.GetVisibleRowHandle(i);
                        DataRow row = gridView2.GetDataRow(i);
                     
                        //if (gridView2.IsGroupRow(rowHandle))
                        //{
                        objTestStainResultM.SUBREQMBSTAINID = 0; // ถ้าเจอ root ใหม่ ให้เคลียร์ค่า แล้ว เริ่ม หาใหม่ เพื่อที่จะได้ไม่ insert ผิด stain.
                                                                     //DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(rowHandle));
                        DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(i));

                        string rootStain = selRow["STAINNAME"].ToString();

                            objTestStainResultM.MBStainID = Convert.ToInt32(selRow["MBSTAINID"].ToString());

                            if (selRow["SUBREQMBSTAINID"].ToString() != "")
                            {
                                objTestStainResultM.SUBREQMBSTAINID = Convert.ToInt32(selRow["SUBREQMBSTAINID"].ToString());                                
                            }
                            else
                            {
                                objTestStainResultM.MBStainID = 0;
                            }

                            objTestStainResultM.SubReqID = Convert.ToInt32(selRow["SUBREQUESTID"].ToString());
                            objTestStainResultM.ColonyID = 0;

                            objTestStainResultM.MORPHOBSERVATION = selRow["MORPHOBSERVATION"].ToString();
                            objTestStainResultM.QUANTITATIVERESULT = selRow["QUANTITATIVERESULT"].ToString();
                            //selRow["QUANTITATIVERESULT"].ToString();
                            objTestStainResultM.COMMENTS = selRow["COMMENTS"].ToString();

                            //if (objTestStainResultM.QUANTITATIVERESULT == "") 
                            //{
                            //throw new Exception("cann't Save because you can not enter QUANTITATIVERESULT.");
                            //}

                            if (selRow["MBSTAINID"] != null)
                            {
                                if (selRow["MBSTAINID"].ToString() != "")
                                {
                                    objTestStainResultM.MBStainID = int.Parse(selRow["MBSTAINID"].ToString());
                                }
                            }

                            if (selRow["SUBREQMBSTAINID"].ToString() != "")
                            {
                                objTestStainResultM.SUBREQMBSTAINID = int.Parse(selRow["SUBREQMBSTAINID"].ToString());
                            }
                            else
                            {
                                objTestStainResultM.SUBREQMBSTAINID = 0;
                            }
                            if (selRow["MORPHOBSERVATION"].ToString() != "")
                            {
                                objTestStainResultM.MORPHOBSERVATION = selRow["MORPHOBSERVATION"].ToString();
                                objTestStainResultM.QUANTITATIVERESULT = selRow["QUANTITATIVERESULT"].ToString();
                                objTestStainResultM.COMMENTS = selRow["COMMENTS"].ToString();
                                objTestStainResultM.SubReqID = int.Parse(selRow["SUBREQUESTID"].ToString());
                            }
                            else
                            {
                                objTestStainResultM.MORPHOBSERVATION = "";
                            }



                        //                      }

                        if (objTestStainResultM.MBStainID != 0) 
                        {
                            if (objTestStainResultM.MORPHOBSERVATION != "") 
                            {
                                objTestController.SaveStainDirectExam(objTestStainResultM);
                                objTestController.SaveStainObservation(objTestStainResultM);
                            }
                        }

                        //if (gridView2.IsDataRow(rowHandle))
                        //{
                        //    objTestStainResultM.StainID = 0;
                        //    DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(rowHandle));

                        //    objTestStainResultM.MORPHOBSERVATION = selRow["MORPHOBSERVATION"].ToString();
                        //    objTestStainResultM.QUANTITATIVERESULT = selRow["QUANTITATIVERESULT"].ToString();
                        //    //selRow["QUANTITATIVERESULT"].ToString();
                        //    objTestStainResultM.COMMENTS = selRow["COMMENTS"].ToString();

                        //    if (selRow["MBSTAINID"] != null)
                        //    {
                        //        if (selRow["MBSTAINID"].ToString() != "")
                        //        {
                        //            objTestStainResultM.MBStainID = int.Parse(selRow["MBSTAINID"].ToString());
                        //        }
                        //    }

                        //    if (selRow["SUBREQMBSTAINID"].ToString() != "")
                        //    {
                        //        objTestStainResultM.SUBREQMBSTAINID = int.Parse(selRow["SUBREQMBSTAINID"].ToString());
                        //    }
                        //    else
                        //    {
                        //        objTestStainResultM.SUBREQMBSTAINID = 0;
                        //    }
                        //    if (selRow["MORPHOBSERVATION"].ToString() != "")
                        //    {
                        //        objTestStainResultM.MORPHOBSERVATION = selRow["MORPHOBSERVATION"].ToString();
                        //        objTestStainResultM.QUANTITATIVERESULT = selRow["QUANTITATIVERESULT"].ToString();
                        //        objTestStainResultM.COMMENTS = selRow["COMMENTS"].ToString();
                        //        objTestStainResultM.SubReqID = int.Parse(selRow["SUBREQUESTID"].ToString());
                        //    }
                        //    else
                        //    {
                        //        objTestStainResultM.MORPHOBSERVATION = "";
                        //    }

                        //    objTestController.SaveStainObservation(objTestStainResultM);
                        //}

                    //}
                }

                TestResultCommentM objTestCommentM = new TestResultCommentM();

                objTestCommentM.SUBREQUESTID = objOrderEntryM.MBRequestID;
                objTestCommentM.COMMENTCODEID = -99;
                objTestCommentM.CreateBy = ControlParameter.loginID;
                objTestCommentM.COMMENTTYPE = "-99";

                Boolean bComment = false;

                // Result Comment
                if (txtComment.Text != "") 
                {
                    objTestCommentM.COMMENTTEXT = txtComment.Text;
                    bComment = true;
                }

                if (cboComment.SelectedText != "") 
                {
                    dtComment.DefaultView.RowFilter = "CUSRESULTTEXT = '" + cboComment.SelectedText + "'";
                    dtComment.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    if (dtComment.DefaultView.Count > 0)
                    {
                        objTestCommentM.COMMENTCODEID = Convert.ToInt64(dtComment.DefaultView[0]["CUSLISTID"].ToString());
                    }
                    else 
                    {
                        objTestCommentM.COMMENTCODEID = -99;
                    }

                    bComment = true;
                }

                if (bComment == true) 
                {
                    objTestController.SaveComment(objTestCommentM);
                }

                objTestCommentM = null;

                objCommonControl.UpdateActionStatus("4", ControlParameter.loginName, ControlParameter.loginName, objOrderEntryM.MBRequestID.ToString() , objOrderEntryM.ProtocolID.ToString ());

                objTestController.UpdateReqTestStatus("10", objOrderEntryM.REQUESTID.ToString (), ControlParameter.loginName);

                Cursor.Current = Cursors.Default;

                MessageBox.Show("Record Success");

            }
             catch (Exception ex)
             {
                MessageBox.Show("Error Desc" + ex.Message);
             }

            //foreach (StainObservations obj in sList)
            //{
            //    if (obj.StainID == null)
            //    {
              
            //    }
            //    else if (obj.StainID != null)
            //    {
            //        if(obj.SubRequsetStainID != 0 && obj.Morphoobservation  != "")
            //        {
            //            if (!checkMorphoObservations(obj.Morphoobservation, obj.SubRequsetStainID))
            //            {
            //                string sql = @" INSERT INTO MB_STAINS_OBSERVATIONS (SUBREQMBSTAINID ,MORPHOBSERVATION, CREATEUSER,CREATIONDATE,QUANTITATIVERESULT,COMMENTS,LOGUSERID,LOGDATE ) 
            //                    VALUES (@SUBREQMBSTAINID, @MORPHOBSERVATION,@CREATEUSER,@CREATIONDATE,@QUANTITATIVERESULT,@COMMENTS,@LOGUSERID,@LOGDATE)";
            //                SqlCommand cmd = new SqlCommand(sql, conn);
            //                cmd.Parameters.Add("@SUBREQMBSTAINID", SqlDbType.Int).Value = obj.SubRequsetStainID;                     
            //                cmd.Parameters.Add("@MORPHOBSERVATION", SqlDbType.VarChar).Value = obj.Morphoobservation;
            //                cmd.Parameters.Add("@CREATEUSER", SqlDbType.VarChar).Value = "SYS";
            //                cmd.Parameters.Add("@CREATIONDATE", SqlDbType.DateTime).Value = DateTime.Now;
            //                cmd.Parameters.Add("@QUANTITATIVERESULT", SqlDbType.VarChar).Value = obj.QUANTITATIVERESULT;
            //                cmd.Parameters.Add("@COMMENTS", SqlDbType.VarChar).Value = obj.COMMENTS;
            //             //   cmd.Parameters.Add("@RESUPDDATE", SqlDbType.DateTime).Value = DateTime.Now;  //Firt insert is not update .
            //                cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
            //                cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
            //                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            //                cmd.ExecuteNonQuery();


            //                refReshGridAfterSave();
            //            }
            //            else
            //            {
            //                //update
            //                string sql = "UPDATE MB_STAINS_OBSERVATIONS SET MORPHOBSERVATION=@MORPHOBSERVATION, QUANTITATIVERESULT=@QUANTITATIVERESULT, COMMENTS=@COMMENTS, RESUPDDATE =@RESUPDDATE, LOGUSERID=@LOGUSERID, LOGDATE=@LOGDATE WHERE MORPHOBSERVATION = '" + obj.Morphoobservation + "' AND SUBREQMBSTAINID = '"+obj.SubRequsetStainID +"'";
            //                SqlCommand cmd = new SqlCommand(sql, conn);
            //                cmd.Parameters.Add("@MORPHOBSERVATION", SqlDbType.VarChar).Value = obj.Morphoobservation;
            //                cmd.Parameters.Add("@QUANTITATIVERESULT", SqlDbType.VarChar).Value = obj.QUANTITATIVERESULT;
            //                cmd.Parameters.Add("@COMMENTS", SqlDbType.VarChar).Value = obj.COMMENTS;
            //                cmd.Parameters.Add("@RESUPDDATE", SqlDbType.DateTime).Value = DateTime.Now;
            //                cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS2";
            //                cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
            //                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            //                cmd.ExecuteNonQuery();
            //                refReshGridAfterSave();
            //            }

            //        }// end of if SubRequsetStainID != 0 then insert new result of stainobservations.
            //        else
            //        {//INSERT SUBREQMB_STAIN first.
            //            if (obj.Morphoobservation != "")
            //            {

            //              // INSERT_INTO_SUBREQMB_STAIN(obj.StainID, obj.SubRequsetMbID, obj.Morphoobservation, obj.QUANTITATIVERESULT, obj.COMMENTS);
            //            }

            //        }
                    
                //}
               // MessageBox.Show(""+obj.SubRequsetStainID + "  "+ obj.Morphoobservation);
            //}
        }

        private void refReshGridAfterSave()
        {
            string sql = @"SELECT SUBREQMB_STAINS.SUBREQMBSTAINID,SUBREQMB_STAINS.SUBREQUESTID,SUBREQMB_STAINS.MBSTAINID,SUBREQMB_STAINS.VALREQUESTED,SUBREQMB_STAINS.CREATEUSER,SUBREQMB_STAINS.CREATIONDATE,SUBREQMB_STAINS.LOGUSERID,SUBREQMB_STAINS.LOGDATE,SUBREQMB_STAINS.NOTPRINTABLE,DICT_MB_STAINS.MBSTAINID,DICT_MB_STAINS.MBSTAINCODE,DICT_MB_STAINS.STAINNAME,DICT_MB_STAINS.DICTCONSOSTATUS, DICT_MB_STAINS.FIRSTITEMID,DICT_MB_STAINS.STARTVALIDDATE,DICT_MB_STAINS.ENDVALIDDATE,DICT_MB_STAINS.NOTPRINTABLE, MB_STAINS_OBSERVATIONS.SUBREQMBSTAINID,MB_STAINS_OBSERVATIONS.MORPHOBSERVATION,MB_STAINS_OBSERVATIONS.CREATEUSER,MB_STAINS_OBSERVATIONS.CREATIONDATE,MB_STAINS_OBSERVATIONS.QUANTITATIVERESULT,MB_STAINS_OBSERVATIONS.COMMENTS,MB_STAINS_OBSERVATIONS.RESUPDDATE,MB_STAINS_OBSERVATIONS.LOGUSERID AS 'LOGUSERID1',MB_STAINS_OBSERVATIONS.LOGDATE AS 'LOGDATE1',MB_STAINS_OBSERVATIONS.MORPHOBSERVATIONID
 FROM SUBREQMB_STAINS LEFT OUTER JOIN DICT_MB_STAINS 
 ON (SUBREQMB_STAINS.MBSTAINID = DICT_MB_STAINS.MBSTAINID )
  LEFT OUTER JOIN MB_STAINS_OBSERVATIONS 
  ON (SUBREQMB_STAINS.SUBREQMBSTAINID = MB_STAINS_OBSERVATIONS.SUBREQMBSTAINID ) 
  LEFT OUTER JOIN MB_REQUESTS ON SUBREQMB_STAINS.SUBREQUESTID = MB_REQUESTS.MBREQUESTID
 WHERE   MB_REQUESTS.MBREQNUMBER = '" + label_specimen_req.Text.Trim() + "'" +
 " ORDER BY MB_STAINS_OBSERVATIONS.MORPHOBSERVATIONID";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            adap.Fill(ds);
            gridControl2.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                checkEnableButton();
            }
        }

        private void checkForDeleteStainObservationResult()
        {
            foreach (StainObservationsForDeleteObservation obj in sListForDel)
            {
               // MessageBox.Show("" + obj.StainID + " Morpho : " + obj.Morphoobservation);
                string sql = "DELETE FROM MB_STAINS_OBSERVATIONS WHERE MORPHOBSERVATION='" + obj.Morphoobservation + "' AND SUBREQMBSTAINID = '" + obj.RequestStainID + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        refReshGridAfterSave();
                    }
                }
            }
//int delObServCOunt = 0;
//int MbRequestID = 0 ;
            foreach (StainObservationsForDeleteRequestStain obj in sListForDelRequestMBStain)
            {
                try
                {
                    string sql = "DELETE FROM SUBREQMB_STAINS WHERE  SUBREQMBSTAINID = '" + obj.MbRequestID + "'";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    {
                        if (cmd.ExecuteNonQuery() > 0)
                        {

                        }
                    }
                }catch{};
                //string sql2 = "DELETE FROM MB_STAINS_OBSERVATIONS WHERE SUBREQMBSTAINID = '" + obj.MbRequestID + "'";
                //SqlCommand cmd2 = new SqlCommand(sql2, conn);
                //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                //{
                //    if (cmd2.ExecuteNonQuery() > 0)
                //    {
                //        delObServCOunt++;
                //        MbRequestID = obj.MbRequestID;
                //    }
                //}           
               
            }
            //if (delObServCOunt > 0)
            //{
            //    string sql = "DELETE FROM SUBREQMB_STAINS WHERE  SUBREQMBSTAINID = '" + MbRequestID + "'";
            //    SqlCommand cmd = new SqlCommand(sql, conn);
            //    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            //    {
            //        if (cmd.ExecuteNonQuery() > 0)
            //        {

            //        }
            //    }
            //}
        }

        private void INSERT_INTO_SUBREQMB_STAIN(int StainID, int subReqMBID, string Morphoobservation, string QUANTITATIVERESULT,string comments)
        {
            int subreqMbStainID = 0;
            string sql = @"INSERT INTO SUBREQMB_STAINS (MBSTAINID,SUBREQUESTID,CREATEUSER,CREATIONDATE,LOGUSERID,LOGDATE) 
                                                    VALUES (@MBSTAINID,@SUBREQUESTID,@CREATEUSER,@CREATIONDATE,@LOGUSERID,@LOGDATE);SELECT CAST(scope_identity() AS int)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MBSTAINID", SqlDbType.Int).Value = StainID;
            cmd.Parameters.Add("@SUBREQUESTID", SqlDbType.Int).Value = subReqMBID;
            cmd.Parameters.Add("@CREATEUSER", SqlDbType.VarChar).Value = "SYS";
            cmd.Parameters.Add("@CREATIONDATE", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
            cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            subreqMbStainID =  Convert.ToInt32(cmd.ExecuteScalar());
            if (subreqMbStainID > 0)
            {
                string sqlStn = @" INSERT INTO MB_STAINS_OBSERVATIONS (SUBREQMBSTAINID ,MORPHOBSERVATION, CREATEUSER,CREATIONDATE,RESUPDDATE,QUANTITATIVERESULT,COMMENTS,LOGUSERID,LOGDATE ) 
                                                VALUES (@SUBREQMBSTAINID,@MORPHOBSERVATION,@CREATEUSER,@CREATIONDATE,@RESUPDDATE,@QUANTITATIVERESULT,@COMMENTS,@LOGUSERID,@LOGDATE)";
                SqlCommand cmdStn = new SqlCommand(sqlStn, conn);
                cmdStn.Parameters.Add("@SUBREQMBSTAINID", SqlDbType.Int).Value = subreqMbStainID;
                cmdStn.Parameters.Add("@MORPHOBSERVATION", SqlDbType.VarChar).Value = Morphoobservation;
                cmdStn.Parameters.Add("@CREATEUSER", SqlDbType.VarChar).Value = "SYS";
                cmdStn.Parameters.Add("@CREATIONDATE", SqlDbType.DateTime).Value = DateTime.Now;
                cmdStn.Parameters.Add("@RESUPDDATE", SqlDbType.DateTime).Value = DateTime.Now;
                cmdStn.Parameters.Add("@QUANTITATIVERESULT", SqlDbType.VarChar).Value = QUANTITATIVERESULT;               
                cmdStn.Parameters.Add("@COMMENTS", SqlDbType.VarChar).Value = comments;            
                cmdStn.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
                cmdStn.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                cmdStn.ExecuteNonQuery();

            }

          
        }

        private bool checkMorphoObservations(string strMopho, int SubRequsetStainID)
        {
            string sql = "SELECT MORPHOBSERVATION FROM MB_STAINS_OBSERVATIONS WHERE MORPHOBSERVATION = '" + strMopho + "' AND SUBREQMBSTAINID = '" + SubRequsetStainID + "'";
            SqlCommand cmd = new SqlCommand(sql,conn    );
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            ada.Fill(ds,"chk");
            if (ds.Tables["chk"].Rows.Count > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

       

        private void addObservationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addNewObservations();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (StainObservationsForDeleteObservation obj in sListForDel)
            {
               // MessageBox.Show(""+ obj.RequestStainID+ " Morpho : "+ obj.Morphoobservation);
            }
        }

        private void culture_Identification1_Load(object sender, EventArgs e)
        {

        }

        private void txtRequestSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnPreliminary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (MessageBox.Show ("Do you confirm Preliminary?" , "Preliminary" ,MessageBoxButtons.YesNo) == DialogResult.Yes ) 
                {
                    objCommonControl.UpdateActionStatus(1.ToString(), ControlParameter.loginName, ControlParameter.loginName, objOrderEntryM.MBRequestID.ToString(), objOrderEntryM.ProtocolID.ToString());
                    this.Close();
                }
                
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }

        private void btnTechnical_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you confirm Technical?", "Technical", MessageBoxButtons.YesNo) == DialogResult.Yes) 
                {
                    objCommonControl.UpdateActionStatus(3.ToString(), ControlParameter.loginName, ControlParameter.loginName, objOrderEntryM.MBRequestID.ToString(), objOrderEntryM.ProtocolID.ToString());
                    this.Close();
                }
                    
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            
        }

        private void btnFinal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you confirm Final?", "Final", MessageBoxButtons.YesNo) == DialogResult.Yes) 
                {
                    objCommonControl.UpdateActionStatus(2.ToString(), ControlParameter.loginName, ControlParameter.loginName, objOrderEntryM.MBRequestID.ToString(), objOrderEntryM.ProtocolID.ToString());
                    this.Close();
                }
                    
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error:" + ex.Message);                
            }
        }
            
        private void txtOrganismCode_KeyDown(object sender, KeyEventArgs e)
        {
            //try
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        //EventTextOraganismCodeClick();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error Desc:" + ex.Message);
            //}
        }

        //private void EventTextOraganismCodeClick()
        //{
        //    try
        //    {
        //        String OraganismCode = txtOrganismCode.Text;

        //        dtOraganism.DefaultView.RowFilter = " ORGANISMCODE = '" + OraganismCode + "'";
        //        dtOraganism.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

        //        txtOrganismName.Text = dtOraganism.DefaultView[0]["ORGANISMNAME"].ToString();

        //        btnAdd.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message); 
        //    }
        //}

        private void EventTextOraganismNameClick()
        {
            //try
            //{
            //    String OraganismName = txtOrganismName.Text;

            //    dtOraganism.DefaultView.RowFilter = " ORGANISMNAME = '" + OraganismName + "'";
            //    dtOraganism.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

            //    txtOrganismCode.Text = dtOraganism.DefaultView[0]["ORGANISMCODE"].ToString();

            //    btnAdd.Focus();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }

        private void txtOrganismName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    EventTextOraganismNameClick();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt ;

            try
            {
                //if (txtOrganismCode.Text != "")
                //{

                //    dt = culture_Identification1.OraganismData;

                //    if (dt == null)
                //    {
                //        dt = new DataTable();
                //        dt.Columns.Add("ORGANISMCODE");
                //        dt.Columns.Add("QUANTITATIVE");
                //        dt.Columns.Add("LOGDATE");
                //        dt.Columns.Add("COMMENTS");
                //        dt.Columns.Add("LOGUSER");
                //    }

                //    DataRow dr = dt.NewRow();
                //    dr["ORGANISMCODE"] = txtOrganismCode.Text;
                //    dr["LOGDATE"] = DateTime.Now.ToString ();
                //    dr["LOGUSER"] = ControlParameter.ControlUser.USERNAME;

                //    dt.Rows.Add (dr);

                //    culture_Identification1.OraganismData = dt;
                //    //culture_Identification1.RefreshData()

                //}
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
        }

        private void txtOrganismCode_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void gridView2_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                contextMenuStrip1.Show(this.gridControl2, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (pExpertrule.Visible == true)
            {
                pExpertrule.Visible = false;
            }
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Name == tabIden.Name) 
            {
                dtExpertRule = culturelIndenAndSensitivity1.dtExpertDisplay;

                if (dtExpertRule != null)
                {
                    if (dtExpertRule.Rows.Count == 0)
                    {
                        pExpertrule.Visible = false;
                    }
                    else 
                    {
                        GetDisplayExpertRule();
                        pExpertrule.Visible = true;
                    }
                }
                else
                {
                    pExpertrule.Visible = false;
                }
            }
        }

        private void gridView2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //int[] selActionRows = ((GridView)gridControl2.MainView).GetSelectedRows();
            //String sResult = "";

            //BandedGridView view =  sender as gridView2;

            if (e.Column.FieldName == "QUANTITATIVERESULT")
            {
                gridView2.SetRowCellValue(e.RowHandle, gridView2.Columns["QUANTITATIVERESULT"], e.Value);
            }
            else 
            { 
            }
            
            ////DataRowView selActionRow = (DataRowView)(((GridView)grdDisplay.MainView).GetRow(selActionRows[0]));
            //ArrayList rows = new ArrayList();

            //DataTable dt = ((System.Data.DataView)gridView2.DataSource).Table;

            //// Add the selected rows to the list.
            //for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            //{
            //    if (gridView2.GetSelectedRows()[i] >= 0)
            //        rows.Add(gridView2.GetDataRow(gridView2.GetSelectedRows()[i]));
            //}

            //try
            //{
            //    //gridView2.BeginUpdate();
            //    DataRow row = rows[0] as DataRow;
                
            //    dt.DefaultView.RowFilter = "MORPHOBSERVATIONID = " + row["MORPHOBSERVATIONID"].ToString();
            //    dt.DefaultView[0][e.Column.FieldName.ToString()] = e.Value;
            //    dt.DefaultView.RowFilter = String.Empty;


            //    //    //dt.DefaultView[0]
            //    //    //row["QUANTITATIVERESULT"] = e.Value;
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    throw new Exception(ex.Message);
            //    //}
            //    //finally
            //    //{
            //    //    dt.DefaultView.RowFilter = String.Empty;
            //    //    //gridView2.EndUpdate();
            //    //}

        }

    }

    public class ControlModelItem
    {
        public ControlModelItem(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public override string ToString()
        {
            return Value;
        }
    }

}
