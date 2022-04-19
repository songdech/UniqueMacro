using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNIQUE.TabbedView.WidgetView
{
    public partial class PendingList : UserControl
    {
        public PendingList()
        {
            InitializeComponent();
        }

        private void PendingList_Load(object sender, EventArgs e)
        {
          
        }

        private void tileItem5_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            Form2 fm = new Form2();
            fm.Show();
        
        }
    }
}
