using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddExpertRule : Form
    {
        private string moduleType;
        private string ExpertID;

        private ExpertM objExpertM;
        private ConfigurationController objConfiguration = new ConfigurationController();

        int rowindex = 0;
        object ANTIBIOTIC01;
        object ANTIBIOTIC02;
        object ANTIBIOTIC03;

        string Printable = "";

        public frmAddExpertRule(string moduleType, string ExpertID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.ExpertID = ExpertID;

            objExpertM = ControlParameter.ExpertRuleM;

            if (moduleType != "add")
            {
                objExpertM = ControlParameter.ExpertRuleM;
            }

        }

        public Action refreshData { get; set; }
        private void frmAddExpertRule_Load(object sender, EventArgs e)
        {            
            LoadData();
        }
        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }
        private void LoadData()
        {
            try
            {
                if (moduleType == "edit")
                {
                    txtCode.Text = objExpertM.EXPERT_RULE_Code;
                    txtName.Text = objExpertM.EXPERT_RULE_Name;
                    txtID.Text = objExpertM.EXPERT_RULE_ID;
                    txtComment.Text = objExpertM.EXPERT_RULE__DESCRIPTION;
                    objExpertM.EXPERT_RULE_LOGUSERID = ControlParameter.loginID.ToString();
                    Printable = objExpertM.EXPERT_RULE_ACTIVE;

                    if (Printable == "1")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }

                    //Get_Antibiotic_Family(objAntibiotic);

                    label9.Visible = true;
                    label9.Text = "Edit Expert rule";
                    this.Text = "Edit Expert rule";
                }
                else if (moduleType == "dup")
                {
                    txtCode.Select();
                    txtName.Text = objExpertM.EXPERT_RULE_Name;
                    txtComment.Text = objExpertM.EXPERT_RULE__DESCRIPTION;
                    objExpertM.EXPERT_RULE_LOGUSERID = ControlParameter.loginID.ToString();
                    Printable = objExpertM.EXPERT_RULE_ACTIVE;

                    if (Printable == "1")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }

                    //Get_Antibiotic_Family(objAntibiotic);

                    label9.Visible = true;
                    label9.Text = "Duplicate Expert rule";
                    this.Text = "Duplicate Expert rule";
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
                    label9.Text = "Add Expert rule";
                    this.Text = "Add Expert rule";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void Get_Antibiotic_Family(AntibioticsM objAntibiotic)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objConfiguration.GetAntibiotic_Family(objAntibiotic);
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

                                objExpertM.EXPERT_RULE_ID = txtID.Text;
                                objExpertM.EXPERT_RULE_Code = txtCode.Text;
                                objExpertM.EXPERT_RULE_Name = txtName.Text;
                                objExpertM.EXPERT_RULE__DESCRIPTION = txtComment.Text;
                                objExpertM.EXPERT_RULE_ACTIVE = Printable;
                                objExpertM.EXPERT_RULE_LOGUSERID = ControlParameter.loginID.ToString();
                                //objExpertM = objConfiguration.SaveAntibiotic(objAntibiotic);

                                if (dataGridView1.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objExpertM.EXPERT_RULE_ID = row.Cells[2].Value.ToString();
                                        //objExpertM = objConfiguration.SaveFamily_in_Antibiotics(objAntibiotic);
                                    }
                                }
                                else
                                {
                                    //objExpertM = objConfiguration.ClearFamily_in_DICT_MB_ANTIBIOTICS(objAntibiotic);
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
                            objExpertM.EXPERT_RULE_Code = txtCode.Text;
                            objExpertM.EXPERT_RULE_Name = txtName.Text;
                            objExpertM.EXPERT_RULE__DESCRIPTION = txtComment.Text;
                            objExpertM.EXPERT_RULE_ACTIVE = Printable;
                            objExpertM.EXPERT_RULE_LOGUSERID = ControlParameter.loginID.ToString();
                            //objExpertM = objConfiguration.SaveAntibiotic(objAntibiotic);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objExpertM.EXPERT_RULE_Code = row.Cells[0].Value.ToString();
                                    //objExpertM = objConfiguration.SaveFamily_in_Antibiotics(objAntibiotic);
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
                            objExpertM.EXPERT_RULE_Code = txtCode.Text;
                            objExpertM.EXPERT_RULE_Name = txtName.Text;
                            objExpertM.EXPERT_RULE__DESCRIPTION = txtComment.Text;
                            objExpertM.EXPERT_RULE_ACTIVE = Printable;
                            objExpertM.EXPERT_RULE_LOGUSERID = ControlParameter.loginID.ToString();
                            //objExpertM = objConfiguration.SaveAntibiotic(objAntibiotic);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objExpertM.EXPERT_RULE_ID = row.Cells[2].Value.ToString();
                                    //objExpertM = objConfiguration.SaveFamily_in_Antibiotics(objAntibiotic);
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

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
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

