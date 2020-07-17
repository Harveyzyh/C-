namespace 联友生产辅助工具.生管码垛线
{
    partial class 码垛线排程导入
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
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.btnShow = new System.Windows.Forms.Button();
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSyncBoxCode = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSyncFromPlan = new System.Windows.Forms.Button();
            this.btnOutput = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.PanelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.AllowUserToOrderColumns = true;
            this.DgvMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(0, 37);
            this.DgvMain.Margin = new System.Windows.Forms.Padding(4);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvMain.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(622, 135);
            this.DgvMain.TabIndex = 2;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(104, 4);
            this.btnShow.Margin = new System.Windows.Forms.Padding(4);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(131, 30);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "显示全部";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.button_Show_Click);
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Controls.Add(this.btnSyncBoxCode);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Controls.Add(this.btnSyncFromPlan);
            this.PanelTitle.Controls.Add(this.btnOutput);
            this.PanelTitle.Controls.Add(this.btnShow);
            this.PanelTitle.Location = new System.Drawing.Point(0, 0);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(858, 38);
            this.PanelTitle.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(522, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "同步或更新用时比较久";
            // 
            // btnSyncBoxCode
            // 
            this.btnSyncBoxCode.Location = new System.Drawing.Point(390, 4);
            this.btnSyncBoxCode.Name = "btnSyncBoxCode";
            this.btnSyncBoxCode.Size = new System.Drawing.Size(125, 30);
            this.btnSyncBoxCode.TabIndex = 8;
            this.btnSyncBoxCode.Text = "只更新纸箱信息";
            this.btnSyncBoxCode.UseVisualStyleBackColor = true;
            this.btnSyncBoxCode.Click += new System.EventHandler(this.btnSyncBoxCode_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(700, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "正在同步数据，请稍等";
            this.label1.Visible = false;
            // 
            // btnSyncFromPlan
            // 
            this.btnSyncFromPlan.Location = new System.Drawing.Point(245, 4);
            this.btnSyncFromPlan.Name = "btnSyncFromPlan";
            this.btnSyncFromPlan.Size = new System.Drawing.Size(129, 30);
            this.btnSyncFromPlan.TabIndex = 6;
            this.btnSyncFromPlan.Text = "同步自生产排程";
            this.btnSyncFromPlan.UseVisualStyleBackColor = true;
            this.btnSyncFromPlan.Click += new System.EventHandler(this.btnSyncFromPlan_Click);
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(7, 4);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(90, 30);
            this.btnOutput.TabIndex = 5;
            this.btnOutput.Text = "导出";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // 码垛线排程导入
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 311);
            this.Controls.Add(this.PanelTitle);
            this.Controls.Add(this.DgvMain);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "码垛线排程导入";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "码垛线排程导入";
            this.SizeChanged += new System.EventHandler(this.FormMain_Resized);
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnSyncFromPlan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSyncBoxCode;
        private System.Windows.Forms.Label label2;
    }
}

