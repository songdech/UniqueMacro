using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class Expert_rule : UserControl
    {
        private ExpertM objExpertM;

        public string strModule
        {
            get
            {
                return "Expert_rule";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private ConfigurationController objConfig = new ConfigurationController();

        public Expert_rule()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "Expert_rule";
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

                ExpertM objExpertM = new ExpertM();
                DataGridViewRow row_1 = this.dataGridView1.Rows[e.RowIndex];

                objExpertM.EXPERT_RULE_ID = row_1.Cells[0].Value.ToString();
                objExpertM.EXPERT_RULE_Code = row_1.Cells[1].Value.ToString();
                objExpertM.EXPERT_RULE_Name = row_1.Cells[2].Value.ToString();
                objExpertM.EXPERT_RULE__DESCRIPTION = row_1.Cells[3].Value.ToString();
                objExpertM.EXPERT_RULE_LOGUSERID = ControlParameter.loginID.ToString();
                ControlParameter.ExpertRuleM = objExpertM;
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strExpert_ID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddExpertRule fm = new ConfigurationFld.frmAddExpertRule("edit", strExpert_ID);
                fm.ShowDialog();
            }
        }

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Expert_rule จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    //objExpertM = ControlParameter.ExpertRuleM;
                    //objConfig.AntibioticFAMS_Antibiotic_Restore(objAntibioticFAMS);
                    //objConfig.AntibioticFAMS_DELETE(objAntibioticFAMS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
