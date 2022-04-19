using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNIQUE.PatientFld
{
    public partial class frmPatientMgnt : Form
    {
        public frmPatientMgnt()
        {
            InitializeComponent();
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
            PatientFld.frmPatientHistory Fm1 = new frmPatientHistory();

            if (!CheckOpened(Fm1.Text))
            {
                Fm1 = new frmPatientHistory();
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
                Fm1 = new frmPatientRegister("Edit");
                Fm1.Show();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PatientFld.frmPatientRegister Fm1 = new frmPatientRegister("New");

            if (!CheckOpened(Fm1.Text))
            {
                Fm1 = new frmPatientRegister("New");
                Fm1.Show();
            }
        }
    }
}
