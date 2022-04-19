using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using UNIQUE.GeneralSetting;

namespace UNIQUE.ConfigurationApp
{
    public partial class frmConfigurationApp : Form
    {
        SqlConnection conn;
        string strModule = "";
        public frmConfigurationApp()
        {
            InitializeComponent();
        }
 

        private void frmConfigurationMain_Load(object sender, EventArgs e)
        {
            conn = new ConnectionString().Connect();            
        }

        private void checkEnableButton()
        {
            if (clsControlsData.currentForm == null)
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnRefresh.Enabled = false;
                btnDupplicate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else if (clsControlsData.currentForm != null)
            {
                btnAdd.Enabled = true;               
            }
        }

        private void enableDuplicateButton()
        {
            btnDupplicate.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnRefresh.Enabled = true;
        }

        public Action yourAction { get; set; }

        private void refreshForm()
        {
            if (clsControlsData.currentForm == "DataBaseSeeting")
            {
                ConfigurationApp.DBSetting fm = new DBSetting();
                fm.Dock = DockStyle.Fill;
                strModule = fm.strModule;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "USerMaster")
            {
                GeneralSetting.UserMaster fm = new UserMaster();
                fm.Dock = DockStyle.Fill;
                strModule = fm.strModule;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            //USerCounterControl
            else if (clsControlsData.currentForm == "UserControlDashboard")
            {
                Forms.GeneralSetting.UserControlDashboard fm = new Forms.GeneralSetting.UserControlDashboard();
                fm.Dock = DockStyle.Fill;
                strModule = fm.strModule;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }


        }
     
        private void frmConfigurationMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsControlsData.currentForm = null;
        }


        private void BtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (clsControlsData.currentForm == "DataBaseSeeting")
            {
                ConfigurationApp.frmDatabaseSetting fm = new ConfigurationApp.frmDatabaseSetting("ADD", "");
                fm.yourAction = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "USerMaster")
            {
                GeneralSetting.frmUserMaster fm = new GeneralSetting.frmUserMaster("ADD", "");
                fm.yourAction = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "UserControlDashboard")
            {
                Forms.GeneralSetting.frmCounterDashboard fm = new Forms.GeneralSetting.frmCounterDashboard("add", "");
                fm.yourAction = refreshForm;
                fm.ShowDialog();
            }

        }

        private void BtnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (clsControlsData.currentForm == "DataBaseSeeting")
            {
                ConfigurationApp.frmDatabaseSetting fm = new ConfigurationApp.frmDatabaseSetting("ADD", "");
                fm.yourAction = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "USerMaster")
            {
                GeneralSetting.frmUserMaster fm = new GeneralSetting.frmUserMaster("ADD", "");
                fm.yourAction = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "UserControlDashboard")
            {
                Forms.GeneralSetting.frmCounterDashboard fm = new Forms.GeneralSetting.frmCounterDashboard("EDIT", "");
                fm.yourAction = refreshForm;
                fm.ShowDialog();
            }

        }

        private void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            refreshForm();
        }

        private void BtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (clsControlsData.currentForm == "USerMaster")
            {
                string strUserName = ControlParameter.ControlUser.USERNAME;

                DialogResult yes = MessageBox.Show("Do you want to delete User : " + strUserName + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    string sql = "DELETE FROM USERS WHERE USERS.USERID = '" + ControlParameter.ControlUser.USERID + "'";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    cmd.ExecuteNonQuery();

                    refreshForm();
                }
                else if (clsControlsData.currentForm == "UserControlDashboard")
                {
                    MessageBox.Show("Delete Not Allow", "", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }

            }
        }

        private void BtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void NavDatabase_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationApp.DBSetting fm = new ConfigurationApp.DBSetting();

            clsControlsData.currentForm = "DataBaseSeeting";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);

        }

        private void NavUserMaster_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            GeneralSetting.UserMaster fm = new GeneralSetting.UserMaster();

            clsControlsData.currentForm = "USerMaster";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navBarItem_counter_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Forms.GeneralSetting.UserControlDashboard fm = new Forms.GeneralSetting.UserControlDashboard();

            clsControlsData.currentForm = "UserControlDashboard";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);

        }
    }
}
