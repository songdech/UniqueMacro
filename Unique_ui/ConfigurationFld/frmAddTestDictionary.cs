using System;
using System.Data;
using System.Windows.Forms;
//using System.Data.SqlClient;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddTestDictionary : Form
    {
        //SqlConnection conn;
        private string moduleType;
        private string testID;

        private ConfigurationController objConfig;
        private TestDictM objTestDictM;
        private DataTable dtSpecimen;
        private DataTable dtProtocals;

        object SPM01;       //specimen code
        object SPM02;       //..       text
        object SPM03;       //..       id

        object PRO01;       //protocol code
        object PRO02;       //..       text
        object PRO03;       //..       id

        public frmAddTestDictionary(string moduleType, string testID)
        {
            InitializeComponent();
            objTestDictM = new TestDictM();
            this.moduleType = moduleType;
            this.testID = testID;
            objConfig = new ConfigurationController();
            if (moduleType != "add")
            {
                objTestDictM = ControlParameter.TestDictM;
            }
        }
        public Action yourAction { get; set; }
        private void frmAddTestDictionary_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
        }
        private void LoadData()
        {

            //NOTE
            if (moduleType == "edit")
            {
                try
                {
                    groupBox1.Visible = true;
                    label9.Visible = true;
                    if (objTestDictM.Printable == 1)
                    {
                        printY.Checked = true;
                    }
                    else
                    {
                        printN.Checked = true;
                    }

                    queryProtocol();
                    querySpecimenType();

                    txtTestCode.Text = objTestDictM.TestCode;
                    txtTestName.Text = objTestDictM.TestName;

                    txtSpecimen_ID.Text = objTestDictM.COLLMATERIALID.ToString();
                    txtProtocol_name.Text = objTestDictM.PROTOCOLTEXT;
                    txtProtocol_ID.Text = objTestDictM.ProtocalID.ToString();
                    txtSpecimen_name.Text = objTestDictM.COLLMATERIALTEXT;

                    txtDescription.Text = objTestDictM.DESCRIPTION;

                    txtProtocol_code.Text = objTestDictM.PROTOCOLCODE;
                    txtSpecimen_code.Text = objTestDictM.COLLMATERIALCODE;

                    comboBox_Tests_TAB.SelectedIndex = objTestDictM.TESTS_TAB;

                    label6.Visible = true;
                    label6.Text = "Edit Dictionary";
                    this.Text = "Edit Dictionary";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else if (moduleType == "dup")
            {

                groupBox1.Visible = true;
                label9.Visible = true;
                printY.Checked = true;

                queryProtocol();
                querySpecimenType();

                txtTestCode.Select();
                txtTestName.Text = objTestDictM.TestName;

                txtSpecimen_ID.Text = objTestDictM.COLLMATERIALID.ToString();
                txtProtocol_name.Text = objTestDictM.PROTOCOLTEXT;
                txtProtocol_ID.Text = objTestDictM.ProtocalID.ToString();
                txtSpecimen_name.Text = objTestDictM.COLLMATERIALTEXT;

                txtDescription.Text = objTestDictM.DESCRIPTION;
                txtSpecimen_code.Text = objTestDictM.PROTOCOLCODE;
                txtProtocol_code.Text = objTestDictM.PROTOCOLCODE;

                comboBox_Tests_TAB.SelectedIndex = objTestDictM.TESTS_TAB;


                label6.Visible = true;
                label6.Text = "Duplicate Dictionary";
                this.Text = "Duplicate Dictionary";

            }
            else if (moduleType == "add")
            {
                groupBox1.Visible = true;
                label9.Visible = true;
                printY.Checked = true;

                queryProtocol();
                querySpecimenType();

                label6.Visible = true;
                label6.Text = "Add Dictionary";
                this.Text = "Add Dictionary";
            }
        }

        private void querySpecimenType()
        {
            try
            {
                SpecimenM objSpecimenM = new SpecimenM();

                objSpecimenM.COLLMATERIALCODE = "";
                objSpecimenM.COLLMATERIALTEXT = "";

                dtSpecimen = objConfig.GetSpecimen(objSpecimenM);
                gridLookUpEdit_Specimen.Properties.DataSource = dtSpecimen;
                gridLookUpEdit_Specimen.Properties.DisplayMember = "COLLMATERIALCODE";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void queryProtocol()
        {
            try
            {
                dtProtocals = objConfig.GetDICTProtocals("%", "%");
                gridLookUpEdit_Protocol.Properties.DataSource = dtProtocals;
                gridLookUpEdit_Protocol.Properties.DisplayMember = "PROTOCOLCODE";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void navClose_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }
        private void navSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            if (moduleType == "add")
            {
                if (txtTestCode.Text != "" && txtProtocol_ID.Text != "")
                {

                    if (printY.Checked == true)
                    {
                        objTestDictM.Printable = 1;
                    }
                    else
                    {
                        objTestDictM.Printable = 0;
                    }

                    objTestDictM.TestCode = txtTestCode.Text;
                    objTestDictM.TestName = txtTestName.Text;
                    objTestDictM.DESCRIPTION = txtDescription.Text;

                    if (txtSpecimen_ID.Text == "")
                    {
                        objTestDictM.COLLMATERIALID = 0;
                    }
                    else
                    {
                        objTestDictM.COLLMATERIALID = Convert.ToInt32(txtSpecimen_ID.Text);
                    }

                    objTestDictM.PROTOCOLTEXT = txtProtocol_name.Text;
                    objTestDictM.ProtocalID = Convert.ToInt32(txtProtocol_ID.Text);
                    objTestDictM.TESTS_TAB = comboBox_Tests_TAB.SelectedIndex;
                    objTestDictM.LOGUSERID = ControlParameter.loginID.ToString();

                    objTestDictM = objConfig.SaveTestDict(objTestDictM);


                    this.Close();
                    Action instance = yourAction;
                    if (instance != null)
                        instance();

                }
                else
                {
                    MessageBox.Show("Please fill information first");
                }
            }
            else if (moduleType == "dup")
            {
                if (txtTestCode.Text != "" && txtProtocol_ID.Text != "" && txtSpecimen_ID.Text != "")
                {

                    if (printY.Checked == true)
                    {
                        objTestDictM.Printable = 1;
                    }
                    else
                    {
                        objTestDictM.Printable = 0;
                    }

                    objTestDictM.TestCode = txtTestCode.Text;
                    objTestDictM.TestName = txtTestName.Text;
                    objTestDictM.DESCRIPTION = txtDescription.Text;

                    objTestDictM.COLLMATERIALID = Convert.ToInt32(txtSpecimen_ID.Text);
                    objTestDictM.PROTOCOLTEXT = txtProtocol_name.Text;
                    objTestDictM.ProtocalID = Convert.ToInt32(txtProtocol_ID.Text);
                    objTestDictM.COLLMATERIALTEXT = txtSpecimen_name.Text;
                    objTestDictM.TESTS_TAB = comboBox_Tests_TAB.SelectedIndex;


                    objTestDictM.LOGUSERID = ControlParameter.loginID.ToString();


                    objTestDictM = objConfig.SaveTestDict(objTestDictM);


                    this.Close();
                    Action instance = yourAction;
                    if (instance != null)
                        instance();
                }
                else
                {
                    MessageBox.Show("Please fill information first");
                }
            }
            else if (moduleType == "edit")
            {
                if (txtTestCode.Text != "")
                {
                    if (txtProtocol_ID.Text != "" && txtSpecimen_ID.Text != "")
                    {
                        txtTestCode.Enabled = true;

                        if (printY.Checked == true)
                        {
                            objTestDictM.Printable = 1;
                        }
                        else
                        {
                            objTestDictM.Printable = 0;
                        }

                        objTestDictM.TestName = txtTestName.Text;
                        objTestDictM.DESCRIPTION = txtDescription.Text;

                        objTestDictM.COLLMATERIALID = Convert.ToInt32(txtSpecimen_ID.Text);
                        objTestDictM.PROTOCOLTEXT = txtProtocol_name.Text;
                        objTestDictM.ProtocalID = Convert.ToInt32(txtProtocol_ID.Text);
                        objTestDictM.COLLMATERIALTEXT = txtSpecimen_name.Text;
                        objTestDictM.TESTS_TAB = comboBox_Tests_TAB.SelectedIndex;

                        objTestDictM.LOGUSERID = ControlParameter.loginID.ToString();


                        objTestDictM = objConfig.SaveTestDict(objTestDictM);

                        ControlParameter.TestDictM = objConfig.SaveTestDict(ControlParameter.TestDictM);

                        this.Close();

                        Action instance = yourAction;
                        if (instance != null)
                            instance();
                    }

                    //}
                    //else
                    //{
                    //    MessageBox.Show("Test code : " + txtTestCode.Text.Trim() + " arleady in database.", "Please use other code.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                }
            }

        }
        private void gridLookUpEdit_Specimen_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = gridLookUpEdit_Specimen.Properties.View as DevExpress.XtraGrid.Views.Grid.GridView;
                SPM01 = view.GetRowCellValue(view.FocusedRowHandle, "COLLMATERIALCODE");
                SPM02 = view.GetRowCellValue(view.FocusedRowHandle, "COLLMATERIALTEXT");
                SPM03 = view.GetRowCellValue(view.FocusedRowHandle, "COLLMATERIALID");

                txtSpecimen_ID.Text = SPM03.ToString();
                txtSpecimen_name.Text = SPM02.ToString();
                txtSpecimen_code.Text = SPM01.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void gridLookUpEdit_Protocol_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                    DevExpress.XtraGrid.Views.Grid.GridView view = gridLookUpEdit_Protocol.Properties.View as DevExpress.XtraGrid.Views.Grid.GridView;
                    PRO01 = view.GetRowCellValue(view.FocusedRowHandle, "PROTOCOLCODE");
                    PRO02 = view.GetRowCellValue(view.FocusedRowHandle, "PROTOCOLTEXT");
                    PRO03 = view.GetRowCellValue(view.FocusedRowHandle, "PROTOCOLID");

                    txtProtocol_ID.Text = PRO03.ToString();
                    txtProtocol_name.Text = PRO02.ToString();
                    txtProtocol_code.Text = PRO01.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
