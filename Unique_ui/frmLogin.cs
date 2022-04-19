using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using UniquePro.Controller;
using UniquePro.Entities.Common;
using UniquePro.Entities.GeneralSetting;
using UNIQUE.GeneralSetting;
using UniquePro.DAO.Configurations;

namespace UNIQUE
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn;
        private UserM objUserM = null;

        ConnectionStringMasterDAO objConstrDAO = new ConnectionStringMasterDAO();
        private static AES128_EncryptAndDecrypt AES128 = new AES128_EncryptAndDecrypt();


        public frmLogin()
        {
            InitializeComponent();
            this.ActiveControl = txtUsername;
            lblVersion.Text = "Version:" + getVersion();
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private  string getVersion()
        {
            System.Reflection.Assembly ProjectAssembly;
            ProjectAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            return ProjectAssembly.GetName().Version.ToString();
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FormMain fm = new FormMain();
            this.Hide();
            fm.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
         
        }

        // Login button
        private void panel1_Click(object sender, EventArgs e)
        {
            try
            {
                loginAction();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Login", MessageBoxButtons.OK);
            }
            
        }

        private void loginAction()
        {
            try
            {
                if (txtUsername.Text != "" )
                {
                    if (txtUsername.Text != "ADMINUNI")
                    {
                        ConnectStringM.ClientDBSet = "PASS";

                        if (ConnectStringM.ClientDBSet == "PASS")
                        {
                            AuthenticateController objAuthen = new AuthenticateController();
                            ConfigurationController objConfiguration = new ConfigurationController();

                            ControlParameter.DatasetComboData = objConfiguration.GetDictCombobox();

                            if (ControlParameter.ControlUser == null)
                            {
                                ControlParameter.ControlUser = new UserM();
                            }

                            ControlParameter.ControlUser.USERNAME = txtUsername.Text.Trim();
                            ControlParameter.ControlUser.PASSWORD = txtPassword.Text.Trim();

                            objUserM = objAuthen.GetUserSearch(ControlParameter.ControlUser);

                            string Pass_decrpyt = AES128.Decrypt(objUserM.PASSWORD);

                            if (txtPassword.Text.Trim() == Pass_decrpyt)
                            {
                                ConfigurationController objConfig = new ConfigurationController();
                                DataTable dt = null;
                                //UserM objUserM = new UserM();

                                objUserM.USERNAME = txtUsername.Text.Trim();

                                try
                                {
                                    dt = objAuthen.GetRole_Input(ControlParameter.ControlUser);

                                    if (dt.Rows.Count > 0)
                                    {
                                        int Alias_count = 0;
                                        bool Alias_inst = false;

                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            // Column1 = USERNAME
                                            // Column2 = ALIAS
                                            // Column3 = FUNCNAME
                                            // Column4 = USERSID

                                            dataGridView1.Rows.Add();
                                            dataGridView1.Rows[i].Cells[0].Value = dt.Rows[i]["USERNAME"].ToString();
                                            dataGridView1.Rows[i].Cells[1].Value = dt.Rows[i]["ALIAS"].ToString();
                                            dataGridView1.Rows[i].Cells[2].Value = dt.Rows[i]["FUNCNAME"].ToString();
                                            dataGridView1.Rows[i].Cells[3].Value = dt.Rows[i]["USERSID"].ToString();
                                        }

                                        foreach (DataGridViewRow row in dataGridView1.Rows)
                                        {
                                            Alias_count++;

                                            if (row.Cells[1].Value.ToString() == "7")
                                            {
                                                Alias_inst = true;
                                            }
                                        }

                                        if (Alias_count == 1 && Alias_inst == true)
                                        {
                                            ControlParameter.loginID = objUserM.USERID;
                                            ControlParameter.loginName = objUserM.NAME;
                                            ControlParameter.loginLastName = objUserM.LASTNAME;
                                            ControlParameter.ControlUser = objUserM;

                                            Terminal_MicroScan fm_MicroScan = new Terminal_MicroScan();

                                            fm_MicroScan.Show();
                                            this.Hide();
                                        }
                                        else
                                        {
                                            ControlParameter.loginID = objUserM.USERID;
                                            ControlParameter.loginName = objUserM.NAME;
                                            ControlParameter.loginLastName = objUserM.LASTNAME;
                                            ControlParameter.ControlUser = objUserM;

                                            ControlParameter.UniqueSystemConfig = objConfiguration.GetUniqueSystemConfig();

                                            FormMain fm = new FormMain();

                                            if (!CheckOpened(fm.Text))
                                            {
                                                fm.Show();
                                            }

                                            this.Hide();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No role and Rights in configure.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.Message);
                                }

                            }
                            else
                            {
                                MessageBox.Show("Please check username or password is correctly? ");
                            }
                        }

                        else
                        {
                            throw new Exception("Please check username or password is correctly? ");
                        }
                    }
                    else
                    {
                        DataTable dt = objConstrDAO.GetUserAddminUNI(txtUsername.Text.Trim(), txtPassword.Text.Trim());
                        if (dt.Rows.Count > 0)
                        {
                            ControlParameter.loginName = dt.Rows[0]["NAME"].ToString();
                            ControlParameter.loginLastName = dt.Rows[0]["LASTNAME"].ToString();

                            //FormMain fm = new FormMain();

                            ConfigurationApp.frmConfigurationApp fm = new ConfigurationApp.frmConfigurationApp();

                            if (!CheckOpened(fm.Text))
                            {
                                fm.Show();
                            }

                            this.Hide();

                        }
                        else
                        {
                            MessageBox.Show("Please check username or password for Unique is correctly? ", "login", MessageBoxButtons.OK);
                        }

                        //throw new Exception("Please Setup Database for Company !!!");                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" Please check username or password. " + ex.Message);
            }
        }

        //private void loginAction1()
        //{

        //    if (txtUsername.Text != "" && txtPassword.Text != "")
        //    {
        //        if (txtUsername.Text != "ADMINUNI")
        //        {
        //            if (ConnectStringM.ClientDBSet == "PASS")
        //            {

        //            }
        //            //string sql = "SELECT  , USERNAME , PASSWORD, NAME, LASTNAME, TELEPHON, EMAIL, POSITION FROM USERS WHERE USERNAME = '" + txtUsername.Text.Trim() + "' AND PASSWORD = '" + txtPassword.Text.Trim() + "'";
        //            //SqlCommand cmd = new SqlCommand(sql, conn);
        //            //SqlDataAdapter adap = new SqlDataAdapter(cmd);
        //            //DataSet ds = new DataSet();
        //            //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
        //            //ds.Clear();
        //            //cmd.ExecuteNonQuery();
        //            //adap.Fill(ds);
        //            //if (ds.Tables[0].Rows.Count > 0)
        //            //{
        //            //    ControlParameter.loginName = ds.Tables[0].Rows[0]["NAME"].ToString();
        //            //    ControlParameter.loginLastName = ds.Tables[0].Rows[0]["LASTNAME"].ToString();
        //            //    //login.Position = ds.Tables[0].Rows[0]["POSITION"].ToString();
        //            //    //login.Telephone = ds.Tables[0].Rows[0]["TELEPHON"].ToString();
        //            //    //login.Email = ds.Tables[0].Rows[0]["EMAIL"].ToString();

        //            //    //   Application.AddMessageFilter(new MessageFilter());
        //            //    FormMain fm = new FormMain();

        //            //    if (!CheckOpened(fm.Text))
        //            //    {
        //            //        // Add the message
        //            //        //  MessageBox.Show("timout.");
        //            //        fm = new FormMain();


        //            //        fm.Show();

        //            //    }
        //            //    //           this.Close();
        //            //    this.Hide();

        //            //}
        //            else
        //            {
        //                MessageBox.Show("Please check username or password is correctly? ", "login", MessageBoxButtons.OK);
        //            }
        //            }

        //            else
        //            {
        //                MessageBox.Show("Please Setup Database for Company !!! ", "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            }
        //        }

        //        else
        //        {
        //            DataTable dt = objConstrDAO.GetUserAddminUNI(txtUsername.Text.Trim(), txtPassword.Text.Trim());
        //            if (dt.Rows.Count > 0)
        //            {
        //                ControlParameter.loginName = dt.Rows[0]["NAME"].ToString();
        //                ControlParameter.loginLastName = dt.Rows[0]["LASTNAME"].ToString();

        //                FormMain fm = new FormMain();

        //                if (!CheckOpened(fm.Text))
        //                {
        //                    // Add the message
        //                    //  MessageBox.Show("timout.");
        //                    fm = new FormMain();
        //                    fm.Show();

        //                }
        //                //           this.Close();
        //                this.Hide();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Please check username or password for Unique is correctly? ", "login", MessageBoxButtons.OK);
        //            }
        //        }

        //    }
        //    else
        //    {
        //        MessageBox.Show("Please check username or password is correctly? ", "login", MessageBoxButtons.OK);
        //    }
        //}

        private bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true;
                }
            }
            return false;
        }

        //private void loginAction()
        //{
        //    try
        //    {
        //        if (txtUsername.Text != "" && txtPassword.Text != "")
        //        {
        //            AuthenticateController objAuthen = new AuthenticateController();                    

        //            objUserM.UserName = txtUsername.Text.Trim();
        //            objUserM.Password = txtPassword.Text.Trim();

        //            objUserM = objAuthen.GetUserSearch(objUserM);

        //            ControlParameter.loginName = objUserM.Name;
        //            ControlParameter.loginLastName = objUserM.Lastname;
        //            ControlParameter.UserInfoM = objUserM;

        //            FormMain fm = new FormMain();
        //            fm = new FormMain();
        //            fm.Show();
        //            this.Hide();
        //        }
        //        else
        //        {
        //            throw new Exception ("Please check username or password is correctly? ");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //}


        private void panel2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                //DataTable dt = new DataTable();

                //dt = objConstrDAO.GetDBConstring();

                //if (dt.Rows.Count > 0)
                //{
                //    ConnectStringM.StrDataSource = dt.Rows[0]["CONDATASOURCE"].ToString();
                //    ConnectStringM.StrCatalog = dt.Rows[0]["CONCATALOG"].ToString();
                //    ConnectStringM.StrUser = dt.Rows[0]["CONUSER"].ToString();
                //    ConnectStringM.StrPassword = dt.Rows[0]["CONPASS"].ToString();
                //    ConnectStringM.ClientDBSet = "PASS";

                //    conn = new ConnectionString().Connect();

                //    labelControl1.Text = getVersion();
                //    labelControl2.Text = "UNIQUE Copyright © 2020";
                //}
                //else
                //{
                //    MessageBox.Show("Please Setup Database for Company !!! ", "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    ConnectStringM.ClientDBSet = "FAIL";
                //}


            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection to Server is Failed!!! " + ex.Message.ToString());
            }

        }

        private void timerIdle_Tick(object sender, EventArgs e)
        {
            //Here perform your action by first validating that idle task is not already running.
            // If you want to redirect user to login page, then first check weather login page is already displayed or not
            // if not then show loign page. Same logic for other task or Implement your own. 
            // Remember after every five minutes or period you defined above this timerIdle_Tick will be called 
            ////so first check weather idle task is already running or not. If not then perform 
            //if(!)
            //{
               
            //  //  PerformNecessoryActions();
            //  //  ShowLoginForm();
            //}

            this.ShowDialog();
            
       
        
            //FormMain fm = new FormMain();
            //fm.Enabled = false;
           // fm.Show();
        }
      
        private void textEdit2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    loginAction();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Login", MessageBoxButtons.OK );
                }
                
            }
            
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}