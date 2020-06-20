namespace HarveyZ
{
    partial class 修改密码
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TextBoxUid = new System.Windows.Forms.TextBox();
            this.TextBoxOldPwd = new System.Windows.Forms.TextBox();
            this.TextBoxNewPwd = new System.Windows.Forms.TextBox();
            this.TextBoxNewPwdConfirm = new System.Windows.Forms.TextBox();
            this.BtnConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户账号:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "原密码:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "新密码:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "新密码确认:";
            // 
            // TextBoxUid
            // 
            this.TextBoxUid.Location = new System.Drawing.Point(113, 43);
            this.TextBoxUid.Name = "TextBoxUid";
            this.TextBoxUid.ReadOnly = true;
            this.TextBoxUid.Size = new System.Drawing.Size(200, 21);
            this.TextBoxUid.TabIndex = 4;
            // 
            // TextBoxOldPwd
            // 
            this.TextBoxOldPwd.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TextBoxOldPwd.Location = new System.Drawing.Point(113, 78);
            this.TextBoxOldPwd.Name = "TextBoxOldPwd";
            this.TextBoxOldPwd.PasswordChar = '*';
            this.TextBoxOldPwd.Size = new System.Drawing.Size(200, 21);
            this.TextBoxOldPwd.TabIndex = 5;
            // 
            // TextBoxNewPwd
            // 
            this.TextBoxNewPwd.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TextBoxNewPwd.Location = new System.Drawing.Point(113, 116);
            this.TextBoxNewPwd.Name = "TextBoxNewPwd";
            this.TextBoxNewPwd.PasswordChar = '*';
            this.TextBoxNewPwd.Size = new System.Drawing.Size(200, 21);
            this.TextBoxNewPwd.TabIndex = 6;
            // 
            // TextBoxNewPwdConfirm
            // 
            this.TextBoxNewPwdConfirm.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TextBoxNewPwdConfirm.Location = new System.Drawing.Point(113, 156);
            this.TextBoxNewPwdConfirm.Name = "TextBoxNewPwdConfirm";
            this.TextBoxNewPwdConfirm.PasswordChar = '*';
            this.TextBoxNewPwdConfirm.Size = new System.Drawing.Size(200, 21);
            this.TextBoxNewPwdConfirm.TabIndex = 7;
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.Location = new System.Drawing.Point(44, 209);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(75, 23);
            this.BtnConfirm.TabIndex = 8;
            this.BtnConfirm.Text = "提交";
            this.BtnConfirm.UseVisualStyleBackColor = true;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // 修改密码
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 426);
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.TextBoxNewPwdConfirm);
            this.Controls.Add(this.TextBoxNewPwd);
            this.Controls.Add(this.TextBoxOldPwd);
            this.Controls.Add(this.TextBoxUid);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "修改密码";
            this.Text = "修改密码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxUid;
        private System.Windows.Forms.TextBox TextBoxOldPwd;
        private System.Windows.Forms.TextBox TextBoxNewPwd;
        private System.Windows.Forms.TextBox TextBoxNewPwdConfirm;
        private System.Windows.Forms.Button BtnConfirm;
    }
}