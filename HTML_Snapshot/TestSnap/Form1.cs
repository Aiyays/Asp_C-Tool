using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestSnap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SnapLibrary.Snapshot snap = new SnapLibrary.Snapshot();
            Bitmap bmp = snap.TakeSnapshot(this.webBrowser1.ActiveXInstance, new Rectangle(0, 0, this.webBrowser1.Width, this.webBrowser1.Height));
            if (bmp != null)
            {
                bmp.Save("1.bmp");
                bmp.Dispose();
            }
        }
    }
}