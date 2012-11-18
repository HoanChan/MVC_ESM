using Model;
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
    public partial class MainForm : Form
    {
        public string[] Args;
        DKMHEntities db = new DKMHEntities();

        public delegate void ProcessParametersDelegate(object sender, string[] args);
        public void ProcessParameters(object sender, string[] args)
        {
            // The form has loaded, and initialization will have been be done.

            // Add the command-line arguments to our textbox, just to confirm that
            // it reached here.
            if (args != null && args.Length != 0)
            {
                //txtArgs.Text += DateTime.Now.ToString("mm:ss.ff") + " ";
                //for (int i = 0; i < args.Length; i++)
                //{
                //    txtArgs.Text += args[i] + "\r\n";
                //}

                switch (args[0])
                {
                    case "0":                        
                        db.Database.ExecuteSqlCommand("DELETE FROM Thi");
                        db.Database.ExecuteSqlCommand("DELETE FROM CaThi");
                        txtArgs.Text += DateTime.Now.ToString() + " DeleteOldData\r\n";
                        break;
                    case "1":
                        Program.AlgorithmRunner.RunCreateAdjacencyMatrix();
                        txtArgs.Text += DateTime.Now.ToString() + " RunCreateAdjacencyMatrix\r\n";
                        break;
                    case "2":
                        Program.AlgorithmRunner.RunColoring();
                        txtArgs.Text += DateTime.Now.ToString() + " RunColoring\r\n";
                        break;
                    case "3":
                        Program.AlgorithmRunner.RunMakeTime();
                        txtArgs.Text += DateTime.Now.ToString() + " RunMakeTime\r\n";
                        break;
                    case "4":
                        Program.AlgorithmRunner.RunRoomArrangement();
                        txtArgs.Text += DateTime.Now.ToString() + " RunRoomArrangement\r\n";
                        break;
                    case "5":
                        Program.AlgorithmRunner.RunSaveToDatabase();
                        txtArgs.Text += DateTime.Now.ToString() + " RunSaveToDatabase\r\n";
                        break;
                    default:
                        txtArgs.Text += DateTime.Now.ToString() + " Not Run Anything\r\n";
                        break;
                }
            }
            else
            {
                txtArgs.Text += DateTime.Now.ToString() + " Run No Args: Init()\r\n";
            }
        }

        public MainForm()
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
            pbCreateAdjacencyMatrix.Value = ProgressHelper.pbCreateMatrix;
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (this.Args != null)
            {
                ProcessParameters(null, this.Args);
                this.Args = null;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM Thi");
            db.Database.ExecuteSqlCommand("DELETE FROM CaThi");
        }

    }
}
