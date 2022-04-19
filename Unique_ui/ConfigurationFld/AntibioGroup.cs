using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Data.SqlClient;

using UniquePro.Controller;

namespace UNIQUE.ConfigurationFld
{
    public partial class AntibioGroup : UserControl
    {

        private ConfigurationController objConfiguration = new ConfigurationController();

        public AntibioGroup()
        {
            InitializeComponent();
        }
        public string strModule
        {
            get
            {
                return "antibiogroup";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private void AntibioGroup_Load(object sender, EventArgs e)
        {
            //conn = new ConnectionString().Connect();
            LoadData();
            Action instance2 = checkEnableButton;
            if (instance2 != null)
                instance2();
        }

        private void LoadData()
        {
            DataTable dt;

            try
            {
                dt =  objConfiguration.GetDictAniboticsGroupData(txtSearchAnticode.Text , txtSearchAntiname.Text );

                bindingSource1.DataSource = dt;                

                if (dt.Rows.Count > 0)
                {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();
                }

                Action instance2 = checkEnableButton;
                if (instance2 != null)
                    instance2();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //private void LoadData()
        //{
        //    string sql = @"SELECT ANTIBIOTICSFAMILYID,ANTIBIOTICSFAMILYCODE,FAMNAME,STARTVALIDDATE
        //                    FROM DICT_MB_ANTIBIOTIC_GROUP WHERE ANTIBIOTICSFAMILYCODE Like '%" + txtSearchDoccode.Text + "%' AND FAMNAME like '%"+txtSearchDocname.Text+"%'   ORDER BY ANTIBIOTICSFAMILYCODE DESC";
        //    SqlCommand cmd = new SqlCommand(sql, conn);
        //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //    ConfigurationFld.DataSet_Antibiotic_Group ds = new ConfigurationFld.DataSet_Antibiotic_Group();
        //    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
        //    ds.Clear();
        //    adp.Fill(ds, "result");
        //    bindingSource1.DataSource = ds;
        //    bindingSource1.DataMember = "result";
        //    if (ds.Tables["result"].Rows.Count > 0)
        //    {


        //        Action instance = EnableDupButton;
        //        if (instance != null)
        //            instance();

        //    }
        //    Action instance2 = checkEnableButton;
        //    if (instance2 != null)
        //        instance2();
         
         
        //}

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                ControlAntibioticFam.AntibioticFamID = row.Cells[0].Value.ToString();
                ControlAntibioticFam.AntibioticFamCode = row.Cells[1].Value.ToString();
                ControlAntibioticFam.AntibioticFamName = row.Cells[2].Value.ToString();
                ControlAntibioticFam.AntibioticFamUseDate = Convert.ToDateTime(row.Cells[3].Value.ToString());

            }
        }

        private void dataGridView1_Enter(object sender, EventArgs e)
        {
           
        }

        private void txtSearchDoccode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }

        private void txtSearchDocname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }                
        }
    }
}
