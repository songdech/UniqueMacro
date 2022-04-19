using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class CustomizeResult : UserControl
    {
        private ConfigurationController objConfig = new ConfigurationController();

        private CustomResultM objcustomResultM = new CustomResultM();

        public CustomizeResult()
        {
            InitializeComponent();
        }
        public string strModule
        {
            get
            {
                return "customizedresult";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "Customized_List";
            LoadData_Customized_List();
        }

        private void LoadData_Customized_List()
        {
            DataTable dt = null;

            ConfigurationController objConfig = new ConfigurationController();
            try
            {
                dt = objConfig.GetCustomized_List(txtCUSLIST_CODE.Text.Trim(), txtCUSLIST_Name.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();

                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();

                    simpleBtn_List.Visible = true;
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
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            clsControlsData.currentForm = "Customized_Result";
            loadData();
        }

        private void loadData()
        {
            DataTable dt = null;

            ConfigurationController objConfig = new ConfigurationController();
            try
            {
                dt = objConfig.GetCustomResult(txtCUS_RES_CODE.Text.Trim(), txtCUS_RES_NAME.Text.Trim());

                if (dt.Rows.Count > 0)
                {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();

                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();

                    simpleButton_Results.Visible = true;
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

                CustomResultM objCustomizedM = new CustomResultM();
                DataGridViewRow row_1 = this.dataGridView1.Rows[e.RowIndex];

                objCustomizedM.CUSTOMIZED_RES_ID = row_1.Cells[0].Value.ToString();
                objCustomizedM.CUSTOMIZED_RES_CODE = row_1.Cells[1].Value.ToString();
                objCustomizedM.CUSTOMIZED_RES_TEXT = row_1.Cells[2].Value.ToString();
                objCustomizedM.DESCRIPTION_RES_ = row_1.Cells[3].Value.ToString();

                objCustomizedM.LOGUSERID_RES_ = ControlParameter.loginID.ToString();

                ControlCustomizeResult.customizeResID = objCustomizedM.CUSTOMIZED_RES_ID;
                ControlParameter.CustomResulM = objCustomizedM;
            }
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                CustomResultM objCustomizedM = new CustomResultM();
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                objCustomizedM.CUSTOMIZED_ID = row.Cells[0].Value.ToString();
                objCustomizedM.CUSTOMIZED_CODE = row.Cells[1].Value.ToString();
                objCustomizedM.CUSTOMIZED_TEXT = row.Cells[2].Value.ToString();
                objCustomizedM.CUSTOMIZED_FULLTEXT = row.Cells[3].Value.ToString();
                objCustomizedM.DESCRIPTION = row.Cells[4].Value.ToString();
                objCustomizedM.CUSTOMIZED_TEXTTYPE = row.Cells[7].Value.ToString();
                objCustomizedM.CUSTOMIZED_LIST_CLASS = row.Cells[8].Value.ToString();

                objCustomizedM.LOGUSERID = ControlParameter.loginID.ToString();
                ControlParameter.CustomResulM = objCustomizedM;
                ControlCustomizeResult.customizeRes_List_ID = objCustomizedM.CUSTOMIZED_ID;
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                string strCustomizedID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmCustomize_LIST fm = new ConfigurationFld.frmCustomize_LIST("edit", strCustomizedID);
                fm.ShowDialog();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strCustomizedID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmCustomize_RESULT fm = new ConfigurationFld.frmCustomize_RESULT("edit", strCustomizedID);
                fm.ShowDialog();
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage1"])
            {
                clsControlsData.currentForm = "Customized_Result";
                label2.Text = clsControlsData.currentForm;

            }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"])
            {
                clsControlsData.currentForm = "Customized_List";
                label2.Text = clsControlsData.currentForm;
            }
        }

        // Delete Lists
        private void simpleButton_Results_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Customized Result, Detection config :Chemistry Config :Stain Config จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    DataTable dt_RESULT_TYPE_PROPERTIES = null;
                    DataTable dt_DICT_MB_CHEMISTRYS = null;
                    DataTable dt_DICT_MB_DETECT_TESTS = null;

                    objcustomResultM = ControlParameter.CustomResulM;

                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CHECK DATA IN 
                    // - DETEC_TESTS
                    // - CHEMISTRYS TESTS
                    // - STAINS
                    //
                    // STEP CLEAR & DELETE
                    // - Check result in ถ้ามีอย่างใดอย่างหนึ่ง จะไม่ให้ลบ
                    // - RESULT_TYPE_PROPERTIES --> CUSRESULTID  --> จะไป Relate กับ DICT_MB_STAINS ถ้ามี Config CUSTOMIZED RESULT in STAINS [can't delete]
                    // - DICT_MB_CHEMISTRYS --> CUSRESULTID --> จะไป Relate กับ DICT_MB_CHEMISTRYS ถ้ามี Config CUSTOMIZED RESULT in CHEMISTRIES [can't delete]
                    // - DICT_MB_DETECT_TESTS   --> CUSRESULTID --> จะไป Relate กับ DICT_MB_DETECT_TESTS ถ้ามี Config CUSTOMIZED RESULT in DETECTION TESTS [can't delete]

                    dt_RESULT_TYPE_PROPERTIES = objConfig.Check_Result_in_RESULT_TYPE_PROPERTIES(objcustomResultM);
                    dt_DICT_MB_CHEMISTRYS = objConfig.Check_Result_in_DICT_MB_CHEMISTRYS(objcustomResultM);
                    dt_DICT_MB_DETECT_TESTS = objConfig.Check_Result_in_DICT_MB_DETECT_TESTS(objcustomResultM);

                    if (dt_RESULT_TYPE_PROPERTIES.Rows.Count > 0 || dt_DICT_MB_CHEMISTRYS.Rows.Count > 0 || dt_DICT_MB_DETECT_TESTS.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Config Dictionary in database STAINS,CHEMISTRY,DETECTION TESTS", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // - RESULT_TYPE_PROPERTIES  --> Relate --> DICT_CUS_RESULTS --> Delete with CUSRESULTID

                        // - DICT_MB_DETEC_TESTS --> Relate --> DICT_CUS_RESULTS -->Use UPDATE with CUSRESULTID = null

                        // - DICT_MB_CHEMISTRYS  --> Relate --> DICT_CUS_RESULTS -->Use UPDATE with CUSRESULTID = null

                        // 1. DICT_CUS_RESULT_LIST ----> Rlate --> DICT_CUS_RESULT -->Use Delete
                        // 2. DICT_CUS_RESULT  ---> Relate --> DICT_CUS_RESULT_LIST -->Use Delete
                        objConfig.Delete_CustomizedResult_in_DICT_CUS_RESULT_LIST(objcustomResultM);
                        objConfig.Delete_CustomizedResult_in_DICT_CUS_RESULTS(objcustomResultM);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // Delete Results 
        private void simpleBtn_List_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล Customized List, จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    DataTable dt = null;

                    objcustomResultM = ControlParameter.CustomResulM;

                    ConfigurationController objConfig = new ConfigurationController();

                    // STEP CHECK DATA IN 
                    // - DICT_CUS_RESULT_LIST
                    //
                    // STEP CLEAR & DELETE
                    // - Check database in
                    // - DICT_CUS_RESULT_LIST --> TEXTID  --> จะไป Relate กับ DICT_MB_TEXTID ถ้ามี Config in DICT_CUS_RESULT_LIST [can't delete]
                    // ขั้นตอนการลบ ออกมี Detail ดังนี้
                    /* * หมายเหตุ เพราะ DICT_CUS_RESULT_LIST จะบันทึกการตั้งค่า โดยนำ Value DICT_MB_TEXT ที่ TEXTID มาใส่ใน DIC_CUS_RESULT โดยในหนึ่ง CODE Customized จะมีได้หลาย CODE ใน 
                     DICT_MB_TEXT ถ้าหากมีการบันทึกค่าเข้าไปแล้ว จะไม่สามารถลบออกได้ หาก
                     ต้องการลบ ต้องลบ ต้นทางที่ เป็น Config DICT_CUS_RESULT ออกก่อน เพราะ DICT_CUS_RESULT จะมีการเช็คค่าที่บันทึก RESULT ใน DETECTION_TEST,CHEMISTRIES,STAINS ก่อนที่จะทำการลบ DICT_CUS_RESULT ออกได้
                    */

                    dt = objConfig.Check_Result_in_DICT_CUS_RESULT_LIST(objcustomResultM);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Can't delete Found Config Dictionary in database Customized Result", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("delete");
                        // 1. DICT_MB_CHEMISTRYS ----> DELETE 
                        // DICT_MB_CHEMISTRYS
                        // Check and Delete DICT_MB_CHEMISTRYS
                        objConfig.Delete_CustomizedList_in_DICT_MB_TEXTS(objcustomResultM);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void txtCUS_RES_CODE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadData();
            }
        }

        private void txtCUS_RES_NAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadData();
            }
        }

        private void txtCUSLIST_CODE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData_Customized_List();
            }
        }
        private void txtCUSLIST_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData_Customized_List();
            }
        }

    }
}
