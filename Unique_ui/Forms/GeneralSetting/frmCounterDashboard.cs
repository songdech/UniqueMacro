using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using UniquePro.Common;
using UniquePro.Controller;
using UniquePro.Entities.Common;
using UniquePro.Entities.Configuration;

namespace UNIQUE.Forms.GeneralSetting
{
    public partial class frmCounterDashboard : Form
    {
        ConnectString objConstr = new ConnectString();

        private ConfigurationController objConfig;
        private DashBoardM objDashboardM;

        private string moduleType;
        private string strDASHBID;

        public Action yourAction { get; set; }
        public frmCounterDashboard(string moduletype, string StrDASHBID)
        {
            InitializeComponent();
            this.moduleType = moduletype;
            this.strDASHBID = StrDASHBID;

            objConfig = new ConfigurationController();
            objDashboardM = new DashBoardM();

            if (moduleType != "add")
            {
                objDashboardM = ControlParameter.DashboardM;
            }
        }

        private void frmCounterDashboard_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                if (moduleType == "edit")
                {
                    txtDASHBID.Text = Convert.ToString(objDashboardM.DASHBOARD_ID);
                    txtDASHNAME.Text = objDashboardM.DASHBOARD_NAME;

                    if (objDashboardM.DASHBOARD_ENABLE == "TRUE")
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox1.SelectedIndex = 1;
                    }

                    txtDASHBTIME.Text = objDashboardM.DASHBOARD_TIME;
                    txtDescription.Text = objDashboardM.DASHBOARD_DESCRIPTION;

                    objDashboardM.DASHBOARD_LOGUSERID = ControlParameter.loginID.ToString();

                    label6.Visible = true;
                    label6.Text = "Edit Dashboard MGNT";
                    this.Text = "Edit Dashboard MGNT";
                }
                else if (moduleType == "dup")
                {
                    txtDASHBID.Select();
                    txtDASHNAME.Text = objDashboardM.DASHBOARD_NAME;

                    if (objDashboardM.DASHBOARD_ENABLE == "TRUE")
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox1.SelectedIndex = 1;
                    }

                    txtDescription.Text = objDashboardM.DASHBOARD_DESCRIPTION;

                    objDashboardM.DASHBOARD_LOGUSERID = ControlParameter.loginID.ToString();

                    label6.Visible = true;
                    label6.Text = "Duplicate Dashboard MGNT";
                    this.Text = "Duplicate Dashboard MGNT";
                }
                else if (moduleType == "add")
                {
                    txtDASHBID.Select();

                    label6.Visible = true;
                    label6.Text = "Add Dashboard MGNT";
                    this.Text = "Add Dashboard MGNT";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
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
                        if (txtDASHNAME.Text != "")
                        {
                            try
                            {
                                txtDASHNAME.Enabled = false;

                                objDashboardM.DASHBOARD_ID = Convert.ToInt32(txtDASHBID.Text);
                                objDashboardM.DASHBOARD_NAME = txtDASHNAME.Text;
                                objDashboardM.DASHBOARD_ENABLE = comboBox1.Text;
                                objDashboardM.DASHBOARD_TIME = txtDASHBTIME.Text;
                                objDashboardM.DASHBOARD_DESCRIPTION = txtDescription.Text;

                                objDashboardM.DASHBOARD_LOGUSERID = ControlParameter.loginID.ToString();
                                objDashboardM = objConfig.SaveDashBoard(objDashboardM);

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
                        if (txtDASHNAME.Text != "")
                        {
                            objDashboardM.DASHBOARD_NAME = txtDASHNAME.Text;
                            objDashboardM.DASHBOARD_ENABLE = comboBox1.Text;
                            objDashboardM.DASHBOARD_TIME = txtDASHBTIME.Text;
                            objDashboardM.DASHBOARD_DESCRIPTION = txtDescription.Text;

                            objDashboardM.DASHBOARD_LOGUSERID = ControlParameter.loginID.ToString();
                            objDashboardM = objConfig.SaveDashBoard(objDashboardM);

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
                    objDashboardM = new DashBoardM();
                    try
                    {
                        if (txtDASHNAME.Text != "")
                        {
                            objDashboardM.DASHBOARD_NAME = txtDASHNAME.Text;
                            objDashboardM.DASHBOARD_ENABLE = comboBox1.Text;
                            objDashboardM.DASHBOARD_TIME = txtDASHBTIME.Text;
                            objDashboardM.DASHBOARD_DESCRIPTION = txtDescription.Text;

                            //objDashboardM.DASHBOARD_LOGUSERID = ControlParameter.loginID.ToString();
                            objDashboardM = objConfig.SaveDashBoard(objDashboardM);

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
    }
}
