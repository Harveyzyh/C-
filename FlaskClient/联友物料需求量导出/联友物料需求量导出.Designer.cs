namespace 联友物料需求量导出
{
    partial class 联友物料需求量导出
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(联友物料需求量导出));
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.DgvList = new System.Windows.Forms.DataGridView();
            this.BtnGetData = new System.Windows.Forms.Button();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.LabelDataMakeDate = new System.Windows.Forms.Label();
            this.BtnData2Excel = new System.Windows.Forms.Button();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.BtnData2Excel);
            this.PanelTitle.Controls.Add(this.LabelDataMakeDate);
            this.PanelTitle.Controls.Add(this.DtpDate);
            this.PanelTitle.Controls.Add(this.BtnGetData);
            this.PanelTitle.Location = new System.Drawing.Point(2, 2);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(607, 50);
            this.PanelTitle.TabIndex = 0;
            // 
            // DgvList
            // 
            this.DgvList.AllowDrop = true;
            this.DgvList.AllowUserToAddRows = false;
            this.DgvList.AllowUserToDeleteRows = false;
            this.DgvList.AllowUserToOrderColumns = true;
            this.DgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvList.Location = new System.Drawing.Point(2, 68);
            this.DgvList.Name = "DgvList";
            this.DgvList.ReadOnly = true;
            this.DgvList.RowHeadersVisible = false;
            this.DgvList.RowTemplate.Height = 23;
            this.DgvList.Size = new System.Drawing.Size(552, 321);
            this.DgvList.TabIndex = 1;
            // 
            // BtnGetData
            // 
            this.BtnGetData.Location = new System.Drawing.Point(269, 11);
            this.BtnGetData.Name = "BtnGetData";
            this.BtnGetData.Size = new System.Drawing.Size(75, 23);
            this.BtnGetData.TabIndex = 0;
            this.BtnGetData.Text = "获取数据";
            this.BtnGetData.UseVisualStyleBackColor = true;
            this.BtnGetData.Click += new System.EventHandler(this.BtnGetData_Click);
            // 
            // DtpDate
            // 
            this.DtpDate.Location = new System.Drawing.Point(99, 13);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(134, 21);
            this.DtpDate.TabIndex = 1;
            // 
            // LabelDataMakeDate
            // 
            this.LabelDataMakeDate.AutoSize = true;
            this.LabelDataMakeDate.Location = new System.Drawing.Point(10, 18);
            this.LabelDataMakeDate.Name = "LabelDataMakeDate";
            this.LabelDataMakeDate.Size = new System.Drawing.Size(83, 12);
            this.LabelDataMakeDate.TabIndex = 2;
            this.LabelDataMakeDate.Text = "数据生成日期:";
            // 
            // BtnData2Excel
            // 
            this.BtnData2Excel.Location = new System.Drawing.Point(381, 10);
            this.BtnData2Excel.Name = "BtnData2Excel";
            this.BtnData2Excel.Size = new System.Drawing.Size(90, 23);
            this.BtnData2Excel.TabIndex = 3;
            this.BtnData2Excel.Text = "导出至Excel";
            this.BtnData2Excel.UseVisualStyleBackColor = true;
            this.BtnData2Excel.Click += new System.EventHandler(this.BtnData2Excel_Click);
            // 
            // 联友物料需求量导出
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 531);
            this.Controls.Add(this.DgvList);
            this.Controls.Add(this.PanelTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "联友物料需求量导出";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "联友物料需求量导出";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.SizeChanged += new System.EventHandler(this.Form_MainResized);
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.Label LabelDataMakeDate;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Button BtnGetData;
        private System.Windows.Forms.DataGridView DgvList;
        private System.Windows.Forms.Button BtnData2Excel;
    }
}

