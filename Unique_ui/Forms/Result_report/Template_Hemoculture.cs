using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using System.Data;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPrinting;
using System.Text;

namespace UNIQUE
{
    public partial class Template_Hemoculture_p12 : DevExpress.XtraReports.UI.XtraReport
    {
        SqlConnection conn;
        string strBfauto;
        string strReqID = "";
        string strSubReqID = "";
        string strOBX = "";
        int obxCount = 1;
        string detectionTestShowAfterOrg = "";
        string NO_detectionTestShowAfterOrg = "";

        DataSet ds_queryQutity_specialHeader3;

        // Format DateTime en-US
        System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");
		string format_DT = "dd/MM/yyyy HH:mm";

        // comment request
        string comment_request = "";

        public Template_Hemoculture_p12()
        {
            InitializeComponent();
        }

        private void Template_Hemoculture_p12_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTable1.Rows.Clear();
            xrTableCell17.Text = DateTime.Now.ToString();
            string strLogdate = "";
            //////try
            //////{
            //////    conn = new Connection_DB().Connect();
            //////}
            //catch (SqlException ex) {
            //    Writedatalog.WriteLog("Template_Hemoculture_p12: processing");
            //    Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method Template_Hemoculture_p12_BeforePrint() : " + ex.Message.ToString()); 
            //}

            try
            {

                string sql_pat = @" SELECT DISTINCT REQUESTS.REQUESTID, PATIENTS.PATID, PATIENTS.PATNUMBER as 'HN', PATIENTS.INTNUMBER, PATIENTS.REFHOSPNUMBER, PATIENTS.BENNUMBER, PATIENTS.NAME, PATIENTS.MAIDENNAME, PATIENTS.FIRSTNAME AS 'LASTNAME', PATIENTS.NAMESK, PATIENTS.FIRSTNAMESK, PATIENTS.ADDRESS1, PATIENTS.ADDRESS2, PATIENTS.CITY, PATIENTS.STATE, PATIENTS.POSTALCODE, PATIENTS.COUNTRY, PATIENTS.BIRTHDATE, CASE (PATIENTS.SEX) WHEN 1 THEN 'M' WHEN 2 THEN 'F' ELSE '-' END AS 'SEX', PATIENTS.REFDOCTOR, PATIENTS.REFLOCATION, PATIENTS.BG_ABO, PATIENTS.BG_RHESUS, PATIENTS.RECALLDATE, PATIENTS.PATROW,PATIENTS.RECBYCNX , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as ageY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as ageM 
        FROM PATIENTS, REQUESTS 
            LEFT OUTER JOIN SUBREQUESTS_MB on REQUESTS.REQUESTID = SUBREQUESTS_MB.REQUESTID
        WHERE PATIENTS.PATID = REQUESTS.PATID 
        AND REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "' " +
                "   and SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString().Trim() + "'" + "   ORDER BY NAME, FIRSTNAME";
                Writedatalog.WriteLog("Step sql_patient ---> " +  sql_pat);
                SqlCommand cmd = new SqlCommand(sql_pat, conn);
                SqlDataAdapter adp_pat = new SqlDataAdapter(cmd);
                DataSet ds_pat = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds_pat.Clear();

                adp_pat.Fill(ds_pat, "result");

                if (ds_pat.Tables["result"].Rows.Count > 0)
                {
                    xrTableCell_hn.Text = ds_pat.Tables["result"].Rows[0]["HN"].ToString().TrimStart('0');
                    xrTableCell_Name.Text = ds_pat.Tables["result"].Rows[0]["NAME"].ToString() + " " + ds_pat.Tables["result"].Rows[0]["LASTNAME"].ToString();
                    string sex = "";
                    if (ds_pat.Tables["result"].Rows[0]["SEX"].ToString().Equals("M"))
                    {
                        sex = "Male";
                    }
                    else if ((ds_pat.Tables["result"].Rows[0]["SEX"].ToString().Equals("F")))
                    {
                        sex = "Female";
                    }
                    else
                    {
                        sex = "-";
                    }
                    xrTableCell_sex.Text = sex;
                    strReqID = ds_pat.Tables["result"].Rows[0]["REQUESTID"].ToString();
                    xrTableCell_Labno.Text = accessnumber.Value.ToString();
                    //New design
                    //xrTableCell55.Text = Hosnumber.Value.ToString();
                    //xrTableCell12.Text = Visitnum.Value.ToString();
                    //Nes design


                    //xrTableCell_ProtocolNo.Text = potocalnumber.Value.ToString();
                    xrTableCell26.Text = potocalnumber.Value.ToString();
                    lblValidateType.Text = validateType.Value.ToString();

                    string str_AgeY = ds_pat.Tables["result"].Rows[0]["ageY"].ToString() + " Year(s)";
                    string str_AgeM = ds_pat.Tables["result"].Rows[0]["ageM"].ToString() + " Month(s)";
                    xrTableCell_age.Text = str_AgeY + " " + str_AgeM;

                    if (strReqID != "")
                    {

                        string sql_res = @"SELECT SUBREQMB_DET_TESTS.SUBREQMBTESTID,SUBREQMB_DET_TESTS.DETECTIONTESTID,SUBREQMB_DET_TESTS.SUBREQUESTID,SUBREQMB_DET_TESTS.COLONYID,SUBREQMB_DET_TESTS.SUBREQMBAGARID,SUBREQMB_DET_TESTS.TESTRESULT,SUBREQMB_DET_TESTS.VALREQUESTED,SUBREQMB_DET_TESTS.CONSOLIDATIONSTATUS,SUBREQMB_DET_TESTS.NOTPRINTABLE,SUBREQMB_DET_TESTS.CREATEUSER,SUBREQMB_DET_TESTS.CREATIONDATE,SUBREQMB_DET_TESTS.RESUPDDATE,SUBREQMB_DET_TESTS.LOGUSERID,SUBREQMB_DET_TESTS.LOGDATE,SUBREQMB_DET_TESTS.COMMENTS,DICT_MB_DETECT_TESTS.DETECTIONTESTID,DICT_MB_DETECT_TESTS.DETECTIONTESTCODE,DICT_MB_DETECT_TESTS.DETECTIONTESTCREDATE,DICT_MB_DETECT_TESTS.SHORTTEXT,DICT_MB_DETECT_TESTS.FULLTEXT,DICT_MB_DETECT_TESTS.DICTCONSOSTATUS,DICT_MB_DETECT_TESTS.NOTPRINTABLE,DICT_MB_DETECT_TESTS.FIRSTITEMID,DICT_MB_DETECT_TESTS.STARTVALIDDATE,DICT_MB_DETECT_TESTS.ENDVALIDDATE,DICT_MB_DETECT_TESTS.UNITS,DICT_MB_DETECT_TESTS.RESTYPE,DICT_MB_DETECT_TESTS.CUSRESULTID,DICT_MB_DETECT_TESTS.LOGUSERID,DICT_MB_DETECT_TESTS.LOGDATE,DICT_MB_DETECT_TESTS.LISTESTCODE,DICT_MB_DETECT_TESTS.GROUPTYPE,DICT_MB_DETECT_TESTS.COMBTESTFORCONSO , DICT_TEXTS.FULLTEXT as 'COMMENT' , SUBREQMB_OCOM.COMMENTTEXT as 'COMMENT_FREE'
,SUBREQMB_DET_TESTS.LOGDATE AS 'AGAR_LOGDATE'
FROM SUBREQMB_DET_TESTS 
LEFT OUTER JOIN DICT_MB_DETECT_TESTS ON (SUBREQMB_DET_TESTS.DETECTIONTESTID = DICT_MB_DETECT_TESTS.DETECTIONTESTID)
LEFT OUTER JOIN SUBREQUESTS_MB ON (SUBREQMB_DET_TESTS.SUBREQUESTID = SUBREQUESTS_MB.SUBREQUESTID)
LEFT OUTER JOIN REQUESTS ON (SUBREQUESTS_MB.REQUESTID = REQUESTS.REQUESTID)
LEFT OUTER JOIN SUBREQMB_OCOM on SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_OCOM.SUBREQUESTID
LEFT OUTER JOIN DICT_TEXTS ON SUBREQMB_OCOM.COMMENTCODEDID = DICT_TEXTS.TEXTID
WHERE REQUESTS.REQUESTID  ='" + strReqID + "'" +
"  and SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString().Trim() + "'" +
" ORDER BY  CREATIONDATE, SUBREQMBTESTID ";
                        Writedatalog.WriteLog("Step sql_result--->" + sql_res);

                        SqlCommand cmd_res = new SqlCommand(sql_res, conn);
                        SqlDataAdapter adp_res = new SqlDataAdapter(cmd_res);
                        DataSet ds_res = new DataSet();
                        if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                        ds_res.Clear();

                        adp_res.Fill(ds_res, "result");

                        if (ds_res.Tables["result"].Rows.Count > 0)
                        {
                            //////////////////////////////////////////////////////////////////////////////////////////
                            ///////////////////////////////// Comment CODE OR COMMENT FREE TEXT //////////////////////
                            //////////////////////////////////////////////////////////////////////////////////////////
                            // Check comment by request CODE Comment
                            for (int k = 0; k < ds_res.Tables["result"].Rows.Count; k++)
                            {
                                // Check comment by request
                                comment_request = ds_res.Tables["result"].Rows[k]["COMMENT"].ToString();

                                // Check comment request
                                if (!comment_request.Equals(""))
                                {
                                    break;
                                }
                            }

                            // check comment if not null
                            if (!comment_request.Equals(""))
                            {
                                xrLabel_comment2.Text = comment_request;
                            }
                            else
                            {
                                // Check comment by request Comment Freetext
                                for (int j = 0; j < ds_res.Tables["result"].Rows.Count; j++)
                                {
                                    // Check comment by request
                                    comment_request = ds_res.Tables["result"].Rows[j]["COMMENT_FREE"].ToString();

                                    // ... Switch on the string. check  date
                                    try
                                    {
                                        string comment_request_start = comment_request[0] + "" + comment_request[1];
                                        int number = Int32.Parse(comment_request_start);
                                        comment_request = "";
                                    }
                                    catch (Exception)
                                    {
                                        // Check comment request
                                        if (!comment_request.Equals("")) break;
                                    }
                                }

                                // Check comment by request
                                if (!comment_request.Equals(""))
                                {
                                    xrLabel_comment2.Text = comment_request;
                                }
                            }
                            //////////////////////////////////////////// END Comment CODE OR COMMENT FREE TEXT  //////////////////////////

                            lblResult.Text = ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString();
                            xrLabel3_AFBFUNGUS.Text = ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString();
                            lblResult.Visible = false;
                            xrLabel3_AFBFUNGUS.Visible = true;
                            xrLabel3_AFBFUNGUS.LocationF = new PointF(800, 200);
                            //lblResult.LocationF = new PointF(297, 500);
                            strOBX += "MB" + ds_res.Tables["result"].Rows[0]["SUBREQMBAGARID"].ToString() + "^" + ds_res.Tables["result"].Rows[0]["FULLTEXT"].ToString() + "||" + ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString() + "\r\n";
                            //strOBX += "MB" + ds_res.Tables["result"].Rows[0]["SUBREQMBAGARID"].ToString() + "^" + ds_res.Tables["result"].Rows[0]["DETECTIONTESTCODE"].ToString() + "||" + ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString() + "\r\n";
                            obxCount++;
                            strSubReqID = ds_res.Tables["result"].Rows[0]["SUBREQUESTID"].ToString();


                            // NEW Design Time to positive
                            // check Have Time to positive Tab Media
                            if (ds_res.Tables["result"].Rows.Count > 1)
                            {
                                // Time to positive Tab Media
                                if ((ds_res.Tables["result"].Rows[1]["DETECTIONTESTCODE"].ToString()).Equals("TIMEPOS") && !(ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString()).Equals("No growth"))
                                {
                                    xrLabel3.Visible = true;
                                    xrLabel5.Visible = true;
                                    xrTableCell_ResTimePOS.Text = "";
                                    xrTable_TimePOS.Visible = true;
                                    xrTableCell_ResTimePOS.Text = ds_res.Tables["result"].Rows[1]["TESTRESULT"].ToString();
                                    strLogdate = ds_res.Tables["result"].Rows[1]["AGAR_LOGDATE"].ToString();
                                    lblResult.Visible = true;
                                    xrLabel3_AFBFUNGUS.Visible = false;
                                    xrLabel3_logdate.Visible = true;
                                    xrLabel3_logdate.Text = "";
                                    xrLabel3_logdate.Text = string.Format("{0:dd/MM/yyyy HH:mm}", Convert.ToDateTime(strLogdate.ToString()));
                                    xrLabel3_logdate.Text = strLogdate;

                                    //GroupFooter1.Visible = true;
                                }
                                else
                                {
                                    xrTable_TimePOS.Rows.Clear();
                                }
                            }
                            else
                            {
                                xrTable_TimePOS.Rows.Clear();
                            }
                            // End New Design time to positive

                        }
                        else
                        {
                            //////lblResult.Text = ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString();
                            //////xrLabel3_AFBFUNGUS.Text = ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString();
                            //////strOBX += "MB" + ds_res.Tables["result"].Rows[0]["SUBREQMBAGARID"].ToString() + "^" + ds_res.Tables["result"].Rows[0]["FULLTEXT"].ToString() + "||" + ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString() + "\r\n";
                            ////////strOBX += "MB" + ds_res.Tables["result"].Rows[0]["SUBREQMBAGARID"].ToString() + "^" + ds_res.Tables["result"].Rows[0]["DETECTIONTESTCODE"].ToString() + "||" + ds_res.Tables["result"].Rows[0]["TESTRESULT"].ToString() + "\r\n";
                            //////obxCount++;
                            //////strSubReqID = ds_res.Tables["result"].Rows[0]["SUBREQUESTID"].ToString();


                            //NEW Design result batch
                            Writedatalog.WriteLog(DateTime.Now.ToString() + "!!!!!!!!!!====> Not result > 0 row start Query Special Header 3 record ");
                            Writedatalog.WriteLog("Found SubrequestID--->" + strSubReqID);
                            if (strReqID != "" && strSubReqID == "")
                            {
                                string sql_special_subreqid = @"SELECT REQUESTS.REQUESTID,SUBREQMB_AGARS.AGARNUMBER,SUBREQMB_AGARS.SUBREQUESTID
FROM SUBREQUESTS_MB
LEFT OUTER JOIN REQUESTS ON SUBREQUESTS_MB.REQUESTID = REQUESTS.REQUESTID
LEFT OUTER JOIN SUBREQMB_AGARS ON SUBREQMB_AGARS.SUBREQUESTID = SUBREQUESTS_MB.SUBREQUESTID
WHERE REQUESTS.REQUESTID  ='" + strReqID + "'  and SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString().Trim() + "' ";
                                Writedatalog.WriteLog("Step Quety special data Subrequest ID in DB---->" + sql_special_subreqid);
                                SqlCommand cmd_subreqid = new SqlCommand(sql_special_subreqid, conn);
                                SqlDataAdapter adp_subreqid = new SqlDataAdapter(cmd_subreqid);
                                DataSet ds_subreqid = new DataSet();
                                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                                ds_subreqid.Clear();
                                adp_subreqid.Fill(ds_subreqid, "result_subreqid");
                                string strSubReqID_1;
                                if (ds_subreqid.Tables["result_subreqid"].Rows.Count > 0)
                                {
                                    for (int im = 0; im < ds_subreqid.Tables["result_subreqid"].Rows.Count; im++)
                                    {
                                        strSubReqID_1 = ds_subreqid.Tables["result_subreqid"].Rows[im]["SUBREQUESTID"].ToString();
                                        strSubReqID = strSubReqID_1;
                                    }
                                    Writedatalog.WriteLog("Step Get Subrequest ID --> == " + strSubReqID);
                                }
                                if (strSubReqID != "")
                                {
                                    // Not thing
                                }
                                else
                                {
                                    serch_batchInfo();
                                    strOBX = "^||" + "\r\n";

                                }
                                queryQutity_specialHeader3();
                            }
                        }


                        //else

                        // End NEW Design
                        searchDoctorInfo();
                        searchRequestInfo();
                        searchLocationInfo();
                        searchProtocolInfo();
                        searchUsersValInfo();
                        searchSentsitivity();

                        /*==============================
                          * SEND RESULTS TO HIS (RAW DATA)
                          * =============================*/
                        //if (CheckStatus.SendTOHIS == "YES")
                        //{
                        //    string strOBXHL7 = strOBX;
                        //    //removeFirstForEdit(accessnumber, potocalnumber);
                        //    System.Threading.Thread.Sleep(5000);//will wait for 5 seconds and generate final message.
                        //    ExportFileToHL7.SendToHIS_HemocultureP12(strOBXHL7, accessnumber, potocalnumber, pathhtml, strSubReqID, lblGroupTest.Text, testcode);
                        //}
                    }
                }

                // Check Clinical not Technical
                string tech_report = xrTableCell_ReportBy.Text;
                string clinical_report = xrTableCell_Approve.Text;
                if (tech_report.Equals("") && !clinical_report.Equals(""))
                {
                    // set technical
                    xrTableCell_ReportBy.Text = clinical_report;
                }

                // Check prelim report
                if (lblValidateType.Text.Equals("PRELIMINARY REPORTED: "))
                {
                    //xrTableCell_CompleteDateTime.Text = "-";
                    //lblResult.LocationF = new PointF(200, 200);
                    lblResult.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                }

                // set datetime create document
                // Change to Technical Date time ACTIONMARK

                //xrLabel_DateTime.Text = DateTime.Now.ToString(format_DT, cultureinfo);
            }
            catch (Exception ex)
            {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method Template_Hemoculture_p12_BeforePrint() : " + ex.Message.ToString());
            }

            Console.WriteLine("LBL result : " + lblResult.Text);
        }

        private void removeFirstForEdit(DevExpress.XtraReports.Parameters.Parameter accessnumber, DevExpress.XtraReports.Parameters.Parameter potocalnumber)
        {
            try
            {
                string mshIDCOunt = "";
                DataSet dsXmlFile;
                string hl7OruPath = "";
                string pathOfHTMLMsg = "";
                try
                {
                    //mshIDCOunt = Settings1.Default.mshid;
                    dsXmlFile = new DataSet();
                    dsXmlFile.ReadXml("setting_micro.xml", XmlReadMode.Auto);
                    hl7OruPath = dsXmlFile.Tables[0].Rows[0]["PathOfHL7"].ToString();
                    pathOfHTMLMsg = dsXmlFile.Tables[0].Rows[0]["PathOfHTMInMSG"].ToString();
                }
                catch (Exception)
                {

                }
                //END

                string strSysDate = DateTime.Now.ToString("yyyyMMddHHmmss");

                SqlConnection connM = new SqlConnection();
                //connM = new Connection_DB().ConnectMonitoring();

                string sql = "SELECT M_TMP  FROM TBCNX_MONITORING WHERE M_ACCESSNUMBER = '" + accessnumber.Value + "' AND M_SUBREQUESTNUMBER = '" + potocalnumber.Value + "' AND M_TYPE = 'ORU' ORDER BY CNXID DESC";
                SqlCommand cmd = new SqlCommand(sql, connM);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (connM.State == ConnectionState.Open) connM.Close(); connM.Open();
                cmd.ExecuteNonQuery();
                adp.Fill(ds, "result");
                if (ds.Tables["result"].Rows.Count > 0)
                {
                    string newMsgHL7 = ds.Tables["result"].Rows[0][0].ToString();
                    //|||F|||
                    newMsgHL7 = newMsgHL7.Replace("|||F|||", "|||D|||");
                    System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + hl7OruPath + "" + potocalnumber.Value.ToString() + "_" + accessnumber.Value.ToString() + "_" + strSysDate + "_d.hl7", true, Encoding.GetEncoding("TIS-620"));

                    file.Write(newMsgHL7);
                    file.Close();

                    int count = Convert.ToInt32(mshIDCOunt) + 1;
                    string value = string.Format("{0:D5}", count);
                    //Settings1.Default.mshid = value;
                    //Settings1.Default.Save();

                }
                else
                {

                }
            }catch(Exception ex)
            {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method removeFirst() : " + ex.Message.ToString()); 
            }
        }

        private void searchStainForPositive()
        {
            try
            {
                GroupFooter4.Visible = true;
                string strQutity = "";
                string strOrganism = "";
                string sql2 = @"SELECT RIGHT(PATIENTS.PATNUMBER,9)AS 'HN', PATIENTS.NAME ,PATIENTS.FIRSTNAME as 'LASTNAME',CASE (PATIENTS.SEX) WHEN 1 THEN 'M' WHEN 2 THEN 'F' ELSE '-' END AS 'SEX',ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as ageY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as ageM, SUBREQMB_STAINS.SUBREQMBSTAINID,SUBREQMB_STAINS.COLONYID,SUBREQMB_STAINS.SUBREQUESTID,SUBREQMB_STAINS.MBSTAINID,SUBREQMB_STAINS.VALREQUESTED,SUBREQMB_STAINS.CONSOLIDATIONSTATUS,SUBREQMB_STAINS.CREATEUSER,SUBREQMB_STAINS.CREATIONDATE,SUBREQMB_STAINS.LOGUSERID,SUBREQMB_STAINS.LOGDATE,SUBREQMB_STAINS.NOTPRINTABLE,DICT_MB_STAINS.MBSTAINID,DICT_MB_STAINS.MBSTAINCODE,DICT_MB_STAINS.MBSTAINCREDATE,DICT_MB_STAINS.SHORTTEXT,DICT_MB_STAINS.FULLTEXT,DICT_MB_STAINS.DICTCONSOSTATUS,DICT_MB_STAINS.LISTESTCODE,DICT_MB_STAINS.FIRSTITEMID,DICT_MB_STAINS.STARTVALIDDATE,DICT_MB_STAINS.ENDVALIDDATE,DICT_MB_STAINS.NOTPRINTABLE, MB_STAINS_OBSERVATIONS.SUBREQMBSTAINID,MB_STAINS_OBSERVATIONS.MORPHOBSERVATION,MB_STAINS_OBSERVATIONS.CREATEUSER,MB_STAINS_OBSERVATIONS.CREATIONDATE,MB_STAINS_OBSERVATIONS.QUANTITATIVERESULT,MB_STAINS_OBSERVATIONS.COMMENTS,MB_STAINS_OBSERVATIONS.RESUPDDATE,MB_STAINS_OBSERVATIONS.LOGUSERID,MB_STAINS_OBSERVATIONS.LOGDATE,MB_STAINS_OBSERVATIONS.MORPHOBSERVATIONID
,REQUESTS.RECEIVEDDATE AS 'RECEIVEDDATE', REQUESTS.REQDATE AS 'REQDATE',  CONVERT(varchar,REQUESTS.COLLECTIONDATE,120) AS 'COLLECTIONDATE' ,
DICT_COLL_MATERIALS.COLLMATERIALCODE,DICT_COLL_MATERIALS.FULLTEXT AS 'MATERIALSTEXT',DICT_MB_PROTOCOLS.FULLTEXT AS 'PROTOCOLTEXT',       DICT_COLL_MATERIALS.FULLTEXT AS 'MATERIALSTEXT',SUBREQUESTS_MB.SUBREQUESTNUMBER,REQUESTS.ACCESSNUMBER
,USERS.USERNAME,SUBREQMB_ACTIONS.ACTIONMARKDATE AS 'VALIDATED',DICT_LOCATIONS.LOCNAME ,   DICT_TEXTS.FULLTEXT as 'COMMENT' , SUBREQMB_OCOM.COMMENTTEXT as 'COMMENT_FREE'
                            FROM SUBREQMB_STAINS LEFT OUTER JOIN DICT_MB_STAINS ON (SUBREQMB_STAINS.MBSTAINID = DICT_MB_STAINS.MBSTAINID ) 
                            LEFT OUTER JOIN MB_STAINS_OBSERVATIONS ON (SUBREQMB_STAINS.SUBREQMBSTAINID = MB_STAINS_OBSERVATIONS.SUBREQMBSTAINID )
                            LEFT OUTER JOIN SUBREQUESTS_MB ON ( SUBREQMB_STAINS.SUBREQUESTID  = SUBREQUESTS_MB.SUBREQUESTID)
                            LEFT OUTER JOIN REQUESTS ON ( SUBREQUESTS_MB.REQUESTID = REQUESTS.REQUESTID )
                            LEFT OUTER JOIN PATIENTS ON ( REQUESTS.PATID = PATIENTS.PATID)
							 LEFT OUTER JOIN DICT_COLL_MATERIALS ON SUBREQUESTS_MB.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID
                             LEFT OUTER JOIN DICT_COLL_SOURCES ON SUBREQUESTS_MB.COLLSOURCEID = DICT_COLL_SOURCES.COLLSOURCEID
							 LEFT OUTER JOIN DICT_MB_PROTOCOLS ON SUBREQUESTS_MB.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
							 LEFT OUTER JOIN SUBREQMB_ACTIONS ON SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ACTIONS.SUBREQUESTID
							 LEFT OUTER JOIN USERS ON USERS.USERID = SUBREQMB_ACTIONS.ACTIONMARKUSERID      
                  	 LEFT OUTER JOIN LOCATIONS ON REQUESTS.REQUESTID = LOCATIONS.REQUESTID
							 LEFT OUTER JOIN DICT_LOCATIONS ON LOCATIONS.LOCID = DICT_LOCATIONS.LOCID
 LEFT OUTER JOIN SUBREQMB_OCOM on SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_OCOM.SUBREQUESTID
 LEFT OUTER JOIN DICT_TEXTS ON SUBREQMB_OCOM.COMMENTCODEDID = DICT_TEXTS.TEXTID
 WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "'" +
         "   AND SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString() + "'" +
          "     AND SUBREQMB_ACTIONS.ACTIONMARKTYPE =" + status.Value.ToString() +
           "    ORDER BY  MB_STAINS_OBSERVATIONS.MORPHOBSERVATIONID DESC      ";

                Writedatalog.WriteLog("Step SearchStainForPositive --->" + sql2);

                SqlCommand cmd2 = new SqlCommand(sql2, conn);
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                DataSet ds2 = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds2.Clear();

                adp2.Fill(ds2, "result");

                if (ds2.Tables["result"].Rows.Count > 0)
                {
                    GroupFooter1.Visible = true;

                    //////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////// Comment CODE OR COMMENT FREE TEXT //////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////
                    // Check comment by request CODE Comment
                    for (int k = 0; k < ds2.Tables["result"].Rows.Count; k++)
                    {
                        // Check comment by request
                        comment_request = ds2.Tables["result"].Rows[k]["COMMENT"].ToString();

                        // Check comment request
                        if (!comment_request.Equals(""))
                        {
                            break;
                        }
                    }

                    // check comment if not null
                    if (!comment_request.Equals(""))
                    {
                        xrLabel_comment2.Text = comment_request;
                    }
                    else
                    {
                        // Check comment by request Comment Freetext
                        for (int j = 0; j < ds2.Tables["result"].Rows.Count; j++)
                        {
                            // Check comment by request
                            comment_request = ds2.Tables["result"].Rows[j]["COMMENT_FREE"].ToString();

                            // ... Switch on the string. check  date
                            try
                            {
                                string comment_request_start = comment_request[0] + "" + comment_request[1];
                                int number = Int32.Parse(comment_request_start);
                                comment_request = "";
                            }
                            catch (Exception)
                            {
                                // Check comment request
                                if (!comment_request.Equals("")) break;
                            }
                        }

                        // Check comment by request
                        if (!comment_request.Equals(""))
                        {
                            xrLabel_comment2.Text = comment_request;
                        }
                    }
                    //////////////////////////////////////////// END Comment CODE OR COMMENT FREE TEXT  //////////////////////////


                    //      xrCultureCommnet.Visible = false;
                    for (int i = 0; i < ds2.Tables["result"].Rows.Count; i++)
                    {
                        if (ds2.Tables["result"].Rows[i]["MORPHOBSERVATION"].ToString() != "XCANCR".Trim() && ds2.Tables["result"].Rows[i]["MORPHOBSERVATION"].ToString() != "")
                        {
                            if (ds2.Tables["result"].Rows[i]["QUANTITATIVERESULT"].ToString() != "XCANCR".Trim())
                            {
                                strQutity += ds2.Tables["result"].Rows[i]["QUANTITATIVERESULT"].ToString() + "\r\n";

                            }

                            strOrganism += ds2.Tables["result"].Rows[i]["QUANTITATIVERESULT"].ToString() + "         " + ds2.Tables["result"].Rows[i]["MORPHOBSERVATION"].ToString() + " \r\n ";
                            // New Design remove  last line ---> +" ("+xrTableCell_CompleteDateTime.Text+ ")\r\n"
                            try
                            {
                                string strDataRes = "";
                                if (ds2.Tables["result"].Rows[i]["MORPHOBSERVATION"].ToString() != "")
                                {
                                    strDataRes = ds2.Tables["result"].Rows[i]["MORPHOBSERVATION"].ToString();
                                }
                                else
                                {
                                    strDataRes = "-";
                                }

                                strOBX += "MB" + ds2.Tables["result"].Rows[i]["SUBREQMBSTAINID"].ToString() + "^" + strDataRes + "||*\r\n";
                            }
                            catch (Exception) { }
                        }
                    }

                }
                else
                {
                    GroupFooter1.Visible = false;
                }

                xrTableCell2.Text = "";

                xrTableCell2_gram.Text = strOrganism;
                xrTableCell2.LocationF = new PointF(400, 500);
            }
            catch (Exception ex)
            {

                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method searchStainForPositive() : " + ex.Message.ToString()); 
            }
        }

        private void searchSentsitivity()
        {
            try
            {
                string sql = @"SELECT SUBREQMB_ORGANISMS.ORGANISMINDEX, PATIENTS.PATNUMBER 'HN', PATIENTS.NAME,PATIENTS.SEX,
       PATIENTS.FIRSTNAME as 'LASTNAME', REQUESTS.REQUESTID,REQUESTS.ACCESSNUMBER,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as ageY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as ageM,
       REQUESTS.PATID, SUBREQUESTS_MB.SUBREQUESTID, CONVERT (varchar,
       SUBREQUESTS_MB.COLLECTIONDATE, 120) AS 'COLLECTIONDATE',
       SUBREQUESTS_MB.COLLMATERIALID,
       SUBREQMB_ORGANISMS.SUBREQMBORGID,
       SUBREQMB_ORGANISMS.ORGANISMID, SUBREQMB_ORGANISMS.IDENTDATE,
       SUBREQMB_ANTIBIOTICS.SUBREQMBANTIBIOTICID,
       SUBREQMB_ANTIBIOTICS.ANTIBIOTICID,
       DICT_MB_ORGANISMS.ORGANISMNAME, DICT_MB_ORGANISMS.MORPHODESC,DICT_MB_ORGANISMS.FULLTEXT
       as 'ORGANISMSNAME', DICT_MB_ANTIBIOTICS.ANTIBIOTICCODE,
       DICT_MB_ANTIBIOTICS.FULLTEXT as 'ANTIBIOTICSNAME',
     CASE (   SUBREQMB_ANTIBIOTICS.UNITS ) WHEN '1' THEN 'mg/L' WHEN '2' THEN 'mm' ELSE '' END AS 'UNITS'
, REQUESTS.RECEIVEDDATE AS 'RECEIVEDDATE',
       REQUESTS.REQDATE, SUBREQMB_ACTIONS.ACTIONMARKUSERID,
       USERS.USERNAME, RIGHT (SUBREQMB_COLONIES.COLONYNUMBER,
       3) AS 'COLONYNUMBER', CASE ( SUBREQMB_ANTIBIOTICS.RESULT) WHEN '1' THEN 'S' WHEN 2 THEN 'I' WHEN 3 THEN 'R' WHEN 8 THEN 'SDD' WHEN 7 THEN 'NS'   END AS 'RESULT' , SUBREQMB_ANTIBIOTICS.RESULT as 'res',
       SUBREQMB_ANTIBIOTICS.RESULTVALUE,
             '('+  SUBREQMB_ANTIBIOTICS.MINIMUM +' - '+ SUBREQMB_ANTIBIOTICS.MAXIMUM +')' as 'breakpoints',
       CASE (SUBREQMB_ACTIONS.ACTIONMARKTYPE) WHEN 8 THEN SUBREQMB_ACTIONS.ACTIONMARKDATE WHEN 23 THEN SUBREQMB_ACTIONS.ACTIONMARKDATE END AS 'REQSTATUS',
       SUBREQUESTS_MB.SUBREQUESTNUMBER,
       DICT_COLL_MATERIALS.COLLMATERIALCODE,
       DICT_COLL_MATERIALS.FULLTEXT AS 'MATERIALSTEXT',
       DICT_MB_PROTOCOLS.PROTOCOLCODE,
       DICT_MB_PROTOCOLS.FULLTEXT AS 'PROTOCALTEXT',
       SUBREQMB_DET_TESTS.TESTRESULT, SUBREQMB_ANTIBIOTICS.UNITS,SUBREQMB_ACTIONS.ACTIONMARKDATE AS 'VALIDATED',DICT_LOCATIONS.LOCNAME,DICT_LOCATIONS.ADDRESS1,DICT_LOCATIONS.ADDRESS2 ,DICT_TEXTS.FULLTEXT as 'COMMENT',
SUBREQMB_COLONIES.COLONYINDEX,DICT_DOCTORS.DOCCODE,DICT_DOCTORS.DOCNAME,DICT_DOCTORS.ADDRESS1 AS 'DOCNAME2' , DICT_TEXTS.FULLTEXT as 'COMMENT' , SUBREQMB_OCOM.COMMENTTEXT as 'COMMENT_FREE' , DICT_MB_ANTIBIO_FAMS.ANTIBIOTICSFAMILYID , DICT_MB_ANTIBIO_FAMS.FULLTEXT as 'ANTIBIOTICS_FAM'
FROM REQUESTS LEFT OUTER JOIN SUBREQUESTS_MB ON (REQUESTS.REQUESTID = SUBREQUESTS_MB.REQUESTID) 
LEFT OUTER JOIN SUBREQMB_ORGANISMS ON (SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ORGANISMS.SUBREQUESTID)
LEFT OUTER JOIN DICT_MB_ORGANISMS ON (SUBREQMB_ORGANISMS.ORGANISMID = DICT_MB_ORGANISMS.ORGANISMID)
LEFT OUTER JOIN SUBREQMB_ANTIBIOTICS ON (SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_ANTIBIOTICS.SUBREQMBORGID) 
LEFT OUTER JOIN DICT_MB_ANTIBIOTICS ON (SUBREQMB_ANTIBIOTICS.ANTIBIOTICID = DICT_MB_ANTIBIOTICS.ANTIBIOTICID) 
LEFT OUTER JOIN DICT_MB_ANTIBIO_FAMS ON (DICT_MB_ANTIBIOTICS.ANTIBIOTICSFAMILYID = DICT_MB_ANTIBIO_FAMS.ANTIBIOTICSFAMILYID) 
LEFT OUTER JOIN SUBREQMB_ACTIONS ON SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ACTIONS.SUBREQUESTID
INNER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID 
LEFT OUTER JOIN DICT_MB_PROTOCOLS ON SUBREQUESTS_MB.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
LEFT OUTER JOIN DICT_COLL_MATERIALS ON SUBREQUESTS_MB.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID
LEFT OUTER JOIN DICT_COLL_SOURCES ON SUBREQUESTS_MB.COLLSOURCEID = DICT_COLL_SOURCES.COLLSOURCEID
LEFT OUTER JOIN USERS ON users.USERID = SUBREQMB_ACTIONS.ACTIONMARKUSERID
LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_COLONIES.SUBREQMBORGID 
LEFT OUTER JOIN SUBREQMB_DET_TESTS ON SUBREQMB_COLONIES.COLONYID = SUBREQMB_DET_TESTS.COLONYID
LEFT OUTER JOIN LOCATIONS ON REQUESTS.REQUESTID = LOCATIONS.REQUESTID
LEFT OUTER JOIN DICT_LOCATIONS ON LOCATIONS.LOCID = DICT_LOCATIONS.LOCID
LEFT OUTER JOIN SUBREQMB_OCOM on SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_OCOM.SUBREQUESTID
LEFT OUTER JOIN DICT_TEXTS ON SUBREQMB_OCOM.COMMENTCODEDID = DICT_TEXTS.TEXTID
LEFT OUTER JOIN DOCTORS ON REQUESTS.REQUESTID  = DOCTORS.REQUESTID
LEFT OUTER JOIN DICT_DOCTORS ON DOCTORS.DOCID = DICT_DOCTORS.DOCID
WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "'" +
   "  AND SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString() + "'" +
  @"  AND SUBREQMB_ACTIONS.ACTIONMARKTYPE = " + status.Value.ToString() +
   "  AND SUBREQMB_ANTIBIOTICS.RESULT is not null" +
   "  AND SUBREQMB_ANTIBIOTICS.RESULT != 0 AND  (SUBREQMB_ANTIBIOTICS.NOTPRINTABLE = 0)" +
   "  -- SUBREQMB_ANTIBIOTICS.RESULT != 0 is Resulult don't have S/I/R" +
   " order by SUBREQMB_ORGANISMS.ORGANISMINDEX,SUBREQMB_COLONIES.COLONYINDEX ,DICT_MB_ANTIBIO_FAMS.ANTIBIOTICSFAMILYID,DICT_MB_ANTIBIO_FAMS.FULLTEXT";

                // ***************----> NEW Design
                // 1 = To be Not print
                // 0 = To be Print
                // NEW Design <<< Add AND  (SUBREQMB_ANTIBIOTICS.NOTPRINTABLE = 0) after AND SUBREQMB_ANTIBIOTICS.RESULT != 0
                Writedatalog.WriteLog("Step SearchSentsitivity---> " + sql);

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds.Clear();
                adp.Fill(ds, "result");

                xrPivotGrid1.DataSource = ds;
                xrPivotGrid1.DataMember = "result";

                string strOrgamnism = "";
                string strCheckOrgan = "";
                if (ds.Tables["result"].Rows.Count > 0)
                {
                    GroupHeader3.Visible = false;
                    GroupHeader4.Visible = true;
                    GroupHeader5.Visible = true;
                    Detail.Visible = true;

                    //////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////// Comment CODE OR COMMENT FREE TEXT //////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////
                    // Check comment by request CODE Comment
                    for (int k = 0; k < ds.Tables["result"].Rows.Count; k++)
                    {
                        // Check comment by request
                        comment_request = ds.Tables["result"].Rows[k]["COMMENT"].ToString();

                        // Check comment request
                        if (!comment_request.Equals(""))
                        {
                            break;
                        }
                    }

                    // check comment if not null
                    if (!comment_request.Equals(""))
                    {
                        xrLabel_comment2.Text = comment_request;
                    }
                    else
                    {
                        // Check comment by request Comment Freetext
                        for (int j = 0; j < ds.Tables["result"].Rows.Count; j++)
                        {
                            // Check comment by request
                            comment_request = ds.Tables["result"].Rows[j]["COMMENT_FREE"].ToString();

                            // ... Switch on the string. check  date
                            try
                            {
                                string comment_request_start = comment_request[0] + "" + comment_request[1];
                                int number = Int32.Parse(comment_request_start);
                                comment_request = "";
                            }
                            catch (Exception)
                            {
                                // Check comment request
                                if (!comment_request.Equals("")) break;
                            }
                        }

                        // Check comment by request
                        if (!comment_request.Equals(""))
                        {
                            xrLabel_comment2.Text = comment_request;
                        }
                    }
                    //////////////////////////////////////////// END Comment CODE OR COMMENT FREE TEXT  //////////////////////////

                    string res_units = "";
                    // Check MID  Sensitivities
                    string res = "";
                    string res_val = "";

                    for (int i = 0; i < ds.Tables["result"].Rows.Count; i++)
                    {
                        res = ds.Tables["result"].Rows[i]["RESULT"].ToString();
                        res_val = ds.Tables["result"].Rows[i]["RESULTVALUE"].ToString();
                        res_units = ds.Tables["result"].Rows[i]["UNITS"].ToString();

                        if (!res.Equals(""))
                        {
                            if (res_units.Equals("mm"))
                            {
                                ds.Tables["result"].Rows[i]["RESULTVALUE"] = "";
                                ds.Tables["result"].Rows[i]["UNITS"] = "";
                            }
                        }
                    } // End Check MID Sensitivities 

                    xrPivotGridField1.FieldName = "ANTIBIOTICSNAME";
                    pivotGridField3.FieldName = "ORGANISMINDEX";
                    xrPivotGridField4.FieldName = "RESULT";
                    pivotGridField2.FieldName = "ANTIBIOTICSFAMILYID";
                    pivotGridField4.FieldName = "ANTIBIOTICS_FAM";
                    pivotGridField1.FieldName = "RESULTVALUE";
                    pivotGridField5.FieldName = "UNITS";

                    // Clear
                    xrTable_Qutity.Rows.Clear();
                    xrTable_Qutity.Visible = true;

                    for (int i = 0; i < ds.Tables["result"].Rows.Count; i++)
                    {
                        if (strCheckOrgan != ds.Tables["result"].Rows[i]["ORGANISMNAME"].ToString())
                        {
                            strOrgamnism += "(" + ds.Tables["result"].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMNAME"].ToString() + "\r\n";

                            string strOrgamnism_table = "";
                            if (ds.Tables["result"].Rows[i]["MORPHODESC"].ToString() != "")
                            {
                                strOrgamnism_table = "<n>(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ")</n> " + ds.Tables["result"].Rows[i]["MORPHODESC"].ToString();
                            }
                            else
                            {
                                strOrgamnism_table = "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString();
                            }

                            Process_StyleText(xrTable_Qutity, strOrgamnism_table, ds.Tables["result"].Rows[i]["ORGANISMINDEX"].ToString());
                        }
                        strOBX += "MB" + ds.Tables["result"].Rows[i]["SUBREQMBORGID"].ToString() + ds.Tables["result"].Rows[i]["ANTIBIOTICCODE"].ToString() + "^" + ds.Tables["result"].Rows[i]["ANTIBIOTICSNAME"].ToString() + "||" + ds.Tables["result"].Rows[i]["ORGANISMNAME"].ToString() + "=" + ds.Tables["result"].Rows[i]["RESULT"].ToString() + "  Value=" + ds.Tables["result"].Rows[i]["RESULTVALUE"].ToString() + "\r\n";
                        strCheckOrgan = ds.Tables["result"].Rows[i]["ORGANISMNAME"].ToString();
                    }

                    queryQutity_Organ();    // Detection test & Resistant

                }
                else
                {
                    //  Don't have sensitivity.

                    searchStainForPositive();
                    queryQutity();  // Detection test & Resistant

                }
            }catch(Exception ex){
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method searchSentsitivity() : " + ex.Message.ToString()); 
            }
        }

        private void queryQutity()
        {
            try
            {
                
                string sql = @"SELECT DISTINCT REQUESTS.ACCESSNUMBER,DICT_MB_DETECT_TESTS.DETECTIONTESTID,DETECTIONTESTCODE,DETECTIONTESTCREDATE,DICT_MB_DETECT_TESTS.FULLTEXT,DICT_MB_DETECT_TESTS.DICTCONSOSTATUS,DICT_MB_DETECT_TESTS.NOTPRINTABLE,DICT_MB_DETECT_TESTS.FIRSTITEMID
,DICT_MB_DETECT_TESTS.STARTVALIDDATE,DICT_MB_DETECT_TESTS.ENDVALIDDATE,DICT_MB_DETECT_TESTS.UNITS,RESTYPE,CUSRESULTID,DICT_MB_DETECT_TESTS.LOGUSERID,DICT_MB_DETECT_TESTS.LOGDATE,DICT_MB_DETECT_TESTS.LISTESTCODE,GROUPTYPE,COMBTESTFORCONSO
,SUBREQMB_DET_TESTS.TESTRESULT,  DICT_MB_ORGANISMS.ORGANISMCODE,DICT_MB_ORGANISMS.FULLTEXT AS 'ORGANISMSNAME',DICT_MB_ORGANISMS.MORPHODESC,DICT_TOPOGRAPHIES.FULLTEXT AS 'TOPOGRAPHIES',SUBREQMB_COLONIES.COLONYINDEX,SUBREQMB_ORGANISMS.ORGANISMINDEX , SUBREQMB_ORGANISMS.COMMENTS
FROM REQUESTS LEFT OUTER JOIN SUBREQUESTS_MB ON (REQUESTS.REQUESTID = SUBREQUESTS_MB.REQUESTID) 
LEFT OUTER JOIN SUBREQMB_ORGANISMS ON (SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ORGANISMS.SUBREQUESTID)
LEFT OUTER JOIN DICT_MB_ORGANISMS ON (SUBREQMB_ORGANISMS.ORGANISMID = DICT_MB_ORGANISMS.ORGANISMID)
LEFT OUTER JOIN SUBREQMB_ANTIBIOTICS ON (SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_ANTIBIOTICS.SUBREQMBORGID) 
LEFT OUTER JOIN DICT_MB_ANTIBIOTICS ON (SUBREQMB_ANTIBIOTICS.ANTIBIOTICID = DICT_MB_ANTIBIOTICS.ANTIBIOTICID) 
LEFT OUTER JOIN SUBREQMB_ACTIONS ON SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ACTIONS.SUBREQUESTID
LEFT OUTER JOIN DICT_MB_PROTOCOLS ON SUBREQUESTS_MB.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_COLONIES.SUBREQMBORGID 
LEFT OUTER JOIN SUBREQMB_DET_TESTS ON SUBREQMB_COLONIES.COLONYID = SUBREQMB_DET_TESTS.COLONYID
LEFT OUTER JOIN DICT_MB_DETECT_TESTS ON SUBREQMB_DET_TESTS.DETECTIONTESTID =  DICT_MB_DETECT_TESTS.DETECTIONTESTID
LEFT OUTER JOIN DICT_TOPOGRAPHIES ON SUBREQUESTS_MB.TOPOGRAPHYID = DICT_TOPOGRAPHIES.TOPOGRAPHYID
WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "'" +
          "   AND SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString() + "'" +
          @" -- and DETECTIONTESTCODE = 'XORC'
--  or DETECTIONTESTCODE = 'XOCU' 
  -- (DO NOT UNMARK because a problem of final report Potocol P04 ) AND DICT_MB_DETECT_TESTS.NOTPRINTABLE = 0       
           order by  SUBREQMB_ORGANISMS.ORGANISMINDEX";

                Writedatalog.WriteLog("Step queryQutity--->  " + sql); 

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

                    // Check organisums if not null not show stain
                    for(int ii=0;ii<ds.Tables["result"].Rows.Count;ii++)
                    {

                        if (!ds.Tables["result"].Rows[ii]["ORGANISMSNAME"].ToString().Equals(""))
                        {
                            GroupFooter1.Visible = false;
                            break;
                        }
                    }

                    int row_ar = ds.Tables["result"].Rows.Count;
                    int column_ar = 2;
                    string[,] ar_2d = new string[row_ar, column_ar];

                    // New ArrayList
                    ArrayList arL_column1 = new ArrayList();
                    ArrayList arL_column2 = new ArrayList();


                    // After print then clean xrTAble.
                    xrTable_Qutity.Rows.Clear();
                    xrTable5.Rows.Clear();
                    //detectionTestShowAfterOrg = "";

                    // Comment by colony
                    string comment_colony = "";
                    string checkSameComment = "";

                    // Process Array2D
                    for (int i = 0; i < row_ar; i++)
                    {
                        ar_2d[i, 0] = ds.Tables["result"].Rows[i]["TESTRESULT"].ToString();

                        // get comment by colony
                        comment_colony = ds.Tables["result"].Rows[i]["COMMENTS"].ToString();

                        if (!ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString().Equals("No growth"))
                        {
                            // ar_2d[i, 1] = "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString();
                            if (ds.Tables["result"].Rows[i]["MORPHODESC"].ToString() != "")
                            {
                                ar_2d[i, 1] = "<n>(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ")</n> " + ds.Tables["result"].Rows[i]["MORPHODESC"].ToString();
                            }
                            else
                            {
                                ar_2d[i, 1] = "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString();
                            }
                        }
                        else
                        {
                            if (ds.Tables["result"].Rows[i]["MORPHODESC"].ToString() != "")
                            {
                                ar_2d[i, 1] = "<n>(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ")</n> " + ds.Tables["result"].Rows[i]["MORPHODESC"].ToString();
                            }
                            else
                            {
                                ar_2d[i, 1] = "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString();
                            }
                        }

                        checkFontOrgIndex = ds.Tables["result"].Rows[i]["ORGANISMINDEX"].ToString();

                        ////////////////// ********************* /////////////////
                        ///// Check WARNING DETECTIONCODE
                        ////////////////// ******************** //////////////////


                        if (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "ESBL" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "CRE" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "METHICILIN" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "VANCOMYCIN" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "COLISTIN" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "MLSB" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "CARBAPENEM" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "DA" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "MCR-1" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "XDR" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "MDR" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "PDR" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "VRE" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "MRSA" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "VAN" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "VISA" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "VRSA")

                        {
                        if (strOrganismChk != ar_2d[i, 1])
                            {
                                //    AddDataToXrTable(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex);
                                Process_StyleText(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                            }//END OF Check organism!!!

                            // Add CRE
                            if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "CRE") && (ds.Tables["result"].Rows[i]["TESTRESULT"].ToString().Trim() == "Resistant"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Add CABAPEMASE
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "CABAPENEM"))
                            {
                                //Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + NO_detectionTestShowAfterOrg, "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Add XDR , MDR , PDR
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "XDR") || (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "MDR") || (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "PDR"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Add VRE
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "VRE"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Add MRSA
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "MRSA"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Add VAN
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "VAN"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString(), "", detectionTestShowAfterOrg);
                               // xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Add VISA
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "VISA"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Add VRSA
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "VRSA"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "ESBL"))
                            {
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }

                            //Add ESBL in the table.
                            else if (ar_2d[i, 0] != "")
                            {
                                //   AddDataToXrTable(xrTable_Qutity, "", "  - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ar_2d[i, 0], "");
                                Process_StyleText(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ar_2d[i, 0], "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                               // xrTableCell31_drug.Text = "Drug resistance";
                            }


                        }
                        else if (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "XORC" || ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Contains("XOCU"))
                        {
                            //arL_column2.Add(ar_2d[i, 1]);
                            //  arL_column1.Add(ar_2d[i, 0]);

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
                            // AddDataToXrTable(xrTable_Qutity, result_test, "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString(), checkFontOrgIndex);
                            Process_StyleText(xrTable_Qutity, result_test, ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);

                        }
                        else
                        {
                            if (ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() != "" && ds.Tables["result"].Rows[i]["ORGANISMCODE"].ToString() != "")
                            {
                                //check code is not special !!! because special code used in H3 template.
                                if (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() != "SPECIAL" && ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() != "BARCODE".Trim())
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
                                else if (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "BARCODE".Trim())
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
                }
                else
                {
                    // NULL Sensitivities.
                }

            }
            catch (Exception ex)
            {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method queryQutity() : " + ex.Message.ToString()); 
            }
        }

        private void queryQutity_Organ()
        {
            try
            {

                string sql = @"SELECT DISTINCT REQUESTS.ACCESSNUMBER,DICT_MB_DETECT_TESTS.DETECTIONTESTID,DETECTIONTESTCODE,DETECTIONTESTCREDATE,DICT_MB_DETECT_TESTS.FULLTEXT,DICT_MB_DETECT_TESTS.DICTCONSOSTATUS,DICT_MB_DETECT_TESTS.NOTPRINTABLE,DICT_MB_DETECT_TESTS.FIRSTITEMID
,DICT_MB_DETECT_TESTS.STARTVALIDDATE,DICT_MB_DETECT_TESTS.ENDVALIDDATE,DICT_MB_DETECT_TESTS.UNITS,RESTYPE,CUSRESULTID,DICT_MB_DETECT_TESTS.LOGUSERID,DICT_MB_DETECT_TESTS.LOGDATE,DICT_MB_DETECT_TESTS.LISTESTCODE,GROUPTYPE,COMBTESTFORCONSO
,SUBREQMB_DET_TESTS.TESTRESULT,  DICT_MB_ORGANISMS.ORGANISMCODE,DICT_MB_ORGANISMS.FULLTEXT AS 'ORGANISMSNAME',DICT_MB_ORGANISMS.MORPHODESC,DICT_TOPOGRAPHIES.FULLTEXT AS 'TOPOGRAPHIES',SUBREQMB_COLONIES.COLONYINDEX,SUBREQMB_ORGANISMS.ORGANISMINDEX , SUBREQMB_ORGANISMS.COMMENTS
FROM REQUESTS LEFT OUTER JOIN SUBREQUESTS_MB ON (REQUESTS.REQUESTID = SUBREQUESTS_MB.REQUESTID) 
LEFT OUTER JOIN SUBREQMB_ORGANISMS ON (SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ORGANISMS.SUBREQUESTID)
LEFT OUTER JOIN DICT_MB_ORGANISMS ON (SUBREQMB_ORGANISMS.ORGANISMID = DICT_MB_ORGANISMS.ORGANISMID)
LEFT OUTER JOIN SUBREQMB_ANTIBIOTICS ON (SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_ANTIBIOTICS.SUBREQMBORGID) 
LEFT OUTER JOIN DICT_MB_ANTIBIOTICS ON (SUBREQMB_ANTIBIOTICS.ANTIBIOTICID = DICT_MB_ANTIBIOTICS.ANTIBIOTICID) 
LEFT OUTER JOIN SUBREQMB_ACTIONS ON SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ACTIONS.SUBREQUESTID
LEFT OUTER JOIN DICT_MB_PROTOCOLS ON SUBREQUESTS_MB.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_COLONIES.SUBREQMBORGID 
LEFT OUTER JOIN SUBREQMB_DET_TESTS ON SUBREQMB_COLONIES.COLONYID = SUBREQMB_DET_TESTS.COLONYID
LEFT OUTER JOIN DICT_MB_DETECT_TESTS ON SUBREQMB_DET_TESTS.DETECTIONTESTID =  DICT_MB_DETECT_TESTS.DETECTIONTESTID
LEFT OUTER JOIN DICT_TOPOGRAPHIES ON SUBREQUESTS_MB.TOPOGRAPHYID = DICT_TOPOGRAPHIES.TOPOGRAPHYID
						
 WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "'" +
          "   AND SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString() + "'" +
          @" -- and DETECTIONTESTCODE = 'XORC'
--  or DETECTIONTESTCODE = 'XOCU' 
  -- (DO NOT UNMARK because a problem of final report Potocol P04 ) AND DICT_MB_DETECT_TESTS.NOTPRINTABLE = 0       
           AND (SUBREQMB_ORGANISMS.NOTPRINTABLE <> 1) AND (SUBREQMB_DET_TESTS.NOTPRINTABLE = 0 OR SUBREQMB_DET_TESTS.NOTPRINTABLE IS NULL) order by  SUBREQMB_ORGANISMS.ORGANISMINDEX";

                // ***************----> NEW Design
                // 1 = To be Not print
                // 0 = To be Print
                // NEW Design <<< Add AND  (SUBREQMB_ANTIBIOTICS.NOTPRINTABLE = 0) Before order by  SUBREQMB_ORGANISMS.ORGANISMINDEX
                // NEW Design >>> Remove AND  (SUBREQMB_ANTIBIOTICS.NOTPRINTABLE = 0)
                // NEW Design >>> Add AND (SUBREQMB_ORGANISMS.NOTPRINTABLE <> 1) AND (SUBREQMB_DET_TESTS.NOTPRINTABLE = 0 OR SUBREQMB_DET_TESTS.NOTPRINTABLE IS NULL) by  SUBREQMB_ORGANISMS.ORGANISMINDEX


                Writedatalog.WriteLog("Step queryQutity_Organ--->  " + sql);

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

                    // Check organisums if not null not show stain
                    for (int ii = 0; ii < ds.Tables["result"].Rows.Count; ii++)
                    {
                        if (!ds.Tables["result"].Rows[ii]["ORGANISMSNAME"].ToString().Equals(""))
                        {
                            GroupFooter1.Visible = false;
                            break;
                        }
                    }

                    int row_ar = ds.Tables["result"].Rows.Count;
                    int column_ar = 2;
                    string[,] ar_2d = new string[row_ar, column_ar];

                    // New ArrayList
                    ArrayList arL_column1 = new ArrayList();
                    ArrayList arL_column2 = new ArrayList();


                    // After print then clean xrTAble.
                    xrTable_Qutity.Rows.Clear();
                    xrTable5.Rows.Clear();

                    // Comment by colony
                    string comment_colony = "";
                    string checkSameComment = "";

                    // Process Array2D
                    for (int i = 0; i < row_ar; i++)
                    {
                        ar_2d[i, 0] = ds.Tables["result"].Rows[i]["TESTRESULT"].ToString();
                        // get comment by colony
                        comment_colony = ds.Tables["result"].Rows[i]["COMMENTS"].ToString();
                        if (!ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString().Equals("No growth"))
                        {
                            // ar_2d[i, 1] = "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString();
                            if (ds.Tables["result"].Rows[i]["MORPHODESC"].ToString() != "")
                            {
                                ar_2d[i, 1] = "<n>(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ")</n> " + ds.Tables["result"].Rows[i]["MORPHODESC"].ToString();
                            }
                            else
                            {
                                ar_2d[i, 1] = "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString();
                            }
                        }
                        else
                        {
                            if (ds.Tables["result"].Rows[i]["MORPHODESC"].ToString() != "")
                            {
                                ar_2d[i, 1] = "<n>(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ")</n> " + ds.Tables["result"].Rows[i]["MORPHODESC"].ToString();
                            }
                            else
                            {
                                ar_2d[i, 1] = "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString();
                            }
                        }

                        checkFontOrgIndex = ds.Tables["result"].Rows[i]["ORGANISMINDEX"].ToString();
                        if (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "ESBL" ||
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "CRE" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "METHICILIN" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "VANCOMYCIN" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "COLISTIN" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "MLSB" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "CABAPEMASE" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "DA" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "MCR-1" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "XDR" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "MDR" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "PDR" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "VRE" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "VRSA" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "VISA" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "CARBAPENEM" || //
                            ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "MRSA") //
                        {
                            if (strOrganismChk != ar_2d[i, 1])
                            {
                                Process_StyleText_Organ(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                            }//END OF Check organism!!!

                            // Add CRE
                            if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "CRE") && (ds.Tables["result"].Rows[i]["TESTRESULT"].ToString() == "Resistant"))
                            {
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Add CABAPEMASE
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString()).Equals("CABAPEMASE"))
                            {
                                //Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + NO_detectionTestShowAfterOrg, "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Add XDR , PDR
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "XDR") || (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "PDR"))
                            {
                                //Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString(), "", detectionTestShowAfterOrg);
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";

                            }
                            // Add VRE , MRSA
                            else if (((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "VRE") || (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "MRSA")) && (ds.Tables["result"].Rows[i]["TESTRESULT"].ToString() != ""))
                            {
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";

                            }
                            // Add MDR 
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "MDR"))
                            {
                                //Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + NO_detectionTestShowAfterOrg, "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "VISA"))
                            {
                                //Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + NO_detectionTestShowAfterOrg, "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "CARBAPENEM"))
                            {
                                //Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + NO_detectionTestShowAfterOrg, "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "VRSA"))
                            {
                                //Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + NO_detectionTestShowAfterOrg, "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "DA"))
                            {
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "MCR-1"))
                            {
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "METHICILIN"))
                            {
                                //Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + NO_detectionTestShowAfterOrg, "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "VANCOMYCIN"))
                            {
                                //Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + NO_detectionTestShowAfterOrg, "", detectionTestShowAfterOrg);

                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "COLISTIN"))
                            {
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "MLSB"))
                            {
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }

                            //TEST   // Notprint Resistant
                            else if ((ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Trim() == "ESBL"))
                            {
                                //Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ds.Tables["result"].Rows[i]["TESTRESULT"].ToString(), "", detectionTestShowAfterOrg);
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + NO_detectionTestShowAfterOrg, "", detectionTestShowAfterOrg);

                                //xrTableCell31_drug.Visible = true;
                                //xrTableCell31_drug.Text = "Drug resistance";
                            }
                            //TEST


                            //Add ESBL in the table.
                            else if (ar_2d[i, 0] != "")
                            {
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + ds.Tables["result"].Rows[i]["FULLTEXT"].ToString() + " " + ar_2d[i, 0], "", detectionTestShowAfterOrg);
                            }

                        }
                        else if (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "XORC" || ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString().Contains("XOCU"))
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
                            // AddDataToXrTable(xrTable_Qutity, result_test, "(" + ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() + ") " + ds.Tables["result"].Rows[i]["ORGANISMSNAME"].ToString(), checkFontOrgIndex);
                            Process_StyleText_Organ(xrTable_Qutity, result_test, ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);

                        }
                        else
                        {
                            if (ds.Tables[0].Rows[i]["ORGANISMINDEX"].ToString() != "" && ds.Tables["result"].Rows[i]["ORGANISMCODE"].ToString() != "")
                            {
                                //check code is not special !!! because special code used in H3 template.
                                if (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() != "SPECIAL" && ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() != "BARCODE".Trim())
                                {
                                    if (strOrganismChk != ar_2d[i, 1])
                                    {
                                        if (ar_2d[i, 0].StartsWith("CD0"))
                                        {
                                            //         AddDataToXrTable(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex);
                                            Process_StyleText_Organ(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                                        }
                                        else
                                        {
                                            //        AddDataToXrTable(xrTable_Qutity, ar_2d[i, 0], ar_2d[i, 1], checkFontOrgIndex);
                                            Process_StyleText_Organ(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                                        }
                                    }
                                    strOrganismChk = ar_2d[i, 1];
                                    //AddDataToXrTable(xrTable_Qutity, ar_2d[i, 0], ar_2d[i, 1], checkFontOrgIndex);
                                }
                                else if (ds.Tables["result"].Rows[i]["DETECTIONTESTCODE"].ToString() == "BARCODE".Trim())
                                {
                                    if (strOrganismChk != ar_2d[i, 1])
                                    {
                                        if (ar_2d[i, 0].StartsWith("CD0") || ar_2d[i, 0].StartsWith("cd0"))
                                        {
                                            //       AddDataToXrTable(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex);
                                            Process_StyleText_Organ(xrTable_Qutity, "", ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
                                        }
                                        else
                                        {
                                            //      AddDataToXrTable(xrTable_Qutity, ar_2d[i, 0], ar_2d[i, 1], checkFontOrgIndex);
                                            Process_StyleText_Organ(xrTable_Qutity, ar_2d[i, 0], ar_2d[i, 1], checkFontOrgIndex, detectionTestShowAfterOrg);
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

                        // Check comment by colony
                        if (!comment_colony.Equals(""))
                        {
                            if (checkSameComment != comment_colony)
                            {
                                Process_StyleText_Organ(xrTable_Qutity, "", " - " + comment_colony, "", "");
                            }
                            checkSameComment = comment_colony;
                        }

                    } //E N D OF For loop.        
                }
                else
                {
                    // NULL Sensitivities.
                }

            }
            catch (Exception ex)
            {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method queryQutity() : " + ex.Message.ToString());
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
                cellQutity.WidthF = xrTableCell3.WidthF;
                cellQutity.Font = new Font(xrTableCell3.Font.FontFamily, xrTableCell3.Font.Size, FontStyle.Regular);
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
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell5.Font.FontFamily, xrTableCell5.Font.Size, FontStyle.Bold | FontStyle.Italic));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell5.Font.FontFamily, xrTableCell5.Font.Size, FontStyle.Bold | FontStyle.Italic));
                                    cell1.Font = new Font(xrTableCell5.Font.FontFamily, xrTableCell5.Font.Size, FontStyle.Bold | FontStyle.Italic);
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
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell5.Font.FontFamily, xrTableCell5.Font.Size, FontStyle.Bold));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell5.Font.FontFamily, xrTableCell5.Font.Size, FontStyle.Bold));
                                    cell1.Font = new Font(xrTableCell5.Font.FontFamily, xrTableCell5.Font.Size, FontStyle.Bold);
                                    row.Cells.Add(cell1);

                                }
                            }
                        }

                    }// END OF For loop.
                    // Set Width end row
                    XRTableCell cell_End = new XRTableCell();
                    cell_End.TextAlignment = TextAlignment.MiddleLeft;
                    cell_End.WidthF = xrTableCell5.WidthF - width_table;
                    row.Cells.Add(cell_End);
                } /* Text is  not Contains <i> or <n> */
                else
                {
                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = text;
                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                    //  width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold));
                    cell1.WidthF = xrTableCell5.WidthF;
                    if (!strORGANISMINDEX.Equals(""))
                    {
                        cell1.Font = new Font(xrTableCell5.Font.FontFamily, xrTableCell5.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    }
                    else
                    {//ESBL Font!
                        cell1.Font = new Font(xrTableCell5.Font.FontFamily, xrTableCell5.Font.Size, FontStyle.Regular);
                    }

                    row.Cells.Add(cell1);
                }

                // Add Rows
                xrTable5.Rows.Add(row);
            }
            catch (Exception ex)
            {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method Process_StyleText() : " + ex.Message.ToString()); 
            }
        }

        private void Process_StyleText_Organ(XRTable xrTable_Qutity, string strQutity, string text, string strORGANISMINDEX, string detectionTestShowAfterOrg)
        {
            try
            {
                // Create new row
                XRTableRow row = new XRTableRow();

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
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell1.Font.FontFamily, xrTableCell1.Font.Size, FontStyle.Bold | FontStyle.Italic));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell1.Font.FontFamily, xrTableCell1.Font.Size, FontStyle.Bold | FontStyle.Italic));
                                    cell1.Font = new Font(xrTableCell1.Font.FontFamily, xrTableCell1.Font.Size, FontStyle.Bold | FontStyle.Italic);
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
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell1.Font.FontFamily, xrTableCell1.Font.Size, FontStyle.Bold));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell1.Font.FontFamily, xrTableCell1.Font.Size, FontStyle.Bold));
                                    cell1.Font = new Font(xrTableCell1.Font.FontFamily, xrTableCell1.Font.Size, FontStyle.Bold);
                                    row.Cells.Add(cell1);

                                }
                            }
                        }

                    }// END OF For loop.
                    // Set Width end row
                    XRTableCell cell_End = new XRTableCell();
                    cell_End.TextAlignment = TextAlignment.MiddleLeft;
                    cell_End.WidthF = xrTableCell1.WidthF - width_table;
                    row.Cells.Add(cell_End);
                } /* Text is  not Contains <i> or <n> */
                else
                {
                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = text;
                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                    //  width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold));
                    cell1.WidthF = xrTableCell1.WidthF;
                    if (!strORGANISMINDEX.Equals(""))
                    {
                        cell1.Font = new Font(xrTableCell1.Font.FontFamily, xrTableCell1.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    }
                    else
                    {//ESBL Font!
                        cell1.Font = new Font(xrTableCell1.Font.FontFamily, xrTableCell1.Font.Size, FontStyle.Regular);
                    }
                    row.Cells.Add(cell1);
                }


                // Add Rows
                xrTable_Qutity.Rows.Add(row);
            }
            catch (Exception ex)
            {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method Process_StyleText() : " + ex.Message.ToString());
            }
        }

        private void searchUsersValInfo()
        {
            /*================================================
              * CHECK USERS VALIDATED and APPROVED.
              * REM: Clinical Validate = Approved by.
              * REM: Last Save in WIP  = Report by.
              ====================================================*/

            try
            {

                if (strSubReqID != "")
                {
                    string sqlVal = @"SELECT SUBREQMB_ACTIONS.SUBACTIONMARKID,SUBREQMB_ACTIONS.SUBREQUESTID,SUBREQMB_ACTIONS.ACTIONMARKTYPE,SUBREQMB_ACTIONS.ACTIONMARKDATE,SUBREQMB_ACTIONS.ACTIONMARKUSERID,SUBREQMB_ACTIONS.ACTIONMARKDATA1,SUBREQMB_ACTIONS.ACTIONMARKDATA2,SUBREQMB_ACTIONS.ACTIONMARKDATA2,SUBREQMB_ACTIONS.ACTIONMARKLINK ,USERS.USERNAME,USERS.NATIONALCODE,SUBREQMB_ACTIONS.ACTIONMARKDATE AS 'VALIDATED' 
FROM SUBREQMB_ACTIONS
LEFT OUTER JOIN USERS ON SUBREQMB_ACTIONS.ACTIONMARKUSERID = USERS.USERID 
WHERE SUBREQUESTID = '" + strSubReqID + "' AND  ( ACTIONMARKTYPE = 27 )  Order by SUBREQMB_ACTIONS.SUBACTIONMARKID DESC";

                    Writedatalog.WriteLog("Step sqlVal--->" + sqlVal);

                    SqlCommand cmdVal = new SqlCommand(sqlVal, conn);
                    DataSet dsVal = new DataSet();
                    SqlDataAdapter adpVal = new SqlDataAdapter(cmdVal);
                    if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                    dsVal.Clear();
                    adpVal.Fill(dsVal, "result");
                    string dsCount = dsVal.Tables["result"].Rows.Count.ToString();
                    if (dsVal.Tables["result"].Rows.Count > 0)
                    {
                        //for (int i = 1; i <= dsVal.Tables["result"].Rows.Count; i++)
                        //{
                        xrTableCell_CompleteDateTime.Text = ((DateTime)(dsVal.Tables["result"].Rows[0]["VALIDATED"])).ToString(format_DT, cultureinfo);

                        if (dsCount == "1")
                        {
                            if (dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() != "")
                            {
                                xrTableCell_Approve.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString() + " (ทน." + dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() + ")";
                            }
                            else
                            {
                                if (dsVal.Tables["result"].Rows[0]["ACTIONMARKUSERID"].ToString() != "SYS")
                                {
                                    xrTableCell_Approve.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString();
                                }
                                else
                                {
                                    xrTableCell_Approve.Text = "PITAK SANTANIRAND (ทน.2212)";
                                }
                            }
                            if (dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() != "")
                            {
                                //    lblLastSave.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString() + " (ทน." + dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() + ")";
                            }
                            else
                            {
                                //   lblLastSave.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString();
                            }
                        }
                        else
                        {
                            if (dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() != "")
                            {
                                xrTableCell_Approve.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString() + " (ทน." + dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() + ")";
                            }
                            else
                            {
                                xrTableCell_Approve.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString();
                            }

                            if (dsVal.Tables["result"].Rows[1]["NATIONALCODE"].ToString() != "")
                            {
                                // lblLastSave.Text = dsVal.Tables["result"].Rows[1]["USERNAME"].ToString() + " (ทน." + dsVal.Tables["result"].Rows[1]["NATIONALCODE"].ToString() + ")";
                            }
                            else
                            {
                                //  lblLastSave.Text = dsVal.Tables["result"].Rows[1]["USERNAME"].ToString();
                            }
                        }

                        string sqlTech = @"SELECT SUBREQMB_ACTIONS.SUBACTIONMARKID,SUBREQMB_ACTIONS.SUBREQUESTID,SUBREQMB_ACTIONS.ACTIONMARKTYPE,SUBREQMB_ACTIONS.ACTIONMARKDATE,SUBREQMB_ACTIONS.ACTIONMARKUSERID,SUBREQMB_ACTIONS.ACTIONMARKDATA1,SUBREQMB_ACTIONS.ACTIONMARKDATA2,SUBREQMB_ACTIONS.ACTIONMARKDATA2,SUBREQMB_ACTIONS.ACTIONMARKLINK ,USERS.USERNAME,USERS.NATIONALCODE
FROM SUBREQMB_ACTIONS
LEFT OUTER JOIN USERS ON SUBREQMB_ACTIONS.ACTIONMARKUSERID = USERS.USERID 
WHERE SUBREQUESTID = '" + strSubReqID + "' AND  ( ACTIONMARKTYPE = 32 )  Order by SUBREQMB_ACTIONS.SUBACTIONMARKID DESC";

                        Writedatalog.WriteLog("Step sqlTech--->" + sqlTech);

                        SqlCommand cmdTech = new SqlCommand(sqlTech, conn);
                        DataSet dsTech = new DataSet();
                        SqlDataAdapter adpTech = new SqlDataAdapter(cmdTech);
                        if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                        dsTech.Clear();
                        adpTech.Fill(dsTech, "result");
                        if (dsTech.Tables["result"].Rows.Count > 0)
                        {
                            if (dsTech.Tables["result"].Rows[0]["NATIONALCODE"].ToString() != "")
                            {
                                xrTableCell_ReportBy.Text = dsTech.Tables["result"].Rows[0]["USERNAME"].ToString() + " (ทน." + dsTech.Tables["result"].Rows[0]["NATIONALCODE"].ToString() + ")";
                                // Fix Report Technical Date time 
                                // Version report V1.14b
                                //
                                xrLabel_DateTime.Text = ((DateTime)(dsTech.Tables["result"].Rows[0]["ACTIONMARKDATE"])).ToString(format_DT, cultureinfo);

                            }
                            else
                            {
                                xrTableCell_ReportBy.Text = dsTech.Tables["result"].Rows[0]["USERNAME"].ToString();
                                // Fix Report Technical Date time 
                                // Version report V1.14b
                                //
                                xrLabel_DateTime.Text = ((DateTime)(dsTech.Tables["result"].Rows[0]["ACTIONMARKDATE"])).ToString(format_DT, cultureinfo);

                            }
                        }
                        else
                        {
                            if (dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() != "")
                            {
                                xrTableCell_ReportBy.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString() + " (ทน." + dsVal.Tables["result"].Rows[0]["NATIONALCODE"].ToString() + ")";
                                // Fix Report Technical Date time 
                                // Version report V1.14b
                                //
                                xrLabel_DateTime.Text = ((DateTime)(dsTech.Tables["result"].Rows[0]["ACTIONMARKDATE"])).ToString(format_DT, cultureinfo);

                            }
                            else
                            {
                                xrTableCell_ReportBy.Text = dsVal.Tables["result"].Rows[0]["USERNAME"].ToString();
                                // Fix Report Technical Date time 
                                // Version report V1.14b
                                //
                                xrLabel_DateTime.Text = ((DateTime)(dsTech.Tables["result"].Rows[0]["ACTIONMARKDATE"])).ToString(format_DT, cultureinfo);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method searchUsersValInfo() : " + ex.Message.ToString()); 
            }
        }

        private void searchProtocolInfo()
        {
            try
            {
                string sql_res = @"SELECT SUBREQUESTS_MB.SUBREQUESTID,SUBREQUESTS_MB.COLLSOURCEID,SUBREQUESTS_MB.COLLMATERIALID,SUBREQUESTS_MB.COLLTECHNIQUEID,SUBREQUESTS_MB.TOPOGRAPHYID,SUBREQUESTS_MB.PROTOCOLID,SUBREQUESTS_MB.SUBREQUESTNUMBER,SUBREQUESTS_MB.SUBREQUESTCREDATE,SUBREQUESTS_MB.NORMALCYTOLOGY,SUBREQUESTS_MB.COLLECTIONDATE,SUBREQUESTS_MB.NOSOINQUIRY,SUBREQUESTS_MB.VALIDATIONSTATUS,SUBREQUESTS_MB.RECEIVEDDATE,SUBREQUESTS_MB.REQUESTID,SUBREQUESTS_MB.URGENT,SUBREQUESTS_MB.NEXTAGARINDEX,SUBREQUESTS_MB.NEXTORGANISMINDEX,SUBREQUESTS_MB.STATUSSTART,SUBREQUESTS_MB.COMPLETED,SUBREQUESTS_MB.NEGATIVEEXAM,SUBREQUESTS_MB.PATHOGENICEXAM,SUBREQUESTS_MB.OPTVALDATE,SUBREQUESTS_MB.CONSOLIDATIONSTATUS,SUBREQUESTS_MB.REFERASKED,SUBREQUESTS_MB.REFERTO,SUBREQUESTS_MB.COLLMATERIALTXT,SUBREQUESTS_MB.COLLSOURCETXT,SUBREQUESTS_MB.COLLTECHNIQUETXT,SUBREQUESTS_MB.TOPOGRAPHYTXT,SUBREQUESTS_MB.LABOID,SUBREQUESTS_MB.SAMPLEID,DICT_COLL_SOURCES.COLLSOURCEID,DICT_COLL_SOURCES.COLLSOURCECODE,DICT_COLL_MATERIALS.FULLTEXT as 'SAMPLE',DICT_COLL_SOURCES.LISTEXTCODE,DICT_COLL_MATERIALS.COLLMATERIALID,DICT_COLL_MATERIALS.COLLMATERIALCODE,DICT_COLL_MATERIALS.SHORTTEXT,DICT_COLL_MATERIALS.LISTEXTCODE,DICT_COLL_TECHNIQUES.COLLTECHNIQUEID,DICT_COLL_TECHNIQUES.COLLTECHNIQUECODE,DICT_COLL_TECHNIQUES.SHORTTEXT,DICT_COLL_TECHNIQUES.LISTEXTCODE,DICT_TOPOGRAPHIES.TOPOGRAPHYID,DICT_TOPOGRAPHIES.TOPOGRAPHYCODE,DICT_TOPOGRAPHIES.SHORTTEXT,DICT_TOPOGRAPHIES.LISTEXTCODE,DICT_MB_PROTOCOLS.PROTOCOLID,DICT_MB_PROTOCOLS.PROTOCOLCODE,DICT_MB_PROTOCOLS.FULLTEXT as 'PROTOCOLS',DICT_MB_PROTOCOLS.PROTOCOLDESCFILE,DICT_MB_PROTOCOLS.TEXTCLASS
 FROM SUBREQUESTS_MB  LEFT OUTER JOIN DICT_COLL_SOURCES ON  SUBREQUESTS_MB.COLLSOURCEID = DICT_COLL_SOURCES.COLLSOURCEID 
  LEFT OUTER JOIN DICT_COLL_MATERIALS ON  SUBREQUESTS_MB.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID 
   LEFT OUTER JOIN DICT_COLL_TECHNIQUES ON  SUBREQUESTS_MB.COLLTECHNIQUEID = DICT_COLL_TECHNIQUES.COLLTECHNIQUEID 
    LEFT OUTER JOIN DICT_TOPOGRAPHIES ON  SUBREQUESTS_MB.TOPOGRAPHYID = DICT_TOPOGRAPHIES.TOPOGRAPHYID 
	 LEFT OUTER JOIN DICT_MB_PROTOCOLS ON  SUBREQUESTS_MB.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID
  WHERE  SUBREQUESTS_MB.SUBREQUESTID = '" + strSubReqID + "'";

                Writedatalog.WriteLog("Step searchProtocolInfo--->" + sql_res);

                SqlCommand cmd_res = new SqlCommand(sql_res, conn);
                SqlDataAdapter adp_res = new SqlDataAdapter(cmd_res);
                DataSet ds_res = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds_res.Clear();

                adp_res.Fill(ds_res, "result");

                if (ds_res.Tables["result"].Rows.Count > 0)
                {
                    lblGroupTest.Text = "" + ds_res.Tables["result"].Rows[0]["PROTOCOLS"].ToString() + "";
                    lblSpecimen.Text = ds_res.Tables["result"].Rows[0]["SAMPLE"].ToString();
                    xrTableCell_Specimen.Text = ds_res.Tables["result"].Rows[0]["SAMPLE"].ToString();
                }

            }catch(Exception ex){

                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method searchProtocolInfo() : " + ex.Message.ToString()); 
            }
        }

        private void searchLocationInfo()
        {
            try
            {
                string sql_loc = @" SELECT LOCATIONS.REQLOCID,LOCATIONS.REQUESTID,LOCATIONS.LOCID,LOCATIONS.PRINTOUTNUMBER,LOCATIONS.LOCREPORTDATE,LOCATIONS.LOGUSERID,LOCATIONS.LOGDATE,DICT_LOCATIONS.LOCID,DICT_LOCATIONS.LOCCODE,DICT_LOCATIONS.LOCCREDATE,DICT_LOCATIONS.LOCNAME,DICT_LOCATIONS.ADDRESS1,DICT_LOCATIONS.ADDRESS2,DICT_LOCATIONS.CITY,DICT_LOCATIONS.STATE,DICT_LOCATIONS.POSTALCODE,DICT_LOCATIONS.COUNTRY,DICT_LOCATIONS.TELEPHON,DICT_LOCATIONS.TELEPHON2,DICT_LOCATIONS.FAX,DICT_LOCATIONS.HOSTCODE,DICT_LOCATIONS.HOSTTEXT,DICT_LOCATIONS.INFORMATION,DICT_LOCATIONS.NATIONALCODE,DICT_LOCATIONS.CATEGORY,DICT_LOCATIONS.FULLTEXT,DICT_LOCATIONS.DEFDISCCODE,DICT_LOCATIONS.AUTONUMBERID,DICT_LOCATIONS.ENDVALIDDATE,DICT_LOCATIONS.SITEID,DICT_LOCATIONS.SITEID,DICT_LOCATIONS.MEDICALDISCID,DICT_LOCATIONS.TITLEID,DICT_TEXTS.TEXTCODE,DICT_TEXTS.SHORTTEXT FROM LOCATIONS LEFT OUTER JOIN DICT_LOCATIONS ON LOCATIONS.LOCID=DICT_LOCATIONS.LOCID LEFT OUTER JOIN DICT_TEXTS ON DICT_LOCATIONS.TITLEID=DICT_TEXTS.TEXTID    WHERE  (LOCATIONS.REQUESTID = '" + strReqID + "')";

                SqlCommand cmd = new SqlCommand(sql_loc, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds_loc = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds_loc.Clear();

                adp.Fill(ds_loc, "result");

                if (ds_loc.Tables["result"].Rows.Count > 0)
                {
                    department.Text = ds_loc.Tables["result"].Rows[0]["LOCNAME"].ToString() + " " + ds_loc.Tables["result"].Rows[0]["ADDRESS1"].ToString() + " " + ds_loc.Tables["result"].Rows[0]["ADDRESS2"].ToString();

                }
            }


            catch (Exception ex) {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method searchLocationInfo() : " + ex.Message.ToString()); 
            }
        }

        private void searchRequestInfo()
        {
            try
            {
                string sql_req = @" SELECT REQUESTS.REQUESTID,REQUESTS.ACCESSNUMBER,REQUESTS.REQCREATIONDATE,REQUESTS.PATID,REQUESTS.HOSPITID,REQUESTS.CATEGORY,REQUESTS.REDACCNUM,REQUESTS.REQSTATUS,REQUESTS.STATUSDATE, REQUESTS.REQDATE AS 'REQDATE',REQUESTS.COLLECTIONDATE,REQUESTS.RECEIVEDDATE AS 'RECEIVEDDATE',REQUESTS.URGENT,REQUESTS.ROOMNUMBER,REQUESTS.LASTUPDTESTDATE,REQUESTS.REQTOVALID,REQUESTS.REQURL,REQUESTS.TOPHONE,REQUESTS.LMP,REQUESTS.NBCHILDS,REQUESTS.AUTODATE,REQUESTS.NECROSCOPY,REQUESTS.ENCLOSED,REQUESTS.LISUSER,REQUESTS.LISDATE,REQUESTS.LOGSESSION,REQUESTS.LABOID,REQUESTS.LOGUSERID,REQUESTS.LOGDATE,REQUESTS.EXTERNALORDERNUMBER,REQUESTS.USERFIELD1,REQUESTS.USERFIELD2,REQUESTS.CREATEDONTDR,REQUESTS.SOURCESITEID,REQUESTS.EXTERNALCOLLECTION,REQUESTS_OCOM.REQCOMSUBJECT FROM REQUESTS LEFT OUTER JOIN REQUESTS_OCOM ON REQUESTS.REQUESTID = REQUESTS_OCOM.REQUESTID WHERE (REQUESTS.REQUESTID = '" + strReqID + "')";

                SqlCommand cmd = new SqlCommand(sql_req, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds_req = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds_req.Clear();

                adp.Fill(ds_req, "result");

                if (ds_req.Tables["result"].Rows.Count > 0)
                {
                    //xrTableCell_Date.Text = ((DateTime)(ds_req.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString("dd/MM/yyyy", cultureinfo);
                    xrTableCell_RequestedDateTime.Text = ((DateTime)(ds_req.Tables["result"].Rows[0]["REQDATE"])).ToString(format_DT, cultureinfo);
                    xrTableCell_CheckInDateTime.Text = ((DateTime)(ds_req.Tables["result"].Rows[0]["RECEIVEDDATE"])).ToString(format_DT, cultureinfo); 
                }
            }
            catch (Exception ex)
            {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method searchRequestInfo() : " + ex.Message.ToString()); 
            }
        }

        private void searchDoctorInfo()
        {

            try
            {
                string sql_doc = @" select DICT_DOCTORS.DOCID,DICT_DOCTORS.DOCCODE, DICT_DOCTORS.DOCNAME from DOCTORS,DICT_DOCTORS 
where (DOCTORS.DOCID=DICT_DOCTORS.DOCID) and (DOCTORS.PRESCRIBER=1) and (DOCTORS.REQUESTID='" + strReqID + "')";
                SqlCommand cmd = new SqlCommand(sql_doc, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds_doc = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds_doc.Clear();

                adp.Fill(ds_doc, "result");

                if (ds_doc.Tables["result"].Rows.Count > 0)
                {
                    //xrTableCell_Doc.Text = ds_doc.Tables["result"].Rows[0]["DOCNAME"].ToString();
                }
            }
            catch (Exception ex)
            {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method searchDoctorInfo() : " + ex.Message.ToString()); 
            }
        }
        private void serch_batchInfo()
        {
            
            try
            {
                string strSubReqID_2;
                string sql_batchInfo = @"SELECT distinct SUBREQMB_ORGANISMS.ORGANISMINDEX, PATIENTS.PATNUMBER AS 'HN', PATIENTS.NAME,PATIENTS.SEX,
       PATIENTS.FIRSTNAME as 'LASTNAME', REQUESTS.REQUESTID,REQUESTS.ACCESSNUMBER,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))/12 as ageY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate()))%12 as ageM,
       REQUESTS.PATID, SUBREQUESTS_MB.SUBREQUESTID, CONVERT (varchar,    SUBREQUESTS_MB.COLLECTIONDATE, 120) AS 'COLLECTIONDATE', SUBREQUESTS_MB.COLLMATERIALID,
       SUBREQMB_ORGANISMS.SUBREQMBORGID,
       SUBREQMB_ORGANISMS.ORGANISMID, SUBREQMB_ORGANISMS.IDENTDATE,
       SUBREQMB_ANTIBIOTICS.SUBREQMBANTIBIOTICID,
       SUBREQMB_ANTIBIOTICS.ANTIBIOTICID,
       DICT_MB_ORGANISMS.ORGANISMNAME, DICT_MB_ORGANISMS.FULLTEXT
       as 'ORGANISMSNAME', DICT_MB_ORGANISMS.MORPHODESC,DICT_MB_ANTIBIOTICS.ANTIBIOTICCODE,
       DICT_MB_ANTIBIOTICS.FULLTEXT as 'ANTIBIOTICSNAME',
     CASE (   SUBREQMB_ANTIBIOTICS.UNITS ) WHEN '1' THEN 'mg/L' WHEN '2' THEN 'mm' ELSE '' END AS 'UNITS'
, REQUESTS.RECEIVEDDATE AS 'RECEIVEDDATE',
       REQUESTS.REQDATE AS 'REQDATE', SUBREQMB_ACTIONS.ACTIONMARKUSERID,
       USERS.USERNAME, RIGHT (SUBREQMB_COLONIES.COLONYNUMBER,
       3) AS 'COLONYNUMBER', CASE ( SUBREQMB_ANTIBIOTICS.RESULT) WHEN '1' THEN 'S' WHEN 2 THEN 'I' WHEN 3 THEN 'R' ELSE '' END AS 'RESULT' ,
       SUBREQMB_ANTIBIOTICS.RESULTVALUE,
             '('+  SUBREQMB_ANTIBIOTICS.MINIMUM +' - '+ SUBREQMB_ANTIBIOTICS.MAXIMUM +')' as 'breakpoints',
       CASE (SUBREQMB_ACTIONS.ACTIONMARKTYPE) WHEN 8 THEN SUBREQMB_ACTIONS.ACTIONMARKDATE WHEN 23 THEN SUBREQMB_ACTIONS.ACTIONMARKDATE END AS 'REQSTATUS',
       SUBREQUESTS_MB.SUBREQUESTNUMBER,
       DICT_COLL_MATERIALS.COLLMATERIALCODE,
       DICT_COLL_MATERIALS.FULLTEXT AS 'MATERIALSTEXT',
       DICT_MB_PROTOCOLS.PROTOCOLCODE,
       DICT_MB_PROTOCOLS.FULLTEXT AS 'PROTOCALTEXT',
       SUBREQMB_DET_TESTS.TESTRESULT, SUBREQMB_ANTIBIOTICS.UNITS,SUBREQMB_ACTIONS.ACTIONMARKDATE AS 'VALIDATED',DICT_LOCATIONS.LOCNAME,DICT_LOCATIONS.ADDRESS1,DICT_LOCATIONS.ADDRESS2 ,DICT_TEXTS.FULLTEXT as 'COMMENT',
SUBREQMB_COLONIES.COLONYINDEX,DICT_DOCTORS.DOCCODE,DICT_DOCTORS.DOCNAME,DICT_DOCTORS.ADDRESS1 AS 'DOCNAME2',SUBREQMB_OCOM.COMMENTTEXT
FROM REQUESTS LEFT OUTER JOIN SUBREQUESTS_MB ON (REQUESTS.REQUESTID = SUBREQUESTS_MB.REQUESTID) 
LEFT OUTER JOIN SUBREQMB_ORGANISMS ON (SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ORGANISMS.SUBREQUESTID)
LEFT OUTER JOIN DICT_MB_ORGANISMS ON (SUBREQMB_ORGANISMS.ORGANISMID = DICT_MB_ORGANISMS.ORGANISMID)
LEFT OUTER JOIN SUBREQMB_ANTIBIOTICS ON (SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_ANTIBIOTICS.SUBREQMBORGID) 
LEFT OUTER JOIN DICT_MB_ANTIBIOTICS ON (SUBREQMB_ANTIBIOTICS.ANTIBIOTICID = DICT_MB_ANTIBIOTICS.ANTIBIOTICID) 
LEFT OUTER JOIN DICT_MB_ANTIBIO_FAMS ON (DICT_MB_ANTIBIOTICS.ANTIBIOTICSFAMILYID = DICT_MB_ANTIBIO_FAMS.ANTIBIOTICSFAMILYID)
LEFT OUTER JOIN SUBREQMB_ACTIONS ON SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ACTIONS.SUBREQUESTID
INNER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID 
LEFT OUTER JOIN DICT_MB_PROTOCOLS ON SUBREQUESTS_MB.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID 
LEFT OUTER JOIN DICT_COLL_MATERIALS ON SUBREQUESTS_MB.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID
LEFT OUTER JOIN DICT_COLL_SOURCES ON SUBREQUESTS_MB.COLLSOURCEID = DICT_COLL_SOURCES.COLLSOURCEID
LEFT OUTER JOIN USERS ON users.USERID = SUBREQMB_ACTIONS.ACTIONMARKUSERID
LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_COLONIES.SUBREQMBORGID 
LEFT OUTER JOIN SUBREQMB_DET_TESTS ON SUBREQMB_COLONIES.COLONYID = SUBREQMB_DET_TESTS.COLONYID
	 LEFT OUTER JOIN LOCATIONS ON REQUESTS.REQUESTID = LOCATIONS.REQUESTID
							 LEFT OUTER JOIN DICT_LOCATIONS ON LOCATIONS.LOCID = DICT_LOCATIONS.LOCID
		 LEFT OUTER JOIN SUBREQMB_OCOM on SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_OCOM.SUBREQUESTID
							 LEFT OUTER JOIN DICT_TEXTS ON SUBREQMB_OCOM.COMMENTCODEDID = DICT_TEXTS.TEXTID
LEFT OUTER JOIN DOCTORS ON REQUESTS.REQUESTID  = DOCTORS.REQUESTID
							 LEFT OUTER JOIN DICT_DOCTORS ON DOCTORS.DOCID = DICT_DOCTORS.DOCID
WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "'   AND SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString().Trim() + "'  AND SUBREQMB_ACTIONS.ACTIONMARKTYPE = " + status.Value.ToString() ;

                Writedatalog.WriteLog("Query result Batch result comment --->" + sql_batchInfo);
                SqlCommand cmd_batch = new SqlCommand(sql_batchInfo, conn);
                SqlDataAdapter adp_batch = new SqlDataAdapter(cmd_batch);
                DataSet ds_batch = new DataSet();
                if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
                ds_batch.Clear();

                adp_batch.Fill(ds_batch, "result_batch");
                

                if (ds_batch.Tables["result_batch"].Rows.Count > 0)
            {
                    GroupHeader7.Visible = true;
                    xrLabel3_batch.Visible = true;
                    xrLabel3.Multiline = true;

                    xrLabel3_batch.LocationF = new PointF(20,185);

                    //xrLabel3_batch.Text = ds_batch.Tables["result_batch"].Rows[0]["COMMENTTEXT"].ToString();
                    // NEW DEsign
                    //var result1 = H3_batch.Split(new string[] { "(" }, StringSplitOptions.None);
                    //xrLabel3_batch.Text = result1[0] + "\r\n" + "(" + result1[1];

                    string H3_batch = ds_batch.Tables["result_batch"].Rows[0]["COMMENTTEXT"].ToString();

                    if (H3_batch.ToLower().Contains("("))
                    {
                        string[] result = H3_batch.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries);
                        xrLabel3_batch.Text = result[0] + "\r\n"+"(" + result[1];

                        //Writedatalog.ORUHL7Log("get string--xrLabel3_batch  >" + "(" + "\r\n" + H3_batch);
                    }
                    else if (H3_batch.ToLower().Contains("^"))
                    {
                        //Writedatalog.ORUHL7Log("get string--xrLabel3_batch  >" + "^" + "\r\n" + H3_batch);
                        String I = "^";
                        int countRegex = 0;
                        foreach (char c in I)
                        {
                            countRegex++;
                        }
                        string[] result = H3_batch.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
                        //Writedatalog.ORUHL7Log("get string--xrLabel3_batch  >" + countRegex);
                        if (countRegex == 1)
                        {
                            xrLabel3_batch.Text = result[0] + "\r\n" + result[1];
                        }
                        if (countRegex == 2)
                        {
                            xrLabel3_batch.Text = result[0] + "\r\n" + ".," + result[1] + "\r\n" + ".," + result[2];
                        }
                        if (countRegex == 3)
                        {
                            xrLabel3_batch.Text = result[0] + "\r\n" + ".," + result[1] + "\r\n" + ".," + result[2] + "\r\n" + ".," + result[3];
                        }
                    }
                    else
                    {
                        //Writedatalog.ORUHL7Log("Not found string ( and ^  >" + ds_batch.Tables["result_batch"].Rows[0]["COMMENTTEXT"].ToString());
                        xrLabel3_batch.Text = ds_batch.Tables["result_batch"].Rows[0]["COMMENTTEXT"].ToString();

                    }
                    // END NEW DEsign

                    strSubReqID_2 = ds_batch.Tables["result_batch"].Rows[0]["SUBREQUESTID"].ToString();
                    strSubReqID = strSubReqID_2;
                    Writedatalog.WriteLog("Step Get Subrequest ID strSubReqID_2 --> == " + strSubReqID_2);


                    Writedatalog.WriteLog("result = >> " + ds_batch.Tables["result_batch"].Rows[0]["COMMENTTEXT"].ToString());
            }
            }
            catch (Exception ex)
            {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Searth Comment result to comment text by batch : " + ex.Message.ToString()); 
            }

}

        private void xrPivotGrid1_CustomCellDisplayText(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotCellDisplayTextEventArgs e)
        {
            PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();

            if (e.DataField == xrPivotGridField4)
            {
                for (int i = 0; i < ds.RowCount; i++)
                {
                    e.DisplayText = String.Format("{0}", ds[i][e.DataField]);
                }
            }

            //RESULT.
            else if (e.DataField == pivotGridField1)
            {
                for (int i = 0; i < ds.RowCount; i++)
                {

                    string strRes = String.Format("{0}", ds[i][e.DataField]);

                    string[] sss = strRes.ToString().Split('.');
                    if (sss.Length > 1)
                    {
                        if (sss[1].Length >= 0) // มีจุดทศนิยม
                        {
                            string res = sss[1].TrimEnd('0');
                            if (res.ToString() != "") // จุดทศนิยม != 0 หรือ ""
                            {
                                e.DisplayText = String.Format("{0}", sss[0] + "." + res);
                            }
                            else
                            {
                                e.DisplayText = String.Format("{0}", sss[0]);
                            }
                        }
                        else // ถ้าไม่มีจุดทศนิยม ให้แสดงค่า default จาก db
                        {
                            e.DisplayText = String.Format("{0}", ds[i][e.DataField]);
                        }
                    }
                    else
                    {
                        e.DisplayText = String.Format("{0}", ds[i][e.DataField]);
                    }

                }
                if (e.Value == "0" || e.Value == "")
                {
                    e.DisplayText = "-";
                }

            }

            // Units
            else if (e.DataField == pivotGridField5)
            {
                for (int i = 0; i < ds.RowCount; i++)
                {
                    e.DisplayText = String.Format("{0}", ds[i][e.DataField]);
                }
            }
        }

        private void Process_StyleText(XRTable xrTable_Qutity, string text, string strORGANISMINDEX)
        {
            try
            {
                // Create new row
                XRTableRow row = new XRTableRow();

                // width for process del width Xrtable
                float width_table = 0;

                string xxx = text;

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
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTable_Qutity.Font.FontFamily, xrTable_Qutity.Font.Size, FontStyle.Bold));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTable_Qutity.Font.FontFamily, xrTable_Qutity.Font.Size, FontStyle.Bold));
                                    cell1.Font = new Font(xrTable_Qutity.Font.FontFamily, xrTable_Qutity.Font.Size, FontStyle.Bold | FontStyle.Italic);
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
                                    width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTable_Qutity.Font.FontFamily, xrTable_Qutity.Font.Size, FontStyle.Bold));
                                    cell1.WidthF = MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTable_Qutity.Font.FontFamily, xrTable_Qutity.Font.Size, FontStyle.Bold));
                                    cell1.Font = new Font(xrTable_Qutity.Font.FontFamily, xrTable_Qutity.Font.Size, FontStyle.Bold);
                                    row.Cells.Add(cell1);


                                }
                            }
                        }

                    }// END OF For loop.
                    // Set Width end row
                    XRTableCell cell_End = new XRTableCell();
                    cell_End.TextAlignment = TextAlignment.MiddleLeft;
                    cell_End.WidthF = xrTable_Qutity.WidthF - width_table;
                    row.Cells.Add(cell_End);
                } /* Text is  not Contains <i> or <n> */
                else
                {
                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = text;
                    cell1.TextAlignment = TextAlignment.MiddleLeft;
                    //  width_table += MeasureTextWidthPixels(ReportUnit.Pixels, abc, new Font(xrTableCell4.Font.FontFamily, xrTableCell4.Font.Size, FontStyle.Bold));
                    cell1.WidthF = xrTable_Qutity.WidthF;
                    if (!strORGANISMINDEX.Equals(""))
                    {
                        cell1.Font = new Font(xrTable_Qutity.Font.FontFamily, xrTable_Qutity.Font.Size, FontStyle.Bold | FontStyle.Italic);
                    }
                    else
                    {//ESBL Font!
                        cell1.Font = new Font(xrTable_Qutity.Font.FontFamily, xrTable_Qutity.Font.Size, FontStyle.Regular);
                    }
                    row.Cells.Add(cell1);
                }

                // Add Rows
                xrTable_Qutity.Rows.Add(row);
            }
            catch (Exception ex)
            {
                Writedatalog.WriteLog(DateTime.Now.ToString() + " Template Template_Hemoculture_p12 error method Process_StyleText() : " + ex.Message.ToString()); 
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

        private void Template_Hemoculture_p12_AfterPrint(object sender, EventArgs e)
        {

        }


        // New Design
        private void queryQutity_specialHeader3()
        {
                string strTopoGraphy = "";
            Writedatalog.WriteLog(strReqID + "<---->" + potocalnumber);

            Writedatalog.WriteLog("Start ---> queryQutity_specialHeader3 No data============> Class cls_querydataset.cs ");

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
                    strOBX += "MB" + dsDetection.Tables[i].Rows[0]["DETECTIONTESTCODE"].ToString() + "^" + dsDetection.Tables[i].Rows[0]["SHORTTEXT"].ToString() + "||" + strResults + "";

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
                    // NEW Design
                    xrLabel_comment2.Text = dsDetection.Tables[0].Rows[i]["COMMENT_FREE"].ToString();
                    // END Design
                    strOBX += "MB" + dsDetection.Tables[0].Rows[i]["SUBREQMBTESTID"].ToString() + "^" + dsDetection.Tables[0].Rows[i]["SHORTTEXT"].ToString() + "||" + strResults + "";
                }
            }
            if (chkSpecial != "")
            {
                xrLabel2.Text = "";
                GroupHeader6.Visible = true;
                GroupHeader3.Visible = false;
                xrLabel2.Visible = true;
                xrLabel2.Multiline = true;
                //xrLabel2.Text += strTopoGraphy;
                // NEW Design

                string batch = strTopoGraphy;
                //xrLabel2.Text = batch;

                if (batch.ToLower().Contains("("))
                {
                    var result1 = batch.Split(new string[] { "(" }, StringSplitOptions.None);
                    xrLabel2.Text = result1[0] + "\r\n" + "(" + result1[1];
                }
                else if (batch.ToLower().Contains("^"))
                {
                    String I = "^";
                    int countRegex = 0;
                    foreach (char c in I)
                    {
                        countRegex++;
                    }
                    string[] result = batch.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
                    if (countRegex == 1)
                    {
                        xrLabel2.Text = result[0] + "\r\n" + result[1];
                    }
                    if (countRegex == 2)
                    {
                        xrLabel2.Text = result[0] + "\r\n" + ".," + result[1] + "\r\n" + ".," + result[2];
                    }
                    if (countRegex == 3)
                    {
                        xrLabel2.Text = result[0] + "\r\n" + ".," + result[1] + "\r\n" + ".," + result[2] + "\r\n" + ".," + result[3]; ;
                    }
                }
                else
                {
                    xrLabel2.Text = batch;
                }

                // End NEW Design

            }
            else
            {
                //  xrTableCell4.Visible = false;
            }
        }

        // End New Design

        private void Query_Batch_negative_H3()
        {
            string sql_batch = "";

            sql_batch = @"SELECT distinct SUBREQMB_ORGANISMS.ORGANISMINDEX, PATIENTS.PATNUMBER AS 'HN', PATIENTS.NAME,PATIENTS.SEX,
       PATIENTS.FIRSTNAME as 'LASTNAME', REQUESTS.REQUESTID,REQUESTS.ACCESSNUMBER,ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate())) / 12 as ageY , ABS(DATEDIFF(month, PATIENTS.BIRTHDATE, getdate())) % 12 as ageM,
       REQUESTS.PATID, SUBREQUESTS_MB.SUBREQUESTID, CONVERT(varchar, SUBREQUESTS_MB.COLLECTIONDATE, 120) AS 'COLLECTIONDATE', SUBREQUESTS_MB.COLLMATERIALID,
       SUBREQMB_ORGANISMS.SUBREQMBORGID,
       SUBREQMB_ORGANISMS.ORGANISMID, SUBREQMB_ORGANISMS.IDENTDATE,
       SUBREQMB_ANTIBIOTICS.SUBREQMBANTIBIOTICID,
       SUBREQMB_ANTIBIOTICS.ANTIBIOTICID,
       DICT_MB_ORGANISMS.ORGANISMNAME, DICT_MB_ORGANISMS.FULLTEXT
       as 'ORGANISMSNAME', DICT_MB_ORGANISMS.MORPHODESC,DICT_MB_ANTIBIOTICS.ANTIBIOTICCODE,
       DICT_MB_ANTIBIOTICS.FULLTEXT as 'ANTIBIOTICSNAME',
       CASE(SUBREQMB_ANTIBIOTICS.UNITS) WHEN '1' THEN 'mg/L' WHEN '2' THEN 'mm' ELSE '' END AS 'UNITS'
       , REQUESTS.RECEIVEDDATE AS 'RECEIVEDDATE',
       REQUESTS.REQDATE AS 'REQDATE', SUBREQMB_ACTIONS.ACTIONMARKUSERID,
       USERS.USERNAME, RIGHT(SUBREQMB_COLONIES.COLONYNUMBER,
       3) AS 'COLONYNUMBER', CASE(SUBREQMB_ANTIBIOTICS.RESULT) WHEN '1' THEN 'S' WHEN 2 THEN 'I' WHEN 3 THEN 'R' ELSE '' END AS 'RESULT' ,
       SUBREQMB_ANTIBIOTICS.RESULTVALUE,
       '(' + SUBREQMB_ANTIBIOTICS.MINIMUM + ' - ' + SUBREQMB_ANTIBIOTICS.MAXIMUM + ')' as 'breakpoints',
       CASE(SUBREQMB_ACTIONS.ACTIONMARKTYPE) WHEN 8 THEN SUBREQMB_ACTIONS.ACTIONMARKDATE WHEN 23 THEN SUBREQMB_ACTIONS.ACTIONMARKDATE END AS 'REQSTATUS',
       SUBREQUESTS_MB.SUBREQUESTNUMBER,
       DICT_COLL_MATERIALS.COLLMATERIALCODE,
       DICT_COLL_MATERIALS.FULLTEXT AS 'MATERIALSTEXT',
       DICT_MB_PROTOCOLS.PROTOCOLCODE,
       DICT_MB_PROTOCOLS.FULLTEXT AS 'PROTOCALTEXT',
       SUBREQMB_DET_TESTS.TESTRESULT, SUBREQMB_ANTIBIOTICS.UNITS,SUBREQMB_ACTIONS.ACTIONMARKDATE AS 'VALIDATED',DICT_LOCATIONS.LOCNAME,DICT_LOCATIONS.ADDRESS1,DICT_LOCATIONS.ADDRESS2 ,DICT_TEXTS.FULLTEXT as 'COMMENT',
SUBREQMB_COLONIES.COLONYINDEX,DICT_DOCTORS.DOCCODE,DICT_DOCTORS.DOCNAME,DICT_DOCTORS.ADDRESS1 AS 'DOCNAME2',SUBREQMB_OCOM.COMMENTTEXT
FROM REQUESTS LEFT OUTER JOIN SUBREQUESTS_MB ON (REQUESTS.REQUESTID = SUBREQUESTS_MB.REQUESTID)
LEFT OUTER JOIN SUBREQMB_ORGANISMS ON(SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ORGANISMS.SUBREQUESTID)
LEFT OUTER JOIN DICT_MB_ORGANISMS ON(SUBREQMB_ORGANISMS.ORGANISMID = DICT_MB_ORGANISMS.ORGANISMID)
LEFT OUTER JOIN SUBREQMB_ANTIBIOTICS ON(SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_ANTIBIOTICS.SUBREQMBORGID)
LEFT OUTER JOIN DICT_MB_ANTIBIOTICS ON(SUBREQMB_ANTIBIOTICS.ANTIBIOTICID = DICT_MB_ANTIBIOTICS.ANTIBIOTICID)
LEFT OUTER JOIN DICT_MB_ANTIBIO_FAMS ON(DICT_MB_ANTIBIOTICS.ANTIBIOTICSFAMILYID = DICT_MB_ANTIBIO_FAMS.ANTIBIOTICSFAMILYID)
LEFT OUTER JOIN SUBREQMB_ACTIONS ON SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_ACTIONS.SUBREQUESTID
INNER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
LEFT OUTER JOIN DICT_MB_PROTOCOLS ON SUBREQUESTS_MB.PROTOCOLID = DICT_MB_PROTOCOLS.PROTOCOLID
LEFT OUTER JOIN DICT_COLL_MATERIALS ON SUBREQUESTS_MB.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID
LEFT OUTER JOIN DICT_COLL_SOURCES ON SUBREQUESTS_MB.COLLSOURCEID = DICT_COLL_SOURCES.COLLSOURCEID
LEFT OUTER JOIN USERS ON users.USERID = SUBREQMB_ACTIONS.ACTIONMARKUSERID
LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_ORGANISMS.SUBREQMBORGID = SUBREQMB_COLONIES.SUBREQMBORGID
LEFT OUTER JOIN SUBREQMB_DET_TESTS ON SUBREQMB_COLONIES.COLONYID = SUBREQMB_DET_TESTS.COLONYID
LEFT OUTER JOIN LOCATIONS ON REQUESTS.REQUESTID = LOCATIONS.REQUESTID
LEFT OUTER JOIN DICT_LOCATIONS ON LOCATIONS.LOCID = DICT_LOCATIONS.LOCID
LEFT OUTER JOIN SUBREQMB_OCOM on SUBREQUESTS_MB.SUBREQUESTID = SUBREQMB_OCOM.SUBREQUESTID
LEFT OUTER JOIN DICT_TEXTS ON SUBREQMB_OCOM.COMMENTCODEDID = DICT_TEXTS.TEXTID
LEFT OUTER JOIN DOCTORS ON REQUESTS.REQUESTID = DOCTORS.REQUESTID
LEFT OUTER JOIN DICT_DOCTORS ON DOCTORS.DOCID = DICT_DOCTORS.DOCID
WHERE REQUESTS.ACCESSNUMBER = '" + accessnumber.Value.ToString() + "'   AND SUBREQUESTS_MB.SUBREQUESTNUMBER = '" + potocalnumber.Value.ToString().Trim() + "' AND SUBREQMB_ACTIONS.ACTIONMARKTYPE = '41' ";

            Writedatalog.WriteLog("Query Batch negative P13 --->" + sql_batch);
            SqlCommand sql_batchP13 = new SqlCommand(sql_batch, conn);
            SqlDataAdapter adp_batchP13 = new SqlDataAdapter(sql_batchP13);
            DataSet ds_batchP13 = new DataSet();
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds_batchP13.Clear();
            adp_batchP13.Fill(ds_batchP13, "result_batchP13");




        }

    }
}
