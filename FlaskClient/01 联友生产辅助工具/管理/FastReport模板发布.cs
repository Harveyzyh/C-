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
        private static FastReportContentUpload_Main frMain = new FastReportContentUpload_Main(FormLogin.infObj.connWG);

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;

        public FastReport模板发布(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag);
            Init();
        }

        private void Init()
        {
            foreach(string tmp in frMain.GetPrintType())
            {
                ComboBoxPrintType.Items.Add(tmp);
            }
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
            foreach (string tmp in frMain.GetPrintName(ComboBoxPrintType.SelectedItem.ToString()))
            {
                ComboBoxPrintName.Items.Add(tmp);
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            if (frMain.UploadContent(ComboBoxPrintType.SelectedItem.ToString(), ComboBoxPrintName.SelectedItem.ToString(), textBox1.Text))
            {
                MessageBox.Show("上传成功", "提示", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("上传失败", "提示", MessageBoxButtons.OK);
            }
        }
    }

    public class FastReportContentUpload_Main
    {
        private static string connWG = "";
        private static Mssql mssql = new Mssql();

        public FastReportContentUpload_Main(string conn)
        {
            connWG = conn;
        }

        public List<string> GetPrintType()
        {
            List<string> rtnList = new List<string>();
            string sqlstr = @"SELECT DISTINCT PRINT_TYPE FROM WG_DB.dbo.WG_PRINT ORDER BY PRINT_TYPE ";
            DataTable dt = mssql.SQLselect(connWG, sqlstr);
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    rtnList.Add(dt.Rows[rowIndex][0].ToString());
                }
            }
            return rtnList;
        }

        public List<string> GetPrintName(string printType)
        {
            List<string> rtnList = new List<string>();
            string sqlstr = @"SELECT DISTINCT PRINT_NAME FROM WG_DB.dbo.WG_PRINT WHERE PRINT_TYPE = '{0}' ORDER BY PRINT_NAME ";
            DataTable dt = mssql.SQLselect(connWG, string.Format(sqlstr, printType));
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    rtnList.Add(dt.Rows[rowIndex][0].ToString());
                }
            }
            return rtnList;
        }

        public bool UploadContent(string printType, string printName, string content)
        {
            string sqlstrExist = @"SELECT CONTENT FROM WG_DB.dbo.WG_PRINT WHERE PRINT_TYPE = '{0}' AND PRINT_NAME = '{1}' ";
            string sqlstrUpdate = @"UPDATE WG_DB.dbo.WG_PRINT SET CONTENT = '{2}' WHERE PRINT_TYPE = '{0}' AND PRINT_NAME = '{1}' ";
            string sqlstrInsert = @"INSERT INTO WG_DB.dbo.WG_PRINT(PRINT_TYPE, PRINT_NAME, CONTENT) VALUES('{0}', '{1}', '{2}') ";
            try {
                if (mssql.SQLexist(connWG, string.Format(sqlstrExist, printType, printName)))
                {
                    mssql.SQLexcute(connWG, string.Format(sqlstrUpdate, printType, printName, content.Replace(@"'", @"''")));
                }
                else
                {
                    mssql.SQLexcute(connWG, string.Format(sqlstrInsert, printType, printName, content.Replace(@"'", @"''")));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
