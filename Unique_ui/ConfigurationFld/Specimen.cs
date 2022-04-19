using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{


    public partial class Specimen : UserControl
    {    
        SpecimenM objSpecimenM = null;

        public Specimen()
        {
            InitializeComponent();
        }

        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }
 
        private void Specimen_Load(object sender, EventArgs e)
        {
            objSpecimenM = new SpecimenM();

            Action instance2 = checkEnableButton;
            if (instance2 != null)
                instance2();
        }
        public string strModule
        {
            get
            {
                return "specimen";
            }
        }
        private void LoadData()
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            SpecimenM objSpecimenM = new SpecimenM();

            try
            {
                dt = objConfig.Get_Dict_Specimen(txtCode.Text.Trim(), txtName.Text.Trim());

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
                objSpecimenM = null;
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
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                objSpecimenM.COLLMATERIALID = Convert.ToInt16 (row.Cells[0].Value.ToString());
                objSpecimenM.COLLMATERIALCODE = row.Cells[1].Value.ToString();
                objSpecimenM.COLLMATERIALTEXT = row.Cells[2].Value.ToString();
                objSpecimenM.COLLMATERIALCOMMENT = row.Cells[3].Value.ToString();
                objSpecimenM.SPMG_NAME = row.Cells[9].Value.ToString();
                objSpecimenM.SPMG_ID = row.Cells[10].Value.ToString();
                objSpecimenM.LogUserID = ControlParameter.ControlUser.USERID.ToString();

                ControlSpecimen.COLLMATERIALID = Convert.ToString(objSpecimenM.COLLMATERIALID);
                ControlParameter.SpecimenInfoM = objSpecimenM;
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                string strSpecimenID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddSpecimen fm = new ConfigurationFld.frmAddSpecimen("edit", strSpecimenID);
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
                //DialogResult Yes = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Collmaterial configure จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                //if (Yes == DialogResult.Yes)
                //{
                //    DataTable dt = null;
                //    objSpecimenM = ControlParameter.SpecimenInfoM;

                //    ConfigurationController objConfig = new ConfigurationController();

                //    // STEP CLEAR & DELETE
                //    // - Check Relation in DICT_MB_PROTOCOLS

                //    dt = objConfig.Check_Relation_SPECIMEN_GROUP(objSpecimenM);

                //    if (dt.Rows.Count > 0)
                //    {
                //        MessageBox.Show("Can't delete Found Result in database", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    }
                //    else
                //    {
                //        // 1. DICT_MB_PROTOCOL ----> is Null
                //        // DICT_MB_GROUP_SPECIMEN delete ID
                //        // DICT_MB_GROUP_SPECIMEN_COLLMATERIAL delete ID

                //        objConfig.DeleteSpecimen_In_DICT_MB_GROUP_SPECIMEN_COLLMATERIAL(objSpecimenM);
                //        objConfig.DeleteSpecimen_In_DICT_MB_GROUP_SPECIMEN(objSpecimenM);
                //    }
               // }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
