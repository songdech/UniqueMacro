using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNIQUE.Instrument.MicroScan.Entites;

namespace UNIQUE.Instrument.MicroScan.Controller
{
    public class MicroscanController
    {
        private CSMicroscanDAO objMicroscanDAO = null;

        public MicroscanController()
        {
            objMicroscanDAO = new CSMicroscanDAO();
        }

        public DataTable GetDICTBattery()
        {
            try
            {
                return objMicroscanDAO.GetDICT();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Get Get_Patnum_Roundup
        public DataTable Get_Result_TUBERESULT(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.Get_TUBERESULT(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Result Control
        public DataTable Get_ResultControl()
        {
            try
            {
                return objMicroscanDAO.Get_ResultControl();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Terminal_GETM SaveTUBE(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.SaveTUBE(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Terminal_GETM UPDATETUBE(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.UPDATE_TUBE(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable Get_PendingRequests()
        {
            try
            {
                return objMicroscanDAO.Get_PendingRequests();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable Get_CompleteRequests()
        {
            try
            {
                return objMicroscanDAO.Get_CompleteRequests();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // UnMatch with Protocol
        public Terminal_GETM MatchProtocol(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.MatchProtocol_IN_DICT(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // purge data in database
        public Terminal_GETM PurgeData(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.PurgeDataProcess(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step 1 UnMatch
        // GET SUBREQUESTID FROM PROTOCOL NUMBER
        public DataTable GetSUBREQUESTID(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GetSUBREQUESTID(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // GetSUBREQUESTID_Multicolony
        public DataTable GetSUBREQUESTID_Multicolony(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GetSUBREQUESTID_Multicolony(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Step 2 Get Data from DICT_MB_ORGANISMS
        public DataTable Get_Info_DICT_MB_ORGANISMS(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.Get_DICT_MB_ORGANISMS(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step 2.1 Get ORGANISMINDEX
        // GET_ORGANISMINDEX
        public DataTable GET_ORGANISMINDEX(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_ORGANISMINDEX(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // GET CHECK UPDATE
        public DataTable GET_ORGANISMINDEX_FOR_UPDATE(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_ORGANISMINDEX_FOR_UPDATE(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step 3.1 Update result ORGANISMS
        // UPDATE SUBREQMB_ORGANISMS
        public Terminal_GETM Update_SUBREQMB_ORGANISMS(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.Update_SUBREQMB_ORGANISMS(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step 3.2 Save result ORGANISMS FROM Microscan
        // INSERT_SUBREQMB_ORGANISMS
        public Terminal_SETM INSERT_SUBREQMB_ORGANISMS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return objMicroscanDAO.INSERT_SUBREQMB_ORGANISMS(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step 4 Search in SUBREQMB_SENSITIVITIES
        public DataTable GET_SUBREQMBSENSITIVITYID(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_SUBREQMBSENSITIVITYID(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step 4.1 Search Sensitivity panal in name
        public DataTable GET_SENSITIVITYID(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_SENSITIVITYID(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step 4.2 Insert in SUBREQMB_SENSITIVITIES
        // 
        public Terminal_SETM INSERT_SUBREQMB_SENSITIVITIES(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return objMicroscanDAO.INSERT_SUBREQMB_SENSITIVITIES(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Step 5.1 Search in DICT_MB_ANTIBIOTICS
        // DICT_MB_ANTIBIOTICS
        public DataTable GET_ANTIBIOTICID(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_ANTIBIOTICID(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step 5.2 INSERT ANTIBIOTIC in SUBREQMB_ANTIBIOTICS
        //
        public Terminal_SETM INSERT_SUBREQMB_ANTIBIOTICS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return objMicroscanDAO.INSERT_SUBREQMB_ANTIBIOTICS(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step 1.2 Search and Select COLONIES
        public DataTable Search_Multicolonies(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.Search_Multicolonies(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Step After Matching
        // Step 2
        // Update_After_UnMatching
        public Terminal_SETM Update_After_UnMatching(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return objMicroscanDAO.Update_TUBE_After_UnMatching(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step After Matching
        // Step 3 
        // Update TUBERESULT
        public Terminal_SETM Update_TUBERESULT(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return objMicroscanDAO.Update_TUBERESULT(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Auto Step
        public DataTable GET_PATIENT_AND_PROTOCOL(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_PATIENT_AND_PROTOCOL(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Auto Step Update TUBE
        public Terminal_SETM Update_Status_TUBE(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return objMicroscanDAO.Update_TUBESTATUS(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Step After AUtocomplete
        public DataTable GET_JOBID(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_JOBID(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Update JOBS
        public Terminal_SETM Update_Status_JOBS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return objMicroscanDAO.Update_JOBSTATUS(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Get Colony name from JOBS and Colonynumber
        //
        public DataTable GET_COLONYNAME(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_COLONYNAME(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // GET DATA IN MICROSCANDB
        public DataTable GET_PROTOCOL_EXITS(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_PROTOCOL_EXITS(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // GET SUBREQMBORGID
        public DataTable GET_SUBREQMBORGID(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_SUBREQMBORGID(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Terminal_SETM UPDATE_SUBREQMB_ORGANISMS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return objMicroscanDAO.UPDATE_SUBREQMB_ORGANISMS(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // GET RESULT SUBREQMB_ANTIBIOTICS
        public DataTable GET_RESULT_SUBREQMB_ANTIBIOTICS(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_RESULT_SUBREQMB_ANTIBIOTICS(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Update  SUBREQMB_ANTIBIOTICS
        public Terminal_SETM UPDATE_SUBREQMB_ANTIBIOTICS(Terminal_SETM objTerminalSETM)
        {
            try
            {
                return objMicroscanDAO.UPDATE_SUBREQMB_ANTIBIOTICS(objTerminalSETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET RESULT SUBREQMB_SENSITIVITY
        public DataTable GET_RESULT_SUBREQMB_SENSITIVITIES(Terminal_GETM objTerminalGETM)
        {
            try
            {
                return objMicroscanDAO.GET_RESULT_SUBREQMB_SENSITIVITIES(objTerminalGETM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
