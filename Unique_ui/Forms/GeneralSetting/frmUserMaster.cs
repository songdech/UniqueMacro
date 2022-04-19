using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniquePro.Entities.Common;
using UniquePro.Entities.GeneralSetting;
using UniquePro.DAO.GeneralSetting;

namespace UNIQUE.GeneralSetting
{
    public partial class frmUserMaster : Form
    {
        ConnectStringM ConStrM;

        UserDAO objUserDAO = new UserDAO();
        UserM objUserM = new UserM();


        string strDataMode;
        private string strUserID;
        public Action yourAction { get; set; }

        public frmUserMaster(string type, string sUserID)
        {
            InitializeComponent();
            this.strDataMode = type;
            this.strUserID = sUserID;
        }


        private void FrmUserMaster_Load(object sender, EventArgs e)
        {
            if (strDataMode == "ADD")
            {
                strDataMode = "ADD";
                txtUserID.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtUserName.Text = "";
                txtPassword.Text = "";
                txtEMail.Text = "";
                txtPosition.Text = "";
                txtUserID.Focus();

            }
            else
            {
                strDataMode = "EDIT";
                txtUserID.Enabled = false;
                txtUserID.Text =  ControlParameter.ControlUser.USERID;
                txtFirstName.Text = ControlParameter.ControlUser.NAME;
                txtLastName.Text = ControlParameter.ControlUser.LASTNAME;
                txtUserName.Text = ControlParameter.ControlUser.USERNAME;
                txtPassword.Text = ControlParameter.ControlUser.PASSWORD;
                txtEMail.Text = ControlParameter.ControlUser.Email;
                txtTel.Text = ControlParameter.ControlUser.TELEPHON;
                txtPosition.Text = ControlParameter.ControlUser.POSITION;

            }
        }


        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }



        private void TxtUserID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtUserID.Text.Trim() != "")
                {
                    DataTable dt = objUserDAO.GetUserMasterByName(txtUserID.Text.ToUpper());
                    if (dt.Rows.Count <= 0)
                    {
                        txtFirstName.Text = "";
                        txtLastName.Text = "";
                        txtUserName.Text = "";
                        txtPassword.Text = "";
                        txtEMail.Text = "";
                        txtPosition.Text = "";
                        txtTel.Text = "";
                        txtFirstName.Focus();
                    }
                    else
                    {
                        MessageBox.Show("This menu has data already.", "GetBarcodeNew", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUserID.Text = "";
                        txtUserID.Focus();
                    }
                }
            }
        }



        private void ClearData()
        {
            strDataMode = "ADD";
            txtUserID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtEMail.Text = "";
            txtTel.Text = "";
            txtPosition.Text = "";
            txtUserID.Focus();

        }


        private Boolean BlindObject()
        {
            try
            {
                ControlParameter.ControlUser.USERID = txtUserID.Text;
                ControlParameter.ControlUser.NAME = txtFirstName.Text;
                ControlParameter.ControlUser.LASTNAME = txtLastName.Text;
                ControlParameter.ControlUser.USERNAME = txtUserName.Text;
                ControlParameter.ControlUser.PASSWORD = txtPassword.Text;
                ControlParameter.ControlUser.Email = txtEMail.Text;
                ControlParameter.ControlUser.TELEPHON = txtTel.Text;
                ControlParameter.ControlUser.POSITION = txtPosition.Text;

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Blind Object", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {

            if (BlindObject())
            {
                string StrErr = objUserDAO.SaveUser(ControlParameter.ControlUser, strDataMode);

                if (StrErr == "")
                {

                    MessageBox.Show("Save Data Complete", "Save Data Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                    Action instance = yourAction;
                    if (instance != null)
                        instance();
                }
                else
                {
                    MessageBox.Show(StrErr, "Save Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClearData();
                }
            }

        }

        private void tileNavPane1_Click(object sender, EventArgs e)
        {

        }
    }
}
