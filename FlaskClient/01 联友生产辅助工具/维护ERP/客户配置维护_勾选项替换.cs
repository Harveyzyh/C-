using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace HarveyZ.维护ERP
{
    public partial class 客户配置维护_勾选项替换 : Form
    {
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        private Mssql mssql = new Mssql();
        private string connYF = FormLogin.infObj.connYF;

        #region 初始化
        public 客户配置维护_勾选项替换(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            Init();
        }

        private void Init()
        {
            DataTable wlnoDt = new DataTable();
            wlnoDt.Columns.Add("原品号");
            wlnoDt.Columns.Add("替换品号");
            for (int index = 0; index < 80; index ++ )
            {
                DataRow dr = wlnoDt.NewRow();
                dr["原品号"] = "";
                dr["替换品号"] = "";
                wlnoDt.Rows.Add(dr);
            }
            DgvKeyValue.DataSource = wlnoDt;

            DgvOpt.SetRowBackColor(DgvKeyValue);
            DgvOpt.SetColWidth(DgvKeyValue, "原品号", 250);
            DgvOpt.SetColWidth(DgvKeyValue, "替换品号", 250);

            UI();
        }
        #endregion

        #region 界面
        private void UI()
        {
            if (TxbKhpzString.Text == "")
            {
                BtnWork.Enabled = false;
            }
            else
            {
                BtnWork.Enabled = true;
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            TxbLog.Text = "";

        }

        private void BtnLayout_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "(*.txt)|*.txt|(*.*)|*.*";
            saveFileDialog.FileName = "Log-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            //将日期时间作为文件名
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, true);
                streamWriter.Write(TxbLog.Text);
                streamWriter.Close();
            }
        }

        private void BtnWork_Click(object sender, EventArgs e)
        {
            string str = TxbKhpzString.Text.ToUpper();
            if (! (str.Contains("TQ001") && str.Contains("TQ002")))
            {
                Msg.ShowErr("SQL语句中必须存在字段TQ001, TQ002");
            }
            else
            {
                if (CheckDgvCorrect())
                {
                    if (Msg.Show("是否确认执行？\n\r请确认好客户配置范围与品号映射表是否正确.") == DialogResult.OK)
                    {
                        tabControl1.SelectedTab = tabControl1.TabPages[1];
                        string msg = "";
                        Work(out msg);
                        if(msg != "")
                        {
                            Msg.Show(msg);
                        }
                    }
                }
                else
                {
                    Msg.ShowErr("品号映射表异常，请检查");
                }
            }
        }
        
        private void TxbKhpzString_TextChanged(object sender, EventArgs e)
        {
            UI();
        }
        #endregion

        #region 逻辑
        /// <summary>
        /// 主处理程序
        /// </summary>
        private void Work(out string msg)
        {
            msg = "";
            LogAppend("\nWork Start");
            if(!CheckBoxPreCheck.Checked)
            {
                LogAppend("Work On Precheck");
            }

            string slqStr = TxbKhpzString.Text.Trim().ToUpper();
            DataTable khpzDt = GetKhpzDt(slqStr);
            if(khpzDt == null)
            {
                msg = "没有获取到客户配置，请检查";
                return;
            }
            else
            {
                DataTable wlnoDtTmp = (DataTable)DgvKeyValue.DataSource;
                DataTable wlnoDt = GetWlnoDt(wlnoDtTmp);
                WorkMain(khpzDt, wlnoDt, out msg);
                LogAppend(msg);
            }
        }

        /// <summary>
        /// 检查Dgv中内容是否正确
        /// </summary>
        /// <returns></returns>
        private bool CheckDgvCorrect()
        {
            bool rtn = false;
            DataTable wlnoDt = (DataTable)DgvKeyValue.DataSource;
            for(int index = 0; index < wlnoDt.Rows.Count; index++)
            {
                if (wlnoDt.Rows[index]["原品号"].ToString() != "" && wlnoDt.Rows[index]["替换品号"].ToString() != "")
                {
                    rtn = true;
                    break;
                }
            }
            return rtn;
        }

        private void LogAppend(string text)
        {
            if (text != "")
            {
                string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                TxbLog.AppendText(now + "\t" + text + "\n");
            }
            else
            {
                TxbLog.AppendText("\n");
            }
        }

        /// <summary>
        /// 根据传入的SQL获取客户配置的范围
        /// </summary>
        /// <param name="slqStr">SQL语句</param>
        /// <returns>客户配置Dt</returns>
        private DataTable GetKhpzDt(string slqStr)
        {
            DataTable dt = mssql.SQLselect(connYF, slqStr);
            return dt;
        }

        private DataTable GetWlnoDt(DataTable dt)
        {
            DataTable wlnoDt = dt.Clone();

            for(int index = 0; index < dt.Rows.Count; index++)
            {
                if (dt.Rows[index]["原品号"].ToString() != "" && dt.Rows[index]["替换品号"].ToString() != "")
                {
                    wlnoDt.ImportRow(dt.Rows[index]);
                }
            }

            return wlnoDt;
        }

        private DataTable GetTr003Dt(string tq001, string tq002, string tr009)
        {
            string slqStr = @"SELECT SUBSTRING(TR003, 1, LEN(TR003)-3) TR003 FROM COPTR WHERE TR001='{0}' AND TR002='{1}' AND TR009='{2}' AND TR017='Y'";
            DataTable tr003Dt = mssql.SQLselect(connYF, string.Format(slqStr, tq001, tq002, tr009));
            return tr003Dt;
        }

        /// <summary>
        /// 查询替换品号是否存在于此客户配置
        /// </summary>
        /// <param name="tq001">成品品号</param>
        /// <param name="tq002">客户配置名称</param>
        /// <param name="tr003">上阶层级码</param>
        /// <param name="wlnoNew">替换品号</param>
        /// <returns></returns>
        private bool GetExistWlno(string tq001, string tq002, string tr003, string wlnoNew)
        {
            string sql = string.Format("SELECT TR017 FROM COPTR WHERE TR001='{0}' AND TR002='{1}' AND SUBSTRING(TR003, 1, LEN(TR003)-3)='{2}' AND TR009='{3}' AND TR017 = 'N' ",
                tq001, tq002, tr003, wlnoNew);

            return mssql.SQLexist(connYF, sql);
        }

        private void WorkMain(DataTable khpzDt, DataTable wlnoDt, out string msg)
        {
            msg = "";
            int khpzRowCount = khpzDt.Rows.Count;

            for(int pzRowIndex = 0; pzRowIndex < khpzDt.Rows.Count; pzRowIndex++)
            {
                string tq001 = khpzDt.Rows[pzRowIndex]["TQ001"].ToString().Trim();
                string tq002 = khpzDt.Rows[pzRowIndex]["TQ002"].ToString().Trim();

                LogAppend("");
                LogAppend("Working With: " + (pzRowIndex+1).ToString() + "/" + khpzRowCount);
                LogAppend("1. " + tq001 + "-" + tq002);

                for(int wlnoRowIndex = 0; wlnoRowIndex < wlnoDt.Rows.Count; wlnoRowIndex++)
                {
                    string wlnoOld = wlnoDt.Rows[wlnoRowIndex]["原品号"].ToString().Trim();
                    string wlnoNew = wlnoDt.Rows[wlnoRowIndex]["替换品号"].ToString().Trim();

                    DataTable tr003Dt = GetTr003Dt(tq001, tq002, wlnoOld);
                    if (tr003Dt != null)
                    {
                        string tr003Str = "";
                        for(int index = 0; index < tr003Dt.Rows.Count; index++)
                        {
                            tr003Str += tr003Dt.Rows[index]["TR003"].ToString() + " | ";
                        }
                        LogAppend("2. WlnoOld: " + wlnoOld + "\t Tr003: " + tr003Str);

                        for(int tr003RowIndex = 0; tr003RowIndex < tr003Dt.Rows.Count; tr003RowIndex++)
                        {
                            string tr003 = tr003Dt.Rows[tr003RowIndex]["TR003"].ToString();
                            LogAppend("3. Tr003: " + tr003);
                            if(GetExistWlno(tq001, tq002, tr003, wlnoNew))
                            {
                                ReplaceWlno(tq001, tq002, tr003, wlnoOld, wlnoNew, CheckBoxPreCheck.Checked);
                            }
                            else
                            {
                                LogAppend("4. WlnoNew: " + wlnoNew + " Not Exist");
                            }
                        }
                    }
                    else
                    {
                        LogAppend("2. WlnoOld: " + wlnoOld + "\t Tr003: None");
                    }
                }
            }
        }

        /// <summary>
        /// 替换客户配置中的勾选项
        /// </summary>
        /// <param name="tq001">成品品号</param>
        /// <param name="tq002">客户配置名称</param>
        /// <param name="tr003">上阶层级码</param>
        /// <param name="wlnoOld">原品号</param>
        /// <param name="wlnoNew">替换品号</param>
        private void ReplaceWlno(string tq001, string tq002, string tr003, string wlnoOld, string wlnoNew, bool work)
        {
            string slqStr = @"UPDATE COPTR SET TR017 = '{4}' WHERE TR001='{0}' AND TR002='{1}' AND SUBSTRING(TR003, 1, LEN(TR003)-3)='{2}' AND TR009='{3}' ";

            string sql1 = string.Format(slqStr, tq001, tq002, tr003, wlnoNew, "Y");
            string sql2 = string.Format(slqStr, tq001, tq002, tr003, wlnoOld, "N");
            LogAppend(string.Format("4. Replace Wlno: {0}", sql1));
            if (work)
            {
                mssql.SQLexcute(connYF, sql1);
            }
            LogAppend(string.Format("4. Replace Wlno: {0}", sql2));
            if (work)
            {
                mssql.SQLexcute(connYF, sql2);
            }
        }
        #endregion
    }
}
