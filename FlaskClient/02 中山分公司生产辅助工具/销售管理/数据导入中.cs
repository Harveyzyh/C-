using System.Windows.Forms;

namespace HarveyZ
{
    public partial class 数据导入中 : Form
    {
        public 数据导入中(string text = "数据正在处理")
        {
            InitializeComponent();
            this.Text = text + this.Text;
        }
    }
}
