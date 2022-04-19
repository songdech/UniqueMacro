using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using UniquePro.Entities.Common;
using UniquePro.Entities.Configuration;
using UniquePro.Entities.GeneralSetting;
using UniquePro.Entities.Patient;

namespace UNIQUE
{
    class ControlParameter
    {
        public static string strStainControlName { get; set; } // frmStainlist.
        public static string strStainControlID { get; set; } // frmStainlist.
        public static int protocolID { get; set; } // fromRequestMGNT.

        //LOGIN INFORMAtion.

        public static string loginName { get; set; }
        public static string loginLastName { get; set; }
        public static string loginID { get; set; }

        public static UserM UserInfoM { get; set; }
        public static  LocationM LocationInfoM { get; set; }
        public static DoctorM DoctorInfoM  { get; set; }
        public static SpecimenM  SpecimenInfoM { get; set; }
        public static SpecimenG SpecimenInfoG { get; set; }

        public static BatteryM BatteryInfoM { get; set; }

        public static ProtocalM  ProtocalM { get; set; }

        public static MediaM MediaM { get; set; }

        public static ColonyDescM ColonyDesc { get; set; }

        public static StainM StainM { get; set; }

        //Culture identification result  to  antimicrobial bereakpoint for Query by organismn.

        public static string oganismnIDforAntimicrobial { get; set; }

        public static UserM ControlUser { get; set; } // 

        public static ConnectStringM ConStrM { get; set; } // 

        public static DataSet  DatasetComboData { get; set; } // 

        public static  UniqueSystemConfigM  UniqueSystemConfig { get; set; } // 

        public static AntibioticsM AntibioticM { get; set; } // Antiviotic .

        public static TestDictM TestDictM { get; set; } // Test Dictionary.

        public static SensitivityPanelM SensitivityPanelM { get; set; } // Sensitivity Panel.
        public static CustomResultM CustomResulM { get; set; } // Customized Result.

        public static DetectionTestM DetectionInfoM { get; set; } // Detection test result.

        public static ChemistryM ChemistryM { get; set; } // Chemistry test result.

        public static BreakPointM BreakPointM { get; set; } // BreakPoint Dictionary.

        public static OrganismM OrganismM { get; set; } // Orgnaisms Dictionary.
        public static SenpanelM SenPanelM { get; set; } // Sensitivity Panel Dictionary.

        public static AntibioticsM AntibioticFAMS { get; set; } // Antibiotic Family Group Panel Dictionary.

        public static OrganismM OrganismFAMS { get; set; } // Organisms Family Group Dictionary.

        public static ExpertM ExpertRuleM { get; set; } // Expert rule Dictionary

        public static DashBoardM DashboardM { get; set; }   // GeneralSetting Dashboard.

        public static PatientM PatientM { get; set; }   // Patient Management.

        //NOTE
        // public static BatteryM BatteryMGNT { get; set; } // fromBattery_MGNT.



        public static class ControlConString
        {
            public static string CONANME { get; set; }
            public static string CONPATH { get; set; }
            public static string CONDATASOURCE { get; set; }
            public static string CONCATALOG { get; set; }
            public static string CONUSER { get; set; }
            public static string CONPASS { get; set; }
        }
    }

}


