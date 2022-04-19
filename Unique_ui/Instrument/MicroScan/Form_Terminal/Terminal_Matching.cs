using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UNIQUE.ConfigurationFld;
using UNIQUE.Instrument.MicroScan;
using UNIQUE.Instrument.MicroScan.Controller;
using UNIQUE.Instrument.MicroScan.Entites;
using UNIQUE.Instrument.MicroScan.Form_Terminal;

namespace UNIQUE
{
    public partial class Terminal_Matching : Form
    {
        private MicroscanController objConfig = new MicroscanController();
        private Terminal_GETM objTerminalGETM;
        private Terminal_SETM objTerminalSETM;

        object COLONIES_1;
        object COLONIES_2;
        object COLONIES_3;

        SqlConnection conn;

        private string Str_PROTOCOLNUM_COMPLETE = "";
        private string Str_PATNUM_COMPLETE = "";
        private string Str_MultiColonies_ID = "";
        private string Str_MultiColonies_NUMBER = "";

        private string Str_UNMATCHNUM = "";
        private string Str_PATNUM = "";
        private string Str_TUBEID = "";

        private bool Bool_updateUnmatch = false;
        private bool Bool_Match = false;

        private const Byte STX = 0x02;
        private const Byte ETX = 0x03;

        // Fix path
        string Pathwst = @".\WST\";
        string Pathwst_backup = @".\WST\Backup\";
        string PathError = @".\WST\Error\";
        string PathProcess = @".\WST\Process\";
        string PathProcessed = @".\WST\Processed\";

        private const int CP_NOCLOSE_BUTTON = 0x200;


        public static Terminal_Matching instance;
        public Button ButtonProcess;

        public Terminal_Matching()
        {
            InitializeComponent();

            instance = this;
            ButtonProcess = button3;
        }
        private void Terminal_Matching_Load(object sender, EventArgs e)
        {
            try
            {
                Check_directory();

                objTerminalGETM = new Terminal_GETM();
                objTerminalSETM = new Terminal_SETM();

                //objTerminalGETM.StartWrite_Bool = true;

                dataGridView_Rescontrol.Rows.Clear();
                Loaddata_DatagridRescontrol();

                if (tabControl1.SelectedIndex == 0)
                {
                    Clear_text();
                    cjButton1.Visible = false;
                    cjPending.Visible = true;
                    lblPending.Visible = true;
                    lblComplete.Visible = true;
                    cjComplete.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        // Read per files wst***.txt
        // Step 1 
        // Read and Save to database MICROSCANDB.mdf
        private void Pickup_files(string Path_Source, string Path_error)
        {
            string file_extension = "*.txt";

            string Messages_H_0 = "";

            string Messages_P_0 = "";       //
            string Messages_P_2 = "";       //Patient ID Number
            string Messages_P_3 = "";       //Patient Last Name
            string Messages_P_4 = "";       //Patient First Name
            string Messages_P_5 = "";       //Patient DOB

            string Messages_B_0 = "";       //
            string Messages_B_2 = "";       //Specimen Number
            string Messages_B_7 = "";       //Sample
            string Messages_B_9 = "";       //Date Specimen Collected
            //string Messages_B_10 = "";      //Time Specimen collected
            //string Messages_B_12 = "";      //Receive Date

            string Messages_R_0 = "";       //
            string Messages_R_1 = "";
            string Messages_R_2 = "";       // ISO Number
            string Messages_R_3 = "";       // Specimen name
            string Messages_R_4 = "";       // Group name
            string Messages_R_5 = "";       // Group Description
            string Messages_R_6 = "";       // Date 
            string Messages_R_7 = "";
            string Messages_R_8 = "";
            string Messages_R_11 = "";      // Organism code
            string Messages_R_12 = "";      // Organism name

            string Messages_M_0 = "";
            string Messages_M_1 = "";
            string Messages_M_2 = "";
            string Messages_M_3 = "";
            string Messages_M_4 = "";
            string Messages_M_5 = "";
            string Messages_M_6 = "";
            string Messages_M_7 = "";
            string Messages_M_8 = "";

            string Messages_L_0 = "";
            string Messages_L_1 = "";
            string Messages_L_2 = "";

            string MSG_H = "";
            string MSG_P = "";
            string MSG_B = "";
            string MSG_R = "";
            string MSG_M = "";
            string MSG_L = "";


            //string str_OBX_ASTM = "";                   // OBX ASTM
            //string str_LINE_CLOSE = "";
            //string IDCounter;
            //string strSysDate = DateTime.Now.ToString("yyyyMMddHHmmss");

            // Check Messages type Delete or not
            //
            //bool bool_MSG_TYPE = true;
            //Boolean Bool_ORMG = false;

            try
            {
                string[] HaveFile = Directory.GetFiles(@"" + Path_Source, file_extension);

                for (int i = 0; i < HaveFile.Length; i++)
                {
                    //string IDCounterF6 = Settings1.Default.CounterF6;

                    //int IDcount = Convert.ToInt32(IDCounterF6) + 1;
                    //string LBLvalue = string.Format("{0:D8}", IDcount);
                    //Settings1.Default.CounterF6 = LBLvalue;
                    //Settings1.Default.Save();
                    //IDCounter = Settings1.Default.CounterF6;

                    string path_hed = HaveFile[i].ToString();
                    // Detail path file from Path *.res
                    string[] split_path_hed = path_hed.Split(new string[] { "get_filename" }, StringSplitOptions.None);
                    string path_res = @"" + split_path_hed[0].Trim();
                    // ext = get_filename
                    string extension_res = Path.GetExtension(path_res);
                    // filename
                    string filenameNoExtension_res = Path.GetFileNameWithoutExtension(path_res);
                    // filesname+get_filename
                    string filename_res = Path.GetFileName(path_res);
                    //string filename_res = Path.GetFileName(HaveFile[i]);
                    string filesSource = @"" + Path_Source + filename_res;

                    FileInfo fi = new FileInfo(filesSource);

                    // Detail files
                    string OEM_filesname_1 = HaveFile[i].ToString();
                    string filename_oem = Path.GetFileName(OEM_filesname_1);
                    string filenameNoExtension_1 = Path.GetFileNameWithoutExtension(OEM_filesname_1);


                    string Prefix = filenameNoExtension_res.Substring(0, 3);

                    StringBuilder BinaryFiles = new StringBuilder();
                    BinaryFiles.Clear();

                    if (!File.Exists(Path_Source))
                    {
                        try
                        {
                            // If พบ files ศูนย์ ให้แจ้งรายงาน Log Error
                            if (fi.Length == 0)
                            {
                                System.IO.File.Move(HaveFile[i], Path_error);
                            }

                            // If ไม่พบ Files ศูนย์ ให้ทำงาน
                            else if (fi.Length > 0)
                            {
                                string strDateTime = string.Format("{0:MM/dd/yyyy HH:mm:ss}", DateTime.Now);
                                if (Directory.Exists(Path_Source))
                                {
                                    Encoding encoding_tis620 = Encoding.GetEncoding("tis-620");

                                    string line_wstfiles;
                                    System.IO.StreamReader file_wst = new System.IO.StreamReader(OEM_filesname_1, encoding_tis620);

                                    while ((line_wstfiles = file_wst.ReadLine()) != null)
                                    {
                                        try
                                        {
                                            if (line_wstfiles.Length > 0)
                                            {

                                                StringReader strReader = new StringReader(line_wstfiles);
                                                string String_read_line = strReader.ReadToEnd();
                                                string SeqMen_ALL = String_read_line.Substring(2, 2);   //  Get Messages 2 Digit for switch case,

                                                switch (SeqMen_ALL)
                                                {
                                                    case "\"H":     // Header Record (H)
                                                        string[] Messages_H = String_read_line.Split(',');
                                                        Messages_H_0 = Messages_H[0].ToString().Trim('"');
                                                        char Header_H0 = Messages_H_0[Messages_H_0.Length - 1];

                                                        MSG_H = @"H|" + Header_H0 + "|";

                                                        BinaryFiles.AppendLine(MSG_H);

                                                        break;
                                                    case "P,":      // Patient record (P)
                                                        string[] Messages_P = String_read_line.Split(',');
                                                        Messages_P_0 = Messages_P[0].ToString().Trim();
                                                        Messages_P_2 = Messages_P[2].ToString().Trim('"');      // PATNUM
                                                        Messages_P_3 = Messages_P[3].ToString().Trim('"');      // NAME
                                                        Messages_P_4 = Messages_P[4].ToString().Trim('"');      // LASTNAME

                                                        char Header_P0 = Messages_P_0[Messages_P_0.Length - 1];

                                                        MSG_P = @"" + Header_P0 + "|" + Messages_P_2 + "|" + Messages_P_3 + "|" + Messages_P_4 + "|";

                                                        BinaryFiles.AppendLine(MSG_P);

                                                        break;
                                                    case "B,":      // Specimen record (B)
                                                        string[] Messages_B = String_read_line.Split(',');
                                                        Messages_B_0 = Messages_B[0].ToString().Trim();
                                                        char Header_B0 = Messages_B_0[Messages_B_0.Length - 1];

                                                        Messages_B_2 = Messages_B[2].ToString().Trim('"');         // Specimen
                                                        Messages_B_7 = Messages_B[7].ToString().Trim('"');         // Sample
                                                        Messages_B_9 = Messages_B[9].ToString().Trim('"');         //Date Specimen Collected

                                                        MSG_B = @"" + Header_B0 + "|" + Messages_B_2 + "|" + Messages_B_7 + "|" + Messages_B_9 + "|";

                                                        BinaryFiles.AppendLine(MSG_B);

                                                        break;
                                                    case "R,":      // Isolate record (R)
                                                        string[] Messages_R = String_read_line.Split(',');
                                                        Messages_R_0 = Messages_R[0].ToString().Trim();
                                                        char Header_R0 = Messages_R_0[Messages_R_0.Length - 1];

                                                        Messages_R_2 = Messages_R[2].ToString().Trim('"');         // ISO Number
                                                        Messages_R_3 = Messages_R[3].ToString().Trim('"');         // Specimen
                                                        Messages_R_4 = Messages_R[4].ToString().Trim('"');         // Group test
                                                        Messages_R_5 = Messages_R[5].ToString().Trim('"');         // Group test description
                                                        Messages_R_6 = Messages_R[6].ToString().Trim('"');         // Date time
                                                        Messages_R_11 = Messages_R[11].ToString().Trim('"');       // Organism number
                                                        Messages_R_12 = Messages_R[12].ToString().Trim('"');       // Orgranism name

                                                        MSG_R = @"" + Header_R0 + "|" + Messages_R_2 + "|" + Messages_R_3 + "|" + Messages_R_4 + "|" + Messages_R_5 + "|" + Messages_R_6 + "|" + Messages_R_11 + "|" + Messages_R_12 + "|";

                                                        BinaryFiles.AppendLine(MSG_R);

                                                        break;
                                                    case "M,":      // Test/MIC record (M)
                                                        string[] Messages_M = String_read_line.Split(',');
                                                        Messages_M_0 = Messages_M[0].ToString().Trim();
                                                        char Header_M0 = Messages_M_0[Messages_M_0.Length - 1];

                                                        Messages_M_1 = Messages_M[1].ToString().Trim('"');      // Number Anitibiotic
                                                        Messages_M_2 = Messages_M[2].ToString().Trim('"');      // Antibiotic code
                                                        Messages_M_3 = Messages_M[3].ToString().Trim('"');      // Antibiotic name
                                                        Messages_M_4 = Messages_M[4].ToString().Trim('"');      // Antibiotic result
                                                        Messages_M_5 = Messages_M[5].ToString().Trim('"');      // 
                                                        Messages_M_7 = Messages_M[7].ToString().Trim('"');      // CLSI
                                                        Messages_M_8 = Messages_M[8].ToString().Trim('"');      // CLSI

                                                        MSG_M = @"" + Header_M0 + "|" + Messages_M_1 + "|" + Messages_M_2 + "|" + Messages_M_3 + "|" + Messages_M_4 + "|" + Messages_M_5 + "|" + Messages_M_6 + "|" + Messages_M_7 + "|" + Messages_M_8 + "|";

                                                        BinaryFiles.AppendLine(MSG_M);

                                                        break;
                                                    case "Q,":      // Trace record (Q)
                                                        break;
                                                    case "C,":      // Comment record (C)
                                                        break;
                                                    case "F,":      // Free text record (F)
                                                        break;
                                                    case "A,":      // Request record (A)
                                                        break;
                                                    case "L,":      // End Record (L)
                                                        string[] Messages_L = String_read_line.Split(',');
                                                        Messages_L_0 = Messages_L[0].ToString().Trim();
                                                        char Header_L0 = Messages_L_0[Messages_L_0.Length - 1];

                                                        Messages_L_1 = Messages_L[1].ToString().Trim('"');
                                                        Messages_L_2 = Messages_L[2].ToString().Trim('"');

                                                        MSG_L = @"" + Header_L0 + "|" + Messages_L_1 + "|" + Messages_L_2 + "|";

                                                        BinaryFiles.AppendLine(MSG_L);

                                                        break;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }
                                    }
                                    // SAVE DETAIL TO TMP DATABASE
                                    // Close Read files in StremReader

                                    file_wst.Close();

                                    // Check files MATCH with Pending
                                    // Messages_P_2 = PATNUM
                                    // Messages_B_2 or Messages_R_3= SPECIMEN /PROTOCOL-COLONYID

                                    // Read Pending for Auto Matching
                                    // Read by Specimen number [Messages_R_3], Patnumber [Messages_P_2]
                                    // Matching_Pending(Messages_R_3, Messages_P_2);
                                    //
                                    // Start Match Automatic
                                    //Bool_Match = CHECK_MATCH_WITH_PENDING(Messages_P_2, Messages_B_2);

                                    // Auto Step 1 Get sentence in SPECIMEN 
                                    string[] Sub_sentences = { Messages_B_2 };
                                    string sPattern = "-";

                                    foreach (string sub in Sub_sentences)
                                    {
                                        if (System.Text.RegularExpressions.Regex.IsMatch(sub, sPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                                        {
                                            string[] One_sentences = sub.Split('-');

                                            string Protocol_1 = One_sentences[0].ToString().Trim();
                                            string Colony_1 = One_sentences[1].ToString().Trim();
                                            string Str_TUBEID = "";

                                            //MessageBox.Show($"  (match for '{Colony_1 + Protocol_1}' found)");
                                            // Auto Step 2 Check PATIENTS and PROTOCOL IN Request create
                                            // Search Files Match with Protocol and Patnumber
                                            // Protocol_1   = PROTOCOL in files
                                            // 
                                            // Messages_P_2 = PATNUM in files
                                            //
                                            // Prepare with Request and files by bool
                                            //

                                            bool bool_Auto_Match = GET_FILES_AND_REQUESTS(Protocol_1, Messages_P_2);
                                            bool bool_Update = false;

                                            if (bool_Auto_Match)
                                            {
                                                // Auto Step 3 Save TUBE
                                                byte[] buffer = Encoding.Default.GetBytes(BinaryFiles.ToString());

                                                objTerminalGETM = new Terminal_GETM();

                                                // Check Data in MicroscanDB 
                                                // PROTOCOLNUM-COLONY and PATNUM
                                                // Messages_B_2 = PROTOCOLNUM-COLONY
                                                // Messages_P_2 = PATNUMBER
                                               
                                                string [] PROTOCOL_EXITS = GET_PROTOCOL_EXITS(Messages_B_2, Messages_P_2);
                                                // [0] = TUBEID

                                                objTerminalGETM.SPMNUMBER = Messages_B_2;  // Specimen number / Protocol number
                                                objTerminalGETM.PATNUM = Messages_P_2;     // PATNUM
                                                objTerminalGETM.NAME = Messages_P_3;       // Patient name
                                                objTerminalGETM.LASTNAME = Messages_P_4;   // Patient lastname
                                                objTerminalGETM.TUBESTATUS = "0";          // Status result
                                                objTerminalGETM.TUBE_RESULT = buffer;      // All result

                                                if (PROTOCOL_EXITS[0] != "" )
                                                {
                                                    // Insert
                                                    objTerminalGETM = objConfig.SaveTUBE(objTerminalGETM);
                                                    // Get TUBEID after save TUBE
                                                    Str_TUBEID = objTerminalGETM.TUBEID;
                                                }
                                                else
                                                {
                                                    // Update
                                                    Str_TUBEID = PROTOCOL_EXITS[0];
                                                    objTerminalGETM = objConfig.UPDATETUBE(objTerminalGETM);
                                                    bool_Update = true;
                                                }
                                                // END Save TUBE

                                                // Auto Step 4 Search Details with Protocol number
                                                string[] Array_BATTERIES = GetSUBREQUESTID_MultiColony(Protocol_1, Colony_1);

                                                // Array_BATTERIES [ ]
                                                // [0] = SUBREQUESTID
                                                // [1] = ACCESSNUMBER
                                                // [2] = COLONYID
                                                // [3] = BATTERYID
                                                // [4] = ISOLATNUMBER
                                                // [5] = PATNUMBER
                                                // [6] = COLONYNUMBER
                                                Matching_Automatic(Protocol_1, Array_BATTERIES[0], Array_BATTERIES[2], Array_BATTERIES[3], Array_BATTERIES[5], Str_TUBEID);


                                                if (Bool_Match && bool_Update == false)
                                                {
                                                    // Update TUBE .TUBESTATUS
                                                    objTerminalSETM.TUBEID = Str_TUBEID;
                                                    objTerminalSETM.TUBESTATUS = "1";          // Status result
                                                    objTerminalSETM.SUBREQUESTID = Array_BATTERIES[0];
                                                    objTerminalSETM.COLONYID = Array_BATTERIES[2];
                                                    objTerminalSETM.COLONYNUMBER = Array_BATTERIES[6];


                                                    objTerminalSETM = objConfig.Update_Status_TUBE(objTerminalSETM);

                                                    // Update Status in JOBS

                                                    // Load JOBS ค้นหา COLONYID and ๋JOBID
                                                    // ค้นหาโดย SUBREQUESTID, COLONYID
                                                    string Str_JOBID = GET_JOBID(Array_BATTERIES[0], Array_BATTERIES[2]);

                                                    // Update ที่ JOBS 
                                                    objTerminalSETM.JOBID = Str_JOBID;
                                                    objTerminalSETM.LogUserID = "WST";
                                                    objTerminalSETM.JOBSTATUS = "1";

                                                    objTerminalSETM = objConfig.Update_Status_JOBS(objTerminalSETM);

                                                    //MessageBox.Show("Auto complete");
                                                }
                                                else if (Bool_Match && bool_Update)
                                                {
                                                    // Update if PROTOCOL-COLONY exist
                                                    // Update TUBE .TUBESTATUS
                                                    //
                                                    objTerminalSETM.TUBEID = PROTOCOL_EXITS[0];
                                                    objTerminalSETM.TUBESTATUS = "1";          // Status result
                                                    objTerminalSETM.SUBREQUESTID = Array_BATTERIES[0];
                                                    objTerminalSETM.COLONYID = Array_BATTERIES[2];
                                                    objTerminalSETM.COLONYNUMBER = Array_BATTERIES[6];


                                                    objTerminalSETM = objConfig.Update_Status_TUBE(objTerminalSETM);

                                                    // Update Status in JOBS

                                                    // Load JOBS ค้นหา COLONYID and ๋JOBID
                                                    // ค้นหาโดย SUBREQUESTID, COLONYID
                                                    string Str_JOBID = GET_JOBID(Array_BATTERIES[0], Array_BATTERIES[2]);

                                                    // Update ที่ JOBS 
                                                    objTerminalSETM.JOBID = Str_JOBID;
                                                    objTerminalSETM.LogUserID = "WST";
                                                    objTerminalSETM.JOBSTATUS = "1";

                                                    objTerminalSETM = objConfig.Update_Status_JOBS(objTerminalSETM);

                                                    //MessageBox.Show("Auto complete");





                                                }
                                                LoadPending_Complete();
                                                LoadPending();

                                            }
                                            else // Go UnMatch files
                                            {
                                                byte[] buffer = Encoding.Default.GetBytes(BinaryFiles.ToString());

                                                //string converted = Encoding.Default.GetString(buffer, 0, buffer.Length);
                                                //textBox1.Text = converted;

                                                objTerminalGETM = new Terminal_GETM();

                                                objTerminalGETM.SPMNUMBER = Messages_B_2;  // Specimen number / Protocol number
                                                objTerminalGETM.PATNUM = Messages_P_2;     // PATNUM
                                                objTerminalGETM.NAME = Messages_P_3;       // Patient name
                                                objTerminalGETM.LASTNAME = Messages_P_4;   // Patient lastname
                                                objTerminalGETM.TUBESTATUS = "0";          // Status result
                                                objTerminalGETM.TUBE_RESULT = buffer;      // All result

                                                objTerminalGETM = objConfig.SaveTUBE(objTerminalGETM);

                                                // Save files to DB
                                                //
                                                // ############################Save files ###############################################################################################
                                                // Extract to files
                                                // Manage Path & Name files name messages
                                                dataGridView1.Rows.Add();
                                                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = Messages_P_2;
                                                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Messages_B_2;

                                                dataGridView_Rescontrol.Rows.Clear();
                                                Loaddata_DatagridRescontrol();
                                            }
                                        }
                                        else // Go UnMatch files
                                        {
                                            byte[] buffer = Encoding.Default.GetBytes(BinaryFiles.ToString());

                                            //string converted = Encoding.Default.GetString(buffer, 0, buffer.Length);
                                            //textBox1.Text = converted;

                                            objTerminalGETM = new Terminal_GETM();

                                            objTerminalGETM.SPMNUMBER = Messages_B_2;  // Specimen number / Protocol number
                                            objTerminalGETM.PATNUM = Messages_P_2;     // PATNUM
                                            objTerminalGETM.NAME = Messages_P_3;       // Patient name
                                            objTerminalGETM.LASTNAME = Messages_P_4;   // Patient lastname
                                            objTerminalGETM.TUBESTATUS = "0";          // Status result
                                            objTerminalGETM.TUBE_RESULT = buffer;      // All result

                                            objTerminalGETM = objConfig.SaveTUBE(objTerminalGETM);

                                            // Save files to DB
                                            //
                                            // ############################Save files ###############################################################################################
                                            // Extract to files
                                            // Manage Path & Name files name messages
                                            dataGridView1.Rows.Add();
                                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = Messages_P_2;
                                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Messages_B_2;

                                            dataGridView_Rescontrol.Rows.Clear();
                                            Loaddata_DatagridRescontrol();
                                        }
                                    }

                                    // END
                                    // Move to Backup path files

                                    try
                                    {
                                        string filesdest_processed = @"" + PathProcessed + "_" + filename_res;
                                        System.IO.File.Move(HaveFile[i], filesdest_processed);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Error Step 1" + ex.Message);
                                    }
                                }
                            }
                            // End
                        }
                        catch (Exception ex) { MessageBox.Show("Error Step 2" + ex.Message); }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error Step 3" + ex.Message); }
        }

        // Step 2 Read files wst***.txt and separate files to array[] 
        private void Readfiles_Matching(string Path_Source, string Path_error)
        {
            string file_extension = "*.txt";

            string MSG_H = "";
            string MSG_P = "";
            string MSG_B = "";
            string MSG_R = "";
            string MSG_M = "";
            string MSG_L = "";

            try
            {
                string[] HaveFile = Directory.GetFiles(@"" + Path_Source, file_extension);

                for (int i = 0; i < HaveFile.Length; i++)
                {
                    string path_hed = HaveFile[i].ToString();
                    string[] split_path_hed = path_hed.Split(new string[] { "get_filename" }, StringSplitOptions.None);
                    string path_res = @"" + split_path_hed[0].Trim();
                    string extension_res = Path.GetExtension(path_res);
                    string filenameNoExtension_res = Path.GetFileNameWithoutExtension(path_res);
                    string filename_res = Path.GetFileName(path_res);
                    string filesSource = @"" + Path_Source + filename_res;
                    FileInfo fi = new FileInfo(filesSource);
                    string OEM_filesname_1 = HaveFile[i].ToString();
                    string filename_oem = Path.GetFileName(OEM_filesname_1);
                    string filenameNoExtension_1 = Path.GetFileNameWithoutExtension(OEM_filesname_1);
                    string Prefix = filenameNoExtension_res.Substring(0, 3);

                    int IntStringBuild = 10;
                    int BuildNumber = 0;

                    StringBuilder[] Rawfiles = new StringBuilder[IntStringBuild];

                    int Found_B = 0;
                    int Found_R = 0;

                    if (!File.Exists(Path_Source))
                    {
                        try
                        {
                            if (fi.Length == 0)
                            {
                                System.IO.File.Move(HaveFile[i], Path_error);
                            }
                            else if (fi.Length > 0)
                            {
                                Encoding encoding_tis620 = Encoding.GetEncoding("tis-620");

                                string line_wstfiles;
                                System.IO.StreamReader file_wst = new System.IO.StreamReader(OEM_filesname_1, encoding_tis620);

                                Rawfiles[BuildNumber] = new StringBuilder();

                                while ((line_wstfiles = file_wst.ReadLine()) != null)
                                {
                                    if (line_wstfiles.Length > 0)
                                    {
                                        StringReader strReader = new StringReader(line_wstfiles);
                                        string String_read_line = strReader.ReadToEnd();
                                        string SeqMen_ALL = String_read_line.Substring(2, 2);   //  Get Messages 2 Digit for switch case,

                                        switch (SeqMen_ALL)
                                        {
                                            case "\"H":     // Header Record (H)

                                                MSG_H = String_read_line;
                                                Rawfiles[BuildNumber].AppendLine(MSG_H);

                                                break;
                                            case "P,":      // Patient record (P)
                                                MSG_P = String_read_line;
                                                Rawfiles[BuildNumber].AppendLine(MSG_P);

                                                break;
                                            case "B,":      // Specimen record (B)

                                                Found_B++;

                                                if (Found_B > 1)
                                                {
                                                    MSG_L = @"" + "L,\"1\",N,0";
                                                    Rawfiles[BuildNumber].AppendLine(MSG_L);

                                                    System.IO.StreamWriter Extend_file = new System.IO.StreamWriter(@"" + PathProcess + filenameNoExtension_res + "_" + BuildNumber + ".txt", true, Encoding.GetEncoding("TIS-620"));
                                                    Extend_file.Write(Rawfiles[BuildNumber]);
                                                    Extend_file.Close();

                                                    BuildNumber++;

                                                    Rawfiles[BuildNumber] = new StringBuilder();
                                                    Rawfiles[BuildNumber].Clear();

                                                    MSG_B = String_read_line;
                                                    Rawfiles[BuildNumber].AppendLine(MSG_P);
                                                    Rawfiles[BuildNumber].AppendLine(MSG_B);
                                                }
                                                else
                                                {
                                                    MSG_B = String_read_line;
                                                    Rawfiles[BuildNumber].AppendLine(MSG_B);
                                                }

                                                break;
                                            case "R,":      // Isolate record (R)

                                                Found_R++;

                                                if (Found_R > 1 && Found_B > 1)
                                                {
                                                    MSG_R = String_read_line;
                                                    Rawfiles[BuildNumber].AppendLine(MSG_R);
                                                }
                                                else
                                                {
                                                    MSG_R = String_read_line;
                                                    Rawfiles[BuildNumber].AppendLine(MSG_R);
                                                }

                                                break;
                                            case "M,":      // Test/MIC record (M)

                                                if (Found_R > 1 && Found_B > 1)
                                                {
                                                    MSG_M = String_read_line;
                                                    Rawfiles[BuildNumber].AppendLine(MSG_M);
                                                }
                                                else
                                                {
                                                    MSG_M = String_read_line;
                                                    Rawfiles[BuildNumber].AppendLine(MSG_M);
                                                }

                                                break;
                                            case "Q,":      // Trace record (Q)
                                                break;
                                            case "C,":      // Comment record (C)
                                                break;
                                            case "F,":      // Free text record (F)
                                                break;
                                            case "A,":      // Request record (A)
                                                break;
                                            case "L,":      // End Record (L)

                                                if (Found_R > 1 && Found_B > 1)
                                                {
                                                    MSG_L = String_read_line;
                                                    Rawfiles[BuildNumber].AppendLine(MSG_L);

                                                    System.IO.StreamWriter Extend_file = new System.IO.StreamWriter(@"" + PathProcess + filenameNoExtension_res + "_" + BuildNumber + ".txt", true, Encoding.GetEncoding("TIS-620"));
                                                    Extend_file.Write(Rawfiles[BuildNumber]);
                                                    Extend_file.Close();

                                                }
                                                else
                                                {
                                                    MSG_L = String_read_line;
                                                    Rawfiles[BuildNumber].AppendLine(MSG_L);

                                                    System.IO.StreamWriter Extend_file = new System.IO.StreamWriter(@"" + PathProcess + filenameNoExtension_res + ".txt", true, Encoding.GetEncoding("TIS-620"));
                                                    Extend_file.Write(Rawfiles[BuildNumber]);
                                                    Extend_file.Close();
                                                }
                                                break;
                                        }
                                    }
                                }
                                // Close Read files in StremReader

                                file_wst.Close();

                                // ############################Save files ###############################################################################################

                                try
                                {
                                    if (!Directory.Exists(Pathwst_backup))
                                    {
                                        Directory.CreateDirectory(Pathwst_backup);
                                        string filesdest_bak = @"" + Pathwst_backup + "_" + filename_res;
                                        System.IO.File.Move(HaveFile[i], filesdest_bak);
                                    }
                                    else
                                    {
                                        string filesdest_bak = @"" + Pathwst_backup + "_" + filename_res;
                                        System.IO.File.Move(HaveFile[i], filesdest_bak);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error Step 1" + ex.Message);
                                }
                            }
                        }
                        catch (Exception ex) { MessageBox.Show("Error Step 2" + ex.Message); }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error Step 3" + ex.Message); }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (button3.Visible == true && button3.Text == "Process..")
                {
                    Pickup_files(PathProcess, PathError);
                    button3.BackColor = Color.GreenYellow;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Check_directory()
        {
            try
            {
                if (!Directory.Exists(Pathwst))
                {
                    Directory.CreateDirectory(Pathwst);
                }
                if (!Directory.Exists(PathError))
                {
                    Directory.CreateDirectory(PathError);
                }
                if (!Directory.Exists(PathProcess))
                {
                    Directory.CreateDirectory(PathProcess);
                }
                if (!Directory.Exists(PathProcessed))
                {
                    Directory.CreateDirectory(PathProcessed);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void Loaddata_DatagridRescontrol()
        {
            MicroscanController objConfig = new MicroscanController();
            DataTable dt = null;

            try
            {
                dt = objConfig.Get_ResultControl();

                dataGridView_Rescontrol.Rows.Clear();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView_Rescontrol.Rows.Add();
                        dataGridView_Rescontrol.Rows[i].Cells[0].Value = dt.Rows[i]["SPMNUMBER"].ToString();
                        dataGridView_Rescontrol.Rows[i].Cells[1].Value = dt.Rows[i]["PATNUM"].ToString();
                        dataGridView_Rescontrol.Rows[i].Cells[2].Value = dt.Rows[i]["NAME"].ToString();
                        dataGridView_Rescontrol.Rows[i].Cells[3].Value = dt.Rows[i]["LASTNAME"].ToString();
                        dataGridView_Rescontrol.Rows[i].Cells[4].Value = dt.Rows[i]["RECEIVEDATE"].ToString();
                        dataGridView_Rescontrol.Rows[i].Cells[5].Value = dt.Rows[i]["TUBEID"].ToString();
                    }
                }
                cjButton1.Text = dataGridView_Rescontrol.RowCount.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objTerminalGETM = null;
            }
        }
        private void Search_Result(Terminal_GETM objTerminalGETM)
        {
            MicroscanController objConfig = new MicroscanController();
            DataTable dt = null;

            try
            {
                dt = objConfig.Get_Result_TUBERESULT(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {

                    string Result_TUBERESULT = dt.Rows[0]["TUBERESULT"].ToString();
                    textBox1.Text = Result_TUBERESULT;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objTerminalGETM = null;
            }
        }
        private void timer_ExpandFiles_Tick(object sender, EventArgs e)
        {
            try
            {
                if (button3.Visible == true && button3.Text == "Process..")
                {
                    Readfiles_Matching(Pathwst, PathError);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        int rowindex = 0;
        private void dataGridView_Rescontrol_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (dataGridView_Rescontrol.Rows.Count > 0)
                    {
                        if (e.RowIndex > -1)
                        {
                            this.dataGridView_Rescontrol.Rows[e.RowIndex].Selected = true;
                            rowindex = e.RowIndex;
                            this.dataGridView_Rescontrol.CurrentCell = this.dataGridView_Rescontrol.Rows[e.RowIndex].Cells[3];

                            DataGridViewRow row = this.dataGridView_Rescontrol.Rows[e.RowIndex];

                            txtPROTOCOL.Text = row.Cells[0].Value.ToString();
                            cjPatnum.Text = row.Cells[1].Value.ToString();
                            txtNAME.Text = row.Cells[2].Value.ToString() + " " + row.Cells[3].Value.ToString();
                            txtRECEIVEDATE.Text = row.Cells[4].Value.ToString();
                            txtID.Text = row.Cells[5].Value.ToString();
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (dataGridView_Rescontrol.Rows.Count > 0)
                    {
                        if (e.RowIndex > -1)
                        {
                            this.dataGridView_Rescontrol.Rows[e.RowIndex].Selected = true;
                            rowindex = e.RowIndex;
                            this.dataGridView_Rescontrol.CurrentCell = this.dataGridView_Rescontrol.Rows[e.RowIndex].Cells[1];
                            this.contextMenuStrip1.Show(this.dataGridView_Rescontrol, e.Location);

                            DataGridViewRow row = this.dataGridView_Rescontrol.Rows[e.RowIndex];

                            // Set text in Dashboard
                            txtPROTOCOL.Text = row.Cells[0].Value.ToString();
                            cjPatnum.Text = row.Cells[1].Value.ToString();
                            txtNAME.Text = row.Cells[2].Value.ToString() + " " + row.Cells[3].Value.ToString();
                            txtRECEIVEDATE.Text = row.Cells[4].Value.ToString();
                            txtID.Text = row.Cells[5].Value.ToString();

                            // Set string for UnMatch diaglog
                            Str_UNMATCHNUM = row.Cells[0].Value.ToString();
                            Str_PATNUM = row.Cells[1].Value.ToString();
                            Str_TUBEID = row.Cells[5].Value.ToString();

                            contextMenuStrip1.Show(Cursor.Position);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView_Rescontrol_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView_Rescontrol.Rows[e.RowIndex];

                objTerminalGETM = new Terminal_GETM();

                Str_UNMATCHNUM = row.Cells[0].Value.ToString();
                Str_PATNUM = row.Cells[1].Value.ToString();
                Str_TUBEID = row.Cells[5].Value.ToString();

                dataGridView2.Rows.Clear();

                Query_TUBERESULT(Str_UNMATCHNUM, Str_PATNUM, Str_TUBEID);

                tabControl1.SelectedIndex = 2;

                Str_UNMATCHNUM = "";
                Str_PATNUM = "";
                Str_TUBEID = "";
            }
        }
        private void Query_TUBERESULT(string Str_SPMNUMBER, string Str_PATNUM, string Str_TUBEID)
        {
            string sql = "";
            try
            {
                conn = new CSMicroscanDAO().Connect();

                sql = @"SELECT PATNUM,TUBERESULT FROM TUBE WHERE SPMNUMBER = '" + Str_SPMNUMBER + "' AND PATNUM = '" + Str_PATNUM + "' AND TUBEID = '" + Str_TUBEID + "' ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    byte[] bytes = (byte[])reader["TUBERESULT"];

                    if (bytes.Length > 0)
                    {
                        string converted = Encoding.Default.GetString(bytes, 0, bytes.Length);
                        string line_wstfiles;
                        StringReader strReader = new StringReader(converted.ToString());

                        while ((line_wstfiles = strReader.ReadLine()) != null)
                        {
                            if (line_wstfiles.Length > 0)
                            {
                                string SeqMen_ALL = line_wstfiles.Substring(0, 1);   //  Get Messages 1 Digit for switch case,

                                switch (SeqMen_ALL)
                                {
                                    case "H":     // Header Record (H)
                                        break;
                                    case "P":      // Patient record (P)
                                        break;
                                    case "B":      // Specimen record (B)
                                        break;
                                    case "R":      // Isolate record (R)
                                        string[] Messages_R = line_wstfiles.Split('|');

                                        dataGridView2.Rows.Add();
                                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = SeqMen_ALL;
                                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = Messages_R[2].ToString().Trim();    //Specimen number /protocol
                                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[5].Value = Messages_R[6].ToString().Trim();    //Organism code / number
                                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[6].Value = Messages_R[7].ToString().Trim();    //Organism name

                                        break;
                                    case "M":      // Test/MIC record (M)
                                        string[] Messages_M = line_wstfiles.Split('|');

                                        dataGridView2.Rows.Add();
                                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = Messages_M[1].ToString().Trim();    //Antibiotic line
                                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = Messages_M[2].ToString().Trim();    //Antibiotic code
                                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = Messages_M[3].ToString().Trim();    //Antibiotic result
                                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[3].Value = Messages_M[4].ToString().Trim();    //
                                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[7].Value = Messages_M[7].ToString().Trim();    //CLSI SYS
                                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[8].Value = Messages_M[8].ToString().Trim();    //CLSI Urine

                                        break;
                                    case "Q":      // Trace record (Q)
                                        break;
                                    case "C":      // Comment record (C)
                                        break;
                                    case "F":      // Free text record (F)
                                        break;
                                    case "A":      // Request record (A)
                                        break;
                                    case "L":      // End Record (L)
                                        break;
                                }
                                //textBox1.AppendText(line_wstfiles);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadPending()
        {
            MicroscanController objConfig = new MicroscanController();
            DataTable dt = null;

            string ColonyID = "";
            try
            {

                dt = objConfig.Get_PendingRequests();

                if (dt.Rows.Count > 0)
                {
                    dataGridView3.Rows.Clear();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView3.Rows.Add();
                        dataGridView3.Rows[i].Cells[0].Value = dt.Rows[i]["MBREQNUMBER"].ToString();
                        dataGridView3.Rows[i].Cells[1].Value = dt.Rows[i]["PATNUMBER"].ToString();
                        dataGridView3.Rows[i].Cells[2].Value = dt.Rows[i]["NAME"].ToString();
                        dataGridView3.Rows[i].Cells[3].Value = dt.Rows[i]["LASTNAME"].ToString();

                        string[] Messages = dt.Rows[i]["JOBNAME"].ToString().Split(':');
                        ColonyID = Messages[1].ToString().Trim();

                        string GetColony_JOBS = GET_COLONY_JOBS(ColonyID);

                        dataGridView3.Rows[i].Cells[4].Value = GetColony_JOBS;
                        dataGridView3.Rows[i].Cells[5].Value = ColonyID;
                    }
                    cjPending.Text = dataGridView3.RowCount.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void LoadPending_Complete()
        {
            MicroscanController objConfig = new MicroscanController();
            DataTable dt = null;

            try
            {

                dt = objConfig.Get_CompleteRequests();

                if (dt.Rows.Count > 0)
                {
                    dataGridView_Complete.Rows.Clear();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView_Complete.Rows.Add();
                        dataGridView_Complete.Rows[i].Cells[0].Value = dt.Rows[i]["SPMNUMBER"].ToString();
                        dataGridView_Complete.Rows[i].Cells[1].Value = dt.Rows[i]["PATNUM"].ToString();
                        dataGridView_Complete.Rows[i].Cells[4].Value = dt.Rows[i]["COLONYNUMBER"].ToString();
                        dataGridView_Complete.Rows[i].Cells[5].Value = dt.Rows[i]["TUBEID"].ToString();
                        dataGridView_Complete.Rows[i].Cells[6].Value = dt.Rows[i]["COLONYID"].ToString();

                    }
                    cjComplete.Text = dataGridView_Complete.RowCount.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void timerPending_Tick(object sender, EventArgs e)
        {
            Worker_Process();
        }
        struct DataParameter
        {
            public int Process;
            public int Delay;
        }
        private DataParameter _inputparameter;
        private static TextBox text_InputProtocol;
        private static TextBox text_Unmatchnum;

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
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
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
            label3.Text = string.Format("Process..{0}%", e.ProgressPercentage);
            progress.Update();
            //timerPending.Start();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            label3.Text = "Wait..";
            progress.Value = 0;
            //timerPending.Stop();
            LoadPending();
            LoadPending_Complete();

        }
        private void Worker_Process()
        {
            if (!backgroundWorker1.IsBusy)
            {
                _inputparameter.Delay = 1;
                _inputparameter.Process = 1000;
                backgroundWorker1.RunWorkerAsync(_inputparameter);
            }
        }
        private void MatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Rows.Clear();

                Query_TUBERESULT(Str_UNMATCHNUM, Str_PATNUM, Str_TUBEID);
                tabControl1.SelectedIndex = 2;

                if (InputBoxClose("Protocol UnMatch", Str_UNMATCHNUM) == DialogResult.OK)
                {

                    string input_protocol = text_InputProtocol.Text;
                    Str_TUBEID = txtID.Text;

                    try
                    {
                        if (!input_protocol.Equals(""))
                        {
                            // Step Mathing 1
                            Matching_(input_protocol);
                            // Step Change Specimen after Matching
                            string[] Array_GETSUBREQUESTID = GetSUBREQUESTID(input_protocol);
                            // Array_GETSUBREQUESTID
                            // [2]  COLONYID
                            // [5]  PATNUMBER
                            // [6]  COLONYNUMBER


                            if (Bool_updateUnmatch)
                            {
                                Update_After_UnMatching(input_protocol, Array_GETSUBREQUESTID[5], Str_TUBEID);

                                if (Str_MultiColonies_ID != "")
                                {
                                    UPDATE_TUBERESULT(input_protocol, Array_GETSUBREQUESTID[5], Str_TUBEID, Str_MultiColonies_ID, Str_MultiColonies_NUMBER);
                                }
                                else
                                {
                                    UPDATE_TUBERESULT(input_protocol, Array_GETSUBREQUESTID[5], Str_TUBEID, Array_GETSUBREQUESTID[2], Array_GETSUBREQUESTID[6]);
                                }

                                MessageBox.Show("Match complete");

                                tabControl1.SelectedIndex = 1;
                                Loaddata_DatagridRescontrol();

                                // Clear UnMatch in Datagridview1

                                if (dataGridView1.Rows.Count > 0)
                                {
                                    foreach (DataGridViewRow row in dataGridView1.Rows)
                                    {
                                        string Cell1 = row.Cells[1].Value.ToString();

                                        if (Cell1 == Str_UNMATCHNUM)
                                        {
                                            if (row.Cells[1].Value.ToString() == Str_UNMATCHNUM)
                                            {
                                                dataGridView1.Rows.Remove(row);
                                            }
                                            //else
                                            //{
                                            //    row.Cells[3].Value = Properties.Resources.cancel_16x16;
                                            //    row.Cells[5].Value = "ลบ";
                                            //    row.DefaultCellStyle.BackColor = Color.LightPink;
                                            //    dataGridView1.Refresh();
                                            //}
                                        }
                                    }
                                }

                                LoadPending_Complete();
                                Str_MultiColonies_ID = "";
                                Str_MultiColonies_NUMBER = "";

                            }
                        }

                        Str_UNMATCHNUM = "";
                        Str_PATNUM = "";
                        Str_TUBEID = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                //Form_Match FM = new Form_Match(objTerminalGETM);
                //FM.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // From Step 1 Get SUBREQUESTID
        private string[] GetSUBREQUESTID(string PROTOCOLNUM)
        {
            string[] Result = new string[10];

            MicroscanController objConfig = new MicroscanController();
            DataTable dt = null;

            try
            {
                objTerminalGETM = new Terminal_GETM();

                objTerminalGETM.PROTOCOLNUM = PROTOCOLNUM;

                dt = objConfig.GetSUBREQUESTID(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result[0] = dt.Rows[0]["SUBREQUESTID"].ToString();
                    Result[1] = dt.Rows[0]["ACCESSNUMBER"].ToString();
                    Result[2] = dt.Rows[0]["COLONYID"].ToString();
                    Result[3] = dt.Rows[0]["BATTERYID"].ToString();
                    Result[4] = dt.Rows[0]["ISOLATNUMBER"].ToString();
                    Result[5] = dt.Rows[0]["PATNUMBER"].ToString();
                    Result[6] = dt.Rows[0]["COLONYNUMBER"].ToString();
                    Result[7] = dt.Rows[0]["NAME"].ToString();
                    Result[8] = dt.Rows[0]["LASTNAME"].ToString();
                    Result[9] = dt.Rows[0]["RECEIVEDDATE"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Result;
        }
        // From Auto Step Get SUBREQUESTID From Multicolony
        private string[] GetSUBREQUESTID_MultiColony(string PROTOCOLNUM, string Str_COLONYID)
        {
            string[] Result = new string[10];

            MicroscanController objConfig = new MicroscanController();
            DataTable dt = null;

            try
            {
                objTerminalGETM = new Terminal_GETM();

                objTerminalGETM.PROTOCOLNUM = PROTOCOLNUM;
                objTerminalGETM.COLONYID = Str_COLONYID;

                dt = objConfig.GetSUBREQUESTID_Multicolony(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result[0] = dt.Rows[0]["SUBREQUESTID"].ToString();
                    Result[1] = dt.Rows[0]["ACCESSNUMBER"].ToString();
                    Result[2] = dt.Rows[0]["COLONYID"].ToString();
                    Result[3] = dt.Rows[0]["BATTERYID"].ToString();
                    Result[4] = dt.Rows[0]["ISOLATNUMBER"].ToString();
                    Result[5] = dt.Rows[0]["PATNUMBER"].ToString();
                    Result[6] = dt.Rows[0]["COLONYNUMBER"].ToString();
                    Result[7] = dt.Rows[0]["NAME"].ToString();
                    Result[8] = dt.Rows[0]["LASTNAME"].ToString();
                    Result[9] = dt.Rows[0]["RECEIVEDDATE"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Result;
        }

        // From Step 1.1 Searth Multi Colonies
        private int SEARCH_MULTICOLONIES_1(string PROTOCOLNUM)
        {
            int Rowcount = -1;

            MicroscanController objConfig = new MicroscanController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                objTerminalGETM.PROTOCOLNUM = PROTOCOLNUM;

                dt = objConfig.GetSUBREQUESTID(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Rowcount++;
                    }
                }
                else
                {
                    Rowcount = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            return Rowcount;
        }
        // From Step 1.2 Search and Select COLONIES
        //
        private string[] SEARCH_MULTICOLONIES_2(string PROTOCOLNUM)
        {
            string[] Result = new string[2];

            MicroscanController objConfig = new MicroscanController();
            frmSearchdict fm = new frmSearchdict();

            DataTable dt;
            try
            {
                objTerminalGETM.PROTOCOLNUM = PROTOCOLNUM;

                dt = objConfig.Search_Multicolonies(objTerminalGETM);

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objTerminalGETM.COLONIES_MULTI_ID = fm.SelectedID.ToString();
                    objTerminalGETM.COLONIES_NAME = fm.SelectedName;
                    objTerminalGETM.COLONYNUMBER = fm.SelectedCode;

                    //COLONIES_1 = objTerminalGETM.COLONIES_CODE;
                    //COLONIES_2 = objTerminalGETM.COLONIES_NAME;
                    //COLONIES_3 = objTerminalGETM.COLONIES_MULTI_ID;

                    Result[0] = objTerminalGETM.COLONIES_MULTI_ID;
                    Result[1] = objTerminalGETM.COLONYNUMBER;

                    //    //int minRowCount = 0;

                    //    //for (int i = 0; i < dataGridView1.RowCount; i++)
                    //    //{
                    //    //    minRowCount++;
                    //    //}

                    //    //if (minRowCount == 0)
                    //    //{
                    //    //    dataGridView1.Rows.Add();
                    //    //    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = ANTIBIOTIC01;
                    //    //    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = ANTIBIOTIC02;
                    //    //    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = ANTIBIOTIC03;
                    //    //}
                    //    //else
                    //    //{
                    //    //    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = ANTIBIOTIC01;
                    //    //    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = ANTIBIOTIC02;
                    //    //    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = ANTIBIOTIC03;
                    //    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }
            return Result;
        }
        // From Step 2 Get ORGANISMID,MEDTHODID FROM DICT_MB_ORGANISMS
        private string[] Get_DICT_MB_ORGANISMS(string Str_Organismscode)
        {
            string[] Result = new string[3];

            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                objTerminalGETM = new Terminal_GETM();

                objTerminalGETM.ORGANISMCODE = Str_Organismscode;

                dt = objConfig.Get_Info_DICT_MB_ORGANISMS(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result[0] = dt.Rows[0]["ORGANISMID"].ToString();
                    Result[1] = dt.Rows[0]["ORGANISMNAME"].ToString();
                    Result[2] = dt.Rows[0]["METHODMBID"].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step 2 Search DICT_MB_ORGANISMS", ex.Message);
            }
            return Result;
        }
        // Form Step 2.1 Find ORGANISMINDEX
        // GET_ORGINDEX_IN_SUBREQMB_ORGANISMS
        private int[] GET_ORGINDEX_IN_SUBREQMB_ORGANISMS(string str_SubrequestID, string str_ColoniesID)
        {
            int[] Result = new int[3];
            int Count_Index = 0;
            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                objTerminalGETM = new Terminal_GETM();

                objTerminalGETM.SUBREQUESTID = str_SubrequestID;
                objTerminalGETM.COLONYID = str_ColoniesID;

                dt = objConfig.GET_ORGANISMINDEX(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result[0] = Convert.ToInt32(dt.Rows[0]["ORGANISMINDEX"].ToString());
                    Result[1] = Convert.ToInt32(dt.Rows[0]["SUBREQMBORGID"].ToString());

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Count_Index++;
                    }
                    Result[2] = Count_Index;
                }
                else
                {
                    Result[2] = Count_Index;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step 2.1 Search ORGANISMINDEX [0] AND SUBREQMBORGID [1] in SUBREQMB_ORGANISMS", ex.Message);
            }
            return Result;
        }
        // Form Step 2.1 Get before SAVE UPDATE?
        // CHECK_UPDATE
        private bool GET_CHECK_UPDATE(string str_SubrequestID, string str_ColoniesID)
        {
            bool Result = false;

            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                objTerminalGETM = new Terminal_GETM();

                objTerminalGETM.SUBREQUESTID = str_SubrequestID;
                objTerminalGETM.COLONYID = str_ColoniesID;

                dt = objConfig.GET_ORGANISMINDEX_FOR_UPDATE(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step 2.1 Search Result before update", ex.Message);
            }
            return Result;
        }
        // From Step 4 Search SUBREQMBSENSITIVITYID
        // GET_SUBREQMBSENSITIVITYID
        private string GET_SUBREQMBSENSITIVITYID(string str_SubrequestID, string str_ColoniesID)
        {
            string Result = "";

            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                //objTerminalGETM = new TerminalM();

                objTerminalGETM.SUBREQUESTID = str_SubrequestID;
                objTerminalGETM.COLONYID = str_ColoniesID;

                dt = objConfig.GET_SUBREQMBSENSITIVITYID(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0]["SUBREQMBSENSITIVITYID"].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step 4 Search SUBREQMBSENSITIVITYID in [SUBREQMB_SENSITIVITIES]", ex.Message);
            }
            return Result;
        }
        // From Step 4.1 Search Sen panel in DIC_MB_SENSITIVITY_PANEL --> MICROSCAN
        //
        private string GET_SENSITIVITY_MICROSCAN()
        {
            string Result = "";

            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                //objTerminalGETM = new TerminalM();

                objTerminalGETM.BATTERIES_CODE = "MICROSCAN";

                dt = objConfig.GET_SENSITIVITYID(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0]["SENSITIVITYID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step 4.1 Search SENSITIVITYID in [DICT_MB_SENSITIVITIES_PANEL]", ex.Message);
            }
            return Result;

        }
        // Form Step 5 Search ANTIBIOTICID in DICT_MB_ANTIBIOTICS
        // GET_ANTIBIOTICID
        private string GET_ANTIBIOTICID(string str_AntibioticCODE)
        {
            string Result = "";

            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                //objTerminalGETM = new TerminalM();

                objTerminalGETM.ANTIBIOTICCODE = str_AntibioticCODE;

                dt = objConfig.GET_ANTIBIOTICID(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0]["ANTIBIOTICID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step 5 Search ANTIBIOTICID in [DICT_MB_ANTIBIOTICS]", ex.Message);
            }
            return Result;
        }
        // Step After Auto
        private string GET_JOBID(string Str_SUBREQUESTID, string Str_COLONYID)
        {
            string Result = "";
            string ColonyID = "";
            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                //objTerminalGETM = new TerminalM();

                objTerminalGETM.SUBREQUESTID = Str_SUBREQUESTID;
                objTerminalGETM.COLONYID = Str_COLONYID;

                dt = objConfig.GET_JOBID(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string[] Messages = dt.Rows[i]["JOBNAME"].ToString().Split(':');
                        ColonyID = Messages[1].ToString().Trim();

                        if (Str_COLONYID == ColonyID)
                        {
                            Result = dt.Rows[i]["JOBID"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step Step After Auto Search JOBID in [JOBS]", ex.Message);
            }
            return Result;

        }
        // Get COLONYID FROM JOBS
        // GET_COLONY_JOBS
        private string GET_COLONY_JOBS(string Str_COLONYID)
        {
            string Result = "";
            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                objTerminalGETM = new Terminal_GETM();

                objTerminalGETM.COLONYID = Str_COLONYID;

                dt = objConfig.GET_COLONYNAME(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0]["COLONYNUMBER"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step Search JOBID in [JOBS] by COLONY to Colonyname", ex.Message);
            }
            return Result;

        }
        // GET DATA in MICROSCANDB
        // PROTOCOL_EXITS
        private string [] GET_PROTOCOL_EXITS(string Str_PROTOCOLNUM ,string Str_PATNUM)
        {
            string [] Result = new string [1];

            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                objTerminalGETM = new Terminal_GETM();

                objTerminalGETM.PROTOCOLNUM = Str_PROTOCOLNUM;
                objTerminalGETM.PATNUM = Str_PATNUM;

                dt = objConfig.GET_PROTOCOL_EXITS(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result[0] = dt.Rows[0]["TUBEID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step Search MICROSCANDB in [TUBE] by PROTOCOLNUMBER-COLONYID", ex.Message);
            }
            return Result;
        }
        // GET_SUBREQMBORGID For Update
        private string GET_SUBREQMBORGID(string Str_SUBREQUESTID, string Str_COLONYID)
        {
            string Result = "";
            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                //objTerminalGETM = new TerminalM();

                objTerminalGETM.SUBREQUESTID = Str_SUBREQUESTID;
                objTerminalGETM.COLONYID = Str_COLONYID;

                dt = objConfig.GET_SUBREQMBORGID(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Result = dt.Rows[i]["SUBREQMBORGID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step GET SUBREQMBORGID SURREQMB_ORGANISM", ex.Message);
            }
            return Result;

        }
        // GET RESULT IN SUBREQMB_ANTIBIOTICS
        // 
        private bool GET_RESULT_SUBREQMB_ANTIBIOTICS(string Str_SUBREQMBORGID, string Str_ANTIBIOTICID, string Str_SUBREQUESTID)
        {
            bool Result = false;

            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                objTerminalGETM = new Terminal_GETM();

                objTerminalGETM.SUBREQMBORGID = Str_SUBREQMBORGID;
                objTerminalGETM.ANTIBIOTICID = Str_ANTIBIOTICID;
                objTerminalGETM.SUBREQUESTID = Str_SUBREQUESTID;

                dt = objConfig.GET_RESULT_SUBREQMB_ANTIBIOTICS(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step GET Result in SUBREQMB_ANTIBIOTICS", ex.Message);
            }
            return Result;
        }
        // GET_RESULT_SUBREQMB_SENSITIVITIES
        private string [] GET_SUBREQMB_SENSITIVITIES(string Str_SUBREQUESTID, string Str_COLONYID)
        {
            string [] Result = new string [1];

            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                objTerminalGETM = new Terminal_GETM();

                objTerminalGETM.SUBREQUESTID = Str_SUBREQUESTID;
                objTerminalGETM.COLONYID = Str_COLONYID;

                dt = objConfig.GET_RESULT_SUBREQMB_SENSITIVITIES(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result[0] = dt.Rows[0]["SUBREQMBSENSITIVITYID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Step GET SUBREQMBSENSITIVITYID in SUBREQMB_SENSITIVITIES", ex.Message);
            }
            return Result;
        }

        // Step Matching with Pending
        private void Matching_(string input_protocol)
        {
            // Do step
            // Step 1
            // GET SUBREQUESTID
            // FROM MB_REQUESTS , REQUESTS, SUBREQMB_BATTERIES
            // 
            // Array_BATTERIES [ ]
            // [0] = SUBREQUESTID
            // [1] = ACCESSNUMBER
            // [2] = COLONYID
            // [3] = BATTERYID
            // [4] = ISOLATNUMBER
            // 
            // Step 1.1
            // Searth Multi Colonies
            // Count ROWS =?
            //

            try
            {
                string[] Array_BATTERIES;
                int RowMultiColonies = 0;

                Array_BATTERIES = GetSUBREQUESTID(input_protocol);

                // Array_BATTERIES [ ]
                // [0] = SUBREQUESTID
                // [1] = ACCESSNUMBER
                // [2] = COLONYID
                // [3] = BATTERYID
                // [4] = ISOLATNUMBER

                if (input_protocol != "")
                {
                    RowMultiColonies = SEARCH_MULTICOLONIES_1(input_protocol);

                    if (RowMultiColonies >= 1)
                    {
                        // Step 1.2 Search and Select Colonies numbers
                        string[] Str_MultiColoies = SEARCH_MULTICOLONIES_2(input_protocol);
                        // [0] COLONYID
                        // [1] COLONYNUMBER

                        //MessageBox.Show(Str_MultiColoies);
                        Matching_Per_Colonies(input_protocol, Array_BATTERIES[0], Str_MultiColoies[0], Array_BATTERIES[3]);
                        Str_MultiColonies_ID = Str_MultiColoies[0];
                        Str_MultiColonies_NUMBER = Str_MultiColoies[1];
                    }
                    else if (RowMultiColonies == 0)
                    {
                        Matching_Per_Colonies(input_protocol, Array_BATTERIES[0], Array_BATTERIES[2], Array_BATTERIES[3]);
                    }
                }
                else
                {
                    MessageBox.Show("Protocol number please.", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Matching_Per_Colonies(string input_protocol, string SUBREQUESTID, string COLONYID, string BATTERYID)
        {
            try
            {
                //string[] Array_BATTERIES;
                string Str_Organismcode = "";
                string Str_Messages_R = "";
                //input_protocol = "";
                bool Save_SUBREQMB_ORGANISMS = true;
                bool Save_SUBREQMB_ANTIBIOTICS = true;

                MicroscanController objConfig = new MicroscanController();
                objTerminalGETM = new Terminal_GETM();
                objTerminalSETM = new Terminal_SETM();

                // IF 1 COLONIES
                // MB_REQUESTS,SUBREQMB_BATTERIES
                // 
                //Array_BATTERIES = GetSUBREQUESTID(input_protocol);


                // Step 2 
                // เช็ค ถ้าไม่ มี SUBREQUESTID กับ BATTERIES ใน Request ก็จะไม่สามารถ Match ได้
                if (SUBREQUESTID != "" && BATTERYID != "")
                {
                    var MicroscanINI = new INIClass("Microscan.ini");
                    int[] ORGANISMINDEX;
                    bool CHECK_UPDATE = false;

                    DataGridViewRow row = dataGridView2.Rows[0];

                    Str_Messages_R = row.Cells[0].Value.ToString();     // MESSAGES ROW 0 = R
                    Str_Organismcode = row.Cells[5].Value.ToString();   // ORGANISM NUMBER FROM MICROSCAN

                    // Check ROW[0] = R? and ORGANISM NUMBER <> ""
                    if (Str_Messages_R == "R" && Str_Organismcode != "")
                    {
                        var Organism = MicroscanINI.Read(Str_Organismcode, "ORGANISM");
                        objTerminalGETM.ORGANISMCODE = Organism;

                        // Array_DICT_MB_ORGANISMS 
                        // [0] = ORGANISMID
                        // [1] = ORGANISMNAME
                        // [2] = METHODMBID

                        if (Organism != "")
                        {
                            string[] Array_DICT_MB_ORGANISMS = Get_DICT_MB_ORGANISMS(Organism);
                            // Step 2.1
                            // หา ORGANISMINDEX เพื่อจัดลำดับ Organisms ในแต่ละ Colonies
                            // หา RESULT ซ้ำใน SUBREQMB_ORGANISMS ว่ามีแล้วให้ UPDATE
                            // หาด้วย SUBREQUESTID, COLONYID , Array_DICT_MB_ORGANISMS[0]

                            // Check update TRUE, FALSE
                            CHECK_UPDATE = GET_CHECK_UPDATE(SUBREQUESTID, COLONYID);
                            //
                            // Step 2.2
                            // Count ORGANISMINDEX
                            //
                            // SUBREQMB_ORGANISMS
                            // [0] = ORGANISMINDEX
                            // [1] = SUBREQMBORGID
                            // [2] = Count_ROW
                            //
                            ORGANISMINDEX = GET_ORGINDEX_IN_SUBREQMB_ORGANISMS(SUBREQUESTID, COLONYID);

                            if (ORGANISMINDEX[2] >= 1)
                            {
                                ORGANISMINDEX[2]++;
                            }
                            else if (ORGANISMINDEX[2] == 0)
                            {
                                ORGANISMINDEX[2] = 1;
                            }

                            // Step 3 now Save New ORGANISMS / Identification in SUBREQMB_ORGANISMS
                            // ถ้าไม่มี CODE ORGANISM กับ COLONYID อยู่แล้ว ให้ INSERT
                            // หาด้วย SUBREQUESTID, COLONYID
                            // Add object
                            // Get for update
                            objTerminalGETM.SUBREQMBORGID = Convert.ToString(ORGANISMINDEX[1]);

                            // Set for Insert
                            objTerminalGETM.BATTERIESID = BATTERYID;

                            objTerminalSETM.COLONYID = COLONYID;
                            objTerminalSETM.ORGANISMID = Array_DICT_MB_ORGANISMS[0];
                            objTerminalSETM.ORGANISMINDEX = Convert.ToString(ORGANISMINDEX[2]);
                            objTerminalSETM.CREATEUSER = "WST";
                            objTerminalSETM.IDENTUSER = "WST";
                            objTerminalSETM.COMMENTS = "";
                            objTerminalSETM.CONSOLIDATESTATUS = "0";
                            objTerminalSETM.NOTPRINTABLE = "0";
                            objTerminalSETM.LogUserID = "WST";
                            objTerminalSETM.SUBREQUESTID = SUBREQUESTID;

                            // CHECK RESULT UPDATE AND GET SUBREQMBORGID
                            // 
                            if (CHECK_UPDATE)
                            {
                                // Step 3.1 Update Organism in SUBREQMB_ORGANISMS
                                //objTerminalGETM = objConfig.Update_SUBREQMB_ORGANISMS(objTerminalGETM);
                                MessageBox.Show("Update");
                            }
                            else
                            {
                                // Step 3.2 Insert Organism in SUBREQMB_ORGANISMS
                                // GET SUBREQMBORGID in [scope_identity]
                                // 
                                Save_SUBREQMB_ORGANISMS = true;
                            }

                            // SUBREQMB_SENSITIVITIES
                            // Step 4 Insert SUBREQMB_SENSITIVITIES And Get [SUBREQMBSENSITIVITYID]
                            // Insert ด้วย SUBREQUESTID, COLONYID with Microscan ID SEARCH in DICT_MB_SENSITIVITY_PANEL

                            // 
                            string SENSITIVITYID = "";
                            string SUBREQMBSENSITIVITYID = "";
                            string ANTIBIOTIC_MICROSCAN = "";
                            // Step 4.1 Search Senpanel MICROSCAN
                            //
                            SENSITIVITYID = GET_SENSITIVITY_MICROSCAN();

                            //SUBREQMBSENSITIVITYID = GET_SUBREQMBSENSITIVITYID(Array_BATTERIES[0], Array_BATTERIES[2]);

                            if (SENSITIVITYID != "")
                            {

                                // Step 5 INSERT SUBREQMB_ANTIBIOTICS
                                // Step 5.1 Get ANTIBIOTIC ID
                                // หาด้วย Mapcode 

                                if (dataGridView2.Rows.Count > 0)
                                {
                                    objTerminalSETM.MEDTHODMBID = "";
                                    objTerminalSETM.SUBREQUESTID = SUBREQUESTID;
                                    objTerminalSETM.SUBREQMBSENSITIVITYID = SUBREQMBSENSITIVITYID;
                                    objTerminalSETM.VALREQUESTED = "0";
                                    objTerminalSETM.UNITS = "0";
                                    objTerminalSETM.UPDATERULEID = "0";
                                    objTerminalSETM.DELTACHECK = "0";
                                    objTerminalSETM.MICID = "0";
                                    // For INSERT SUBREQMB_SENSITIVITIES

                                    objTerminalSETM.SENSITIVITYID = SENSITIVITYID;

                                    foreach (DataGridViewRow row_Antibiotic in dataGridView2.Rows)
                                    {
                                        if (row_Antibiotic.Cells[0].Value == null || row_Antibiotic.Cells[0].Value == DBNull.Value || String.IsNullOrWhiteSpace(row_Antibiotic.Cells[0].Value.ToString()))
                                        {
                                            objTerminalSETM.ANTIBIOTICID = "";
                                        }
                                        else
                                        {
                                            objTerminalSETM.ANTIBIOTICID = row_Antibiotic.Cells[0].Value.ToString();
                                        }
                                        if (row_Antibiotic.Cells[1].Value == null || row_Antibiotic.Cells[1].Value == DBNull.Value || String.IsNullOrWhiteSpace(row_Antibiotic.Cells[1].Value.ToString()))
                                        {
                                            ANTIBIOTIC_MICROSCAN = "";
                                        }
                                        else
                                        {
                                            ANTIBIOTIC_MICROSCAN = row_Antibiotic.Cells[1].Value.ToString();
                                        }

                                        if (objTerminalSETM.ANTIBIOTICID != "R")
                                        {
                                            var AntibioticID = MicroscanINI.Read(ANTIBIOTIC_MICROSCAN, "ANTIBIOTIC");

                                            string Str_ANTIBIOTICID = GET_ANTIBIOTICID(AntibioticID);

                                            if (Str_ANTIBIOTICID == "")
                                            {
                                                Save_SUBREQMB_ANTIBIOTICS = false;

                                                MessageBox.Show("Not found Antibiotic in Mapcode--> Microscan =" + ANTIBIOTIC_MICROSCAN + " System code =" + AntibioticID);
                                                break;
                                            }
                                        }
                                    }

                                    if (Save_SUBREQMB_ANTIBIOTICS && Save_SUBREQMB_ORGANISMS)
                                    {
                                        // From Step 4 Insert
                                        // Step 4.2 Insert [SUBREQMB_SENSITIVITIES]
                                        //

                                        objTerminalSETM = objConfig.INSERT_SUBREQMB_SENSITIVITIES(objTerminalSETM);

                                        // From Step 3.2 Insert Organism in SUBREQMB_ORGANISMS
                                        // Insert

                                        objTerminalSETM = objConfig.INSERT_SUBREQMB_ORGANISMS(objTerminalSETM);
                                        //
                                        // Update TUBE
                                        Bool_updateUnmatch = true;
                                    }

                                    if (Save_SUBREQMB_ANTIBIOTICS)
                                    {
                                        foreach (DataGridViewRow row_Antibiotic in dataGridView2.Rows)
                                        {

                                            if (row_Antibiotic.Cells[0].Value == null || row_Antibiotic.Cells[0].Value == DBNull.Value || String.IsNullOrWhiteSpace(row_Antibiotic.Cells[0].Value.ToString()))
                                            {
                                                objTerminalSETM.ANTIBIOTICID = "";
                                            }
                                            else
                                            {
                                                objTerminalSETM.ANTIBIOTICID = row_Antibiotic.Cells[0].Value.ToString();
                                            }
                                            if (row_Antibiotic.Cells[1].Value == null || row_Antibiotic.Cells[1].Value == DBNull.Value || String.IsNullOrWhiteSpace(row_Antibiotic.Cells[1].Value.ToString()))
                                            {
                                                //objTerminalGETM.ANTIBIOTICCODE = "";
                                                ANTIBIOTIC_MICROSCAN = "";
                                            }
                                            else
                                            {
                                                //objTerminalGETM.ANTIBIOTICCODE = row_Antibiotic.Cells[1].Value.ToString();
                                                ANTIBIOTIC_MICROSCAN = row_Antibiotic.Cells[1].Value.ToString();
                                            }
                                            if (row_Antibiotic.Cells[2].Value == null || row_Antibiotic.Cells[2].Value == DBNull.Value || String.IsNullOrWhiteSpace(row_Antibiotic.Cells[2].Value.ToString()))
                                            {
                                                objTerminalSETM.ANTIBIOTICNAME = "";
                                            }
                                            else
                                            {
                                                objTerminalSETM.ANTIBIOTICNAME = row_Antibiotic.Cells[2].Value.ToString();
                                            }
                                            if (row_Antibiotic.Cells[3].Value == null || row_Antibiotic.Cells[3].Value == DBNull.Value || String.IsNullOrWhiteSpace(row_Antibiotic.Cells[3].Value.ToString()))
                                            {
                                                objTerminalSETM.ANTIBIOTIC_RESULT_VALUE = "";
                                            }
                                            else
                                            {
                                                objTerminalSETM.ANTIBIOTIC_RESULT_VALUE = row_Antibiotic.Cells[3].Value.ToString();
                                            }
                                            if (row_Antibiotic.Cells[7].Value == null || row_Antibiotic.Cells[7].Value == DBNull.Value || String.IsNullOrWhiteSpace(row_Antibiotic.Cells[7].Value.ToString()))
                                            {
                                                objTerminalSETM.ANTIBIOTIC_CLSI_SYS = "N/A";
                                            }
                                            else
                                            {
                                                objTerminalSETM.ANTIBIOTIC_CLSI_SYS = row_Antibiotic.Cells[7].Value.ToString();
                                            }
                                            if (row_Antibiotic.Cells[8].Value == null || row_Antibiotic.Cells[8].Value == DBNull.Value || String.IsNullOrWhiteSpace(row_Antibiotic.Cells[8].Value.ToString()))
                                            {
                                                objTerminalSETM.ANTIBIOTIC_CLSI_URINE = "";
                                            }
                                            else
                                            {
                                                objTerminalSETM.ANTIBIOTIC_CLSI_URINE = row_Antibiotic.Cells[8].Value.ToString();
                                            }

                                            if (objTerminalSETM.ANTIBIOTICID != "R")
                                            {
                                                var AntibioticID = MicroscanINI.Read(ANTIBIOTIC_MICROSCAN, "ANTIBIOTIC");

                                                string Str_ANTIBIOTICID = GET_ANTIBIOTICID(AntibioticID);
                                                // Step 5.1 Searth ANTIBIOTICID
                                                objTerminalSETM.ANTIBIOTICID = Str_ANTIBIOTICID;
                                                // Step 5.2 Insert database SUBREQMB_ANTIBIOTICS
                                                objTerminalSETM = objConfig.INSERT_SUBREQMB_ANTIBIOTICS(objTerminalSETM);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Not found Sensitivity panel. Microscan ");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Not found Organism mapcode ---> Microscan=  " + Str_Organismcode + " System organism = " + Organism);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Not Found Batteries in Request!.", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Step Match Automatic
        private void Matching_Automatic(string input_protocol, string Str_SUBREQUESTID, string Str_COLONYID, string Str_BATTERYID, string Str_PATNUMBER, string Str_TUBEID)
        {
            try
            {
                string sql = "";
                string Seq_R_0 = "";
                string Seq_R_1 = "";
                string Seq_R_2 = "";

                string Seq_M_1 = "";
                string Seq_M_2 = "";
                string Seq_M_4 = "";
                string Seq_M_7 = "";
                //string Seq_M_9 = "";

                bool Save_SUBREQMB_ORGANISMS = false;
                bool Save_SUBREQMB_ANTIBIOTICS = true;

                string SENSITIVITYID = "";
                string SUBREQMBSENSITIVITYID = "";
                string ANTIBIOTIC_MICROSCAN = "";
                string ORGANISMID = "";

                MicroscanController objConfig = new MicroscanController();
                objTerminalGETM = new Terminal_GETM();
                objTerminalSETM = new Terminal_SETM();

                conn = new CSMicroscanDAO().Connect();

                sql = @"SELECT TUBERESULT FROM TUBE WHERE PATNUM = '" + Str_PATNUMBER + "' AND TUBEID = '" + Str_TUBEID + "' ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var MicroscanINI = new INIClass("Microscan.ini");
                    string line_wstfiles = "";
                    string converted = "";
                    StringReader strReader = null;

                    bool Bool_read_1 = false;
                    bool Bool_read_2 = false;

                    bool CHECK_UPDATE = false;

                    reader.Read();
                    byte[] bytes = (byte[])reader["TUBERESULT"];

                    objTerminalSETM.MEDTHODMBID = "";
                    objTerminalSETM.SUBREQUESTID = Str_SUBREQUESTID;
                    objTerminalSETM.SUBREQMBSENSITIVITYID = SUBREQMBSENSITIVITYID;
                    objTerminalSETM.VALREQUESTED = "0";
                    objTerminalSETM.UNITS = "0";
                    objTerminalSETM.UPDATERULEID = "0";
                    objTerminalSETM.DELTACHECK = "0";
                    objTerminalSETM.MICID = "0";
                    objTerminalGETM.BATTERIESID = Str_BATTERYID;
                    objTerminalSETM.COLONYID = Str_COLONYID;
                    objTerminalSETM.CREATEUSER = "WST";
                    objTerminalSETM.IDENTUSER = "WST";
                    objTerminalSETM.COMMENTS = "";
                    objTerminalSETM.CONSOLIDATESTATUS = "0";
                    objTerminalSETM.NOTPRINTABLE = "0";
                    objTerminalSETM.LogUserID = "WST";

                    if (bytes.Length > 0)
                    {
                        converted = Encoding.Default.GetString(bytes, 0, bytes.Length);
                        strReader = new StringReader(converted.ToString());

                        Bool_read_1 = true;
                    }
                    // อ่านรอบแรก เพื่อตรวจสอบ Microscan code กับ ระบบว่ามีอยู่จริงหรือไม่
                    // อ่านรอบแรก
                    if (Bool_read_1)
                    {
                        while ((line_wstfiles = strReader.ReadLine()) != null)
                        {
                            if (line_wstfiles.Length > 0)
                            {
                                string SeqMen_ALL = line_wstfiles.Substring(0, 1);   //  Get Messages 1 Digit for switch case,
                                // switch 1 for check mapcode
                                switch (SeqMen_ALL)
                                {
                                    case "H":     // Header Record (H)
                                        break;
                                    case "P":      // Patient record (P)
                                        break;
                                    case "B":      // Specimen record (B)
                                        break;
                                    case "R":      // Isolate record (R)
                                        string[] Messages_R = line_wstfiles.Split('|');

                                        Seq_R_0 = Messages_R[2].ToString().Trim();    //Specimen number /protocol
                                        Seq_R_1 = Messages_R[6].ToString().Trim();    //Organism code / number
                                        Seq_R_2 = Messages_R[7].ToString().Trim();    //Organism name

                                        if (Str_SUBREQUESTID != "" && Str_BATTERYID != "")
                                        {
                                            int[] ORGANISMINDEX;

                                            if (Seq_R_1 != "")
                                            {
                                                var Organism = MicroscanINI.Read(Seq_R_1, "ORGANISM");
                                                objTerminalGETM.ORGANISMCODE = Organism;

                                                // Array_DICT_MB_ORGANISMS 
                                                // [0] = ORGANISMID
                                                // [1] = ORGANISMNAME
                                                // [2] = METHODMBID

                                                if (Organism != "")
                                                {

                                                    // หา RESULT ซ้ำใน SUBREQMB_ORGANISMS ว่ามีแล้วให้ UPDATE
                                                    // หาด้วย SUBREQUESTID, COLONYID
                                                    // Check update TRUE, FALSE
                                                    CHECK_UPDATE = GET_CHECK_UPDATE(Str_SUBREQUESTID, Str_COLONYID);

                                                    // CHECK RESULT UPDATE AND GET SUBREQMBORGID
                                                    // 
                                                    if (CHECK_UPDATE)
                                                    {
                                                        // Step 3.1 Update Organism in SUBREQMB_ORGANISMS
                                                        //objTerminalGETM = objConfig.Update_SUBREQMB_ORGANISMS(objTerminalGETM);
                                                        //MessageBox.Show("Update");

                                                        string[] Array_DICT_MB_ORGANISMS = Get_DICT_MB_ORGANISMS(Organism);
                                                        ORGANISMID = Array_DICT_MB_ORGANISMS[0];
                                                    }
                                                    else
                                                    {
                                                        // Step 3.2 Insert Organism in SUBREQMB_ORGANISMS
                                                        // GET SUBREQMBORGID in [scope_identity]
                                                        // 
                                                        // เก็บค่า การบันทึก Organism
                                                        string[] Array_DICT_MB_ORGANISMS = Get_DICT_MB_ORGANISMS(Organism);

                                                        // Array_DICT_MB_ORGANISMS
                                                        // [0] = ORGANISMID
                                                        // [1] = ORGANISMNAME
                                                        // [2] = METHODMBID

                                                        // Count ORGANISMINDEX
                                                        //
                                                        // SUBREQMB_ORGANISMS
                                                        // [0] = ORGANISMINDEX
                                                        // [1] = SUBREQMBORGID
                                                        // [2] = Count_ROW
                                                        //
                                                        // หาด้วย SUBREQUESTID, COLONYID
                                                        // หา ORGANISMINDEX เพื่อจัดลำดับ Organisms ในแต่ละ Colonies
                                                        ORGANISMINDEX = GET_ORGINDEX_IN_SUBREQMB_ORGANISMS(Str_SUBREQUESTID, Str_COLONYID);

                                                        if (ORGANISMINDEX[2] >= 1)
                                                        {
                                                            ORGANISMINDEX[2]++;
                                                        }
                                                        else if (ORGANISMINDEX[2] == 0)
                                                        {
                                                            ORGANISMINDEX[2] = 1;
                                                        }

                                                        // Step  3 now Save New ORGANISMS / Identification in SUBREQMB_ORGANISMS

                                                        objTerminalGETM.SUBREQMBORGID = Convert.ToString(ORGANISMINDEX[1]);

                                                        // Set ข้อมูลสำหรับ Insert Organism in SUBREQMB_ORGANISM
                                                        // Array_DICT_MB_ORGANISMS[0] = ORGANISMID
                                                        // Array_DICT_MB_ORGANISMS[2] = SUBREQMBORGID
                                                        // 
                                                        objTerminalSETM.ORGANISMID = Array_DICT_MB_ORGANISMS[0];
                                                        objTerminalSETM.ORGANISMINDEX = Convert.ToString(ORGANISMINDEX[2]);

                                                        // เช็ค Code Batteries ว่ามีอยู่จริง
                                                        // บันทึก Sensitivity panel อัตโนมัติ 
                                                        // เป็นรหัส MICROSCAN
                                                        //
                                                        SENSITIVITYID = GET_SENSITIVITY_MICROSCAN();
                                                        objTerminalSETM.SENSITIVITYID = SENSITIVITYID;

                                                        // กำหนดให้บันทึก Organism เป็นจริง
                                                        Save_SUBREQMB_ORGANISMS = true;
                                                        // กำหนดให้ การอ่านครั้งที่สอง เป็นจริง
                                                        Bool_read_2 = true;

                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "M":      // Test/MIC record (M)
                                        string[] Messages_M = line_wstfiles.Split('|');

                                        Seq_M_1 = Messages_M[1].ToString().Trim();    //Antibiotic line
                                        Seq_M_2 = Messages_M[2].ToString().Trim();    //Antibiotic code
                                        Seq_M_4 = Messages_M[4].ToString().Trim();    //Antibiotic result
                                        //Seq_M_4 = Messages_M[4].ToString().Trim();  //
                                        Seq_M_7 = Messages_M[7].ToString().Trim();    //CLSI SYS
                                        //Seq_M_9 = Messages_M[9].ToString().Trim();  //CLSI Urine


                                        ANTIBIOTIC_MICROSCAN = Seq_M_2;

                                        var AntibioticID = MicroscanINI.Read(ANTIBIOTIC_MICROSCAN, "ANTIBIOTIC");

                                        string Str_ANTIBIOTICID = GET_ANTIBIOTICID(AntibioticID);

                                        if (Str_ANTIBIOTICID == "")
                                        {
                                            // กำหนดให้ Antibiotics เป็นเท็จ จะทำให้การบันทึกครั้งที่สอง ไม่ทำงาน
                                            // เพราะไม่เจอ Mapcode บางตัว 
                                            // 
                                            Save_SUBREQMB_ANTIBIOTICS = false;

                                            // เก็บบันทึก Mapcode ที่ไม่เจอไว้ใน Log files

                                            Writelog.Log_Microscan("Not found Antibiotic in Mapcode--> Microscan =" + ANTIBIOTIC_MICROSCAN + " System code =" + AntibioticID);
                                            break;
                                        }
                                        break;
                                    case "Q":      // Trace record (Q)
                                        break;
                                    case "C":      // Comment record (C)
                                        break;
                                    case "F":      // Free text record (F)
                                        break;
                                    case "A":      // Request record (A)
                                        break;
                                    case "L":      // End Record (L)
                                        break;
                                }
                            }
                        }
                    }
                    // Read รอบ สอง และ บันทึก ORGANISM and ANTIBIOTIC เป็นจริง
                    // บันทึก Organism ถ้ามี Mapcode และเป็น บันทึกครั้งแรก
                    // บันทึก Antibiotic ถ้ามี Mapcode ครบ 
                    // 
                    if (Bool_read_2 && Save_SUBREQMB_ORGANISMS && Save_SUBREQMB_ANTIBIOTICS)
                    {
                        converted = Encoding.Default.GetString(bytes, 0, bytes.Length);
                        strReader = new StringReader(converted.ToString());

                        while ((line_wstfiles = strReader.ReadLine()) != null)
                        {
                            if (line_wstfiles.Length > 0)
                            {
                                string SeqMen_ALL = line_wstfiles.Substring(0, 1);   //  Get Messages 1 Digit for switch case,
                                // switch 2 for insert if Mapping correct.
                                switch (SeqMen_ALL)
                                {
                                    case "H":     // Header Record (H)
                                        break;
                                    case "P":      // Patient record (P)
                                        break;
                                    case "B":      // Specimen record (B)
                                        break;
                                    case "R":      // Isolate record (R)
                                        string[] Messages_R = line_wstfiles.Split('|');

                                        Seq_R_0 = Messages_R[2].ToString().Trim();    //Specimen number /protocol
                                        Seq_R_1 = Messages_R[6].ToString().Trim();    //Organism code / number
                                        Seq_R_2 = Messages_R[7].ToString().Trim();    //Organism name

                                        objTerminalSETM = objConfig.INSERT_SUBREQMB_SENSITIVITIES(objTerminalSETM);

                                        objTerminalSETM = objConfig.INSERT_SUBREQMB_ORGANISMS(objTerminalSETM);

                                        break;
                                    case "M":      // Test/MIC record (M)
                                        string[] Messages_M = line_wstfiles.Split('|');

                                        Seq_M_1 = Messages_M[1].ToString().Trim();    //Antibiotic line
                                        Seq_M_2 = Messages_M[2].ToString().Trim();    //Antibiotic CODE
                                        Seq_M_4 = Messages_M[4].ToString().Trim();    //Antibiotic result
                                      //Seq_M_4 = Messages_M[5].ToString().Trim();    //
                                        Seq_M_7 = Messages_M[7].ToString().Trim();    //CLSI SYS
                                        //Seq_M_9 = Messages_M[9].ToString().Trim();    //CLSI Urine


                                        ANTIBIOTIC_MICROSCAN = Seq_M_2;

                                        var AntibioticID = MicroscanINI.Read(ANTIBIOTIC_MICROSCAN, "ANTIBIOTIC");

                                        string Str_ANTIBIOTICID = GET_ANTIBIOTICID(AntibioticID);

                                        if (Seq_M_4 == "")
                                        {
                                            objTerminalSETM.ANTIBIOTIC_RESULT_VALUE = "";
                                        }
                                        else
                                        {
                                            objTerminalSETM.ANTIBIOTIC_RESULT_VALUE = Seq_M_4;
                                        }
                                        if (Seq_M_7 == "")
                                        {
                                            objTerminalSETM.ANTIBIOTIC_CLSI_SYS = "N/A";
                                        }
                                        else
                                        {
                                            objTerminalSETM.ANTIBIOTIC_CLSI_SYS = Seq_M_7;
                                        }
                                        //if (Seq_M_9 == "")
                                        //{
                                        //    objTerminalSETM.ANTIBIOTIC_CLSI_URINE = "N/A";
                                        //}
                                        //else
                                        //{
                                        //    objTerminalSETM.ANTIBIOTIC_CLSI_URINE = Seq_M_9;
                                        //}

                                        // Step 5.1 Searth ANTIBIOTICID
                                        objTerminalSETM.ANTIBIOTICID = Str_ANTIBIOTICID;
                                        // Step 5.2 Insert database SUBREQMB_ANTIBIOTICS
                                        objTerminalSETM = objConfig.INSERT_SUBREQMB_ANTIBIOTICS(objTerminalSETM);

                                        break;
                                    case "Q":      // Trace record (Q)
                                        break;
                                    case "C":      // Comment record (C)
                                        break;
                                    case "F":      // Free text record (F)
                                        break;
                                    case "A":      // Request record (A)
                                        break;
                                    case "L":      // End Record (L)
                                        break;
                                }
                            }
                        }
                        // มีการบันทึกเป็นจริง 
                        // 
                        // Close Automatch by update status = 1 in Table TUBE
                        //
                        Bool_Match = true;

                    }

                    // ถ้าเป็นการ Update Result
                    //
                    if (CHECK_UPDATE)
                    {
                        converted = Encoding.Default.GetString(bytes, 0, bytes.Length);
                        strReader = new StringReader(converted.ToString());

                        string Str_SUBREQMBORGID = "";

                        while ((line_wstfiles = strReader.ReadLine()) != null)
                        {
                            if (line_wstfiles.Length > 0)
                            {
                                string SeqMen_ALL = line_wstfiles.Substring(0, 1);   //  Get Messages 1 Digit for switch case,
                                // switch 2 for insert if Mapping correct.
                                switch (SeqMen_ALL)
                                {
                                    case "H":     // Header Record (H)
                                        break;
                                    case "P":      // Patient record (P)
                                        break;
                                    case "B":      // Specimen record (B)
                                        break;
                                    case "R":      // Isolate record (R)
                                        string[] Messages_R = line_wstfiles.Split('|');

                                        Seq_R_0 = Messages_R[2].ToString().Trim();    //Specimen number /protocol
                                        Seq_R_1 = Messages_R[6].ToString().Trim();    //Organism code / number
                                        Seq_R_2 = Messages_R[7].ToString().Trim();    //Organism name

                                        // Update Organism in SUBREQMB_ORGANISMS
                                        //
                                        // ค้นหา SUBREQMBORGID เพื่อทำการ Update
                                        Str_SUBREQMBORGID = GET_SUBREQMBORGID(Str_SUBREQUESTID, Str_COLONYID);
                                        // Update SUBREQMB_ORGANISMS
                                        // Update
                                        objTerminalSETM.SUBREQMBORGID = Str_SUBREQMBORGID;
                                        objTerminalSETM.ORGANISMID = ORGANISMID;
                                        ORGANISMID = "";

                                        //objTerminalSETM.SUBREQUESTID = Str_SUBREQUESTID;
                                        //objTerminalSETM.COLONYID = Str_COLONYID;
                                        objTerminalSETM = objConfig.UPDATE_SUBREQMB_ORGANISMS(objTerminalSETM);

                                        break;
                                    case "M":      // Test/MIC record (M)
                                        string[] Messages_M = line_wstfiles.Split('|');

                                        //Seq_M_1 = Messages_M[1].ToString().Trim();  //Antibiotic line
                                        Seq_M_2 = Messages_M[2].ToString().Trim();    //Antibiotic CODE
                                        Seq_M_4 = Messages_M[4].ToString().Trim();    //Antibiotic result
                                        //Seq_M_4 = Messages_M[5].ToString().Trim();  //
                                        Seq_M_7 = Messages_M[7].ToString().Trim();    //CLSI SYS
                                        //Seq_M_9 = Messages_M[9].ToString().Trim();  //CLSI Urine

                                        ANTIBIOTIC_MICROSCAN = Seq_M_2;

                                        var AntibioticID = MicroscanINI.Read(ANTIBIOTIC_MICROSCAN, "ANTIBIOTIC");

                                        string Str_ANTIBIOTICID = GET_ANTIBIOTICID(AntibioticID);

                                        if (Seq_M_4 == "")
                                        {
                                            objTerminalSETM.ANTIBIOTIC_RESULT_VALUE = "";
                                        }
                                        else
                                        {
                                            objTerminalSETM.ANTIBIOTIC_RESULT_VALUE = Seq_M_4;
                                        }
                                        if (Seq_M_7 == "")
                                        {
                                            objTerminalSETM.ANTIBIOTIC_CLSI_SYS = "N/A";
                                        }
                                        else
                                        {
                                            objTerminalSETM.ANTIBIOTIC_CLSI_SYS = Seq_M_7;
                                        }
                                        //if (Seq_M_9 == "")
                                        //{
                                        //    objTerminalSETM.ANTIBIOTIC_CLSI_URINE = "N/A";
                                        //}
                                        //else
                                        //{
                                        //    objTerminalSETM.ANTIBIOTIC_CLSI_URINE = Seq_M_9;
                                        //}

                                        // Step 5.1 Searth ANTIBIOTICID
                                        objTerminalSETM.ANTIBIOTICID = Str_ANTIBIOTICID;
                                        // Step Update in SUBREQMB_ANTIBIOTICS
                                        // UPDATE SUBREQMB_ANTIBIOTICS
                                        // 
                                        // METHODMBID
                                        // SUBREQMBORGID
                                        // ANTIBIOTICID
                                        // SUBREQUESTID
                                        // SUBREQMBSENSITIVITYID
                                        // 
                                        // เช็ค Result เดิม
                                        bool Bool_checkResult = GET_RESULT_SUBREQMB_ANTIBIOTICS(Str_SUBREQMBORGID, Str_ANTIBIOTICID, Str_SUBREQUESTID);

                                        // ถ้ามี Antibiotic อยู่แล้ว ให้ Update
                                        if (Bool_checkResult)
                                        {
                                            objTerminalSETM.SUBREQMBORGID = Str_SUBREQMBORGID;
                                            //objTerminalSETM.SUBREQUESTID = Str_SUBREQUESTID;
                                            //objTerminalSETM.COLONYID = Str_COLONYID;

                                            objTerminalSETM = objConfig.UPDATE_SUBREQMB_ANTIBIOTICS(objTerminalSETM);
                                        }
                                        // ถ้าไม่มี Antibiotic ใน SUBREQMBORGID ให้ Insert เพิ่ม
                                        else
                                        {
                                            // GET SUBREQMBSENSITIVITYID
                                            // GET_RESULT_SUBREQMB_ANTIBIOTICS(Str_SUBREQMBORGID, Str_ANTIBIOTICID, Str_SUBREQUESTID);
                                            //
                                            //string [] RESULT_SUBREQMB_ANTIBIOTICS = GET_SUBREQMB_ANTIBIOTICS(Str_SUBREQMBORGID, Str_ANTIBIOTICID, Str_SUBREQUESTID);
                                            string[] RESULT_SUBREQMB_SENSITIVITIES = GET_SUBREQMB_SENSITIVITIES(Str_SUBREQUESTID,Str_COLONYID);


                                            objTerminalSETM.MEDTHODMBID = "";
                                            objTerminalSETM.SUBREQUESTID = Str_SUBREQUESTID;
                                            objTerminalSETM.SUBREQMBSENSITIVITYID = RESULT_SUBREQMB_SENSITIVITIES[0];
                                            objTerminalSETM.VALREQUESTED = "0";
                                            objTerminalSETM.UNITS = "0";
                                            objTerminalSETM.UPDATERULEID = "0";
                                            objTerminalSETM.DELTACHECK = "0";
                                            objTerminalSETM.MICID = "0";
                                            objTerminalSETM.COLONYID = Str_COLONYID;
                                            objTerminalSETM.CREATEUSER = "WST";
                                            objTerminalSETM.IDENTUSER = "WST";
                                            objTerminalSETM.COMMENTS = "";
                                            objTerminalSETM.CONSOLIDATESTATUS = "0";
                                            objTerminalSETM.NOTPRINTABLE = "0";
                                            objTerminalSETM.LogUserID = "WST";

                                            objTerminalSETM = objConfig.INSERT_SUBREQMB_ANTIBIOTICS(objTerminalSETM);
                                        }


                                        break;
                                    case "Q":      // Trace record (Q)
                                        break;
                                    case "C":      // Comment record (C)
                                        break;
                                    case "F":      // Free text record (F)
                                        break;
                                    case "A":      // Request record (A)
                                        break;
                                    case "L":      // End Record (L)
                                        break;
                                }
                            }
                        }
                        // มีการบันทึกเป็นจริง 
                        // 
                        // Close Automatch by update status = 1 in Table TUBE
                        //
                        Bool_Match = true;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Step After Matching 2 Update TUBE
        // Update_After_UnMatching
        private void Update_After_UnMatching(string Str_PROTOCOLNUM, string Str_PATNUM, string Str_TUBEID)
        {
            try
            {
                MicroscanController objConfig = new MicroscanController();
                objTerminalSETM = new Terminal_SETM();

                objTerminalSETM.TUBEID = Str_TUBEID;
                objTerminalSETM.PROTOCOLNUM = Str_PROTOCOLNUM + "-" + Str_TUBEID;
                objTerminalSETM.PATNUM = Str_PATNUM;
                objTerminalSETM.PATNAME = "";
                objTerminalSETM.PATLASTNAME = "";
                objTerminalSETM.TUBESTATUS = "1";


                objTerminalSETM = objConfig.Update_After_UnMatching(objTerminalSETM);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Step After Matching 3 Update TUBERESULT
        // Step After Unmatching
        // Auto Step 4
        private bool GET_FILES_AND_REQUESTS(string Str_PROTOCOLNUM, string Str_PATNUM)
        {
            bool Result = false;

            try
            {
                MicroscanController objConfig = new MicroscanController();
                DataTable dt = null;

                objTerminalGETM = new Terminal_GETM();

                objTerminalGETM.PROTOCOLNUM = Str_PROTOCOLNUM;
                objTerminalGETM.PATNUM = Str_PATNUM;

                dt = objConfig.GET_PATIENT_AND_PROTOCOL(objTerminalGETM);

                if (dt.Rows.Count > 0)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Auto Step 4 Search Request and Protocol before AutoMatching", ex.Message);
            }
            return Result;
        }
        //
        private void UPDATE_TUBERESULT(string Str_PROTOCOLNUM, string Str_PATNUM, string Str_TUBEID, string Str_COLONYID, string Str_COLONYNUMBER)
        {
            string sql = "";
            string MSG_P = "";
            string MSG_B = "";
            string MSG_R = "";
            string MSG_M = "";
            string MSG_L = "";

            string Header_P0 = "";
            string Messages_P_1 = "";
            string Messages_P_2 = "";
            string Messages_P_3 = "";

            string Header_B0 = "";
            string Messages_B_1 = "";
            string Messages_B_2 = "";
            string Messages_B_3 = "";

            string Header_R0 = "";
            string Messages_R_1 = "";
            string Messages_R_2 = "";
            string Messages_R_3 = "";
            string Messages_R_4 = "";
            string Messages_R_5 = "";
            string Messages_R_6 = "";
            string Messages_R_7 = "";

            try
            {
                conn = new CSMicroscanDAO().Connect();

                sql = @"SELECT TUBERESULT FROM TUBE WHERE TUBEID = '" + Str_TUBEID + "' ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    byte[] bytes = (byte[])reader["TUBERESULT"];

                    StringBuilder BinaryFiles = new StringBuilder();
                    BinaryFiles.Clear();

                    if (bytes.Length > 0)
                    {
                        string converted = Encoding.Default.GetString(bytes, 0, bytes.Length);
                        string String_read_line;
                        StringReader strReader = new StringReader(converted.ToString());

                        while ((String_read_line = strReader.ReadLine()) != null)
                        {
                            if (String_read_line.Length > 0)
                            {
                                string SeqMen_ALL = String_read_line.Substring(0, 1);   //  Get Messages 1 Digit for switch case,

                                switch (SeqMen_ALL)
                                {
                                    case "H":     // Header Record (H)
                                        break;
                                    case "P":      // Patient record (P)
                                        string[] Messages_P = String_read_line.Split('|');

                                        Header_P0 = Messages_P[0].ToString().Trim();
                                        Messages_P_1 = Messages_P[0].ToString().Trim();     // PATNUM
                                        Messages_P_2 = "";     // NAME      
                                        Messages_P_3 = "";     // LASTNAME      

                                        MSG_P = @"" + Header_P0 + "|" + Str_PATNUM + "|" + Messages_P_2 + "|" + Messages_P_3 + "|";

                                        BinaryFiles.AppendLine(MSG_P);

                                        break;
                                    case "B":      // Specimen record (B)
                                        string[] Messages_B = String_read_line.Split('|');

                                        Header_B0 = Messages_B[0].ToString().Trim();
                                        Messages_B_1 = Str_PROTOCOLNUM;         // Specimen
                                        Messages_B_2 = Messages_B[2].ToString().Trim();      // Sample         
                                        Messages_B_3 = Messages_B[3].ToString().Trim();      //Date Specimen Collected         

                                        MSG_B = @"" + Header_B0 + "|" + Messages_B_1 + "|" + Messages_B_2 + "|" + Messages_B_3 + "|";

                                        BinaryFiles.AppendLine(MSG_B);


                                        break;
                                    case "R":      // Isolate record (R)
                                        string[] Messages_R = String_read_line.Split('|');

                                        Header_R0 = Messages_R[0].ToString().Trim();
                                        Messages_R_1 = Messages_R[1].ToString().Trim();         // ISO Number
                                        Messages_R_2 = Str_PROTOCOLNUM;                            // Specimen
                                        Messages_R_3 = Messages_R[3].ToString().Trim();         // Group test
                                        Messages_R_4 = Messages_R[4].ToString().Trim();         // Group test description
                                        Messages_R_5 = Messages_R[5].ToString().Trim();         // Date time
                                        Messages_R_6 = Messages_R[6].ToString().Trim();         // Organism number
                                        Messages_R_7 = Messages_R[7].ToString().Trim();         // Orgranism name

                                        MSG_R = @"" + Header_R0 + "|" + Messages_R_1 + "|" + Messages_R_2 + "|" + Messages_R_3 + "|" + Messages_R_4 + "|" + Messages_R_5 + "|" + Messages_R_6 + "|" + Messages_R_7 + "|";

                                        BinaryFiles.AppendLine(MSG_R);

                                        break;
                                    case "M":      // Test/MIC record (M)

                                        MSG_M = String_read_line;
                                        BinaryFiles.AppendLine(MSG_M);

                                        break;
                                    case "Q":      // Trace record (Q)
                                        break;
                                    case "C":      // Comment record (C)
                                        break;
                                    case "F":      // Free text record (F)
                                        break;
                                    case "A":      // Request record (A)
                                        break;
                                    case "L":      // End Record (L)

                                        MSG_L = String_read_line;
                                        BinaryFiles.AppendLine(MSG_L);

                                        break;
                                }
                            }
                        }

                        byte[] buffer = Encoding.Default.GetBytes(BinaryFiles.ToString());

                        objTerminalSETM = new Terminal_SETM();

                        objTerminalSETM.TUBE_RESULT = buffer;      // All result
                        objTerminalSETM.TUBEID = Str_TUBEID;
                        objTerminalSETM.COLONYID = Str_COLONYID;
                        objTerminalSETM.COLONYNUMBER = Str_COLONYNUMBER;

                        objTerminalSETM = objConfig.Update_TUBERESULT(objTerminalSETM);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            cjButton1.Visible = true;
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
        private void Clear_text()
        {
            txtNAME.Text = "";
            cjPatnum.Text = "";
            txtPROTOCOL.Text = "";
            txtCOLLECTDATE.Text = "";
            txtPROTOCOL.Text = "";
            txtRECEIVEDATE.Text = "";
            txtLABNO.Text = "";
            txtID.Text = "";
            txt_COLONYID.Text = "";
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                Clear_text();
                cjButton1.Visible = false;
                cjPending.Visible = true;
                lblPending.Visible = true;
                lblComplete.Visible = true;
                lblUnmatch.Visible = false;
                cjComplete.Visible = true;

                dataGridView2.Rows.Clear();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                Clear_text();
                cjButton1.Visible = true;
                cjPending.Visible = false;
                lblPending.Visible = false;
                lblComplete.Visible = false;
                lblUnmatch.Visible = true;
                cjComplete.Visible = false;

                dataGridView2.Rows.Clear();
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                cjButton1.Visible = false;
                cjPending.Visible = false;
                lblPending.Visible = false;
                lblComplete.Visible = false;
                lblUnmatch.Visible = false;
                cjComplete.Visible = false;
            }
        }
        public static DialogResult InputBoxClose(string title, string Str_Unmatchnum)
        {
            // New Form
            Form form = new Form();

            text_Unmatchnum = new TextBox();
            text_Unmatchnum.TextAlign = HorizontalAlignment.Center;
            text_Unmatchnum.MaxLength = 20;

            text_InputProtocol = new TextBox();
            text_InputProtocol.TextAlign = HorizontalAlignment.Center;
            text_InputProtocol.MaxLength = 20;

            form.Text = title;

            Button buttonOk = new Button();
            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.SetBounds(160, 110, 100, 30);
            buttonOk.Font = new Font("Tahoma", 12, FontStyle.Regular);

            Button buttonCancel = new Button();
            buttonCancel.Text = "Close";
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.SetBounds(270, 110, 100, 30);
            buttonCancel.Font = new Font("Tahoma", 12, FontStyle.Regular);

            Label label = new Label();
            label.Text = "Unmatch number:";
            label.SetBounds(12, 36, 372, 13);
            label.Font = new Font("Tahoma", 12, FontStyle.Regular);

            Label label2 = new Label();
            label2.Text = "Protocol number:";
            label2.SetBounds(12, 75, 372, 13);
            label2.Font = new Font("Tahoma", 12, FontStyle.Regular);

            text_Unmatchnum.Text = Str_Unmatchnum;
            text_Unmatchnum.SetBounds(160, 35, 300, 40);
            text_Unmatchnum.Font = new Font("Tahoma", 14, FontStyle.Regular);

            text_InputProtocol.SetBounds(160, 69, 300, 40);
            text_InputProtocol.Font = new Font("Tahoma", 14, FontStyle.Regular);

            label.AutoSize = true;
            label2.AutoSize = true;

            text_Unmatchnum.Anchor = text_Unmatchnum.Anchor | AnchorStyles.Right;
            text_InputProtocol.Anchor = text_InputProtocol.Anchor | AnchorStyles.Right;

            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(500, 200);

            form.Controls.AddRange(new Control[] { label, label2, text_InputProtocol, text_Unmatchnum, buttonOk, buttonCancel });

            form.ClientSize = new Size(Math.Max(500, label.Right + 10), form.ClientSize.Height);

            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            return dialogResult;
        }
        private void button1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loaddata_DatagridRescontrol();
        }
        private void dataGridView_Rescontrol_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.contextMenuStrip1.Show(this.dataGridView_Rescontrol, e.Location);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void dataGridView_Complete_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView_Complete.Rows[e.RowIndex];

                objTerminalGETM = new Terminal_GETM();

                Str_PROTOCOLNUM_COMPLETE = row.Cells[0].Value.ToString();
                Str_PATNUM_COMPLETE = row.Cells[1].Value.ToString();
                Str_TUBEID = row.Cells[5].Value.ToString();

                dataGridView2.Rows.Clear();

                Query_TUBERESULT(Str_PROTOCOLNUM_COMPLETE, Str_PATNUM_COMPLETE, Str_TUBEID);

                tabControl1.SelectedIndex = 2;

                Str_PROTOCOLNUM_COMPLETE = "";
                Str_PATNUM_COMPLETE = "";
                Str_TUBEID = "";
            }
        }
        private void dataGridView_Complete_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (dataGridView_Complete.Rows.Count > 0)
                    {
                        if (e.RowIndex > -1)
                        {
                            this.dataGridView_Complete.Rows[e.RowIndex].Selected = true;
                            rowindex = e.RowIndex;
                            this.dataGridView_Complete.CurrentCell = this.dataGridView_Complete.Rows[e.RowIndex].Cells[0];

                            DataGridViewRow row = this.dataGridView_Complete.Rows[e.RowIndex];


                            string[] Sub_sentences = { row.Cells[0].Value.ToString() };
                            string sPattern = "-";
                            string Protocol_1 = "";
                            string Colony_1 = "";

                            foreach (string sub in Sub_sentences)
                            {
                                if (System.Text.RegularExpressions.Regex.IsMatch(sub, sPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                                {
                                    string[] One_sentences = sub.Split('-');

                                    Protocol_1 = One_sentences[0].ToString().Trim();
                                    Colony_1 = One_sentences[1].ToString().Trim();

                                    txtPROTOCOL.Text = Protocol_1;
                                }
                            }
                            string[] Array_GetSUBREQUESTID = GetSUBREQUESTID_MultiColony(Protocol_1, Colony_1);
                            // Array_GetSUBREQUESTID
                            // [1] ACCESSNUMBER
                            // [7] PATNAME
                            // [8] LASTNAME
                            // [9] RECEIVEDATE

                            cjPatnum.Text = row.Cells[1].Value.ToString();
                            txtLABNO.Text = Array_GetSUBREQUESTID[1];
                            txtNAME.Text = Array_GetSUBREQUESTID[7] + " " + Array_GetSUBREQUESTID[8];
                            txtRECEIVEDATE.Text = Array_GetSUBREQUESTID[9];
                            txtID.Text = row.Cells[5].Value.ToString();
                            txt_COLONYID.Text = row.Cells[6].Value.ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView3_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (dataGridView3.Rows.Count > 0)
                    {
                        if (e.RowIndex > -1)
                        {
                            this.dataGridView3.Rows[e.RowIndex].Selected = true;
                            rowindex = e.RowIndex;
                            this.dataGridView3.CurrentCell = this.dataGridView3.Rows[e.RowIndex].Cells[0];

                            DataGridViewRow row = this.dataGridView3.Rows[e.RowIndex];

                            txtPROTOCOL.Text = row.Cells[0].Value.ToString();
                            string[] Array_GetSUBREQUESTID = GetSUBREQUESTID(row.Cells[0].Value.ToString());
                            // Array_GetSUBREQUESTID
                            // [1] ACCESSNUMBER
                            // [7] PATNAME
                            // [8] LASTNAME
                            // [9] RECEIVEDATE

                            cjPatnum.Text = row.Cells[1].Value.ToString();
                            txtNAME.Text = Array_GetSUBREQUESTID[7] + " " + Array_GetSUBREQUESTID[8];
                            txtRECEIVEDATE.Text = Array_GetSUBREQUESTID[9];
                            txt_COLONYID.Text = row.Cells[5].Value.ToString();
                            txtLABNO.Text = Array_GetSUBREQUESTID[1];
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

///*
// * SELECT anti . ANTIBIOTICID 
//, anti . CREATEUSER 
//, anti . CREATIONDATE 
//, anti . COMMENTS 
//, anti . LOGUSERID 
//, anti . LOGDATE 
//, anti . UNITS 
//, anti . METHODMBID 
//, method . METHODMBCODE 
//, anti . RESULT 
//, anti . RESULTVALUE 
//, dict_anti . FULLTEXT 
//, sen . SENSITIVITYID 
//, sen . COLONYID 
//, anti . MICID 
//, anti . SUBREQMBSENSITIVITYID 
//, colony . COLONYNUMBER 
//, ANTI . NOTPRINTABLE 
//FROM SUBREQMB_ANTIBIOTICS anti 
//inner join DICT_MB_ANTIBIOTICS dict_anti on anti . ANTIBIOTICID = dict_anti . ANTIBIOTICID 
//join SUBREQMB_SENSITIVITIES sen on sen . SUBREQMBSENSITIVITYID = anti . SUBREQMBSENSITIVITYID 
//join DICT_MB_METHODS method on anti . METHODMBID = method . METHODMBID 
//join SUBREQMB_COLONIES colony on colony . COLONYID = sen . COLONYID 
//where anti . SUBREQUESTID = 25
// * 
