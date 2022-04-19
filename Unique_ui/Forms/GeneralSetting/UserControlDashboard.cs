using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Entities.Common;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.Forms.GeneralSetting
{
    public partial class UserControlDashboard : UserControl
    {
        ConnectString objConstr = new ConnectString();
        private ConfigurationController objConfig = new ConfigurationController();

        DashBoardM objDashBoardM = new DashBoardM();

        public UserControlDashboard()
        {
            InitializeComponent();
        }

        public string strModule
        {
            get
            {
                return "UserControlDashboard";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "UserControlDashboard";
            LoadData();
        }

        private void LoadData()
        {
            DataTable dt = null;

            ConfigurationController objConfig = new ConfigurationController();
            try
            {
                dt = objConfig.Get_DashBoard(txtSearchCode.Text.Trim(), txtSearchName.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();

                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();
                }
                else
                {
                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();
                }

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row_1 = this.dataGridView1.Rows[e.RowIndex];

                    objDashBoardM.DASHBOARD_ID = Convert.ToInt16(row_1.Cells[1].Value.ToString());
                    objDashBoardM.DASHBOARD_NAME = row_1.Cells[2].Value.ToString();
                    objDashBoardM.DASHBOARD_ENABLE = row_1.Cells[3].Value.ToString();
                    objDashBoardM.DASHBOARD_TIME = row_1.Cells[4].Value.ToString();
                    objDashBoardM.DASHBOARD_DESCRIPTION = row_1.Cells[5].Value.ToString();

                    objDashBoardM.DASHBOARD_LOGUSERID = ControlParameter.loginID.ToString();

                    ControlDashBoardItem.DASHBOARD_ID = Convert.ToString(objDashBoardM.DASHBOARD_ID);
                    ControlParameter.DashboardM = objDashBoardM;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strDashBoardID = row.Cells[0].Value.ToString();
                frmCounterDashboard fm = new frmCounterDashboard("edit", strDashBoardID);
                fm.ShowDialog();
            }
        }

        private void txtSearchCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }

        private void txtSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }
    }
}
