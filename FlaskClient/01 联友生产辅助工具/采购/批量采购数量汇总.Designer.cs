namespace HarveyZ.采购
{
    partial class 批量采购数量汇总
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
            this.CheckBoxMonth = new System.Windows.Forms.CheckBox();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BackColor = System.Drawing.SystemColors.Control;
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.label3);
            this.PanelTitle.Controls.Add(this.CheckBoxMonth);
            this.PanelTitle.Controls.Add(this.DtpEndDate);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Controls.Add(this.btnOutput);
            this.PanelTitle.Controls.Add(this.btnSelect);
            this.PanelTitle.Controls.Add(this.DtpStartDate);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Location = new System.Drawing.Point(2, 2);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(809, 85);
            this.PanelTitle.TabIndex = 0;
            // 
            // CheckBoxMonth
            // 
            this.CheckBoxMonth.AutoSize = true;
            this.CheckBoxMonth.Location = new System.Drawing.Point(20, 50);
            this.CheckBoxMonth.Name = "CheckBoxMonth";
            this.CheckBoxMonth.Size = new System.Drawing.Size(96, 16);
            this.CheckBoxMonth.TabIndex = 7;
            this.CheckBoxMonth.Text = "按月汇总显示";
            this.CheckBoxMonth.UseVisualStyleBackColor = true;
            // 
            // DtpEndDate
            // 
            this.DtpEndDate.CustomFormat = "yyyy-MM-dd";
            this.DtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEndDate.Location = new System.Drawing.Point(317, 11);
            this.DtpEndDate.Name = "DtpEndDate";
            this.DtpEndDate.Size = new System.Drawing.Size(115, 21);
            this.DtpEndDate.TabIndex = 5;
            this.DtpEndDate.ValueChanged += new System.EventHandler(this.DtpEndDate_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "工单日期结束：";
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(203, 46);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(75, 23);
            this.btnOutput.TabIndex = 3;
            this.btnOutput.Text = "导出";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(122, 46);
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
            this.DtpStartDate.Location = new System.Drawing.Point(102, 11);
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
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "工单日期起始：";
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(2, 93);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.RowHeadersVisible = false;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(240, 150);
            this.DgvMain.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(474, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(293, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "按工单开单日期来查询需要批量采购的物料的数量汇总";
            // 
            // 批量采购数量汇总
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 605);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Name = "批量采购数量汇总";
            this.Text = "批量采购数量汇总";
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
        private System.Windows.Forms.CheckBox CheckBoxMonth;
        private System.Windows.Forms.Label label3;
    }
}