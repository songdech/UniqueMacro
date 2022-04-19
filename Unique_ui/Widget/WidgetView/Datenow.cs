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
    public partial class Datenow : UserControl
    {
        Timer timer = new Timer();
        public Datenow()
        {
            InitializeComponent();
            timer.Interval = 1000;
            timer.Tick += OnTick;
            timer.Start();
            OnTick(null, null);
        }

        private void OnTick(object sender, EventArgs e)
        {
            System.DateTime currentDate = System.DateTime.Now;
            labelControl1.Text = "<b>" + string.Format("{0:T}", currentDate) + "</b><br><size=10>" + currentDate.ToString("D");
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

           
     
        }

       
    }
}
