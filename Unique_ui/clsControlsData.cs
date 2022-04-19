using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;

namespace UNIQUE
{
  
    class clsControlsData
    {
        internal static void CheckCurrentMudult()
        {
            
    
        }
        public static string currentForm { get; set; }


    

        public static int patnumlenght { get; set; }
        internal static void GetConfigurations(SqlConnection conn)
        {
            string sql = "SELECT PATNUMLENGHT FROM CONFIGURATIONS";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            System.Data.DataSet ds = new System.Data.DataSet();
            if (conn.State == System.Data.ConnectionState.Open) conn.Close(); conn.Open();
            ds.Clear();
            adap.Fill(ds, "config");
            if (ds.Tables["config"].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables["config"].Rows.Count; i++)
                {
                    patnumlenght = Convert.ToInt32(ds.Tables["config"].Rows[i]["PATNUMLENGHT"].ToString());
                }
            }
            
        }
    }

    public static class ControlStain
    {
        public static int stainID { get; set; }
        public static string stainCode { get; set; }
        public static string stainName { get; set; }

        public static int observationID { get; set; }
        public static int quantitativeID { get; set; }

    }

    public static class ControlCustomizeResult
    {
        public static string customizeResID { get; set; }
        public static string customizeResCode { get; set; }
        public static string customizeResText { get; set; }

        public static string customizeRes_List_ID { get; set; }
        public static string customizeRes_List_Description { get; set; }
        public static string customizeRes_List_Text { get; set; }
        public static string customizeRes_List_LOGUSERID { get; set; }

    }
    public static class ControlAgars
    {
        public static string AgarID { get; set; }
        public static string AgarCode { get; set; }
        public static string AgarName { get; set; }
        public static string Description { get; set; }
        public static DateTime AgarStartDate { get; set; }
    }
    public static class ControlProtocol
    {
        public static string ProtocolID { get; set; }
        public static string ProtocolCODE { get; set; }
        public static string ProtocolName { get; set; }
        public static string ProtocolComment { get; set; }
        public static DateTime ProtocolStartDate { get; set; }
        public static DateTime ProtocolEndDate { get; set; }

        public static string ReportFormat  {get; set;}
        public static int ProtocolSpecimenID { get; set; }
        public static int Protocol_SPM_ID { get; set; }
        public static string Protocol_SPM_Name { get; set; }

    }
    public static class ControlAntibiotic
    {
        public static string AntibioticID { get; set; }
        public static string AntibioticCode { get; set; }
        public static string AntibioticName { get; set; }
        public static DateTime AntibioticUseDate { get; set; }
    }
    public static class ControlAntibioticFam
    {
        public static string AntibioticFamID { get; set; }
        public static string AntibioticFamCode { get; set; }
        public static string AntibioticFamName { get; set; }
        public static DateTime AntibioticFamUseDate { get; set; }
        public static string ANTIBIOTIC_FAMS_ID { get; set; }

    }

    public static class ControlOrganismsFam
    {
        public static string OrganismsFamID { get; set; }
        public static string OrganismsFamCode { get; set; }
        public static string OrganismsFamName { get; set; }
        public static string Organisms_FAMS_ID { get; set; }

    }

    public static class ControlTestDictionary
    {
        public static string TestID { get; set; }
        public static string TestCode { get; set; }
        public static string TestName { get; set; }
        public static string TestUnit { get; set; }
        public static string TestWarningLow { get; set; }
        public static string TestWarningHi { get; set; }
        public static string TestCriticalLow { get; set; }
        public static string TestCriticalHi { get; set; }
        public static string TestPrint { get; set; }
        public static string TestSpecimentGrp { get; set; }
        public static string TestProtocol { get; set; }
        public static int TestProtocolID { get; set; }
        public static string COLLMATERIALID { get; set; }
        

    }
    public static class ControlLocation
    {
        public static string LocID { get; set; }
        public static string Loccode { get; set; }
        public static string Locname { get; set; }
        public static string LocAddress { get; set; }
        public static string LocCity { get; set; }
        public static string LocState { get; set; }
        public static string LocZipcode { get; set; }
        public static string LocHomePhone { get; set; }
        public static string LocMobile { get; set; }
        public static string LocEmail { get; set; }
        public static DateTime LocStartVlidate { get; set; }
    }
    public static class ControlDoctor
    {
        public static string DoctorID { get; set; }
        public static string DoctorCode { get; set; }
        public static string DoctorName { get; set; }
        public static DateTime DoctorStartVlidate { get; set; }
        public static string DoctorAddress { get; set; }
        public static string DoctorCity { get; set; }
        public static string DoctorState { get; set; }
        public static string DoctorZipCode { get; set; }
        public static string DoctorHomePhone { get; set; }
        public static string DoctorMobilePhone { get; set; }
        public static string DoctorEMail { get; set; }
    }

    public static class ControlSpecimen
    {
        public static string COLLMATERIALID { get; set; }
        public static string COLLMATERIALCODE { get; set; }
        public static string COLLMATERIALTEXT { get; set; }
        public static string COLLMATERIALComment { get; set; }
    }

    public static class ControlUser_Role
    {
        public static string USERSID { get; set; }
        public static string USERID { get; set; }
        public static string USERNAME { get; set; }
        public static string ROLEID { get; set; }
        public static string ROLENAME { get; set; }
    }

    public static class ControlSpecimenGroup
    {

        public static string SPM_GROUP_ID { get; set; }
        public static string SPM_CODE { get; set; }
        public static string SPM_NAME { get; set; }
        public static string SPM_COMMENT { get; set; }
        public static DateTime SPM_CREDATE { get; set; }
    }

    public static class ControlBattery
    {

        public static string BAT_ID { get; set; }
        public static string BAT_CODE { get; set; }
        public static string BAT_SHORTNAME { get; set; }
        public static string BAT_NAME { get; set; }
        public static string BAT_COMMENT { get; set; }
        public static DateTime BAT_CREADATE { get; set; }
        public static string BAT_USERID { get; set; }
        public static DateTime SPM_CREDATE { get; set; }
    }

    public static class ControlDetectionTest
    {
        public static string DETECTION_ID { get; set; }
        public static string DETECTION_CODE { get; set; }
        public static string DETECTION_CREADATE { get; set; }
        public static string DETECTION_SHORTTEXT { get; set; }
        public static string DETECTION_FULLTEXT { get; set; }
        public static string DETECTION_LOGUSERID { get; set; }
        public static string DETECTION_LOGDATE { get; set; }
        public static string DETECTION_PRINT { get; set; }
        public static string DETECTION_CUSTOMIZED_ID { get; set; }
        public static string DETECTION_CUSTOMIZED_NAME { get; set; }
        public static string DETECTION_CUSTOMIZED_CODE { get; set; }

    }
    public static class ControlChemistryTests
    {
        public static string CHEMISTRY_ID { get; set; }
        public static string CHEMISTRY_CODE { get; set; }
        public static string CHEMISTRY_CREADATE { get; set; }
        public static string CHEMISTRY_SHORTTEXT { get; set; }
        public static string CHEMISTRY_FULLTEXT { get; set; }
        public static string CHEMISTRY_LOGUSERID { get; set; }
        public static string CHEMISTRY_LOGDATE { get; set; }
        public static string CHEMISTRY_PRINT { get; set; }
        public static string CHEMISTRY_CUSTOMIZED_ID { get; set; }
        public static string CHEMISTRY_CUSTOMIZED_NAME { get; set; }
        public static string CHEMISTRY_CUSTOMIZED_CODE { get; set; }

    }

    public static class ControlSensitivitiesPanel
    {
        public static string SENPANEL_ID { get; set; }
        public static string SENPANEL_CODE { get; set; }
        public static string SENPANEL_SHORTTEXT { get; set; }
        public static string SENPANEL_FULLTEXT { get; set; }
        public static string SENPANEL_METHOD { get; set; }
        public static string SENPANEL_LOGUSERID { get; set; }
        public static string SENPANEL_LOGDATE { get; set; }

        public static string SENPANEL_ANTIBIOTIC_ID { get; set; }

    }
    public static class ControlBreakPoints
    {
        public static string BREAKPOINT_ID { get; set; }
        public static string BREAKPOINT_CODE { get; set; }
        public static string BREAKPOINT_TEXT { get; set; }

        public static string BREAKPOINT_LIST_ID { get; set; }
        public static string BREAKPOINT_LIST_CODE { get; set; }
        public static string BREAKPOINT_LIST_TEXT { get; set; }
        public static string BREAKPOINT_LIST_USERID { get; set; }

    }
    public static class ControlDashBoardItem
    {
        public static string DASHBOARD_ID { get; set; }
        public static string DASHBOARD_NAME { get; set; }
        public static string DASHBOARD_ENABLE { get; set; }

        public static string DASHBOARD_TIME { get; set; }
        public static string DASHBOARD_TEXT { get; set; }
        public static string DASHBOARD_LOGUSERID { get; set; }

    }


}
