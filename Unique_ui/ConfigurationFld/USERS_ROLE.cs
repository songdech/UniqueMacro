using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.GeneralSetting;

namespace UNIQUE.ConfigurationFld
{


    public partial class USERS_ROLE : UserControl
    {    
        UserM objUserM = null;

        public USERS_ROLE()
        {
            InitializeComponent();
        }

        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }
 
        private void USERS_ROLE_Load(object sender, EventArgs e)
        {
            objUserM = new UserM();

            Action instance2 = checkEnableButton;
            if (instance2 != null)
                instance2();
        }
        public string strModule
        {
            get
            {
                return "Role";
            }
        }
        private void LoadData()
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            UserM objUserM = new UserM();

            try
            {
                dt = objConfig.Get_Role(txtName.Text.Trim());

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
                objUserM = null;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                    objUserM.ROLEID = row.Cells[0].Value.ToString();
                    objUserM.ROLENAME = row.Cells[1].Value.ToString();
                    objUserM.DESCRIPTION = row.Cells[2].Value.ToString();
                    objUserM.LOGUSERID = row.Cells[3].Value.ToString();

                    objUserM.LogUserID = ControlParameter.ControlUser.USERID.ToString();

                    ControlUser_Role.ROLEID = Convert.ToString(objUserM.ROLEID);
                    ControlParameter.UserInfoM = objUserM;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                string strROLEID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddUSERS_ROLE fm = new ConfigurationFld.frmAddUSERS_ROLE("edit", strROLEID);
                fm.refreshData = LoadData;
                fm.ShowDialog();
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
