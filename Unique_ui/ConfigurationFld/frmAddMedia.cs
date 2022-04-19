using System;
using System.Windows.Forms;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddMedia : Form
    {        
        private string moduleType;
        private string strMediaID;
        private MediaM objMediaM;

        private ConfigurationController objConfiguration = new ConfigurationController();

        public frmAddMedia(string moduleType, string strMediaID)
        {
            InitializeComponent();
            this.moduleType = moduleType;
            this.strMediaID = strMediaID;
            objMediaM = ControlParameter.MediaM;

            if (moduleType != "add")
            {
                objMediaM = ControlParameter.MediaM;
            }
        }

        public Action refreshData { get; set; }
        private void frmAddMedia_Load(object sender, EventArgs e)
        {
            Loaddata();
        }

        private void Loaddata()
        {
            try
            {
                if (moduleType == "edit")
                {
                    txtCode.Enabled = false;
                    txtName.Select();
                    txtID.Text = Convert.ToString(objMediaM.AgarID);
                    txtCode.Text = objMediaM.AgarCode;
                    txtName.Text = objMediaM.AgarName;
                    txtDescription.Text = objMediaM.Description;

                    objMediaM.LogUserID = ControlParameter.loginID.ToString();

                    label6.Visible = true;
                    label6.Text = "Edit Media";
                    this.Text = "Edit Media";
                }
                else if (moduleType == "dup")
                {

                    txtCode.Select();

                    txtName.Text = objMediaM.AgarName;
                    txtDescription.Text = objMediaM.Description;

                    objMediaM.LogUserID = ControlParameter.loginID.ToString();

                    label6.Visible = true;
                    label6.Text = "Duplicate Media";
                    this.Text = "Duplicate Media";
                }
                else if (moduleType == "add")
                {
                    txtCode.Select();
                    label6.Visible = true;
                    label6.Text = "Add Media";
                    this.Text = "Add Media";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void navSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                if (moduleType == "edit")
                {
                    try
                    {
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            try
                            {
                                txtCode.Enabled = false;
                                objMediaM.AgarID = Convert.ToInt32(txtID.Text);
                                objMediaM.AgarCode = txtCode.Text;
                                objMediaM.AgarName = txtName.Text;
                                objMediaM.Description = txtDescription.Text;

                                objMediaM.LogUserID = ControlParameter.loginID.ToString();
                                objMediaM = objConfiguration.SaveMedia_in_DICT_MB_AGARS(objMediaM);

                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [edit]", ex.Message);
                    }
                }
                else if (moduleType == "dup")
                {
                    try
                    {
                        // Code & Name  Fix do not empty!
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            objMediaM.AgarCode = txtCode.Text;
                            objMediaM.AgarName = txtName.Text;
                            objMediaM.Description = txtDescription.Text;

                            objMediaM.LogUserID = ControlParameter.loginID.ToString();
                            objMediaM = objConfiguration.SaveMedia_in_DICT_MB_AGARS(objMediaM);

                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [dup]", "Warning" + ex.Message);
                    }
                }
                else if (moduleType == "add")
                {
                    try
                    {
                        // Code & Name  Fix do not empty!
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            objMediaM.AgarCode = txtCode.Text;
                            objMediaM.AgarName = txtName.Text;
                            objMediaM.Description = txtDescription.Text;

                            objMediaM.LogUserID = ControlParameter.loginID.ToString();
                            objMediaM = objConfiguration.SaveMedia_in_DICT_MB_AGARS(objMediaM);

                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error in Function Save [add]", ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Error Function Save Desc.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }
    }
}