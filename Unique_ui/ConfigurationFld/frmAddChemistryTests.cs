using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddChemistryTests : Form
    {

        private string moduleType;
        private string ChemistryCustomizedID;
        private ChemistryM objChemistryM;

        private ConfigurationController objConfiguration = new ConfigurationController();

        int rowindex = 0;

        object CUSTL01;
        object CUSTL02;
        object CUSTL03;

        public Action refreshData { get; set; }

        public frmAddChemistryTests(string moduleType,string ChemistryID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.ChemistryCustomizedID = ChemistryID;
            objConfiguration = new ConfigurationController();

            objChemistryM = new ChemistryM();

            if (moduleType != "add")
            {
                objChemistryM = ControlParameter.ChemistryM;
            }
        }

        private void frmAddChemisrtyTests_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (moduleType == "edit")
                {
                    txtCode.Enabled = false;
                    txtName.Select();
                    txtID.Text = objChemistryM.CHEMISTRY_ID;
                    txtCode.Text = objChemistryM.CHEMISTRY_CODE;
                    txtName.Text = objChemistryM.CHEMISTRY_SHORTTEXT;
                    txtFulltext.Text = objChemistryM.CHEMISTRY_FULLTEXT;
                    txtDescription.Text = objChemistryM.CHEMISTRY_DESCRIPTION;

                    if (objChemistryM.CHEMISTRY_PRINT == "No")
                    {
                        checkBox_Print.Checked = false;
                    }
                    else if (objChemistryM.CHEMISTRY_PRINT == "Yes")
                    {
                        checkBox_Print.Checked = true;
                    }

                    objChemistryM.CHEMISTRY_LOGUSERID = ControlParameter.loginID.ToString();

                    Get_ChemistryTests_IN_Dict_(objChemistryM);

                    label6.Visible = true;
                    label6.Text = "Edit Chemistry Result";
                    this.Text = "Edit Chemistry Result";
                }
                else if (moduleType == "dup")
                {

                    txtCode.Select();
                    txtName.Text = objChemistryM.CHEMISTRY_SHORTTEXT;
                    txtFulltext.Text = objChemistryM.CHEMISTRY_FULLTEXT;
                    txtDescription.Text = objChemistryM.CHEMISTRY_DESCRIPTION;

                    objChemistryM.CHEMISTRY_LOGUSERID = ControlParameter.loginID.ToString();

                    Get_ChemistryTests_IN_Dict_(objChemistryM);

                    label6.Visible = true;
                    label6.Text = "Duplicate Chemistry Result";
                    this.Text = "Duplicate Chemistry Result";
                }
                else if (moduleType == "add")
                {
                    txtCode.Select();
                    label6.Visible = true;
                    label6.Text = "Add Chemistry Result";
                    this.Text = "Add Chemistry Result";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_ElementClick_1(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void Get_ChemistryTests_IN_Dict_(ChemistryM objChemistryM)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objConfiguration.GetChemistryTests_In_dict(objChemistryM);
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

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
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
                                objChemistryM.CHEMISTRY_ID = txtID.Text;
                                objChemistryM.CHEMISTRY_CODE = txtCode.Text;
                                objChemistryM.CHEMISTRY_SHORTTEXT = txtName.Text;
                                objChemistryM.CHEMISTRY_FULLTEXT = txtFulltext.Text;
                                objChemistryM.CHEMISTRY_DESCRIPTION = txtDescription.Text;

                                if (checkBox_Print.Checked == true)
                                {
                                    objChemistryM.CHEMISTRY_PRINT = "1";
                                }
                                else if (checkBox_Print.Checked == false)
                                {
                                    objChemistryM.CHEMISTRY_PRINT = "0";
                                }

                                objChemistryM.CHEMISTRY_LOGUSERID = ControlParameter.loginID.ToString();
                                objChemistryM = objConfiguration.SaveChemistryTests(objChemistryM);

                                if (dataGridView1.Rows.Count > 0)
                                {
                                    objChemistryM = objConfiguration.ClearChemistryTests_Customized_Result(objChemistryM);

                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objChemistryM.CHEMISTRY_CUSTOMIZED_ID = row.Cells[2].Value.ToString();
                                        objChemistryM = objConfiguration.SaveChemistryTests_Customized_Result(objChemistryM);
                                    }
                                }
                                else
                                {
                                    objChemistryM = objConfiguration.ClearChemistryTests_Customized_Result(objChemistryM);
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
                        if (txtCode.Text != "")
                        {
                            objChemistryM.CHEMISTRY_CODE = txtCode.Text;
                            objChemistryM.CHEMISTRY_SHORTTEXT = txtName.Text;
                            objChemistryM.CHEMISTRY_FULLTEXT = txtFulltext.Text;
                            objChemistryM.CHEMISTRY_DESCRIPTION = txtDescription.Text;

                            if (checkBox_Print.Checked == true)
                            {
                                objChemistryM.CHEMISTRY_PRINT = "1";
                            }
                            else if (checkBox_Print.Checked == false)
                            {
                                objChemistryM.CHEMISTRY_PRINT = "0";
                            }

                            objChemistryM.CHEMISTRY_LOGUSERID = ControlParameter.loginID.ToString();
                            objChemistryM = objConfiguration.SaveChemistryTests(objChemistryM);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objChemistryM.CHEMISTRY_CUSTOMIZED_ID = row.Cells[2].Value.ToString();
                                    objChemistryM = objConfiguration.SaveChemistryTests_Customized_Result(objChemistryM);
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
                        // Code & Name  Fix do not empty!
                        if (txtCode.Text != "")
                        {
                            objChemistryM.CHEMISTRY_CODE = txtCode.Text;
                            objChemistryM.CHEMISTRY_SHORTTEXT = txtName.Text;
                            objChemistryM.CHEMISTRY_FULLTEXT = txtFulltext.Text;
                            objChemistryM.CHEMISTRY_DESCRIPTION = txtDescription.Text;

                            if (checkBox_Print.Checked == true)
                            {
                                objChemistryM.CHEMISTRY_PRINT = "1";
                            }
                            else if (checkBox_Print.Checked == false)
                            {
                                objChemistryM.CHEMISTRY_PRINT = "0";
                            }

                            objChemistryM.CHEMISTRY_LOGUSERID = ControlParameter.loginID.ToString();
                            objChemistryM = objConfiguration.SaveChemistryTests(objChemistryM);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objChemistryM.CHEMISTRY_CUSTOMIZED_ID = row.Cells[2].Value.ToString();
                                    objChemistryM = objConfiguration.SaveChemistryTests_Customized_Result(objChemistryM);
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

        private void Btn_search_G1_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetGridChemistryTests();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objChemistryM.CHEMISTRY_CUSTOMIZED_ID = fm.SelectedID.ToString();
                    objChemistryM.CHEMISTRY_CUSTOMIZED_NAME = fm.SelectedName;
                    objChemistryM.CHEMISTRY_CUSTOMIZED_CODE = fm.SelectedCode;

                    CUSTL01 = objChemistryM.CHEMISTRY_CUSTOMIZED_CODE;
                    CUSTL02 = objChemistryM.CHEMISTRY_CUSTOMIZED_NAME;
                    CUSTL03 = objChemistryM.CHEMISTRY_CUSTOMIZED_ID;

                    int minRowCount = 0;

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        minRowCount++;
                    }
                    if (minRowCount == 0)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = CUSTL01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = CUSTL02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = CUSTL03;
                    }
                    else
                    {
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = CUSTL01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = CUSTL02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = CUSTL03;
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

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
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
                        this.contextMenuStrip1.Show(this.dataGridView1, e.Location);
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
            }
        }
    }
}
