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
using UniquePro.Entities;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmColony : UserControl
    {
        private ConfigurationController objConfiguration = new ConfigurationController();
        //private MediaM objMedia = new MediaM();
        private ColonyDescM objColony = new ColonyDescM();


        public frmColony()
        {
            InitializeComponent();
        }

        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private void frmColony_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }

            catch (Exception ex)

            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
        }

        public string strModule
        {
            get
            {
                return "colonydesciption";
            }

        }

        private void LoadData()
        {
            DataTable dt = null;
            try
            {
                objColony.ColonyCode= txtSearchCode.Text;
                objColony.ColonyDescription  = txtSearchName.Text;

                dt = objConfiguration.GetColonyDesc (objColony.ColonyCode, objColony.ColonyDescription);

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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                LoadData();
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

                objColony.ColonyDescID= Convert.ToInt16(row.Cells[0].Value.ToString());
                objColony.ColonyCode = row.Cells[1].Value.ToString();
                objColony.ColonyDescription = row.Cells[2].Value.ToString();

                ControlParameter.ColonyDesc = objColony;

            }
        }

       

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string strMediaID = row.Cells[0].Value.ToString();
                ConfigurationFld.frmAddColony fm = new ConfigurationFld.frmAddColony("edit", strMediaID);
                fm.refreshData = LoadData;
                fm.ShowDialog();
            }
        }
    }
}
