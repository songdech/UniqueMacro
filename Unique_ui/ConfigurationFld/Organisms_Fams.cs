using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class Organisms_Fams : UserControl
    {
        private OrganismM objOrganismFAMS;
        private ConfigurationController objConfig = new ConfigurationController();

        public string strModule
        {
            get
            {
                return "Organisms_Fams";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }


        public Organisms_Fams()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "Organisms_Fams";
            loadData();
        }

        private void loadData()
        {
            DataTable dt = null;

            ConfigurationController objConfig = new ConfigurationController();
            try
            {
                dt = objConfig.GetOrganisms_FAMS(txt_Code.Text.Trim(), txt_Name.Text.Trim());

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
                OrganismM objOrganismFAMS = new OrganismM();
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                objOrganismFAMS.OrganismID = Convert.ToInt32(row.Cells[0].Value.ToString());
                objOrganismFAMS.OrganismCode = row.Cells[1].Value.ToString();
                objOrganismFAMS.OrganismName = row.Cells[2].Value.ToString();
                objOrganismFAMS.Organism_DESCRIPTION = row.Cells[3].Value.ToString();
                objOrganismFAMS.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                ControlOrganismsFam.OrganismsFamID = Convert.ToString(objOrganismFAMS.OrganismID);
                ControlParameter.OrganismFAMS = objOrganismFAMS;
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strOrganismFAMS_ID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddOrganism_FAM fm = new ConfigurationFld.frmAddOrganism_FAM("edit", strOrganismFAMS_ID);
                fm.ShowDialog();
            }
        }

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Organisms Family จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    objOrganismFAMS = ControlParameter.OrganismFAMS;

                    // Family clear
                    // Check and Restore ORGANISMSFAMID in DICT_MB_ORGANISMS = null
                    objConfig.OrganismsFAMS_Antibiotic_Restore(objOrganismFAMS);
                    // Check and Delete in DICT_MB_ORGANISM_FAMILY  row > 0
                    objConfig.OrganismsFAMS_DELETE(objOrganismFAMS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
