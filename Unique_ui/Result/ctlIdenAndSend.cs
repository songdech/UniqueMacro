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
using UniquePro.Entities.Testing;
using UniquePro.Controller;
using DevExpress.XtraGrid.Views.Grid;

namespace UNIQUE.Result
{
    public partial class ctlIdenAndSend : DevExpress.XtraEditors.XtraUserControl
    {
        public DataTable TestResultData { get; set; }

        //public OrderEntryM objRequest { get; set; }
        public TestResultM objTestResult { get; set; }

        public TestColonyResultM TestColonyResultSelect;

        public TestAgarsResultM TestAgarResultSelect;

        public DataTable MediaTestingResult { get; set; }

        public DataTable DICTOrganism { get; set; }

        public TestController objTesting;


        public ctlIdenAndSend()
        {
            InitializeComponent();
        }

        public void BeginingData()
        {
            ConfigurationController objConfiguration = new ConfigurationController();

            try
            {
                objTesting = new TestController();

                DICTOrganism = objConfiguration.GetDICTOrganisms();

                MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult ,0,0);

                objTestResult = GetDatatableToObjectTestResult(MediaTestingResult, objTestResult);

                objTestResult = objTesting.SaveMediaByIden(objTestResult);

                //MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult ,0,0);

                grdMedia.DataSource = MediaTestingResult;
                grdMedia.RefreshDataSource();

                grdViewMedia.OptionsBehavior.Editable = false;
                grdViewMedia.OptionsSelection.EnableAppearanceFocusedCell = false;
                grdViewMedia.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;

                pRight.Visible = false;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
           
        }

        private TestResultM GetDatatableToObjectTestResult(DataTable dt, TestResultM objResult)
        {
            TestAgarsResultM objAgar = null;
            TestColonyResultM objColony = null;

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
                        objAgar.NOTPRINTABLE= dt.DefaultView[i]["NOTPRINTABLE"].ToString();

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
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchList fm = new frmSearchList();
            TestAgarsResultM objAgarResult;
            DataTable dt;
            DataRow dr = null;
            Boolean bCheck = true;
            try
            {

                dt = objConfiguration.GetMedia("", "");

                dt.Columns["AGARCODE"].ColumnName = "code";
                dt.Columns["AGARNAME"].ColumnName = "name";
                dt.Columns["AGARID"].ColumnName = "id";

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

                    MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult , 0,0);

                    //MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult);
                    //objTestResult = GetDatatableToObjectTestResult(MediaTestingResult, objTestResult);


                    grdMedia.DataSource = MediaTestingResult;
                    grdMedia.RefreshDataSource();

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



        private void addObservationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearchList fm = new frmSearchList();

            try
            {
                DICTOrganism.Columns["ORGANISMCODE"].ColumnName = "code";
                DICTOrganism.Columns["ORGANISMNAME"].ColumnName = "name";
                DICTOrganism.Columns["ORGANISMID"].ColumnName = "id";

                fm.SearchData = DICTOrganism;
                fm.RefreshData();

                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    TestColonyResultM objColony = TestColonyResultSelect;

                    objColony.OrganismID = fm.SelectedID;
                    objColony.OrganismIndex = 1;
                    objColony.OrganismNumber = fm.SelectedCode;

                    objTesting.SaveColony(objColony, objTestResult, TestAgarResultSelect);

                    MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult , objColony.AgarID, objColony.ColonyID );

                    objTestResult = GetDatatableToObjectTestResult(MediaTestingResult, objTestResult);

                    grdMedia.DataSource = MediaTestingResult;
                    grdMedia.RefreshDataSource();

                    //objRequest.SUBREQMBAGARID = Convert.ToInt16(contextMenuStrip1.Items[1].Tag);
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

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pRight.Visible = true;
            pRight.Left = pLeft.Width + 10;

            //pMain.Width = pMain.Width - pRight.Width;
            //pRight.Left = pMain.Width + 10;
        }

        private void grdOrganism_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                if (TestResultData.Rows.Count > 0)
                {
                    contextMenuStrip1.Items[0].Visible = false;
                    contextMenuStrip1.Items[1].Visible = true;
                    contextMenuStrip1.Items[2].Visible = true;
                }
                else
                {
                    contextMenuStrip1.Items[0].Visible = false;
                    contextMenuStrip1.Items[1].Visible = false;
                    contextMenuStrip1.Items[2].Visible = false;
                    //contextMenuStrip1.Items[3].Visible = false;
                }


                //contextMenuStrip1.Show(treeAga, e.Location);
                //contextMenuStrip1.Show(Cursor.Position);

            }
        }

        private void gridControl2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (TestResultData.Rows.Count > 0)
                {
                    contextMenuStrip1.Items[0].Visible = false;
                    contextMenuStrip1.Items[1].Visible = true;
                    contextMenuStrip1.Items[2].Visible = true;
                }
                else
                {
                    contextMenuStrip1.Items[0].Visible = false;
                    contextMenuStrip1.Items[1].Visible = false;
                    contextMenuStrip1.Items[2].Visible = false;
                    //contextMenuStrip1.Items[3].Visible = false;
                }


                //contextMenuStrip1.Show(treeAga, e.Location);
                //contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void grdMedia_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Items[0].Visible = true;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;

                contextMenuStrip1.Show(grdMedia, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void IdenandSen_Load(object sender, EventArgs e)
        {
            try
            {
                BeginingData();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void grdViewMedia_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {


                if (e.Button == MouseButtons.Right)
                {
                    if (grdViewMedia.GetFocusedRowCellValue("SUBREQMBAGARID") != null)
                    {
                        TestAgarsResultM objAgar;
                        TestColonyResultM objColony;

                        int iSubReqAkaID = Convert.ToInt16(grdViewMedia.GetFocusedRowCellValue("SUBREQMBAGARID").ToString().Trim());
                        int iColonyID = Convert.ToInt16(grdViewMedia.GetFocusedRowCellValue("COLONYID").ToString().Trim());

                        for (int i = 0; i <= objTestResult.TestResultAkars.Count - 1; i++)
                        {
                            objAgar = objTestResult.TestResultAkars[i];

                            if (objAgar.SubReqMBAgarID == iSubReqAkaID)
                            {

                                TestAgarResultSelect = objAgar;

                                for (int j = 0; j <= objAgar.Colonies.Count - 1; j++)
                                {
                                    objColony = objAgar.Colonies[j];

                                    if (objColony.ColonyID == iColonyID)
                                    {
                                        TestColonyResultSelect = objColony;
                                        break;
                                    }
                                    else
                                    {
                                        TestColonyResultSelect = null;
                                    }
                                }
                            }

                        }

                        contextMenuStrip1.Items[1].Tag = iSubReqAkaID;
                        contextMenuStrip1.Items[0].Visible = false;
                        contextMenuStrip1.Items[1].Visible = true;
                        contextMenuStrip1.Items[2].Visible = false;

                    }
                    else
                    {
                        if (grdViewMedia.GetFocusedRowCellValue("ORGANISMCODE") != null)
                        {
                        }
                        else
                        {
                            contextMenuStrip1.Items[0].Visible = false;
                            contextMenuStrip1.Items[1].Visible = false;
                            contextMenuStrip1.Items[2].Visible = true;
                        }
                    }

                    contextMenuStrip1.Show(grdMedia, e.Location);
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pRight.Visible = true;
        }

        private void grdViewMedia_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            int dataSourceRowIndex = view.GetDataSourceRowIndex(e.RowHandle);
            int visibleIndex = view.GetVisibleIndex(e.RowHandle);
            if (e.Column.FieldName == "gridColumnRowHandle")
                e.DisplayText = e.RowHandle.ToString();
            if (e.Column.FieldName == "gridColumnVisibleIndex")
                e.DisplayText = visibleIndex.ToString();
            if (e.Column.FieldName == "gridColumnListSourceIndex")
                e.DisplayText = dataSourceRowIndex.ToString();
        }

        private void grdViewMedia_RowClick_1(object sender, RowClickEventArgs e)
        {
            try
            {


                if (e.Button == MouseButtons.Right)
                {
                    if (grdViewMedia.GetFocusedRowCellValue("SUBREQMBAGARID") != null)
                    {
                        TestAgarsResultM objAgar;
                        TestColonyResultM objColony;

                        int iSubReqAkaID = Convert.ToInt16(grdViewMedia.GetFocusedRowCellValue("SUBREQMBAGARID").ToString().Trim());
                        int iColonyID = Convert.ToInt16(grdViewMedia.GetFocusedRowCellValue("COLONYID").ToString().Trim());

                        for (int i = 0; i <= objTestResult.TestResultAkars.Count - 1; i++)
                        {
                            objAgar = objTestResult.TestResultAkars[i];

                            if (objAgar.SubReqMBAgarID == iSubReqAkaID)
                            {

                                TestAgarResultSelect = objAgar;

                                for (int j = 0; j <= objAgar.Colonies.Count - 1; j++)
                                {
                                    objColony = objAgar.Colonies[j];

                                    if (objColony.ColonyID == iColonyID)
                                    {
                                        TestColonyResultSelect = objColony;
                                        break;
                                    }
                                    else
                                    {
                                        TestColonyResultSelect = null;
                                    }
                                }
                            }

                        }

                        contextMenuStrip1.Items[1].Tag = iSubReqAkaID;
                        contextMenuStrip1.Items[0].Visible = false;
                        contextMenuStrip1.Items[1].Visible = true;
                        contextMenuStrip1.Items[2].Visible = false;

                    }
                    else
                    {
                        if (grdViewMedia.GetFocusedRowCellValue("ORGANISMCODE") != null)
                        {
                        }
                        else
                        {
                            contextMenuStrip1.Items[0].Visible = false;
                            contextMenuStrip1.Items[1].Visible = false;
                            contextMenuStrip1.Items[2].Visible = true;
                        }
                    }

                    contextMenuStrip1.Show(grdMedia, e.Location);
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
        }

        private void grdMedia_Click(object sender, EventArgs e)
        {

        }
    }
}
