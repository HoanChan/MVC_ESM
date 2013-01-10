namespace Mvc_ESM
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRunCalc = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnRunSaveToDatabase = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnCreateAdjacencyMatrix_Stop = new System.Windows.Forms.Button();
            this.btnCreateAdjacencyMatrix = new System.Windows.Forms.Button();
            this.ProgressUpdater = new System.Windows.Forms.Timer(this.components);
            this.txtArgs = new System.Windows.Forms.TextBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRunCalc);
            this.groupBox3.Location = new System.Drawing.Point(181, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(148, 91);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Calc";
            // 
            // btnRunCalc
            // 
            this.btnRunCalc.Location = new System.Drawing.Point(36, 43);
            this.btnRunCalc.Name = "btnRunCalc";
            this.btnRunCalc.Size = new System.Drawing.Size(75, 23);
            this.btnRunCalc.TabIndex = 2;
            this.btnRunCalc.Text = "Run";
            this.btnRunCalc.UseVisualStyleBackColor = true;
            this.btnRunCalc.Click += new System.EventHandler(this.btnRunCalc_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnRunSaveToDatabase);
            this.groupBox4.Location = new System.Drawing.Point(335, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(148, 91);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "SaveToDatabase";
            // 
            // btnRunSaveToDatabase
            // 
            this.btnRunSaveToDatabase.Location = new System.Drawing.Point(38, 43);
            this.btnRunSaveToDatabase.Name = "btnRunSaveToDatabase";
            this.btnRunSaveToDatabase.Size = new System.Drawing.Size(75, 23);
            this.btnRunSaveToDatabase.TabIndex = 2;
            this.btnRunSaveToDatabase.Text = "Run";
            this.btnRunSaveToDatabase.UseVisualStyleBackColor = true;
            this.btnRunSaveToDatabase.Click += new System.EventHandler(this.btnRunSaveToDatabase_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnCreateAdjacencyMatrix_Stop);
            this.groupBox5.Controls.Add(this.btnCreateAdjacencyMatrix);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(163, 91);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "CreateAdjacencyMatrix";
            // 
            // btnCreateAdjacencyMatrix_Stop
            // 
            this.btnCreateAdjacencyMatrix_Stop.Enabled = false;
            this.btnCreateAdjacencyMatrix_Stop.Location = new System.Drawing.Point(46, 53);
            this.btnCreateAdjacencyMatrix_Stop.Name = "btnCreateAdjacencyMatrix_Stop";
            this.btnCreateAdjacencyMatrix_Stop.Size = new System.Drawing.Size(75, 23);
            this.btnCreateAdjacencyMatrix_Stop.TabIndex = 3;
            this.btnCreateAdjacencyMatrix_Stop.Text = "Stop";
            this.btnCreateAdjacencyMatrix_Stop.UseVisualStyleBackColor = true;
            this.btnCreateAdjacencyMatrix_Stop.Click += new System.EventHandler(this.btnCreateAdjacencyMatrix_Stop_Click);
            // 
            // btnCreateAdjacencyMatrix
            // 
            this.btnCreateAdjacencyMatrix.Location = new System.Drawing.Point(46, 24);
            this.btnCreateAdjacencyMatrix.Name = "btnCreateAdjacencyMatrix";
            this.btnCreateAdjacencyMatrix.Size = new System.Drawing.Size(75, 23);
            this.btnCreateAdjacencyMatrix.TabIndex = 2;
            this.btnCreateAdjacencyMatrix.Text = "Run";
            this.btnCreateAdjacencyMatrix.UseVisualStyleBackColor = true;
            this.btnCreateAdjacencyMatrix.Click += new System.EventHandler(this.btnCreateAdjacencyMatrix_Click);
            // 
            // ProgressUpdater
            // 
            this.ProgressUpdater.Interval = 1000;
            this.ProgressUpdater.Tick += new System.EventHandler(this.ProgressUpdater_Tick);
            // 
            // txtArgs
            // 
            this.txtArgs.Location = new System.Drawing.Point(15, 109);
            this.txtArgs.Multiline = true;
            this.txtArgs.Name = "txtArgs";
            this.txtArgs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtArgs.Size = new System.Drawing.Size(466, 214);
            this.txtArgs.TabIndex = 5;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Mvc_ESM Server Win App";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 330);
            this.Controls.Add(this.txtArgs);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Progress";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnRunCalc;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnRunSaveToDatabase;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnCreateAdjacencyMatrix;
        private System.Windows.Forms.Timer ProgressUpdater;
        private System.Windows.Forms.Button btnCreateAdjacencyMatrix_Stop;
        private System.Windows.Forms.TextBox txtArgs;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

