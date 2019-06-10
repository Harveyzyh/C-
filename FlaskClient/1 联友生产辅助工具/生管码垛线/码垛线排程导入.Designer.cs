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
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.btnShow = new System.Windows.Forms.Button();
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.btnSyncFromPlan = new System.Windows.Forms.Button();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnInput = new System.Windows.Forms.Button();
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
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(622, 135);
            this.DgvMain.TabIndex = 2;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(198, 4);
            this.btnShow.Margin = new System.Windows.Forms.Padding(4);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(177, 30);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "显示全部";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.button_Show_Click);
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.btnSyncFromPlan);
            this.PanelTitle.Controls.Add(this.btnOutput);
            this.PanelTitle.Controls.Add(this.btnInput);
            this.PanelTitle.Controls.Add(this.btnShow);
            this.PanelTitle.Location = new System.Drawing.Point(0, 0);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(622, 38);
            this.PanelTitle.TabIndex = 4;
            // 
            // btnSyncFromPlan
            // 
            this.btnSyncFromPlan.Location = new System.Drawing.Point(382, 4);
            this.btnSyncFromPlan.Name = "btnSyncFromPlan";
            this.btnSyncFromPlan.Size = new System.Drawing.Size(129, 30);
            this.btnSyncFromPlan.TabIndex = 6;
            this.btnSyncFromPlan.Text = "同步自生产排程";
            this.btnSyncFromPlan.UseVisualStyleBackColor = true;
            this.btnSyncFromPlan.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(101, 4);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(90, 30);
            this.btnOutput.TabIndex = 5;
            this.btnOutput.Text = "导出";
            this.btnOutput.UseVisualStyleBackColor = true;
            // 
            // btnInput
            // 
            this.btnInput.Location = new System.Drawing.Point(5, 4);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(90, 30);
            this.btnInput.TabIndex = 4;
            this.btnInput.Text = "导入";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Visible = false;
            this.btnInput.Click += new System.EventHandler(this.button_Input_Click);
            // 
            // 码垛线排程导入
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 181);
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
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.Button btnSyncFromPlan;
    }
}

