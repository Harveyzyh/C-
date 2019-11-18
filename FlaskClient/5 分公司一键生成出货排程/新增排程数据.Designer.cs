namespace 联友中山分公司生产辅助工具
{
    partial class 新增排程数据
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(新增排程数据));
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.TxbSl = new System.Windows.Forms.TextBox();
            this.BtnSelect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.LabelMsg = new System.Windows.Forms.Label();
            this.TxbScdh = new System.Windows.Forms.TextBox();
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
            this.PanelTitle.Controls.Add(this.DtpDate);
            this.PanelTitle.Controls.Add(this.BtnAdd);
            this.PanelTitle.Controls.Add(this.TxbSl);
            this.PanelTitle.Controls.Add(this.BtnSelect);
            this.PanelTitle.Controls.Add(this.label3);
            this.PanelTitle.Controls.Add(this.LabelMsg);
            this.PanelTitle.Controls.Add(this.TxbScdh);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Location = new System.Drawing.Point(1, 1);
            this.PanelTitle.Margin = new System.Windows.Forms.Padding(4);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(1099, 76);
            this.PanelTitle.TabIndex = 0;
            // 
            // DtpDate
            // 
            this.DtpDate.Location = new System.Drawing.Point(108, 6);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(174, 24);
            this.DtpDate.TabIndex = 7;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(138, 42);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 23);
            this.BtnAdd.TabIndex = 6;
            this.BtnAdd.Text = "添加";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // TxbSl
            // 
            this.TxbSl.Location = new System.Drawing.Point(613, 6);
            this.TxbSl.Name = "TxbSl";
            this.TxbSl.Size = new System.Drawing.Size(107, 24);
            this.TxbSl.TabIndex = 5;
            // 
            // BtnSelect
            // 
            this.BtnSelect.Location = new System.Drawing.Point(38, 42);
            this.BtnSelect.Name = "BtnSelect";
            this.BtnSelect.Size = new System.Drawing.Size(75, 23);
            this.BtnSelect.TabIndex = 4;
            this.BtnSelect.Text = "查询";
            this.BtnSelect.UseVisualStyleBackColor = true;
            this.BtnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(538, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "数量(套)：";
            // 
            // LabelMsg
            // 
            this.LabelMsg.AutoSize = true;
            this.LabelMsg.Location = new System.Drawing.Point(267, 46);
            this.LabelMsg.Name = "LabelMsg";
            this.LabelMsg.Size = new System.Drawing.Size(0, 15);
            this.LabelMsg.TabIndex = 2;
            // 
            // TxbScdh
            // 
            this.TxbScdh.Location = new System.Drawing.Point(369, 6);
            this.TxbScdh.Margin = new System.Windows.Forms.Padding(4);
            this.TxbScdh.Name = "TxbScdh";
            this.TxbScdh.Size = new System.Drawing.Size(156, 24);
            this.TxbScdh.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(289, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "生产单号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "出货日期：";
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DgvMain.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(1, 78);
            this.DgvMain.Margin = new System.Windows.Forms.Padding(4);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvMain.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(1099, 445);
            this.DgvMain.TabIndex = 1;
            // 
            // 新增排程数据
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1102, 525);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "新增排程数据";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增排程数据";
            this.Load += new System.EventHandler(this.新增排程数据_Load);
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.TextBox TxbSl;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LabelMsg;
        private System.Windows.Forms.TextBox TxbScdh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Label label2;
    }
}