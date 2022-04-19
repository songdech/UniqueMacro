using System;
using System.Data;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddSpecimen : Form
    {

        object SPM01;   //SpecimenG ID
        object SPM02;   //SpecimenG Name
        object SPM03;   //SpecimenG Code

        private string moduleType;
        private string specimenID;

        private ConfigurationController objConfig = new ConfigurationController();
        private SpecimenM objSpecimenM;

        public Action refreshData { get; set; }
        public frmAddSpecimen(string moduleType, string specimenID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.specimenID = specimenID;
            objSpecimenM = new SpecimenM();
            if (moduleType != "add")
            {
                objSpecimenM = ControlParameter.SpecimenInfoM;                
            }
            
        }
        private void frmAddSpecimen_Load(object sender, EventArgs e)
        {
            Loaddata();
        }

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                if (moduleType == "edit")
                {
                    objSpecimenM.COLLMATERIALCODE = txtCOLLMATERIALCODE.Text;
                    objSpecimenM.COLLMATERIALTEXT = txtCOLLMATERIALTEXT.Text;
                    objSpecimenM.COLLMATERIALCOMMENT = txtCOLLMATERIALComment.Text;
                    objSpecimenM.LogUserID = ControlParameter.loginID.ToString();

                    objSpecimenM = objConfig.SaveSpecimen(objSpecimenM);

                    // DataGrid Specimen Group

                    if (dataGridView1.Rows.Count > 0)
                    {
                        objSpecimenM = objConfig.DeleteSpecimenGroupInDict(objSpecimenM);

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            objSpecimenM.SPMG_ID = row.Cells[2].Value.ToString();
                            objSpecimenM = objConfig.SaveSpecimenGroupIn_DICT_COLL_MATERIAL(objSpecimenM);
                        }
                    }

                }
                else if (moduleType == "dup")
                {

                    objSpecimenM.COLLMATERIALCODE = txtCOLLMATERIALCODE.Text;
                    objSpecimenM.COLLMATERIALTEXT = txtCOLLMATERIALTEXT.Text;
                    objSpecimenM.COLLMATERIALCOMMENT = txtCOLLMATERIALComment.Text;
                    objSpecimenM.LogUserID = ControlParameter.loginID.ToString();

                    objSpecimenM = objConfig.SaveSpecimen(objSpecimenM);

                    // DataGrid Specimen Group

                    if (dataGridView1.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            objSpecimenM.SPMG_ID = row.Cells[2].Value.ToString();
                            objSpecimenM = objConfig.SaveSpecimenGroupIn_DICT_COLL_MATERIAL(objSpecimenM);
                        }
                    }
                }
                else if (moduleType == "add")
                {

                    objSpecimenM.COLLMATERIALCODE = txtCOLLMATERIALCODE.Text;
                    objSpecimenM.COLLMATERIALTEXT = txtCOLLMATERIALTEXT.Text;
                    objSpecimenM.COLLMATERIALCOMMENT = txtCOLLMATERIALComment.Text;
                    objSpecimenM.LogUserID = ControlParameter.loginID.ToString();

                    objSpecimenM = objConfig.SaveSpecimen(objSpecimenM);

                    // DataGrid Specimen Group

                    if (dataGridView1.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            objSpecimenM.SPMG_ID = row.Cells[2].Value.ToString();
                            objSpecimenM = objConfig.SaveSpecimenGroupIn_DICT_COLL_MATERIAL(objSpecimenM);
                        }
                    }
                }

                this.Close();
                Action instance = refreshData;
                if (instance != null)
                    instance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc:" + ex.Message);
            }
        }

        private void Loaddata()
        {
            if (moduleType == "edit")
            {

                txtCOLLMATERIALCODE.Text = objSpecimenM.COLLMATERIALCODE;
                txtCOLLMATERIALTEXT.Text = objSpecimenM.COLLMATERIALTEXT;
                txtCOLLMATERIALComment.Text = objSpecimenM.COLLMATERIALCOMMENT;

                objSpecimenM.COLLMATERIALID = Convert.ToInt16(ControlSpecimen.COLLMATERIALID);

                Get_Specimen_GROUP(objSpecimenM);

                label6.Visible = true;
                label6.Text = "Edit Specimen";
                this.Text = "Edit Specimen";

            }
            else if (moduleType == "dup")
            {
                txtCOLLMATERIALCODE.Text = objSpecimenM.COLLMATERIALCODE;
                txtCOLLMATERIALTEXT.Text = objSpecimenM.COLLMATERIALTEXT;
                txtCOLLMATERIALComment.Text = objSpecimenM.COLLMATERIALCOMMENT;

                objSpecimenM.COLLMATERIALID = Convert.ToInt16(ControlSpecimen.COLLMATERIALID);

                label6.Visible = true;
                label6.Text = "Duplicate Specimen";
                this.Text = "Duplicate Specimen";
            }

            else if (moduleType == "add")
            {
                txtCOLLMATERIALCODE.Text = objSpecimenM.COLLMATERIALCODE;
                txtCOLLMATERIALTEXT.Text = objSpecimenM.COLLMATERIALTEXT;
                txtCOLLMATERIALComment.Text = objSpecimenM.COLLMATERIALCOMMENT;

                label6.Visible = true;
                label6.Text = "Add Specimen";
                this.Text = "Add Specimen";
            }
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void Get_Specimen_GROUP (SpecimenM ObjCOLLMATERIALID)
        {

            ConfigurationController objConfig = new ConfigurationController();
            DataTable dt = null;
            SpecimenM objSpecimenM = new SpecimenM();

            try
            {
                dt = objConfig.GetSpecimenGroup_in_Collmaterial(ObjCOLLMATERIALID);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = dt.Rows[i]["CODE"].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = dt.Rows[i]["TEXT"].ToString();
                        dataGridView1.Rows[i].Cells[2].Value = dt.Rows[i]["ID"].ToString();
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

        private void Btn_search_G1_Click(object sender, EventArgs e)
        {
            ConfigurationController objConfiguration = new ConfigurationController();
            frmSearchdict fm = new frmSearchdict();
            SpecimenM objSpecimenM = new SpecimenM();

            DataTable dt;
            try
            {
                dt = objConfiguration.GetSpecimenLookup(objSpecimenM);

                dt.Columns["CODE"].ColumnName = "code";
                dt.Columns["TEXT"].ColumnName = "name";
                dt.Columns["ID"].ColumnName = "id";

                fm.SearchData = dt;
                fm.RefreshData();
                fm.ShowDialog();

                if (fm.Selected == true)
                {
                    objSpecimenM.SPMG_ID = fm.SelectedID.ToString();
                    objSpecimenM.SPMG_NAME = fm.SelectedName;
                    objSpecimenM.SPMG_CODE = fm.SelectedCode;

                    SPM01 = objSpecimenM.SPMG_CODE;
                    SPM02 = objSpecimenM.SPMG_NAME;
                    SPM03 = objSpecimenM.SPMG_ID;

                    int minRowCount = 0;
                    bool Checkcode = false;

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        minRowCount++;
                    }

                    if (minRowCount == 0)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = SPM01;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = SPM02;
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = SPM03;

                    }
                    else
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[2].Value.ToString().Contains(SPM03.ToString()))
                            {
                                Checkcode = true;
                            }
                        }
                        if (Checkcode == true)
                        {
                            MessageBox.Show("Code Exits please select another code");
                        }
                        else if (Checkcode == false)
                        {
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = SPM01;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = SPM02;
                            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[2].Value = SPM03;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Desc" + ex.Message);
            }
            finally
            {
                fm = null;
            }

        }

        private void Btn_Clear_G4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
}
