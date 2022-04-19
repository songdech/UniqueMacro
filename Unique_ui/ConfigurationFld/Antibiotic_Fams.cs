using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class Antibiotic_Fams : UserControl
    {
        private AntibioticsM objAntibioticFAMS;

        public string strModule
        {
            get
            {
                return "Antibiotic_Fams";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private ConfigurationController objConfig = new ConfigurationController();

        public Antibiotic_Fams()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "Antibiotic_Fams";
            loadData();
        }

        private void loadData()
        {
            DataTable dt = null;

            ConfigurationController objConfig = new ConfigurationController();
            try
            {
                dt = objConfig.GetAnitibiotic_FAMS(txt_Code.Text.Trim(), txt_Name.Text.Trim());

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

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                AntibioticsM objAntibioticFAMS = new AntibioticsM();
                DataGridViewRow row_1 = this.dataGridView1.Rows[e.RowIndex];

                objAntibioticFAMS.AntibioticsID = Convert.ToInt32(row_1.Cells[0].Value.ToString());
                objAntibioticFAMS.AntibioticsCode = row_1.Cells[1].Value.ToString();
                objAntibioticFAMS.AntibioticsName = row_1.Cells[2].Value.ToString();
                objAntibioticFAMS.ANTIBIOTIC_FAMS_DESCRIPTION = row_1.Cells[3].Value.ToString();
                objAntibioticFAMS.AntibioticsLOGUSERID = ControlParameter.loginID.ToString();
                ControlParameter.AntibioticFAMS = objAntibioticFAMS;
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strAntibioticFAMS_ID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddAntibiotic_FAM fm = new ConfigurationFld.frmAddAntibiotic_FAM("edit", strAntibioticFAMS_ID);
                fm.ShowDialog();
            }
        }

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Antibiotic Family จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    objAntibioticFAMS = ControlParameter.AntibioticFAMS;

                    // Antibiotic Family clear default
                    // Check and Restore ANTIBIOTICSFAMILYID in DICT_MB_ANTIBIOTIC = null
                    objConfig.AntibioticFAMS_Antibiotic_Restore(objAntibioticFAMS);
                    // Check and Delete in DICT_ANTIBIOTICS_GROUP  row > 0
                    objConfig.AntibioticFAMS_DELETE(objAntibioticFAMS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
