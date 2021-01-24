namespace HarveyZ.生管排程
{
    partial class 排程物料导出_生产
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
            this.CmBoxDptType = new System.Windows.Forms.ComboBox();
            this.CheckBoxAll = new System.Windows.Forms.CheckBox();
            this.CheckBoxNew = new System.Windows.Forms.CheckBox();
            this.CheckBoxFinished = new System.Windows.Forms.CheckBox();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BackColor = System.Drawing.SystemColors.Control;
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.CmBoxDptType);
            this.PanelTitle.Controls.Add(this.CheckBoxAll);
            this.PanelTitle.Controls.Add(this.CheckBoxNew);
            this.PanelTitle.Controls.Add(this.CheckBoxFinished);
            this.PanelTitle.Controls.Add(this.DtpEndDate);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Controls.Add(this.btnOutput);
            this.PanelTitle.Controls.Add(this.btnSelect);
            this.PanelTitle.Controls.Add(this.DtpStartDate);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Controls.Add(this.label3);
            this.PanelTitle.Location = new System.Drawing.Point(2, 2);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(809, 85);
            this.PanelTitle.TabIndex = 0;
            // 
            // CmBoxDptType
            // 
            this.CmBoxDptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmBoxDptType.FormattingEnabled = true;
            this.CmBoxDptType.Location = new System.Drawing.Point(461, 46);
            this.CmBoxDptType.Name = "CmBoxDptType";
            this.CmBoxDptType.Size = new System.Drawing.Size(141, 20);
            this.CmBoxDptType.TabIndex = 9;
            // 
            // CheckBoxAll
            // 
            this.CheckBoxAll.AutoSize = true;
            this.CheckBoxAll.Location = new System.Drawing.Point(305, 50);
            this.CheckBoxAll.Name = "CheckBoxAll";
            this.CheckBoxAll.Size = new System.Drawing.Size(72, 16);
            this.CheckBoxAll.TabIndex = 8;
            this.CheckBoxAll.Text = "显示全部";
            this.CheckBoxAll.UseVisualStyleBackColor = true;
            this.CheckBoxAll.CheckedChanged += new System.EventHandler(this.CheckBoxAll_CheckedChanged);
            // 
            // CheckBoxNew
            // 
            this.CheckBoxNew.AutoSize = true;
            this.CheckBoxNew.Checked = true;
            this.CheckBoxNew.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxNew.Location = new System.Drawing.Point(162, 50);
            this.CheckBoxNew.Name = "CheckBoxNew";
            this.CheckBoxNew.Size = new System.Drawing.Size(120, 16);
            this.CheckBoxNew.TabIndex = 7;
            this.CheckBoxNew.Text = "只显示未导出物料";
            this.CheckBoxNew.UseVisualStyleBackColor = true;
            this.CheckBoxNew.CheckedChanged += new System.EventHandler(this.CheckBoxNew_CheckedChanged);
            // 
            // CheckBoxFinished
            // 
            this.CheckBoxFinished.AutoSize = true;
            this.CheckBoxFinished.Location = new System.Drawing.Point(20, 50);
            this.CheckBoxFinished.Name = "CheckBoxFinished";
            this.CheckBoxFinished.Size = new System.Drawing.Size(120, 16);
            this.CheckBoxFinished.TabIndex = 6;
            this.CheckBoxFinished.Text = "只显示已导出物料";
            this.CheckBoxFinished.UseVisualStyleBackColor = true;
            this.CheckBoxFinished.CheckedChanged += new System.EventHandler(this.CheckBoxFinished_CheckedChanged);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(401, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "生产部门：";
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(2, 93);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowHeadersVisible = false;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(240, 150);
            this.DgvMain.TabIndex = 1;
            // 
            // 排程物料导出_生产
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 605);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Name = "排程物料导出_生产";
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
        private System.Windows.Forms.CheckBox CheckBoxFinished;
        private System.Windows.Forms.CheckBox CheckBoxAll;
        private System.Windows.Forms.CheckBox CheckBoxNew;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CmBoxDptType;
    }
}