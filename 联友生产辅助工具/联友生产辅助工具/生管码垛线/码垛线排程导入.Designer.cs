﻿namespace 联友生产进度工具.生管码垛线
{
    partial class 码垛线排程导入
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
            this.DataGridView_List = new System.Windows.Forms.DataGridView();
            this.button_Show = new System.Windows.Forms.Button();
            this.panel_Title = new System.Windows.Forms.Panel();
            this.button_Output = new System.Windows.Forms.Button();
            this.button_Input = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).BeginInit();
            this.panel_Title.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataGridView_List
            // 
            this.DataGridView_List.AllowUserToAddRows = false;
            this.DataGridView_List.AllowUserToDeleteRows = false;
            this.DataGridView_List.AllowUserToOrderColumns = true;
            this.DataGridView_List.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DataGridView_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_List.Location = new System.Drawing.Point(0, 37);
            this.DataGridView_List.Margin = new System.Windows.Forms.Padding(4);
            this.DataGridView_List.Name = "DataGridView_List";
            this.DataGridView_List.ReadOnly = true;
            this.DataGridView_List.RowHeadersVisible = false;
            this.DataGridView_List.RowTemplate.Height = 23;
            this.DataGridView_List.Size = new System.Drawing.Size(622, 135);
            this.DataGridView_List.TabIndex = 2;
            // 
            // button_Show
            // 
            this.button_Show.Location = new System.Drawing.Point(198, 4);
            this.button_Show.Margin = new System.Windows.Forms.Padding(4);
            this.button_Show.Name = "button_Show";
            this.button_Show.Size = new System.Drawing.Size(177, 30);
            this.button_Show.TabIndex = 3;
            this.button_Show.Text = "显示全部";
            this.button_Show.UseVisualStyleBackColor = true;
            this.button_Show.Click += new System.EventHandler(this.button_Show_Click);
            // 
            // panel_Title
            // 
            this.panel_Title.Controls.Add(this.label1);
            this.panel_Title.Controls.Add(this.button_Output);
            this.panel_Title.Controls.Add(this.button_Input);
            this.panel_Title.Controls.Add(this.button_Show);
            this.panel_Title.Location = new System.Drawing.Point(0, 0);
            this.panel_Title.Name = "panel_Title";
            this.panel_Title.Size = new System.Drawing.Size(622, 38);
            this.panel_Title.TabIndex = 4;
            // 
            // button_Output
            // 
            this.button_Output.Location = new System.Drawing.Point(101, 4);
            this.button_Output.Name = "button_Output";
            this.button_Output.Size = new System.Drawing.Size(90, 30);
            this.button_Output.TabIndex = 5;
            this.button_Output.Text = "导出";
            this.button_Output.UseVisualStyleBackColor = true;
            // 
            // button_Input
            // 
            this.button_Input.Location = new System.Drawing.Point(5, 4);
            this.button_Input.Name = "button_Input";
            this.button_Input.Size = new System.Drawing.Size(90, 30);
            this.button_Input.TabIndex = 4;
            this.button_Input.Text = "导入";
            this.button_Input.UseVisualStyleBackColor = true;
            this.button_Input.Click += new System.EventHandler(this.button_Input_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(418, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // 码垛线排程导入
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 181);
            this.Controls.Add(this.panel_Title);
            this.Controls.Add(this.DataGridView_List);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "码垛线排程导入";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "联友生产进度工具";
            this.SizeChanged += new System.EventHandler(this.FormMain_Resized);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).EndInit();
            this.panel_Title.ResumeLayout(false);
            this.panel_Title.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView DataGridView_List;
        private System.Windows.Forms.Button button_Show;
        private System.Windows.Forms.Panel panel_Title;
        private System.Windows.Forms.Button button_Output;
        private System.Windows.Forms.Button button_Input;
        private System.Windows.Forms.Label label1;
    }
}

