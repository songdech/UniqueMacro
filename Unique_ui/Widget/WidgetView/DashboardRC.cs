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
    public partial class DashboardRC : UserControl
    {
        ConnectString objConstr = new ConnectString();
        private String Constr = "";

        string StrThisDASHB = "RC";
        string StrRC_DASHB_ENABLE = "";
        string StrRC_DASHB_TIME = "";


        public DashboardRC()
        {
            InitializeComponent();
        }

        struct DataParameter
        {
            public int Process;
            public int Delay;
        }
        private DataParameter _inputparameter;

        private void DashboardRC_Load(object sender, EventArgs e)
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
            tileItem5.Text = "0";
            tileItem9.Text = "0";

            //
            // Check Enable && TIME
            Dashboard_Get_check();
            //
            // Enable Function Timer Tick
            Dashboard_Process();
            // Process on load
            Process_DashboardRC();

        }

        private void tileItem9_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            // RC Urgent
            Form_DashboardRC_Urgent Form_SR = new Form_DashboardRC_Urgent();
            Form_SR.ShowDialog();
        }

        private void tileItem5_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            // RC Routine
            Form_DashboardRC_Routine Form_SR = new Form_DashboardRC_Routine();
            Form_SR.ShowDialog();
        }

        private void Dashboard_Get_check()
        {
            DBFactory dbFactory = new DBFactory();
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT DASHB_ID,DASHB_NAME,RTRIM(DASHB_ENABLE) AS DASHB_ENABLE,DASHB_TIME,DASHB_TEXT FROM DICT_DASHBOARD WHERE DASHB_NAME= '" + StrThisDASHB + "' ");
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
                    StrRC_DASHB_ENABLE = ds.Tables["result"].Rows[0]["DASHB_ENABLE"].ToString();
                    StrRC_DASHB_TIME = ds.Tables["result"].Rows[0]["DASHB_TIME"].ToString();
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
                if (StrRC_DASHB_ENABLE == "TRUE")
                {
                    timer1.Enabled = true;
                    timer1.Interval = Convert.ToInt32(StrRC_DASHB_TIME);
                }
                else
                {
                    timer1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int Dashboard_Query_count_Routine()
        {
            //tileItem5.Text = "";   SELECT TRANSMISSIONSTATUS  FROM SP_REQUESTS WHERE TRANSMISSIONSTATUS ='10'
            int counter = 0;

            DBFactory dbFactory = new DBFactory();
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT TRANSMISSIONSTATUS  FROM SP_REQUESTS WHERE TRANSMISSIONSTATUS ='0' AND SP_REQUESTS.REQURGENT ='0' ");
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
            sql.Append("SELECT TRANSMISSIONSTATUS  FROM SP_REQUESTS WHERE TRANSMISSIONSTATUS ='0' AND SP_REQUESTS.REQURGENT ='1' ");
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
            if (!backgroundWorkerRC.IsBusy)
            {
                _inputparameter.Delay = 10;
                _inputparameter.Process = 1200;
                backgroundWorkerRC.RunWorkerAsync(_inputparameter);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
        }

        private void backgroundWorkerRC_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int process = ((DataParameter)e.Argument).Process;
            int delay = ((DataParameter)e.Argument).Delay;
            int index = 1;
            try
            {
                for (int i = 0; i < process; i++)
                {
                    if (!backgroundWorkerRC.CancellationPending)
                    {
                        backgroundWorkerRC.ReportProgress(index++ * 100 / process, string.Format("Process  {0}", i));
                        System.Threading.Thread.Sleep(delay);
                        //..
                        //.. Add code if do anything.
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void backgroundWorkerRC_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = string.Format("Process..{0}%", e.ProgressPercentage);
            progressBar1.Update();
        }

        private void backgroundWorkerRC_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            label1.Text = "Wait..";
            progressBar1.Value = 0;
            Process_DashboardRC();

        }
        private void Process_DashboardRC()
        {
            int Show_routine = Dashboard_Query_count_Routine();
            tileItem5.Text = Convert.ToString(Show_routine);

            int Show_Urgent = Dashboard_Query_count_Urgent();
            tileItem9.Text = Convert.ToString(Show_Urgent);
        }

    }
}
