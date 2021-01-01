namespace HarveyZ.生管排程
{
    partial class 排程物料导出_汇总
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
            this.CheckBoxJh = new System.Windows.Forms.CheckBox();
            this.CheckBoxJhDl = new System.Windows.Forms.CheckBox();
            this.CheckBoxDl = new System.Windows.Forms.CheckBox();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.TxbWl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxbPh = new System.Windows.Forms.TextBox();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BackColor = System.Drawing.SystemColors.Control;
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.TxbPh);
            this.PanelTitle.Controls.Add(this.label4);
            this.PanelTitle.Controls.Add(this.TxbWl);
            this.PanelTitle.Controls.Add(this.label3);
            this.PanelTitle.Controls.Add(this.CheckBoxJh);
            this.PanelTitle.Controls.Add(this.CheckBoxJhDl);
            this.PanelTitle.Controls.Add(this.CheckBoxDl);
            this.PanelTitle.Controls.Add(this.DtpEndDate);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Controls.Add(this.btnOutput);
            this.PanelTitle.Controls.Add(this.btnSelect);
            this.PanelTitle.Controls.Add(this.DtpStartDate);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Location = new System.Drawing.Point(2, 2);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(809, 116);
            this.PanelTitle.TabIndex = 0;
            // 
            // CheckBoxJh
            // 
            this.CheckBoxJh.AutoSize = true;
            this.CheckBoxJh.Location = new System.Drawing.Point(305, 77);
            this.CheckBoxJh.Name = "CheckBoxJh";
            this.CheckBoxJh.Size = new System.Drawing.Size(84, 16);
            this.CheckBoxJh.TabIndex = 8;
            this.CheckBoxJh.Text = "进货量 = 0";
            this.CheckBoxJh.UseVisualStyleBackColor = true;
            this.CheckBoxJh.CheckedChanged += new System.EventHandler(this.CheckBoxJh_CheckedChanged);
            // 
            // CheckBoxJhDl
            // 
            this.CheckBoxJhDl.AutoSize = true;
            this.CheckBoxJhDl.Location = new System.Drawing.Point(162, 77);
            this.CheckBoxJhDl.Name = "CheckBoxJhDl";
            this.CheckBoxJhDl.Size = new System.Drawing.Size(114, 16);
            this.CheckBoxJhDl.TabIndex = 7;
            this.CheckBoxJhDl.Text = "进货量 < 未领量";
            this.CheckBoxJhDl.UseVisualStyleBackColor = true;
            this.CheckBoxJhDl.CheckedChanged += new System.EventHandler(this.CheckBoxJhDl_CheckedChanged);
            // 
            // CheckBoxDl
            // 
            this.CheckBoxDl.AutoSize = true;
            this.CheckBoxDl.Location = new System.Drawing.Point(20, 77);
            this.CheckBoxDl.Name = "CheckBoxDl";
            this.CheckBoxDl.Size = new System.Drawing.Size(96, 16);
            this.CheckBoxDl.TabIndex = 6;
            this.CheckBoxDl.Text = "未领数量 > 0";
            this.CheckBoxDl.UseVisualStyleBackColor = true;
            // 
            // DtpEndDate
            // 
            this.DtpEndDate.CustomFormat = "yyyy-MM-dd";
            this.DtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEndDate.Location = new System.Drawing.Point(297, 11);
            this.DtpEndDate.Name = "DtpEndDate";
            this.DtpEndDate.Size = new System.Drawing.Size(115, 21);
            this.DtpEndDate.TabIndex = 5;
            this.DtpEndDate.ValueChanged += new System.EventHandler(this.DtpEndDate_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "排程结束：";
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(527, 7);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(75, 23);
            this.btnOutput.TabIndex = 3;
            this.btnOutput.Text = "导出";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(446, 7);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "查询";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // DtpStartDate
            // 
            this.DtpStartDate.CustomFormat = "yyyy-MM-dd";
            this.DtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStartDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.DtpStartDate.Location = new System.Drawing.Point(78, 11);
            this.DtpStartDate.Name = "DtpStartDate";
            this.DtpStartDate.Size = new System.Drawing.Size(115, 21);
            this.DtpStartDate.TabIndex = 0;
            this.DtpStartDate.TabStop = false;
            this.DtpStartDate.ValueChanged += new System.EventHandler(this.DtpStartDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "排程起始：";
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(12, 228);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.RowHeadersVisible = false;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(240, 150);
            this.DgvMain.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "物料信息：";
            // 
            // TxbWl
            // 
            this.TxbWl.Location = new System.Drawing.Point(78, 41);
            this.TxbWl.Name = "TxbWl";
            this.TxbWl.Size = new System.Drawing.Size(115, 21);
            this.TxbWl.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(239, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "批号信息：";
            // 
            // TxbPh
            // 
            this.TxbPh.Location = new System.Drawing.Point(297, 41);
            this.TxbPh.Name = "TxbPh";
            this.TxbPh.Size = new System.Drawing.Size(115, 21);
            this.TxbPh.TabIndex = 12;
            // 
            // 排程物料导出_汇总
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 605);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Name = "排程物料导出_汇总";
            this.Text = "排程生产物料导出";
            this.Resize += new System.EventHandler(this.FormMain_Resized);
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.DateTimePicker DtpStartDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DateTimePicker DtpEndDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox CheckBoxDl;
        private System.Windows.Forms.CheckBox CheckBoxJh;
        private System.Windows.Forms.CheckBox CheckBoxJhDl;
        private System.Windows.Forms.TextBox TxbPh;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxbWl;
        private System.Windows.Forms.Label label3;
    }
}