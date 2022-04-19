using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using UniquePro.Controller;

namespace UNIQUE.Result
{
    public partial class frmIdentificationAndSensititities : Form
    {
        SqlConnection conn;
        private DataTable dtOrganism; 

        public frmIdentificationAndSensititities()
        {
            InitializeComponent();
        }

        private void frmIdentificationAndSensititities_Load(object sender, EventArgs e)
        {
            try
            {

                BeginingData();           
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
            //conn = new ConnectionString().Connect();
            // TODO: This line of code loads data into the 'dataSet_Culture.DICT_MB_ANTIBIOTICS' table. You can move, or remove it, as needed.
            //this.dICT_MB_ANTIBIOTICSTableAdapter.Fill(this.dataSet_Culture.DICT_MB_ANTIBIOTICS);

        }

        private void BeginingData()
        {
            ConfigurationController objConfiration = new ConfigurationController();

            try
            {
                dtOrganism = objConfiration.GetDICTOrganisms();
                gridControl1.DataSource = dtOrganism;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            gridView1.AddNewRow();

            int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
            if (gridView1.IsNewItemRow(rowHandle))
            {
                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[0], "a");
               // gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], val2);
              //  gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], val3);
            }
        }
    }
}
