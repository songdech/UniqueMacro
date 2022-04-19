using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using UNIQUE.Instrument.MicroScan;

namespace UNIQUE

{
    class Writelog
    {
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
