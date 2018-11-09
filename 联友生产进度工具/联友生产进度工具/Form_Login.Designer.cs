namespace 联友生产进度工具
{
    partial class Form_Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Login));
            this.label1 = new System.Windows.Forms.Label();
            this.Form_Login_TextBox_UID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Form_Login_TextBox_PWD = new System.Windows.Forms.TextBox();
            this.Form_Login_Button_Login = new System.Windows.Forms.Button();
            this.Form_Login_Button_Exit = new System.Windows.Forms.Button();
            this.Form_Login_Botton_Linktest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 13F);
            this.label1.Location = new System.Drawing.Point(88, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "账号：";
            // 
            // Form_Login_TextBox_UID
            // 
            this.Form_Login_TextBox_UID.AcceptsTab = true;
            this.Form_Login_TextBox_UID.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Form_Login_TextBox_UID.Location = new System.Drawing.Point(140, 51);
            this.Form_Login_TextBox_UID.Name = "Form_Login_TextBox_UID";
            this.Form_Login_TextBox_UID.Size = new System.Drawing.Size(137, 21);
            this.Form_Login_TextBox_UID.TabIndex = 1;
            this.Form_Login_TextBox_UID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form_Login_TextBox_UID_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 13F);
            this.label2.Location = new System.Drawing.Point(88, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码：";
            // 
            // Form_Login_TextBox_PWD
            // 
            this.Form_Login_TextBox_PWD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Form_Login_TextBox_PWD.Location = new System.Drawing.Point(140, 114);
            this.Form_Login_TextBox_PWD.Name = "Form_Login_TextBox_PWD";
            this.Form_Login_TextBox_PWD.PasswordChar = '*';
            this.Form_Login_TextBox_PWD.Size = new System.Drawing.Size(137, 21);
            this.Form_Login_TextBox_PWD.TabIndex = 3;
            this.Form_Login_TextBox_PWD.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form_Login_TextBox_PWD_KeyUp);
            // 
            // Form_Login_Button_Login
            // 
            this.Form_Login_Button_Login.Font = new System.Drawing.Font("宋体", 12F);
            this.Form_Login_Button_Login.Location = new System.Drawing.Point(91, 170);
            this.Form_Login_Button_Login.Name = "Form_Login_Button_Login";
            this.Form_Login_Button_Login.Size = new System.Drawing.Size(75, 23);
            this.Form_Login_Button_Login.TabIndex = 4;
            this.Form_Login_Button_Login.Text = "确认";
            this.Form_Login_Button_Login.UseVisualStyleBackColor = true;
            this.Form_Login_Button_Login.Click += new System.EventHandler(this.Form_Login_Button_Login_Click);
            // 
            // Form_Login_Button_Exit
            // 
            this.Form_Login_Button_Exit.Font = new System.Drawing.Font("宋体", 12F);
            this.Form_Login_Button_Exit.Location = new System.Drawing.Point(204, 170);
            this.Form_Login_Button_Exit.Name = "Form_Login_Button_Exit";
            this.Form_Login_Button_Exit.Size = new System.Drawing.Size(75, 23);
            this.Form_Login_Button_Exit.TabIndex = 5;
            this.Form_Login_Button_Exit.Text = "退出";
            this.Form_Login_Button_Exit.UseVisualStyleBackColor = true;
            this.Form_Login_Button_Exit.Click += new System.EventHandler(this.Form_Login_Button_Exit_Click);
            // 
            // Form_Login_Botton_Linktest
            // 
            this.Form_Login_Botton_Linktest.Location = new System.Drawing.Point(298, 208);
            this.Form_Login_Botton_Linktest.Name = "Form_Login_Botton_Linktest";
            this.Form_Login_Botton_Linktest.Size = new System.Drawing.Size(75, 23);
            this.Form_Login_Botton_Linktest.TabIndex = 6;
            this.Form_Login_Botton_Linktest.Text = "数据库连接测试";
            this.Form_Login_Botton_Linktest.UseVisualStyleBackColor = true;
            this.Form_Login_Botton_Linktest.Visible = false;
            this.Form_Login_Botton_Linktest.Click += new System.EventHandler(this.Form_Login_Botton_Linktest_Click);
            // 
            // Form_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 230);
            this.Controls.Add(this.Form_Login_Botton_Linktest);
            this.Controls.Add(this.Form_Login_Button_Exit);
            this.Controls.Add(this.Form_Login_Button_Login);
            this.Controls.Add(this.Form_Login_TextBox_PWD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Form_Login_TextBox_UID);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户登录";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Form_Login_TextBox_UID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Form_Login_TextBox_PWD;
        private System.Windows.Forms.Button Form_Login_Button_Login;
        private System.Windows.Forms.Button Form_Login_Button_Exit;
        private System.Windows.Forms.Button Form_Login_Botton_Linktest;
    }
}