using System;
using System.Drawing;
using System.Collections;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using DevExpress.XtraPivotGrid;
using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using UniquePro.Controller;

namespace UNIQUE
{
    /*
protocolnumber.Contains("P01") 
protocolnumber.Contains("P02") 
protocolnumber.Contains("P04") 
protocolnumber.Contains("P07") 
protocolnumber.Contains("P17") 
protocolnumber.Contains("P18") 
protocolnumber.Contains("P19") 
protocolnumber.Contains("P20") 
protocolnumber.Contains("P21") 
protocolnumber.Contains("P22") 
protocolnumber.Contains("P23") 
protocolnumber.Contains("P24") 
protocolnumber.Contains("P28") 
protocolnumber.Contains("P32") 
protocolnumber.Contains("P33")) 
     * 
     * 
     * */


    public partial class Template_Culture : DevExpress.XtraReports.UI.XtraReport
    {
        SqlConnection conn;

        private MBReportController objConfig = new MBReportController();
        private MBReportM objMBReportM;

        string subRequestsID ="";

        string strTopology = "";
        string strOganismns_Compare_topo = "";
        string strGroupTest_Protocol = "";

        DataSet ds_special;
        DataSet ds_queryQutity_specialH3;

        string detectionTestShowAfterOrg = "";
        string strOBX = "";
        private Boolean status_prelim = false;

        // comment request
        string comment_request = "";

        // P04 Special check after MGIT Media result.   :CODE CASE : P04_0001
        bool Bool_P04Special = false;
        // END :CODE CASE : P04_0001
        // Format DateTime en-US

        System.Globalization.CultureInfo cultureinfo_TH = new System.Globalization.CultureInfo("th-TH");
        System.Globalization.CultureInfo cultureinfo_ENG = new System.Globalization.CultureInfo("en-US");
        string Str_calendar = Properties.Settings.Default.Calendar;

        string format_DT = "dd/MM/yyyy HH:mm:ss";
        string format_DT_Eng = "dd-MM-yyyy";

        public Template_Culture()
        {
            InitializeComponent();
        }

        private void XtraReport4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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

            xrPivotGrid1.BestFit(PivotValue);
            xrPivotGrid1.BestFit(PivotSIR);
            xrPivotGrid1.BestFit(pivotAntibiotic);

            xrTable_organ.Rows.Clear();
            xrTable5.Rows.Clear();

            /*====================
             * 
             * 
             * Sample meterial is "MSU" then not report some Antibiotic Fam.
             * 
             */

            try
            {

                string sql = "";
                sql = @" SELECT 
  DICT_MB_ANTIBIOTIC_GROUP.ANTIBIOTICSFAMILYID
, SUBREQMB_ORGANISMS.ORGANISMINDEX
, PATIENTS.PATNUMBER AS 'HN'
, PATIENTS.NAME
, PATIENTS.SEX
, PATIENTS.LASTNAME
, REQUESTS.REQUESTID
, REQUESTS.ACCESSNUMBER
, REQUESTS.COMMENT
, PATIENTS.BIRTHDATE
, REQUESTS.PATID
, MB_REQUESTS.MBREQUESTID
, MB_REQUESTS.COLLECTIONDATE
, MB_REQUESTS.COLLMATERIALID
, SUBREQMB_ORGANISMS.SUBREQMBORGID
, SUBREQMB_ORGANISMS.ORGANISMID
, SUBREQMB_ORGANISMS.IDENTDATE
, SUBREQMB_ANTIBIOTICS.SUBREQMBANTIBIOTICID
, SUBREQMB_ANTIBIOTICS.ANTIBIOTICID
, DICT_MB_ORGANISMS.ORGANISMNAME as 'ORGANISMSNAME'
, DICT_MB_ANTIBIOTICS.ANTIBIOTICCODE
, DICT_MB_ANTIBIOTICS.FULLTEXT as 'ANTIBIOTICSNAME'
, CASE ( SUBREQMB_ANTIBIOTICS.UNITS ) WHEN '0' THEN 'mg/L' WHEN '1' THEN 'mm' ELSE '' END AS 'UNITS'
, REQUESTS.RECEIVEDDATE AS 'RECEIVEDDATE'
, REQUESTS.REQDATE AS 'REQDATE'
, MB_ACTIONS.LOGUSERID
, USERS.USERNAME
--, RIGHT (SUBREQMB_COLONIES.COLONYNUMBER, 3) AS 'COLONYNUMBER'
, SUBREQMB_ANTIBIOTICS.RESULT AS 'RESULT' 
--, SUBREQMB_ANTIBIOTICS.RESULT as 'res'
, SUBREQMB_ANTIBIOTICS.RESULTVALUE
, '('+  SUBREQMB_ANTIBIOTICS.MINIMUM +' - '+ SUBREQMB_ANTIBIOTICS.MAXIMUM +')' as 'breakpoints'
, CASE (MB_ACTIONS.ACTIONTYPE) WHEN 1 THEN MB_ACTIONS.ACTIONTYPE WHEN 2 THEN MB_ACTIONS.ACTIONTYPE WHEN 3 THEN MB_ACTIONS.ACTIONTYPE WHEN 4 THEN MB_ACTIONS.ACTIONTYPE END AS 'REQSTATUS'
, MB_REQUESTS.MBREQNUMBER
, DICT_COLL_MATERIALS.COLLMATERIALCODE
, DICT_COLL_MATERIALS.COLLMATERIALTEXT AS 'MATERIALSTEXT'
, DICT_MB_PROTOCOLS.PROTOCOLCODE
, DICT_MB_PROTOCOLS.PROTOCOLTEXT
--, SUBREQMB_DET_TESTS.TESTRESULT
--, SUBREQMB_ANTIBIOTICS.UNITS
, MB_ACTIONS.ACTIONDATE AS 'VALIDATED'
, DICT_LOCATIONS.LOCNAME
--, SUBREQMB_COLONIES.COLONYINDEX
, DICT_DOCTORS.DOCCODE
, DICT_DOCTORS.DOCNAME
, DICT_DOCTORS.ADDRESS1 AS 'DOCNAME2'
, SUBREQMB_ANTIBIOTICS.NOTPRINTABLE 
, SUBREQMB_OCOM.COMMENTTEXT as 'COMMENT_FREE' 
, DICT_MB_ANTIBIOTIC_GROUP.FAMNAME as 'ANTIBIOTICS_FAM'
FROM REQUESTS 
LEFT OUTER JOIN MB_REQUESTS ON (REQUESTS.REQUESTID = MB_REQUESTS.REQUESTID) 
LEFT OUTER JOIN SUBREQMB_ORGANISMS ON (MB_REQUESTS.MBREQUESTID = SUBREQMB_ORGANISMS.SUBREQUESTID)
LEFT OUTER JOIN DICT_MB_ORGANISMS ON (SUBREQMB_ORGANISMS.ORGANISMID = DICT_MB_ORGANISMS.ORGANISMID)
LEFT OUTER JOIN SUBREQMB_ANTIBIOTICS ON (SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_ANTIBIOTICS.SUBREQMBORGID) 
--LEFT OUTER JOIN SUBREQMB_ANTIBIOTICS ON (MB_REQUESTS.MBREQUESTID = SUBREQMB_ANTIBIOTICS.SUBREQUESTID) 
LEFT OUTER JOIN DICT_MB_ANTIBIOTICS ON (SUBREQMB_ANTIBIOTICS.ANTIBIOTICID = DICT_MB_ANTIBIOTICS.ANTIBIOTICID) 
LEFT OUTER JOIN DICT_MB_ANTIBIOTIC_GROUP ON (DICT_MB_ANTIBIOTICS.ANTIBIOTICSFAMILYID = DICT_MB_ANTIBIOTIC_GROUP.ANTIBIOTICSFAMILYID) 
LEFT OUTER JOIN MB_ACTIONS ON MB_REQUESTS.MBREQUESTID = MB_ACTIONS.SUBREQUESTID
LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID 
LEFT OUTER JOIN DICT_MB_PROTOCOLS ON MB_REQUESTS.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
LEFT OUTER JOIN DICT_COLL_MATERIALS ON MB_REQUESTS.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID 
LEFT OUTER JOIN USERS ON USERS.USERID = MB_ACTIONS.LOGUSERID 
--LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_COLONIES.SUBREQMBORGID 
--LEFT OUTER JOIN SUBREQMB_DET_TESTS ON SUBREQMB_COLONIES.COLONYID = SUBREQMB_DET_TESTS.COLONYID 
LEFT OUTER JOIN DICT_LOCATIONS ON REQUESTS.REQLOCATION = DICT_LOCATIONS.LOCCODE 
LEFT OUTER JOIN SUBREQMB_OCOM on MB_REQUESTS.MBREQUESTID = SUBREQMB_OCOM.SUBREQUESTID 
LEFT OUTER JOIN DICT_DOCTORS ON REQUESTS.REQDOCTOR  = DICT_DOCTORS.DOCCODE
WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "' AND MB_REQUESTS.MBREQNUMBER = '" + potocalnumber.Value.ToString() + "'" +
"AND MB_ACTIONS.ACTIONTYPE = '" + status.Value.ToString() + "' AND SUBREQMB_ANTIBIOTICS.RESULTVALUE is not null" +
// --" AND ((SUBREQMB_ANTIBIOTICS.RESULT != 0 ) AND  (SUBREQMB_ANTIBIOTICS.NOTPRINTABLE = 0)  OR ( DICT_MB_ANTIBIOTICS.ANTIBIOTICCODE = 'STF1') )  " +
@" GROUP BY
  DICT_MB_ANTIBIOTIC_GROUP.ANTIBIOTICSFAMILYID
, SUBREQMB_ANTIBIOTICS.ANTIBIOTICID
, SUBREQMB_ORGANISMS.ORGANISMINDEX
, PATIENTS.PATNUMBER
, PATIENTS.NAME
, PATIENTS.SEX
, PATIENTS.LASTNAME
, REQUESTS.REQUESTID
, REQUESTS.ACCESSNUMBER
, PATIENTS.BIRTHDATE
, REQUESTS.PATID
, MB_REQUESTS.MBREQUESTID
, MB_REQUESTS.COLLECTIONDATE
, MB_REQUESTS.COLLMATERIALID
, SUBREQMB_ORGANISMS.SUBREQMBORGID
, SUBREQMB_ORGANISMS.ORGANISMID
, SUBREQMB_ORGANISMS.IDENTDATE
, SUBREQMB_ANTIBIOTICS.SUBREQMBANTIBIOTICID
, SUBREQMB_ANTIBIOTICS.ANTIBIOTICID
--, DICT_MB_ORGANISMS.ORGANISMNAME
, DICT_MB_ORGANISMS.ORGANISMNAME
, DICT_MB_ANTIBIOTICS.ANTIBIOTICCODE
, DICT_MB_ANTIBIOTICS.FULLTEXT
, SUBREQMB_ANTIBIOTICS.UNITS
, REQUESTS.RECEIVEDDATE
, REQUESTS.REQDATE
, REQUESTS.COMMENT
, MB_ACTIONS.LOGUSERID
, USERS.USERNAME
--, SUBREQMB_COLONIES.COLONYNUMBER
, SUBREQMB_ANTIBIOTICS.RESULT
, SUBREQMB_ANTIBIOTICS.RESULTVALUE
, SUBREQMB_ANTIBIOTICS.MINIMUM
, SUBREQMB_ANTIBIOTICS.MAXIMUM
, MB_ACTIONS.ACTIONTYPE
, MB_REQUESTS.MBREQNUMBER
, DICT_COLL_MATERIALS.COLLMATERIALCODE
, DICT_COLL_MATERIALS.COLLMATERIALTEXT
, DICT_MB_PROTOCOLS.PROTOCOLCODE
, DICT_MB_PROTOCOLS.PROTOCOLTEXT
, MB_ACTIONS.ACTIONDATE
--, SUBREQMB_DET_TESTS.TESTRESULT
--, SUBREQMB_ANTIBIOTICS.UNITS
, DICT_LOCATIONS.LOCNAME
--, SUBREQMB_COLONIES.COLONYINDEX
, DICT_DOCTORS.DOCCODE
, DICT_DOCTORS.DOCNAME
, DICT_DOCTORS.ADDRESS1
, DICT_MB_ANTIBIOTIC_GROUP.FAMNAME
, SUBREQMB_ANTIBIOTICS.NOTPRINTABLE
, SUBREQMB_OCOM.COMMENTTEXT";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds.Clear();
                adp.Fill(ds, "result");

                xrPivotGrid1.DataSource = ds;
                xrPivotGrid1.DataMember = "result";

                if (ds.Tables["result"].Rows.Count > 0)
                {
                    // Check MID  Sensitivities
                    //string res = "";
                    //string res_val = "";
                    //string res_units = "";

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
                        xr_DOB.Text =  _DOB.ToString("dd/MM/yyyy");
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



                    //// Search Hospitalnumber
                    //ReportConfigurationController objConfig = new ReportConfigurationController();
                    //DataTable dt = null;
                    //AllVaraibleM objAllVaraibleM = new AllVaraibleM();

                    //try
                    //{
                    //    objAllVaraibleM.GET_Accessnumber = accessnumber.Value.ToString();
                    //    dt = objConfig.Get_Hospitalnumber(objAllVaraibleM);

                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        if (dt.Rows[0]["HOSTORDERNUMBER"].ToString() == null || String.IsNullOrWhiteSpace(dt.Rows[0]["HOSTORDERNUMBER"].ToString()))
                    //        {
                    //            xrHosnumber.Text = "-";
                    //        }
                    //        else
                    //        {
                    //            xrHosnumber.Text = dt.Rows[0]["HOSTORDERNUMBER"].ToString();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        xrHosnumber.Text = "-";
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw new Exception(ex.Message);
                    //}
                    //finally
                    //{
                    //    objAllVaraibleM = null;
                    //}


                    //for (int i = 0; i < ds.Tables["result"].Rows.Count; i++)
                    //{
                    //    res = ds.Tables["result"].Rows[i]["RESULT"].ToString();
                    //    res_val = ds.Tables["result"].Rows[i]["RESULTVALUE"].ToString();
                    //    res_units = ds.Tables["result"].Rows[i]["UNITS"].ToString();

                    //    if (!res.Equals(""))
                    //    {
                    //        if (res_units.Equals("mm"))
                    //        {
                    //            ds.Tables["result"].Rows[i]["RESULTVALUE"] = "";
                    //            ds.Tables["result"].Rows[i]["UNITS"] = "";
                    //        }
                    //    }
                    //} // End Check MID Sensitivities 

                    GroupHeader1.Visible = true;
                    GroupHeader6.Visible = true;
                    subRequestsID = ds.Tables["result"].Rows[0]["MBREQUESTID"].ToString();

                    pivotAntibiotic.FieldName = "ANTIBIOTICSNAME";
                    pivotIndex.FieldName = "ORGANISMINDEX";
                    PivotSIR.FieldName = "RESULT";
                    PivotValue.FieldName = "RESULTVALUE";
                    pivotUnits.FieldName = "UNITS";

                    //   pivotGridField3.FieldName = "ANTIBIOTICSNAME";
                    //pivotGridField7.FieldName = "ANTIBIOTICSFAMILYID";


                    /*----------------------------------------
                     * 
                     * Header Informations.
                     * 
                     *--------------------------------------*/
                    // NEW Design
                    // ---> Old code---> lblHn.Text = ds.Tables["result"].Rows[0]["HN"].ToString().TrimStart('0');
                    //lblHn.Text = ds.Tables["result"].Rows[0]["HN"].ToString().Substring(11);
                    // END New Design
                    // NEW Design for versing 1.3b
                    //lblName.Text = ds.Tables["result"].Rows[0]["NAME"].ToString() + " " + ds.Tables["result"].Rows[0]["LASTNAME"].ToString();

                    //Query_Patient_ORM(accessnumber.Value.ToString());
                    //if (StrPatientName_ORM != "" || StrPatientLast_ORM != "")
                    //{
                    //    lblName.Text = StrPatientName_ORM + " " + StrPatientLast_ORM;
                    //}
                    //else
                    //{
                    //    lblName.Text = ds.Tables["result"].Rows[0]["NAME"].ToString() + " " + ds.Tables["result"].Rows[0]["LASTNAME"].ToString();
                    //}

                    //// END New Design
                    //// Search specimen in ORM database
                    //string Str_Specimenin_ORM = GET_Speciman(potocalnumber.Value.ToString());

                    //if (Str_Specimenin_ORM !="-")
                    //{
                    //    lblSpecimen.Text = Str_Specimenin_ORM;
                    //}
                    //else
                    //{
                    //    lblSpecimen.Text = ds.Tables["result"].Rows[0]["MATERIALSTEXT"].ToString();
                    //}
                    // End Search specimen i ORM database

                    //lblCollected.Text = ds.Tables["result"].Rows[0]["COLLECTIONDATE"].ToString();
                    //lblReceived.Text = String.Format("{0:dd-MM-yyyy HH:mm:ss}", Convert.ToDateTime(ds.Tables["result"].Rows[0]["RECEIVEDDATE"].ToString()) );
                    //
                    // Received to Thai Buddish calendar
                    //if (Str_calendar == "TH")
                    //{
                    //    lblReceived.Text = ((DateTime)(ds.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                    //    lblValidated.Text = ((DateTime)(ds.Tables["result"].Rows[0]["VALIDATED"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                    //    xrDOB.Text = ((DateTime)(ds.Tables["result"].Rows[0]["BIRTHDATE"])).ToString("dd-MM-yyyy", cultureinfo_TH);
                    //}
                    //else
                    //{
                        //xrDOB.Text = ((DateTime)(ds.Tables["result"].Rows[0]["BIRTHDATE"])).ToString("dd-MM-yyyy", cultureinfo_ENG);
                   //}
                    //
                    //lblReceived.Text = ((DateTime)(ds.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd-MM-yyyy HH:mm:ss", Cultureinfo);
                    //lblApproveBy.Text = ds.Tables["result"].Rows[0]["USERNAME"].ToString();
                    //lblGroupTest.Text = "* " + ds.Tables["result"].Rows[0]["PROTOCALTEXT"].ToString() + " * ";
                    //strGroupTest_Protocol = "* " + ds.Tables["result"].Rows[0]["PROTOCALTEXT"].ToString() + " * ";
                    xr_Labno.Text = ds.Tables["result"].Rows[0]["ACCESSNUMBER"].ToString();
                    xrProtocol.Text = ds.Tables["result"].Rows[0]["MBREQNUMBER"].ToString();


                    //xrLocation.Text = ds.Tables["result"].Rows[0]["LOCNAME"].ToString() + " " + ds.Tables["result"].Rows[0]["ADDRESS1"].ToString() + " " + ds.Tables["result"].Rows[0]["ADDRESS2"].ToString();
                    //xrTableCell5.Text = "* " + ds.Tables["result"].Rows[0]["PROTOCALTEXT"].ToString() + " * ";
                    // Collection source
                    //xrCollectionSource.Text = ds.Tables["result"].Rows[0]["COLLSOURCETXT"].ToString();
                    //lblReceived.Text = ds.Tables["result"].Rows[0]["DOCNAME"].ToString() + " " + ds.Tables["result"].Rows[0]["DOCNAME2"].ToString();
                    
                    // Get User name for report by String Array
                    //string[] Get_reportManual = GET_User();
                    //xrUserreport.Text = Get_reportManual[0] + " (" + Get_reportManual[1] + ")";
                    //if (Report_function.Value.ToString() == "MANUAL")
                    //{
                    //    xrLabel25.Visible = true;
                    //    xrUserreport.Visible = true;
                    //    xrLabel29.Visible = true;
                    //    xrDTReport.Text = DTReport.Value.ToString();
                    //}
                    //else
                    //{
                    //    xrLabel25.Visible = false;
                    //    xrUserreport.Visible = false;
                    //    xrLabel29.Visible = false;
                    //    xrDTReport.Visible = false;
                    //}

                    // End Get User for report

                    //////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////// Comment CODE OR COMMENT FREE TEXT //////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////
                    // Check comment by request CODE Comment
                    //for (int k = 0; k < ds.Tables["result"].Rows.Count; k++)
                    //{
                    //    // Check comment by request
                    //    comment_request = ds.Tables["result"].Rows[k]["COMMENT"].ToString();

                    //    // Check comment request
                    //    if (!comment_request.Equals(""))
                    //    {
                    //        break;
                    //    }
                    //}

                    // check comment if not null
                    //if (!comment_request.Equals(""))
                    //{
                    //    xrLabel_comment.Text = comment_request;
                    //    GroupFooter1.Visible = true;
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
                    //        xrLabel_comment.Text = comment_request;
                    //        GroupFooter1.Visible = true;
                    //    }
                    //}
                    //////////////////////////////////////////// END Comment CODE OR COMMENT FREE TEXT  //////////////////////////

                    string sex = "";
                    if (ds.Tables["result"].Rows[0]["SEX"].ToString() == "1")
                    {
                        sex = "ชาย";
                    }
                    else if (ds.Tables["result"].Rows[0]["SEX"].ToString() == "2")
                    {
                        sex = "หญิง";
                    }
                    else
                    {
                        // If u result -
                        //sex = "ไม่ระบุ";
                        sex = "ไม่ระบุ";
                    }
                    xr_sex.Text = sex;

                    //queryQutity_specialH3();
                    queryQutity();

                    ///*for obx hl7 */
                    //string checkCode = "";
                    //for (int hl7 = 0; hl7 < ds.Tables["result"].Rows.Count; hl7++)
                    //{
                    //    if (checkCode != ds.Tables["result"].Rows[hl7]["SUBREQMBANTIBIOTICID"].ToString())
                    //    {
                    //        if (!String.IsNullOrEmpty(ds.Tables["result"].Rows[hl7]["ANTIBIOTICSNAME"].ToString()))
                    //        {
                    //            strOBX += "MB" + ds.Tables["result"].Rows[hl7]["SUBREQMBANTIBIOTICID"].ToString() + "^" + ds.Tables["result"].Rows[hl7]["ANTIBIOTICSNAME"].ToString() + "||" + ds.Tables["result"].Rows[hl7]["ORGANISMNAME"].ToString() + "=" + ds.Tables["result"].Rows[hl7]["RESULT"].ToString() + " Value=" + ds.Tables["result"].Rows[hl7]["RESULTVALUE"].ToString() + "\r\n";
                    //        }
                    //        checkCode = ds.Tables["result"].Rows[hl7]["SUBREQMBANTIBIOTICID"].ToString();
                    //    }
                    //    else
                    //    {
                    //        checkCode = "";
                    //    }

                    //}
                }
                /* Don't have sensitivities Using for  Aerobe special report */
//                else
//                {
//                    string sql_special = "";
//                    if (checkSampleMSU())
//                    {
//                        sql_special = @"SELECT distinct 
//  SUBREQMB_ORGANISMS.ORGANISMINDEX
//, PATIENTS.PATNUMBER AS 'HN'
//, PATIENTS.NAME,PATIENTS.SEX
//, PATIENTS.FIRSTNAME AS 'LASTNAME'
//, REQUESTS.REQUESTID
//, REQUESTS.ACCESSNUMBER
//, PATIENTS.BIRTHDATE
//, CAST(DATEDIFF(yy,PATIENTS.BIRTHDATE,GETDATE()) AS varchar) AS 'ageY'
//, ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, GETDATE()))%12 as ageM
//, REQUESTS.PATID
//, SUBREQUESTS_MB.SUBREQUESTID
//, CONVERT (varchar, SUBREQUESTS_MB.COLLECTIONDATE, 120) AS 'COLLECTIONDATE'
//, SUBREQUESTS_MB.COLLMATERIALID
//, SUBREQMB_ORGANISMS.SUBREQMBORGID
//, SUBREQMB_ORGANISMS.ORGANISMID
//, SUBREQMB_ORGANISMS.IDENTDATE
//, SUBREQMB_ANTIBIOTICS.SUBREQMBANTIBIOTICID
//, SUBREQMB_ANTIBIOTICS.ANTIBIOTICID
//, DICT_MB_ORGANISMS.ORGANISMNAME
//, DICT_MB_ORGANISMS.FULLTEXT AS 'ORGANISMSNAME'
//, DICT_MB_ORGANISMS.MORPHODESC
//, DICT_MB_ANTIBIOTICS.ANTIBIOTICCODE
//, DICT_MB_ANTIBIOTICS.FULLTEXT AS 'ANTIBIOTICSNAME'
//, CASE ( SUBREQMB_ANTIBIOTICS.UNITS ) WHEN '1' THEN 'ug/ml' WHEN '2' THEN 'mm' ELSE '' END AS 'UNITS'
//, REQUESTS.RECEIVEDDATE
//, REQUESTS.REQDATE
//, SUBREQMB_ACTIONS.ACTIONMARKUSERID
//, USERS.USERNAME, RIGHT (SUBREQMB_COLONIES.COLONYNUMBER, 3) AS 'COLONYNUMBER'
//, CASE ( SUBREQMB_ANTIBIOTICS.RESULT) WHEN '1' THEN 'S' WHEN 2 THEN 'I' WHEN 3 THEN 'R' ELSE '' END AS 'RESULT' 
//, SUBREQMB_ANTIBIOTICS.RESULTVALUE
//, '(' + SUBREQMB_ANTIBIOTICS.MINIMUM +' - '+ SUBREQMB_ANTIBIOTICS.MAXIMUM +')' AS 'breakpoints'
//, CASE (SUBREQMB_ACTIONS.ACTIONMARKTYPE) WHEN 8 THEN SUBREQMB_ACTIONS.ACTIONMARKDATE WHEN 23 THEN SUBREQMB_ACTIONS.ACTIONMARKDATE END AS 'REQSTATUS'
//, SUBREQUESTS_MB.SUBREQUESTNUMBER
//, DICT_COLL_MATERIALS.COLLMATERIALCODE
//, DICT_COLL_MATERIALS.FULLTEXT AS 'MATERIALSTEXT'
//, DICT_MB_PROTOCOLS.PROTOCOLCODE
//, DICT_MB_PROTOCOLS.FULLTEXT AS 'PROTOCALTEXT'
//, SUBREQMB_DET_TESTS.TESTRESULT
//, SUBREQMB_ANTIBIOTICS.UNITS
//, SUBREQMB_ACTIONS.ACTIONMARKDATE AS 'VALIDATED'
//, DICT_LOCATIONS.LOCNAME
//, DICT_LOCATIONS.ADDRESS1
//, DICT_LOCATIONS.ADDRESS2 
//, DICT_TEXTS.FULLTEXT AS 'COMMENT'
//, SUBREQMB_COLONIES.COLONYINDEX
//, DICT_DOCTORS.DOCCODE
//, DICT_DOCTORS.DOCNAME
//, DICT_DOCTORS.ADDRESS1 AS 'DOCNAME2'
//, SUBREQMB_OCOM.COMMENTTEXT
//, SUBREQUESTS_MB.COLLSOURCETXT

// FROM REQUESTS LEFT OUTER JOIN SUBREQUESTS_MB ON (REQUESTS.REQUESTID = SUBREQUESTS_MB.REQUESTID) 
// LEFT OUTER JOIN SUBREQMB_ORGANISMS ON (SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ORGANISMS.SUBREQUESTID)
// LEFT OUTER JOIN DICT_MB_ORGANISMS ON (SUBREQMB_ORGANISMS.ORGANISMID = DICT_MB_ORGANISMS.ORGANISMID)
// LEFT OUTER JOIN SUBREQMB_ANTIBIOTICS ON (SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_ANTIBIOTICS.SUBREQMBORGID) 
// LEFT OUTER JOIN DICT_MB_ANTIBIOTICS ON (SUBREQMB_ANTIBIOTICS.ANTIBIOTICID = DICT_MB_ANTIBIOTICS.ANTIBIOTICID) 
// LEFT OUTER JOIN DICT_MB_ANTIBIO_FAMS ON (DICT_MB_ANTIBIOTICS.ANTIBIOTICSFAMILYID = DICT_MB_ANTIBIO_FAMS.ANTIBIOTICSFAMILYID)
// LEFT OUTER JOIN SUBREQMB_ACTIONS ON SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ACTIONS.SUBREQUESTID
// LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID 
// LEFT OUTER JOIN DICT_MB_PROTOCOLS ON SUBREQUESTS_MB.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
// LEFT OUTER JOIN DICT_COLL_MATERIALS ON SUBREQUESTS_MB.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID
// LEFT OUTER JOIN DICT_COLL_SOURCES ON SUBREQUESTS_MB.COLLSOURCEID = DICT_COLL_SOURCES.COLLSOURCEID
// LEFT OUTER JOIN USERS ON users.USERID = SUBREQMB_ACTIONS.ACTIONMARKUSERID
// LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_COLONIES.SUBREQMBORGID 
// LEFT OUTER JOIN SUBREQMB_DET_TESTS ON SUBREQMB_COLONIES.COLONYID = SUBREQMB_DET_TESTS.COLONYID
// LEFT OUTER JOIN LOCATIONS ON REQUESTS.REQUESTID = LOCATIONS.REQUESTID
// LEFT OUTER JOIN DICT_LOCATIONS ON LOCATIONS.LOCID = DICT_LOCATIONS.LOCID
// LEFT OUTER JOIN SUBREQMB_OCOM on SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_OCOM.SUBREQUESTID
// LEFT OUTER JOIN DICT_TEXTS ON SUBREQMB_OCOM.COMMENTCODEDID = DICT_TEXTS.TEXTID
// LEFT OUTER JOIN DOCTORS ON REQUESTS.REQUESTID  = DOCTORS.REQUESTID
// LEFT OUTER JOIN DICT_DOCTORS ON DOCTORS.DOCID = DICT_DOCTORS.DOCID
// WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "'" +
//" AND SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString() + "'" +
// @" AND SUBREQMB_ACTIONS.ACTIONMARKTYPE = '" + status.Value.ToString() + "'" +
// // "  AND SUBREQMB_ANTIBIOTICS.NOTPRINTABLE = 0 " +
//" AND ((SUBREQMB_DET_TESTS.NOTPRINTABLE =0) OR (   SUBREQMB_DET_TESTS.NOTPRINTABLE is null) ) " +
// //       "  AND DICT_MB_ANTIBIO_FAMS.ANTIBIOTICSFAMILYCODE not in (" + Setting_NotReport_FAM.Default.strAntibioticFamCode.ToString() + ")" +

// " -- order by DICT_MB_ANTIBIOTICS.FULLTEXT " +
// " order by SUBREQMB_ORGANISMS.ORGANISMINDEX,SUBREQMB_COLONIES.COLONYINDEX ,DICT_MB_ANTIBIO_FAMS.ANTIBIOTICSFAMILYID,DICT_MB_ANTIBIO_FAMS.FULLTEXT";
//                    }
//                    else
//                    {
//                        // ============ No Growth =========

//                        sql_special = @"SELECT distinct 
//  SUBREQMB_ORGANISMS.ORGANISMINDEX
//, PATIENTS.PATNUMBER AS 'HN'
//, PATIENTS.NAME
//, PATIENTS.SEX
//, PATIENTS.FIRSTNAME as 'LASTNAME'
//, REQUESTS.REQUESTID
//, REQUESTS.ACCESSNUMBER
//, PATIENTS.BIRTHDATE
//, CAST(DATEDIFF(yy,PATIENTS.BIRTHDATE,GETDATE()) AS varchar) AS 'ageY'
//, ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, GETDATE()))%12 as ageM
//, REQUESTS.PATID
//, SUBREQUESTS_MB.SUBREQUESTID
//, CONVERT (varchar, SUBREQUESTS_MB.COLLECTIONDATE, 120) AS 'COLLECTIONDATE'
//, SUBREQUESTS_MB.COLLMATERIALID
//, SUBREQMB_ORGANISMS.SUBREQMBORGID
//, SUBREQMB_ORGANISMS.ORGANISMID
//, SUBREQMB_ORGANISMS.IDENTDATE
//, SUBREQMB_ANTIBIOTICS.SUBREQMBANTIBIOTICID
//, SUBREQMB_ANTIBIOTICS.ANTIBIOTICID
//, DICT_MB_ORGANISMS.ORGANISMNAME
//, DICT_MB_ORGANISMS.FULLTEXT AS 'ORGANISMSNAME'
//, DICT_MB_ORGANISMS.MORPHODESC
//, DICT_MB_ANTIBIOTICS.ANTIBIOTICCODE
//, DICT_MB_ANTIBIOTICS.FULLTEXT as 'ANTIBIOTICSNAME'
//, CASE (SUBREQMB_ANTIBIOTICS.UNITS) WHEN '1' THEN 'ug/ml' WHEN '2' THEN 'mm' ELSE '' END AS 'UNITS'
//, REQUESTS.RECEIVEDDATE
//, REQUESTS.REQDATE
//, SUBREQMB_ACTIONS.ACTIONMARKUSERID
//, USERS.USERNAME, RIGHT (SUBREQMB_COLONIES.COLONYNUMBER, 3) AS 'COLONYNUMBER'
//, CASE (SUBREQMB_ANTIBIOTICS.RESULT) WHEN '1' THEN 'S' WHEN 2 THEN 'I' WHEN 3 THEN 'R' ELSE '' END AS 'RESULT' 
//, SUBREQMB_ANTIBIOTICS.RESULTVALUE
//, '('+  SUBREQMB_ANTIBIOTICS.MINIMUM +' - '+ SUBREQMB_ANTIBIOTICS.MAXIMUM +')' as 'breakpoints'
//, CASE (SUBREQMB_ACTIONS.ACTIONMARKTYPE) WHEN 8 THEN SUBREQMB_ACTIONS.ACTIONMARKDATE WHEN 23 THEN SUBREQMB_ACTIONS.ACTIONMARKDATE END AS 'REQSTATUS'
//, SUBREQUESTS_MB.SUBREQUESTNUMBER
//, DICT_COLL_MATERIALS.COLLMATERIALCODE
//, DICT_COLL_MATERIALS.FULLTEXT AS 'MATERIALSTEXT'
//, DICT_MB_PROTOCOLS.PROTOCOLCODE
//, DICT_MB_PROTOCOLS.FULLTEXT AS 'PROTOCALTEXT'
//, SUBREQMB_DET_TESTS.TESTRESULT
//, SUBREQMB_ANTIBIOTICS.UNITS
//, SUBREQMB_ACTIONS.ACTIONMARKDATE AS 'VALIDATED'
//, DICT_LOCATIONS.LOCNAME
//, DICT_LOCATIONS.ADDRESS1
//, DICT_LOCATIONS.ADDRESS2 
//, DICT_TEXTS.FULLTEXT as 'COMMENT'
//, SUBREQMB_COLONIES.COLONYINDEX
//, DICT_DOCTORS.DOCCODE
//, DICT_DOCTORS.DOCNAME
//, DICT_DOCTORS.ADDRESS1 AS 'DOCNAME2'
//, SUBREQMB_OCOM.COMMENTTEXT
//, SUBREQUESTS_MB.COLLSOURCETXT

// FROM REQUESTS LEFT OUTER JOIN SUBREQUESTS_MB ON (REQUESTS.REQUESTID = SUBREQUESTS_MB.REQUESTID) 
// LEFT OUTER JOIN SUBREQMB_ORGANISMS ON (SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ORGANISMS.SUBREQUESTID)
// LEFT OUTER JOIN DICT_MB_ORGANISMS ON (SUBREQMB_ORGANISMS.ORGANISMID = DICT_MB_ORGANISMS.ORGANISMID)
// LEFT OUTER JOIN SUBREQMB_ANTIBIOTICS ON (SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_ANTIBIOTICS.SUBREQMBORGID) 
// LEFT OUTER JOIN DICT_MB_ANTIBIOTICS ON (SUBREQMB_ANTIBIOTICS.ANTIBIOTICID = DICT_MB_ANTIBIOTICS.ANTIBIOTICID) 
// LEFT OUTER JOIN DICT_MB_ANTIBIO_FAMS ON (DICT_MB_ANTIBIOTICS.ANTIBIOTICSFAMILYID = DICT_MB_ANTIBIO_FAMS.ANTIBIOTICSFAMILYID)
// LEFT OUTER JOIN SUBREQMB_ACTIONS ON SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ACTIONS.SUBREQUESTID
// INNER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID 
// LEFT OUTER JOIN DICT_MB_PROTOCOLS ON SUBREQUESTS_MB.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
// LEFT OUTER JOIN DICT_COLL_MATERIALS ON SUBREQUESTS_MB.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID
// LEFT OUTER JOIN DICT_COLL_SOURCES ON SUBREQUESTS_MB.COLLSOURCEID = DICT_COLL_SOURCES.COLLSOURCEID
// LEFT OUTER JOIN USERS ON users.USERID = SUBREQMB_ACTIONS.ACTIONMARKUSERID
// LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_COLONIES.SUBREQMBORGID 
// LEFT OUTER JOIN SUBREQMB_DET_TESTS ON SUBREQMB_COLONIES.COLONYID = SUBREQMB_DET_TESTS.COLONYID
// LEFT OUTER JOIN LOCATIONS ON REQUESTS.REQUESTID = LOCATIONS.REQUESTID
// LEFT OUTER JOIN DICT_LOCATIONS ON LOCATIONS.LOCID = DICT_LOCATIONS.LOCID
// LEFT OUTER JOIN SUBREQMB_OCOM on SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_OCOM.SUBREQUESTID
// LEFT OUTER JOIN DICT_TEXTS ON SUBREQMB_OCOM.COMMENTCODEDID = DICT_TEXTS.TEXTID
// LEFT OUTER JOIN DOCTORS ON REQUESTS.REQUESTID  = DOCTORS.REQUESTID
// LEFT OUTER JOIN DICT_DOCTORS ON DOCTORS.DOCID = DICT_DOCTORS.DOCID
// WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "'" +
// "AND SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString() + "'" +
// @"  AND SUBREQMB_ACTIONS.ACTIONMARKTYPE = '" + status.Value.ToString() + "'" +
////"  AND  SUBREQMB_DET_TESTS.NOTPRINTABLE != 1 " +
//// "    AND ((SUBREQMB_DET_TESTS.NOTPRINTABLE =0) OR (   SUBREQMB_DET_TESTS.NOTPRINTABLE is null) ) "+
//"-- order by DICT_MB_ANTIBIOTICS.FULLTEXT " +
//"order by SUBREQMB_ORGANISMS.ORGANISMINDEX,SUBREQMB_COLONIES.COLONYINDEX ,DICT_MB_ANTIBIO_FAMS.ANTIBIOTICSFAMILYID,DICT_MB_ANTIBIO_FAMS.FULLTEXT";
//                    }

//                    SqlCommand cmd_special = new SqlCommand(sql_special, conn);
//                    SqlDataAdapter adp_special = new SqlDataAdapter(cmd_special);
//                    ds_special = new DataSet();
//                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
//                    ds_special.Clear();
//                    adp_special.Fill(ds_special, "result");

//                    if (ds_special.Tables["result"].Rows.Count > 0)
//                    {
//                        subRequestsID = ds_special.Tables["result"].Rows[0]["SUBREQUESTID"].ToString();
//                        // NEW Design
//                        // ---> Old code---> lblHn.Text = ds_special.Tables["result"].Rows[0]["HN"].ToString().TrimStart('0');
//                        lblHn.Text = ds_special.Tables["result"].Rows[0]["HN"].ToString();

//                        //// Search Hospitalnumber
//                        //ReportConfigurationController objConfig = new ReportConfigurationController();
//                        //DataTable dt = null;
//                        //AllVaraibleM objAllVaraibleM = new AllVaraibleM();

//                        //try
//                        //{
//                        //    objAllVaraibleM.GET_Accessnumber = accessnumber.Value.ToString();
//                        //    dt = objConfig.Get_Hospitalnumber(objAllVaraibleM);

//                        //    if (dt.Rows.Count > 0)
//                        //    {
//                        //        if (dt.Rows[0]["HOSTORDERNUMBER"].ToString() == null || String.IsNullOrWhiteSpace(dt.Rows[0]["HOSTORDERNUMBER"].ToString()))
//                        //        {
//                        //            xrHosnumber.Text = "-";
//                        //        }
//                        //        else
//                        //        {
//                        //            xrHosnumber.Text = dt.Rows[0]["HOSTORDERNUMBER"].ToString();
//                        //        }
//                        //    }
//                        //    else
//                        //    {
//                        //        xrHosnumber.Text = "-";
//                        //    }
//                        //}
//                        //catch (Exception ex)
//                        //{
//                        //    throw new Exception(ex.Message);
//                        //}
//                        //finally
//                        //{
//                        //    objAllVaraibleM = null;
//                        //}



//                        // END New Design

//                        // NEW Design for versing 1.3b
//                        //lblName.Text = ds_special.Tables["result"].Rows[0]["NAME"].ToString() + " " + ds_special.Tables["result"].Rows[0]["LASTNAME"].ToString();
//                        //Query_Patient_ORM(accessnumber.Value.ToString());
//                        //if (StrPatientName_ORM != "" || StrPatientLast_ORM != "")
//                        //{
//                        //    lblName.Text = StrPatientName_ORM + " " + StrPatientLast_ORM;
//                        //}
//                        //else
//                        //{
//                        //    lblName.Text = ds_special.Tables["result"].Rows[0]["NAME"].ToString() + " " + ds_special.Tables["result"].Rows[0]["LASTNAME"].ToString();
//                        //}
//                        //// END New Design for versing 1.3b

//                        //// Search specimen in ORM database
//                        //string Str_Specimenin_ORM = GET_Speciman(potocalnumber.Value.ToString());

//                        //if (Str_Specimenin_ORM != "-")
//                        //{
//                        //    lblSpecimen.Text = Str_Specimenin_ORM;
//                        //}
//                        //else
//                        //{
//                        //    lblSpecimen.Text = ds_special.Tables["result"].Rows[0]["MATERIALSTEXT"].ToString();
//                        //}
//                        // End Search specimen i ORM database

//                        //lblReceived.Text = ((DateTime)(ds_special.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd-MM-yyyy HH:mm:ss", Cultureinfo);
//                        //lblReceived.Text = String.Format("{0:dd-MM-yyyy HH:mm:ss}", Convert.ToDateTime(ds_special.Tables["result"].Rows[0]["RECEIVEDDATE"].ToString()) );
//                        //
//                        //Received to Thai Buddish calendar
//                        //if (Str_calendar == "TH")
//                        //{
//                        //    lblReceived.Text = ((DateTime)(ds_special.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
//                        //    lblValidated.Text = ((DateTime)(ds_special.Tables["result"].Rows[0]["VALIDATED"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
//                        //    xrDOB.Text = ((DateTime)(ds_special.Tables["result"].Rows[0]["BIRTHDATE"])).ToString("dd-MM-yyyy", cultureinfo_TH);
//                        //}
//                        //else
//                        //{
//                            lblReceived.Text = ((DateTime)(ds_special.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_ENG);
//                            lblValidated.Text = ((DateTime)(ds_special.Tables["result"].Rows[0]["VALIDATED"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_ENG);
//                            xrDOB.Text = ((DateTime)(ds_special.Tables["result"].Rows[0]["BIRTHDATE"])).ToString("dd-MM-yyyy", cultureinfo_ENG);
//                        //}
//                        //
//                        //lblApproveBy.Text = ds_special.Tables["result"].Rows[0]["USERNAME"].ToString();
//                        strGroupTest_Protocol = ds_special.Tables["result"].Rows[0]["PROTOCALTEXT"].ToString();
//                        lblAccessnumber.Text = ds_special.Tables["result"].Rows[0]["ACCESSNUMBER"].ToString();
//                        //lblSampleNumber.Text = ds_special.Tables["result"].Rows[0]["SUBREQUESTNUMBER"].ToString();
//                        //xrLocation.Text = ds_special.Tables["result"].Rows[0]["LOCNAME"].ToString() + " " + ds_special.Tables["result"].Rows[0]["ADDRESS1"].ToString() + " " + ds_special.Tables["result"].Rows[0]["ADDRESS2"].ToString();
//                        //xrTableCell5.Text = ds_special.Tables["result"].Rows[0]["PROTOCALTEXT"].ToString();
//                        //xrTableCell11.Text = ds_special.Tables["result"].Rows[0]["PROTOCALTEXT"].ToString();
//                        // Collection source
//                        //
//                        //xrCollectionSource.Text = ds_special.Tables["result"].Rows[0]["COLLSOURCETXT"].ToString();
//                        // if DOB to English in Year
//                        // DateTime DT = Convert.ToDateTime(ds_special.Tables["result"].Rows[0]["BIRTHDATE"].ToString());
//                        // string Str_BirthDate = DT.ToString(format_DT_Eng, System.Globalization.CultureInfo.CreateSpecificCulture("hu-HU"));
//                        // xrDOB.Text = Str_BirthDate;
//                        //

//                        // Get Username for report manual
//                        //string[] Get_reportManual = GET_User();
//                        //xrUserreport.Text = Get_reportManual[0] + " (" + Get_reportManual[1] + ")"; ;
//                        //if (Report_function.Value.ToString() == "MANUAL")
//                        //{
//                        //    xrLabel25.Visible = true;
//                        //    xrUserreport.Visible = true;
//                        //    xrLabel29.Visible = true;
//                        //    xrDTReport.Text = DTReport.Visible.ToString();
//                        //}
//                        //else
//                        //{
//                        //    xrLabel25.Visible = false;
//                        //    xrUserreport.Visible = false;
//                        //    xrLabel29.Visible = false;
//                        //    xrDTReport.Visible = false;
//                        //}
//                        //xrDTReport.Text = DTReport.Value.ToString();

//                        // End Get User for report

//                        string str_AgeY = ds_special.Tables["result"].Rows[0]["ageY"].ToString() + " ปี.";
//                        string str_AgeM = ds_special.Tables["result"].Rows[0]["ageM"].ToString() + " M";
//                        int I_AgeY = Convert.ToInt32(ds_special.Tables["result"].Rows[0]["ageY"].ToString());
//                        int I_AgeM = Convert.ToInt32(ds_special.Tables["result"].Rows[0]["ageM"].ToString());


//                        if (I_AgeY > 0)
//                        {
//                            lblAge.Text = str_AgeY + " " + str_AgeM;
//                        }
//                        else if (I_AgeY == 0)
//                        {
//                            lblAge.Text = str_AgeM;
//                        }


//                        string strSex = "";
//                        if (ds_special.Tables["result"].Rows[0]["SEX"].ToString() == "1")
//                        {
//                            strSex = "ชาย";
//                        }
//                        else if (ds_special.Tables["result"].Rows[0]["SEX"].ToString() == "2")
//                        {
//                            strSex = "หญิง";
//                        }
//                        else
//                        {
//                            strSex = "ไม่ระบุ";
//                        }
//                        // set sex
//                        lblSex.Text = strSex;

//                        //////////////////////////////////////////////////////////////////////////////////////////
//                        ///////////////////////////////// Comment CODE OR COMMENT FREE TEXT //////////////////////
//                        //////////////////////////////////////////////////////////////////////////////////////////
//                        // Check comment by request CODE Comment
//                        //for (int k = 0; k < ds_special.Tables["result"].Rows.Count; k++)
//                        //{
//                        //    // Check comment by request
//                        //    comment_request = ds_special.Tables["result"].Rows[k]["COMMENT"].ToString();

//                        //    // Check comment request
//                        //    if (!comment_request.Equals(""))
//                        //    {
//                        //        break;
//                        //    }
//                        //}

//                        // check comment if not null
//                        if (!comment_request.Equals(""))
//                        {
//                            xrLabel_comment2.Text = comment_request;
//                            GroupFooter3.Visible = true;
//                        }
//                        else
//                        {
//                            // Check comment by request Comment Freetext
//                            for (int j = 0; j < ds_special.Tables["result"].Rows.Count; j++)
//                            {
//                                // Check comment by request
//                                comment_request = ds_special.Tables["result"].Rows[j]["COMMENTTEXT"].ToString();

//                                // ... Switch on the string. check  date
//                                try
//                                {
//                                    string comment_request_start = comment_request[0] + "" + comment_request[1];
//                                    int number = Int32.Parse(comment_request_start);
//                                    comment_request = "";
//                                }
//                                catch (Exception)
//                                {
//                                    // Check comment request
//                                    if (!comment_request.Equals("")) break;
//                                }
//                            }

//                            // Check comment by request
//                            if (!comment_request.Equals(""))
//                            {
//                                xrLabel_comment2.Text = comment_request;
//                                GroupFooter3.Visible = true;
//                            }
//                        }
//                        //////////////////////////////////////////// END Comment CODE OR COMMENT FREE TEXT  //////////////////////////

//                        // organisum result for raw data organisums
//                        string organ = "";
//                        string organ_res = "";
//                        string tech_comment = "";
//                        string tmp_organ = "";
//                        for (int i = 0; i < ds_special.Tables["result"].Rows.Count; i++)
//                        {
//                            if (!tmp_organ.Equals(ds_special.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString()))
//                            {
//                                // update temp oranism
//                                tmp_organ = ds_special.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString();

//                                if (ds_special.Tables["result"].Rows.Count < 2)
//                                {
//                                    organ = ds_special.Tables["result"].Rows[i]["SUBREQMBORGID"].ToString() + ds_special.Tables["result"].Rows[i]["ORGANISMID"].ToString() + "^" + ds_special.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString();
//                                    organ_res = ds_special.Tables["result"].Rows[i]["TESTRESULT"].ToString();
//                                    if (organ_res.Equals(""))
//                                    {
//                                        organ_res = "-";
//                                    }

//                                    // check OBX not null
//                                    if (!String.IsNullOrEmpty(ds_special.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString()))
//                                    {
//                                        strOBX += organ + "||" + organ_res + "\r\n";
//                                    }

//                                    // Check Organism is not null
//                                    if (!ds_special.Tables["result"].Rows[i]["ORGANISMINDEX"].ToString().Equals(""))
//                                    {
//                                        // visible gram and comment
//                                        xrTableCell2_gram.Visible = false;
//                                        xrComment.Visible = false;

//                                        // Process Text style
//                                        Process_StyleText_Organ("<n>(" + ds_special.Tables["result"].Rows[i]["ORGANISMINDEX"].ToString() + ")</n>  " + ds_special.Tables["result"].Rows[i]["MORPHODESC"].ToString());
//                                    }
//                                }
//                                else
//                                {
//                                    // get technical comment
//                                    tech_comment = ds_special.Tables["result"].Rows[i]["COMMENTTEXT"].ToString();

//                                    // ... Switch on the string. check  date
//                                    try
//                                    {
//                                        string comment_request_start = tech_comment[0] + "" + tech_comment[1];
//                                        int number = Int32.Parse(comment_request_start);
//                                    }
//                                    catch (Exception)
//                                    {
//                                        organ = ds_special.Tables["result"].Rows[i]["SUBREQMBORGID"].ToString() + ds_special.Tables["result"].Rows[i]["ORGANISMID"].ToString() + "^" + ds_special.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString();
//                                        organ_res = ds_special.Tables["result"].Rows[i]["TESTRESULT"].ToString();
//                                        if (organ_res.Equals(""))
//                                        {
//                                            organ_res = "-";
//                                        }

//                                        // check OBX not null                            
//                                        if (!String.IsNullOrEmpty(ds_special.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString()))
//                                        {
//                                            strOBX += organ + "||" + organ_res + "\r\n";
//                                        }

//                                        // Check Organism is not null
//                                        if (!ds_special.Tables["result"].Rows[i]["ORGANISMINDEX"].ToString().Equals(""))
//                                        {
//                                            // visible gram and comment
//                                            xrTableCell2_gram.Visible = false;
//                                            xrComment.Visible = false;

//                                            // Process Text style
//                                            Process_StyleText_Organ("<n>(" + ds_special.Tables["result"].Rows[i]["ORGANISMINDEX"].ToString() + ")</n>  " + ds_special.Tables["result"].Rows[i]["MORPHODESC"].ToString());
//                                        }
//                                    }
//                                }
//                            }
//                        }


//                        if (!(ds_special.Tables["result"].Rows[0]["COMMENTTEXT"].ToString().Equals("")))
//                        {
//                            if (!ds_special.Tables["result"].Rows[0]["COMMENTTEXT"].ToString().Equals("No growth"))
//                            {
//                                //xrComment.Text = ds_special.Tables["result"].Rows[0]["COMMENTTEXT"].ToString();
//                            }
//                            else
//                            {
//                                xrLabel_commentNogrowth.Text = ds_special.Tables["result"].Rows[0]["COMMENTTEXT"].ToString();
//                            }

//                            GroupHeader4.Visible = false;
//                            GroupHeader2.Visible = true;
//                            if (ds_special.Tables["result"].Rows[0]["COMMENTTEXT"].ToString() != "")
//                            {
//                                strOBX += ds_special.Tables["result"].Rows[0]["SUBREQUESTID"].ToString() + "^COMMENT" + "||" + ds_special.Tables["result"].Rows[0]["COMMENTTEXT"].ToString() + "\r\n";
//                            }
//                            else
//                            {
//                                strOBX += ds_special.Tables["result"].Rows[0]["SUBREQUESTID"].ToString() + "^COMMENT" + "||-" + "\r\n";

//                            }

//                            if (xrTableCell2_gram.Text == "")
//                            {
//                                xrLabel9.Visible = false;
//                            }
//                        }
//                        else
//                        {
//                            xrComment.Text = ds_special.Tables["result"].Rows[0]["COMMENT"].ToString();
//                            GroupHeader4.Visible = true;
//                            GroupHeader2.Visible = false;
//                            if (ds_special.Tables["result"].Rows[0]["COMMENT"].ToString() != "")
//                            {
//                                strOBX += ds_special.Tables["result"].Rows[0]["SUBREQUESTID"].ToString() + "^COMMENT" + "||" + ds_special.Tables["result"].Rows[0]["COMMENT"].ToString() + "\r\n";
//                            }
//                            else
//                            {
//                                strOBX += ds_special.Tables["result"].Rows[0]["SUBREQUESTID"].ToString() + "^COMMENT" + "||-" + "\r\n";
//                            }

//                            if (xrTableCell2_gram.Text == "")
//                            {
//                                xrLabel9.Visible = false;
//                            }
//                            if (potocalnumber.Value.ToString().Contains("P04"))
//                            {
//                                xrLabel9.Visible = false;
//                            }
//                        }

//                        /*
//                        *   P04 
//                        *   Consolidate + gramstain and don't have sensitivity.
//                        */
//                        queryAerobeConsolidateGramP04();

//                        //topography.
//                        queryQutity_specialH3();

//                        // Organisms
//                        queryQutity();

//                    }
//                    else
//                    {
 
//                    }

//                    //========================== P04 No growth ========================//
//                    if (potocalnumber.Value.ToString().Contains("P04"))
//                    {

//                        /*
//                         * [ OLD CONFIG IN P04 ]
//                         * 
//                         *  
//                        // Check P04 No growth
//                        string sql_P04_Nogrowth = @"SELECT SUBREQMB_DET_TESTS.SUBREQMBTESTID,SUBREQMB_DET_TESTS.DETECTIONTESTID,SUBREQMB_DET_TESTS.SUBREQUESTID,SUBREQMB_DET_TESTS.COLONYID,SUBREQMB_DET_TESTS.SUBREQMBAGARID,SUBREQMB_DET_TESTS.TESTRESULT,SUBREQMB_DET_TESTS.VALREQUESTED,SUBREQMB_DET_TESTS.CONSOLIDATIONSTATUS,SUBREQMB_DET_TESTS.NOTPRINTABLE,SUBREQMB_DET_TESTS.CREATEUSER,SUBREQMB_DET_TESTS.CREATIONDATE,SUBREQMB_DET_TESTS.RESUPDDATE,SUBREQMB_DET_TESTS.LOGUSERID,SUBREQMB_DET_TESTS.LOGDATE,SUBREQMB_DET_TESTS.COMMENTS,DICT_MB_DETECT_TESTS.DETECTIONTESTID,DICT_MB_DETECT_TESTS.DETECTIONTESTCODE,DICT_MB_DETECT_TESTS.DETECTIONTESTCREDATE,DICT_MB_DETECT_TESTS.SHORTTEXT,DICT_MB_DETECT_TESTS.FULLTEXT,DICT_MB_DETECT_TESTS.DICTCONSOSTATUS,DICT_MB_DETECT_TESTS.NOTPRINTABLE,DICT_MB_DETECT_TESTS.FIRSTITEMID,DICT_MB_DETECT_TESTS.STARTVALIDDATE,DICT_MB_DETECT_TESTS.ENDVALIDDATE,DICT_MB_DETECT_TESTS.UNITS,DICT_MB_DETECT_TESTS.RESTYPE,DICT_MB_DETECT_TESTS.CUSRESULTID,DICT_MB_DETECT_TESTS.LOGUSERID,DICT_MB_DETECT_TESTS.LOGDATE,DICT_MB_DETECT_TESTS.LISTESTCODE,DICT_MB_DETECT_TESTS.GROUPTYPE,DICT_MB_DETECT_TESTS.COMBTESTFORCONSO 
//                                                FROM SUBREQMB_DET_TESTS 
//                                                LEFT OUTER JOIN DICT_MB_DETECT_TESTS ON (SUBREQMB_DET_TESTS.DETECTIONTESTID = DICT_MB_DETECT_TESTS.DETECTIONTESTID )
//                                                LEFT OUTER JOIN SUBREQUESTS_MB ON (SUBREQMB_DET_TESTS.SUBREQUESTID = SUBREQUESTS_MB.SUBREQUESTID)
//                                                LEFT OUTER JOIN REQUESTS ON (SUBREQUESTS_MB.REQUESTID = REQUESTS.REQUESTID)
//                                                WHERE SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString().Trim() + "'" +
//                                                " ORDER BY  CREATIONDATE, SUBREQMBTESTID ";

//                        SqlCommand cmd_P04_Nogrowth = new SqlCommand(sql_P04_Nogrowth, conn);
//                        SqlDataAdapter adp_P04_Nogrowth = new SqlDataAdapter(cmd_P04_Nogrowth);
//                        DataSet ds_res_P04_Nogrowth = new DataSet();
//                        if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
//                        ds_res_P04_Nogrowth.Clear();
//                        adp_P04_Nogrowth.Fill(ds_res_P04_Nogrowth, "result_P04_Nogrowth");

//                        if (ds_res_P04_Nogrowth.Tables["result_P04_Nogrowth"].Rows.Count > 0)
//                        {
//                            string result_nogrowth = ds_res_P04_Nogrowth.Tables["result_P04_Nogrowth"].Rows[0]["TESTRESULT"].ToString();
//                            if (result_nogrowth.Equals("No growth"))
//                            {
//                                GroupHeader2.Visible = true;
//                                xrTableCell_specialP04.Text = ds_res_P04_Nogrowth.Tables["result_P04_Nogrowth"].Rows[0]["TESTRESULT"].ToString();

//                                // check OBX not null                            
//                                if (!String.IsNullOrEmpty(ds_res_P04_Nogrowth.Tables["result_P04_Nogrowth"].Rows[0]["FULLTEXT"].ToString()))
//                                {
//                                    strOBX += "MB" + ds_res_P04_Nogrowth.Tables["result_P04_Nogrowth"].Rows[0]["SUBREQMBAGARID"].ToString() + "^" + ds_res_P04_Nogrowth.Tables["result_P04_Nogrowth"].Rows[0]["FULLTEXT"].ToString() + "||" + ds_res_P04_Nogrowth.Tables["result_P04_Nogrowth"].Rows[0]["TESTRESULT"].ToString() + "\r\n";
//                                }
//                            }
//                        }

//                        [ END OF OLD CONFIG P04 ]
//                        */

//                        string strReqID = "";

//                        strReqID = ds_special.Tables["result"].Rows[0]["REQUESTID"].ToString();

//                        if (strReqID == "tests")
//                        {

//                            string sql_res = @"SELECT 
// SUBREQMB_DET_TESTS.SUBREQMBTESTID
//,SUBREQMB_DET_TESTS.DETECTIONTESTID
//,SUBREQMB_DET_TESTS.SUBREQUESTID
//,SUBREQMB_DET_TESTS.COLONYID
//,SUBREQMB_DET_TESTS.SUBREQMBAGARID
//,SUBREQMB_DET_TESTS.TESTRESULT
//,SUBREQMB_DET_TESTS.VALREQUESTED
//,SUBREQMB_DET_TESTS.CONSOLIDATIONSTATUS
//,SUBREQMB_DET_TESTS.NOTPRINTABLE
//,SUBREQMB_DET_TESTS.CREATEUSER
//,SUBREQMB_DET_TESTS.CREATIONDATE
//,SUBREQMB_DET_TESTS.RESUPDDATE
//,SUBREQMB_DET_TESTS.LOGUSERID
//,SUBREQMB_DET_TESTS.LOGDATE
//,SUBREQMB_DET_TESTS.COMMENTS
//,DICT_MB_DETECT_TESTS.DETECTIONTESTID
//,DICT_MB_DETECT_TESTS.DETECTIONTESTCODE
//,DICT_MB_DETECT_TESTS.DETECTIONTESTCREDATE
//,DICT_MB_DETECT_TESTS.SHORTTEXT
//,DICT_MB_DETECT_TESTS.FULLTEXT
//,DICT_MB_DETECT_TESTS.DICTCONSOSTATUS
//,DICT_MB_DETECT_TESTS.NOTPRINTABLE
//,DICT_MB_DETECT_TESTS.FIRSTITEMID
//,DICT_MB_DETECT_TESTS.STARTVALIDDATE
//,DICT_MB_DETECT_TESTS.ENDVALIDDATE
//,DICT_MB_DETECT_TESTS.UNITS
//,DICT_MB_DETECT_TESTS.RESTYPE
//,DICT_MB_DETECT_TESTS.CUSRESULTID
//,DICT_MB_DETECT_TESTS.LOGUSERID
//,DICT_MB_DETECT_TESTS.LOGDATE
//,DICT_MB_DETECT_TESTS.LISTESTCODE
//,DICT_MB_DETECT_TESTS.GROUPTYPE
//,DICT_MB_DETECT_TESTS.COMBTESTFORCONSO 
//,DICT_TEXTS.FULLTEXT as 'COMMENT' , SUBREQMB_OCOM.COMMENTTEXT as 'COMMENT_FREE'
// FROM SUBREQMB_DET_TESTS 
// LEFT OUTER JOIN DICT_MB_DETECT_TESTS ON (SUBREQMB_DET_TESTS.DETECTIONTESTID = DICT_MB_DETECT_TESTS.DETECTIONTESTID )
// LEFT OUTER JOIN SUBREQUESTS_MB ON (SUBREQMB_DET_TESTS.SUBREQUESTID = SUBREQUESTS_MB.SUBREQUESTID)
// LEFT OUTER JOIN REQUESTS ON (SUBREQUESTS_MB.REQUESTID = REQUESTS.REQUESTID)
// LEFT OUTER JOIN SUBREQMB_OCOM on SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_OCOM.SUBREQUESTID
// LEFT OUTER JOIN DICT_TEXTS ON SUBREQMB_OCOM.COMMENTCODEDID = DICT_TEXTS.TEXTID
// WHERE REQUESTS.REQUESTID  ='" + strReqID + "'" +
// " AND SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString().Trim() + "'" +
// " ORDER BY  SUBREQMBTESTID DESC ";

//                            SqlCommand cmd_res = new SqlCommand(sql_res, conn);
//                            SqlDataAdapter adp_res = new SqlDataAdapter(cmd_res);
//                            DataSet ds_res = new DataSet();
//                            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
//                            ds_res.Clear();
//                            adp_res.Fill(ds_res, "result");

//                            if (ds_res.Tables["result"].Rows.Count > 0)
//                            {

//                                //////////////////////////////////////////////////////////////////////////////////////////
//                                ///////////////////////////////// Comment CODE OR COMMENT FREE TEXT //////////////////////
//                                //////////////////////////////////////////////////////////////////////////////////////////
//                                // Check comment by request CODE Comment
//                                for (int k = 0; k < ds_res.Tables["result"].Rows.Count; k++)
//                                {
//                                    // Check comment by request
//                                    comment_request = ds_res.Tables["result"].Rows[k]["COMMENT"].ToString();

//                                    // Check comment request
//                                    if (!comment_request.Equals(""))
//                                    {
//                                        break;
//                                    }
//                                }

//                                // check comment if not null
//                                if (!comment_request.Equals(""))
//                                {
//                                    GroupFooter3.Visible = true;
//                                    xrLabel_comment2.Text = comment_request;
//                                }
//                                else
//                                {
//                                    // Check comment by request Comment Freetext
//                                    for (int j = 0; j < ds_res.Tables["result"].Rows.Count; j++)
//                                    {
//                                        // Check comment by request
//                                        comment_request = ds_res.Tables["result"].Rows[j]["COMMENT_FREE"].ToString();

//                                        // ... Switch on the string. check  date
//                                        try
//                                        {
//                                            string comment_request_start = comment_request[0] + "" + comment_request[1];
//                                            int number = Int32.Parse(comment_request_start);
//                                            comment_request = "";
//                                        }
//                                        catch (Exception)
//                                        {
//                                            // Check comment request
//                                            if (!comment_request.Equals("")) break;
//                                        }
//                                    }

//                                    // Check comment by request
//                                    if (!comment_request.Equals(""))
//                                    {
//                                        GroupFooter3.Visible = true;
//                                        xrLabel_comment2.Text = comment_request;
//                                    }
//                                }
//                                //////////////////////////////// END Comment CODE OR COMMENT FREE TEXT  //////////////////////////
//                                GroupHeader2.Visible = true;

//                                // P04 Special check after MGIT Media result.   :CODE CASE : P04_0001
//                                if (Bool_P04Special)
//                                {
//                                    // Before if Status Plilim GroupHeader4.Visible = false;

//                                    GroupHeader4.Visible = true;
//                                    GroupHeader2.Visible = true;
//                                }
//                                else
//                                {
//                                    if (ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString() != "")
//                                    {
//                                        xrTableCell_specialP04.Text = ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString();
//                                    }
//                                }
//                                // END :CODE CASE : P04_0001


//                                //strOBX += "MB" + ds_res.Tables["result"].Rows[0]["SUBREQMBAGARID"].ToString() + "^" + ds_res.Tables["result"].Rows[0]["FULLTEXT"].ToString() + "||" + ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString() + "\r\n";
//                            }
//                        }
//                    }
//                    //========================= End P04 No growth =====================//
//                }   // END OF ELSE MAIN.

                //lblValidateType.Text = validateType.Value.ToString();
                //  lblReportByType.Text = validateType.Value.ToString() + " BY:";

                /*==================================================
               * CHECK USERS VALIDATED and APPROVED. 
               * REM: Clinical   = Approved by.
               * REM: Technical  = Report by.
               ====================================================

# Status in SUBREQMB_ACTIONS
1   Result phoned
2	Refer requested
3	Cancel refer
4	Stopped requests
5	Restarted requests
6	Negative exam
7	Complete exam
8	Clinical validated requests
9	Technical validated requests
10 	Start nocosomial inquiry
11	Done nocosomial inquiry
12	Cancel nocosomial inquiry
13	View visual aspect
14	View chemistry
15	View cytology
16	View direct exam
17	View identification
18	View sensitivities
19	Isolat
20	Negative consolidate
21	Regular cyto consolidate
22	Culture prog consolidate
23	Consolidate
24	Reopen
25	Mandatory declaration done
26	Comments consolidate
27	Audit consolidate
28	Notes warning
29	Update comment
30	Requests declaration
31	Audit reopen
32	Audit technical validation
33	Send to validation queue
34	Request last save
35	Material consolidate
36	Source consolidate
37	Technique consolidate
38	Topography consolidate
39	Alert sent
40	Pathogenic consolidate
41	Complete exam consolidate
42	Flag infectious history
43	Audit the removal of a demographic alert
====================================================*/

                //try
                //{
                //    // Technical & Clinical User & date/time
                //    //if (subRequestsID != "")
                //    //{
                //    //    ReportConfigurationController objConfig = new ReportConfigurationController();
                //    //    DataTable dt = null;
                //    //    AllVaraibleM objAllVaraibleM = new AllVaraibleM();
                //    //    try
                //    //    {
                //    //        objAllVaraibleM.GET_Val_SubReqID = subRequestsID;
                //    //        dt = objConfig.Get_Validate_User(objAllVaraibleM);


                //    //        string dsCount = dt.Rows.Count.ToString();

                //    //        if (dt.Rows.Count > 0)
                //    //        {
                //    //            if (dsCount == "1")
                //    //            {
                //    //                if (dt.Rows[0]["NATIONALCODE"].ToString() == null || String.IsNullOrWhiteSpace(dt.Rows[0]["NATIONALCODE"].ToString()))
                //    //                {
                //    //                    lblApproveBy.Text = dt.Rows[0]["USERNAME"].ToString();
                //    //                    if (Str_calendar == "TH")
                //    //                    {
                //    //                        xrDTApproveBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                //    //                    }
                //    //                    else
                //    //                    {
                //    //                        xrDTApproveBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_ENG);
                //    //                    }
                //    //                }
                //    //                else
                //    //                {
                //    //                    lblApproveBy.Text = dt.Rows[0]["USERNAME"].ToString() + " (" + dt.Rows[0]["NATIONALCODE"].ToString() + ")";
                //    //                    if (Str_calendar == "TH")
                //    //                    {
                //    //                        xrDTApproveBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                //    //                    }
                //    //                    else
                //    //                    {
                //    //                        xrDTApproveBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_ENG);
                //    //                    }
                //    //                }
                //    //            }
                //    //            else
                //    //            {
                //    //                if (dt.Rows[0]["NATIONALCODE"].ToString() == null || String.IsNullOrWhiteSpace(dt.Rows[0]["NATIONALCODE"].ToString()))
                //    //                {
                //    //                    lblApproveBy.Text = dt.Rows[0]["USERNAME"].ToString();
                //    //                    if (Str_calendar == "TH")
                //    //                    {
                //    //                        xrDTApproveBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                //    //                    }
                //    //                    else
                //    //                    {
                //    //                        xrDTApproveBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_ENG);
                //    //                    }

                //    //                }
                //    //                else
                //    //                {
                //    //                    lblApproveBy.Text = dt.Rows[0]["USERNAME"].ToString() + " (" + dt.Rows[0]["NATIONALCODE"].ToString() + ")";
                //    //                    if (Str_calendar == "TH")
                //    //                    {
                //    //                        xrDTApproveBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                //    //                    }
                //    //                    else
                //    //                    {
                //    //                        xrDTApproveBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_ENG);
                //    //                    }

                //    //                }
                //    //            }
                //    //        }
                //    //    }
                //    //    catch (Exception ex)
                //    //    {
                //    //        throw new Exception(ex.Message);
                //    //    }

                //    //    /*
                //    //     * ###############
                //    //     * For Validate user & date/time
                //    //     * */

                //    //    DataTable dtTechval = null;
                //    //    try
                //    //    {
                //    //        objAllVaraibleM.GET_Val_SubReqID = subRequestsID;
                //    //        dtTechval = objConfig.Get_TechVal_User(objAllVaraibleM);

                //    //        if (dtTechval.Rows.Count > 0)
                //    //        {
                //    //            if (dt.Rows[0]["NATIONALCODE"].ToString() == null || String.IsNullOrWhiteSpace(dt.Rows[0]["NATIONALCODE"].ToString()))
                //    //            {
                //    //                lblLastSave.Text = dtTechval.Rows[0]["USERNAME"].ToString();
                //    //                if (Str_calendar == "TH")
                //    //                {
                //    //                    xrDTReportBy.Text = ((DateTime)(dtTechval.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                //    //                }
                //    //                else
                //    //                {
                //    //                    xrDTReportBy.Text = ((DateTime)(dtTechval.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_ENG);
                //    //                }
                //    //            }
                //    //            else
                //    //            {
                //    //                lblLastSave.Text = dtTechval.Rows[0]["USERNAME"].ToString() + " (" + dtTechval.Rows[0]["NATIONALCODE"].ToString() + ")";
                //    //                if (Str_calendar == "TH")
                //    //                {
                //    //                    xrDTReportBy.Text = ((DateTime)(dtTechval.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                //    //                }
                //    //                else
                //    //                {
                //    //                    xrDTReportBy.Text = ((DateTime)(dtTechval.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_ENG);
                //    //                }
                //    //            }
                //    //        }
                //    //        else
                //    //        {
                //    //            if (dt.Rows[0]["NATIONALCODE"].ToString() == null || String.IsNullOrWhiteSpace(dt.Rows[0]["NATIONALCODE"].ToString()))
                //    //            {
                //    //                lblLastSave.Text = dt.Rows[0]["USERNAME"].ToString();
                //    //                if (Str_calendar == "TH")
                //    //                {
                //    //                    xrDTReportBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                //    //                }
                //    //                else
                //    //                {
                //    //                    xrDTReportBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                //    //                }
                //    //            }
                //    //            else
                //    //            {
                //    //                lblLastSave.Text = dt.Rows[0]["USERNAME"].ToString() + " (" + dt.Rows[0]["NATIONALCODE"].ToString() + ")";
                //    //                if (Str_calendar == "TH")
                //    //                {
                //    //                    xrDTReportBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                //    //                }
                //    //                else
                //    //                {
                //    //                    xrDTReportBy.Text = ((DateTime)(dt.Rows[0]["ACTIONMARKDATE"])).ToString("dd-MM-yyyy HH:mm:ss", cultureinfo_TH);
                //    //                }
                //    //            }
                //    //        }
                //    //    }
                //    //    catch (Exception ex)
                //    //    {
                //    //        throw new Exception(ex.Message);
                //    //    }
                //    //    finally
                //    //    {
                //    //        objAllVaraibleM = null;
                //    //    }
                //    //    // 
                //    //    if (lblApproveBy.Text == "")
                //    //    {
                //    //        lblApproveBy.Text = lblLastSave.Text;
                //    //    }

                //    //     /*==============================
                //    //      * SEND RESULTS TO HIS (RAW DATA)
                //    //      * =============================*/
                //    //    //try
                //    //    //{
                //    //    //    if (CheckStatus.SendTOHIS == "YES")
                //    //    //    {
                //    //    //        //string strOBXHL7 = strOBX;
                //    //    //        //removeFirstForEdit(accessnumber, potocalnumber);
                //    //    //        System.Threading.Thread.Sleep(3000);//will wait for 5 seconds and generate final message.
                //    //    //        //ExportFileToHL7.SendToHIS_culture(strOBXHL7, accessnumber, potocalnumber, pathhtml, subRequestsID, lblGroupTest.Text, testcode);
                //    //    //        //ORUReporting.Export_HL7messages.SendToHIS_culture(strOBXHL7, accessnumber, potocalnumber, pathhtml, subRequestsID, lblGroupTest.Text, testcode);
                //    //    //        //ORUReporting.Export_HL7messages.SendPDFArchive("CULTURE", accessnumber, potocalnumber, pathhtml, subRequestsID, lblGroupTest.Text, testcode);
                //    //    //        OEM.Export_HL7messages.SendToNexlab_OUL24("CULTURE", accessnumber, potocalnumber, pathhtml, subRequestsID, lblGroupTest.Text, testcode);
                //    //    //    }
                //    //    //    /*=============================================================
                //    //    //    * DATE:2021-04
                //    //    //    * Desception: Save data Log.
                //    //    //    =============================================================*/
                //    //    //}
                //    //    //catch (Exception ex)
                //    //    //{
                //    //    //    throw new Exception(ex.Message);
                //    //    //}
                //    //}
                //}
                //catch (Exception ex)
                //{
                //}


                //                if (subRequestsID != "")
                ////                {
                //                    string sqlVal = @"SELECT 
                //SUBREQMB_ACTIONS.SUBACTIONMARKID
                //,SUBREQMB_ACTIONS.SUBREQUESTID
                //,SUBREQMB_ACTIONS.ACTIONMARKTYPE
                //,SUBREQMB_ACTIONS.ACTIONMARKDATE
                //,SUBREQMB_ACTIONS.ACTIONMARKUSERID
                //,SUBREQMB_ACTIONS.ACTIONMARKDATA1
                //,SUBREQMB_ACTIONS.ACTIONMARKDATA2
                //,SUBREQMB_ACTIONS.ACTIONMARKLINK 
                //,USERS.USERNAME,USERS.NATIONALCODE
                //FROM SUBREQMB_ACTIONS
                //LEFT OUTER JOIN USERS ON SUBREQMB_ACTIONS.ACTIONMARKUSERID = USERS.USERID 
                //WHERE SUBREQUESTID = '" + subRequestsID + "' AND  ( ACTIONMARKTYPE = 41 )  Order by SUBREQMB_ACTIONS.SUBACTIONMARKID DESC";

                //                    SqlCommand cmdVal = new SqlCommand(sqlVal, conn);
                //                    DataSet dsVal = new DataSet();
                //                    SqlDataAdapter adpVal = new SqlDataAdapter(cmdVal);
                //                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                //                    dsVal.Clear();
                //                    adpVal.Fill(dsVal, "result");
                //                    string dsCount = dsVal.Tables["result"].Rows.Count.ToString();
                //                    if (dsVal.Tables["result"].Rows.Count > 0)
                //                    {
                //                        if (dsCount == "1")
                //                        {
                //                            if (dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() != "")
                //                            {
                //                                lblApproveBy.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString() + " (" + dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() + ")";
                //                                xrDTApproveBy.Text = ((DateTime)(dsVal.Tables["result"].Rows[0]["ACTIONMARKDATE"])).ToString(format_DT, Cultureinfo);
                //                            }
                //                            else
                //                            {
                //                                if (dsVal.Tables["result"].Rows[0]["ACTIONMARKUSERID"].ToString() != "SYS")
                //                                {
                //                                    lblApproveBy.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString();
                //                                }
                //                                else
                //                                {
                //                                    // lblApproveBy.Text = "PITAK SANTANIRAND (ทน.2212)";
                //                                    //   lblApproveBy.Text = lblLastSave.Text;
                //                                }
                //                            }
                //                        }
                //                        else
                //                        {
                //                            if (dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() != "")
                //                            {
                //                                lblApproveBy.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString() + " (" + dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() + ")";
                //                            }
                //                            else
                //                            {
                //                                lblApproveBy.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString();
                //                            }
                //                        }
                //                        string sqlTech = @"SELECT 
                // SUBREQMB_ACTIONS.SUBACTIONMARKID
                //,SUBREQMB_ACTIONS.SUBREQUESTID
                //,SUBREQMB_ACTIONS.ACTIONMARKTYPE
                //,SUBREQMB_ACTIONS.ACTIONMARKDATE
                //,SUBREQMB_ACTIONS.ACTIONMARKUSERID
                //,SUBREQMB_ACTIONS.ACTIONMARKDATA1
                //,SUBREQMB_ACTIONS.ACTIONMARKDATA2
                //,SUBREQMB_ACTIONS.ACTIONMARKLINK 
                //,USERS.USERNAME,USERS.NATIONALCODE
                // FROM SUBREQMB_ACTIONS
                // LEFT OUTER JOIN USERS ON SUBREQMB_ACTIONS.ACTIONMARKUSERID = USERS.USERID 
                // WHERE SUBREQUESTID = '" + subRequestsID + "' AND  ( ACTIONMARKTYPE = 23 )  Order by SUBREQMB_ACTIONS.SUBACTIONMARKID DESC";

                //                        Writedatalog.WriteLog_Reporting("Tack action Tecnical validate time -->" + "\r\n" + sqlTech);

                //                        SqlCommand cmdTech = new SqlCommand(sqlTech, conn);
                //                        DataSet dsTech = new DataSet();
                //                        SqlDataAdapter adpTech = new SqlDataAdapter(cmdTech);
                //                        if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                //                        dsTech.Clear();
                //                        adpTech.Fill(dsTech, "result");
                //                        if (dsTech.Tables["result"].Rows.Count > 0)
                //                        {
                //                            if (dsTech.Tables["result"].Rows[0]["NATIONALCODE"].ToString() != "")
                //                            {
                //                                lblLastSave.Text = dsTech.Tables["result"].Rows[0]["USERNAME"].ToString() + " (" + dsTech.Tables["result"].Rows[0]["NATIONALCODE"].ToString() + ")";
                //                                xrDTReportBy.Text = ((DateTime)(dsTech.Tables["result"].Rows[0]["ACTIONMARKDATE"])).ToString(format_DT, Cultureinfo);
                //                            }
                //                            else
                //                            {
                //                                lblLastSave.Text = dsTech.Tables["result"].Rows[0]["USERNAME"].ToString();
                //                                xrDTReportBy.Text = ((DateTime)(dsTech.Tables["result"].Rows[0]["ACTIONMARKDATE"])).ToString(format_DT, Cultureinfo);
                //                            }
                //                        }
                //                        else
                //                        {
                //                            if (dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() != "")
                //                            {
                //                                lblLastSave.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString() + " (" + dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() + ")";
                //                                xrDTReportBy.Text = ((DateTime)(dsTech.Tables["result"].Rows[0]["ACTIONMARKDATE"])).ToString(format_DT, Cultureinfo);
                //                            }
                //                            else
                //                            {
                //                                lblLastSave.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString();
                //                                xrDTReportBy.Text = ((DateTime)(dsTech.Tables["result"].Rows[0]["ACTIONMARKDATE"])).ToString(format_DT, Cultureinfo);
                //                            }
                //                        }

                //    if (lblApproveBy.Text == "")
                //    {
                //        lblApproveBy.Text = lblLastSave.Text;
                //    }

                //    /*==============================
                //      * SEND RESULTS TO HIS (RAW DATA)
                //      * =============================*/
                //    try
                //    {
                //        if (CheckStatus.SendTOHIS == "YES")
                //        {
                //            string strOBXHL7 = strOBX;
                //            //removeFirstForEdit(accessnumber, potocalnumber);
                //            System.Threading.Thread.Sleep(5000);//will wait for 5 seconds and generate final message.
                //            //ExportFileToHL7.SendToHIS_culture(strOBXHL7, accessnumber, potocalnumber, pathhtml, subRequestsID, lblGroupTest.Text, testcode);
                //            ORUReporting.Export_HL7messages.SendToHIS_culture(strOBXHL7, accessnumber, potocalnumber, pathhtml, subRequestsID, lblGroupTest.Text, testcode);
                //            ORUReporting.Export_HL7messages.SendPDFArchive("CULTURE", accessnumber, potocalnumber, pathhtml, subRequestsID, lblGroupTest.Text, testcode);

                //        }

                //        /*=============================================================
                //        * DATE:2021-04
                //        * Desception: Save data Log.
                //        =============================================================*/
                //    }
                //    catch (Exception ex)
                //    {
                //        throw new Exception(ex.Message);
                //    }
                //}
                //} // end of check val.

                // if prelim report not show GroupHeader4
                if (status_prelim)
                {
                    GroupHeader4.Visible = false;
                }

                /*=================================
                 * Teample customize form.
                 * ===============================*/

                if (xrTableCell3.Text == "")
                {
                    xrTable3.Rows.Clear();
                }
                if (xrCultureCommnet.Text == "")
                {
                    xrCultureCommnetTable.Rows.Clear();
                }
                //if (pivotGridField3.FieldName == "" || xrPivotGridField3.FieldName == "" || xrPivotGridField4.FieldName == "")
                //{
                //    Detail.Visible = false;
                //}

                // Check Sensitivities Table
                if (GroupHeader1.Visible == false)
                {
                    if (xrTable_Qutity.Rows.Count > 1)
                    {
                        xrTable_Qutity.LocationF = new PointF(30, 400);
                    }
                    else
                    {
                        xrTable_Qutity.LocationF = new PointF(30, 500);
                        xrCultureCommnet.LocationF = new PointF(30, 500);
                    }

                    if (xrTable5.Rows.Count > 1)
                    {
                        xrTable5.LocationF = new PointF(30, 400);
                    }
                    else
                    {
                        xrTable5.LocationF = new PointF(30, 500);
                    }
                }
                else
                {
                    GroupHeader2.Visible = false;
                }

                // Check Clinical not Technical
                //string tech_report = lblLastSave.Text;
                //string clinical_report = lblApproveBy.Text;
                //if (tech_report.Equals("") && !clinical_report.Equals(""))
                //{
                //    // set technical
                //    lblLastSave.Text = clinical_report;
                //}

                //// Set WaterMask 
                //this.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                //this.Watermark.Font = new Font(this.Watermark.Font.FontFamily, 100);
                //this.Watermark.ForeColor = Color.Gray;
                //this.Watermark.TextTransparency = 210;
                //this.Watermark.ShowBehind = false;
                //this.Watermark.Text = "สำเนา";

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        // Process Text style
        private void Process_StyleText_Organ(string text_organ)
        {
            try
            {
                // Create new row
                XRTableRow row = new XRTableRow();

                // width for process del width Xrtable
                float width_table = 0;

                string res_organ = text_organ;

                string abc = "";
                string tmp = "";

                int firstIndex = 0;  // FontStyle.Italic
                int firstIndexN = 0;  // FontStyle.Regular         
                int lastIndex = 0;
                int lastIndexN = 0;

                if (res_organ.ToString().Contains("<i>") || res_organ.ToString().Contains("<n>"))
                {
                    for (int i = 0; i < res_organ.Length; i++)
                    {

                        tmp += res_organ[i].ToString();

                        if (i > 2)
                        {
                            if (tmp.ToString().Substring(i - 3, 3) == "<i>")
                            {
                                firstIndex = i;

                            }
                            if (i > 4)
                            {
                                //string yyy = xxx.ToString().Substring(i - 3, 3); 
                                if (tmp.ToString().Substring(i - 3, 4) == "</i>")
                                {
                                    lastIndex = i + 1;
                                    abc = tmp.Substring(firstIndex, (lastIndex - 4) - firstIndex);
                                    //string item = xxx.Substring(1, i - 3); 
                                    //Console.WriteLine(abc);

                                    XRTableCell cell1 = new XRTableCell();
                                    cell1.Text = abc;
                                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell2.Font.FontFamily, xrTableCell2.Font.Size, FontStyle.Bold | FontStyle.Italic));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell2.Font.FontFamily, xrTableCell2.Font.Size, FontStyle.Bold | FontStyle.Italic));
                                    cell1.Font = new Font(xrTableCell2.Font.FontFamily, xrTableCell2.Font.Size, FontStyle.Bold | FontStyle.Italic);
                                    row.Cells.Add(cell1);

                                }
                            }

                        }

                        if (i > 2)
                        {
                            if (tmp.ToString().Substring(i - 3, 3) == "<n>")
                            {
                                firstIndexN = i;

                            }
                            if (i > 4)
                            {
                                //string yyy = xxx.ToString().Substring(i - 3, 3); 
                                if (tmp.ToString().Substring(i - 3, 4) == "</n>")
                                {
                                    lastIndexN = i + 1;
                                    abc = tmp.Substring(firstIndexN, (lastIndexN - 4) - firstIndexN);
                                    //string item = xxx.Substring(1, i - 3); 
                                    //Console.WriteLine(abc);

                                    XRTableCell cell1 = new XRTableCell();
                                    cell1.Text = abc;
                                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell2.Font.FontFamily, xrTableCell2.Font.Size, FontStyle.Bold));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell2.Font.FontFamily, xrTableCell2.Font.Size, FontStyle.Bold));
                                    cell1.Font = new Font(xrTableCell2.Font.FontFamily, xrTableCell2.Font.Size, FontStyle.Bold);
                                    row.Cells.Add(cell1);

                                }
                            }
                        }

                    }// END OF For loop.

                    // Set Width end row
                    XRTableCell cell_End = new XRTableCell();
                    cell_End.TextAlignment = TextAlignment.MiddleLeft;
                    cell_End.WidthF = xrTableCell2.WidthF - width_table;
                    row.Cells.Add(cell_End);
                } /* Text is  not Contains <i> or <n> */
                else
                {
                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = text_organ;
                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                    cell1.WidthF = xrTableCell2.WidthF;
                    cell1.Font = new Font(xrTableCell2.Font.FontFamily, xrTableCell2.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    row.Cells.Add(cell1);
                }

                // Add Rows
                xrTable_organ.Rows.Add(row);
            }
            catch (Exception ex)
            {

            }
        }

        private void queryQutity_specialH3()
        {

        //Query topography and detection test (Special,PCR) for special report of the Culture protocol.
            try
            {
                string sql = @"
 SELECT SUBREQUESTS_MB.SUBREQUESTID
,SUBREQUESTS_MB.COLLSOURCEID
,SUBREQUESTS_MB.COLLMATERIALID
,SUBREQUESTS_MB.COLLTECHNIQUEID
,SUBREQUESTS_MB.TOPOGRAPHYID
,SUBREQUESTS_MB.PROTOCOLID
,SUBREQUESTS_MB.SUBREQUESTNUMBER
,SUBREQUESTS_MB.SUBREQUESTCREDATE
,SUBREQUESTS_MB.NORMALCYTOLOGY
,SUBREQUESTS_MB.COLLECTIONDATE
,SUBREQUESTS_MB.NOSOINQUIRY
,SUBREQUESTS_MB.VALIDATIONSTATUS
,SUBREQUESTS_MB.RECEIVEDDATE
,SUBREQUESTS_MB.REQUESTID
,SUBREQUESTS_MB.URGENT
,SUBREQUESTS_MB.NEXTAGARINDEX
,SUBREQUESTS_MB.NEXTORGANISMINDEX
,SUBREQUESTS_MB.STATUSSTART
,SUBREQUESTS_MB.COMPLETED
,SUBREQUESTS_MB.NEGATIVEEXAM
,SUBREQUESTS_MB.PATHOGENICEXAM
,SUBREQUESTS_MB.OPTVALDATE
,SUBREQUESTS_MB.CONSOLIDATIONSTATUS
,SUBREQUESTS_MB.REFERASKED
,SUBREQUESTS_MB.REFERTO
,SUBREQUESTS_MB.COLLMATERIALTXT
,SUBREQUESTS_MB.COLLSOURCETXT
,SUBREQUESTS_MB.COLLTECHNIQUETXT
,SUBREQUESTS_MB.TOPOGRAPHYTXT
,SUBREQUESTS_MB.LABOID
,SUBREQUESTS_MB.SAMPLEID
,DICT_COLL_SOURCES.COLLSOURCEID
,DICT_COLL_SOURCES.COLLSOURCECODE
,DICT_COLL_SOURCES.SHORTTEXT
,DICT_COLL_SOURCES.LISTEXTCODE
,DICT_COLL_MATERIALS.COLLMATERIALID
,DICT_COLL_MATERIALS.COLLMATERIALCODE
,DICT_COLL_MATERIALS.SHORTTEXT
,DICT_COLL_MATERIALS.LISTEXTCODE
,DICT_COLL_TECHNIQUES.COLLTECHNIQUEID
,DICT_COLL_TECHNIQUES.COLLTECHNIQUECODE
,DICT_COLL_TECHNIQUES.SHORTTEXT
,DICT_COLL_TECHNIQUES.LISTEXTCODE
,DICT_TOPOGRAPHIES.TOPOGRAPHYID
,DICT_TOPOGRAPHIES.TOPOGRAPHYCODE
,DICT_TOPOGRAPHIES.FULLTEXT AS 'TOPO'
,DICT_TOPOGRAPHIES.LISTEXTCODE
,DICT_MB_PROTOCOLS.PROTOCOLID
,DICT_MB_PROTOCOLS.PROTOCOLCODE
,DICT_MB_PROTOCOLS.SHORTTEXT
,DICT_MB_PROTOCOLS.PROTOCOLDESCFILE
,DICT_MB_PROTOCOLS.TEXTCLASS 
 FROM SUBREQUESTS_MB  
 LEFT OUTER JOIN DICT_COLL_SOURCES ON  SUBREQUESTS_MB.COLLSOURCEID = DICT_COLL_SOURCES.COLLSOURCEID  
 LEFT OUTER JOIN DICT_COLL_MATERIALS ON  SUBREQUESTS_MB.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID  
 LEFT OUTER JOIN DICT_COLL_TECHNIQUES ON  SUBREQUESTS_MB.COLLTECHNIQUEID = DICT_COLL_TECHNIQUES.COLLTECHNIQUEID  
 LEFT OUTER JOIN DICT_TOPOGRAPHIES ON  SUBREQUESTS_MB.TOPOGRAPHYID = DICT_TOPOGRAPHIES.TOPOGRAPHYID  
 LEFT OUTER JOIN DICT_MB_PROTOCOLS ON  SUBREQUESTS_MB.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID  
 WHERE  SUBREQUESTS_MB.SUBREQUESTID = '" + subRequestsID + "'";


                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                ds_queryQutity_specialH3 = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds_queryQutity_specialH3.Clear();
                adp.Fill(ds_queryQutity_specialH3, "result");

                string strTopoGraphy = "";
                if (ds_queryQutity_specialH3.Tables["result"].Rows.Count > 0)
                {
                    /*
                     *  SPECIAL REPORTS.
                     *  include topography.
                     */
                    if (ds_queryQutity_specialH3.Tables["result"].Rows[0]["TOPOGRAPHYCODE"].ToString() != "" || ds_queryQutity_specialH3.Tables["result"].Rows[0]["TOPOGRAPHYTXT"].ToString() != "")
                    {
                        GroupHeader4.Visible = true;
                        if (ds_queryQutity_specialH3.Tables["result"].Rows[0]["TOPOGRAPHYCODE"].ToString() != "")
                        {
                            //lblGroupTest.Text += "     For " + ds_queryQutity_specialH3.Tables[0].Rows[0]["TOPO"].ToString();
                            strTopology = ds_queryQutity_specialH3.Tables[0].Rows[0]["TOPO"].ToString();
                            if (!String.IsNullOrEmpty(ds_queryQutity_specialH3.Tables[0].Rows[0]["TOPOGRAPHYID"].ToString()))
                            {
                                strOBX += ds_queryQutity_specialH3.Tables[0].Rows[0]["TOPOGRAPHYID"].ToString() + "^  For " + ds_queryQutity_specialH3.Tables[0].Rows[0]["TOPO"].ToString() + "||-" + "\r\n";
                            }
                        }
                        else
                        {
                            //lblGroupTest.Text += "     For " + ds_queryQutity_specialH3.Tables[0].Rows[0]["TOPOGRAPHYTXT"].ToString();
                            strTopology = ds_queryQutity_specialH3.Tables[0].Rows[0]["TOPO"].ToString();
                            if (!String.IsNullOrEmpty(ds_queryQutity_specialH3.Tables[0].Rows[0]["COLLMATERIALID"].ToString()))
                            {
                                strOBX += ds_queryQutity_specialH3.Tables[0].Rows[0]["COLLMATERIALID"].ToString() + "^  For " + ds_queryQutity_specialH3.Tables[0].Rows[0]["TOPOGRAPHYTXT"].ToString() + "||-" + "\r\n";
                            }
                        }
                        DataSet dsDetection = new DataSet();
                        //cls_querydataset.queryDetectionTest(dsDetection, accessnumber, potocalnumber, conn);

                        string chkSpecial = "";
                        string strResults = "-";
                        for (int i = 0; i < dsDetection.Tables["result"].Rows.Count; i++)
                        {
                            if (dsDetection.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "PCR")
                            {

                                if (dsDetection.Tables[i].Rows[0]["TESTRESULT"].ToString().Contains("^"))
                                {
                                    strResults = dsDetection.Tables[i].Rows[0]["TESTRESULT"].ToString().Replace("^", "*");
                                }
                                else
                                {
                                    strResults = dsDetection.Tables[i].Rows[0]["TESTRESULT"].ToString();
                                    if (strResults == "")
                                    {
                                        strResults = "-";
                                    }
                                }

                                xrTableCell3.Text = dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString();

                                // check OBX not null                            
                                if (!String.IsNullOrEmpty(dsDetection.Tables[i].Rows[0]["SHORTTEXT"].ToString()))
                                {
                                    strOBX += dsDetection.Tables[i].Rows[0]["DETECTIONTESTCODE"].ToString() + "^" + dsDetection.Tables[i].Rows[0]["SHORTTEXT"].ToString() + "||" + strResults + "";
                                }

                            }
                            else if (dsDetection.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "SPECIAL")
                            {
                                strTopoGraphy = dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString();
                                chkSpecial = dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString();


                                if (dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString().Contains("^"))
                                {
                                    strResults = dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString().Replace("^", "*");
                                }
                                else
                                {
                                    strResults = dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString();
                                    if (strResults == "")
                                    {
                                        strResults = "-";
                                    }
                                }

                                // check OBX not null                            
                                if (!String.IsNullOrEmpty(dsDetection.Tables[0].Rows[i]["SHORTTEXT"].ToString()))
                                {
                                    strOBX += dsDetection.Tables[0].Rows[i]["SUBREQMBTESTID"].ToString() + "^" + dsDetection.Tables[0].Rows[i]["SHORTTEXT"].ToString() + "||" + strResults + "";
                                }
                            }
                        }

                        // strTopoGraphy += " " + ds_queryQutity_specialH3.Tables[0].Rows[0]["TOPOGRAPHYTXT"].ToString();
                        if (chkSpecial != "")
                        {
                            // xrTableCell4.Text = strTopoGraphy;
                            xrCultureCommnet.Text += strTopoGraphy;
                            GroupHeader2.Visible = false;
                        }
                        else
                        {
                            //  xrTableCell4.Visible = false;
                        }

                    }// to po is not null.
                    else
                    {

                        //Noting//
                        DataSet dsDetection = new DataSet();
                        //cls_querydataset.queryDetectionTest(dsDetection, accessnumber, potocalnumber, conn);

                        string chkSpecial = "";
                        string strResults = "-";
                        for (int i = 0; i < dsDetection.Tables["result"].Rows.Count; i++)
                        {
                            if (dsDetection.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "PCR")
                            {

                                if (dsDetection.Tables[i].Rows[0]["TESTRESULT"].ToString().Contains("^"))
                                {
                                    strResults = dsDetection.Tables[i].Rows[0]["TESTRESULT"].ToString().Replace("^", "*");
                                }
                                else
                                {
                                    strResults = dsDetection.Tables[i].Rows[0]["TESTRESULT"].ToString();
                                    if (strResults == "")
                                    {
                                        strResults = "-";
                                    }
                                }

                                xrTableCell3.Text = dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString();
                                
                                // check OBX not null                            
                                if (!String.IsNullOrEmpty(dsDetection.Tables[i].Rows[0]["SHORTTEXT"].ToString()))
                                {
                                    strOBX += dsDetection.Tables[i].Rows[0]["DETECTIONTESTCODE"].ToString() + "^" + dsDetection.Tables[i].Rows[0]["SHORTTEXT"].ToString() + "||" + strResults + "";
                                }

                            }
                            else if (dsDetection.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "SPECIAL")
                            {
                                // This Medthod check in :CODE CASE: P04_0001
                                Bool_P04Special = true;
                                // If true use GroupHeader 4 --> Iden TAB
                                // If false use GroupHeader 2 --> Media TAB
                                // END :CODE CASE: P04_0001

                                strTopoGraphy = dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString();
                                chkSpecial = dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString();


                                if (dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString().Contains("^"))
                                {
                                    strResults = dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString().Replace("^", "*");
                                }
                                else
                                {
                                    strResults = dsDetection.Tables[0].Rows[i]["TESTRESULT"].ToString();
                                    if (strResults == "")
                                    {
                                        strResults = "-";
                                    }
                                }

                                // check OBX not null                            
                                if (!String.IsNullOrEmpty(dsDetection.Tables[0].Rows[i]["SHORTTEXT"].ToString()))
                                {
                                    strOBX += dsDetection.Tables[0].Rows[i]["SUBREQMBTESTID"].ToString() + "^" + dsDetection.Tables[0].Rows[i]["SHORTTEXT"].ToString() + "||" + strResults + "";
                                }
                            }
                        }

                        //  strTopoGraphy += " " + ds_queryQutity_specialH3.Tables[0].Rows[0]["TOPOGRAPHYTXT"].ToString();
                        if (chkSpecial != "")
                        {
                            //     xrTableCell4.Text = strTopoGraphy;
                            xrCultureCommnet.Text += strTopoGraphy;
                            xrTableCell_specialP04.Text = strTopoGraphy.ToString();     // This block in GroupHeader 2

                            GroupHeader2.Visible = false;
                        }
                        else if (chkSpecial == "")
                        {
                            Bool_P04Special = false;

                            if (potocalnumber.Value.ToString().Contains("P04"))
                            {
                                GroupHeader2.Visible = true;
                                //xrTableCell_specialP04.Text = strTopoGraphy.ToString();     // This block in GroupHeader 2
                            }
                            //  xrTableCell4.Visible = false;
                        }
                    }
                }
                else
                {
                    //Noting //
                }
            }
            catch (Exception ex)
            {
                //Writedatalog.WriteLog_Reporting(DateTime.Now.ToString() + " Template Culture error method queryQutity_specialH3() : " + ex.Message.ToString());
            }
        }

        private void queryQutity()
        {
            try
            {
                string sql = @"SELECT DISTINCT
 REQUESTS.ACCESSNUMBER
,DICT_MB_DETECT_TESTS.DETECTIONID
,DETECTIONCODE
,DETECTIONCREDATE
,DICT_MB_DETECT_TESTS.MORPHODESC
,DICT_MB_DETECT_TESTS.NOTPRINTABLE
,DICT_MB_DETECT_TESTS.LOGUSERID
,DICT_MB_DETECT_TESTS.LOGDATE
,SUBREQMB_DET_TESTS.TESTRESULT
,DICT_MB_ORGANISMS.ORGANISMCODE
,DICT_MB_ORGANISMS.ORGANISMNAME
,DICT_MB_ORGANISMS.MORPHODESC
,DICT_TOPOGRAPHIES.TOPONAME AS 'TOPOGRAPHIES'
,SUBREQMB_COLONIES.COLONYINDEX
,SUBREQMB_ORGANISMS.ORGANISMINDEX 
,SUBREQMB_ORGANISMS.COMMENTS
 FROM REQUESTS 

LEFT OUTER JOIN MB_REQUESTS ON REQUESTS.REQUESTID = MB_REQUESTS.REQUESTID
LEFT OUTER JOIN SUBREQMB_ORGANISMS ON MB_REQUESTS.MBREQUESTID = SUBREQMB_ORGANISMS.SUBREQUESTID
LEFT OUTER JOIN DICT_MB_ORGANISMS ON SUBREQMB_ORGANISMS.ORGANISMID = DICT_MB_ORGANISMS.ORGANISMID
LEFT OUTER JOIN SUBREQMB_ANTIBIOTICS ON SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_ANTIBIOTICS.SUBREQMBORGID 
LEFT OUTER JOIN DICT_MB_ANTIBIOTICS ON SUBREQMB_ANTIBIOTICS.ANTIBIOTICID = DICT_MB_ANTIBIOTICS.ANTIBIOTICID 
LEFT OUTER JOIN MB_ACTIONS ON MB_REQUESTS.MBREQUESTID = MB_ACTIONS.SUBREQUESTID
LEFT OUTER JOIN DICT_MB_PROTOCOLS ON MB_REQUESTS.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_ORGANISMS.COLONYID = SUBREQMB_COLONIES.COLONYID 
LEFT OUTER JOIN SUBREQMB_DET_TESTS ON SUBREQMB_COLONIES.COLONYID = SUBREQMB_DET_TESTS.COLONYID
LEFT OUTER JOIN DICT_MB_DETECT_TESTS ON SUBREQMB_DET_TESTS.DETECTIONTESTID =  DICT_MB_DETECT_TESTS.DETECTIONID
LEFT OUTER JOIN DICT_TOPOGRAPHIES ON MB_REQUESTS.TOPOGRAPHYID = DICT_TOPOGRAPHIES.TOTPGRAPHYID
						
WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "' " + 
"AND MB_REQUESTS.MBREQNUMBER = '" + potocalnumber.Value.ToString() + "' ORDER BY  SUBREQMB_ORGANISMS.ORGANISMINDEX ";
						

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds.Clear();

                adp.Fill(ds, "result");
                string strOrganismChk = "";
                string checkFontOrgIndex = "";
                if (ds.Tables["result"].Rows.Count > 0)
                {

                    GroupHeader4.Visible = true;

                    int row_ar = ds.Tables["result"].Rows.Count;
                    int column_ar = 2;
                    string[,] ar_2d = new string[row_ar, column_ar];

                    // New ArrayList
                    ArrayList arL_column1 = new ArrayList();
                    ArrayList arL_column2 = new ArrayList();

                    // After print then clean xrTAble.
                    xrTable_Qutity.Rows.Clear();
                    xrTable5.Rows.Clear();
                    detectionTestShowAfterOrg = "";

                    // Comment by colony
                    string comment_colony = "";

                    // Process Array2D
                    string checkSameComment = "";
                    for (int i = 0; i < row_ar; i++)
                    {
                        ar_2d[i, 0] = ds.Tables["result"].Rows[i]["TESTRESULT"].ToString();

                        // get comment by colony
                        comment_colony = ds.Tables["result"].Rows[i]["COMMENTS"].ToString();

                        if (!ds.Tables["result"].Rows[i]["ORGANISMNAME"].ToString().Equals("No growth"))
                        {
                            // เพิ่มตัวเอียง
                            if (ds.Tables["result"].Rows[i]["MORPHODESC"].ToString() != "")
                            {
                                // P04 Special check after MGIT Media result.   :CODE CASE : P04_0001
                                // In case row > 0 check get result in Indentification TAB
                                Bool_P04Special = true;
                                // END :CODE CASE : P04_0001

                                ar_2d[i, 1] = "<n>(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ")</n> " + ds.Tables["result"].Rows[i]["MORPHODESC"].ToString();

                            }
                            else
                            {
                                ar_2d[i, 1] = "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMNAME"].ToString();
                            }
                        }
                        else
                        {
                            // เพิ่มตัวเอียง
                            if (ds.Tables["result"].Rows[i]["MORPHODESC"].ToString() != "")
                            {
                                // P04 Special check after MGIT Media result.   :CODE CASE : P04_0001
                                // In case row > 0 check get result in Indentification TAB
                                Bool_P04Special = true;
                                // END :CODE CASE : P04_0001

                                ar_2d[i, 1] = "<n>(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ")</n> " + ds.Tables["result"].Rows[i]["MORPHODESC"].ToString();
                            }
                            else
                            {
                                ar_2d[i, 1] = "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMNAME"].ToString();
                            }
                        }

                        checkFontOrgIndex = ds.Tables["result"].Rows[i]["ORGANISMINDEX"].ToString();

                        ////////////////// ********************* /////////////////
                        ///// Check WARNING DETECTIONCODE
                        ////////////////// ******************** //////////////////
                        //if (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() != "WARNING".Trim() )
                        //{                     

                        // NEW design Add BETALAC,STRAIN1,STRAIN2,STRAIN3 for version V1.3b 

                        if (ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "ESBL" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "SXT" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "CARBAPENEM" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "METHICILIN" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "VANCOMYCIN" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "COLISTIN" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "MLSB" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "CABAPEMASE" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "DA" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "MCR-1" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "XDR" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "MDR" 
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "PDR"
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "BETALAC"
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "STRAIN1"
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "STRAIN2"
                            || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "VRE"
                            )
                        {
                            // 
                            if (strOrganismChk != ar_2d[i, 1])
                            {
                                Process_StyleText(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                            }//END OF Check organism!!!

                            // Add CARBAPENEM
                            if ((ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "CARBAPENEM") 
                                && (ds.Tables["result"].Rows[i]["TESTRESULT"].ToString().Trim() == "Resistant"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                            }
                            // Add CARBAPENEM
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "CABAPEMASE"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                            }
                            // Add XDR , MDR , PDR
                            // NEW design Add STRAIN1,STRAIN2,STRAIN3
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "XDR") 
                                || (ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "MDR") 
                                || (ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "PDR")
                                || (ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "STRAIN1")
                                || (ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "STRAIN2")
                                || (ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Trim() == "STRAIN3"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["MORPHODESC"].ToString(), "", detectionTestShowAfterOrg);
                            }
                            //Add ESBL in the table.
                            else if (ar_2d[i, 0] != "")
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["MORPHODESC"].ToString() + " " + ar_2d[i, 0], "", detectionTestShowAfterOrg);
                            }

                        }
                        else if (ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "XORC" || ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString().Contains("XOCU"))
                        {
                            /*=================================================
                            * Adding Quatity to left xrTable. (Put in string first)
                             * 
                             *=================================================*/
                            string result_test = ds.Tables["result"].Rows[i]["TESTRESULT"].ToString();
                            if (result_test.StartsWith("CD0"))
                            {
                                result_test = "";
                            }

                            /*======================================
                             * 
                             * Adding Organismsname to xrTable.
                             * 
                             *=====================================*/
                            Process_StyleText(xrTable_Qutity, result_test, ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() != "" && ds.Tables["result"].Rows[i]["ORGANISMCODE"].ToString() != "")
                            {
                                //check code is not special !!! because special code used in H3 template.
                                if (ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() != "SPECIAL" && ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() != "BARCODE".Trim())
                                {
                                    if (strOrganismChk != ar_2d[i, 1])
                                    {
                                        if (ar_2d[i, 0].StartsWith("CD0"))
                                        {
                                            //         AddDataToXrTable(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex);
                                            Process_StyleText(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                                        }
                                        else
                                        {
                                            //        AddDataToXrTable(xrTable_Qutity, ar_2d[i, 0], ar_2d[i, 1], checkFontOrgIndex);
                                            Process_StyleText(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                                        }
                                    }
                                    strOrganismChk = ar_2d[i, 1];
                                    //AddDataToXrTable(xrTable_Qutity, ar_2d[i, 0], ar_2d[i, 1], checkFontOrgIndex);
                                }
                                else if (ds.Tables["result"].Rows[i]["DETECTIONCODE"].ToString() == "BARCODE".Trim())
                                {
                                    if (strOrganismChk != ar_2d[i, 1])
                                    {
                                        if (ar_2d[i, 0].StartsWith("CD0") || ar_2d[i, 0].StartsWith("cd0"))
                                        {
                                            //       AddDataToXrTable(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex);
                                            Process_StyleText(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                                        }
                                        else
                                        {
                                            //      AddDataToXrTable(xrTable_Qutity, ar_2d[i, 0], ar_2d[i, 1], checkFontOrgIndex);
                                            Process_StyleText(xrTable_Qutity, ar_2d[i, 0], ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                                        }
                                    }
                                    strOrganismChk = ar_2d[i, 1];
                                }
                            }
                        }

                        strOrganismChk = ar_2d[i, 1];

                        if (strOrganismChk == "" || strOrganismChk.Trim() == "()".Trim())
                        {
                            GroupHeader4.Visible = true;
                        }

                        //}//END OF WARNING Code!!!!            

                        /*==================*/
                        // Adding topology and compare with organismn
                        //
                        strOganismns_Compare_topo = ds.Tables["result"].Rows[i]["ORGANISMNAME"].ToString();
                        if (strTopology == ds.Tables["result"].Rows[i]["ORGANISMNAME"].ToString())
                        {
                            //string xxx = "TOpo:" + strTopology;
                            xrTableTopology.Rows.Clear();

                            string strText = "";
                            if (ds.Tables["result"].Rows[i]["MORPHODESC"].ToString() != "")
                            {
                                strText = ds.Tables["result"].Rows[i]["MORPHODESC"].ToString();
                            }
                            else
                            {
                                strText = ds.Tables["result"].Rows[i]["ORGANISMNAME"].ToString();
                            }
                            AddTopologyTable(xrTableTopology, strText);
                        }

                        // Check comment by colony
                        if (!comment_colony.Equals(""))
                        {
                            if (checkSameComment != comment_colony)
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + comment_colony, "", "");
                            }
                            checkSameComment = comment_colony;
                        }
                    } //E N D OF For loop.  
       
                    string xx = xrTableCell11.Text;
                    if (xrTableCell11.Text == "" || xrTableCell11.Text == "-")
                    {
                        if (strTopology != "")
                        {
                            //  xrTableTopology.Rows.Clear();
                            xrTableCell11.Text = strGroupTest_Protocol + "   For " + strTopology;
                        }
                        else
                        {
                            xrTableCell11.Text = strGroupTest_Protocol;
                        }
                    }
                    else
                    {
                        if (strTopology != "")
                        {
                            //  xrTableTopology.Rows.Clear();
                            xrTableCell11.Text = strGroupTest_Protocol + "   For " + strTopology;
                        }
                        else
                        {
                            xrTableCell11.Text = strGroupTest_Protocol;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void AddTopologyTable(XRTable xrTableTopology, string text)
        {
            try
            {
                // Create new row
                XRTableRow row = new XRTableRow();
                float width_table = 0;
                XRTableCell cellpotocol = new XRTableCell();
                if (text != "")
                {
                    cellpotocol.Text = strGroupTest_Protocol + "   For ";
                }
                else
                {
                    cellpotocol.Text = strGroupTest_Protocol;
                }
                cellpotocol.TextAlignment = TextAlignment.MiddleLeft;
                width_table += MeasureTextWidthPixels(ReportUnit.Pixels, strGroupTest_Protocol + "   For ", new Font(xrTableCell11.Font.FontFamily, xrTableCell11.Font.Size, FontStyle.Bold));
                cellpotocol.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, strGroupTest_Protocol + "   For ", new Font(xrTableCell11.Font.FontFamily, xrTableCell11.Font.Size, FontStyle.Bold));
                cellpotocol.Font = new Font(xrTableCell11.Font.FontFamily, xrTableCell11.Font.Size, FontStyle.Bold | FontStyle.Regular);
                row.Cells.Add(cellpotocol);

                // width for process del width Xrtable


                string xxx = text;

                string abc = "";
                string tmp = "";


                int firstIndex = 0;  // FontStyle.Italic
                int firstIndexN = 0;  // FontStyle.Regular         
                int lastIndex = 0;
                int lastIndexN = 0;



                if (xxx.ToString().Contains("<i>") || xxx.ToString().Contains("<n>"))
                {
                    for (int i = 0; i < xxx.Length; i++)
                    {

                        tmp += xxx[i].ToString();



                        if (i > 2)
                        {
                            if (tmp.ToString().Substring(i - 3, 3) == "<i>")
                            {
                                firstIndex = i;

                            }
                            if (i > 4)
                            {
                                //string yyy = xxx.ToString().Substring(i - 3, 3); 
                                if (tmp.ToString().Substring(i - 3, 4) == "</i>")
                                {
                                    lastIndex = i + 1;

                                    abc = tmp.Substring(firstIndex, (lastIndex - 4) - firstIndex);
                                    //string item = xxx.Substring(1, i - 3); 
                                    //Console.WriteLine(abc);

                                    XRTableCell cell1 = new XRTableCell();
                                    cell1.Text = abc;
                                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell11.Font.FontFamily, xrTableCell11.Font.Size, FontStyle.Bold));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell11.Font.FontFamily, xrTableCell11.Font.Size, FontStyle.Bold));
                                    //   cell1.WidthF = xrTableCell11.WidthF - width_table;
                                    cell1.Font = new Font(xrTableCell11.Font.FontFamily, xrTableCell11.Font.Size, FontStyle.Bold | FontStyle.Italic);
                                    row.Cells.Add(cell1);

                                }
                            }
                        }

                        if (i > 2)
                        {
                            if (tmp.ToString().Substring(i - 3, 3) == "<n>")
                            {
                                firstIndexN = i;

                            }
                            if (i > 4)
                            {
                                //string yyy = xxx.ToString().Substring(i - 3, 3); 
                                if (tmp.ToString().Substring(i - 3, 4) == "</n>")
                                {
                                    lastIndexN = i + 1;
                                    abc = tmp.Substring(firstIndexN, (lastIndexN - 4) - firstIndexN);
                                    //string item = xxx.Substring(1, i - 3); 
                                    //Console.WriteLine(abc);

                                    XRTableCell cell1 = new XRTableCell();
                                    cell1.Text = abc;
                                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell11.Font.FontFamily, xrTableCell11.Font.Size, FontStyle.Bold));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell11.Font.FontFamily, xrTableCell11.Font.Size, FontStyle.Bold));
                                    //  cell1.WidthF = xrTableCell11.WidthF - width_table;
                                    cell1.Font = new Font(xrTableCell11.Font.FontFamily, xrTableCell11.Font.Size, FontStyle.Bold);
                                    row.Cells.Add(cell1);


                                }
                            }
                        }

                    }// END OF For loop.
                    // Set Width end row
                    XRTableCell cell_End = new XRTableCell();
                    cell_End.TextAlignment = TextAlignment.MiddleLeft;
                    cell_End.WidthF = xrTableCell11.WidthF - width_table;
                    row.Cells.Add(cell_End);
                } /* Text is  not Contains <i> or <n> */
                else
                {
                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = text;
                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                    //   width_table += MeasureTextWidthPixels(ReportUnit.Pixels, text, new Font(xrTableCell11.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold));
                    cell1.WidthF = xrTableCell11.WidthF;
                    //if (!strORGANISMINDEX.Equals(""))
                    //{
                    cell1.Font = new Font(xrTableCell11.Font.FontFamily, xrTableCell11.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    //}
                    //else
                    //{//ESBL Font!
                    //    cell1.Font = new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Regular);
                    //}
                    row.Cells.Add(cell1);
                }

                // Add Rows
                xrTableTopology.Rows.Add(row);
            }
            catch (Exception ex)
            {

                //Writedatalog.WriteLog_Reporting(DateTime.Now.ToString() + " Template Culture error method AddTopologyTable() : " + ex.Message.ToString());
            }
         
        }

        private void Process_StyleText(XRTable xrTable_Qutity, string strQutity, string text, string strORGANISMINDEX, string detectionTestShowAfterOrg)
        {
            try
            {
                // Create new row
                XRTableRow row = new XRTableRow();

                XRTableCell cellQutity = new XRTableCell();
                cellQutity.Text = strQutity;
                cellQutity.TextAlignment = TextAlignment.MiddleRight;
                cellQutity.WidthF = xrTableCell6.WidthF;
                cellQutity.Font = new Font(xrTableCell6.Font.FontFamily, xrTableCell6.Font.Size, FontStyle.Regular);
                row.Cells.Add(cellQutity);

                //Cell Center seperate column of the result and oganismn.
                XRTableCell cellCenter = new XRTableCell();
                //cellCenter.WidthF = xrTableCell18.WidthF;
                cellCenter.WidthF = 65;
                row.Cells.Add(cellCenter);

                // width for process del width Xrtable
                float width_table = 0;

                string xxx = text + detectionTestShowAfterOrg;

                string abc = "";
                string tmp = "";
                string chkForgotPutTag = ""; //Check for the dictionary don't have <i>,<n>

                int firstIndex = 0;  // FontStyle.Italic
                int firstIndexN = 0;  // FontStyle.Regular         
                int lastIndex = 0;
                int lastIndexN = 0;

                if (xxx.ToString().Contains("<i>") || xxx.ToString().Contains("<n>"))
                {
                    for (int i = 0; i < xxx.Length; i++)
                    {

                        tmp += xxx[i].ToString();

                        if (i > 2)
                        {
                            if (tmp.ToString().Substring(i - 3, 3) == "<i>")
                            {
                                firstIndex = i;

                            }
                            if (i > 4)
                            {
                                //string yyy = xxx.ToString().Substring(i - 3, 3); 
                                if (tmp.ToString().Substring(i - 3, 4) == "</i>")
                                {
                                    lastIndex = i + 1;
                                    abc = tmp.Substring(firstIndex, (lastIndex - 4) - firstIndex);
                                    //string item = xxx.Substring(1, i - 3); 
                                    //Console.WriteLine(abc);

                                    XRTableCell cell1 = new XRTableCell();
                                    cell1.Text = abc;
                                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold));
                                    cell1.Font = new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold | FontStyle.Italic);
                                    row.Cells.Add(cell1);

                                }
                            }

                        }

                        if (i > 2)
                        {
                            if (tmp.ToString().Substring(i - 3, 3) == "<n>")
                            {
                                firstIndexN = i;

                            }
                            if (i > 4)
                            {
                                //string yyy = xxx.ToString().Substring(i - 3, 3); 
                                if (tmp.ToString().Substring(i - 3, 4) == "</n>")
                                {
                                    lastIndexN = i + 1;
                                    abc = tmp.Substring(firstIndexN, (lastIndexN - 4) - firstIndexN);
                                    //string item = xxx.Substring(1, i - 3); 
                                    //Console.WriteLine(abc);

                                    XRTableCell cell1 = new XRTableCell();
                                    cell1.Text = abc;
                                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold));
                                    cell1.Font = new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold);
                                    row.Cells.Add(cell1);

                                }
                            }
                        }

                    }// END OF For loop.
                    // Set Width end row
                    XRTableCell cell_End = new XRTableCell();
                    cell_End.TextAlignment = TextAlignment.MiddleLeft;
                    cell_End.WidthF = xrTableCell4.WidthF - width_table;
                    row.Cells.Add(cell_End);
                } /* Text is  not Contains <i> or <n> */
                else
                {
                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = text;
                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                    //  width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold));
                    cell1.WidthF = xrTableCell4.WidthF;
                    if (!strORGANISMINDEX.Equals(""))
                    {
                        cell1.Font = new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    }
                    else
                    {//ESBL Font!
                        cell1.Font = new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Regular);
                    }
                    row.Cells.Add(cell1);
                }


                // Add Rows
                xrTable5.Rows.Add(row);
            }
            catch (Exception ex)
            {
               // Writedatalog.WriteLog_Reporting(DateTime.Now.ToString() + " Template Culture error method Process_StyleText() : " + ex.Message.ToString());
            }
         
        }
        // Calculation Cell For Have word spp
        public static float MeasureTextWidthPixels(ReportUnit unit, string text, Font font)
        {
            Graphics gr = Graphics.FromHwnd(IntPtr.Zero);

            int factor;
            if (unit == ReportUnit.HundredthsOfAnInch)
            {
                gr.PageUnit = GraphicsUnit.Inch;
                factor = 100;
            }
            else
            {
                gr.PageUnit = GraphicsUnit.Millimeter;
                factor = 10;
            }

            SizeF size = gr.MeasureString(text, font);
            gr.Dispose();

            float s = size.Width * factor;
            return s;
        }

        private void xrPivotGrid1_CustomCellDisplayText_4(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotCellDisplayTextEventArgs e)
        {
            PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();


            if (e.DataField == PivotSIR)
            {
                //e.DisplayText = String.Format("{0}", ds[0][e.DataField]);

                for (int i = 0; i < ds.RowCount; i++)
                {
                    e.DisplayText = String.Format("{0}", ds[i][e.DataField]);
                }
            }


            ////RESULT.
            else if (e.DataField == PivotValue)
            {
                for (int i = 0; i < ds.RowCount; i++)
                {

                    //        string strRes = String.Format("{0}", ds[i][e.DataField]);

                    //        string[] sss = strRes.ToString().Split('.');
                    //        if (sss.Length > 1)
                    //        {
                    //            if (sss[1].Length >= 0) // มีจุดทศนิยม
                    //            {
                    //                string res = sss[1].TrimEnd('0');
                    //                if (res.ToString() != "") // จุดทศนิยม != 0 หรือ ""
                    //                {
                    //                    e.DisplayText = String.Format("{0}", sss[0] + "." + res);
                    //                }
                    //                else
                    //                {
                    //                    e.DisplayText = String.Format("{0}", sss[0]);
                    //                }
                    //            }
                    //            else // ถ้าไม่มีจุดทศนิยม ให้แสดงค่า default จาก db
                    //            {
                    //                e.DisplayText = String.Format("{0}", ds[i][e.DataField]);
                    //            }
                    //        }
                    //        else
                    //        {
                    e.DisplayText = String.Format("{0}", ds[i][e.DataField]);
                    //        }

                    //    }
                    //    if (e.Value == "0" || e.Value == "")
                    //    {
                    //        e.DisplayText = "-";
                }

            }
            else if (e.DataField == pivotUnits)
            {
                for (int i = 0; i < ds.RowCount; i++)
                {
                    e.DisplayText = String.Format("{0}", ds[i][e.DataField]);
                }
            }
        }

        // New Design Query Patient for Long name from ORM database
        //private void Query_Patient_ORM(string Accessnumber_orm)
        //{
        //    try
        //    {
        //        string sql_ORM_Patient = "";
        //        string newAcc = Accessnumber_orm.Substring(1, 9);
        //        sql_ORM_Patient = @"SELECT ACCESSNUMBER,HN,NAME,LASTNAME FROM PRE_ORMLOG where ACCESSNUMBER='" + newAcc + "' ";
        //        connORM = new Connection_DB().ConnectORM();
        //        SqlCommand cmdORMPATIENT = new SqlCommand(sql_ORM_Patient, connORM);
        //        SqlDataAdapter adpORMPATIENT = new SqlDataAdapter(cmdORMPATIENT);
        //        DataSet dsORMPATIENT = new DataSet();
        //        if (connORM.State == ConnectionState.Open) connORM.Close(); connORM.Open();
        //        dsORMPATIENT.Clear();
        //        adpORMPATIENT.Fill(dsORMPATIENT, "result_orm_patient");
        //        if (dsORMPATIENT.Tables["result_orm_patient"].Rows.Count > 0)
        //        {
        //            StrPatientName_ORM = dsORMPATIENT.Tables["result_orm_patient"].Rows[0]["NAME"].ToString();
        //            StrPatientLast_ORM = dsORMPATIENT.Tables["result_orm_patient"].Rows[0]["LASTNAME"].ToString();
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        Writedatalog.WriteLog_Reporting(DateTime.Now.ToString() + " Error Find from ORM connection db : " + ex.Message.ToString()); }
        //    }
        //private string[] GET_User()
        //{
        //    string[] Get_User = new string[2];
        //    // Search Username
        //    ReportConfigurationController objConfig = new ReportConfigurationController();
        //    DataTable dt = null;
        //    AllVaraibleM objAllVaraibleM = new AllVaraibleM();

        //    try
        //    {
        //        objAllVaraibleM.GET_User = user.Value.ToString();
        //        dt = objConfig.Get_Username(objAllVaraibleM);

        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows[0]["USERNAME"].ToString() == null || String.IsNullOrWhiteSpace(dt.Rows[0]["USERNAME"].ToString()))
        //            {
        //                Get_User[0] = "-";
        //            }
        //            else
        //            {
        //                Get_User[0] = dt.Rows[0]["USERNAME"].ToString();
        //            }

        //            if (dt.Rows[0]["NATIONALCODE"].ToString() == null || String.IsNullOrWhiteSpace(dt.Rows[0]["NATIONALCODE"].ToString()))
        //            {
        //                Get_User[1] = "";
        //            }
        //            else
        //            {
        //                Get_User[1] = dt.Rows[0]["NATIONALCODE"].ToString();
        //            }
        //        }
        //        else
        //        {
        //            Get_User[0] = "-";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        objAllVaraibleM = null;
        //    }

        //    return Get_User;
        //}

        //private string GET_Speciman(string Protocol)
        //{
        //    string Str_Specimen = "";

        //    ReportConfigurationController objConfig = new ReportConfigurationController();
        //    AllVaraibleM objAllVaraibleM = new AllVaraibleM();
        //    DataTable dt = null;

        //    objAllVaraibleM.SET_Protocol = Protocol;
        //    dt = objConfig.Get_Specimen_(objAllVaraibleM);

        //    if (dt.Rows.Count > 0)
        //    {
        //        Str_Specimen = dt.Rows[0]["COLLMATERIALNAME"].ToString();
        //    }
        //    else
        //    {
        //        Str_Specimen = "-";
        //    }

        //    return Str_Specimen;
        //}

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
               throw new Exception (ex.Message);
            }
            return Result;
        }
        private string[] Get_Array_report2(string Str_PROTOCOLNUM, string Str_ACCESSNUMBER, string Str_STATUS)
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
                    Result[6] = dt.Rows[0]["COLLMATERIALTEXT"].ToString();
                    Result[7] = dt.Rows[0]["SPM_NAME"].ToString();
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
