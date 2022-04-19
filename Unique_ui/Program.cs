using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniquePro.Entities.Common;
using UniquePro.Entities.GeneralSetting;
using UniquePro.Controller;

using UNIQUE;

namespace UNIQUE
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static UserM objUserM = null;

        [STAThread]
        static void Main()
        {
            objUserM = new UserM();
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "unique");

            //try
            //{
            //    if (mutex.WaitOne(0, false))
            //    {
            //        // Run the application
            //        Application.EnableVisualStyles();
            //        Application.SetCompatibleTextRenderingDefault(false);
            //        Application.Run(new frmLogin());
            //    }
            //    else
            //    {
            //        MessageBox.Show("โปรแกรมเปิดไว้อยู่แล้ว ไม่สามารถเปิดใหม่ได้อีกครั้ง", "Error", MessageBoxButtons.OK);
            //    }
            //}
            //finally
            //{
            //    if (mutex != null)
            //    {
            //        mutex.Close();
            //        mutex = null;
            //    }
            //}
            objUserM = new UserM();
            //ConfigurationController objConfiguation = new ConfigurationController();

            try
            {
                //
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmLogin());
                //   Application.AddMessageFilter(new MessageFilter());
                //Application.Run(new FormMain());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
