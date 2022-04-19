using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class Media : UserControl
    {
        private ConfigurationController objConfiguration = new ConfigurationController();
        private MediaM objMedia = new MediaM();

        public Media()
        {
            InitializeComponent();
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        public string strModule
        {
            get
            {
                return "media";
            }

        }

        private void LoadData()
        {
            ConfigurationController objMediaM = new ConfigurationController();
            DataTable dt = null;

            dt = objConfiguration.GetMedia_in_DICT(txtSearchCode.Text.Trim(), txtSearchName.Text.Trim() );

            try
            {

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
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MediaM objMediaM = new MediaM();
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                objMedia.AgarID = Convert.ToInt16 (row.Cells[0].Value.ToString());
                objMedia.AgarCode = row.Cells[1].Value.ToString();
                objMedia.AgarName = row.Cells[2].Value.ToString();
                objMedia.Description = row.Cells[3].Value.ToString();

                ControlAgars.AgarID = Convert.ToString(objMedia.AgarID );
                ControlParameter.MediaM = objMedia;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strMediaID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddMedia fm = new ConfigurationFld.frmAddMedia("edit", strMediaID);
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

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Media จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    DataTable dt = null;
                    objMedia = ControlParameter.MediaM;

                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CHECK RESULTS IN SUBREQMB_AGARS
                    //
                    // STEP CLEAR & DELETE
                    // - Check result in SUBREQMB_AGARS
                    dt = objConfig.Check_Result_in_SUBREQMB_AGARS(objMedia);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Result in database", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // 1. Delete_DICT_MB_AGARS ----> DELETE 
                        // 2. Delete_DICT_MB_PROTOCOL_MEDIA ----> DELETE
                        // Delete_DICT_MB_AGARS , DICT_MB_PROTOCOL_MEDIA
                        objConfig.Delete_DICT_MB_PROTOCOL_MEDIA(objMedia);

                        // Check and Delete Delete_DICT_MB_AGARS
                        objConfig.Delete_DICT_MB_AGARS(objMedia);
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
