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
using UniquePro.DAO.Configurations;
using UniquePro.Entities.Configuration;
using UniquePro.Entities.Common;

namespace UNIQUE.ConfigurationApp
{
    public partial class DBSetting : UserControl
    {

        //SqlConnection conn;
        public DBSetting()
        {
            InitializeComponent();
        }

        public string strModule
        {
            get
            {
                return "DatabaseSetting";
            }
        }
        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private void Locations_Load(object sender, EventArgs e)
        {
            //conn = new ConnectionString().Connect();
            searchData();
            Action instance2 = checkEnableButton;
            if (instance2 != null)
                instance2();
        }

        private void searchData()
        {

            ConnectionStringMasterDAO objConfig = new ConnectionStringMasterDAO();

            DataTable dt = null;
            ConnectStringM objConstring = new ConnectStringM();

            try
            {
                dt = objConfig.GetDBConstring();

                if (dt.Rows.Count > 0)
                {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();
                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();
                }

                dvConnectionString.DataSource = dt;
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
                DataGridViewRow row = this.dvConnectionString.Rows[e.RowIndex];

                ControlParameter.ControlConString.CONANME = row.Cells[1].Value.ToString();
                ControlParameter.ControlConString.CONPATH = row.Cells[2].Value.ToString();
                ControlParameter.ControlConString.CONDATASOURCE = row.Cells[3].Value.ToString();
                ControlParameter.ControlConString.CONCATALOG = row.Cells[4].Value.ToString();
                ControlParameter.ControlConString.CONUSER = row.Cells[5].Value.ToString();
                ControlParameter.ControlConString.CONPASS = row.Cells[6].Value.ToString();

            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dvConnectionString.Rows[e.RowIndex];
                string strLocid = row.Cells[1].Value.ToString();
                ConfigurationApp.frmDatabaseSetting fm = new ConfigurationApp.frmDatabaseSetting("EDIT", strLocid);
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
    }
}
