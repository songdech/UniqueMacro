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

namespace UNIQUE.ConfigurationFld
{
    public partial class frmAddCustomizedResult : Form
    {
        SqlConnection conn;
        private string moduleType;
        private string strCusResID;

        int rowIndex = 0;
        public frmAddCustomizedResult(string moduleType, string strCusResID)
        {
           InitializeComponent();
            this.moduleType = moduleType;
            this.strCusResID = strCusResID;
        }
        public Action refreshData { get; set; }
        private void frmAddCustomizedResult_Load(object sender, EventArgs e)
        {            
            
            conn = new ConnectionString().Connect();
            if (moduleType == "edit")
            {
                loadData();
            }
        }

        private void loadData()
        {

            txtCode.Text = ControlCustomizeResult.customizeResCode;
            txtName.Text = ControlCustomizeResult.customizeResText;

            string sql = @"SELECT  DICT_CUS_RESULT_LIST.CUSRESULTTEXT,DICT_CUS_RESULT_LIST.CUSRESULTTEXT AS 'CUSRESULTTEXT2'
                            FROM DICT_CUS_RESULT_LIST LEFT OUTER JOIN 
                         DICT_CUS_RESULTS ON DICT_CUS_RESULT_LIST.CUSRESULTID = DICT_CUS_RESULTS.CUSRESULTID WHERE DICT_CUS_RESULT_LIST.CUSRESULTID = '"+strCusResID+"'";
           SqlCommand cmd = new SqlCommand(sql, conn);
           SqlDataAdapter adp = new SqlDataAdapter(cmd);
           DataSet ds = new DataSet();
           if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
           
           ds.Clear();
           adp.Fill(ds, "RES");
           if (ds.Tables["RES"].Rows.Count > 0)
           {
               bindingSource1 = new BindingSource();
               bindingSource1.DataSource = ds;
               bindingSource1.DataMember = "RES";
               dataGridView1.DataSource = bindingSource1;
           }
        }

        private void btnExit_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void btnSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            if (txtName.Text != "" && txtCode.Text != "")
            {
               // RetrieveIdentity(conn);
            }
            else
            {
                MessageBox.Show("Please enter code or text value");
            }
            if (txtCode.Text != "" && txtName.Text != "")
            {
                try
                {
                    if (moduleType == "add")
                    {
                        int primaryKey;
                        string sql = "INSERT INTO DICT_CUS_RESULTS (CUSRESULTCODE,CUSRESULTTEXT,CUSRESULTCREDATE,LOGUSERID,LOGDATE) VALUES (@CUSRESULTCODE,@CUSRESULTTEXT,@CUSRESULTCREDATE,@LOGUSERID,@LOGDATE);SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.Add("@CUSRESULTCODE", SqlDbType.VarChar).Value = txtCode.Text.Trim();
                        cmd.Parameters.Add("@CUSRESULTTEXT", SqlDbType.VarChar).Value = txtName.Text.Trim();
                        cmd.Parameters.Add("@CUSRESULTCREDATE", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
                        cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();

                        primaryKey = Convert.ToInt32(cmd.ExecuteScalar());


                        insert_cus_result_list(primaryKey);
                    }
                    else if (moduleType == "edit")
                    {
                        string sql = "UPDATE DICT_CUS_RESULTS SET CUSRESULTCODE=@CUSRESULTCODE,CUSRESULTTEXT=@CUSRESULTTEXT,LOGUSERID=@LOGUSERID,LOGDATE=@LOGDATE WHERE CUSRESULTID='" + strCusResID + "' ";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.Add("@CUSRESULTCODE", SqlDbType.VarChar).Value = txtCode.Text.Trim();
                        cmd.Parameters.Add("@CUSRESULTTEXT", SqlDbType.VarChar).Value = txtName.Text.Trim();
                       // cmd.Parameters.Add("@CUSRESULTCREDATE", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
                        cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            insert_cus_result_list(strCusResID);
                        }
                    }
                }
                catch (SqlException ex) { MessageBox.Show(ex.Message); }

            }
        }

        private void RetrieveIdentity(SqlConnection connection)
        {
            // Create a SqlDataAdapter based on a SELECT query.
            SqlDataAdapter adapter =
                new SqlDataAdapter(
                "SELECT CUSRESULTID, CUSRESULTCODE,CUSRESULTTEXT,CUSRESULTCREDATE,LOGUSERID,LOGDATE FROM DICT_CUS_RESULTS",
                connection);

            //Create the SqlCommand to execute the stored procedure.
            adapter.InsertCommand = new SqlCommand("InsertCustomizedResult",
                connection);
            adapter.InsertCommand.CommandType = CommandType.StoredProcedure;

            // Add the parameter for the CategoryName. Specifying the
            // ParameterDirection for an input parameter is not required.
            adapter.InsertCommand.Parameters.Add(
               new SqlParameter("@CUSRESULTCODE", SqlDbType.VarChar, 15,
               "CUSRESULTCODE"));
            adapter.InsertCommand.Parameters.Add(
              new SqlParameter("@CUSRESULTTEXT", SqlDbType.VarChar, 2000,
              "CUSRESULTTEXT"));
            adapter.InsertCommand.Parameters.Add(
              new SqlParameter("@LOGUSERID", SqlDbType.VarChar, 15,
              "LOGUSERID"));
            adapter.InsertCommand.Parameters.Add(
             new SqlParameter("@CUSRESULTCREDATE", SqlDbType.DateTime,2000,"CUSRESULTCREDATE"));
            adapter.InsertCommand.Parameters.Add(
    new SqlParameter("@LOGDATE", SqlDbType.DateTime, 2000, "LOGDATE"));


            // Add the SqlParameter to retrieve the new identity value.
            // Specify the ParameterDirection as Output.
            SqlParameter parameter =
                adapter.InsertCommand.Parameters.Add(
                "@Identity", SqlDbType.Int, 0, "CUSRESULTID");
            parameter.Direction = ParameterDirection.Output;

            // Create a DataTable and fill it.
            DataTable categories = new DataTable();
            adapter.Fill(categories);

            // Add a new row. 
            DataRow newRow = categories.NewRow();
            newRow["CUSRESULTCODE"] = txtCode.Text.Trim();
            newRow["CUSRESULTTEXT"] = txtName.Text.Trim();
            newRow["LOGUSERID"] = "SYS";
            newRow["CUSRESULTCREDATE"] = DateTime.Now;          
            newRow["LOGDATE"] = DateTime.Now;            
            categories.Rows.Add(newRow);

            adapter.Update(categories);

            DataRow lastRow = categories.Rows[categories.Rows.Count - 1];

            Console.WriteLine("List All Rows:");

            object id = 0;
                foreach (DataRow row in categories.Rows)
                {
                    {
                        Console.WriteLine("{0}: {1}", row[categories.Rows.Count - 1], row[1]);
                        id = row[0];
                        
                    }
                }

                //insert_cus_result_list(id);
        }

        private void insert_cus_result_list(object strCustomizedID)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int rows = 0; rows < dataGridView1.RowCount; rows++)
                {
                    //string conte = dataGridView1.Rows[rows].Cells[0].Value.ToString();
                    if (!string.IsNullOrEmpty(dataGridView1.Rows[rows].Cells[0].Value as string))
                    {
                        
                            string strCusText = dataGridView1.Rows[rows].Cells[0].Value.ToString();
                           // string strCusTextORG = dataGridView1.Rows[rows].Cells[1].Value.ToString();

                            if (string.IsNullOrEmpty(dataGridView1.Rows[rows].Cells[1].Value as string)) // if original value is null = INSERT to CUstomized result list.
                            {
                                string sql = "INSERT INTO DICT_CUS_RESULT_LIST (CUSRESULTID,CUSRESULTTEXT,LOGUSERID,LOGDATE) VALUES (@CUSRESULTID,@CUSRESULTTEXT,@LOGUSERID,@LOGDATE)      SELECT SCOPE_IDENTITY()";
                                SqlCommand cmd = new SqlCommand(sql, conn);
                                cmd.Parameters.Add("@CUSRESULTID", SqlDbType.Int).Value = strCustomizedID;
                                SqlParameter personId = new SqlParameter("@CUSRESULTID", SqlDbType.Int);
                                personId.Direction = ParameterDirection.Output;
                                cmd.Parameters.Add("@CUSRESULTTEXT", SqlDbType.VarChar).Value = strCusText;
                                cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
                                cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
                                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();

                                cmd.ExecuteNonQuery();
                            }else if ((!string.IsNullOrEmpty(dataGridView1.Rows[rows].Cells[1].Value as string)) && (dataGridView1.Rows[rows].Cells[0].Value.ToString().Trim() != dataGridView1.Rows[rows].Cells[1].Value.ToString().Trim())) // update customized result list.
                            {
                                string sql = "UPDATE DICT_CUS_RESULT_LIST SET CUSRESULTTEXT=@CUSRESULTTEXT,LOGUSERID=@LOGUSERID,LOGDATE=@LOGDATE WHERE CUSRESULTTEXT='"+dataGridView1.Rows[rows].Cells[1].Value.ToString().Trim()+"'";
                                SqlCommand cmd = new SqlCommand(sql, conn);
                                //cmd.Parameters.Add("@CUSRESULTID", SqlDbType.Int).Value = strCustomizedID;
                                //cmd.Parameters.Add("@CUSRESULTID", SqlDbType.Int).Value = strCusResID;
                                cmd.Parameters.Add("@CUSRESULTTEXT", SqlDbType.VarChar).Value = dataGridView1.Rows[rows].Cells[0].Value.ToString().Trim();
                                cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
                                cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
                                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();

                                cmd.ExecuteNonQuery();
                                                    
                            }                        
                    }
                }
            }
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int rows = 0; rows < dataGridView1.RowCount; rows++)
                {
                    //string conte = dataGridView1.Rows[rows].Cells[0].Value.ToString();
                    if (!string.IsNullOrEmpty(dataGridView1.Rows[rows].Cells[0].Value as string))
                        {
                            string value = dataGridView1.Rows[rows].Cells[0].Value.ToString();
                        }                    
                }
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //bindingSource1.EndEdit();
            //DataTable dt = (DataTable)bindingSource1.DataSource;

            //// Just for test.... Try this with or without the EndEdit....
            //DataTable changedTable = dt.GetChanges();
            //Console.WriteLine(changedTable.Rows.Count);

            //int rowsUpdated = sqlDataAdapter1.Update(dt);
            //Console.WriteLine(rowsUpdated);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    sqlDataAdapter.Update(dataTable);
            //}
            //catch (Exception exceptionObj)
            //{
            //    MessageBox.Show(exceptionObj.Message.ToString());
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                //sqlDataAdapter.Update(dataTable);
            }
            catch (Exception exceptionObj)
            {
                MessageBox.Show(exceptionObj.Message.ToString());
            }
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
           // DataTable changes = ((DataTable)dataGridView1.DataSource).GetChanges();
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
                 
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex > 0)
                {
                    this.dataGridView1.Rows[e.RowIndex].Selected = true;
                    rowIndex = e.RowIndex;
                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[0];
                    this.contextMenuStrip1.Show(this.dataGridView1, e.Location);
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView1.Rows[this.rowIndex].IsNewRow)
            {
                //this.dataGridView1.Rows.RemoveAt(this.rowIndex);
                //dataGridView1.Rows[e.RowIndex].Cells[0]
                try
                {
                    string sql = "DELETE FROM DICT_CUS_RESULT_LIST WHERE CUSRESULTTEXT='" + dataGridView1.Rows[this.rowIndex].Cells[0].Value.ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        this.dataGridView1.Rows.RemoveAt(this.rowIndex);
                    }
                    else
                    {
                        this.dataGridView1.Rows.RemoveAt(this.rowIndex);
                    }

                }
                catch (SqlException) { }
            }
            else
            {
 
            }
        }
    }
}
