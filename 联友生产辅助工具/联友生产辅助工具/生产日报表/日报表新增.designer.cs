namespace 联友生产辅助工具.生产日报表
{
    partial class 日报表新增
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(日报表新增));
            this.panel_Tile = new System.Windows.Forms.Panel();
            this.DtpReportInputWorkDate = new System.Windows.Forms.DateTimePicker();
            this.ButtonReportInputXLSelect = new System.Windows.Forms.Button();
            this.ButtonReportInputCommit = new System.Windows.Forms.Button();
            this.DataGridView_List = new System.Windows.Forms.DataGridView();
            this.LabelReportWorkDateInput = new System.Windows.Forms.Label();
            this.panel_Tile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Tile
            // 
            this.panel_Tile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Tile.Controls.Add(this.DtpReportInputWorkDate);
            this.panel_Tile.Controls.Add(this.ButtonReportInputXLSelect);
            this.panel_Tile.Controls.Add(this.ButtonReportInputCommit);
            this.panel_Tile.Controls.Add(this.LabelReportWorkDateInput);
            this.panel_Tile.Font = new System.Drawing.Font("宋体", 12F);
            this.panel_Tile.Location = new System.Drawing.Point(1, 1);
            this.panel_Tile.Margin = new System.Windows.Forms.Padding(4);
            this.panel_Tile.Name = "panel_Tile";
            this.panel_Tile.Size = new System.Drawing.Size(599, 45);
            this.panel_Tile.TabIndex = 153;
            // 
            // DtpReportInputWorkDate
            // 
            this.DtpReportInputWorkDate.CustomFormat = "yyyy-MM-dd";
            this.DtpReportInputWorkDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpReportInputWorkDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.DtpReportInputWorkDate.Location = new System.Drawing.Point(85, 7);
            this.DtpReportInputWorkDate.Margin = new System.Windows.Forms.Padding(4);
            this.DtpReportInputWorkDate.Name = "DtpReportInputWorkDate";
            this.DtpReportInputWorkDate.Size = new System.Drawing.Size(137, 26);
            this.DtpReportInputWorkDate.TabIndex = 3;
            this.DtpReportInputWorkDate.TabStop = false;
            this.DtpReportInputWorkDate.ValueChanged += new System.EventHandler(this.DtpReportInputWorkDate_ValueChanged);
            // 
            // ButtonReportInputXLSelect
            // 
            this.ButtonReportInputXLSelect.Location = new System.Drawing.Point(240, 4);
            this.ButtonReportInputXLSelect.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonReportInputXLSelect.Name = "ButtonReportInputXLSelect";
            this.ButtonReportInputXLSelect.Size = new System.Drawing.Size(120, 32);
            this.ButtonReportInputXLSelect.TabIndex = 2;
            this.ButtonReportInputXLSelect.TabStop = false;
            this.ButtonReportInputXLSelect.Text = "选择组别";
            this.ButtonReportInputXLSelect.UseVisualStyleBackColor = true;
            this.ButtonReportInputXLSelect.Click += new System.EventHandler(this.ButtonReportInputXLSelect_Click);
            // 
            // ButtonReportInputCommit
            // 
            this.ButtonReportInputCommit.Location = new System.Drawing.Point(368, 4);
            this.ButtonReportInputCommit.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonReportInputCommit.Name = "ButtonReportInputCommit";
            this.ButtonReportInputCommit.Size = new System.Drawing.Size(120, 32);
            this.ButtonReportInputCommit.TabIndex = 1;
            this.ButtonReportInputCommit.TabStop = false;
            this.ButtonReportInputCommit.Text = "新增保存";
            this.ButtonReportInputCommit.UseVisualStyleBackColor = true;
            this.ButtonReportInputCommit.Click += new System.EventHandler(this.ButtonReportInputCommit_Click);
            // 
            // DataGridView_List
            // 
            this.DataGridView_List.AllowUserToAddRows = false;
            this.DataGridView_List.AllowUserToDeleteRows = false;
            this.DataGridView_List.AllowUserToOrderColumns = true;
            this.DataGridView_List.AllowUserToResizeRows = false;
            this.DataGridView_List.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.DataGridView_List.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_List.GridColor = System.Drawing.Color.White;
            this.DataGridView_List.Location = new System.Drawing.Point(1, 48);
            this.DataGridView_List.Margin = new System.Windows.Forms.Padding(4);
            this.DataGridView_List.Name = "DataGridView_List";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView_List.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridView_List.RowTemplate.Height = 23;
            this.DataGridView_List.Size = new System.Drawing.Size(599, 145);
            this.DataGridView_List.TabIndex = 0;
            this.DataGridView_List.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewReportInput_CellValueChanged);
            // 
            // LabelReportWorkDateInput
            // 
            this.LabelReportWorkDateInput.AutoSize = true;
            this.LabelReportWorkDateInput.Location = new System.Drawing.Point(5, 10);
            this.LabelReportWorkDateInput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelReportWorkDateInput.Name = "LabelReportWorkDateInput";
            this.LabelReportWorkDateInput.Size = new System.Drawing.Size(88, 16);
            this.LabelReportWorkDateInput.TabIndex = 4;
            this.LabelReportWorkDateInput.Text = "生产日期：";
            // 
            // 日报表新增
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1444, 776);
            this.Controls.Add(this.panel_Tile);
            this.Controls.Add(this.DataGridView_List);
            this.Font = new System.Drawing.Font("宋体", 13F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "日报表新增";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "联友生产辅助工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.SizeChanged += new System.EventHandler(this.Form_MainResized);
            this.panel_Tile.ResumeLayout(false);
            this.panel_Tile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel_Tile;
        private System.Windows.Forms.DataGridView DataGridView_List;
        private System.Windows.Forms.Button ButtonReportInputCommit;
        private System.Windows.Forms.Button ButtonReportInputXLSelect;
        private System.Windows.Forms.DateTimePicker DtpReportInputWorkDate;
        private System.Windows.Forms.Label LabelReportWorkDateInput;
    }
}

