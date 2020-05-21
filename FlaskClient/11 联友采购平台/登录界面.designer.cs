namespace HarveyZ
{
    partial class 登录界面
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(登录界面));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUid = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPwd = new System.Windows.Forms.TextBox();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.labelVersion = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBoxUid
            // 
            this.textBoxUid.AcceptsTab = true;
            resources.ApplyResources(this.textBoxUid, "textBoxUid");
            this.textBoxUid.Name = "textBoxUid";
            this.textBoxUid.TextChanged += new System.EventHandler(this.textBoxChanged);
            this.textBoxUid.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxUid_KeyUp);
            this.textBoxUid.Leave += new System.EventHandler(this.textBoxUid_Leave);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBoxPwd
            // 
            resources.ApplyResources(this.textBoxPwd, "textBoxPwd");
            this.textBoxPwd.Name = "textBoxPwd";
            this.textBoxPwd.TextChanged += new System.EventHandler(this.textBoxChanged);
            this.textBoxPwd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxPwd_KeyUp);
            this.textBoxPwd.Leave += new System.EventHandler(this.textBoxPwd_Leave);
            // 
            // BtnLogin
            // 
            resources.ApplyResources(this.BtnLogin, "BtnLogin");
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.UseVisualStyleBackColor = true;
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // BtnExit
            // 
            resources.ApplyResources(this.BtnExit, "BtnExit");
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // labelVersion
            // 
            resources.ApplyResources(this.labelVersion, "labelVersion");
            this.labelVersion.Name = "labelVersion";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // 登录界面
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnLogin);
            this.Controls.Add(this.textBoxPwd);
            this.Controls.Add(this.textBoxUid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "登录界面";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPwd;
        private System.Windows.Forms.Button BtnLogin;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label3;
    }
}