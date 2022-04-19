using System;
using System.Data;

namespace UNIQUE
{
    public class MBReportController
    {
        private MBReportDAO objMBReportDAO = null;

        public MBReportController()
        {
            objMBReportDAO = new MBReportDAO();
        }

        public DataTable Get_Info_Report1(MBReportM objMBReportM)
        {
            try
            {
                return objMBReportDAO.Get_Info_Report1(objMBReportM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable Get_ReportFormat(MBReportM objMBReportM)
        {
            try
            {
                return objMBReportDAO.Get_ReportFormat(objMBReportM);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
