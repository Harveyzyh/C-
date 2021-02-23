using System;
using System.Data;
using System.Windows.Forms;
using HarveyZ.仓储中心;

namespace HarveyZ.生管排程
{
    public partial class 工单信息管理 : Form
    {
        DataGridViewRow dgvr = null;

        private Mssql mssql = new Mssql();
        private string connWG = FormLogin.infObj.connWG;
        private string connYF = FormLogin.infObj.connYF;

        public 工单信息管理(string text = "", DataGridViewRow dr = null)
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            this.dgvr = dr;
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
            if(dgvr == null)
            {
                Msg.ShowErr("传入信息错误");
            }
            else
            {
                TextBoxIndex.Text = dgvr.Cells["序号"].Value.ToString();
                TextBoxDd.Text = dgvr.Cells["订单号"].Value.ToString();
                TextBoxDpt.Text = dgvr.Cells["生产车间"].Value.ToString();
                TextBoxDdType.Text = dgvr.Cells["订单类型"].Value.ToString();
                TextBoxTradeType.Text = dgvr.Cells["贸易方式"].Value.ToString();
                TextBoxSxSl.Text = dgvr.Cells["上线数量"].Value.ToString();

                TextBoxWlno.Text = dgvr.Cells["品号"].Value.ToString();
                TextBoxWlnoName.Text = dgvr.Cells["品名"].Value.ToString();
                TextBoxWlnoSpec.Text = dgvr.Cells["规格"].Value.ToString();
                TextBoxDdDate.Text = dgvr.Cells["订单时间"].Value.ToString();
                TextBoxGdSl.Text = GetBoundGsSl();

                TextBoxKhpz.Text = dgvr.Cells["配置方案"].Value.ToString();
                TextBoxMsbz.Text = dgvr.Cells["描述备注"].Value.ToString();
                TextBoxChangeReason.Text = dgvr.Cells["变更原因"].Value.ToString();

                DgvShow();
            }

            DgvOpt.SetRowBackColor(DgvMain);
            DgvOpt.SetColHeadMiddleCenter(DgvMain);
        }

        #endregion

        #region 按钮
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            DgvShow();
        }

        private void DgvMain_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // 添加右键按钮items
                contextMenuStrip_DgvMain.Items.Clear();

                contextMenuStrip_DgvMain.Items.Add("修改为当前排程序号");
                contextMenuStrip_DgvMain.Items.Add("清除排程序号");

                if (DgvMain.Rows[e.RowIndex].Cells["审核码"].Value.ToString() == "未审核")
                {
                    contextMenuStrip_DgvMain.Items.Add("审核工单");
                }
                if (DgvMain.Rows[e.RowIndex].Cells["审核码"].Value.ToString() == "已审核")
                {
                    contextMenuStrip_DgvMain.Items.Add("生成领料单");
                }

                // 显示定位
                if (contextMenuStrip_DgvMain.Items.Count > 0)
                {
                    if (e.RowIndex >= 0)
                    {
                        contextMenuStrip_DgvMain.Visible = true;
                        DgvMain.ClearSelection();
                        DgvMain.Rows[e.RowIndex].Selected = true;
                        DgvMain.CurrentCell = DgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        contextMenuStrip_DgvMain.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }

        private void contextMenuStrip_DgvMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            contextMenuStrip_DgvMain.Visible = false;
            int rowIndex = DgvMain.CurrentCell.RowIndex;

            if (e.ClickedItem.Text == "审核工单")
            {
                contextMenuStrip_DgvMain_ItemClicked_GdConferm();
            }
            if (e.ClickedItem.Text == "生成领料单")
            {
                contextMenuStrip_DgvMain_ItemClicked_LlCreate();
            }
            if (e.ClickedItem.Text == "同步部门")
            {
                contextMenuStrip_DgvMain_ItemClicked_GdUpdateDpt();
            }
            if (e.ClickedItem.Text == "修改为当前排程序号")
            {
                contextMenuStrip_DgvMain_ItemClicked_GdUpdateIndex();
            }
            if (e.ClickedItem.Text == "清除排程序号")
            {
                contextMenuStrip_DgvMain_ItemClicked_GdUpdateIndexClean();
            }

            TextBoxGdSl.Text = GetBoundGsSl(); 
            DgvShow();

            DgvOpt.SelectLastRow(DgvMain, rowIndex);
        }
        #endregion

        #region 逻辑
        #region 右键按钮
        private void contextMenuStrip_DgvMain_ItemClicked_GdUpdateDpt()
        {
            //int rowIndex = DgvMain.CurrentCell.RowIndex;
            //string slqStr = @"";
        }

        private void contextMenuStrip_DgvMain_ItemClicked_LlCreate()
        {
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            string db = DgvMain.Rows[rowIndex].Cells["工单别"].Value.ToString();
            string dh = DgvMain.Rows[rowIndex].Cells["工单号"].Value.ToString();
            string gdStr = dh;
            生成领料单 frm = new 生成领料单("仓储中心_生成领料单", gdStr);
            try
            {
                frm.ShowDialog();
            }
            catch { }
        }

        private void contextMenuStrip_DgvMain_ItemClicked_GdConferm()
        {
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            string db = DgvMain.Rows[rowIndex].Cells["工单别"].Value.ToString();
            string dh = DgvMain.Rows[rowIndex].Cells["工单号"].Value.ToString();
            
            string slqStr1 = @"UPDATE dbo.MOCTA SET TA040=CONVERT(VARCHAR(8), GETDATE(), 112), TA013='U', FLAG=(FLAG%999)+1, TA041='{2}' "
                           + "WHERE TA001='{0}' AND TA002='{1}'";
            string slqStr2 = @"SELECT TB001, TB002, TB003, TB006 FROM dbo.MOCTB WHERE TB001='{0}' AND TB002='{1}' ";

            string slqStr3 = @"UPDATE dbo.MOCTB SET TB018='Y', FLAG=(FLAG%999)+1 "
                           + @"WHERE TB001='{0}' AND TB002='{1}' AND TB003='{2}' AND TB006='{3}' ";


            string slqStr4 = @"UPDATE dbo.MOCTA SET TA013='Y', TA049='N' WHERE TA001='{0}' AND TA002='{1}' ";
            
            if (mssql.SQLexcute(connYF, string.Format(slqStr1, db, dh, FormLogin.infObj.userId)) == 1) // 出错，未执行
            {
                Msg.ShowErr(string.Format("工单：{0}-{1} 未审核!", db, dh));
            }
            else
            {
                DataTable dt = mssql.SQLselect(connYF, string.Format(slqStr2, db, dh));
                if (dt != null)
                {
                    for (int index = 0; index < dt.Rows.Count; index++)
                    {
                        mssql.SQLexcute(connYF, string.Format(slqStr3, db, dh, dt.Rows[index]["TB003"].ToString().Trim(), dt.Rows[index]["TB006"].ToString()));
                    }
                }
                mssql.SQLexcute(connYF, string.Format(slqStr4, db, dh));
                Msg.Show(string.Format("工单：{0}-{1} 已审核!", db, dh));
            }
        }

        private void contextMenuStrip_DgvMain_ItemClicked_GdUpdateIndex()
        {
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            string slqStr = @"UPDATE MOCTA SET UDF02 = '{2}' WHERE TA001 = '{0}' AND TA002 = '{1}' ";
            string slqStr2 = @"UPDATE MOCTA SET UDF03 = SC003 FROM MOCTA INNER JOIN WG_DB.dbo.SC_PLAN ON K_ID = MOCTA.UDF02 WHERE ISNULL(UDF03, '') != SC003 ";
            mssql.SQLexcute(connYF, string.Format(slqStr, DgvMain.Rows[rowIndex].Cells["工单别"].Value.ToString(),
                DgvMain.Rows[rowIndex].Cells["工单号"].Value.ToString(), TextBoxIndex.Text));
            mssql.SQLexcute(connYF, slqStr2);
        }

        private void contextMenuStrip_DgvMain_ItemClicked_GdUpdateIndexClean()
        {
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            string slqStr = @"UPDATE MOCTA SET UDF02 = '' WHERE TA001 = '{0}' AND TA002 = '{1}' ";
            mssql.SQLexcute(connYF, string.Format(slqStr, DgvMain.Rows[rowIndex].Cells["工单别"].Value.ToString(),
                DgvMain.Rows[rowIndex].Cells["工单号"].Value.ToString()));
        }
        #endregion

        private string GetBoundGsSl()
        {
            string slqStr = @"SELECT CAST(SUM(TA015) AS FLOAT) TA015 FROM COMFORT.dbo.MOCTA WHERE TA013 NOT IN ('V', 'U') AND RTRIM(TA026)+'-'+RTRIM(TA027)+'-'+RTRIM(TA028) = '{0}' AND MOCTA.UDF02 = '{1}' AND MOCTA.TA006 = '{2}' ";
            DataTable dt = mssql.SQLselect(connYF, string.Format(slqStr, TextBoxDd.Text, TextBoxIndex.Text, TextBoxWlno.Text));
            if (dt != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        private void DgvShow()
        {
            string slqStr = @"SELECT ISNULL(MOCTA.UDF02, '') 排程序号, RTRIM(TA001) 工单别, RTRIM(TA002) 工单号, TA003 开单日期, 
                                CAST(TA015 AS FLOAT) 预计产量, RTRIM(ME002) 生产部门, 
                                (CASE TA011 WHEN '1' THEN '未生产' WHEN '2' THEN '已发料' WHEN '3' THEN '生产中' WHEN 'Y' THEN '已完工' WHEN 'y' THEN '指定结束' END) 工单状态, 
                                (CASE TA013 WHEN 'N' THEN '未审核' WHEN 'Y' THEN '已审核' ELSE TA013 END) 审核码, 
                                RTRIM(TA006) 品号, RTRIM(MB002) 品名, RTRIM(MB003) 规格 
                                FROM MOCTA 
                                LEFT JOIN CMSME ON TA064 = ME001 
                                LEFT JOIN INVMB ON MB001 = TA006 
                                WHERE 1=1
                                AND RTRIM(TA026) + '-' + RTRIM(TA027) +'-'+ RTRIM(TA028) = '{0}' ";
            if (CheckBoxScGd.Checked && !CheckBoxScGd.Checked) slqStr += @" AND TA001 = '5121' ";
            if (!CheckBoxScGd.Checked && CheckBoxScGd.Checked) slqStr += @" AND TA001 = '5101' ";
            if (CheckBoxScGd.Checked && CheckBoxScGd.Checked) slqStr += @" AND TA001 IN ('5101', '5121') ";
            if (CheckBoxSameIndex.Checked) slqStr += string.Format(@" AND MOCTA.UDF02 = '{0}' ", TextBoxIndex.Text);
            if (CheckBoxSameWlno.Checked) slqStr += string.Format(@" AND MOCTA.TA006 = '{0}' ", TextBoxWlno.Text);

            slqStr += " ORDER BY RTRIM(TA001), RTRIM(TA002) ";

            DataTable dt = mssql.SQLselect(connYF, string.Format(slqStr, TextBoxDd.Text));
            if (dt != null)
            {
                DgvMain.DataSource = dt;
                DtOpt.DtDateFormat(dt, "日期");
                DgvOpt.SetColMiddleCenter(DgvMain);
                DgvOpt.SetColWidth(DgvMain, "规格", 180);
            }
        }
        #endregion
    }
}
