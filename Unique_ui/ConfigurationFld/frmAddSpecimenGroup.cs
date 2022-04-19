using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddSpecimenGroup : Form
    {
        private string moduleType;
        private string specimenID;
        private ConfigurationController objConfiguration = new ConfigurationController();
        private SpecimenG objSpecimenG;

        public Action refreshData { get; set; }
        public frmAddSpecimenGroup(string moduleType, string specimenID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.specimenID = specimenID;
            objSpecimenG = new SpecimenG();
            if (moduleType != "add")
            {
                objSpecimenG = ControlParameter.SpecimenInfoG;                
            }
        }

        private void frmAddSpecimenGroup_Load(object sender, EventArgs e)
        {
            queryData();
        }

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                if (moduleType == "edit")
                {
                    if (txtSPMG_Code.Text != "")
                    {
                        objSpecimenG.SPMG_ID = Convert.ToInt32(txtID.Text);
                        objSpecimenG.SPMG_CODE = txtSPMG_Code.Text;
                        objSpecimenG.SPMG_NAME = txtSPMG_NAME.Text;
                        objSpecimenG.SPMG_LOGUSERID = ControlParameter.loginID.ToString();
                        objSpecimenG.SPMG_FULLTEXT = txtSPMG_Comment.Text;

                        objSpecimenG = objConfiguration.SaveSpecimenG(objSpecimenG);


                        if (dataGridView1.Rows.Count > 0)
                        {
                            objSpecimenG = objConfiguration.DeleteSpecimenListGroup(objSpecimenG);

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                objSpecimenG.SPMG_SPECIMEN_ID = row.Cells[2].Value.ToString();
                                objSpecimenG.SPMG_SPECIMEN_LIST_NAME = row.Cells[1].Value.ToString();
                                //objSpecimenG.SPMG_SPECIMEN_LIST_INDEX = row.Index + 1;
                                objSpecimenG = objConfiguration.SaveSpecimenList(objSpecimenG);
                            }
                        }
                        else
                        {
                            objSpecimenG = objConfiguration.DeleteSpecimenListGroup(objSpecimenG);
                        }
                        this.Close();
                        Action instance = refreshData;
                        if (instance != null)
                            instance();
                    }
                    else
                    {
                        MessageBox.Show("Please Fill data first");
                    }
                }
                else if (moduleType == "dup")
                {
                    if (txtSPMG_Code.Text != "")
                    {
                        objSpecimenG.SPMG_CODE = txtSPMG_Code.Text;
                        objSpecimenG.SPMG_NAME = txtSPMG_NAME.Text;
                        objSpecimenG.SPMG_LOGUSERID = ControlParameter.loginID.ToString();
                        objSpecimenG.SPMG_FULLTEXT = txtSPMG_Comment.Text;

                        objSpecimenG = objConfiguration.SaveSpecimenG(objSpecimenG);

                        if (dataGridView1.Rows.Count > 0)
                        {
                            objSpecimenG = objConfiguration.DeleteSpecimenListGroup(objSpecimenG);

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                objSpecimenG.SPMG_SPECIMEN_ID = row.Cells[4].Value.ToString();
                                objSpecimenG.SPMG_SPECIMEN_LIST_NAME = row.Cells[2].Value.ToString();
                                //objSpecimenG.SPMG_SPECIMEN_LIST_INDEX = row.Index + 1;
                                objSpecimenG = objConfiguration.SaveSpecimenList(objSpecimenG);
                            }
                        }
                        else
                        {
                            objSpecimenG = objConfiguration.DeleteSpecimenListGroup(objSpecimenG);
                        }
                        this.Close();
                        Action instance = refreshData;
                        if (instance != null)
                            instance();
                    }
                    else
                    {
                        MessageBox.Show("Please Fill data first");
                    }


                }
                else if (moduleType == "add")
                {
                    if (txtSPMG_Code.Text != "")
                    {
                        objSpecimenG.SPMG_CODE = txtSPMG_Code.Text;
                        objSpecimenG.SPMG_NAME = txtSPMG_NAME.Text;
                        objSpecimenG.SPMG_LOGUSERID = ControlParameter.loginID.ToString();
                        objSpecimenG.SPMG_FULLTEXT = txtSPMG_Comment.Text;
                        objSpecimenG = objConfiguration.SaveSpecimenG(objSpecimenG);

                        if (dataGridView1.Rows.Count > 0)
                        {
                            objSpecimenG = objConfiguration.DeleteSpecimenListGroup(objSpecimenG);

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                objSpecimenG.SPMG_SPECIMEN_ID = row.Cells[2].Value.ToString();
                                objSpecimenG.SPMG_SPECIMEN_LIST_NAME = row.Cells[1].Value.ToString();
                                //objSpecimenG.SPMG_SPECIMEN_LIST_INDEX = row.Index + 1;
                                objSpecimenG = objConfiguration.SaveSpecimenList(objSpecimenG);
                            }
                        }
                        else
                        {
                            objSpecimenG = objConfiguration.DeleteSpecimenListGroup(objSpecimenG);
                        }
                        this.Close();
                        Action instance = refreshData;
                        if (instance != null)
                            instance();
                    }
                    else
                    {
                        MessageBox.Show("Please Fill data first");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc : Specimen Group Save Function " + ex.Message);
            }
        }

        private void queryData()
        {
            if (moduleType == "edit")
            {
                txtID.Text = Convert.ToString(objSpecimenG.SPMG_ID);
                txtSPMG_Code.Text =  objSpecimenG.SPMG_CODE;
                txtSPMG_NAME.Text = objSpecimenG.SPMG_NAME;
                txtSPMG_Comment.Text = objSpecimenG.SPMG_COMMENT;

                Get_Specimen_in_Dict(txtID.Text);

                label6.Visible = true;
                label6.Text = "Edit Specimen Group";
                this.Text = "Edit Specimen Group";
            }
            else if (moduleType == "dup")
            {
                txtSPMG_Code.Select();
                txtSPMG_NAME.Text = objSpecimenG.SPMG_NAME;
                txtSPMG_Comment.Text = objSpecimenG.SPMG_COMMENT;

                Get_Specimen_in_Dict(Convert.ToString(objSpecimenG.SPMG_ID));


                label6.Visible = true;
                label6.Text = "Duplicate Specimen Group";

                this.Text = "Duplicate Specimen Group";
            }
            else if (moduleType == "add")
            {
                txtSPMG_Code.Select();
                label6.Visible = true;
                label6.Text = "Add Specimen Group";

                this.Text = "Add Specimen Group";
            }

        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }
        private void Get_Specimen_List()
        {

            ConfigurationController objConfigForGridLookupSPMG = new ConfigurationController();
            DataTable dt = null;
            SpecimenM objSpecimenM = new SpecimenM();

            try
            {

                dt = objConfigForGridLookupSPMG.GetSpecimenList(objSpecimenM);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[i].Cells[0].Value = dt.Rows[i]["COLLMATERIALCODE"].ToString();
                        dataGridView2.Rows[i].Cells[1].Value = dt.Rows[i]["COLLMATERIALTEXT"].ToString();
                        dataGridView2.Rows[i].Cells[2].Value = dt.Rows[i]["COLLMATERIALID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objSpecimenM = null;
            }
        }

        private void Get_Specimen_in_Dict(string SPMID)
        {

            ConfigurationController objConfigForGridLookupSPMG = new ConfigurationController();
            DataTable dt = null;

            try
            {

                dt = objConfigForGridLookupSPMG.GetSpecimen_in_Dict(SPMID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = dt.Rows[i]["COLLMATERIALCODE"].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = dt.Rows[i]["COLLMATERIALTEXT"].ToString();
                        dataGridView1.Rows[i].Cells[2].Value = dt.Rows[i]["COLLMATERIALID"].ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Get_Specimen_List();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = dataGridView2.RowCount - 1; i >= 0; i--)
                {
                    DataGridViewRow row = dataGridView2.Rows[i];
                    if (Convert.ToBoolean(row.Cells["Select1"].Value))
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = row.Cells[0].Value;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = row.Cells[1].Value;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = row.Cells[2].Value;

                        dataGridView2.Rows.RemoveAt(row.Index);
                    }
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = dataGridView1.RowCount - 1; i >= 0; i--)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    if (Convert.ToBoolean(row.Cells["Select2"].Value))
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = row.Cells[0].Value;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = row.Cells[1].Value;
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[3].Value = row.Cells[3].Value;
                        dataGridView1.Rows.RemoveAt(row.Index);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var DT = (DataTable)dataGridView2.DataSource;
            var DV = DT.DefaultView;
            DV.RowFilter = string.Format("Colcode like '{0}%'", textBox1.Text);

            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.Rows[0].Selected = true;
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
        }
    }
}
