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
using UniquePro.DAO.GeneralSetting;
using UniquePro.Entities.GeneralSetting;

namespace UNIQUE.GeneralSetting
{
    public partial class UserMaster : UserControl
    {

        //SqlConnection conn;
        public UserMaster()
        {
            InitializeComponent();
        }

        public string strModule
        {
            get
            {
                return "USerMaster";
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
            UserDAO objUserDAO = new UserDAO();
            DataTable dt = null;
            UserM objUser = new UserM();

            try
            {
                objUser.USERNAME = txtSearchUserName.Text;
                objUser.NAME = txtSearchFirstName.Text;

                dt = objUserDAO.GetUserMasterList(objUser);

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
                dataGridView1.AutoGenerateColumns = false;
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
                ControlParameter.ControlUser.USERID = row.Cells["USERID"].Value.ToString();
                ControlParameter.ControlUser.NAME = row.Cells["NAME"].Value.ToString();
                ControlParameter.ControlUser.LASTNAME = row.Cells["LASTNAME"].Value.ToString();
                ControlParameter.ControlUser.USERNAME = row.Cells["USERNAME"].Value.ToString();
                ControlParameter.ControlUser.PASSWORD = row.Cells["PASSWORD"].Value.ToString();
                ControlParameter.ControlUser.Email = row.Cells["EMAIL"].Value.ToString();
                ControlParameter.ControlUser.TELEPHON = row.Cells["TELEPHON"].Value.ToString();
                ControlParameter.ControlUser.POSITION = row.Cells["POSITION"].Value.ToString();
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string StrUserID = row.Cells[0].Value.ToString();
                GeneralSetting.frmUserMaster fm = new GeneralSetting.frmUserMaster("EDIT", StrUserID);
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

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row;
            row = e.RowIndex;

            if (e.ColumnIndex == 0)
            {

                string StrUserID = dataGridView1.Rows[row].Cells["USERID"].Value.ToString();

                ControlParameter.ControlUser.USERID = dataGridView1.Rows[row].Cells["USERID"].Value.ToString();
                ControlParameter.ControlUser.NAME = dataGridView1.Rows[row].Cells["NAME"].Value.ToString();
                ControlParameter.ControlUser.LASTNAME = dataGridView1.Rows[row].Cells["LASTNAME"].Value.ToString();

                ControlParameter.ControlUser.USERNAME = dataGridView1.Rows[row].Cells["USERNAME"].Value.ToString();
                ControlParameter.ControlUser.PASSWORD = dataGridView1.Rows[row].Cells["PASSWORD"].Value.ToString();

                ControlParameter.ControlUser.Email = dataGridView1.Rows[row].Cells["EMAIL"].Value.ToString();

                ControlParameter.ControlUser.TELEPHON = dataGridView1.Rows[row].Cells["TELEPHON"].Value.ToString();
                ControlParameter.ControlUser.POSITION = dataGridView1.Rows[row].Cells["POSITION"].Value.ToString();


                frmUserMaster fm = new GeneralSetting.frmUserMaster("EDIT", StrUserID);
                fm.yourAction = searchData;
                fm.ShowDialog();
            }
        }
    }
}
