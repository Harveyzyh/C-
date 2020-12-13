using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ
{
    public partial class FastReport模板发布 : Form
    {
        private FastReportManager frManager = new FastReportManager(FormLogin.infObj.connWG);

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        public FastReport模板发布(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            Init();
        }

        private void Init()
        {
            SetComboBoxPrintTypeList();
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            string fullPath = "";

            if (FileOpt.OpenFile("FastReport文件|*.frx", out fullPath))
            {
                LabelFilePath.Text = fullPath;
                string content = FileOpt.ReadFileGetContent(fullPath);
                textBox1.Text = content;
            }
        }

        private void ComboBoxPrintType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string printType = ComboBoxPrintType.SelectedItem == null ? ComboBoxPrintType.Text.Trim() : ComboBoxPrintType.SelectedItem.ToString();

            SetComboBoxPrintNameList(printType);
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            string printType = ComboBoxPrintType.SelectedItem == null ? ComboBoxPrintType.Text.Trim() : ComboBoxPrintType.SelectedItem.ToString();
            string printName = ComboBoxPrintName.SelectedItem == null ? ComboBoxPrintName.Text.Trim() : ComboBoxPrintName.SelectedItem.ToString();
            string context = textBox1.Text;

            if (frManager.SetPrintFile(printType, printName, context))
            {
                Msg.Show("上传成功", "提示", MessageBoxButtons.OK);
            }
            else
            {
                Msg.Show("上传失败", "提示", MessageBoxButtons.OK);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string printType = ComboBoxPrintType.SelectedItem == null ? ComboBoxPrintType.Text.Trim() : ComboBoxPrintType.SelectedItem.ToString();
            string printName = ComboBoxPrintName.SelectedItem == null ? ComboBoxPrintName.Text.Trim() : ComboBoxPrintName.SelectedItem.ToString();

            if (Msg.Show(string.Format("确认删除FastReport模板。类型：{0}， 名称：{1} ？", printType, printName)) == DialogResult.OK)
            {
                if (frManager.DelPrintFile(printType, printName))
                {
                    Msg.Show("删除成功", "提示", MessageBoxButtons.OK);
                }
                else
                {
                    Msg.Show("删除失败", "提示", MessageBoxButtons.OK);
                }
            }
        }

        private void SetComboBoxPrintTypeList()
        {
            ComboBoxPrintType.Items.Clear();

            foreach (string tmp in frManager.GetPrintTypeList())
            {
                ComboBoxPrintType.Items.Add(tmp);
            }
        }

        private void SetComboBoxPrintNameList(string printType)
        {
            ComboBoxPrintName.Items.Clear();

            foreach (string tmp in frManager.GetPrintNameList(printType))
            {
                ComboBoxPrintName.Items.Add(tmp);
            }
        }
    }
}
