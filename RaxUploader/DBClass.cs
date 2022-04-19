using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineDLL.Entities;

namespace AOITRI
{
    class DBClass
    {
        public bool IsServerConnected(string connectionString)
        {
            using (var l_oConnection = new SqlConnection(connectionString))
            {
                try
                {
                    l_oConnection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public bool updateAOIresult(string connectionstring, FixtureResult _fixture)
        {

            var PanelID = new SqlParameter("@Panelid", _fixture.PanelID);
            var Position = new SqlParameter("@Position", _fixture.Position);
            var AOIResult = new SqlParameter("@AOIResult", _fixture.Result);
            var AOIDate = new SqlParameter("@AOIDate", _fixture.CreatedDate);

            using (SqlConnection conn = new SqlConnection(connectionstring))
            {   
                SqlCommand cmd = new SqlCommand("UpdateAOIResult", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(PanelID);
                cmd.Parameters.Add(Position);
                cmd.Parameters.Add(AOIResult);
                cmd.Parameters.Add(AOIDate);

                try { 
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }catch(Exception e)
                {
                    throw new Exception(e.Message);
                    
                }
            }
    }
        public bool clearAOIresultbyFixture(string connectionstring, FixtureResult _fixture)
        {

            var PanelID = new SqlParameter("@Panelid", _fixture.PanelID);


            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("ClearAOIResultBYFixture", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(PanelID);


                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
        public bool updateAOIresultbyFixture(string connectionstring, FixtureResult _fixture)
        {

            var PanelID = new SqlParameter("@Panelid", _fixture.PanelID);
            var Position = new SqlParameter("@Position", _fixture.Position);
            var AOIResult = new SqlParameter("@AOIResult", _fixture.Result);
            //var AOIDate = new SqlParameter("@AOIDate", Convert.ToDateTime (String.Format("{0:u}" ,_fixture.CreatedDate)));
            var AOIDate = new SqlParameter("@AOIDate", _fixture.CreatedDate);
            var Machinename = new SqlParameter("@MachineName", _fixture.MachineName);

            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("UpdateAOIResultBYFixture", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(PanelID);
                cmd.Parameters.Add(Position);
                cmd.Parameters.Add(AOIResult);
                cmd.Parameters.Add(AOIDate);
                cmd.Parameters.Add(Machinename);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);

                }
            }
        }

    } }
