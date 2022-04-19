using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;


namespace UNIQUE.ConfigurationFld
{
    public partial class TestDictionary : UserControl
    {

        private TestDictM objTestM;

        public TestDictionary()
        {
            InitializeComponent();
            objTestM = new TestDictM();
        }

        public string strModule
        {
            get
            {
                return "test";
            }
        }
        private void TestDictionary_Load(object sender, EventArgs e)
        {            
            try
            {
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }
        private void loadData()
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            try
            {
                dt = objConfig.GetDictTest(txtSearchCode.Text.Trim(), txtSearchName.Text.Trim());

                if (dt.Rows.Count > 0) {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();
                }

                Action instance2 = checkEnableButton;
                if (instance2 != null)
                    instance2();

                //dataGridView1.DataSource = dt;
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
                    DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                    objTestM.TestCode = row.Cells[0].Value.ToString();
                    objTestM.TestName = row.Cells[1].Value.ToString();
                    objTestM.PROTOCOLCODE = row.Cells[2].Value.ToString();
                    objTestM.Printable = Convert.ToInt32(row.Cells[3].Value.ToString());
                    objTestM.COLLMATERIALTEXT = row.Cells[4].Value.ToString();
                    objTestM.TestID = Convert.ToInt32(row.Cells[8].Value.ToString());
                    objTestM.PROTOCOLTEXT = row.Cells[9].Value.ToString();
                    objTestM.COLLMATERIALID = Convert.ToInt32(row.Cells[11].Value.ToString());
                    objTestM.DESCRIPTION = row.Cells[12].Value.ToString();
                    objTestM.COLLMATERIALCODE = row.Cells[13].Value.ToString();
                    objTestM.TESTS_TAB = Convert.ToInt32(row.Cells[14].Value.ToString());

                    objTestM.ProtocalID = Convert.ToInt32(row.Cells[10].Value.ToString());
                    ControlParameter.TestDictM = objTestM;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                string strTestid = row.Cells[8].Value.ToString();
                ConfigurationFld.frmAddTestDictionary fm = new ConfigurationFld.frmAddTestDictionary("edit", strTestid);
                fm.yourAction = loadData;
                fm.ShowDialog();

            }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            loadData();
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
                DialogResult Yes = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Tests configure จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Yes == DialogResult.Yes)
                {
                    DataTable dt = null;
                    objTestM = ControlParameter.TestDictM;

                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CLEAR & DELETE
                    // - Check result in MB_REQUESTS

                    dt = objConfig.Check_Result_DICT_TESTS(objTestM);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Result in database", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // 1. DICT_TESTS ----> DELETE 

                        objConfig.DeleteTESTS_In_DICT_TESTS(objTestM);
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
