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
    public partial class SystemMGNT : UserControl
    {

        ControlID objControlID = null;
        private ConfigurationController objConfiguration = new ConfigurationController();


        int Numlength;
        int Minnumber;
        int Maxnumber;

        public Action refreshData { get; set; }

        public SystemMGNT()
        {
            InitializeComponent();
        }

        public string strModule
        {
            get
            {
                return "SystemMGNT";
            }
        }

        public Action checkEnableButton { get; set; }
        public Action EnableDupButton { get; set; }

        private void frmSystemMGNT_Load(object sender, EventArgs e)
        {
            objControlID = new ControlID();

            txtControlID.Select();
            LoadData();
        }

        private void LoadData()
        {

            ConfigurationController objConfigcontrolID = new ConfigurationController();
            DataTable dt = null;
            ControlID objControlID = new ControlID();
            //DoctorM objDoctor = new DoctorM();

            try
            {
                objControlID.CONFID = txtControlID.Text;
                objControlID.SHORTTEXT = txtShorttext.Text;

                dt = objConfigcontrolID.GetControlID(objControlID);

                dataGridView1.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    Action instance = EnableDupButton;
                    if (instance != null)
                        instance();
                    Action instance2 = checkEnableButton;
                    if (instance2 != null)
                        instance2();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objControlID = null;
            }
        }


        private void txtIDPatnumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtMinnumber_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (txtNumlength.Text != "")
            {
                Numlength = Convert.ToInt32(txtNumlength.Text);
                txtMinnumber.MaxLength = Numlength;
            }
            else
            {
                txtMinnumber.Text = "";
                MessageBox.Show("Please insert Number length first");
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtMaxnumber_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (txtNumlength.Text != "" && txtMinnumber.Text !="")
            {
                Numlength = Convert.ToInt32(txtNumlength.Text);
                txtMaxnumber.MaxLength = Numlength;
            }
            else
            {
                MessageBox.Show("Please insert Numberlength && Minnumber first");
                txtMaxnumber.Text = "";
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtControlID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }

        private void txtShorttext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }
        private void txtNumlength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                txtControlID.Text = row.Cells[0].Value.ToString();
                txtMinnumber.Text = row.Cells[2].Value.ToString();
                txtMaxnumber.Text = row.Cells[3].Value.ToString();
                txtShorttext.Text = row.Cells[4].Value.ToString();
                txtFulltext.Text = row.Cells[5].Value.ToString();
                txtDescription.Text = row.Cells[6].Value.ToString();
                txtNumlength.Text = row.Cells[9].Value.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtControlID.Text != "" && txtNumlength.Text !="" && txtMinnumber.Text !="" && txtMaxnumber.Text !="" && txtShorttext.Text !="")
                {
                    objControlID = new ControlID();

                    objControlID.CONFID = txtControlID.Text;
                    objControlID.NUMLENGTH = txtNumlength.Text;
                    objControlID.CONF_MINNUMBER = txtMinnumber.Text;
                    objControlID.CONF_MAXNUMBER = txtMaxnumber.Text;
                    objControlID.SHORTTEXT = txtShorttext.Text;
                    objControlID.FULLTEXT = txtFulltext.Text;
                    objControlID.DESCRIPTION = txtDescription.Text;

                    objControlID.LOGUSERID = ControlParameter.loginID.ToString();

                    Minnumber = Convert.ToInt32(txtMinnumber.Text);
                    Maxnumber = Convert.ToInt32(txtMaxnumber.Text);

                    if (Minnumber < Maxnumber)
                    {
                        if (txtMinnumber.Text.Length != Numlength && txtMaxnumber.Text.Length != Numlength)
                        {
                            MessageBox.Show("Min number not correct in Numberlength Please check!");
                        }
                        else
                        {
                            objControlID = objConfiguration.SaveControlID(objControlID);
                            CleanScreen();
                            LoadData();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Min number more than Max number Please check!");
                    }
                }
                else
                {
                    MessageBox.Show("Please Fill data first");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc : Control ID Save Function " + ex.Message);
            }
        }
        private void CleanScreen()
        {
            txtControlID.Select();
            txtControlID.Text="";
            txtMinnumber.Text = "";
            txtMaxnumber.Text = "";
            txtShorttext.Text = "";
            txtFulltext.Text = "";
            txtDescription.Text = "";
            txtNumlength.Text = "";
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtControlID.Text != "")
                {
                    objControlID.CONFID = txtControlID.Text;
                    objControlID.NUMLENGTH = txtNumlength.Text;
                    objControlID.CONF_MINNUMBER = txtMinnumber.Text;
                    objControlID.CONF_MAXNUMBER = txtMaxnumber.Text;
                    objControlID.SHORTTEXT = txtShorttext.Text;
                    objControlID.FULLTEXT = txtFulltext.Text;
                    objControlID.DESCRIPTION = txtDescription.Text;

                    objControlID.LOGUSERID = ControlParameter.loginID.ToString();

                    DialogResult result = MessageBox.Show("You want delete Control number", "Delete control number!", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        objControlID = objConfiguration.DeleteControlID(objControlID);

                        CleanScreen();
                        LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Please select: ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc : Control ID Delete Function " + ex.Message);
            }

        }

        private void txtNumlength_TextChanged(object sender, EventArgs e)
        {
            txtMinnumber.Text = "";
            txtMaxnumber.Text = "";
        }
    }
}
