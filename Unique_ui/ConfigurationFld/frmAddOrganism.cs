using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddOrganism : Form
    {
        string type;
        string strOrganism;
        private string moduleType;

        private ConfigurationController objConfig;
        private OrganismM objOrganisM;

        int rowindex = 0;

        object BREAKPOINT01;
        object BREAKPOINT02;
        object BREAKPOINT03;

        object SENPANEL01;
        object SENPANEL02;
        object SENPANEL03;

        object FAMILY01;
        object FAMILY02;
        object FAMILY03;

        public Action refreshData { get; set; }

        public frmAddOrganism(string moduleType, string strOrganism)
        {
            InitializeComponent();
            this.moduleType = moduleType;

            this.strOrganism = strOrganism;

            objConfig = new ConfigurationController();
            objOrganisM = new OrganismM();

            if (moduleType != "add")
            {
                objOrganisM = ControlParameter.OrganismM;
            }
        }
        private void frmAddOrganism_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (moduleType == "edit")
                {
                    txtCode.Text = objOrganisM.OrganismCode;
                    txtName.Text = objOrganisM.OrganismName;
                    txtMORPHODESC.Text = objOrganisM.MorphoDesc;

                    txtID.Text = Convert.ToString(objOrganisM.OrganismID);

                    if (objOrganisM.Organism_MEDTHOD_CODE == "MIC")
                    {
                        comboBoxEdit1.SelectedIndex = 0;
                    }
                    else if (objOrganisM.Organism_MEDTHOD_CODE == "MID")
                    {
                        comboBoxEdit1.SelectedIndex = 1;
                    }

                    txtComment.Text = objOrganisM.Organism_DESCRIPTION;
                    objOrganisM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                    Get_BreakPoints(objOrganisM);
                    Get_Sensitivity(objOrganisM);
                    Get_Family(objOrganisM);

                    label9.Visible = true;
                    label9.Text = "Edit Organism";
                    this.Text = "Edit Organism";
                }
                else if (moduleType == "dup")
                {
                    txtCode.Text = objOrganisM.OrganismCode;
                    txtName.Text = objOrganisM.OrganismName;
                    txtMORPHODESC.Text = objOrganisM.MorphoDesc;


                    if (objOrganisM.Organism_MEDTHOD_CODE == "MIC")
                    {
                        comboBoxEdit1.SelectedIndex = 0;
                    }
                    else if (objOrganisM.Organism_MEDTHOD_CODE == "MID")
                    {
                        comboBoxEdit1.SelectedIndex = 1;
                    }


                    txtComment.Text = objOrganisM.Organism_DESCRIPTION;
                    objOrganisM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                    Get_BreakPoints(objOrganisM);
                    Get_Sensitivity(objOrganisM);
                    //Get_Family(objOrganisM);

                    label9.Visible = true;
                    label9.Text = "Duplicate Organism";
                    this.Text = "Duplicate Organism";
                }
                else if (moduleType == "add")
                {
                    txtName.Text = objOrganisM.OrganismName;
                    comboBoxEdit1.Text = objOrganisM.Organism_MEDTHOD_NAME;

                    txtComment.Text = objOrganisM.Organism_DESCRIPTION;
                    objOrganisM.Organism_LOGUSERID = ControlParameter.loginID.ToString();

                    label9.Visible = true;
                    label9.Text = "Add Organism";
                    this.Text = "Add Organism";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void Get_BreakPoints(OrganismM objOrganismM)
        {
            ConfigurationController objGetBreakPoint = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objGetBreakPoint.GetGridOrganism_BreakPoint(objOrganismM);
                // fill dt to datagrid BreakPoint
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
        private void Get_Sensitivity(OrganismM objOrganismM)
        {
            ConfigurationController objGetSensitivity = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objGetSensitivity.GetGridOrganism_Sensitivity(objOrganismM);
                // fill dt to datagrid BreakPoint
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
        private void Get_Family(OrganismM objOrganismM)
        {
            ConfigurationController objGetSensitivity = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objGetSensitivity.GetGridOrganism_Family(objOrganismM);
                // fill dt to datagrid BreakPoint
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView3.Rows.Add();

                        dataGridView3.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView3.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView3.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
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
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
        }
        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView1.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView1.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex) { }
            }
        }
        private void contextMenuStrip2_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView2.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView2.Rows.RemoveAt(this.rowindex);

                }
                catch (Exception ex) { }
            }

        }
        private void contextMenuStrip3_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView3.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView3.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex) { }
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
                        contextMenuStrip2.Show(Cursor.Position);
                    }
                }
            }

        }
        private void dataGridView3_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView3.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView3.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView3.CurrentCell = this.dataGridView3.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip3.Show(this.dataGridView3, e.Location);
                        contextMenuStrip3.Show(Cursor.Position);
                    }
                }
            }

        }
        private void btnSearch_G1_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetDICT_SensitivityPanel_Organisms();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objOrganisM.OrganismID = Convert.ToInt32(fm.SelectedID.ToString());
                    objOrganisM.OrganismName = fm.SelectedName;
                    objOrganisM.OrganismCode = fm.SelectedCode;

                    SENPANEL01 = objOrganisM.OrganismCode;
                    SENPANEL02 = objOrganisM.OrganismName;
                    SENPANEL03 = objOrganisM.OrganismID;

                    int minRowCount = 0;

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        minRowCount++;
                    }
                    if (minRowCount == 0)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = SENPANEL01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = SENPANEL02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = SENPANEL03;
                    }
                    else
                    {
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = SENPANEL01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = SENPANEL02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = SENPANEL03;
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
        private void btnSearch_G2_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetDICT_BreakPoint_Organisms();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objOrganisM.OrganismID = Convert.ToInt32(fm.SelectedID.ToString());
                    objOrganisM.OrganismName = fm.SelectedName;
                    objOrganisM.OrganismCode = fm.SelectedCode;

                    BREAKPOINT01 = objOrganisM.OrganismCode;
                    BREAKPOINT02 = objOrganisM.OrganismName;
                    BREAKPOINT03 = objOrganisM.OrganismID;

                    int minRowCount = 0;

                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        minRowCount++;
                    }
                    if (minRowCount == 0)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = BREAKPOINT01;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = BREAKPOINT02;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = BREAKPOINT03;
                    }
                    else
                    {
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = BREAKPOINT01;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = BREAKPOINT02;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = BREAKPOINT03;
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
        private void btnSearch_G3_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetDICT_Family_Organisms();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objOrganisM.OrganismID = Convert.ToInt32(fm.SelectedID.ToString());
                    objOrganisM.OrganismName = fm.SelectedName;
                    objOrganisM.OrganismCode = fm.SelectedCode;

                    FAMILY01 = objOrganisM.OrganismCode;
                    FAMILY02 = objOrganisM.OrganismName;
                    FAMILY03 = objOrganisM.OrganismID;

                    int minRowCount = 0;

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        minRowCount++;
                    }
                    if (minRowCount == 0)
                    {
                        dataGridView3.Rows.Add();
                        dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[0].Value = FAMILY01;
                        dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[1].Value = FAMILY02;
                        dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[2].Value = FAMILY03;
                    }
                    else
                    {
                        dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[0].Value = FAMILY01;
                        dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[1].Value = FAMILY02;
                        dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[2].Value = FAMILY03;
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
        // SAVE
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

                                objOrganisM.OrganismID = Convert.ToInt32(txtID.Text);
                                objOrganisM.OrganismCode = txtCode.Text;
                                objOrganisM.OrganismName = txtName.Text;
                                objOrganisM.MorphoDesc = txtMORPHODESC.Text;
                                objOrganisM.Organism_DESCRIPTION = txtComment.Text;

                                if (comboBoxEdit1.SelectedIndex == 0)
                                {
                                    objOrganisM.Organism_MEDTHOD_ID = "1";
                                }
                                else if (comboBoxEdit1.SelectedIndex == 1)
                                {
                                    objOrganisM.Organism_MEDTHOD_ID = "2";
                                }
                                else
                                {
                                    objOrganisM.Organism_MEDTHOD_ID = "0";
                                }

                                objOrganisM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                                objOrganisM = objConfig.SaveOrganisms(objOrganisM);

                                // Sensitivity save
                                if (dataGridView1.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objOrganisM.Organism_SENSITIVITY_PANEL_ID = row.Cells[2].Value.ToString();
                                        objOrganisM = objConfig.SaveSensitivity_In_DICT_ORGANISMS(objOrganisM);
                                    }
                                }
                                else
                                {
                                    // Clear to Null in Sensitivity ID
                                    objOrganisM = objConfig.DeleteSensitivity_List_DICT_ORGANISMS(objOrganisM);
                                }

                                // BreakPoint save
                                if (dataGridView2.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objOrganisM.Organism_BREAKPOINT_LIST_ID = row.Cells[2].Value.ToString();
                                        objOrganisM = objConfig.SaveBreakPoint_In_DICT_ORGANISMS(objOrganisM);
                                    }
                                }
                                else
                                {
                                    objOrganisM = objConfig.DeleteBreakPoint_List_DICT_ORGANISMS(objOrganisM);
                                }

                                // Family save
                                if (dataGridView3.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objOrganisM.Organism_FAMILY_LIST_ID = row.Cells[2].Value.ToString();
                                        objOrganisM = objConfig.SaveFamily_In_DICT_ORGANISMS(objOrganisM);
                                    }
                                }
                                else
                                {
                                    objOrganisM = objConfig.DeleteFamily_List_DICT_ORGANISMS(objOrganisM);
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
                            try
                            {
                                objOrganisM.OrganismCode = txtCode.Text;
                                objOrganisM.OrganismName = txtName.Text;
                                objOrganisM.MorphoDesc = txtMORPHODESC.Text;
                                objOrganisM.Organism_DESCRIPTION = txtComment.Text;

                                if (comboBoxEdit1.SelectedIndex == 0)
                                {
                                    objOrganisM.Organism_MEDTHOD_ID = "1";
                                }
                                else if (comboBoxEdit1.SelectedIndex == 1)
                                {
                                    objOrganisM.Organism_MEDTHOD_ID = "2";
                                }
                                else
                                {
                                    objOrganisM.Organism_MEDTHOD_ID = "0";
                                }

                                objOrganisM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                                objOrganisM = objConfig.SaveOrganisms(objOrganisM);

                                // Sensitivity save
                                if (dataGridView1.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objOrganisM.Organism_SENSITIVITY_PANEL_ID = row.Cells[2].Value.ToString();
                                        objOrganisM = objConfig.SaveSensitivity_In_DICT_ORGANISMS(objOrganisM);
                                    }
                                }
                                else
                                {
                                    // Clear to Null in Sensitivity ID
                                    objOrganisM = objConfig.DeleteSensitivity_List_DICT_ORGANISMS(objOrganisM);
                                }

                                // BreakPoint save
                                if (dataGridView2.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objOrganisM.Organism_BREAKPOINT_LIST_ID = row.Cells[2].Value.ToString();
                                        objOrganisM = objConfig.SaveBreakPoint_In_DICT_ORGANISMS(objOrganisM);
                                    }
                                }
                                else
                                {
                                    objOrganisM = objConfig.DeleteBreakPoint_List_DICT_ORGANISMS(objOrganisM);
                                }

                                // Family save
                                if (dataGridView3.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objOrganisM.Organism_FAMILY_LIST_ID = row.Cells[2].Value.ToString();
                                        objOrganisM = objConfig.SaveFamily_In_DICT_ORGANISMS(objOrganisM);
                                    }
                                }
                                else
                                {
                                    objOrganisM = objConfig.DeleteFamily_List_DICT_ORGANISMS(objOrganisM);
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
                        MessageBox.Show("Error in Function Save [dup]", "Warning" + ex.Message);
                    }
                }
                else if (moduleType == "add")
                {
                    try
                    {
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            try
                            {
                                objOrganisM.OrganismCode = txtCode.Text;
                                objOrganisM.OrganismName = txtName.Text;
                                objOrganisM.MorphoDesc = txtMORPHODESC.Text;
                                objOrganisM.Organism_DESCRIPTION = txtComment.Text;

                                if (comboBoxEdit1.SelectedIndex == 0)
                                {
                                    objOrganisM.Organism_MEDTHOD_ID = "1";
                                }
                                else if (comboBoxEdit1.SelectedIndex == 1)
                                {
                                    objOrganisM.Organism_MEDTHOD_ID = "2";
                                }
                                else
                                {
                                    objOrganisM.Organism_MEDTHOD_ID = "0";
                                }

                                objOrganisM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                                objOrganisM = objConfig.SaveOrganisms(objOrganisM);

                                // Sensitivity save
                                if (dataGridView1.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objOrganisM.Organism_SENSITIVITY_PANEL_ID = row.Cells[2].Value.ToString();
                                        objOrganisM = objConfig.SaveSensitivity_In_DICT_ORGANISMS(objOrganisM);
                                    }
                                }

                                // BreakPoint save
                                if (dataGridView2.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objOrganisM.Organism_BREAKPOINT_LIST_ID = row.Cells[2].Value.ToString();
                                        objOrganisM = objConfig.SaveBreakPoint_In_DICT_ORGANISMS(objOrganisM);
                                    }
                                }

                                // Family save
                                if (dataGridView3.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objOrganisM.Organism_FAMILY_LIST_ID = row.Cells[2].Value.ToString();
                                        objOrganisM = objConfig.SaveFamily_In_DICT_ORGANISMS(objOrganisM);
                                    }
                                }

                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
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
    }
}
