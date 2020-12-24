using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace HarveyZ.品管
{
    public partial class 成品标签打印 : Form
    {
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;
        
        private Mssql mssql = new Mssql();
        private string connWG = FormLogin.infObj.connWG;
        private string connYF = FormLogin.infObj.connYF;
        private DataTable printDt = new DataTable();
        private DataSet printDs = new DataSet();

        public 成品标签打印(string text = "", string dd = null, string num = null)
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            TextBoxDd.Text = dd == null ? "" : dd;
            TextBoxNumber.Text = num == null ? "" : num;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            Init();
            FormMain_Resized_Work();
        }

        #region 窗口大小变化设置
        private void FormMain_Resized(object sender, EventArgs e)
        {
            FormMain_Resized_Work();
        }

        public void FormMain_Resized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
        }

        #endregion

        #region 界面
        private void Init()
        {
            foreach(string tmp in GetPrintName("产品标签"))
            {
                ComboBoxPrintName.Items.Add(tmp);
            }
            if (ComboBoxPrintName.Items.Count > 0) ComboBoxPrintName.SelectedIndex = 0;


            printDt.TableName = "Dict";
            printDs.Tables.Add(printDt);

            printDt.Columns.Add("序号");
            printDt.Columns.Add("客户编号");
            printDt.Columns.Add("单号");
            printDt.Columns.Add("品号");
            printDt.Columns.Add("产品名称");
            printDt.Columns.Add("颜色");
            printDt.Columns.Add("净重");
            printDt.Columns.Add("毛重");
            printDt.Columns.Add("纸箱尺寸");
            printDt.Columns.Add("备注");
            printDt.Columns.Add("商品代码");
            printDt.Columns.Add("电商代码");
            printDt.Columns.Add("检验日期");

            if(TextBoxDd.Text != "")
            {
                TextBoxDd.Enabled = false;
                GetData();
            }
        }

        private void TextBoxDd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                AddPrintDt();
                FastReport打印预览 frm = new FastReport打印预览(printDs, getfrx(ComboBoxPrintName.SelectedItem.ToString()));
                frm.ShowDialog();
            }
            catch
            {
                Msg.ShowErr("打印出现错误。");
            }
        }

        #endregion

        #region 逻辑
        public List<string> GetPrintName(string printType)
        {
            List<string> rtnList = new List<string>();
            string slqStr = @"SELECT DISTINCT PRINT_NAME FROM WG_DB.dbo.WG_PRINT WHERE PRINT_TYPE = '{0}' ORDER BY PRINT_NAME ";
            DataTable dt = mssql.SQLselect(connWG, string.Format(slqStr, printType));
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    rtnList.Add(dt.Rows[rowIndex][0].ToString());
                }
            }
            return rtnList;
        }
        
        //获取打印格式
        private string getfrx(string fileName)
        {
            string slqStr = @"SELECT CONTENT FROM dbo.WG_PRINT WHERE PRINT_TYPE = '产品标签' AND PRINT_NAME = '{0}' ";
            string frx = mssql.SQLselect(connWG, string.Format(slqStr, fileName)).Rows[0][0].ToString();
            return frx;
        }

        private void GetData()
        {
            string slqStr = "Select * From V_LabelPrint_WG where 订单号 = '{0}' ";
            if (TextBoxDd.Text != "")
            {
                DataTable dt = mssql.SQLselect(connYF, string.Format(slqStr, TextBoxDd.Text));

                if (dt != null)
                {
                    TextBoxNumber.Text = TextBoxNumber.Text == "" ? dt.Rows[0]["订单数量"].ToString() : TextBoxNumber.Text;
                    TextBoxCustomerId.Text = dt.Rows[0]["客户编号"].ToString();
                    TextBoxWlno.Text = dt.Rows[0]["品号"].ToString();
                    TextBoxName.Text = dt.Rows[0]["品名"].ToString();
                    TextBoxSpec.Text = dt.Rows[0]["规格"].ToString();
                    TextBoxByName.Text = dt.Rows[0]["保友品名"].ToString();
                    TextBoxByCode.Text = dt.Rows[0]["产品代码"].ToString();
                    TextBoxKhpz.Text = dt.Rows[0]["配置方案"].ToString();
                    TextBoxPzms.Text = dt.Rows[0]["配置描述"].ToString();
                    TextBoxJdCode.Text = dt.Rows[0]["描述备注"].ToString();
                    
                    string colorStr = dt.Rows[0]["配置方案"].ToString();
                    try
                    { if (colorStr.Substring(0, 3) == "保友 " || colorStr.Substring(0, 3) == "京东 " || colorStr.Substring(0, 3) == "电商 ")
                        {
                            // 修改打印模板
                            if (colorStr.Substring(0, 3) == "京东 " || colorStr.Substring(0, 3) == "电商 ")
                            {
                                ComboBoxPrintName.SelectedIndex = 1;
                            }
                            else
                            {
                                ComboBoxPrintName.SelectedIndex = 0;
                            }

                            // 修改颜色名称
                            if (colorStr.Length > 3)
                            {
                                colorStr = colorStr.Substring(3, colorStr.Length - 3);
                            }
                        }
                    }
                    catch
                    {
                        
                    }

                    TextBoxColor.Text = colorStr;
                    TextBoxRemark.Text = dt.Rows[0]["描述备注"].ToString();
                    TextBoxBoxSize.Text = dt.Rows[0]["纸箱尺寸"].ToString();
                    DateTimePickerCheckDate.Text = dt.Rows[0]["检验日期"].ToString();
                    TextBoxCrossWeight.Text = dt.Rows[0]["毛重"].ToString();
                    TextBoxNetWeight.Text = dt.Rows[0]["净重"].ToString();
                }
            }
        }

        //添加数据
        private void AddPrintDt()
        {
            printDt.Clear();

            for (int index = 0; index < int.Parse(TextBoxNumber.Text); index++)
            {
                DataRow dr = printDt.NewRow();
                dr["序号"] = (index+1).ToString();
                dr["客户编号"] = TextBoxCustomerId.Text;
                dr["单号"] = TextBoxDd.Text;
                dr["品号"] = TextBoxWlno.Text;
                dr["产品名称"] = TextBoxByName.Text;
                dr["颜色"] = TextBoxColor.Text;
                dr["净重"] = TextBoxCrossWeight.Text == "" ? "" : TextBoxCrossWeight.Text + "Kg";
                dr["毛重"] = TextBoxNetWeight.Text == "" ? "" : TextBoxNetWeight.Text + "Kg";
                dr["纸箱尺寸"] = TextBoxBoxSize.Text;
                dr["备注"] = TextBoxRemark.Text;
                dr["商品代码"] = TextBoxByCode.Text;
                dr["电商代码"] = TextBoxRemark.Text;
                dr["检验日期"] = DateTimePickerCheckDate.Value.ToString("yyyy-MM-dd");

                printDt.Rows.Add(dr);
            }
        }
        #endregion
    }
}
