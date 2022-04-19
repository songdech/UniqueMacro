using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UniquePro.Controller;
using UniquePro.Entities;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddColony : Form
    {
        private string type;
        private string strMediaID;

        private  ColonyDescM objColonyM = new ColonyDescM ();


        public frmAddColony(string type, string strMediaID)
        {
            InitializeComponent();
            this.type = type;
            this.strMediaID = strMediaID;
            objColonyM = ControlParameter.ColonyDesc ;

            if (objColonyM == null)
            {
                objColonyM = new ColonyDescM();
                objColonyM.LogUserID = ControlParameter.loginID.ToString ();
            }

        }

        public Action refreshData { get; set; }

        private void queryData()
        {
            if (type == "edit")
            {
                txtCode.Text = ControlParameter.ColonyDesc.ColonyCode;
                txtName.Text = ControlParameter.ColonyDesc.ColonyDescription;
                objColonyM.LogUserID = ControlParameter.loginID ;
            }
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void navRegister_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();

            try
            {
                objColonyM.ColonyDescription= txtName.Text;
                objColonyM.ColonyCode= txtCode.Text;                

                ControlParameter.ColonyDesc  = objConfiguration.SaveColonyDesc (objColonyM);

                this.Close();

                Action instance2 = refreshData;
                if (instance2 != null)
                    instance2();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        
    }

        private void tileNavPane1_Click(object sender, EventArgs e)
        {

        }

        private void frmAddColony_Load(object sender, EventArgs e)
        {
            queryData();
        }
    }
}
