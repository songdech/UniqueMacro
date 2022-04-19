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

namespace UNIQUE.ConfigurationApp
{
    public partial class frmDatabaseSetting : Form
    {
        UniquePro.DAO.Configurations.ConnectionStringMasterDAO objConstring = new UniquePro.DAO.Configurations.ConnectionStringMasterDAO();

        private string moduleType;
        private string strConName;
        SqlConnection conn;


        public Action yourAction { get; set; }
        public frmDatabaseSetting(string type, string sConName)
        {
            InitializeComponent();
            this.moduleType = type;
            this.strConName = sConName;
        }

        private void TxtConName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtConName.Text.Trim() != "")
                {
                    if (!checkConName())
                    {
                        txtConPath.Text = "";
                        txtDataSource.Text = "";
                        txtCatalog.Text = "";
                        txtUsername.Text = "";
                        txtPassword.Text = "";
                        txtConPath.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Connection Name : " + txtConName.Text.Trim() + " arleady in database.", "Please use other code.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtConName.Text = "";
                        txtConName.Focus();
                    }
                }
            }
        }

        private void TxtConPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtConPath.Text.Trim() != "")
                {
                    txtDataSource.Focus();
                }
            }
        }

        private void TxtDataSource_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtDataSource.Text.Trim() != "")
                {
                    txtCatalog.Focus();
                }
            }
        }

        private void TxtCatalog_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtCatalog.Text.Trim() != "")
                {
                    txtUsername.Focus();
                }
            }
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtUsername.Text.Trim() != "")
                {
                    txtPassword.Focus();
                }
            }
        }





        private void AddLocations_Load(object sender, EventArgs e)
        {
            conn = new ConnectionString().Connect();
            if (moduleType == "EDIT")
            {
                txtConName.Text = ControlParameter.ControlConString.CONANME;
                txtConPath.Text = ControlParameter.ControlConString.CONPATH;
                txtDataSource.Text = ControlParameter.ControlConString.CONDATASOURCE;
                txtCatalog.Text = ControlParameter.ControlConString.CONCATALOG;
                txtUsername.Text = ControlParameter.ControlConString.CONUSER;
                txtPassword.Text = ControlParameter.ControlConString.CONPASS;
                this.Text = "Edit Database";
            }

            else
            {
                this.Text = "Add Database";
            }
        }

        private void BtnTestConnecttion_Click(object sender, EventArgs e)
        {
            string strTestCon = TestConnecttion();
            if (strTestCon == "")
            {
                MessageBox.Show("Connect Success", "Connect success ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(strTestCon, "Connect Faill!!! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            if (txtConName.Text != "")
            {
                string strTestCon = TestConnecttion();
                if (strTestCon == "")
                {
                    if (BlindObject())
                    {
                        if (moduleType == "ADD")
                        {

                            if (!checkConName())
                            {
                                string StrErr = objConstring.SaveDatabaseSetting(objConstring, moduleType);
                                if (StrErr == "")
                                {
                                    MessageBox.Show("Save Data Complete", "Save Data Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    ConnectStringM.StrDataSource = objConstring.CONDATASOURCE;
                                    ConnectStringM.StrCatalog = objConstring.CONCATALOG;
                                    ConnectStringM.StrUser = objConstring.CONUSER;
                                    ConnectStringM.StrPassword = objConstring.CONPASS;
                                    ConnectStringM.ClientDBSet = "PASS";

                                    this.Close();
                                    Action instance = yourAction;
                                    if (instance != null)
                                        instance();
                                }
                                else
                                {
                                    MessageBox.Show(StrErr, "Save Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                            }
                            else
                            {
                                MessageBox.Show("Connection Name : " + txtConName.Text.Trim() + " arleady in database.", "Please use other code.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }
                        else if (moduleType == "EDIT")
                        {

                            string StrErr = objConstring.SaveDatabaseSetting(objConstring, moduleType);
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

                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show(strTestCon, "Connect Faill!!! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }


        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }




        private Boolean BlindObject()
        {
            try
            {
                objConstring.CONANME = txtConName.Text;
                objConstring.CONPATH = txtConPath.Text;
                objConstring.CONDATASOURCE = txtDataSource.Text;
                objConstring.CONCATALOG = txtCatalog.Text;
                objConstring.CONUSER = txtUsername.Text;
                objConstring.CONPASS = txtPassword.Text;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Blind Object", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        private bool checkConName()
        {
            DataTable dt = objConstring.GetDBConstringByName(txtConName.Text);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string TestConnecttion()
        {
            try
            {
                //string strTestCon = @"Data Source=" + txtDataSource.Text + " ;Initial Catalog=" + txtCatalog.Text + ";User ID=" + txtUsername.Text + ";Password=" + txtPassword.Text;
                //string strTestCon = @"Data Source=" + txtDataSource.Text + " ;Initial Catalog=" + txtCatalog.Text + ";User ID=" + txtUsername.Text + ";Password=" + txtPassword.Text;

                string strTestCon = @"Data Source=" + txtDataSource.Text.Trim () + ";Initial Catalog=" + txtCatalog.Text.Trim() + ";Persist Security Info=True;User ID=" + txtUsername.Text.Trim() + ";Password=" + txtPassword.Text.Trim() ;

                using (var connection = new SqlConnection(strTestCon))
                {
                    connection.Open();
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

       

    }
}
