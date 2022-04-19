using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UNIQUE.Instrument.MicroScan.Controller;
using UNIQUE.Instrument.MicroScan.Entites;

namespace UNIQUE.Instrument.MicroScan
{
    public partial class Form_microscan_config : Form
    {
        private Settings_MicroScan settings = Settings_MicroScan.Default;

        private MicroscanController objConfig = new MicroscanController();
        private Terminal_GETM objTerminalM;

        public Form_microscan_config()
        {
            InitializeComponent();
            settings.Reload();
            InitializeControlValues();
        }
        private void Form_microscan_config_Load(object sender, EventArgs e)
        {
            try
            {
                objTerminalM = new Terminal_GETM();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitializeControlValues()
        {
            cmbParity.Items.Clear(); cmbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
            cmbStopBits.Items.Clear(); cmbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

            cmbParity.Text = settings.Parity.ToString();
            cmbStopBits.Text = settings.StopBits.ToString();
            cmbDataBits.Text = settings.DataBits.ToString();
            cmbParity.Text = settings.Parity.ToString();
            cmbBaudRate.Text = settings.BaudRate.ToString();
            CurrentDataMode = settings.DataMode;

            //RefreshComPortList();

            // If it is still avalible, select the last com port used
            if (cmbPortName.Items.Contains(settings.PortName)) cmbPortName.Text = settings.PortName;
            else if (cmbPortName.Items.Count > 0) cmbPortName.SelectedIndex = cmbPortName.Items.Count - 1;
            else
            {
                MessageBox.Show(this, "There are no COM Ports detected on this computer.\nPlease install a COM Port and restart this app.", "No COM Ports Installed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void Form_microscan_config_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
        private DataMode CurrentDataMode
        {
            get
            {
                if (rbHex.Checked)
                {
                    return DataMode.Hex;
                }
                else
                {
                    return DataMode.Text;
                }
            }
            set
            {
                if (value == DataMode.Text)
                {
                    rbText.Checked = true;
                }
                else
                {
                    rbHex.Checked = true;
                }
            }
        }

        private void SaveSettings()
        {
            try
            {
                settings.BaudRate = int.Parse(cmbBaudRate.Text);
                settings.DataBits = int.Parse(cmbDataBits.Text);
                settings.DataMode = CurrentDataMode;
                settings.Parity = (Parity)Enum.Parse(typeof(Parity), cmbParity.Text);
                settings.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cmbStopBits.Text);
                settings.PortName = cmbPortName.Text;
                settings.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rbText_CheckedChanged(object sender, EventArgs e)
        {
            { if (rbText.Checked) CurrentDataMode = DataMode.Text; }
        }

        private void rbHex_CheckedChanged(object sender, EventArgs e)
        {
            { if (rbHex.Checked) CurrentDataMode = DataMode.Hex; }
        }

        private void navRegister_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            settings.Save();
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult No = MessageBox.Show("คำเตือน!! คุณต้องการลบใช่หรือไม่? ข้อมูล จะถูกลบออกทั้งหมด ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (No == DialogResult.Yes)
                {
                    DataTable dt = null;

                    // STEP CLEAN DATA IN MICROSCANDB

                    //dt = objConfig.Check_Result_in_SUBREQMB_BATTERIES(objBatteryM);

                    MessageBox.Show("Process delete");

                    //objBatteryM = objConfig.Delete_Battery_Step1(objBatteryM);
                    //objBatteryM = objConfig.Delete_MAPPINGLINK(objBatteryM);
                    objConfig.PurgeData(objTerminalM);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
