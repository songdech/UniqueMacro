using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Data.SqlClient;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddDoctors : Form
    {
        //SqlConnection conn;
        private string moduleType;
        private string strLocID;
        private DoctorM objDoctorM = null;
        private ConfigurationController objConfig = null;

        public Action yourAction { get; set; }

        public frmAddDoctors(string type)
        {
            InitializeComponent();
         
            this.Text = type;
            moduleType = type;
            objConfig = new ConfigurationController();
            objDoctorM = ControlParameter.DoctorInfoM;
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void frmAddDoctors_Load(object sender, EventArgs e)
        {
            queryData();
            //if (objDoctorM == null)
            //{
            //    objDoctorM = new DoctorM();
            //    objDoctorM.LogUserID =  ControlParameter.UserInfoM.UserID.ToString();

            //}
            //else
            //{
            //    txtDoccode.Text = objDoctorM.DoctorCode;
            //    txtDocname.Text = objDoctorM.DocName;
            //    txtHomePhoe.Text = objDoctorM.Telephone;
            //    txtMail.Text = objDoctorM.Email;

            //}
        }

        private void queryData()
        {
           

            if (moduleType == "edit")
            {
                txtDoccode.Text = objDoctorM.DoctorCode;
                txtDocname.Text = objDoctorM.DocName;
                txtHomePhoe.Text = objDoctorM.Telephone;
                txtMail.Text = objDoctorM.Email;

                this.Text = "Edit Doctor";

            }
            else if (moduleType == "dup")
            {
                txtDoccode.Text = objDoctorM.DoctorCode;
                txtDocname.Text = objDoctorM.DocName;
                txtHomePhoe.Text = objDoctorM.Telephone;
                txtMail.Text = objDoctorM.Email;
                this.Text = "Add Doctor";
            }
            else
            {
                objDoctorM = new DoctorM();
                objDoctorM.LogUserID = ControlParameter.ControlUser.USERID.ToString();
            }

        }

        private void txtDoccode_TextChanged(object sender, EventArgs e)
        {
            
        }
        
        private void navRegister_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                objDoctorM.DoctorCode= txtDoccode.Text;
                objDoctorM.DocName = txtDocname.Text;
                objDoctorM.Telephone = txtHomePhoe.Text;
                objDoctorM.Email = txtMail.Text;                
                objConfig.SaveDoctor(objDoctorM);
                this.Close();
                Action instance = yourAction;
                if (instance != null)
                    instance();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Add location", MessageBoxButtons.OK);
            }

            //if (strType == "add" || strType == "dup")
            //{
            //    if (txtDoccode.Text != "")
            //    {
            //        if (!checkDocCode())
            //        {
            //            string sql = @"INSERT INTO DICT_DOCTORS (DOCCODE,DOCNAME,TELEPHON,EMAIL,STARTVALIDDATE,LOGDATE,DOCCREDATE,LOGUSERID) 
            //            VALUES (@DOCCODE,@DOCNAME,@TELEPHON,@EMAIL,@STARTVALIDDATE,@LOGDATE,@DOCCREDATE,@LOGUSERID)";
            //            SqlCommand cmd = new SqlCommand(sql, conn);
            //            cmd.Parameters.Add("@DOCCODE", SqlDbType.VarChar).Value = txtDoccode.Text;
            //            cmd.Parameters.Add("@DOCNAME", SqlDbType.VarChar).Value = txtDocname.Text;     
            //            cmd.Parameters.Add("@TELEPHON", SqlDbType.VarChar).Value = txtHomePhoe.Text;
            //            cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = txtMail.Text;
            //            cmd.Parameters.Add("@STARTVALIDDATE", SqlDbType.DateTime).Value = DateTime.Now;
            //            cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
            //            cmd.Parameters.Add("@DOCCREDATE", SqlDbType.DateTime).Value = DateTime.Now;
            //            cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
            //            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            //            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            //            cmd.ExecuteNonQuery();
            //            this.Close();


            //            Action instance = yourAction;
            //            if (instance != null)
            //                instance();
            //        }
            //        else
            //        {
            //            MessageBox.Show("Doctor code : " + txtDoccode.Text.Trim() + " arleady in database.", "Please use other code.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //}
            //else if (strType == "edit")
            //{
            //    if (txtDoccode.Text != "")
            //    {
            //        if (!checkDocCode())
            //        {
            //            string sql = "UPDATE DICT_DOCTORS set DOCCODE=@DOCCODE, DOCNAME=@DOCNAME,TELEPHON=@TELEPHON,EMAIL=@EMAIL,LOGDATE=@LOGDATE WHERE DOCID ='" + strDocID + "' ";
            //            SqlCommand cmd = new SqlCommand(sql, conn);
            //            cmd.Parameters.Add("@DOCCODE", SqlDbType.VarChar).Value = txtDoccode.Text;
            //            cmd.Parameters.Add("@DOCNAME", SqlDbType.VarChar).Value = txtDocname.Text;                       
            //            cmd.Parameters.Add("@TELEPHON", SqlDbType.VarChar).Value = txtHomePhoe.Text;
            //            cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = txtMail.Text;
            //            cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;

            //            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            //            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            //            cmd.ExecuteNonQuery();

            //            Action instance = yourAction;
            //            if (instance != null)
            //                instance();


            //            this.Close();
            //        }
            //        else
            //        {
            //            MessageBox.Show("Doctor code : " + txtDoccode.Text.Trim() + " arleady in database.", "Please use other code.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //}
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //private bool checkDocCode()
        //{ string sql="";
        //    if(strType == "edit"){
        //     sql = " SELECT DICT_DOCTORS.DOCCODE FROM DICT_DOCTORS WHERE DICT_DOCTORS.DOCCODE = '" + txtDoccode.Text.Trim() + "' AND DICT_DOCTORS.DOCCODE != '"+ControlDoctor.DoctorCode +"'";
        //    } else{
        //        sql = " SELECT DICT_DOCTORS.DOCCODE FROM DICT_DOCTORS WHERE DICT_DOCTORS.DOCCODE = '" + txtDoccode.Text.Trim() + "'";
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
    }
}
