namespace HarveyZ.财务
{
    partial class 成本异常报表导出
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
            this.btnOutput = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.TbxRemark = new System.Windows.Forms.TextBox();
            this.PanelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BackColor = System.Drawing.SystemColors.Control;
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.btnOutput);
            this.PanelTitle.Controls.Add(this.dateTimePicker1);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Location = new System.Drawing.Point(2, 2);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(809, 50);
            this.PanelTitle.TabIndex = 0;
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(231, 8);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(136, 23);
            this.btnOutput.TabIndex = 3;
            this.btnOutput.Text = "查询异常并导出";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.dateTimePicker1.Location = new System.Drawing.Point(78, 9);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(115, 21);
            this.dateTimePicker1.TabIndex = 0;
            this.dateTimePicker1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "查询年月：";
            // 
            // TbxRemark
            // 
            this.TbxRemark.BackColor = System.Drawing.Color.White;
            this.TbxRemark.Font = new System.Drawing.Font("宋体", 10F);
            this.TbxRemark.ForeColor = System.Drawing.Color.Red;
            this.TbxRemark.Location = new System.Drawing.Point(3, 94);
            this.TbxRemark.Multiline = true;
            this.TbxRemark.Name = "TbxRemark";
            this.TbxRemark.ReadOnly = true;
            this.TbxRemark.Size = new System.Drawing.Size(518, 143);
            this.TbxRemark.TabIndex = 1;
            this.TbxRemark.Text = "为避免大量占用ERP资源，务必遵守以下注意事项：\r\n\r\n1.请在执行“月底成本计价”后再查询，否则会导出大量数据；\r\n2.请勿在“月底成本计价”正在运行时查询，否" +
    "则会影响ERP运行速度；\r\n3.数据导出时会消耗大量时间，若存在超时报错，按确定即可。";
            // 
            // 成本异常报表导出
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 605);
            this.Controls.Add(this.TbxRemark);
            this.Controls.Add(this.PanelTitle);
            this.Name = "成本异常报表导出";
            this.Text = "销货信息_带入库部门_查询";
            this.Resize += new System.EventHandler(this.FormMain_Resized);
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.TextBox TbxRemark;
    }
}