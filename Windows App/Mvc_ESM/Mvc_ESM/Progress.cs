using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mvc_ESM
{
    public partial class Progress : Form
    {
        public Progress()
        {
            InitializeComponent();
        }

        private void btnRunColoring_Click(object sender, EventArgs e)
        {
            Program.AlgorithmRunner.RunColoring();
        }

        private void btnRunMakeTime_Click(object sender, EventArgs e)
        {
            Program.AlgorithmRunner.RunMakeTime();
        }

        private void btnRunRoomArrangement_Click(object sender, EventArgs e)
        {
            Program.AlgorithmRunner.RunRoomArrangement();
        }

        private void btnRunSaveToDatabase_Click(object sender, EventArgs e)
        {
            Program.AlgorithmRunner.RunSaveToDatabase();
        }


    }
}
