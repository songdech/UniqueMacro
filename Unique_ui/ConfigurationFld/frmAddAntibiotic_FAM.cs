using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddAntibiotic_FAM : Form
    {

        private string moduleType;
        private AntibioticsM objAntibioticsM;

        private ConfigurationController objConfig;
        int rowindex = 0;
        object ANTIFAMS01;
        object ANTIFAMS02;
        object ANTIFAMS03;

        public Action refreshData { get; set; }

        public frmAddAntibiotic_FAM(string moduleType, string antiID)
        {
            InitializeComponent();
            this.moduleType = moduleType;

            objConfig = new ConfigurationController();
            objAntibioticsM = new AntibioticsM();

            if (moduleType != "add")
            {
                objAntibioticsM = ControlParameter.AntibioticFAMS;
            }
        }
        private void frmAntibitotic_Load(object sender, EventArgs e)
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
                    txtCode.Text = objAntibioticsM.AntibioticsCode;
                    txtName.Text = objAntibioticsM.AntibioticsName;
                    txtID.Text = Convert.ToString(objAntibioticsM.AntibioticsID);
                    txtComment.Text = objAntibioticsM.ANTIBIOTIC_FAMS_DESCRIPTION;
                    objAntibioticsM.ANTIBIOTIC_FAMS_LOGUSERID = ControlParameter.loginID.ToString();
                    Get_Antibiotic_FAMS(objAntibioticsM);

                    label6.Visible = true;
                    label6.Text = "Edit Antibiotic Family";
                    this.Text = "Edit Antibiotic Family";
                }
                else if (moduleType == "dup")
                {
                    txtCode.Select();
                    txtName.Text = objAntibioticsM.AntibioticsName;
                    txtComment.Text = objAntibioticsM.ANTIBIOTIC_FAMS_DESCRIPTION;
                    objAntibioticsM.ANTIBIOTIC_FAMS_LOGUSERID = ControlParameter.loginID.ToString();
                    Get_Antibiotic_FAMS(objAntibioticsM);

                    label6.Visible = true;
                    label6.Text = "Duplicate Antibiotic Family";
                    this.Text = "Duplicate Antibiotic Family";
                }
                else if (moduleType == "add")
                {
                    txtName.Select();
                    label6.Visible = true;
                    label6.Text = "Add Antibiotic Family";
                    this.Text = "Add Antibiotic Family";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Get_Antibiotic_FAMS(AntibioticsM objAntibioticsM)
        {
            ConfigurationController objGridAntibioticFAMS = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objGridAntibioticFAMS.GetGridAntibiotic_For_FAM(objAntibioticsM);
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

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
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

                                objAntibioticsM.AntibioticsID = Convert.ToInt32(txtID.Text);
                                objAntibioticsM.AntibioticsCode = txtCode.Text;
                                objAntibioticsM.AntibioticsName = txtName.Text;
                                objAntibioticsM.ANTIBIOTIC_FAMS_DESCRIPTION = txtComment.Text;

                                objAntibioticsM.AntibioticsLOGUSERID = ControlParameter.loginID.ToString();
                                objAntibioticsM = objConfig.SaveAntibiotic_FAMS(objAntibioticsM);

                                if (dataGridView2.Rows.Count > 0)
                                {
                                    objAntibioticsM = objConfig.ClearAntibioticFAMS_in_Antibiotic(objAntibioticsM);
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objAntibioticsM.ANTIBIOTIC_FAMS_CODE = row.Cells[0].Value.ToString();
                                        objAntibioticsM.ANTIBIOTIC_FAMS_NAME = row.Cells[1].Value.ToString();
                                        objAntibioticsM.ANTIBIOTIC_FAMS_ID = row.Cells[2].Value.ToString();


                                        objAntibioticsM = objConfig.SaveAntibioticFAMS_in_Antibiotic(objAntibioticsM);
                                    }
                                }
                                else
                                {
                                    objAntibioticsM = objConfig.ClearAntibioticFAMS_in_Antibiotic(objAntibioticsM);
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
                            objAntibioticsM.AntibioticsCode = txtCode.Text;
                            objAntibioticsM.AntibioticsName = txtName.Text;
                            objAntibioticsM.ANTIBIOTIC_FAMS_DESCRIPTION = txtComment.Text;

                            objAntibioticsM.AntibioticsLOGUSERID = ControlParameter.loginID.ToString();
                            objAntibioticsM = objConfig.SaveAntibiotic_FAMS(objAntibioticsM);

                            if (dataGridView2.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    objAntibioticsM.ANTIBIOTIC_FAMS_CODE = row.Cells[0].Value.ToString();
                                    objAntibioticsM.ANTIBIOTIC_FAMS_NAME = row.Cells[1].Value.ToString();
                                    objAntibioticsM.ANTIBIOTIC_FAMS_ID = row.Cells[2].Value.ToString();

                                    objAntibioticsM = objConfig.SaveAntibioticFAMS_in_Antibiotic(objAntibioticsM);
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
                            objAntibioticsM.AntibioticsCode = txtCode.Text;
                            objAntibioticsM.AntibioticsName = txtName.Text;
                            objAntibioticsM.ANTIBIOTIC_FAMS_DESCRIPTION = txtComment.Text;

                            objAntibioticsM.AntibioticsLOGUSERID = ControlParameter.loginID.ToString();
                            objAntibioticsM = objConfig.SaveAntibiotic_FAMS(objAntibioticsM);

                            if (dataGridView2.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    objAntibioticsM.ANTIBIOTIC_FAMS_CODE = row.Cells[0].Value.ToString();
                                    objAntibioticsM.ANTIBIOTIC_FAMS_NAME = row.Cells[1].Value.ToString();
                                    objAntibioticsM.ANTIBIOTIC_FAMS_ID = row.Cells[2].Value.ToString();

                                    objAntibioticsM = objConfig.SaveAntibioticFAMS_in_Antibiotic(objAntibioticsM);
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

        private void Btn_search_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetDICTAntibiotic_For_FAM();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objAntibioticsM.AntibioticsFamilyID = Convert.ToInt32(fm.SelectedID.ToString());
                    objAntibioticsM.AntibioticsFamilyName = fm.SelectedName;
                    objAntibioticsM.AntibioticsFamilyCode = fm.SelectedCode;

                    ANTIFAMS01 = objAntibioticsM.AntibioticsFamilyCode;
                    ANTIFAMS02 = objAntibioticsM.AntibioticsFamilyName;
                    ANTIFAMS03 = objAntibioticsM.AntibioticsFamilyID;

                    int minRowCount = 0;
                    bool Checkcode = false;
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = ANTIFAMS01;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = ANTIFAMS02;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = ANTIFAMS03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells[2].Value.ToString().Contains(ANTIFAMS03.ToString()))
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
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = ANTIFAMS01;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = ANTIFAMS02;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = ANTIFAMS03;
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
    }
}
