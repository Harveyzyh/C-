namespace HarveyZ
{
    partial class 添加用户
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
            this.TextBoxCompanyId = new System.Windows.Forms.TextBox();
            this.TextBoxUid = new System.Windows.Forms.TextBox();
            this.TextBoxName = new System.Windows.Forms.TextBox();
            this.TextBoxPwd = new System.Windows.Forms.TextBox();
            this.LabelCompanyName = new System.Windows.Forms.Label();
            this.BtnConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "供应商编号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "用户账号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "用户名称：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "初始化密码：";
            // 
            // TextBoxCompanyId
            // 
            this.TextBoxCompanyId.Location = new System.Drawing.Point(105, 30);
            this.TextBoxCompanyId.Name = "TextBoxCompanyId";
            this.TextBoxCompanyId.Size = new System.Drawing.Size(100, 21);
            this.TextBoxCompanyId.TabIndex = 4;
            this.TextBoxCompanyId.Leave += new System.EventHandler(this.TextBoxCompanyId_Leave);
            // 
            // TextBoxUid
            // 
            this.TextBoxUid.Location = new System.Drawing.Point(105, 69);
            this.TextBoxUid.MaxLength = 10;
            this.TextBoxUid.Name = "TextBoxUid";
            this.TextBoxUid.Size = new System.Drawing.Size(100, 21);
            this.TextBoxUid.TabIndex = 5;
            // 
            // TextBoxName
            // 
            this.TextBoxName.Location = new System.Drawing.Point(105, 107);
            this.TextBoxName.MaxLength = 20;
            this.TextBoxName.Name = "TextBoxName";
            this.TextBoxName.Size = new System.Drawing.Size(141, 21);
            this.TextBoxName.TabIndex = 6;
            // 
            // TextBoxPwd
            // 
            this.TextBoxPwd.Location = new System.Drawing.Point(105, 138);
            this.TextBoxPwd.MaxLength = 20;
            this.TextBoxPwd.Name = "TextBoxPwd";
            this.TextBoxPwd.Size = new System.Drawing.Size(100, 21);
            this.TextBoxPwd.TabIndex = 7;
            this.TextBoxPwd.Text = "123456";
            // 
            // LabelCompanyName
            // 
            this.LabelCompanyName.AutoSize = true;
            this.LabelCompanyName.Location = new System.Drawing.Point(212, 35);
            this.LabelCompanyName.Name = "LabelCompanyName";
            this.LabelCompanyName.Size = new System.Drawing.Size(0, 12);
            this.LabelCompanyName.TabIndex = 8;
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.Location = new System.Drawing.Point(38, 187);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(75, 23);
            this.BtnConfirm.TabIndex = 9;
            this.BtnConfirm.Text = "提交";
            this.BtnConfirm.UseVisualStyleBackColor = true;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // 添加用户
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 495);
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.LabelCompanyName);
            this.Controls.Add(this.TextBoxPwd);
            this.Controls.Add(this.TextBoxName);
            this.Controls.Add(this.TextBoxUid);
            this.Controls.Add(this.TextBoxCompanyId);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "添加用户";
            this.Text = "添加用户";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxCompanyId;
        private System.Windows.Forms.TextBox TextBoxUid;
        private System.Windows.Forms.TextBox TextBoxName;
        private System.Windows.Forms.TextBox TextBoxPwd;
        private System.Windows.Forms.Label LabelCompanyName;
        private System.Windows.Forms.Button BtnConfirm;
    }
}