namespace 联友采购平台.供应商
{
    partial class 录入供应商送货单
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
            this.TextBoxFliter = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnGet = new System.Windows.Forms.Button();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.BtnRuin = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.TextBoxBarCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ComboBoxVersionList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.LabelScaned = new System.Windows.Forms.Label();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.LabelScaned);
            this.PanelTitle.Controls.Add(this.TextBoxFliter);
            this.PanelTitle.Controls.Add(this.label4);
            this.PanelTitle.Controls.Add(this.BtnGet);
            this.PanelTitle.Controls.Add(this.BtnPrint);
            this.PanelTitle.Controls.Add(this.BtnRuin);
            this.PanelTitle.Controls.Add(this.BtnSave);
            this.PanelTitle.Controls.Add(this.TextBoxBarCode);
            this.PanelTitle.Controls.Add(this.label3);
            this.PanelTitle.Controls.Add(this.ComboBoxVersionList);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Controls.Add(this.DtpDate);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Location = new System.Drawing.Point(2, 1);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(749, 74);
            this.PanelTitle.TabIndex = 0;
            // 
            // TextBoxFliter
            // 
            this.TextBoxFliter.Location = new System.Drawing.Point(487, 42);
            this.TextBoxFliter.Name = "TextBoxFliter";
            this.TextBoxFliter.Size = new System.Drawing.Size(256, 21);
            this.TextBoxFliter.TabIndex = 11;
            this.TextBoxFliter.TextChanged += new System.EventHandler(this.TextBoxFliter_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(449, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "搜索：";
            // 
            // BtnGet
            // 
            this.BtnGet.Location = new System.Drawing.Point(26, 41);
            this.BtnGet.Name = "BtnGet";
            this.BtnGet.Size = new System.Drawing.Size(75, 23);
            this.BtnGet.TabIndex = 9;
            this.BtnGet.Text = "获取数据";
            this.BtnGet.UseVisualStyleBackColor = true;
            this.BtnGet.Click += new System.EventHandler(this.BtnGet_Click);
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(313, 41);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(75, 23);
            this.BtnPrint.TabIndex = 8;
            this.BtnPrint.Text = "打印";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnRuin
            // 
            this.BtnRuin.Enabled = false;
            this.BtnRuin.Location = new System.Drawing.Point(216, 41);
            this.BtnRuin.Name = "BtnRuin";
            this.BtnRuin.Size = new System.Drawing.Size(75, 23);
            this.BtnRuin.TabIndex = 7;
            this.BtnRuin.TabStop = false;
            this.BtnRuin.Text = "作废";
            this.BtnRuin.UseVisualStyleBackColor = true;
            this.BtnRuin.Visible = false;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(119, 41);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 6;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // TextBoxBarCode
            // 
            this.TextBoxBarCode.Location = new System.Drawing.Point(487, 6);
            this.TextBoxBarCode.Name = "TextBoxBarCode";
            this.TextBoxBarCode.ReadOnly = true;
            this.TextBoxBarCode.Size = new System.Drawing.Size(256, 21);
            this.TextBoxBarCode.TabIndex = 5;
            this.TextBoxBarCode.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(449, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "条码：";
            // 
            // ComboBoxVersionList
            // 
            this.ComboBoxVersionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxVersionList.FormattingEnabled = true;
            this.ComboBoxVersionList.Location = new System.Drawing.Point(304, 6);
            this.ComboBoxVersionList.Name = "ComboBoxVersionList";
            this.ComboBoxVersionList.Size = new System.Drawing.Size(93, 20);
            this.ComboBoxVersionList.TabIndex = 3;
            this.ComboBoxVersionList.TabStop = false;
            this.ComboBoxVersionList.SelectedIndexChanged += new System.EventHandler(this.ComboBoxVersionList_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "版本序号：";
            // 
            // DtpDate
            // 
            this.DtpDate.CustomFormat = "yyyy-MM-dd";
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.DtpDate.Location = new System.Drawing.Point(82, 6);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(129, 21);
            this.DtpDate.TabIndex = 1;
            this.DtpDate.TabStop = false;
            this.DtpDate.ValueChanged += new System.EventHandler(this.DtpDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "送货日期：";
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.AllowUserToOrderColumns = true;
            this.DgvMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(2, 115);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.RowHeadersVisible = false;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvMain.Size = new System.Drawing.Size(749, 431);
            this.DgvMain.TabIndex = 1;
            this.DgvMain.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvMain_CellValueChanged);
            // 
            // LabelScaned
            // 
            this.LabelScaned.AutoSize = true;
            this.LabelScaned.ForeColor = System.Drawing.Color.Red;
            this.LabelScaned.Location = new System.Drawing.Point(678, 10);
            this.LabelScaned.Name = "LabelScaned";
            this.LabelScaned.Size = new System.Drawing.Size(65, 12);
            this.LabelScaned.TabIndex = 12;
            this.LabelScaned.Text = "已扫描进货";
            // 
            // 录入供应商送货单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 632);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Name = "录入供应商送货单";
            this.Text = "录入供应商送货单";
            this.Resize += new System.EventHandler(this.FormMain_Resized);
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnRuin;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.TextBox TextBoxBarCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ComboBoxVersionList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnGet;
        private System.Windows.Forms.TextBox TextBoxFliter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LabelScaned;
    }
}