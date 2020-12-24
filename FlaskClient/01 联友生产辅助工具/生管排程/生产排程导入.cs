using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ.生管排程
{
    public partial class 生产排程导入 : Form
    {
        private string connYF = FormLogin.infObj.connYF;
        private string connWG = FormLogin.infObj.connWG;

        private Mssql mssql = new Mssql();

        private DataTable inputDt = null;
        private bool ignore = false;

        #region 初始化
        public 生产排程导入(DataTable dt)
        {
            InitializeComponent();
            inputDt = dt;
            Init();
        }

        private void Init()
        {
            BtnInput.Enabled = false;

            DgvOpt.SetRowBackColor(DgvPreDo);
            DgvOpt.SetColHeadMiddleCenter(DgvPreDo);
            DgvOpt.SetRowBackColor(DgvDone);
            DgvOpt.SetColHeadMiddleCenter(DgvDone);

            if (inputDt != null)
            {
                inputDt.Columns.Add("状态");
                inputDt.Columns["状态"].SetOrdinal(0);
                if (inputDt.Columns.Contains("ERP订单数量")) inputDt.Columns.Remove("ERP订单数量");
                inputDt.Columns.Add("ERP订单数量");
                if (inputDt.Columns.Contains("已排数量")) inputDt.Columns.Remove("已排数量");
                inputDt.Columns.Add("已排数量");
                if (inputDt.Columns.Contains("品号")) inputDt.Columns.Remove("品号");
                inputDt.Columns.Add("品号");
                if (inputDt.Columns.Contains("品名")) inputDt.Columns.Remove("品名");
                inputDt.Columns.Add("品名");
                if (inputDt.Columns.Contains("规格")) inputDt.Columns.Remove("规格");
                inputDt.Columns.Add("规格");
                if (inputDt.Columns.Contains("客户配置")) inputDt.Columns.Remove("客户配置");
                inputDt.Columns.Add("客户配置");
                if (inputDt.Columns.Contains("客户名称")) inputDt.Columns.Remove("客户名称");
                inputDt.Columns.Add("客户名称");
                if (inputDt.Columns.Contains("交货日期")) inputDt.Columns.Remove("交货日期");
                inputDt.Columns.Add("交货日期");
                if (inputDt.Columns.Contains("已排行数")) inputDt.Columns.Remove("已排行数");
                inputDt.Columns.Add("已排行数");

                DtOpt.DtDateFormat(inputDt, "日期");
                DgvPreDo.DataSource = inputDt;
                DgvOpt.SetColHeadMiddleCenter(DgvPreDo);
                DgvOpt.SetColReadonly(DgvPreDo);
                DgvOpt.SetColMiddleCenter(DgvPreDo, "状态");
                DgvOpt.SetColMiddleCenter(DgvPreDo, "日期");
                DgvOpt.SetColMiddleCenter(DgvPreDo, "数量");
                DgvOpt.SetColWritable(DgvPreDo, "上线数量");
                DgvOpt.SetColWidth(DgvPreDo, "生产单号", 150);

            }
            else
            {
                Msg.ShowErr("无传入的数据！");
            }
            UI();
        }

        private void 生产排程导入_Resize(object sender, EventArgs e)
        {
            PanelDataPreDo.Width = PanelMain.Width * 3 / 5;
        }
        #endregion

        #region 界面
        private void UI()
        {
            label3.Text = "";
            if (DgvPreDo.DataSource != null)
            {
                BtnCheck.Enabled = true;
                label3.Text = string.Format("共有：{0} 行", ((DataTable)DgvPreDo.DataSource).Rows.Count.ToString());
            }
            else
            {
                DgvDone.DataSource = null;
            }
        }

        private void DgvDone_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // 显示定位
                if (e.RowIndex >= 0)
                {
                    contextMenuStrip_DgvDone.Visible = true;
                    DgvDone.ClearSelection();
                    DgvDone.Rows[e.RowIndex].Selected = true;
                    DgvDone.CurrentCell = DgvDone.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    contextMenuStrip_DgvDone.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void contextMenuStrip_DgvDone_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "修改")
            {
                contextMenuStrip_DgvDone_ItemClicked_Edit();
            }
            if (e.ClickedItem.Text == "删除")
            {
                contextMenuStrip_DgvDone_ItemClicked_Delete();
            }
            int rowIndex = DgvPreDo.CurrentCell.RowIndex;
            GetPreErpData(rowIndex);
            CheckPreData(rowIndex);

            ShowDoneData(rowIndex);

            UI();
        }

        private void DgvPreDo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!ignore)
            {
                int rowIndex = e.RowIndex;
                int colIndex = e.ColumnIndex;
                if (DgvPreDo.Columns[colIndex].Name == "上线数量")
                {
                    GetPreErpData(rowIndex);
                    CheckPreData(rowIndex);
                }
            }
        }

        private void DgvPreDo_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    // 显示定位
                    if (e.RowIndex >= 0)
                    {
                        contextMenuStrip_DgvPreDo.Visible = true;
                        DgvPreDo.ClearSelection();
                        DgvPreDo.Rows[e.RowIndex].Selected = true;
                        DgvPreDo.CurrentCell = DgvPreDo.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        contextMenuStrip_DgvPreDo.Show(MousePosition.X, MousePosition.Y);
                    }
                }

                if (e.Button == MouseButtons.Left)
                {
                    ShowDoneData(e.RowIndex);
                }
            }
            catch
            {

            }
        }

        private void contextMenuStrip_DgvPreDo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "删除")
            {
                contextMenuStrip_DgvPreDo_ItemClicked_Delete();
            }
            UI();
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            for (int rowIndex = 0; rowIndex < DgvPreDo.Rows.Count; rowIndex++)
            {
                GetPreErpData(rowIndex);
                CheckPreData(rowIndex);
            }
            Msg.Show("检查完成！");
            BtnInput.Enabled = true;
            DgvDone.DataSource = null;
        }

        private void BtnInput_Click(object sender, EventArgs e)
        {
            InputData();
            if (DgvPreDo.Rows.Count > 0)
            {
                Msg.Show("已导入部分，还存在状态为异常的行未导入！");
            }
            else
            {
                Msg.Show("已全部导入！");
                DgvDone.DataSource = null;
            }
            UI();
        }
        #endregion

        #region 逻辑
        #region 待导入排程数据处理逻辑
        private void contextMenuStrip_DgvPreDo_ItemClicked_Delete()
        {
            int rowIndex = DgvPreDo.CurrentCell.RowIndex;
            DgvPreDo.Rows.RemoveAt(rowIndex);
        }

        private void CheckPreData(int rowIndex)
        {
            if(rowIndex >= 0)
            {
                float slPre = float.Parse(DgvPreDo.Rows[rowIndex].Cells["上线数量"].Value.ToString());
                float slErp = float.Parse(DgvPreDo.Rows[rowIndex].Cells["ERP订单数量"].Value.ToString());
                float slDone = float.Parse(DgvPreDo.Rows[rowIndex].Cells["已排数量"].Value.ToString());
                float count = float.Parse(DgvPreDo.Rows[rowIndex].Cells["已排行数"].Value.ToString());

                if (GetPreExistPlanData(rowIndex))
                {
                    DgvPreDo.Rows[rowIndex].Cells["状态"].Value = "已存在";
                    DgvPreDo.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                }
                else
                {
                    if (slErp - slDone >= slPre)
                    {
                        DgvPreDo.Rows[rowIndex].Cells["状态"].Value = "";
                        DgvPreDo.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;

                    }
                    else
                    {
                        if ((slPre == slErp) && (slPre == slDone) && (count == 1))
                        {
                            DgvPreDo.Rows[rowIndex].Cells["状态"].Value = "变更日期车间";
                            DgvPreDo.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Purple;
                        }
                        else
                        {
                            DgvPreDo.Rows[rowIndex].Cells["状态"].Value = "异常";
                            DgvPreDo.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }

        private void CheckPreData()
        {
            for(int rowIndex = 0; rowIndex < DgvPreDo.Rows.Count; rowIndex++)
            {
                CheckPreData(rowIndex);
            }
        }

        private void GetPreErpData(int rowIndex)
        {
            ignore = true;
            string dd = DgvPreDo.Rows[rowIndex].Cells["生产单号"].Value.ToString();
            string sqlStr1 = @"SELECT RTRIM(MA002) 客户名称, RTRIM(TD004) 品号, RTRIM(TD005) 品名, RTRIM(TD006) 规格, RTRIM(TD053) 客户配置, CAST(TD008 AS FLOAT) ERP订单数量, TD013 交货日期
                                FROM COPTC(NOLOCK) 
                                INNER JOIN COPTD(NOLOCK) ON TD001 = TC001 AND TD002 = TC002 
                                LEFT JOIN COPMA(NOLOCK) ON MA001 = TC004
                                WHERE RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) = '{0}'";

            string sqlStr2 = @"SELECT ISNULL(CAST(SUM(SC013) AS FLOAT), 0) 已排数量 FROM SC_PLAN(NOLOCK) WHERE SC001 = '{0}' GROUP BY SC001 ";
            string sqlStr3 = @"SELECT ISNULL(COUNT(SC001), 0) 已排行数 FROM SC_PLAN(NOLOCK) WHERE SC001 = '{0}' GROUP BY SC001, SC003, SC013, SC023 ";
            DataTable dt1 = mssql.SQLselect(connYF, string.Format(sqlStr1, dd));
            DataTable dt2 = mssql.SQLselect(connWG, string.Format(sqlStr2, dd));
            DataTable dt3 = mssql.SQLselect(connWG, string.Format(sqlStr3, dd));
            if (dt1 != null)
            {
                DtOpt.DtDateFormat(dt1, "日期");
                DgvPreDo.Rows[rowIndex].Cells["客户名称"].Value = dt1.Rows[0]["客户名称"].ToString();
                DgvPreDo.Rows[rowIndex].Cells["品号"].Value = dt1.Rows[0]["品号"].ToString();
                DgvPreDo.Rows[rowIndex].Cells["品名"].Value = dt1.Rows[0]["品名"].ToString();
                DgvPreDo.Rows[rowIndex].Cells["规格"].Value = dt1.Rows[0]["规格"].ToString();
                DgvPreDo.Rows[rowIndex].Cells["客户配置"].Value = dt1.Rows[0]["客户配置"].ToString();
                DgvPreDo.Rows[rowIndex].Cells["ERP订单数量"].Value = dt1.Rows[0]["ERP订单数量"].ToString();
                DgvPreDo.Rows[rowIndex].Cells["交货日期"].Value = dt1.Rows[0]["交货日期"].ToString();
            }
            else
            {
                DgvPreDo.Rows[rowIndex].Cells["品号"].Value = "不存在ERP信息";
            }

            if (dt2 != null)
            {
                DgvPreDo.Rows[rowIndex].Cells["已排数量"].Value = dt2.Rows[0]["已排数量"].ToString();
            }
            else
            {
                DgvPreDo.Rows[rowIndex].Cells["已排数量"].Value = "0";
            }

            if (dt3 != null)
            {
                DgvPreDo.Rows[rowIndex].Cells["已排行数"].Value = dt3.Rows[0]["已排行数"].ToString();
            }
            else
            {
                DgvPreDo.Rows[rowIndex].Cells["已排行数"].Value = "0";
            }

            ignore = false;
        }

        private void GetPreErpData()
        {
            for(int rowIndex = 0; rowIndex < DgvPreDo.Rows.Count; rowIndex++)
            {
                GetPreErpData(rowIndex);
            }
        }

        private bool GetPreExistPlanData(int rowIndex)
        {
            string dd = DgvPreDo.Rows[rowIndex].Cells["生产单号"].Value.ToString();
            string sl = DgvPreDo.Rows[rowIndex].Cells["上线数量"].Value.ToString();
            string dpt = DgvPreDo.Rows[rowIndex].Cells["生产车间"].Value.ToString();
            string rq = DgvPreDo.Rows[rowIndex].Cells["上线日期"].Value.ToString().Replace("-", "");

            string sqlStr = @"SELECT 1 FROM SC_PLAN(NOLOCK) WHERE SC001 = '{0}' AND SC003 = '{1}' AND SC013 = '{2}' AND SC023 = '{3}' ";
            return mssql.SQLexist(connWG, string.Format(sqlStr, dd, rq, sl, dpt));
        }
        
        #endregion

        #region 已排排程数据逻辑
        private void contextMenuStrip_DgvDone_ItemClicked_Delete()
        {
            string slqStr = @"DELETE FROM dbo.SC_PLAN WHERE K_ID = '{0}' AND SC001 = '{1}' AND SC003 = '{2}' AND SC013 = '{3}' AND SC023 = '{4}'";
            int rowIndex = DgvDone.CurrentCell.RowIndex;
            string msg = string.Format("是否确认删除当前行！\n\n序号：'{0}'\n生产单号：'{1}'\n上线日期'{2}'\n生产车间：'{3}'\n上线数量：'{4}'",
                    DgvDone.Rows[rowIndex].Cells["序号"].Value.ToString(),
                    DgvDone.Rows[rowIndex].Cells["生产单号"].Value.ToString(),
                    DgvDone.Rows[rowIndex].Cells["上线日期"].Value.ToString().Replace("-", ""),
                    DgvDone.Rows[rowIndex].Cells["生产车间"].Value.ToString(),
                    DgvDone.Rows[rowIndex].Cells["上线数量"].Value.ToString());

            if (Msg.Show(msg) == DialogResult.OK)
            {
                mssql.SQLexcute(connWG, string.Format(slqStr, DgvDone.Rows[rowIndex].Cells["序号"].Value.ToString(),
                    DgvDone.Rows[rowIndex].Cells["生产单号"].Value.ToString(),
                    DgvDone.Rows[rowIndex].Cells["上线日期"].Value.ToString().Replace("-", ""),
                    DgvDone.Rows[rowIndex].Cells["上线数量"].Value.ToString(),
                    DgvDone.Rows[rowIndex].Cells["生产车间"].Value.ToString()
                    ));
            }
        }

        private void contextMenuStrip_DgvDone_ItemClicked_Edit()
        {
            int rowIndex = DgvDone.CurrentCell.RowIndex;
            生产排程修改 frm = new 生产排程修改("Edit", DgvDone.Rows[rowIndex].Cells["序号"].Value.ToString(), DgvDone.Rows[rowIndex].Cells["生产单号"].Value.ToString());
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                frm.Dispose();
                if (生产排程修改.saveFlag)
                {

                }
            }
        }

        private DataTable GetPlanData(int rowIndex)
        {
            string sqlStr = @"SELECT K_ID 序号, SC001 生产单号, SC003 上线日期, SC023 生产车间, SC013 上线数量 FROM SC_PLAN WHERE SC001 = '{0}' ORDER BY K_ID";
            string dd = DgvPreDo.Rows[rowIndex].Cells["生产单号"].Value.ToString();
            DataTable dt = mssql.SQLselect(connWG, string.Format(sqlStr, dd));
            return dt;
        }

        private void ShowDoneData(int rowIndex)
        {
            DataTable dt = GetPlanData(rowIndex);
            if (dt != null)
            {
                DtOpt.DtDateFormat(dt, "日期");
                DgvDone.DataSource = dt;
                DgvOpt.SetColWidth(DgvDone, "生产单号", 150);

            }
            else
            {
                DgvDone.DataSource = null;
            }
        }
        #endregion

        #region 导入数据处理
        private void InputData()
        {
            int rowIndexOffset = 0;
            int count = DgvPreDo.Rows.Count;
            for (; DgvPreDo.Rows.Count > rowIndexOffset;)
            {
                if (DgvPreDo.Rows[rowIndexOffset].Cells["状态"].Value.ToString() == "已存在")
                {
                    DgvPreDo.Rows.RemoveAt(rowIndexOffset);
                }
                else
                {
                    if (DgvPreDo.Rows[rowIndexOffset].Cells["状态"].Value.ToString() == "")
                    {
                        if (InputDataWork(rowIndexOffset))
                        {
                            DgvPreDo.Rows.RemoveAt(rowIndexOffset);
                        }
                        else
                        {
                            rowIndexOffset++;
                        }
                    }
                    else if(DgvPreDo.Rows[rowIndexOffset].Cells["状态"].Value.ToString() == "变更日期车间")
                    {
                        if (InputDataWork2(rowIndexOffset))
                        {
                            DgvPreDo.Rows.RemoveAt(rowIndexOffset);
                        }
                        else
                        {
                            rowIndexOffset++;
                        }
                    }
                    else if(DgvPreDo.Rows[rowIndexOffset].Cells["状态"].Value.ToString() == "异常")
                    {
                        rowIndexOffset++;
                    }
                }
            }
        }

        private bool InputDataWork(int rowIndex)
        {
            string slqStr = @"INSERT INTO WG_DB.dbo.SC_PLAN (CREATOR, CREATE_DATE, K_ID, SC001, SC003, SC013, SC023) 
                                    VALUES ('{0}', LEFT(COMFORT.dbo.f_getTime(1), 14), (SELECT ISNULL(MAX(K_ID), 0) + 1 FROM WG_DB.dbo.SC_PLAN), 
                                    '{1}', '{2}', '{3}', '{4}') ";
            int rtn = mssql.SQLexcute(connWG, string.Format(slqStr, FormLogin.infObj.userId,
                        DgvPreDo.Rows[rowIndex].Cells["生产单号"].Value.ToString(),
                        DgvPreDo.Rows[rowIndex].Cells["上线日期"].Value.ToString().Replace("-", ""),
                        DgvPreDo.Rows[rowIndex].Cells["上线数量"].Value.ToString(),
                        DgvPreDo.Rows[rowIndex].Cells["生产车间"].Value.ToString()));
            return rtn == 0 ? true : false;
        }

        private bool InputDataWork2(int rowIndex)
        {
            string slqStr = @"UPDATE WG_DB.dbo.SC_PLAN SET SC003 = '{2}', SC023 = '{3}', SC028 = '' WHERE SC001 = '{0}' AND SC013 = '{1}' ";
            string aa = string.Format(slqStr,
                        DgvPreDo.Rows[rowIndex].Cells["生产单号"].Value.ToString(),
                        DgvPreDo.Rows[rowIndex].Cells["上线数量"].Value.ToString(),
                        DgvPreDo.Rows[rowIndex].Cells["上线日期"].Value.ToString().Replace("-", ""),
                        DgvPreDo.Rows[rowIndex].Cells["生产车间"].Value.ToString());
            int rtn = mssql.SQLexcute(connWG, string.Format(slqStr, 
                        DgvPreDo.Rows[rowIndex].Cells["生产单号"].Value.ToString(),
                        DgvPreDo.Rows[rowIndex].Cells["上线数量"].Value.ToString(),
                        DgvPreDo.Rows[rowIndex].Cells["上线日期"].Value.ToString().Replace("-", ""),
                        DgvPreDo.Rows[rowIndex].Cells["生产车间"].Value.ToString()));
            return rtn == 0 ? true : false;
        }
        #endregion

        #endregion
    }
}
