using System.Windows.Forms;

namespace 联友生产辅助工具.中山分公司
{
    partial class 联友物料需求量导出
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(联友物料需求量导出));
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.DtpPlanEndDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.MaterialSpec = new System.Windows.Forms.TextBox();
            this.MaterialName = new System.Windows.Forms.TextBox();
            this.Material = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DtpPlanStartDate = new System.Windows.Forms.DateTimePicker();
            this.BtnData2Excel = new System.Windows.Forms.Button();
            this.DtpGenerateDate = new System.Windows.Forms.DateTimePicker();
            this.BtnGetData = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LabelDataMakeDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.DtpPlanEndDate);
            this.PanelTitle.Controls.Add(this.label6);
            this.PanelTitle.Controls.Add(this.MaterialSpec);
            this.PanelTitle.Controls.Add(this.MaterialName);
            this.PanelTitle.Controls.Add(this.Material);
            this.PanelTitle.Controls.Add(this.label5);
            this.PanelTitle.Controls.Add(this.label4);
            this.PanelTitle.Controls.Add(this.label3);
            this.PanelTitle.Controls.Add(this.DtpPlanStartDate);
            this.PanelTitle.Controls.Add(this.BtnData2Excel);
            this.PanelTitle.Controls.Add(this.DtpGenerateDate);
            this.PanelTitle.Controls.Add(this.BtnGetData);
            this.PanelTitle.Controls.Add(this.label2);
            this.PanelTitle.Controls.Add(this.LabelDataMakeDate);
            this.PanelTitle.Controls.Add(this.label1);
            this.PanelTitle.Location = new System.Drawing.Point(3, 2);
            this.PanelTitle.Margin = new System.Windows.Forms.Padding(4);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(1287, 102);
            this.PanelTitle.TabIndex = 0;
            // 
            // DtpPlanEndDate
            // 
            this.DtpPlanEndDate.Location = new System.Drawing.Point(697, 18);
            this.DtpPlanEndDate.Name = "DtpPlanEndDate";
            this.DtpPlanEndDate.ShowCheckBox = true;
            this.DtpPlanEndDate.Size = new System.Drawing.Size(175, 24);
            this.DtpPlanEndDate.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(592, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 15);
            this.label6.TabIndex = 13;
            this.label6.Text = "排程结束时间：";
            // 
            // MaterialSpec
            // 
            this.MaterialSpec.Location = new System.Drawing.Point(545, 54);
            this.MaterialSpec.Name = "MaterialSpec";
            this.MaterialSpec.Size = new System.Drawing.Size(175, 24);
            this.MaterialSpec.TabIndex = 12;
            this.MaterialSpec.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyUp);
            // 
            // MaterialName
            // 
            this.MaterialName.Location = new System.Drawing.Point(333, 54);
            this.MaterialName.Name = "MaterialName";
            this.MaterialName.Size = new System.Drawing.Size(156, 24);
            this.MaterialName.TabIndex = 11;
            this.MaterialName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyUp);
            // 
            // Material
            // 
            this.Material.Location = new System.Drawing.Point(116, 54);
            this.Material.Name = "Material";
            this.Material.Size = new System.Drawing.Size(156, 24);
            this.Material.TabIndex = 10;
            this.Material.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(495, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "规格：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(281, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "品名：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(70, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "品号：";
            // 
            // DtpPlanStartDate
            // 
            this.DtpPlanStartDate.Location = new System.Drawing.Point(393, 18);
            this.DtpPlanStartDate.Margin = new System.Windows.Forms.Padding(4);
            this.DtpPlanStartDate.Name = "DtpPlanStartDate";
            this.DtpPlanStartDate.ShowCheckBox = true;
            this.DtpPlanStartDate.Size = new System.Drawing.Size(175, 24);
            this.DtpPlanStartDate.TabIndex = 5;
            // 
            // BtnData2Excel
            // 
            this.BtnData2Excel.Location = new System.Drawing.Point(872, 51);
            this.BtnData2Excel.Margin = new System.Windows.Forms.Padding(4);
            this.BtnData2Excel.Name = "BtnData2Excel";
            this.BtnData2Excel.Size = new System.Drawing.Size(120, 29);
            this.BtnData2Excel.TabIndex = 3;
            this.BtnData2Excel.Text = "导出至Excel";
            this.BtnData2Excel.UseVisualStyleBackColor = true;
            this.BtnData2Excel.Click += new System.EventHandler(this.BtnData2Excel_Click);
            // 
            // DtpGenerateDate
            // 
            this.DtpGenerateDate.Location = new System.Drawing.Point(116, 18);
            this.DtpGenerateDate.Margin = new System.Windows.Forms.Padding(4);
            this.DtpGenerateDate.Name = "DtpGenerateDate";
            this.DtpGenerateDate.Size = new System.Drawing.Size(156, 24);
            this.DtpGenerateDate.TabIndex = 1;
            // 
            // BtnGetData
            // 
            this.BtnGetData.Location = new System.Drawing.Point(739, 51);
            this.BtnGetData.Margin = new System.Windows.Forms.Padding(4);
            this.BtnGetData.Name = "BtnGetData";
            this.BtnGetData.Size = new System.Drawing.Size(119, 29);
            this.BtnGetData.TabIndex = 0;
            this.BtnGetData.Text = "获取数据";
            this.BtnGetData.UseVisualStyleBackColor = true;
            this.BtnGetData.Click += new System.EventHandler(this.BtnGetData_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(289, 22);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "排程开始日期：";
            // 
            // LabelDataMakeDate
            // 
            this.LabelDataMakeDate.AutoSize = true;
            this.LabelDataMakeDate.Location = new System.Drawing.Point(13, 22);
            this.LabelDataMakeDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelDataMakeDate.Name = "LabelDataMakeDate";
            this.LabelDataMakeDate.Size = new System.Drawing.Size(105, 15);
            this.LabelDataMakeDate.TabIndex = 2;
            this.LabelDataMakeDate.Text = "数据生成日期:";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(954, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 80);
            this.label1.TabIndex = 4;
            this.label1.Text = "注意：绿色字为玖友库存量大于联友物料需求量\r\n\r\n      蓝色字为玖友库存数量大于0\r\n\r\n      红色字为联友物料需求量大于分公司库存量";
            // 
            // DgvMain
            // 
            this.DgvMain.AllowDrop = true;
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.AllowUserToOrderColumns = true;
            this.DgvMain.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 11F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvMain.DefaultCellStyle = dataGridViewCellStyle1;
            this.DgvMain.Location = new System.Drawing.Point(3, 112);
            this.DgvMain.Margin = new System.Windows.Forms.Padding(4);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowHeadersVisible = false;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(736, 401);
            this.DgvMain.TabIndex = 1;
            this.DgvMain.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvMain_ColumnHeaderMouseClick);
            // 
            // 联友物料需求量导出
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 805);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "联友物料需求量导出";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "联友物料需求量导出";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.SizeChanged += new System.EventHandler(this.Form_MainResized);
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.Label LabelDataMakeDate;
        private System.Windows.Forms.DateTimePicker DtpGenerateDate;
        private System.Windows.Forms.Button BtnGetData;
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button BtnData2Excel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpPlanStartDate;
        private System.Windows.Forms.TextBox MaterialSpec;
        private System.Windows.Forms.TextBox MaterialName;
        private System.Windows.Forms.TextBox Material;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker DtpPlanEndDate;
    }
}

