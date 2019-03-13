namespace 联友生产辅助工具.生产日报表
{
    partial class 日报表修改
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(日报表修改));
            this.panel_Title = new System.Windows.Forms.Panel();
            this.ButtonReportUpdateXLSelect = new System.Windows.Forms.Button();
            this.ButtonReportUpdateCommit = new System.Windows.Forms.Button();
            this.DtpReportUpdateWorkDate = new System.Windows.Forms.DateTimePicker();
            this.LabelReportUpdateWorkDate = new System.Windows.Forms.Label();
            this.DataGridView_List = new System.Windows.Forms.DataGridView();
            this.panel_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Title
            // 
            this.panel_Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Title.Controls.Add(this.ButtonReportUpdateXLSelect);
            this.panel_Title.Controls.Add(this.ButtonReportUpdateCommit);
            this.panel_Title.Controls.Add(this.DtpReportUpdateWorkDate);
            this.panel_Title.Controls.Add(this.LabelReportUpdateWorkDate);
            this.panel_Title.Font = new System.Drawing.Font("宋体", 12F);
            this.panel_Title.Location = new System.Drawing.Point(0, 0);
            this.panel_Title.Name = "panel_Title";
            this.panel_Title.Size = new System.Drawing.Size(652, 45);
            this.panel_Title.TabIndex = 153;
            // 
            // ButtonReportUpdateXLSelect
            // 
            this.ButtonReportUpdateXLSelect.Location = new System.Drawing.Point(229, 3);
            this.ButtonReportUpdateXLSelect.Name = "ButtonReportUpdateXLSelect";
            this.ButtonReportUpdateXLSelect.Size = new System.Drawing.Size(120, 32);
            this.ButtonReportUpdateXLSelect.TabIndex = 6;
            this.ButtonReportUpdateXLSelect.Text = "选择组别";
            this.ButtonReportUpdateXLSelect.UseVisualStyleBackColor = true;
            this.ButtonReportUpdateXLSelect.Click += new System.EventHandler(this.ButtonReportUpdateXLSelect_Click);
            // 
            // ButtonReportUpdateCommit
            // 
            this.ButtonReportUpdateCommit.Font = new System.Drawing.Font("宋体", 12F);
            this.ButtonReportUpdateCommit.Location = new System.Drawing.Point(354, 3);
            this.ButtonReportUpdateCommit.Name = "ButtonReportUpdateCommit";
            this.ButtonReportUpdateCommit.Size = new System.Drawing.Size(120, 32);
            this.ButtonReportUpdateCommit.TabIndex = 3;
            this.ButtonReportUpdateCommit.Text = "修改保存";
            this.ButtonReportUpdateCommit.UseVisualStyleBackColor = true;
            this.ButtonReportUpdateCommit.Click += new System.EventHandler(this.ButtonReportUpdateCommit_Click);
            // 
            // DtpReportUpdateWorkDate
            // 
            this.DtpReportUpdateWorkDate.CustomFormat = "yyyy-MM-dd";
            this.DtpReportUpdateWorkDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpReportUpdateWorkDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.DtpReportUpdateWorkDate.Location = new System.Drawing.Point(91, 6);
            this.DtpReportUpdateWorkDate.Name = "DtpReportUpdateWorkDate";
            this.DtpReportUpdateWorkDate.Size = new System.Drawing.Size(128, 26);
            this.DtpReportUpdateWorkDate.TabIndex = 1;
            this.DtpReportUpdateWorkDate.TabStop = false;
            this.DtpReportUpdateWorkDate.ValueChanged += new System.EventHandler(this.DtpReportUpdateWorkDate_ValueChanged);
            // 
            // LabelReportUpdateWorkDate
            // 
            this.LabelReportUpdateWorkDate.AutoSize = true;
            this.LabelReportUpdateWorkDate.Location = new System.Drawing.Point(6, 10);
            this.LabelReportUpdateWorkDate.Name = "LabelReportUpdateWorkDate";
            this.LabelReportUpdateWorkDate.Size = new System.Drawing.Size(88, 16);
            this.LabelReportUpdateWorkDate.TabIndex = 0;
            this.LabelReportUpdateWorkDate.Text = "生产日期：";
            // 
            // DataGridView_List
            // 
            this.DataGridView_List.AllowUserToAddRows = false;
            this.DataGridView_List.AllowUserToDeleteRows = false;
            this.DataGridView_List.AllowUserToResizeRows = false;
            this.DataGridView_List.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_List.GridColor = System.Drawing.Color.White;
            this.DataGridView_List.Location = new System.Drawing.Point(0, 46);
            this.DataGridView_List.Name = "DataGridView_List";
            this.DataGridView_List.RowTemplate.Height = 23;
            this.DataGridView_List.Size = new System.Drawing.Size(652, 200);
            this.DataGridView_List.TabIndex = 5;
            this.DataGridView_List.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewReportUpdate_CellValueChanged);
            // 
            // 日报表修改
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1348, 737);
            this.Controls.Add(this.panel_Title);
            this.Controls.Add(this.DataGridView_List);
            this.Font = new System.Drawing.Font("宋体", 13F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "日报表修改";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生产日报表修改";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.SizeChanged += new System.EventHandler(this.Form_MainResized);
            this.panel_Title.ResumeLayout(false);
            this.panel_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Panel panel_Title;
        private System.Windows.Forms.DateTimePicker DtpReportUpdateWorkDate;
        private System.Windows.Forms.Label LabelReportUpdateWorkDate;
        private System.Windows.Forms.Button ButtonReportUpdateCommit;
        private System.Windows.Forms.Button ButtonReportUpdateXLSelect;
        private System.Windows.Forms.DataGridView DataGridView_List;
    }
}

