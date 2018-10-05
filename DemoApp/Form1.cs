using System;
using System.Windows.Forms;

namespace WinformsDemoApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            var btn = new Button
            {
                Location = new System.Drawing.Point(50, 100),
                Size = new System.Drawing.Size(75, 23),
                Text = "new btb",
                UseVisualStyleBackColor = true
            };
            this.Controls.Add(btn);
        }
    }
}
