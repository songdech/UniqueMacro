using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using System.Collections;

using UniquePro.Controller;
using UniquePro.Entities.Testing;

namespace UNIQUE.Result
{
    public partial class CultureIdentification : DevExpress.XtraEditors.XtraUserControl
    {
        public DataTable TestResultData { get; set; }

        public TestResultM objTestResult { get; set; }

        public TestColonyResultM TestColonyResultSelect;

        public TestAgarsResultM TestAgarResultSelect;

        public DataTable MediaTestingResult { get; set; }

        public DataTable DICTOrganism { get; set; }

        public TestController objTesting;

        private void CultureIdentification_Load(object sender, EventArgs e)
        {
            BeginingData();
        }

        public void BeginingData()
        {
            ConfigurationController objConfiguration = new ConfigurationController();

            try
            {
                objTesting = new TestController();

                //DICTOrganism = objConfiguration.GetDICTOrganisms();

                //MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult);
                MediaTestingResult = objTesting.GetAgarHeader(objTestResult);

                //objTestResult = GetDatatableToObjectTestResult(MediaTestingResult, objTestResult);

                //objTestResult = objTesting.SaveMediaByIden(objTestResult);

                //MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult);

                //grdMedia.DataSource = MediaTestingResult;
                //grdMedia.RefreshDataSource();

                //grdViewMedia.OptionsBehavior.Editable = false;
                //grdViewMedia.OptionsSelection.EnableAppearanceFocusedCell = false;
                //grdViewMedia.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;

                CreateNode(MediaTestingResult);

                //pRight.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void CreateNode(DataTable dt)
        {
            TreeView Node;
            DataTable MediaHeader;
            DataTable dtColony;
            try
            {
                MediaHeader = objTesting.GetAgarHeader(objTestResult);

                dtColony = objTesting.GetMediaTestingWithRequestnumber(objTestResult ,0,0);

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    tMain.Nodes.Add(dt.Rows[i]["AGARID"].ToString(), dt.Rows[i]["AGARNAME"].ToString(), 0);

                    dtColony.DefaultView.RowFilter = "AGARID = " + dt.Rows[i]["AGARID"].ToString();
                    dtColony.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    if (dtColony.DefaultView.Count > 0)
                    {
                        TreeNode nChild = tMain.Nodes[dt.Rows[i]["AGARID"].ToString()];

                        for (int j = 0; j <= dtColony.DefaultView.Count - 1; j++)
                        {
                            nChild.Nodes.Add(dtColony.DefaultView[j]["COLONYID"].ToString(), dtColony.DefaultView[j]["COLONYNUMBER"].ToString(), 5);

                            // Organism O
                            dtColony.DefaultView.RowFilter = "OGANISMID = " + dtColony.DefaultView[j]["OGANISMID"].ToString();
                            dtColony.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                            if (dtColony.DefaultView.Count > 0)
                            {
                                TreeNode nColony = tMain.Nodes[dt.Rows[i]["COLONYID"].ToString()];

                                nColony.Text = "AAA";


                            }
                        }


                    }

                    //MediaTestingResult.DefaultView.RowFilter = " "
                }

                //tMain.Nodes.Add("aa").Nodes.Add ("select");
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

                    //objTestResult = objTesting.SaveMediaByIden(objTestResult);

                    MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult , 0,0);

                    dsTree.DataSource = MediaTestingResult;

                    //MediaTestingResult = objTesting.GetMediaTestingWithRequestnumber(objTestResult);
                    //objTestResult = GetDatatableToObjectTestResult(MediaTestingResult, objTestResult);


                    //grdMedia.DataSource = MediaTestingResult;
                    //grdMedia.RefreshDataSource();

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

        private void tMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Items[0].Visible = true;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;

                contextMenuStrip1.Show(tMain, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

    }
}
