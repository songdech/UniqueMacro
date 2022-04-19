using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Patient;
using UNIQUE.OEM;


namespace UNIQUE.PatientFld
{
    public partial class frmPatientRegister : Form
    {
        string strAction = "";
        private string StrPatnumber;
        private PatientM ObjpatientM;
        private ConfigurationController objConfiguration = new ConfigurationController();

        System.Globalization.CultureInfo cultureinfo_TH = new System.Globalization.CultureInfo("th-TH");

        System.Globalization.CultureInfo cultureinfo_ENG = new System.Globalization.CultureInfo("en-US");

        public frmPatientRegister(string strAction)
        {
            InitializeComponent();
            this.strAction = strAction;
            this.ActiveControl = txtHN;
            ObjpatientM = new PatientM();
        }

        public frmPatientRegister(string strAction,string StrHN)
        {
            InitializeComponent();
            this.strAction = strAction;
            this.StrPatnumber = StrHN;
        }

        private void frmPatientRegister_Load(object sender, EventArgs e)
        {
            lblLoginCode.Text = ControlParameter.loginID.ToString();
            lblLoginName.Text = ControlParameter.loginName.ToString();
            txtHN.Text = StrPatnumber;

            // Check Refer from Patient Search Click Edit.
            if (txtHN.Text != "")
            {
                LoadPatient();
            }

            if (strAction == "New")
            {
                txtHN.Select();
                grpRegister.Text = "Create Patient";
            }
            else if (strAction == "Edit")
            {
                txtHN.Select();
                grpRegister.Text = "Edit Patient";
                //queryPatientInfo();
            }
        }
        private void LoadPatient()
        {
            PatientController objPatient = new PatientController();
            DataTable dt = null;
            PatientM objPatientM = new PatientM();
            try
            {
                if (txtHN.Text != "")
                {
                    objPatientM.PatientNo = txtHN.Text;
                    dt = objPatient.GetPatientNumber_register(objPatientM);

                    if (dt.Rows.Count != 0)
                    {
                        grpRegister.Text = "Edit Patient";

                        txtAltnum.Text = objPatientM.PatAltnumber;
                        //dateBOD.Value = Convert.ToDateTime(objPatientM.BirthDate);
                        DateTime STR_DOB = Convert.ToDateTime(objPatientM.BirthDate, cultureinfo_ENG);

                        dateBOD.Value = STR_DOB;

                        comboBox_Title1.Text = objPatientM.Title1;
                        txtTitle2.Text = objPatientM.Title2;
                        txtName.Text = objPatientM.PatientName;
                        txtLastName.Text = objPatientM.LastName;

                        int IntSex = Convert.ToInt32(objPatientM.Sex);

                        if (IntSex == 0)
                        {
                            radioButtonOther.Checked = true;
                        }
                        else if (IntSex == 1)
                        {
                            radioButtonMale.Checked = true;
                        }
                        else if (IntSex == 2)
                        {
                            radioButtonFemale.Checked = true;
                        }
                        else
                        {
                            radioButtonOther.Checked = true;
                        }

                        txtRace.Text = objPatientM.PatRace;
                        txtNation.Text = objPatientM.PatNation;
                        txtRegion.Text = objPatientM.PatRegion;
                        //VIP  --> value 1 =VIP or 0=None
                        //Secret Result  --> value 1=Secret or 0=None

                        int IntVip = Convert.ToInt32(objPatientM.VIP);
                        int IntSecret = Convert.ToInt32(objPatientM.SecretResult);

                        if (IntVip == 1)
                        {
                            checkBoxVIP.Checked = true;
                        }
                        else
                        {
                            checkBoxVIP.Checked = false;

                        }
                        if (IntSecret == 1)
                        {
                            chkSecret.Checked = true;
                        }
                        else
                        {
                            chkSecret.Checked = false;
                        }

                        //ID Card
                        txtRoomNo.Text = objPatientM.RoomNo;
                        comboABO.Text = objPatientM.BGABO;
                        comboRH.Text = objPatientM.BGRHSUS;
                        txtRefDoc.Text = objPatientM.RefDoctor;
                        txtLocation.Text = objPatientM.RefLocation;
                        txtCity.Text = objPatientM.City;
                        txtAddress1.Text = objPatientM.PatAddress_1;
                        txtAddress2.Text = objPatientM.PatAddress_2;
                        txtState.Text = objPatientM.State;
                        txtPostcode.Text = objPatientM.PostCode;
                        txtCountry.Text = objPatientM.Country;
                        txtPhone1.Text = objPatientM.Telephone;
                        txtPhone2.Text = objPatientM.Telephone2;
                        txtEmail.Text = objPatientM.Email;
                    }
                    else
                    {
                        grpRegister.Text = "Create Patient";
                    }
                }
                else
                {
                    MessageBox.Show("Please fill number HN");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void queryPatientInfo()
        {
            //string sql = "SELECT PATNUMBER,PATNAME,PATLASTNAME FROM PATIENTS";
            //SqlCommand cmd = new SqlCommand(sql,conn);
            //SqlDataAdapter adp = new SqlDataAdapter(cmd);
        }

        private void navButton2_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void frmPatientRegister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                MessageBox.Show("Escape key pressed");

                // prevent child controls from handling this event as well
                e.SuppressKeyPress = true;
            }
        }

        private void frmPatientRegister_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)

                this.Close();
        }
        private void clearText()
        {
            txtHN.Clear();
            txtName.Clear();
            txtLastName.Clear();
            txtLocation.Clear();
            dateBOD.Checked = false;
            radioButtonOther.Checked = true;
            txtRefDoc.Text = "";
            dateAdmid.Checked = false;
            txtRoomNo.Clear();
            checkBoxVIP.Checked = false;
            chkSecret.Checked = false;
            txtAddress1.Clear();
            txtAddress2.Clear();
            txtCity.Clear();
            txtState.Clear();
            txtPostcode.Clear();
            txtCountry.Clear();
            txtPhone1.Text = "";
            txtPhone2.Text = "";
            txtEmail.Text = "";
            txtAltnum.Text = "";
            comboBox_Title1.Text ="";
            txtRace.Text = "";
            txtNation.Text = "";
            txtRegion.Text = "";
            comboABO.Text ="";
            comboRH.Text ="";
            txtComment.Text = "";
        }

        private void navButton3_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            clearText();
        }

        private void txtHN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadPatient();
            }
        }

        private void BarbtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                PatientM objPatientM = new PatientM();
                if (txtHN.Text != "" && txtName.Text != "" )
                {

                    objPatientM.PatientNo = txtHN.Text;
                    objPatientM.PatAltnumber = txtAltnum.Text;
                    string Str_DOB = Convert.ToDateTime(dateBOD.Value).ToString("yyyy-MM-dd", cultureinfo_ENG);

                    objPatientM.BirthDay = Str_DOB;
                    objPatientM.Title1 = comboBox_Title1.Text;
                    objPatientM.Title2 = txtTitle2.Text;
                    objPatientM.PatientName =txtName.Text;
                    objPatientM.LastName = txtLastName.Text;
                    objPatientM.PatRace = txtRace.Text;
                    objPatientM.PatNation = txtNation.Text;
                    objPatientM.PatRegion = txtRegion.Text;
                    //VIP  --> value 1 =VIP or 0=None
                    //Secret Result  --> value 1=Secret or 0=None

                    int IntVip = 0;
                    int IntSecret = 0;
                    int IntSex = 0;
                    string StrSex = "";

                    if (radioButtonMale.Checked == true)
                    {
                        IntSex = 1;
                        StrSex = "ชาย";
                    }
                    else if (radioButtonFemale.Checked == true)
                    {
                        IntSex = 2;
                        StrSex = "หญิง";

                    }
                    else
                    {
                        IntSex = 0;
                        StrSex = "ไม่ระบุเพศ";
                    }

                    if (checkBoxVIP.Checked == true)
                    {
                        IntVip = 1;
                    }
                    else
                    {
                        IntVip = 0;
                    }
                    if (chkSecret.Checked == true)
                    {
                        IntSecret = 1;
                    }
                    else
                    {
                        IntSecret = 0;
                    }

                    objPatientM.Sex = Convert.ToString(IntSex);
                    objPatientM.VIP = Convert.ToString(IntVip);
                    objPatientM.SecretResult = Convert.ToString(IntSecret);
                    objPatientM.RoomNo = txtRoomNo.Text;
                    objPatientM.BGABO = comboABO.Text;
                    objPatientM.BGRHSUS = comboRH.Text;
                    objPatientM.RefDoctor = txtRefDoc.Text;
                    objPatientM.RefLocation = txtLocation.Text;
                    objPatientM.Address1 = txtAddress1.Text;
                    objPatientM.Address2 = txtAddress2.Text;
                    objPatientM.City = txtCity.Text;
                    objPatientM.State = txtState.Text;
                    objPatientM.PostCode = txtPostcode.Text;
                    objPatientM.Country = txtCountry.Text;
                    objPatientM.Telephone = txtPhone1.Text;
                    objPatientM.Telephone2 = txtPhone2.Text;
                    objPatientM.Email = txtEmail.Text;
                    objPatientM.Comment = txtComment.Text;

                    // Set Login user
                    objPatientM.LOGUSERID = ControlParameter.loginID.ToString();
                    objPatientM = objConfiguration.SavePatient_register(objPatientM);

                    clearText();
                    if (grpRegister.Text == "Create Patient")
                    {
                        DialogResult result = MessageBox.Show("Save complete?", "Create new Request?", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                        if (result == DialogResult.Yes)
                        {
                            //MessageBox.Show("Create New Request");
                            objPatientM.Sex = StrSex;
                            string StrBirthDate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(objPatientM.BirthDay));
                            string StrRequestNew_name = objPatientM.PatientName + "  " + objPatientM.LastName;
                            objPatientM.PatientName = StrRequestNew_name;
                            objPatientM.BirthDate = StrBirthDate;
                            this.Close();
                            Form_REQUEST_NEWORDER FM_REQUEST = new Form_REQUEST_NEWORDER(objPatientM);
                            FM_REQUEST.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Complete Edit patient");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void barBtnClear_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clearText();
        }

        private void txtHN_Leave(object sender, EventArgs e)
        {
            txtName.Clear();
            txtLastName.Clear();
            txtLocation.Clear();
            dateBOD.Checked = false;
            radioButtonOther.Checked = true;
            txtRefDoc.Text = "";
            dateAdmid.Checked = false;
            txtRoomNo.Clear();
            checkBoxVIP.Checked = false;
            chkSecret.Checked = false;
            txtAddress1.Clear();
            txtAddress2.Clear();
            txtCity.Clear();
            txtState.Clear();
            txtPostcode.Clear();
            txtCountry.Clear();
            txtPhone1.Text = "";
            txtPhone2.Text = "";
            txtEmail.Text = "";
            txtAltnum.Text = "";
            comboBox_Title1.Text = "";
            txtRace.Text = "";
            txtNation.Text = "";
            txtRegion.Text = "";
            comboABO.Text = "";
            comboRH.Text = "";
            txtComment.Text = "";

            LoadPatient();
        }
    }
}