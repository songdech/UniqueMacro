using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using UniquePro.Common;
using UNIQUE.Instrument.MicroScan.Entites;
using System.Collections.Generic;
using UniquePro.Entities.Common;

namespace UNIQUE
{
    public class MBReportDAO
    {
        private String Constr = "";
        private SqlTransaction trans;
        private DBFactory dbFactory;

        public MBReportDAO()
        {
            ConnectString objConstr = new ConnectString();

            try
            {
                Constr = objConstr.Connectionstring();

                dbFactory = new DBFactory();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable Get_Info_Report1(MBReportM objMBReportM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 MB_ACTIONS.SUBREQUESTID
,MB_ACTIONS.ACTIONTYPE
,MB_ACTIONS.ACTIONDATE
,MB_ACTIONS.ACTIONUSER
,REQUESTS.ACCESSNUMBER
,REQUESTS.REQDOCTOR
,REQUESTS.REQLOCATION
,DICT_DOCTORS.DOCNAME
,DICT_LOCATIONS.LOCNAME
,DICT_COLL_MATERIALS.COLLMATERIALTEXT AS 'REQTEST_SPECIMEN'
,DICT_MB_GROUP_SPECIMEN.SPM_NAME AS 'GROUP_SPECIMENNAME'
,MB_REQUESTS.MBREQNUMBER
,PATIENTS.PATNUMBER
,PATIENTS.TITLE1
,PATIENTS.TITLE2
,PATIENTS.NAME
,PATIENTS.LASTNAME
,USERS.NAME
,USERS.LASTNAME
,USERS.CERTIFICATE
 FROM MB_ACTIONS
 LEFT OUTER JOIN MB_REQUESTS ON MB_REQUESTS.MBREQUESTID = MB_ACTIONS.SUBREQUESTID
 LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID
 LEFT OUTER JOIN REQ_TESTS ON REQUESTS.REQUESTID = REQ_TESTS.REQUESTID
 LEFT OUTER JOIN DICT_DOCTORS ON REQUESTS.REQDOCTOR = DICT_DOCTORS.DOCCODE
 LEFT OUTER JOIN DICT_LOCATIONS ON REQUESTS.REQLOCATION = DICT_LOCATIONS.LOCNAME
 LEFT OUTER JOIN DICT_COLL_MATERIALS ON REQ_TESTS.COLLMATERIALID = DICT_COLL_MATERIALS.COLLMATERIALID
 LEFT OUTER JOIN DICT_MB_GROUP_SPECIMEN ON DICT_COLL_MATERIALS.SPM_GROUP_ID = DICT_MB_GROUP_SPECIMEN.SPM_GROUP_ID
 LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
 LEFT OUTER JOIN USERS ON MB_ACTIONS.ACTIONUSER = USERS.USERNAME
 WHERE MB_REQUESTS.MBREQNUMBER=@PROTOCOLNUM AND REQUESTS.ACCESSNUMBER=@ACCESSNUMBER
 AND MB_ACTIONS.ACTIONTYPE = @STATUS
 ORDER BY MB_ACTIONS.ACTIONDATE DESC";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PROTOCOLNUM", SqlDbType.VarChar, objMBReportM.PROTOCOLNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ACCESSNUMBER", SqlDbType.VarChar, objMBReportM.ACCESSNUMBER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@STATUS", SqlDbType.VarChar, objMBReportM.STATUS));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];

                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, ds, dt);
            }
        }
        // Get_Patnum_Roundup
        // INSERT TUBE
        public DataTable Get_ReportFormat(MBReportM objMBReportM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 PROTOCOLID
,PROTOCOLCODE
,PROTOCOLCREDATE
,PROTOCOLTEXT
,REPORTFORMAT
 FROM DICT_MB_PROTOCOLS WHERE PROTOCOLCODE=@PROTOCOLCODE";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PROTOCOLCODE", SqlDbType.VarChar, objMBReportM.PROTOCOLCODE));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];

                return dt;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, ds, dt);
            }
        }

    }
}
