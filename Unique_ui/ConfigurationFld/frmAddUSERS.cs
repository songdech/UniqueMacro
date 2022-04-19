using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.GeneralSetting;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddUSERS : Form
    {

        object USER02;   //USER Name
        object USER03;   //USER ID

        private string moduleType;
        private string USERID;

        private static AES128_EncryptAndDecrypt AES128 = new AES128_EncryptAndDecrypt();

        private ConfigurationController objConfig = new ConfigurationController();
        private UserM objUserM;

        public Action refreshData { get; set; }
        public frmAddUSERS(string moduleType, string UserID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.USERID = UserID;
            objUserM = new UserM();
            if (moduleType != "add")
            {
                objUserM = ControlParameter.UserInfoM;                
            }
            
        }
        private void frmAddUSERS_Load(object sender, EventArgs e)
        {
            Loaddata();
        }

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                if (moduleType == "edit")
                {
                    if (txtPass1.Text != "" && txtPass2.Text != "")
                    {

                        objUserM.USERSID = Convert.ToInt16(txtID.Text);
                        objUserM.NAME = txtName.Text;
                        objUserM.LASTNAME = txtLastname.Text;
                        objUserM.QUALIFICATION = txtQualification.Text;
                        objUserM.CERTIFICATE = txtCer.Text;
                        objUserM.PASSWORD = AES128.Encrypt(txtPass2.Text);
                        objUserM.EMAIL = txtEmail.Text;
                        objUserM.TELEPHON = txtTel.Text;
                        objUserM.POSITION = txtPosition.Text;
                        objUserM.DESCRIPTION = txtDescription.Text;
                        objUserM.USERTIMEOUT = txtTimeout.Text;

                        objUserM.LogUserID = ControlParameter.loginID.ToString();
                        objUserM = objConfig.SaveUser(objUserM);
                        if (dataGridView1.Rows.Count > 0)
                        {
                            objUserM = objConfig.DeleteRoleInUser(objUserM);
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                objUserM.ROLEID = row.Cells[1].Value.ToString();
                                objUserM = objConfig.SaveRole_in_USER_ROLE(objUserM);
                            }
                        }
                        else if (dataGridView1.Rows.Count <= 0)
                        {
                            objUserM = objConfig.DeleteRoleInUser(objUserM);
                        }
                    }
                    else
                    {
                        objUserM.USERSID = Convert.ToInt16(txtID.Text);
                        objUserM.NAME = txtName.Text;
                        objUserM.LASTNAME = txtLastname.Text;
                        objUserM.QUALIFICATION = txtQualification.Text;
                        objUserM.CERTIFICATE = txtCer.Text;
                        objUserM.EMAIL = txtEmail.Text;
                        objUserM.TELEPHON = txtTel.Text;
                        objUserM.POSITION = txtPosition.Text;
                        objUserM.DESCRIPTION = txtDescription.Text;
                        objUserM.USERTIMEOUT = txtTimeout.Text;

                        objUserM.LogUserID = ControlParameter.loginID.ToString();
                        objUserM = objConfig.SaveUser(objUserM);
                        if (dataGridView1.Rows.Count > 0)
                        {
                            objUserM = objConfig.DeleteRoleInUser(objUserM);
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                objUserM.ROLEID = row.Cells[1].Value.ToString();
                                objUserM = objConfig.SaveRole_in_USER_ROLE(objUserM);
                            }
                        }
                        else if (dataGridView1.Rows.Count <= 0)
                        {
                            objUserM = objConfig.DeleteRoleInUser(objUserM);
                        }
                    }
                }
                else if (moduleType == "dup")
                {
                    objUserM.USERNAME = txtUsername.Text;
                    objUserM.NAME = txtName.Text;
                    objUserM.LASTNAME = txtLastname.Text;
                    objUserM.QUALIFICATION = txtQualification.Text;
                    objUserM.CERTIFICATE = txtCer.Text;
                    objUserM.PASSWORD = AES128.Encrypt(txtPass2.Text);
                    objUserM.EMAIL = txtEmail.Text;
                    objUserM.TELEPHON = txtTel.Text;
                    objUserM.POSITION = txtPosition.Text;
                    objUserM.DESCRIPTION = txtDescription.Text;
                    objUserM.USERTIMEOUT = txtTimeout.Text;

                    objUserM.LogUserID = ControlParameter.loginID.ToString();

                    objUserM = objConfig.SaveUser(objUserM);

                    // DataGrid Specimen Group

                    if (dataGridView1.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            objUserM.ROLEID = row.Cells[1].Value.ToString();
                            objUserM = objConfig.SaveRole_in_USER_ROLE(objUserM);
                        }
                    }
                }
                else if (moduleType == "add")
                {
                    objUserM.USERNAME = txtUsername.Text;
                    objUserM.NAME = txtName.Text;
                    objUserM.LASTNAME = txtLastname.Text;
                    objUserM.QUALIFICATION = txtQualification.Text;
                    objUserM.CERTIFICATE = txtCer.Text;
                    objUserM.PASSWORD = AES128.Encrypt(txtPass2.Text);
                    objUserM.EMAIL = txtEmail.Text;
                    objUserM.TELEPHON = txtTel.Text;
                    objUserM.POSITION = txtPosition.Text;
                    objUserM.DESCRIPTION = txtDescription.Text;
                    objUserM.USERTIMEOUT = txtTimeout.Text;

                    objUserM.LogUserID = ControlParameter.loginID.ToString();

                    objUserM = objConfig.SaveUser(objUserM);

                    // DataGrid Specimen Group
                    if (dataGridView1.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            objUserM.ROLEID = row.Cells[1].Value.ToString();
                            objUserM = objConfig.SaveRole_in_USER_ROLE(objUserM);
                        }
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
                txtID.Text = Convert.ToString(objUserM.USERSID);

                txtUsername.Text = objUserM.USERNAME;
                txtName.Text = objUserM.NAME;
                txtLastname.Text = objUserM.LASTNAME;
                txtQualification.Text = objUserM.QUALIFICATION;
                txtCer.Text = objUserM.CERTIFICATE;
                txtEmail.Text = objUserM.EMAIL;
                txtTel.Text = objUserM.TELEPHON;
                txtPosition.Text = objUserM.POSITION;
                txtDescription.Text = objUserM.DESCRIPTION;
                txtTimeout.Text = objUserM.USERTIMEOUT;

                objUserM.USERID = ControlUser_Role.USERID;

                Get_UserRole(objUserM);

                label6.Visible = true;
                label6.Text = "Edit User";
                this.Text = "Edit User";

            }
            else if (moduleType == "dup")
            {
                txtName.Text = objUserM.NAME;
                txtLastname.Text = objUserM.LASTNAME;
                txtQualification.Text = objUserM.QUALIFICATION;
                txtCer.Text = objUserM.CERTIFICATE;
                txtEmail.Text = objUserM.EMAIL;
                txtTel.Text = objUserM.TELEPHON;
                txtDescription.Text = objUserM.DESCRIPTION;
                txtTimeout.Text = objUserM.USERTIMEOUT;

                objUserM.USERID = ControlUser_Role.USERID;

                label6.Visible = true;
                label6.Text = "Duplicate User";
                this.Text = "Duplicate User";
            }

            else if (moduleType == "add")
            {
                label6.Visible = true;
                label6.Text = "Add User";
                this.Text = "Add User";
            }
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void Get_UserRole(UserM objUserM)
        {

            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;

            try
            {
                dt = objConfig.GetRole_In_USER_ROLE_LIST(objUserM);

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

        private void Btn_search_G1_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();
            UserM objUserM = new UserM();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetRoleLookup(objUserM);

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objUserM.ROLEID = fm.SelectedID.ToString();
                    objUserM.ROLENAME = fm.SelectedName;

                    USER02 = objUserM.ROLENAME;
                    USER03 = objUserM.ROLEID;

                    int minRowCount = 0;
                    bool Checkcode = false;

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = USER02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = USER03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[1].Value.ToString().Contains(USER03.ToString()))
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
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = USER02;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = USER03;
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

        private void Btn_Clear_G4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtPass1.Text == txtPass2.Text)
            {
                pictureBox1.Visible = true;
            }
            else
            {
                pictureBox1.Visible = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (txtPass2.PasswordChar == '*')
                {
                    txtPass2.PasswordChar = '\0';
                }
            }
            else
            {
                if (txtPass2.PasswordChar == '\0')
                {
                    txtPass2.PasswordChar = '*';
                }
            }
        }

        private void txtPass1_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtPass1.Text == txtPass2.Text)
            {
                pictureBox1.Visible = true;
            }
            else
            {
                pictureBox1.Visible = false;
            }
        }
    }
}
