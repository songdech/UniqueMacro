using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddBattery : Form
    {
        private string moduleType;
        private string batteryID;

        string Str_MaplinkID = "";
        string Str_ObjectLink = "";

        private ConfigurationController objConfig = new ConfigurationController();
        private BatteryM objBatteryM;

        private bool CheckMap = false;

        public Action refreshData { get; set; }
        public frmAddBattery(string moduleType, string BatteryID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.batteryID = BatteryID;
            objBatteryM = new BatteryM();
            if (moduleType != "add")
            {
                objBatteryM = ControlParameter.BatteryInfoM;
            }
        }

        private void frmAddBattery_Load(object sender, EventArgs e)
        {
            Loaddata();

            if (moduleType == "edit")
            {
                checkBox1.Enabled = true;

                if (checkBox1.Checked == true)
                {
                    CheckMap = true;
                }
                else if (checkBox1.Checked == false)
                {
                    CheckMap = false;
                }
            }
        }

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                if (txtBattery_code.Text != "")
                {
                    if (moduleType == "edit")
                    {
                        objBatteryM.BATTERY_ID = Convert.ToInt16(textBox1.Text);
                        objBatteryM.BATTERY_CODE = txtBattery_code.Text;
                        objBatteryM.BATTERY_NAME = txtBattery_name.Text;
                        objBatteryM.BATTERY_SHORTNAME = txtShortname.Text;
                        objBatteryM.BATTERY_COMMENT = txtBattery_comment.Text;
                        objBatteryM.MAPCODE_LINKID = Convert.ToInt16(textBox4.Text);

                        objBatteryM.LogUserID = ControlParameter.loginID.ToString();

                        objBatteryM = objConfig.SaveBatteryM(objBatteryM);
                    }
                    else if (moduleType == "dup")
                    {
                        objBatteryM.BATTERY_CODE = txtBattery_code.Text;
                        objBatteryM.BATTERY_NAME = txtBattery_name.Text;
                        objBatteryM.BATTERY_SHORTNAME = txtShortname.Text;
                        objBatteryM.LogUserID = ControlParameter.loginID.ToString();
                        objBatteryM.BATTERY_COMMENT = txtBattery_comment.Text;
                        objBatteryM.MAPCODE_LINKID = 0;

                        objBatteryM = objConfig.SaveBatteryM(objBatteryM);
                    }
                    else if (moduleType == "add")
                    {
                        objBatteryM.BATTERY_CODE = txtBattery_code.Text;
                        objBatteryM.BATTERY_NAME = txtBattery_name.Text;
                        objBatteryM.BATTERY_SHORTNAME = txtShortname.Text;
                        objBatteryM.LogUserID = ControlParameter.loginID.ToString();
                        objBatteryM.BATTERY_COMMENT = txtBattery_comment.Text;
                        objBatteryM.MAPCODE_LINKID = 0;

                        objBatteryM = objConfig.SaveBatteryM(objBatteryM);
                    }

                    this.Close();
                    Action instance = refreshData;
                    if (instance != null)
                        instance();
                }
                else
                {
                    MessageBox.Show("Please Fill data first");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc : Battery Save Function " + ex.Message);
            }
        }

        private void Loaddata()
        {
            if (moduleType == "edit")
            {
                textBox1.Text = Convert.ToString(objBatteryM.BATTERY_ID);
                textBox4.Text = Convert.ToString(objBatteryM.MAPCODE_LINKID);
                txtBattery_code.Text = objBatteryM.BATTERY_CODE;
                txtBattery_name.Text = objBatteryM.BATTERY_NAME;
                txtShortname.Text = objBatteryM.BATTERY_SHORTNAME;
                txtBattery_comment.Text = objBatteryM.BATTERY_COMMENT;

                if (objBatteryM.MAPCODE_LINKID != 0)
                {
                    Get_Battery_Mapcode_name(objBatteryM);
                }
                else
                {
                    textBox3.Text = "No mapping code";
                }

                label6.Visible = true;
                label6.Text = "Edit Battery";
                this.Text = "Edit Battery";
            }
            else if (moduleType == "dup")
            {
                txtBattery_code.Select();
                txtBattery_name.Text = objBatteryM.BATTERY_NAME;
                txtShortname.Text = objBatteryM.BATTERY_SHORTNAME;
                txtBattery_comment.Text = objBatteryM.BATTERY_COMMENT;

                label6.Visible = true;
                label6.Text = "Duplicate Battery";

                this.Text = "Duplicate Battery";
            }
            else if (moduleType == "add")
            {
                txtBattery_code.Select();
                label6.Visible = true;
                label6.Text = "Add Battery";

                this.Text = "Add Battery";
            }
        }
        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // DataGrid MAPPING CODE VIA INSTRUMENT
                // Check mapping Checkbox & MAPPING NAME TextBox3

                if (checkBox1.Checked == true && textBox3.Text != "")
                {
                    // Insert ID in CODE_MAPPING_LINKS

                    Battery_Mapcode.BATTERY_ID = Convert.ToInt16(textBox1.Text);
                    Battery_Mapcode.MAPCODE_NAME = textBox3.Text;
                    Battery_Mapcode.MAPCODE_LINKID = Convert.ToInt16(textBox4.Text);

                    frmAddBattery_Mapcode FM_MAPCODE = new frmAddBattery_Mapcode();
                    FM_MAPCODE.ShowDialog();


                    // Update DICT_MB_BATTERY
                    // หลังจากได้ Mapcode ID แล้ว Update 
                    //objBatteryM = objConfig.Update_MAPPINGLINK(objBatteryM);
                    // END Insert & Update ID

                    // Select index in Mapcode link
                    //objBatteryM.MAPCODE_OBJECTLINK = Convert.ToString(comboBox1.SelectedIndex);
                    //objBatteryM.MAPCODE_OBJECTLINKNAME = comboBox1.Text;

                    // MAP ID:
                    //if (textBox4.Text != "0" && textBox4.Text != "")
                    //{
                    //    objBatteryM.MAPCODE_LINKID = Convert.ToInt16(textBox4.Text);
                    //    objBatteryM = objConfig.Delete_Battery_mappingInGRID(objBatteryM);
                    //}

                    //if (dataGridView1.Rows.Count > 0)
                    //{
                    //    foreach (DataGridViewRow row in dataGridView1.Rows)
                    //    {
                    //        objBatteryM.MAP_SYSTEMCODE = row.Cells[0].Value.ToString();
                    //        objBatteryM.MAP_OTHERCODE = row.Cells[1].Value.ToString();
                    //        objBatteryM.MAP_FULLNAME = row.Cells[2].Value.ToString();
                    //        objBatteryM = objConfig.Save_MAPCODE_BATTERY(objBatteryM);
                    //    }

                    //}
                    //else if (dataGridView1.Rows.Count == 0)
                    //{
                    //    // Clear Mapping 
                    //    objBatteryM = objConfig.Delete_MAPPINGLINK(objBatteryM);
                    //    objBatteryM = objConfig.Update_MAPPINGLINK_Default(objBatteryM);
                    //}
                }
                else
                {
                    MessageBox.Show("Please fill Mapcode name...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                objBatteryM = null;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                button1.Enabled = true;
                textBox3.Enabled = true;
                groupControl5.Enabled = true;

                CheckMap = true;
            }
            else if (checkBox1.Checked == false)
            {
                button1.Enabled = false;
                textBox3.Enabled = false;
                groupControl5.Enabled = false;

                CheckMap = false;
            }
        }
        int rowindex = 0;
        bool DelRowDatagrid = false;
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        if (e.RowIndex > -1)
                        {
                            this.dataGridView1.Rows[e.RowIndex].Selected = true;
                            rowindex = e.RowIndex;
                            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[3];
                            this.contextMenuStrip_G1.Show(this.dataGridView1, e.Location);

                            DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                            Str_MaplinkID = row.Cells[0].Value.ToString();
                            Str_ObjectLink = row.Cells[1].Value.ToString();

                            DelRowDatagrid = true;
                            contextMenuStrip_G1.Show(Cursor.Position);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.dataGridView1.Rows[this.rowindex].IsNewRow)
                {
                    this.dataGridView1.Rows.RemoveAt(this.rowindex);

                    objBatteryM.MAPCODE_LINKID = Convert.ToInt16(Str_MaplinkID);
                    objBatteryM.MAPCODE_OBJECTLINK = Str_ObjectLink;

                    objBatteryM = objConfig.Delete_Battery_mappingInGRID(objBatteryM);
                    // Clear Mapping 

                    if (dataGridView1.Rows.Count == 0)
                    {
                        MessageBox.Show("Clear all Mapcode?");
                        //    // Clear Mapping 
                        objBatteryM = objConfig.Delete_MAPPINGLINK(objBatteryM);
                        objBatteryM = objConfig.Update_MAPPINGLINK_Default(objBatteryM);
                        this.Close();
                    }

                dataGridView1.Refresh();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    // DataGrid MAPPING CODE VIA INSTRUMENT
            //    // Check mapping Checkbox & MAPPING NAME TextBox3

            //    objBatteryM.BATTERY_ID = Convert.ToInt16(textBox1.Text);

            //    if (checkBox1.Checked == true && textBox3.Text != "")
            //    {
            //        // Insert ID in CODE_MAPPING_LINKS
            //        objBatteryM.MAPCODE_NAME = textBox3.Text;

            //        // ถ้าเท่ากับ 0 แสดงว่ายังไม่พบ Mapcode ให้ Get 0 ไว้ก่อนแล้ว Save Link จะได้ Mapcode ID
            //        if (textBox4.Text == "0")
            //        {
            //            objBatteryM.MAPCODE_LINKID = Convert.ToInt16(textBox4.Text);
            //            objBatteryM = objConfig.SaveBattery_Link_Object(objBatteryM);

            //            // หลังจาก Save แล้วจะได้ Mapcode ID 
            //            textBox4.Text = Convert.ToString(objBatteryM.MAPCODE_LINKID);
            //        }
            //        else
            //        {
            //            objBatteryM = objConfig.SaveBattery_Link_Object(objBatteryM);
            //        }

            //        // Update DICT_MB_BATTERY
            //        // หลังจากได้ Mapcode ID แล้ว Update 
            //        objBatteryM = objConfig.Update_MAPPINGLINK(objBatteryM);
            //        // END Insert & Update ID

            //        // Select index in Mapcode link
            //        objBatteryM.MAPCODE_OBJECTLINK = Convert.ToString(comboBox1.SelectedIndex);
            //        objBatteryM.MAPCODE_OBJECTLINKNAME = comboBox1.Text;

            //        // MAP ID:
            //        //if (textBox4.Text != "0" && textBox4.Text != "")
            //        //{
            //        //    objBatteryM.MAPCODE_LINKID = Convert.ToInt16(textBox4.Text);
            //        //    objBatteryM = objConfig.Delete_Battery_mappingInGRID(objBatteryM);
            //        //}

            //        if (dataGridView1.Rows.Count > 0)
            //        {
            //            foreach (DataGridViewRow row in dataGridView1.Rows)
            //            {
            //                objBatteryM.MAP_SYSTEMCODE = row.Cells[0].Value.ToString();
            //                objBatteryM.MAP_OTHERCODE = row.Cells[1].Value.ToString();
            //                objBatteryM.MAP_FULLNAME = row.Cells[2].Value.ToString();
            //                objBatteryM = objConfig.Save_MAPCODE_BATTERY(objBatteryM);
            //            }
                        
            //        }
            //        //else if (dataGridView1.Rows.Count == 0)
            //        //{
            //        //    // Clear Mapping 
            //        //    objBatteryM = objConfig.Delete_MAPPINGLINK(objBatteryM);
            //        //    objBatteryM = objConfig.Update_MAPPINGLINK_Default(objBatteryM);
            //        //}
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please fill Mapcode name...");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    objBatteryM = null;
            //}
        }
        private void Get_Battery_Mapcode_name(BatteryM objBatteryM)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;

            try
            {
                objBatteryM.MAPCODE_LINKID = Convert.ToInt16(textBox4.Text);

                dt = objConfig.GetBattery_Mapping_name(objBatteryM);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = dt.Rows[i]["MAPLINKID"].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = dt.Rows[i]["OBJECTLINK"].ToString();
                        dataGridView1.Rows[i].Cells[2].Value = dt.Rows[i]["LINKNAME"].ToString();
                        dataGridView1.Rows[i].Cells[3].Value = dt.Rows[i]["OBJECTLINKNAME"].ToString();
                    }
                    textBox3.Text = dt.Rows[0]["LINKNAME"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objBatteryM = null;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string MAPLINKID = row.Cells[0].Value.ToString();
                string OBJECTLINK = row.Cells[1].Value.ToString();
                frmAddBattery_Mapcode fm_mapcode = new frmAddBattery_Mapcode("Read_Only", MAPLINKID, OBJECTLINK);
                fm_mapcode.ShowDialog();
            }
        }

        private void btn_Activate_Click(object sender, EventArgs e)
        {
            try
            {
                String Str_OTHERCODE = "";
                String Str_SYSTEMCODE = "";

                //String Str_Antibiotic = "";
                //String Str_Organism = "";
                //String Str_Detection = "";
                //String Str_Resistance_Markers = "";
                //String Str_Resistance_Marker_result = "";


                ConfigurationController objConfig = new ConfigurationController();
                DataTable dt = null;

                objBatteryM.MAPCODE_LINKID = Convert.ToInt16(textBox4.Text);
                dt = objConfig.Activate_Mapcode(objBatteryM);

                var MyIni = new IniFile("Microscan.ini");

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        // You can also check for the existence of a key like so;
                        //if (!MyIni.KeyExists("DefaultVolume", "ANTIBIOTIC"))
                        //{
                        //    MyIni.Write("DefaultVolume", "100", "Audio");
                        //}

                        string OBJECTLINK_NUMBER = dt.Rows[i]["OBJECTLINK"].ToString();
                        Str_OTHERCODE = dt.Rows[i]["OTHERCODE"].ToString();
                        Str_SYSTEMCODE = dt.Rows[i]["SYSTEMCODE"].ToString();

                        switch (OBJECTLINK_NUMBER)
                        {
                            case "0":
                                MyIni.Write(Str_OTHERCODE, Str_SYSTEMCODE, "ANTIBIOTIC");
                                break;
                            case "1":
                                MyIni.Write(Str_OTHERCODE,Str_SYSTEMCODE, "ORGANISM");
                                break;
                            case "2":
                                MyIni.Write(Str_OTHERCODE, Str_SYSTEMCODE, "DETECTION");
                                break;
                            case "3":
                                MyIni.Write(Str_OTHERCODE, Str_SYSTEMCODE, "RESISTANCE_MARKERS");
                                break;
                            case "4":
                                MyIni.Write(Str_OTHERCODE, Str_SYSTEMCODE, "RESISTANCE_MARKER_RESULT");
                                break;
                        }
                    }
                }
            }
                catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        class IniFile
        {
            string Path;
            string EXE = Assembly.GetExecutingAssembly().GetName().Name;

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

            public IniFile(string IniPath = null)
            {
                Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
            }

            public string Read(string Key, string Section = null)
            {
                var RetVal = new StringBuilder(255);
                GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
                return RetVal.ToString();
            }

            public void Write(string Key, string Value, string Section = null)
            {
                WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
            }

            public void DeleteKey(string Key, string Section = null)
            {
                Write(Key, null, Section ?? EXE);
            }

            public void DeleteSection(string Section = null)
            {
                Write(null, null, Section ?? EXE);
            }

            public bool KeyExists(string Key, string Section = null)
            {
                return Read(Key, Section).Length > 0;
            }
        }
    }
}
