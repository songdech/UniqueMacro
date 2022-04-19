using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddBattery_Mapcode : Form
    {
        private ConfigurationController objConfig = new ConfigurationController();
        private BatteryM objBatteryM;
        private string moduleType;
        private string MaplinkID;
        private string Objectlink;


        public frmAddBattery_Mapcode()
        {
            objBatteryM = new BatteryM();
            InitializeComponent();
        }

        public frmAddBattery_Mapcode(string ModuleType, string MAPLINKID, string OBJECTLINK)
        {
            objBatteryM = new BatteryM();
            InitializeComponent();

            this.MaplinkID = MAPLINKID;
            this.Objectlink = OBJECTLINK;
            this.moduleType = ModuleType;

            if (ModuleType == "Read_Only")
            {
                textBox2.Visible = false;
                textBox4.Visible = false;
                comboBox1.Visible = false;
                button2.Visible = false;
                btn_Import.Visible = false;

                objBatteryM = ControlParameter.BatteryInfoM;
            }
        }
        private void btn_Import_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string fileExt = string.Empty;
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; //get the path of the file  
                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt); //read excel file  
                        dataGridView1.DataSource = dtExcel;
                        textBox2.Text = filePath; //+ fileExt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
                }
            }
        }
        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=Yes';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Close();
                }
            }
            return dtexcel;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                objBatteryM.BATTERY_ID = Battery_Mapcode.BATTERY_ID;
                objBatteryM.MAPCODE_NAME = Battery_Mapcode.MAPCODE_NAME;
                objBatteryM.MAPCODE_LINKID = Battery_Mapcode.MAPCODE_LINKID;

                objBatteryM.LogUserID = ControlParameter.loginID.ToString();

                // ถ้าเท่ากับ 0 แสดงว่ายังไม่พบ Mapcode ให้ Get 0 ไว้ก่อนแล้ว Save Link จะได้ Mapcode ID
                if (textBox4.Text == "0" && dataGridView1.Rows.Count > 0)
                {
                    objBatteryM = objConfig.SaveBattery_Link_Object(objBatteryM);
                    // หลังจาก Save แล้วจะได้ Mapcode ID 
                    textBox4.Text = Convert.ToString(objBatteryM.MAPCODE_LINKID);

                    objBatteryM = objConfig.Update_MAPPINGLINK(objBatteryM);

                }
                else if (textBox4.Text !="0" && dataGridView1.Rows.Count > 0)
                {
                    objBatteryM = objConfig.SaveBattery_Link_Object(objBatteryM);
                    // Update DICT_MB_BATTERY
                    // หลังจากได้ Mapcode ID แล้ว Update 
                    objBatteryM = objConfig.Update_MAPPINGLINK(objBatteryM);
                    // END Insert & Update ID
                }

                // Select index in Mapcode link
                objBatteryM.MAPCODE_OBJECTLINK = Convert.ToString(comboBox1.SelectedIndex);
                objBatteryM.MAPCODE_OBJECTLINKNAME = comboBox1.Text;

                // MAP ID:
                //if (textBox4.Text != "0" && textBox4.Text != "")
                //{
                //    objBatteryM.MAPCODE_LINKID = Convert.ToInt16(textBox4.Text);
                //}
                if (comboBox1.Text != "")
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        objBatteryM = objConfig.Delete_Battery_mappingInGRID(objBatteryM);

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            objBatteryM.MAP_SYSTEMCODE = row.Cells[0].Value.ToString();
                            objBatteryM.MAP_OTHERCODE = row.Cells[1].Value.ToString();
                            objBatteryM.MAP_FULLNAME = row.Cells[2].Value.ToString();
                            objBatteryM = objConfig.Save_MAPCODE_BATTERY(objBatteryM);
                        }

                    }
                }

                this.Close();
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
        private void frmAddBattery_Mapcode_Load(object sender, EventArgs e)
        {
            Loaddata();
            textBox4.Text = Convert.ToString(Battery_Mapcode.MAPCODE_LINKID);
        }

        private void Loaddata()
        {
            if (moduleType == "Read_Only")
            {
                if (objBatteryM.MAPCODE_LINKID != 0)
                {
                    Get_Mapcode_Object(objBatteryM);
                }

                label6.Visible = true;
                label6.Text = "Read only";
            }
        }
        private void Get_Mapcode_Object(BatteryM objBatteryM)
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;

            try
            {
                objBatteryM.MAPCODE_OBJECTLINK = Objectlink;

                dt = objConfig.GetBattery_Objectlink(objBatteryM);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                button2.Enabled = true;
            }

        }
    }
}
