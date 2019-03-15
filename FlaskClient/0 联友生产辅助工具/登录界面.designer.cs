namespace HarveyZ
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.label1 = new System.Windows.Forms.Label();
            this.FormLogin_TextBox_UID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FormLogin_TextBox_PWD = new System.Windows.Forms.TextBox();
            this.FormLogin_Button_Login = new System.Windows.Forms.Button();
            this.FormLogin_Button_Exit = new System.Windows.Forms.Button();
            this.labelVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 13F);
            this.label1.Location = new System.Drawing.Point(132, 86);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "账号：";
            // 
            // FormLogin_TextBox_UID
            // 
            this.FormLogin_TextBox_UID.AcceptsTab = true;
            this.FormLogin_TextBox_UID.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.FormLogin_TextBox_UID.Location = new System.Drawing.Point(197, 83);
            this.FormLogin_TextBox_UID.Margin = new System.Windows.Forms.Padding(4);
            this.FormLogin_TextBox_UID.Name = "FormLogin_TextBox_UID";
            this.FormLogin_TextBox_UID.Size = new System.Drawing.Size(182, 41);
            this.FormLogin_TextBox_UID.TabIndex = 1;
            this.FormLogin_TextBox_UID.TextChanged += new System.EventHandler(this.FormLogin_TextBox_Changed);
            this.FormLogin_TextBox_UID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormLogin_TextBox_UID_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 13F);
            this.label2.Location = new System.Drawing.Point(132, 146);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 35);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码：";
            // 
            // FormLogin_TextBox_PWD
            // 
            this.FormLogin_TextBox_PWD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.FormLogin_TextBox_PWD.Location = new System.Drawing.Point(197, 143);
            this.FormLogin_TextBox_PWD.Margin = new System.Windows.Forms.Padding(4);
            this.FormLogin_TextBox_PWD.Name = "FormLogin_TextBox_PWD";
            this.FormLogin_TextBox_PWD.PasswordChar = '*';
            this.FormLogin_TextBox_PWD.Size = new System.Drawing.Size(182, 41);
            this.FormLogin_TextBox_PWD.TabIndex = 3;
            this.FormLogin_TextBox_PWD.TextChanged += new System.EventHandler(this.FormLogin_TextBox_Changed);
            this.FormLogin_TextBox_PWD.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormLogin_TextBox_PWD_KeyUp);
            // 
            // FormLogin_Button_Login
            // 
            this.FormLogin_Button_Login.Font = new System.Drawing.Font("宋体", 12F);
            this.FormLogin_Button_Login.Location = new System.Drawing.Point(121, 213);
            this.FormLogin_Button_Login.Margin = new System.Windows.Forms.Padding(4);
            this.FormLogin_Button_Login.Name = "FormLogin_Button_Login";
            this.FormLogin_Button_Login.Size = new System.Drawing.Size(100, 29);
            this.FormLogin_Button_Login.TabIndex = 4;
            this.FormLogin_Button_Login.Text = "确认";
            this.FormLogin_Button_Login.UseVisualStyleBackColor = true;
            this.FormLogin_Button_Login.Click += new System.EventHandler(this.FormLogin_Button_Login_Click);
            // 
            // FormLogin_Button_Exit
            // 
            this.FormLogin_Button_Exit.Font = new System.Drawing.Font("宋体", 12F);
            this.FormLogin_Button_Exit.Location = new System.Drawing.Point(272, 213);
            this.FormLogin_Button_Exit.Margin = new System.Windows.Forms.Padding(4);
            this.FormLogin_Button_Exit.Name = "FormLogin_Button_Exit";
            this.FormLogin_Button_Exit.Size = new System.Drawing.Size(100, 29);
            this.FormLogin_Button_Exit.TabIndex = 5;
            this.FormLogin_Button_Exit.Text = "退出";
            this.FormLogin_Button_Exit.UseVisualStyleBackColor = true;
            this.FormLogin_Button_Exit.Click += new System.EventHandler(this.FormLogin_Button_Exit_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.Font = new System.Drawing.Font("宋体", 8F);
            this.labelVersion.Location = new System.Drawing.Point(337, 276);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelVersion.Size = new System.Drawing.Size(161, 12);
            this.labelVersion.TabIndex = 6;
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(499, 288);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.FormLogin_Button_Exit);
            this.Controls.Add(this.FormLogin_Button_Login);
            this.Controls.Add(this.FormLogin_TextBox_PWD);
            this.Controls.Add(this.FormLogin_TextBox_UID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户登录";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FormLogin_TextBox_UID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FormLogin_TextBox_PWD;
        private System.Windows.Forms.Button FormLogin_Button_Login;
        private System.Windows.Forms.Button FormLogin_Button_Exit;
        private System.Windows.Forms.Label labelVersion;
    }
}