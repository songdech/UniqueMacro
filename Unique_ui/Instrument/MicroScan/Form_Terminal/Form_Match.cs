using System;
using System.Data;
using System.Windows.Forms;
using UNIQUE.Instrument.MicroScan.Controller;
using UNIQUE.Instrument.MicroScan.Entites;

namespace UNIQUE.Instrument.MicroScan.Form_Terminal
{
    public partial class Form_Match : Form
    {
        private MicroscanController objConfig = new MicroscanController();
        private Terminal_GETM objTerminalM;

        string Str_PATNUM = "";

        public Form_Match(Terminal_GETM objTerminalM)
        {
            InitializeComponent();

            txtUnmatch.Text = objTerminalM.UNMATCHNUM;
            this.Str_PATNUM = objTerminalM.PATNUM;

        }

        private void Form_Match_Load(object sender, System.EventArgs e)
        {
            try
            {
                txtProtocol.Select();
                objTerminalM = new Terminal_GETM();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // STEP 1 GET SUBREQUESTID
        private string GetSUBREQUESTID(string PROTOCOLNUM)
        {
            string Result = "";

            MicroscanController objConfig = new MicroscanController();
            DataTable dt = null;

            try
            {
                objTerminalM.SUBREQUESTID = PROTOCOLNUM;

                dt = objConfig.GetSUBREQUESTID(objTerminalM);

                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0]["SUBREQUESTID"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string SUBREQUESTID = "";

                var MicroscanINI = new INIClass("Microscan.ini");

                if (txtProtocol.Text != "")
                {
                    // GET SUBREQUESTID
                    // STEP 1
                    SUBREQUESTID = GetSUBREQUESTID(txtProtocol.Text);

                    // STEP 2 READ ORGANISM IN ROW 

                    //var Antibiotic = MicroscanINI.Read("aaa", "ANTIBIOTIC");

                    //if (dataGridView2.Rows.Count > 0)
                    //{
                    //    foreach (DataGridViewRow row in dataGridView2.Rows)
                    //    {
                    //        objBatteryM.MAP_SYSTEMCODE = row.Cells[0].Value.ToString();
                    //        objBatteryM.MAP_OTHERCODE = row.Cells[1].Value.ToString();
                    //        objBatteryM.MAP_FULLNAME = row.Cells[2].Value.ToString();
                    //        objBatteryM = objConfig.Save_MAPCODE_BATTERY(objBatteryM);
                    //    }

                        var Organism = MicroscanINI.Read("eee", "ORGANISM");



                    objTerminalM.UNMATCHNUM = txtUnmatch.Text;
                    objTerminalM.PROTOCOLNUM = txtProtocol.Text;
                    objTerminalM.PATNUM = Str_PATNUM;

                    objTerminalM.LogUserID = ControlParameter.loginID.ToString();

                    







                    objTerminalM = objConfig.MatchProtocol(objTerminalM);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
