using System;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using DevExpress.XtraEditors;
using UniquePro.Controller;

namespace UNIQUE
{
    public partial class Template_Grams : DevExpress.XtraReports.UI.XtraReport
    {
        SqlConnection conn;

        private MBReportController objConfig = new MBReportController();
        private MBReportM objMBReportM;

        string subRequestsID = "";

        // Format DateTime en-US
        System.Globalization.CultureInfo cultureinfo_TH = new System.Globalization.CultureInfo("th-TH");
        System.Globalization.CultureInfo cultureinfo_ENG = new System.Globalization.CultureInfo("en-US");
        string Str_calendar = Properties.Settings.Default.Calendar;

        string format_DT = "dd/MM/yyyy HH:mm";

        public Template_Grams()
        {
            InitializeComponent();
        }

        private void XtraReport_GramS_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                conn = new Connection_ORM().Connect();
                objMBReportM = new MBReportM();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }

            ////////catch (SqlException ex)
            ////////{ 
            ////////    Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Grams error method XtraReport_GramS_BeforePrint() : " + ex.Message.ToString());
            ////////    XtraMessageBox.Show(ex.Message.ToString());
            ////////}

            try
            {
                string sql = @"SELECT DISTINCT
  REQUESTS.ACCESSNUMBER
, REQUESTS.COMMENT
, MB_REQUESTS.MBREQNUMBER
, MB_STAINS_OBSERVATIONS.MORPHOBSERVATIONID
, REQUESTS.REQUESTID
, PATIENTS.PATNUMBER AS 'HN'
, PATIENTS.NAME
, PATIENTS.LASTNAME
, PATIENTS.BIRTHDATE
, CASE (PATIENTS.SEX) WHEN 1 THEN'M' WHEN 2 THEN 'F' ELSE '-' END AS 'SEX'
, PATIENTS.BIRTHDATE
, SUBREQMB_STAINS.SUBREQMBSTAINID
, SUBREQMB_STAINS.COLONYID
, SUBREQMB_STAINS.SUBREQUESTID
, SUBREQMB_STAINS.MBSTAINID
, SUBREQMB_STAINS.CREATEUSER
, SUBREQMB_STAINS.CREATIONDATE
, SUBREQMB_STAINS.LOGUSERID
, SUBREQMB_STAINS.LOGDATE
, SUBREQMB_STAINS.NOTPRINTABLE
, DICT_MB_STAINS.MBSTAINID
, DICT_MB_STAINS.MBSTAINCODE
, DICT_MB_STAINS.STAINNAME
, MB_STAINS_OBSERVATIONS.SUBREQMBSTAINID
, MB_STAINS_OBSERVATIONS.QUANTITATIVERESULT
, MB_STAINS_OBSERVATIONS.MORPHOBSERVATION
, REQUESTS.RECEIVEDDATE
, REQUESTS.REQDATE
, REQUESTS.COLLECTIONDATE
, DICT_COLL_MATERIALS.COLLMATERIALCODE
, DICT_COLL_MATERIALS.COLLMATERIALTEXT
, DICT_MB_PROTOCOLS.PROTOCOLTEXT
, MB_REQUESTS.MBREQNUMBER
, USERS.USERNAME
, MB_ACTIONS.ACTIONTYPE
, DICT_LOCATIONS.LOCNAME
, DICT_DOCTORS.DOCNAME
, REQUESTS.HOSTORDERNUMBER
, MB_ACTIONS.ACTIONDATE AS 'VALIDATED'

  FROM SUBREQMB_STAINS
  LEFT OUTER JOIN DICT_MB_STAINS ON SUBREQMB_STAINS.MBSTAINID = DICT_MB_STAINS.MBSTAINID
  LEFT OUTER JOIN MB_STAINS_OBSERVATIONS ON SUBREQMB_STAINS.SUBREQMBSTAINID = MB_STAINS_OBSERVATIONS.SUBREQMBSTAINID
  LEFT OUTER JOIN MB_REQUESTS ON MB_REQUESTS.MBREQUESTID = SUBREQMB_STAINS.SUBREQUESTID
  LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID
  LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
  LEFT OUTER JOIN DICT_COLL_MATERIALS ON MB_REQUESTS.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID
  LEFT OUTER JOIN DICT_MB_PROTOCOLS ON MB_REQUESTS.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID
  LEFT OUTER JOIN MB_ACTIONS ON MB_REQUESTS.MBREQUESTID = MB_ACTIONS.SUBREQUESTID
  LEFT OUTER JOIN DICT_LOCATIONS ON REQUESTS.REQLOCATION = DICT_LOCATIONS.LOCCODE
  LEFT OUTER JOIN DICT_DOCTORS ON REQUESTS.REQDOCTOR = DICT_DOCTORS.DOCCODE
  LEFT OUTER JOIN USERS ON MB_ACTIONS.LOGUSERID = USERS.USERID

  WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "'" +
"AND MB_REQUESTS.MBREQNUMBER = '" + potocalnumber.Value.ToString() + "'" +
"AND MB_ACTIONS.ACTIONTYPE =" + status.Value.ToString() +
"--AND SUBREQMB_STAINS.NOTPRINTABLE != 0" +
"ORDER BY MB_STAINS_OBSERVATIONS.MORPHOBSERVATIONID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds.Clear();
                cmd.ExecuteNonQuery();
                adap.Fill(ds, "result");
                DataTable dt = new DataTable();

                StringBuilder bdGramS = new StringBuilder();
                StringBuilder bdGramS_text = new StringBuilder();

                // get the data from the database
                SqlDataReader rdr = cmd.ExecuteReader();

                // load into a datatable for processing
                DataTable tbl = new DataTable();
                tbl.Load(rdr);
                DataTable pivotData1 = new DataTable();

                string strQuantity = "";
                string strMopho = "";

                if (ds.Tables["result"].Rows.Count > 0)
                {
                    subRequestsID = ds.Tables["result"].Rows[0]["SUBREQUESTID"].ToString();

                    for (int i = 0; i < ds.Tables["result"].Rows.Count; i++)
                    {
                        if (ds.Tables["result"].Rows[i]["MORPHOBSERVATION"].ToString() != "XCANCR".Trim())
                        {
                            if (ds.Tables["result"].Rows[i]["QUANTITATIVERESULT"].ToString() != "XCANCR".Trim())
                            {
                                strQuantity += ds.Tables["result"].Rows[i]["QUANTITATIVERESULT"].ToString() + "\r\n";
                                strMopho += "       " + ds.Tables["result"].Rows[i]["MORPHOBSERVATION"].ToString() + "\r\n";
                            }
                            else
                            {
                                strMopho += "       " + ds.Tables["result"].Rows[i]["MORPHOBSERVATION"].ToString() + "\r\n";
                            }
                        }
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////// Comment CODE OR COMMENT FREE TEXT //////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////
                    //// Check comment by request CODE Comment
                    //for (int k = 0; k < ds.Tables["result"].Rows.Count; k++)
                    //{
                    //    // Check comment by request
                    //    comment_request = ds.Tables["result"].Rows[k]["COMMENTS"].ToString();

                    //    // Check comment request
                    //    if (!comment_request.Equals(""))
                    //    {
                    //        break;
                    //    }
                    //}

                    //// check comment if not null
                    //if (!comment_request.Equals(""))
                    //{
                    //    xrLabel_comment2.Text = comment_request;
                    //}
                    //else
                    //{
                    //    // Check comment by request Comment Freetext
                    //    for (int j = 0; j < ds.Tables["result"].Rows.Count; j++)
                    //    {
                    //        // Check comment by request
                    //        comment_request = ds.Tables["result"].Rows[j]["COMMENT_FREE"].ToString();

                    //        // ... Switch on the string. check  date
                    //        try
                    //        {
                    //            string comment_request_start = comment_request[0] + "" + comment_request[1];
                    //            int number = Int32.Parse(comment_request_start);
                    //            comment_request = "";
                    //        }
                    //        catch (Exception)
                    //        {
                    //            // Check comment request
                    //            if (!comment_request.Equals("")) break;
                    //        }
                    //    }

                    //    // Check comment by request
                    //    if (!comment_request.Equals(""))
                    //    {
                    //        xrLabel_comment2.Text = comment_request;
                    //    }
                    //}
                    //////////////////////////////////////////// END Comment CODE OR COMMENT FREE TEXT  //////////////////////////



                    string Str_ACCESSNUMBER = accessnumber.Value.ToString();
                    string Str_PROTOCOLNUM = potocalnumber.Value.ToString();
                    string Str_STATUS = status.Value.ToString();
                    string Str_STATUS_4 = "4";    // Report by or Last save

                    string[] Get_DetailReport1 = Get_Array_report1(Str_PROTOCOLNUM, Str_ACCESSNUMBER, Str_STATUS);
                    string[] Get_DetailReport2 = Get_Array_report1(Str_PROTOCOLNUM, Str_ACCESSNUMBER, Str_STATUS_4);

                    // Get_DetailReport1
                    // [0] = NAME
                    // [1] = LASTNAME
                    // [2] = CERTIFICATE
                    // [3] = ACTION DATE/TIME
                    // [4] = DOCNAME
                    // [5] = LOCNAME
                    // [6] = REQTEST_SPECIMEN
                    // [7] = GROUP_SPECIMENNAME
                    // [8] = TITLE1
                    // [9] = TITLE2


                    xr_hn.Text = ds.Tables["result"].Rows[0]["HN"].ToString();
                    xr_Name.Text = Get_DetailReport1[8] + Get_DetailReport1[9] + " " + Get_DetailReport1[0] + " " + Get_DetailReport1[1];
                    xr_Labno.Text = ds.Tables["result"].Rows[0]["ACCESSNUMBER"].ToString();

                    xr_Specimen.Text = Get_DetailReport1[6];
                    xrRequestBy.Text = Get_DetailReport1[4];
                    xr_Location.Text = Get_DetailReport1[5];
                    //
                    //xrReceiveBy.Text =
                    xr_CommentReq.Text = ds.Tables["result"].Rows[0]["COMMENT"].ToString();
                    xrBarCode_samplenumber.Text = accessnumber.Value.ToString();
                    xrProtocol.Text = potocalnumber.Value.ToString();

                    // * Report BY get Status in MB_ACTION.ACTIONTYPE = 4
                    xrReport_by.Text = Get_DetailReport2[0] + " " + Get_DetailReport2[1] + " (" + Get_DetailReport2[2] + ")";
                    xrReported_date.Text = String.Format("{0:dd/MM/yyyy HH:mm}", Get_DetailReport2[3]);

                    // * Approved Report BY get Status in MB_ACTION.ACTIONTYPE = 1,2
                    xrApprove_by.Text = Get_DetailReport1[0] + " " + Get_DetailReport1[1] + " (" + Get_DetailReport1[2] + ")";
                    xr_TimeApproved.Text = String.Format("{0:dd/MM/yyyy HH:mm}", Get_DetailReport1[3]);

                    DateTime _DOB = Convert.ToDateTime(ds.Tables["result"].Rows[0]["BIRTHDATE"].ToString());

                    if (Str_calendar == "TH")
                    {
                        int _DOB_1 = Convert.ToInt32(_DOB.ToString("yyyy"));
                        int _DOB_2 = _DOB_1 + 543;

                        xr_DOB.Text = _DOB.ToString("dd/MM/") + _DOB_2;

                        xrReceived_date.Text = ((DateTime)(ds.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd/MM/yyyy HH:mm", cultureinfo_TH);
                        xrCompletedate.Text = ((DateTime)(ds.Tables["result"].Rows[0]["VALIDATED"])).ToString("dd/MM/yyyy HH:mm", cultureinfo_TH);
                        xrCollectionTime.Text = ((DateTime)(ds.Tables["result"].Rows[0]["COLLECTIONDATE"])).ToString("dd/MM/yyyy HH:mm", cultureinfo_TH);
                        xrReceived_date.Text = ((DateTime)(ds.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd/MM/yyyy HH:mm", cultureinfo_TH);
                        xr_Printingdate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm", cultureinfo_TH);
                    }
                    else
                    {
                        //xr_DOB.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ds.Tables["result"].Rows[0]["BIRTHDATE"].ToString(), cultureinfo_ENG));
                        xr_DOB.Text = _DOB.ToString("dd/MM/yyyy");
                        xrReceived_date.Text = ((DateTime)(ds.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd/MM/yyyy HH:mm", cultureinfo_ENG);
                        xrCompletedate.Text = ((DateTime)(ds.Tables["result"].Rows[0]["VALIDATED"])).ToString("dd/MM/yyyy HH:mm", cultureinfo_ENG);
                        xrCollectionTime.Text = ((DateTime)(ds.Tables["result"].Rows[0]["COLLECTIONDATE"])).ToString("dd/MM/yyyy HH:mm", cultureinfo_ENG);
                        xrReceived_date.Text = ((DateTime)(ds.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd/MM/yyyy HH:mm", cultureinfo_ENG);
                        xr_Printingdate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", cultureinfo_ENG);
                    }


                    string Str_DOB = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(ds.Tables["result"].Rows[0]["BIRTHDATE"].ToString()));
                    string Str_DateOfBirth = "";
                    // Start Date of Birth in calcu
                    if (Str_DOB == null || Str_DOB == "")
                    {
                        Str_DateOfBirth = "-";
                    }
                    else
                    {
                        string[] Array_DOB = Str_DOB.Split("-".ToCharArray());

                        int Str_Year = Convert.ToInt32(Array_DOB[2]);
                        int Str_Month = Convert.ToInt32(Array_DOB[1]);
                        int Str_Day = Convert.ToInt32(Array_DOB[0]);

                        DateTime myDate = new DateTime(Str_Year, Str_Month, Str_Day);
                        DateTime ToDate = DateTime.Now;
                        DateDifference dDiff = new DateDifference(myDate, ToDate);

                        Str_DateOfBirth = dDiff.ToString();
                    }
                    xr_age.Text = Str_DateOfBirth;
                    // END Date of Birth in calcu

                    if (Str_STATUS == "1")
                    {
                        xr_Statusreport.Text = "Plilimnary";
                    }
                    else
                    {
                        xr_Statusreport.Text = "Final Report";
                    }

                    //xr_hn.Text = ds.Tables["result"].Rows[0]["HN"].ToString();
                    //xr_hn.Text = xr_hn.Text.TrimStart('0');
                    //xr_Name.Text = ds.Tables["result"].Rows[0]["NAME"].ToString() + " " + ds.Tables["result"].Rows[0]["LASTNAME"].ToString();
                    //xrTableCell_Specimen.Text = ds.Tables["result"].Rows[0]["MATERIALSTEXT"].ToString();
                    //xrTableCell_Specimen.Text = ds.Tables["result"].Rows[0]["MATERIALSTEXT"].ToString();
                    //xrTableCell_Date.Text = ((DateTime)(ds.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd/MM/yyyyy", cultureinfo);
                    //xrTableCell_RequestedDateTime.Text = ((DateTime)(ds.Tables["result"].Rows[0]["REQDATE"])).ToString(format_DT, cultureinfo);
                    //xrReceived_date.Text = ((DateTime)(ds.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString(format_DT, cultureinfo);
                    //xrTableCell_CompleteDateTime.Text = ((DateTime)(ds.Tables["result"].Rows[0]["VALIDATED"])).ToString(format_DT, cultureinfo);
                    //lblGroupTest.Text = "" + ds.Tables["result"].Rows[0]["PROTOCOLTEXT"].ToString() + "";
                    //xrProtocol.Text = ds.Tables["result"].Rows[0]["MBREQNUMBER"].ToString();
                    //xrTableCell12.Text = ds.Tables["result"].Rows[0]["MBREQNUMBER"].ToString();

                    string sex = "";
                    if (ds.Tables["result"].Rows[0]["SEX"].ToString().Equals("M"))
                    {
                        sex = "Male";
                    }
                    else if (ds.Tables["result"].Rows[0]["SEX"].ToString().Equals("F"))
                    {
                        sex = "Female";
                    }
                    else
                    {
                        sex = "-";
                    }
                    xr_sex.Text = sex;

                    //string str_AgeY = ds.Tables["result"].Rows[0]["ageY"].ToString() + " Year(s)";
                    //string str_AgeM = ds.Tables["result"].Rows[0]["ageM"].ToString() + " Month(s)";
                    //xr_age.Text = str_AgeY + " " + str_AgeM;

                    //xrTableCell_Doc.Text = ds.Tables["result"].Rows[0]["DOCNAME"].ToString();
                    //xr_Location.Text = ds.Tables["result"].Rows[0]["LOCNAME"].ToString();


                    /*==================================================
                   * CHECK USERS VALIDATED and APPROVED.
                   * REM: Clinical Validate = Approved by.
                   * REM: Last Save in WIP  = Report by.
                   ====================================================*/
//                    if (subRequestsID != "")
//                    {
//                        string sqlVal = @"SELECT 
// MB_ACTIONS.MBACTIONID
//,MB_ACTIONS.SUBREQUESTID
//,MB_ACTIONS.ACTIONTYPE
//,MB_ACTIONS.ACTIONDATE
//,MB_ACTIONS.LOGUSERID
//,USERS.NAME
//,USERS.LASTNAME
//FROM MB_ACTIONS
//LEFT OUTER JOIN USERS ON MB_ACTIONS.LOGUSERID = USERS.USERID 
//WHERE SUBREQUESTID = '" + subRequestsID + "' AND  ( ACTIONTYPE = 2 )  ORDER BY MB_ACTIONS.MBACTIONID DESC";

//                        SqlCommand cmdVal = new SqlCommand(sqlVal, conn);
//                        DataSet dsVal = new DataSet();
//                        SqlDataAdapter adp = new SqlDataAdapter(cmdVal);
//                        if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
//                        dsVal.Clear();
//                        adp.Fill(dsVal, "result");
//                        string dsCount = dsVal.Tables["result"].Rows.Count.ToString();
//                        if (dsVal.Tables["result"].Rows.Count > 0)
//                        {
//                            //for (int i = 1; i <= dsVal.Tables["result"].Rows.Count; i++)
//                            //{

//                            if (dsCount == "1")
//                            {
//                                if (dsVal.Tables["result"].Rows[0]["LOGUSERID"].ToString() != "")
//                                {
//                                    xrApprove_by.Text = dsVal.Tables["result"].Rows[0]["NAME"].ToString() + " " + dsVal.Tables["result"].Rows[0]["LASTNAME"].ToString();
//                                    xrApproved_date.Text = ((DateTime)(dsVal.Tables["result"].Rows[0]["ACTIONDATE"])).ToString(format_DT, cultureinfo);

//                                }
//                                else
//                                {
//                                    if (dsVal.Tables["result"].Rows[0]["LOGUSERID"].ToString() != "SYS")
//                                    {
//                                        xrApprove_by.Text = dsVal.Tables["result"].Rows[0]["LOGUSERID"].ToString();
//                                    }
//                                    else
//                                    {
//                                        xrApprove_by.Text = dsVal.Tables["result"].Rows[0]["NAME"].ToString() + " " + dsVal.Tables["result"].Rows[0]["LASTNAME"].ToString();
//                                        xrApproved_date.Text = ((DateTime)(dsVal.Tables["result"].Rows[0]["ACTIONDATE"])).ToString(format_DT, cultureinfo);
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                if (dsVal.Tables["result"].Rows[0]["LOGUSERID"].ToString() != "")
//                                {
//                                    xrApprove_by.Text = dsVal.Tables["result"].Rows[0]["NAME"].ToString() + " " + dsVal.Tables["result"].Rows[0]["LASTNAME"].ToString();
//                                    xrApproved_date.Text = ((DateTime)(dsVal.Tables["result"].Rows[0]["ACTIONDATE"])).ToString(format_DT, cultureinfo);
//                                }
//                                else
//                                {
//                                    xrApprove_by.Text = dsVal.Tables["result"].Rows[0]["LOGUSERID"].ToString();
//                                }
//                            }

//                            // Report Technical
//                            string sqlTech = @"SELECT 
// MB_ACTIONS.MBACTIONID
//,MB_ACTIONS.SUBREQUESTID
//,MB_ACTIONS.ACTIONTYPE
//,MB_ACTIONS.ACTIONDATE
//,MB_ACTIONS.LOGUSERID
//,USERS.NAME
//,USERS.LASTNAME
//FROM MB_ACTIONS
//LEFT OUTER JOIN USERS ON MB_ACTIONS.LOGUSERID = USERS.USERID 
//WHERE SUBREQUESTID = '" + subRequestsID + "' AND  ( ACTIONTYPE = 3 )  ORDER BY MB_ACTIONS.MBACTIONID DESC";

//                            SqlCommand cmdTech = new SqlCommand(sqlTech, conn);
//                            DataSet dsTech = new DataSet();
//                            SqlDataAdapter adpTech = new SqlDataAdapter(cmdTech);
//                            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
//                            dsTech.Clear();
//                            adpTech.Fill(dsTech, "result");
//                            if (dsTech.Tables["result"].Rows.Count > 0)
//                            {
//                                if (dsTech.Tables["result"].Rows[0]["LOGUSERID"].ToString() != "")
//                                {
//                                    xrReport_by.Text = dsTech.Tables["result"].Rows[0]["NAME"].ToString() + " " + dsTech.Tables["result"].Rows[0]["LASTNAME"].ToString();
//                                    //
//                                    xrReported_date.Text = ((DateTime)(dsTech.Tables["result"].Rows[0]["ACTIONDATE"])).ToString(format_DT, cultureinfo);
//                                }
//                                else
//                                {
//                                    xrReport_by.Text = dsTech.Tables["result"].Rows[0]["NAME"].ToString();
//                                    // Fix Report Technical Date time 
//                                    //
//                                    xrReported_date.Text = ((DateTime)(dsTech.Tables["result"].Rows[0]["ACTIONDATE"])).ToString(format_DT, cultureinfo);
//                                }
//                            }

//                        }

//                    } // end of check val.             
                }


                xrTableCell1.Text = strQuantity;
                xrTableCell2.Text = strMopho;

                // Check Clinical not Technical
                //string tech_report = xrReport_by.Text;
                //string clinical_report = xrTableCell_Approve.Text;
                //if (tech_report.Equals("") && !clinical_report.Equals(""))
                //{
                //    // set technical
                //    xrReport_by.Text = clinical_report;
                //}
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }
        private string[] Get_Array_report1(string Str_PROTOCOLNUM, string Str_ACCESSNUMBER, string Str_STATUS)
        {
            string[] Result = new string[10];

            try
            {
                MBReportController objConfig = new MBReportController();
                DataTable dt = null;

                objMBReportM = new MBReportM();

                objMBReportM.PROTOCOLNUM = Str_PROTOCOLNUM;
                objMBReportM.ACCESSNUMBER = Str_ACCESSNUMBER;
                objMBReportM.STATUS = Str_STATUS;

                dt = objConfig.Get_Info_Report1(objMBReportM);

                if (dt.Rows.Count > 0)
                {
                    Result[0] = dt.Rows[0]["NAME"].ToString();
                    Result[1] = dt.Rows[0]["LASTNAME"].ToString();
                    Result[2] = dt.Rows[0]["CERTIFICATE"].ToString();
                    Result[3] = dt.Rows[0]["ACTIONDATE"].ToString();
                    Result[4] = dt.Rows[0]["DOCNAME"].ToString();
                    Result[5] = dt.Rows[0]["LOCNAME"].ToString();
                    Result[6] = dt.Rows[0]["REQTEST_SPECIMEN"].ToString();
                    Result[7] = dt.Rows[0]["GROUP_SPECIMENNAME"].ToString();
                    Result[8] = dt.Rows[0]["TITLE1"].ToString();
                    Result[9] = dt.Rows[0]["TITLE2"].ToString();

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
