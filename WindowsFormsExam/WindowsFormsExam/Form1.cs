using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsExam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Interval = 500;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = Mvc_ESM.Static_Helper.ProgressHelper.CreateMatrixInfo;
            label2.Text = Mvc_ESM.Static_Helper.ProgressHelper.pbCreateMatrix.ToString();
        }
        DKMHEntities db = new DKMHEntities();
        private void button2_Click(object sender, EventArgs e)
        {
            var q = (from su in db.monhocs select su).ToList();
            DateTime bg = DateTime.Now;
            monhoc m;
            for (int i = 0; i < 100000; i++)
            {
                m = q.FirstOrDefault(x => x.MaMonHoc == "");   
            }
            TimeSpan t = DateTime.Now - bg;
            MessageBox.Show(t.ToString());
        }
    }
}
