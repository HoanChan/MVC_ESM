namespace WindowsFormsExam
{
    partial class frmConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnection));
            this.lblServerName = new System.Windows.Forms.Label();
            this.lblAuthentication = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.cboAuthentication = new System.Windows.Forms.ComboBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblProgramName = new System.Windows.Forms.Label();
            this.cboDatabases = new System.Windows.Forms.ComboBox();
            this.lblDatabases = new System.Windows.Forms.Label();
            this.btnConnectToServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(13, 86);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(63, 13);
            this.lblServerName.TabIndex = 0;
            this.lblServerName.Text = "Tên Server:";
            // 
            // lblAuthentication
            // 
            this.lblAuthentication.AutoSize = true;
            this.lblAuthentication.Location = new System.Drawing.Point(13, 112);
            this.lblAuthentication.Name = "lblAuthentication";
            this.lblAuthentication.Size = new System.Drawing.Size(87, 13);
            this.lblAuthentication.TabIndex = 1;
            this.lblAuthentication.Text = "Loại chứng thực:";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(13, 139);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(85, 13);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "Tên người dùng:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(13, 165);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(55, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Mật khẩu:";
            // 
            // cboAuthentication
            // 
            this.cboAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAuthentication.FormattingEnabled = true;
            this.cboAuthentication.Items.AddRange(new object[] {
            "Từ tài khoản Windows",
            "Từ tài khoản SQL Server"});
            this.cboAuthentication.Location = new System.Drawing.Point(145, 109);
            this.cboAuthentication.Name = "cboAuthentication";
            this.cboAuthentication.Size = new System.Drawing.Size(253, 21);
            this.cboAuthentication.TabIndex = 4;
            this.cboAuthentication.SelectedIndexChanged += new System.EventHandler(this.cboAuthentication_SelectedIndexChanged);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(145, 136);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(253, 20);
            this.txtUserName.TabIndex = 5;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(145, 162);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(253, 20);
            this.txtPassword.TabIndex = 6;
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(145, 83);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(253, 20);
            this.txtServerName.TabIndex = 7;
            this.txtServerName.Text = "HOANCHAN-PC\\SQLEXPRESS";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(173, 217);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(81, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "Chấp nhận";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(260, 217);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Thoát";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblInfo.Location = new System.Drawing.Point(220, 40);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(81, 19);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "Version: 1.0";
            // 
            // lblProgramName
            // 
            this.lblProgramName.AutoSize = true;
            this.lblProgramName.BackColor = System.Drawing.Color.Transparent;
            this.lblProgramName.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblProgramName.Location = new System.Drawing.Point(55, 9);
            this.lblProgramName.Name = "lblProgramName";
            this.lblProgramName.Size = new System.Drawing.Size(290, 31);
            this.lblProgramName.TabIndex = 11;
            this.lblProgramName.Text = "SQL DATABASE Viewer";
            // 
            // cboDatabases
            // 
            this.cboDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDatabases.Enabled = false;
            this.cboDatabases.FormattingEnabled = true;
            this.cboDatabases.Location = new System.Drawing.Point(145, 188);
            this.cboDatabases.Name = "cboDatabases";
            this.cboDatabases.Size = new System.Drawing.Size(253, 21);
            this.cboDatabases.TabIndex = 13;
            // 
            // lblDatabases
            // 
            this.lblDatabases.AutoSize = true;
            this.lblDatabases.Enabled = false;
            this.lblDatabases.Location = new System.Drawing.Point(13, 191);
            this.lblDatabases.Name = "lblDatabases";
            this.lblDatabases.Size = new System.Drawing.Size(71, 13);
            this.lblDatabases.TabIndex = 12;
            this.lblDatabases.Text = "Cơ sở dữ liệu:";
            // 
            // btnConnectToServer
            // 
            this.btnConnectToServer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConnectToServer.Location = new System.Drawing.Point(69, 217);
            this.btnConnectToServer.Name = "btnConnectToServer";
            this.btnConnectToServer.Size = new System.Drawing.Size(98, 23);
            this.btnConnectToServer.TabIndex = 14;
            this.btnConnectToServer.Text = "Kết nối tới Server";
            this.btnConnectToServer.UseVisualStyleBackColor = true;
            this.btnConnectToServer.Click += new System.EventHandler(this.btnConnectToServer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(410, 252);
            this.Controls.Add(this.btnConnectToServer);
            this.Controls.Add(this.cboDatabases);
            this.Controls.Add(this.lblDatabases);
            this.Controls.Add(this.lblProgramName);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtServerName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.cboAuthentication);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblAuthentication);
            this.Controls.Add(this.lblServerName);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.Label lblAuthentication;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.ComboBox cboAuthentication;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblProgramName;
        private System.Windows.Forms.ComboBox cboDatabases;
        private System.Windows.Forms.Label lblDatabases;
        private System.Windows.Forms.Button btnConnectToServer;


    }
}

