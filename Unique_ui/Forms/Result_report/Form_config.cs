using System;
using System.Data;
using System.Windows.Forms;

namespace UNIQUE
{
    public partial class Form_config : Form
    {
        string Str_calendar = Properties.Settings.Default.Calendar;

        private MBReportM objMBReportM;
        private MBReportController objConfig;

        public Form_config()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                Str_calendar = "TH";
                Properties.Settings.Default.Calendar = Str_calendar;
                Properties.Settings.Default.Save();
            }
            else
            {
                checkBox2.Checked = true;
                Str_calendar = "ENG";
                Properties.Settings.Default.Calendar = Str_calendar;
                Properties.Settings.Default.Save();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                Str_calendar = "ENG";
                Properties.Settings.Default.Calendar = Str_calendar;
                Properties.Settings.Default.Save();
            }
            else
            {
                checkBox1.Checked = true;
                Str_calendar = "TH";
                Properties.Settings.Default.Calendar = Str_calendar;
                Properties.Settings.Default.Save();
            }
        }

        private void Form_config_Load(object sender, EventArgs e)
        {
            objMBReportM = new MBReportM();
            objConfig = new MBReportController();

            if (Str_calendar =="TH")
            {
                checkBox1.Checked = true;
                checkBox2.Checked = false;
            }
            else if (Str_calendar == "ENG")
            {
                checkBox1.Checked = false;
                checkBox2.Checked = true;
            }
        }
    }
}
