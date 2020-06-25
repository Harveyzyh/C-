using System.Data;
using System.Windows.Forms;
using FastReport;

namespace 联友采购平台.供应商
{
    public partial class 打印供应商送货单 : Form
    {
        Report report = new Report();
        public 打印供应商送货单(string frx, DataSet ds)
        {
            InitializeComponent();
            PreviewShow(frx, ds);
        }

        private void PreviewShow(string frx, DataSet ds)
        {
            previewControl1.Buttons = (PreviewButtons.Print);
            report.LoadFromString(frx);
            report.RegisterData(ds);
            report.PrintSettings.ShowDialog = true;
            report.Preview = previewControl1;

            report.Prepare();
            report.ShowPrepared();
        }
    }
}
