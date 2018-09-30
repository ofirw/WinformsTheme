using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformsThemeTests
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            Control control = Control.FromHandle(m.HWnd);
            if (control == this)
            { }
            base.WndProc(ref m);
        }
    }
}
