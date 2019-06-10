using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友中山分公司生产辅助工具
{
    public partial class 生成出货排程 : Form
    {
        private static Mssql mssql = new Mssql();
        private static string connYLs = Global_Const.strConnection_Y_LS;
        private string uid = 物控登录.Uid;
        private 新增排程数据 frm = new 新增排程数据();

        public 生成出货排程()
        {
            InitializeComponent();
            DtpStartDate.Checked = true;
            DtpEndDate.Checked = false;
            DtpStartDate.Value = DateTime.Now.AddDays(-1);
            DtpEndDate.Value = DateTime.Now.AddDays(1);
            ShowDt();
        }
        
        #region 主窗体按钮
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) //窗体上的关闭按钮
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region 窗口大小变化设置
        private void Form_MainResized(object sender, EventArgs e)
        {
            Form_MainResized_Work();
        }

        private void Form_MainResized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
            PanelTitle.Location = new Point(2, 2);
            PanelTitle.Size = new Size(FormWidth, PanelTitle.Height);
            DgvMain.Location = new Point(2, PanelTitle.Height + 3);
            DgvMain.Size = new Size(FormWidth, FormHeight - PanelTitle.Height - 6);
        }
        #endregion
        
        private DateTime GetNextMonday()
        {
            DateTime dTime = new DateTime();
            DateTime dTimeBack = new DateTime();

            string dTimeStr = mssql.SQLselect(connYLs, "SELECT CONVERT(VARCHAR(20), GETDATE(), 112) ").Rows[0][0].ToString();

            dTime = DateTime.ParseExact(dTimeStr, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

            if (dTime.DayOfWeek.ToString() == "Monday")
            {
                dTime = dTime.AddDays(1);
            }

            for (bool breakFLag = false; !breakFLag; dTime = dTime.AddDays(1))
            {
                if (dTime.DayOfWeek.ToString() == "Monday")
                {
                    dTimeBack = dTime;
                    breakFLag = true;
                }
            }

            return dTimeBack;
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            ShowDt();
        }

        private void BtnLayout_Click(object sender, EventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            新增排程数据.modeAdd = true;
            frm.ShowDialog();
            ShowDt();
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            string sqlstr = @"
                            -- 创建单号
                            declare @dh varchar(20)
                            declare @sl int
                            declare @uid varchar(20)
                            set @uid = '{0}'
                            set @dh = (select top 1 'XP-' +convert(varchar(20), substring(crkdh, 4, 9) +1) as crkdh from mf_chpd order by crkdh desc )
                            set @sl = (
	                            select count(*) from (
		                            select distinct posd.wlno, posd.name, posd.cz, posd.ys, posd.spec, posd.unit, (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) as sl, 0 as dj, post.cgrq + 1 as chrq, 
		                            posd.bz, dh.crkdh, ROW_NUMBER() over(order by posd.seq) as seq, post.cgdh as indh, posd.seq as inseq, 'CHPD' as crklx, 0 as jsbz, posd.dddh as dddh, 
		                            posd.csdh, posd.ck, 0 as chsl, '联友公司' as gxss, null as thinkdh, null as thinseq
		                            from mf_pos as post
		                            inner join tf_pos as posd on post.cgdh = posd.cgdh
		                            left join (
			                            select wlno, indh, inseq, sum(sl) as sl from tf_chpd group by wlno, indh, inseq
		                            ) as chpdd on chpdd.wlno = posd.wlno and chpdd.indh = posd.cgdh and chpdd.inseq = posd.seq
		                            left join (select '' as crkdh) as dh on 1=1
		                            where 1=1
		                            and post.polx = 'PA' -- 客户订单
		                            and post.gysno = 'C001'
		                            and post.shbz = 1
		                            and posd.jsbz = 0
		                            and post.jsbz = 0
		                            and (posd.dddh like '3308%' or posd.dddh like '3305%')
		                            and posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end) != 0
		                            and convert(datetime, post.cgrq, 112) = convert(varchar(20), getdate(), 112)	
	                            ) as sl
                            )

                            if(@sl > 0)
                            begin

	                            -- 写入单头
	                            insert into mf_chpd
	                            select @dh as crkdh, @uid as zydm, null as zy, null as crkcpt, 'TS01' as gsdm, '仓库部' as zbdm , 
	                            0 as jsbz, 'CHPD' as crklx, convert(datetime, convert(varchar(20), getdate(), 112), 112) as crkrq, 
	                            @uid as shr, getdate() as shrq, 1 as shbz, null as shdh, getdate() as ZDRQ, 'C001' as khno, '联友' as khname 

	                            -- 写入单身
	                            insert into tf_chpd
	                            select distinct posd.wlno, posd.name, posd.cz, posd.ys, posd.spec, posd.unit, (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) as sl, 0 as dj, post.cgrq + 1 as chrq, 
	                            posd.bz, dh.crkdh, ROW_NUMBER() over(order by posd.seq) as seq, post.cgdh as indh, posd.seq as inseq, 'CHPD' as crklx, 0 as jsbz, posd.dddh as dddh, 
	                            posd.csdh, posd.ck, 0 as chsl, '联友公司' as gxss, null as thinkdh, null as thinseq
	                            -- update tf_pos set cgjq = post.cgrq + 1
	                            from mf_pos as post
	                            inner join tf_pos as posd on post.cgdh = posd.cgdh
	                            left join (
		                            select wlno, indh, inseq, sum(sl) as sl from tf_chpd group by wlno, indh, inseq
	                            ) as chpdd on chpdd.wlno = posd.wlno and chpdd.indh = posd.cgdh and chpdd.inseq = posd.seq
	                            left join (select @dh as crkdh) as dh on 1=1
	                            where 1=1
	                            and post.polx = 'PA' -- 客户订单
	                            and post.gysno = 'C001'
	                            and post.shbz = 1
	                            and posd.jsbz = 0
	                            and post.jsbz = 0
	                            and (posd.dddh like '3308%' or posd.dddh like '3305%')
	                            and convert(datetime, post.cgrq, 112) = convert(varchar(20), getdate(), 112)
	                            order by post.cgdh, posd.seq
                            end

                            select @dh as a, @sl as a  ";
            DataTable dt = mssql.SQLselect(connYLs, string.Format(sqlstr, uid));
            if (dt != null)
            {
                if (int.Parse(dt.Rows[0][1].ToString()) > 0)
                {
                    UpdPcsl(dt.Rows[0][0].ToString());
                    MessageBox.Show("已生成出货排程，单号：" + dt.Rows[0][0].ToString(), "提示");
                }
                else MessageBox.Show("没有未生成出货排程的订单，没有创建新的排程单", "提示");
            }
            else
            {
                MessageBox.Show("没有获取到数据", "错误");
            }
            ShowDt();
        }

        private void BtnGenerate2_Click(object sender, EventArgs e)
        {
            string sqlstr = @"select convert(varchar(20), chrq, 112), scdh, sl from zzzz_chpc where flag = 0 order by chrq";
            string msg = "";
            string pcdh = GetPcdh();
            DataTable dt = mssql.SQLselect(connYLs, sqlstr);
            for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                if (dt.Rows[rowIndex][2].ToString() != "-1")
                {
                    if (CheckError(dt.Rows[rowIndex][1].ToString(), dt.Rows[rowIndex][2].ToString(), out msg))
                    {
                        InsertDetail21(pcdh, dt.Rows[rowIndex][0].ToString(), dt.Rows[rowIndex][1].ToString());
                        UpdChpc(dt.Rows[rowIndex][0].ToString(), dt.Rows[rowIndex][1].ToString(), "1");
                    }
                    else
                    {
                        UpdChpc(dt.Rows[rowIndex][0].ToString(), dt.Rows[rowIndex][1].ToString(), "1", msg);
                    }
                }
                else
                {
                    InsertDetail22(pcdh, dt.Rows[rowIndex][0].ToString(), dt.Rows[rowIndex][1].ToString());
                    UpdChpc(dt.Rows[rowIndex][0].ToString(), dt.Rows[rowIndex][1].ToString(), "1");
                }
            }
            UpdChrq(pcdh);
            UpdPcsl(pcdh);
            MessageBox.Show("已生成出货排程，排程单号为：" + pcdh, "提示");
            ShowDt();
        }

        private void DgvMain_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = DgvMain.CurrentRow.Index;
            新增排程数据.modeAdd = false;
            新增排程数据.scdh = DgvMain.Rows[rowIndex].Cells[1].Value.ToString();
            新增排程数据.date = DgvMain.Rows[rowIndex].Cells[0].Value.ToString();
            新增排程数据.sl = DgvMain.Rows[rowIndex].Cells[2].Value.ToString();
            frm.ShowDialog();
        }

        private void ShowDt()
        {
            string sqlstr = @"select distinct convert(varchar(20), zzzz_chpc.chrq, 112) 出货日期, zzzz_chpc.scdh 生产单号, zzzz_chpc.sl 数量, flag 已处理, msg 异常记录
                                from zzzz_chpc
                                left join tf_pos on zzzz_chpc.scdh = tf_pos.csdh
                                where 1=1
                                and (zzzz_chpc.scdh like '%{0}%' or tf_pos.bz like '%{0}%' )";
            if (DtpStartDate.Checked) { sqlstr += string.Format(" and convert(varchar(20), zzzz_chpc.chrq, 112) >= '{0}' ", DtpStartDate.Value.ToString("yyyyMMdd")); }
            if (DtpEndDate.Checked) { sqlstr += string.Format(" and convert(varchar(20), zzzz_chpc.chrq, 112) <= '{0}' ", DtpEndDate.Value.ToString("yyyyMMdd")); }
            sqlstr += " order by convert(varchar(20), zzzz_chpc.chrq, 112), zzzz_chpc.scdh, zzzz_chpc.sl, flag, msg ";

            DataTable dt = mssql.SQLselect(connYLs, string.Format(sqlstr, textBox1.Text));
            DgvMain.DataSource = null;
            if (dt != null)
            {
                DgvMain.DataSource = dt;
                DgvOpt.SetRowColor(DgvMain);
            }
        }

        public static DataTable GetBaseDt(string scdh)
        {
            string sqlstr = @"declare @scdh varchar(20)
                                declare @rownum int
                                declare @minnum int
                                declare @yucount int
                                declare @shang int
                                declare @maxnum int
                                set @scdh = '{0}'

                                select @rownum = count(posd.wlno)
                                from mf_pos as post
                                inner join tf_pos as posd on post.cgdh = posd.cgdh
                                left join (
	                                select wlno, indh, inseq, sum(sl) as sl from tf_chpd group by wlno, indh, inseq
                                ) as chpdd on chpdd.wlno = posd.wlno and chpdd.indh = posd.cgdh and chpdd.inseq = posd.seq
                                left join (select '' as crkdh) as dh on 1=1
                                where 1=1
                                and post.polx = 'PA' -- 客户订单
                                and post.gysno = 'C001'
                                and post.shbz = 1
                                and posd.jsbz = 0
                                and post.jsbz = 0
                                and posd.dddh like '3301%'
                                and posd.csdh = @scdh
                                and posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end) > 0

                                select @minnum = min(posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end))
                                from mf_pos as post
                                inner join tf_pos as posd on post.cgdh = posd.cgdh
                                left join (
	                                select wlno, indh, inseq, sum(sl) as sl from tf_chpd group by wlno, indh, inseq
                                ) as chpdd on chpdd.wlno = posd.wlno and chpdd.indh = posd.cgdh and chpdd.inseq = posd.seq
                                left join (select '' as crkdh) as dh on 1=1
                                where 1=1
                                and post.polx = 'PA' -- 客户订单
                                and post.gysno = 'C001'
                                and post.shbz = 1
                                and posd.jsbz = 0
                                and post.jsbz = 0
                                and posd.dddh like '3301%'
                                and posd.csdh = @scdh
                                and posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end) > 0

                                select @yucount = count(posd.wlno)
                                from mf_pos as post
                                inner join tf_pos as posd on post.cgdh = posd.cgdh
                                left join (
	                                select wlno, indh, inseq, sum(sl) as sl from tf_chpd group by wlno, indh, inseq
                                ) as chpdd on chpdd.wlno = posd.wlno and chpdd.indh = posd.cgdh and chpdd.inseq = posd.seq
                                left join (select '' as crkdh) as dh on 1=1
                                where 1=1
                                and post.polx = 'PA' -- 客户订单
                                and post.gysno = 'C001'
                                and post.shbz = 1
                                and posd.jsbz = 0
                                and post.jsbz = 0
                                and posd.dddh like '3301%'
                                and posd.csdh = @scdh
                                and (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) % @minnum  > 0
                                and posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end) > 0

                                select @shang = max((posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) / @minnum)
                                from mf_pos as post
                                inner join tf_pos as posd on post.cgdh = posd.cgdh
                                left join (
	                                select wlno, indh, inseq, sum(sl) as sl from tf_chpd group by wlno, indh, inseq
                                ) as chpdd on chpdd.wlno = posd.wlno and chpdd.indh = posd.cgdh and chpdd.inseq = posd.seq
                                left join (select '' as crkdh) as dh on 1=1
                                where 1=1
                                and post.polx = 'PA' -- 客户订单
                                and post.gysno = 'C001'
                                and post.shbz = 1
                                and posd.jsbz = 0
                                and post.jsbz = 0
                                and posd.dddh like '3301%'
                                and posd.csdh = @scdh
                                and (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) / @minnum  > 0
                                and posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end) > 0

                                select @maxnum = @minnum - chpc.sl from (
                                select distinct (case when chpc.sl is null then 0 else chpc.sl end) as sl from zzzz_test as test
                                left join (select sum(sl) as sl from zzzz_chpc where flag = 0 and scdh =@scdh group by scdh) as chpc on 1=1
                                ) as chpc

                                select @scdh, @rownum, (case when @minnum is null then 0 else @minnum end), @yucount, 
                                (case when @shang is null then 0 else @shang end), (case when @maxnum is null then 0 else @maxnum end)";

            DataTable dt = mssql.SQLselect(connYLs, string.Format(sqlstr, scdh));
            return dt;
        }

        private string GetPcdh()
        {
            string pcdh = "";

            string sqlstr = @"
                            -- 创建单号
                            declare @dh varchar(20)
                            declare @uid varchar(20)
                            set @uid = '{0}'
                            set @dh = (select top 1 'XP-' +convert(varchar(20), substring(crkdh, 4, 9) +1) as crkdh from mf_chpd order by crkdh desc )
                            -- 写入单头
	                        insert into mf_chpd
	                        select @dh as crkdh, @uid as zydm, null as zy, null as crkcpt, 'TS01' as gsdm, '仓库部' as zbdm , 
	                        0 as jsbz, 'CHPD' as crklx, convert(datetime, convert(varchar(20), getdate(), 112), 112) as crkrq, 
	                        @uid as shr, getdate() as shrq, 1 as shbz, null as shdh, getdate() as ZDRQ, 'C001' as khno, '联友' as khname 
                            select @dh ";
            DataTable dt = mssql.SQLselect(connYLs, string.Format(sqlstr, uid));
            if(dt != null)
            {
                pcdh = dt.Rows[0][0].ToString();
            }
            return pcdh;
        }

        private bool CheckError(string scdh, string sl, out string msg)
        {
            msg = ""; 
            DataTable dt = GetBaseDt(scdh);
            string kk = dt.Rows[0][2].ToString();
            if (int.Parse(dt.Rows[0][1].ToString()) > 0)
            {
                if (int.Parse(dt.Rows[0][3].ToString()) == 0)
                {
                    if (int.Parse(dt.Rows[0][4].ToString()) <= 5)
                    {
                        if (int.Parse(dt.Rows[0][2].ToString()) >= int.Parse(sl))
                        {
                            return true;
                        }
                        else
                        {
                            msg = "可生成套数小于已录入套数";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "物料最大倍数超出5";
                        return false;
                    }
                }
                else
                {
                    msg = "数量已不成套";
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void InsertDetail21(string pcdh, string chrq, string scdh)
        {
            string sqlstr = @"
                            declare @dh varchar(20)
                            declare @uid varchar(20)
                            declare @scdh varchar(20)
                            declare @chrq varchar(20)
                            declare @chrqDate datetime
                            declare @maxseq int
                            declare @addsl int
                            declare @minnum int
                            set @uid = '{0}'
                            set @dh = '{1}'
                            set @scdh = '{2}'
                            set @chrq = '{3}'

                            -- 获取最大序号
                            select @maxseq = (case when max(seq) is null then 0 else max(seq)+1 end) from tf_chpd where crkdh = @dh

                            -- 获取套数
                            select @addsl = sl, @chrqDate = chrq from zzzz_chpc where chrq = @chrq and convert(varchar(20), scdh, 112) = @scdh

                            -- 获取最小值
                            select @minnum = min(bb.sl) from (
                            select distinct posd.wlno, posd.name, posd.cz, posd.ys, posd.spec, posd.unit, (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) as sl, 0 as dj, @chrqDate as chrq, 
                            posd.bz, dh.crkdh, ROW_NUMBER() over(order by posd.seq) + @maxseq as seq, post.cgdh as indh, posd.seq as inseq, 'CHPD' as crklx, 0 as jsbz, posd.dddh as dddh, 
                            posd.csdh, posd.ck, 0 as chsl, '联友公司' as gxss, null as thinkdh, null as thinseq
                            from mf_pos as post
                            inner join tf_pos as posd on post.cgdh = posd.cgdh
                            left join (
	                            select wlno, indh, inseq, sum(sl) as sl from tf_chpd group by wlno, indh, inseq
                            ) as chpdd on chpdd.wlno = posd.wlno and chpdd.indh = posd.cgdh and chpdd.inseq = posd.seq
                            left join (select @dh as crkdh) as dh on 1=1
                            inner join zzzz_chpc as chpc on chpc.scdh = posd.csdh
                            where 1=1
                            and post.polx = 'PA' -- 客户订单
                            and post.gysno = 'C001'
                            and post.shbz = 1
                            and posd.jsbz = 0
                            and post.jsbz = 0
                            and posd.dddh like '3301%'
                            and (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) > 0
                            and posd.csdh = @scdh
                            ) as bb

                            -- 写入单身
                            insert into tf_chpd
                            select distinct posd.wlno, posd.name, posd.cz, posd.ys, posd.spec, posd.unit, convert(numeric(10 ,2), (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end))/@minnum*@addsl) as sl, 
                            0 as dj, @chrqDate as chrq, posd.bz, dh.crkdh, ROW_NUMBER() over(order by posd.seq) + @maxseq as seq, post.cgdh as indh, posd.seq as inseq, 'CHPD' as crklx, 0 as jsbz, 
                            posd.dddh as dddh, posd.csdh, posd.ck, 0 as chsl, '联友公司' as gxss, null as thinkdh, null as thinseq
                            from mf_pos as post
                            inner join tf_pos as posd on post.cgdh = posd.cgdh
                            left join (
	                            select wlno, indh, inseq, sum(sl) as sl from tf_chpd group by wlno, indh, inseq
                            ) as chpdd on chpdd.wlno = posd.wlno and chpdd.indh = posd.cgdh and chpdd.inseq = posd.seq
                            left join (select @dh as crkdh) as dh on 1=1
                            inner join zzzz_chpc as chpc on chpc.scdh = posd.csdh and chpc.chrq = @chrqDate
                            where 1=1
                            and post.polx = 'PA' -- 客户订单
                            and post.gysno = 'C001'
                            and post.shbz = 1
                            and posd.jsbz = 0
                            and post.jsbz = 0
                            and posd.dddh like '3301%'
                            and (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) > 0
                            and posd.csdh = @scdh
                            order by post.cgdh, posd.seq ";
            mssql.SQLexcute(connYLs, string.Format(sqlstr, uid, pcdh, scdh, chrq));
        }

        private void InsertDetail22(string pcdh, string chrq, string scdh)
        {
            string sqlstr = @"declare @dh varchar(20)
                                declare @chrq varchar(20)
                                declare @scdh varchar(20)
                                declare @maxseq int
                                set @dh = '{0}'
                                set @chrq = '{1}'
                                set @scdh = '{2}'

                                -- 获取最大序号
                                select @maxseq = (case when max(seq) is null then 0 else max(seq)+1 end) from tf_chpd where crkdh = @dh

	                            -- 写入单身
	                            insert into tf_chpd
                                select distinct posd.wlno, posd.name, posd.cz, posd.ys, posd.spec, posd.unit, (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) as sl, 0 as dj, convert(datetime, @chrq, 112) as chrq, 
                                posd.bz, dh.crkdh, ROW_NUMBER() over(order by posd.seq) + @maxseq as seq, post.cgdh as indh, posd.seq as inseq, 'CHPD' as crklx, 0 as jsbz, posd.dddh as dddh, 
                                posd.csdh, posd.ck, 0 as chsl, '联友公司' as gxss, null as thinkdh, null as thinseq
                                from mf_pos as post
                                inner join tf_pos as posd on post.cgdh = posd.cgdh
                                left join (
	                                select wlno, indh, inseq, sum(sl) as sl from tf_chpd group by wlno, indh, inseq
                                ) as chpdd on chpdd.wlno = posd.wlno and chpdd.indh = posd.cgdh and chpdd.inseq = posd.seq
                                left join (select @dh as crkdh) as dh on 1=1
                                where 1=1
                                and post.polx = 'PA' -- 客户订单
                                and post.gysno = 'C001'
                                and post.shbz = 1
                                and posd.jsbz = 0
                                and posd.dddh like '3301%'
                                and (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) > 0
                                and posd.csdh = @scdh
                                order by post.cgdh, posd.seq ";
            mssql.SQLexcute(connYLs, string.Format(sqlstr, pcdh, chrq, scdh));
        }

        private void UpdChpc(string date, string scdh, string flag, string msg = "")
        {
            string sqlstr = @"update zzzz_chpc set flag = '{2}', msg = '{3}' where convert(varchar(20), chrq, 112) = '{0}' and scdh = '{1}' ";
            mssql.SQLexcute(connYLs, string.Format(sqlstr, date, scdh, flag, msg));
        }

        private void UpdChrq(string pcdh)
        {
            string sqlstr = @" update tf_chpd set chrq = convert(datetime, '{1}' , 112) where crkdh = '{0}' and ys != '大件产品' ";
            mssql.SQLexcute(connYLs, string.Format(sqlstr, pcdh, GetNextMonday().ToString("yyyyMMdd")));
        }

        private void UpdPcsl(string pcdh)
        {
            string sqlstr = @"update tf_pos set chpdsl = (case when chpd.sl is null then 0 else chpd.sl end)
                                from tf_pos as posd
                                left join 
                                (
                                select indh, inseq, wlno, sum(sl) as sl from tf_chpd group by indh, inseq, wlno
                                ) as chpd on chpd.wlno = posd.wlno and chpd.indh = posd.cgdh and chpd.inseq = posd.seq
                                where 1=1
                                and posd.cgdh in (select indh from tf_chpd where crkdh = '{0}')";
            mssql.SQLexcute(connYLs, string.Format(sqlstr, pcdh));
        }
    }
}
