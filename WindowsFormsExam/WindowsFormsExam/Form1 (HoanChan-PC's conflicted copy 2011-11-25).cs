using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsExam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(new SolidBrush(Color.Blue), 100, 100, 30, 30);
            e.Graphics.DrawEllipse(new Pen(Color.Red,4), 100, 100, 30, 30);
            Pen aPen = new Pen(Color.Blue,2);
            aPen.EndCap = LineCap.Custom;
            aPen.CustomEndCap = new AdjustableArrowCap(10,10);
            e.Graphics.DrawLine(aPen,100,100,200,200);
            base.OnPaint(e);
        }
    }
}
