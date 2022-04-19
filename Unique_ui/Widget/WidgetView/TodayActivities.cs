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
    public partial class TodayActivities : UserControl
    {

        private ReportController objReportControl; 
        public TodayActivities()
        {
            InitializeComponent();
        }

        private void TodayActivities_Load(object sender, EventArgs e)
        {
            //ChartControl sideBySideBarChart = new ChartControl();
            DataTable dt;
            objReportControl = new ReportController();

            dt = objReportControl.GetSpecimentReceivedByMonth();

            // Create the first side-by-side bar series and add points to it.
            Series series1 = new Series("Speciment Received", ViewType.Line);

            for (int i = 0; i <= dt.Rows.Count - 1; i++) 
            {
                SeriesPoint objPoint = new SeriesPoint(dt.Rows[i]["rec_date"].ToString() , dt.Rows[i]["rec"].ToString());
                series1.Points.Add(objPoint);
                objPoint = null;
            }

            //series1.Points.Add(new SeriesPoint("7", 2));
            //series1.Points.Add(new SeriesPoint("6", 10));
            //series1.Points.Add(new SeriesPoint("5", 12));
            //series1.Points.Add(new SeriesPoint("4", 14));
            //series1.Points.Add(new SeriesPoint("3", 17));
            //series1.Points.Add(new SeriesPoint("2", 17));
            //series1.Points.Add(new SeriesPoint("1", 1));
            //series1.SeriesPointsSortingKey = SeriesPointKey.Argument;
            //series1.SeriesPointsSorting = SortingMode.Ascending;
            

            // Create the second side-by-side bar series and add points to it.
            //Series series2 = new Series("Side-by-Side Bar Series 2", ViewType.Bar);
            //series2.Points.Add(new SeriesPoint("A", 15));
            //series2.Points.Add(new SeriesPoint("B", 18));
            //series2.Points.Add(new SeriesPoint("C", 25));
            //series2.Points.Add(new SeriesPoint("D", 33));

            // Add the series to the chart.
            pChart.Series.Add(series1);
            //pChart.Series.Add(series2);

            // Hide the legend (if necessary).
            pChart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            // Rotate the diagram (if necessary).
            //((XYDiagram)pChart.Diagram).Rotated = true;

            // Add a title to the chart (if necessary).
            //pChart.Titles.Add(chartTitle1);

            // Add the chart to the form.
            pChart.Dock = DockStyle.Fill;
            this.Controls.Add(pChart);
        }
    }
}
