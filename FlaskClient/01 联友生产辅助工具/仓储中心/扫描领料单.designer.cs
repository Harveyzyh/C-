namespace HarveyZ.仓储中心
{
    partial class 扫描领料单
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(扫描领料单));
            this.TextBox_Danhao = new System.Windows.Forms.TextBox();
            this.DataGridView_List = new System.Windows.Forms.DataGridView();
            this.Button_Select = new System.Windows.Forms.Button();
            this.Button_Upload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Lable_Danhao = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel_Title = new System.Windows.Forms.Panel();
            this.checkedListBoxGroup = new System.Windows.Forms.CheckedListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).BeginInit();
            this.panel_Title.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextBox_Danhao
            // 
            this.TextBox_Danhao.Location = new System.Drawing.Point(69, 35);
            this.TextBox_Danhao.Name = "TextBox_Danhao";
            this.TextBox_Danhao.Size = new System.Drawing.Size(200, 21);
            this.TextBox_Danhao.TabIndex = 0;
            this.TextBox_Danhao.TextChanged += new System.EventHandler(this.TextBoxChange);
            this.TextBox_Danhao.KeyUp += new System.Windows.Forms.KeyEventHandler(this.button1_Click_Enter);
            // 
            // DataGridView_List
            // 
            this.DataGridView_List.AllowUserToAddRows = false;
            this.DataGridView_List.AllowUserToDeleteRows = false;
            this.DataGridView_List.AllowUserToOrderColumns = true;
            this.DataGridView_List.AllowUserToResizeColumns = false;
            this.DataGridView_List.AllowUserToResizeRows = false;
            this.DataGridView_List.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_List.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.DataGridView_List.Location = new System.Drawing.Point(1, 123);
            this.DataGridView_List.Name = "DataGridView_List";
            this.DataGridView_List.ReadOnly = true;
            this.DataGridView_List.RowHeadersVisible = false;
            this.DataGridView_List.RowTemplate.Height = 23;
            this.DataGridView_List.Size = new System.Drawing.Size(862, 282);
            this.DataGridView_List.TabIndex = 1;
            // 
            // Button_Select
            // 
            this.Button_Select.Location = new System.Drawing.Point(280, 33);
            this.Button_Select.Name = "Button_Select";
            this.Button_Select.Size = new System.Drawing.Size(75, 23);
            this.Button_Select.TabIndex = 2;
            this.Button_Select.Text = "查询";
            this.Button_Select.UseVisualStyleBackColor = true;
            this.Button_Select.Click += new System.EventHandler(this.button1_Click);
            // 
            // Button_Upload
            // 
            this.Button_Upload.Location = new System.Drawing.Point(280, 62);
            this.Button_Upload.Name = "Button_Upload";
            this.Button_Upload.Size = new System.Drawing.Size(75, 23);
            this.Button_Upload.TabIndex = 3;
            this.Button_Upload.Text = "上传";
            this.Button_Upload.UseVisualStyleBackColor = true;
            this.Button_Upload.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 71);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 4;
            // 
            // Lable_Danhao
            // 
            this.Lable_Danhao.AutoSize = true;
            this.Lable_Danhao.Location = new System.Drawing.Point(9, 39);
            this.Lable_Danhao.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lable_Danhao.Name = "Lable_Danhao";
            this.Lable_Danhao.Size = new System.Drawing.Size(65, 12);
            this.Lable_Danhao.TabIndex = 5;
            this.Lable_Danhao.Text = "领料单号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(164, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 6;
            // 
            // panel_Title
            // 
            this.panel_Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Title.Controls.Add(this.checkBox2);
            this.panel_Title.Controls.Add(this.checkedListBoxGroup);
            this.panel_Title.Controls.Add(this.label8);
            this.panel_Title.Controls.Add(this.dateTimePicker1);
            this.panel_Title.Controls.Add(this.label2);
            this.panel_Title.Controls.Add(this.checkBox1);
            this.panel_Title.Controls.Add(this.label3);
            this.panel_Title.Controls.Add(this.TextBox_Danhao);
            this.panel_Title.Controls.Add(this.Lable_Danhao);
            this.panel_Title.Controls.Add(this.Button_Select);
            this.panel_Title.Controls.Add(this.label1);
            this.panel_Title.Controls.Add(this.Button_Upload);
            this.panel_Title.Location = new System.Drawing.Point(1, 1);
            this.panel_Title.Name = "panel_Title";
            this.panel_Title.Size = new System.Drawing.Size(1150, 104);
            this.panel_Title.TabIndex = 7;
            // 
            // checkedListBoxGroup
            // 
            this.checkedListBoxGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkedListBoxGroup.FormattingEnabled = true;
            this.checkedListBoxGroup.Location = new System.Drawing.Point(535, 2);
            this.checkedListBoxGroup.MultiColumn = true;
            this.checkedListBoxGroup.Name = "checkedListBoxGroup";
            this.checkedListBoxGroup.Size = new System.Drawing.Size(400, 98);
            this.checkedListBoxGroup.TabIndex = 31;
            this.checkedListBoxGroup.TabStop = false;
            this.checkedListBoxGroup.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxGroup_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(481, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 48);
            this.label8.TabIndex = 30;
            this.label8.Text = "不上传的\r\n包含勾选\r\n工作组\r\n的领料单：";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(256, 6);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(99, 21);
            this.dateTimePicker1.TabIndex = 9;
            this.dateTimePicker1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "排程日期：";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(11, 7);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(168, 16);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.TabStop = false;
            this.checkBox1.Text = "获取绑定排程工单的领料单";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(381, 7);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(108, 16);
            this.checkBox2.TabIndex = 32;
            this.checkBox2.Text = "包含二次领料单";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // 扫描领料单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 405);
            this.Controls.Add(this.panel_Title);
            this.Controls.Add(this.DataGridView_List);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "扫描领料单";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "扫描领料单";
            this.SizeChanged += new System.EventHandler(this.Form_MainResized);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).EndInit();
            this.panel_Title.ResumeLayout(false);
            this.panel_Title.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox TextBox_Danhao;
        private System.Windows.Forms.DataGridView DataGridView_List;
        private System.Windows.Forms.Button Button_Select;
        private System.Windows.Forms.Button Button_Upload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Lable_Danhao;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel_Title;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox checkedListBoxGroup;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}

