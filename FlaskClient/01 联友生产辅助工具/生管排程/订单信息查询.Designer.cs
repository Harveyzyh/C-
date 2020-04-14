namespace 联友生产辅助工具.生管排程
{
    partial class 订单信息查询
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
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.BtnOutput = new System.Windows.Forms.Button();
            this.BtnSelect = new System.Windows.Forms.Button();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.CmBoxType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BackColor = System.Drawing.SystemColors.Control;
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.BtnOutput);
            this.PanelTitle.Controls.Add(this.BtnSelect);
            this.PanelTitle.Controls.Add(this.DtpEndDate);
            this.PanelTitle.Controls.Add(this.DtpStartDate);
            this.PanelTitle.Controls.Add(this.CmBoxType);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Controls.Add(this.label3);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Location = new System.Drawing.Point(1, 2);
            this.PanelTitle.Margin = new System.Windows.Forms.Padding(4);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(1029, 86);
            this.PanelTitle.TabIndex = 0;
            // 
            // BtnOutput
            // 
            this.BtnOutput.Enabled = false;
            this.BtnOutput.Location = new System.Drawing.Point(727, 45);
            this.BtnOutput.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOutput.Name = "BtnOutput";
            this.BtnOutput.Size = new System.Drawing.Size(100, 29);
            this.BtnOutput.TabIndex = 7;
            this.BtnOutput.Text = "导出";
            this.BtnOutput.UseVisualStyleBackColor = true;
            this.BtnOutput.Click += new System.EventHandler(this.BtnOutput_Click);
            // 
            // BtnSelect
            // 
            this.BtnSelect.Location = new System.Drawing.Point(727, 9);
            this.BtnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSelect.Name = "BtnSelect";
            this.BtnSelect.Size = new System.Drawing.Size(100, 29);
            this.BtnSelect.TabIndex = 6;
            this.BtnSelect.Text = "查询";
            this.BtnSelect.UseVisualStyleBackColor = true;
            this.BtnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // DtpEndDate
            // 
            this.DtpEndDate.CustomFormat = "yyyy年MM月dd日 HH时mm分ss秒";
            this.DtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEndDate.Location = new System.Drawing.Point(371, 48);
            this.DtpEndDate.Margin = new System.Windows.Forms.Padding(4);
            this.DtpEndDate.Name = "DtpEndDate";
            this.DtpEndDate.ShowCheckBox = true;
            this.DtpEndDate.Size = new System.Drawing.Size(273, 24);
            this.DtpEndDate.TabIndex = 5;
            // 
            // DtpStartDate
            // 
            this.DtpStartDate.CustomFormat = "yyyy年MM月dd日 HH时mm分ss秒";
            this.DtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStartDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.DtpStartDate.Location = new System.Drawing.Point(371, 10);
            this.DtpStartDate.Margin = new System.Windows.Forms.Padding(4);
            this.DtpStartDate.Name = "DtpStartDate";
            this.DtpStartDate.ShowCheckBox = true;
            this.DtpStartDate.Size = new System.Drawing.Size(273, 24);
            this.DtpStartDate.TabIndex = 2;
            // 
            // CmBoxType
            // 
            this.CmBoxType.FormattingEnabled = true;
            this.CmBoxType.Items.AddRange(new object[] {
            "生产三部",
            "半成品",
            "原材料",
            "所有订单"});
            this.CmBoxType.Location = new System.Drawing.Point(92, 11);
            this.CmBoxType.Margin = new System.Windows.Forms.Padding(4);
            this.CmBoxType.Name = "CmBoxType";
            this.CmBoxType.Size = new System.Drawing.Size(154, 23);
            this.CmBoxType.TabIndex = 0;
            this.CmBoxType.SelectedIndexChanged += new System.EventHandler(this.CmBoxType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "订单类别：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(292, 52);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "结束时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "开始时间：";
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(1, 108);
            this.DgvMain.Margin = new System.Windows.Forms.Padding(4);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.RowHeadersVisible = false;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(1015, 298);
            this.DgvMain.TabIndex = 1;
            // 
            // 订单信息查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 415);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "订单信息查询";
            this.Text = "订单信息导出";
            this.SizeChanged += new System.EventHandler(this.FormMain_Resized);
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.ComboBox CmBoxType;
        private System.Windows.Forms.Button BtnOutput;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.DateTimePicker DtpEndDate;
        private System.Windows.Forms.DateTimePicker DtpStartDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}