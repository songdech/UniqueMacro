using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmCustomize_RESULT : Form
    {

        private string moduleType;
        private SystemController objSystemControl;
        private CustomResultM objcustomResultM;

        private ConfigurationController objConfig;

        int rowindex = 0;
        object CUSTL01;
        object CUSTL02;
        object CUSTL03;

        public Action refreshData { get; set; }

        public frmCustomize_RESULT(string moduleType,string CustomizedID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            objConfig = new ConfigurationController();

            objcustomResultM = new CustomResultM();

            if (moduleType != "add")
            {
                objcustomResultM = ControlParameter.CustomResulM;
            }
        }

        private void frmCustomize_RESULT_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (moduleType == "edit")
                {
                        txtCode.Text = objcustomResultM.CUSTOMIZED_RES_CODE;
                        txtName.Text = objcustomResultM.CUSTOMIZED_RES_TEXT;
                        txtID.Text = objcustomResultM.CUSTOMIZED_RES_ID;
                        txtComment.Text = objcustomResultM.DESCRIPTION_RES_;

                        objcustomResultM.LOGUSERID_RES_ = ControlParameter.loginID.ToString();
                        Get_CustomizedResult_(objcustomResultM);

                        label6.Visible = true;
                        label6.Text = "Edit Customized Result";
                        this.Text = "Edit Customized Result";
                }
                else if (moduleType == "dup")
                {
                        txtCode.Select();
                        txtName.Text = objcustomResultM.CUSTOMIZED_RES_TEXT;

                        objcustomResultM.LOGUSERID_RES_ = ControlParameter.loginID.ToString();
                        Get_CustomizedResult_(objcustomResultM);

                        label6.Visible = true;
                        label6.Text = "Duplicate Customized Result";
                        this.Text = "Duplicate Customized Result";
                }
                else if (moduleType == "add")
                {
                    txtName.Select();
                    label6.Visible = true;
                    label6.Text = "Add Customized Result";
                    this.Text = "Add Customized Result";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Get_CustomizedResult_(CustomResultM objcustomResultM)
        {
            ConfigurationController objGridCustomizedResult = new ConfigurationController();
            DataTable dt = null;

            try
            {
                dt = objGridCustomizedResult.GetGridCutomizedResult(objcustomResultM);
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

        bool IsNumbericOnly(string strProtocol)
        {
            foreach (char A in strProtocol)
            {
                if (A < '0' || A > '9')
                    return false;
            }
            return true;
        }

        private void btnExit_ElementClick_1(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
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
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            try
                            {
                                txtCode.Enabled = false;

                                objcustomResultM.CUSTOMIZED_RES_ID = txtID.Text;
                                objcustomResultM.CUSTOMIZED_RES_CODE = txtCode.Text;
                                objcustomResultM.CUSTOMIZED_RES_TEXT = txtName.Text;
                                objcustomResultM.DESCRIPTION_RES_ = txtComment.Text;

                                objcustomResultM.LOGUSERID_RES_ = ControlParameter.loginID.ToString();
                                objcustomResultM = objConfig.SaveCustomized_Result(objcustomResultM);

                                if (dataGridView2.Rows.Count > 0)
                                {
                                    objcustomResultM = objConfig.DeleteCustomized_Result(objcustomResultM);
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        objcustomResultM.CUSTOMIZED_CODE = row.Cells[0].Value.ToString();
                                        objcustomResultM.CUSTOMIZED_TEXT = row.Cells[1].Value.ToString();
                                        objcustomResultM.CUSTOMIZED_ID = row.Cells[2].Value.ToString();

                                        objcustomResultM = objConfig.SaveCustomizedList_In_DICT_RESULT_LIST(objcustomResultM);
                                    }
                                }
                                else
                                {
                                    objcustomResultM = objConfig.DeleteCustomized_Result(objcustomResultM);
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
                            objcustomResultM.CUSTOMIZED_RES_CODE = txtCode.Text;
                            objcustomResultM.CUSTOMIZED_RES_TEXT = txtName.Text;
                            objcustomResultM.DESCRIPTION_RES_ = txtComment.Text;

                            objcustomResultM.LOGUSERID_RES_ = ControlParameter.loginID.ToString();
                            objcustomResultM = objConfig.SaveCustomized_Result(objcustomResultM);

                            if (dataGridView2.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    objcustomResultM.CUSTOMIZED_CODE = row.Cells[0].Value.ToString();
                                    objcustomResultM.CUSTOMIZED_TEXT = row.Cells[1].Value.ToString();
                                    objcustomResultM.CUSTOMIZED_ID = row.Cells[2].Value.ToString();

                                    objcustomResultM = objConfig.SaveCustomizedList_In_DICT_RESULT_LIST(objcustomResultM);
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
                            objcustomResultM.CUSTOMIZED_RES_CODE = txtCode.Text;
                            objcustomResultM.CUSTOMIZED_RES_TEXT = txtName.Text;
                            objcustomResultM.DESCRIPTION_RES_ = txtComment.Text;
                            // Add SCOPE IDENT before Save
                            objcustomResultM.LOGUSERID_RES_ = ControlParameter.loginID.ToString();
                            objcustomResultM = objConfig.SaveCustomized_Result(objcustomResultM);

                            if (dataGridView2.Rows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dataGridView2.Rows)
                                {
                                    objcustomResultM.CUSTOMIZED_CODE = row.Cells[0].Value.ToString();
                                    objcustomResultM.CUSTOMIZED_TEXT = row.Cells[1].Value.ToString();
                                    objcustomResultM.CUSTOMIZED_ID = row.Cells[2].Value.ToString();

                                    objcustomResultM = objConfig.SaveCustomizedList_In_DICT_RESULT_LIST(objcustomResultM);
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
                dt = objConfiguration.GetCustomizedList();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objcustomResultM.CUSTOMIZED_LIST_ID = fm.SelectedID.ToString();
                    objcustomResultM.CUSTOMIZED_LIST_NAME = fm.SelectedName;
                    objcustomResultM.CUSTOMIZED_LIST_CODE = fm.SelectedCode;

                    CUSTL01 = objcustomResultM.CUSTOMIZED_LIST_CODE;
                    CUSTL02 = objcustomResultM.CUSTOMIZED_LIST_NAME;
                    CUSTL03 = objcustomResultM.CUSTOMIZED_LIST_ID;

                    int minRowCount = 0;
                    bool CheckCustomizedResult = false;
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = CUSTL01;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = CUSTL02;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = CUSTL03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells[2].Value.ToString()==(CUSTL03.ToString()))
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
                            dataGridView2.Rows.Add();
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = CUSTL01;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = CUSTL02;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = CUSTL03;
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

        private void button2_Click_1(object sender, EventArgs e)
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
    }
}
