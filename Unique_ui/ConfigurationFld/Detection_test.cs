using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class Detection_test : UserControl
    {
        private ConfigurationController objConfig = new ConfigurationController();
        private DetectionTestM objDetectionM = new DetectionTestM();

        public string strModule
        {
            get
            {
                return "DetectionTests";
            }
        }

        public Detection_test()
        {
            InitializeComponent();
        }
        private void simpleBtn_Search_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "DetectionTests";
            loadData();
        }

        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private void loadData()
        {
            DataTable dt = null;

            ConfigurationController objConfig = new ConfigurationController();
            try
            {
                dt = objConfig.GetDICTDectionTests(txtSearchCode.Text.Trim(), txtSearchName.Text.Trim());

                if (dt.Rows.Count > 0)
                {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();

                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();

                    simpleBtn_Delete.Visible = true;
                }
                else
                {
                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();
                }

                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DetectionTestM objDetectionM = new DetectionTestM();
                    DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                    objDetectionM.DETECTION_ID = row.Cells[0].Value.ToString();
                    objDetectionM.DETECTION_CODE = row.Cells[1].Value.ToString();
                    objDetectionM.DETECTION_SHORTTEXT = row.Cells[2].Value.ToString();
                    objDetectionM.DETECTION_FULLTEXT = row.Cells[3].Value.ToString();
                    objDetectionM.DESCRIPTION = row.Cells[4].Value.ToString();
                    objDetectionM.DETECTION_PRINT = row.Cells[5].Value.ToString();
                    objDetectionM.DETECTION_Morphodesc = row.Cells[8].Value.ToString();

                    ControlDetectionTest.DETECTION_ID = objDetectionM.DETECTION_ID;
                    ControlParameter.DetectionInfoM = objDetectionM;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                string strDetectionID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddDetectionTests fm = new ConfigurationFld.frmAddDetectionTests("edit", strDetectionID);
                fm.ShowDialog();
            }
        }

        private void txtSearchCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadData();
            }
        }

        private void txtSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadData();
            }
        }

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Detection Tests,Result type, จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    DataTable dt = null;
                    objDetectionM = ControlParameter.DetectionInfoM;

                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CHECK DATA IN SUBREQMB_DET_TESTS
                    //
                    dt = objConfig.Check_Result_in_SUBREQMB_DET_TESTS(objDetectionM);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Result in database", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else
                    {
                        // 1. DICT_MB_DETECT_TESTS ----> DELETE 
                        // DICT_MB_DETECT_TESTS
                        // Check and Delete DICT_MB_DETECT_TESTS
                        objConfig.Delete_DICT_MB_DETECT_TESTS(objDetectionM);
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
