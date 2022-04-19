using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniquePro.Entities.Patient;

namespace UNIQUE.PatientFld
{
    public partial class frmPatientHistory : Form
    {
        private string Patnum;
        private string PatAltnumber;
        private string PatHosnumber;
        private string Patname;
        private string PatTitle;
        private string PatBirthdate;
        private string PatSex;
        private string PatAge;

        private string PatAddress1;
        private string PatAddress2;
        private string PatCity;
        private string PatState;
        private string PatPostcode;
        private string PatContry;
        private string PatTel1;
        private string PatTel2;
        private string PatEmail;
        private string PatComment;




        public frmPatientHistory(PatientM objPatientM)
        {
            InitializeComponent();
            this.Patnum = objPatientM.PatientNo;
            this.PatTitle = objPatientM.Title1;
            this.Patname = objPatientM.PatientName;
            this.PatBirthdate = objPatientM.BirthDate;
            this.PatSex = objPatientM.Sex;
            this.PatAge = objPatientM.Age;
            this.PatAltnumber = objPatientM.PatAltnumber;

            this.PatAddress1 = objPatientM.Address1;
            this.PatAddress2 = objPatientM.Address2;
            this.PatCity = objPatientM.City;
            this.PatState = objPatientM.State;
            this.PatPostcode = objPatientM.PostCode;
            this.PatContry = objPatientM.Country;
            this.PatTel1 = objPatientM.Telephone;
            this.PatTel2 = objPatientM.Telephone2;
            this.PatEmail = objPatientM.Email;
            this.PatComment = objPatientM.Comment;
        }

        private void frmPatientHistory_Load(object sender, EventArgs e)
        {
            lblPatnum.Text = Patnum;
            lblAltnum.Text = PatAltnumber;
            lblName.Text = Patname;
            //lblAdmit.Text =
            lblDOB.Text = PatBirthdate;
            lblSex.Text = PatSex;
            //lblBGroup.Text = 
            lblAddress1.Text = PatAddress1;
            lblAddress2.Text = PatAddress2;
            lblCity.Text = PatCity;
            lblPoscode.Text = PatPostcode;
            lblContry.Text = PatContry;
            lblTel1.Text = PatTel1;
            lblTel2.Text = PatTel2;
            lblEmail.Text = PatEmail;
        }
    }
}
