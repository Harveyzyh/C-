namespace 联友生产辅助工具.生管排程
{
    partial class 生管排成导入导出部门选择
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
            this.comboBoxDpt = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnEnter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxDpt
            // 
            this.comboBoxDpt.FormattingEnabled = true;
            this.comboBoxDpt.Location = new System.Drawing.Point(113, 38);
            this.comboBoxDpt.Name = "comboBoxDpt";
            this.comboBoxDpt.Size = new System.Drawing.Size(137, 20);
            this.comboBoxDpt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择部门：";
            // 
            // BtnEnter
            // 
            this.BtnEnter.Location = new System.Drawing.Point(102, 79);
            this.BtnEnter.Name = "BtnEnter";
            this.BtnEnter.Size = new System.Drawing.Size(75, 23);
            this.BtnEnter.TabIndex = 2;
            this.BtnEnter.Text = "确定";
            this.BtnEnter.UseVisualStyleBackColor = true;
            this.BtnEnter.Click += new System.EventHandler(this.BtnEnter_Click);
            // 
            // 生管排成导入导出部门选择
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 114);
            this.Controls.Add(this.BtnEnter);
            this.Controls.Add(this.comboBoxDpt);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "生管排成导入导出部门选择";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择部门";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxDpt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnEnter;
    }
}