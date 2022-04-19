using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniquePro.Controller;

namespace UNIQUE.TabbedView.WidgetView
{
    public partial class BacterialActivities : UserControl
    {
        private ReportController objReportControl;

        public BacterialActivities()
        {
            InitializeComponent();
        }

        private void BacterialActivities_Load(object sender, EventArgs e)
        {
            //ChartControl sideBySideBarChart = new ChartControl();
            DataTable dt;
            objReportControl = new ReportController();

            dt = objReportControl.GetBacteriaReceivedByMonth ();

            // Create the first side-by-side bar series and add points to it.
            Series series1 = new Series("Bacteria Received", ViewType.Line);

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                SeriesPoint objPoint = new SeriesPoint(dt.Rows[i]["iden_date"].ToString(), dt.Rows[i]["rec"].ToString());
                series1.Points.Add(objPoint);
                objPoint = null;
            }


            // Add the series to the chart.
            pChart.Series.Add(series1);
            // Hide the legend (if necessary).
            pChart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            // Add the chart to the form.
            pChart.Dock = DockStyle.Fill;
            this.Controls.Add(pChart);

        }
    }
}
