using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using UniquePro.Common;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;
using UniquePro.Entities.Common;


namespace UNIQUE.ConfigurationFld
{
    public partial class Specimen_Group : UserControl
    {

        SpecimenG objSpecimenG = null;

        public Specimen_Group()
        {
            InitializeComponent();
        }

        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }
 
        private void Specimen_Load(object sender, EventArgs e)
        {
            objSpecimenG = new SpecimenG();

            LoadData();
            Action instance2 = checkEnableButton;
            if (instance2 != null)
                instance2();
        }
        public string strModule
        {
            get
            {
                return "specimen Group";
            }
        }
        private void LoadData()
        {

            ConfigurationController objConfigSpecimenG = new ConfigurationController();
            DataTable dt = null;
            SpecimenG objSpecimenG = new SpecimenG();
            //DoctorM objDoctor = new DoctorM();

            try
            {
                objSpecimenG.SPMG_CODE = txtCode.Text;
                objSpecimenG.SPMG_NAME = txtName.Text;

                dt = objConfigSpecimenG.GetSpecimenG(objSpecimenG);

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
                objSpecimenG = null;
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
                SpecimenG objSpecimenG = new SpecimenG();

                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                objSpecimenG.SPMG_ID = Convert.ToInt16(row.Cells[0].Value.ToString());
                objSpecimenG.SPMG_CODE = row.Cells[1].Value.ToString();
                objSpecimenG.SPMG_NAME = row.Cells[2].Value.ToString();
                objSpecimenG.SPMG_COMMENT = row.Cells[6].Value.ToString();
                objSpecimenG.SPMG_CREDATE = row.Cells[4].Value.ToString();
                objSpecimenG.SPMG_LOGUSERID = row.Cells[5].Value.ToString();
                objSpecimenG.SPMG_LOGDATE = row.Cells[3].Value.ToString();

                objSpecimenG.LogUserID = ControlParameter.ControlUser.USERID.ToString();

                ControlParameter.SpecimenInfoG = objSpecimenG;
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                string strSpecimenID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddSpecimenGroup fm = new ConfigurationFld.frmAddSpecimenGroup("edit", strSpecimenID);
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
                DialogResult Yes = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Specimen Group config จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Yes == DialogResult.Yes)
                {
                    DataTable dt = null;
                    objSpecimenG = ControlParameter.SpecimenInfoG;


                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CLEAR & DELETE
                    // - Check Relation in DICT_MB_PROTOCOLS

                    dt = objConfig.Check_Relation_SPECIMEN_GROUP(objSpecimenG);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Result in database", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // 1. DICT_MB_PROTOCOL ----> is Null
                        // DICT_MB_GROUP_SPECIMEN delete ID
                        // DICT_MB_GROUP_SPECIMEN_COLLMATERIAL delete ID

                        objConfig.DeleteSpecimen_In_DICT_MB_GROUP_SPECIMEN_COLLMATERIAL(objSpecimenG);
                        objConfig.DeleteSpecimen_In_DICT_MB_GROUP_SPECIMEN(objSpecimenG);
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
