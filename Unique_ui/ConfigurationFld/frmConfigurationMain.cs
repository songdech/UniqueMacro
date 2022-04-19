using System;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.Configuration
{
    public partial class frmConfigurationMain : Form
    {
        string strModule = "";
        ConfigurationController objConfiguration = new ConfigurationController();
        OrganismController objOraganism = new OrganismController();
        private ConfigurationController objConfigcustomized_List;
        private ConfigurationController objConfigDetection_Tests;
        private ConfigurationController objConfigChemistry_Tests;
        private ConfigurationController objConfigSensitivityPanel_List;


        private CustomResultM objcustomResultM;

        public frmConfigurationMain()
        {
            InitializeComponent();
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.frmDoctors fm = new ConfigurationFld.frmDoctors();
            fm.Dock = DockStyle.Fill;
            strModule = fm.strModule;
            pcMain.Controls.Clear();
            pcMain.Controls.Add(fm);


            //Configuration.doctor userControl = new Configuration.doctor();
            //userControl.Dock = DockStyle.Fill;
            //xtraUserControl11.Controls.Clear();
            //xtraUserControl11.Controls.Add(userControl);


        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmConfigurationMain_Load(object sender, EventArgs e)
        {
            //conn = new ConnectionString().Connect();
        }

        private void checkEnableButton()
        {
            if (clsControlsData.currentForm == null)
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnRefresh.Enabled = false;
                btnDupplicate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else if (clsControlsData.currentForm != null)
            {
                btnAdd.Enabled = true;
            }
        }

        private void enableDuplicateButton()
        {
            btnDupplicate.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnRefresh.Enabled = true;
        }

        private void navBarItem1_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ////Configuration.doctor fm = new doctor();

            ConfigurationFld.frmDoctors fm = new ConfigurationFld.frmDoctors();
            fm.Dock = DockStyle.Fill;
            strModule = fm.strModule;
            pcMain.Controls.Clear();
            pcMain.Controls.Add(fm);

            //fm.Dock = DockStyle.Fill;
            //       pcMain.Controls.Clear();
            //       fm.EnableDupButton = enableDuplicateButton;
            //       fm.checkEnableButton = checkEnableButton;
            //pcMain.Controls.Add(fm);


        }

        public Action yourAction { get; set; }

        /*
         *  ADD
         */
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (clsControlsData.currentForm == "doctor")
            {
                ConfigurationFld.frmAddDoctors fm = new ConfigurationFld.frmAddDoctors("add");
                fm.yourAction = refreshForm;
                fm.ShowDialog();

            }
            if (clsControlsData.currentForm == "location")
            {
                ConfigurationFld.AddLocations fm = new ConfigurationFld.AddLocations("add", "");
                fm.yourAction = refreshForm;
                fm.ShowDialog();

            }
            if (clsControlsData.currentForm == "test")
            {
                ConfigurationFld.frmAddTestDictionary fm = new ConfigurationFld.frmAddTestDictionary("add", "");
                fm.yourAction = refreshForm;
                fm.ShowDialog();

            }
            if (clsControlsData.currentForm == "Antibiotic_Fams")
            {
                ConfigurationFld.frmAddAntibiotic_FAM fm = new ConfigurationFld.frmAddAntibiotic_FAM("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }

            if (clsControlsData.currentForm == "antibiotic")
            {
                ConfigurationFld.frmAddAntibiotic fm = new ConfigurationFld.frmAddAntibiotic("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }

            if (clsControlsData.currentForm == "specimen")
            {
                ConfigurationFld.frmAddSpecimen fm = new ConfigurationFld.frmAddSpecimen("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }

            if (clsControlsData.currentForm == "specimen Group")
            {
                ConfigurationFld.frmAddSpecimenGroup fm = new ConfigurationFld.frmAddSpecimenGroup("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }


            if (clsControlsData.currentForm == "protocol")
            {
                ConfigurationFld.frmAddProtocol fm = new ConfigurationFld.frmAddProtocol("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }
            if (clsControlsData.currentForm == "media")
            {
                ConfigurationFld.frmAddMedia fm = new ConfigurationFld.frmAddMedia("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }
            if (clsControlsData.currentForm == "customizedresult")
            {
                if (clsControlsData.currentForm == "customizedresult")
                {
                    MessageBox.Show("Please select Group!");
                    //ConfigurationFld.frmCustomize_RESULT fm = new ConfigurationFld.frmCustomize_RESULT("add", "");
                    //fm.refreshData = refreshForm;
                    //fm.ShowDialog();
                }
            }
            if (clsControlsData.currentForm == "stain")
            {
                ConfigurationFld.frmAddStain fm = new ConfigurationFld.frmAddStain("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            if (clsControlsData.currentForm == "organism")
            {
                ConfigurationFld.frmAddOrganism fm = new ConfigurationFld.frmAddOrganism("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }

            if (clsControlsData.currentForm == "colonydescription")
            {
                ConfigurationFld.frmAddColony fm = new ConfigurationFld.frmAddColony("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }

            if (clsControlsData.currentForm == "Battery")
            {
                ConfigurationFld.frmAddBattery fm = new ConfigurationFld.frmAddBattery("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }
            if (clsControlsData.currentForm == "Customized_List")
            {
                ConfigurationFld.frmCustomize_LIST fm = new ConfigurationFld.frmCustomize_LIST("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }

            if (clsControlsData.currentForm == "Customized_Result")
            {
                ConfigurationFld.frmCustomize_RESULT fm = new ConfigurationFld.frmCustomize_RESULT("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            if (clsControlsData.currentForm == "DetectionTests")
            {
                ConfigurationFld.frmAddDetectionTests fm = new ConfigurationFld.frmAddDetectionTests("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            if (clsControlsData.currentForm == "ChemistryTests")
            {
                ConfigurationFld.frmAddChemistryTests fm = new ConfigurationFld.frmAddChemistryTests("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            if (clsControlsData.currentForm == "sensitivitypanel")
            {
                ConfigurationFld.frmAddSensitivityPanel fm = new ConfigurationFld.frmAddSensitivityPanel("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            if (clsControlsData.currentForm == "BreakPoint")
            {
                ConfigurationFld.frmBreakpoint fm = new ConfigurationFld.frmBreakpoint("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            if (clsControlsData.currentForm == "Organisms_Fams")
            {
                ConfigurationFld.frmAddOrganism_FAM fm = new ConfigurationFld.frmAddOrganism_FAM("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            if (clsControlsData.currentForm == "Users")
            {
                ConfigurationFld.frmAddUSERS fm = new ConfigurationFld.frmAddUSERS("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            if (clsControlsData.currentForm == "Role")
            {
                ConfigurationFld.frmAddUSERS_ROLE fm = new ConfigurationFld.frmAddUSERS_ROLE("add", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }

            //
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (clsControlsData.currentForm == "doctor")
            {
                string text = ControlDoctor.DoctorCode;

                DialogResult yes = MessageBox.Show("Do you want to delete doctor : " + text + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    string sql = "DELETE FROM DICT_DOCTORS WHERE DICT_DOCTORS.DOCID = '" + ControlDoctor.DoctorID + "'";
                    //SqlCommand cmd = new SqlCommand(sql, conn);
                    //SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    //cmd.ExecuteNonQuery();

                    refreshForm();
                }
            }
            else if (clsControlsData.currentForm == "location")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete location : " + ControlLocation.Loccode + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    string sql = "DELETE FROM DICT_LOCATIONS WHERE LOCID= '" + ControlLocation.LocID + "'";
                    //SqlCommand cmd = new SqlCommand(sql, conn);
                    //SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    //cmd.ExecuteNonQuery();

                    refreshForm();
                }

            }
            else if (clsControlsData.currentForm == "test")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete Test : " + ControlTestDictionary.TestCode + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    string sql = "DELETE FROM DICT_TESTS WHERE TESTID= '" + ControlTestDictionary.TestID + "'";
                    //SqlCommand cmd = new SqlCommand(sql, conn);
                    //SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    //cmd.ExecuteNonQuery();

                    refreshForm();
                }

            }
            else if (clsControlsData.currentForm == "Antibiotic_Fams")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete Antibiotic Family : " + ControlAntibioticFam.AntibioticFamCode + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    string sql = "DELETE FROM DICT_MB_ANTIBIOTIC_GROUP WHERE ANTIBIOTICSFAMILYID= '" + ControlAntibioticFam.AntibioticFamID + "'";
                    //SqlCommand cmd = new SqlCommand(sql, conn);
                    //SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    //cmd.ExecuteNonQuery();

                    refreshForm();
                }

            }

            else if (clsControlsData.currentForm == "specimen")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete specimen : " + ControlSpecimen.COLLMATERIALCODE + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    string sql = "DELETE FROM DICT_COLL_MATERIALS WHERE COLLMATERIALID= '" + ControlSpecimen.COLLMATERIALID + "'";
                    //SqlCommand cmd = new SqlCommand(sql, conn);
                    //SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    //cmd.ExecuteNonQuery();

                    refreshForm();
                }

            }

            else if (clsControlsData.currentForm == "specimen Group")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete specimen Group: " + ControlSpecimenGroup.SPM_CODE + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    string sql = "DELETE FROM DICT_MB_GROUP_SPECIMEN WHERE SPM_GROUP_ID= '" + ControlSpecimenGroup.SPM_GROUP_ID + "'";
                    //SqlCommand cmd = new SqlCommand(sql, conn);
                    //SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    //cmd.ExecuteNonQuery();

                    refreshForm();
                }
            }


            else if (clsControlsData.currentForm == "protocol")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete Protocol : " + ControlProtocol.ProtocolCODE + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    string sql = "DELETE FROM DICT_MB_PROTOCOLS WHERE PROTOCOLID= '" + ControlProtocol.ProtocolID + "'";
                    //SqlCommand cmd = new SqlCommand(sql, conn);
                    //SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    //cmd.ExecuteNonQuery();

                    refreshForm();
                }

            }
            else if (clsControlsData.currentForm == "media")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete Media : " + ControlAgars.AgarCode + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    string sql = "DELETE FROM DICT_MB_AGARS WHERE AGARID= '" + ControlAgars.AgarID + "'";
                    //SqlCommand cmd = new SqlCommand(sql, conn);
                    //SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    //cmd.ExecuteNonQuery();

                    refreshForm();
                }

            }
            else if (clsControlsData.currentForm == "customizedresult")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete Customized Result : " + ControlCustomizeResult.customizeResText + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    try
                    {
                        //string sql = "DELETE FROM DICT_CUS_RESULTS WHERE CUSRESULTID= '" + ControlCustomizeResult.customizeResID + "'";
                        ////SqlCommand cmd = new SqlCommand(sql, conn);
                        ////SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        ////if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                        ////cmd.ExecuteNonQuery();

                        refreshForm();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
                }

            }
            else if (clsControlsData.currentForm == "Customized_List")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete Customized Result List : " + ControlCustomizeResult.customizeResText + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    try
                    {
                        objConfigcustomized_List = new ConfigurationController();
                        CustomResultM objCustomizedM = new CustomResultM();

                        objCustomizedM.CUSTOMIZED_ID = ControlCustomizeResult.customizeRes_List_ID;
                        objCustomizedM = objConfigcustomized_List.DeleteMasterCustomized_List_Dict(objCustomizedM);
                        //refreshForm();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
                }

            }

            else if (clsControlsData.currentForm == "antibiotic")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete Antibiotic : " + ControlCustomizeResult.customizeResText + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    try
                    {
                        objConfiguration.DeleteAntibiotic(ControlParameter.AntibioticM);

                        refreshForm();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
                }
            }
            else if (clsControlsData.currentForm == "sensitivitypanel")
            {
                if (ControlSensitivitiesPanel.SENPANEL_CODE != null)
                {
                    DialogResult yes = MessageBox.Show("Do you want to delete sensitivitypanel : " + ControlSensitivitiesPanel.SENPANEL_CODE + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (yes == DialogResult.Yes)
                    {
                        try
                        {
                            objConfigSensitivityPanel_List = new ConfigurationController();
                            SenpanelM objSensitivityM = new SenpanelM();

                            objSensitivityM.SENPANEL_ID = ControlSensitivitiesPanel.SENPANEL_ID;
                            objSensitivityM = objConfigSensitivityPanel_List.DeleteSensitivityPanel(objSensitivityM);

                            refreshForm();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }

                    ControlSensitivitiesPanel.SENPANEL_ID = null;
                    ControlSensitivitiesPanel.SENPANEL_CODE = null;
                    ControlSensitivitiesPanel.SENPANEL_SHORTTEXT = null;
                    ControlSensitivitiesPanel.SENPANEL_FULLTEXT = null;
                    ControlSensitivitiesPanel.SENPANEL_METHOD = null;
                    ControlSensitivitiesPanel.SENPANEL_ANTIBIOTIC_ID = null;
                }
            }

            else if (clsControlsData.currentForm == "Battery")
            {
                DialogResult yes = MessageBox.Show("Do you want to delete Battery : " + ControlBattery.BAT_CODE + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yes == DialogResult.Yes)
                {
                    try
                    {
                        // Wait dev for Delete
                        //objOraganism.DeleteSensitivityPanelDict(ControlParameter.SensitivityPanelM);

                        refreshForm();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
                }
            }
            else if (clsControlsData.currentForm == "DetectionTests")
            {

                if (ControlDetectionTest.DETECTION_CODE != null)
                {
                    DialogResult yes = MessageBox.Show("Do you want to delete DetectionTests : " + ControlDetectionTest.DETECTION_CODE + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (yes == DialogResult.Yes)
                    {
                        try
                        {
                            objConfigDetection_Tests = new ConfigurationController();
                            DetectionTestM objDetectionM = new DetectionTestM();

                            objDetectionM.DETECTION_ID = ControlDetectionTest.DETECTION_ID;
                            objDetectionM = objConfigDetection_Tests.DeleteDetectionTests(objDetectionM);

                            refreshForm();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }

                    ControlDetectionTest.DETECTION_ID = null;
                    ControlDetectionTest.DETECTION_CODE = null;
                    ControlDetectionTest.DETECTION_SHORTTEXT = null;
                    ControlDetectionTest.DETECTION_FULLTEXT = null;
                    ControlDetectionTest.DETECTION_PRINT = null;
                    ControlDetectionTest.DETECTION_CUSTOMIZED_ID = null;
                    ControlDetectionTest.DETECTION_CUSTOMIZED_CODE = null;
                    ControlDetectionTest.DETECTION_CUSTOMIZED_NAME = null;
                    ControlDetectionTest.DETECTION_ID = null;

                }
            }
            else if (clsControlsData.currentForm == "ChemistryTests")
            {

                if (ControlChemistryTests.CHEMISTRY_CODE != null)
                {
                    DialogResult yes = MessageBox.Show("Do you want to delete DetectionTests : " + ControlChemistryTests.CHEMISTRY_CODE + " ?", "Delete? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (yes == DialogResult.Yes)
                    {
                        try
                        {
                            objConfigChemistry_Tests = new ConfigurationController();
                            ChemistryM objChemistryM = new ChemistryM();

                            objChemistryM.CHEMISTRY_ID = ControlChemistryTests.CHEMISTRY_ID;
                            objChemistryM = objConfigChemistry_Tests.DeleteChemistryTests(objChemistryM);

                            refreshForm();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }

                    ControlChemistryTests.CHEMISTRY_ID = null;
                    ControlChemistryTests.CHEMISTRY_CODE = null;
                    ControlChemistryTests.CHEMISTRY_SHORTTEXT = null;
                    ControlChemistryTests.CHEMISTRY_FULLTEXT = null;
                    ControlChemistryTests.CHEMISTRY_PRINT = null;
                    ControlChemistryTests.CHEMISTRY_CUSTOMIZED_ID = null;
                    ControlChemistryTests.CHEMISTRY_CUSTOMIZED_CODE = null;
                    ControlChemistryTests.CHEMISTRY_CUSTOMIZED_NAME = null;
                    ControlChemistryTests.CHEMISTRY_ID = null;

                }
            }

        }

        private void refreshForm()
        {
            if (clsControlsData.currentForm == "doctor")
            {
                ConfigurationFld.frmDoctors fm = new ConfigurationFld.frmDoctors();
                fm.Dock = DockStyle.Fill;
                strModule = fm.strModule;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "location")
            {
                ConfigurationFld.frmLocations fm = new ConfigurationFld.frmLocations();
                fm.Dock = DockStyle.Fill;
                strModule = fm.strModule;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "test")
            {
                ConfigurationFld.TestDictionary fm = new ConfigurationFld.TestDictionary();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }

            else if (clsControlsData.currentForm == "Antibiotic_Fams")
            {
                ConfigurationFld.Antibiotic_Fams fm = new ConfigurationFld.Antibiotic_Fams();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }

            else if (clsControlsData.currentForm == "specimen")
            {
                ConfigurationFld.Specimen fm = new ConfigurationFld.Specimen();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "specimen Group")
            {
                ConfigurationFld.Specimen_Group fm = new ConfigurationFld.Specimen_Group();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }

            else if (clsControlsData.currentForm == "protocol")
            {
                ConfigurationFld.Protocol fm = new ConfigurationFld.Protocol();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }

            else if (clsControlsData.currentForm == "media")
            {
                ConfigurationFld.Media fm = new ConfigurationFld.Media();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "customizedresult" || clsControlsData.currentForm == "Customized_Result" || clsControlsData.currentForm == "Customized_List")
            {
                ConfigurationFld.CustomizeResult fm = new ConfigurationFld.CustomizeResult();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "organism")
            {
                ConfigurationFld.Organism fm = new ConfigurationFld.Organism();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "antibiotic")
            {
                ConfigurationFld.Antibitotic fm = new ConfigurationFld.Antibitotic();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "stain")
            {
                ConfigurationFld.stain  fm = new ConfigurationFld.stain();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "sensitivitypanel")
            {
                ConfigurationFld.SensitivitiesPanel fm = new ConfigurationFld.SensitivitiesPanel();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "colonydescription")
            {
                ConfigurationFld.frmColony fm = new ConfigurationFld.frmColony();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "Battery")
            {
                ConfigurationFld.Battery fm = new ConfigurationFld.Battery();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "SystemMGNT")
            {
                ConfigurationFld.SystemMGNT fm = new ConfigurationFld.SystemMGNT();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "DetectionTests")
            {
                ConfigurationFld.Detection_test fm = new ConfigurationFld.Detection_test();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                fm.EnableDupButton = enableDuplicateButton;
                fm.checkEnableButton = checkEnableButton;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "ChemistryTests")
            {
                ConfigurationFld.Chemistry fm = new ConfigurationFld.Chemistry();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                fm.EnableDupButton = enableDuplicateButton;
                fm.checkEnableButton = checkEnableButton;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "BreakPoint")
            {
                ConfigurationFld.BreakPoints fm = new ConfigurationFld.BreakPoints();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "Organisms_Fams")
            {
                ConfigurationFld.Organisms_Fams fm = new ConfigurationFld.Organisms_Fams();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "Users")
            {
                ConfigurationFld.USERS fm = new ConfigurationFld.USERS();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }
            else if (clsControlsData.currentForm == "Role")
            {
                ConfigurationFld.USERS_ROLE fm = new ConfigurationFld.USERS_ROLE();
                strModule = fm.strModule;
                fm.Dock = DockStyle.Fill;
                pcMain.Controls.Clear();
                pcMain.Controls.Add(fm);
            }


            //
        }

        private void barButtonItem7_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        // Button EDIT
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (clsControlsData.currentForm == "doctor")
            {
                ConfigurationFld.frmAddDoctors fm = new ConfigurationFld.frmAddDoctors("edit");
                fm.yourAction = refreshForm;
                fm.ShowDialog();

            }
            else if (clsControlsData.currentForm == "location")
            {

                ConfigurationFld.AddLocations fm = new ConfigurationFld.AddLocations("edit",ControlLocation.LocID);
                    fm.yourAction = refreshForm;
                    fm.ShowDialog();

                
            }
            else if (clsControlsData.currentForm == "test")
            {

                ConfigurationFld.frmAddTestDictionary fm = new ConfigurationFld.frmAddTestDictionary("edit", ControlTestDictionary.TestID);
                fm.yourAction = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "Antibiotic_Fams")
            {

                ConfigurationFld.frmAddAntibiotic_FAM fm = new ConfigurationFld.frmAddAntibiotic_FAM("edit", ControlAntibioticFam.ANTIBIOTIC_FAMS_ID);
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "Organisms_Fams")
            {
                if (ControlOrganismsFam.OrganismsFamID != null)
                {
                    ConfigurationFld.frmAddOrganism_FAM fm = new ConfigurationFld.frmAddOrganism_FAM("edit", ControlOrganismsFam.Organisms_FAMS_ID);
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
                }
            }

            else if (clsControlsData.currentForm == "antibiotic")
            {

                ConfigurationFld.frmAddAntibiotic fm = new ConfigurationFld.frmAddAntibiotic("edit", ControlAntibiotic.AntibioticID);
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }

            else if (clsControlsData.currentForm == "specimen")
            {

                ConfigurationFld.frmAddSpecimen fm = new ConfigurationFld.frmAddSpecimen("edit",ControlSpecimen.COLLMATERIALID);
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "specimen Group")
            {

                ConfigurationFld.frmAddSpecimenGroup fm = new ConfigurationFld.frmAddSpecimenGroup("edit", ControlSpecimenGroup.SPM_GROUP_ID);
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }


            else if (clsControlsData.currentForm == "protocol")
            {

                ConfigurationFld.frmAddProtocol fm = new ConfigurationFld.frmAddProtocol("edit",ControlProtocol.ProtocolID);
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "media")
            {

                ConfigurationFld.frmAddMedia fm = new ConfigurationFld.frmAddMedia("edit", ControlAgars.AgarID);
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "customizedresult")
            {
                if (clsControlsData.currentForm == "customizedresult")
                {
                    MessageBox.Show("Please select Group!");
                }
                //ConfigurationFld.frmAddCustomizedResult fm = new ConfigurationFld.frmAddCustomizedResult("edit", ControlCustomizeResult.customizeResID);
                //fm.refreshData = refreshForm;
                //fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "Customized_List")
            {
                ConfigurationFld.frmCustomize_LIST fm = new ConfigurationFld.frmCustomize_LIST("edit", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "Customized_Result")
            {
                ConfigurationFld.frmCustomize_RESULT fm = new ConfigurationFld.frmCustomize_RESULT("edit","");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }

            else if (clsControlsData.currentForm == "stain")
            {

                ConfigurationFld.frmAddStain fm = new ConfigurationFld.frmAddStain("edit", ControlStain.stainID.ToString());
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "organism")
            {
                ConfigurationFld.frmAddOrganism fm = new ConfigurationFld.frmAddOrganism("edit", "");
            //    fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "Battery")
            {
                ConfigurationFld.frmAddBattery fm = new ConfigurationFld.frmAddBattery("edit", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "DetectionTests")
            {
                if (ControlDetectionTest.DETECTION_ID != null)
                {
                    ConfigurationFld.frmAddDetectionTests fm = new ConfigurationFld.frmAddDetectionTests("edit", "");
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
                }
            }
            else if (clsControlsData.currentForm == "ChemistryTests")
            {
                if (ControlChemistryTests.CHEMISTRY_ID != null)
                {
                    ConfigurationFld.frmAddChemistryTests fm = new ConfigurationFld.frmAddChemistryTests("edit", "");
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
                }
            }
            else if (clsControlsData.currentForm == "sensitivitypanel")
            {
                //if (ControlSensitivitiesPanel.SENPANEL_ID != null)
                //{
                    ConfigurationFld.frmAddSensitivityPanel fm = new ConfigurationFld.frmAddSensitivityPanel("edit", "");
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
                //}
            }
            else if (clsControlsData.currentForm == "BreakPoint")
            {
                    ConfigurationFld.frmBreakpoint fm = new ConfigurationFld.frmBreakpoint("edit", "");
                    //fm.refreshData = refreshForm;
                    fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "Users")
            {

                ConfigurationFld.frmAddUSERS fm = new ConfigurationFld.frmAddUSERS("edit", ControlUser_Role.USERID);
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "Role")
            {

                ConfigurationFld.frmAddUSERS_ROLE fm = new ConfigurationFld.frmAddUSERS_ROLE("edit", ControlUser_Role.ROLEID);
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }

        }

        private void ribbonPageGroup4_CaptionButtonClick(object sender, DevExpress.XtraBars.Ribbon.RibbonPageGroupEventArgs e)
        {
        //    this.Close();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            refreshForm();
        }

        private void frmConfigurationMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsControlsData.currentForm = null;
        }

        // Duplicate Function
        private void btnDupplicate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (clsControlsData.currentForm == "doctor")
            {
                ConfigurationFld.frmAddDoctors fm = new ConfigurationFld.frmAddDoctors("dup");
                fm.yourAction = refreshForm;
                fm.ShowDialog();

            }else if (clsControlsData.currentForm == "location")
            {
                ConfigurationFld.AddLocations fm = new ConfigurationFld.AddLocations("dup", "");
                fm.yourAction = refreshForm;
                fm.ShowDialog();

            }
            else if (clsControlsData.currentForm == "test")
            {
                ConfigurationFld.frmAddTestDictionary fm = new ConfigurationFld.frmAddTestDictionary("dup", "");
                fm.yourAction = refreshForm;
                fm.ShowDialog();

            }
            else if (clsControlsData.currentForm == "Antibiotic_Fams")
            {
                ConfigurationFld.frmAddAntibiotic_FAM fm = new ConfigurationFld.frmAddAntibiotic_FAM("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }
            else if (clsControlsData.currentForm == "antibiotic")
            {
                ConfigurationFld.frmAddAntibiotic fm = new ConfigurationFld.frmAddAntibiotic("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }

            else if (clsControlsData.currentForm == "Organisms_Fams")
            {
                if (ControlOrganismsFam.OrganismsFamID != null)
                {
                    ConfigurationFld.frmAddOrganism_FAM fm = new ConfigurationFld.frmAddOrganism_FAM("dup", "");
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
                }
            }
            else if (clsControlsData.currentForm == "specimen")
            {
                ConfigurationFld.frmAddSpecimen fm = new ConfigurationFld.frmAddSpecimen("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }
            else if (clsControlsData.currentForm == "specimen Group")
            {
                ConfigurationFld.frmAddSpecimenGroup fm = new ConfigurationFld.frmAddSpecimenGroup("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();

            }

            else if (clsControlsData.currentForm == "protocol")
            {
                ConfigurationFld.frmAddProtocol fm = new ConfigurationFld.frmAddProtocol("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "sensitivitypanel")
            {
                if (ControlSensitivitiesPanel.SENPANEL_ID != null)
                {

                    ConfigurationFld.frmAddSensitivityPanel fm = new ConfigurationFld.frmAddSensitivityPanel("dup", "");
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
                }
            }
            else if (clsControlsData.currentForm == "Battery")
            {
                ConfigurationFld.frmAddBattery fm = new ConfigurationFld.frmAddBattery("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "Customized_List")
            {
                ConfigurationFld.frmCustomize_LIST fm = new ConfigurationFld.frmCustomize_LIST("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "Customized_Result")
            {
                ConfigurationFld.frmCustomize_RESULT fm = new ConfigurationFld.frmCustomize_RESULT("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "DetectionTests")
            {
                if (ControlDetectionTest.DETECTION_ID != null)
                {
                    ConfigurationFld.frmAddDetectionTests fm = new ConfigurationFld.frmAddDetectionTests("dup", "");
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
                }
            }
            else if (clsControlsData.currentForm == "ChemistryTests")
            {
                if (ControlChemistryTests.CHEMISTRY_ID != null)
                {
                    ConfigurationFld.frmAddChemistryTests fm = new ConfigurationFld.frmAddChemistryTests("dup", "");
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
                }
            }
            else if (clsControlsData.currentForm == "BreakPoint")
            {
                    ConfigurationFld.frmBreakpoint fm = new ConfigurationFld.frmBreakpoint("dup", "");
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "organism")
            {
                ConfigurationFld.frmAddOrganism fm = new ConfigurationFld.frmAddOrganism("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "stain")
            {
                if (ControlStain.stainID.ToString() != null)
                {
                    ConfigurationFld.frmAddStain fm = new ConfigurationFld.frmAddStain("dup", "");
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
                }
            }
            else if (clsControlsData.currentForm == "media")
            {
                if (ControlAgars.AgarID.ToString() != null)
                {
                    ConfigurationFld.frmAddMedia fm = new ConfigurationFld.frmAddMedia("dup", "");
                    fm.refreshData = refreshForm;
                    fm.ShowDialog();
                }
            }
            else if (clsControlsData.currentForm == "Users")
            {
                ConfigurationFld.frmAddUSERS fm = new ConfigurationFld.frmAddUSERS("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }
            else if (clsControlsData.currentForm == "Role")
            {
                ConfigurationFld.frmAddUSERS_ROLE fm = new ConfigurationFld.frmAddUSERS_ROLE("dup", "");
                fm.refreshData = refreshForm;
                fm.ShowDialog();
            }

        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.frmLocations fm = new ConfigurationFld.frmLocations();

            clsControlsData.currentForm = "location";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);

        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.TestDictionary fm = new ConfigurationFld.TestDictionary();

            clsControlsData.currentForm = "test";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void btnAntibioticGroup_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.Antibiotic_Fams fm = new ConfigurationFld.Antibiotic_Fams();

            clsControlsData.currentForm = "Antibiotic_Fams";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void btnAntibiotic_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.Antibitotic fm = new ConfigurationFld.Antibitotic();

            clsControlsData.currentForm = "antibiotic";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void btnSpecimen_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.Specimen fm = new ConfigurationFld.Specimen();

            clsControlsData.currentForm = "specimen";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }
        private void btnSpecimen_group_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.Specimen_Group fm = new ConfigurationFld.Specimen_Group();

            clsControlsData.currentForm = "specimen Group";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }


        private void btnProtocol_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.Protocol fm = new ConfigurationFld.Protocol();

            clsControlsData.currentForm = "protocol";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navMedia_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.Media fm = new ConfigurationFld.Media();

            clsControlsData.currentForm = "media";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navCustomizeRes_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.CustomizeResult fm = new ConfigurationFld.CustomizeResult();

            clsControlsData.currentForm = "customizedresult";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.stain fm = new ConfigurationFld.stain();

            clsControlsData.currentForm = "stain";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navBarItem11_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.CustomizeResult fm = new ConfigurationFld.CustomizeResult();

            clsControlsData.currentForm = "customizedresult";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.Organism fm = new ConfigurationFld.Organism();

            clsControlsData.currentForm = "organism";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navSensitivityPanel_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.SensitivitiesPanel fm = new ConfigurationFld.SensitivitiesPanel();
            clsControlsData.currentForm = "sensitivitypanel";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navColony_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.frmColony fm = new ConfigurationFld.frmColony();
            clsControlsData.currentForm = "colonydescription";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void btnBattery_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.Battery fm = new ConfigurationFld.Battery();
            clsControlsData.currentForm = "Battery";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navSystemManagement_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.SystemMGNT fm = new ConfigurationFld.SystemMGNT();

            clsControlsData.currentForm = "SystemMGNT";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            pcMain.Controls.Add(fm);
        }

        private void navDetect_Test_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.Detection_test fm = new ConfigurationFld.Detection_test();
            clsControlsData.currentForm = "DetectionTests";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navBiochemistry_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.Chemistry fm = new ConfigurationFld.Chemistry();
            clsControlsData.currentForm = "ChemistryTests";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navBreakpoint_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.BreakPoints fm = new ConfigurationFld.BreakPoints();

            clsControlsData.currentForm = "BreakPoint";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navOrganismFamily_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //Organisms_Fams
            ConfigurationFld.Organisms_Fams fm = new ConfigurationFld.Organisms_Fams();
            clsControlsData.currentForm = "Organisms_Fams";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navBarItem1_LinkClicked_2(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //Expert rule control
            ConfigurationFld.Expert_rule fm = new ConfigurationFld.Expert_rule();
            clsControlsData.currentForm = "Expert_rule";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);

        }

        private void navBarItem2_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.USERS fm = new ConfigurationFld.USERS();

            clsControlsData.currentForm = "Users";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }

        private void navBarItem3_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ConfigurationFld.USERS_ROLE fm = new ConfigurationFld.USERS_ROLE();

            clsControlsData.currentForm = "Role";
            fm.Dock = DockStyle.Fill;
            pcMain.Controls.Clear();
            fm.EnableDupButton = enableDuplicateButton;
            fm.checkEnableButton = checkEnableButton;
            pcMain.Controls.Add(fm);
        }
    }
}
