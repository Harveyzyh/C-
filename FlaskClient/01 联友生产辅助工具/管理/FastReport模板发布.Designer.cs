namespace HarveyZ
{
    partial class FastReport模板发布
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
            this.BtnOpenFile = new System.Windows.Forms.Button();
            this.BtnUpload = new System.Windows.Forms.Button();
            this.LabelFilePath = new System.Windows.Forms.Label();
            this.ComboBoxPrintType = new System.Windows.Forms.ComboBox();
            this.ComboBoxPrintName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtnOpenFile
            // 
            this.BtnOpenFile.Location = new System.Drawing.Point(32, 50);
            this.BtnOpenFile.Name = "BtnOpenFile";
            this.BtnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.BtnOpenFile.TabIndex = 0;
            this.BtnOpenFile.Text = "打开文件";
            this.BtnOpenFile.UseVisualStyleBackColor = true;
            this.BtnOpenFile.Click += new System.EventHandler(this.BtnOpenFile_Click);
            // 
            // BtnUpload
            // 
            this.BtnUpload.Location = new System.Drawing.Point(32, 88);
            this.BtnUpload.Name = "BtnUpload";
            this.BtnUpload.Size = new System.Drawing.Size(75, 23);
            this.BtnUpload.TabIndex = 1;
            this.BtnUpload.Text = "上传";
            this.BtnUpload.UseVisualStyleBackColor = true;
            this.BtnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // LabelFilePath
            // 
            this.LabelFilePath.AutoSize = true;
            this.LabelFilePath.Location = new System.Drawing.Point(113, 55);
            this.LabelFilePath.Name = "LabelFilePath";
            this.LabelFilePath.Size = new System.Drawing.Size(0, 12);
            this.LabelFilePath.TabIndex = 2;
            // 
            // ComboBoxPrintType
            // 
            this.ComboBoxPrintType.FormattingEnabled = true;
            this.ComboBoxPrintType.Location = new System.Drawing.Point(103, 14);
            this.ComboBoxPrintType.Name = "ComboBoxPrintType";
            this.ComboBoxPrintType.Size = new System.Drawing.Size(174, 20);
            this.ComboBoxPrintType.TabIndex = 3;
            this.ComboBoxPrintType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxPrintType_SelectedIndexChanged);
            // 
            // ComboBoxPrintName
            // 
            this.ComboBoxPrintName.FormattingEnabled = true;
            this.ComboBoxPrintName.Location = new System.Drawing.Point(377, 14);
            this.ComboBoxPrintName.Name = "ComboBoxPrintName";
            this.ComboBoxPrintName.Size = new System.Drawing.Size(284, 20);
            this.ComboBoxPrintName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "模板类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(308, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "模板名称：";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(32, 127);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(629, 312);
            this.textBox1.TabIndex = 7;
            // 
            // FastReport模板发布
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 564);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ComboBoxPrintName);
            this.Controls.Add(this.ComboBoxPrintType);
            this.Controls.Add(this.LabelFilePath);
            this.Controls.Add(this.BtnUpload);
            this.Controls.Add(this.BtnOpenFile);
            this.Name = "FastReport模板发布";
            this.Text = "FastReport模板发布";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOpenFile;
        private System.Windows.Forms.Button BtnUpload;
        private System.Windows.Forms.Label LabelFilePath;
        private System.Windows.Forms.ComboBox ComboBoxPrintType;
        private System.Windows.Forms.ComboBox ComboBoxPrintName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
    }
}