using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNIQUE
{
    public partial class Welcome : UserControl
    {
        public Welcome()
        {
            InitializeComponent();



            // Handling the QueryControl event that will populate all automatically generated Documents
            this.widgetView1.QueryControl += widgetView1_QueryControl;
        }

        // Assigning a required content for each auto generated Document
        void widgetView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {

            //if (e.Document == datenowDocument)
            //    e.Control = new UNIQUE.TabbedView.WidgetView.Datenow();           
            if (e.Control == null)
                e.Control = new System.Windows.Forms.Control();

            //if (e.Document == documentRC) // Request Creation (RC)
            //    e.Control = new UNIQUE.TabbedView.WidgetView.DashboardRC();

            if (e.Document == documentSR) //Specimen Reception (SR)
                e.Control = new UNIQUE.TabbedView.WidgetView.DashboardSR();

            if (e.Document == documentRE) //Result Entry. (RE)
                e.Control = new UNIQUE.TabbedView.WidgetView.DashboardRE();

            if (e.Document == documentPR)//Preliminary Report(PR)
                e.Control = new UNIQUE.TabbedView.WidgetView.DashboardPR();

            if (e.Document == documentCR)//Clinical Report. (CR)
                e.Control = new UNIQUE.TabbedView.WidgetView.DashBoardCR();

            if (e.Document == documentTR) //Technical Report (TR)
                e.Control = new UNIQUE.TabbedView.WidgetView.DashboardTR();

            if (e.Document == documentRP)
                e.Control = new UNIQUE.TabbedView.WidgetView.DashboardRP();

            if (e.Document == documentDT)
                e.Control = new UNIQUE.TabbedView.WidgetView.Datenow();

            if (e.Document == ReportSpeciment)
            {
                e.Control = new UNIQUE.TabbedView.WidgetView.TodayActivities();
            }

            if (e.Document == BacteriaReceived)
            {
                e.Control = new UNIQUE.TabbedView.WidgetView.BacterialActivities();
            }

            
            if (e.Document ==documentLogin)
                e.Control = new UNIQUE.TabbedView.WidgetView.LoginInfo();
            //if (e.Document == documentRC)


            //    e.Control = new UNIQUE.TabbedView.WidgetView.Detail();
            //if (e.Document == documentRP)
            //    e.Control = new UNIQUE.TabbedView.WidgetView.UserControl1();
            //if (e.Document == documentCR)
            //    e.Control = new UNIQUE.TabbedView.WidgetView.navMenubar();
         
        
      
            ;
        }

        private void Welcome_Load(object sender, EventArgs e)
        {

        }

     

    

       
    }
}
