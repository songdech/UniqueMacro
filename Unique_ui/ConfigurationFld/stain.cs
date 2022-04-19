using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class stain : UserControl
    {
        private ConfigurationController objConfig = new ConfigurationController();
        private StainM objStainM = new StainM();

        public string strModule
        {
            get
            {
                return "stain";
            }
        }

        public stain()
        {
            InitializeComponent();
        }

        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "stain";
            loadData();
        }

        private void loadData()
        {
            DataTable dt = null;

            ConfigurationController objConfig = new ConfigurationController();
            try
            {
                dt = objConfig.GetStainConfiguration(txt_Code.Text.Trim(), txt_Name.Text.Trim());

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
                    StainM objStainM = new StainM();
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                    objStainM.MBSTAINID = Convert.ToInt32(row.Cells[0].Value.ToString());
                    objStainM.MBSTAINCODE = row.Cells[1].Value.ToString();
                    objStainM.STAINNAME = row.Cells[2].Value.ToString();
                    objStainM.STAIN_DESCRIPTION = row.Cells[3].Value.ToString();
                    objStainM.STAIN_NOTPRINTABLE = row.Cells[4].Value.ToString();
                    objStainM.STAIN_LOGUSERID = ControlParameter.loginID.ToString();
                    ControlStain.stainID = Convert.ToInt32(objStainM.MBSTAINID);
                    ControlParameter.StainM = objStainM;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strMBSTAINID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddStain fm = new ConfigurationFld.frmAddStain("edit", strMBSTAINID);
                fm.ShowDialog();
            }
        }

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Stain,Result type, Protocol stain จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    DataTable dt = null;
                    objStainM = ControlParameter.StainM;

                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CHECK DATA IN SUBREQMB_STAINS
                    //
                    dt = objConfig.Check_Result_in_SUBREQMB_STAINS(objStainM);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Result in database" ," ",MessageBoxButtons.OK ,MessageBoxIcon.Warning);

                    }
                    else
                    {
                        // STEP CLEAR & DELETE
                        // 1. RESULT_TYPE_PROPERTIES ----> DELETE 
                        // Relate with DICT_MB_STAINS
                        // 2. DICT_MB_PROTOCOL_STAINS   ----> DELETE  in MBSTAINID
                        // 3. DICT_MB_STAINS         ----> DELETE
                        // RESULT_TYPE_PROPERTIES clear
                        // Check and Delete RESULT_TYPE_PROPERTIES = Delete
                        objConfig.Delete_Stain_in_RESULT_TYPE_PROPERTIES(objStainM);
                        // Check and Delete in DICT_PROTOCOL_STAINS = Delete
                        objConfig.Delete_Stain_in_DICT_MB_PROTOCOL_STAINS(objStainM);
                        // Check and Delete in DICT_MB_STAINS = Delete
                        objConfig.Delete_Stain_in_DICT_MB_STAINS(objStainM);
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
