using System;
using System.Data;
using System.Windows.Forms;

namespace HarveyZ.生管排程
{
    public partial class 生产排程修改 : Form
    {
        private string connWG = FormLogin.infObj.connWG;
        private string connYF = FormLogin.infObj.connYF;
        Mssql mssql = new Mssql();

        private string index, mode, dd;
        public static string indexRtn = "";
        public static bool saveFlag = false;

        public 生产排程修改(string mode, string index, string dd)
        {
            InitializeComponent();

            saveFlag = false;
            textBoxDd.ReadOnly = true;
            this.mode = mode;
            this.index = index;
            this.dd = dd;
            indexRtn = index;

            label1.Text = "排程序号：" + index;
            textBoxDd.Text =  dd;
            
            Init();

            if (mode == "Add")
            {
                GetInfo();
                textBoxDd.ReadOnly = true;
            }
            if (mode == "New")
            {
                if(textBoxDd.Text != "") GetInfo();
                textBoxDd.ReadOnly = false;
            }
            if (mode == "Edit")
            {
                GetInfo();
                textBoxDd.ReadOnly = true;
            }
        }

        #region 按钮
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (mode == "Add") BtnSaveNewWork();
            else if (mode == "Edit") BtnSaveEditWork();
            else if (mode == "New") BtnSaveNewWork();
            else this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxDd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBoxDd.ReadOnly == false)
            {
                GetInfo();
            }
        }

        private void textBoxDd_Leave(object sender, EventArgs e)
        {
            if (textBoxDd.ReadOnly == false)
            {
                GetInfo();
            }
        }
        #endregion

        private void Init()
        {
            string slqStr = "SELECT Dpt FROM dbo.SC_PLAN_DPT_TYPE WHERE Valid = 1 AND Type = 'In' ORDER BY K_ID";
            DataTable dt = mssql.SQLselect(connWG, slqStr);
            if (dt != null)
            {
                for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                {
                    comboBoxDpt.Items.Add(dt.Rows[rowIdx][0].ToString());
                }
            }

            if (comboBoxDpt.SelectedItem == null) comboBoxDpt.SelectedIndex = 0;
        }

        private void GetInfo()
        {
            string slqStr = @"SELECT RTRIM(TD004) 品号, RTRIM(TD005) 品名, RTRIM(TD006) 规格, RTRIM(TD053) 配置方案, CAST(TD008 AS FLOAT) ERP订单数量, ISNULL(SC013, 0) 已排数量 
                                FROM dbo.COPTD 
                                LEFT JOIN (SELECT SC001, SUM(SC013) SC013 FROM WG_DB.dbo.SC_PLAN GROUP BY SC001) AS SC_PLAN ON RTRIM(TD001)+'-'+RTRIM(TD002)+'-'+RTRIM(TD003) = SC001
                                WHERE RTRIM(TD001)+'-'+RTRIM(TD002)+'-'+RTRIM(TD003) = '{0}'";
            DataTable dt = mssql.SQLselect(connYF, string.Format(slqStr, textBoxDd.Text));

            string sqlStr2 = @"SELECT SC013 上线数量, SC003 上线日期, SC023 生产车间 FROM SC_PLAN WHERE K_ID = '{0}' AND SC001 = '{1}' ";
            DataTable dt2 = mssql.SQLselect(connWG, string.Format(sqlStr2, index, dd));


            if (dt2 != null)
            {
                DtOpt.DtDateFormat(dt2, "日期");
                dateTimePicker1.Text = dt2.Rows[0]["上线日期"].ToString();
                comboBoxDpt.Text = dt2.Rows[0]["生产车间"].ToString();
                textBoxSl.Text = dt2.Rows[0]["上线数量"].ToString();
            }

            if (dt != null)
            {
                label3.Text = "品号：" + dt.Rows[0]["品号"].ToString();
                label4.Text = "品名：" + dt.Rows[0]["品名"].ToString();
                label5.Text = "规格：" + dt.Rows[0]["规格"].ToString();
                label6.Text = "配置方案：" + dt.Rows[0]["配置方案"].ToString();
                label10.Text = "ERP订单数量：" + dt.Rows[0]["ERP订单数量"].ToString();
                label11.Text = "已排数量：" + dt.Rows[0]["已排数量"].ToString();

                if(textBoxSl.Text == "")
                {
                    textBoxSl.Text = (int.Parse(dt.Rows[0]["ERP订单数量"].ToString()) - int.Parse(dt.Rows[0]["已排数量"].ToString())).ToString();
                }
            }
        }

        private float GetDsl()
        {
            string slqStr = @"SELECT CAST(ISNULL(TD008 - ISNULL(SC1.SC013, 0) + ISNULL(SC2.SC013, 0), 0) AS FLOAT) DSL FROM COMFORT.dbo.COPTD 
                                LEFT JOIN dbo.SC_PLAN AS SC1 ON RTRIM(TD001)+'-'+RTRIM(TD002)+'-'+RTRIM(TD003) = SC1.SC001 
                                LEFT JOIN dbo.SC_PLAN AS SC2 ON RTRIM(TD001)+'-'+RTRIM(TD002)+'-'+RTRIM(TD003) = SC2.SC001 AND CAST(SC2.K_ID AS VARCHAR(100)) = '{1}'
                                WHERE RTRIM(TD001)+'-'+RTRIM(TD002)+'-'+RTRIM(TD003) = '{0}'";
            DataTable dt = mssql.SQLselect(connWG, string.Format(slqStr, textBoxDd.Text, index));
            if (dt != null)
            {
                return float.Parse(dt.Rows[0][0].ToString());
            }
            else return 0;
        }
        
        private void UptInfo()
        {
            string slqStr = @"UPDATE WG_DB.dbo.SC_PLAN SET 
                                SC002 = X00, SC004 = X01, SC005 = X02, SC006 = X03, SC007 = X04, SC008 = X05, 
                                SC009 = X06, SC010 = X07, SC011 = X08, SC012 = X09, SC015 = X10, 
                                SC016 = X11, SC017 = X12, SC018 = X13, SC019 = X14, SC020 = X15, SC021 = X16, 
                                SC022 = X17, SC024 = X18, SC025 = X19, SC026 = X20, SC027 = X21, SC028 = X22
                                FROM WG_DB.dbo.SC_PLAN
                                INNER JOIN (
	                                SELECT 
	                                RTRIM(COPTD.TD001) + '-' + RTRIM(COPTD.TD002) + '-' + RTRIM(COPTD.TD003) AS DD,
	                                (CASE WHEN TC004='0118' THEN '内销' ELSE '外销' END) AS X00,
	                                RTRIM(COPMA.MA002) AS X01,
	                                RTRIM(COPTC.TC015) AS X02,
	                                RTRIM((CASE WHEN COPTF.TF003 IS NULL THEN '' WHEN COPTF.TF003 IS NOT NULL AND COPTF.TF017 = 'Y' 
		                                THEN '指定结束'+':'+'变更版本号'+COPTF.TF003+'_'+COPTF.UDF11 ELSE '变更版本号'+COPTF.TF003+'_'+COPTF.UDF11 END)) AS X03,
	                                RTRIM((CASE WHEN COPTD.TD013 = '' THEN '' WHEN COPTD.TD013 IS NULL THEN '' ELSE COPTD.TD013 END)) AS X04,
	                                RTRIM((CASE WHEN COPTD.UDF03 = '' THEN '' WHEN COPTD.UDF03 IS NULL THEN '' ELSE COPTD.UDF03 END)) AS X05,
	                                RTRIM(COPTD.UDF01) AS X06,
	                                RTRIM(COPTD.TD005) AS X07,
	                                RTRIM(COPTD.UDF08) AS X08,
	                                RTRIM(COPTD.TD006) AS X09,
	                                RTRIM(COPTD.TD053) AS X10,
	                                RTRIM(COPTQ.TQ003) AS X11,
	                                RTRIM((COPTQ.UDF07+COPTD.TD020)) AS X12, 
	                                RTRIM(COPTD.TD204) AS X13, 
	                                RTRIM(COPTC.TC035) AS X14,
	                                RTRIM(CMSMV.MV002) AS X15, 
	                                RTRIM(COPTC.TC012) AS X16, 
	                                RTRIM(COPTD.TD014) AS X17,
	                                RTRIM(COPTD.UDF05) AS X18,
	                                RTRIM(COPTD.UDF10) AS X19,
	                                RTRIM((CASE WHEN COPTC.UDF09='否' THEN '' ELSE '是' END)) AS X20,
	                                RTRIM(COPTD.UDF12) AS X21, 
	                                RTRIM(COPTD.TD004) AS X22, 
                                    RTRIM(COPTQ.UDF07) AS X23 
	                                FROM COMFORT.dbo.COPTD AS COPTD 
	                                Left JOIN COMFORT.dbo.COPTC AS COPTC On COPTD.TD001=COPTC.TC001 and COPTD.TD002=COPTC.TC002 
	                                Left JOIN COMFORT.dbo.COPTQ AS COPTQ On COPTD.TD053=COPTQ.TQ002 and COPTD.TD004=COPTQ.TQ001 
	                                Left JOIN COMFORT.dbo.COPMA AS COPMA On COPTC.TC004=COPMA.MA001 
	                                Left JOIN COMFORT.dbo.CMSMV AS CMSMV On COPTC.TC006=CMSMV.MV001 
	                                LEFT JOIN COMFORT.dbo.INVMB AS INVMB ON COPTD.TD004=INVMB.MB001 
	                                Left JOIN COMFORT.dbo.COPTF AS COPTF On COPTD.TD001=COPTF.TF001 and COPTD.TD002=COPTF.TF002 and COPTD.TD003=COPTF.TF104 
		                                AND COPTF.TF003 = (SELECT MAX(TF003) FROM COMFORT.dbo.COPTF  
			                                WHERE COPTD.TD001=COPTF.TF001 and COPTD.TD002=COPTF.TF002 and COPTD.TD003=COPTF.TF104) 
	                                LEFT JOIN COMFORT.dbo.CMSME AS CMSME ON CMSME.ME001 = INVMB.MB445 
                                ) AS A ON SC001 = DD
                                WHERE (SC028 IS NULL OR SC028 ='') ";
            mssql.SQLexcute(connWG, slqStr);
        }

        private void BtnSaveNewWork()
        {
            if (textBoxDd.Text != "")
            {
                string slqStr = @"INSERT INTO dbo.SC_PLAN (CREATOR, CREATE_DATE, K_ID, SC001, SC003, SC013, SC014, SC023) 
                                VALUES ('{0}', LEFT(COMFORT.dbo.f_getTime(1), 14), (SELECT ISNULL(MAX(K_ID), 0) + 1 FROM WG_DB.dbo.SC_PLAN), 
                                '{1}', '{2}', {3}, 0, '{4}') ";
                string slqStr2 = @"SELECT ISNULL(MAX(K_ID), -1) FROM dbo.SC_PLAN WHERE SC001 = '{0}' AND SC003 = '{1}' AND SC013 = {2} AND SC023 = '{3}' ";
                try
                {
                    float sl = float.Parse(textBoxSl.Text);
                    float dsl = GetDsl();
                    if (sl <= dsl)
                    {

                        mssql.SQLexcute(connWG, string.Format(slqStr, FormLogin.infObj.userId, textBoxDd.Text, dateTimePicker1.Value.ToString("yyyyMMdd"), textBoxSl.Text, comboBoxDpt.SelectedItem.ToString()));
                        indexRtn = mssql.SQLselect(connWG, string.Format(slqStr2, textBoxDd.Text, dateTimePicker1.Value.ToString("yyyyMMdd"), textBoxSl.Text, comboBoxDpt.SelectedItem.ToString())).Rows[0][0].ToString();
                        UptInfo();
                        saveFlag = true;
                        this.Close();
                    }
                    else
                    {
                        Msg.ShowErr("数量输入错误，当前订单最大可排数量为：" + dsl.ToString());
                        textBoxSl.Select();
                        textBoxSl.SelectAll();
                    }
                }
                catch
                {
                    Msg.ShowErr("数量输入不正确");
                }
            }
            else
            {
                Msg.ShowErr("订单号不能为空");
            }
        }

        private void BtnSaveEditWork()
        {
            string slqStr = @"UPDATE dbo.SC_PLAN SET SC003 = '{2}', SC013 = {3}, SC023 = '{4}', SC028 = '', SC029=LEFT(dbo.f_getTime(1), 12) WHERE K_ID = '{0}' AND SC001 = '{1}' ";
            try
            {
                float sl = float.Parse(textBoxSl.Text);
                float dsl = GetDsl();
                if (sl <= dsl)
                {
                    mssql.SQLexcute(connWG, string.Format(slqStr, index, textBoxDd.Text, dateTimePicker1.Value.ToString("yyyyMMdd"), textBoxSl.Text, comboBoxDpt.SelectedItem.ToString()));
                    UptInfo();
                    saveFlag = true;
                    this.Close();
                }
                else
                {
                    Msg.ShowErr("数量输入错误，当前订单最大可排数量为：" + dsl.ToString());
                    textBoxSl.Select();
                    textBoxSl.SelectAll();
                }
            }
            catch
            {
                Msg.ShowErr("数量输入不正确");
            }
        }
    }
}
