using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNIQUE.ConfigurationFld
{
    public partial class menubar : UserControl
    {
        public menubar()
        {
            InitializeComponent();
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //Control fm = new Control();
            //fm.Controls.Add();

            navBarItem1.Tag = new Controls("Tasks", typeof(ConfigurationFld.frmDoctors));
        }

        private void navBarControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
