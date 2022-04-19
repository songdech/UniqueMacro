using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace UNIQUE.Forms.Report_worksheet
{
    public partial class Form_Worksheet_printing : Form
    {
        public Form_Worksheet_printing()
        {
            InitializeComponent();
        }

        private void Form_Worksheet_printing_Load(object sender, EventArgs e)
        {
            XtraReport_WorkSheet report = new XtraReport_WorkSheet();
            documentViewer1.DocumentSource = report;
            report.CreateDocument();

        }

        private void printPreviewBarItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
