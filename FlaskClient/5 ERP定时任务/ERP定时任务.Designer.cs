namespace ERP定时任务
{
    partial class ERP定时任务
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ERP定时任务));
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnAutoLrpRun = new System.Windows.Forms.Button();
            this.BtnSingleUsages = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxLog
            // 
            this.textBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxLog.Location = new System.Drawing.Point(0, 0);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.Size = new System.Drawing.Size(814, 477);
            this.textBoxLog.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtnSingleUsages);
            this.panel1.Controls.Add(this.BtnAutoLrpRun);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 477);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(814, 38);
            this.panel1.TabIndex = 2;
            // 
            // BtnAutoLrpRun
            // 
            this.BtnAutoLrpRun.Location = new System.Drawing.Point(11, 5);
            this.BtnAutoLrpRun.Name = "BtnAutoLrpRun";
            this.BtnAutoLrpRun.Size = new System.Drawing.Size(140, 23);
            this.BtnAutoLrpRun.TabIndex = 0;
            this.BtnAutoLrpRun.Text = "立即执行LRP计划作业";
            this.BtnAutoLrpRun.UseVisualStyleBackColor = true;
            this.BtnAutoLrpRun.Click += new System.EventHandler(this.BtnAutoLrpRun_Click);
            // 
            // BtnSingleUsages
            // 
            this.BtnSingleUsages.Location = new System.Drawing.Point(184, 4);
            this.BtnSingleUsages.Name = "BtnSingleUsages";
            this.BtnSingleUsages.Size = new System.Drawing.Size(75, 23);
            this.BtnSingleUsages.TabIndex = 1;
            this.BtnSingleUsages.Text = "计算单用量";
            this.BtnSingleUsages.UseVisualStyleBackColor = true;
            // 
            // ERP定时任务
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 515);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Name = "ERP定时任务";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ERP定时任务";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ERP定时任务_FormClosing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnAutoLrpRun;
        private System.Windows.Forms.Button BtnSingleUsages;
    }
}

