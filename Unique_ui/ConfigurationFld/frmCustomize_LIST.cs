using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmCustomize_LIST : Form
    {

        private string moduleType;
        private CustomResultM objcustomResultM;
        private ConfigurationController objConfiguration = new ConfigurationController();

        int rowindex = 0;

        object CUSTL01;
        object CUSTL02;
        object CUSTL03;

        public Action refreshData { get; set; }

        public frmCustomize_LIST(string moduleType,string CustomizedID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            objcustomResultM = new CustomResultM();
            if (moduleType != "add")
            {
                objcustomResultM = ControlParameter.CustomResulM;
            }
        }

        private void frmCustomize_LIST_Load(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "Customized_List";

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (moduleType == "edit")
                {
                    txtID.Text = objcustomResultM.CUSTOMIZED_ID;
                    txtCODE.Text = objcustomResultM.CUSTOMIZED_CODE;
                    txtName.Text = objcustomResultM.CUSTOMIZED_TEXT;
                    txtFullText.Text = objcustomResultM.CUSTOMIZED_FULLTEXT;
                    txtDescription.Text = objcustomResultM.DESCRIPTION;
                    txtType.Text = objcustomResultM.CUSTOMIZED_TEXTTYPE;
                    comboBox1.Text = objcustomResultM.CUSTOMIZED_LIST_CLASS;

                    Get_CustomizedResult_(objcustomResultM);

                    label6.Visible = true;
                    label6.Text = "Edit Customized List";
                    this.Text = "Edit Customized List";
                }
                else if (moduleType == "dup")
                {
                    txtCODE.Select();

                    txtName.Text = objcustomResultM.CUSTOMIZED_TEXT;
                    txtFullText.Text = objcustomResultM.CUSTOMIZED_FULLTEXT;
                    txtDescription.Text = objcustomResultM.DESCRIPTION;
                    txtType.Text = objcustomResultM.CUSTOMIZED_TEXTTYPE;
                    comboBox1.Text = objcustomResultM.CUSTOMIZED_LIST_CLASS;

                    Get_CustomizedResult_(objcustomResultM);

                    label6.Visible = true;
                    label6.Text = "Duplicate Customized List";
                    this.Text = "Duplicate Customized List";
                }
                else if (moduleType == "add")
                {
                    txtCODE.Select();

                    label6.Visible = true;
                    label6.Text = "Add Customized List";
                    this.Text = "Add Customized List";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Get_CustomizedResult_(CustomResultM objcustomResultM)
        {
            ConfigurationController objGridCustomizedResult = new ConfigurationController();
            DataTable dt = null;

            try
            {
                dt = objGridCustomizedResult.GetGridCutomizedList(objcustomResultM);
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
                        if (txtCODE.Text != "" || txtName.Text != "" || comboBox1.Text !="")
                        {

                            txtID.Enabled = false;

                            objcustomResultM.CUSTOMIZED_ID = txtID.Text;
                            objcustomResultM.CUSTOMIZED_CODE = txtCODE.Text;
                            objcustomResultM.CUSTOMIZED_TEXT = txtName.Text;
                            objcustomResultM.CUSTOMIZED_FULLTEXT = txtFullText.Text;
                            objcustomResultM.DESCRIPTION = txtDescription.Text;
                            objcustomResultM.CUSTOMIZED_TEXTTYPE = txtType.Text;
                            objcustomResultM.CUSTOMIZED_LIST_CLASS = comboBox1.Text;

                            objcustomResultM.LOGUSERID = ControlParameter.loginID.ToString();
                            objcustomResultM = objConfiguration.SaveCustomized_List(objcustomResultM);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                objcustomResultM = objConfiguration.DeleteCustomized_Lists_in_DICT_CUS_RESULT_LIST(objcustomResultM);
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objcustomResultM.CUSTOMIZED_RES_CODE = row.Cells[0].Value.ToString();
                                    objcustomResultM.CUSTOMIZED_RES_TEXT = row.Cells[1].Value.ToString();
                                    objcustomResultM.CUSTOMIZED_RES_ID = row.Cells[2].Value.ToString();

                                    objcustomResultM = objConfiguration.SaveCustomized_Lists_In_DICT_RESULT_LIST(objcustomResultM);
                                }
                            }
                            else
                            {
                                objcustomResultM = objConfiguration.DeleteCustomized_Lists_in_DICT_CUS_RESULT_LIST(objcustomResultM);
                            }

                        }
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (moduleType == "add")
                {
                    try
                    {
                        if (txtCODE.Text != "" || txtName.Text != "" || comboBox1.Text != "")
                        {
                            objcustomResultM.CUSTOMIZED_CODE = txtCODE.Text;
                            objcustomResultM.CUSTOMIZED_TEXT = txtName.Text;
                            objcustomResultM.CUSTOMIZED_FULLTEXT = txtFullText.Text;
                            objcustomResultM.DESCRIPTION = txtDescription.Text;
                            objcustomResultM.CUSTOMIZED_TEXTTYPE = txtType.Text;
                            objcustomResultM.CUSTOMIZED_LIST_CLASS = comboBox1.Text;

                            objcustomResultM.LOGUSERID = ControlParameter.loginID.ToString();
                            objcustomResultM = objConfiguration.SaveCustomized_List(objcustomResultM);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objcustomResultM.CUSTOMIZED_RES_CODE = row.Cells[0].Value.ToString();
                                    objcustomResultM.CUSTOMIZED_RES_TEXT = row.Cells[1].Value.ToString();
                                    objcustomResultM.CUSTOMIZED_RES_ID = row.Cells[2].Value.ToString();

                                    objcustomResultM = objConfiguration.SaveCustomized_Lists_In_DICT_RESULT_LIST(objcustomResultM);
                                }
                            }

                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (moduleType == "dup")
                {
                    try
                    {
                        if (txtCODE.Text != "" || txtName.Text != "" || comboBox1.Text != "")
                        {
                            objcustomResultM.CUSTOMIZED_CODE = txtCODE.Text;
                            objcustomResultM.CUSTOMIZED_TEXT = txtName.Text;
                            objcustomResultM.CUSTOMIZED_FULLTEXT = txtFullText.Text;
                            objcustomResultM.DESCRIPTION = txtDescription.Text;
                            objcustomResultM.CUSTOMIZED_TEXTTYPE = txtType.Text;
                            objcustomResultM.CUSTOMIZED_LIST_CLASS = comboBox1.Text;

                            objcustomResultM.LOGUSERID = ControlParameter.loginID.ToString();
                            objcustomResultM = objConfiguration.SaveCustomized_List(objcustomResultM);

                            if (dataGridView1.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    objcustomResultM.CUSTOMIZED_RES_CODE = row.Cells[0].Value.ToString();
                                    objcustomResultM.CUSTOMIZED_RES_TEXT = row.Cells[1].Value.ToString();
                                    objcustomResultM.CUSTOMIZED_RES_ID = row.Cells[2].Value.ToString();
                                    objcustomResultM = objConfiguration.SaveCustomized_Lists_In_DICT_RESULT_LIST(objcustomResultM);
                                }
                            }

                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void Btn_search_G1_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetCustomizedLookup_in_DICT_CUS_RESULTS();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objcustomResultM.CUSTOMIZED_RES_ID = fm.SelectedID.ToString();
                    objcustomResultM.CUSTOMIZED_RES_TEXT = fm.SelectedName;
                    objcustomResultM.CUSTOMIZED_RES_CODE = fm.SelectedCode;

                    CUSTL01 = objcustomResultM.CUSTOMIZED_RES_CODE;
                    CUSTL02 = objcustomResultM.CUSTOMIZED_RES_TEXT;
                    CUSTL03 = objcustomResultM.CUSTOMIZED_RES_ID;

                    int minRowCount = 0;
                    bool CheckCustomizedResult = false;
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
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[2].Value.ToString() == (CUSTL03.ToString()))
                            {
                                CheckCustomizedResult = true;
                            }
                        }
                        if (CheckCustomizedResult == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckCustomizedResult == false)
                        {
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = CUSTL01;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = CUSTL02;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = CUSTL03;
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
