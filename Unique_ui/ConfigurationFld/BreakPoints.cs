using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class BreakPoints : UserControl
    {
        private BreakPointM objBreakPointM;

        public BreakPoints()
        {
            InitializeComponent();
        }
        public string strModule
        {
            get
            {
                return "BreakPoint";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private ConfigurationController objConfig = new ConfigurationController();

        private void loadData()
        {
            DataTable dt = null;

            ConfigurationController objConfig = new ConfigurationController();
            try
            {
                dt = objConfig.GetBreakPoint(txtBreak_CODE.Text.Trim(), txtBreakPoint_Name.Text.Trim());

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

                BreakPointM objBreakPointM = new BreakPointM();
                DataGridViewRow row_1 = this.dataGridView1.Rows[e.RowIndex];
                objBreakPointM.CUSTOMIZED_BREAKPOINT_CODE = row_1.Cells[1].Value.ToString();
                objBreakPointM.CUSTOMIZED_BREAKPOINT_TEXT = row_1.Cells[2].Value.ToString();
                objBreakPointM.CUSTOMIZED_BREAKPOINT_ID = row_1.Cells[0].Value.ToString();
                objBreakPointM.BREAKPOINT_DESCRIPTION = row_1.Cells[7].Value.ToString();

                objBreakPointM.LOGUSERID = ControlParameter.loginID.ToString();
                ControlParameter.BreakPointM = objBreakPointM;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strBreakPointID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmBreakpoint fm = new ConfigurationFld.frmBreakpoint("edit", strBreakPointID);
                fm.ShowDialog();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "BreakPoint";
            loadData();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage1"])
            {
                clsControlsData.currentForm = "BreakPoint";
                label2.Text = clsControlsData.currentForm;
            }
        }

        private void simpleBtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Antimicrobial List จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                

                if (No == DialogResult.Yes)
                {
                    objBreakPointM = ControlParameter.BreakPointM;

                    // Check and Restore MICID in DICT_MB_ORGANISMS = 0
                    objBreakPointM = objConfig.BreakPoint_ORGANISM_Restore(objBreakPointM);
                    // Check and Delete in DICT_ANTIMICROBIAL_BREAKPOINT_LIST  row > 0
                    objConfig.BreakPoint_DELETE_LIST(objBreakPointM);
                    // Check and Delete in DICT_ANTIMICROBIAL_BREAKPOINT  row > 0
                    objBreakPointM = objConfig.BreakPoint_DELETE(objBreakPointM);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
