namespace HarveyZ.生管排程
{
    partial class 生产排程导入
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(生产排程导入));
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.BtnInput = new System.Windows.Forms.Button();
            this.BtnCheck = new System.Windows.Forms.Button();
            this.PanelMain = new System.Windows.Forms.Panel();
            this.PanelDataDone = new System.Windows.Forms.Panel();
            this.DgvDone = new System.Windows.Forms.DataGridView();
            this.PanelDataDone_Title = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.PanelDataPreDo = new System.Windows.Forms.Panel();
            this.DgvPreDo = new System.Windows.Forms.DataGridView();
            this.PanelDataPreDo_Title = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip_DgvPreDo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip_DgvDone = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PanelTitle.SuspendLayout();
            this.PanelMain.SuspendLayout();
            this.PanelDataDone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvDone)).BeginInit();
            this.PanelDataDone_Title.SuspendLayout();
            this.PanelDataPreDo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvPreDo)).BeginInit();
            this.PanelDataPreDo_Title.SuspendLayout();
            this.contextMenuStrip_DgvPreDo.SuspendLayout();
            this.contextMenuStrip_DgvDone.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.label4);
            this.PanelTitle.Controls.Add(this.BtnInput);
            this.PanelTitle.Controls.Add(this.BtnCheck);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTitle.Location = new System.Drawing.Point(0, 0);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(877, 54);
            this.PanelTitle.TabIndex = 0;
            // 
            // BtnInput
            // 
            this.BtnInput.Location = new System.Drawing.Point(173, 11);
            this.BtnInput.Name = "BtnInput";
            this.BtnInput.Size = new System.Drawing.Size(75, 23);
            this.BtnInput.TabIndex = 1;
            this.BtnInput.Text = "导入";
            this.BtnInput.UseVisualStyleBackColor = true;
            this.BtnInput.Click += new System.EventHandler(this.BtnInput_Click);
            // 
            // BtnCheck
            // 
            this.BtnCheck.Location = new System.Drawing.Point(39, 12);
            this.BtnCheck.Name = "BtnCheck";
            this.BtnCheck.Size = new System.Drawing.Size(75, 23);
            this.BtnCheck.TabIndex = 0;
            this.BtnCheck.Text = "检查异常";
            this.BtnCheck.UseVisualStyleBackColor = true;
            this.BtnCheck.Click += new System.EventHandler(this.BtnCheck_Click);
            // 
            // PanelMain
            // 
            this.PanelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelMain.Controls.Add(this.PanelDataDone);
            this.PanelMain.Controls.Add(this.PanelDataPreDo);
            this.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelMain.Location = new System.Drawing.Point(0, 54);
            this.PanelMain.Name = "PanelMain";
            this.PanelMain.Size = new System.Drawing.Size(877, 496);
            this.PanelMain.TabIndex = 1;
            // 
            // PanelDataDone
            // 
            this.PanelDataDone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelDataDone.Controls.Add(this.DgvDone);
            this.PanelDataDone.Controls.Add(this.PanelDataDone_Title);
            this.PanelDataDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelDataDone.Location = new System.Drawing.Point(524, 0);
            this.PanelDataDone.Name = "PanelDataDone";
            this.PanelDataDone.Size = new System.Drawing.Size(351, 494);
            this.PanelDataDone.TabIndex = 1;
            // 
            // DgvDone
            // 
            this.DgvDone.AllowUserToAddRows = false;
            this.DgvDone.AllowUserToDeleteRows = false;
            this.DgvDone.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.DgvDone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvDone.Location = new System.Drawing.Point(0, 31);
            this.DgvDone.Name = "DgvDone";
            this.DgvDone.ReadOnly = true;
            this.DgvDone.RowHeadersVisible = false;
            this.DgvDone.RowTemplate.Height = 23;
            this.DgvDone.Size = new System.Drawing.Size(349, 461);
            this.DgvDone.TabIndex = 1;
            this.DgvDone.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvDone_CellMouseDown);
            // 
            // PanelDataDone_Title
            // 
            this.PanelDataDone_Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelDataDone_Title.Controls.Add(this.label2);
            this.PanelDataDone_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelDataDone_Title.Location = new System.Drawing.Point(0, 0);
            this.PanelDataDone_Title.Name = "PanelDataDone_Title";
            this.PanelDataDone_Title.Size = new System.Drawing.Size(349, 31);
            this.PanelDataDone_Title.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "此列为已导入排程信息";
            // 
            // PanelDataPreDo
            // 
            this.PanelDataPreDo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelDataPreDo.Controls.Add(this.DgvPreDo);
            this.PanelDataPreDo.Controls.Add(this.PanelDataPreDo_Title);
            this.PanelDataPreDo.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelDataPreDo.Location = new System.Drawing.Point(0, 0);
            this.PanelDataPreDo.Name = "PanelDataPreDo";
            this.PanelDataPreDo.Size = new System.Drawing.Size(524, 494);
            this.PanelDataPreDo.TabIndex = 0;
            // 
            // DgvPreDo
            // 
            this.DgvPreDo.AllowUserToAddRows = false;
            this.DgvPreDo.AllowUserToDeleteRows = false;
            this.DgvPreDo.BackgroundColor = System.Drawing.Color.White;
            this.DgvPreDo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvPreDo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvPreDo.Location = new System.Drawing.Point(0, 31);
            this.DgvPreDo.Name = "DgvPreDo";
            this.DgvPreDo.RowHeadersVisible = false;
            this.DgvPreDo.RowTemplate.Height = 23;
            this.DgvPreDo.Size = new System.Drawing.Size(522, 461);
            this.DgvPreDo.TabIndex = 1;
            this.DgvPreDo.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvPreDo_CellMouseDown);
            this.DgvPreDo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvPreDo_CellValueChanged);
            // 
            // PanelDataPreDo_Title
            // 
            this.PanelDataPreDo_Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelDataPreDo_Title.Controls.Add(this.label3);
            this.PanelDataPreDo_Title.Controls.Add(this.label1);
            this.PanelDataPreDo_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelDataPreDo_Title.Location = new System.Drawing.Point(0, 0);
            this.PanelDataPreDo_Title.Name = "PanelDataPreDo_Title";
            this.PanelDataPreDo_Title.Size = new System.Drawing.Size(522, 31);
            this.PanelDataPreDo_Title.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "此列为待导入排程信息";
            // 
            // contextMenuStrip_DgvPreDo
            // 
            this.contextMenuStrip_DgvPreDo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem1});
            this.contextMenuStrip_DgvPreDo.Name = "contextMenuStrip_DgvPreDo";
            this.contextMenuStrip_DgvPreDo.ShowImageMargin = false;
            this.contextMenuStrip_DgvPreDo.Size = new System.Drawing.Size(76, 26);
            this.contextMenuStrip_DgvPreDo.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_DgvPreDo_ItemClicked);
            // 
            // contextMenuStrip_DgvDone
            // 
            this.contextMenuStrip_DgvDone.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.修改ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip_DgvDone.Name = "contextMenuStrip_DgvDone";
            this.contextMenuStrip_DgvDone.ShowImageMargin = false;
            this.contextMenuStrip_DgvDone.Size = new System.Drawing.Size(76, 48);
            this.contextMenuStrip_DgvDone.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_DgvDone_ItemClicked);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(75, 22);
            this.修改ToolStripMenuItem.Text = "修改";
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(75, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(75, 22);
            this.删除ToolStripMenuItem1.Text = "删除";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(199, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(278, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(365, 24);
            this.label4.TabIndex = 2;
            this.label4.Text = "先检查异常，后根据实际情况调整已倒入排程的数据，无误后导入。\r\n状态列中：已存在的数据会跳过，异常数据不做处理，保留在界面。";
            // 
            // 生产排程导入
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 550);
            this.Controls.Add(this.PanelMain);
            this.Controls.Add(this.PanelTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "生产排程导入";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "生产排程导入";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.生产排程导入_Resize);
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            this.PanelMain.ResumeLayout(false);
            this.PanelDataDone.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvDone)).EndInit();
            this.PanelDataDone_Title.ResumeLayout(false);
            this.PanelDataDone_Title.PerformLayout();
            this.PanelDataPreDo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvPreDo)).EndInit();
            this.PanelDataPreDo_Title.ResumeLayout(false);
            this.PanelDataPreDo_Title.PerformLayout();
            this.contextMenuStrip_DgvPreDo.ResumeLayout(false);
            this.contextMenuStrip_DgvDone.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.Button BtnCheck;
        private System.Windows.Forms.Panel PanelMain;
        private System.Windows.Forms.Panel PanelDataDone;
        private System.Windows.Forms.DataGridView DgvDone;
        private System.Windows.Forms.Panel PanelDataDone_Title;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel PanelDataPreDo;
        private System.Windows.Forms.DataGridView DgvPreDo;
        private System.Windows.Forms.Panel PanelDataPreDo_Title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnInput;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_DgvPreDo;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_DgvDone;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}