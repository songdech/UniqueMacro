using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddProtocol : Form
    {        
        private string moduleType;
        private string strProtocolID;
        private SystemController objSystemControl;
        private ProtocalM objProtocalM; 

        private ConfigurationController objConfig;

        int rowindex = 0;

        object SPM01;   //SpecimenG ID
        object SPM02;   //SpecimenG Name
        object SPM03;   //SpecimenG Name

        // STAIN
        object STN01;   //Stain ID
        object STN02;   //Stain name
        object STN03;    //Stain code

        // SENSITIVITY PANEL
        object IDEN01;   //IDEN ID
        object IDEN02;   //IDEN name
        object IDEN03;   //IDEN code

        // MEDIA
        object MEDIA01;   //Media ID
        object MEDIA02;   //Media name
        object MEDIA03;    //Media code

        // ANTIBIOTIC
        object ANTIBIOTIC01;   //Media ID
        object ANTIBIOTIC02;   //Media name
        object ANTIBIOTIC03;    //Media code

        // SAMPLE 
        object SAMPLE01;   //SAMPLE ID
        object SAMPLE02;   //SAMPLE name
        object SAMPLE03;   //SAMPLE code

        // TOPO 
        object TOPO01;   //TOPO ID
        object TOPO02;   //TOPO name
        object TOPO03;   //TOPO code

        // CHEM 
        object CHEM01;   //CHEM ID
        object CHEM02;   //CHEM name
        object CHEM03;   //CHEM code

        // CYTO 
        object CYTO01;   //CYTO ID
        object CYTO02;   //CYTO name
        object CYTO03;   //CYTO code

        public frmAddProtocol(string moduleType, string strProtocolID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.strProtocolID = strProtocolID;

            objSystemControl = new SystemController();
            objConfig = new ConfigurationController();
            objProtocalM = new ProtocalM();

            if (moduleType != "protocol")
            {
                objProtocalM = ControlParameter.ProtocalM;
            }
        }

        public Action refreshData { get; set; }
        private void frmAddProtocol_Load(object sender, EventArgs e)
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
            DataTable dt = null;

            try
            {
                if (moduleType == "edit")
                {
                    // Query Specimen Group to gridLookup
                    txtCode.Text = objProtocalM.PROTOCOLCODE;
                    txtName.Text = objProtocalM.PROTOCOL_NAME;
                    txtComment.Text = objProtocalM.PROTOCOL_DESCRIPTION;

                    objProtocalM.PROTOCOL_LOGUSERID = ControlParameter.loginID.ToString();

                    if (objProtocalM.REPORTFORMA == 1)
                    {
                        comboBoxEdit1.SelectedIndex = 0;
                    }
                    else if (objProtocalM.REPORTFORMA == 2)
                    {
                        comboBoxEdit1.SelectedIndex = 1;
                    }
                    else if (objProtocalM.REPORTFORMA == 3)
                    {
                        comboBoxEdit1.SelectedIndex = 2;
                    }
                    else if (objProtocalM.REPORTFORMA == 4)
                    {
                        comboBoxEdit1.SelectedIndex = 3;
                    }
                    else if (objProtocalM.REPORTFORMA == 5)
                    {
                        comboBoxEdit1.SelectedIndex = 4;
                    }

                    // Find ProtocolStain in Dictionary

                    objProtocalM.PROTOCOLID = Convert.ToInt16(ControlProtocol.ProtocolID);

                    Get_ProtocolSpecimenGroup_(objProtocalM);
                    Get_ProtocolStain_(objProtocalM);
                    //Get_ProtocolIden_(objProtocalM);
                    Get_ProtocolMedia_(objProtocalM);
                    //Get_ProtocolAntibiotic_(objProtocalM);

                    label6.Visible = true;
                    label6.Text = "Edit Protocol";
                    this.Text = "Edit Protocol";

                }
                else if (moduleType == "dup")
                {

                    txtName.Text = objProtocalM.PROTOCOL_NAME;
                    txtComment.Text = objProtocalM.PROTOCOL_DESCRIPTION;

                    objProtocalM.PROTOCOL_LOGUSERID = ControlParameter.loginID.ToString();

                    if (objProtocalM.REPORTFORMA == 1)
                    {
                        comboBoxEdit1.SelectedIndex = 0;
                    }
                    else if (objProtocalM.REPORTFORMA == 2)
                    {
                        comboBoxEdit1.SelectedIndex = 1;
                    }
                    else if (objProtocalM.REPORTFORMA == 3)
                    {
                        comboBoxEdit1.SelectedIndex = 2;
                    }
                    else if (objProtocalM.REPORTFORMA == 4)
                    {
                        comboBoxEdit1.SelectedIndex = 3;
                    }
                    else if (objProtocalM.REPORTFORMA == 5)
                    {
                        comboBoxEdit1.SelectedIndex = 4;
                    }

                    objProtocalM.PROTOCOLID = Convert.ToInt16(ControlProtocol.ProtocolID);

                    //Get_ProtocolSpecimenGroup_(objProtocalM);
                    //Get_ProtocolStain_(objProtocalM);
                    //Get_ProtocolMedia_(objProtocalM);

                    label6.Visible = true;
                    label6.Text = "Duplicate Protocol";
                    this.Text = "Duplicate Protocol";

                }
                else if (moduleType == "add")
                {
                    txtCode.Select();

                    // Query Report Format in comboboxEdit

                    dt = objSystemControl.GetLookupField(UniqueController.LOOKUP_REPORTFORMAT);
                    comboBoxEdit1.Properties.Items.Clear();
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        comboBoxEdit1.Properties.Items.Add(dt.Rows[i]["LOOKUP_DISPLAY_ENG"].ToString());
                        comboBoxEdit1.SelectedIndex = 0;
                    }
                    // End ComboboxEdit Report format

                    objProtocalM.PROTOCOL_LOGUSERID = ControlParameter.loginID.ToString();

                    label6.Visible = true;
                    label6.Text = "Add Protocol";
                    this.Text = "Add Protocol";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // G1  Specimen
        private void Get_ProtocolSpecimenGroup_(ProtocalM objProtocolID)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            ProtocalM objProtocolM = new ProtocalM();

            try
            {
                dt = objConfig.GetGridviewSpecimenGroup(objProtocolID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView1.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objProtocolM = null;
            }
        }
        // G2 Sample
        private void Get_ProtocolSampleMaterial_(ProtocalM objProtocolID)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            ProtocalM objProtocolM = new ProtocalM();

            try
            {
                dt = objConfig.GetProtocalWithStain(objProtocolID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView2.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView2.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objProtocolM = null;
            }
        }
        // G3 Topography
        private void Get_ProtocolTopography_(ProtocalM objProtocolID)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            ProtocalM objProtocolM = new ProtocalM();

            try
            {
                dt = objConfig.GetProtocalWithStain(objProtocolID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView3.Rows.Add();
                        dataGridView3.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView3.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView3.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objProtocolM = null;
            }
        }
        // G8 Chemistries
        private void Get_ProtocolChemisties_(ProtocalM objProtocolID)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            ProtocalM objProtocolM = new ProtocalM();

            try
            {
                dt = objConfig.GetProtocalWithStain(objProtocolID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView8.Rows.Add();
                        dataGridView8.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView8.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView8.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objProtocolM = null;
            }
        }
        // G9 Cytology
        private void Get_ProtocolCytology_(ProtocalM objProtocolID)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            ProtocalM objProtocolM = new ProtocalM();

            try
            {
                dt = objConfig.GetProtocalWithStain(objProtocolID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView9.Rows.Add();
                        dataGridView9.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView9.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView9.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objProtocolM = null;
            }
        }

        // G4 Stain
        private void Get_ProtocolStain_(ProtocalM objProtocolID)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            ProtocalM objProtocolM = new ProtocalM();

            try
            {
                dt = objConfig.GetProtocalWithStain(objProtocolID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView4.Rows.Add();
                        dataGridView4.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView4.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView4.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objProtocolM = null;
            }
        }
        // G5 Iden & sensitivity
        private void Get_ProtocolIden_(ProtocalM objProtocolID)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            ProtocalM objProtocolM = new ProtocalM();

            try
            {
                dt = objConfig.GetProtocalWithIden(objProtocolID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView5.Rows.Add();
                        dataGridView5.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView5.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView5.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objProtocolM = null;
            }
        }
        // G6 Media
        private void Get_ProtocolMedia_(ProtocalM objProtocolID)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            ProtocalM objProtocolM = new ProtocalM();

            try
            {
                dt = objConfig.GetProtocalWithMedia(objProtocolID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView6.Rows.Add();
                        dataGridView6.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView6.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView6.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objProtocolM = null;
            }
        }
        // G7 Antibiotic
        private void Get_ProtocolAntibiotic_(ProtocalM objProtocolID)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            ProtocalM objProtocolM = new ProtocalM();

            try
            {
                dt = objConfig.GetProtocalWithAntibiotic(objProtocolID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView7.Rows.Add();
                        dataGridView7.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView7.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView7.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objProtocolM = null;
            }
        }

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                bool Second;
                bool Thirth;
                if (moduleType == "edit")
                {
                    try
                    {
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            try
                            {
                                objProtocalM.PROTOCOLCODE = txtCode.Text;
                                txtCode.ReadOnly = true;
                                objProtocalM.PROTOCOLTEXT = txtName.Text;
                                objProtocalM.Comment = txtComment.Text;
                                //objProtocalM.PROTOCOL_SPMGROUPID = Convert.ToInt16(txtSPMG_ID.Text);

                                if (comboBoxEdit1.SelectedIndex == 0)
                                {
                                    objProtocalM.REPORTFORMA = 1;
                                }
                                else if (comboBoxEdit1.SelectedIndex == 1)
                                {
                                    objProtocalM.REPORTFORMA = 2;
                                }
                                else if (comboBoxEdit1.SelectedIndex == 2)
                                {
                                    objProtocalM.REPORTFORMA = 3;
                                }
                                else if (comboBoxEdit1.SelectedIndex == 3)
                                {
                                    objProtocalM.REPORTFORMA = 4;
                                }
                                else if (comboBoxEdit1.SelectedIndex == 4)
                                {
                                    objProtocalM.REPORTFORMA = 5;
                                }

                                // DataGrid Specimen Group

                                if (dataGridView1.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        objProtocalM.PROTOCOL_SPMGROUPID = row.Cells[2].Value.ToString();
                                    }
                                }
                                else
                                {
                                    objProtocalM.PROTOCOL_SPMGROUPID = "";
                                }

                                objProtocalM = objConfig.SaveProtocalDict(objProtocalM);

                                StainM objStainM = new StainM();
                                // Delete if Module = Edit Protocolstain if exist in database

                                // DataGrid Stain select
                                if (dataGridView4.Rows.Count > 0)
                                {
                                    objProtocalM = objConfig.DeleteProtocolStainDict(objProtocalM);

                                    foreach (DataGridViewRow row in dataGridView4.Rows)
                                    {
                                        objStainM.MBSTAINID = Convert.ToUInt16(row.Cells[2].Value.ToString());
                                        objProtocalM = objConfig.SaveProtocolStainDict(objProtocalM, objStainM);
                                    }
                                }
                                else
                                {
                                    objProtocalM = objConfig.DeleteProtocolStainDict(objProtocalM);
                                }

                                // DataGrid Media select
                                if (dataGridView6.Rows.Count > 0)
                                {
                                    objProtocalM = objConfig.DeleteProtocolMediaDict(objProtocalM);

                                    foreach (DataGridViewRow row in dataGridView6.Rows)
                                    {
                                        objProtocalM.PROTOCOL_MEDIA_ID = Convert.ToUInt16(row.Cells[2].Value.ToString());
                                        objProtocalM.PROTOCOL_MEDIA_INDEX = row.Index + 1;
                                        objProtocalM = objConfig.SaveProtocolMediaDict(objProtocalM);
                                    }
                                }
                                else
                                {
                                    objProtocalM = objConfig.DeleteProtocolMediaDict(objProtocalM);
                                }

                                this.Close();
                                Action instance = refreshData;
                                if (instance != null)
                                    instance();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [edit]", ex.Message);
                    }
                }
                else if (moduleType == "dup")
                {
                    try
                    {
                        // Code & Name  Fix do not empty!
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            // Charector need to 3 Digit only after Save
                            int CharProtocol = txtCode.Text.Length;
                            if (CharProtocol == 3)
                            {
                                string FirstDigit_Code = txtCode.Text.Substring(0, 1);  //Start protocol name to [P]
                                string SeconDigit_Code = txtCode.Text.Substring(1, 1);  //2 Number protocol digit only
                                string ThirthDigit_Code = txtCode.Text.Substring(2, 1); //3 Number protocol digit only

                                Second = IsNumbericOnly(SeconDigit_Code);
                                Thirth = IsNumbericOnly(ThirthDigit_Code);

                                if (FirstDigit_Code == "P" && Second == true && Thirth == true)
                                {
                                    bool ProtocolCode;

                                    objProtocalM.PROTOCOLCODE = txtCode.Text;
                                    objProtocalM.PROTOCOLTEXT = txtName.Text;
                                    objProtocalM.Comment = txtComment.Text;

                                    ProtocolCode = objConfig.CheckExistProtocalDict(objProtocalM);

                                    if (ProtocolCode)
                                    {
                                        MessageBox.Show("Duplicate Code exist please use another code!!");
                                    }
                                    else
                                    {
                                        if (comboBoxEdit1.SelectedIndex == 0)
                                        {
                                            objProtocalM.REPORTFORMA = 1;
                                        }
                                        else if (comboBoxEdit1.SelectedIndex == 1)
                                        {
                                            objProtocalM.REPORTFORMA = 2;
                                        }
                                        else if (comboBoxEdit1.SelectedIndex == 2)
                                        {
                                            objProtocalM.REPORTFORMA = 3;
                                        }
                                        else if (comboBoxEdit1.SelectedIndex == 3)
                                        {
                                            objProtocalM.REPORTFORMA = 4;
                                        }
                                        else if (comboBoxEdit1.SelectedIndex == 4)
                                        {
                                            objProtocalM.REPORTFORMA = 5;
                                        }


                                        // DataGrid Specimen Group

                                        if (dataGridView1.Rows.Count > 0)
                                        {
                                            foreach (DataGridViewRow row in dataGridView1.Rows)
                                            {
                                                objProtocalM.PROTOCOL_SPMGROUPID = row.Cells[2].Value.ToString();
                                            }
                                        }
                                        else
                                        {
                                            objProtocalM.PROTOCOL_SPMGROUPID = "";
                                        }

                                        objProtocalM = objConfig.SaveDupProtocalDict(objProtocalM);
                                        StainM objStainM = new StainM();

                                        if (dataGridView4.Rows.Count > 0)
                                        {
                                            foreach (DataGridViewRow row in dataGridView4.Rows)
                                            {
                                                objStainM.MBSTAINID = Convert.ToUInt16(row.Cells[2].Value.ToString());
                                                objProtocalM = objConfig.SaveProtocolStainDict(objProtocalM, objStainM);
                                            }
                                        }

                                        if (dataGridView6.Rows.Count > 0)
                                        {
                                            foreach (DataGridViewRow row in dataGridView6.Rows)
                                            {
                                                objProtocalM.PROTOCOL_MEDIA_ID = Convert.ToUInt16(row.Cells[2].Value.ToString());
                                                objProtocalM.PROTOCOL_MEDIA_INDEX = row.Index + 1;
                                                objProtocalM = objConfig.SaveProtocolMediaDict(objProtocalM);
                                            }
                                        }


                                        this.Close();
                                        Action instance = refreshData;
                                        if (instance != null)
                                            instance();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please check code is not correct!!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("All charector need to 3 digit");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [dup]","Warning" + ex.Message);
                    }
                }
                else if (moduleType == "add")
                {
                    try
                    {
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            // Charector need to 3 Digit only after Save
                            int CharProtocol = txtCode.Text.Length;
                            if (CharProtocol == 3)
                            {
                                string FirstDigit_Code = txtCode.Text.Substring(0, 1);  //Start protocol name to [P]
                                string SeconDigit_Code = txtCode.Text.Substring(1, 1);  //2 Number protocol digit only
                                string ThirthDigit_Code = txtCode.Text.Substring(2, 1); //3 Number protocol digit only

                                Second = IsNumbericOnly(SeconDigit_Code);
                                Thirth = IsNumbericOnly(ThirthDigit_Code);

                                if (FirstDigit_Code == "P" && Second == true && Thirth == true)
                                {
                                    bool ProtocolCode;

                                    objProtocalM.PROTOCOLCODE = txtCode.Text;
                                    objProtocalM.PROTOCOLTEXT = txtName.Text;
                                    objProtocalM.Comment = txtComment.Text;
                                    //objProtocalM.PROTOCOL_SPMGROUPID = Convert.ToInt16(txtSPMG_ID.Text);

                                    ProtocolCode = objConfig.CheckExistProtocalDict(objProtocalM);

                                    if (ProtocolCode)
                                    {
                                        MessageBox.Show("Add Code exist please use another code!!");
                                    }
                                    else
                                    {
                                        if (comboBoxEdit1.SelectedIndex == 0)
                                        {
                                            MessageBox.Show("Please select Report format : We'll set Default = Culture");
                                            objProtocalM.REPORTFORMA = 1;
                                        }
                                        else if (comboBoxEdit1.SelectedIndex == 1)
                                        {
                                            objProtocalM.REPORTFORMA = 2;
                                        }
                                        else if (comboBoxEdit1.SelectedIndex == 2)
                                        {
                                            objProtocalM.REPORTFORMA = 3;
                                        }
                                        else if (comboBoxEdit1.SelectedIndex == 3)
                                        {
                                            objProtocalM.REPORTFORMA = 4;
                                        }
                                        else if (comboBoxEdit1.SelectedIndex == 4)
                                        {
                                            objProtocalM.REPORTFORMA = 5;
                                        }


                                        // DataGrid Specimen Group

                                        if (dataGridView1.Rows.Count > 0)
                                        {
                                            foreach (DataGridViewRow row in dataGridView1.Rows)
                                            {
                                                objProtocalM.PROTOCOL_SPMGROUPID = row.Cells[2].Value.ToString();
                                            }
                                        }
                                        else
                                        {
                                            objProtocalM.PROTOCOL_SPMGROUPID = "";
                                        }

                                        objProtocalM = objConfig.SaveDupProtocalDict(objProtocalM);

                                        StainM objStainM = new StainM();

                                        if (dataGridView4.Rows.Count > 0)
                                        {
                                            foreach (DataGridViewRow row in dataGridView4.Rows)
                                            {
                                                objStainM.MBSTAINID = Convert.ToUInt16(row.Cells[2].Value.ToString());
                                                objProtocalM = objConfig.SaveProtocolStainDict(objProtocalM, objStainM);
                                            }
                                        }

                                        if (dataGridView6.Rows.Count > 0)
                                        {
                                            //int AGARINDEX = 0;
                                            foreach (DataGridViewRow row in dataGridView6.Rows)
                                            {
                                                //AGARINDEX++;
                                                objProtocalM.PROTOCOL_MEDIA_ID = Convert.ToUInt16(row.Cells[2].Value.ToString());
                                                objProtocalM.PROTOCOL_MEDIA_INDEX = row.Index + 1;
                                                objProtocalM = objConfig.SaveProtocolMediaDict(objProtocalM);
                                            }
                                        }

                                        this.Close();

                                        Action instance = refreshData;
                                        if (instance != null)
                                            instance();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please check code is not correct!!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("All charector need to 3 digit");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [add]", ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Error Function Save Desc.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        bool IsNumbericOnly(string strProtocol)
        {
            foreach (char A in strProtocol)
            {
                if (A < '0' || A > '9')
                    return false;
            }
            return true;
        }
        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }
        private void Btn_search_G4_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetGridviewStain();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objProtocalM.PROTOCOL_STAIN_ID = fm.SelectedID.ToString();
                    objProtocalM.PROTOCOL_STAIN_NAME = fm.SelectedName;
                    objProtocalM.PROTOCOL_STAIN_CODE = fm.SelectedCode;

                    STN01 = objProtocalM.PROTOCOL_STAIN_CODE;
                    STN02 = objProtocalM.PROTOCOL_STAIN_NAME;
                    STN03 = objProtocalM.PROTOCOL_STAIN_ID;

                    int minRowCount = 0;
                    bool CheckCustomizedResult = false;
                    for (int i = 0; i < dataGridView4.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView4.Rows.Add();
                        dataGridView4.Rows[dataGridView4.RowCount - 1].Cells[0].Value = STN01;
                        dataGridView4.Rows[dataGridView4.RowCount - 1].Cells[1].Value = STN02;
                        dataGridView4.Rows[dataGridView4.RowCount - 1].Cells[2].Value = STN03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView4.Rows)
                        {
                            if (row.Cells[2].Value.ToString() == (STN03.ToString()))
                            {
                                CheckCustomizedResult = true;
                            }
                        }
                        if (CheckCustomizedResult == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckCustomizedResult == false)
                        {
                            dataGridView4.Rows.Add();
                            dataGridView4.Rows[dataGridView4.RowCount - 1].Cells[0].Value = STN01;
                            dataGridView4.Rows[dataGridView4.RowCount - 1].Cells[1].Value = STN02;
                            dataGridView4.Rows[dataGridView4.RowCount - 1].Cells[2].Value = STN03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }
        }
        private void Btn_search_G5_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetGridviewIden();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objProtocalM.PROTOCOL_IDEN_ID = fm.SelectedID.ToString();
                    objProtocalM.PROTOCOL_IDEN_NAME = fm.SelectedName;
                    objProtocalM.PROTOCOL_IDEN_CODE = fm.SelectedCode;

                    IDEN01 = objProtocalM.PROTOCOL_IDEN_CODE;
                    IDEN02 = objProtocalM.PROTOCOL_IDEN_NAME;
                    IDEN03 = objProtocalM.PROTOCOL_IDEN_ID;

                    int minRowCount = 0;
                    bool CheckCustomizedResult = false;
                    for (int i = 0; i < dataGridView5.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView5.Rows.Add();
                        dataGridView5.Rows[dataGridView5.RowCount - 1].Cells[0].Value = IDEN01;
                        dataGridView5.Rows[dataGridView5.RowCount - 1].Cells[1].Value = IDEN02;
                        dataGridView5.Rows[dataGridView5.RowCount - 1].Cells[2].Value = IDEN03;
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView5.Rows)
                        {
                            if (row.Cells[2].Value.ToString() == (IDEN03.ToString()))
                            {
                                CheckCustomizedResult = true;
                            }
                        }
                        if (CheckCustomizedResult == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckCustomizedResult == false)
                        {
                            dataGridView5.Rows.Add();
                            dataGridView5.Rows[dataGridView5.RowCount - 1].Cells[0].Value = IDEN01;
                            dataGridView5.Rows[dataGridView5.RowCount - 1].Cells[1].Value = IDEN02;
                            dataGridView5.Rows[dataGridView5.RowCount - 1].Cells[2].Value = IDEN03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }
        }
        private void Btn_search_G6_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetGridviewMedia();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objProtocalM.PROTOCOL_MEDIA_ID_ = fm.SelectedID.ToString();
                    objProtocalM.PROTOCOL_MEDIA_NAME = fm.SelectedName;
                    objProtocalM.PROTOCOL_MEDIA_CODE = fm.SelectedCode;

                    MEDIA01 = objProtocalM.PROTOCOL_MEDIA_CODE;
                    MEDIA02 = objProtocalM.PROTOCOL_MEDIA_NAME;
                    MEDIA03 = objProtocalM.PROTOCOL_MEDIA_ID_;

                    int minRowCount = 0;
                    bool CheckCustomizedResult = false;
                    for (int i = 0; i < dataGridView6.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView6.Rows.Add();
                        dataGridView6.Rows[dataGridView6.RowCount - 1].Cells[0].Value = MEDIA01;
                        dataGridView6.Rows[dataGridView6.RowCount - 1].Cells[1].Value = MEDIA02;
                        dataGridView6.Rows[dataGridView6.RowCount - 1].Cells[2].Value = MEDIA03;
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView6.Rows)
                        {
                            if (row.Cells[2].Value.ToString() == (MEDIA03.ToString()))
                            {
                                CheckCustomizedResult = true;
                            }
                        }
                        if (CheckCustomizedResult == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckCustomizedResult == false)
                        {
                            dataGridView6.Rows.Add();
                            dataGridView6.Rows[dataGridView6.RowCount - 1].Cells[0].Value = MEDIA01;
                            dataGridView6.Rows[dataGridView6.RowCount - 1].Cells[1].Value = MEDIA02;
                            dataGridView6.Rows[dataGridView6.RowCount - 1].Cells[2].Value = MEDIA03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }
        }
        private void Btn_search_G7_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetGridviewAntibioticGroup();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objProtocalM.PROTOCOL_ANTIBIOTIC_ID = fm.SelectedID.ToString();
                    objProtocalM.PROTOCOL_ANTIBIOTIC_NAME = fm.SelectedName;
                    objProtocalM.PROTOCOL_ANTIBIOTIC_CODE = fm.SelectedCode;

                    ANTIBIOTIC01 = objProtocalM.PROTOCOL_ANTIBIOTIC_CODE;
                    ANTIBIOTIC02 = objProtocalM.PROTOCOL_ANTIBIOTIC_NAME;
                    ANTIBIOTIC03 = objProtocalM.PROTOCOL_ANTIBIOTIC_ID;

                    int minRowCount = 0;
                    bool CheckCustomizedResult = false;
                    for (int i = 0; i < dataGridView7.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView7.Rows.Add();
                        dataGridView7.Rows[dataGridView7.RowCount - 1].Cells[0].Value = ANTIBIOTIC01;
                        dataGridView7.Rows[dataGridView7.RowCount - 1].Cells[1].Value = ANTIBIOTIC02;
                        dataGridView7.Rows[dataGridView7.RowCount - 1].Cells[2].Value = ANTIBIOTIC03;
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView7.Rows)
                        {
                            if (row.Cells[2].Value.ToString() == (ANTIBIOTIC03.ToString()))
                            {
                                CheckCustomizedResult = true;
                            }
                        }
                        if (CheckCustomizedResult == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckCustomizedResult == false)
                        {
                            dataGridView7.Rows.Add();
                            dataGridView7.Rows[dataGridView6.RowCount - 1].Cells[0].Value = ANTIBIOTIC01;
                            dataGridView7.Rows[dataGridView6.RowCount - 1].Cells[1].Value = ANTIBIOTIC02;
                            dataGridView7.Rows[dataGridView6.RowCount - 1].Cells[2].Value = ANTIBIOTIC03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }

        }
        private void Btn_Clear_G4_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
        }
        private void Btn_Clear_G5_Click(object sender, EventArgs e)
        {
            dataGridView5.Rows.Clear();
        }
        private void Btn_Clear_G6_Click(object sender, EventArgs e)
        {
            dataGridView6.Rows.Clear();
        }
        private void Btn_Clear_G7_Click(object sender, EventArgs e)
        {
            dataGridView7.Rows.Clear();
        }
        private void contextMenuStrip4_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView4.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView4.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void dataGridView4_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView4.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView4.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView4.CurrentCell = this.dataGridView4.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip4.Show(this.dataGridView4, e.Location);
                        contextMenuStrip4.Show(Cursor.Position);
                    }
                }
            }

        }
        private void contextMenuStrip5_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView5.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView5.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void dataGridView5_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView5.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView5.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView5.CurrentCell = this.dataGridView5.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip5.Show(this.dataGridView5, e.Location);
                        contextMenuStrip5.Show(Cursor.Position);
                    }
                }
            }
        }
        private void contextMenuStrip6_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView6.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView6.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void dataGridView6_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView6.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView6.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView6.CurrentCell = this.dataGridView6.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip6.Show(this.dataGridView6, e.Location);
                        contextMenuStrip6.Show(Cursor.Position);
                    }
                }
            }
        }
        private void contextMenuStrip7_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView7.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView7.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void dataGridView7_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView7.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView7.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView7.CurrentCell = this.dataGridView7.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip7.Show(this.dataGridView7, e.Location);
                        contextMenuStrip7.Show(Cursor.Position);
                    }
                }
            }

        }

        private void Btn_search_G1_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();
            SpecimenM objSpecimenM = new SpecimenM();


            DataTable dt;
            try
            {
                dt = objConfiguration.GetSpecimenLookup(objSpecimenM);

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objProtocalM.PROTOCOL_SPM_ID = fm.SelectedID.ToString();
                    objProtocalM.PROTOCOL_SPM_NAME = fm.SelectedName;
                    objProtocalM.PROTOCOL_SPM_CODE = fm.SelectedCode;

                    SPM01 = objProtocalM.PROTOCOL_SPM_CODE;
                    SPM02 = objProtocalM.PROTOCOL_SPM_NAME;
                    SPM03 = objProtocalM.PROTOCOL_SPM_ID;

                    int minRowCount = 0;

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = SPM01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = SPM02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = SPM03;

                    }
                    else
                    {
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = SPM01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = SPM02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = SPM03;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }

        }
        private void Btn_search_G2_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetGridviewStain();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objProtocalM.PROTOCOL_SAMPLE_ID = fm.SelectedID.ToString();
                    objProtocalM.PROTOCOL_SAMPLE_NAME = fm.SelectedName;
                    objProtocalM.PROTOCOL_SAMPLE_CODE = fm.SelectedCode;

                    SAMPLE01 = objProtocalM.PROTOCOL_SAMPLE_CODE;
                    SAMPLE02 = objProtocalM.PROTOCOL_SAMPLE_NAME;
                    SAMPLE03 = objProtocalM.PROTOCOL_SAMPLE_ID;

                    int minRowCount = 0;
                    bool CheckCustomizedResult = false;
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = SAMPLE01;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = SAMPLE02;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = SAMPLE03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells[2].Value.ToString() == (SAMPLE03.ToString()))
                            {
                                CheckCustomizedResult = true;
                            }
                        }
                        if (CheckCustomizedResult == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckCustomizedResult == false)
                        {
                            dataGridView2.Rows.Add();
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = SAMPLE01;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = SAMPLE02;
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = SAMPLE03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }

        }
        private void Btn_search_G3_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetGridviewTOPO();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objProtocalM.PROTOCOL_TOPO_ID = fm.SelectedID.ToString();
                    objProtocalM.PROTOCOL_TOPO_NAME = fm.SelectedName;
                    objProtocalM.PROTOCOL_TOPO_CODE = fm.SelectedCode;

                    TOPO01 = objProtocalM.PROTOCOL_TOPO_CODE;
                    TOPO02 = objProtocalM.PROTOCOL_TOPO_NAME;
                    TOPO03 = objProtocalM.PROTOCOL_TOPO_ID;

                    int minRowCount = 0;
                    bool CheckCustomizedResult = false;
                    for (int i = 0; i < dataGridView3.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView3.Rows.Add();
                        dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[0].Value = TOPO01;
                        dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[1].Value = TOPO02;
                        dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[2].Value = TOPO03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView3.Rows)
                        {
                            if (row.Cells[2].Value.ToString() == (TOPO03.ToString()))
                            {
                                CheckCustomizedResult = true;
                            }
                        }
                        if (CheckCustomizedResult == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckCustomizedResult == false)
                        {
                            dataGridView3.Rows.Add();
                            dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[0].Value = TOPO01;
                            dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[1].Value = TOPO02;
                            dataGridView3.Rows[dataGridView3.RowCount - 1].Cells[2].Value = TOPO03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }

        }
        private void Btn_search_G8_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetGridviewCHEM();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objProtocalM.PROTOCOL_CHEM_ID = fm.SelectedID.ToString();
                    objProtocalM.PROTOCOL_CHEM_NAME = fm.SelectedName;
                    objProtocalM.PROTOCOL_CHEM_CODE = fm.SelectedCode;

                    CHEM01 = objProtocalM.PROTOCOL_CHEM_CODE;
                    CHEM02 = objProtocalM.PROTOCOL_CHEM_NAME;
                    CHEM03 = objProtocalM.PROTOCOL_CHEM_ID;

                    int minRowCount = 0;
                    bool CheckCustomizedResult = false;
                    for (int i = 0; i < dataGridView8.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView8.Rows.Add();
                        dataGridView8.Rows[dataGridView8.RowCount - 1].Cells[0].Value = CHEM01;
                        dataGridView8.Rows[dataGridView8.RowCount - 1].Cells[1].Value = CHEM02;
                        dataGridView8.Rows[dataGridView8.RowCount - 1].Cells[2].Value = CHEM03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView8.Rows)
                        {
                            if (row.Cells[2].Value.ToString() == (CHEM03.ToString()))
                            {
                                CheckCustomizedResult = true;
                            }
                        }
                        if (CheckCustomizedResult == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckCustomizedResult == false)
                        {
                            dataGridView8.Rows.Add();
                            dataGridView8.Rows[dataGridView8.RowCount - 1].Cells[0].Value = CHEM01;
                            dataGridView8.Rows[dataGridView8.RowCount - 1].Cells[1].Value = CHEM02;
                            dataGridView8.Rows[dataGridView8.RowCount - 1].Cells[2].Value = CHEM03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }
        }
        private void Btn_search_G9_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetGridviewCYTO();

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objProtocalM.PROTOCOL_CYTO_ID = fm.SelectedID.ToString();
                    objProtocalM.PROTOCOL_CYTO_NAME = fm.SelectedName;
                    objProtocalM.PROTOCOL_CYTO_CODE = fm.SelectedCode;

                    CYTO01 = objProtocalM.PROTOCOL_CYTO_CODE;
                    CYTO02 = objProtocalM.PROTOCOL_CYTO_NAME;
                    CYTO03 = objProtocalM.PROTOCOL_CYTO_ID;

                    int minRowCount = 0;
                    bool CheckCustomizedResult = false;
                    for (int i = 0; i < dataGridView9.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView9.Rows.Add();
                        dataGridView9.Rows[dataGridView9.RowCount - 1].Cells[0].Value = CYTO01;
                        dataGridView9.Rows[dataGridView9.RowCount - 1].Cells[1].Value = CYTO02;
                        dataGridView9.Rows[dataGridView9.RowCount - 1].Cells[2].Value = CYTO03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView9.Rows)
                        {
                            if (row.Cells[2].Value.ToString() == (CYTO03.ToString()))
                            {
                                CheckCustomizedResult = true;
                            }
                        }
                        if (CheckCustomizedResult == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (CheckCustomizedResult == false)
                        {
                            dataGridView9.Rows.Add();
                            dataGridView9.Rows[dataGridView9.RowCount - 1].Cells[0].Value = CYTO01;
                            dataGridView9.Rows[dataGridView9.RowCount - 1].Cells[1].Value = CYTO02;
                            dataGridView9.Rows[dataGridView9.RowCount - 1].Cells[2].Value = CYTO03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }
        }
        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView1.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView1.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        private void contextMenuStrip2_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView2.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView2.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        private void contextMenuStrip3_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView3.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView3.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        private void contextMenuStrip8_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView8.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView8.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        private void contextMenuStrip9_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView9.Rows[this.rowindex].IsNewRow)
            {
                try
                {
                    this.dataGridView9.Rows.RemoveAt(this.rowindex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView1.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip1.Show(this.dataGridView1, e.Location);
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
            }
        }
        private void dataGridView2_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView2.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView2.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView2.CurrentCell = this.dataGridView2.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip2.Show(this.dataGridView2, e.Location);
                        contextMenuStrip2.Show(Cursor.Position);
                    }
                }
            }
        }
        private void dataGridView3_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView3.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView3.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView3.CurrentCell = this.dataGridView3.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip3.Show(this.dataGridView3, e.Location);
                        contextMenuStrip3.Show(Cursor.Position);
                    }
                }
            }

        }
        private void dataGridView8_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView8.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView8.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView8.CurrentCell = this.dataGridView8.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip8.Show(this.dataGridView8, e.Location);
                        contextMenuStrip8.Show(Cursor.Position);
                    }
                }
            }
        }
        private void dataGridView9_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView9.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        this.dataGridView9.Rows[e.RowIndex].Selected = true;
                        rowindex = e.RowIndex;
                        this.dataGridView9.CurrentCell = this.dataGridView9.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip9.Show(this.dataGridView9, e.Location);
                        contextMenuStrip9.Show(Cursor.Position);
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
}
