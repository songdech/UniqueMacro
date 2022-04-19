using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddDetectionTests : Form
    {

        private string moduleType;
        private string DetectionCustomizedID;
        private DetectionTestM objDetectionM;

        private ConfigurationController objConfiguration = new ConfigurationController();

        int rowindex = 0;

        object CUSTL01;
        object CUSTL02;
        object CUSTL03;

        string Printable = "";


        public Action refreshData { get; set; }

        public frmAddDetectionTests(string moduleType,string DetectionID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.DetectionCustomizedID = DetectionID;
            objConfiguration = new ConfigurationController();

            objDetectionM = new DetectionTestM();

            if (moduleType != "add")
            {
                objDetectionM = ControlParameter.DetectionInfoM;
            }
        }

        private void frmAddDetectionTests_Load(object sender, EventArgs e)
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
                    txtID.Text = objDetectionM.DETECTION_ID;
                    txtCode.Text = objDetectionM.DETECTION_CODE;
                    txtName.Text = objDetectionM.DETECTION_SHORTTEXT;
                    txtDescription.Text = objDetectionM.DESCRIPTION;
                    txtFulltext.Text = objDetectionM.DETECTION_FULLTEXT;
                    txtMORPHODESC.Text = objDetectionM.DETECTION_Morphodesc;

                    Printable = objDetectionM.DETECTION_PRINT;

                    if (Printable == "No")
                    {
                        checkBox_Print.Checked = false;
                    }
                    else if (Printable == "Yes")
                    {
                        checkBox_Print.Checked = true;
                    }

                    objDetectionM.DETECTION_LOGUSERID = ControlParameter.loginID.ToString();

                    Get_DetectionTests_IN_Dict_(objDetectionM);

                    label6.Visible = true;
                    label6.Text = "Edit Detection Result";
                    this.Text = "Edit Detection Result";
                }
                else if (moduleType == "dup")
                {
                    txtCode.Select();
                    txtName.Text = objDetectionM.DETECTION_SHORTTEXT;
                    txtFulltext.Text = objDetectionM.DETECTION_FULLTEXT;
                    txtDescription.Text = objDetectionM.DESCRIPTION;
                    txtMORPHODESC.Text = objDetectionM.DETECTION_Morphodesc;

                    Printable = objDetectionM.DETECTION_PRINT;

                    if (Printable == "No")
                    {
                        checkBox_Print.Checked = false;
                    }
                    else if (Printable == "Yes")
                    {
                        checkBox_Print.Checked = true;
                    }

                    objDetectionM.DETECTION_LOGUSERID = ControlParameter.loginID.ToString();

                    Get_DetectionTests_IN_Dict_(objDetectionM);

                    label6.Visible = true;
                    label6.Text = "Duplicate Detection Result";
                    this.Text = "Duplicate Detection Result";
                }
                else if (moduleType == "add")
                {
                    txtCode.Select();

                    label6.Visible = true;
                    label6.Text = "Add Detection Result";
                    this.Text = "Add Detection Result";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Get_DetectionTests_IN_Dict_(DetectionTestM objDetectionM)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objConfiguration.GetGridDetectionTest(objDetectionM);
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
                                objDetectionM.DETECTION_ID = txtID.Text;
                                objDetectionM.DETECTION_CODE = txtCode.Text;
                                objDetectionM.DETECTION_SHORTTEXT = txtName.Text;
                                objDetectionM.DETECTION_FULLTEXT = txtFulltext.Text;
                                objDetectionM.DETECTION_Morphodesc = txtMORPHODESC.Text;

                                objDetectionM.DESCRIPTION = txtDescription.Text;

                                if (checkBox_Print.Checked == true)
                                {
                                    objDetectionM.DETECTION_PRINT = "1";
                                }
                                else if (checkBox_Print.Checked == false)
                                {
                                    objDetectionM.DETECTION_PRINT = "0";
                                }

                                objDetectionM.DETECTION_LOGUSERID = ControlParameter.loginID.ToString();
                                objDetectionM = objConfiguration.SaveDetectionTests(objDetectionM);

                                if (dataGridView1.Rows.Count > 0)
                                {
                                    objDetectionM = objConfiguration.ClearDetectionTests_Customized_Result(objDetectionM);

                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objDetectionM.DETECTION_CUSTOMIZED_ID = row.Cells[2].Value.ToString();
                                        objDetectionM = objConfiguration.SaveDetectionTests_Customized_Result(objDetectionM);
                                    }
                                }
                                else
                                {
                                    objDetectionM = objConfiguration.ClearDetectionTests_Customized_Result(objDetectionM);
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

                            objDetectionM.DETECTION_CODE = txtCode.Text;
                            objDetectionM.DETECTION_SHORTTEXT = txtName.Text;
                            objDetectionM.DETECTION_FULLTEXT = txtFulltext.Text;
                            objDetectionM.DETECTION_Morphodesc = txtMORPHODESC.Text;

                            objDetectionM.DESCRIPTION = txtDescription.Text;

                            if (checkBox_Print.Checked == true)
                            {
                                objDetectionM.DETECTION_PRINT = "1";
                            }
                            else if (checkBox_Print.Checked == false)
                            {
                                objDetectionM.DETECTION_PRINT = "0";
                            }

                            objDetectionM.DETECTION_LOGUSERID = ControlParameter.loginID.ToString();
                            objDetectionM = objConfiguration.SaveDetectionTests(objDetectionM);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objDetectionM.DETECTION_CUSTOMIZED_ID = row.Cells[2].Value.ToString();
                                    objDetectionM = objConfiguration.SaveDetectionTests_Customized_Result(objDetectionM);
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
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            objDetectionM.DETECTION_CODE = txtCode.Text;
                            objDetectionM.DETECTION_SHORTTEXT = txtName.Text;
                            objDetectionM.DETECTION_FULLTEXT = txtFulltext.Text;
                            objDetectionM.DETECTION_Morphodesc = txtMORPHODESC.Text;

                            objDetectionM.DESCRIPTION = txtDescription.Text;

                            if (checkBox_Print.Checked == true)
                            {
                                objDetectionM.DETECTION_PRINT = "1";
                            }
                            else if (checkBox_Print.Checked == false)
                            {
                                objDetectionM.DETECTION_PRINT = "0";
                            }

                            objDetectionM.DETECTION_LOGUSERID = ControlParameter.loginID.ToString();
                            objDetectionM = objConfiguration.SaveDetectionTests(objDetectionM);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objDetectionM.DETECTION_CUSTOMIZED_ID = row.Cells[2].Value.ToString();
                                    objDetectionM = objConfiguration.SaveDetectionTests_Customized_Result(objDetectionM);
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
        private void btnExit_ElementClick_1(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void Btn_search_G1_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetDetection_In_dict();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objDetectionM.DETECTION_CUSTOMIZED_ID = fm.SelectedID.ToString();
                    objDetectionM.DETECTION_CUSTOMIZED_NAME = fm.SelectedName;
                    objDetectionM.DETECTION_CUSTOMIZED_CODE = fm.SelectedCode;

                    CUSTL01 = objDetectionM.DETECTION_CUSTOMIZED_CODE;
                    CUSTL02 = objDetectionM.DETECTION_CUSTOMIZED_NAME;
                    CUSTL03 = objDetectionM.DETECTION_CUSTOMIZED_ID;

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
