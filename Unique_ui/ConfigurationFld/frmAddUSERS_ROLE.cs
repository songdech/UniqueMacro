using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.GeneralSetting;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddUSERS_ROLE : Form
    {

        object ROLE02;   // Name
        object ROLE03;   // ID

        private string moduleType;
        private string ROLEID;

        private ConfigurationController objConfig = new ConfigurationController();
        private UserM objUserM;

        public Action refreshData { get; set; }
        public frmAddUSERS_ROLE(string moduleType, string RoleID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.ROLEID = RoleID;
            objUserM = new UserM();
            if (moduleType != "add")
            {
                objUserM = ControlParameter.UserInfoM;
            }
            
        }
        private void frmAddUSERS_ROLE_Load(object sender, EventArgs e)
        {
            Loaddata();
        }

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                if (moduleType == "edit")
                {
                    if (txtName.Text != "")
                    {
                        objUserM.ROLEID = txtID.Text;
                        objUserM.ROLENAME = txtName.Text;
                        objUserM.DESCRIPTION = txtDescription.Text;

                        objUserM.LogUserID = ControlParameter.loginID.ToString();

                        objUserM = objConfig.SaveRole(objUserM);
                        // DataGrid

                        if (dataGridView1.Rows.Count > 0)
                        {
                            objUserM = objConfig.DeleteRole(objUserM);

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                objUserM.FUNC_ALIAS_NAME = row.Cells[0].Value.ToString();
                                objUserM.FUNC_ALIAS = row.Cells[1].Value.ToString();        // Use combox index
                                objUserM = objConfig.SaveRole_Rights(objUserM);
                            }
                        }
                        else if (dataGridView1.Rows.Count <= 0)
                        {
                            objUserM = objConfig.DeleteRole(objUserM);
                        }
                    }
                }
                else if (moduleType == "dup")
                {
                    if (txtName.Text != "")
                    {
                        objUserM.ROLENAME = txtName.Text;
                        objUserM.DESCRIPTION = txtDescription.Text;

                        objUserM.LogUserID = ControlParameter.loginID.ToString();

                        objUserM = objConfig.SaveRole(objUserM);

                        // DataGrid Specimen Group

                        if (dataGridView1.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                objUserM.FUNC_ALIAS_NAME = row.Cells[0].Value.ToString();
                                objUserM.FUNC_ALIAS = row.Cells[1].Value.ToString();        // Use combox index
                                objUserM = objConfig.SaveRole_Rights(objUserM);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please insert data?");
                    }
                }
                else if (moduleType == "add")
                {
                    if (txtName.Text != "")
                    {
                        objUserM.ROLENAME = txtName.Text;
                        objUserM.DESCRIPTION = txtDescription.Text;
                        objUserM.LogUserID = ControlParameter.loginID.ToString();

                        objUserM = objConfig.SaveRole(objUserM);

                        // DataGrid Specimen Group

                        if (dataGridView1.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                objUserM.FUNC_ALIAS_NAME = row.Cells[0].Value.ToString();
                                objUserM.FUNC_ALIAS = row.Cells[1].Value.ToString();        // Use combox index
                                objUserM = objConfig.SaveRole_Rights(objUserM);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please insert data?");
                    }

                }

                this.Close();
                Action instance = refreshData;
                if (instance != null)
                    instance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
        }

        private void Loaddata()
        {
            if (moduleType == "edit")
            {
                txtID.Text = objUserM.ROLEID;
                txtName.Text = objUserM.ROLENAME;
                txtDescription.Text = objUserM.DESCRIPTION;

                objUserM.ROLEID = ControlUser_Role.ROLEID;

                Get_Role(objUserM);

                label6.Visible = true;
                label6.Text = "Edit Role";
                this.Text = "Edit Role";

            }
            else if (moduleType == "dup")
            {
                txtName.Text = objUserM.ROLENAME;
                txtDescription.Text = objUserM.DESCRIPTION;

                objUserM.ROLEID = ControlUser_Role.ROLEID;

                label6.Visible = true;
                label6.Text = "Duplicate Role";
                this.Text = "Duplicate Role";
            }

            else if (moduleType == "add")
            {
                txtName.Text = objUserM.ROLENAME;
                txtDescription.Text = objUserM.DESCRIPTION;

                label6.Visible = true;
                label6.Text = "Add Role";
                this.Text = "Add Role";
            }
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void Get_Role (UserM objUserM)
        {

            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;

            try
            {
                dt = objConfig.GetRole_In_USERFUNCTION(objUserM);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objUserM = null;
            }
        }

        private void Btn_Clear_G4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void Btn_Addrights_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text != "")
            {
                objUserM.FUNCNAME = comboBox1.Text;
                objUserM.FUNCTIONID = Convert.ToString(comboBox1.SelectedIndex);

                ROLE02 = objUserM.FUNCNAME;
                ROLE03 = objUserM.FUNCTIONID;

                int minRowCount = 0;
                bool Checkcode = false;

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    minRowCount++;
                }

                if (minRowCount == 0)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = ROLE02;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = ROLE03;

                }
                else
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[1].Value.ToString().Contains(ROLE03.ToString()))
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
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = ROLE02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = ROLE03;
                    }
                }
            }

        }
    }
}
