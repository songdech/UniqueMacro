using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using UNIQUE.Instrument.MicroScan;

namespace UNIQUE

{
    class Writedatalog
    {
        public static void WriteLog(string Log)
        {
            string pathSPY = "";
            string setting_spy = "setting_spy.ini";
            try
            {
                DataSet XmlFile = new DataSet();
                XmlFile.ReadXml(setting_spy, XmlReadMode.Auto);
                // Get setting from file
                pathSPY = XmlFile.Tables[0].Rows[0]["pathSPY"].ToString();

                //string pathOfORUSpy = Settings1.Default.writelogpath;
                string FileName = @"" + pathSPY + "OEMlog.spy";
                System.Text.Encoding encoder;
                encoder = System.Text.Encoding.UTF8;
                StreamWriter obj;
                obj = new StreamWriter(FileName, true, System.Text.Encoding.Default);
                obj.WriteLine(Log);
                obj.Close();
            }
            catch (Exception ex)
            {


            } //  WriteLog(DateTime.Now.ToString(),"Error!!! WriterLog();  ", ex.Message);
        }

        public static void WriteLog_ORM(string Log)
        {
            string pathSPY = "";
            string setting_ORM = "setting_ORM.xml";
            try
            {
                DataSet XmlFile = new DataSet();
                XmlFile.ReadXml(setting_ORM, XmlReadMode.Auto);
                // Get setting from file
                pathSPY = XmlFile.Tables[0].Rows[0]["pathSPY"].ToString();

                //string pathOfORUSpy = Settings1.Default.writelogpath;
                string FileName = @"" + pathSPY + "ORM_log.spy";
                System.Text.Encoding encoder;
                encoder = System.Text.Encoding.UTF8;
                StreamWriter obj;
                obj = new StreamWriter(FileName, true, System.Text.Encoding.Default);
                obj.WriteLine(Log);
                obj.Close();
            }
            catch (Exception ex) { } //  WriteLog(DateTime.Now.ToString(),"Error!!! WriterLog();  ", ex.Message);
        }

        public static void WriteLog_Rectube(string Log)
        {
            string pathSPY = "";
            string setting_spy = "setting_ORM.xml";
            try
            {
                DataSet XmlFile = new DataSet();
                XmlFile.ReadXml(setting_spy, XmlReadMode.Auto);
                // Get setting from file
                pathSPY = XmlFile.Tables[0].Rows[0]["pathSPY"].ToString();

                //string pathOfORUSpy = Settings1.Default.writelogpath;
                string FileName = @"" + pathSPY + "Rectubelog.spy";
                System.Text.Encoding encoder;
                encoder = System.Text.Encoding.UTF8;
                StreamWriter obj;
                obj = new StreamWriter(FileName, true, System.Text.Encoding.Default);
                obj.WriteLine(Log);
                obj.Close();
            }
            catch (Exception ex) { } //  WriteLog(DateTime.Now.ToString(),"Error!!! WriterLog();  ", ex.Message);
        }
        public static void Log_Microscan(string Log)
        {
            try
            {
                string Pathspy = Settings_MicroScan.Default.Pathspy;

                string FileName = @"" + Pathspy + "files.spy";

                System.Text.Encoding encoder;
                encoder = System.Text.Encoding.UTF8;
                StreamWriter obj;
                obj = new StreamWriter(FileName, true, System.Text.Encoding.Default);
                obj.WriteLine(Log);
                obj.Close();
            }
            catch (Exception ex)

            {
                throw new Exception (ex.Message);
            } 
        }
    }
}
