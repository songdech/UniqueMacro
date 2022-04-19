using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNIQUE
{
    class MessageFilter : IMessageFilter
    {
        //Following are the Windows API hex values. You can find more at http://faisalmb.com/blog/post/2009/01/Values-of-Windows-API.aspx
        // Here we are only interested in only Keyboard and Mouse activities
        private int WM_LBUTTONDOWN = 0x0201;
        private int WM_KEYDOWN = 0x0100;
        private int WM_RBUTTONDOWN = 0x0204;
        private int WM_MBUTTONDOWN = 0x0207;
        private int WM_MOUSEWHEEL = 0x020A;
        private int WM_MOUSEMOVE = 0x0200;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_MOUSEMOVE || m.Msg == WM_KEYDOWN || m.Msg == WM_LBUTTONDOWN || m.Msg == WM_MOUSEWHEEL || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_MBUTTONDOWN)
            {
                //Reset the timer of form1
                FormMain.timerIdle.Stop();
                FormMain.timerIdle.Start();
            }
            return false;
        }
    }
}
