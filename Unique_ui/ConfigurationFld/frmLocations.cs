using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmLocations : UserControl
    {
        public LocationM objLocationM = null;

        public frmLocations()
        {
            InitializeComponent();
        }

        public string strModule
        {
            get
            {
                return "location";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private void Locations_Load(object sender, EventArgs e)
        {
            objLocationM = new LocationM();            
            searchData();
           // Action instance2 = checkEnableButton;
            //if (instance2 != null)
            //    instance2();
        }

        private void searchData()
        {
            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            LocationM objLocation = new  LocationM();

            try
            {
                objLocation.LocCode= txtSearchDoccode.Text;
                objLocation.LocName = txtSearchDocname.Text;

                dt = objConfig.GetLocationSearch (objLocation);
             
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
                dataGridView1.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                
                objLocationM.LocID = Convert.ToInt16 (row.Cells[1].Value.ToString());
                objLocationM.LocCode = row.Cells[0].Value.ToString();
                objLocationM.LocName= row.Cells[2].Value.ToString();
                objLocationM.Telephone = row.Cells[8].Value.ToString();
                objLocationM.Email = row.Cells[11].Value.ToString();
                objLocationM.LogUserID = ControlParameter.ControlUser.USERID.ToString ();

                ControlParameter.LocationInfoM = objLocationM;

            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
               string strLocid  = row.Cells[1].Value.ToString();
                ConfigurationFld.AddLocations fm = new ConfigurationFld.AddLocations("edit", strLocid);
                fm.yourAction = searchData;
                fm.ShowDialog();

            }
        }

        private void txtSearchDoccode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchData();
            }
        }

        private void txtSearchDocname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchData();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            searchData();
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }       
}
