using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNIQUE.TabbedView.WidgetView
{
    public partial class LoginInfo : UserControl
    {
        public LoginInfo()
        {
            InitializeComponent();
            OnClick();
        }

        private void OnClick()
        {
        //    labelControl1.Text = "<size=10> " + "นายพิทักศิลป์ คำเสมอ" + "<br> <size=8>" + "" + " Doctor";

            label1.Text = ControlParameter.loginName + " " + ControlParameter.loginLastName;
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
