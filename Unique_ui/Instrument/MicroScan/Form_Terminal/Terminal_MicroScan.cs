using System;
using System.Linq;
using System.Data;
using System.Text;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using System.Threading;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Timers;
using UNIQUE;
using UNIQUE.Properties;
using UNIQUE.Instrument.MicroScan.Controller;
using UNIQUE.Instrument.MicroScan.Entites;
using UNIQUE.Instrument.MicroScan;

namespace UNIQUE
{
    #region Public Enumerations

    public enum DataMode { Text, Hex }

    public enum LogMsgType { Incoming, Outgoing, Normal, Warning, Error };
    #endregion

    public partial class Terminal_MicroScan : Form
    {
        private MicroscanController objConstring = new MicroscanController();
        private Terminal_GETM objTerminalM;

        private string SN = "";
        bool BoolBuildstring = false;

        StringBuilder strTemp;


        public static Terminal_MicroScan instance;
        private int duration = 6;

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        private const int RECV_DATA_MAX = 10240;
        private void frmTerminal_Load(object sender, EventArgs e)
        {
            try
            {
                objTerminalM = new Terminal_GETM();
                Terminal_Matching FM_Microscan_Matching = new Terminal_Matching();
                FM_Microscan_Matching.Show();


                string Pathspy = Settings_MicroScan.Default.Pathspy;
                textBox1.Text = Settings_MicroScan.Default.Pathspy;

                Terminal_Matching.instance.ButtonProcess.Visible = true;

                Auto_Opencomport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // If the com port has been closed, do nothing
            if (!comport.IsOpen) return;

            string data = "";
            comport.NewLine = "\r\n";
            comport.ReadTimeout = 5000;

            string IDCounterM1 = "";

            IDCounterM1 = Settings_MicroScan.Default.CounterM1;

            if (BoolBuildstring == false)
            {
                strTemp = new StringBuilder();
                strTemp.Clear();
            }

            if (comport.BytesToRead > 13)
            {
                try
                {
                    BoolBuildstring = true;

                    // Streming data by ReadLine
                    data = comport.ReadLine();

                    StringReader strReader = new StringReader(data);
                    string String_read_line = strReader.ReadToEnd();
                    string SeqMen_ALL = String_read_line.Substring(2, 2);   //  Get Messages 2 Digit for switch case,

                    switch (SeqMen_ALL)
                    {
                        case "\"H":     // Header Record (H)
                            //Writedatalog.Log_Microscan("Get-Messages = Herder");
                            strTemp.AppendLine(String_read_line);
                            Worker_Process();
                            break;
                        case "P,":      // Patient record (P)
                            //Writedatalog.Log_Microscan("Get-Messages = P,");
                            strTemp.AppendLine(String_read_line);

                            Worker_Process();
                            break;
                        case "B,":      // Specimen record (B)
                            //Writedatalog.Log_Microscan("Get-Messages = B,");
                            strTemp.AppendLine(String_read_line);

                            Worker_Process();
                            break;
                        case "R,":      // Isolate record (R)
                            //Writedatalog.Log_Microscan("Get-Messages = R,");
                            strTemp.AppendLine(String_read_line);

                            Worker_Process();
                            break;
                        case "M,":      // Test/MIC record (M)
                            //Writedatalog.Log_Microscan("Get-Messages = M,");
                            strTemp.AppendLine(String_read_line);
                            Worker_Process();
                            break;
                        case "Q,":      // Trace record (Q)
                            //Writedatalog.Log_Microscan("Get-Messages = Q,");
                            strTemp.AppendLine(String_read_line);
                            Worker_Process();
                            break;
                        case "C,":      // Comment record (C)
                            //Writedatalog.Log_Microscan("Get-Messages = C,");
                            strTemp.AppendLine(String_read_line);
                            Worker_Process();
                            break;
                        case "F,":      // Free text record (F)
                            //Writedatalog.Log_Microscan("Get-Messages = F,");
                            strTemp.AppendLine(String_read_line);
                            Worker_Process();
                            break;
                        case "A,":      // Request record (A)
                            //Writedatalog.Log_Microscan("Get-Messages = A,");
                            strTemp.AppendLine(String_read_line);
                            Worker_Process();
                            break;
                        case "L,":      // End Record (L)
                            //Writedatalog.Log_Microscan("Get-Messages = L,");
                            strTemp.AppendLine(String_read_line);

                            WriteFiles(IDCounterM1, strTemp);
                            Worker_Process();
                            break;
                    }

                    Writedatalog.Log_Microscan(data);
                    Log(LogMsgType.Incoming, data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else if (comport.BytesToRead >= 1)
            {
                comport.Write(c, 0, c.Length);
            }
        }
        private void WriteFiles(string Str_Counter, StringBuilder Str_Files)
        {
            // Write files into Folder \\WST
            try
            {
                string Pathwst = @".\WST\";
                if (Directory.Exists(Pathwst))
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter(Pathwst + "wst_" + Str_Counter + ".txt", true, Encoding.GetEncoding("TIS-620"));
                    file.Write(Str_Files);
                    file.Close();
                }
                else
                {
                    Directory.CreateDirectory(Pathwst);
                    System.IO.StreamWriter file = new System.IO.StreamWriter(Pathwst + "wst_" + Str_Counter + ".txt", true, Encoding.GetEncoding("TIS-620"));
                    file.Write(Str_Files);
                    file.Close();
                }
                BoolBuildstring = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Write files Terminal!" , ex.Message);
            }
            // Create new counter number
            string CounterM1 = Settings_MicroScan.Default.CounterM1;
            int IDcount = Convert.ToInt32(CounterM1) + 1;
            string LBLvalue = string.Format("{0:D6}", IDcount);
            Settings_MicroScan.Default.CounterM1 = LBLvalue;
            Settings_MicroScan.Default.Save();

        }
        private void button4_Click(object sender, EventArgs e)
        {
            timer_Writefiles.Start();
            duration = 6;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Test Timer
            //timer_Writefiles = new System.Windows.Forms.Timer();
            //timer_Writefiles.Tick += new EventHandler(count_down);
            //timer_Writefiles.Interval = 1000;
            //timer_Writefiles.Start();

            // END test timer
            // TEST Set boolean
            //objTerminalM.StartWrite_Bool = true;
            //if (objTerminalM.StartWrite_Bool)
            //{
            //    button1.Text = "Start Write";
            //}
            // END Test Set Boolean


            //// TEST Check 2 Directory & Write files
            //StringBuilder strTemp = new StringBuilder();
            ////strTemp.Clear();

            //strTemp.Append("test");

            //WriteFiles("000001", strTemp);
            // END TEST Check 2

            // TEST Check 1 Directory & Write files
            //StringBuilder strTemp = new StringBuilder();
            //strTemp.Clear();

            //strTemp.Append("test");

            //string Pathwst = @".\WST\";

            //if (Directory.Exists(Pathwst))
            //{
            //    System.IO.StreamWriter file = new System.IO.StreamWriter(Pathwst + "test_" + ".txt", true, Encoding.GetEncoding("TIS-620"));
            //    file.Write(strTemp);
            //    file.Close();
            //}
            //else
            //{
            //    Directory.CreateDirectory(Pathwst);

            //    System.IO.StreamWriter file = new System.IO.StreamWriter(Pathwst + "test_" + ".txt", true, Encoding.GetEncoding("TIS-620"));
            //    file.Write(strTemp);
            //    file.Close();
            //}
            Worker_Process();
            // END TEST Check 1




            // TEST Cut string
            //string Messages = " ,\"Labpro\", ";

            //string result = "";
            //string[] Messages_H = Messages.Split(',');

            ////result = Messages_H[1].Trim(new Char[] { ' ', '"', '"' });
            //result = Messages_H[1].Trim('"');

            //Char Header = result[result.Length - 1];
            //MessageBox.Show(Header.ToString());

            //Worker_Process();

            //dataGridView_Rescontrol.Rows.Clear();
            // END TEST Cut string

            // TEST CONNECTION
            //try
            //{
            //    DataTable dt = objConstring.GetDICTBattery();

            //    if (dt.Rows.Count > 0)
            //    {
            //        MessageBox.Show(">0");
            //    }
            //    else
            //    {
            //        MessageBox.Show("=0");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            // END TESTCONNECTION 

            //var s = "This is a string";
            //var ascii = Encoding.ASCII.GetBytes(s);
            //byte[] ascii2 = Encoding.ASCII.GetBytes(s);
            //for (int i = 0; i < s.Length; i++)
            //MessageBox.Show(ascii2[i] + " "); // print num
            //for (int i = 0; i < s.Length; i++)
            //{
            //    var s1 = s[i];
            //    MessageBox.Show(s1 + " "); // Character
            //}
            //for (int i = 0; i < s.Length; i++) MessageBox.Show(s[i] + " "); // Character
            //var ascii3 = s.ToCharArray();
            //for (int i = 0; i < s.Length; i++) MessageBox.Show(ascii3.ElementAt(i) + " "); // Character
            //for (int i = 0; i < s.Length; i++) MessageBox.Show(ascii3[i] + " "); // Character
            //Console.ReadLine();


            //try
            //{
            //    richTextBox1.Text = "";
            //    comport.Open();
            //    buffer[0] = 0xFF;
            //    comport.Write(buffer, 0, 1);   // send the null command
            //    try
            //    {
            //        int data = comport.ReadByte();   // look for an echo
            //        if (data == 255)
            //            serialEcho = true;  // we get here if we are using RS-232
            //    }
            //    catch (TimeoutException)
            //    {
            //        serialEcho = false;     // we get here if we are using TTL serial
            //    }

            //    // read the device signature
            //    string str = "";
            //    buffer[0] = 0x81;
            //    if (!(sendCommandPacket(buffer, 1, 7, ref str)))
            //        richTextBox1.Text += "failure\r\n" + str;
            //    else
            //    {
            //        richTextBox1.Text += "" + str + "\r\n";
            //        for (int i = 0; i < 7; i++)
            //            richTextBox1.Text += (char)buffer[i];
            //    }
            //}
            //catch (Exception)
            //{
            //    richTextBox1.Text += "Exception!";
            //}
            //if (comport.IsOpen)
            //    comport.Close();

        }
        private void count_down()
        {
            if (duration == 0)
            {
                //timer_Writefiles.Stop();
                button3.Text = "Pickup";
                button3.Visible = false;

                Terminal_Matching.instance.ButtonProcess.Visible = true;
            }
            else if (duration > 0)
            {
                duration--;
                button3.Text = duration.ToString();
                button3.BackColor = Color.GreenYellow;
                button3.Visible = true;

                Terminal_Matching.instance.ButtonProcess.Visible = false;
            }
        }
        private void timer_Writefiles_Tick(object sender, EventArgs e)
        {
            count_down();
        }

        #region Local Variables
        private const Byte SOH = 0x01;
        private const Byte STX = 0x02;
        private const Byte ETX = 0x03;
        private const Byte EOT = 0x04;
        private const Byte ENQ = 0x05;
        private const Byte ACK = 0x06;
        private const Byte CR = 0x0d;
        private const Byte NUL = 0x00;

        //private const Byte  \x08 = BEL
        //private const Byte  \x09 = HT
        //private const Byte  \x0A = LF
        //private const Byte  \x0C = FF
        //private const Byte  \x0D = CR
        //private const Byte  \x0E = SO
        //private const Byte  \x0F = SI
        //private const Byte  \x10 = DLE
        //private const Byte  \x11 = DC1
        //private const Byte  \x13 = DC3
        //private const Byte  \x14 = DC4
        //private const Byte / \x15 = NAK
        //private const Byte  \x16 = SYN
        private const Byte ETB = 0x17;
        //private const Byte  \x19 = EM


        // The main control for communicating through the RS-232 port
        private SerialPort comport = new SerialPort();

        // Various colors for logging info
        private Color[] LogMsgTypeColor = { Color.Blue, Color.Green, Color.Black, Color.Orange, Color.Red };

        // Temp holder for whether a key was pressed
        private bool KeyHandled = false;

        private Settings_MicroScan settings = Settings_MicroScan.Default;
        #endregion

        #region Constructor

        System.Timers.Timer NoDataAtPort = new System.Timers.Timer(10000);
        public enum REPLY : int { NO_REPLY, YES_REPLY, TIMEOUT_REPLY }
        string InputData = String.Empty;
        delegate void SetTextCallback(string text);

        public Terminal_MicroScan()
        {
            // Load user settings
            settings.Reload();

            // Build the form
            InitializeComponent();

            // Restore the users settings
            InitializeControlValues();

            // Enable/disable controls based on the current state
            EnableControls();

            // When data is recieved through the port, call this method
            comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            comport.PinChanged += new SerialPinChangedEventHandler(comport_PinChanged);
        }

        void comport_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            // Show the state of the pins
            UpdatePinState();
        }

        private void UpdatePinState()
        {
            this.Invoke(new ThreadStart(() =>
            {
                // Show the state of the pins
                chkCD.Checked = comport.CDHolding;
                chkCTS.Checked = comport.CtsHolding;
                chkDSR.Checked = comport.DsrHolding;
            }));
        }
        #endregion

        #region Local Methods
        /// <summary> Save the user's settings. </summary>
        private void SaveSettings()
        {
            settings.BaudRate = int.Parse(cmbBaudRate.Text);
            settings.DataBits = int.Parse(cmbDataBits.Text);
            settings.DataMode = CurrentDataMode;
            settings.Parity = (Parity)Enum.Parse(typeof(Parity), cmbParity.Text);
            settings.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cmbStopBits.Text);
            settings.PortName = cmbPortName.Text;
            settings.ClearOnOpen = chkClearOnOpen.Checked;
            settings.ClearWithDTR = chkClearWithDTR.Checked;

            settings.Save();
        }
        /// <summary> Populate the form's controls with default settings. </summary>
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

            RefreshComPortList();

            chkClearOnOpen.Checked = settings.ClearOnOpen;
            chkClearWithDTR.Checked = settings.ClearWithDTR;

            // If it is still avalible, select the last com port used
            if (cmbPortName.Items.Contains(settings.PortName)) cmbPortName.Text = settings.PortName;
            else if (cmbPortName.Items.Count > 0) cmbPortName.SelectedIndex = cmbPortName.Items.Count - 1;
            else
            {
                MessageBox.Show(this, "There are no COM Ports detected on this computer.\nPlease install a COM Port and restart this app.", "No COM Ports Installed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        /// <summary> Enable/disable controls based on the app's current state. </summary>
        private void EnableControls()
        {
            // Enable/disable controls based on whether the port is open or not
            gbPortSettings.Enabled = !comport.IsOpen;
            txtSendData.Enabled = btnSend.Enabled = comport.IsOpen;
            //chkDTR.Enabled = chkRTS.Enabled = comport.IsOpen;

            if (comport.IsOpen)
            {
                btnOpenPort.Text = "&Close Port";
                pictureBox3.Visible = true;
                pictureBox4.Visible = false;
            }
            else
            {
                btnOpenPort.Text = "&Open Port";
                pictureBox3.Visible = false;
                pictureBox4.Visible = true;
            }
        }

        /// <summary> Send the user's data currently entered in the 'send' box.</summary>
        private void SendData()
        {
            if (CurrentDataMode == DataMode.Text)
            {
                // \x00 = NUL
                // \x01 = SOH
                // \x02 = STX
                // \x03 = ETX
                // \x04 = EOT
                // \x05 = ENQ
                // \x06 = ACK
                // \x08 = BEL
                // \x09 = HT
                // \x0A = LF
                // \x0B = VT
                // \x0C = FF
                // \x0D = CR
                // \x0E = SO
                // \x0F = SI
                // \x10 = DLE
                // \x11 = DC1
                // \x12 = DC2
                // \x13 = DC3
                // \x14 = DC4
                // \x15 = NAK
                // \x16 = SYN
                // \x17 = ETB
                // \x19 = EM

                string lon = "\x06";   // <STX>LON<ETX>
                // Send the user's text straight out the port
                //comport.Write(txtSendData.Text);






                comport.Write(lon);
                // Show in the terminal window the user's text
                Log(LogMsgType.Outgoing, txtSendData.Text + "\n");
            }
            else
            {
                try
                {
                    // Convert the user's string of hex digits (ex: B4 CA E2) to a byte array
                    byte[] data = HexStringToByteArray(txtSendData.Text);

                    // Send the binary data out the port
                    comport.Write(data, 0, data.Length);

                    // Show the hex digits on in the terminal window
                    Log(LogMsgType.Outgoing, ByteArrayToHexString(data) + "\n");
                }
                catch (FormatException)
                {
                    // Inform the user if the hex string was not properly formatted
                    Log(LogMsgType.Error, "Not properly formatted hex string: " + txtSendData.Text + "\n");
                }
            }
            txtSendData.SelectAll();
        }

        /// <summary> Log data to the terminal window. </summary>
        /// <param name="msgtype"> The type of message to be written. </param>
        /// <param name="msg"> The string containing the message to be shown. </param>
        private void Log(LogMsgType msgtype, string msg)
        {
            rtfTerminal.Invoke(new EventHandler(delegate
            {
                rtfTerminal.SelectedText = string.Empty;
                rtfTerminal.SelectionFont = new Font(rtfTerminal.SelectionFont, FontStyle.Bold);
                rtfTerminal.SelectionColor = LogMsgTypeColor[(int)msgtype];
                rtfTerminal.AppendText(msg);
                rtfTerminal.ScrollToCaret();
            }));
        }

        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
        /// <returns> Returns an array of bytes. </returns>
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }
        #endregion

        #region Local Properties
        private DataMode CurrentDataMode
        {
            get
            {
                if (rbHex.Checked) return DataMode.Hex;
                else return DataMode.Text;
            }
            set
            {
                if (value == DataMode.Text) rbText.Checked = true;
                else rbHex.Checked = true;
            }
        }
        private void frmTerminal_Shown(object sender, EventArgs e)
        {
            Log(LogMsgType.Normal, String.Format("Application Started at {0}\n", DateTime.Now));
        }
        private void rbText_CheckedChanged(object sender, EventArgs e)
        { if (rbText.Checked) CurrentDataMode = DataMode.Text; }
        private void rbHex_CheckedChanged(object sender, EventArgs e)
        { if (rbHex.Checked) CurrentDataMode = DataMode.Hex; }
        private void cmbBaudRate_Validating(object sender, CancelEventArgs e)
        { int x; e.Cancel = !int.TryParse(cmbBaudRate.Text, out x); }
        private void cmbDataBits_Validating(object sender, CancelEventArgs e)
        { int x; e.Cancel = !int.TryParse(cmbDataBits.Text, out x); }
        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            bool error = false;

            // If the port is open, close it.
            if (comport.IsOpen)
            {
                comport.Close();

                label4.Text = "COM Port:";

            }
            else
            {
                var Parity = settings.Parity.ToString();
                var StopBits = settings.StopBits.ToString();
                var DataBits = settings.DataBits.ToString();
                var BaudRate = settings.BaudRate.ToString();
                var DataMode = settings.DataMode;
                var PortName = settings.PortName;

                comport.BaudRate = int.Parse(BaudRate);
                comport.DataBits = int.Parse(DataBits);
                comport.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBits);
                comport.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
                comport.PortName = PortName;

                label4.Text = "Microscan " + PortName;
                // Set the port's settings
                //comport.BaudRate = int.Parse(cmbBaudRate.Text);
                //comport.DataBits = int.Parse(cmbDataBits.Text);
                //comport.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cmbStopBits.Text);
                //comport.Parity = (Parity)Enum.Parse(typeof(Parity), cmbParity.Text);
                //comport.PortName = cmbPortName.Text;

                try
                {
                    // Open the port
                    comport.Open();
                }
                catch (UnauthorizedAccessException) { error = true; }
                catch (IOException) { error = true; }
                catch (ArgumentException) { error = true; }

                if (error) MessageBox.Show(this, "Could not open the COM port.  Most likely it is already in use, has been removed, or is unavailable.", "COM Port Unavalible", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else
                {
                    // Show the initial pin states
                    UpdatePinState();
                    chkDTR.Checked = comport.DtrEnable;
                    chkRTS.Checked = comport.RtsEnable;
                }
            }

            // Change the state of the form's controls
            EnableControls();

            // If the port is open, send focus to the send data box
            //if (comport.IsOpen)
            //{
            //    txtSendData.Focus();
            //    if (chkClearOnOpen.Checked) ClearTerminal();
            //}
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            SendData();
        }
        private void Auto_Opencomport()
        {
            bool error = false;

            // If the port is open, close it.
            if (comport.IsOpen)
            {
                comport.Close();

                label4.Text = "COM Port:";
            }
            else
            {
                var Parity = settings.Parity.ToString();
                var StopBits = settings.StopBits.ToString();
                var DataBits = settings.DataBits.ToString();
                var BaudRate = settings.BaudRate.ToString();
                var DataMode = settings.DataMode;
                var PortName = settings.PortName;

                comport.BaudRate = int.Parse(BaudRate);
                comport.DataBits = int.Parse(DataBits);
                comport.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBits);
                comport.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
                comport.PortName = PortName;

                label4.Text = "Microscan " + PortName;

                try
                {
                    comport.Open();
                }
                catch (UnauthorizedAccessException) { error = true; }
                catch (IOException) { error = true; }
                catch (ArgumentException) { error = true; }

                if (error) MessageBox.Show(this, "Could not open the COM port.  Most likely it is already in use, has been removed, or is unavailable.", "COM Port Unavalible", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else
                {
                    // Show the initial pin states
                    UpdatePinState();
                    chkDTR.Checked = comport.DtrEnable;
                    chkRTS.Checked = comport.RtsEnable;
                }
            }

            EnableControls();
        }
        // TEST
        private int readDataSub(Byte[] recvBytes, SerialPort serialPortInstance)
        {
            int recvSize = 0;
            bool isCommandRes = false;
            byte d;
            // 
            // Distinguish between command response and read data.
            // 
            try
            {
                d = Convert.ToByte(serialPortInstance.ReadByte()); // DirectCast(serialPortInstance.ReadByte(), [Byte])
                recvBytes[System.Math.Max(System.Threading.Interlocked.Increment(ref recvSize), recvSize - 1)] = d;
                if (d == ENQ)
                    // Distinguish between command response and read data.
                    isCommandRes = true;
            }
            catch (TimeoutException generatedExceptionName)
            {
                // No data received.
                return 0;
            }

            // 
            // Receive data until the terminator character.
            // 
            while (true)
            {
                try
                {
                    d = Convert.ToByte(serialPortInstance.ReadByte());
                    recvBytes[System.Math.Max(System.Threading.Interlocked.Increment(ref recvSize), recvSize - 1)] = d;

                    if (isCommandRes && (d == 4))
                        // Command response is received completely.
                        break;

                    else if (d == 2)
                    {

                        MessageBox.Show(d.ToString());

                        comport.Write(c, 0, c.Length);



                        //if (checkDataSize(recvBytes, recvSize))
                        //    // Read data is received completely.
                        //    break;
                    }
                }
                catch (TimeoutException ex)
                {
                    // 
                    // No terminator is received.
                    // 
                    MessageBox.Show(ex.Message);
                    return 0;
                }
            }

            return recvSize;
        }
        private void ReceiveData()
        {
            Byte[] recvBytes = new Byte[RECV_DATA_MAX - 1 + 1];
            int recvSize;

            if (this.comport.IsOpen == false)

                MessageBox.Show(comport.PortName + " is disconnected.");

            while (true)
            {
                try
                {
                    recvSize = readDataSub(recvBytes, this.comport);
                }
                catch (IOException ex)
                {
                    //MessageBox.Show(serialPort1.PortName + "\r\n" + ex.Message);    // disappeared
                    MessageBox.Show(ex.Message);

                    break;
                }
                if (recvSize == 0)
                {
                    //MessageBox.Show(serialPort1.PortName + " has no data.");
                    MessageBox.Show(" has no data.");
                    break;
                }
                else if (recvBytes[0] == ENQ)
                {
                    MessageBox.Show(" ACK.");

                    continue;
                }
                else
                {
                    // 
                    // Show the receive data after converting the receive data to Shift-JIS.
                    // Terminating null to handle as string.
                    // 
                    recvBytes[recvSize] = 0;

                    SN = Encoding.GetEncoding("Shift_JIS").GetString(recvBytes).ToString().Replace("\r", "").Replace("\0", "");

                    if (!(SN.Contains("ERROR") | SN.Contains("ER,lon")))

                        break;
                }
            }
        }
        // TEST
        public static void ReadWholeArray(Stream stream, byte[] data)
        {
            int offset = 0;
            int remaining = data.Length;
            while (remaining > 0)
            {
                int read = stream.Read(data, offset, remaining);
                if (read <= 0)
                    throw new EndOfStreamException
                        (String.Format("End of stream reached with {0} bytes left to read", remaining));
                remaining -= read;
                offset += read;
            }

        }
        private void SetTheText(string strText)
        {
            var MESSAGES_H = 14;
            var MESSAGES_P = 24;
            var MESSAGES_B = 23;
            var MESSAGES_R = 39;
            var MESSAGES_M = 25;
            var MESSAGES_L = 3;

            byte[] c = new byte[] {0x00,0x01
            //0x00,   // \x00 = NUL   Array 0
            //0x01,   // \x01 = SOH   Array 1
            //0x02,   // \x02 = STX   Array 2
            //0x03,   // \x03 = ETX   Array 3
            //0x04,   // \x04 = EOT   Array 4
            //0x05,   // \x05 = ENQ   Array 5
            //0x06,   // \x06 = ACK   Array 6
            //0x08,   // \x08 = BEL   Array 7
            //0x09,   // \x09 = HT    Array 8
            //0x0A,   // \x0A = LF    Array 9
            //0x08,   // \x0B = VT    Array 10
            //0x0C,   // \x0C = FF    Array 11
            //0x0D,   // \x0D = CR    Array 12
            //0x0E,   // \x0E = SO    Array 13
            //0x0F,   // \x0F = SI    Array 14
            //0x10,   // \x10 = DLE   Array 15
            //0x11,   // \x11 = DC1   Array 16
            //0x12,   // \x12 = DC2   Array 17
            //0x13,   // \x13 = DC3   Array 18
            //0x14,   // \x14 = DC4   Array 19
            //0x15,   // \x15 = NAK   Array 20
            //0x16,   // \x16 = SYN   Array 21
            //0x17,   // \x17 = ETB   Array 22
            //0x19    // \x19 = EM    Array 23
            };

            try
            {
                // TEST 1

                // END TEST 1
                //var s = strText.Substring(0,1);
                //Writedatalog.WriteLog(strText);

                StringReader strReader = new StringReader(strText);
                string String_read_line = strReader.ReadToEnd();

                //var ascii = Encoding.ASCII.GetBytes(s);
                //byte[] ascii2 = Encoding.ASCII.GetBytes(s);

                //for (int i = 0; i < s.Length; i++)
                //{
                //    MessageBox.Show(ascii2[i] + " ");

                //    string Str_Ack = Convert.ToString(ascii2[i]);

                //    // \x05 = ENQ
                //    if (Str_Ack == "5")
                //    {
                //        button4.BackColor = Color.LightGreen;
                //        comport.Write(c, 0, c.Length);
                //        Writedatalog.WriteLog(String_read_line);

                //    }
                //    // \x02 = STX
                //    else if (Str_Ack == "2")
                //    {
                //        string[] _line = String_read_line.Split(',');

                //        Writedatalog.WriteLog(String_read_line);

                //        //string str_PATNUM = _line[2].ToString().Trim();
                //        //string str_LASTNAME = _line[3].ToString().Trim();
                //        //string str_PATNAME = _line[4].ToString().Trim();

                //        button4.BackColor = Color.LightGreen;
                //        comport.Write(c, 0, c.Length);
                //    }
                //    // \x04 = EOT
                //    else if (Str_Ack == "4")
                //    {
                //        button4.BackColor = Color.LightGreen;
                //        Writedatalog.WriteLog(String_read_line);

                //        comport.Write(c, 0, c.Length);

                //    } // \x03 = ETX
                //    else if (Str_Ack == "3")
                //    {
                //        button4.BackColor = Color.LightGreen;
                //        Writedatalog.WriteLog(String_read_line);

                Writedatalog.WriteLog(String_read_line);

                //comport.Write(c, 0, c.Length);
                //MessageBox.Show(textBox1.Text);
                //    }
                //    else
                //    {
                //        Writedatalog.WriteLog(String_read_line);
                //        comport.Write(c, 0, c.Length);
                //    }
                //}

                //comport.Write(c, 0, c.Length);
                //    }
                //}
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }
        private object List()
        {
            throw new NotImplementedException();
        }
        private void DoupDate(object s, EventArgs e)
        {
            richTextBox1.Text = comport.ReadExisting();
        }
        //StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        //Thread readThread = new Thread(Read);
        //Boolean _continue = true;

        private delegate void SetTextDeleg(string text);
        static bool _continue;
        private int Reply_Status;
        private void si_DataReceived(string data)
        {
            textBox1.Text = data.Trim();

        }
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            string data = comport.ReadLine();

            this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { data });
        }
        // For TEST 1
        static Encoding enc8 = Encoding.UTF8;
        const int MAX_BUFFER_SIZE = 2048;
        // END TEST 1
        byte[] c = new byte[] { 0x00, 0x01 };
        delegate void serialCalback(string val);
        private void txtSendData_KeyDown(object sender, KeyEventArgs e)
        {
            // If the user presses [ENTER], send the data now
            if (KeyHandled = e.KeyCode == Keys.Enter) { e.Handled = true; SendData(); }
        }
        private void txtSendData_KeyPress(object sender, KeyPressEventArgs e)
        { e.Handled = KeyHandled; }
        #endregion

        private void chkDTR_CheckedChanged(object sender, EventArgs e)
        {
            comport.DtrEnable = chkDTR.Checked;
            if (chkDTR.Checked && chkClearWithDTR.Checked) ClearTerminal();
        }

        private void chkRTS_CheckedChanged(object sender, EventArgs e)
        {
            comport.RtsEnable = chkRTS.Checked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTerminal();
        }

        private void ClearTerminal()
        {
            rtfTerminal.Clear();
        }

        private void tmrCheckComPorts_Tick(object sender, EventArgs e)
        {
            // checks to see if COM ports have been added or removed
            // since it is quite common now with USB-to-Serial adapters
            RefreshComPortList();
        }
        private void RefreshComPortList()
        {
            // Determain if the list of com port names has changed since last checked
            string selected = RefreshComPortList(cmbPortName.Items.Cast<string>(), cmbPortName.SelectedItem as string, comport.IsOpen);

            // If there was an update, then update the control showing the user the list of port names
            if (!String.IsNullOrEmpty(selected))
            {
                cmbPortName.Items.Clear();
                cmbPortName.Items.AddRange(OrderedPortNames());
                cmbPortName.SelectedItem = selected;
            }
        }
        private string[] OrderedPortNames()
        {
            // Just a placeholder for a successful parsing of a string to an integer
            int num;

            // Order the serial port names in numberic order (if possible)
            return SerialPort.GetPortNames().OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out num) ? num : 0).ToArray();
        }

        private string RefreshComPortList(IEnumerable<string> PreviousPortNames, string CurrentSelection, bool PortOpen)
        {
            // Create a new return report to populate
            string selected = null;

            // Retrieve the list of ports currently mounted by the operating system (sorted by name)
            string[] ports = SerialPort.GetPortNames();

            // First determain if there was a change (any additions or removals)
            bool updated = PreviousPortNames.Except(ports).Count() > 0 || ports.Except(PreviousPortNames).Count() > 0;

            // If there was a change, then select an appropriate default port
            if (updated)
            {
                // Use the correctly ordered set of port names
                ports = OrderedPortNames();

                // Find newest port if one or more were added
                string newest = SerialPort.GetPortNames().Except(PreviousPortNames).OrderBy(a => a).LastOrDefault();

                // If the port was already open... (see logic notes and reasoning in Notes.txt)
                if (PortOpen)
                {
                    if (ports.Contains(CurrentSelection)) selected = CurrentSelection;
                    else if (!String.IsNullOrEmpty(newest)) selected = newest;
                    else selected = ports.LastOrDefault();
                }
                else
                {
                    if (!String.IsNullOrEmpty(newest)) selected = newest;
                    else if (ports.Contains(CurrentSelection)) selected = CurrentSelection;
                    else selected = ports.LastOrDefault();
                }
            }

            // If there was a change to the port list, return the recommended default selection
            return selected;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Process.Start("explorer.exe", @"");

            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = fbd.SelectedPath + "\\";

                    string path = Settings_MicroScan.Default.Pathspy;

                    Settings_MicroScan.Default.Pathspy = textBox1.Text;
                    Settings_MicroScan.Default.Save();

                }
            }
        }

        private string Write_ATE(string Data_To_ATE)
        {
            string Data_From_ATE = "";
            Reply_Status = (int)REPLY.NO_REPLY;
            comport.Write(Data_To_ATE);
            while (Reply_Status == (int)REPLY.NO_REPLY)
            {
                NoDataAtPort.Enabled = true;
            }
            NoDataAtPort.Enabled = false;
            if (Reply_Status == (int)REPLY.TIMEOUT_REPLY)
            {
                Reply_Status = (int)REPLY.NO_REPLY;
                comport.Write(Data_To_ATE);
                while (Reply_Status == (int)REPLY.NO_REPLY)
                {
                    NoDataAtPort.Enabled = true;
                }
                NoDataAtPort.Enabled = false;
                if (Reply_Status == (int)REPLY.TIMEOUT_REPLY)
                {
                    Data_From_ATE = "TIMEOUT";
                    return (Data_From_ATE);
                }
                else if (Reply_Status == (int)REPLY.YES_REPLY)
                {
                    Data_From_ATE = comport.ReadTo("\r\n");
                    if ((Data_From_ATE.Substring(0, 1)) == (Data_To_ATE.Substring(1, 1)))
                    {
                        return (Data_From_ATE);
                    }
                }
                else
                {
                    Data_From_ATE = "SERIOUS_ERROR";
                    return (Data_From_ATE);
                }
            }
            else if (Reply_Status == (int)REPLY.YES_REPLY)
            {
                Data_From_ATE = comport.ReadTo("\r\n");
                if ((Data_From_ATE.Substring(0, 1)) == (Data_To_ATE.Substring(1, 1)))
                {
                    return (Data_From_ATE);
                }
                //add hardware replies to this section as below
                /*else if ((Data_From_ATE.Substring(0, 1)) == "E")
                {
                     Reply_Status = (int)REPLY.NO_REPLY;
                     ATEComPort.Write(Data_To_ATE);
                     while (Reply_Status == (int)REPLY.NO_REPLY)
                     {
                         NoDataAtPort.Enabled = true;
                     }
                     NoDataAtPort.Enabled = false;
                     if (Reply_Status == (int)REPLY.TIMEOUT_REPLY)
                     {
                        Data_From_ATE = "TIMEOUT";
                        return (Data_From_ATE);
                     }
                     else if (Reply_Status == (int)REPLY.YES_REPLY)
                     {
                        Data_From_ATE = ATEComPort.ReadTo("\r\n");
                      if ((Data_From_ATE.Substring(0, 1)) == (Data_To_ATE.Substring(1, 1)))
                     {
                            return (Data_From_ATE);
                      }
                      else if ((Data_From_ATE.Substring(0, 1)) == "E")
                      {
                                   return (Data_From_ATE);
                      }
                    }
                    else
                    {
                         Data_From_ATE = "SERIOUS_ERROR";
                          return (Data_From_ATE);
                    }
               }*/
            }
            else
            {
                Data_From_ATE = "SERIOUS_ERROR";
                return (Data_From_ATE);
            }
            return (Data_From_ATE);
        }
        private void OnTimeOut(object sender, ElapsedEventArgs e)
        {
            Reply_Status = (int)REPLY.TIMEOUT_REPLY;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                comport.Close();
                dataReceiveThrread.Abort();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        Thread dataReceiveThrread;
        private void Terminal_MicroScan_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Environment.Exit(1);
        }
        private void btn_exit_Click(object sender, EventArgs e)
        {
            //frmLogin fm = new frmLogin();

            //fm.Show();
            //this.Close();

            Environment.Exit(1);

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin fm = new frmLogin();

            fm.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int LENGTH_FIRST = 9;

                string fmt = new String('0', LENGTH_FIRST -1) + 1;

                button6.Text = fmt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

            private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form_microscan_config FM = new Form_microscan_config();
            FM.ShowDialog();
        }
        struct DataParameter
        {
            public int Process;
            public int Delay;
        }
        private DataParameter _inputparameter;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int process = ((DataParameter)e.Argument).Process;
            int delay = ((DataParameter)e.Argument).Delay;
            int index = 1;
            try
            {
                for (int i = 0; i < process; i++)
                {
                    if (!backgroundWorker1.CancellationPending)
                    {
                        backgroundWorker1.ReportProgress(index++ * 100 / process, string.Format("Process  {0}", i));
                        System.Threading.Thread.Sleep(delay);
                        //..
                        //.. Add code if do anything.
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
            label3.Text = string.Format("Process..{0}%", e.ProgressPercentage);
            progress.Update();

            //timer_Writefiles.Stop();
            //button3.Visible = false;
            //Terminal_Matching.instance.ButtonProcess.Text = "Wait..";
            //Terminal_Matching.instance.ButtonProcess.Visible = false;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label3.Text = "Wait..";
            progress.Value = 0;

            duration = 6;

            //timer_Writefiles.Start();
            //button3.Visible = true;
            //Terminal_Matching.instance.ButtonProcess.Text = "Process..";
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void Worker_Process()
        {
            if (!backgroundWorker1.IsBusy)
            {
                _inputparameter.Delay = 1;
                _inputparameter.Process = 30;
                backgroundWorker1.RunWorkerAsync(_inputparameter);
            }
        }
        // TEST 1
        private static string ReadFromBuffer(FileStream fStream)
        {
            Byte[] bytes = new Byte[MAX_BUFFER_SIZE];
            string output = String.Empty;
            Decoder decoder8 = enc8.GetDecoder();

            while (fStream.Position < fStream.Length)
            {
                int nBytes = fStream.Read(bytes, 0, bytes.Length);
                int nChars = decoder8.GetCharCount(bytes, 0, nBytes);

                char[] chars = new char[nChars];
                nChars = decoder8.GetChars(bytes, 0, nBytes, chars, 0);
                output += new string(chars, 0, nChars);
            }
            return output;
        }
        // END TEST 1
        private void ReturnText(string text)
        {
            richTextBox1.AppendText(text);
            comport.Write(c, 0, c.Length);

            Writedatalog.WriteLog(text);

        }
        private void SetText(string text)
        {

            richTextBox1.AppendText(text);
            comport.Write(c, 0, c.Length);

            Writedatalog.WriteLog(text);

            //if (this.richTextBox1.InvokeRequired)
            //{
            //    serialCalback scb = new serialCalback(SetText);
            //    this.Invoke(scb, new object[] { text });
            //    comport.Write(c, 0, c.Length);
            //}
            //else
            //{
            //    richTextBox1.Text = text;
            //}

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }



        ///// <summary>
        /////Turntable monitoring,
        /////Transmission result, listening thread,
        ///// </summary>
        ////Monitoring serial port thread
        //static Thread dataReceiveThread_1, dataReceiveThread_2;



        //static SerialPort serialPort = new SerialPort();

        //public static void StartSerialPortListen()
        //{
        //    //set up
        //    serialPort.PortName = "COM1";
        //    serialPort.BaudRate = 1000;
        //    serialPort.ReadTimeout = 500;
        //    serialPort.WriteTimeout = 500;
        //    serialPort.DataReceived += null;

        //    //Do not use DataReceived, write your own thread to listen
        //    //Enable thread monitoring serial port
        //    try
        //    {
        //        //Serial port data analysis method (ignore the second parameter, write it yourself according to your own analysis method, and put a company's serial port analysis here)
        //        dataReceiveThread_1 = new Thread(() => DataReceiveFunction(serialPort, xuanzhuan));
        //        if (dataReceiveThread_1 != null && dataReceiveThread_1.IsAlive)
        //        {
        //            dataReceiveThread_1.Abort();
        //        }
        //        dataReceiveThread_1.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError("Failed to open listening event! ex=" + ex.Message);
        //    }
        //}

        //#region serial port data analysis
        //static void DataReceiveFunction(SerialPort serialPort, SensorResult sensorResult)
        //{
        //    try
        //    {
        //        /** **/
        //        byte[] RxBuffer = new byte[1000];
        //        UInt16 usRxLength = 0;

        //        int bytes = 0;
        //        int flag0 = 0xFF;
        //        int flag1 = 0xAA;
        //        int index = 0;//Used to record the data order at this time
        //        while (true)
        //        {
        //            if (serialPort != null && serialPort.IsOpen)
        //            {
        //                try
        //                {
        //                    byte[] byteTemp = new byte[1000];

        //                    try
        //                    {
        //                        UInt16 usLength = 0;
        //                        try
        //                        {
        //                            usLength = (UInt16)serialPort.Read(RxBuffer, usRxLength, 700);
        //                        }
        //                        catch (Exception err)
        //                        {
        //                            ////MessageBox.Show(err.Message);
        //                            ////return;
        //                            //Debug.LogError("spSerialPort.Read!! ex=" + err.Message);
        //                        }
        //                        usRxLength += usLength;
        //                        while (usRxLength >= 11)
        //                        {
        //                            //UpdateData Update = new UpdateData(DecodeData);
        //                            RxBuffer.CopyTo(byteTemp, 0);
        //                            if (!((byteTemp[0] == 0x55) & ((byteTemp[1] & 0x50) == 0x50)))
        //                            {
        //                                for (int i = 1; i < usRxLength; i++) RxBuffer[i - 1] = RxBuffer[i];
        //                                usRxLength--;
        //                                continue;
        //                            }
        //                            if (((byteTemp[0] + byteTemp[1] + byteTemp[2] + byteTemp[3] + byteTemp[4] + byteTemp[5] + byteTemp[6] + byteTemp[7] + byteTemp[8] + byteTemp[9]) & 0xff) == byteTemp[10])
        //                            {
        //                                Thread t = new Thread(() => XuanzhuanReceiveData(byteTemp, sensorResult));
        //                                t.Start();
        //                            }
        //                            for (int i = 11; i < usRxLength; i++) RxBuffer[i - 11] = RxBuffer[i];
        //                            usRxLength -= 11;
        //                        }

        //                        Thread.Sleep(10);
        //                    }
        //                    finally
        //                    {

        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    // Debug.Log(ex.Message);
        //                }
        //            }
        //            Thread.Sleep(100);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        //log.Error("DataReceiveFunction!! ex=" + ex.Message);
        //    }
        //}

        //private static void XuanzhuanReceiveData(byte[] RxBuffer, SensorResult sensorResult)
        //{
        //    try
        //    {
        //        DecodeData(ref sensorResult, RxBuffer);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError(" ComReceiveData ex={0}" + ex.Message);
        //    }
        //}
        ///// <summary>
        /////The input data is parsed into sr
        ///// </summary>
        ///// <param name="sr"></param>
        ///// <param name="byteTemp"></param>
        //private static void DecodeData(ref SensorResult sr, byte[] byteTemp)
        //{
        //    DateTime TimeStart = DateTime.Now;
        //    short sRightPack = 0;
        //    short[] ChipTime = new short[7];
        //    double Temperature;
        //    double Pressure, Altitude, GroundVelocity, GPSYaw, GPSHeight;
        //    long Longitude, Latitude;
        //    double[] LastTime = new double[10];



        //    float[] Data = new float[4];
        //    double TimeElapse = (DateTime.Now - TimeStart).TotalMilliseconds / 1000;

        //    Data[0] = BitConverter.ToInt16(byteTemp, 2);
        //    Data[1] = BitConverter.ToInt16(byteTemp, 4);
        //    Data[2] = BitConverter.ToInt16(byteTemp, 6);
        //    Data[3] = BitConverter.ToInt16(byteTemp, 8);
        //    sRightPack++;
        //    switch (byteTemp[1])
        //    {
        //        case 0x52:
        //            //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
        //            Temperature = Data[3] / 100.0;
        //            Data[0] = Data[0] / 32768.0f * 2000;
        //            Data[1] = Data[1] / 32768.0f * 2000;
        //            Data[2] = Data[2] / 32768.0f * 2000;
        //            sr.WX = Data[0];
        //            sr.WY = Data[1];
        //            sr.WZ = Data[2];
        //            sr.WO = Data[3];
        //            if ((TimeElapse - LastTime[2]) < 0.1) return;
        //            LastTime[2] = TimeElapse;
        //            break;

        //        case 0x55:
        //            sr.D0 = Data[0];
        //            sr.D1 = Data[1];
        //            sr.D2 = Data[2];
        //            sr.D3 = Data[3];
        //            break;

        //        default:
        //            break;
        //    }



        //}

        //int deviceNum = 7;
        //bool serialEcho;
        //byte[] buffer;

        //private bool sendCommandPacket(byte[] packet, int sendBytes, int readBytes, ref string str)
        //{
        //    /* This function transmits the specified command packet and then waits to receive
        //     * the specified number of bytes in response.  Bytes to transmit are provided via the
        //     * 'packet' array, and received bytes are stored in the 'packet' array.
        //     * It blocks execution until the desired number of bytes have been received from the
        //     * the TReX or until the serial read times out (150 ms).
        //     *   packet - an array of bytes to transmit to the TReX; when the function is through,
        //     *            this array will contain any bytes received from the TReX, so the size of
        //     *            this array must be greater than or equal to Max(sendBytes, readBytes)
        //     *   sendBytes - the number of bytes in the command packet
        //     *   readBytes - the number of bytes to try to receive from the TReX in response
        //     *   str - a string that contains information about what was sent and received;
        //     *         used for debugging/feedback purposes
        //     */

        //    int i;
        //    str = "";

        //    try
        //    {
        //        // if there are any unread bytes in the read buffer, they are junk
        //        //  read them now so the buffer is clear to receive anything the TReX
        //        //  might send back in response to the command
        //        while (comport.BytesToRead > 0)
        //            comport.ReadByte();

        //        str += " TX={ ";
        //        if (expandedProtocolCheckBox.Checked)
        //        {
        //            for (i = sendBytes - 1; i >= 0; i--)
        //                packet[i + 2] = packet[i];
        //            packet[0] = 0x80;
        //            packet[1] = (byte)deviceNum;
        //            packet[2] -= 0x80;     // clear MSB of command byte
        //            sendBytes += 2;
        //        }

        //        for (i = 0; i < sendBytes; i++)
        //            str += packet[i].ToString("X2") + " ";
        //        comport.Write(packet, 0, sendBytes);
        //        if (serialEcho)
        //        {
        //            str += "};  Echo={ ";
        //            for (i = 0; i < sendBytes; i++)
        //                str += comport.ReadByte().ToString("X2") + " ";
        //        }
        //        str += "};  RX={ ";
        //        for (i = 0; i < readBytes; i++)
        //        {
        //            packet[i] = (byte)comport.ReadByte();
        //            str += packet[i].ToString("X2") + " ";
        //        }
        //        str += "}";
        //    }

        //    catch (Exception)
        //    {
        //        str += " *TIMEOUT*";
        //        return false;
        //    }

        //    return true;
        //}

        //# Port data Receive Test
        //
        //this.BeginInvoke(new EventHandler(delegate { SetTheText(data); }));
        //comport.ReadTimeout = 100;

        //comport.Write(c, 0, c.Length);
        //

        // END TEST 0
        //    //int intBuffer;
        //    //intBuffer = comport.BytesToRead;
        //    //byte[] byteBuffer = new byte[intBuffer];
        //    //comport.Read(byteBuffer, 0, intBuffer);

        //    //this.Invoke(new EventHandler(DoupDate));

        //    //textBox1.Invoke(new EventHandler(delegate
        //    //{
        //    //    textBox1.AppendText(strText);
        //    //}
        //    //));

        //    //Writedatalog.WriteLog(data);

        //    //richTextBox1.Text = data;
        //    //Writedatalog.WriteLog(data);
        //    //ReceiveData();

        //    // Display the text to the user in the terminal
        //    Log(LogMsgType.Incoming, data);
        //}
        //else
        //{
        //    // Obtain the number of bytes waiting in the port's buffer
        //    int bytes = comport.BytesToRead;

        //    // Create a byte array buffer to hold the incoming data
        //    byte[] buffer = new byte[bytes];

        //    // Read the data from the port and store it in our buffer
        //    comport.Read(buffer, 0, bytes);

        //    // Show the user the incoming data in hex format
        //    Log(LogMsgType.Incoming, ByteArrayToHexString(buffer));
        //}

        //InputData = comport.ReadLine();

        //Writedatalog.WriteLog(InputData);

        ////if (InputData != String.Empty)
        ////{
        ////this.BeginInvoke(new SetTextCallback(SetText), new object[] { InputData });
        //MessageBox.Show(InputData.Trim());
        //this.BeginInvoke(new EventHandler(delegate { SetText(InputData); }));

        ////    SetText(InputData);
        ////}

        //Log(LogMsgType.Incoming, InputData);

        //else
        //{
        //    comport.Write(c, 0, c.Length);
        //}


        //Reply_Status = (int)REPLY.YES_REPLY;

        // TEST 1 working but wrong writelog

        //byte[] buffer = new byte[24];
        //int bytes = 0;
        //while (true)
        //{
        //    if (comport != null && comport.IsOpen)
        //    {
        //        try
        //        {
        //            bytes = comport.Read(buffer, 0, buffer.Length);
        //            if (bytes == 0)
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                string strbytes = Encoding.ASCII.GetString(buffer);

        //                //Log(LogMsgType.Incoming, strbytes);
        //                //richTextBox1.AppendText(strbytes);

        //                Writedatalog.WriteLog(strbytes);
        //                comport.Write(c, 0, c.Length);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //if (ex.GetType() != typeof(ThreadAbortException))
        //            //{

        //            //}
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        break;
        //    }
        //    Thread.Sleep(10);
        //}

        // END TEST 1

        // TEST 2
        //try
        //{
        //    /** **/
        //    byte[] RxBuffer = new byte[1000];
        //    UInt16 usRxLength = 0;

        //    int bytes = 0;
        //    int flag0 = 0xFF;
        //    int flag1 = 0xAA;
        //    int index = 0;//Used to record the data order at this time
        //    while (true)
        //    {
        //        if (comport != null && comport.IsOpen)
        //        {
        //            try
        //            {
        //                byte[] byteTemp = new byte[1000];

        //                try
        //                {
        //                    UInt16 usLength = 0;
        //                    try
        //                    {
        //                        usLength = (UInt16)comport.Read(RxBuffer, usRxLength, 700);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show(ex.Message);
        //                        ////return;
        //                        //Debug.LogError("spSerialPort.Read!! ex=" + err.Message);
        //                    }
        //                    usRxLength += usLength;
        //                    while (usRxLength >= 11)
        //                    {
        //                        //UpdateData Update = new UpdateData(DecodeData);
        //                        RxBuffer.CopyTo(byteTemp, 0);
        //                        if (!((byteTemp[0] == 0x55) & ((byteTemp[1] & 0x50) == 0x50)))
        //                        {
        //                            for (int i = 1; i < usRxLength; i++) RxBuffer[i - 1] = RxBuffer[i];
        //                            usRxLength--;
        //                            continue;
        //                        }
        //                        //if (((byteTemp[0] + byteTemp[1] + byteTemp[2] + byteTemp[3] + byteTemp[4] + byteTemp[5] + byteTemp[6] + byteTemp[7] + byteTemp[8] + byteTemp[9]) & 0xff) == byteTemp[10])
        //                        //{
        //                        //    Thread t = new Thread(() => XuanzhuanReceiveData(byteTemp, sensorResult));
        //                        //    t.Start();
        //                        //}
        //                        for (int i = 11; i < usRxLength; i++) RxBuffer[i - 11] = RxBuffer[i];
        //                        usRxLength -= 11;
        //                    }

        //                    Thread.Sleep(10);
        //                }
        //                finally
        //                {

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }

        //        Thread.Sleep(100);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}

        // END TEST 2

        // TEST 3
        //FileStream fStream = new FileStream(@".\Utf8Example.txt", FileMode.Open);
        //string contents = null;

        //if (fStream.Length <= MAX_BUFFER_SIZE)
        //{
        //    Byte[] bytes = new byte[fStream.Length];
        //    fStream.Read(bytes, 0, bytes.Length);
        //    contents = enc8.GetString(bytes);
        //}
        //else
        //{
        //    contents = ReadFromBuffer(fStream);
        //}
        //fStream.Close();
        //Writedatalog.WriteLog(contents);



        // END TEST 3
        // TEST 4
        //byte[] buffer = new byte[1024];
        //int bytes = 0;
        //int nBytes = 0;
        //int nChars = 0;

        //string output = String.Empty;
        //Decoder decoder8 = enc8.GetDecoder();

        //while (true)
        //{
        //    if (comport != null && comport.IsOpen)
        //    {
        //        try
        //        {
        //            bytes = comport.Read(buffer, 0, buffer.Length);

        //            if (bytes == 0)
        //            {
        //                continue;
        //            }
        //            else 
        //            {
        //                string strbytes = Encoding.Default.GetString(buffer);

        //                var s = strbytes;
        //                var ascii = Encoding.ASCII.GetBytes(s);
        //                byte[] ascii2 = Encoding.ASCII.GetBytes(s);
        //                //for (int i = 0; i < s.Length; i++)
        //                //{
        //                    string Str_Ack = Convert.ToString(ascii2[0]);

        //                //\x05 = ENQ
        //                if (Str_Ack == "5")
        //                {
        //                    //MessageBox.Show("ENQ" + strbytes);
        //                    //Writedatalog.WriteLog(Str_Ack);

        //                    comport.Write(c, 0, c.Length);
        //                }
        //                else if (Str_Ack == "2")
        //                {
        //                    //Writedatalog.WriteLog(Str_Ack);

        //                    //MessageBox.Show(strbytes);

        //                    InputData = comport.ReadExisting();
        //                    this.BeginInvoke(new EventHandler(delegate { ReturnText(InputData); }));

        //                    richTextBox1.AppendText(InputData);
        //                    Log(LogMsgType.Incoming, InputData);
        //                    //MessageBox.Show(InputData);
        //                    //Writedatalog.WriteLog(InputData);

        //                    comport.Write(c, 0, c.Length);
        //                    //}
        //                    //}
        //                    //comport.Write(c, 0, c.Length);
        //                }
        //                else if (Str_Ack == "4")
        //                {
        //                    Writedatalog.WriteLog("EOT");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex.GetType() != typeof(ThreadAbortException))
        //            {

        //            }
        //        }
        //    }
        //    else
        //    {
        //        break;
        //    }
        //    Thread.Sleep(10);
        //}

        // END TEST 4


        // TEST 5
        //Byte[] recvBytes = new Byte[RECV_DATA_MAX - 1 + 1];
        //int recvSize;

        //while (true)
        //{
        //    try
        //    {
        //        recvSize = readDataSub(recvBytes, this.comport);
        //    }
        //    catch (IOException ex)
        //    {
        //        //MessageBox.Show(serialPort1.PortName + "\r\n" + ex.Message);    // disappeared
        //        MessageBox.Show(ex.Message);

        //        break;
        //    }
        //    // 
        //    // Show the receive data after converting the receive data to Shift-JIS.
        //    // Terminating null to handle as string.
        //    // 
        //    recvBytes[recvSize] = 0;

        //    SN = Encoding.GetEncoding("Shift_JIS").GetString(recvBytes).ToString().Replace("\r", "").Replace("\0", "");

        //    if (!(SN.Contains("ERROR") | SN.Contains("ER,lon")))

        //        break;
        //}

        // END TEST 5

        // TEST 6

        //byte[] buffer = new byte[24];
        //int bytes = 0;
        //comport.NewLine = "\r";

        //while (true)
        //{
        //    if (comport != null && comport.IsOpen)
        //    {
        //        try
        //        {
        //            bytes = comport.Read(buffer, 0, buffer.Length);
        //            if (bytes == 0)
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                string strbytes = Encoding.ASCII.GetString(buffer);

        //                //Log(LogMsgType.Incoming, strbytes);
        //                //richTextBox1.AppendText(strbytes);

        //                Writedatalog.WriteLog(strbytes);
        //                comport.Write(c, 0, c.Length);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        break;
        //    }
        //    Thread.Sleep(10);
        //}

        // END TEST6 
    }
}