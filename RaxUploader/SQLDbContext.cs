using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineDLL.Entities;

namespace AOITRI
{
    class SQLDbContext : DbContext
    {

        public SQLDbContext(string connectionString) : base(new SqlConnection(connectionString), true)
        {

        }

        public DbSet<FixtureResult> FixtureResult { get; set; }

        public String updateAOIresult(FixtureResult _fixture) 
        {
            int returnvalue = -1;
            var PanelID = new SqlParameter("@Panelid", _fixture.PanelID);
            var Position = new SqlParameter("@Position", _fixture.Position);
            var AOIResult = new SqlParameter("@AOIResult", _fixture.Result);
            var AOIDate = new SqlParameter("@AOIDate",DateTime.Now);
            try {
                returnvalue=this.Database.ExecuteSqlCommand("exec [UpdateAOIResult] @Panelid ,@Position ,@AOIResult,@AOIDate", PanelID, Position, AOIResult, AOIDate);
                Console.WriteLine("[{0}] - return", returnvalue.ToString());                
            }
            catch(Exception e)
            {
                Console.WriteLine("[{0}] - result", e.Message);
            }
            return "OK";
        }

    }
}
