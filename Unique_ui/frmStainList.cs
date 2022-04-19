using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using UNIQUE.Result;
using UniquePro.Controller;
using UniquePro.Entities.Configuration;

namespace UNIQUE
{
    public partial class frmStainList : Form
    {
        SqlConnection conn;
        int rowIndex = 0;
        public frmStainList()
        {
            InitializeComponent();
        }


        public Action actionAddNewStain { get; set; }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Action instance = actionAddNewStain;
            if (instance != null)
                instance();

            this.Close();
        }

        private void frmStainList_Load(object sender, EventArgs e)
        {
                     
            //conn = new ConnectionString().Connect();
            ControlParameter.strStainControlID = "";
            queryData();
        }

        private void queryData()
        {
            ConfigurationController objConfig = new ConfigurationController();
            StainM objStainM = new StainM();
            DataTable dt;

            //          string sql = @" SELECT [MBSTAINID]
            //    ,[MBSTAINCODE]
            //    ,[STAINNAME]
            //    ,[DICTCONSOSTATUS]
            //    ,[FIRSTITEMID]
            //    ,[STARTVALIDDATE]
            //    ,[MBSTAINCREATEDATE]
            //    ,[ENDVALIDDATE]
            //    ,[NOTPRINTABLE]
            //    ,[LOGUSERID]
            //    ,[LOGDATE]
            //FROM [DICT_MB_STAINS]";

            //          SqlCommand cmd = new SqlCommand(sql,conn);
            //          SqlDataAdapter adp = new SqlDataAdapter(cmd);
            //          DataSet ds = new DataSet();
            objStainM.MBSTAINCODE = "%";
            objStainM.STAINNAME = "%";

            dt = objConfig.GetStain(objStainM);
            bindingSource1.DataSource = dt;

            //if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            //ds.Clear();
            //adp.Fill(ds, "result");
            //if (ds.Tables["result"].Rows.Count > 0)
            //{
            //    bindingSource1.DataSource = ds;
            //    bindingSource1.DataMember = "result";
            //}
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (dataGridView1.SelectedRows.Count != 0)
                {
                    this.dataGridView1.Rows[e.RowIndex].Selected = true;
                    rowIndex = e.RowIndex;
                    ControlParameter.strStainControlName = this.dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    ControlParameter.strStainControlID = this.dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                }
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (dataGridView1.SelectedRows.Count != 0)
                {
                    this.dataGridView1.Rows[rowIndex].Selected = true;
                    ControlParameter.strStainControlName = this.dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
                    ControlParameter.strStainControlID = this.dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
                }
            }
        }
    }
}
