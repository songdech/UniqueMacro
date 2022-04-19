using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;

namespace UNIQUE
{
    public partial class Controls : UserControl
    {
        string name;
        UserControl moduleType;
        private string p;
        private Type type;
        public Controls(string name, Type moduleType)
        {
            InitializeComponent();
            this.name = name;
            this.type = moduleType;

     
           // xtraUserControl1.Controls.Clear();
           // xtraUserControl1.Controls.Add(type);
        }

    
    }

    public class NavBarGroupTagObject
    {
        

        //public string Name { get { return name; } }
        //public Type ModuleType { get { return moduleType; } }
       
    }


}
