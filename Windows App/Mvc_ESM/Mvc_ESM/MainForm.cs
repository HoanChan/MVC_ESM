﻿using Model;
using Mvc_ESM.Static_Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Mvc_ESM
{
    public partial class MainForm : Form
    {
        public string[] Args;
        DKMHEntities db = new DKMHEntities();
        AlgorithmRunner AlgorithmRunner;
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
                        AlgorithmRunner.RunDeleteOldDatabase();
                        txtArgs.Text += DateTime.Now.ToString() + " DeleteOldData\r\n";
                        break;
                    case "1":
                        AlgorithmRunner.RunCreateAdjacencyMatrix();
                        txtArgs.Text += DateTime.Now.ToString() + " RunCreateAdjacencyMatrix\r\n";
                        break;
                    case "2":
                        AlgorithmRunner.RunColoring();
                        txtArgs.Text += DateTime.Now.ToString() + " RunColoring\r\n";
                        break;
                    case "3":
                        AlgorithmRunner.RunMakeTime();
                        txtArgs.Text += DateTime.Now.ToString() + " RunMakeTime\r\n";
                        break;
                    case "4":
                        AlgorithmRunner.RunRoomArrangement();
                        txtArgs.Text += DateTime.Now.ToString() + " RunRoomArrangement\r\n";
                        break;
                    case "5":
                        AlgorithmRunner.RunSaveToDatabase();
                        txtArgs.Text += DateTime.Now.ToString() + " RunSaveToDatabase\r\n";
                        break;
                    case "6":
                        AlgorithmRunner.RunHandmade();
                        txtArgs.Text += DateTime.Now.ToString() + " Handmade\r\n";
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
            AlgorithmRunner = new AlgorithmRunner();
            AlgorithmRunner.Init();
        }

        private void btnRunColoring_Click(object sender, EventArgs e)
        {
            AlgorithmRunner.RunColoring();
        }

        private void btnRunMakeTime_Click(object sender, EventArgs e)
        {
            AlgorithmRunner.RunMakeTime();
        }

        private void btnRunRoomArrangement_Click(object sender, EventArgs e)
        {
            AlgorithmRunner.RunRoomArrangement();
        }

        private void btnRunSaveToDatabase_Click(object sender, EventArgs e)
        {
            AlgorithmRunner.RunSaveToDatabase();
        }

        private void btnCreateAdjacencyMatrix_Click(object sender, EventArgs e)
        {
            AlgorithmRunner.RunCreateAdjacencyMatrix();
            btnCreateAdjacencyMatrix.Enabled = false;
            ProgressUpdater.Enabled = true;
            btnCreateAdjacencyMatrix_Stop.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            AlgorithmRunner.RunDeleteOldDatabase();
        }

        private void btnRunHandmade_Click(object sender, EventArgs e)
        {
            AlgorithmRunner.RunHandmade();
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

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

    }
}
