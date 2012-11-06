using Mvc_ESM.Static_Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mvc_ESM
{
    public partial class Progress : Form
    {
        public Progress()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
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

        private void btnCreateAdjacencyMatrix_Click(object sender, EventArgs e)
        {
            Program.AlgorithmRunner.RunCreateAdjacencyMatrix();
            btnCreateAdjacencyMatrix.Enabled = false;
            ProgressUpdater.Enabled = true;
            btnCreateAdjacencyMatrix_Stop.Enabled = true;
        }

        private void ProgressUpdater_Tick(object sender, EventArgs e)
        {
            lblCreateAdjacencyMatrix.Text = ProgressHelper.CreateMatrixInfo;
            pbCreateAdjacencyMatrix.Value = ProgressHelper.pbCreateMatrix % 101;
        }

        private void btnCreateAdjacencyMatrix_Stop_Click(object sender, EventArgs e)
        {
            btnCreateAdjacencyMatrix_Stop.Enabled = false;
            CreateAdjacencyMatrix.Stop = true;
            Thread thread = new Thread(new ThreadStart(() => {
                while (CreateAdjacencyMatrix.Stoped == false) { Thread.Sleep(100); }
                MessageBox.Show("Đã dừng!");
                btnCreateAdjacencyMatrix.Enabled = true;
                ProgressUpdater.Enabled = false;
            }));
            thread.Start();
        }

    }
}
