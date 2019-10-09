namespace 联友中山分公司生产辅助工具
{
    partial class 生产入库领料明细
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
            this.BtnLayout = new System.Windows.Forms.Button();
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.BtnSelect = new System.Windows.Forms.Button();
            this.TxbScdh = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BackColor = System.Drawing.Color.White;
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.label3);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Controls.Add(this.dateTimePicker2);
            this.PanelTitle.Controls.Add(this.dateTimePicker1);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Controls.Add(this.TxbScdh);
            this.PanelTitle.Controls.Add(this.BtnSelect);
            this.PanelTitle.Controls.Add(this.BtnLayout);
            this.PanelTitle.Location = new System.Drawing.Point(2, 1);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(749, 65);
            this.PanelTitle.TabIndex = 0;
            // 
            // BtnLayout
            // 
            this.BtnLayout.Location = new System.Drawing.Point(389, 38);
            this.BtnLayout.Name = "BtnLayout";
            this.BtnLayout.Size = new System.Drawing.Size(75, 23);
            this.BtnLayout.TabIndex = 0;
            this.BtnLayout.Text = "导出";
            this.BtnLayout.UseVisualStyleBackColor = true;
            this.BtnLayout.Click += new System.EventHandler(this.BtnLayout_Click);
            // 
            // DgvMain
            // 
            this.DgvMain.AllowDrop = true;
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(2, 97);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(240, 150);
            this.DgvMain.TabIndex = 1;
            // 
            // BtnSelect
            // 
            this.BtnSelect.Location = new System.Drawing.Point(285, 38);
            this.BtnSelect.Name = "BtnSelect";
            this.BtnSelect.Size = new System.Drawing.Size(75, 23);
            this.BtnSelect.TabIndex = 1;
            this.BtnSelect.Text = "查询";
            this.BtnSelect.UseVisualStyleBackColor = true;
            this.BtnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // TxbScdh
            // 
            this.TxbScdh.Location = new System.Drawing.Point(344, 4);
            this.TxbScdh.Name = "TxbScdh";
            this.TxbScdh.Size = new System.Drawing.Size(134, 21);
            this.TxbScdh.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(273, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "生产单号：";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = " yyyy年 MM月 dd日";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.dateTimePicker1.Location = new System.Drawing.Point(104, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(140, 21);
            this.dateTimePicker1.TabIndex = 4;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = " yyyy年 MM月 dd日";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.dateTimePicker2.Location = new System.Drawing.Point(104, 37);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(140, 21);
            this.dateTimePicker2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "排单开始日：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "排单结束日：";
            // 
            // 生产入库领料明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 466);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Name = "生产入库领料明细";
            this.Text = "生产日入库明细";
            this.Resize += new System.EventHandler(this.Form_MainResized);
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button BtnLayout;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxbScdh;
    }
}