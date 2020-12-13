namespace HarveyZ.生管排程
{
    partial class 未排订单查询
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(未排订单查询));
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.BtnShow = new System.Windows.Forms.Button();
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.labelDdSlSum = new System.Windows.Forms.Label();
            this.TxBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxBoxOrder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.BtnOutput = new System.Windows.Forms.Button();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.LabelReportSelectEndDate = new System.Windows.Forms.Label();
            this.LabelReportSelectStartDate = new System.Windows.Forms.Label();
            this.contextMenuStrip_DgvMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.PanelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // DgvMain
            // 
            this.DgvMain.AllowDrop = true;
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.AllowUserToOrderColumns = true;
            this.DgvMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(0, 82);
            this.DgvMain.Margin = new System.Windows.Forms.Padding(4);
            this.DgvMain.MultiSelect = false;
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowHeadersVisible = false;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(978, 249);
            this.DgvMain.TabIndex = 2;
            this.DgvMain.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvMain_CellMouseDown);
            // 
            // BtnShow
            // 
            this.BtnShow.Location = new System.Drawing.Point(3, 37);
            this.BtnShow.Margin = new System.Windows.Forms.Padding(4);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(90, 30);
            this.BtnShow.TabIndex = 3;
            this.BtnShow.Text = "查询";
            this.BtnShow.UseVisualStyleBackColor = true;
            this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.labelDdSlSum);
            this.PanelTitle.Controls.Add(this.TxBoxName);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Controls.Add(this.TxBoxOrder);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Controls.Add(this.DtpEndDate);
            this.PanelTitle.Controls.Add(this.BtnOutput);
            this.PanelTitle.Controls.Add(this.DtpStartDate);
            this.PanelTitle.Controls.Add(this.BtnShow);
            this.PanelTitle.Controls.Add(this.LabelReportSelectEndDate);
            this.PanelTitle.Controls.Add(this.LabelReportSelectStartDate);
            this.PanelTitle.Location = new System.Drawing.Point(0, 0);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(1126, 75);
            this.PanelTitle.TabIndex = 4;
            // 
            // labelDdSlSum
            // 
            this.labelDdSlSum.AutoSize = true;
            this.labelDdSlSum.Location = new System.Drawing.Point(575, 44);
            this.labelDdSlSum.Name = "labelDdSlSum";
            this.labelDdSlSum.Size = new System.Drawing.Size(97, 15);
            this.labelDdSlSum.TabIndex = 25;
            this.labelDdSlSum.Text = "订单总数量：";
            // 
            // TxBoxName
            // 
            this.TxBoxName.Location = new System.Drawing.Point(419, 37);
            this.TxBoxName.Name = "TxBoxName";
            this.TxBoxName.Size = new System.Drawing.Size(138, 24);
            this.TxBoxName.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(370, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "品名：";
            // 
            // TxBoxOrder
            // 
            this.TxBoxOrder.Location = new System.Drawing.Point(208, 37);
            this.TxBoxOrder.Name = "TxBoxOrder";
            this.TxBoxOrder.Size = new System.Drawing.Size(147, 24);
            this.TxBoxOrder.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11F);
            this.label1.Location = new System.Drawing.Point(129, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "订单号：";
            // 
            // DtpEndDate
            // 
            this.DtpEndDate.CustomFormat = "yyyy-MM-dd";
            this.DtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEndDate.Location = new System.Drawing.Point(452, 6);
            this.DtpEndDate.Name = "DtpEndDate";
            this.DtpEndDate.ShowCheckBox = true;
            this.DtpEndDate.Size = new System.Drawing.Size(147, 24);
            this.DtpEndDate.TabIndex = 19;
            // 
            // BtnOutput
            // 
            this.BtnOutput.Location = new System.Drawing.Point(3, 3);
            this.BtnOutput.Name = "BtnOutput";
            this.BtnOutput.Size = new System.Drawing.Size(90, 30);
            this.BtnOutput.TabIndex = 5;
            this.BtnOutput.Text = "导出";
            this.BtnOutput.UseVisualStyleBackColor = true;
            this.BtnOutput.Click += new System.EventHandler(this.BtnOutput_Click);
            // 
            // DtpStartDate
            // 
            this.DtpStartDate.CustomFormat = "yyyy-MM-dd";
            this.DtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStartDate.Location = new System.Drawing.Point(210, 6);
            this.DtpStartDate.Name = "DtpStartDate";
            this.DtpStartDate.ShowCheckBox = true;
            this.DtpStartDate.Size = new System.Drawing.Size(147, 24);
            this.DtpStartDate.TabIndex = 18;
            this.DtpStartDate.TabStop = false;
            // 
            // LabelReportSelectEndDate
            // 
            this.LabelReportSelectEndDate.AutoSize = true;
            this.LabelReportSelectEndDate.Font = new System.Drawing.Font("宋体", 11F);
            this.LabelReportSelectEndDate.Location = new System.Drawing.Point(373, 9);
            this.LabelReportSelectEndDate.Name = "LabelReportSelectEndDate";
            this.LabelReportSelectEndDate.Size = new System.Drawing.Size(82, 15);
            this.LabelReportSelectEndDate.TabIndex = 16;
            this.LabelReportSelectEndDate.Text = "结束日期：";
            // 
            // LabelReportSelectStartDate
            // 
            this.LabelReportSelectStartDate.AutoSize = true;
            this.LabelReportSelectStartDate.Font = new System.Drawing.Font("宋体", 11F);
            this.LabelReportSelectStartDate.Location = new System.Drawing.Point(129, 9);
            this.LabelReportSelectStartDate.Name = "LabelReportSelectStartDate";
            this.LabelReportSelectStartDate.Size = new System.Drawing.Size(82, 15);
            this.LabelReportSelectStartDate.TabIndex = 15;
            this.LabelReportSelectStartDate.Text = "起始日期：";
            // 
            // contextMenuStrip_DgvMain
            // 
            this.contextMenuStrip_DgvMain.Name = "contextMenuStrip_DgvMain";
            this.contextMenuStrip_DgvMain.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStrip_DgvMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_DgvMain_ItemClicked);
            // 
            // 未排订单查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 396);
            this.Controls.Add(this.PanelTitle);
            this.Controls.Add(this.DgvMain);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "未排订单查询";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生管排程导入";
            this.SizeChanged += new System.EventHandler(this.FormMain_Resized);
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button BtnShow;
        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.Button BtnOutput;
        private System.Windows.Forms.DateTimePicker DtpEndDate;
        private System.Windows.Forms.DateTimePicker DtpStartDate;
        private System.Windows.Forms.Label LabelReportSelectEndDate;
        private System.Windows.Forms.Label LabelReportSelectStartDate;
        private System.Windows.Forms.TextBox TxBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxBoxOrder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelDdSlSum;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_DgvMain;
    }
}

