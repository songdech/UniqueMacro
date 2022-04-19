using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UNIQUE.OEM;

namespace UNIQUE
{
    public partial class FormMain : Form
    {

        internal static Timer timerIdle;
        frmLogin _frmLogin;

    public FormMain()
        {
            InitializeComponent();       
        }

                 
        private void Form1_Load(object sender, EventArgs e)
        {
           // mainTileBar.SelectedItem = menuPatientmanagement;
            Welcome userControl = new Welcome();
            userControl.Dock = DockStyle.Fill;
            xtraUserControl_main.Controls.Clear();
            xtraUserControl_main.Controls.Add(userControl);

            this.Text = "UNIQUE DATA Microbiology. Laboratory Information System. v." +  getVersion();

       ////     //SESSION of the LOGIN.
       //timerIdle = new System.Windows.Forms.Timer();
       //timerIdle.Enabled = true;
       //timerIdle.Interval = 5000; // Idle time period. Here after 5 minutes perform task in  timerIdle_Tick
       //timerIdle.Tick += new EventHandler(timerIdle_Tick);


        }

        private void timerIdle_Tick(object sender, EventArgs e)
        {
            //Here perform your action by first validating that idle task is not already running.
            // If you want to redirect user to login page, then first check weather login page is already displayed or not
            // if not then show loign page. Same logic for other task or Implement your own. 
            // Remember after every five minutes or period you defined above this timerIdle_Tick will be called 
            ////so first check weather idle task is already running or not. If not then perform 
            //if (Login.Visible == false)
            //{
            //    PerformNecessoryActions();
            //    ShowLoginForm();
            //}
          //  MessageBox.Show("timout.");
            if (!CheckOpened(this.Text))
            {
                
            }
            _frmLogin = new frmLogin();
            if (_frmLogin.Visible == false)
            {
                _frmLogin.ShowDialog();
            }
        
        }
         

        private void menuPatientmanagement_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
         //   xtr
            //Patient userControl = new Patient();
            //userControl.Dock = DockStyle.Fill;
            //xtraUserControl_main.Controls.Clear();
            //xtraUserControl_main.Controls.Add(userControl);
            PatientFld.frmPatientMgnt fm = new PatientFld.frmPatientMgnt();
            fm.Show();
          
        }

       

        private void tileBarItem1_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            Welcome userControl = new Welcome();
            userControl.Dock = DockStyle.Fill;
            xtraUserControl_main.Controls.Clear();
            xtraUserControl_main.Controls.Add(userControl);
        }

        private void navButtonClose_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {

            //this.Hide();

            DialogResult yes = MessageBox.Show("Do you want to exit program ?", " Exit Program", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (yes == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void tileBarItem8_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

        }

        public static string getVersion()
        {
            System.Reflection.Assembly ProjectAssembly;
            ProjectAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            return ProjectAssembly.GetName().Version.ToString();
        }

        private void tileBarItem3_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            Form_REQUESTS fm = new Form_REQUESTS();

            //RequestFld.frmRequestMgnt fm = new RequestFld.frmRequestMgnt();


            if (!CheckOpened(fm.Text))
            {
                /**Edit By Songdech S.
                 * Edit Date: 17/03/2021
                 * Description: add new function for search test result
                 * 
                 */

                fm.Form_Search_TestResult = false;
                //fm = new RequestFld.Form_REQUESTS();
                fm.Show();
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

        private void tileBarItem9_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            Configuration.frmConfigurationMain fm = new Configuration.frmConfigurationMain();


            if (!CheckOpened(fm.Text))
            {
                fm = new Configuration.frmConfigurationMain();
                fm.Show();
            }
        }

        private void tileBarItem2_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            //Result.frmStain fm = new Result.frmStain();
            //Result.frmResultSearch fm = new Result.frmResultSearch();
            Form_REQUESTS fm = new Form_REQUESTS();

            if (!CheckOpened(fm.Text))
            {
                fm.Form_Search_TestResult = true;                
                fm.Show();
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Result.frmIdentificationAndSensititities fm = new Result.frmIdentificationAndSensititities();
            fm.Show();
        }

        private void tileNavPane_Click(object sender, EventArgs e)
        {

        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            //Application.Exit();
        }

        private void tileBarItem4_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            Forms.Specimen.Form_Rectube  fm = new Forms.Specimen.Form_Rectube("");
            if (!CheckOpened(fm.Text))
            {
                fm = new Forms.Specimen.Form_Rectube("");
                fm.Show();
            }
        }

        private void mainTileBar_Click(object sender, EventArgs e)
        {

        }

        private void navButton5_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            ConfigurationApp.frmConfigurationApp fm = new ConfigurationApp.frmConfigurationApp();


            if (!CheckOpened(fm.Text))
            {
                fm = new ConfigurationApp.frmConfigurationApp();
                fm.Show();
            }

        }

        private void tileBarItem5_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            Micro_Manual fm = new Micro_Manual();

            if (!CheckOpened(fm.Text))
            {
                fm = new Micro_Manual();
                fm.Show();
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult yes = MessageBox.Show("Do you want to exit program ?", " Exit Program", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (yes == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void navButton3_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmLogin fm = new frmLogin();

            fm.Show();
            this.Close();

        }
    }
}
