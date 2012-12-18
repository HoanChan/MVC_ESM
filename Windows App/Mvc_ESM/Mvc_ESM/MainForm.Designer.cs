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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRunColoring = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRunMakeTime = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRunRoomArrangement = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnRunSaveToDatabase = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar4 = new System.Windows.Forms.ProgressBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnCreateAdjacencyMatrix_Stop = new System.Windows.Forms.Button();
            this.btnCreateAdjacencyMatrix = new System.Windows.Forms.Button();
            this.lblCreateAdjacencyMatrix = new System.Windows.Forms.Label();
            this.pbCreateAdjacencyMatrix = new System.Windows.Forms.ProgressBar();
            this.ProgressUpdater = new System.Windows.Forms.Timer(this.components);
            this.txtArgs = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnRunHandmade = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar5 = new System.Windows.Forms.ProgressBar();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRunColoring);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Location = new System.Drawing.Point(210, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 123);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Coloring";
            // 
            // btnRunColoring
            // 
            this.btnRunColoring.Location = new System.Drawing.Point(59, 76);
            this.btnRunColoring.Name = "btnRunColoring";
            this.btnRunColoring.Size = new System.Drawing.Size(75, 23);
            this.btnRunColoring.TabIndex = 2;
            this.btnRunColoring.Text = "Run";
            this.btnRunColoring.UseVisualStyleBackColor = true;
            this.btnRunColoring.Click += new System.EventHandler(this.btnRunColoring_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 47);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(180, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRunMakeTime);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.progressBar2);
            this.groupBox2.Location = new System.Drawing.Point(408, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 123);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MakeTime";
            // 
            // btnRunMakeTime
            // 
            this.btnRunMakeTime.Location = new System.Drawing.Point(59, 76);
            this.btnRunMakeTime.Name = "btnRunMakeTime";
            this.btnRunMakeTime.Size = new System.Drawing.Size(75, 23);
            this.btnRunMakeTime.TabIndex = 2;
            this.btnRunMakeTime.Text = "Run";
            this.btnRunMakeTime.UseVisualStyleBackColor = true;
            this.btnRunMakeTime.Click += new System.EventHandler(this.btnRunMakeTime_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(6, 47);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(180, 23);
            this.progressBar2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRunRoomArrangement);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.progressBar3);
            this.groupBox3.Location = new System.Drawing.Point(12, 141);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(192, 123);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "RoomArrangement";
            // 
            // btnRunRoomArrangement
            // 
            this.btnRunRoomArrangement.Location = new System.Drawing.Point(59, 76);
            this.btnRunRoomArrangement.Name = "btnRunRoomArrangement";
            this.btnRunRoomArrangement.Size = new System.Drawing.Size(75, 23);
            this.btnRunRoomArrangement.TabIndex = 2;
            this.btnRunRoomArrangement.Text = "Run";
            this.btnRunRoomArrangement.UseVisualStyleBackColor = true;
            this.btnRunRoomArrangement.Click += new System.EventHandler(this.btnRunRoomArrangement_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "label3";
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(6, 47);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(180, 23);
            this.progressBar3.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnRunSaveToDatabase);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.progressBar4);
            this.groupBox4.Location = new System.Drawing.Point(210, 141);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(192, 123);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "SaveToDatabase";
            // 
            // btnRunSaveToDatabase
            // 
            this.btnRunSaveToDatabase.Location = new System.Drawing.Point(59, 76);
            this.btnRunSaveToDatabase.Name = "btnRunSaveToDatabase";
            this.btnRunSaveToDatabase.Size = new System.Drawing.Size(75, 23);
            this.btnRunSaveToDatabase.TabIndex = 2;
            this.btnRunSaveToDatabase.Text = "Run";
            this.btnRunSaveToDatabase.UseVisualStyleBackColor = true;
            this.btnRunSaveToDatabase.Click += new System.EventHandler(this.btnRunSaveToDatabase_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "label4";
            // 
            // progressBar4
            // 
            this.progressBar4.Location = new System.Drawing.Point(6, 47);
            this.progressBar4.Name = "progressBar4";
            this.progressBar4.Size = new System.Drawing.Size(180, 23);
            this.progressBar4.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnCreateAdjacencyMatrix_Stop);
            this.groupBox5.Controls.Add(this.btnCreateAdjacencyMatrix);
            this.groupBox5.Controls.Add(this.lblCreateAdjacencyMatrix);
            this.groupBox5.Controls.Add(this.pbCreateAdjacencyMatrix);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(192, 123);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "CreateAdjacencyMatrix";
            // 
            // btnCreateAdjacencyMatrix_Stop
            // 
            this.btnCreateAdjacencyMatrix_Stop.Enabled = false;
            this.btnCreateAdjacencyMatrix_Stop.Location = new System.Drawing.Point(111, 94);
            this.btnCreateAdjacencyMatrix_Stop.Name = "btnCreateAdjacencyMatrix_Stop";
            this.btnCreateAdjacencyMatrix_Stop.Size = new System.Drawing.Size(75, 23);
            this.btnCreateAdjacencyMatrix_Stop.TabIndex = 3;
            this.btnCreateAdjacencyMatrix_Stop.Text = "Stop";
            this.btnCreateAdjacencyMatrix_Stop.UseVisualStyleBackColor = true;
            this.btnCreateAdjacencyMatrix_Stop.Click += new System.EventHandler(this.btnCreateAdjacencyMatrix_Stop_Click);
            // 
            // btnCreateAdjacencyMatrix
            // 
            this.btnCreateAdjacencyMatrix.Location = new System.Drawing.Point(6, 94);
            this.btnCreateAdjacencyMatrix.Name = "btnCreateAdjacencyMatrix";
            this.btnCreateAdjacencyMatrix.Size = new System.Drawing.Size(75, 23);
            this.btnCreateAdjacencyMatrix.TabIndex = 2;
            this.btnCreateAdjacencyMatrix.Text = "Run";
            this.btnCreateAdjacencyMatrix.UseVisualStyleBackColor = true;
            this.btnCreateAdjacencyMatrix.Click += new System.EventHandler(this.btnCreateAdjacencyMatrix_Click);
            // 
            // lblCreateAdjacencyMatrix
            // 
            this.lblCreateAdjacencyMatrix.AutoSize = true;
            this.lblCreateAdjacencyMatrix.Location = new System.Drawing.Point(6, 31);
            this.lblCreateAdjacencyMatrix.Name = "lblCreateAdjacencyMatrix";
            this.lblCreateAdjacencyMatrix.Size = new System.Drawing.Size(35, 13);
            this.lblCreateAdjacencyMatrix.TabIndex = 1;
            this.lblCreateAdjacencyMatrix.Text = "label5";
            // 
            // pbCreateAdjacencyMatrix
            // 
            this.pbCreateAdjacencyMatrix.Location = new System.Drawing.Point(6, 47);
            this.pbCreateAdjacencyMatrix.Name = "pbCreateAdjacencyMatrix";
            this.pbCreateAdjacencyMatrix.Size = new System.Drawing.Size(180, 23);
            this.pbCreateAdjacencyMatrix.TabIndex = 0;
            // 
            // ProgressUpdater
            // 
            this.ProgressUpdater.Interval = 1000;
            this.ProgressUpdater.Tick += new System.EventHandler(this.ProgressUpdater_Tick);
            // 
            // txtArgs
            // 
            this.txtArgs.Location = new System.Drawing.Point(15, 306);
            this.txtArgs.Multiline = true;
            this.txtArgs.Name = "txtArgs";
            this.txtArgs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtArgs.Size = new System.Drawing.Size(582, 137);
            this.txtArgs.TabIndex = 5;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(248, 270);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(126, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete Database";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnRunHandmade);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.progressBar5);
            this.groupBox6.Location = new System.Drawing.Point(414, 141);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(192, 123);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Handmade";
            // 
            // btnRunHandmade
            // 
            this.btnRunHandmade.Location = new System.Drawing.Point(59, 76);
            this.btnRunHandmade.Name = "btnRunHandmade";
            this.btnRunHandmade.Size = new System.Drawing.Size(75, 23);
            this.btnRunHandmade.TabIndex = 2;
            this.btnRunHandmade.Text = "Run";
            this.btnRunHandmade.UseVisualStyleBackColor = true;
            this.btnRunHandmade.Click += new System.EventHandler(this.btnRunHandmade_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "label5";
            // 
            // progressBar5
            // 
            this.progressBar5.Location = new System.Drawing.Point(6, 47);
            this.progressBar5.Name = "progressBar5";
            this.progressBar5.Size = new System.Drawing.Size(180, 23);
            this.progressBar5.TabIndex = 0;
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
            this.ClientSize = new System.Drawing.Size(609, 455);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.txtArgs);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Progress";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRunColoring;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRunMakeTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnRunRoomArrangement;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnRunSaveToDatabase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnCreateAdjacencyMatrix;
        private System.Windows.Forms.Label lblCreateAdjacencyMatrix;
        private System.Windows.Forms.ProgressBar pbCreateAdjacencyMatrix;
        private System.Windows.Forms.Timer ProgressUpdater;
        private System.Windows.Forms.Button btnCreateAdjacencyMatrix_Stop;
        private System.Windows.Forms.TextBox txtArgs;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnRunHandmade;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar progressBar5;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

