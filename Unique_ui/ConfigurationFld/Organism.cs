using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class Organism : UserControl
    {
        private OrganismM objOrganismM;

        public string strModule
        {
            get
            {
                return "organism";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private ConfigurationController objConfig = new ConfigurationController();

        public Organism()
        {
            InitializeComponent();
        }

        private OrganismController objOraganism = new OrganismController();

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "organism";
            loadData();
        }

        private void loadData()
        {
            DataTable dt = null;

            ConfigurationController objConfig = new ConfigurationController();
            try
            {
                dt = objConfig.GetOrganisms(txt_Code.Text.Trim(), txt_Name.Text.Trim());

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

                OrganismM objOrganisM = new OrganismM();
                DataGridViewRow row_1 = this.dataGridView1.Rows[e.RowIndex];

                objOrganisM.OrganismID = Convert.ToInt32(row_1.Cells[0].Value.ToString());
                objOrganisM.OrganismCode = row_1.Cells[1].Value.ToString();
                objOrganisM.OrganismName = row_1.Cells[2].Value.ToString();
                objOrganisM.Organism_MEDTHOD_CODE = row_1.Cells[3].Value.ToString();
                objOrganisM.Organism_MEDTHOD_NAME = row_1.Cells[4].Value.ToString();
                objOrganisM.MorphoDesc = row_1.Cells[7].Value.ToString();

                objOrganisM.Organism_LOGUSERID = ControlParameter.loginID.ToString();
                ControlParameter.OrganismM = objOrganisM;
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strOrganismID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddOrganism fm = new ConfigurationFld.frmAddOrganism("edit", strOrganismID);
                fm.ShowDialog();
            }
        }

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Organisms List จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    objOrganismM = ControlParameter.OrganismM;

                    // Family clear
                    // Check and Restore MICID in DICT_MB_ORGANISMS = 0
                    //objOrganismM = objConfig.BreakPoint_ORGANISM_Restore(objOrganismM);
                    // Sensitivity Panel clear
                    // Check and Delete in DICT_ANTIMICROBIAL_BREAKPOINT_LIST  row > 0
                    //objConfig.BreakPoint_DELETE_LIST(objOrganismM);
                    // BreakPoint clear
                    // Check and Delete in DICT_ANTIMICROBIAL_BREAKPOINT  row > 0
                    //objOrganismM = objConfig.BreakPoint_DELETE(objOrganismM);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
