using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UNIQUE
{
    public partial class Micro_Printout : Form
    {
        SqlConnection conn;
        string accessnumber = "";
        string protocolnumber = "";

        private MBReportController objConfig = new MBReportController();
        private MBReportM objMBReportM;

        private static string Version_Report = "Microbiology_Report printing Version V1.2";

        public Micro_Printout(string Str_ACCESSNUMBER, string Str_PROTOCOLNUM)
        {
            InitializeComponent();
            this.accessnumber = Str_ACCESSNUMBER;
            this.protocolnumber = Str_PROTOCOLNUM;
        }

        private void Micro_Printout_Load(object sender, EventArgs e)
        {
            try
            {
                conn = new Connection_ORM().Connect();
                objMBReportM = new MBReportM();

                this.Text = Version_Report;
                openReported();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchReports()
        {
            string sql = "";
            if (accessnumber != "" || protocolnumber != "")
            {
                if (accessnumber != "")
                {
                    sql = @"  SELECT DISTINCT
    REQUESTS.ACCESSNUMBER,REQUESTS.RECEIVEDDATE
  , MB_REQUESTS.MBREQNUMBER
  , PATIENTS.NAME +' '+ PATIENTS.LASTNAME AS 'NAME'
  , MB_ACTIONS.ACTIONTYPE AS 'REQSTATUS'
  , DICT_COLL_MATERIALS.COLLMATERIALTEXT AS 'SAMPLE'
  , CASE (MB_REQUESTS.VALIDATIONSTATUS ) WHEN '1' THEN 'CONSOL' WHEN '2' THEN 'VAL' ELSE 'Inprocess' END AS 'STATUS'

   FROM REQUESTS
   LEFT OUTER JOIN MB_REQUESTS ON REQUESTS.REQUESTID = MB_REQUESTS.REQUESTID
   LEFT OUTER JOIN MB_ACTIONS ON MB_REQUESTS.MBREQUESTID = MB_ACTIONS.SUBREQUESTID
   LEFT OUTER JOIN DICT_COLL_MATERIALS ON  MB_REQUESTS.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID
   LEFT OUTER JOIN PATIENTS ON PATIENTS.PATID = REQUESTS.PATID
   WHERE REQUESTS.ACCESSNUMBER ='" + accessnumber + "' AND MB_ACTIONS.ACTIONTYPE !=0 AND  (MB_ACTIONS.ACTIONTYPE = 1 OR MB_ACTIONS.ACTIONTYPE = 2) ";
                }
                else if (protocolnumber != "")
                {
                    sql = @"SELECT 
 REQUESTS.ACCESSNUMBER
,REQUESTS.RECEIVEDDATE
,REQUESTS.REQDATE
,MB_REQUESTS.MBREQNUMBER
,DICT_COLL_MATERIALS.COLLMATERIALTEXT AS 'SAMPLE'
,PATIENTS.NAME +' '+PATIENTS.LASTNAME as 'NAME'
,CASE (MB_REQUESTS.VALIDATIONSTATUS ) WHEN '1' THEN 'CONSOL' WHEN '2' THEN 'VAL' ELSE 'Inprocess' END AS 'STATUS'

FROM REQUESTS
LEFT OUTER JOIN MB_REQUESTS ON REQUESTS.REQUESTID = MB_REQUESTS.REQUESTID
LEFT OUTER JOIN MB_ACTIONS ON MB_REQUESTS.MBREQUESTID = MB_ACTIONS.SUBREQUESTID
LEFT OUTER JOIN DICT_COLL_MATERIALS ON  MB_REQUESTS.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID  
LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID= PATIENTS.PATID 
WHERE MB_REQUESTS.MBREQNUMBER ='" + protocolnumber + "'  AND MB_ACTIONS.ACTIONTYPE !=0 AND  (MB_ACTIONS.ACTIONTYPE = 1 OR MB_ACTIONS.ACTIONTYPE = 2) ";
                }
                
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    ds.Clear();
                    adp.Fill(ds, "req");

                    if (ds.Tables["req"].Rows.Count > 0)
                    {


                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please Input parameter","Warning!!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void openReported()
        {
            try
            {
                if (accessnumber != "" && protocolnumber != "")
                {
                    string chkStatusOfRequest = "";
                    string sqlStatus = "";
                    string PROTOCOLCODE = protocolnumber.Substring(0, 3);

                    string [] Str_ReportFormat = Get_Result_ReportFormat(PROTOCOLCODE);


                    sqlStatus = @"SELECT 
 REQUESTS.ACCESSNUMBER
,MB_REQUESTS.MBREQNUMBER
,MB_ACTIONS.SUBREQUESTID
,MB_ACTIONS.ACTIONTYPE
,MB_ACTIONS.ACTIONDATE
,MB_ACTIONS.ACTIONUSER

FROM MB_ACTIONS
LEFT OUTER JOIN MB_REQUESTS ON MB_ACTIONS.SUBREQUESTID = MB_REQUESTS.MBREQUESTID
LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID
WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Trim() + "'" +
    "AND MB_REQUESTS.MBREQNUMBER = '" + protocolnumber.Trim() + "'" +
    "AND MB_ACTIONS.ACTIONTYPE != 0 AND(MB_ACTIONS.ACTIONTYPE = 1 OR MB_ACTIONS.ACTIONTYPE = 2) ORDER BY MBACTIONID DESC";

                    SqlCommand cmdStatus = new SqlCommand(sqlStatus, conn);
                    SqlDataAdapter adpStatus = new SqlDataAdapter(cmdStatus);
                    DataSet dsStatus = new DataSet();
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    dsStatus.Clear();
                    adpStatus.Fill(dsStatus, "STATUS");
                    if (dsStatus.Tables["STATUS"].Rows.Count > 0)
                    {
                        if (dsStatus.Tables["STATUS"].Rows[0]["ACTIONTYPE"].ToString() != "")
                        {
                            if (dsStatus.Tables["STATUS"].Rows[0]["ACTIONTYPE"].ToString().Trim() == "1")
                            {
                                chkStatusOfRequest = "1";       // Plelimnary
                            }
                            else if (dsStatus.Tables["STATUS"].Rows[0]["ACTIONTYPE"].ToString().Trim() == "2")
                            {
                                chkStatusOfRequest = "2";      // Validation
                            }
                        }
                    }

                    /*
                     * END OF CHECK SATATAUS OF THE REQUESTS.
                     */
                    if (chkStatusOfRequest == "1" || chkStatusOfRequest == "2")
                    {

                        //Template : Culture.    

                        if ( Str_ReportFormat[0] == "1") // Arobe Culture.// Arobe Culture.
                        {
                            Template_Culture report = new Template_Culture();
                            report.Parameters["accessnumber"].Value = accessnumber;
                            report.Parameters["accessnumber"].Visible = false;
                            report.Parameters["potocalnumber"].Value = protocolnumber;
                            report.Parameters["potocalnumber"].Visible = false;
                            report.Parameters["status"].Value = chkStatusOfRequest.ToString();
                            report.Parameters["status"].Visible = false;
                            report.Parameters["testcode"].Value = "DEF";
                            report.Parameters["testcode"].Visible = false;

                            // Close of parameter
                            report.Parameters["Visitnum"].Value = "DEF";
                            report.Parameters["Visitnum"].Visible = false;
                            report.Parameters["Hosnumber"].Value = "DEF";
                            report.Parameters["Hosnumber"].Visible = false;
                            //

                            string validateType = "";
                            if (chkStatusOfRequest == "1")
                            {
                                validateType = "PRELIMINARY REPORTED: ";
                            }
                            else if (chkStatusOfRequest == "2")
                            {
                                validateType = "FINAL REPORTED: ";
                            }

                            report.Parameters["validateType"].Value = validateType;
                            report.Parameters["validateType"].Visible = false;
                            documentViewer1.DocumentSource = report;
                            report.CreateDocument();

                        }

                        /* 
                         * Template : Hemoculture.                         
                         */
                        else if (Str_ReportFormat[0] == "3")
                        {

                            Template_Hemoculture_p12 report = new Template_Hemoculture_p12();
                            report.Parameters["accessnumber"].Value = accessnumber;
                            report.Parameters["accessnumber"].Visible = false;
                            report.Parameters["potocalnumber"].Value = protocolnumber;
                            report.Parameters["potocalnumber"].Visible = false;
                            report.Parameters["status"].Value = chkStatusOfRequest.ToString();
                            report.Parameters["status"].Visible = false;
                            report.Parameters["testcode"].Value = "DEF";
                            report.Parameters["testcode"].Visible = false;

                            // Close of parameter
                            report.Parameters["Visitnum"].Value = "DEF";
                            report.Parameters["Visitnum"].Visible = false;
                            report.Parameters["Hosnumber"].Value = "DEF";
                            report.Parameters["Hosnumber"].Visible = false;
                            //

                            string validateType = "";
                            if (chkStatusOfRequest == "1")
                            {
                                validateType = "PRELIMINARY REPORTED: ";
                            }
                            else if (chkStatusOfRequest == "2")
                            {
                                validateType = "FINAL REPORTED: ";
                            }

                            report.Parameters["validateType"].Value = validateType;
                            report.Parameters["validateType"].Visible = false;
                            documentViewer1.DocumentSource = report;
                            report.CreateDocument();
                        }

                        /* 
                         * Template : Gram Stain                         
                         */
                        else if (Str_ReportFormat[0] == "2")
                        {
                            Template_Grams report = new Template_Grams();
                            report.Parameters["accessnumber"].Value = accessnumber;
                            report.Parameters["accessnumber"].Visible = false;
                            report.Parameters["potocalnumber"].Value = protocolnumber;
                            report.Parameters["potocalnumber"].Visible = false;
                            report.Parameters["status"].Value = chkStatusOfRequest.ToString();
                            report.Parameters["status"].Visible = false;
                            report.Parameters["testcode"].Value = "DEF";
                            report.Parameters["testcode"].Visible = false;

                            // Close of parameter
                            report.Parameters["Visitnum"].Value = "DEF";
                            report.Parameters["Visitnum"].Visible = false;
                            report.Parameters["Hosnumber"].Value = "DEF";
                            report.Parameters["Hosnumber"].Visible = false;
                            //

                            string validateType = "";
                            if (chkStatusOfRequest == "1")
                            {
                                validateType = "PRELIMINARY REPORTED: ";
                            }
                            else if (chkStatusOfRequest == "2")
                            {
                                validateType = "FINAL REPORTED: ";
                            }

                            report.Parameters["validateType"].Value = validateType;
                            report.Parameters["validateType"].Visible = false;
                            documentViewer1.DocumentSource = report;
                            report.CreateDocument();
                        }

                        else
                        {
                            documentViewer1.Refresh();
                        }
                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถแสดง Report กรุณาตรวจสอบการออกผล", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("ยังไม่ได้คลิกเลือกรายการที่จะแสดง", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form_config FM = new Form_config();
            FM.ShowDialog();
        }

        private string[] Get_Result_ReportFormat(string Str_PROTOCOLCODE)
        {
            string[] Result = new string[2];

            try
            {
                MBReportController objConfig = new MBReportController();
                DataTable dt = null;

                objMBReportM = new MBReportM();

                objMBReportM.PROTOCOLCODE = Str_PROTOCOLCODE;
                dt = objConfig.Get_ReportFormat(objMBReportM);

                if (dt.Rows.Count > 0)
                {
                    Result[0] = dt.Rows[0]["REPORTFORMAT"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Result;
        }

    }
}
