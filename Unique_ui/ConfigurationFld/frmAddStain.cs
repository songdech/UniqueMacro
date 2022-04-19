using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddStain : Form
    {
        private string moduleType;
        private string strStainID;

        private StainM objStainM;
        private ConfigurationController objConfiguration = new ConfigurationController();

        int rowindex = 0;
        object STAIN01;
        object STAIN02;
        object STAIN03;


        string Printable ="";

        public frmAddStain(string moduleType, string strStainID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.strStainID = strStainID;
            objStainM = ControlParameter.StainM;

            if (moduleType != "add")
            {
                objStainM = ControlParameter.StainM;
            }
        }
        public Action refreshData { get; set; }      
        private void frmAddStain_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            //DataTable dt;
            try
            {
                if (moduleType == "edit")
                {
                    txtCode.Text = objStainM.MBSTAINCODE;
                    txtName.Text = objStainM.STAINNAME;
                    txtID.Text = Convert.ToString(objStainM.MBSTAINID);
                    txtComment.Text = objStainM.STAIN_DESCRIPTION;
                    objStainM.STAIN_LOGUSERID = ControlParameter.loginID.ToString();
                    Printable = objStainM.STAIN_NOTPRINTABLE;

                    if (Printable == "1")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }

                    Get_Morphology_Results(objStainM);
                    Get_Quantitative_Results(objStainM);

                    label9.Visible = true;
                    label9.Text = "Edit Stain";
                    this.Text = "Edit Stain";
                }
                else if (moduleType == "dup")
                {
                    txtCode.Select();
                    txtName.Text = objStainM.STAINNAME;
                    txtComment.Text = objStainM.STAIN_DESCRIPTION;
                    objStainM.STAIN_LOGUSERID = ControlParameter.loginID.ToString();
                    Printable = objStainM.STAIN_NOTPRINTABLE;

                    if (Printable == "1")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }

                    Get_Morphology_Results(objStainM);
                    Get_Quantitative_Results(objStainM);

                    label9.Visible = true;
                    label9.Text = "Duplicate Stain";
                    this.Text = "Duplicate Stain";
                }
                else if (moduleType == "add")
                {
                    txtName.Select();

                    if (checkBox1.Checked == true)
                    {
                        Printable = "1";
                    }
                    else
                    {
                        Printable = "0";
                    }

                    label9.Visible = true;
                    label9.Text = "Add Stain";
                    this.Text = "Add Stain";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void Get_Morphology_Results(StainM objStainM)
        {
            // Properties = =0
            ConfigurationController objConfiguration = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objConfiguration.GetResultProperty(objStainM);
                // Manual fill dt to datagrid
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView2.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView2.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void Get_Quantitative_Results(StainM objStainM)
        {
            // Properties = 1
            ConfigurationController objConfiguration = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objConfiguration.GetResultProperty_Quantitative(objStainM);
                // Manual fill dt to datagrid
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView1.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }
        private void navSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                if (moduleType == "edit")
                {
                    try
                    {
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            try
                            {
                                txtCode.Enabled = false;

                                objStainM.MBSTAINID = Convert.ToInt32(txtID.Text);
                                objStainM.MBSTAINCODE = txtCode.Text;
                                objStainM.STAINNAME = txtName.Text;
                                objStainM.STAIN_DESCRIPTION = txtComment.Text;
                                objStainM.STAIN_NOTPRINTABLE = Printable;
                                objStainM.STAIN_LOGUSERID = ControlParameter.loginID.ToString();
                                objStainM = objConfiguration.SaveStain(objStainM);

                                // Morphology result 
                                // Property result = 0
                                if (dataGridView2.Rows.Count > 0)
                                {
                                    objStainM = objConfiguration.DeleteResultProperty_Morphology(objStainM);
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objStainM.CUSTRSULT_MOR_ID = Convert.ToInt32(row.Cells[2].Value.ToString());
                                        objStainM = objConfiguration.SaveResultProperty_Morphology(objStainM);
                                    }
                                }
                                else
                                {
                                    objStainM = objConfiguration.DeleteResultProperty_Morphology(objStainM);
                                }

                                // Quantitative result
                                // Property result = 1
                                if (dataGridView1.Rows.Count > 0)
                                {
                                    objStainM = objConfiguration.DeleteResultProperty_Quantitative(objStainM);
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objStainM.CUSRESULT_QUAN_ID = Convert.ToInt32(row.Cells[2].Value.ToString());
                                        objStainM = objConfiguration.SaveResultProperty_Quantitative(objStainM);
                                    }
                                }
                                else
                                {
                                    objStainM = objConfiguration.DeleteResultProperty_Quantitative(objStainM);
                                }


                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [edit]", ex.Message);
                    }
                }
                else if (moduleType == "dup")
                {
                    try
                    {
                        // Code & Name  Fix do not empty!
                        if (txtCode.Text != "" && txtName.Text != "")
                        {

                            objStainM.MBSTAINCODE = txtCode.Text;
                            objStainM.STAINNAME = txtName.Text;
                            objStainM.STAIN_DESCRIPTION = txtComment.Text;
                            objStainM.STAIN_NOTPRINTABLE = Printable;
                            objStainM.STAIN_LOGUSERID = ControlParameter.loginID.ToString();
                            objStainM = objConfiguration.SaveStain(objStainM);

                            // Morphology result 
                            // Property result = 0
                            if (dataGridView2.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    objStainM.CUSTRSULT_MOR_ID = Convert.ToInt32(row.Cells[2].Value.ToString());
                                    objStainM = objConfiguration.SaveResultProperty_Morphology(objStainM);
                                }
                            }

                            // Quantitative result
                            // Property result = 1
                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objStainM.CUSRESULT_QUAN_ID = Convert.ToInt32(row.Cells[2].Value.ToString());
                                    objStainM = objConfiguration.SaveResultProperty_Quantitative(objStainM);
                                }
                            }

                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [dup]", "Warning" + ex.Message);
                    }
                }
                else if (moduleType == "add")
                {
                    try
                    {
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            objStainM.MBSTAINCODE = txtCode.Text;
                            objStainM.STAINNAME = txtName.Text;
                            objStainM.STAIN_DESCRIPTION = txtComment.Text;
                            objStainM.STAIN_NOTPRINTABLE = Printable;
                            objStainM.STAIN_LOGUSERID = ControlParameter.loginID.ToString();
                            objStainM = objConfiguration.SaveStain(objStainM);

                            // Morphology result 
                            // Property result = 0
                            if (dataGridView2.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    objStainM.CUSTRSULT_MOR_ID = Convert.ToInt32(row.Cells[2].Value.ToString());
                                    objStainM = objConfiguration.SaveResultProperty_Morphology(objStainM);
                                }
                            }

                            // Quantitative result
                            // Property result = 1
                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objStainM.CUSRESULT_QUAN_ID = Convert.ToInt32(row.Cells[2].Value.ToString());
                                    objStainM = objConfiguration.SaveResultProperty_Quantitative(objStainM);
                                }
                            }
                            this.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [add]", ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Error Function Save Desc.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView2.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView2.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void dataGridView2_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView2.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView2.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView2.CurrentCell = this.dataGridView2.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip1.Show(this.dataGridView2, e.Location);
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Printable = "1";
            }
            else
            {
                Printable = "0";
            }
        }

        private void Btn_search_G2_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetDICT_CUSTOMIZED_RESULT_FOR_STAIN();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objStainM.CUSTRSULT_MOR_ID = Convert.ToInt32(fm.SelectedID.ToString());
                    objStainM.CUSRESULT_MOR_NAME = fm.SelectedName;
                    objStainM.CUSTRSULT_MOR_CODE = fm.SelectedCode;

                    STAIN01 = objStainM.CUSTRSULT_MOR_CODE;
                    STAIN02 = objStainM.CUSRESULT_MOR_NAME;
                    STAIN03 = objStainM.CUSTRSULT_MOR_ID;

                    int minRowCount = 0;

                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        minRowCount++;
                    }
                    if (minRowCount == 0)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = STAIN01;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = STAIN02;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = STAIN03;
                    }
                    else
                    {
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = STAIN01;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = STAIN02;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = STAIN03;
                    }
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

        private void Btn_search_G1_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetDICT_CUSTOMIZED_RESULT_FOR_STAIN();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objStainM.CUSRESULT_QUAN_ID = Convert.ToInt32(fm.SelectedID.ToString());
                    objStainM.CUSTRSULT_QUAN_NAME = fm.SelectedName;
                    objStainM.CUSRESULT_QUAN_CODE = fm.SelectedCode;

                    STAIN01 = objStainM.CUSRESULT_QUAN_CODE;
                    STAIN02 = objStainM.CUSTRSULT_QUAN_NAME;
                    STAIN03 = objStainM.CUSRESULT_QUAN_ID;

                    int minRowCount = 0;

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        minRowCount++;
                    }
                    if (minRowCount == 0)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = STAIN01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = STAIN02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = STAIN03;
                    }
                    else
                    {
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = STAIN01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = STAIN02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = STAIN03;
                    }
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

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void contextMenuStrip2_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView1.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView1.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView1.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip2.Show(this.dataGridView1, e.Location);
                        contextMenuStrip2.Show(Cursor.Position);
                    }
                }
            }

        }
    }
}
