using System;
using System.Data;
using System.Windows.Forms;
using UNIQUE.Result;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddAntibiotic : Form
    {
        private string moduleType;
        private string AntiID;

        private AntibioticsM objAntibiotic;
        private ConfigurationController objConfiguration = new ConfigurationController();

        int rowindex = 0;
        object ANTIBIOTIC01;
        object ANTIBIOTIC02;
        object ANTIBIOTIC03;

        string Printable = "";

        public frmAddAntibiotic(string moduleType, string AntiID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.AntiID = AntiID;

            objAntibiotic = ControlParameter.AntibioticM;

            if (moduleType != "add")
            {
                objAntibiotic = ControlParameter.AntibioticM;
            }

        }

        public Action refreshData { get; set; }
        private void frmAddAntibiotic_Load(object sender, EventArgs e)
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
                    txtCode.Text = objAntibiotic.AntibioticsCode;
                    txtName.Text = objAntibiotic.AntibioticsName;
                    txtID.Text = Convert.ToString(objAntibiotic.AntibioticsID);
                    txtComment.Text = objAntibiotic.Antibiotics_DESCRIPTION;
                    objAntibiotic.AntibioticsLOGUSERID = ControlParameter.loginID.ToString();
                    Printable = objAntibiotic.Antibiotics_NOTPRINTABLE;

                    if (Printable == "1")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }

                    Get_Antibiotic_Family(objAntibiotic);

                    label9.Visible = true;
                    label9.Text = "Edit Antibiotic";
                    this.Text = "Edit Antibiotic";
                }
                else if (moduleType == "dup")
                {
                    txtCode.Select();
                    txtName.Text = objAntibiotic.AntibioticsName;
                    txtComment.Text = objAntibiotic.Antibiotics_DESCRIPTION;
                    objAntibiotic.AntibioticsLOGUSERID = ControlParameter.loginID.ToString();
                    Printable = objAntibiotic.Antibiotics_NOTPRINTABLE;

                    if (Printable == "1")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }

                    Get_Antibiotic_Family(objAntibiotic);

                    label9.Visible = true;
                    label9.Text = "Duplicate Antibiotic";
                    this.Text = "Duplicate Antibiotic";
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
                    label9.Text = "Add Antibiotic";
                    this.Text = "Add Antibiotic";
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

                                objAntibiotic.AntibioticsID = Convert.ToInt32(txtID.Text);
                                objAntibiotic.AntibioticsCode = txtCode.Text;
                                objAntibiotic.AntibioticsName = txtName.Text;
                                objAntibiotic.Antibiotics_DESCRIPTION = txtComment.Text;
                                objAntibiotic.Antibiotics_NOTPRINTABLE = Printable;
                                objAntibiotic.AntibioticsLOGUSERID = ControlParameter.loginID.ToString();
                                objAntibiotic = objConfiguration.SaveAntibiotic(objAntibiotic);

                                if (dataGridView1.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objAntibiotic.ANTIBIOTIC_FAMS_ID = row.Cells[2].Value.ToString();
                                        objAntibiotic = objConfiguration.SaveFamily_in_Antibiotics(objAntibiotic);
                                    }
                                }
                                else
                                {
                                    objAntibiotic = objConfiguration.ClearFamily_in_DICT_MB_ANTIBIOTICS(objAntibiotic);
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
                            objAntibiotic.AntibioticsCode = txtCode.Text;
                            objAntibiotic.AntibioticsName = txtName.Text;
                            objAntibiotic.Antibiotics_DESCRIPTION = txtComment.Text;
                            objAntibiotic.Antibiotics_NOTPRINTABLE = Printable;
                            objAntibiotic.AntibioticsLOGUSERID = ControlParameter.loginID.ToString();
                            objAntibiotic = objConfiguration.SaveAntibiotic(objAntibiotic);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objAntibiotic.AntibioticsCode = row.Cells[0].Value.ToString();
                                    objAntibiotic = objConfiguration.SaveFamily_in_Antibiotics(objAntibiotic);
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
                            objAntibiotic.AntibioticsCode = txtCode.Text;
                            objAntibiotic.AntibioticsName = txtName.Text;
                            objAntibiotic.Antibiotics_DESCRIPTION = txtComment.Text;
                            objAntibiotic.Antibiotics_NOTPRINTABLE = Printable;
                            objAntibiotic.AntibioticsLOGUSERID = ControlParameter.loginID.ToString();
                            objAntibiotic = objConfiguration.SaveAntibiotic(objAntibiotic);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objAntibiotic.ANTIBIOTIC_FAMS_ID = row.Cells[2].Value.ToString();
                                    objAntibiotic = objConfiguration.SaveFamily_in_Antibiotics(objAntibiotic);
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
                dt = objConfiguration.GetFamily_in_DICT_MB_ANTIBIOTIC_GROUP_FOR_ANTIBIOTICS();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objAntibiotic.ANTIBIOTIC_FAMS_ID = fm.SelectedID.ToString();
                    objAntibiotic.ANTIBIOTIC_FAMS_NAME = fm.SelectedName;
                    objAntibiotic.ANTIBIOTIC_FAMS_CODE = fm.SelectedCode;

                    ANTIBIOTIC01 = objAntibiotic.ANTIBIOTIC_FAMS_CODE;
                    ANTIBIOTIC02 = objAntibiotic.ANTIBIOTIC_FAMS_NAME;
                    ANTIBIOTIC03 = objAntibiotic.ANTIBIOTIC_FAMS_ID;

                    int minRowCount = 0;

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = ANTIBIOTIC01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = ANTIBIOTIC02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = ANTIBIOTIC03;
                    }
                    else
                    {
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = ANTIBIOTIC01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = ANTIBIOTIC02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = ANTIBIOTIC03;
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

