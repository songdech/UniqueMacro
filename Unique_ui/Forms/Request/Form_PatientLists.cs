using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Patient;

namespace UNIQUE.Forms.Request
{
    public partial class Form_PatientLists : Form
    {
        private string textEdit_search;

        //private PatientM objPatientM;
        private ConfigurationController objConfiguration = new ConfigurationController();

        public Form_PatientLists(string textEdit_search)
        {
            InitializeComponent();
            this.textEdit_search = textEdit_search;
        }

        private void Form_PatientLists_Load(object sender, EventArgs e)
        {
            LoadPatient();
            PatientM objPatientM = new PatientM();
        }
        private void LoadPatient()
        {
            PatientController objPatient = new PatientController();
            DataTable dt = null;
            PatientM objPatientM = new PatientM();

            try
            {
                objPatientM.PatientNo = "%" + textEdit_search + "%";

                dt = objPatient.GetPatientNumber_(objPatientM);

                if (dt.Rows.Count != 0)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                this.Close();
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                PatientM objpatientM = new PatientM();
                DataGridViewRow row_1 = this.dataGridView1.Rows[e.RowIndex];
                objpatientM.PatientNo = row_1.Cells[0].Value.ToString();
                ControlParameter.PatientM = objpatientM;
            }
        }
    }
}
