namespace 联友生产辅助工具.管理
{
    partial class 用户管理
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
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.BtnSetBasePerm = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnFind = new System.Windows.Forms.Button();
            this.TextBoxU_ID = new System.Windows.Forms.TextBox();
            this.LableU_ID = new System.Windows.Forms.Label();
            this.DgvUser = new System.Windows.Forms.DataGridView();
            this.BtnReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvUser)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.AllowUserToResizeRows = false;
            this.DgvMain.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.DgvMain.Location = new System.Drawing.Point(4, 97);
            this.DgvMain.MultiSelect = false;
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.RowHeadersVisible = false;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(358, 398);
            this.DgvMain.TabIndex = 0;
            // 
            // PanelTitle
            // 
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.BtnReset);
            this.PanelTitle.Controls.Add(this.BtnSetBasePerm);
            this.PanelTitle.Controls.Add(this.BtnSave);
            this.PanelTitle.Controls.Add(this.BtnFind);
            this.PanelTitle.Controls.Add(this.TextBoxU_ID);
            this.PanelTitle.Controls.Add(this.LableU_ID);
            this.PanelTitle.Location = new System.Drawing.Point(2, 3);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(727, 89);
            this.PanelTitle.TabIndex = 1;
            // 
            // BtnSetBasePerm
            // 
            this.BtnSetBasePerm.Location = new System.Drawing.Point(502, 21);
            this.BtnSetBasePerm.Name = "BtnSetBasePerm";
            this.BtnSetBasePerm.Size = new System.Drawing.Size(91, 23);
            this.BtnSetBasePerm.TabIndex = 4;
            this.BtnSetBasePerm.Text = "更新基础权限";
            this.BtnSetBasePerm.UseVisualStyleBackColor = true;
            this.BtnSetBasePerm.Click += new System.EventHandler(this.BtnSetBasePerm_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Enabled = false;
            this.BtnSave.Location = new System.Drawing.Point(379, 22);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 3;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnFind
            // 
            this.BtnFind.Location = new System.Drawing.Point(265, 22);
            this.BtnFind.Name = "BtnFind";
            this.BtnFind.Size = new System.Drawing.Size(75, 23);
            this.BtnFind.TabIndex = 2;
            this.BtnFind.Text = "查找";
            this.BtnFind.UseVisualStyleBackColor = true;
            this.BtnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // TextBoxU_ID
            // 
            this.TextBoxU_ID.Location = new System.Drawing.Point(93, 25);
            this.TextBoxU_ID.Name = "TextBoxU_ID";
            this.TextBoxU_ID.Size = new System.Drawing.Size(132, 21);
            this.TextBoxU_ID.TabIndex = 1;
            this.TextBoxU_ID.TextChanged += new System.EventHandler(this.TextBoxU_ID_TextChanged);
            // 
            // LableU_ID
            // 
            this.LableU_ID.AutoSize = true;
            this.LableU_ID.Location = new System.Drawing.Point(21, 32);
            this.LableU_ID.Name = "LableU_ID";
            this.LableU_ID.Size = new System.Drawing.Size(65, 12);
            this.LableU_ID.TabIndex = 0;
            this.LableU_ID.Text = "输入账号：";
            // 
            // DgvUser
            // 
            this.DgvUser.AllowUserToAddRows = false;
            this.DgvUser.AllowUserToDeleteRows = false;
            this.DgvUser.AllowUserToResizeRows = false;
            this.DgvUser.BackgroundColor = System.Drawing.Color.White;
            this.DgvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvUser.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.DgvUser.Location = new System.Drawing.Point(368, 98);
            this.DgvUser.MultiSelect = false;
            this.DgvUser.Name = "DgvUser";
            this.DgvUser.ReadOnly = true;
            this.DgvUser.RowHeadersVisible = false;
            this.DgvUser.RowTemplate.Height = 23;
            this.DgvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvUser.Size = new System.Drawing.Size(360, 397);
            this.DgvUser.TabIndex = 2;
            this.DgvUser.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DgvUser_MouseDoubleClick);
            // 
            // BtnReset
            // 
            this.BtnReset.Enabled = false;
            this.BtnReset.Location = new System.Drawing.Point(265, 52);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(94, 23);
            this.BtnReset.TabIndex = 5;
            this.BtnReset.Text = "允许重置密码";
            this.BtnReset.UseVisualStyleBackColor = true;
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // 用户管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 520);
            this.Controls.Add(this.DgvUser);
            this.Controls.Add(this.PanelTitle);
            this.Controls.Add(this.DgvMain);
            this.Name = "用户管理";
            this.Text = "权限管理";
            this.SizeChanged += new System.EventHandler(this.FormMain_Resized);
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.Button BtnSetBasePerm;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnFind;
        private System.Windows.Forms.TextBox TextBoxU_ID;
        private System.Windows.Forms.Label LableU_ID;
        private System.Windows.Forms.DataGridView DgvUser;
        private System.Windows.Forms.Button BtnReset;
    }
}