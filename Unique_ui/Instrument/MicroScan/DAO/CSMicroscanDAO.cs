using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using UniquePro.Common;
using UNIQUE.Instrument.MicroScan.Entites;
using System.Collections.Generic;
using UniquePro.Entities.Common;

namespace UNIQUE.Instrument.MicroScan
{
    public class CSMicroscanDAO
    {
        string Connectionstr = ConfigurationManager.ConnectionStrings["MICROSCANDB"].ToString();
        public SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MICROSCANDB"].ToString());

        private String Constr = "";
        private SqlTransaction trans;
        private DBFactory dbFactory;

        public SqlConnection Connect()
        {
            try
            {
                conn = new SqlConnection(Connectionstr);

                if (conn.State == System.Data.ConnectionState.Open) conn.Close(); conn.Open();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return conn;
        }

        public CSMicroscanDAO()
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
        public DataTable GetDICT()
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT * FROM MCDB";

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, null);

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
        // Get ResultControl
        public DataTable Get_ResultControl()
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 
 PATNUM
,NAME
,LASTNAME
,RECEIVEDATE
,SPMNUMBER 
,TUBEID
 FROM TUBE WHERE TUBESTATUS=0";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                //sqlParamsIn.Add(dbFactory.CreateParameterInput("@SPMNUMBER", SqlDbType.VarChar, objTerminalGETM.SPMNUMBER));
                //sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));

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
        public DataTable Get_TUBERESULT(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT TUBERESULT FROM TUBE WHERE PATNUM=@PATNUM AND SPMNUMBER=@SPMNUMBER";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SPMNUMBER", SqlDbType.VarChar, objTerminalGETM.SPMNUMBER));

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

        public DataTable GetUserAddminUNI()
        {
            DataTable dt = new DataTable();
            SqlConnection conn;

            conn = new SqlConnection(Connectionstr);
            if (conn.State == System.Data.ConnectionState.Open) conn.Close(); conn.Open();

            string sql = @" SELECT * FROM MCDB";

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            cmd.ExecuteNonQuery();
            adap.Fill(dt);

            return dt;
        }

//        public DataTable SaveDatabaseSetting()
//        {
//            try
//            {
//                SqlCommand sqlCmd;
//                string errMsg = "";


//                if (conn.State == ConnectionState.Open) conn.Close();
//                conn.Open();
//                trans = conn.BeginTransaction();

//                String sql = @" INSERT INTO MCDB
//(
// PATID,
// LASTNAME
//)
//VALUES(
//1,
//NAME )";
//                sqlCmd = new SqlCommand(sql.ToString());
//                errMsg = ExecuteData(sqlCmd);
//                if (errMsg != "")
//                {
//                    RollBackTrans();
//                    return errMsg;
//                }

//                CommitTrans();

//                return "";
//            }
//            catch (Exception ex)
//            {
//                RollBackTrans();
//                return ex.ToString();
//            }
//        }
        public string ExecuteData(SqlCommand sqlCmd)
        {
            sqlCmd.Connection = conn;
            string s = sqlCmd.CommandText;
            sqlCmd.Transaction = trans;
            try
            {
                int rowEffect = sqlCmd.ExecuteNonQuery();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public void RollBackTrans()
        {
            if (trans != null)
            {
                trans.Rollback();
                trans = null;
                conn.Close();
            }
        }
        public void CommitTrans()
        {
            trans.Commit();
            trans = null;
            conn.Close();
        }
        // INSERT TUBE
        public Terminal_GETM SaveTUBE(Terminal_GETM objTerminalGETM)
        {
            try
            {
                if (HaveTUBERESULT(objTerminalGETM) == true)
                {
                    return UpdateTUBERESULT(objTerminalGETM);
                }
                else
                {
                    return InsertTUBERESULT(objTerminalGETM);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Boolean HaveTUBERESULT(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT SPMNUMBER,PATNUM FROM TUBE WHERE SPMNUMBER=@SPMNUMBER AND PATNUM=@PATNUM";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SPMNUMBER", SqlDbType.VarChar, objTerminalGETM.SPMNUMBER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        private Terminal_GETM InsertTUBERESULT(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                string sql = @"INSERT 
INTO TUBE 
(SPMNUMBER
,PATNUM
,NAME
,LASTNAME
,TUBESTATUS
,RECEIVEDATE
,LOGDATE
,TUBERESULT) 
VALUES 
(
@SPMNUMBER,
@PATNUM,
@NAME,
@LASTNAME,
@TUBESTATUS,
getdate(),
getdate(),
@TUBERESULT ) SELECT CAST(scope_identity() AS int) ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SPMNUMBER", SqlDbType.VarChar, objTerminalGETM.SPMNUMBER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@NAME", SqlDbType.VarChar, objTerminalGETM.NAME));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@LASTNAME", SqlDbType.VarChar, objTerminalGETM.LASTNAME));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBESTATUS", SqlDbType.VarChar, objTerminalGETM.TUBESTATUS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBERESULT", SqlDbType.VarBinary, objTerminalGETM.TUBE_RESULT));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];

                objTerminalGETM.TUBEID = dt.Rows[0][0].ToString();

                return objTerminalGETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        private Terminal_GETM UpdateTUBERESULT(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
TUBE SET 
TUBERESULT=@TUBERESULT ,
LOGDATE= getdate()
WHERE SPMNUMBER=@SPMNUMBER AND PATNUM = @PATNUM ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SPMNUMBER", SqlDbType.VarChar, objTerminalGETM.SPMNUMBER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBERESULT", SqlDbType.VarBinary, objTerminalGETM.TUBE_RESULT));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalGETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }

        // UPDATE TUBE
        public Terminal_GETM UPDATE_TUBE(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return Update_TUBERE_Process(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Terminal_GETM Update_TUBERE_Process(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
TUBE SET 
TUBERESULT=@TUBERESULT ,
LOGDATE= getdate()
WHERE SPMNUMBER=@SPMNUMBER AND PATNUM = @PATNUM ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SPMNUMBER", SqlDbType.VarChar, objTerminalGETM.SPMNUMBER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBERESULT", SqlDbType.VarBinary, objTerminalGETM.TUBE_RESULT));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalGETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        public DataTable Get_PendingRequests()
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT DISTINCT
 JOBS.JOBID
,JOBS.SUBREQUESTID
,JOBS.JOBPRIORITY
,JOBS.JOBSTATUS
,JOBS.JOBTYPE
,JOBS.JOBNAME
,JOBS.JOBDATE
,MB_REQUESTS.REQUESTID
,MB_REQUESTS.MBREQNUMBER
,REQUESTS.ACCESSNUMBER
,PATIENTS.PATID
,PATIENTS.NAME
,PATIENTS.LASTNAME
,PATIENTS.PATNUMBER
 FROM JOBS
 LEFT OUTER JOIN MB_REQUESTS ON JOBS.SUBREQUESTID = MB_REQUESTS.MBREQUESTID
 LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID
 LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
 WHERE JOBS.JOBSTATUS ='0'";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

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
        public DataTable Get_CompleteRequests()
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 TUBEID
,SPMNUMBER
,PATNUM
,COLONYID
,COLONYNUMBER
 FROM TUBE
 WHERE TUBESTATUS='1'";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

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


        // UnMatch with Protocol
        public Terminal_GETM MatchProtocol_IN_DICT(Terminal_GETM objTerminalGETM)
        {
            try
            {
                if (HaveProtocol(objTerminalGETM) == true)
                {
                    return UpdateProtocol_with_result(objTerminalGETM);
                }
                else
                {
                    return InsertProtocol_with_result(objTerminalGETM);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Boolean HaveProtocol(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT SPMNUMBER,PATNUM FROM TUBE WHERE SPMNUMBER=@SPMNUMBER AND PATNUM=@PATNUM";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SPMNUMBER", SqlDbType.VarChar, objTerminalGETM.SPMNUMBER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        private Terminal_GETM InsertProtocol_with_result(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                string sql = @"INSERT 
INTO TUBE 
(SPMNUMBER
,PATNUM
,NAME
,LASTNAME
,TUBESTATUS
,RECEIVEDATE
,LOGDATE
,TUBERESULT) 
VALUES 
(
@SPMNUMBER,
@PATNUM,
@NAME,
@LASTNAME,
@TUBESTATUS,
getdate(),
getdate(),
@TUBERESULT ) SELECT CAST(scope_identity() AS int) ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SPMNUMBER", SqlDbType.VarChar, objTerminalGETM.SPMNUMBER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@NAME", SqlDbType.VarChar, objTerminalGETM.NAME));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@LASTNAME", SqlDbType.VarChar, objTerminalGETM.LASTNAME));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBESTATUS", SqlDbType.VarChar, objTerminalGETM.TUBESTATUS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBERESULT", SqlDbType.VarBinary, objTerminalGETM.TUBE_RESULT));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];

                objTerminalGETM.TUBEID = dt.Rows[0][0].ToString();

                return objTerminalGETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        private Terminal_GETM UpdateProtocol_with_result(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
TUBE SET 
TUBERESULT=@TUBERESULT ,
LOGDATE= getdate()
WHERE SPMNUMBER=@SPMNUMBER AND PATNUM = @PATNUM ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SPMNUMBER", SqlDbType.VarChar, objTerminalGETM.SPMNUMBER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBERESULT", SqlDbType.VarBinary, objTerminalGETM.TUBE_RESULT));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalGETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        // End

        // Purge database
        public Terminal_GETM PurgeDataProcess(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataTable dt = null;

            try
            {
                String sql = @"DELETE FROM TUBE ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalGETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        // Step 1
        // GET SUBREQUESTID FROM PROTOCOLNUM
        public DataTable GetSUBREQUESTID(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 MB_REQUESTS.MBREQUESTID AS 'SUBREQUESTID'
,REQUESTS.ACCESSNUMBER
,MB_REQUESTS.PROTOCOLID
,MB_REQUESTS.REQUESTID
,MB_REQUESTS.MBREQNUMBER
,SUBREQMB_BATTERIES.COLONYID
,SUBREQMB_BATTERIES.BATTERYID
,SUBREQMB_BATTERIES.ISOLATNUMBER
,PATIENTS.PATNUMBER
,PATIENTS.NAME
,PATIENTS.LASTNAME
,REQUESTS.RECEIVEDDATE
,SUBREQMB_COLONIES.COLONYNUMBER

 FROM MB_REQUESTS
 LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID
 LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
 LEFT OUTER JOIN SUBREQMB_BATTERIES ON MB_REQUESTS.MBREQUESTID = SUBREQMB_BATTERIES.SUBREQUESTID
 LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_BATTERIES.COLONYID = SUBREQMB_COLONIES.COLONYID
 WHERE MBREQNUMBER=@PROTOCOLNUM ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PROTOCOLNUM", SqlDbType.VarChar, objTerminalGETM.PROTOCOLNUM));

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

        // GET SUBREQUESTID & COLONY FROM PROTOCOLNUM AND COLONYID
        public DataTable GetSUBREQUESTID_Multicolony(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 MB_REQUESTS.MBREQUESTID AS 'SUBREQUESTID'
,REQUESTS.ACCESSNUMBER
,MB_REQUESTS.PROTOCOLID
,MB_REQUESTS.REQUESTID
,MB_REQUESTS.MBREQNUMBER
,SUBREQMB_BATTERIES.COLONYID
,SUBREQMB_BATTERIES.BATTERYID
,SUBREQMB_BATTERIES.ISOLATNUMBER
,PATIENTS.PATNUMBER
,PATIENTS.NAME
,PATIENTS.LASTNAME
,REQUESTS.RECEIVEDDATE
,SUBREQMB_COLONIES.COLONYNUMBER

 FROM MB_REQUESTS
 LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID
 LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
 LEFT OUTER JOIN SUBREQMB_BATTERIES ON MB_REQUESTS.MBREQUESTID = SUBREQMB_BATTERIES.SUBREQUESTID
 LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_BATTERIES.COLONYID = SUBREQMB_COLONIES.COLONYID
 WHERE MBREQNUMBER=@PROTOCOLNUM AND SUBREQMB_BATTERIES.COLONYID=@COLONYID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PROTOCOLNUM", SqlDbType.VarChar, objTerminalGETM.PROTOCOLNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.VarChar, objTerminalGETM.COLONYID));

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

        // Step 1.2 Search Multi Colonies
        //
        public DataTable Search_Multicolonies(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 SUBREQMB_COLONIES.COLONYNUMBER AS 'CODE'
 ,DICT_MB_BATTERIES.BATTERYCODE AS 'TEXT'
 ,SUBREQMB_BATTERIES.COLONYID AS 'ID'

 FROM MB_REQUESTS
 LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID
 LEFT OUTER JOIN SUBREQMB_BATTERIES ON MB_REQUESTS.MBREQUESTID = SUBREQMB_BATTERIES.SUBREQUESTID
 LEFT OUTER JOIN DICT_MB_BATTERIES ON SUBREQMB_BATTERIES.BATTERYID = DICT_MB_BATTERIES.BATTERYID
 LEFT OUTER JOIN SUBREQMB_COLONIES ON SUBREQMB_BATTERIES.COLONYID = SUBREQMB_COLONIES.COLONYID
 WHERE MBREQNUMBER=@PROTOCOLNUM ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PROTOCOLNUM", SqlDbType.VarChar, objTerminalGETM.PROTOCOLNUM));

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
        // Step 2
        public DataTable Get_DICT_MB_ORGANISMS(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 ORGANISMID
,ORGANISMCODE
,ORGANISMNAME
,METHODMBID
,SENSITIVITYID
,ORGANISMSFAMID
 FROM DICT_MB_ORGANISMS
 WHERE ORGANISMCODE =@ORGANISMSCODE";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ORGANISMSCODE", SqlDbType.VarChar, objTerminalGETM.ORGANISMCODE));

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
        // Step 2.1 Get ORGANISMINDEX
        // GET_ORGANISMINDEX
        public DataTable GET_ORGANISMINDEX(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 COLONYID
,ORGANISMID
,ORGANISMINDEX
,SUBREQUESTID
,SUBREQMBORGID
 FROM SUBREQMB_ORGANISMS
 WHERE SUBREQUESTID=@SUBREQUESTID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                //sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.Int, objTerminalGETM.COLONYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.Int, objTerminalGETM.SUBREQUESTID));

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
        // GET FOR UPDATE
        public DataTable GET_ORGANISMINDEX_FOR_UPDATE(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 COLONYID
,ORGANISMID
,ORGANISMINDEX
,SUBREQUESTID
 FROM SUBREQMB_ORGANISMS
 WHERE COLONYID=@COLONYID AND SUBREQUESTID=@SUBREQUESTID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.Int, objTerminalGETM.COLONYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.Int, objTerminalGETM.SUBREQUESTID));
                //sqlParamsIn.Add(dbFactory.CreateParameterInput("@ORGANISMID", SqlDbType.Int, objTerminalGETM.ORGANISMID));

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
        // Step 3.1 Update SUBREQMB_ORGANISMS
        public Terminal_GETM Update_SUBREQMB_ORGANISMS(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
SUBREQMB_ORGANISMS SET 
IDENTDATE=getdate(),
LOGDATE= getdate()
WHERE COLONYID=@COLONYID AND ORGANISMID=@ORGANISMID ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.Int, objTerminalGETM.COLONYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ORGANISMID", SqlDbType.Int, objTerminalGETM.ORGANISMID));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalGETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        // Step 3.2 Save result in SUBREQMB_ORGANISMS
        public Terminal_SETM INSERT_SUBREQMB_ORGANISMS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                    return Insert_IN_SUBREQMB_ORGANISMS(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
//        private Boolean Have_COLONYID_AND_ORGANISMID_IN_SUBREQMB_ORGANISMS(Terminal_GETM objTerminalSETM)
//        {
//            dbFactory.SetDBConnString(Constr);
//            SqlConnection objConn = dbFactory.GetDBConnection();
//            DataSet ds = null;
//            DataTable dt = null;

//            try
//            {
//                String sql = @"SELECT 
// COLONYID
//,ORGANISMID
//,ORGANISMINDEX
//,SUBREQUESTID
// FROM SUBREQMB_ORGANISMS
// WHERE COLONYID=@COLONYID AND SUBREQUESTID=@SUBREQUESTID AND ORGANISMID =@ORGANISMID";

//                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

//                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.Int, objTerminalGETM.COLONYID));
//                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.Int, objTerminalGETM.SUBREQUESTID));
//                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ORGANISMID", SqlDbType.Int, objTerminalGETM.ORGANISMID));

//                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

//                dt = ds.Tables[0];

//                if (dt.Rows.Count == 0)
//                {
//                    return false;
//                }
//                else
//                {
//                    return true;
//                }

//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                dbFactory.CloseConnection(objConn, null, dt);
//            }
//        }
        private Terminal_SETM Insert_IN_SUBREQMB_ORGANISMS(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                string sql = @"INSERT 
INTO SUBREQMB_ORGANISMS 
(COLONYID
,ORGANISMID
,ORGANISMINDEX
,CREATEUSER
,CREATIONDATE
,IDENTDATE
,IDENTUSER
,COMMENTS
,CONSOLIDATIONSTATUS
,NOTPRINTABLE
,LOGUSERID
,LOGDATE
,SUBREQUESTID) 
VALUES 
(
@COLONYID,
@ORGANISMID,
@ORGANISMINDEX,
@CREATEUSER,
getdate(),
getdate(),
@IDENTUSER,
@COMMENTS,
@CONSOLIDATIONSTATUS,
@NOTPRINTABLE,
@LOGUSERID,
getdate(),
@SUBREQUESTID ) SELECT CAST(scope_identity() AS int) ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.Int, objTerminalSETM.COLONYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ORGANISMID", SqlDbType.Int, objTerminalSETM.ORGANISMID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ORGANISMINDEX", SqlDbType.VarChar, objTerminalSETM.ORGANISMINDEX));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@CREATEUSER", SqlDbType.VarChar, objTerminalSETM.CREATEUSER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@IDENTUSER", SqlDbType.VarChar, objTerminalSETM.IDENTUSER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COMMENTS", SqlDbType.VarChar, objTerminalSETM.COMMENTS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@CONSOLIDATIONSTATUS", SqlDbType.VarChar, objTerminalSETM.CONSOLIDATESTATUS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@NOTPRINTABLE", SqlDbType.VarChar, objTerminalSETM.NOTPRINTABLE));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@LOGUSERID", SqlDbType.VarChar, objTerminalSETM.LogUserID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.Int, objTerminalSETM.SUBREQUESTID));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];
                objTerminalSETM.SUBREQMBORGID = dt.Rows[0][0].ToString();

                return objTerminalSETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        private Terminal_GETM Update_IN_SUBREQMB_ORGANISMS(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
SUBREQMB_ORGANISMS SET 
IDENTDATE=getdate(),
LOGDATE= getdate()
WHERE COLONYID=@COLONYID AND ORGANISMID=@ORGANISMID ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.Int, objTerminalGETM.COLONYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ORGANISMID", SqlDbType.Int, objTerminalGETM.ORGANISMID));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalGETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        // Step 4 Search SUBREQMB_SENSITIVITIES
        public DataTable GET_SUBREQMBSENSITIVITYID(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 SUBREQMBSENSITIVITYID
,SENSITIVITYID
,SUBREQUESTID
,COLONYID
 FROM SUBREQMB_SENSITIVITIES
 WHERE COLONYID=@COLONYID AND SUBREQUESTID=@SUBREQUESTID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.Int, objTerminalGETM.COLONYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.Int, objTerminalGETM.SUBREQUESTID));

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
        // Step 4.1 Search Sensitivity panel by name MICROSCAN
        public DataTable GET_SENSITIVITYID(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 SENSITIVITYID
,SENSITIVITYCODE
,SENSITIVITYNAME
 FROM DICT_MB_SENSITIVITIES_PANEL
 WHERE SENSITIVITYCODE=@SENSITIVITYID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SENSITIVITYID", SqlDbType.VarChar, objTerminalGETM.BATTERIES_CODE));

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
        // Step 4.2 Insert SUBREQMB_SENSITIVITIES
        public Terminal_SETM INSERT_SUBREQMB_SENSITIVITIES(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return Insert_IN_SUBREQMB_SENSITIVITIES(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Terminal_SETM Insert_IN_SUBREQMB_SENSITIVITIES(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                string sql = @"INSERT 
INTO SUBREQMB_SENSITIVITIES 
(SENSITIVITYID
,SUBREQUESTID
,COLONYID
,CREATEUSER
,CREATIONDATE
,COMMENTS
,CONSOLIDATIONSTATUS
,LOGUSERID
,LOGDATE
,NOTPRINTABLE) 
VALUES 
(
@SENSITIVITYID,
@SUBREQUESTID,
@COLONYID,
@CREATEUSER,
getdate(),
@COMMENTS,
@CONSOLIDATIONSTATUS,
@LOGUSERID,
getdate(),
@NOTPRINTABLE ) SELECT CAST(scope_identity() AS int) ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SENSITIVITYID", SqlDbType.Int, objTerminalSETM.SENSITIVITYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.Int, objTerminalSETM.SUBREQUESTID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.Int, objTerminalSETM.COLONYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@CREATEUSER", SqlDbType.VarChar, objTerminalSETM.CREATEUSER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COMMENTS", SqlDbType.VarChar, objTerminalSETM.COMMENTS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@CONSOLIDATIONSTATUS", SqlDbType.VarChar, objTerminalSETM.CONSOLIDATESTATUS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@LOGUSERID", SqlDbType.VarChar, objTerminalSETM.LogUserID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@NOTPRINTABLE", SqlDbType.VarChar, objTerminalSETM.NOTPRINTABLE));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];
                objTerminalSETM.SUBREQMBSENSITIVITYID = dt.Rows[0][0].ToString();

                return objTerminalSETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }


        // Step 5.1 Search in DICT_MB_ANTIBIOTICS
        // DICT_MB_ANTIBIOTICS
        public DataTable GET_ANTIBIOTICID(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 ANTIBIOTICID
,ANTIBIOTICCODE
 FROM DICT_MB_ANTIBIOTICS
 WHERE ANTIBIOTICCODE=@ANTIBIOTICCODE";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ANTIBIOTICCODE", SqlDbType.VarChar, objTerminalGETM.ANTIBIOTICCODE));

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
        // Step 5.2 Insert in SUBREQMB_ANTIBIOTICS
        // INSERT_SUBREQMB_ANTIBIOTICS

        public Terminal_SETM INSERT_SUBREQMB_ANTIBIOTICS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return Insert_IN_SUBREQMB_ANTIBIOTICS(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //private Boolean Have_SAME_IN_SUBREQMB_ANTIBIOTICS(TerminalM objTerminalGETM)
//        {
//            dbFactory.SetDBConnString(Constr);
//            SqlConnection objConn = dbFactory.GetDBConnection();
//            DataSet ds = null;
//            DataTable dt = null;

//            try
//            {
//                String sql = @"SELECT 
// SUBREQMB_ANTIBIOTICS.SUBREQMBANTIBIOTICID
//,SUBREQMB_ANTIBIOTICS.SUBREQMBORGID
//,SUBREQMB_ANTIBIOTICS.ANTIBIOTICID
//,SUBREQMB_ANTIBIOTICS.SUBREQUESTID
//,SUBREQMB_ANTIBIOTICS.SUBREQMBSENSITIVITYID
//,SUBREQMB_BATTERIES.BATTERYID
//,SUBREQMB_BATTERIES.COLONYID
// FROM SUBREQMB_ANTIBIOTICS
// LEFT OUTER JOIN SUBREQMB_BATTERIES ON SUBREQMB_ANTIBIOTICS.SUBREQUESTID = SUBREQMB_BATTERIES.SUBREQUESTID
// WHERE SUBREQMB_ANTIBIOTICS.SUBREQUESTID =@SUBREQUESTID AND SUBREQMB_ANTIBIOTICS.ANTIBIOTICID=@ANTIBIOTICID";

//                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

//                //sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.Int, objTerminalGETM.COLONYID));
//                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ANTIBIOTICID", SqlDbType.Int, objTerminalGETM.ANTIBIOTICID));
//                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.Int, objTerminalGETM.SUBREQUESTID));
//                //sqlParamsIn.Add(dbFactory.CreateParameterInput("@ORGANISMID", SqlDbType.Int, objTerminalGETM.ORGANISMID));

//                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

//                dt = ds.Tables[0];

//                if (dt.Rows.Count == 0)
//                {
//                    return false;
//                }
//                else
//                {
//                    return true;
//                }

//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//            finally
//            {
//                dbFactory.CloseConnection(objConn, null, dt);
//            }
//        }
        private Terminal_SETM Insert_IN_SUBREQMB_ANTIBIOTICS(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                string sql = @"INSERT 
INTO SUBREQMB_ANTIBIOTICS 
(
 METHODMBID
,SUBREQMBORGID
,ANTIBIOTICID
,SUBREQUESTID
,SUBREQMBSENSITIVITYID
,CREATEUSER
,CREATIONDATE
,VALREQUESTED
,CONSOLIDATIONSTATUS
,RESULT
,RESULTVALUE
,UNITS
,RESUPDDATE
,UPDATERULEID
,NOTPRINTABLE
,DELTACHECK
,LOGUSERID
,LOGDATE
,MICID) 
VALUES 
(
@METHODMBID,
@SUBREQMBORGID,
@ANTIBIOTICID,
@SUBREQUESTID,
@SUBREQMBSENSITIVITYID,
@CREATEUSER,
getdate(),
@VALREQUESTED,
@CONSOLIDATIONSTATUS,
@RESULT,
@RESULTVALUE,
@UNITS,
getdate(),
@UPDATERULEID,
@NOTPRINTABLE,
@DELTACHECK,
@LOGUSERID,
getdate(),
@MICID
) SELECT CAST(scope_identity() AS int) ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@METHODMBID", SqlDbType.VarChar, objTerminalSETM.MEDTHODMBID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQMBORGID", SqlDbType.VarChar, objTerminalSETM.SUBREQMBORGID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ANTIBIOTICID", SqlDbType.VarChar, objTerminalSETM.ANTIBIOTICID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.VarChar, objTerminalSETM.SUBREQUESTID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQMBSENSITIVITYID", SqlDbType.VarChar, objTerminalSETM.SUBREQMBSENSITIVITYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@CREATEUSER", SqlDbType.VarChar, objTerminalSETM.CREATEUSER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@VALREQUESTED", SqlDbType.VarChar, objTerminalSETM.VALREQUESTED));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@CONSOLIDATIONSTATUS", SqlDbType.VarChar, objTerminalSETM.CONSOLIDATESTATUS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@RESULT", SqlDbType.VarChar, objTerminalSETM.ANTIBIOTIC_CLSI_SYS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@RESULTVALUE", SqlDbType.VarChar, objTerminalSETM.ANTIBIOTIC_RESULT_VALUE));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@UNITS", SqlDbType.VarChar, objTerminalSETM.UNITS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@UPDATERULEID", SqlDbType.VarChar, objTerminalSETM.UPDATERULEID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@NOTPRINTABLE", SqlDbType.VarChar, objTerminalSETM.NOTPRINTABLE));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@DELTACHECK", SqlDbType.VarChar, objTerminalSETM.DELTACHECK));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@LOGUSERID", SqlDbType.VarChar, objTerminalSETM.LogUserID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@MICID", SqlDbType.VarChar, objTerminalSETM.MICID));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];
                //objTerminalGETM.SUBREQMBANTIBIOTICID = dt.Rows[0][0].ToString();

                return objTerminalSETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        //private TerminalM Update_IN_SUBREQMB_ANTIBIOTICS(TerminalM objTerminalGETM)
        //        {
        //            dbFactory.SetDBConnString(Constr);
        //            SqlConnection objConn = dbFactory.GetDBConnection();
        //            DataTable dt = null;

        //            try
        //            {
        //                String sql = @"UPDATE 
        //SUBREQMB_ORGANISMS SET 
        //IDENTDATE=getdate(),
        //LOGDATE= getdate()
        //WHERE COLONYID=@COLONYID AND ORGANISMID=@ORGANISMID ";

        //                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

        //                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.Int, objTerminalGETM.COLONYID));
        //                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ORGANISMID", SqlDbType.Int, objTerminalGETM.ORGANISMID));

        //                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

        //                return objTerminalGETM;

        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(ex.Message);
        //            }
        //            finally
        //            {
        //                dbFactory.CloseConnection(objConn, null, dt);
        //            }
        //        }

        // Step Ater Matching 
        // Step 1
        public Terminal_SETM Update_TUBE_After_UnMatching(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
TUBE SET 
SPMNUMBER=@PROTOCOLNUM,
PATNUM=@PATNUM,
NAME=@NAME,
LASTNAME=@LASTNAME,
TUBESTATUS=@TUBESTATUS,
LOGDATE= getdate() 
WHERE TUBEID=@TUBEID ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PROTOCOLNUM", SqlDbType.VarChar, objTerminalSETM.PROTOCOLNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalSETM.PATNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@NAME", SqlDbType.VarChar, objTerminalSETM.PATNAME));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@LASTNAME", SqlDbType.VarChar, objTerminalSETM.PATLASTNAME));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBESTATUS", SqlDbType.VarChar, objTerminalSETM.TUBESTATUS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBEID", SqlDbType.VarChar, objTerminalSETM.TUBEID));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalSETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        // Step After Matching
        // Step 2 
        public Terminal_SETM Update_TUBERESULT(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return Update_TUBERESULT_Process(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Terminal_SETM Update_TUBERESULT_Process(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
TUBE SET 
TUBERESULT=@TUBERESULT ,
COLONYID=@COLONYID,
COLONYNUMBER=@COLONYNUMBER,
LOGDATE= getdate()
WHERE TUBEID=@TUBEID ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBEID", SqlDbType.VarChar, objTerminalSETM.TUBEID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.VarChar, objTerminalSETM.COLONYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYNUMBER", SqlDbType.VarChar, objTerminalSETM.COLONYNUMBER));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBERESULT", SqlDbType.VarBinary, objTerminalSETM.TUBE_RESULT));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalSETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        // Auto Step  Get protocol and Patient match
        public DataTable GET_PATIENT_AND_PROTOCOL(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 MB_REQUESTS.MBREQUESTID
,MB_REQUESTS.REQUESTID
,MB_REQUESTS.MBREQNUMBER
,REQUESTS.ACCESSNUMBER
,REQUESTS.PATID
,PATIENTS.PATNUMBER
 FROM MB_REQUESTS
 LEFT OUTER JOIN REQUESTS ON MB_REQUESTS.REQUESTID = REQUESTS.REQUESTID
 LEFT OUTER JOIN PATIENTS ON REQUESTS.PATID = PATIENTS.PATID
 WHERE MB_REQUESTS.MBREQNUMBER=@PROTOCOL AND PATIENTS.PATNUMBER=@PATNUM ";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PROTOCOL", SqlDbType.VarChar, objTerminalGETM.PROTOCOLNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));

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

        // AUto Step Update TUBE in TUBESTATUS

        public Terminal_SETM Update_TUBESTATUS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                if (HaveTUBEID_FOR_UPDATE_STATUS(objTerminalSETM) == true)
                {
                    return UpdateTUBESTATUS(objTerminalSETM);
                }
                else
                {
                    return objTerminalSETM;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Boolean HaveTUBEID_FOR_UPDATE_STATUS(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT TUBEID FROM TUBE WHERE TUBEID=@TUBEID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBEID", SqlDbType.VarChar, objTerminalSETM.TUBEID));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        private Terminal_SETM UpdateTUBESTATUS(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
TUBE SET 
TUBESTATUS=@TUBESTATUS,
COLONYID=@COLONYID,
COLONYNUMBER=@COLONYNUMBER,
LOGDATE= getdate()
WHERE TUBEID=@TUBEID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBESTATUS", SqlDbType.VarChar, objTerminalSETM.TUBESTATUS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@TUBEID", SqlDbType.VarChar, objTerminalSETM.TUBEID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.VarChar, objTerminalSETM.COLONYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYNUMBER", SqlDbType.VarChar, objTerminalSETM.COLONYNUMBER));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalSETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }

        // Step After Automatch complete
        public DataTable GET_JOBID(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 JOBID,
 SUBREQUESTID,
 JOBNAME 
 FROM JOBS
 WHERE SUBREQUESTID=@SUBREQUESTID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.VarChar, objTerminalGETM.SUBREQUESTID));

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

        // Update JOBS at JOBSTATUS
        public Terminal_SETM Update_JOBSTATUS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                if (HaveUpdate_JOBSTATUS(objTerminalSETM) == true)
                {
                    return Update_JOBSTATUS_Process(objTerminalSETM);
                }
                else
                {
                    return objTerminalSETM;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Boolean HaveUpdate_JOBSTATUS(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT JOBID FROM JOBS WHERE JOBID=@JOBID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@JOBID", SqlDbType.VarChar, objTerminalSETM.JOBID));

                ds = dbFactory.ExecuteQuerySQL(objConn, sql, sqlParamsIn.ToArray());

                dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        private Terminal_SETM Update_JOBSTATUS_Process(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
JOBS SET 
JOBSTATUS=@JOBSTATUS,
LOGUSERID=@LOGUSERID,
LOGDATE= getdate()

WHERE JOBID=@JOBID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@JOBSTATUS", SqlDbType.VarChar, objTerminalSETM.JOBSTATUS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@LOGUSERID", SqlDbType.VarChar, objTerminalSETM.LogUserID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@JOBID", SqlDbType.VarChar, objTerminalSETM.JOBID));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalSETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        // Get Colony name
        public DataTable GET_COLONYNAME(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 COLONYID, COLONYNUMBER 
 FROM SUBREQMB_COLONIES
 WHERE COLONYID=@COLONYID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.VarChar, objTerminalGETM.COLONYID));

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
        // GET DATA IN MICROSCANDB
        public DataTable GET_PROTOCOL_EXITS(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Connectionstr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 TUBEID,
 SPMNUMBER, 
 PATNUM 
 FROM TUBE
 WHERE SPMNUMBER=@PROTOCOLNUM AND PATNUM=@PATNUM";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PROTOCOLNUM", SqlDbType.VarChar, objTerminalGETM.PROTOCOLNUM));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@PATNUM", SqlDbType.VarChar, objTerminalGETM.PATNUM));

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
        // GET 
        public DataTable GET_SUBREQMBORGID(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 SUBREQMBORGID,
 COLONYID,
 SUBREQUESTID 
 FROM SUBREQMB_ORGANISMS
 WHERE SUBREQUESTID=@SUBREQUESTID AND COLONYID=@COLONYID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.VarChar, objTerminalGETM.SUBREQUESTID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.VarChar, objTerminalGETM.COLONYID));

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

        // UPDATE SUBREQMB_ORGANISMS
        public Terminal_SETM UPDATE_SUBREQMB_ORGANISMS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return Update_SUBREQMB_ORGANISMS(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Terminal_SETM Update_SUBREQMB_ORGANISMS(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
SUBREQMB_ORGANISMS SET
ORGANISMID=@ORGANISMID,
LOGDATE= getdate()
WHERE SUBREQMBORGID=@SUBREQMBORGID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQMBORGID", SqlDbType.VarChar, objTerminalSETM.SUBREQMBORGID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ORGANISMID", SqlDbType.VarChar, objTerminalSETM.ORGANISMID));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalSETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        // GET FOR UPDATE in RESULT_SUBREQMB_ANTIBIOTICS
        public DataTable GET_RESULT_SUBREQMB_ANTIBIOTICS(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 SUBREQMBANTIBIOTICID
,SUBREQMBORGID
,ANTIBIOTICID
,SUBREQUESTID
,SUBREQMBSENSITIVITYID 
 FROM SUBREQMB_ANTIBIOTICS
 WHERE SUBREQMBORGID=@SUBREQMBORGID AND ANTIBIOTICID=@ANTIBIOTICID AND SUBREQUESTID=@SUBREQUESTID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQMBORGID", SqlDbType.VarChar, objTerminalGETM.SUBREQMBORGID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ANTIBIOTICID", SqlDbType.VarChar, objTerminalGETM.ANTIBIOTICID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.VarChar, objTerminalGETM.SUBREQUESTID));

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
        // UPDATE 
        public Terminal_SETM UPDATE_SUBREQMB_ANTIBIOTICS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return UPDATE_IN_SUBREQMB_ANTIBIOTICS(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Terminal_SETM UPDATE_IN_SUBREQMB_ANTIBIOTICS(Terminal_SETM objTerminalSETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataTable dt = null;

            try
            {
                String sql = @"UPDATE 
SUBREQMB_ANTIBIOTICS SET 
RESULT=@RESULT,
RESULTVALUE=@RESULTVALUE,
RESUPDDATE=getdate(),
LOGDATE= getdate() 
WHERE SUBREQMBORGID=@SUBREQMBORGID AND ANTIBIOTICID=@ANTIBIOTICID AND SUBREQUESTID=@SUBREQUESTID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQMBORGID", SqlDbType.VarChar, objTerminalSETM.SUBREQMBORGID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@ANTIBIOTICID", SqlDbType.VarChar, objTerminalSETM.ANTIBIOTICID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.VarChar, objTerminalSETM.SUBREQUESTID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@RESULT", SqlDbType.VarChar, objTerminalSETM.ANTIBIOTIC_CLSI_SYS));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@RESULTVALUE", SqlDbType.VarChar, objTerminalSETM.ANTIBIOTIC_RESULT_VALUE));

                dbFactory.ExecuteNonQuerySQL(objConn, null, sql, sqlParamsIn.ToArray());

                return objTerminalSETM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbFactory.CloseConnection(objConn, null, dt);
            }
        }
        // GET RESULT 
        public DataTable GET_RESULT_SUBREQMB_SENSITIVITIES(Terminal_GETM objTerminalGETM)
        {
            dbFactory.SetDBConnString(Constr);
            SqlConnection objConn = dbFactory.GetDBConnection();
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                String sql = @"SELECT 
 SUBREQMBSENSITIVITYID
,SENSITIVITYID
,SUBREQUESTID
,COLONYID
  FROM SUBREQMB_SENSITIVITIES
  WHERE SUBREQUESTID=@SUBREQUESTID AND COLONYID=@COLONYID";

                List<SqlParameter> sqlParamsIn = new List<SqlParameter>();

                sqlParamsIn.Add(dbFactory.CreateParameterInput("@COLONYID", SqlDbType.VarChar, objTerminalGETM.COLONYID));
                sqlParamsIn.Add(dbFactory.CreateParameterInput("@SUBREQUESTID", SqlDbType.VarChar, objTerminalGETM.SUBREQUESTID));

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
