using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Patient;
using UNIQUE.OEM;
using UniquePro.Entities.Configuration;


namespace UNIQUE.PatientFld
{
    public partial class frmPatientMgnt : Form
    {
        //Menu config
        IniParser PARSER;
        String StrPARSER = "";
        int A, B, C, D, E, F, G, H, I, J, K, L, M, N, O;
        //End
        //Check box config
        string IntSex = "";
        string IntVIP = "";
        string IntSecret = "";
        //End

        System.Globalization.CultureInfo cultureinfo_TH = new System.Globalization.CultureInfo("th-TH");
        System.Globalization.CultureInfo cultureinfo_ENG = new System.Globalization.CultureInfo("en-US");

        public frmPatientMgnt()
        {
            InitializeComponent();
        }

        private void frmPatientMgnt_Load(object sender, EventArgs e)
        {
            ClearData();

            lblLoginCode.Text = ControlParameter.loginID.ToString();
            lblLoginName.Text = ControlParameter.loginName.ToString();

            StrPARSER = lblLoginCode.Text + "_PatientMGNT_" + ".ini";
            if (File.Exists(StrPARSER))
            {
                PARSER = new IniParser(StrPARSER);
                A = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Patient_number"));
                B = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Name"));
                C = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Lastname"));
                D = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Location"));
                E = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Doctor"));
                F = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Register_date"));
                G = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Lastupdate"));
                H = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Create_by"));
                I = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Comment"));
                J = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Accessnumber"));
                K = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Receive_date"));
                L = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Protocol"));
                M = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Specimen_name"));
                N = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Receive_by"));
                O = Convert.ToInt16(PARSER.GetSetting("frmPatientMgnt", "Status"));


                CreateMenuStrip();
            }
            else
            {
                StringBuilder newSample = new StringBuilder();
                String WriterParser =
                 @"[frmPatientMgnt]" + "\r\n" +
                "Patient_number=1" + "\r\n" +
                "Name=1" + "\r\n" +
                "Lastname=1" + "\r\n" +
                "Location=1" + "\r\n" +
                "Doctor=1" + "\r\n" +
                "Register_date=1" + "\r\n" +
                "Lastupdate=1" + "\r\n" +
                "Create_by=1" + "\r\n" +
                "Comment=1" + "\r\n" +
                "Accessnumber=1" + "\r\n" +
                "Receive_date=1" + "\r\n" +
                "Protocol=1" + "\r\n" +
                "Specimen_name=1" + "\r\n" +
                "Receive_by=1" + "\r\n" +
                "Status=1";

                using (StreamWriter writer = new StreamWriter(StrPARSER))
                {
                    writer.WriteLine(WriterParser);
                }
            }
        }


        private void navSearchPatient_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            txtHN.Focus();
        }

        private void frmPatientMgnt_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void navRegister_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            PatientFld.frmPatientRegister Fm1 = new frmPatientRegister("New");

            if (!CheckOpened(Fm1.Text))
            {
                Fm1 = new frmPatientRegister("New");
                Fm1.Show();
            }
        }

        private bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true;
                }
            }
            return false;
        }

        private void navButton3_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            
        }

        private void navMerge_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            PatientFld.FrmMergePatient Fm1 = new FrmMergePatient();

            if (!CheckOpened(Fm1.Text))
            {
                Fm1 = new FrmMergePatient();
                Fm1.Show();
            }
        }

        private void navHistory_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PatientM objPatientM = new PatientM();
            PatientFld.frmPatientHistory Fm1 = new frmPatientHistory(objPatientM);

            if (!CheckOpened(Fm1.Text))
            {
                if (radioButtonMale.Checked == true)
                {
                    objPatientM.Sex = "ชาย";
                }
                else if (radioButtonFemale.Checked == true)
                {
                    objPatientM.Sex = "หญิง";
                }
                else if (radioButtonOther.Checked == true)
                {
                    objPatientM.Sex = "ไม่ระบุเพศ";
                }

                string StrHN = txtHN.Text;
                string StrName = txtName.Text + "  " + txtLastName.Text;
                //string StrAge = row.Cells[18].Value.ToString() + " ปี" + row.Cells[19].Value.ToString() + " เดือน";
                string StrBirthDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dateBOD.Value));

                objPatientM.PatientNo = StrHN;
                objPatientM.PatientName = StrName;
                objPatientM.BirthDate = StrBirthDate;

                Fm1 = new frmPatientHistory(objPatientM);
                Fm1.Show();
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PatientFld.FrmMergePatient Fm1 = new FrmMergePatient();

            if (!CheckOpened(Fm1.Text))
            {
                Fm1 = new FrmMergePatient();
                Fm1.Show();
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PatientFld.frmPatientRegister Fm1 = new frmPatientRegister("Edit");

            if (!CheckOpened(Fm1.Text))
            {
                if (txtHN.Text != "")
                {
                    string StrHn = txtHN.Text;
                    Fm1 = new frmPatientRegister("Edit", StrHn);
                    Fm1.Show();
                }
                else
                {
                    MessageBox.Show("Please select Patient for E-dit");
                }
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PatientFld.frmPatientRegister Fm1 = new frmPatientRegister("New");

            if (!CheckOpened(Fm1.Text))
            {
                Fm1 = new frmPatientRegister("New");
                Fm1.ShowDialog();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_patient();
        }

        private void Search_patient()
        {
            // VIP
            if (txtAccessnumber.Text != "" || txtHosnumber.Text != "" || txtSIDnumber.Text != "")
            {
                txtHN.Text = "";
                txtName.Text = "";
                txtLastName.Text = "";
                txtAltnum.Text = "";
                txtDoctor.Text = "";
                txtLocation.Text = "";
                txtIDcard.Text = "";
                radioButtonMale.Checked = false;
                radioButtonFemale.Checked = false;
                radioButtonOther.Checked = false;
                dateBOD.Checked = false;
                checkBoxVIP.Checked = false;
                checkBoxSecret.Checked = false;

                Search_data_byLN();
            }
            else if (txtProtocolnumber.Text != "")
            {
                txtAccessnumber.Text = "";
                txtSIDnumber.Text = "";
                txtHosnumber.Text = "";
                txtHN.Text = "";
                txtName.Text = "";
                txtLastName.Text = "";
                txtAltnum.Text = "";
                txtDoctor.Text = "";
                txtLocation.Text = "";
                txtIDcard.Text = "";
                radioButtonMale.Checked = false;
                radioButtonFemale.Checked = false;
                radioButtonOther.Checked = false;
                dateBOD.Checked = false;
                checkBoxVIP.Checked = false;
                checkBoxSecret.Checked = false;

                Search_data_byProtocol();
            }
            else if (txtHN.Text != "" || txtAltnum.Text != "" || txtLastName.Text != "" || txtDoctor.Text != "" || txtLocation.Text != "" || txtIDcard.Text != "" || IntSex != "" || IntVIP != "" || IntSecret != "")
            {
                txtAccessnumber.Text = "";
                txtProtocolnumber.Text = "";
                txtSIDnumber.Text = "";
                txtHosnumber.Text = "";
                //radioButtonMale.Checked = false;
                //radioButtonFemale.Checked = false;
                //radioButtonOther.Checked = false;
                //dateBOD.Checked = false;
                //checkBoxVIP.Checked = false;
                //checkBoxSecret.Checked = false;

                Search_data();
            }
        }
        private void Search_data()
        {
            PatientController objPatient = new PatientController();
            DataTable dt = null;
            PatientM objPatientM = new PatientM();
            try
            {
                objPatientM.Sex = "%" + IntSex + "%";
                objPatientM.VIP = "%" + IntVIP + "%";
                objPatientM.SecretResult = "%" + IntSecret + "%";

                objPatientM.PatientNo = "%" + txtHN.Text + "%";
                objPatientM.PATSEARCH_ALTNUMBER = "%" + txtAltnum.Text + "%";
                objPatientM.PatientName = "%" + txtName.Text + "%";
                objPatientM.LastName = "%" + txtLastName.Text + "%";
                objPatientM.PATSEARCH_DOCTOR = "%" + txtDoctor.Text + "%";
                objPatientM.PATSEARCH_LOCATION = "%" + txtLocation.Text + "%";
                objPatientM.PATIDCARD = "%" + txtIDcard.Text + "%";


                dt = objPatient.GetSearchPatient(objPatientM);

                if (txtHN.Text != "" && dt.Rows.Count == 0)
                {
                    Gridview_Clear();
                    DialogResult result = MessageBox.Show("Register new Patient?", "Register new patient", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        frmPatientRegister FM_regis = new frmPatientRegister("New");
                        FM_regis.ShowDialog();
                        //this.Close();
                    }
                }
                else
                {
                    grdView.DataSource = dt;
                    lblTotalline.Text = "Total : " + Convert.ToString(grdView.RowCount);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Search_data_byLN()
        {
            PatientController objPatient = new PatientController();
            DataTable dt = null;
            PatientM objPatientM = new PatientM();
            try
            {
                objPatientM.PATSEARCH_ACCNUMBER = "%" + txtAccessnumber.Text + "%";
                objPatientM.PATSEARCH_SAMPLENUMBER = "%" + txtSIDnumber.Text + "%";
                objPatientM.PATSEARCH_HOSNUMBER = "%" + txtHosnumber.Text + "%";

                dt = objPatient.GetSearchPatient_byLN(objPatientM);

                if (txtHN.Text != "" && dt.Rows.Count == 0)
                {
                    MessageBox.Show("Not found data? Please find another infomation");
                }
                else
                {
                    grdView.DataSource = dt;
                    lblTotalline.Text = "Total : " + Convert.ToString(grdView.RowCount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Search_data_byProtocol()
        {
            PatientController objPatient = new PatientController();
            DataTable dt = null;
            PatientM objPatientM = new PatientM();
            try
            {
                objPatientM.PATSEARCH_PROTOCOL = txtProtocolnumber.Text;
                dt = objPatient.GetSearchPatient_byProtocol(objPatientM);

                if (txtHN.Text != "" && dt.Rows.Count == 0)
                {
                    MessageBox.Show("Not Found protocol search? Please find another infomation");
                }
                else
                {
                    grdView.DataSource = dt;
                    lblTotalline.Text = "Total : " + Convert.ToString(grdView.RowCount);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                    //if (e.RowIndex > -1)
                    //{
                    //    this.dataGridView1.Rows[e.RowIndex].Selected = true;
                    //    rowindex = e.RowIndex;
                        //this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[0];
                        this.contextMenuStrip1.Show(this.dataGridView1, e.Location);
                        contextMenuStrip1.Show(Cursor.Position);
                    //}
            }
        }

        private void grdView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.grdView.Rows[e.RowIndex];
                    txtHN.Text = row.Cells[0].Value.ToString();
                    txtName.Text = row.Cells[1].Value.ToString();
                    txtLastName.Text = row.Cells[2].Value.ToString();
                    txtLocation.Text = row.Cells[3].Value.ToString();
                    txtDoctor.Text = row.Cells[4].Value.ToString();
                    txtAltnum.Text = row.Cells[12].Value.ToString();
                    //  string StrBirthDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dateBOD.Value));

                    //dateBOD.Value = Convert.ToDateTime(row.Cells[11].Value.ToString());
                    DateTime STR_DOB = Convert.ToDateTime(row.Cells[11].Value.ToString(),cultureinfo_ENG);


                    dateBOD.Value = STR_DOB;

                    int IntSex = Convert.ToInt32(row.Cells[10].Value.ToString());
                    int IntVIP = Convert.ToInt32(row.Cells[22].Value.ToString());
                    int IntSecret = Convert.ToInt32(row.Cells[23].Value.ToString());

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
                    // VIP
                    if (IntVIP == 0)
                    {
                        checkBoxVIP.Checked = false;
                    }
                    else if (IntVIP == 1)
                    {
                        checkBoxVIP.Checked = true;
                    }
                    // Secret
                    if (IntSecret == 0)
                    {
                        checkBoxSecret.Checked = false;
                    }
                    else if (IntSecret == 1)
                    {
                        checkBoxSecret.Checked = true;
                    }



                    string StrHN = row.Cells[0].Value.ToString();
                    string StrName = row.Cells[1].Value.ToString() + " " + row.Cells[2].Value.ToString();
                    //string StrAge = row.Cells[18].Value.ToString() + " ปี" + row.Cells[19].Value.ToString() + " เดือน";
                    string StrBirthDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row.Cells[11].Value.ToString()));

                    PatientController objPatient = new PatientController();
                    DataTable dt = null;
                    PatientM objPatientM = new PatientM();
                    try
                    {
                        objPatientM.Title1 = row.Cells[15].Value.ToString();
                        objPatientM.Title2 = row.Cells[16].Value.ToString();
                        objPatientM.PatientNo = StrHN;
                        objPatientM.PatientName = StrName;


                        objPatientM.BirthDate = StrBirthDate;
                        objPatientM.Sex = row.Cells[17].Value.ToString();
                        string DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(row.Cells[11].Value.ToString()));

                        string Str_DateOfBirth = "";
                        // Start Date of Birth in calcu
                        if (DOB == null || DOB == "")
                        {
                            Str_DateOfBirth = "-";
                        }
                        else
                        {
                            string[] Array_DOB = DOB.Split("-".ToCharArray());

                            int Str_Year = Convert.ToInt32(Array_DOB[2]);
                            int Str_Month = Convert.ToInt32(Array_DOB[1]);
                            int Str_Day = Convert.ToInt32(Array_DOB[0]);

                            DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                            DateTime ToDate = DateTime.Now;
                            DateDifference dDiff = new DateDifference(myDate, ToDate);

                            Str_DateOfBirth = dDiff.ToString();
                        }
                        // END Date of Birth in calcu

                        objPatientM.Age = Str_DateOfBirth;
                        //objPatientM.DIAGNOSTIC

                        dt = objPatient.GetPatSearch_Allaccess(objPatientM);
                        if (dt.Rows.Count == 0)
                        {
                            Gridview2_Clear();
                            DialogResult result = MessageBox.Show("Not found request", "Create new request?", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.Yes)
                            {
                                Form_REQUEST_NEWORDER FM = new Form_REQUEST_NEWORDER(objPatientM);
                                FM.Show();
                                //this.Close();
                            }
                        }
                        else
                        {
                            dataGridView1.DataSource = dt;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearData()
        {
            txtAccessnumber.Text = "";
            txtProtocolnumber.Text = "";
            txtSIDnumber.Text = "";
            txtHosnumber.Text = "";
            //-------------
            txtHN.Text = "";
            txtName.Text = "";
            txtLastName.Text = "";
            txtLocation.Text = "";
            txtDoctor.Text = "";
            txtAltnum.Text = "";
            txtProtocolnumber.Text = "";
            txtIDcard.Text = "";
            radioButtonMale.Checked = false;
            radioButtonFemale.Checked = false;
            radioButtonOther.Checked = false;
            dateBOD.Checked = false;
            checkBoxVIP.Checked = false;
            checkBoxSecret.Checked = false;
            dateBOD.Value = DateTime.Now;

            Gridview_Clear();
            Gridview2_Clear();

        }
        private void Gridview_Clear()
        {
            lblTotalline.Text = "Total : -";
            PatientController objPatient = new PatientController();
            DataTable dt = null;
            PatientM objPatientM = new PatientM();
            try
            {
                dt = objPatient.GetClear_Gridview1(objPatientM);
                grdView.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Gridview2_Clear()
        {
            lblTotalline.Text = "Total : -";
            PatientController objPatient = new PatientController();
            DataTable dt = null;
            PatientM objPatientM = new PatientM();
            try
            {
                dt = objPatient.GetClear_Gridview2(objPatientM);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_data();
            }
        }

        private void txtHN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_data();
            }
        }

        private void txtLNnumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_data_byLN();
            }
        }

        private void txtAltnum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_data();
            }
        }

        private void txtLastName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_data();
            }
        }

        private void txtDoctor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_data();
            }
        }

        private void txtLocation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_data();
            }
        }

        private void txtSIDnumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_data_byLN();
            }
        }

        private void txtSIDnumber_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_data_byLN();
            }
        }

        private void txtHosnumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_data_byLN();
            }
        }

        private void txtSIDnumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void barBtnRequest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtHN.Text != "")
            {
                PatientM objPatientM = new PatientM();

                if (radioButtonMale.Checked == true)
                {
                    objPatientM.Sex = "ชาย";
                }
                else if (radioButtonFemale.Checked == true)
                {
                    objPatientM.Sex = "หญิง";
                }
                else if (radioButtonOther.Checked == true)
                {
                    objPatientM.Sex = "ไม่ระบุเพศ";
                }

                string StrHN = txtHN.Text;
                string StrName = txtName.Text + "  " + txtLastName.Text;
                //string StrAge = row.Cells[18].Value.ToString() + " ปี" + row.Cells[19].Value.ToString() + " เดือน";
                string StrBirthDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dateBOD.Value));
                string DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(dateBOD.Value));

                string Str_DateOfBirth = "";
                // Start Date of Birth in calcu
                if (DOB == null || DOB == "")
                {
                    Str_DateOfBirth = "-";
                }
                else
                {
                    string[] Array_DOB = DOB.Split("-".ToCharArray());

                    int Str_Year = Convert.ToInt32(Array_DOB[2]);
                    int Str_Month = Convert.ToInt32(Array_DOB[1]);
                    int Str_Day = Convert.ToInt32(Array_DOB[0]);

                    DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                    DateTime ToDate = DateTime.Now;
                    DateDifference dDiff = new DateDifference(myDate, ToDate);

                    Str_DateOfBirth = dDiff.ToString();
                }
                // END Date of Birth in calcu

                objPatientM.PatientNo = StrHN;
                objPatientM.PatientName = StrName;
                objPatientM.BirthDate = StrBirthDate;
                objPatientM.Age = Str_DateOfBirth;

                Form_REQUEST_NEWORDER FM_REQUEST = new Form_REQUEST_NEWORDER(objPatientM);
                FM_REQUEST.ShowDialog();
            }
        }

        private void txtAccessnumber_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void radioButtonMale_CheckedChanged(object sender, EventArgs e)
        {
            if ( radioButtonMale.Checked == true)
            {
                IntSex = "1";
            }
            else
            {
                IntSex = "";
            }
        }

        private void radioButtonFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFemale.Checked == true)
            {
                IntSex = "2";
            }
            else
            {
                IntSex = "";
            }
        }

        private void radioButtonOther_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonOther.Checked == true)
            {
                IntSex = "0";
            }
            else
            {
                IntSex = "";
            }
        }

        private void checkBoxVIP_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxVIP.Checked == true)
            {
                IntVIP = "1";
            }
            else
            {
                IntVIP = "";
            }
        }

        private void checkBoxSecret_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSecret.Checked == true)
            {
                IntSecret = "1";
            }
            else
            {
                IntSecret = "";
            }
        }

        private void grdView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip2.Show(this.grdView, e.Location);
                contextMenuStrip2.Show(Cursor.Position);
            }

        }

        private void ToolStripAccessnumber_Click(object sender, EventArgs e)
        {
            if (ToolStripAccessnumber.Checked == true)
            {
                dataGridView1.Columns["ACCESSNUMBER"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["ACCESSNUMBER"].Visible = false;
            }
        }

        private void ToolStripAccessnumber_CheckedChanged(object sender, EventArgs e)
        {

            if (ToolStripAccessnumber.Checked == true)
            {
                dataGridView1.Columns["ACCESSNUMBER"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["ACCESSNUMBER"].Visible = false;
            }
        }

        private void toolStripReceivedate_Click(object sender, EventArgs e)
        {
            if (toolStripReceivedate.Checked == true)
            {
                dataGridView1.Columns["RECEIVEDATE"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["RECEIVEDATE"].Visible = false;
            }

        }

        private void toolStripReceivedate_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripReceivedate.Checked == true)
            {
                dataGridView1.Columns["RECEIVEDATE"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["RECEIVEDATE"].Visible = false;
            }
        }

        private void toolStripMenuProtocol_Click(object sender, EventArgs e)
        {
            if (toolStripMenuProtocol.Checked == true)
            {
                dataGridView1.Columns["PROTOCOL"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["PROTOCOL"].Visible = false;
            }
        }

        private void toolStripMenuProtocol_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuProtocol.Checked == true)
            {
                dataGridView1.Columns["PROTOCOL"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["PROTOCOL"].Visible = false;
            }
        }

        private void toolStripMenuSpecimen_Click(object sender, EventArgs e)
        {
            if (toolStripMenuSpecimen.Checked == true)
            {
                dataGridView1.Columns["SPECIMEN"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["SPECIMEN"].Visible = false;
            }
        }

        private void toolStripMenuSpecimen_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuSpecimen.Checked == true)
            {
                dataGridView1.Columns["SPECIMEN"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["SPECIMEN"].Visible = false;
            }
        }

        private void toolStripMenuReceive_Click(object sender, EventArgs e)
        {
            if (toolStripMenuReceive.Checked == true)
            {
                dataGridView1.Columns["LOGUSERID"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["LOGUSERID"].Visible = false;
            }
        }

        private void toolStripMenuReceive_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuReceive.Checked == true)
            {
                dataGridView1.Columns["LOGUSERID"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["LOGUSERID"].Visible = false;
            }
        }

        private void toolStripMenuStatus_Click(object sender, EventArgs e)
        {
            if (toolStripMenuStatus.Checked == true)
            {
                dataGridView1.Columns["STATUS"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["STATUS"].Visible = false;
            }
        }

        private void toolStripMenuStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuStatus.Checked == true)
            {
                dataGridView1.Columns["STATUS"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["STATUS"].Visible = false;
            }
        }

        private void toolStripMenuDefault_Click(object sender, EventArgs e)
        {
            ToolStripAccessnumber.Checked = true;
            toolStripReceivedate.Checked = true;
            toolStripMenuProtocol.Checked = true;
            toolStripMenuSpecimen.Checked = true;
            toolStripMenuReceive.Checked = true;
            toolStripMenuStatus.Checked = true;
            //
            dataGridView1.Columns["ACCESSNUMBER"].Visible = true;
            dataGridView1.Columns["PROTOCOL"].Visible = true;
            dataGridView1.Columns["SPECIMEN"].Visible = true;
            dataGridView1.Columns["LOGUSERID"].Visible = true;
            dataGridView1.Columns["RECEIVEDATE"].Visible = true;
            dataGridView1.Columns["STATUS"].Visible = true;
        }

        private void toolStripMenuPatient_Click(object sender, EventArgs e)
        {
            if (toolStripMenuPatient.Checked == true)
            {
                grdView.Columns["PATNUMBER"].Visible = true;
            }
            else
            {
                grdView.Columns["PATNUMBER"].Visible = false;
            }

        }

        private void toolStripMenuPatient_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuPatient.Checked == true)
            {
                grdView.Columns["PATNUMBER"].Visible = true;
            }
            else
            {
                grdView.Columns["PATNUMBER"].Visible = false;
            }

        }

        private void toolStripMenuName_Click(object sender, EventArgs e)
        {
            if (toolStripMenuName.Checked == true)
            {
                grdView.Columns["NAME"].Visible = true;
            }
            else
            {
                grdView.Columns["NAME"].Visible = false;
            }

        }

        private void toolStripMenuName_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuName.Checked == true)
            {
                grdView.Columns["NAME"].Visible = true;
            }
            else
            {
                grdView.Columns["NAME"].Visible = false;
            }

        }

        private void toolStripMenuLastname_Click(object sender, EventArgs e)
        {
            if (toolStripMenuLastname.Checked == true)
            {
                grdView.Columns["LASTNAME"].Visible = true;
            }
            else
            {
                grdView.Columns["LASTNAME"].Visible = false;
            }


        }


        private void toolStripMenuLastname_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuLastname.Checked == true)
            {
                grdView.Columns["NAME"].Visible = true;
            }
            else
            {
                grdView.Columns["NAME"].Visible = false;
            }

        }

        private void toolStripMenuLocation_Click(object sender, EventArgs e)
        {
            if (toolStripMenuLocation.Checked == true)
            {
                grdView.Columns["LOCATION"].Visible = true;
            }
            else
            {
                grdView.Columns["LOCATION"].Visible = false;
            }

        }

        private void toolStripMenuLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuLocation.Checked == true)
            {
                grdView.Columns["LOCATION"].Visible = true;
            }
            else
            {
                grdView.Columns["LOCATION"].Visible = false;
            }

        }

        private void toolStripMenuDoctor_Click(object sender, EventArgs e)
        {
            if (toolStripMenuDoctor.Checked == true)
            {
                grdView.Columns["DOCTOR"].Visible = true;
            }
            else
            {
                grdView.Columns["DOCTOR"].Visible = false;
            }

        }

        private void toolStripMenuDoctor_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuDoctor.Checked == true)
            {
                grdView.Columns["DOCTOR"].Visible = true;
            }
            else
            {
                grdView.Columns["DOCTOR"].Visible = false;
            }

        }

        private void toolStripMenuRegister_Click(object sender, EventArgs e)
        {
            if (toolStripMenuRegister.Checked == true)
            {
                grdView.Columns["INCOMMINGDATE"].Visible = true;
            }
            else
            {
                grdView.Columns["INCOMMINGDATE"].Visible = false;
            }

        }

        private void toolStripMenuRegister_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuRegister.Checked == true)
            {
                grdView.Columns["INCOMMINGDATE"].Visible = true;
            }
            else
            {
                grdView.Columns["INCOMMINGDATE"].Visible = false;
            }

        }

        private void toolStripMenuLastupdate_Click(object sender, EventArgs e)
        {
            if (toolStripMenuLastupdate.Checked == true)
            {
                grdView.Columns["LASTUPDATE"].Visible = true;
            }
            else
            {
                grdView.Columns["LASTUPDATE"].Visible = false;
            }

        }

        private void toolStripMenuLastupdate_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuLastupdate.Checked == true)
            {
                grdView.Columns["LASTUPDATE"].Visible = true;
            }
            else
            {
                grdView.Columns["LASTUPDATE"].Visible = false;
            }

        }

        private void toolStripMenuCreateby_Click(object sender, EventArgs e)
        {
            if (toolStripMenuCreateby.Checked == true)
            {
                grdView.Columns["CREATEBY"].Visible = true;
            }
            else
            {
                grdView.Columns["CREATEBY"].Visible = false;
            }

        }

        private void toolStripMenuCreateby_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuCreateby.Checked == true)
            {
                grdView.Columns["CREATEBY"].Visible = true;
            }
            else
            {
                grdView.Columns["CREATEBY"].Visible = false;
            }

        }

        private void toolStripMenuComment_Click(object sender, EventArgs e)
        {
            if (toolStripMenuComment.Checked == true)
            {
                grdView.Columns["COMMENT"].Visible = true;
            }
            else
            {
                grdView.Columns["COMMENT"].Visible = false;
            }

        }

        private void toolStripMenuComment_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuComment.Checked == true)
            {
                grdView.Columns["COMMENT"].Visible = true;
            }
            else
            {
                grdView.Columns["COMMENT"].Visible = false;
            }

        }

        private void toolStripMenuDefault1_Click(object sender, EventArgs e)
        {
            toolStripMenuPatient.Checked = true;
            toolStripMenuName.Checked = true;
            toolStripMenuLastname.Checked = true;
            toolStripMenuLocation.Checked = true;
            toolStripMenuDoctor.Checked = true;
            toolStripMenuReceive.Checked = true;
            toolStripMenuLastupdate.Checked = true;
            toolStripMenuCreateby.Checked = true;
            toolStripMenuComment.Checked = true;
            //
            grdView.Columns["PATNUMBER"].Visible = true;
            grdView.Columns["NAME"].Visible = true;
            grdView.Columns["LASTNAME"].Visible = true;
            grdView.Columns["LOCATION"].Visible = true;
            grdView.Columns["DOCTOR"].Visible = true;
            grdView.Columns["INCOMMINGDATE"].Visible = true;
            grdView.Columns["LASTUPDATE"].Visible = true;
            grdView.Columns["CREATEBY"].Visible = true;
            grdView.Columns["COMMENT"].Visible = true;

        }

        private void frmPatientMgnt_FormClosed(object sender, FormClosedEventArgs e)
        {
            string Patnumber = "";
            string Name = "";
            string Lastname = "";
            string Location = "";
            string Doctor = "";
            string Register_date = "";
            string Lastupdate = "";
            string Create_by = "";
            string Comment = "";
            string Accessnumber = "";
            string Receive_date = "";
            string Protocol = "";
            string Specimen = "";
            string Receive_by = "";
            string Status = "";

            foreach (ToolStripMenuItem item in contextMenuStrip1.Items)
            {
                if (toolStripMenuPatient.Checked == true){ Patnumber = "Patient_number=1"; } else{ Patnumber = "Patient_number=0"; }
                if (toolStripMenuName.Checked == true) { Name = "Name=1"; } else { Name = "Name=0"; }
                if (toolStripMenuLastname.Checked == true) { Lastname = "Lastname=1"; } else { Lastname = "Lastname=0"; }
                if (toolStripMenuLocation.Checked == true) { Location = "Location=1"; } else { Location = "Location=0"; }
                if (toolStripMenuDoctor.Checked == true) { Doctor = "Doctor=1"; } else { Doctor = "Doctor=0"; }
                if (toolStripMenuRegister.Checked == true) { Register_date = "Register_date=1"; } else { Register_date = "Register_date=0"; }
                if (toolStripMenuLastupdate.Checked == true) { Lastupdate = "Lastupdate=1"; } else { Lastupdate = "Lastupdate=0"; }
                if (toolStripMenuCreateby.Checked == true) { Create_by = "Create_by=1"; } else { Create_by = "Create_by=0"; }
                if (toolStripMenuComment.Checked == true) { Comment = "Comment=1"; } else { Comment = "Comment=0"; }
                if (ToolStripAccessnumber.Checked == true) { Accessnumber = "Accessnumber=1"; } else { Accessnumber = "Accessnumber=0"; }
                if (toolStripReceivedate.Checked == true) { Receive_date = "Receive_date=1"; } else { Receive_date = "Receive_date=0"; }
                if (toolStripMenuProtocol.Checked == true) { Protocol = "Protocol=1"; } else { Protocol = "Protocol=0"; }
                if (toolStripMenuSpecimen.Checked == true) { Specimen = "Specimen_name=1"; } else { Specimen = "Specimen_name=0"; }
                if (toolStripMenuReceive.Checked == true) { Receive_by = "Receive_by=1"; } else { Receive_by = "Receive_by=0"; }
                if (toolStripMenuStatus.Checked == true) { Status = "Status=1"; } else { Status = "Status=0"; }
            }
            StringBuilder newSample = new StringBuilder();
            String WriterParser =

             @"[frmPatientMgnt]" + "\r\n" +
            Patnumber + "\r\n" +
            Name + "\r\n" +
            Lastname + "\r\n" +
            Location + "\r\n" +
            Doctor + "\r\n" +
            Register_date + "\r\n" +
            Lastupdate + "\r\n" +
            Create_by + "\r\n" +
            Comment + "\r\n" +
            Accessnumber + "\r\n" +
            Receive_date + "\r\n" +
            Protocol + "\r\n" +
            Specimen + "\r\n" +
            Receive_by + "\r\n" + 
            Status;
            using (StreamWriter writer = new StreamWriter(StrPARSER))
            {
                writer.WriteLine(WriterParser);
            }
        }
        private void CreateMenuStrip()
        {
            if (A == 0)
            {
                toolStripMenuPatient.Checked = false;
                grdView.Columns["PATNUMBER"].Visible = false;
            }
            else if (A == 1)
            {
                toolStripMenuPatient.Checked = true;
                grdView.Columns["PATNUMBER"].Visible = true;
            }
            if (B == 0)
            {
                toolStripMenuName.Checked = false;
                grdView.Columns["NAME"].Visible = false;
            }
            else if (B == 1)
            {
                toolStripMenuName.Checked = true;
                grdView.Columns["NAME"].Visible = true;
            }
            if (C == 0)
            {
                toolStripMenuLastname.Checked = false;
                grdView.Columns["LASTNAME"].Visible = false;
            }
            else if (C == 1)
            {
                toolStripMenuLastname.Checked = true;
                grdView.Columns["LASTNAME"].Visible = true;
            }
            if (D == 0)
            {
                toolStripMenuLocation.Checked = false;
                grdView.Columns["LOCATION"].Visible = false;
            }
            else if (D == 1)
            {
                toolStripMenuLocation.Checked = true;
                grdView.Columns["LOCATION"].Visible = true;
            }
            if (E == 0)
            {
                toolStripMenuDoctor.Checked = false;
                grdView.Columns["DOCTOR"].Visible = false;
            }
            else if (E == 1)
            {
                toolStripMenuDoctor.Checked = true;
                grdView.Columns["DOCTOR"].Visible = true;
            }
            if (F == 0)
            {
                toolStripMenuRegister.Checked = false;
                grdView.Columns["INCOMMINGDATE"].Visible = false;
            }
            else if (F == 1)
            {
                toolStripMenuRegister.Checked = true;
                grdView.Columns["INCOMMINGDATE"].Visible = true;
            }
            if (G == 0)
            {
                toolStripMenuLastupdate.Checked = false;
                grdView.Columns["LASTUPDATE"].Visible = false;
            }
            else if (G == 1)
            {
                toolStripMenuLastupdate.Checked = true;
                grdView.Columns["LASTUPDATE"].Visible = true;
            }
            if (H == 0)
            {
                toolStripMenuCreateby.Checked = false;
                grdView.Columns["CREATEBY"].Visible = false;
            }
            else if (H == 1)
            {
                toolStripMenuCreateby.Checked = true;
                grdView.Columns["CREATEBY"].Visible = true;
            }
            if (I == 0)
            {
                toolStripMenuComment.Checked = false;
                grdView.Columns["COMMENT"].Visible = false;
            }
            else if (I == 1)
            {
                toolStripMenuComment.Checked = true;
                grdView.Columns["COMMENT"].Visible = true;
            }
            if (J == 0)
            {
                ToolStripAccessnumber.Checked = false;
                dataGridView1.Columns["ACCESSNUMBER"].Visible = false;
            }
            else if (J == 1)
            {
                ToolStripAccessnumber.Checked = true;
                dataGridView1.Columns["ACCESSNUMBER"].Visible = true;
            }
            if (K == 0)
            {
                toolStripReceivedate.Checked = false;
                dataGridView1.Columns["RECEIVEDATE"].Visible = false;
            }
            else if (K == 1)
            {
                toolStripReceivedate.Checked = true;
                dataGridView1.Columns["RECEIVEDATE"].Visible = true;
            }
            if (L == 0)
            {
                toolStripMenuProtocol.Checked = false;
                dataGridView1.Columns["PROTOCOL"].Visible = false;
            }
            else if (L == 1)
            {
                toolStripMenuProtocol.Checked = true;
                dataGridView1.Columns["PROTOCOL"].Visible = true;
            }
            if (M == 0)
            {
                toolStripMenuSpecimen.Checked = false;
                dataGridView1.Columns["SPECIMEN"].Visible = false;
            }
            else if (M == 1)
            {
                toolStripMenuSpecimen.Checked = true;
                dataGridView1.Columns["SPECIMEN"].Visible = true;
            }
            if (N == 0)
            {
                toolStripMenuReceive.Checked = false;
                dataGridView1.Columns["RECEIVEDATE"].Visible = false;
            }
            else if (N == 1)
            {
                toolStripMenuReceive.Checked = true;
                dataGridView1.Columns["RECEIVEDATE"].Visible = true;
            }
            if (O == 0)
            {
                toolStripMenuStatus.Checked = false;
                dataGridView1.Columns["STATUS"].Visible = false;
            }
            else if (O == 1)
            {
                toolStripMenuStatus.Checked = true;
                dataGridView1.Columns["STATUS"].Visible = true;
            }

        }
    }
}
