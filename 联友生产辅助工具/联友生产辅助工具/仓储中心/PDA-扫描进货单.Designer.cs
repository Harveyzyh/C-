namespace 联友生产辅助工具.仓储中心
{
    partial class PDA_扫描进货单
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
            this.panel_Title = new System.Windows.Forms.Panel();
            this.数量T = new System.Windows.Forms.TextBox();
            this.条码T = new System.Windows.Forms.TextBox();
            this.条码L = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.入库仓库查 = new System.Windows.Forms.Button();
            this.供应商查 = new System.Windows.Forms.Button();
            this.入库单别查 = new System.Windows.Forms.Button();
            this.入库仓库 = new System.Windows.Forms.Label();
            this.送货日期 = new System.Windows.Forms.Label();
            this.供应商 = new System.Windows.Forms.Label();
            this.入库单别 = new System.Windows.Forms.Label();
            this.送货单号T = new System.Windows.Forms.TextBox();
            this.入库单别L = new System.Windows.Forms.Label();
            this.送货日期L = new System.Windows.Forms.Label();
            this.送货单号L = new System.Windows.Forms.Label();
            this.供应商L = new System.Windows.Forms.Label();
            this.入库仓库L = new System.Windows.Forms.Label();
            this.数量L = new System.Windows.Forms.Label();
            this.DataGridView_List = new System.Windows.Forms.DataGridView();
            this.C2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_Last = new System.Windows.Forms.Panel();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.panel_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).BeginInit();
            this.panel_Last.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Title
            // 
            this.panel_Title.Controls.Add(this.数量T);
            this.panel_Title.Controls.Add(this.条码T);
            this.panel_Title.Controls.Add(this.条码L);
            this.panel_Title.Controls.Add(this.dateTimePicker1);
            this.panel_Title.Controls.Add(this.入库仓库查);
            this.panel_Title.Controls.Add(this.供应商查);
            this.panel_Title.Controls.Add(this.入库单别查);
            this.panel_Title.Controls.Add(this.入库仓库);
            this.panel_Title.Controls.Add(this.送货日期);
            this.panel_Title.Controls.Add(this.供应商);
            this.panel_Title.Controls.Add(this.入库单别);
            this.panel_Title.Controls.Add(this.送货单号T);
            this.panel_Title.Controls.Add(this.入库单别L);
            this.panel_Title.Controls.Add(this.送货日期L);
            this.panel_Title.Controls.Add(this.送货单号L);
            this.panel_Title.Controls.Add(this.供应商L);
            this.panel_Title.Controls.Add(this.入库仓库L);
            this.panel_Title.Controls.Add(this.数量L);
            this.panel_Title.Location = new System.Drawing.Point(0, 0);
            this.panel_Title.Margin = new System.Windows.Forms.Padding(4);
            this.panel_Title.Name = "panel_Title";
            this.panel_Title.Size = new System.Drawing.Size(819, 119);
            this.panel_Title.TabIndex = 0;
            // 
            // 数量T
            // 
            this.数量T.Location = new System.Drawing.Point(434, 73);
            this.数量T.Name = "数量T";
            this.数量T.Size = new System.Drawing.Size(178, 24);
            this.数量T.TabIndex = 16;
            this.数量T.KeyUp += new System.Windows.Forms.KeyEventHandler(this.数量_KeyUp);
            // 
            // 条码T
            // 
            this.条码T.Location = new System.Drawing.Point(119, 73);
            this.条码T.Name = "条码T";
            this.条码T.Size = new System.Drawing.Size(161, 24);
            this.条码T.TabIndex = 15;
            this.条码T.KeyUp += new System.Windows.Forms.KeyEventHandler(this.条码_KeyUp);
            // 
            // 条码L
            // 
            this.条码L.AutoSize = true;
            this.条码L.Location = new System.Drawing.Point(42, 80);
            this.条码L.Name = "条码L";
            this.条码L.Size = new System.Drawing.Size(76, 15);
            this.条码L.TabIndex = 15;
            this.条码L.Text = "条   码：";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-DD";
            this.dateTimePicker1.Location = new System.Drawing.Point(119, 40);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(161, 24);
            this.dateTimePicker1.TabIndex = 13;
            // 
            // 入库仓库查
            // 
            this.入库仓库查.Location = new System.Drawing.Point(317, 10);
            this.入库仓库查.Margin = new System.Windows.Forms.Padding(4);
            this.入库仓库查.Name = "入库仓库查";
            this.入库仓库查.Size = new System.Drawing.Size(35, 29);
            this.入库仓库查.TabIndex = 11;
            this.入库仓库查.Text = "查";
            this.入库仓库查.UseVisualStyleBackColor = true;
            this.入库仓库查.Click += new System.EventHandler(this.入库仓库查_Click);
            // 
            // 供应商查
            // 
            this.供应商查.Location = new System.Drawing.Point(614, 9);
            this.供应商查.Margin = new System.Windows.Forms.Padding(4);
            this.供应商查.Name = "供应商查";
            this.供应商查.Size = new System.Drawing.Size(35, 29);
            this.供应商查.TabIndex = 12;
            this.供应商查.Text = "查";
            this.供应商查.UseVisualStyleBackColor = true;
            this.供应商查.Click += new System.EventHandler(this.供应商查_Click);
            // 
            // 入库单别查
            // 
            this.入库单别查.Location = new System.Drawing.Point(4, 9);
            this.入库单别查.Margin = new System.Windows.Forms.Padding(4);
            this.入库单别查.Name = "入库单别查";
            this.入库单别查.Size = new System.Drawing.Size(35, 29);
            this.入库单别查.TabIndex = 10;
            this.入库单别查.Text = "查";
            this.入库单别查.UseVisualStyleBackColor = true;
            this.入库单别查.Click += new System.EventHandler(this.送货单别查_Click);
            // 
            // 入库仓库
            // 
            this.入库仓库.AutoSize = true;
            this.入库仓库.Location = new System.Drawing.Point(431, 16);
            this.入库仓库.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.入库仓库.Name = "入库仓库";
            this.入库仓库.Size = new System.Drawing.Size(0, 15);
            this.入库仓库.TabIndex = 9;
            // 
            // 送货日期
            // 
            this.送货日期.AutoSize = true;
            this.送货日期.Location = new System.Drawing.Point(121, 44);
            this.送货日期.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.送货日期.Name = "送货日期";
            this.送货日期.Size = new System.Drawing.Size(0, 15);
            this.送货日期.TabIndex = 8;
            // 
            // 供应商
            // 
            this.供应商.AutoSize = true;
            this.供应商.Location = new System.Drawing.Point(733, 16);
            this.供应商.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.供应商.Name = "供应商";
            this.供应商.Size = new System.Drawing.Size(0, 15);
            this.供应商.TabIndex = 7;
            // 
            // 入库单别
            // 
            this.入库单别.AutoSize = true;
            this.入库单别.Location = new System.Drawing.Point(119, 15);
            this.入库单别.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.入库单别.Name = "入库单别";
            this.入库单别.Size = new System.Drawing.Size(0, 15);
            this.入库单别.TabIndex = 6;
            // 
            // 送货单号T
            // 
            this.送货单号T.Location = new System.Drawing.Point(434, 39);
            this.送货单号T.Margin = new System.Windows.Forms.Padding(4);
            this.送货单号T.Name = "送货单号T";
            this.送货单号T.Size = new System.Drawing.Size(178, 24);
            this.送货单号T.TabIndex = 14;
            this.送货单号T.KeyUp += new System.Windows.Forms.KeyEventHandler(this.送货单号_KeyUp);
            // 
            // 入库单别L
            // 
            this.入库单别L.AutoSize = true;
            this.入库单别L.Location = new System.Drawing.Point(39, 16);
            this.入库单别L.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.入库单别L.Name = "入库单别L";
            this.入库单别L.Size = new System.Drawing.Size(82, 15);
            this.入库单别L.TabIndex = 0;
            this.入库单别L.Text = "入库单别：";
            // 
            // 送货日期L
            // 
            this.送货日期L.AutoSize = true;
            this.送货日期L.Location = new System.Drawing.Point(39, 45);
            this.送货日期L.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.送货日期L.Name = "送货日期L";
            this.送货日期L.Size = new System.Drawing.Size(82, 15);
            this.送货日期L.TabIndex = 2;
            this.送货日期L.Text = "送货日期：";
            // 
            // 送货单号L
            // 
            this.送货单号L.AutoSize = true;
            this.送货单号L.Location = new System.Drawing.Point(354, 45);
            this.送货单号L.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.送货单号L.Name = "送货单号L";
            this.送货单号L.Size = new System.Drawing.Size(82, 15);
            this.送货单号L.TabIndex = 4;
            this.送货单号L.Text = "送货单号：";
            // 
            // 供应商L
            // 
            this.供应商L.AutoSize = true;
            this.供应商L.Location = new System.Drawing.Point(651, 16);
            this.供应商L.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.供应商L.Name = "供应商L";
            this.供应商L.Size = new System.Drawing.Size(83, 15);
            this.供应商L.TabIndex = 1;
            this.供应商L.Text = "供 应 商：";
            // 
            // 入库仓库L
            // 
            this.入库仓库L.AutoSize = true;
            this.入库仓库L.Location = new System.Drawing.Point(354, 16);
            this.入库仓库L.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.入库仓库L.Name = "入库仓库L";
            this.入库仓库L.Size = new System.Drawing.Size(82, 15);
            this.入库仓库L.TabIndex = 3;
            this.入库仓库L.Text = "入库仓库：";
            // 
            // 数量L
            // 
            this.数量L.AutoSize = true;
            this.数量L.Location = new System.Drawing.Point(354, 80);
            this.数量L.Name = "数量L";
            this.数量L.Size = new System.Drawing.Size(84, 15);
            this.数量L.TabIndex = 17;
            this.数量L.Text = "数    量：";
            // 
            // DataGridView_List
            // 
            this.DataGridView_List.AllowUserToAddRows = false;
            this.DataGridView_List.AllowUserToDeleteRows = false;
            this.DataGridView_List.AllowUserToOrderColumns = true;
            this.DataGridView_List.AllowUserToResizeRows = false;
            this.DataGridView_List.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_List.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.C2,
            this.C3,
            this.C4,
            this.C5,
            this.C6,
            this.C7,
            this.C8,
            this.C9,
            this.C10});
            this.DataGridView_List.Location = new System.Drawing.Point(1, 120);
            this.DataGridView_List.Margin = new System.Windows.Forms.Padding(4);
            this.DataGridView_List.Name = "DataGridView_List";
            this.DataGridView_List.RowHeadersVisible = false;
            this.DataGridView_List.RowTemplate.Height = 23;
            this.DataGridView_List.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView_List.Size = new System.Drawing.Size(818, 422);
            this.DataGridView_List.TabIndex = 1;
            this.DataGridView_List.TabStop = false;
            // 
            // C2
            // 
            this.C2.HeaderText = "品号";
            this.C2.Name = "C2";
            this.C2.ReadOnly = true;
            this.C2.Width = 150;
            // 
            // C3
            // 
            this.C3.HeaderText = "品名";
            this.C3.Name = "C3";
            this.C3.Width = 200;
            // 
            // C4
            // 
            this.C4.HeaderText = "规格";
            this.C4.Name = "C4";
            this.C4.Width = 200;
            // 
            // C5
            // 
            this.C5.HeaderText = "数量";
            this.C5.Name = "C5";
            this.C5.Width = 70;
            // 
            // C6
            // 
            this.C6.HeaderText = "仓库";
            this.C6.Name = "C6";
            this.C6.Width = 70;
            // 
            // C7
            // 
            this.C7.HeaderText = "批号";
            this.C7.Name = "C7";
            this.C7.Width = 70;
            // 
            // C8
            // 
            this.C8.HeaderText = "供应商";
            this.C8.Name = "C8";
            // 
            // C9
            // 
            this.C9.HeaderText = "送货单";
            this.C9.Name = "C9";
            // 
            // C10
            // 
            this.C10.HeaderText = "条码";
            this.C10.Name = "C10";
            this.C10.Width = 200;
            // 
            // panel_Last
            // 
            this.panel_Last.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Last.Controls.Add(this.buttonUpload);
            this.panel_Last.Controls.Add(this.buttonDelete);
            this.panel_Last.Location = new System.Drawing.Point(0, 551);
            this.panel_Last.Name = "panel_Last";
            this.panel_Last.Size = new System.Drawing.Size(819, 44);
            this.panel_Last.TabIndex = 2;
            // 
            // buttonUpload
            // 
            this.buttonUpload.Location = new System.Drawing.Point(108, 9);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(75, 23);
            this.buttonUpload.TabIndex = 1;
            this.buttonUpload.TabStop = false;
            this.buttonUpload.Text = "上传";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(12, 9);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 0;
            this.buttonDelete.TabStop = false;
            this.buttonDelete.Text = "删除";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // PDA_扫描进货单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 595);
            this.Controls.Add(this.panel_Last);
            this.Controls.Add(this.DataGridView_List);
            this.Controls.Add(this.panel_Title);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PDA_扫描进货单";
            this.ShowIcon = false;
            this.Text = "扫描进货单";
            this.SizeChanged += new System.EventHandler(this.FormMain_Resized);
            this.VisibleChanged += new System.EventHandler(this.FormMain_Resized);
            this.panel_Title.ResumeLayout(false);
            this.panel_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_List)).EndInit();
            this.panel_Last.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_Title;
        private System.Windows.Forms.Label 入库仓库L;
        private System.Windows.Forms.Label 送货日期L;
        private System.Windows.Forms.Label 供应商L;
        private System.Windows.Forms.Label 入库单别L;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button 入库仓库查;
        private System.Windows.Forms.Button 供应商查;
        private System.Windows.Forms.Button 入库单别查;
        private System.Windows.Forms.Label 入库仓库;
        private System.Windows.Forms.Label 送货日期;
        private System.Windows.Forms.Label 供应商;
        private System.Windows.Forms.Label 入库单别;
        private System.Windows.Forms.TextBox 送货单号T;
        private System.Windows.Forms.Label 送货单号L;
        private System.Windows.Forms.DataGridView DataGridView_List;
        private System.Windows.Forms.TextBox 数量T;
        private System.Windows.Forms.TextBox 条码T;
        private System.Windows.Forms.Label 数量L;
        private System.Windows.Forms.Label 条码L;
        private System.Windows.Forms.Panel panel_Last;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn C2;
        private System.Windows.Forms.DataGridViewTextBoxColumn C3;
        private System.Windows.Forms.DataGridViewTextBoxColumn C4;
        private System.Windows.Forms.DataGridViewTextBoxColumn C5;
        private System.Windows.Forms.DataGridViewTextBoxColumn C6;
        private System.Windows.Forms.DataGridViewTextBoxColumn C7;
        private System.Windows.Forms.DataGridViewTextBoxColumn C8;
        private System.Windows.Forms.DataGridViewTextBoxColumn C9;
        private System.Windows.Forms.DataGridViewTextBoxColumn C10;
    }
}