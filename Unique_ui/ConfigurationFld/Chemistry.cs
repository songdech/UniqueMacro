using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class Chemistry : UserControl
    {
        private ConfigurationController objConfig = new ConfigurationController();
        private ChemistryM objchemistryM = new ChemistryM();

        public Chemistry()
        {
            InitializeComponent();
        }

        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        public string strModule
        {
            get
            {
                return "ChemistryTests";
            }
        }

        private void loadData()
        {
            try
            {
                ConfigurationController objChemistryM = new ConfigurationController();
                DataTable dt = null;

                dt = objChemistryM.GetDICTChemistryTests(txtSearchCode.Text.Trim() , txtSearchName.Text.Trim());

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
                    ChemistryM objchemistryM = new ChemistryM();
                    DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                    objchemistryM.CHEMISTRY_ID = row.Cells[0].Value.ToString();
                    objchemistryM.CHEMISTRY_CODE = row.Cells[1].Value.ToString();
                    objchemistryM.CHEMISTRY_SHORTTEXT = row.Cells[2].Value.ToString();
                    objchemistryM.CHEMISTRY_FULLTEXT = row.Cells[3].Value.ToString();
                    objchemistryM.CHEMISTRY_DESCRIPTION = row.Cells[4].Value.ToString();
                    objchemistryM.CHEMISTRY_PRINT = row.Cells[5].Value.ToString();

                    ControlChemistryTests.CHEMISTRY_ID = objchemistryM.CHEMISTRY_ID;
                    ControlParameter.ChemistryM = objchemistryM;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                string strChemistryTestsID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddChemistryTests fm = new ConfigurationFld.frmAddChemistryTests("edit", strChemistryTestsID);
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
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Chemistry Tests, Chemistry customized จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    DataTable dt = null;
                    objchemistryM = ControlParameter.ChemistryM;

                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CHECK DATA IN SUBREQMB_CHEMISTRIES
                    //
                    // STEP CLEAR & DELETE
                    // - Check result in SUBREQMB_CHEMISTRIES

                    dt = objConfig.Check_Result_in_SUBREQMB_CHEMISTRIES(objchemistryM);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Result in database", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else
                    {
                        // 1. DICT_MB_CHEMISTRYS ----> DELETE 
                        // DICT_MB_CHEMISTRYS
                        // Check and Delete DICT_MB_CHEMISTRYS
                        objConfig.Delete_DICT_MB_CHEMISTRYS(objchemistryM);
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
