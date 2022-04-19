using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;

namespace UNIQUE
{
    public partial class XtraReport_WorkSheet : DevExpress.XtraReports.UI.XtraReport
    {

        SqlConnection conn;

        // Format DateTime en-US
        System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");
        //string format_DT = "dd/MM/yyyy HH:mm:ss";

        public XtraReport_WorkSheet()
        {
            InitializeComponent();
        }

        public static int CalculateAge(DateTime birthDay)
        {
            int years = DateTime.Now.Year - birthDay.Year;

            if ((birthDay.Month > DateTime.Now.Month) || (birthDay.Month == DateTime.Now.Month && birthDay.Day > DateTime.Now.Day))
                years--;

            return years;
        }

        private void XtraReport_SummariesISSUE_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                //conn = new Connect_DB().ConnectBBS();
            }
            catch (SqlException ex) { }
        }
    }
}
