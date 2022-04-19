using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixtureScanner
{
    class FixtureResult
    {
        public string FilePath { get; set; }
        public string FixtureId { get; set; }
        public string JobName { get; set; }
        public string PanelID { get; set; }
        public string Position { get; set; }
        public string Result { get; set; }        
        public string MachineName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        //public string CreatedDate { get; set; }
    }
}
