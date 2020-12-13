namespace HarveyZ.维护ERP
{
    partial class 客户配置维护_勾选项替换
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.DgvKeyValue = new System.Windows.Forms.DataGridView();
            this.panelKhpzString = new System.Windows.Forms.Panel();
            this.TxbKhpzString = new System.Windows.Forms.TextBox();
            this.panelTipsLabel = new System.Windows.Forms.Panel();
            this.BtnWork = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TxbTips = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TxbLog = new System.Windows.Forms.TextBox();
            this.panelLog = new System.Windows.Forms.Panel();
            this.BtnLayout = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.CheckBoxPreCheck = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvKeyValue)).BeginInit();
            this.panelKhpzString.SuspendLayout();
            this.panelTipsLabel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panelLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(777, 511);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DgvKeyValue);
            this.tabPage1.Controls.Add(this.panelKhpzString);
            this.tabPage1.Controls.Add(this.TxbTips);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(769, 485);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据配置页";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // DgvKeyValue
            // 
            this.DgvKeyValue.AllowUserToAddRows = false;
            this.DgvKeyValue.AllowUserToDeleteRows = false;
            this.DgvKeyValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvKeyValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvKeyValue.Location = new System.Drawing.Point(3, 234);
            this.DgvKeyValue.Name = "DgvKeyValue";
            this.DgvKeyValue.RowTemplate.Height = 23;
            this.DgvKeyValue.Size = new System.Drawing.Size(611, 248);
            this.DgvKeyValue.TabIndex = 3;
            // 
            // panelKhpzString
            // 
            this.panelKhpzString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelKhpzString.Controls.Add(this.TxbKhpzString);
            this.panelKhpzString.Controls.Add(this.panelTipsLabel);
            this.panelKhpzString.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelKhpzString.Location = new System.Drawing.Point(3, 3);
            this.panelKhpzString.Name = "panelKhpzString";
            this.panelKhpzString.Size = new System.Drawing.Size(611, 231);
            this.panelKhpzString.TabIndex = 2;
            // 
            // TxbKhpzString
            // 
            this.TxbKhpzString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxbKhpzString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxbKhpzString.Location = new System.Drawing.Point(0, 80);
            this.TxbKhpzString.Multiline = true;
            this.TxbKhpzString.Name = "TxbKhpzString";
            this.TxbKhpzString.Size = new System.Drawing.Size(609, 149);
            this.TxbKhpzString.TabIndex = 1;
            this.TxbKhpzString.TextChanged += new System.EventHandler(this.TxbKhpzString_TextChanged);
            // 
            // panelTipsLabel
            // 
            this.panelTipsLabel.Controls.Add(this.CheckBoxPreCheck);
            this.panelTipsLabel.Controls.Add(this.BtnWork);
            this.panelTipsLabel.Controls.Add(this.label1);
            this.panelTipsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTipsLabel.Location = new System.Drawing.Point(0, 0);
            this.panelTipsLabel.Name = "panelTipsLabel";
            this.panelTipsLabel.Size = new System.Drawing.Size(609, 80);
            this.panelTipsLabel.TabIndex = 0;
            // 
            // BtnWork
            // 
            this.BtnWork.Location = new System.Drawing.Point(501, 41);
            this.BtnWork.Name = "BtnWork";
            this.BtnWork.Size = new System.Drawing.Size(75, 23);
            this.BtnWork.TabIndex = 1;
            this.BtnWork.Text = "开始处理";
            this.BtnWork.UseVisualStyleBackColor = true;
            this.BtnWork.Click += new System.EventHandler(this.BtnWork_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(14, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(299, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "在下列空格中填客户配置的范围，以SQL语句确定范围。\r\n\r\n注意：SQL必须存在字段TQ001, TQ002\r\n否则必定会出错。 ";
            // 
            // TxbTips
            // 
            this.TxbTips.BackColor = System.Drawing.Color.White;
            this.TxbTips.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxbTips.Dock = System.Windows.Forms.DockStyle.Right;
            this.TxbTips.Location = new System.Drawing.Point(614, 3);
            this.TxbTips.Multiline = true;
            this.TxbTips.Name = "TxbTips";
            this.TxbTips.ReadOnly = true;
            this.TxbTips.Size = new System.Drawing.Size(152, 479);
            this.TxbTips.TabIndex = 0;
            this.TxbTips.Text = "提示：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TxbLog);
            this.tabPage2.Controls.Add(this.panelLog);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(769, 485);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "操作日志页";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TxbLog
            // 
            this.TxbLog.BackColor = System.Drawing.Color.White;
            this.TxbLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxbLog.Location = new System.Drawing.Point(3, 45);
            this.TxbLog.Multiline = true;
            this.TxbLog.Name = "TxbLog";
            this.TxbLog.ReadOnly = true;
            this.TxbLog.Size = new System.Drawing.Size(763, 437);
            this.TxbLog.TabIndex = 1;
            // 
            // panelLog
            // 
            this.panelLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLog.Controls.Add(this.BtnLayout);
            this.panelLog.Controls.Add(this.BtnClear);
            this.panelLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLog.Location = new System.Drawing.Point(3, 3);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(763, 42);
            this.panelLog.TabIndex = 0;
            // 
            // BtnLayout
            // 
            this.BtnLayout.Location = new System.Drawing.Point(120, 9);
            this.BtnLayout.Name = "BtnLayout";
            this.BtnLayout.Size = new System.Drawing.Size(109, 23);
            this.BtnLayout.TabIndex = 1;
            this.BtnLayout.Text = "导出日志记录";
            this.BtnLayout.UseVisualStyleBackColor = true;
            this.BtnLayout.Click += new System.EventHandler(this.BtnLayout_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(14, 9);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(87, 23);
            this.BtnClear.TabIndex = 0;
            this.BtnClear.Text = "清空日志记录";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // CheckBoxPreCheck
            // 
            this.CheckBoxPreCheck.AutoSize = true;
            this.CheckBoxPreCheck.Location = new System.Drawing.Point(503, 15);
            this.CheckBoxPreCheck.Name = "CheckBoxPreCheck";
            this.CheckBoxPreCheck.Size = new System.Drawing.Size(72, 16);
            this.CheckBoxPreCheck.TabIndex = 2;
            this.CheckBoxPreCheck.Text = "替换处理";
            this.CheckBoxPreCheck.UseVisualStyleBackColor = true;
            // 
            // 客户配置维护_勾选项替换
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 511);
            this.Controls.Add(this.tabControl1);
            this.Name = "客户配置维护_勾选项替换";
            this.Text = "客户配置维护_勾选项替换";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvKeyValue)).EndInit();
            this.panelKhpzString.ResumeLayout(false);
            this.panelKhpzString.PerformLayout();
            this.panelTipsLabel.ResumeLayout(false);
            this.panelTipsLabel.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panelLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox TxbLog;
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.Button BtnLayout;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Panel panelKhpzString;
        private System.Windows.Forms.TextBox TxbKhpzString;
        private System.Windows.Forms.Panel panelTipsLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxbTips;
        private System.Windows.Forms.DataGridView DgvKeyValue;
        private System.Windows.Forms.Button BtnWork;
        private System.Windows.Forms.CheckBox CheckBoxPreCheck;
    }
}