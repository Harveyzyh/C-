namespace HarveyZ
{
    partial class 版本发布
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
            this.BtnSetNew = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnSetNew
            // 
            this.BtnSetNew.Location = new System.Drawing.Point(24, 22);
            this.BtnSetNew.Name = "BtnSetNew";
            this.BtnSetNew.Size = new System.Drawing.Size(115, 23);
            this.BtnSetNew.TabIndex = 0;
            this.BtnSetNew.Text = "发布此版本号";
            this.BtnSetNew.UseVisualStyleBackColor = true;
            this.BtnSetNew.Click += new System.EventHandler(this.BtnSetNew_Click);
            // 
            // 版本发布
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 347);
            this.Controls.Add(this.BtnSetNew);
            this.Name = "版本发布";
            this.Text = "版本发布";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnSetNew;
    }
}