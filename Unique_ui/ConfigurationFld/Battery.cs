using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;


namespace UNIQUE.ConfigurationFld
{
    public partial class Battery : UserControl
    {

        BatteryM objBatteryM = null;

        public Battery ()
        {
            InitializeComponent();
        }

        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }
 
        private void Battery_Load(object sender, EventArgs e)
        {
            objBatteryM = new BatteryM();

            LoadData();
            Action instance2 = checkEnableButton;
            if (instance2 != null)
                instance2();
        }

        public string strModule
        {
            get
            {
                return "Battery";
            }
        }
        private void LoadData()
        {

            ConfigurationController objConfigBatteryM = new ConfigurationController();
            DataTable dt = null;
            BatteryM objBatteryM = new BatteryM();

            try
            {
                objBatteryM.BATTERY_CODE = txtCode.Text;
                objBatteryM.BATTERY_SHORTNAME = txtName.Text;

                dt = objConfigBatteryM.GetBattery(txtCode.Text.Trim(), txtName.Text.Trim());

                dataGridView2.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();
                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objBatteryM = null;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BatteryM objBatteryM = new BatteryM();

                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                objBatteryM.BATTERY_ID = Convert.ToInt16(row.Cells[0].Value.ToString());
                objBatteryM.BATTERY_CODE = row.Cells[1].Value.ToString();
                objBatteryM.BATTERY_SHORTNAME = row.Cells[2].Value.ToString();
                objBatteryM.BATTERY_NAME = row.Cells[3].Value.ToString();
                objBatteryM.BATTERY_COMMENT = row.Cells[4].Value.ToString();
                objBatteryM.BATTER_CREDATE = Convert.ToDateTime(row.Cells[5].Value.ToString());
                objBatteryM.BATTERY_USERID = row.Cells[6].Value.ToString();
                objBatteryM.BATTERY_LOGDATE = Convert.ToDateTime(row.Cells[7].Value.ToString());

                objBatteryM.MAPCODE_LINKID = Convert.ToInt16(row.Cells[8].Value.ToString());
                objBatteryM.LogUserID = ControlParameter.ControlUser.USERID.ToString();

                ControlParameter.BatteryInfoM = objBatteryM;
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                string strBatteryID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddBattery fm = new ConfigurationFld.frmAddBattery("edit", strBatteryID);
                fm.refreshData = LoadData;
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Battery จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    DataTable dt = null;
                    objBatteryM = ControlParameter.BatteryInfoM;

                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CHECK DATA IN SUBREQMB_BATTERIES
                    //
                    // STEP CLEAR & DELETE
                    // - Check result in 

                    dt = objConfig.Check_Result_in_SUBREQMB_BATTERIES(objBatteryM);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Result in database", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else
                    {
                        MessageBox.Show("Process delete");
                        // 1. DELETE CODES_MAPPING --> MAPLINKID
                        // 2. DELETE CODES_MAPPING_LINK --> MAPLINKID
                        // 3. DELETE DICT_MB_BATERIES

                        objBatteryM = objConfig.Delete_Battery_Step1(objBatteryM);
                        objBatteryM = objConfig.Delete_MAPPINGLINK(objBatteryM);
                        objConfig.Delete_DICT_MB_BATTERIES(objBatteryM);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
