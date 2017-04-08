using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheKnight
{
    public partial class Form3 : Form
    {
        Form1 mainform;
        private const int WM_NCHITTEST = 0x84;
        private const int HTCAPTION = 0x2;

        public Form3()
        {
            InitializeComponent();

            FadeOutIn(this, 30);
        }

        private async void FadeOutIn(Form o, int interval)
        {
            while (o.Opacity > 0.01)
            {
                await Task.Delay(interval / 2);
                o.Opacity -= 0.01;
            }
            o.Opacity = 0;

            while (o.Opacity < 1.0)
            {
                await Task.Delay(interval / 2);
                o.Opacity += 0.01;
            }

            o.Opacity = 1;
            this.Hide();
            mainform = new Form1(this);
            mainform.Show();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath shape = new System.Drawing.Drawing2D.GraphicsPath();
            shape.AddEllipse(0, 0, this.Width, this.Height);
            this.Region = new System.Drawing.Region(shape);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    m.Result = (IntPtr)HTCAPTION;
                    return;
            }
            base.WndProc(ref m);
        }
    }
}
