using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniquePro;
using UniquePro.Entities;

namespace UNIQUE.Result
{
    public partial class frmSearchList : Form
    {
        public DataTable  SearchData { get; set; }
        public String  SelectedCode { get; set; }
        public String SelectedName { get; set; }
        public int  SelectedID { get; set; }
        public int MethodID { get; set; }

        public Boolean Selected { get; set; }
        public Boolean IsSensitivities { get; set; }
        public Boolean IsMultiSelect { get; set; }

        public ArrayList MultiSelectValue { get; set; }

        public frmSearchList()
        {
            InitializeComponent();
        }

        private void frmSearchList_Load(object sender, EventArgs e)
        {
            SelectedCode = "";
            SelectedName = "";
            Selected = false;

            if (IsSensitivities == true)
            {
                lblUnit.Visible = true;
                optMic.Visible = true;
                optMid.Visible = true;
            }
            else
            {
                lblUnit.Visible = false;
                optMic.Visible = false;
                optMid.Visible = false;
            }

            gridView1.OptionsSelection.MultiSelect = IsMultiSelect;
            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;

            MultiSelectValue = new ArrayList();

        }

        private void EventTextCodeClick()
        {
            try
            {
                String Code = txtCode.Text;

                SearchData.DefaultView.RowFilter = " code like '" + Code + "%'";
                SearchData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                //txtName.Text = SearchData.DefaultView[0]["name"].ToString();

                grdView.DataSource = SearchData.DefaultView;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void EventTextNameClick()
        {
            try
            {
                String sName = txtName.Text;

                SearchData.DefaultView.RowFilter = " name like '" + sName + "%'";
                SearchData.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                //txtCode.Text = SearchData.DefaultView[0]["code"].ToString();

                grdView.DataSource = SearchData.DefaultView;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RefreshData()
        {
            AutoCompleteStringCollection objCode = new AutoCompleteStringCollection();
            AutoCompleteStringCollection objName = new AutoCompleteStringCollection();


            try
            {
                if (SearchData == null)
                {
                    throw new Exception("Search Data is null.");
                }

                for (int i = 0; i <= SearchData.Rows.Count - 1; i++)
                {
                    objCode.Add(SearchData.Rows[i]["code"].ToString());
                    objName.Add(SearchData.Rows[i]["name"].ToString());
                }

                txtCode.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtCode.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtCode.MaskBox.AutoCompleteCustomSource = objCode;

                txtName.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtName.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtName.MaskBox.AutoCompleteCustomSource = objName;

                txtCode.Focus();

                grdView.DataSource = SearchData;               

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (IsMultiSelect == false) 
                {
                    SelectData(Convert.ToInt16(gridView1.GetFocusedRowCellValue("id").ToString().Trim()), gridView1.GetFocusedRowCellValue("code").ToString().Trim(), gridView1.GetFocusedRowCellValue("name").ToString().Trim());

                    //SelectedCode = gridView1.GetFocusedRowCellValue("code").ToString().Trim();
                    //SelectedName = gridView1.GetFocusedRowCellValue("name").ToString().Trim();

                    //SelectedID = Convert.ToInt16(gridView1.GetFocusedRowCellValue("id").ToString().Trim());

                    //Selected = true;

                    //if (IsSensitivities == true)
                    //{
                    //    if (optMic.Checked == true)
                    //    {
                    //        MethodID = 1;
                    //    }
                    //    else
                    //    {
                    //        if (optMid.Checked == true)
                    //        {
                    //            MethodID = 2;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    MethodID = 0;
                    //}

                    //this.Close();

                }                                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                //SelectedCode = gridView1.GetFocusedRowCellValue("code").ToString().Trim();
                //SelectedName = gridView1.GetFocusedRowCellValue("name").ToString().Trim();
                //SelectedID = Convert.ToInt16(gridView1.GetFocusedRowCellValue("id").ToString().Trim());

                if (IsMultiSelect == true)
                {
                    Int32[] selectedRowHandles = gridView1.GetSelectedRows();
                    for (int i = 0; i < selectedRowHandles.Length; i++)
                    {
                        int selectedRowHandle = selectedRowHandles[i];
                        if (selectedRowHandle >= 0)
                            MultiSelectValue.Add(gridView1.GetDataRow(selectedRowHandle));
                    }

                    if (IsSensitivities == true)
                    {
                        if (optMic.Checked == true)
                        {
                            MethodID = 1;
                        }
                        else
                        {
                            if (optMid.Checked == true)
                            {
                                MethodID = 2;
                            }
                        }
                    }
                    else
                    {
                        MethodID = 0;
                    }
                    Selected = true;
                    this.Close();

                }
                else 
                {
                    SelectData(Convert.ToInt16(gridView1.GetFocusedRowCellValue("id").ToString().Trim()), gridView1.GetFocusedRowCellValue("code").ToString().Trim(), gridView1.GetFocusedRowCellValue("name").ToString().Trim());
                }

                //if (gridView1.SelectedRowsCount == 1)
                //{
                    
                //}
                //else 
                //{
                //    if (gridView1.SelectedRowsCount > 1) 
                //    {                        


                        

                //    }
                //}

              
                //if (IsSensitivities == true)
                //{
                //    if (optMic.Checked == true)
                //    {
                //        MethodID = 1;
                //    }
                //    else
                //    {
                //        if (optMid.Checked == true)
                //        {
                //            MethodID = 2;
                //        }
                //    }
                //}
                //else
                //{
                //    MethodID = 0;
                //}

                //Selected = true;
                //this.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }

        }

        private void SelectData(int ID, String Code, String Name) 
        {
            SelectedCode = Code;
            SelectedName = Name;
            SelectedID = ID;

            if (IsSensitivities == true)
            {
                if (optMic.Checked == true)
                {
                    MethodID = 1;
                }
                else
                {
                    if (optMid.Checked == true)
                    {
                        MethodID = 2;
                    }
                }
            }
            else
            {
                MethodID = 0;
            }

            Selected = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdView_Click(object sender, EventArgs e)
        {

        }

        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13) 
            //{
            //    EventTextCodeClick();                
            //}
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                 EventTextNameClick();
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                EventTextCodeClick();
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                EventTextNameClick();
            }
        }
    }
}
