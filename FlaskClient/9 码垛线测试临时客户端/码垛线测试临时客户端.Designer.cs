namespace 码垛线测试临时客户端
{
    partial class 码垛线测试临时客户端
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(码垛线测试临时客户端));
            this.panel_Title = new System.Windows.Forms.Panel();
            this.btn_Show_Type = new System.Windows.Forms.Button();
            this.btn_Fine = new System.Windows.Forms.Button();
            this.btn_Find_Type = new System.Windows.Forms.Button();
            this.btn_Test_Save = new System.Windows.Forms.Button();
            this.btn_Test_Add = new System.Windows.Forms.Button();
            this.btn_Reflash = new System.Windows.Forms.Button();
            this.btn_Test_Reset = new System.Windows.Forms.Button();
            this.dgv_Main = new System.Windows.Forms.DataGridView();
            this.txb_Fine = new System.Windows.Forms.TextBox();
            this.panel_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Title
            // 
            this.panel_Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Title.Controls.Add(this.txb_Fine);
            this.panel_Title.Controls.Add(this.btn_Show_Type);
            this.panel_Title.Controls.Add(this.btn_Fine);
            this.panel_Title.Controls.Add(this.btn_Find_Type);
            this.panel_Title.Controls.Add(this.btn_Test_Save);
            this.panel_Title.Controls.Add(this.btn_Test_Add);
            this.panel_Title.Controls.Add(this.btn_Reflash);
            this.panel_Title.Controls.Add(this.btn_Test_Reset);
            this.panel_Title.Location = new System.Drawing.Point(2, 2);
            this.panel_Title.Name = "panel_Title";
            this.panel_Title.Size = new System.Drawing.Size(1307, 59);
            this.panel_Title.TabIndex = 0;
            // 
            // btn_Show_Type
            // 
            this.btn_Show_Type.Location = new System.Drawing.Point(1203, 18);
            this.btn_Show_Type.Name = "btn_Show_Type";
            this.btn_Show_Type.Size = new System.Drawing.Size(88, 23);
            this.btn_Show_Type.TabIndex = 6;
            this.btn_Show_Type.Text = "显示";
            this.btn_Show_Type.UseVisualStyleBackColor = true;
            this.btn_Show_Type.Click += new System.EventHandler(this.Show_Type_Click);
            // 
            // btn_Fine
            // 
            this.btn_Fine.Location = new System.Drawing.Point(199, 18);
            this.btn_Fine.Name = "btn_Fine";
            this.btn_Fine.Size = new System.Drawing.Size(75, 23);
            this.btn_Fine.TabIndex = 5;
            this.btn_Fine.Text = "查找";
            this.btn_Fine.UseVisualStyleBackColor = true;
            this.btn_Fine.Visible = false;
            this.btn_Fine.Click += new System.EventHandler(this.btn_Fine_Click);
            // 
            // btn_Find_Type
            // 
            this.btn_Find_Type.Location = new System.Drawing.Point(108, 18);
            this.btn_Find_Type.Name = "btn_Find_Type";
            this.btn_Find_Type.Size = new System.Drawing.Size(75, 23);
            this.btn_Find_Type.TabIndex = 4;
            this.btn_Find_Type.Text = "查找订单";
            this.btn_Find_Type.UseVisualStyleBackColor = true;
            this.btn_Find_Type.Visible = false;
            this.btn_Find_Type.Click += new System.EventHandler(this.btn_Find_Type_Click);
            // 
            // btn_Test_Save
            // 
            this.btn_Test_Save.Location = new System.Drawing.Point(1064, 18);
            this.btn_Test_Save.Name = "btn_Test_Save";
            this.btn_Test_Save.Size = new System.Drawing.Size(127, 23);
            this.btn_Test_Save.TabIndex = 3;
            this.btn_Test_Save.Text = "测试订单修改保存";
            this.btn_Test_Save.UseVisualStyleBackColor = true;
            this.btn_Test_Save.Click += new System.EventHandler(this.btn_Test_Save_Click);
            // 
            // btn_Test_Add
            // 
            this.btn_Test_Add.Location = new System.Drawing.Point(850, 18);
            this.btn_Test_Add.Name = "btn_Test_Add";
            this.btn_Test_Add.Size = new System.Drawing.Size(75, 23);
            this.btn_Test_Add.TabIndex = 2;
            this.btn_Test_Add.Text = "增加测试订单号";
            this.btn_Test_Add.UseVisualStyleBackColor = true;
            this.btn_Test_Add.Visible = false;
            this.btn_Test_Add.Click += new System.EventHandler(this.btn_Test_Add_Click);
            // 
            // btn_Reflash
            // 
            this.btn_Reflash.Location = new System.Drawing.Point(18, 18);
            this.btn_Reflash.Name = "btn_Reflash";
            this.btn_Reflash.Size = new System.Drawing.Size(75, 23);
            this.btn_Reflash.TabIndex = 1;
            this.btn_Reflash.Text = "刷新";
            this.btn_Reflash.UseVisualStyleBackColor = true;
            this.btn_Reflash.Click += new System.EventHandler(this.btn_Reflash_Click);
            // 
            // btn_Test_Reset
            // 
            this.btn_Test_Reset.Location = new System.Drawing.Point(938, 18);
            this.btn_Test_Reset.Name = "btn_Test_Reset";
            this.btn_Test_Reset.Size = new System.Drawing.Size(114, 23);
            this.btn_Test_Reset.TabIndex = 0;
            this.btn_Test_Reset.Text = "重置测试订单信息";
            this.btn_Test_Reset.UseVisualStyleBackColor = true;
            this.btn_Test_Reset.Click += new System.EventHandler(this.btn_Test_Reset_Click);
            // 
            // dgv_Main
            // 
            this.dgv_Main.AllowUserToAddRows = false;
            this.dgv_Main.AllowUserToDeleteRows = false;
            this.dgv_Main.AllowUserToResizeRows = false;
            this.dgv_Main.BackgroundColor = System.Drawing.Color.White;
            this.dgv_Main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Main.Location = new System.Drawing.Point(2, 63);
            this.dgv_Main.MultiSelect = false;
            this.dgv_Main.Name = "dgv_Main";
            this.dgv_Main.RowHeadersVisible = false;
            this.dgv_Main.RowTemplate.Height = 23;
            this.dgv_Main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Main.Size = new System.Drawing.Size(881, 363);
            this.dgv_Main.TabIndex = 1;
            // 
            // txb_Fine
            // 
            this.txb_Fine.Location = new System.Drawing.Point(289, 20);
            this.txb_Fine.Name = "txb_Fine";
            this.txb_Fine.Size = new System.Drawing.Size(240, 21);
            this.txb_Fine.TabIndex = 8;
            // 
            // 码垛线测试临时客户端
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 831);
            this.Controls.Add(this.dgv_Main);
            this.Controls.Add(this.panel_Title);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "码垛线测试临时客户端";
            this.Text = "码垛线测试临时客户端";
            this.Resize += new System.EventHandler(this.Form_MainResized);
            this.panel_Title.ResumeLayout(false);
            this.panel_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_Title;
        private System.Windows.Forms.DataGridView dgv_Main;
        private System.Windows.Forms.Button btn_Show_Type;
        private System.Windows.Forms.Button btn_Fine;
        private System.Windows.Forms.Button btn_Find_Type;
        private System.Windows.Forms.Button btn_Test_Save;
        private System.Windows.Forms.Button btn_Test_Add;
        private System.Windows.Forms.Button btn_Reflash;
        private System.Windows.Forms.Button btn_Test_Reset;
        private System.Windows.Forms.TextBox txb_Fine;
    }
}

