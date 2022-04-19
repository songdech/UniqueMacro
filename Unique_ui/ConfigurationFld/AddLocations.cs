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
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class AddLocations : Form
    {
        private string moduleType;
        private string strLocID;        
        private LocationM objLocationM = null;
        private ConfigurationController objConfig = null;

        public Action yourAction { get; set; }
        public AddLocations(string type, string strLocID)
        {
            InitializeComponent();
            this.moduleType = type;
            this.strLocID = strLocID;
            objConfig = new ConfigurationController();
            objLocationM = ControlParameter.LocationInfoM;
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                objLocationM.LocCode = txtCode.Text ;
                objLocationM.LocName = txtName.Text;
                objLocationM.Telephone = txtHomePhoe.Text;
                objLocationM.Email = txtMail.Text;
                objLocationM.NationalCode = "";
                objConfig.SaveLocation(objLocationM);
                this.Close();
                Action instance = yourAction;
                if (instance != null)
                    instance();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Add location", MessageBoxButtons.OK);
            }
            
            //if (moduleType == "add" || moduleType == "dup")
            //{
            //    if (txtCode.Text != "")
            //    {

                    
            //        //if (!checkLocCode())
            //        //{
            //        //    string sql = @"INSERT INTO DICT_LOCATIONS (LOCCODE,LOCNAME,TELEPHON,EMAIL,STARTVALIDDATE,LOGDATE,CREATEDATE) 
            //        //    VALUES (@LOCCODE,@LOCNAME,@TELEPHON,@EMAIL,@STARTVALIDDATE,@LOGDATE,@CREATEDATE)";
            //        //    SqlCommand cmd = new SqlCommand(sql, conn);
            //        //    cmd.Parameters.Add("@LOCCODE", SqlDbType.VarChar).Value = txtCode.Text;
            //        //    cmd.Parameters.Add("@LOCNAME", SqlDbType.VarChar).Value = txtName.Text;                 
            //        //    cmd.Parameters.Add("@TELEPHON", SqlDbType.VarChar).Value = txtHomePhoe.Text;               
            //        //    cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = txtMail.Text;
            //        //    cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
            //        //    cmd.Parameters.Add("@CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
            //        //    cmd.Parameters.Add("@STARTVALIDDATE", SqlDbType.DateTime).Value = DateTime.Now;

            //        //    SqlDataAdapter adap = new SqlDataAdapter(cmd);
            //        //    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            //        //    cmd.ExecuteNonQuery();

            //        //    this.Close();
            //        //    Action instance = yourAction;
            //        //    if (instance != null)
            //        //        instance();
            //        //}
            //        //else
            //        //{
            //        //    MessageBox.Show("Location code : " + txtCode.Text.Trim() + " arleady in database.", "Please use other code.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        //}
            //    }
            //}
            //else if (moduleType == "edit")
            //{
            //    if (txtCode.Text != "")
            //    {
            //        if (!checkLocCode())
            //        {
            //            string sql = "UPDATE DICT_LOCATIONS set LOCCODE=@LOCCODE, LOCNAME=@LOCNAME,TELEPHON=@TELEPHON,EMAIL=@EMAIL,LOGDATE=@LOGDATE WHERE LOCID ='" + strLocID + "' ";
            //            SqlCommand cmd = new SqlCommand(sql, conn);
            //            cmd.Parameters.Add("@LOCCODE", SqlDbType.VarChar).Value = txtCode.Text;
            //            cmd.Parameters.Add("@LOCNAME", SqlDbType.VarChar).Value = txtName.Text;                        
            //            cmd.Parameters.Add("@TELEPHON", SqlDbType.VarChar).Value = txtHomePhoe.Text;
            //            cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = txtMail.Text;
            //            cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;

            //            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            //            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            //            cmd.ExecuteNonQuery();


            //            this.Close();
            //            Action instance = yourAction;
            //            if (instance != null)
            //                instance();


                       
            //        }
            //        else
            //        {
            //            MessageBox.Show("Location code : " + txtCode.Text.Trim() + " arleady in database.", "Please use other code.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }                    
            //    }
            //}
        }

        //private bool checkLocCode()
        //{
        //    string sql = "";
        //    if (moduleType == "edit")
        //    {
        //        sql = " SELECT LOCCODE FROM DICT_LOCATIONS WHERE LOCCODE = '" + txtCode.Text.Trim() + "' AND LOCCODE != '" + ControlLocation.Loccode + "'";
        //    }
        //    else
        //    {
        //        sql = " SELECT LOCCODE FROM DICT_LOCATIONS WHERE LOCCODE = '" + txtCode.Text.Trim() + "'";
        //    }
        //    SqlCommand cmd = new SqlCommand(sql, conn);
        //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();
        //    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
        //    cmd.ExecuteNonQuery();
        //    adp.Fill(ds);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private void AddLocations_Load(object sender, EventArgs e)
        {
            if (moduleType == "edit")
            {
                txtCode.Text = objLocationM.LocCode;
                txtName.Text = objLocationM.LocName;
                txtHomePhoe.Text = objLocationM.Telephone;
                txtMail.Text = objLocationM.Email;
                this.Text = "Edit Location";
            }
            else if (moduleType == "dup")
            {
                txtCode.Text = objLocationM.LocCode;
                txtName.Text = objLocationM.LocName;
                txtHomePhoe.Text = objLocationM.Telephone;
                txtMail.Text = objLocationM.Email;
                this.Text = "Dupplicate Location";
            }
            else
            {
                objLocationM = new LocationM();
                this.Text = "Add Location";
            }
        }

        private void tileNavPane1_Click(object sender, EventArgs e)
        {

        }

        private void AddLocations_FormClosed(object sender, FormClosedEventArgs e)
        {
            objConfig = null;
            objLocationM = null;
        }

    }
}
