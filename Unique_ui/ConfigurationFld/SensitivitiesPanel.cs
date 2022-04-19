using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;


namespace UNIQUE.ConfigurationFld
{
    public partial class SensitivitiesPanel : UserControl
    {
        private SenpanelM ObjSenPanelM;

        private ConfigurationController objConfig = new ConfigurationController();

        public SensitivitiesPanel()
        {
            InitializeComponent();
        }

        public string strModule
        {
            get
            {
                return "sensitivitypanel";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }      

        private void loadData()
        {
            try
            {
                ConfigurationController objSensitivityM = new ConfigurationController();
                DataTable dt = null;

                dt = objSensitivityM.GetDICTSensitivityPanel(txtSearchCode.Text.Trim(),txtSearchName.Text.Trim());

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
                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                clsControlsData.currentForm = "sensitivitypanel";
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    SenpanelM ObjSenPanelM = new SenpanelM();
                    DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                    ObjSenPanelM.SENPANEL_ID = row.Cells[0].Value.ToString();
                    ObjSenPanelM.SENPANEL_CODE = row.Cells[1].Value.ToString();
                    ObjSenPanelM.SENPANEL_SHORTTEXT = row.Cells[2].Value.ToString();
                    ObjSenPanelM.SENPANEL_FULLTEXT = row.Cells[3].Value.ToString();
                    ObjSenPanelM.SENPANEL_METHOD = row.Cells[4].Value.ToString();

                    ObjSenPanelM.SENPANEL_LOGUSERID = ControlParameter.loginID.ToString();
                    ControlParameter.SenPanelM = ObjSenPanelM;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                string strSensitivityID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddSensitivityPanel fm = new ConfigurationFld.frmAddSensitivityPanel("edit", strSensitivityID);
                //fm.refreshData = loadData;
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
                DialogResult yes = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Sensitivity Panel และ Organism จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (yes == DialogResult.Yes)
                {
                    ObjSenPanelM = ControlParameter.SenPanelM;

                    // Sensitivity clear in DICT_MB_ORGANISMS
                    // Check and Restore SENSITIVITYID DICT_MB_ORGANISMS = null
                    objConfig.SenPanel_Organism_Restore(ObjSenPanelM);
                    // DICT_ANTIBIOTICS_LIST clear
                    // Check and Delete in DICT_ANTIBIOTICS_LIST  row > 0
                    objConfig.SenPanel_ANTIBIOIC_DELETE(ObjSenPanelM);
                    // Delete DICT_MD_SENSITIVITIES_PANEL
                    // Check and Delete in DICT_ANTIMICROBIAL_BREAKPOINT  row > 0
                    objConfig.Sensitivity_DELETE(ObjSenPanelM);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
