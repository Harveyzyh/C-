namespace 联友中山分公司生产辅助工具
{
    partial class 生成出货排程
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.BtnGenerate2 = new System.Windows.Forms.Button();
            this.BtnGenerate = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.BtnLayout = new System.Windows.Forms.Button();
            this.BtnShow = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.BtnGenerate2);
            this.PanelTitle.Controls.Add(this.BtnGenerate);
            this.PanelTitle.Controls.Add(this.BtnAdd);
            this.PanelTitle.Controls.Add(this.BtnLayout);
            this.PanelTitle.Controls.Add(this.BtnShow);
            this.PanelTitle.Controls.Add(this.textBox1);
            this.PanelTitle.Controls.Add(this.label3);
            this.PanelTitle.Controls.Add(this.DtpEndDate);
            this.PanelTitle.Controls.Add(this.DtpStartDate);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Location = new System.Drawing.Point(1, 1);
            this.PanelTitle.Margin = new System.Windows.Forms.Padding(4);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(1374, 76);
            this.PanelTitle.TabIndex = 0;
            // 
            // BtnGenerate2
            // 
            this.BtnGenerate2.Location = new System.Drawing.Point(795, 43);
            this.BtnGenerate2.Name = "BtnGenerate2";
            this.BtnGenerate2.Size = new System.Drawing.Size(112, 23);
            this.BtnGenerate2.TabIndex = 10;
            this.BtnGenerate2.Text = "生成交货排程";
            this.BtnGenerate2.UseVisualStyleBackColor = true;
            this.BtnGenerate2.Click += new System.EventHandler(this.BtnGenerate2_Click);
            // 
            // BtnGenerate
            // 
            this.BtnGenerate.Location = new System.Drawing.Point(662, 42);
            this.BtnGenerate.Name = "BtnGenerate";
            this.BtnGenerate.Size = new System.Drawing.Size(113, 23);
            this.BtnGenerate.TabIndex = 9;
            this.BtnGenerate.Text = "生成3308排程";
            this.BtnGenerate.UseVisualStyleBackColor = true;
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(549, 43);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 23);
            this.BtnAdd.TabIndex = 8;
            this.BtnAdd.Text = "新增排程";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnLayout
            // 
            this.BtnLayout.Location = new System.Drawing.Point(662, 6);
            this.BtnLayout.Name = "BtnLayout";
            this.BtnLayout.Size = new System.Drawing.Size(75, 23);
            this.BtnLayout.TabIndex = 7;
            this.BtnLayout.Text = "导出";
            this.BtnLayout.UseVisualStyleBackColor = true;
            this.BtnLayout.Visible = false;
            this.BtnLayout.Click += new System.EventHandler(this.BtnLayout_Click);
            // 
            // BtnShow
            // 
            this.BtnShow.Location = new System.Drawing.Point(549, 6);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(75, 23);
            this.BtnShow.TabIndex = 6;
            this.BtnShow.Text = "查询";
            this.BtnShow.UseVisualStyleBackColor = true;
            this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(127, 41);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(379, 24);
            this.textBox1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "生产单号：";
            // 
            // DtpEndDate
            // 
            this.DtpEndDate.CustomFormat = "yyyy年MM月dd日";
            this.DtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEndDate.Location = new System.Drawing.Point(334, 7);
            this.DtpEndDate.Name = "DtpEndDate";
            this.DtpEndDate.ShowCheckBox = true;
            this.DtpEndDate.Size = new System.Drawing.Size(172, 24);
            this.DtpEndDate.TabIndex = 3;
            // 
            // DtpStartDate
            // 
            this.DtpStartDate.CustomFormat = "yyyy年MM月dd日";
            this.DtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStartDate.Location = new System.Drawing.Point(127, 7);
            this.DtpStartDate.Name = "DtpStartDate";
            this.DtpStartDate.ShowCheckBox = true;
            this.DtpStartDate.Size = new System.Drawing.Size(174, 24);
            this.DtpStartDate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "出货排程日期:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(301, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "---";
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DgvMain.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(1, 85);
            this.DgvMain.Margin = new System.Windows.Forms.Padding(4);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowHeadersVisible = false;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvMain.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(320, 188);
            this.DgvMain.TabIndex = 1;
            this.DgvMain.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvMain_CellMouseDoubleClick);
            // 
            // 生成出货排程
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 739);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "生成出货排程";
            this.Text = "分公司一键生成出货排程";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Resize += new System.EventHandler(this.Form_MainResized);
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.DateTimePicker DtpEndDate;
        private System.Windows.Forms.DateTimePicker DtpStartDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnLayout;
        private System.Windows.Forms.Button BtnShow;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnGenerate2;
        private System.Windows.Forms.Button BtnGenerate;
        private System.Windows.Forms.Button BtnAdd;
    }
}

