using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddSensitivityPanel : Form
    {
        private string moduleType;
        //private string strSensitivityPanelID;

        private ConfigurationController objConfigController;
        private SenpanelM objSenPanelM;

        int rowindex = 0;

        object CUSTL01;
        object CUSTL02;
        object CUSTL03;

        object ORGANISM01;
        object ORGANISM02;
        object ORGANISM03;

        object ANTIBIO01;
        object ANTIBIO02;
        object ANTIBIO03;

        public frmAddSensitivityPanel(string moduleType, string strSensitivityPanelID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            //this.strSensitivityPanelID = strSensitivityPanelID;
            objConfigController = new ConfigurationController();
            objSenPanelM = new SenpanelM();

            if (moduleType != "add")
            {
                objSenPanelM = ControlParameter.SenPanelM;
            }
        }

        public Action refreshData { get; set; }

        private void frmAddSensitivityPanel_Load(object sender, EventArgs e)
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
                    txtID.Text = objSenPanelM.SENPANEL_ID;
                    txtCode.Text = objSenPanelM.SENPANEL_CODE;
                    txtName.Text = objSenPanelM.SENPANEL_SHORTTEXT;
                    txtComment.Text = objSenPanelM.SENPANEL_FULLTEXT;


                    if (objSenPanelM.SENPANEL_METHOD == "MIC")
                    {
                        checkBox1.Checked = true;
                        checkBox2.Checked = false;
                    }
                    else if (objSenPanelM.SENPANEL_METHOD == "MID")
                    {
                        checkBox1.Checked = false;
                        checkBox2.Checked = true;
                    }

                    Get_AntibioticList_(objSenPanelM);
                    Get_Organism_(objSenPanelM);

                    objSenPanelM.SENPANEL_LOGUSERID = ControlParameter.loginID.ToString();

                    label6.Visible = true;
                    label6.Text = "Edit Sensitivities Panel";
                    this.Text = "Edit Sensitivities Panel";
                }
                else if (moduleType == "dup")
                {
                    txtCode.Select();
                    txtName.Text = objSenPanelM.SENPANEL_SHORTTEXT;
                    txtComment.Text = objSenPanelM.SENPANEL_FULLTEXT;

                    Get_AntibioticList_(objSenPanelM);
                    Get_Organism_(objSenPanelM);

                    objSenPanelM.SENPANEL_LOGUSERID = ControlParameter.loginID.ToString();

                    label6.Visible = true;
                    label6.Text = "Duplicate Sensitivities Panel";
                    this.Text = "Duplicate Sensitivities Panel";
                }
                else if (moduleType == "add")
                {

                    txtCode.Select();
                    label6.Visible = true;
                    label6.Text = "Add Sensitivities Panel";
                    this.Text = "Add Sensitivities Panel";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Get_AntibioticList_(SenpanelM objSenPanelM)
        {
            ConfigurationController objAntibioticList = new ConfigurationController();
            DataTable dt = null;

            try
            {

                dt = objAntibioticList.GetAntibioticWithSensitivityPanel(objSenPanelM);
                // Manual fill dt to datagrid
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[i].Cells[0].Value = dt.Rows[i]["ANTIBIOTICCODE"].ToString();
                        dataGridView2.Rows[i].Cells[1].Value = dt.Rows[i]["FULLTEXT"].ToString();
                        dataGridView2.Rows[i].Cells[2].Value = dt.Rows[i]["ANTIBIOTICID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objSenPanelM = null;
            }
        }
        private void Get_Organism_(SenpanelM objSenPanelM)
        {
            ConfigurationController objConfigGetOrganisms_sensitivity = new ConfigurationController();
            DataTable dt = null;

            try
            {

                dt = objConfigGetOrganisms_sensitivity.Get_Organisms_With_SensitivityPanel(objSenPanelM);
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
            finally
            {
                objSenPanelM = null;
            }
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }
        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                if (moduleType == "edit")
                {
                    try
                    {
                        if (txtCode.Text != "")
                        {
                            try
                            {
                                SenpanelM objSenPanelM = new SenpanelM();

                                txtCode.ReadOnly = true;
                                objSenPanelM.SENPANEL_ID = txtID.Text;
                                objSenPanelM.SENPANEL_CODE = txtCode.Text;
                                objSenPanelM.SENPANEL_SHORTTEXT = txtName.Text;
                                objSenPanelM.SENPANEL_FULLTEXT = txtComment.Text;

                                objSenPanelM.SENPANEL_LOGUSERID = ControlParameter.loginID.ToString();

                                if (checkBox1.Checked == true)
                                {
                                    objSenPanelM.SENPANEL_METHOD = "1";
                                }
                                else
                                {
                                    if (checkBox2.Checked == true)
                                    {
                                        objSenPanelM.SENPANEL_METHOD = "2";
                                    }
                                }
                                objSenPanelM = objConfigController.SaveSensitivityPanel(objSenPanelM);

                                // Delete if Module = Edit Antibiotic if exist in database
                                // Antibiotic Lists
                                if (dataGridView2.Rows.Count > 0)
                                {
                                    objSenPanelM = objConfigController.DeleteAntibioticList(objSenPanelM);

                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objSenPanelM.SENPANEL_ANTIBIOTIC_ID = (row.Cells[2].Value.ToString());
                                        objSenPanelM = objConfigController.SaveAntibiotic_list_SensitivityPanel(objSenPanelM);
                                    }
                                }
                                else
                                {
                                    objSenPanelM = objConfigController.DeleteAntibioticList(objSenPanelM);
                                }

                                // Organisms Lists
                                if (dataGridView1.Rows.Count > 0)
                                {
                                    objSenPanelM = objConfigController.ClearOrganisms_List_in_SenPanel(objSenPanelM);

                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objSenPanelM.SENPANEL_Organisms_ID = (row.Cells[2].Value.ToString());
                                        objSenPanelM = objConfigController.SaveOrganisms_list_SenPanel(objSenPanelM);
                                    }
                                }
                                else
                                {
                                    objSenPanelM = objConfigController.ClearOrganisms_List_in_SenPanel(objSenPanelM);
                                }


                                this.Close();
                                Action instance = refreshData;
                                if (instance != null)
                                    instance();
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
                        // Code Fix do not empty!
                        if (txtCode.Text != "")
                        {
                            objSenPanelM.SENPANEL_CODE = txtCode.Text;
                            objSenPanelM.SENPANEL_SHORTTEXT = txtName.Text;
                            objSenPanelM.SENPANEL_FULLTEXT = txtComment.Text;

                            objSenPanelM.SENPANEL_LOGUSERID = ControlParameter.loginID.ToString();

                            if (checkBox1.Checked == true)
                            {
                                objSenPanelM.SENPANEL_METHOD = "1";
                            }
                            else
                            {
                                if (checkBox2.Checked == true)
                                {
                                    objSenPanelM.SENPANEL_METHOD = "2";
                                }
                            }
                            objSenPanelM = objConfigController.SaveSensitivityPanel(objSenPanelM);

                                if (dataGridView2.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                    objSenPanelM.SENPANEL_ANTIBIOTIC_ID = (row.Cells[2].Value.ToString());
                                    objSenPanelM = objConfigController.SaveAntibiotic_list_SensitivityPanel(objSenPanelM);
                                    }
                                }
                                this.Close();
                                Action instance = refreshData;
                                if (instance != null)
                                    instance();
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
                        if (txtCode.Text != "")
                        {
                            SenpanelM objSenPanelM = new SenpanelM();
                            objSenPanelM.SENPANEL_CODE = txtCode.Text;
                            objSenPanelM.SENPANEL_SHORTTEXT = txtName.Text;
                            objSenPanelM.SENPANEL_FULLTEXT = txtComment.Text;

                            objSenPanelM.SENPANEL_LOGUSERID = ControlParameter.loginID.ToString();

                            if (checkBox1.Checked == true)
                            {
                                objSenPanelM.SENPANEL_METHOD = "1";
                            }
                            else
                            {
                                if (checkBox2.Checked == true)
                                {
                                    objSenPanelM.SENPANEL_METHOD = "2";
                                }
                            }
                            objSenPanelM = objConfigController.SaveSensitivityPanel(objSenPanelM);

                                if (dataGridView2.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                    objSenPanelM.SENPANEL_ANTIBIOTIC_ID = (row.Cells[2].Value.ToString());
                                    objSenPanelM = objConfigController.SaveAntibiotic_list_SensitivityPanel(objSenPanelM);
                                    }
                                }

                                this.Close();
                                Action instance = refreshData;
                                if (instance != null)
                                    instance();
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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
            }
            else if (checkBox2.Checked == false)
            {
                checkBox1.Checked = true;
            }

        }
        private void btnClearStain_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
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
                        this.contextMenuStrip.Show(this.dataGridView2, e.Location);
                        contextMenuStrip.Show(Cursor.Position);
                    }
                }
            }

        }
        private void contextMenuStrip_Click(object sender, EventArgs e)
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
            }
            else if (checkBox1.Checked == false)
            {
                checkBox2.Checked = true;
            }
        }

        private void btnSearch_G1_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.Get_Organisms_SensitivityPanel();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objSenPanelM.SENSITIVITYID = Convert.ToInt32(fm.SelectedID.ToString());
                    objSenPanelM.SENSITIVITYNAME = fm.SelectedName;
                    objSenPanelM.SENSITIVITYCODE = fm.SelectedCode;

                    ORGANISM01 = objSenPanelM.SENSITIVITYCODE;
                    ORGANISM02 = objSenPanelM.SENSITIVITYNAME;
                    ORGANISM03 = objSenPanelM.SENSITIVITYID;

                    int minRowCount = 0;
                    bool CheckBreakPointcode = false;
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = ORGANISM01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = ORGANISM02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = ORGANISM03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[2].Value.ToString().Contains(ORGANISM03.ToString()))
                            {
                                CheckBreakPointcode = true;
                            }
                        }
                        if (CheckBreakPointcode == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckBreakPointcode == false)
                        {
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = ORGANISM01;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = ORGANISM02;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = ORGANISM03;
                        }
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
                dt = objConfiguration.Get_Antibiotics();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["NAME"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objSenPanelM.SENSITIVITYID = Convert.ToInt32(fm.SelectedID.ToString());
                    objSenPanelM.SENSITIVITYNAME = fm.SelectedName;
                    objSenPanelM.SENSITIVITYCODE = fm.SelectedCode;

                    ANTIBIO01 = objSenPanelM.SENSITIVITYCODE;
                    ANTIBIO02 = objSenPanelM.SENSITIVITYNAME;
                    ANTIBIO03 = objSenPanelM.SENSITIVITYID;

                    int minRowCount = 0;
                    bool Checkcode = false;
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = ANTIBIO01;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = ANTIBIO02;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = ANTIBIO03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells[2].Value.ToString().Contains(ANTIBIO03.ToString()))
                            {
                                Checkcode = true;
                            }
                        }
                        if (Checkcode == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (Checkcode == false)
                        {
                            dataGridView2.Rows.Add();
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = ANTIBIO01;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = ANTIBIO02;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = ANTIBIO03;
                        }
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
    }
}
