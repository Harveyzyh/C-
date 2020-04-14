namespace 联友中山分公司生产辅助工具
{
    partial class 主界面
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(主界面));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.生产入库领料明细ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关闭当前界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelParent = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.生产入库领料明细ToolStripMenuItem,
            this.关闭当前界面ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1155, 26);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 生产入库领料明细ToolStripMenuItem
            // 
            this.生产入库领料明细ToolStripMenuItem.Name = "生产入库领料明细ToolStripMenuItem";
            this.生产入库领料明细ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.生产入库领料明细ToolStripMenuItem.Text = "生产入库领料明细";
            this.生产入库领料明细ToolStripMenuItem.Click += new System.EventHandler(this.生产入库领料明细ToolStripMenuItem_Click);
            // 
            // 关闭当前界面ToolStripMenuItem
            // 
            this.关闭当前界面ToolStripMenuItem.Name = "关闭当前界面ToolStripMenuItem";
            this.关闭当前界面ToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.关闭当前界面ToolStripMenuItem.Text = "关闭当前界面";
            this.关闭当前界面ToolStripMenuItem.Visible = false;
            this.关闭当前界面ToolStripMenuItem.Click += new System.EventHandler(this.关闭当前界面ToolStripMenuItem_Click);
            // 
            // panelParent
            // 
            this.panelParent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelParent.Location = new System.Drawing.Point(1, 29);
            this.panelParent.Margin = new System.Windows.Forms.Padding(4);
            this.panelParent.Name = "panelParent";
            this.panelParent.Size = new System.Drawing.Size(267, 125);
            this.panelParent.TabIndex = 2;
            // 
            // 主界面
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 639);
            this.Controls.Add(this.panelParent);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "主界面";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "联友中山分公司生产辅助工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Resize += new System.EventHandler(this.Form_MainResized);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panelParent;
        private System.Windows.Forms.ToolStripMenuItem 关闭当前界面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生产入库领料明细ToolStripMenuItem;
    }
}