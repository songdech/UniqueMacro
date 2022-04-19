using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using UniquePro.Controller;
using UniquePro.Entities.Testing;
using UniquePro.Entities.Configuration;
using UniquePro.Entities.Common;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System.Collections;
using DevExpress.XtraEditors.Repository;
using DevExpress.CodeParser;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.ViewInfo;

namespace UNIQUE.Result
{
    public partial class CulturelIndenAndSensitivity : DevExpress.XtraEditors.XtraUserControl
    {
        frmStain frmParent;

        public DataTable TestResultData { get; set; }

        RepositoryItemComboBox riComboResult;
        public TestResultM objTestResult { get; set; }

        public TestColonyResultM TestColonyResultSelect;

        public TestAgarsResultM TestAgarResultSelect;

        private ArrayList oldrows ;

        public DataTable MediaTestingResult { get; set; }

        private TestColonyResultM objColonyM;

        private TestResultM objTestResultM;
        
        public DataTable BreakpointData { get; set; }

        public DataTable DICTOrganism { get; set; }

        public TestController objTesting;
        public SensitivitiesController objSensitivities;

        private DataTable dtCultureData;
        private DataTable dtAntibioticBreakpoint;
        private DataTable dtMedia;
        private DataTable dtColonyData;

        private RulesBaseController objRuleBase;

        private DataTable dtRulesBase;
        //private DataTable dtExpertDisplay;
        public DataTable dtExpertDisplay { get; set; }

        //public DataTable dtExpertDisplay { get; set; }


        private const String ProgramID = "1";

        RepositoryItemComboBox riComboDetectionTest;
        RepositoryItemComboBox riCombo;

        private ActionTypeM objActionTypeM;

        private Boolean bSelectColony;

        public CulturelIndenAndSensitivity()
        {
            InitializeComponent();
            frmParent = (frmStain)this.ParentForm;
        }

        private void loadComboDetectionTest()
        {
            if (gridView1.SelectedRowsCount > 0)
            {
                string[] strObservation = { };
                ConfigurationController objConfig = new ConfigurationController();

                //List<string> observation = new List<string>();
                List<string> quantitative = new List<string>();

                int[] selRows = ((GridView)grdDisplay.MainView).GetSelectedRows();
                DataRowView selRow = (DataRowView)(((GridView)grdDisplay.MainView).GetRow(selRows[0]));
                //String Action = selRow["action"].ToString();
                if (selRow != null)
                {
                    String Action = selRow["actiontype"].ToString();

                    if (Action == objActionTypeM.ActionType_DetectionTest)
                    {
                        int DetectionID = Convert.ToInt16(selRow["DETECTIONID"].ToString());

                        //CommonController objCommon = new CommonController();
                        //TestController objTestCon = new TestController();
                        DetectionTestM objDetectionTest = new DetectionTestM();
                        objDetectionTest.DETECTION_ID = DetectionID.ToString();

                        //DataTable dt = objTestCon.GetResultDetectionTest(DetectionID);
                        DataTable dt = objTesting.GetGridDetectionTestResult(objDetectionTest);

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                //if (dt.Rows[i]["PROPERTY"].ToString() == "1")
                                //{
                                quantitative.Add(dt.Rows[i]["CUSRESULTTEXT"].ToString());  //Stain
                                                                                           //}
                            }
                        }

                        riComboDetectionTest = new RepositoryItemComboBox();
                        //riCombo = new RepositoryItemComboBox();                    
                        //riCombo.Items.AddRange(quantitative);

                        riComboDetectionTest.Items.AddRange(quantitative);
                        //      riCombo.Items.Add(ds.Tables["text"].Rows);

                        //Add the item to the internal repository
                        //gridControl2.RepositoryItems.Add(riCombo);
                        grdDisplay.RepositoryItems.Add(riComboDetectionTest);
                        //Now you can define the repository item as an in-place column editor
                        //colQUANTITATIVERESULT.ColumnEdit = riCombo;

                        cultureResult.ColumnEdit = riComboDetectionTest;

                        riComboDetectionTest.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                    }
                    else
                    {
                        if (Action == objActionTypeM.ActionType_Biochemistry)
                        {
                            int ChemistryID = Convert.ToInt16(selRow["CHEMISTRYID"].ToString());

                            //CommonController objCommon = new CommonController();
                            //TestController objTestCon = new TestController();
                            ChemistryM objChemistryM = new ChemistryM();

                            objChemistryM.CHEMISTRY_ID = ChemistryID.ToString();

                            //DataTable dt = objTestCon.GetResultDetectionTest(DetectionID);
                            DataTable dt = objTesting.GetChemistryResult(objChemistryM);

                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    //if (dt.Rows[i]["PROPERTY"].ToString() == "1")
                                    //{
                                    quantitative.Add(dt.Rows[i]["FULLTEXT"].ToString());  //Stain
                                                                                          //}
                                }
                            }

                            riComboDetectionTest = new RepositoryItemComboBox();
                            //riCombo = new RepositoryItemComboBox();                    
                            //riCombo.Items.AddRange(quantitative);

                            riComboDetectionTest.Items.AddRange(quantitative);
                            //      riCombo.Items.Add(ds.Tables["text"].Rows);

                            //Add the item to the internal repository
                            //gridControl2.RepositoryItems.Add(riCombo);
                            grdDisplay.RepositoryItems.Add(riComboDetectionTest);
                            //Now you can define the repository item as an in-place column editor
                            //colQUANTITATIVERESULT.ColumnEdit = riCombo;

                            cultureResult.ColumnEdit = riComboDetectionTest;

                            riComboDetectionTest.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                        }
                    }
                }


            }
        }

        private void CulturelIndenAndSensitivity_Load(object sender, EventArgs e)
        {
            BeginingData();
            bSelectColony = false;
            //pExpertrule.Visible = false;
        }

        public void BeginingData()
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            DataTable dt;

            try
            {
                objTesting = new TestController();
                objSensitivities = new SensitivitiesController();

                MenuAction.Items[0].Enabled = false;

                objActionTypeM = new ActionTypeM();

                dtMedia = new DataTable();

                dtMedia.Columns.Add("REQUESTID");
                dtMedia.Columns.Add("AGARID");
                dtMedia.Columns.Add("AGARCODE");
                dtMedia.Columns.Add("AGARNAME");
                dtMedia.Columns.Add("AGARINDEX");
                dtMedia.Columns.Add("CREATEUSER");
                dtMedia.Columns.Add("CREATIONDATE");
                dtMedia.Columns.Add("SUBREQMBAGARID");
                dtMedia.Columns.Add("LOGUSERID");
                dtMedia.Columns.Add("LOGDATE");
                dtMedia.Columns.Add("SUBREQUESTID");

                dtColonyData = new DataTable();

                dtColonyData.Columns.Add("LOGUSERID");
                dtColonyData.Columns.Add("LOGDATE");
                dtColonyData.Columns.Add("NOTPRINTABLE");
                dtColonyData.Columns.Add("CONSOLIDATIONSTATUS");
                dtColonyData.Columns.Add("COLONYNUMBER");
                dtColonyData.Columns.Add("COMMENTS");
                dtColonyData.Columns.Add("COLONYID");
                dtColonyData.Columns.Add("COLONYINDEX");
                dtColonyData.Columns.Add("FULLTEXT");
                dtColonyData.Columns.Add("ORGANISMID");
                dtColonyData.Columns.Add("AGARID");


                dtCultureData = new DataTable();

                dtCultureData.Columns.Add("datetime");
                dtCultureData.Columns.Add("LOGUSERID");
                dtCultureData.Columns.Add("action");
                dtCultureData.Columns.Add("actiondetail");
                dtCultureData.Columns.Add("Comment");
                dtCultureData.Columns.Add("COLONYNUMBER");
                dtCultureData.Columns.Add("SUBREQMBAGARID");
                dtCultureData.Columns.Add("COLONYID");
                dtCultureData.Columns.Add("ORGANISMID");
                dtCultureData.Columns.Add("SENSITIVITYID");
                dtCultureData.Columns.Add("METHODID");
                dtCultureData.Columns.Add("BATTERYID");
                dtCultureData.Columns.Add("DETECTIONID");
                dtCultureData.Columns.Add("MICID");
                dtCultureData.Columns.Add("actiontype");
                dtCultureData.Columns.Add("CHEMISTRYID");
                dtCultureData.Columns.Add("SUBREQMBORGID");
                dtCultureData.Columns.Add("SUBREQMBSENSITIVITYID");
                dtCultureData.Columns.Add("PRINTSTATUS");

                MediaTestingResult = objTesting.GetAgarHeader(objTestResult);

                if (MediaTestingResult.Rows.Count == 0)
                {
                    CreateNode(dtMedia, "NEW");
                }
                else
                {
                    for (int i = 0; i <= MediaTestingResult.Rows.Count - 1; i++)
                    {
                        DataRow dr = dtMedia.NewRow();
                        for (int j = 0; j <= MediaTestingResult.Columns.Count - 1; j++)
                        {
                            switch (MediaTestingResult.Columns[j].ColumnName)
                            {
                                case "REQUESTID":
                                    dr[MediaTestingResult.Columns[j].ColumnName] = MediaTestingResult.Rows[i][j].ToString();
                                    break;
                                case "AGARID":
                                    dr[MediaTestingResult.Columns[j].ColumnName] = MediaTestingResult.Rows[i][j].ToString();
                                    break;
                                case "CREATEUSER":
                                    dr[MediaTestingResult.Columns[j].ColumnName] = MediaTestingResult.Rows[i][j].ToString();
                                    break;
                                case "AGARCODE":
                                    dr[MediaTestingResult.Columns[j].ColumnName] = MediaTestingResult.Rows[i][j].ToString();
                                    break;
                                case "AGARNAME":
                                    dr[MediaTestingResult.Columns[j].ColumnName] = MediaTestingResult.Rows[i][j].ToString();
                                    break;
                                case "AGARINDEX":
                                    dr[MediaTestingResult.Columns[j].ColumnName] = MediaTestingResult.Rows[i][j].ToString();
                                    break;
                                case "LOGUSERID":
                                    dr[MediaTestingResult.Columns[j].ColumnName] = MediaTestingResult.Rows[i][j].ToString();
                                    break;
                                case "SUBREQMBAGARID":
                                    dr[MediaTestingResult.Columns[j].ColumnName] = MediaTestingResult.Rows[i][j].ToString();
                                    break;
                                case "LOGDATE":
                                    dr[MediaTestingResult.Columns[j].ColumnName] = MediaTestingResult.Rows[i][j].ToString();
                                    break;
                                case "SUBREQUESTID":
                                    dr[MediaTestingResult.Columns[j].ColumnName] = MediaTestingResult.Rows[i][j].ToString();
                                    break;

                            }

                        }

                        dtMedia.Rows.Add(dr);

                        dr = null;

                    }

                    CreateNode(dtMedia, "SAVE");

                }

                dtColonyData = objTesting.GetColonyData(objTestResult);

                dtCultureData = objTesting.GetCultureDataWithTestResult(objTestResult);

                dtAntibioticBreakpoint = objTesting.GetAntibioticsTestResult(objTestResult);

                if (dtAntibioticBreakpoint.Rows.Count == 0)
                {
                    dtAntibioticBreakpoint = null;

                    dtAntibioticBreakpoint = new DataTable();

                    dtAntibioticBreakpoint.Columns.Add("FULLTEXT");
                    dtAntibioticBreakpoint.Columns.Add("RESULT");
                    dtAntibioticBreakpoint.Columns.Add("RESULTVALUE");
                    dtAntibioticBreakpoint.Columns.Add("CONCLOWVALUE");
                    dtAntibioticBreakpoint.Columns.Add("CONCHIGHVALUE");
                    dtAntibioticBreakpoint.Columns.Add("THRESHOLDLOWER");
                    dtAntibioticBreakpoint.Columns.Add("THRESHOLDHIGHER");
                    dtAntibioticBreakpoint.Columns.Add("METHODMBCODE");
                    dtAntibioticBreakpoint.Columns.Add("BREAKPOINT");
                    dtAntibioticBreakpoint.Columns.Add("UNITS");
                    dtAntibioticBreakpoint.Columns.Add("SENSITIVITYID");
                    dtAntibioticBreakpoint.Columns.Add("ANTIBIOTICID");
                    dtAntibioticBreakpoint.Columns.Add("SUBREQMBORGID");
                    dtAntibioticBreakpoint.Columns.Add("COLONYID");
                    dtAntibioticBreakpoint.Columns.Add("COLONYNUMBER");
                    dtAntibioticBreakpoint.Columns.Add("METHODID");
                    dtAntibioticBreakpoint.Columns.Add("MICID");
                    dtAntibioticBreakpoint.Columns.Add("PRINTSTATUS");
                    dtAntibioticBreakpoint.Columns.Add("OLDRESULT");
                    dtAntibioticBreakpoint.Columns.Add("SUBREQMBSENSITIVITYID");

                    //dtAntibioticBreakpoint.Columns.Add("SUBREQMBORGID");

                }

                dtExpertDisplay = new DataTable();

                dtExpertDisplay.Columns.Add("no");
                dtExpertDisplay.Columns.Add("displaytext");

                dtCultureData.DefaultView.RowFilter = String.Empty;
                dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                dtAntibioticBreakpoint.DefaultView.RowFilter = String.Empty;
                dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                CultueDataBind.DataSource = dtCultureData;

                grdAnti.DataSource = dtAntibioticBreakpoint;

                objRuleBase = new RulesBaseController();

                dtRulesBase = objRuleBase.GetRulesBase(Convert.ToInt64(ProgramID));
                //pExpertrule.Visible = false;
                CalculateExpertRule();

                CreateConditionRules();

                objRuleBase = null;

                GridStyle();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objConfiguration = null;
            }

        }

        private void GridStyle()
        {
            RepositoryItemTextEdit te = new RepositoryItemTextEdit();
            grdDisplay.RepositoryItems.Add(te);
            //te.
            gridView1.Columns["PRINTSTATUS"].ColumnEdit = te;
            gridView5.Columns["PRINTSTATUS"].ColumnEdit = te;
            
            //te.ContextImage = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_32x32.png");
        }

        private void CreateConditionRules()
        {
            gridView1.FormatConditions.Clear();
            gridView1.FormatRules.Clear();

            GridFormatRule gridFormatRule = new GridFormatRule();
            //FormatConditionRuleExpression formatConditionRuleExpression2 = new FormatConditionRuleExpression();            

            //GridFormatRule gridFormatRule = new GridFormatRule();

            FormatConditionRuleIconSet formatConditionRuleIconSet = new FormatConditionRuleIconSet();
            FormatConditionIconSet iconSet = formatConditionRuleIconSet.IconSet = new FormatConditionIconSet();
            FormatConditionIconSetIcon icon1 = new FormatConditionIconSetIcon();
            FormatConditionIconSetIcon icon2 = new FormatConditionIconSetIcon();
            FormatConditionIconSetIcon icon3 = new FormatConditionIconSetIcon();

            //Choose predefined icons.
            icon1.PredefinedName = "Symbols3_1.png";
            icon2.PredefinedName = "Symbols3_2.png";
            icon3.PredefinedName = "Symbols3_3.png";

            //Specify the type of threshold values.
            iconSet.ValueType = FormatConditionValueType.Number;

            //Define ranges to which icons are applied by setting threshold values.
            icon1.Value = 0; // target range: 67% <= value
            icon1.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            icon2.Value = 1; // target range: 33% <= value < 67%
            icon2.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            icon3.Value = 2; // target range: 0% <= value < 33%
            icon3.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;

            //Add icons to the icon set.
            iconSet.Icons.Add(icon1);
            iconSet.Icons.Add(icon2);
            iconSet.Icons.Add(icon3);

            gridFormatRule.Column = PrintStatus;
            //Add the formatting rule to the GridView.
            //gridView1.FormatRules.Add(gridFormatRule);
            gridFormatRule.ApplyToRow = true;
            gridFormatRule.Rule = formatConditionRuleIconSet;
            gridView1.FormatRules.Add(gridFormatRule);


        }


        private TestResultM GetDatatableToObjectTestResult(DataTable dt, TestResultM objResultM)
        {
            TestAgarsResultM objAgar = null;
            TestColonyResultM objColony = null;
            TestResultM objResult = new TestResultM();

            try
            {

                var query = from row in dt.AsEnumerable()
                            group row by row.Field<int>("AGARID") into grp
                            select new
                            {
                                Id = grp.Key
                            };

                foreach (var grp in query)
                {
                    objAgar = new TestAgarsResultM();

                    dt.DefaultView.RowFilter = " AGARID = " + grp.Id;
                    dt.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    objAgar.AgarID = Convert.ToInt16(grp.Id);

                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objColony = new TestColonyResultM();

                        objAgar.SubReqMBAgarID = Convert.ToInt16(dt.DefaultView[i]["SUBREQMBAGARID"]);
                        objAgar.AgarNumber = dt.DefaultView[i]["AGARNUMBER"].ToString();
                        objAgar.AgarIndex = Convert.ToInt32(dt.DefaultView[i]["AGARINDEX"].ToString());
                        objAgar.Status = dt.DefaultView[i]["CONSOLIDATIONSTATUS"].ToString();
                        objAgar.CreateBy = dt.DefaultView[i]["CREATEUSER"].ToString();
                        objAgar.NOTPRINTABLE = dt.DefaultView[i]["NOTPRINTABLE"].ToString();

                        objColony.ColonyID = Convert.ToInt16(dt.DefaultView[i]["COLONYID"]);
                        objColony.ColonyIndex = Convert.ToInt16(dt.DefaultView[i]["COLONYINDEX"]);
                        objColony.ColonyNumber = dt.DefaultView[i]["COLONYNUMBER"].ToString();
                        objColony.ColonyComment = dt.DefaultView[i]["COMMENTS"].ToString();

                        if (objAgar.Colonies == null)
                        {
                            objAgar.Colonies = new List<TestColonyResultM>();
                        }

                        objAgar.Colonies.Add(objColony);

                        objColony = null;

                    }

                    if (objResult.TestResultAkars == null)
                    {
                        objResult.TestResultAkars = new List<TestAgarsResultM>();
                    }

                    objResult.TestResultAkars.Add(objAgar);

                }

                return objResult;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void addBloodAgarMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void CreateNode(DataTable dt, String Status)
        {
            DataTable dtOrganism;
            DataTable dtColony;
            int val = 64;
            Boolean bNext = false;

            try
            {
                objTestResultM = null;

                objTestResultM = new TestResultM();

                dtColony = objTesting.GetMediaTestingWithRequestnumber(objTestResult, 0, 0);

                if (Status == "NEW")
                {
                    if (tMain.SelectedNode == null)
                    {
                        bNext = true;
                    }
                    else
                    {
                        if (tMain.SelectedNode.IsSelected == true)
                        {
                            if (tMain.SelectedNode.Level == 0)
                            {
                                dt.DefaultView.RowFilter = " AGARNAME = '" + tMain.SelectedNode.Text + "'";
                                dt.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                if (dt.DefaultView.Count > 0)
                                {
                                    dtColonyData.DefaultView.RowFilter = "AGARID = '" + dt.DefaultView[0]["AGARID"].ToString() + "'";
                                    dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                    if (dtColonyData.DefaultView.Count > 0)
                                    {
                                        Boolean bHave = false;

                                        TreeNode nChild = tMain.Nodes[dt.DefaultView[0]["AGARID"].ToString()];

                                        for (int i = 0; i <= dtColonyData.DefaultView.Count - 1; i++)
                                        {
                                            bHave = false;

                                            for (int j = 0; j <= nChild.Nodes.Count - 1; j++)
                                            {
                                                if (nChild.Nodes[j].Text.Contains(dtColonyData.DefaultView[i]["COLONYNUMBER"].ToString()) == true)
                                                {
                                                    bHave = true;
                                                    break;
                                                }
                                            }
                                            if (bHave == false)
                                            {
                                                String sColonyTemp = dtColonyData.DefaultView[i]["COLONYNUMBER"].ToString();

                                                String sTmp = DateTime.Now.ToString("ddMMyyyyHH:mm:ss");

                                                sColonyTemp = "(" + sColonyTemp + "-" + sTmp + ")";

                                                nChild.Nodes.Add(dtColonyData.DefaultView[i]["COLONYNUMBER"].ToString(), sColonyTemp, 5);
                                                nChild.Nodes[dtColonyData.DefaultView[i]["COLONYNUMBER"].ToString()].Tag = dtColonyData.DefaultView[i]["COLONYNUMBER"].ToString();
                                            }
                                        }
                                    }

                                }
                            }


                        }
                      
                        bNext = false;
                    }
                }
                else
                {
                    if (Status == "NEWAGAR") 
                    {
                        bool bHave = false;

                        for (int i = 0; i <= dt.Rows.Count - 1; i++) 
                        {
                            bHave = false;

                            foreach (TreeNode t in tMain.Nodes)
                            {
                                if (dt.Rows[i]["AGARNAME"].ToString() == t.Text) 
                                {
                                    bHave = true;
                                    break;
                                }
                            }

                            if (bHave == false) 
                            {
                                tMain.Nodes.Add(dt.Rows[i]["AGARID"].ToString(), dt.Rows[i]["AGARNAME"].ToString(), 0);
                            }

                        }
                      

                        bNext = false;


                    }
                    bNext = true;
                    //if (Status == "NEWAGAR") 
                    //{
                    //    bNext = true;
                    //}
                }

                if (bNext == true)
                {
                    tMain.Nodes.Clear();
                    if (dtColony.Rows.Count > 0)
                    {
                        dtOrganism = objTesting.GetIdenWithOrganismbyRequest(objTestResult);

                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            tMain.Nodes.Add(dt.Rows[i]["AGARID"].ToString(), dt.Rows[i]["AGARNAME"].ToString(), 0);

                            dtColony.DefaultView.RowFilter = "AGARID = " + dt.Rows[i]["AGARID"].ToString();
                            dtColony.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            TreeNode nChild = tMain.Nodes[dt.Rows[i]["AGARID"].ToString()];

                            if (dtColony.DefaultView.Count > 0)
                            {
                                for (int j = 0; j <= dtColony.DefaultView.Count - 1; j++)
                                {
                                    String sColonytemp = "";

                                    sColonytemp = dtColony.DefaultView[j]["COLONYNUMBER"].ToString();

                                    nChild.Nodes.Add(dtColony.DefaultView[j]["COLONYID"].ToString(), dtColony.DefaultView[j]["COLONYNUMBER"].ToString(), 5);

                                    TreeNode nColony = nChild.Nodes[dtColony.DefaultView[j]["COLONYID"].ToString()];

                                    // Organism O
                                    dtOrganism.DefaultView.RowFilter = "COLONYID = " + dtColony.DefaultView[j]["COLONYID"].ToString();
                                    dtOrganism.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                    char ch = Convert.ToChar(val + j + 1);
                                    //sColonytemp = dtColony.DefaultView[j]["COLONYNUMBER"].ToString();
                                    if (dtOrganism.DefaultView.Count > 0)
                                    {
                                        nColony.Text = "(" + sColonytemp + "-" + dtOrganism.DefaultView[0]["ORGANISMNAME"].ToString() + ")";
                                    }
                                    else
                                    {
                                        String sTmp = DateTime.Now.ToString("ddMMyyyyHH:mm:ss");

                                        nColony.Text = "(" + sColonytemp + "-" + sTmp + ")";

                                    }

                                    nColony.Tag = sColonytemp;

                                }

                            }
                            else
                            {
                                dtColonyData.DefaultView.RowFilter = " AGARID = " + dt.Rows[i]["AGARID"].ToString();
                                dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                if (dtColonyData.DefaultView.Count > 0)
                                {
                                    String sTmp = DateTime.Now.ToString("ddMMyyyyHH:mm:ss");

                                    TreeNode SelectNode = nChild.Nodes.Add(dtColonyData.DefaultView[0]["COLONYNUMBER"].ToString(), "(" + dtColonyData.DefaultView[0]["COLONYNUMBER"].ToString() + "-" + sTmp + ")", 5);
                                    SelectNode.Tag = dtColonyData.DefaultView[0]["COLONYNUMBER"].ToString();
                                }

                                //AGARID
                            }

                        }

                    }
                    else
                    {
                        if (dtColonyData.Rows.Count > 0)
                        {
                            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                            {
                                tMain.Nodes.Add(dt.Rows[i]["AGARID"].ToString(), dt.Rows[i]["AGARNAME"].ToString(), 0);

                                dtColonyData.DefaultView.RowFilter = "AGARID = " + dt.Rows[i]["AGARID"].ToString();
                                dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                TreeNode nChild = tMain.Nodes[dt.Rows[i]["AGARID"].ToString()];

                                if (dtColonyData.DefaultView.Count > 0)
                                {
                                    for (int j = 0; j <= dtColonyData.DefaultView.Count - 1; j++)
                                    {
                                        String sColonytemp = "";

                                        dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + dtColonyData.DefaultView[j]["COLONYNUMBER"].ToString() + "' and action = '" + objActionTypeM.ActionType_Identification + "'";
                                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                        nChild.Nodes.Add(dtColonyData.DefaultView[j]["COLONYNUMBER"].ToString(), dtColonyData.DefaultView[j]["COLONYNUMBER"].ToString(), 5);

                                        TreeNode nColony = nChild.Nodes[dtColonyData.DefaultView[j]["COLONYNUMBER"].ToString()];

                                        // Organism O

                                        //dtOrganism.DefaultView.RowFilter = "COLONYID = " + dtColony.DefaultView[j]["COLONYID"].ToString();
                                        //dtOrganism.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                        char ch = Convert.ToChar(val + j + 1);

                                        sColonytemp = dtColonyData.DefaultView[j]["COLONYNUMBER"].ToString();

                                        if (dtCultureData.DefaultView.Count > 0)
                                        {
                                            nColony.Text = "(" + sColonytemp + "-" + dtCultureData.DefaultView[0]["actiondetail"].ToString() + ")";
                                        }
                                        else
                                        {
                                            String sTmp = DateTime.Now.ToString("ddMMyyyyHH:mm:ss");
                                            nColony.Text = "(" + sColonytemp + "-" + sTmp + ")";

                                        }


                                        //if (dtOrganism.DefaultView.Count > 0)
                                        //{
                                        //    nColony.Text = "(" + sColonytemp + "-" + dtOrganism.DefaultView[0]["ORGANISMNAME"].ToString() + ")";
                                        //}
                                        //else
                                        //{
                                        //    String sTmp = DateTime.Now.ToString("ddMMyyyyHH:mm:ss");

                                        //    nColony.Text = "(" + sColonytemp + "-" + sTmp + ")";
                                        //}

                                        nColony.Tag = sColonytemp;

                                    }

                                }


                            }
                        }
                        else
                        {
                            DataTable dtProtocalWithMedia = objTesting.GetMediaWithProtocal(Convert.ToInt64(objTestResult.ProtocalID));

                            if (dt.Rows.Count > dtProtocalWithMedia.Rows.Count)
                            {
                                for (int main = 0; main <= dt.Rows.Count - 1; main++)
                                {
                                    tMain.Nodes.Add(dt.Rows[main]["AGARID"].ToString(), dt.Rows[main]["AGARNAME"].ToString(), 0);
                                }
                            }
                            else
                            {
                                if (dtProtocalWithMedia.Rows.Count > 0)
                                {
                                    DataRow dr;

                                    for (int main = 0; main <= dtProtocalWithMedia.Rows.Count - 1; main++)
                                    {
                                        dr = dtMedia.NewRow();

                                        dr["REQUESTID"] = objTestResult.RequestID;
                                        dr["SUBREQMBAGARID"] = objTestResult.MBRequestID;
                                        dr["CREATEUSER"] = ControlParameter.loginName;
                                        dr["LOGUSERID"] = ControlParameter.loginName;
                                        dr["AGARCODE"] = dtProtocalWithMedia.Rows[main]["AGARCODE"].ToString();
                                        dr["AGARID"] = dtProtocalWithMedia.Rows[main]["AGARID"].ToString();
                                        dr["AGARNAME"] = dtProtocalWithMedia.Rows[main]["AGARNAME"].ToString();
                                        dr["AGARINDEX"] = dtProtocalWithMedia.Rows[main]["AGARINDEX"].ToString();
                                        dr["SUBREQUESTID"] = objTestResult.MBRequestID;

                                        dtMedia.Rows.Add(dr);

                                        tMain.Nodes.Add(dtProtocalWithMedia.Rows[main]["AGARID"].ToString(), dtProtocalWithMedia.Rows[main]["AGARNAME"].ToString(), 0);

                                        dr = null;

                                    }
                                }
                            }
                        }
                    }


                }


                //MediaTestingResult = dtColony;

                tMain.ExpandAll();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void RefreshNode(String ColonyNo, String OrganismName, String ColonyID)
        {
            try
            {

                foreach (TreeNode ChildNode in tMain.Nodes)
                {
                    if (ChildNode.Nodes.Count > 0)
                    {
                        foreach (TreeNode Child1 in ChildNode.Nodes)
                        {
                            if (Child1.Name.ToString() == ColonyID)
                            {
                                Child1.Text = "(" + ColonyNo + "-" + OrganismName + ")";
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (ChildNode.Level == 1)
                        {
                            if (ChildNode.Tag.ToString() == ColonyNo)
                            {
                                ChildNode.Text = "(" + ColonyNo + "-" + OrganismName + ")";
                                break;
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CulturelIndenAndSensitivity_Load_1(object sender, EventArgs e)
        {
            try
            {
                BeginingData();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void tMain_AfterSelect(object sender, TreeViewEventArgs e)
        {

            try
            {
                TreeNode Node = tMain.SelectedNode;

                TreeNode SelectNode = tMain.SelectedNode;
                TreeNode ParentNode = SelectNode.Parent;

                if (SelectNode.Level == 1)
                {
                    //DataRowView selRow = (DataRowView)(((GridView)grdDisplay.MainView).GetRow(selRows[0]));
                    //selRow["COLONYNUMBER"] = SelectNode.Tag.ToString();// TestColonyResultSelect.ColonyNumber;
                    //selRow["COLONYID"] = SelectNode.Name;//TestColonyResultSelect.ColonyID;
                    //selRow["SUBREQMBAGARID"] = TestAgarResultSelect.SubReqMBAgarID;

                    dtCultureData = (DataTable)CultueDataBind.DataSource;

                    dtCultureData.DefaultView.RowFilter = "COLONYNUMBER =  '" + SelectNode.Tag.ToString() + "'";
                    dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    if (dtCultureData.Rows.Count > 0)
                    {
                        dtCultureData.DefaultView.RowFilter = "COLONYID =  '" + SelectNode.Name.ToString() + "' and action = '" + objActionTypeM.ActionType_Sensitivities + "'";
                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        if (dtCultureData.DefaultView.Count > 0)
                        {
                            dtAntibioticBreakpoint.DefaultView.RowFilter = " COLONYID = '" + dtCultureData.DefaultView[0]["COLONYID"].ToString() + "'";

                            grdAnti.DataSource = dtAntibioticBreakpoint;

                        }
                        else
                        {

                            grdAnti.DataSource = null;
                        }
                    }

                    dtCultureData.DefaultView.RowFilter = string.Empty;

                    if (dtCultureData.DefaultView.Count > 0)
                    {

                        //TreeNode ParentNode = Node.Parent;
                        //String ColonyNo = Node.Tag.ToString();

                        //dtCultureData.DefaultView.RowFilter = "  Action =  '" + objActionTypeM.ActionType_Identification + "' and colonynumber = '" + SelectNode.Tag.ToString() + "'";
                        dtCultureData.DefaultView.RowFilter = "  Action =  '" + objActionTypeM.ActionType_Identification + "' and COLONYID = '" + SelectNode.Name.ToString() + "'";
                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        if (dtCultureData.DefaultView.Count == 0)
                        {
                            AddIdentification.Enabled = true;
                            //AddIdentification.Visible = true;
                        }
                        else
                        {
                            AddIdentification.Enabled = false;
                            //AddIdentification.Visible = false;
                        }

                        dtCultureData.DefaultView.RowFilter = string.Empty;

                        dtCultureData.DefaultView.RowFilter = "COLONYNUMBER =  '" + SelectNode.Tag.ToString() + "' and action = '" + objActionTypeM.ActionType_Sensitivities + "'";
                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        if (dtCultureData.DefaultView.Count > 0)
                        {

                            //dtAntibioticBreakpoint.DefaultView.RowFilter = "SENSITIVITYID = '" + dtCultureData.DefaultView[0]["SENSITIVITYID"].ToString() + "' and COLONYNUMBER = '" + dtCultureData.DefaultView[0]["COLONYNUMBER"].ToString () + "'";
                            dtAntibioticBreakpoint.DefaultView.RowFilter = "COLONYNUMBER = '" + dtCultureData.DefaultView[0]["COLONYNUMBER"].ToString() + "'";
                            dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            grdAnti.DataSource = dtAntibioticBreakpoint;

                        }

                        dtCultureData.DefaultView.RowFilter = string.Empty;

                        dtCultureData.DefaultView.RowFilter = "COLONYNUMBER =  '" + SelectNode.Tag.ToString() + "'";
                        //dtCultureData.DefaultView.RowFilter = "COLONYID =  '" + SelectNode.Name.ToString() + "'";
                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        CultueDataBind.DataSource = dtCultureData;

                    }
                    else
                    {
                        AddIdentification.Enabled = true;
                    }
                }
                else
                {
                    AddIdentification.Enabled = true;
                }

                if (Node.Level == 1)
                {
                    addAntibioticsToolStripMenuItem.Visible = true;
                }
                else 
                {
                    addAntibioticsToolStripMenuItem.Visible = false;
                }

                //Menumedia.Show();                

                //if (Node.Level == 0)
                //{
                //    deleteColonyToolStripMenuItem.Enabled = false;
                //}
                //else {
                //    if (Node.Level == 1)
                //    {
                //        deleteColonyToolStripMenuItem.Enabled  = true;
                //    }
                //}

                //if (Node != null)
                //{
                //    TestAgarResultSelect.CreateBy = ControlParameter.loginID;
                //    TestAgarResultSelect.UpdateBy = ControlParameter.loginID;

                //    // Select Root Node
                //    if (Node.Level  == 0)
                //    {
                //        Int64 iSubAgarId = 0;
                //        TestAgarResultSelect.AgarID = Convert.ToInt64(Node.Name.ToString());

                //        if (objTestResultM.TestResultAkars != null)
                //        {
                //            for (int i = 0; i <= objTestResultM.TestResultAkars.Count - 1; i++)
                //            {
                //                if (objTestResultM.TestResultAkars[i].AgarID == Convert.ToInt64(Node.Name.ToString()))
                //                {
                //                    TestAgarResultSelect.AgarID = Convert.ToInt64(Node.Name.ToString());
                //                    TestAgarResultSelect.SubReqMBAgarID = objTestResultM.TestResultAkars[i].SubReqMBAgarID;
                //                    iSubAgarId = TestAgarResultSelect.SubReqMBAgarID;

                //                    break;
                //                    //dt = objTesting.GetMediaTestingWithRequestnumber(objTestResult , objTestResultM.TestResultAkars[i].AgarID , 0);
                //                }
                //            }

                //        }

                //    }
                //    else
                //    {
                //        // Child Node
                //        if (Node.Level == 1)
                //        {
                //            TreeNode ParentNode = Node.Parent ;
                //            String ColonyNo = Node.Tag.ToString ();

                //            dtCultureData.DefaultView.RowFilter = "  Action =  '" + objActionTypeM.ActionType_Identification + "' and colonynumber = '" + ColonyNo + "'";
                //            dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                //            if (dtCultureData.DefaultView.Count == 0)
                //            {
                //                //AddIdentification.Enabled = true;
                //                AddIdentification.Visible = true;
                //            }
                //            else 
                //            {
                //                AddIdentification.Visible = false;

                //            }

                //            if (objTestResultM.TestResultAkars != null)
                //            {
                //                for (int i = 0; i <= objTestResultM.TestResultAkars.Count - 1; i++)
                //                {
                //                    if (objTestResultM.TestResultAkars[i].AgarID == Convert.ToInt64(ParentNode.Name.ToString()))
                //                    {
                //                        TestAgarResultSelect.AgarID = Convert.ToInt64(ParentNode.Name.ToString());
                //                        TestAgarResultSelect.SubReqMBAgarID = objTestResultM.TestResultAkars[i].SubReqMBAgarID;
                //                        ParentNode.Tag = TestAgarResultSelect.SubReqMBAgarID;

                //                        if (TestColonyResultSelect == null)
                //                        {
                //                            TestColonyResultSelect = new TestColonyResultM();
                //                            TestColonyResultSelect.ColonyID = Convert.ToInt64(Node.Name);
                //                            TestColonyResultSelect.ColonyNumber = Node.Tag.ToString();

                //                            if (TestAgarResultSelect.Colonies == null)
                //                            {
                //                                TestAgarResultSelect.Colonies = new List<TestColonyResultM>();
                //                            }

                //                            TestAgarResultSelect.Colonies.Add(TestColonyResultSelect);

                //                            break;
                //                        }
                //                    }
                //                }
                //            }



                //        }
                //    }

                //}

                //if (TestColonyResultSelect != null)
                //{
                //bSelectColony = true;



                //    TestColonyResultSelect = null;                    
                //}


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }



        }

        private void LoaddisplayDetail(int iLevel)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void addOrganismToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchList fm = new frmSearchList();
            TestAgarsResultM objAgarResult;
            DataTable dt;
            DataRow dr = null;
            Boolean bCheck = true;
            try
            {

                dt = objConfiguration.GetDICTOrganisms();

                dt.Columns["ORGANISMCODE"].ColumnName = "code";
                dt.Columns["ORGANISMNAME"].ColumnName = "name";
                dt.Columns["ORGANISMID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();

                fm.ShowDialog();

                if (fm.Selected == true)
                {

                    if (TestAgarResultSelect != null)
                    {
                        if (TestAgarResultSelect.Colonies[0] != null)
                        {
                            TestColonyResultSelect = TestAgarResultSelect.Colonies[0];
                        }
                    }

                    TestColonyResultSelect.OrganismID = fm.SelectedID;
                    TestColonyResultSelect.OrganismIndex = 1;
                    TestColonyResultSelect.OrganismNumber = fm.SelectedCode;
                    TestColonyResultSelect.RequestID = objTestResult.RequestID;
                    TestColonyResultSelect.MBRequestID = objTestResult.MBRequestID;
                    TestColonyResultSelect.SubReqMBAgarID = TestAgarResultSelect.SubReqMBAgarID;

                    TestColonyResultSelect.CreateBy = ControlParameter.loginName;

                    //objTesting.InsertOrganism(TestColonyResultSelect);
                    TestColonyResultSelect = objTesting.SaveOrganism(TestColonyResultSelect);

                    MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult, 0, 0);

                    CreateNode(MediaTestingResult, "SAVE");

                    tMain.ExpandAll();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
                objConfiguration = null;
            }
        }

        private void addBloodAgarMenuItem_Click_1(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchList fm = new frmSearchList();
            //TestAgarsResultM objAgarResult;
            DataTable dt;
            DataRow dr = null;
            //Boolean bCheck = true;
            try
            {

                dt = objConfiguration.GetMedia("", "");

                dt.Columns["AGARCODE"].ColumnName = "code";
                dt.Columns["AGARNAME"].ColumnName = "name";
                dt.Columns["AGARID"].ColumnName = "id";

                fm.IsMultiSelect = true;

                fm.SearchData = dt;
                fm.RefreshData();

                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    ArrayList objRows = fm.MultiSelectValue;
                    Boolean bCheck = false;

                    for (int i = 0; i <= objRows.Count - 1; i++)
                    {
                        DataRow row = objRows[i] as DataRow;

                        bCheck = false;

                        if (dtMedia.Rows.Count > 0)
                        {
                            dtMedia.DefaultView.RowFilter = " AGARID = '" + row["id"] + "'";
                            dtMedia.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            if (dtMedia.DefaultView.Count >= 1)
                            {
                                bCheck = false;
                            }
                            else
                            {
                                bCheck = true;
                            }

                        }
                        else
                        {
                            bCheck = true;
                        }

                        if (bCheck == true)
                        {
                            dr = dtMedia.NewRow();

                            dr["REQUESTID"] = objTestResult.RequestID;
                            dr["SUBREQMBAGARID"] = objTestResult.MBRequestID;
                            dr["CREATEUSER"] = ControlParameter.loginName;
                            dr["LOGUSERID"] = ControlParameter.loginName;
                            dr["AGARID"] = row["id"];
                            dr["AGARCODE"] = row["code"];
                            dr["AGARNAME"] = row["name"];
                            dr["SUBREQUESTID"] = objTestResult.MBRequestID;
                           
                            if (tMain.Nodes == null)
                            {
                                dr["AGARINDEX"] = 1;
                            }
                            else
                            {
                                dr["AGARINDEX"] = tMain.Nodes.Count + 1;
                            }

                            dtMedia.Rows.Add(dr);

                        }

                    }

                    CreateNode(dtMedia, "NEWAGAR");

                    //objAgarResult = new TestAgarsResultM();

                    //objAgarResult.AgarID = fm.SelectedID;
                    //objAgarResult.AgarNumber = fm.SelectedCode;
                    //objAgarResult.AgarIndex = tMain.Nodes.Count +1;
                    //objAgarResult.Status = "1";
                    //objAgarResult.CreateBy = objTestResult.CreateBy;
                    //objAgarResult.UpdateBy = objTestResult.UpdateBy;

                    //if (objTestResult.TestResultAkars == null)
                    //{
                    //    objTestResult.TestResultAkars = new List<TestAgarsResultM>();

                    //    objTestResult.TestResultAkars.Add(objAgarResult);

                    //}
                    //else
                    //{
                    //    for (int i = 0; i <= objTestResult.TestResultAkars.Count - 1; i++)
                    //    {
                    //        if (objTestResult.TestResultAkars[i].AgarID == objAgarResult.AgarID)
                    //        {
                    //            objTestResult.TestResultAkars[i] = objAgarResult;
                    //            bCheck = true;
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            bCheck = false;
                    //        }
                    //    }

                    //    if (bCheck == false)
                    //    {
                    //        objTestResult.TestResultAkars.Add(objAgarResult);
                    //    }

                    //}

                    //objAgarResult = null;

                    //objTestResult = objTesting.SaveMediaByIden(objTestResult);

                    //dt = objTesting.GetAgarHeader(objTestResult);

                    //MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult ,0,0);

                    //CreateNode(dt);

                    tMain.ExpandAll();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
                objConfiguration = null;
            }
        }


        private void AddColonyDescMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchList fm = new frmSearchList();

            DataTable dt;
            try
            {

                dt = objConfiguration.GetColonyDesc("", "");

                dt.Columns["COLONYCODE"].ColumnName = "code";
                dt.Columns["COLONYDESCRIPTION"].ColumnName = "name";
                dt.Columns["COLONYDESCID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();

                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    if (TestAgarResultSelect != null)
                    {
                        if (TestAgarResultSelect.Colonies[0] != null)
                        {
                            TestColonyResultSelect = TestAgarResultSelect.Colonies[0];
                        }
                    }

                    TestColonyResultSelect.DescrcodeID = fm.SelectedID.ToString();
                    TestColonyResultSelect.DescrcodeDescription = fm.SelectedName;

                    TestColonyResultSelect.CreateBy = ControlParameter.loginName;

                    objTesting.SaveColony(TestColonyResultSelect);

                    //objTesting.UpdateColony (TestColonyResultSelect);

                    MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult, 0, 0);

                    CreateNode(MediaTestingResult, "SAVE");

                    grdDisplay.DataSource = MediaTestingResult;

                    tMain.ExpandAll();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
                objConfiguration = null;
            }
        }

        private void batteryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchList fm = new frmSearchList();

            DataTable dt;
            try
            {

                dt = objConfiguration.GetDICTBattery();

                dt.Columns["BATTERYCODE"].ColumnName = "code";
                dt.Columns["FULLTEXT"].ColumnName = "name";
                dt.Columns["BATTERYID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();

                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    if (TestAgarResultSelect != null)
                    {
                        if (TestAgarResultSelect.Colonies[0] != null)
                        {
                            TestColonyResultSelect = TestAgarResultSelect.Colonies[0];
                        }
                    }

                    TestColonyResultSelect.DescrcodeID = fm.SelectedID.ToString();
                    TestColonyResultSelect.DescrcodeDescription = fm.SelectedName;

                    TestColonyResultSelect.CreateBy = ControlParameter.loginName;

                    objTesting.SaveColony(TestColonyResultSelect);

                    //objTesting.UpdateColony(TestColonyResultSelect);

                    MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult, 0, 0);

                    CreateNode(MediaTestingResult, "SAVE");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
                //objConfiguration = null;
            }
        }

        private void addSensitivitiesPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmSearchList fm = new frmSearchList();
            TestAgarsResultM objAgarResult;
            DataTable dt;
            DataRow dr = null;
            Boolean bCheck = true;

            try
            {

                if (objTesting == null)
                {
                    objTesting = new TestController();
                }

                dt = objTesting.GetDICTSensitivitiesPanel("", "");

                dt.Columns["SENSITIVITYCODE"].ColumnName = "code";
                dt.Columns["SENSITIVITYNAME"].ColumnName = "name";
                dt.Columns["SENSITIVITYID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();

                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objAgarResult = new TestAgarsResultM();

                    objAgarResult.AgarID = fm.SelectedID;
                    objAgarResult.AgarNumber = fm.SelectedCode;
                    objAgarResult.AgarIndex = MediaTestingResult.Rows.Count + 1;
                    objAgarResult.Status = "1";
                    objAgarResult.CreateBy = objTestResult.CreateBy;
                    objAgarResult.UpdateBy = objTestResult.UpdateBy;

                    if (objTestResult.TestResultAkars == null)
                    {
                        objTestResult.TestResultAkars = new List<TestAgarsResultM>();

                        objTestResult.TestResultAkars.Add(objAgarResult);

                    }
                    else
                    {
                        for (int i = 0; i <= objTestResult.TestResultAkars.Count - 1; i++)
                        {
                            if (objTestResult.TestResultAkars[i].AgarID == objAgarResult.AgarID)
                            {
                                objTestResult.TestResultAkars[i] = objAgarResult;
                                bCheck = true;
                                break;
                            }
                            else
                            {
                                bCheck = false;
                            }
                        }

                        if (bCheck == false)
                        {
                            objTestResult.TestResultAkars.Add(objAgarResult);
                        }

                    }

                    objAgarResult = null;

                    objTestResult = objTesting.SaveMediaByIden(objTestResult);

                    MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult, 0, 0);

                    CreateNode(MediaTestingResult, "SAVE");

                    //dt = objTesting.GetMediaTestingWithRequestnumber(objTestResult);
                    grdDisplay.DataSource = MediaTestingResult;

                    tMain.ExpandAll();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }
        }

        private void gridControl3_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //GridView gridView =  grdDisplay.FocusedView as GridView;
                //GridView gridView =  gridView5.SelectRow grdDisplay.FocusedView as GridView;
                //object row = grdAnti.GetRow(gridView.FocusedRowHandle);

                if (gridView5.SelectedRowsCount > 0)
                {
                    MenuSensitivity.Items[1].Enabled = true;
                    MenuSensitivity.Show(grdAnti, e.Location);
                    //MenuSensitivity.Show(Cursor.Position);
                }
                else
                {
                    MenuSensitivity.Items[1].Enabled = false;
                    MenuSensitivity.Show(Cursor.Position);
                }

            }

        }

        private void addColonyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TestAgarsResultM objTestSelect = TestAgarResultSelect;
            //TestColonyResultM objTestColony = new TestColonyResultM();
            //int Index = 0;
            //int val = 64;

            try
            {
                TreeNode SelectNode = tMain.SelectedNode;

                if (SelectNode == null)
                {
                    throw new Exception("Please select Media before add colony.");
                }

                if (SelectNode.Level == 0)
                {
                    //Index = SelectNode.Nodes.Count + 1;

                    String sTmp;
                    String sColonytemp;

                    sTmp = (SelectNode.Index + 1).ToString();

                    if (sTmp.Length == 1)
                    {
                        sTmp = "0" + sTmp;
                    }

                    if (SelectNode.LastNode == null)
                    {
                        sColonytemp = sTmp + "A";
                    }
                    else
                    {
                        String sLastColony = SelectNode.LastNode.Tag.ToString();

                        String sLastDigit = sLastColony.Substring(sLastColony.Length - 1, 1);

                        char[] ch = sLastDigit.ToCharArray();
                        char chNext = (char)((int)ch[0] + 1);

                        //char ch = Convert.ToChar(val + Index);

                        //Check Character is duplicate or not
                        //TreeNode SelectChild = SelectNode.
                        //SelectNode.LastNode. 
                        //SelectNode.Nodes [SelectNode.Nodes.Count]

                        sColonytemp = sTmp + chNext;

                    }

                    DataRow dr;

                    dr = dtColonyData.NewRow();
                    dr["LOGUSERID"] = ControlParameter.loginName;
                    dr["COLONYNUMBER"] = sColonytemp;
                    dr["COLONYINDEX"] = SelectNode.Nodes.Count + 1;
                    dr["AGARID"] = SelectNode.Name;

                    dtColonyData.Rows.Add(dr);

                    dr = null;

                    CreateNode(dtMedia, "NEW");

                    //MediaTestingResult.DefaultView.RowFilter = " AGARID = " + SelectNode.Name ;
                    //MediaTestingResult.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    //objTestColony.SubReqMBAgarID = Convert.ToInt64(MediaTestingResult.DefaultView[0]["SUBREQMBAGARID"].ToString());
                    ////TestAgarResultSelect.SubReqMBAgarID ;
                    //objTestColony.ColonyIndex = Index;
                    //objTestColony.ColonyNumber = sColonytemp;
                    //objTestColony.ColonyComment = "";
                    //objTestColony.Status = "1";
                    //objTestColony.CreateBy = ControlParameter.loginName ;
                    //objTestColony.UpdateBy = ControlParameter.loginName;

                    //objTesting.InsertColony(objTestColony);

                }
                //for (int i = 0; i <= objTestResultM.TestResultAkars.Count - 1; i++) 
                //{
                //    if (objTestResultM.TestResultAkars[i].AgarID == objTestSelect.AgarID) 
                //    {
                //        if (objTestResultM.TestResultAkars[i].Colonies != null) 
                //        {
                //            if (objTestResultM.TestResultAkars[i].Colonies.Count > 0)
                //            {
                //                Index = objTestResultM.TestResultAkars[i].Colonies.Count;
                //                Index = Index + 1;

                //                String sTmp;
                //                String sColonytemp;                                
                //                int iAgar = 0;

                //                iAgar = i + 1;

                //                sTmp = (iAgar).ToString();

                //                if (sTmp.Length == 1)
                //                {
                //                    sTmp = "0" + sTmp;
                //                }

                //                char ch = Convert.ToChar(val + Index);

                //                sColonytemp = sTmp + ch ;

                //                objTestColony.SubReqMBAgarID = objTestSelect.SubReqMBAgarID;
                //                objTestColony.ColonyIndex = Index;
                //                objTestColony.ColonyNumber = sColonytemp;
                //                objTestColony.ColonyComment = "";
                //                objTestColony.Status = "1";
                //                objTestColony.CreateBy = objTestSelect.CreateBy;
                //                objTestColony.UpdateBy = objTestSelect.CreateBy;

                //                objTesting.InsertColony(objTestColony);

                //                break;

                //            }
                //        }

                //    }

                //}

                //MediaTestingResult = objTesting.GetAgarHeader (objTestResult);

                //CreateNode(MediaTestingResult);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteColonyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestAgarsResultM objTestSelect = TestAgarResultSelect;
            TestColonyResultM objTestColony = new TestColonyResultM();
            DataTable dt;

            try
            {
                TreeNode SelectNode = tMain.SelectedNode;

                if (dtMedia.Rows.Count > 0) 
                {
                    // Select Colony 
                    if (SelectNode.Level == 1)
                    {
                        dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + SelectNode.Name + "'";
                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        if (dtCultureData.DefaultView.Count > 0)
                        {
                            for (int i = dtCultureData.DefaultView.Count - 1; i >= 0; i--)
                            {
                                dtCultureData.DefaultView.Delete(i);
                            }
                        }

                        dtAntibioticBreakpoint.DefaultView.RowFilter = " COLONYNUMBER = '" + SelectNode.Name + "'"; ;
                        dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        if (dtAntibioticBreakpoint.DefaultView.Count > 0)
                        {
                            for (int i = dtAntibioticBreakpoint.DefaultView.Count - 1; i >= 0; i--)
                            {
                                dtAntibioticBreakpoint.DefaultView.Delete(i);
                            }
                        }
                     
                        dtColonyData.DefaultView.RowFilter = " COLONYNUMBER = '" + SelectNode.Name + "'";
                        dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        if (dtColonyData.DefaultView.Count > 0)
                        {
                            dtColonyData.DefaultView.Delete(0);
                        }

                        SelectNode.Remove();

                    }
                    else
                    {
                        if (SelectNode.Level == 0)
                        {
                            dtColonyData.DefaultView.RowFilter = String.Empty;
                            dtCultureData.DefaultView.RowFilter = String.Empty;
                            dtMedia.DefaultView.RowFilter = String.Empty;

                            foreach (TreeNode Child in SelectNode.Nodes)
                            {
                                dtAntibioticBreakpoint.DefaultView.RowFilter = " COLONYNUMBER = '" + Child.Name + "'" ;
                                dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                if (dtAntibioticBreakpoint.DefaultView.Count > 0)
                                {
                                    for (int i = dtAntibioticBreakpoint.DefaultView.Count - 1; i >= 0; i--)
                                    {
                                        dtAntibioticBreakpoint.DefaultView.Delete(i);
                                    }
                                }

                                dtCultureData.DefaultView.RowFilter = "colonynumber = '" + Child.Name + "'";
                                dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                for (int i = dtCultureData.DefaultView.Count - 1; i >= 0; i--)
                                {
                                    dtCultureData.DefaultView.Delete(i);
                                }

                                dtColonyData.DefaultView.RowFilter = " COLONYNUMBER = '" + Child.Name + "'";
                                dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                if (dtColonyData.DefaultView.Count > 0)
                                {
                                    dtColonyData.DefaultView.Delete(0);
                                }

                            }
                            
                            dtMedia.DefaultView.RowFilter = " AGARID = " + SelectNode.Name;
                            dtMedia.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            if (dtMedia.DefaultView.Count > 0) 
                            {
                                dtMedia.DefaultView.Delete(0);
                            }

                            SelectNode.Remove();

                        }
                    }   
                    //dt = objTesting.GetAgarHeader(objTestResult);

                    //if (dt.Rows.Count > 0)
                    //{
                    //    if (SelectNode.Level == 1)
                    //    {
                    //        if (IsNumeric(SelectNode.Name) == true)
                    //        {
                    //            objTestColony.ColonyID = Convert.ToInt16(SelectNode.Name);
                    //            objTestColony.MBRequestID = Convert.ToInt16(objTestResult.MBRequestID);

                    //            objTesting.DeleteColony(objTestColony.ColonyID.ToString(), objTestColony.MBRequestID.ToString());

                    //        }
                    //        else
                    //        {
                    //            dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + SelectNode.Name + "'";
                    //            dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    //            if (dtCultureData.DefaultView.Count > 0)
                    //            {
                    //                for (int i = dtCultureData.DefaultView.Count - 1; i >= 0; i--)
                    //                {
                    //                    dtCultureData.DefaultView.Delete(i);
                    //                }

                    //            }
                    //        }


                    //        dtColonyData.DefaultView.RowFilter = " COLONYNUMBER = '" + SelectNode.Name + "'";
                    //        dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    //        if (dtColonyData.DefaultView.Count > 0)
                    //        {
                    //            dtColonyData.DefaultView.Delete(0);
                    //        }

                    //        SelectNode.Remove();

                    //        //objTesting.DeleteColony(objTestColony);
                    //    }
                    //    else
                    //    {
                    //        if (SelectNode.Level == 0)
                    //        {
                    //            objTesting.DeleteMedia(SelectNode.Name, objTestResult.MBRequestID.ToString());
                    //        }
                    //    }

                    //    //BeginingData();
                    //}
                    //else
                    //{
                    //    if (SelectNode.Level == 1)
                    //    {
                    //        //objTestColony.ColonyID = Convert.ToInt64(SelectNode.Name.ToString());

                    //        if (dtCultureData.Rows.Count > 0)
                    //        {
                    //            dtCultureData.DefaultView.RowFilter = "  COLONYID = '" + SelectNode.Name + "'";
                    //            dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    //            //if (SelectNode.Tag == null)
                    //            //{
                    //            //}
                    //            //else
                    //            //{
                    //            //    dtCultureData.DefaultView.RowFilter = "  COLONYNUMBER = '" + SelectNode.Tag + "'";
                    //            //    dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                    //            //}

                    //            if (dtCultureData.DefaultView.Count > 0)
                    //            {
                    //                for (int i = 0; i <= dtCultureData.Rows.Count - 1; i++)
                    //                {
                    //                    if (dtCultureData.Rows[i]["COLONYID"].ToString() == SelectNode.Name.ToString())
                    //                    {
                    //                        DataRow dr = dtCultureData.Rows[i];
                    //                        dtCultureData.Rows.Remove(dr);
                    //                        dr = null;
                    //                    }
                    //                }
                    //            }

                    //        }

                    //        for (int i = 0; i <= dtColonyData.Rows.Count - 1; i++)
                    //        {
                    //            if (SelectNode.Name == dtColonyData.Rows[i]["colonynumber"].ToString())
                    //            {
                    //                DataRow dr = dtColonyData.Rows[i];
                    //                dtColonyData.Rows.Remove(dr);

                    //                dr = null;
                    //            }
                    //        }

                    //    }
                    //    else
                    //    {
                    //        if (SelectNode.Level == 0)
                    //        {
                    //            dtColonyData.DefaultView.RowFilter = String.Empty;
                    //            dtCultureData.DefaultView.RowFilter = String.Empty;

                    //            foreach (TreeNode Child in SelectNode.Nodes)
                    //            {
                    //                for (int i = 0; i <= dtCultureData.Rows.Count - 1; i++)
                    //                {
                    //                    if (Child.Name == dtCultureData.Rows[i]["colonynumber"].ToString())
                    //                    {
                    //                        DataRow dr = dtCultureData.Rows[i];
                    //                        dtCultureData.Rows.Remove(dr);
                    //                        dr = null;
                    //                    }
                    //                }

                    //                for (int i = 0; i <= dtColonyData.Rows.Count - 1; i++)
                    //                {
                    //                    if (Child.Name == dtColonyData.Rows[i]["colonynumber"].ToString())
                    //                    {
                    //                        DataRow dr = dtColonyData.Rows[i];
                    //                        dtColonyData.Rows.Remove(dr);
                    //                        Child.Remove();
                    //                        dr = null;
                    //                    }
                    //                }

                    //                for (int i = 0; i <= dtAntibioticBreakpoint.Rows.Count - 1; i++)
                    //                {
                    //                    if (Child.Name == dtAntibioticBreakpoint.Rows[i]["colonynumber"].ToString())
                    //                    {
                    //                        DataRow dr = dtAntibioticBreakpoint.Rows[i];
                    //                        dtAntibioticBreakpoint.Rows.Remove(dr);
                    //                        dr = null;
                    //                    }
                    //                }

                    //            }

                    //            for (int i = 0; i <= dtMedia.Rows.Count - 1; i++)
                    //            {
                    //                if (SelectNode.Name == dtMedia.Rows[i]["AGARID"].ToString())
                    //                {
                    //                    DataRow dr = dtMedia.Rows[i];
                    //                    dtMedia.Rows.Remove(dr);
                    //                    SelectNode.Remove();
                    //                    dr = null;
                    //                }
                    //            }


                    //            //AGARNAME
                    //        }
                    //    }

                    //    CreateNode(dtMedia, "NEW");

                    //    grdDisplay.DataSource = null;
                    //    grdAnti.DataSource = null;

                    //}
                }
             

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                objTestColony = null;
                dt = null;
            }
        }

        private void AddIdentification_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchList fm = new frmSearchList();
            DataTable dt;
            DataRow dr;

            try
            {

                if (tMain.SelectedNode == null) 
                {
                    throw new Exception("Could you please select colony first.");
                }

                dt = objConfiguration.GetDICTOrganisms();

                dt.Columns["ORGANISMCODE"].ColumnName = "code";
                dt.Columns["ORGANISMNAME"].ColumnName = "name";
                dt.Columns["ORGANISMID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();

                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    TreeNode SelectNode = tMain.SelectedNode;
                    TreeNode ParentNode = SelectNode.Parent;

                    string Action = objActionTypeM.ActionType_Identification;
                    string Colony = SelectNode.Tag.ToString();
                    string Colonyid = SelectNode.Name.ToString();
                    string ActionDetail = fm.SelectedCode + ":" + fm.SelectedName;
                    string SubAgarID = objTestResult.MBRequestID.ToString();
                    string MICID = "";

                    dt.DefaultView.RowFilter = " code = '" + fm.SelectedCode + "'";
                    dt.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    if (dt.DefaultView.Count > 0)
                    {
                        if (dt.DefaultView[0].Row["MICID"].ToString() != null)
                        {
                            if (dt.DefaultView[0].Row["MICID"].ToString() != "")
                            {
                                MICID = dt.DefaultView[0].Row["MICID"].ToString();
                            }
                            else
                            {
                                //MICID = "2";
                                MICID = "";
                            }

                        }

                    }

                    dtCultureData = (DataTable)CultueDataBind.DataSource;

                    RefreshNode(Colony, ActionDetail, Colonyid);

                    //if (dtCultureData.DefaultView.Count != 0)
                    //{
                    //    RefreshNode(Colony, ActionDetail);
                    //}

                    dr = dtCultureData.NewRow();

                    dr["datetime"] = DateTime.Now.ToString();
                    dr["LOGUSERID"] = ControlParameter.loginName;
                    dr["COLONYNUMBER"] = Colony;
                    dr["SUBREQMBAGARID"] = SubAgarID;
                    dr["action"] = Action;
                    dr["actiondetail"] = ActionDetail;
                    dr["ORGANISMID"] = fm.SelectedID;
                    dr["MICID"] = MICID;
                    dr["COLONYID"] = Colonyid;
                    dr["SUBREQMBORGID"] = Colonyid;
                    dr["PRINTSTATUS"] = "0";

                    dtCultureData.Rows.Add(dr);

                    dr = null;

                    CultueDataBind.DataSource = dtCultureData;

                    AddIdentification.Enabled  = false;

                    CalculateExpertRule();

                    //int[] selRows = ((GridView)grdDisplay.MainView).GetSelectedRows();
                    //DataRowView selRow = (DataRowView)(((GridView)grdDisplay.MainView).GetRow(selRows[0]));

                    //string Action = selRow["action"].ToString();
                    //string Colony = selRow["COLONYNUMBER"].ToString();
                    //string ActionDetail = fm.SelectedCode + ":" + fm.SelectedName;
                    //string ActionType = selRow["action"].ToString();
                    //string SubAgarID = selRow["SUBREQMBAGARID"].ToString();

                    //selRow["action"] = objActionTypeM.ActionType_Identification;
                    //selRow["actiondetail"] = ActionDetail;
                    //selRow["ORGANISMID"] = fm.SelectedID;

                    //if (dtCultureData != null)
                    //{
                    //    if (dtCultureData.Rows.Count > 0)
                    //    {

                    //        dtCultureData = (DataTable)  CultueDataBind.DataSource;

                    //        dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + Colony + "'";
                    //        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    //        if (dtCultureData.DefaultView.Count != 0)
                    //        {
                    //            RefreshNode(Colony, ActionDetail);
                    //        }

                    //        dr = dtCultureData.NewRow();

                    //        dr["datetime"] = DateTime.Now.ToString();
                    //        dr["LOGUSERID"] = ControlParameter.loginName;
                    //        dr["COLONYNUMBER"] = Colony;
                    //        dr["SUBREQMBAGARID"] = SubAgarID;

                    //        dtCultureData.Rows.Add(dr);

                    //        dr = null;

                    //        CultueDataBind.DataSource = dtCultureData;

                    //    }

                    //}


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
            finally
            {
                objConfiguration = null;
                fm = null;
                dt = null;
            }
        }

       

        private void grdDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (tMain.SelectedNode == null)
                {
                    MenuAction.Items[0].Enabled = false;
                    MenuAction.Items[1].Enabled = false;
                    MenuAction.Items[2].Enabled = false;
                    MenuAction.Items[3].Enabled = false;
                    MenuAction.Items[4].Enabled = false;
                    MenuAction.Items[5].Enabled = false;
                    MenuAction.Items[6].Enabled = false;
                }
                else
                {
                    if (tMain.SelectedNode.Level == 0)
                    {
                        MenuAction.Items[0].Enabled = false;
                        MenuAction.Items[1].Enabled = false;
                        MenuAction.Items[2].Enabled = false;
                        MenuAction.Items[3].Enabled = false;
                        MenuAction.Items[4].Enabled = false;
                    }
                    else
                    {
                        if (tMain.SelectedNode.Level == 1)
                        {
                            //MenuAction.Items[0].Enabled = true;
                            MenuAction.Items[1].Enabled = true;
                            MenuAction.Items[2].Enabled = true;
                            MenuAction.Items[3].Enabled = true;
                            MenuAction.Items[4].Enabled = true;
                            MenuAction.Items[5].Enabled = true;
                            MenuAction.Items[6].Enabled = true;
                        }
                    }
                }

            }
        }

        private void AddSensitivity_Click(object sender, EventArgs e)
        {
            frmSearchList fm = new frmSearchList();
            DataTable dt;
            try
            {

                dt = objTesting.GetDICTSensitivitiesPanel("", "");

                dt.Columns["SENSITIVITYCODE"].ColumnName = "code";
                dt.Columns["SENSITIVITYNAME"].ColumnName = "name";
                dt.Columns["SENSITIVITYID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.IsSensitivities = true;
                fm.IsMultiSelect = true;

                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    if (tMain.SelectedNode != null) 
                    {
                        TreeNode SelectNode = tMain.SelectedNode;
                        TreeNode ParentNode = SelectNode.Parent;
                        DataRow dr;

                        string Action = objActionTypeM.ActionType_Sensitivities;
                        string Colony = SelectNode.Tag.ToString();
                        //string ColonyID = SelectNode.Name.ToString();
                        string SubAgarID = objTestResult.MBRequestID.ToString();
                        string ActionDetail = "";

                        ArrayList objRows = fm.MultiSelectValue;
                        Boolean bCheck = false;

                        dtCultureData = (DataTable)CultueDataBind.DataSource;

                        for (int i = 0; i <= objRows.Count - 1; i++)
                        {
                            DataRow row = objRows[i] as DataRow;
                            ActionDetail = row["code"] + ":" + row["name"];

                            bCheck = false;

                            dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + Colony + "' and SENSITIVITYID = '" + row["id"] + "'";
                            dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            if (dtCultureData.DefaultView.Count > 0)
                            {
                                bCheck = true;
                            }
                            else
                            {
                                bCheck = false;
                            }

                            if (bCheck == false)
                            {
                                dr = dtCultureData.NewRow();

                                dr["datetime"] = DateTime.Now.ToString();
                                dr["LOGUSERID"] = ControlParameter.loginName;
                                dr["COLONYNUMBER"] = Colony;
                                dr["SUBREQMBAGARID"] = SubAgarID;
                                dr["action"] = Action;
                                dr["actiondetail"] = ActionDetail;
                                dr["ORGANISMID"] = "";
                                dr["COLONYID"] = 0;
                                dr["SENSITIVITYID"] = row["id"];
                                dr["METHODID"] = fm.MethodID;
                                dr["PRINTSTATUS"] = "0";

                                dtCultureData.Rows.Add(dr);

                                dr = null;

                                //* Defaul Antibiotic
                                DataTable dtAnti;
                                //DataRow dr;
                                SensitivityPanelM objSensitivitiesM = new SensitivityPanelM();

                                objSensitivitiesM.SENSITIVITYID = Convert.ToInt16(row["id"].ToString());

                                dtAnti = objSensitivities.GetSensitivitiesWithAntibiotic(objSensitivitiesM);

                                for (int j = 0; j <= dtAnti.Rows.Count - 1; j++)
                                {
                                    bCheck = false;

                                    if (dtAntibioticBreakpoint.Rows.Count > 0)
                                    {
                                        dtAntibioticBreakpoint.DefaultView.RowFilter = "SENSITIVITYID = '" + dtAnti.Rows[j]["SENSITIVITYID"] + "' and COLONYNUMBER = '" + Colony + "' and ANTIBIOTICID = '" + dtAnti.Rows[j]["ANTIBIOTICID"] + "'";
                                        dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                        if (dtAntibioticBreakpoint.DefaultView.Count > 0)
                                        {
                                            bCheck = true;
                                        }
                                        else
                                        {
                                            bCheck = false;
                                        }

                                    }
                                    else
                                    {
                                        bCheck = false;
                                    }

                                    if (bCheck == false)
                                    {
                                        dr = dtAntibioticBreakpoint.NewRow();
                                        dr["FULLTEXT"] = dtAnti.Rows[j]["FULLTEXT"];
                                        dr["SENSITIVITYID"] = dtAnti.Rows[j]["SENSITIVITYID"];
                                        dr["ANTIBIOTICID"] = dtAnti.Rows[j]["ANTIBIOTICID"];
                                        dr["SUBREQMBORGID"] = SubAgarID;
                                        dr["COLONYID"] = 0;
                                        dr["COLONYNUMBER"] = Colony;
                                        dr["METHODID"] = fm.MethodID;

                                        if (fm.MethodID == 1)
                                        {
                                            dr["UNITS"] = "0";
                                            dr["METHODMBCODE"] = "MIC";
                                        }
                                        else
                                        {
                                            if (fm.MethodID == 2)
                                            {
                                                dr["UNITS"] = "1";
                                                dr["METHODMBCODE"] = "MID";
                                            }
                                            else
                                            {
                                                dr["UNITS"] = "2";
                                                dr["METHODMBCODE"] = "N/A";
                                            }

                                        }

                                        dr["PRINTSTATUS"] = "0";

                                        dtAntibioticBreakpoint.Rows.Add(dr);

                                        dr = null;

                                    }

                                }

                            }



                        }

                        dtCultureData.DefaultView.RowFilter = "COLONYNUMBER = '" + Colony + "'";
                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        CultueDataBind.DataSource = dtCultureData;

                        dtAntibioticBreakpoint.DefaultView.RowFilter = " COLONYNUMBER = '" + Colony + "'";
                        dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        if (dtAntibioticBreakpoint.DefaultView.Count > 0)
                        {
                            grdAnti.DataSource = dtAntibioticBreakpoint;
                        }

                        List<string> objResultList = new List<string>();

                        objResultList.Add("");
                        objResultList.Add("N/A");
                        objResultList.Add("S");
                        objResultList.Add("I");
                        objResultList.Add("R");
                        objResultList.Add("SDD");
                        objResultList.Add("NS");
                        objResultList.Add("NR");

                        riComboResult = new RepositoryItemComboBox();

                        riComboResult.Items.AddRange(objResultList);

                        //Add the item to the internal repository                    
                        grdAnti.RepositoryItems.Add(riComboResult);

                        //Now you can define the repository item as an in-place column editor
                        colRESULT.ColumnEdit = riComboResult;

                        riComboResult.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
            finally
            {
                fm = null;
                dt = null;
            }
        }

        private void LoadComboboxinAnti()
        {

        }
        private void addInstrumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearchList fm = new frmSearchList();
            ConfigurationController objConfig = new ConfigurationController();
            BatteryM objBatteryM = new BatteryM();
            DataTable dt;
            try
            {

                objBatteryM.BATTERY_CODE = "";
                objBatteryM.BATTERY_SHORTNAME = "";

                dt = objConfig.GetBattery("","");

                dt.Columns["BATTERYCODE"].ColumnName = "code";
                dt.Columns["FULLTEXT"].ColumnName = "name";
                dt.Columns["BATTERYID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();

                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    if (dtCultureData != null)
                    {
                        TreeNode SelectNode = tMain.SelectedNode;
                        TreeNode ParentNode = SelectNode.Parent;
                        DataRow dr;

                        string Action = objActionTypeM.ActionType_Instrument;
                        string Colony = SelectNode.Tag.ToString();
                        string ActionDetail = fm.SelectedCode + ":" + fm.SelectedName;
                        //string SubAgarID = ParentNode.Tag.ToString();
                        string SubAgarID = objTestResult.MBRequestID.ToString();
                        string Colonyid = SelectNode.Name.ToString();

                        dtCultureData = (DataTable)CultueDataBind.DataSource;

                        //if (dtCultureData.DefaultView.Count != 0)
                        //{
                        //    RefreshNode(Colony, ActionDetail, Colonyid);
                        //}

                        dr = dtCultureData.NewRow();

                        dr["datetime"] = DateTime.Now.ToString();
                        dr["LOGUSERID"] = ControlParameter.loginName;
                        dr["COLONYNUMBER"] = Colony;
                        dr["SUBREQMBAGARID"] = SubAgarID;
                        dr["action"] = Action;
                        dr["actiondetail"] = ActionDetail;
                        dr["ORGANISMID"] = fm.SelectedID;
                        dr["COLONYID"] = Colonyid;
                        dr["BATTERYID"] = fm.SelectedID;
                        dr["DETECTIONID"] = "";
                        dr["PRINTSTATUS"] = "0";

                        dtCultureData.Rows.Add(dr);

                        dr = null;

                        CultueDataBind.DataSource = dtCultureData;

                        //string ActionDetail = fm.SelectedCode + ":" + fm.SelectedName;

                        //int[] selRows = ((GridView)grdDisplay.MainView).GetSelectedRows();
                        //DataRowView selRow = (DataRowView)(((GridView)grdDisplay.MainView).GetRow(selRows[0]));

                        //string Action = selRow["action"].ToString();
                        //string Colony = selRow["COLONYNUMBER"].ToString();
                        //string ActionType = selRow["action"].ToString();
                        //string SubAgarID = selRow["SUBREQMBAGARID"].ToString();

                        //selRow["action"] = objActionTypeM.ActionType_Instrument;
                        //selRow["actiondetail"] = ActionDetail;
                        //selRow["ORGANISMID"] = fm.SelectedID;

                        //DataRow dr;

                        //dr = dtCultureData.NewRow();

                        //dr["datetime"] = DateTime.Now.ToString();
                        //dr["LOGUSERID"] = ControlParameter.loginName;
                        //dr["COLONYNUMBER"] = Colony;
                        //dr["SUBREQMBAGARID"] = SubAgarID;

                        //dtCultureData.Rows.Add(dr);

                        //dr = null;

                        //CultueDataBind.DataSource = dtCultureData;

                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
            finally
            {
                fm = null;
                dt = null;
            }
        }

        private void gridView5_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string vMin;
            string vMax;
            string signMin;
            string signMax;
            string method;
            string currentvalue = "";
            Boolean bNext = false;
            string ColonyNo = "";
            string SenID = "";
            string AntiID = "";
            string OldResult = "";
            string OrganismID = "";
            TreeNode SelectNode = tMain.SelectedNode;
            TreeNode ParentNode = SelectNode.Parent;
            string sResultValue = "";
            //// Create an empty list.
            ArrayList rows = new ArrayList();
            string value;
            string Colonyid = SelectNode.Name.ToString();

            int[] selActionRows = ((GridView)grdDisplay.MainView).GetSelectedRows();

            //DataRowView selActionRow = (DataRowView)(((GridView)grdDisplay.MainView).GetRow(selActionRows[0]));

            // Add the selected rows to the list.
            for (int i = 0; i < gridView5.SelectedRowsCount; i++)
            {
                if (gridView5.GetSelectedRows()[i] >= 0)
                    rows.Add(gridView5.GetDataRow(gridView5.GetSelectedRows()[i]));
            }
            try
            {
                gridView5.BeginUpdate();
                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i] as DataRow;
                    try
                    {
                        vMin = row["CONCLOWVALUE"].ToString();
                        vMax = row["CONCHIGHVALUE"].ToString();
                        value = row["RESULTVALUE"].ToString();
                        currentvalue = row[1].ToString();
                        signMin = row["THRESHOLDLOWER"].ToString();
                        signMax = row["THRESHOLDHIGHER"].ToString();
                        method = row["METHODMBCODE"].ToString();
                        ColonyNo = row["COLONYNUMBER"].ToString();
                        OldResult = row["OLDRESULT"].ToString();

                        AntiID = row["ANTIBIOTICID"].ToString();

                        int ANTIBIOTICID = Convert.ToInt32(row["ANTIBIOTICID"].ToString());

                        SenID = row["SENSITIVITYID"].ToString();

                        dtAntibioticBreakpoint.DefaultView.RowFilter = " ANTIBIOTICID = " + AntiID + " and SENSITIVITYID = " + SenID + " and COLONYNUMBER = '" + ColonyNo.Trim() + "'";
                        dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        String CLSI_S = "";
                        String CLSI_I = "";
                        String CLSI_R = "";
                        String CLSI_SDD = "";
                        String OldValue = "";

                        //if (dtAntibioticBreakpoint.DefaultView.Count >= 0)
                        //{
                        //    OldValue = dtAntibioticBreakpoint.DefaultView[0]["RESULTVALUE"].ToString();
                        //    OldResult = dtAntibioticBreakpoint.DefaultView[0]["RESULT"].ToString();
                        //}

                        Boolean bActive = false;

                        if (OldResult == "")
                        {
                            bActive = true;
                        }

                        if (bActive == false)
                        {
                            if (value != OldValue)
                            {
                                bActive = true;
                            }
                        }

                        bActive = true;

                        if (bActive == true)
                        {

                            dtCultureData.DefaultView.RowFilter = " action = '" + objActionTypeM.ActionType_Identification + "' AND COLONYNUMBER = '" + dtCultureData.Rows[i]["COLONYNUMBER"].ToString() + "'";
                            dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            if (dtCultureData.DefaultView.Count > 0)
                            {
                                if (dtCultureData.DefaultView[0]["MICID"].ToString() != "")
                                {
                                    BreakpointData.DefaultView.RowFilter = " ANTIBIOTICID = " + ANTIBIOTICID + " and MICID = '" + dtCultureData.DefaultView[0]["MICID"].ToString() + "'";
                                    BreakpointData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                                }
                                else
                                {
                                    BreakpointData.DefaultView.RowFilter = " ANTIBIOTICID = " + ANTIBIOTICID;
                                    BreakpointData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                                }

                                OrganismID = dtCultureData.DefaultView[0]["ORGANISMID"].ToString();

                            }
                            else
                            {
                                OrganismID = "";

                                BreakpointData.DefaultView.RowFilter = " ANTIBIOTICID = " + ANTIBIOTICID;
                                BreakpointData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                            }

                            //if (selActionRow["Action"].ToString () == objActionTypeM.ActionType_Identification)
                            //{
                            //    BreakpointData.DefaultView.RowFilter = " ANTIBIOTICID = " + ANTIBIOTICID + " and MICID = '" + selActionRow["MICID"].ToString () + "'";
                            //    BreakpointData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                            //}
                            //else 
                            //{
                            //    BreakpointData.DefaultView.RowFilter = " ANTIBIOTICID = " + ANTIBIOTICID;
                            //    BreakpointData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                            //}

                            if (BreakpointData.DefaultView.Count > 0)
                            {
                                if (method == "MIC")
                                {
                                    CLSI_S = BreakpointData.DefaultView[0]["MIDS"].ToString();
                                    CLSI_I = BreakpointData.DefaultView[0]["MIDI"].ToString();
                                    CLSI_R = BreakpointData.DefaultView[0]["MIDR"].ToString();
                                    CLSI_SDD = BreakpointData.DefaultView[0]["MIDSDD"].ToString();

                                }
                                else
                                {
                                    if (method == "MID")
                                    {
                                        CLSI_S = BreakpointData.DefaultView[0]["MICS"].ToString();
                                        CLSI_I = BreakpointData.DefaultView[0]["MICI"].ToString();
                                        CLSI_R = BreakpointData.DefaultView[0]["MICR"].ToString();
                                        CLSI_SDD = BreakpointData.DefaultView[0]["MICSDD"].ToString();
                                    }
                                }
                            }

                            string[] separatingChars = { "<=", ">=", "=", ">", "<", "-" };

                            double tempvalue = 0;

                            double iValue = 0;

                            if (IsNumeric(value.ToString()) == true)
                            {
                                iValue = Convert.ToDouble(value.ToString());
                            }
                            else 
                            {
                                string[] TmpValue = value.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);

                                if (TmpValue.Length == 1) 
                                {
                                    if (IsNumeric(TmpValue[0]) == true) 
                                    {
                                        tempvalue = Convert.ToDouble(TmpValue[0].ToString());

                                        if (value.Contains(">") == true)
                                        {
                                            tempvalue += 1;
                                        }
                                        else
                                        {
                                            if (value.Contains("<") == true) 
                                            {
                                                tempvalue -= 1;
                                            }
                                        }
                                    }

                                    
                                }

                                iValue = tempvalue;

                            }
                           

                            //string[] separatingChars = { "<<", "..." };
                            //CLSI S**********************************************
                            string[] Tmp = CLSI_S.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);

                            if (IsNumeric(CLSI_S.ToString()) == true)
                            {
                                if (iValue.ToString() == CLSI_S)
                                {
                                    //row["RESULT"] = "S";
                                    sResultValue = "S";
                                    bNext = false;
                                }
                                else
                                {
                                    bNext = true;
                                }
                            }
                            else
                            {
                                if (Tmp.Length == 1)
                                {
                                    double iCLSI_S = Convert.ToDouble(Tmp[0]);

                                    bNext = false;

                                    if (CLSI_S.Contains(">=") == true)
                                    {
                                        if (iValue >= iCLSI_S)
                                        {
                                            //row["RESULT"] = "S";
                                            sResultValue = "S";
                                            bNext = false;
                                        }
                                        else
                                        {
                                            bNext = true;
                                        }
                                    }
                                    else
                                    {
                                        if (CLSI_S.Contains("<=") == true)
                                        {
                                            if (iValue <= iCLSI_S)
                                            {
                                                //row["RESULT"] = "S";
                                                sResultValue = "S";
                                                bNext = false;
                                            }
                                            else
                                            {
                                                bNext = true;
                                            }
                                        }
                                        else
                                        {
                                            if (CLSI_S.Contains("<") == true)
                                            {
                                                if (iValue < iCLSI_S)
                                                {
                                                    //row["RESULT"] = "S";
                                                    sResultValue = "S";
                                                    bNext = false;
                                                }
                                                else
                                                {
                                                    bNext = true;
                                                }
                                            }
                                            else
                                            {
                                                if (CLSI_S.Contains(">") == true)
                                                {
                                                    if (iValue > iCLSI_S)
                                                    {
                                                        //row["RESULT"] = "S";
                                                        sResultValue = "S";
                                                        bNext = false;
                                                    }
                                                    else
                                                    {
                                                        bNext = true;
                                                    }
                                                }
                                                else
                                                {

                                                    bNext = true;
                                                }
                                            }

                                        }

                                    }

                                }
                                else
                                {
                                    if (Tmp.Length > 1)
                                    {
                                        double iMin = Convert.ToDouble(Tmp[0]);
                                        double iMax = Convert.ToDouble(Tmp[1]);

                                        if ((iValue >= iMin) || (iValue <= iMax))
                                        {
                                            //row["RESULT"] = "S";
                                            sResultValue = "S";
                                            bNext = false;
                                        }
                                        else
                                        {
                                            bNext = true;
                                        }

                                    }
                                }
                            }

                            //CLSI_I *****************************
                            if (bNext == true)
                            {
                                Tmp = CLSI_I.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);

                                if (IsNumeric(CLSI_I) == true)
                                {
                                    if (CLSI_I.ToString() == value.ToString())
                                    {
                                        //row["RESULT"] = "I";
                                        sResultValue = "I";
                                        bNext = false;
                                    }
                                    else
                                    {
                                        bNext = true;
                                    }
                                }
                                else
                                {
                                    if (Tmp.Length == 1)
                                    {
                                        double iCLSI_I = Convert.ToDouble(Tmp[0]);

                                        bNext = false;

                                        if (CLSI_S.Contains(">=") == true)
                                        {
                                            if (iValue >= iCLSI_I)
                                            {
                                                //row["RESULT"] = "I";
                                                sResultValue = "I";
                                                bNext = false;
                                            }
                                            else
                                            {
                                                bNext = true;
                                            }
                                        }
                                        else
                                        {
                                            if (CLSI_S.Contains("<=") == true)
                                            {
                                                if (iValue <= iCLSI_I)
                                                {
                                                    //row["RESULT"] = "I";
                                                    sResultValue = "I";
                                                    bNext = false;
                                                }
                                                else
                                                {
                                                    bNext = true;
                                                }
                                            }
                                            else
                                            {
                                                if (CLSI_S.Contains("<") == true)
                                                {
                                                    if (iValue < iCLSI_I)
                                                    {
                                                        //row["RESULT"] = "I";
                                                        sResultValue = "I";
                                                        bNext = false;
                                                    }
                                                    else
                                                    {
                                                        bNext = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (CLSI_S.Contains(">") == true)
                                                    {
                                                        if (iValue > iCLSI_I)
                                                        {
                                                            //row["RESULT"] = "I";
                                                            sResultValue = "I";
                                                            bNext = false;
                                                        }
                                                        else
                                                        {
                                                            bNext = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bNext = true;
                                                    }
                                                }

                                            }

                                        }

                                    }
                                    else
                                    {
                                        if (Tmp.Length > 1)
                                        {
                                            double iMin = Convert.ToDouble(Tmp[0]);
                                            double iMax = Convert.ToDouble(Tmp[1]);

                                            if ((iValue >= iMin) && (iValue <= iMax))
                                            {
                                                //row["RESULT"] = "I";
                                                sResultValue = "I";
                                                bNext = false;
                                            }
                                            else
                                            {
                                                bNext = true;
                                            }

                                        }
                                        else
                                        {
                                            bNext = true;
                                        }
                                    }

                                }

                            }

                            //CLSI_I *****************************
                            if (bNext == true)
                            {
                                Tmp = CLSI_R.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);

                                if (IsNumeric(CLSI_R) == true)
                                {
                                    if (CLSI_R.ToString() == value.ToString())
                                    {
                                        //row["RESULT"] = "R";
                                        sResultValue = "R";
                                        bNext = false;
                                    }
                                    else
                                    {
                                        bNext = true;
                                    }
                                }
                                else
                                {
                                    if (Tmp.Length == 1)
                                    {
                                        double iCLSI_R = Convert.ToDouble(Tmp[0]);

                                        bNext = false;

                                        if (CLSI_R.Contains(">=") == true)
                                        {
                                            if (iValue >= iCLSI_R)
                                            {
                                                //row["RESULT"] = "R";
                                                sResultValue = "R";
                                                bNext = false;
                                            }
                                            else
                                            {
                                                bNext = true;
                                            }
                                        }
                                        else
                                        {
                                            if (CLSI_R.Contains("<=") == true)
                                            {
                                                if (iValue <= iCLSI_R)
                                                {
                                                    //row["RESULT"] = "R";
                                                    sResultValue = "R";
                                                    bNext = false;
                                                }
                                                else
                                                {
                                                    bNext = true;
                                                }
                                            }
                                            else
                                            {
                                                if (CLSI_R.Contains("<") == true)
                                                {
                                                    if (iValue < iCLSI_R)
                                                    {
                                                        //row["RESULT"] = "R";
                                                        sResultValue = "R";
                                                        bNext = false;
                                                    }
                                                    else
                                                    {
                                                        bNext = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (CLSI_R.Contains(">") == true)
                                                    {
                                                        if (iValue > iCLSI_R)
                                                        {
                                                            //row["RESULT"] = "R";
                                                            sResultValue = "R";
                                                            bNext = false;
                                                        }
                                                        else
                                                        {
                                                            bNext = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bNext = true;
                                                    }
                                                }

                                            }

                                        }

                                    }
                                    else
                                    {
                                        if (Tmp.Length > 1)
                                        {
                                            double iMin = Convert.ToDouble(Tmp[0]);
                                            double iMax = Convert.ToDouble(Tmp[1]);

                                            if ((iValue >= iMin) && (iValue <= iMax))
                                            {
                                                //row["RESULT"] = "R";
                                                sResultValue = "R";
                                                bNext = false;
                                            }
                                            else
                                            {
                                                bNext = true;
                                            }

                                        }
                                    }

                                }


                            }

                            //CLSI_SDD *****************************
                            //CLSI_I *****************************
                            if (bNext == true)
                            {
                                Tmp = CLSI_SDD.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);

                                if (IsNumeric(CLSI_SDD) == true)
                                {
                                    if (CLSI_SDD.ToString() == value.ToString())
                                    {
                                        //row["RESULT"] = "R";
                                        sResultValue = "R";
                                        bNext = false;
                                    }
                                    else
                                    {
                                        bNext = true;
                                    }
                                }
                                else
                                {
                                    if (Tmp.Length == 1)
                                    {
                                        double iCLSI_SDD = Convert.ToDouble(Tmp[0]);

                                        bNext = false;

                                        if (CLSI_R.Contains(">=") == true)
                                        {
                                            if (iValue >= iCLSI_SDD)
                                            {
                                                //row["RESULT"] = "R";
                                                sResultValue = "SDD";
                                                bNext = false;
                                            }
                                            else
                                            {
                                                bNext = true;
                                            }
                                        }
                                        else
                                        {
                                            if (CLSI_SDD.Contains("<=") == true)
                                            {
                                                if (iValue <= iCLSI_SDD)
                                                {
                                                    //row["RESULT"] = "R";
                                                    sResultValue = "SDD";
                                                    bNext = false;
                                                }
                                                else
                                                {
                                                    bNext = true;
                                                }
                                            }
                                            else
                                            {
                                                if (CLSI_R.Contains("<") == true)
                                                {
                                                    if (iValue < iCLSI_SDD)
                                                    {
                                                        //row["RESULT"] = "R";
                                                        sResultValue = "SDD";
                                                        bNext = false;
                                                    }
                                                    else
                                                    {
                                                        bNext = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (CLSI_R.Contains(">") == true)
                                                    {
                                                        if (iValue > iCLSI_SDD)
                                                        {
                                                            //row["RESULT"] = "R";
                                                            sResultValue = "SDD";
                                                            bNext = false;
                                                        }
                                                        else
                                                        {
                                                            bNext = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bNext = true;
                                                    }
                                                }

                                            }

                                        }

                                    }
                                    else
                                    {
                                        if (Tmp.Length > 1)
                                        {
                                            double iMin = Convert.ToDouble(Tmp[0]);
                                            double iMax = Convert.ToDouble(Tmp[1]);

                                            if ((iValue >= iMin) && (iValue <= iMax))
                                            {
                                                //row["RESULT"] = "R";
                                                sResultValue = "SDD";
                                                bNext = false;
                                            }
                                            else
                                            {
                                                bNext = true;
                                            }

                                        }
                                    }

                                }


                            }

                            if (bNext == true)
                            {
                                sResultValue = "";
                                //row["RESULT"] = "";
                            }
                        }

                        if (currentvalue == "")
                        {
                            row["RESULT"] = sResultValue;
                            row["OLDRESULT"] = sResultValue;                            
                        }
                        else 
                        {
                            if (currentvalue != sResultValue) 
                            {
                                if (OldResult != currentvalue)
                                {
                                    row["RESULT"] = currentvalue;
                                    row["OLDRESULT"] = currentvalue;
                                }
                                else 
                                {
                                    row["RESULT"] = sResultValue;
                                    row["OLDRESULT"] = sResultValue;
                                }
                                
                            }
                        }

                        dtCultureData.DefaultView.RowFilter = String.Empty;

                        dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + tMain.SelectedNode.Tag + "'";
                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        dtAntibioticBreakpoint.DefaultView.RowFilter = String.Empty;

                        dtAntibioticBreakpoint.DefaultView.RowFilter = " COLONYNUMBER = '" + ColonyNo + "' and SENSITIVITYID = '" + SenID.Trim() + "'";
                        dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        // Change the field value.
                        // row["RESULTVALUE"] = "X";
                        ExpertRules(OrganismID, AntiID, row["RESULT"].ToString());

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                }
            }
            finally
            {
                gridView5.EndUpdate();
            }
        }

        private void ExpertRules(String OrganismID, String AnitibioticID, String Breakpoint)
        {
            Boolean bTrigger = true;

            if (OrganismID == "")
            {
                bTrigger = false;
            }

            if (AnitibioticID == "")
            {
                bTrigger = false;
            }

            if (Breakpoint == "")
            {
                bTrigger = false;
            }

            if (bTrigger == true)
            {

                String sBreakpoint = "";
                String sCondition = "";
                String sDisplay = "";
                String sAntibiotic = "";

                dtRulesBase.DefaultView.RowFilter = " ORGANISMID = " + OrganismID + " and ANTIBIOTICID = " + AnitibioticID;
                dtRulesBase.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                if (dtRulesBase.DefaultView.Count > 0)
                {
                    sBreakpoint = dtRulesBase.DefaultView[0]["INSTRINSIC"].ToString();
                    sCondition = dtRulesBase.DefaultView[0]["CONDITION"].ToString();
                    sDisplay = dtRulesBase.DefaultView[0]["ALERTMESSAGE"].ToString();
                    sAntibiotic = dtRulesBase.DefaultView[0]["ANTIBIOTICCODE"].ToString();

                    if (sCondition.Contains("OR"))
                    {
                        if (sBreakpoint.Contains(Breakpoint) == true)
                        {
                            DataRow dr;
                            int i = 0;

                            if (dtExpertDisplay != null)
                            {
                                if (dtExpertDisplay.Rows.Count == 0)
                                {
                                    i = 1;
                                }
                                else
                                {
                                    i = dtExpertDisplay.Rows.Count + 1;
                                }
                            }

                            if (CheckDuplicate(sDisplay.Replace("@FULLTEXT", sAntibiotic)) == false)
                            {
                                dr = dtExpertDisplay.NewRow();

                                dr[0] = i.ToString();
                                dr[1] = sDisplay.Replace("@FULLTEXT", sAntibiotic);

                                dtExpertDisplay.Rows.Add(dr);

                            }
                            dr = null;

                            if (frmParent == null)
                            {
                                frmParent = (frmStain)this.ParentForm;
                            }

                            frmParent.dtExpertRule = dtExpertDisplay;
                            frmParent.GetDisplayExpertRule();

                            //GetDisplayExpertRule();
                            //pExpertrule.Visible = true;

                        }
                    }
                }

            }

        }

        private Boolean CheckDuplicate(String sDisplay)
        {
            Boolean bDup = false;

            for (int i = 0; i <= dtExpertDisplay.Rows.Count - 1; i++)
            {
                if (dtExpertDisplay.Rows[i][1].ToString() == sDisplay)
                {
                    bDup = true;
                    break;
                }
            }
            return bDup;
        }
        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }


        private void gridView5_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "UNITS")
            {
                if (Convert.ToString(e.Value) == "0")
                {
                    e.DisplayText = "mg/L";
                }
                else
                {
                    if (Convert.ToString(e.Value) == "1")
                    {
                        e.DisplayText = "mm";
                    }
                    else
                    {
                        e.DisplayText = "n/a";
                    }
                }
            }
            //if (Convert.ToString(e.Value) == "1") e.DisplayText = "mg/L";
            //else e.DisplayText = "mm";
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            try
            {
                int[] selRows = ((GridView)grdDisplay.MainView).GetSelectedRows();

                for (int i = 0; i <= selRows.Length - 1; i++)
                {
                    DataRowView selRow = (DataRowView)(((GridView)grdDisplay.MainView).GetRow(selRows[0]));

                    if (selRow["Action"].ToString() == objActionTypeM.ActionType_Identification)
                    {
                        TestColonyResultM objTestOrganism = new TestColonyResultM();

                        if (IsNumeric (selRow["COLONYID"].ToString()) == true) 
                        {
                            objTestOrganism.ColonyID = Convert.ToInt64(selRow["COLONYID"].ToString());
                            objTestOrganism.SubReqMBAgarID = objTestResult.MBRequestID;
                            objTestOrganism.OrganismID = Convert.ToInt32(selRow["ORGANISMID"].ToString());

                            objTesting.DeleteOrganism(objTestOrganism);

                        }
                        

                        foreach (TreeNode Node in tMain.Nodes)
                        {
                            foreach (TreeNode ChildNode in Node.Nodes)
                            {
                                if (ChildNode.Level == 1)
                                {
                                    if (ChildNode.Tag.ToString() == selRow["COLONYNUMBER"].ToString())
                                    {
                                        String sTmp = DateTime.Now.ToString("ddMMyyyyHH:mm:ss");

                                        ChildNode.Text = "(" + selRow["COLONYNUMBER"].ToString() + "-" + sTmp + ")";

                                        break;

                                    }
                                }
                            }

                        }

                        CalculateExpertRule();

                        AddIdentification.Enabled = true;

                    }
                    else
                    {
                        if (selRow["Action"].ToString() == objActionTypeM.ActionType_Sensitivities)
                        {
                            TestColonyResultSelect = new TestColonyResultM();
                            TestColonyResultSelect.MBRequestID = objTestResult.MBRequestID;
                            TestColonyResultSelect.ColonyID = Convert.ToInt64(selRow["COLONYID"].ToString());

                            String SubReqSenID = selRow["SUBREQMBSENSITIVITYID"].ToString();

                            dtAntibioticBreakpoint.DefaultView.RowFilter = " colonynumber = '" + selRow["colonynumber"].ToString() + "' and SENSITIVITYID = " + selRow["SENSITIVITYID"].ToString();
                            dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            if (dtAntibioticBreakpoint.DefaultView.Count > 0)
                            {
                                SubReqSenID = dtAntibioticBreakpoint.DefaultView[0]["SUBREQMBSENSITIVITYID"].ToString();
                                foreach (DataRowView row in dtAntibioticBreakpoint.DefaultView)
                                {
                                    row.Delete();
                                }

                            }

                            TestSensitivitiesResultM objSenM = new TestSensitivitiesResultM();

                            if (SubReqSenID != "")
                            {
                                objSenM.SubReqSensitivitiesID = Convert.ToInt64(SubReqSenID);
                                objTesting.DeleteAntibiotic(TestColonyResultSelect, SubReqSenID);
                                objTesting.DeleteSensitivities(objSenM);
                            }
                            

                        }
                        else 
                        {
                            if (selRow["Action"].ToString() == objActionTypeM.ActionType_Instrument)
                            {
                                TestBatteriesResultM objBateryM = new TestBatteriesResultM();
                                if (IsNumeric(selRow["COLONYID"].ToString()) == true) 
                                {
                                    objBateryM.ColonyID = Convert.ToInt64(selRow["COLONYID"].ToString());
                                    objBateryM.BatteryID = Convert.ToInt64(selRow["BATTERYID"].ToString());

                                    objTesting.DeleteBattery(objBateryM);

                                }

                                objBateryM = null;

                            }
                            else if (selRow["actiontype"].ToString() == objActionTypeM.ActionType_DetectionTest) 
                            {
                                TestDetectionTestResultM objDetectionM = new TestDetectionTestResultM();

                                if (IsNumeric(selRow["COLONYID"].ToString()) == true) 
                                {
                                    objDetectionM.ColonyID = Convert.ToInt64(selRow["COLONYID"].ToString());
                                    objDetectionM.DetectionTestID = Convert.ToInt16(selRow["DETECTIONID"].ToString());

                                    objTesting.DeleteDetectionTest(objDetectionM);

                                }

                                objDetectionM = null;

                            }
                            else if (selRow["actiontype"].ToString() == objActionTypeM.ActionType_Biochemistry)
                            {
                                TestBiochemistryResultM objBiochemryM = new TestBiochemistryResultM();

                                if (IsNumeric(selRow["COLONYID"].ToString()) == true) 
                                {
                                    objBiochemryM.CHEMISTRYID = Convert.ToInt64(selRow["CHEMISTRYID"].ToString());
                                    objBiochemryM.ColonyID = Convert.ToInt16(selRow["COLONYID"].ToString());
                                    objBiochemryM.SubReqMBID = objTestResult.MBRequestID;

                                    objTesting.DeleteBiochemtry(objBiochemryM);
                                }
                              
                                objBiochemryM = null;

                            }

                        }
                    }

                    dtAntibioticBreakpoint.DefaultView.RowFilter = " colonynumber = '" + selRow["colonynumber"].ToString() + "'";
                    dtCultureData = (DataTable)CultueDataBind.DataSource;

                    dtCultureData.Rows.Remove(selRow.Row);

                    CalculateExpertRule();

                }

                
                //string rootStain = selRow["STAINNAME"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
        }

        private void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                loadComboDetectionTest();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
        }

        private void addAntibioticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearchList fm = new frmSearchList();
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt;
            try
            {

                dt = objConfig.GetDictAniboticsData("", "");

                dt.Columns["ANTIBIOTICCODE"].ColumnName = "code";
                dt.Columns["FULLTEXT"].ColumnName = "name";
                dt.Columns["ANTIBIOTICID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.IsSensitivities = true;

                fm.ShowDialog();

                
                
                if (fm.Selected == true)
                {
                    TreeNode SelectNode = tMain.SelectedNode;
                    
                    //TreeNode ParentNode = SelectNode.Parent;
                    DataRow dr;

                    string Action = objActionTypeM.ActionType_Sensitivities;
                    string Colony = SelectNode.Tag.ToString();
                    string ColonyID = SelectNode.Name.ToString();
                    string ActionDetail = fm.SelectedCode + ":" + fm.SelectedName;
                    string SubAgarID = objTestResult.MBRequestID.ToString();

                    dtCultureData = (DataTable)CultueDataBind.DataSource;

                    dtCultureData.DefaultView.RowFilter = " action = '" + objActionTypeM.ActionType_Sensitivities + "' and " + " COLONYNUMBER = '" + Colony + "'";
                    dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    if (dtCultureData.DefaultView.Count > 0)
                    {
                        dr = dtAntibioticBreakpoint.NewRow();

                        dr["FULLTEXT"] = fm.SelectedName;
                        dr["SENSITIVITYID"] = dtCultureData.DefaultView[0]["SENSITIVITYID"].ToString();
                        dr["ANTIBIOTICID"] = fm.SelectedID;
                        dr["SUBREQMBORGID"] = SubAgarID;
                        dr["COLONYID"] = ColonyID;
                        dr["COLONYNUMBER"] = Colony;
                        dr["METHODID"] = fm.MethodID;
                        dr["PRINTSTATUS"] = "0";

                        if (fm.MethodID == 1)
                        {
                            dr["UNITS"] = "0";
                            dr["METHODMBCODE"] = "MIC";
                        }
                        else
                        {
                            if (fm.MethodID == 2)
                            {
                                dr["UNITS"] = "1";
                                dr["METHODMBCODE"] = "MID";
                            }
                            else
                            {
                                dr["UNITS"] = "2";
                                dr["METHODMBCODE"] = "N/A";
                            }

                        }
                        dtAntibioticBreakpoint.Rows.Add(dr);

                        dr = null;

                    }

                    dtAntibioticBreakpoint.DefaultView.RowFilter = " COLONYNUMBER = '" + Colony + "'";
                    dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    if (dtAntibioticBreakpoint.DefaultView.Count > 0)
                    {
                        grdAnti.DataSource = dtAntibioticBreakpoint;
                    }

                    List<string> objResultList = new List<string>();

                    objResultList.Add(" ");
                    objResultList.Add("N/A");
                    objResultList.Add("S");
                    objResultList.Add("I");
                    objResultList.Add("R");
                    objResultList.Add("SDD");
                    objResultList.Add("NS");
                    objResultList.Add("NR");

                    riComboResult = new RepositoryItemComboBox();

                    riComboResult.Items.AddRange(objResultList);

                    //Add the item to the internal repository                    
                    grdAnti.RepositoryItems.Add(riComboResult);

                    //Now you can define the repository item as an in-place column editor
                    colRESULT.ColumnEdit = riComboResult;

                    riComboResult.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                    dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + Colony + "'";
                    dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
            finally
            {
                fm = null;
                dt = null;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int[] selRows = ((GridView)grdAnti.MainView).GetSelectedRows();
                DataRowView selRow = (DataRowView)(((GridView)grdAnti.MainView).GetRow(selRows[0]));
                String SenID = selRow["SENSITIVITYID"].ToString();
                String COLONYNUMBER = selRow["COLONYNUMBER"].ToString();
                String AntiID = selRow["ANTIBIOTICID"].ToString();

                dtAntibioticBreakpoint.DefaultView.RowFilter = " SENSITIVITYID = '" + SenID + "' and COLONYNUMBER = '" + COLONYNUMBER + "' and ANTIBIOTICID = '" + AntiID + "'";
                dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                if (dtAntibioticBreakpoint.DefaultView.Count > 0)
                {
                    foreach (DataRowView row in dtAntibioticBreakpoint.DefaultView)
                    {
                        row.Delete();
                    }
                }

                dtAntibioticBreakpoint.DefaultView.RowFilter = " SENSITIVITYID = '" + SenID + "' and COLONYNUMBER = '" + COLONYNUMBER + "' ";
                dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                grdAnti.DataSource = dtAntibioticBreakpoint;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveData()
        {
            //String sTmp = "";
            TestSensitivitiesResultM objTestSen;
            TestAntibioticsResultM objTestAnti;
            TestBatteriesResultM objTestBatteries;
            TestColonyResultM objColonyResult;

            int iOrganismIndex = 0;

            TestAgarsResultM objTestAgarM = new TestAgarsResultM();
            TestColonyResultM objColonyM;
            try
            {
                // Save Media and colony 
                //

                Cursor.Current = Cursors.WaitCursor;

              
                if (dtMedia != null) 
                {
                    //Delete All 
                    objTesting.DeleteAllResult(Convert.ToInt16(dtMedia.Rows[0]["SUBREQUESTID"].ToString()));

                    for (int i = 0; i <= dtMedia.Rows.Count - 1; i++)
                    {
                        objTestAgarM.RequestID = Convert.ToInt16(dtMedia.Rows[i]["REQUESTID"].ToString());
                        objTestAgarM.AgarID = Convert.ToInt16(dtMedia.Rows[i]["AGARID"].ToString());
                        objTestAgarM.SubrequestID = Convert.ToInt16(dtMedia.Rows[i]["SUBREQUESTID"].ToString());
                        objTestAgarM.AgarNumber = (i + 1).ToString();
                        objTestAgarM.AgarIndex = i + 1;
                        objTestAgarM.SubReqMBAgarID = Convert.ToInt16(dtMedia.Rows[i]["SUBREQMBAGARID"].ToString());
                        objTestAgarM.CreateBy = ControlParameter.loginID;
                        objTestAgarM.UpdateBy = ControlParameter.loginID ;
                        objTestAgarM.Status = "0";

                        objTestAgarM = objTesting.SaveAgar(objTestAgarM);

                        dtColonyData.DefaultView.RowFilter = " AGARID = '" + objTestAgarM.AgarID + "'";
                        dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        for (int x = 0; x <= dtColonyData.DefaultView.Count - 1; x++)
                        {
                            DataRowView dv = dtColonyData.DefaultView[x];

                            objColonyM = new TestColonyResultM();

                            objColonyM.AgarID = objTestAgarM.AgarID;
                            objColonyM.SubReqMBAgarID = objTestAgarM.SubReqMBAgarID;
                            objColonyM.RequestID = Convert.ToInt16(objTestAgarM.RequestID);
                            objColonyM.MBRequestID = objTestResultM.MBRequestID;
                            objColonyM.ColonyNumber = dtColonyData.DefaultView[x]["COLONYNUMBER"].ToString();
                            objColonyM.ColonyIndex = x + 1;
                            objColonyM.CreateBy = ControlParameter.loginID;
                            objColonyM.UpdateBy = ControlParameter.loginID ;
                            objColonyM.Status = "0";

                            if (dtColonyData.DefaultView[x]["colonyid"].ToString() != "0")
                            {
                                if (dtColonyData.DefaultView[x]["colonyid"].ToString() != "")
                                {
                                    objColonyM.ColonyID = Convert.ToInt64(dtColonyData.DefaultView[x]["colonyid"].ToString());
                                }
                                else
                                {
                                    objColonyM.ColonyID = 0;
                                }

                            }

                            //dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + dtColonyData.DefaultView[x]["COLONYNUMBER"].ToString() + "' and action = '" + objActionTypeM.ActionType_Identification + "'";
                            //dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            //if (dtCultureData.DefaultView.Count > 0)
                            //{
                            //    objColonyM.OrganismID = Convert.ToInt16(dtCultureData.DefaultView[0]["ORGANISMID"].ToString());
                            //    //objColonyM.OrganismID = 
                            //}
                            //else 
                            //{
                            //    objColonyM.OrganismID = 0;
                            //}

                            objColonyM.ColonyComment = "";

                            objColonyM = objTesting.SaveColony(objColonyM);

                            dv["COLONYID"] = objColonyM.ColonyID;

                        }
                    }

                    dtCultureData.DefaultView.RowFilter = String.Empty;
                    dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    dtColonyData.DefaultView.RowFilter = String.Empty;
                    dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    for (int i = 0; i <= dtCultureData.Rows.Count - 1; i++)
                    {
                        DataRowView dv = dtCultureData.DefaultView[i];

                        dtColonyData.DefaultView.RowFilter = " COLONYNUMBER = '" + dtCultureData.Rows[i]["COLONYNUMBER"].ToString() + "'";
                        dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        if (dtCultureData.Rows[i]["action"].ToString() == objActionTypeM.ActionType_Sensitivities)
                        {
                            objTestSen = new TestSensitivitiesResultM();

                            objTestSen.SubReqID = Convert.ToInt16(objTestResult.MBRequestID);
                            objTestSen.SensitivitiesID = Convert.ToInt64(dtCultureData.Rows[i]["SENSITIVITYID"].ToString());
                            objTestSen.LoginBy = ControlParameter.loginName;
                            objTestSen.COLONYNUMBER = dtCultureData.Rows[i]["COLONYNUMBER"].ToString();
                            objTestSen.NOTPRINTABLE = dtCultureData.Rows[i]["PRINTSTATUS"].ToString();

                            if (dtCultureData.Rows[i]["COLONYID"].ToString() != "0")
                            {
                                if (dtColonyData.DefaultView[0]["COLONYID"].ToString() != "")
                                {
                                    dtCultureData.Rows[i]["COLONYID"] = dtColonyData.DefaultView[0]["COLONYID"].ToString();
                                    objTestSen.COLONYID = Convert.ToInt64(dtCultureData.Rows[i]["COLONYID"].ToString());
                                }
                                else
                                {
                                    objTestSen.COLONYID = 0;
                                }

                            }
                            else
                            {
                                // Get Colony ID  
                                dtColonyData.DefaultView.RowFilter = " COLONYNUMBER = '" + objTestSen.COLONYNUMBER + "'";
                                dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                if (dtColonyData.DefaultView.Count > 0)
                                {
                                    if (dtColonyData.DefaultView[0]["COLONYID"].ToString() != "")
                                    {
                                        objTestSen.COLONYID = Convert.ToInt16(dtColonyData.DefaultView[0]["COLONYID"].ToString());
                                    }
                                }

                            }

                            objTestSen = objTesting.SaveSensitivities(objTestSen);

                            dv["SUBREQMBSENSITIVITYID"] = objTestSen.SubReqSensitivitiesID;

                            //dtAntibioticBreakpoint.DefaultView.RowFilter = String.Empty;
                            //dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            //dtAntibioticBreakpoint.DefaultView.RowFilter = " SENSITIVITYID = '" + objTestSen.SensitivitiesID + "' and COLONYNUMBER = '" + objTestSen.COLONYNUMBER + "'";
                            //dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            //for (int j = 0; j <= dtAntibioticBreakpoint.DefaultView.Count - 1; j++)
                            //{
                            //    objTestAnti = new TestAntibioticsResultM();

                            //    objTestAnti.SubReqID = Convert.ToInt16(objTestResult.MBRequestID);
                            //    objTestAnti.SubReqMBSensitivitiesID = Convert.ToInt16(objTestSen.SubReqSensitivitiesID );
                            //    objTestAnti.AntibioticID = Convert.ToInt16 (dtAntibioticBreakpoint.DefaultView[j]["ANTIBIOTICID"].ToString());
                            //    objTestAnti.MethodID = Convert.ToInt16(dtAntibioticBreakpoint.DefaultView[j]["METHODID"].ToString()) ;
                            //    objTestAnti.Result = dtAntibioticBreakpoint.DefaultView[j]["RESULT"].ToString();
                            //    objTestAnti.ResultValue = dtAntibioticBreakpoint.DefaultView[j]["RESULTVALUE"].ToString();
                            //    objTestAnti.CreateBy = objTestSen.LoginBy;
                            //    objTestAnti.Comment = "";
                            //    //objTestAnti.Comment = dtAntibioticBreakpoint.DefaultView[j]["comment"].ToString();
                            //    objTestAnti.Units = Convert.ToInt16(dtAntibioticBreakpoint.DefaultView[j]["UNITS"].ToString()); ;
                            //    objTestAnti.MethodID = Convert.ToInt16 (dtAntibioticBreakpoint.Rows[j]["METHODID"].ToString());
                            //    objTestAnti.MicID = 0;//Convert.ToInt16(dtAntibioticBreakpoint.Rows[j]["MICID"].ToString());

                            //    objTestAnti = objTesting.SaveAntibiotics (objTestAnti);

                            //    objTestAnti = null;

                            //}

                        }
                        else
                        {
                            if (dtCultureData.Rows[i]["action"].ToString() == objActionTypeM.ActionType_Identification)
                            {
                                objColonyResult = new TestColonyResultM();

                                objColonyResult.SubReqMBAgarID = Convert.ToInt64(objTestResult.MBRequestID);

                                dtColonyData.DefaultView.RowFilter = " COLONYNUMBER = '" + dtCultureData.Rows[i]["COLONYNUMBER"].ToString() + "'";
                                dtColonyData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                                if (dtColonyData.DefaultView.Count != 0)
                                {
                                    objColonyResult.ColonyID = Convert.ToInt64(dtColonyData.DefaultView[0]["COLONYID"].ToString());
                                    dtCultureData.Rows[i]["COLONYID"] = objColonyResult.ColonyID.ToString();
                                }
                                else
                                {
                                    throw new Exception("Cann't found colony");
                                }

                                //objColonyResult.ColonyID = Convert.ToInt64(dtCultureData.Rows[i]["COLONYID"].ToString());
                                objColonyResult.OrganismID = Convert.ToInt16(dtCultureData.Rows[i]["ORGANISMID"].ToString());

                                iOrganismIndex = iOrganismIndex + 1;

                                objColonyResult.OrganismIndex = iOrganismIndex;

                                objColonyResult.CreateBy = ControlParameter.loginID;
                                objColonyResult.SubReqMBAgarID = objTestResult.MBRequestID;
                                objColonyResult.OrganismComment = "";
                                objColonyResult.NOTPRINTABLE = dtCultureData.Rows[i]["PRINTSTATUS"].ToString();

                                TestColonyResultSelect = objTesting.SaveOrganism(objColonyResult);

                                dv["SUBREQMBORGID"] = TestColonyResultSelect.SubReqMBOrgID;

                                //objTesting.InsertOrganism(objColonyResult);

                                objColonyResult = null;

                            }
                            else
                            {
                                if (dtCultureData.Rows[i]["action"].ToString() == objActionTypeM.ActionType_Instrument)
                                {
                                    objTestBatteries = new TestBatteriesResultM();

                                    objTestBatteries.SubReqMBID = Convert.ToInt16(objTestResult.MBRequestID);
                                    objTestBatteries.ColonyID = Convert.ToInt64(dtColonyData.DefaultView[0]["COLONYID"].ToString());//Convert.ToInt64(dtCultureData.Rows[i]["COLONYID"].ToString());
                                    objTestBatteries.BatteryID = Convert.ToInt16(dtCultureData.Rows[i]["BATTERYID"].ToString());
                                    objTestBatteries.CreateBy = ControlParameter.loginID;
                                    objTestBatteries.Comment = dtCultureData.Rows[i]["comment"].ToString();
                                    objTestBatteries.NOTPRINTABLE = dtCultureData.Rows[i]["PRINTSTATUS"].ToString();

                                    objTesting.SaveBatteriesResult(objTestBatteries);

                                    CommonController objCommonCon = new CommonController();
                                    JobsInfoM objJobinfoM = new JobsInfoM();
                                    JobParaInfoM objJobParaM = new JobParaInfoM();

                                    objJobinfoM.LOGUSERID = ControlParameter.loginID;
                                    objJobinfoM.JOBNAME = "BatteryID:" + objTestBatteries.ColonyID + ":SubReqID:" + objTestBatteries.SubReqMBID.ToString() + "batterryid:" + objTestBatteries.BatteryID.ToString ();
                                    objJobinfoM.JOBPRIORITY = 1;
                                    objJobinfoM.JOBRANK = 0;
                                    objJobinfoM.JOBTYPEID = objTestBatteries.BatteryID;
                                    objJobinfoM.JOBSTATUS = 0;
                                    objJobinfoM.COMMENTTEXT = "";
                                    objJobinfoM.SubReqID = objTestAgarM.SubrequestID;
                                    objJobParaM.JOBPARAMNAME = "battery interface";
                                    objJobParaM.JOBPARAMTYPE = objTestBatteries.BatteryID;

                                    objCommonCon.JobManagement(objJobinfoM, objJobParaM);

                                    objJobParaM = null;
                                    objJobinfoM = null;
                                    objTestBatteries = null;

                                }
                                else
                                {
                                    if (dtCultureData.Rows[i]["actiontype"].ToString() == objActionTypeM.ActionType_DetectionTest)
                                    {
                                        //DetectionTestM objDetectionTestM = new DetectionTestM();
                                        TestDetectionTestResultM objDetectionTestResultM = new TestDetectionTestResultM();

                                        objDetectionTestResultM.SubReqMBID = Convert.ToInt16(objTestResult.MBRequestID);
                                        objDetectionTestResultM.ColonyID = Convert.ToInt64(dtColonyData.DefaultView[0]["COLONYID"].ToString());//Convert.ToInt64(dtCultureData.Rows[i]["COLONYID"].ToString());
                                        objDetectionTestResultM.DetectionTestID = Convert.ToInt16(dtCultureData.Rows[i]["DETECTIONID"].ToString());
                                        objDetectionTestResultM.Comment = dtCultureData.Rows[i]["comment"].ToString();
                                        objDetectionTestResultM.CreateBy = ControlParameter.loginID;
                                        objDetectionTestResultM.NOTPRINTABLE = dtCultureData.Rows[i]["PRINTSTATUS"].ToString();
                                        objDetectionTestResultM.CONSOLIDATIONSTATUS = "";
                                        objDetectionTestResultM.TESTRESULT = dtCultureData.Rows[i]["actiondetail"].ToString();
                                        objDetectionTestResultM.SUBREQMBAGARID = Convert.ToInt16(objTestAgarM.SubReqMBAgarID);

                                        objDetectionTestResultM = objTesting.SaveDetectionTestResult(objDetectionTestResultM);

                                        objDetectionTestResultM = null;

                                    }
                                    else
                                    {
                                        if (dtCultureData.Rows[i]["actiontype"].ToString() == objActionTypeM.ActionType_Biochemistry)
                                        {

                                            TestBiochemistryResultM objChemistryResultM = new TestBiochemistryResultM();

                                            objChemistryResultM.SubReqMBID = Convert.ToInt16(objTestResult.MBRequestID);
                                            objChemistryResultM.ColonyID = Convert.ToInt64(dtColonyData.DefaultView[0]["COLONYID"].ToString()); //Convert.ToInt64(dtCultureData.Rows[i]["COLONYID"].ToString());
                                            objChemistryResultM.CHEMISTRYID = Convert.ToInt16(dtCultureData.Rows[i]["CHEMISTRYID"].ToString());
                                            objChemistryResultM.Comment = dtCultureData.Rows[i]["comment"].ToString();
                                            objChemistryResultM.CreateBy = ControlParameter.loginID;
                                            objChemistryResultM.NOTPRINTABLE = dtCultureData.Rows[i]["PRINTSTATUS"].ToString();
                                            objChemistryResultM.CONSOLIDATIONSTATUS = "";
                                            objChemistryResultM.RESULTVALUE = dtCultureData.Rows[i]["actiondetail"].ToString();
                                            objChemistryResultM.CHEMISTRYRESULT = dtCultureData.Rows[i]["action"].ToString();
                                            objChemistryResultM.SUBREQMBAGARID = Convert.ToInt16(objTestAgarM.SubReqMBAgarID);

                                            objChemistryResultM = objTesting.SaveBiochemistryResult(objChemistryResultM);

                                            objChemistryResultM = null;


                                        }
                                    }
                                }
                            }
                        }

                    }

                    for (int j = 0; j <= dtAntibioticBreakpoint.Rows.Count - 1; j++)
                    {
                        objTestAnti = new TestAntibioticsResultM();

                        //dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + dtAntibioticBreakpoint.Rows[j]["COLONYNUMBER"].ToString() + "' and action = '" + objActionTypeM.ActionType_Identification + "'";
                        //dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        //if (dtCultureData.DefaultView.Count == 0)
                        //{
                        //    throw new Exception("cann't found organism with this colony no:" + dtAntibioticBreakpoint.Rows[j]["COLONYNUMBER"].ToString());
                        //}

                        //if (dtCultureData.DefaultView[0]["SUBREQMBORGID"].ToString() == "0")
                        //{
                        //    throw new Exception("cann't found organism with this colony no:" + dtAntibioticBreakpoint.Rows[j]["COLONYNUMBER"].ToString());
                        //}

                        if (dtCultureData.DefaultView[0]["SUBREQMBORGID"].ToString() != "")
                        {
                            objTestAnti.SUBREQMBORGID = Convert.ToInt32(dtCultureData.DefaultView[0]["SUBREQMBORGID"].ToString());

                            //throw new Exception("cann't found organism with this colony no:" + dtAntibioticBreakpoint.Rows[j]["COLONYNUMBER"].ToString());
                        }
                                                
                        dtCultureData.DefaultView.RowFilter = string.Empty;

                        dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + dtAntibioticBreakpoint.Rows[j]["COLONYNUMBER"].ToString() + "' and action = '" + objActionTypeM.ActionType_Sensitivities + "'";
                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        if (dtCultureData.DefaultView.Count == 0)
                        {
                            throw new Exception("cann't found sensitivities panel with this colony no:" + dtAntibioticBreakpoint.Rows[j]["COLONYNUMBER"].ToString());
                        }

                        objTestAnti.SubReqMBSensitivitiesID = Convert.ToInt32(dtCultureData.DefaultView[0]["SUBREQMBSENSITIVITYID"].ToString());

                        dtCultureData.DefaultView.RowFilter = string.Empty;

                        objTestAnti.SubReqID = Convert.ToInt16(objTestResult.MBRequestID);
                        objTestAnti.AntibioticID = Convert.ToInt16(dtAntibioticBreakpoint.Rows[j]["ANTIBIOTICID"].ToString());
                        objTestAnti.MethodID = Convert.ToInt16(dtAntibioticBreakpoint.Rows[j]["METHODID"].ToString());
                        objTestAnti.Result = dtAntibioticBreakpoint.Rows[j]["RESULT"].ToString();
                        objTestAnti.ResultValue = dtAntibioticBreakpoint.Rows[j]["RESULTVALUE"].ToString();
                        objTestAnti.CreateBy = ControlParameter.loginID;
                        objTestAnti.Comment = "";

                        if (dtAntibioticBreakpoint.Rows[j]["PRINTSTATUS"].ToString() == "")
                        {
                            objTestAnti.NOTPRINTABLE = "0";
                        }
                        else
                        {
                            objTestAnti.NOTPRINTABLE = dtAntibioticBreakpoint.Rows[j]["PRINTSTATUS"].ToString();
                        }

                        objTestAnti.Units = Convert.ToInt16(dtAntibioticBreakpoint.Rows[j]["UNITS"].ToString()); ;
                        objTestAnti.MethodID = Convert.ToInt16(dtAntibioticBreakpoint.Rows[j]["METHODID"].ToString());
                        objTestAnti.MicID = 0;//Convert.ToInt16(dtAntibioticBreakpoint.Rows[j]["MICID"].ToString());
                                              //objTestAnti.SUBREQMBANTIBIOTICID = Convert.ToInt16(dtAntibioticBreakpoint.Rows[j]["SUBREQMBANTIBIOTICID"].ToString());
                        objTestAnti = objTesting.SaveAntibiotics(objTestAnti);

                        objTestAnti = null;

                    }
                }



                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void addDetectionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearchList fm = new frmSearchList();
            //ConfigurationController objConfig = new ConfigurationController();

            BatteryM objBatteryM = new BatteryM();
            DataTable dt;
            try
            {

                objBatteryM.BATTERY_CODE = "";
                objBatteryM.BATTERY_SHORTNAME = "";

                dt = objTesting.GetDetectionTest();

                dt.Columns["DETECTIONCODE"].ColumnName = "code";
                dt.Columns["FULLTEXT"].ColumnName = "name";
                dt.Columns["DETECTIONID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.IsMultiSelect = true;

                fm.ShowDialog();

                if (fm.Selected == true)
                {

                    ArrayList objRows = fm.MultiSelectValue;

                    if (dtCultureData != null)
                    {
                        TreeNode SelectNode = tMain.SelectedNode;
                        TreeNode ParentNode = SelectNode.Parent;
                        DataRow dr;

                        string Action = fm.SelectedName.ToString();//objActionTypeM.ActionType_DetectionTest;
                        string Colony = SelectNode.Tag.ToString();
                        //string ActionDetail = fm.SelectedCode + ":" + fm.SelectedName;                        
                        string ActionDetail = "";
                        string SubAgarID = objTestResult.MBRequestID.ToString();
                        string Colonyid = SelectNode.Name.ToString();
                        Boolean bCheck;

                        dtCultureData = (DataTable)CultueDataBind.DataSource;

                        for (int i = 0; i <= objRows.Count - 1; i++)
                        {
                            DataRow row = objRows[i] as DataRow;
                            //ActionDetail = row["code"] + ":" + row["name"];
                            Action = row["name"].ToString();

                            bCheck = false;

                            dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + Colony + "' and DETECTIONID = '" + row["id"] + "'";
                            dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            if (dtCultureData.DefaultView.Count > 0)
                            {
                                bCheck = true;
                            }
                            else
                            {
                                bCheck = false;
                            }

                            if (bCheck == false)
                            {
                                dr = dtCultureData.NewRow();

                                dr["datetime"] = DateTime.Now.ToString();
                                dr["LOGUSERID"] = ControlParameter.loginName;
                                dr["COLONYNUMBER"] = Colony;
                                dr["SUBREQMBAGARID"] = SubAgarID;
                                dr["action"] = Action;
                                dr["actiondetail"] = ActionDetail;
                                dr["COLONYID"] = Colonyid;
                                dr["DETECTIONID"] = row["id"];
                                dr["actiontype"] = objActionTypeM.ActionType_DetectionTest;
                                dr["PRINTSTATUS"] = "0";

                                dtCultureData.Rows.Add(dr);

                                dr = null;

                            }

                        }

                        //dr = dtCultureData.NewRow();

                        //dr["datetime"] = DateTime.Now.ToString();
                        //dr["LOGUSERID"] = ControlParameter.loginName;
                        //dr["COLONYNUMBER"] = Colony;
                        //dr["SUBREQMBAGARID"] = SubAgarID;
                        //dr["action"] = Action;
                        //dr["actiondetail"] = ActionDetail;
                        //dr["COLONYID"] = Colonyid;
                        //dr["DETECTIONID"] = fm.SelectedID;
                        //dr["actiontype"] = objActionTypeM.ActionType_DetectionTest;

                        //dtCultureData.Rows.Add(dr);

                        //dr = null;

                        //CultueDataBind.DataSource = dtCultureData;

                    }


                    dtCultureData.DefaultView.RowFilter = string.Empty;

                    dtCultureData.DefaultView.RowFilter = "COLONYNUMBER =  '" + tMain.SelectedNode.Tag.ToString() + "'";
                    //dtCultureData.DefaultView.RowFilter = "COLONYID =  '" + tMain.SelectedNode.Name.ToString() + "'";
                    dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    CultueDataBind.DataSource = dtCultureData;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
            finally
            {
                fm = null;
                dt = null;
            }
        }

        private void addBiochemistryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearchList fm = new frmSearchList();
            //ConfigurationController objConfig = new ConfigurationController();

            BatteryM objBatteryM = new BatteryM();
            DataTable dt;
            try
            {
                objBatteryM.BATTERY_CODE = "";
                objBatteryM.BATTERY_SHORTNAME = "";

                dt = objTesting.GetDICTBiochemistry();

                dt.Columns["CHEMISTRYCODE"].ColumnName = "code";
                dt.Columns["FULLTEXT"].ColumnName = "name";
                dt.Columns["CHEMISTRYID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.IsMultiSelect = true;

                fm.RefreshData();

                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    ArrayList objRows = fm.MultiSelectValue;

                    if (dtCultureData != null)
                    {
                        TreeNode SelectNode = tMain.SelectedNode;
                        TreeNode ParentNode = SelectNode.Parent;
                        DataRow dr;

                        string Action = objActionTypeM.ActionType_Biochemistry;
                        string Colony = SelectNode.Tag.ToString();
                        //string ActionDetail = fm.SelectedCode + ":" + fm.SelectedName;
                        string SubAgarID = objTestResult.MBRequestID.ToString();
                        string Colonyid = SelectNode.Name.ToString();

                        Boolean bCheck;

                        dtCultureData = (DataTable)CultueDataBind.DataSource;

                        for (int i = 0; i <= objRows.Count - 1; i++)
                        {
                            DataRow row = objRows[i] as DataRow;
                            //ActionDetail = row["code"] + ":" + row["name"];
                            Action = row["name"].ToString();

                            string ActionDetail = "";

                            bCheck = false;

                            dtCultureData.DefaultView.RowFilter = " COLONYNUMBER = '" + Colony + "' and CHEMISTRYID = '" + row["id"] + "'";
                            dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            if (dtCultureData.DefaultView.Count > 0)
                            {
                                bCheck = true;
                            }
                            else
                            {
                                bCheck = false;
                            }

                            if (bCheck == false)
                            {
                                dr = dtCultureData.NewRow();

                                dr["datetime"] = DateTime.Now.ToString();
                                dr["LOGUSERID"] = ControlParameter.loginName;
                                dr["COLONYNUMBER"] = Colony;
                                dr["SUBREQMBAGARID"] = SubAgarID;
                                dr["action"] = Action;
                                dr["actiondetail"] = ActionDetail;
                                dr["ORGANISMID"] = "";
                                dr["COLONYID"] = Colonyid;
                                dr["BATTERYID"] = "";
                                dr["DETECTIONID"] = "";
                                dr["CHEMISTRYID"] = row["id"].ToString();
                                dr["actiontype"] = objActionTypeM.ActionType_Biochemistry;
                                dr["PRINTSTATUS"] = "0";

                                dtCultureData.Rows.Add(dr);

                                dr = null;

                                dr = null;

                            }

                        }


                        //dr = dtCultureData.NewRow();
                        //    dr["datetime"] = DateTime.Now.ToString();
                        //    dr["LOGUSERID"] = ControlParameter.loginName;
                        //    dr["COLONYNUMBER"] = Colony;
                        //    dr["SUBREQMBAGARID"] = SubAgarID;
                        //    dr["action"] = Action;
                        //    dr["actiondetail"] = ActionDetail;
                        //    dr["ORGANISMID"] = fm.SelectedID;
                        //    dr["COLONYID"] = Colonyid;
                        //    dr["BATTERYID"] = "";
                        //    dr["DETECTIONID"] = "";
                        //    dr["CHEMISTRYID"] = fm.SelectedID;
                        //    dr["actiontype"] = objActionTypeM.ActionType_Biochemistry;
                        //    dtCultureData.Rows.Add(dr);

                        //    dr = null;

                        dtCultureData.DefaultView.RowFilter = string.Empty;

                        dtCultureData.DefaultView.RowFilter = "COLONYNUMBER =  '" + tMain.SelectedNode.Tag.ToString() + "'";
                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        CultueDataBind.DataSource = dtCultureData;

                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
            finally
            {
                fm = null;
                dt = null;
            }
        }

        private void tMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tMain.SelectedNode = e.Node;
                Menumedia.Show(e.Location);
            }

        }

        private void gridView1_AfterPrintRow(object sender, DevExpress.XtraGrid.Views.Printing.PrintRowEventArgs e)
        {
            loadComboDetectionTest();
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            loadComboDetectionTest();
        }

        private void gridView5_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            string vMin;
            string vMax;
            string signMin;
            string signMax;
            string method;
            ArrayList rows = new ArrayList();
            string value;

        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void expertRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dtExpertDisplay != null)
            {
                if (dtExpertDisplay.Rows.Count > 0)
                {
                    frmParent.dtExpertRule = dtExpertDisplay;
                    frmParent.GetDisplayExpertRule();
                    //pExpertrule.Visible = true;
                }
            }
        }

        private void expertRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dtExpertDisplay != null)
            {
                if (dtExpertDisplay.Rows.Count > 0)
                {
                    frmParent.dtExpertRule = dtExpertDisplay;
                    frmParent.GetDisplayExpertRule();
                    //pExpertrule.Visible = true;
                }
            }
        }

        private void expertRuleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dtExpertDisplay != null)
            {
                if (dtExpertDisplay.Rows.Count > 0)
                {
                    frmParent.dtExpertRule = dtExpertDisplay;
                    frmParent.GetDisplayExpertRule();
                    //pExpertrule.Visible = true;
                }
            }
        }

        private void CalculateExpertRule()
        {
            try
            {
                if (dtAntibioticBreakpoint != null)
                {
                    if (dtAntibioticBreakpoint.Rows.Count > 0)
                    {

                        dtExpertDisplay = new DataTable();

                        dtExpertDisplay.Columns.Add("no");
                        dtExpertDisplay.Columns.Add("displaytext");

                        for (int i = 0; i <= dtAntibioticBreakpoint.Rows.Count - 1; i++)
                        {
                            int AntiID = Convert.ToInt32(dtAntibioticBreakpoint.Rows[i]["ANTIBIOTICID"].ToString());
                            String Result = dtAntibioticBreakpoint.Rows[i]["RESULT"].ToString();

                            dtCultureData.DefaultView.RowFilter = " COLONYNUMBER =  '" + dtAntibioticBreakpoint.Rows[i]["COLONYNUMBER"].ToString() + "' and action = '" + objActionTypeM.ActionType_Identification + "'";
                            dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            if (dtCultureData.DefaultView.Count > 0)
                            {
                                ExpertRules(dtCultureData.DefaultView[0]["ORGANISMID"].ToString(), AntiID.ToString(), Result);
                            }
                            else 
                            {
                                if (dtExpertDisplay.Rows.Count != 0)
                                {
                                    frmParent.dtExpertRule = dtExpertDisplay;
                                    frmParent.GetDisplayExpertRule();
                                }
                            }


                        }

                        dtCultureData.DefaultView.RowFilter = String.Empty;
                        dtCultureData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void gridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "PRINTSTATUS")
            {                
                if (!gridView1.IsDataRow(e.RowHandle)) return;
                GridCellInfo cell = e.Cell as GridCellInfo;                
                TextEditViewInfo viewInfo = cell.ViewInfo as TextEditViewInfo;                

                if (viewInfo != null)
                {
                    if (!String.IsNullOrEmpty(e.CellValue.ToString()))
                    {
                        if (e.CellValue.ToString() == "0")
                        {                            
                            e.Cache.DrawImage(DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_16x16.png"), e.Bounds.Location);
                        }
                        else
                        {
                            e.Cache.DrawImage(DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/cancel_16x16.png"), e.Bounds.Location);
                        }
                    }
                    else
                    {
                        e.Cache.DrawImage(DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/cancel_16x16.png"), e.Bounds.Location);
                    }
                   //viewInfo.CalcViewInfo(e.Graphics);

                }
            }
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            try
            {
                int[] selActionRows = ((GridView)grdDisplay.MainView).GetSelectedRows();
                DataRowView selActionRow = (DataRowView)(((GridView)grdDisplay.MainView).GetRow(selActionRows[0]));

                if (selActionRow["PRINTSTATUS"].ToString() == "0")
                {
                    toBePrintedToolStripMenuItem.Checked = true;
                }
                else
                {
                    toBePrintedToolStripMenuItem.Checked = false;
                }


                if (selActionRow["action"].ToString() == objActionTypeM.ActionType_Sensitivities)
                {
                    dtAntibioticBreakpoint.DefaultView.RowFilter = " colonynumber = '" + selActionRow["colonynumber"].ToString() + "' and  SENSITIVITYID = " + selActionRow["SENSITIVITYID"].ToString();
                    dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void toBePrintedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selRows = ((GridView)grdDisplay.MainView).GetSelectedRows();

            if (MessageBox.Show("Can change print status?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) { 
            }
            for (int i = 0; i <= selRows.Length - 1; i++)
            {
                DataRowView selRow = (DataRowView)(((GridView)grdDisplay.MainView).GetRow(selRows[0]));

                if (selRow["PRINTSTATUS"].ToString() == "0")
                {
                    selRow["PRINTSTATUS"]= "1";
                } else 
                {
                    selRow["PRINTSTATUS"] = "0";
                }                
            }
        }

        private void gridView5_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "PRINTSTATUS")
            {
                if (!gridView5.IsDataRow(e.RowHandle)) return;
                GridCellInfo cell = e.Cell as GridCellInfo;
                //GridCellInfo cell = e.Cell as GridCellInfo;                
                TextEditViewInfo viewInfo = cell.ViewInfo as TextEditViewInfo;
                //RepositoryItemTextEdit te = (RepositoryItemTextEdit) gridView1.ViewRepository;

                if (viewInfo != null)
                {
                    if (!String.IsNullOrEmpty(e.CellValue.ToString()))
                    {
                        if (e.CellValue.ToString() == "0")
                        {
                            e.Cache.DrawImage(DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_16x16.png"), e.Bounds.Location);
                        }
                        else
                        {
                            e.Cache.DrawImage(DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/cancel_16x16.png"), e.Bounds.Location);
                        }
                    }
                    else
                    {
                        e.Cache.DrawImage(DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/cancel_16x16.png"), e.Bounds.Location);
                    }
                    //viewInfo.CalcViewInfo(e.Graphics);

                }
            }
        }

        private void grdAnti_Click(object sender, EventArgs e)
        {

        }

        private void gridView5_RowClick(object sender, RowClickEventArgs e)
        {
            int[] selActionRows = ((GridView)grdAnti.MainView).GetSelectedRows();
            DataRowView selActionRow = (DataRowView)(((GridView)grdAnti.MainView).GetRow(selActionRows[0]));

            if (selActionRow["PRINTSTATUS"].ToString() == "0")
            {
                toBePrintedToolStripMenuItem.Checked = true;
            }
            else
            {
                toBePrintedToolStripMenuItem.Checked = false;
            }

        }

        private void TobePrintedAnti_Click(object sender, EventArgs e)
        {
            int[] selRows = ((GridView)grdAnti.MainView).GetSelectedRows();

            if (MessageBox.Show("Can change print status?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
            }
            for (int i = 0; i <= selRows.Length - 1; i++)
            {
                DataRowView selRow = (DataRowView)(((GridView)grdAnti.MainView).GetRow(selRows[0]));

                if (selRow["PRINTSTATUS"].ToString() == "0")
                {
                    selRow["PRINTSTATUS"] = "1";
                }
                else
                {
                    selRow["PRINTSTATUS"] = "0";
                }
            }

        }

        private void gridView5_CellValueChanging_1(object sender, CellValueChangedEventArgs e)
        {
            //int[] selActionRows = ((GridView)grdDisplay.MainView).GetSelectedRows();

            oldrows = new ArrayList();

            //String a = "aaa";x    

            for (int i = 0; i < gridView5.SelectedRowsCount; i++)
            {
                if (gridView5.GetSelectedRows()[i] >= 0)
                    oldrows.Add(gridView5.GetDataRow(gridView5.GetSelectedRows()[i]));
            }
                        
            String a = "111";
            //dtAntibioticBreakpoint.DefaultView.RowFilter = " ANTIBIOTICID = " + AntiID + " and SENSITIVITYID = " + SenID + " and COLONYNUMBER = '" + ColonyNo.Trim() + "'";
            //dtAntibioticBreakpoint.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;


            //TestOldValue = 
        }

        private void MenuSensitivity_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
