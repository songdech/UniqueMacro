using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class Antibitotic : UserControl
    {
        
        private ConfigurationController objConfiguration = new ConfigurationController();

        private AntibioticsM objAntibiotics = new AntibioticsM();

        public Antibitotic()
        {
            InitializeComponent();
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        public string strModule
        {
            get
            {
                return "antibiotic";
            }
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "antibiotic";
            LoadData();
        }

        private void LoadData()
        {
            DataTable dt;

            try
            {
                dt = objConfiguration.GetDictAniboticsData(txtSearchCode.Text, txtSearchName.Text);

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
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    objAntibiotics.AntibioticsID = Convert.ToInt16(row.Cells[0].Value.ToString());
                    objAntibiotics.AntibioticsCode = row.Cells[1].Value.ToString();
                    objAntibiotics.AntibioticsName = row.Cells[2].Value.ToString();
                    objAntibiotics.Antibiotics_DESCRIPTION = row.Cells[3].Value.ToString();
                    objAntibiotics.Antibiotics_NOTPRINTABLE = row.Cells[4].Value.ToString();
                    ControlAntibiotic.AntibioticID = Convert.ToString(objAntibiotics.AntibioticsID);
                    ControlParameter.AntibioticM = objAntibiotics;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message); 
            }

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strANtibioticID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddAntibiotic fm = new ConfigurationFld.frmAddAntibiotic("edit", strANtibioticID);
                fm.ShowDialog();
            }
        }

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Antibiotic จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    DataTable dt = null;
                    objAntibiotics = ControlParameter.AntibioticM;

                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CHECK DATA IN SUBREQMB_ANTIBIOTICS
                    //
                    dt = objConfig.Check_Result_in_SUBREQMB_ANTIBIOTICS(objAntibiotics);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Result in database", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // STEP CLEAR & DELETE
                        // -DICT_ANTIBIOTICS_LIST, ------> this for SENSITIVITIES PANEL
                        // -DICT_ANTIMICROBIAL_BREAKPOINT_LISTS, -----> this for BreakPoint
                        // -SUBREQMB_ANTIBIOTICS, ----> this for result

                        // Relate with DICT_MB_ANTIBIOTICS
                        // 1. DICT_ANTIBIOTICS_LIST   ----> DELETE
                        // 2. DICT_ANTIMICROBIAL_BREAKPOINT_LISTS         ----> DELETE
                        // 3. DICT_ANTIBIOTICS ----> DELETE

                        // 
                        // Check and Delete DICT_ANTIBIOTICS_LIST = Delete
                        objConfig.Delete_ANTIBIOTIC_in_DICT_ANTIBIOTICS_LIST(objAntibiotics);
                        // Check and Delete in DICT_ANTIMICROBIAL_BREAKPOINT_LISTS = Delete
                        objConfig.Delete_ANTIBIOTIC_in_DICT_ANTIMICROBIAL_BREAKPOINT_LISTS(objAntibiotics);
                        // Check and Delete in DICT_MB_STAINS = Delete
                        objConfig.Delete_ANTIBIOTIC_in_DICT_MB_ANTIBIOTICS(objAntibiotics);
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
