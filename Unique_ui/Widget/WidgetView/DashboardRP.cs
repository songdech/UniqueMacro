using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using UniquePro.Common;
using UniquePro.Entities.Common;
using UNIQUE.Forms.Dashboard;

namespace UNIQUE.TabbedView.WidgetView
{
    public partial class DashboardRP : UserControl
    {
        // Report Result  //

        ConnectString objConstr = new ConnectString();
        private String Constr = "";

        string StrThisDASHB = "RP";
        string StrRP_DASHB_ENABLE = "";
        string StrRP_DASHB_TIME = "";

        public DashboardRP()
        {
            InitializeComponent();
        }

        struct DataParameter
        {
            public int Process;
            public int Delay;
        }
        private DataParameter _inputparameter;

        private void DashboardRP_Load(object sender, EventArgs e)
        {
            try
            {
                Constr = objConstr.Connectionstring();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            // Clear counter 
            tileItem1.Text = "0";
            tileItem2.Text = "0";

            //
            // Check Enable && TIME
            Dashboard_Get_check();
            //
            // Enable Function Timer Tick
            Dashboard_Process();

            // Process on load
            Process_DashboardRP();
        }

        private void tileItem1_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            Form_DashboardRP_Urgent Form_RE_UR = new Form_DashboardRP_Urgent();
            Form_RE_UR.ShowDialog();
        }

        private void tileItem2_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            // RC Routine
            Form_DashboardRP_Routine Form_RP_ROU = new Form_DashboardRP_Routine();
            Form_RP_ROU.ShowDialog();
        }

        private void Dashboard_Get_check()
        {
            DBFactory dbFactory = new DBFactory();
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            StringBuilder sql = new StringBuilder();
            sql.Append
                ("SELECT " +
                " DASHB_ID,DASHB_NAME" +
                ",RTRIM(DASHB_ENABLE) AS DASHB_ENABLE" +
                ",DASHB_TIME" +
                ",DASHB_TEXT " +
                " FROM DICT_DASHBOARD WHERE DASHB_NAME= '" + StrThisDASHB + "' ");
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql.ToString(), objConn);
                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                ds.Clear();
                adp.Fill(ds, "result");
                SqlDataReader reader = sqlCmd.ExecuteReader();
                if (ds.Tables["result"].Rows.Count > 0)
                {
                    StrRP_DASHB_ENABLE = ds.Tables["result"].Rows[0]["DASHB_ENABLE"].ToString();
                    StrRP_DASHB_TIME = ds.Tables["result"].Rows[0]["DASHB_TIME"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Dashboard_Process()
        {
            try
            {
                if (StrRP_DASHB_ENABLE == "TRUE")
                {
                    timer1.Enabled = true;
                    timer1.Interval = Convert.ToInt32(StrRP_DASHB_TIME);
                    timer2.Enabled = true;
                    timer2.Interval = Convert.ToInt32(StrRP_DASHB_TIME);
                }
                else
                {
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int Dashboard_Query_count_Routine()
        {
            int counter = 0;

            DBFactory dbFactory = new DBFactory();
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            //StringBuilder sql = new StringBuilder();
            string sql = "";

            sql = @"SELECT 
MB_REQUESTS.VALIDATIONSTATUS,
REQUESTS.URGENT
FROM MB_REQUESTS
LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID
WHERE REQUESTS.URGENT =0 AND
(MB_REQUESTS.VALIDATIONSTATUS= '1'
OR MB_REQUESTS.VALIDATIONSTATUS= '2')";

            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql.ToString(), objConn);
                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                if (objConn.State == ConnectionState.Open) objConn.Close(); objConn.Open();
                sqlCmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "result");
                SqlDataReader reader = sqlCmd.ExecuteReader();
                if (ds.Tables["result"].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables["result"].Rows.Count; i++)
                    {
                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return counter;
        }

        private int Dashboard_Query_count_Urgent()
        {
            int counter = 0;
            DBFactory dbFactory = new DBFactory();
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT " +
                "MB_REQUESTS.VALIDATIONSTATUS " +
                ",REQUESTS.URGENT " +
                "FROM MB_REQUESTS " +
                "LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID " +
                "WHERE REQUESTS.URGENT = 1 AND " +
                "(MB_REQUESTS.VALIDATIONSTATUS= '1' " +
                "OR MB_REQUESTS.VALIDATIONSTATUS= '3' " +
                "OR MB_REQUESTS.VALIDATIONSTATUS= '4')");

            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql.ToString(), objConn);
                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                if (objConn.State == ConnectionState.Open) objConn.Close(); objConn.Open();
                sqlCmd.ExecuteNonQuery();
                ds.Clear();
                adp.Fill(ds, "result");
                SqlDataReader reader = sqlCmd.ExecuteReader();
                if (ds.Tables["result"].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables["result"].Rows.Count; i++)
                    {
                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return counter;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorkerRP.IsBusy)
            {
                _inputparameter.Delay = 10;
                _inputparameter.Process = 1200;
                backgroundWorkerRP.RunWorkerAsync(_inputparameter);
            }
        }

        private void backgroundWorkerCR_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int process = ((DataParameter)e.Argument).Process;
            int delay = ((DataParameter)e.Argument).Delay;
            int index = 1;
            try
            {
                for (int i = 0; i < process; i++)
                {
                    if (!backgroundWorkerRP.CancellationPending)
                    {
                        backgroundWorkerRP.ReportProgress(index++ * 100 / process, string.Format("Process  {0}", i));
                        System.Threading.Thread.Sleep(delay);
                        //..
                        //.. Add code if do anything.
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorkerRP_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = string.Format("Process..{0}%", e.ProgressPercentage);
            progressBar1.Update();
        }

        private void backgroundWorkerRP_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            label1.Text = "Wait..";
            progressBar1.Value = 0;
            Process_DashboardRP();
        }

        private void Process_DashboardRP()
        {
            int Show_Urgent = Dashboard_Query_count_Urgent();
            tileItem1.Text = Convert.ToString(Show_Urgent);

            int Show_routine = Dashboard_Query_count_Routine();
            tileItem2.Text = Convert.ToString(Show_routine);
        }
    }
}
