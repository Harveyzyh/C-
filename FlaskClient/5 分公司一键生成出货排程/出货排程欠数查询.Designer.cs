namespace 联友中山分公司生产辅助工具
{
    partial class 出货排程欠数查询
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
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.BtnSelect = new System.Windows.Forms.Button();
            this.BtnLayout = new System.Windows.Forms.Button();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BackColor = System.Drawing.Color.White;
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.BtnLayout);
            this.PanelTitle.Controls.Add(this.BtnSelect);
            this.PanelTitle.Location = new System.Drawing.Point(2, 1);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(932, 90);
            this.PanelTitle.TabIndex = 0;
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(2, 97);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(240, 150);
            this.DgvMain.TabIndex = 1;
            // 
            // BtnSelect
            // 
            this.BtnSelect.Location = new System.Drawing.Point(293, 27);
            this.BtnSelect.Name = "BtnSelect";
            this.BtnSelect.Size = new System.Drawing.Size(75, 23);
            this.BtnSelect.TabIndex = 0;
            this.BtnSelect.Text = "查询";
            this.BtnSelect.UseVisualStyleBackColor = true;
            this.BtnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // BtnLayout
            // 
            this.BtnLayout.Location = new System.Drawing.Point(428, 26);
            this.BtnLayout.Name = "BtnLayout";
            this.BtnLayout.Size = new System.Drawing.Size(75, 23);
            this.BtnLayout.TabIndex = 1;
            this.BtnLayout.Text = "导出";
            this.BtnLayout.UseVisualStyleBackColor = true;
            this.BtnLayout.Click += new System.EventHandler(this.BtnLayout_Click);
            // 
            // 出货排程欠数查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 315);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Name = "出货排程欠数查询";
            this.Text = "生产日入库明细";
            this.Resize += new System.EventHandler(this.Form_MainResized);
            this.PanelTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.Button BtnLayout;
    }
}