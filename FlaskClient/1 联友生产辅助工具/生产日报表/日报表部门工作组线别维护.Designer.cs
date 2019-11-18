namespace 联友生产辅助工具.生产日报表
{
    partial class 日报表部门工作组线别维护
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
            this.ButtonSave = new System.Windows.Forms.Button();
            this.ButtonReflash = new System.Windows.Forms.Button();
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTitle
            // 
            this.PanelTitle.BackColor = System.Drawing.Color.White;
            this.PanelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitle.Controls.Add(this.ButtonSave);
            this.PanelTitle.Controls.Add(this.ButtonReflash);
            this.PanelTitle.Location = new System.Drawing.Point(2, 1);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(871, 46);
            this.PanelTitle.TabIndex = 0;
            // 
            // ButtonSave
            // 
            this.ButtonSave.Location = new System.Drawing.Point(133, 10);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(75, 23);
            this.ButtonSave.TabIndex = 1;
            this.ButtonSave.Text = "保存";
            this.ButtonSave.UseVisualStyleBackColor = true;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // ButtonReflash
            // 
            this.ButtonReflash.Location = new System.Drawing.Point(10, 11);
            this.ButtonReflash.Name = "ButtonReflash";
            this.ButtonReflash.Size = new System.Drawing.Size(75, 23);
            this.ButtonReflash.TabIndex = 0;
            this.ButtonReflash.Text = "刷新";
            this.ButtonReflash.UseVisualStyleBackColor = true;
            this.ButtonReflash.Click += new System.EventHandler(this.ButtonReflash_Click);
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMain.Location = new System.Drawing.Point(2, 97);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowTemplate.Height = 23;
            this.DgvMain.Size = new System.Drawing.Size(240, 150);
            this.DgvMain.TabIndex = 1;
            // 
            // 日报表部门工作组线别维护
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 556);
            this.Controls.Add(this.DgvMain);
            this.Controls.Add(this.PanelTitle);
            this.Name = "日报表部门工作组线别维护";
            this.Text = "日报表部门工作组线别维护";
            this.Resize += new System.EventHandler(this.Form_MainResized);
            this.PanelTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTitle;
        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.Button ButtonReflash;
    }
}