using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNIQUE.RequestFld
{
    public partial class frmRequestMgnt : Form
    {
        SqlConnection conn;
        public frmRequestMgnt()
        {
            InitializeComponent();
            this.ActiveControl = txtPatnumber;
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void navRegister_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            RequestFld.frmRequestCreate fm = new RequestFld.frmRequestCreate("");
            if (!CheckOpened(fm.Text))
            {
                fm = new RequestFld.frmRequestCreate("");
                fm.HN = lblHN.Text;
                fm.NAME = lblName.Text + " " +lblLastName.Text;
                fm.SEX = lblSex.Text;
                fm.BIRTHDATE = lblBirthDate.Text;
                fm.AGE = lblAge.Text;
                fm.PATID = lblPATID.Text;
                fm.Show();
            }
        }

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

        private void txtPatnumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchPAT();
            }
        }

        private void SearchPAT()
        {
           
            string strWhere = "";
            if (txtPatnumber.Text != "")
            {
                strWhere = " WHERE PATIENTS.PATNUMBER LIKE '" + txtPatnumber.Text + "%'";
            }
            if (txtName.Text != "")
            {
                strWhere = " WHERE PATIENTS.NAME LIKE '%" + txtName.Text + "'";
            }
             if (txtLastName.Text != "")
            {
                strWhere = " WHERE PATIENTS.LASTNAME LIKE '%" + txtLastName.Text + "'";
            }
             if (txtRequestNumber.Text != "")
            {
                strWhere = " WHERE REQUESTS.ACCESSNUMBER LIKE '%" + txtRequestNumber.Text + "'";
            }
            string sql = @"SELECT distinct PATIENTS.PATID
      ,[PATNUMBER]
      ,[NAME]
      ,[LASTNAME]
      ,[ADDRESS1]
      ,[ADDRESS2]
      ,[CITY]
      ,[STATE]
      ,[POSTALCODE]
      ,[COUNTRY]
      ,[BIRTHDATE]
      ,[SEX]
      ,[TELEPHON]
      ,[TELEPHON2]
      ,[FAX]
      ,[EMAIL]
      ,[INCOMMINGDATE]
      ,[DEATHDATE]
      ,[SECRETRESULT]
      ,[VIP]   
      ,[PATCREATIONDATE]
      ,[STARTVALIDDATE]
      ,[ENDVALIDDATE]
        ,PATIENTS.COMMENT
	  ,REQUESTS.ACCESSNUMBER,REQUESTS.REQDATE,REQUESTS.RECEIVEDDATE
  FROM [PATIENTS] 
  LEFT OUTER JOIN REQUESTS ON PATIENTS.PATID = REQUESTS.PATID  "+ strWhere;

            SqlCommand cmd = new SqlCommand(sql,conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            adp.Fill(ds,"PATIENTS");
            bindingSource1.DataSource = ds;
            bindingSource1.DataMember = "PATIENTS";



            if (ds.Tables["PATIENTS"].Rows.Count > 0)
            {

            //    lblPATID.Text = ds.Tables["PATIENTS"].Rows[0]["PATID"].ToString().Trim();

               

                navRegister.Enabled = true;
                navModification.Enabled = true;
            }
            else
            {
                navRegister.Enabled = false;
                navModification.Enabled = false;
            }



        }

        private void frmRequestMgnt_Load(object sender, EventArgs e)
        {
       
            conn = new ConnectionString().Connect();
    
            clsControlsData.GetConfigurations(conn);
            txtPatnumber.MaxLength = clsControlsData.patnumlenght;
         
      
           
        }

        private void lblHNm_Click(object sender, EventArgs e)
        {
           // lblHNm.Text = 
        }

        private void navSearchPatient_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            SearchPAT();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchPAT();
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchPAT();
            }
        }

        private void txtRequestNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchPAT();
            }
        }

        private void lblBirthDate_BindingContextChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void lblBirthDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lblBirthDate.Text != "" || lblBirthDate.Text != "-" || lblBirthDate.Text != string.Empty)
                {
                    //if (dataGridView2.SelectedRows.Count > 0)
                    //{

                        DateTime bDate = Convert.ToDateTime(lblBirthDate.Text);
                        DateTime cDate = DateTime.Now;
                        AgeCalculate age = new AgeCalculate(bDate, cDate);
                        ////  Console.WriteLine("It's been {0} years, {1} months, and {2} days since your birthday", age.Years, age.Months, age.Days);
                        lblAge.Text = age.Years + " ปี " + age.Months + " เดือน " + age.Days + " วัน ";
                        //lblBirthDate.Text = bDate.ToString("dd-MM-yyyy");
                   // }
                }
                else
                {
                    lblAge.Text = "-";
                }

            }
            catch (Exception)
            {
                lblAge.Text = "-";
            }
        }

        private void lblRequestNumber_TextChanged(object sender, EventArgs e)
        {
            if (lblRequestNumber.Text != "" || lblRequestNumber.Text != "-")
            {
                navModification.Enabled = true;
            }
            else
            {
                navModification.Enabled = false;
            }
        }

        private void navModification_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            RequestFld.frmRequestCreate fm = new RequestFld.frmRequestCreate("");
            if (!CheckOpened(fm.Text))
            {
                fm = new RequestFld.frmRequestCreate(lblRequestNumber.Text);
                fm.HN = lblHN.Text;
                fm.NAME = lblName.Text + " " + lblLastName.Text;
                fm.SEX = lblSex.Text;
                fm.BIRTHDATE = lblBirthDate.Text;
                fm.AGE = lblAge.Text;
                fm.PATID = lblPATID.Text;
                fm.Show();
            }
        }

        private void tileNavPane1_Click(object sender, EventArgs e)
        {

        }
    }
}
