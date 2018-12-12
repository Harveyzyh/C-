namespace 联友生产辅助工具.生产日报表
{
    partial class 日报表查询
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(日报表查询));
            this.panel_Title = new System.Windows.Forms.Panel();
            this.ButtonReportSelectWG = new System.Windows.Forms.Button();
            this.ComboBoxReportSelectType = new System.Windows.Forms.ComboBox();
            this.LabelReportSelectType2 = new System.Windows.Forms.Label();
            this.ButtonReportSelectLayout = new System.Windows.Forms.Button();
            this.DtpReportSelectEndDate = new System.Windows.Forms.DateTimePicker();
            this.DtpReportSelectStartDate = new System.Windows.Forms.DateTimePicker();
            this.ButtonReportSelectSubmit = new System.Windows.Forms.Button();
            this.LabelReportSelectEndDate = new System.Windows.Forms.Label();
            this.LabelReportSelectStartDate = new System.Windows.Forms.Label();
            this.ComboBoxReportDptType = new System.Windows.Forms.ComboBox();
            this.LabelReportSelectDpt = new System.Windows.Forms.Label();
            this.LabelReportSelectType = new System.Windows.Forms.Label();
            this.DataGridView_List = new System.Windows.Forms.DataGridView();
            this.panel_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Title
            // 
            this.panel_Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Title.Controls.Add(this.ButtonReportSelectWG);
            this.panel_Title.Controls.Add(this.ComboBoxReportSelectType);
            this.panel_Title.Controls.Add(this.LabelReportSelectType2);
            this.panel_Title.Controls.Add(this.ButtonReportSelectLayout);
            this.panel_Title.Controls.Add(this.DtpReportSelectEndDate);
            this.panel_Title.Controls.Add(this.DtpReportSelectStartDate);
            this.panel_Title.Controls.Add(this.ButtonReportSelectSubmit);
            this.panel_Title.Controls.Add(this.LabelReportSelectEndDate);
            this.panel_Title.Controls.Add(this.LabelReportSelectStartDate);
            this.panel_Title.Controls.Add(this.ComboBoxReportDptType);
            this.panel_Title.Controls.Add(this.LabelReportSelectDpt);
            this.panel_Title.Controls.Add(this.LabelReportSelectType);
            this.panel_Title.Font = new System.Drawing.Font("宋体", 12F);
            this.panel_Title.Location = new System.Drawing.Point(2, 1);
            this.panel_Title.Name = "panel_Title";
            this.panel_Title.Size = new System.Drawing.Size(823, 78);
            this.panel_Title.TabIndex = 151;
            // 
            // ButtonReportSelectWG
            // 
            this.ButtonReportSelectWG.Location = new System.Drawing.Point(92, 37);
            this.ButtonReportSelectWG.Name = "ButtonReportSelectWG";
            this.ButtonReportSelectWG.Size = new System.Drawing.Size(85, 29);
            this.ButtonReportSelectWG.TabIndex = 16;
            this.ButtonReportSelectWG.Text = "选择组别";
            this.ButtonReportSelectWG.UseVisualStyleBackColor = true;
            this.ButtonReportSelectWG.Click += new System.EventHandler(this.ButtonReportSelectWG_Click);
            // 
            // ComboBoxReportSelectType
            // 
            this.ComboBoxReportSelectType.FormattingEnabled = true;
            this.ComboBoxReportSelectType.Items.AddRange(new object[] {
            "全部",
            "组别-系列",
            "部门-组别-系列"});
            this.ComboBoxReportSelectType.Location = new System.Drawing.Point(383, 41);
            this.ComboBoxReportSelectType.Name = "ComboBoxReportSelectType";
            this.ComboBoxReportSelectType.Size = new System.Drawing.Size(147, 24);
            this.ComboBoxReportSelectType.TabIndex = 15;
            // 
            // LabelReportSelectType2
            // 
            this.LabelReportSelectType2.AutoSize = true;
            this.LabelReportSelectType2.Location = new System.Drawing.Point(303, 45);
            this.LabelReportSelectType2.Name = "LabelReportSelectType2";
            this.LabelReportSelectType2.Size = new System.Drawing.Size(88, 16);
            this.LabelReportSelectType2.TabIndex = 14;
            this.LabelReportSelectType2.Text = "汇总方式：";
            // 
            // ButtonReportSelectLayout
            // 
            this.ButtonReportSelectLayout.Location = new System.Drawing.Point(688, 40);
            this.ButtonReportSelectLayout.Name = "ButtonReportSelectLayout";
            this.ButtonReportSelectLayout.Size = new System.Drawing.Size(89, 29);
            this.ButtonReportSelectLayout.TabIndex = 13;
            this.ButtonReportSelectLayout.TabStop = false;
            this.ButtonReportSelectLayout.Text = "导出Excel";
            this.ButtonReportSelectLayout.UseVisualStyleBackColor = true;
            this.ButtonReportSelectLayout.Click += new System.EventHandler(this.ButtonReportSelectLeyout_Click);
            // 
            // DtpReportSelectEndDate
            // 
            this.DtpReportSelectEndDate.CustomFormat = "yyyy-MM-dd";
            this.DtpReportSelectEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpReportSelectEndDate.Location = new System.Drawing.Point(630, 4);
            this.DtpReportSelectEndDate.Name = "DtpReportSelectEndDate";
            this.DtpReportSelectEndDate.ShowCheckBox = true;
            this.DtpReportSelectEndDate.Size = new System.Drawing.Size(147, 26);
            this.DtpReportSelectEndDate.TabIndex = 12;
            // 
            // DtpReportSelectStartDate
            // 
            this.DtpReportSelectStartDate.CustomFormat = "yyyy-MM-dd";
            this.DtpReportSelectStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpReportSelectStartDate.Location = new System.Drawing.Point(383, 4);
            this.DtpReportSelectStartDate.Name = "DtpReportSelectStartDate";
            this.DtpReportSelectStartDate.ShowCheckBox = true;
            this.DtpReportSelectStartDate.Size = new System.Drawing.Size(147, 26);
            this.DtpReportSelectStartDate.TabIndex = 11;
            this.DtpReportSelectStartDate.TabStop = false;
            // 
            // ButtonReportSelectSubmit
            // 
            this.ButtonReportSelectSubmit.Font = new System.Drawing.Font("宋体", 12F);
            this.ButtonReportSelectSubmit.Location = new System.Drawing.Point(593, 41);
            this.ButtonReportSelectSubmit.Name = "ButtonReportSelectSubmit";
            this.ButtonReportSelectSubmit.Size = new System.Drawing.Size(89, 28);
            this.ButtonReportSelectSubmit.TabIndex = 9;
            this.ButtonReportSelectSubmit.Text = "确定";
            this.ButtonReportSelectSubmit.UseVisualStyleBackColor = true;
            this.ButtonReportSelectSubmit.Click += new System.EventHandler(this.ButtonSelectSubmit_Click);
            // 
            // LabelReportSelectEndDate
            // 
            this.LabelReportSelectEndDate.AutoSize = true;
            this.LabelReportSelectEndDate.Font = new System.Drawing.Font("宋体", 12F);
            this.LabelReportSelectEndDate.Location = new System.Drawing.Point(549, 9);
            this.LabelReportSelectEndDate.Name = "LabelReportSelectEndDate";
            this.LabelReportSelectEndDate.Size = new System.Drawing.Size(88, 16);
            this.LabelReportSelectEndDate.TabIndex = 4;
            this.LabelReportSelectEndDate.Text = "结束时间：";
            // 
            // LabelReportSelectStartDate
            // 
            this.LabelReportSelectStartDate.AutoSize = true;
            this.LabelReportSelectStartDate.Font = new System.Drawing.Font("宋体", 12F);
            this.LabelReportSelectStartDate.Location = new System.Drawing.Point(302, 9);
            this.LabelReportSelectStartDate.Name = "LabelReportSelectStartDate";
            this.LabelReportSelectStartDate.Size = new System.Drawing.Size(88, 16);
            this.LabelReportSelectStartDate.TabIndex = 3;
            this.LabelReportSelectStartDate.Text = "开始时间：";
            // 
            // ComboBoxReportDptType
            // 
            this.ComboBoxReportDptType.Font = new System.Drawing.Font("宋体", 12F);
            this.ComboBoxReportDptType.FormattingEnabled = true;
            this.ComboBoxReportDptType.Items.AddRange(new object[] {
            "生产一部",
            "生产二部",
            "生产三部",
            "生产四部",
            "生产五部",
            "全部"});
            this.ComboBoxReportDptType.Location = new System.Drawing.Point(167, 5);
            this.ComboBoxReportDptType.Name = "ComboBoxReportDptType";
            this.ComboBoxReportDptType.Size = new System.Drawing.Size(121, 24);
            this.ComboBoxReportDptType.TabIndex = 5;
            this.ComboBoxReportDptType.TabStop = false;
            // 
            // LabelReportSelectDpt
            // 
            this.LabelReportSelectDpt.AutoSize = true;
            this.LabelReportSelectDpt.Font = new System.Drawing.Font("宋体", 12F);
            this.LabelReportSelectDpt.Location = new System.Drawing.Point(89, 8);
            this.LabelReportSelectDpt.Name = "LabelReportSelectDpt";
            this.LabelReportSelectDpt.Size = new System.Drawing.Size(88, 16);
            this.LabelReportSelectDpt.TabIndex = 1;
            this.LabelReportSelectDpt.Text = "生产部门：";
            // 
            // LabelReportSelectType
            // 
            this.LabelReportSelectType.AutoSize = true;
            this.LabelReportSelectType.Font = new System.Drawing.Font("宋体", 12F);
            this.LabelReportSelectType.Location = new System.Drawing.Point(9, 8);
            this.LabelReportSelectType.Name = "LabelReportSelectType";
            this.LabelReportSelectType.Size = new System.Drawing.Size(88, 16);
            this.LabelReportSelectType.TabIndex = 0;
            this.LabelReportSelectType.Text = "查询条件：";
            // 
            // DataGridView_List
            // 
            this.DataGridView_List.AllowUserToAddRows = false;
            this.DataGridView_List.AllowUserToDeleteRows = false;
            this.DataGridView_List.AllowUserToOrderColumns = true;
            this.DataGridView_List.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_List.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.DataGridView_List.Location = new System.Drawing.Point(2, 85);
            this.DataGridView_List.Name = "DataGridView_List";
            this.DataGridView_List.ReadOnly = true;
            this.DataGridView_List.RowTemplate.Height = 23;
            this.DataGridView_List.Size = new System.Drawing.Size(823, 111);
            this.DataGridView_List.TabIndex = 10;
            this.DataGridView_List.TabStop = false;
            // 
            // 日报表查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 356);
            this.Controls.Add(this.panel_Title);
            this.Controls.Add(this.DataGridView_List);
            this.Font = new System.Drawing.Font("宋体", 13F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "日报表查询";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生产日报表查询";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.SizeChanged += new System.EventHandler(this.FormMain_Resized);
            this.panel_Title.ResumeLayout(false);
            this.panel_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel_Title;
        private System.Windows.Forms.ComboBox ComboBoxReportDptType;
        private System.Windows.Forms.Label LabelReportSelectType;
        private System.Windows.Forms.Label LabelReportSelectEndDate;
        private System.Windows.Forms.Label LabelReportSelectStartDate;
        private System.Windows.Forms.Label LabelReportSelectDpt;
        private System.Windows.Forms.Button ButtonReportSelectSubmit;
        private System.Windows.Forms.DateTimePicker DtpReportSelectEndDate;
        private System.Windows.Forms.DateTimePicker DtpReportSelectStartDate;
        private System.Windows.Forms.Button ButtonReportSelectLayout;
        private System.Windows.Forms.ComboBox ComboBoxReportSelectType;
        private System.Windows.Forms.Label LabelReportSelectType2;
        private System.Windows.Forms.Button ButtonReportSelectWG;
        private System.Windows.Forms.DataGridView DataGridView_List;
    }
}

