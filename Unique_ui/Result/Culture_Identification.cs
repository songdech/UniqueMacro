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
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using System.Collections;

using UniquePro.Controller;

namespace UNIQUE.Result
{
    public partial class Culture_Identification : DevExpress.XtraEditors.XtraUserControl
    {
        SqlConnection conn;
        List<string> sList = new List<string>();

        public DataTable OraganismData { get; set; }

        private OrganismController objOraganismController = new OrganismController();

        RepositoryItemComboBox riCombo;       
        public Culture_Identification()
        {
            InitializeComponent();
        }

        private void Culture_Identification_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dtOrganism = objOraganismController.GetDictOranismData();
                DataSet ds = new DataSet();

                conn = new SqlConnection();
                conn.ConnectionString = "Data Source=uniquedb.ckgq5ecwqhep.ap-southeast-1.rds.amazonaws.com,1433;Initial Catalog=UNIQUE_DB;User ID=admin;Password=admin1234";
                conn.Open();

                dICT_MB_ORGANISMSTableAdapter.Connection = conn;
                //uNIQUEDataSet2 = ds;
                dICT_MB_ORGANISMSTableAdapter.Fill(uNIQUEDataSet2.DICT_MB_ORGANISMS);

                //this.dICT_MB_ORGANISMSTableAdapter.Fill(this.uNIQUEDataSet2.DICT_MB_ORGANISMS);

                //DataTable dt2 = new DataTable();
                //dt2.Columns.Add("TEST", Type.GetType("System.String"));
                //DataRow dr = dt2.NewRow();
                //dt2.Rows.Add(dr);
                //gridControl1.DataSource = dt2;

                //DataTable dt = new DataTable();
                //dt.Columns.Add("ORGANISM", Type.GetType("System.String"));
                ////dt.Columns.Add("");
                //DataRow dr = dt.NewRow();
                //dt.Rows.Add(dr);
                //gridControl2.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
       }      

        private void loadData()
        {

            gridView2.AddNewRow();

            int rowHandle = gridView2.GetRowHandle(gridView2.DataRowCount);
            if (gridView2.IsNewItemRow(rowHandle))
            {
                gridView2.SetRowCellValue(rowHandle, gridView2.Columns[0], "a");
               // gridView2.SetRowCellValue(rowHandle, gridView2.Columns[1], "b");
              //  gridView2.SetRowCellValue(rowHandle, gridView2.Columns[2], "c");
            }
            gridControl2.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  loadData();

          
         
      
            //      riCombo.Items.Add(ds.Tables["text"].Rows);

            //Add the item to the internal repository
        
            object obj;
            List<string> sl = new List<string>();
            for (int i = 0; i < gridView1.RowCount-1 ; i++)
            {

               



                DataRow row = gridView1.GetDataRow(i);
                string name = row["TEST"].ToString();

                if (name.Contains("Organism A"))
                {
                    riCombo = new RepositoryItemComboBox();
                    riCombo.Items.AddRange(new string[] { "San Francisco", "Monterey", "Toronto", "Boston", "Kuaui", "Singapore", "Tokyo" });

                    gridControl1.RepositoryItems.Add(riCombo);

                    //Now you can define the repository item as an in-place column editor
                    gridColumn3.ColumnEdit = riCombo;
                }
                else
                {
                    riCombo = new RepositoryItemComboBox();
                    riCombo.Items.AddRange(new string[] { "a", "b", "c", "d", "e", "f", "g" });

                    gridControl1.RepositoryItems.Add(riCombo);

                    //Now you can define the repository item as an in-place column editor
                    gridColumn3.ColumnEdit = riCombo;
                }

                //switch (name.ToString())
                //{
                //    case "Organism A":

                  
                //                break;
                //}
            }
        
        }

        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            //DataRow row = gridView1.GetDataRow(e.RowHandle);
            //row[0] = "ABC";
       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TEST",Type.GetType("System.String"));
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            gridControl1.DataSource = dt;
        }

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {

            try
            {
                System.Data.DataRow row5 = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);


                gridControl1.RefreshDataSource();
                gridControl1.RefreshDataSource();

                gridView2.AddNewRow();



                string text = row5["TEST"].ToString();
                // string text = gridView1.GetFocusedDisplayText();

                List<string> abc = new List<string>();
                for (int i = 0; i < gridView1.RowCount - 2; i++)
                {
                    DataRow row = gridView1.GetDataRow(i);
                    abc.Add(row["TEST"].ToString());
                }
                foreach (string obj in abc)
                {
                    if (obj.ToString() == text)
                    {
                        gridView1.DeleteRow(gridView1.FocusedRowHandle);
                        gridView2.DeleteRow(gridView2.FocusedRowHandle);
                        break;
                    }
                    else
                    {

                        //   break;
                    }
                }

                int rowHandle2 = gridView2.GetRowHandle(gridView2.DataRowCount);

                if (gridView2.IsNewItemRow(rowHandle2))
                {
                    DataRowView selRow = (DataRowView)(((GridView)gridControl1.MainView).GetRow(rowHandle2));
                    //
                    System.Data.DataRow row2 = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    //    string rootStain = selRow["TEST"].ToString();
                    gridView2.SetRowCellValue(rowHandle2, gridView2.Columns[0], gridView1.GetFocusedDisplayText());

               //     MessageBox.Show(gridView1.GetFocusedDisplayText());
                    sList.Add(row2["TEST"].ToString());





                }
                gridControl2.EndUpdate();
                gridControl2.RefreshDataSource();
                gridControl2.RefreshDataSource();

            }
            catch (Exception) { }
            // }
            //  }

            //   MessageBox.Show("ex");
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Data.DataRow row2 = gridView1.GetDataRow(gridView1.FocusedRowHandle);
        
            int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
            string text = row2["TEST"].ToString();

                for (int i = 1; i < gridView1.RowCount -2; i++)
                {
                    DataRow row = gridView1.GetDataRow(i);
                    string name = row["TEST"].ToString();
                    if (name == text)
                    {
                        MessageBox.Show("xxx");
                    }
                }
           // }
   
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
          //  MessageBox.Show("click");
            pRight.Visible = true;

            ControlParameter.oganismnIDforAntimicrobial = "0";
            try
            {
                System.Data.DataRow row2 = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
                string orgID = row2["TEST"].ToString();



                ControlParameter.oganismnIDforAntimicrobial = orgID;
                //      MessageBox.Show("xxx :" + text);
                /*
                 *  Antimicrobial Breakpoint.
                 */


                string sql = @"
select dict_mb_organisms.*, DICT_MB_ANTIBIOTICS.ANTIBIOTICCODE,DICT_MB_ANTIBIOTICS.FULLTEXT,SUBREQMB_ANTIBIOTICS.RESULT,SUBREQMB_ANTIBIOTICS.RESULTVALUE
,DICT_ANTIMICROBIAL_BREAKPOINT.MICCODE,DICT_ANTIMICROBIAL_BREAKPOINT.BREAKPOINTNAME,DICT_MB_METHODS.METHODMBCODE,DICT_MB_METHODS.UNITS,DICT_ANTIMICROBIAL_BREAKPOINT.THRESHOLDLOWER,DICT_ANTIMICROBIAL_BREAKPOINT.THRESHOLDHIGHER,DICT_ANTIMICROBIAL_BREAKPOINT_LISTS.CONCLOWVALUE,DICT_ANTIMICROBIAL_BREAKPOINT_LISTS.CONCHIGHVALUE, 
	CASE DICT_ANTIMICROBIAL_BREAKPOINT.THRESHOLDLOWER
		WHEN '0' THEN '<'
		WHEN '1' THEN '<='
		ELSE '' 
		END AS 'THRESHOLDLOWER',
	CASE DICT_ANTIMICROBIAL_BREAKPOINT.THRESHOLDHIGHER
	WHEN '3' THEN '>'
	WHEN '2' THEN '>='
	ELSE '' + ' '
	END AS 'THRESHOLDHIGHER', CONCLOWVALUE + ' - ' +CONCHIGHVALUE AS 'BREAKPOINT' from dict_mb_organisms
left outer join dict_antimicrobial_breakpoint 
	on ( dict_mb_organisms.micid = dict_antimicrobial_breakpoint.micid)
left outer join DICT_ANTIMICROBIAL_BREAKPOINT_LISTS 
	on DICT_ANTIMICROBIAL_BREAKPOINT.micid = DICT_ANTIMICROBIAL_BREAKPOINT_LISTS.micid
left outer join DICT_MB_ANTIBIOTICS 
	on DICT_ANTIMICROBIAL_BREAKPOINT_LISTS.ANTIBIOTICID = DICT_MB_ANTIBIOTICS.ANTIBIOTICID
left outer join SUBREQMB_ANTIBIOTICS
	 ON DICT_MB_ANTIBIOTICS.ANTIBIOTICID = SUBREQMB_ANTIBIOTICS.ANTIBIOTICID
left outer join DICT_MB_METHODS  ON dict_mb_organisms.METHODMBID = DICT_MB_METHODS.METHODMBID

    where dict_mb_organisms.ORGANISMID = '" + orgID + "' ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //   dataSet_Antimicrobial_Breakpoint = new DataSet_Antimicrobial_Breakpoint();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds.Clear();
                adap.Fill(ds, "atb");
                cmd.ExecuteNonQuery();
                if (ds.Tables["atb"].Rows.Count > 0)
                {
                    bindingSource_ATB_Breakpoint.DataSource = ds;
                    bindingSource_ATB_Breakpoint.DataMember = "atb";

                    lblOrganism_For_ATB.DataBindings.Clear();
                    lblOrganism_For_ATB.DataBindings.Add("TEXT", ds.Tables["atb"], "ORGANISMNAME");
                }
            }catch(Exception){}
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
            
                gridView2.DeleteRow(gridView2.FocusedRowHandle);

            }
            catch (Exception)
            {
              //  int rowHandle = gridView2.FocusedRowHandle;

              //  int[] selRows = ((GridView)gridControl2.MainView).GetSelectedRows();
              //  DataRowView selRow = (DataRowView)(((GridView)gridControl2.MainView).GetRow(rowHandle));
              //  string rootStain = selRow["STAINNAME"].ToString();
              //  string stain = selRow["SUBREQMBSTAINID"].ToString();
              //  //   MessageBox.Show("" + rootStain.ToString() + " stain : "+ stain);
              //  StainObservationsForDeleteRequestStain obsDel = new StainObservationsForDeleteRequestStain();
              //  obsDel.MbRequestID = int.Parse(stain.ToString());
              ////  sListForDelRequestMBStain.Add(obsDel);
              //  gridView2.DeleteRow(gridView2.FocusedRowHandle);
            }
    
         
        }

        private void gridView2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                //object abc = gridView2.GetRowCellValue(rowHandle, "FULLTEXT");
                //MessageBox.Show(""+abc);

                contextMenuStrip1.Show(this.gridControl2, e.Location);
                contextMenuStrip1.Show(Cursor.Position);


            }
        }

        private void gridView3_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            


        }

        private void gridView3_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
         
        }

        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //  MessageBox.Show("xxx");

            string vMin;
            string vMax;
            string signMin;
            string signMax;
            string method;

            //// Create an empty list.
            ArrayList rows = new ArrayList();
            string value;
            // Add the selected rows to the list.
            for (int i = 0; i < gridView3.SelectedRowsCount; i++)
            {
                if (gridView3.GetSelectedRows()[i] >= 0)
                    rows.Add(gridView3.GetDataRow(gridView3.GetSelectedRows()[i]));
            
            }
            try
            {
                gridView3.BeginUpdate();
                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i] as DataRow;
                    try
                    {
                        vMin = row["CONCLOWVALUE"].ToString();
                        vMax = row["CONCHIGHVALUE"].ToString();
                        value = row["RESULTVALUE"].ToString();
                        signMin = row["THRESHOLDLOWER"].ToString();
                        signMax = row["THRESHOLDHIGHER"].ToString();
                        method = row["METHODMBCODE"].ToString();

                        if (method == "MIC")
                        {
                            if (signMin == "0" && signMax == "3")
                            {
                                if (Convert.ToInt32(value.ToString()) < Convert.ToInt32(vMin.ToString()))
                                {
                                    row["RESULT"] = "S";
                                }
                                else if (Convert.ToInt32(value.ToString()) > Convert.ToInt32(vMax.ToString()))
                                {
                                    row["RESULT"] = "R";
                                }
                                else if (Convert.ToInt32(value.ToString()) > Convert.ToInt32(vMin.ToString()) && Convert.ToInt32(value.ToString()) < Convert.ToInt32(vMax.ToString()))
                                {
                                    row["RESULT"] = "I";
                                }
                                else
                                {
                                    row["RESULT"] = "";
                                }
                            }
                            else if (signMin == "1" && signMax == "2")
                            {
                                if (Convert.ToInt32(value.ToString()) <= Convert.ToInt32(vMin.ToString()))
                                {
                                    row["RESULT"] = "S";
                                }
                                else if (Convert.ToInt32(value.ToString()) >= Convert.ToInt32(vMax.ToString()))
                                {
                                    row["RESULT"] = "R";
                                }
                                else if (Convert.ToInt32(value.ToString()) >= Convert.ToInt32(vMin.ToString()) && Convert.ToInt32(value.ToString()) <= Convert.ToInt32(vMax.ToString()))
                                {
                                    row["RESULT"] = "I";
                                }
                                else
                                {
                                    row["RESULT"] = "";
                                }
                            }
                        }

                        if (method == "MID")
                        {
                            if (signMin == "0" && signMax == "3")
                            {
                                if (Convert.ToInt32(value.ToString()) < Convert.ToInt32(vMin.ToString()))
                                {
                                    row["RESULT"] = "R";
                                }
                                else if (Convert.ToInt32(value.ToString()) > Convert.ToInt32(vMax.ToString()))
                                {
                                    row["RESULT"] = "S";
                                }
                                else if (Convert.ToInt32(value.ToString()) > Convert.ToInt32(vMin.ToString()) && Convert.ToInt32(value.ToString()) < Convert.ToInt32(vMax.ToString()))
                                {
                                    row["RESULT"] = "I";
                                }
                                else
                                {
                                    row["RESULT"] = "";
                                }
                            }
                            else if (signMin == "1" && signMax == "2")
                            {
                                if (Convert.ToInt32(value.ToString()) <= Convert.ToInt32(vMin.ToString()))
                                {
                                    row["RESULT"] = "R";
                                }
                                else if (Convert.ToInt32(value.ToString()) >= Convert.ToInt32(vMax.ToString()))
                                {
                                    row["RESULT"] = "S";
                                }
                                else if (Convert.ToInt32(value.ToString()) >= Convert.ToInt32(vMin.ToString()) && Convert.ToInt32(value.ToString()) <= Convert.ToInt32(vMax.ToString()))
                                {
                                    row["RESULT"] = "I";
                                }
                                else
                                {
                                    row["RESULT"] = "";
                                }
                            }
                        }

                        // Change the field value.

                        // row["RESULTVALUE"] = "X";

                    }catch(Exception){}

                }
            }
            finally
            {
                gridView3.EndUpdate();
            }      
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
           
        }

        private void gridView3_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void gridView3_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "UNITS")
                if (Convert.ToString(e.Value) == "0") e.DisplayText = "mg/L";
                else e.DisplayText = "mm";
        }
    }
}
