using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddOrganism_FAM : Form
    {

        private string moduleType;
        private OrganismM objOrganismM;

        private ConfigurationController objConfig;
        int rowindex = 0;
        object ORGFAMS01;
        object ORGFAMS02;
        object ORGFAMS03;

        public Action refreshData { get; set; }

        public frmAddOrganism_FAM(string moduleType, string antiID)
        {
            InitializeComponent();
            this.moduleType = moduleType;

            objConfig = new ConfigurationController();
            objOrganismM = new OrganismM();

            if (moduleType != "add")
            {
                objOrganismM = ControlParameter.OrganismFAMS;
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
                    txtCode.Text = objOrganismM.OrganismCode;
                    txtName.Text = objOrganismM.OrganismName;
                    txtID.Text = Convert.ToString(objOrganismM.OrganismID);
                    txtComment.Text = objOrganismM.Organism_DESCRIPTION;
                    objOrganismM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                    Get_Organisms_FAMS(objOrganismM);

                    label6.Visible = true;
                    label6.Text = "Edit Organisms Family";
                    this.Text = "Edit Organisms Family";
                }
                else if (moduleType == "dup")
                {
                    txtCode.Select();
                    txtName.Text = objOrganismM.OrganismName;
                    txtComment.Text = objOrganismM.Organism_DESCRIPTION;
                    objOrganismM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                    Get_Organisms_FAMS(objOrganismM);

                    label6.Visible = true;
                    label6.Text = "Duplicate Organisms Family";
                    this.Text = "Duplicate Organisms Family";
                }
                else if (moduleType == "add")
                {
                    txtName.Select();
                    label6.Visible = true;
                    label6.Text = "Add Organisms Family";
                    this.Text = "Add Organisms Family";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Get_Organisms_FAMS(OrganismM objOrganismM)
        {
            ConfigurationController objOrganismFAMS = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objOrganismFAMS.GetGridOrganisms_For_FAM(objOrganismM);
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

                                objOrganismM.OrganismID = Convert.ToInt32(txtID.Text);
                                objOrganismM.OrganismCode = txtCode.Text;
                                objOrganismM.OrganismName = txtName.Text;
                                objOrganismM.Organism_DESCRIPTION = txtComment.Text;

                                objOrganismM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                                objOrganismM = objConfig.SaveOrganisms_FAMS(objOrganismM);

                                if (dataGridView2.Rows.Count > 0)
                                {
                                    objOrganismM = objConfig.ClearOrganismsFAMS_in_Organisms(objOrganismM);
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objOrganismM.Organism_FAMS_CODE = row.Cells[0].Value.ToString();
                                        objOrganismM.Organism_FAMS_NAME = row.Cells[1].Value.ToString();
                                        objOrganismM.Organism_FAMS_ID = row.Cells[2].Value.ToString();


                                        objOrganismM = objConfig.SaveOrganismsFAMS_in_Organisms(objOrganismM);
                                    }
                                }
                                else
                                {
                                    objOrganismM = objConfig.ClearOrganismsFAMS_in_Organisms(objOrganismM);
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
                            objOrganismM.OrganismCode = txtCode.Text;
                            objOrganismM.OrganismName = txtName.Text;
                            objOrganismM.Organism_DESCRIPTION = txtComment.Text;

                            objOrganismM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                            objOrganismM = objConfig.SaveOrganisms_FAMS(objOrganismM);

                            if (dataGridView2.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    objOrganismM.Organism_FAMS_CODE = row.Cells[0].Value.ToString();
                                    objOrganismM.Organism_FAMS_NAME = row.Cells[1].Value.ToString();
                                    objOrganismM.Organism_FAMS_ID = row.Cells[2].Value.ToString();

                                    objOrganismM = objConfig.SaveOrganismsFAMS_in_Organisms(objOrganismM);
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
                            objOrganismM.OrganismCode = txtCode.Text;
                            objOrganismM.OrganismName = txtName.Text;
                            objOrganismM.Organism_DESCRIPTION = txtComment.Text;

                            objOrganismM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                            objOrganismM = objConfig.SaveOrganisms_FAMS(objOrganismM);

                            if (dataGridView2.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    objOrganismM.Organism_FAMS_CODE = row.Cells[0].Value.ToString();
                                    objOrganismM.Organism_FAMS_NAME = row.Cells[1].Value.ToString();
                                    objOrganismM.Organism_FAMS_ID = row.Cells[2].Value.ToString();

                                    objOrganismM = objConfig.SaveOrganismsFAMS_in_Organisms(objOrganismM);
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
                dt = objConfiguration.GetDICTOrganisms_For_FAM();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objOrganismM.OrganismFamilyId = fm.SelectedID.ToString();
                    objOrganismM.OrganismFamilyName = fm.SelectedName;
                    objOrganismM.OrganismFamilyCode = fm.SelectedCode;

                    ORGFAMS01 = objOrganismM.OrganismFamilyCode;
                    ORGFAMS02 = objOrganismM.OrganismFamilyName;
                    ORGFAMS03 = objOrganismM.OrganismFamilyId;

                    int minRowCount = 0;
                    bool Checkcode = false;
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = ORGFAMS01;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = ORGFAMS02;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = ORGFAMS03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells[2].Value.ToString().Contains(ORGFAMS03.ToString()))
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
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = ORGFAMS01;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = ORGFAMS02;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = ORGFAMS03;
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
