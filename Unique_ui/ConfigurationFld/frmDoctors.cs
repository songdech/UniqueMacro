using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmDoctors : UserControl
    {
        //SqlConnection conn;
        string strDocid = "";
        public DoctorM objDoctorM = null;

        public frmDoctors()
        {
            InitializeComponent();
           
        }

        public string strModule
        {
            get
            {
                return "doctor";
            }
        }
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Visible = false;
        }

        private void txtSearchDoccode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchData();
            }
        }

        private void searchData()
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            DoctorM objDoctor = new DoctorM();

            try
            {
                objDoctor.DoctorCode = txtSearchDoccode.Text;
                objDoctor.DocName = txtSearchDocname.Text; 

                dt = objConfig.GetDoctorSerach(objDoctor);
                
               
                if (dt.Rows.Count > 0)
                {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();
                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();
                }

                dataGridView1.DataSource = dt;
                //dataGridView1.DataMember = "req";

                //MessageBox.Show(ds.Tables["req"].Rows[0][0].ToString());
                //lblDocName.DataBindings.Clear();
                //lblDocName.DataBindings.Add("Text", dataset_Doctors, "DICT_DOCTORS.DOCCODE");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //string sql = @"SELECT DICT_DOCTORS.DOCID, DICT_DOCTORS.DOCCODE , DICT_DOCTORS.DOCNAME ,ADDRESS1,CITY,STATE,POSTALCODE,TELEPHON,TELEPHON2,EMAIL  ,FORMAT(STARTVALIDDATE,'dd-MM-yyyy')  AS 'STARTVALIDDATE'  
            //FROM DICT_DOCTORS WHERE DICT_DOCTORS.DOCCODE like '%" + txtSearchDoccode.Text + "%' AND DICT_DOCTORS.DOCNAME  like '%" + txtSearchDocname.Text + "%' ORDER BY DICT_DOCTORS.DOCID DESC";
            //SqlCommand cmd = new SqlCommand(sql, conn);
            //SqlDataAdapter adap = new SqlDataAdapter(cmd);
            //dataset_Doctors = new ConfigurationFld.Dataset_Doctors();
            //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            //dataset_Doctors.Clear();
            //adap.Fill(dataset_Doctors, "DICT_DOCTORS");
            //bindingSource1.DataSource = dataset_Doctors;
            //bindingSource1.DataMember = "DICT_DOCTORS";
            
            //if (dataset_Doctors.Tables["DICT_DOCTORS"].Rows.Count > 0)
            //{


            //    Action instance = EnableDupButton;
            //    if (instance != null)
            //        instance();
            //    Action instance2 = checkEnableButton;
            //    if (instance2 != null)
            //        instance2();
            //}
          //  dataGridView1.DataSource = ds;
            // dataGridView1.DataMember = "req";

           //MessageBox.Show(ds.Tables["req"].Rows[0][0].ToString());
         //   lblDocName.DataBindings.Clear();
        //    lblDocName.DataBindings.Add("Text", dataset_Doctors, "DICT_DOCTORS.DOCCODE");

        }

        private void doctor_Load(object sender, EventArgs e)
        {
            try
            {
                objDoctorM = new DoctorM();
                searchData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Doctor", MessageBoxButtons.OK);
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ConfigurationFld.frmAddDoctors fm = new ConfigurationFld.frmAddDoctors("Add");
            fm.ShowDialog();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           

            txtSearchDocname.Focus();
            searchData();
          
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
        }

        private void EditDoctor()
        {

            if (dataGridView1.SelectedRows.Count > 0) // make sure user select at least 1 row 
            {
             
             
            }
    

        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            searchData();
        }

        private void txtSearchDocname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchData();
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                strDocid = row.Cells[0].Value.ToString();


                objDoctorM.DocID= Convert.ToInt16(row.Cells[0].Value.ToString());
                objDoctorM.DoctorCode= row.Cells[1].Value.ToString();
                objDoctorM .DocName= row.Cells[2].Value.ToString();
                objDoctorM.Telephone = row.Cells[3].Value.ToString();
                objDoctorM.Email = row.Cells[4].Value.ToString();
                objDoctorM.LogUserID = ControlParameter.ControlUser.USERID.ToString();

                ControlParameter.DoctorInfoM= objDoctorM;

                //ControlDoctor.DoctorID = row.Cells[0].Value.ToString();
                //ControlDoctor.DoctorCode = row.Cells[1].Value.ToString();
                //ControlDoctor.DoctorName = row.Cells[2].Value.ToString();
                //ControlDoctor.DoctorHomePhone = row.Cells[3].Value.ToString();
                //ControlDoctor.DoctorEMail = row.Cells[4].Value.ToString();
                // ControlDoctor.DoctorAddress = row.Cells[3].Value.ToString();
                //  ControlDoctor.DoctorCity = row.Cells[4].Value.ToString();
                //  ControlDoctor.DoctorState = row.Cells[5].Value.ToString();
                //  ControlDoctor.DoctorZipCode = row.Cells[6].Value.ToString();                
                //  ControlDoctor.DoctorMobilePhone = row.Cells[8].Value.ToString();                 
                //  ControlDoctor.DoctorStartVlidate = Convert.ToDateTime( row.Cells[10].Value.ToString());
            }
        }


        public string setText
        {

            set
            {
              //   label2.Text = value;
            }       
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            searchData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellMouseDoubleClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                strDocid = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddDoctors fm = new ConfigurationFld.frmAddDoctors("edit");
                fm.yourAction = searchData;
                fm.ShowDialog();

            }
        }

        private void dataGridView1_RowEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                strDocid = row.Cells[0].Value.ToString();


                objDoctorM.DocID = Convert.ToInt16(row.Cells[0].Value.ToString());
                objDoctorM.DoctorCode = row.Cells[1].Value.ToString();
                objDoctorM.DocName = row.Cells[2].Value.ToString();
                objDoctorM.Telephone = row.Cells[3].Value.ToString();
                objDoctorM.Email = row.Cells[4].Value.ToString();
                objDoctorM.LogUserID = ControlParameter.ControlUser.USERID.ToString();

                ControlParameter.DoctorInfoM = objDoctorM;

            }
        }
    }
}
