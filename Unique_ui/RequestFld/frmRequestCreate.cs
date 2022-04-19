using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Collections;

namespace UNIQUE.RequestFld
{
    public partial class frmRequestCreate : Form
    {
        SqlConnection conn;
        Label[] lblTestcode;
        CheckBox[] btn_choice;
        CheckBox[] b1;
        int count = 0;
        string[] code;
        string accessnumber = "";

        ArrayList rowsTestID = new ArrayList();
       
        public frmRequestCreate(string accessnumber)
        {
            InitializeComponent();
            this.accessnumber = accessnumber;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmRequestCreate_Load(object sender, EventArgs e)
        {
            conn = new ConnectionString().Connect();

            queryLocation();
            querySpecimenType();
            queryDoctor();




            createTestBox();

            if (accessnumber == "")
            {

                /*
                 * Manage Running number of the request creation.
                 */
                int numLength = 8;

                int runningNumber = new Nullable<int>(getRunningNumber()).GetValueOrDefault();
                int padLeftZero = Convert.ToInt32(numLength - Convert.ToInt32(runningNumber.ToString().Length));
                string newNumber = runningNumber.ToString().PadLeft(padLeftZero + 1, '0');
                //    CultureInfo abc = new CultureInfo();
                DateTime dt = DateTime.Now;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                //   txtRequestNumber.Text = dt.ToString("yyMMdd") + newNumber;
                txtRequestNumber.Text = dt.ToString("yy") + newNumber;

                insertNewRunningNumber(newNumber);
            }
            else //edit
            {

            }

        }

        private void queryDoctor()
        {

            SqlCommand cmd = new SqlCommand(" SELECT DOCCODE, DOCNAME, DOCID FROM DICT_DOCTORS", conn);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            adap.Fill(ds, "result");
            bindingSource_Doctor.DataSource = ds;
            bindingSource_Doctor.DataMember = "result";
        }

        private void querySpecimenType()
        {
            SqlCommand cmd = new SqlCommand("SELECT COLLMATERIALCODE,COLLMATERIALTEXT,COLLMATERIALID FROM DICT_COLL_MATERIALS", conn);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            Dataset_SpecimenType ds = new Dataset_SpecimenType();

            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            adap.Fill(ds, "result");
            bindingSource_SpecimenTYpe.DataSource = ds;
            bindingSource_SpecimenTYpe.DataMember = "result";
        }

        private void queryLocation()
        {
            SqlCommand cmd = new SqlCommand("SELECT LOCCODE, LOCID, LOCNAME FROM DICT_LOCATIONS", conn);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            adap.Fill(ds, "result");
            bindingSource_Location.DataSource = ds;
            bindingSource_Location.DataMember = "result";
        }

        private void insertNewRunningNumber(string newNumber)
        {
            string sql = "UPDATE MANAGECOUNTER SET COUNTERNUMBER=@COUNTERNUMBER  ";
            SqlCommand cmd = new SqlCommand(sql,conn);
            cmd.Parameters.Add("@COUNTERNUMBER", SqlDbType.VarChar).Value = newNumber;
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            cmd.ExecuteNonQuery();
        }

        private int getRunningNumber()
        {
            int  number = 0;
            string sql = "SELECT COUNTERNUMBER FROM MANAGECOUNTER  ORDER BY COUNTERNUMBER DESC";
            SqlCommand cmd = new SqlCommand(sql, conn);
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            cmd.ExecuteNonQuery();
            adap.Fill(ds, "reqnumber");
            if (ds.Tables["reqnumber"].Rows.Count > 0)
            {
               
                number = Int32.Parse( ds.Tables["reqnumber"].Rows[0][0].ToString()) +1;
            }

            return number;

        }

        private int getRunningNumberOfProtocol()
        {
            int number = 0;
            string sql = "SELECT COUNTERNUMBER FROM PROTOCOL_COUNTERS  ORDER BY COUNTERNUMBER DESC";
            SqlCommand cmd = new SqlCommand(sql, conn);
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            cmd.ExecuteNonQuery();
            adap.Fill(ds, "reqnumber");
            if (ds.Tables["reqnumber"].Rows.Count > 0)
            {

                number = Int32.Parse(ds.Tables["reqnumber"].Rows[0][0].ToString()) + 1;
            }

            return number;

        }

    

        private void createTestBox()
        {
        
            string sql = "SELECT * FROM DICT_TESTS";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            adap.Fill(ds, "test");
            if (ds.Tables["test"].Rows.Count > 0) {

               
                   int x = 20, y = 0, w = 51, h = 50;
                   int x2 = 20, y2 = 80, w2 = 51, h2 = 50;  
           count = ds.Tables["test"].Rows.Count;
           btn_choice = new CheckBox[count];
            lblTestcode = new Label[count];
            code = new string[count];
            b1 = new CheckBox[count];
            for (int i = 0; i < count; i++)
            {
            //   count = Convert.ToInt32(ds.Tables["test"].Rows[i][i].ToString());

                btn_choice[i] = new CheckBox();
                panel_test.Controls.Add(btn_choice[i]);
                btn_choice[i].Size = new Size(x, y);
                btn_choice[i].SetBounds(x, y, w, h);
                btn_choice[i].Width = 200;
                btn_choice[i].Height = 80;
            //    btn_choice[i].Name = "abc";
                btn_choice[i].Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btn_choice[i].ForeColor = System.Drawing.Color.Black;
                code[i] = ds.Tables["test"].Rows[i][0].ToString();


                x = x + w + 180;
                x2 = x2 + 2 + 180;
                if (i % 4 ==3)
                {
                    x = 20;
                    y += h + 10;

                    x2 = 20;
                    y2 += h + 10;
                }

                btn_choice[i].Text = ds.Tables["test"].Rows[i]["TESTNAME"].ToString().Trim();
                btn_choice[i].Name = "NAME" + ds.Tables["test"].Rows[i]["TESTID"].ToString().Trim() ;
              
            
              //  lblTestcode[i].Text = ds.Tables["test"].Rows[i]["TESTNAME"].ToString();

            //    btn_choice[i].Click += new EventHandler(this.btn_choice_click);

                string strTextID = ds.Tables["test"].Rows[i]["TESTID"].ToString();

                btn_choice[i].CheckedChanged += (sender, EventArgs) => { buttonNext_Click(sender, EventArgs, strTextID); };
            }

            //    this.tuse[i].SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
              
//CheckBox[] chk = new CheckBox[10];
//      int height = 1;
//      int padding = 10;

//      for (int i = 0; i <= 9; i++)
//      {

//        chk[i] = new CheckBox();

//        chk[i].Name = i.ToString();

//        chk[i].Text = i.ToString();

//        chk[i].TabIndex = i;

//        chk[i].AutoCheck = true;

//        chk[i].Bounds = new Rectangle(10, 20 + padding + height, 40, 22);

       
//                splitContainerControl1.Panel2.Controls.Add(chk[i]);

//                height += 22;

           }
        }

        private void buttonNext_Click(object sender, EventArgs EventArgs, string strTextID)
        {
            CheckBox bt = (CheckBox)sender;
      
            if (bt.Checked == true)
            {
                bt.ForeColor = System.Drawing.Color.Teal;
                dataGridView1.Rows.Add(bt.Text.ToString());      
        //        MessageBox.Show("" + bt.Name.ToString() + "  " + strTextID +" Name:"+bt.Text.ToString());

                rowsTestID.Add(strTextID);
            }
            else
            {
                bt.ForeColor = System.Drawing.Color.Black;

                rowsTestID.Remove(strTextID);
                //foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                //{
                //    if (!row.IsNewRow)
                //        dataGridView1.Rows.Remove(row);
                //}
            }
         //   MessageBox.Show("" + bt.Name.ToString() + "  " + strTextID + " Name:" + bt.Text.ToString());
        }    

      
        private void CheckBoxTestHandler(object sender, EventArgs e)
        {
           
             CheckBox bt = (CheckBox)sender;
             if (bt.Checked == true)
             {
               
                     string abc = "xxx";
                     MessageBox.Show(bt.ToString() + " "+ e.ToString());
              
             }

             for (int i = 0; i < count; i++)
             {
               //  if (i == Convert.ToInt32(bt.Text))
                // {
                 
                   //  b1[i].Checked = true;
                  //   MessageBox.Show("" + code[bt.Checked].ToString());
                 //}
               //  }
             }
        }

        public string HN
        {
            get { return lblHN.Text; }  // Getter
            set { lblHN.Text = value; } // Setter
        }
        public string NAME
        {
            get { return lblName.Text; }  // Getter
            set { lblName.Text = value; } // Setter
        }
        public string SEX
        {
            get { return lblSex.Text; }  // Getter
            set { lblSex.Text = value; } // Setter
        }
        public string BIRTHDATE
        {
            get { return lblBirthDate.Text; }  // Getter
            set { lblBirthDate.Text = value; } // Setter
        }

        public string AGE
        {
            get { return lblAge.Text; }  // Getter
            set { lblAge.Text = value; } // Setter
        }

        public string PATID //Patameter from frmRequestMGNT to insert in the REQUESTS.
        {
            get { return lblPATID.Text; }  // Getter
            set { lblPATID.Text = value; } // Setter
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

             //   DataGridViewSelectedRowCollection rows = dataGridView1.SelectedRows;
             //   dataGridView1.Rows.RemoveAt(rows);

            } 
         
         
        }

        private void gridView3_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
         //   gridView2.SetRowCellValue(e.RowHandle, "gridColumn1", "abc");
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            for (int i = 0; i < rowsTestID.Count; i++)
            {
                string abc = rowsTestID[i].ToString();
                //  // Change the field value.
                //row["Discontinued"] = true;
                MessageBox.Show("testID:  " + abc);
         
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            insertintoRequest();
            for (int i = 0; i < rowsTestID.Count; i++)
            {
                string abc = rowsTestID[i].ToString();
                //  // Change the field value.
                //row["Discontinued"] = true;
              //  MessageBox.Show("testID:  " + abc);

            }
        }

        private void insertintoRequest()
        {
            int reqID = 0;
            DateTime dt = DateTime.Now;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");


            string sql = @"INSERT INTO REQUESTS (PATID, ACCESSNUMBER, REQCREATIONDATE, REQSTATUS, STATUSDATE, REQDATE, COLLECTIONDATE, URGENT, COMMENT, LASTUPDATE, RECEIVEDDATE, EXTERNALORDERNUMBER, LOGUSERID, LOGDATE) 
                                         VALUES (@PATID,@ACCESSNUMBER,@REQCREATIONDATE,@REQSTATUS,@STATUSDATE,@REQDATE,@COLLECTIONDATE,@URGENT,@COMMENT,@LASTUPDATE,@RECEIVEDDATE,@EXTERNALORDERNUMBER,@LOGUSERID,@LOGDATE);SELECT CAST(scope_identity() AS int)";
            SqlCommand cmd = new SqlCommand(sql,conn );
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            cmd.Parameters.Add("@PATID", SqlDbType.Int).Value = Convert.ToInt32(lblPATID.Text);
            cmd.Parameters.Add("@ACCESSNUMBER", SqlDbType.VarChar).Value = txtRequestNumber.Text.Trim();
            cmd.Parameters.Add("@REQCREATIONDATE", SqlDbType.Int).Value = dt.ToString("yyyyMMdd");
            cmd.Parameters.Add("@REQSTATUS", SqlDbType.Int).Value = 1; //RC (Request Creation)
            cmd.Parameters.Add("@REQDATE", SqlDbType.DateTime).Value = dateEdit_REQDATE.EditValue;
            cmd.Parameters.Add("@STATUSDATE", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@COLLECTIONDATE", SqlDbType.DateTime).Value = dateEdit_COLLECTIONDATE.EditValue;
            if (radioButton2.Checked == true)
            {
                cmd.Parameters.Add("@URGENT", SqlDbType.Int).Value = 1; //Routine
            }
            else
            {
                cmd.Parameters.Add("@URGENT", SqlDbType.Int).Value = 2; //Ugent
            }
            cmd.Parameters.Add("@COMMENT", SqlDbType.VarChar).Value = txtComment.Text.Trim();
            cmd.Parameters.Add("@LASTUPDATE", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@RECEIVEDDATE", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@EXTERNALORDERNUMBER", SqlDbType.VarChar).Value = txtHostNumber.Text.Trim();
            cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
            cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;

            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            reqID = Convert.ToInt32(cmd.ExecuteScalar());

            INSERTTO_REQUEST_TESTS(reqID);  
        }

        private void INSERTTO_REQUEST_TESTS(int reqID)
        {
            if(rowsTestID.Count > 0){
                for (int i = 0; i < rowsTestID.Count; i++)
                {
                    string testID = rowsTestID[i].ToString();               

                    string sql = "INSERT INTO REQ_TESTS (TESTID,REQUESTID,LASTUPDATE,LOGUSERID,LOGDATE) VALUES (@TESTID,@REQUESTID,@LASTUPDATE,@LOGUSERID,@LOGDATE)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@TESTID", SqlDbType.Int).Value = testID;
                cmd.Parameters.Add("@REQUESTID", SqlDbType.Int).Value = reqID;
                cmd.Parameters.Add("@LASTUPDATE", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
                cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        string strProtocol = GetProtocolOfTESTS(testID);                       
                       if (strProtocol != "" )
                        {
                            INSERT_MB_REQUESTS(strProtocol, reqID);  //PROTOCOLS.
                        }
                    }
                }
            }// Test ID is not null.           
        }

        private void INSERT_MB_REQUESTS(string strProtocol, int reqID)
        {
            int mbreqID = 0;
            /*
             * Manage Running number of the subrequest creation.
             */
            int numLength = 8;

            int runningNumber = new Nullable<int>(getRunningNumberOfProtocol()).GetValueOrDefault();
            int padLeftZero = Convert.ToInt32(numLength - Convert.ToInt32(runningNumber.ToString().Length));
            string newNumber = runningNumber.ToString().PadLeft(padLeftZero + 1, '0');
            //    CultureInfo abc = new CultureInfo();
            DateTime dt = DateTime.Now;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            string protocolnumber = strProtocol + newNumber;


            string sql = @"INSERT INTO MB_REQUESTS (PROTOCOLID,REQUESTID,MBREQNUMBER,SUBREQUESTCREDATE,COLLECTIONDATE,RECEIVEDDATE,LOGUSERID,LOGDATE,MBREQEUSTSTATUS) VALUES
                                                   (@PROTOCOLID,@REQUESTID,@MBREQNUMBER,@SUBREQUESTCREDATE,@COLLECTIONDATE,@RECEIVEDDATE,@LOGUSERID,@LOGDATE,@MBREQEUSTSTATUS);SELECT CAST(scope_identity() AS int)";
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@PROTOCOLID", SqlDbType.Int).Value = ControlParameter.protocolID;
            cmd.Parameters.Add("@REQUESTID", SqlDbType.Int).Value = reqID;
            cmd.Parameters.Add("@MBREQNUMBER", SqlDbType.VarChar).Value = protocolnumber;
            cmd.Parameters.Add("@SUBREQUESTCREDATE", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@COLLECTIONDATE", SqlDbType.DateTime).Value = dateEdit_COLLECTIONDATE.EditValue;
            cmd.Parameters.Add("@RECEIVEDDATE", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
            cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@MBREQEUSTSTATUS", SqlDbType.Int).Value = "1"; // Request Creation.
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
      
             mbreqID = Convert.ToInt32(cmd.ExecuteScalar());
            if (mbreqID > 0)
            {
                 insertNewRunningNumberOfProtocolNumber(newNumber); // updated new protocolnumber after mb_requests;
                 INSERT_TRIGGER_DEFAULT_STAiN(mbreqID, strProtocol);
           }


        }

        private void INSERT_TRIGGER_DEFAULT_STAiN(int subrequestID, string strProtocol)
        {
            DataSet ds = new DataSet();
           ds = checkStainDefaultOfProtocol(ds,strProtocol);
            if (ds.Tables["stain"].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables["stain"].Rows.Count; i++)
                {
                    string sql = @"INSERT INTO SUBREQMB_STAINS (MBSTAINID,SUBREQUESTID,CREATEUSER,CREATIONDATE,LOGUSERID,LOGDATE) 
                                                    VALUES (@MBSTAINID,@SUBREQUESTID,@CREATEUSER,@CREATIONDATE,@LOGUSERID,@LOGDATE)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@MBSTAINID", SqlDbType.Int).Value = ds.Tables["stain"].Rows[i]["MBSTAINID"].ToString();
                    cmd.Parameters.Add("@SUBREQUESTID", SqlDbType.Int).Value = subrequestID;
                    cmd.Parameters.Add("@CREATEUSER", SqlDbType.VarChar).Value = "SYS";
                    cmd.Parameters.Add("@CREATIONDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@LOGUSERID", SqlDbType.VarChar).Value = "SYS";
                    cmd.Parameters.Add("@LOGDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    SqlDataAdapter ada = new SqlDataAdapter(cmd);
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    cmd.ExecuteNonQuery();
                    
                }
            }
        }

        private DataSet checkStainDefaultOfProtocol(DataSet ds, string strProtocol)
        {
            string sql = @"SELECT DICT_MB_PROTOCOLS.PROTOCOLID,DICT_MB_PROTOCOLS.PROTOCOLCODE ,DICT_MB_STAINS.MBSTAINID,MBSTAINCODE,DICT_MB_STAINS.STAINNAME    
  FROM DICT_MB_PROTOCOLS LEFT OUTER JOIN 
  DICT_MB_PROTOCOL_STAINS ON DICT_MB_PROTOCOLS.PROTOCOLID = DICT_MB_PROTOCOL_STAINS.PROTOCOLID
  LEFT OUTER JOIN  DICT_MB_STAINS ON DICT_MB_PROTOCOL_STAINS.MBSTAINID = DICT_MB_STAINS.MBSTAINID
  WHERE DICT_MB_PROTOCOLS.PROTOCOLCODE = '"+strProtocol+"'";
            SqlCommand cmd = new SqlCommand(sql,conn    );
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            ds = new DataSet();
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            cmd.ExecuteNonQuery();
            adap.Fill(ds, "stain");
            return ds;

        }

        private void insertNewRunningNumberOfProtocolNumber(string newNumber)
        {
            string sql = "UPDATE PROTOCOL_COUNTERS SET COUNTERNUMBER=@COUNTERNUMBER  ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@COUNTERNUMBER", SqlDbType.VarChar).Value = newNumber;
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            cmd.ExecuteNonQuery();
            
        }

        private string GetProtocolOfTESTS(string testID)
        {
            string strProtocolNumber = "";
  

            string sql = @"SELECT DICT_TESTS.TESTID,DICT_TESTS.TESTCODE,DICT_MB_PROTOCOLS.PROTOCOLCODE,DICT_MB_PROTOCOLS.PROTOCOLTEXT,DICT_MB_PROTOCOLS.REPORTFORMAT,DICT_MB_PROTOCOLS.PROTOCOLID
                        FROM DICT_TESTS
                        LEFT OUTER JOIN DICT_MB_PROTOCOLS ON DICT_TESTS.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
                        WHERE DICT_TESTS.TESTID = '" +testID+"'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            cmd.ExecuteNonQuery();
            adap.Fill(ds, "PROTOCOL");
            if (ds.Tables["PROTOCOL"].Rows.Count > 0)
            {

                strProtocolNumber = ds.Tables["PROTOCOL"].Rows[0]["PROTOCOLCODE"].ToString().Trim();
                ControlParameter.protocolID = 0;
                ControlParameter.protocolID = Convert.ToInt32(ds.Tables["PROTOCOL"].Rows[0]["PROTOCOLID"].ToString().Trim());
            }

 
            return strProtocolNumber;  
        }   
    }
}
