namespace 联友采购平台.供应商
{
    partial class 打印供应商送货单
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
            this.previewControl1 = new FastReport.Preview.PreviewControl();
            this.SuspendLayout();
            // 
            // previewControl1
            // 
            this.previewControl1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.previewControl1.Buttons = ((FastReport.PreviewButtons)((FastReport.PreviewButtons.Print | FastReport.PreviewButtons.PageSetup)));
            this.previewControl1.Clouds = FastReport.PreviewClouds.None;
            this.previewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewControl1.Exports = FastReport.PreviewExports.None;
            this.previewControl1.Font = new System.Drawing.Font("宋体", 9F);
            this.previewControl1.Location = new System.Drawing.Point(0, 0);
            this.previewControl1.Name = "previewControl1";
            this.previewControl1.PageOffset = new System.Drawing.Point(10, 10);
            this.previewControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.previewControl1.SaveInitialDirectory = null;
            this.previewControl1.Size = new System.Drawing.Size(899, 613);
            this.previewControl1.TabIndex = 0;
            // 
            // 打印供应商送货单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 613);
            this.Controls.Add(this.previewControl1);
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Name = "打印供应商送货单";
            this.Text = "打印供应商送货单";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private FastReport.Preview.PreviewControl previewControl1;
    }
}