﻿namespace HarveyZ
{
    partial class 主界面
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(主界面));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.供应商ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.供应商_录入送货单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.管理_权限管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关闭当前界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.此用户没有任何权限ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelParent = new System.Windows.Forms.Panel();
            this.Label_Test = new System.Windows.Forms.Label();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusLabelUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabelLocalConn = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabelIP = new System.Windows.Forms.ToolStripStatusLabel();
            this.生成领料单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.管理_修改密码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.管理_添加用户ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.供应商ToolStripMenuItem,
            this.管理ToolStripMenuItem,
            this.关闭当前界面ToolStripMenuItem,
            this.此用户没有任何权限ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1185, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 供应商ToolStripMenuItem
            // 
            this.供应商ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.供应商_录入送货单ToolStripMenuItem});
            this.供应商ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.供应商ToolStripMenuItem.Name = "供应商ToolStripMenuItem";
            this.供应商ToolStripMenuItem.Size = new System.Drawing.Size(65, 22);
            this.供应商ToolStripMenuItem.Text = "供应商";
            // 
            // 供应商_录入送货单ToolStripMenuItem
            // 
            this.供应商_录入送货单ToolStripMenuItem.Name = "供应商_录入送货单ToolStripMenuItem";
            this.供应商_录入送货单ToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.供应商_录入送货单ToolStripMenuItem.Text = "录入送货单";
            this.供应商_录入送货单ToolStripMenuItem.Click += new System.EventHandler(this.供应商_录入送货单ToolStripMenuItem_Click);
            // 
            // 管理ToolStripMenuItem
            // 
            this.管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.管理_添加用户ToolStripMenuItem,
            this.管理_修改密码ToolStripMenuItem,
            this.管理_权限管理ToolStripMenuItem});
            this.管理ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.管理ToolStripMenuItem.Name = "管理ToolStripMenuItem";
            this.管理ToolStripMenuItem.Size = new System.Drawing.Size(50, 22);
            this.管理ToolStripMenuItem.Text = "管理";
            // 
            // 管理_权限管理ToolStripMenuItem
            // 
            this.管理_权限管理ToolStripMenuItem.Name = "管理_权限管理ToolStripMenuItem";
            this.管理_权限管理ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.管理_权限管理ToolStripMenuItem.Text = "权限管理";
            this.管理_权限管理ToolStripMenuItem.Click += new System.EventHandler(this.管理_权限管理ToolStripMenuItem_Click);
            // 
            // 关闭当前界面ToolStripMenuItem
            // 
            this.关闭当前界面ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.关闭当前界面ToolStripMenuItem.Name = "关闭当前界面ToolStripMenuItem";
            this.关闭当前界面ToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.关闭当前界面ToolStripMenuItem.Text = "关闭当前界面";
            this.关闭当前界面ToolStripMenuItem.Click += new System.EventHandler(this.关闭当前界面ToolStripMenuItem_Click);
            // 
            // 此用户没有任何权限ToolStripMenuItem
            // 
            this.此用户没有任何权限ToolStripMenuItem.Enabled = false;
            this.此用户没有任何权限ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.此用户没有任何权限ToolStripMenuItem.Name = "此用户没有任何权限ToolStripMenuItem";
            this.此用户没有任何权限ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.此用户没有任何权限ToolStripMenuItem.Text = "此用户没有任何权限";
            this.此用户没有任何权限ToolStripMenuItem.Visible = false;
            // 
            // panelParent
            // 
            this.panelParent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelParent.Font = new System.Drawing.Font("宋体", 9F);
            this.panelParent.Location = new System.Drawing.Point(1, 31);
            this.panelParent.Name = "panelParent";
            this.panelParent.Size = new System.Drawing.Size(841, 390);
            this.panelParent.TabIndex = 153;
            // 
            // Label_Test
            // 
            this.Label_Test.AutoSize = true;
            this.Label_Test.Location = new System.Drawing.Point(481, 9);
            this.Label_Test.Name = "Label_Test";
            this.Label_Test.Size = new System.Drawing.Size(0, 18);
            this.Label_Test.TabIndex = 154;
            // 
            // statusBar
            // 
            this.statusBar.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelUser,
            this.statusLabelLocalConn,
            this.statusLabelIP});
            this.statusBar.Location = new System.Drawing.Point(0, 596);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1185, 26);
            this.statusBar.TabIndex = 156;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusLabelUser
            // 
            this.statusLabelUser.AutoSize = false;
            this.statusLabelUser.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusLabelUser.Name = "statusLabelUser";
            this.statusLabelUser.Size = new System.Drawing.Size(100, 21);
            this.statusLabelUser.Text = "statusLabelUser";
            // 
            // statusLabelLocalConn
            // 
            this.statusLabelLocalConn.AutoSize = false;
            this.statusLabelLocalConn.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusLabelLocalConn.Name = "statusLabelLocalConn";
            this.statusLabelLocalConn.Size = new System.Drawing.Size(133, 21);
            this.statusLabelLocalConn.Text = "statusLabelLocalConn";
            // 
            // statusLabelIP
            // 
            this.statusLabelIP.AutoSize = false;
            this.statusLabelIP.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusLabelIP.Name = "statusLabelIP";
            this.statusLabelIP.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusLabelIP.Size = new System.Drawing.Size(88, 21);
            this.statusLabelIP.Text = "statusLabelIP";
            // 
            // 生成领料单ToolStripMenuItem
            // 
            this.生成领料单ToolStripMenuItem.Name = "生成领料单ToolStripMenuItem";
            this.生成领料单ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // 管理_修改密码ToolStripMenuItem
            // 
            this.管理_修改密码ToolStripMenuItem.Name = "管理_修改密码ToolStripMenuItem";
            this.管理_修改密码ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.管理_修改密码ToolStripMenuItem.Text = "修改密码";
            this.管理_修改密码ToolStripMenuItem.Click += new System.EventHandler(this.管理_修改密码ToolStripMenuItem_Click);
            // 
            // 管理_添加用户ToolStripMenuItem
            // 
            this.管理_添加用户ToolStripMenuItem.Name = "管理_添加用户ToolStripMenuItem";
            this.管理_添加用户ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.管理_添加用户ToolStripMenuItem.Text = "添加用户";
            this.管理_添加用户ToolStripMenuItem.Click += new System.EventHandler(this.管理_添加用户ToolStripMenuItem_Click);
            // 
            // 主界面
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1185, 622);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.Label_Test);
            this.Controls.Add(this.panelParent);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("宋体", 13F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(200, 200);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "主界面";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "联友生产辅助工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.SizeChanged += new System.EventHandler(this.Form_MainResized);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panelParent;
        private System.Windows.Forms.Label Label_Test;
        private System.Windows.Forms.ToolStripMenuItem 管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 管理_权限管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关闭当前界面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 此用户没有任何权限ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelUser;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelLocalConn;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelIP;
        private System.Windows.Forms.ToolStripMenuItem 生成领料单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 供应商ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 供应商_录入送货单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 管理_修改密码ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 管理_添加用户ToolStripMenuItem;
    }
}