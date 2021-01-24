using HarveyZ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HarveyZ.仓储中心
{
    public partial class 扫描领料单 : Form
    {
        DataTable dttmp = null;

        Mssql mssql = new Mssql();

        string connYF = FormLogin.infObj.connYF;
        
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        string xa007 = "";
        private bool MsgFlag = false;

        public 扫描领料单(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            Button_Upload.Enabled = false;
            checkedListBoxGroup.Enabled = false;
            checkBox2.Enabled = false;
            DgvOpt.SetRowBackColor(DataGridView_List);
            dateTimePicker1.Value = DateTime.Now.AddDays(2);

            //工作组
            foreach (string tmp in GetGroupList())
            {
                checkedListBoxGroup.Items.Add(tmp);
            }
        }

        #region 窗口大小变化设置
        private void Form_MainResized(object sender, EventArgs e)
        {
            Form_MainResized_Work();
            panel_Title.Enabled = true;
        }

        private void Form_MainResized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
            panel_Title.Size = new Size(FormWidth, panel_Title.Height);
            DataGridView_List.Location = new Point(0, panel_Title.Height + 2);
            DataGridView_List.Size = new Size(FormWidth, FormHeight - panel_Title.Height - 2);

        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            button1_Work();
        }

        private void button1_Work()
        {
            DataGridView_List.DataSource = null;
            if (checkBox1.Checked)
            {
                showList("", "");
            }
            else
            {
                if (TextBox_Danhao.Text != "")
                {
                    string danhao_L = TextBox_Danhao.Text;
                    string[] danhao_arry = danhao_L.Split('-');
                    string danbie = danhao_arry[0];
                    string danhao = danhao_arry[1];
                    check(danbie, danhao);
                }
                else
                {
                    MessageBox.Show("请录入单别单号", "错误");
                    Button_Upload.Enabled = false;
                    dttmp = null;
                    DataGridView_List.DataSource = null;
                }
            }
        }

        private void button1_Click_Enter(object sender, KeyEventArgs e)
        {
            if((MsgFlag == true) && (e.KeyCode == Keys.Enter))
            {
                MsgFlag = false;
                return;
            }
            else
            {
                MsgFlag = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                button1_Work();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MsgFlag = true;
            insertsql();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DataGridView_List.DataSource = null;
            if (checkBox1.Checked)
            {
                TextBox_Danhao.Text = "";
                TextBox_Danhao.Enabled = false;
                dateTimePicker1.Enabled = true;
                checkedListBoxGroup.Enabled = true;
                checkBox2.Enabled = true;
            }
            else
            {
                TextBox_Danhao.Enabled = true;
                dateTimePicker1.Enabled = false;
                checkedListBoxGroup.Enabled = false;
                checkBox2.Enabled = false;
            }
        }

        private void TextBoxChange(object sender, EventArgs e)
        {
            MsgFlag = false;
        }

        private void checkedListBoxGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridView_List.DataSource = null;
            Button_Upload.Enabled = false;
        }

        public List<string> GetGroupList()
        {
            string slqStr = "SELECT DISTINCT CAST(MW003 AS VARCHAR(100)) MW003 FROM CMSMW WHERE MW003 IS NOT NULL AND CAST(MW003 AS VARCHAR(100)) != '' ";
            DataTable dt = mssql.SQLselect(connYF, slqStr);
            if (dt != null)
            {
                List<string> groupList = new List<string>();
                for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                {
                    groupList.Add(dt.Rows[rowIdx][0].ToString());
                }
                return groupList;
            }
            else
            {
                return null;
            }
        }

        #region 判断是否有后台在运行
        private bool checkJobExists() {
            string slqStr = @" SELECT * FROM  DSCSYS.dbo.JOBQUEUE WHERE JOBNAME = 'BMSAB01' AND STATUS IN ('P', 'N') ";
            if (mssql.SQLselect(connYF, slqStr) == null) return false;
            else return true;
        }
        #endregion
        
        private string getTime()
        {
            string slqStr = @"SELECT dbo.f_getTime(1) ";
            return mssql.SQLselect(connYF, slqStr).Rows[0][0].ToString();
        }

        private void check(string danbie, string danhao)
        {
            if (checkMOCTC(danbie, danhao))
            {
                if (!checkLLXA(danbie, danhao))
                {
                    showList(danbie, danhao);
                }
                else
                {
                    MessageBox.Show("领料单：" + danbie + "-" + danhao + " 已扫描！");
                    TextBox_Danhao.Text = "";
                }
            }
            else
            {
                MessageBox.Show("领料单：" + danbie + "-" + danhao + " 不存在于ERP系统！");
                TextBox_Danhao.Text = "";
            }

        }

        private bool checkMOCTC(string danbie, string danhao)
        {
            string slqStr = "SELECT TC001, TC002 FROM MOCTC WHERE TC001 = '{0}' AND TC002 = '{1}'";
            return mssql.SQLexist(connYF, string.Format(slqStr, danbie, danhao));
        }

        private bool checkLLXA(string danbie, string danhao)
        {
            string slqStr = "SELECT LLXA001 FROM LL_LYXA WHERE LLXA001='{0}' AND LLXA002='{1}'";
            return mssql.SQLexist(connYF, string.Format(slqStr, danbie, danhao));
        }

        private bool checkLLXA007(string LLXA007)
        {
            string slqStr = "SELECT LLXA007 FROM LL_LYXA WHERE LLXA007='{0}'";
            return mssql.SQLexist(connYF, string.Format(slqStr, LLXA007));
        }

        private string getLLXA007()
        {
        	string time = getTime();
        	string long_time = "";
        	for(int index = 1; ;index ++)
        	{
        		long_time = "LL" + time + index.ToString().PadLeft(4, '0');
        		if (!checkLLXA007(long_time))
        		{
        			break;
        		}
        	}
        	return long_time;
        }

        private string GetWorkGroupNotExistStr()
        {
            if(checkedListBoxGroup.CheckedItems.Count > 0)
            {
                string GroupList = "";
                string rtnStr = "AND NOT EXISTS(SELECT 1 FROM MOCTE(NOLOCK) AS TE2 INNER JOIN CMSMW(NOLOCK) AS MW2 ON TE2.TE009 = MW2.MW001 WHERE TE2.TE001 = TC001 AND TE2.TE002 = TC002 AND CAST(MW2.MW003 AS VARCHAR(100)) IN ({0}))";
                for (int rowIdx = 0; rowIdx < checkedListBoxGroup.CheckedItems.Count; rowIdx++)
                {
                    if (rowIdx + 1 == checkedListBoxGroup.CheckedItems.Count)
                    {
                        GroupList += "'" + checkedListBoxGroup.CheckedItems[rowIdx].ToString() + "' ";
                    }
                    else
                    {
                        GroupList += "'" + checkedListBoxGroup.CheckedItems[rowIdx].ToString() + "', ";
                    }
                }
                return string.Format(rtnStr, GroupList);
            }
            else
            {
                return "";
            }
        }

        private void showList(string danbie, string danhao)
        {
            if (!checkJobExists())
            {
                xa007 = getLLXA007();
                string sqlStr = "";
                if (!checkBox1.Checked)
                {
                    sqlStr = "SELECT TE001 AS LLXA001,TE002 AS LLXA002,TE003 AS LLXA003,TE004 AS LLXA012,"
                        + " TE017 AS pinming,TE018 AS guige,CONVERT(FLOAT, TE005) AS LLXA017,TE008 AS LLXA013,MC002,TE009 AS LLXA011,"
                        + " MW002,TE010 AS LLXA015,TE011 AS LLXA009,TE012 AS LLXA010,TE014 AS LLXA018,"
                        + " '" + xa007 + "' AS LLXA007 "
                        + " FROM VMOCTEJ WHERE TE001 = '" + danbie + "' AND TE002 = '" + danhao + "'";
                }
                else
                {
                    sqlStr = @"SELECT TE001 AS LLXA001,TE002 AS LLXA002,TE003 AS LLXA003,TE004 AS LLXA012,
                                TE017 AS pinming,TE018 AS guige,CONVERT(FLOAT, TE005) AS LLXA017, TE008 AS LLXA013, MC002, TE009 AS LLXA011,
                                   MW002, TE010 AS LLXA015, TE011 AS LLXA009, TE012 AS LLXA010, TE014 AS LLXA018, "
                            + "'" + xa007 + @"' AS LLXA007
                                FROM MOCTE(NOLOCK) 
                                INNER JOIN MOCTC(NOLOCK) ON TE001 = TC001 AND TE002 = TC002 
                                INNER JOIN CMSMC(NOLOCK) ON TE008 = MC001 
                                INNER JOIN CMSMW(NOLOCK) ON TE009 = MW001 
                                INNER JOIN MOCTB(NOLOCK) ON TB001 = TE011 AND TB002 = TE012 AND TB003 = TE004 AND TB006 = TE009
                                INNER JOIN MOCTA(NOLOCK) ON TA001 = TB001 AND TA002 = TB002 
                                INNER JOIN WG_DB.dbo.SC_PLAN(NOLOCK) ON K_ID = MOCTA.UDF02 
                                WHERE 1 = 1
                                " + GetWorkGroupNotExistStr() + @"
                                AND NOT EXISTS(SELECT 1 FROM LL_LYXA(NOLOCK) AS A WHERE A.LLXA001 = TC001 AND A.LLXA002 = TC002)
                                AND NOT EXISTS(SELECT 1 FROM MOCTE(NOLOCK) AS TE2 INNER JOIN CMSMW(NOLOCK) AS MW2 ON TE2.TE009 = MW2.MW001 WHERE TE2.TE001 = TC001 AND TE2.TE002 = TC002 AND CAST(MW2.MW003 AS VARCHAR(100)) IN ('气压棒组'))
                                AND SC003 = '" + dateTimePicker1.Value.ToString("yyyyMMdd") + @"'
                                AND LEFT(TC001, 2) = '54'
                                AND TA013 = 'Y'
                                AND TC009 = 'N'
                                AND TE019 = 'N' ";
                    if (checkBox2.Checked)
                    {
                        sqlStr += @" AND (RTRIM(TC019) = '' OR TC019 IS NULL) ";
                    }
                    sqlStr +=" ORDER BY TE001, TE002, TE003";
                }
                dttmp = mssql.SQLselect(connYF, sqlStr);
                if (dttmp != null)
                {
                    DataGridView_List.DataSource = dttmp;
                    Button_Upload.Enabled = true;
                    label1.Text = "领料单共" + dttmp.Rows.Count.ToString() + "条！";
                    Button_Upload.Select();
                }
                else
                {
                    Button_Upload.Enabled = false;
                    if (checkBox1.Checked)
                    {
                        Msg.Show("无数据！");
                    }
                    else
                    {
                        Msg.Show("领料单：" + danbie + "-" + danhao + " 已审核！", "错误");
                    }
                }
            }
            else
            {
                MessageBox.Show("后台存在未结束的‘批量生成领料单’程序，为避免领料单数据丢失，在后台程序结束前，不允许运行扫描领料单！", "错误");
            }
        }

        private void insertsql()
        {
            string slqStr = "";
            string danhao = "";
            int index;
            if (dttmp != null)
            {
                danhao = dttmp.Rows[0][0].ToString().Trim() + "-" + dttmp.Rows[0][1].ToString().Trim();
                string create_date = getTime();
                string xa001 = "";
                string xa002 = "";
                string xa003 = "";
                string xa009 = "";
                string xa010 = "";
                string xa011 = "";
                string xa012 = "";
                string xa013 = "";
                string xa015 = "";
                string xa017 = "";
                string xa018 = "";
                if (!checkLLXA007(xa007))
                {
                    xa007 = getLLXA007();
                }
                for (index = 0; index < dttmp.Rows.Count; index++)
                {
                    xa001 = dttmp.Rows[index][0].ToString().Trim();
                    xa002 = dttmp.Rows[index][1].ToString().Trim();
                    xa003 = dttmp.Rows[index][2].ToString().Trim();
                    xa009 = dttmp.Rows[index][12].ToString().Trim();
                    xa010 = dttmp.Rows[index][13].ToString().Trim();
                    xa011 = dttmp.Rows[index][9].ToString().Trim();
                    xa012 = dttmp.Rows[index][3].ToString().Trim();
                    xa013 = dttmp.Rows[index][7].ToString().Trim();
                    xa015 = dttmp.Rows[index][11].ToString().Trim();
                    xa017 = dttmp.Rows[index][6].ToString().Trim();
                    xa018 = dttmp.Rows[index][14].ToString().Trim();

                    slqStr += "INSERT INTO LL_LYXA (COMPANY, CREATOR, USR_GROUP, CREATE_DATE, LLXA001, LLXA002, LLXA003, LLXA007, LLXA009, LLXA010, LLXA011, LLXA012, LLXA013, LLXA015, LLXA017, LLXA018) VALUES ( 'COMFORT', "
                            +  "'" + FormLogin.infObj.userId + "', '" + FormLogin.infObj.userGroup + "', '" + create_date + "', '" + xa001 + "', '" + xa002 + "', '" + xa003 + "', '" + xa007 + "', '" + xa009 + "','" + xa010 + "', '" + xa011 + "', '" + xa012 + "','" 
                            + xa013 + "', '" + xa015 + "', '" + xa017 + "', '" + xa018 + "')   ";
                    
                }
                mssql.SQLexcute(connYF, slqStr);

                DataGridView_List.DataSource = null;
                Button_Upload.Enabled = false;
                TextBox_Danhao.Text = "";
                TextBox_Danhao.Select();
                Lable_Danhao.Text = "";
                label3.Text = "已上传" + index.ToString() + "条记录！";
                
                dttmp = null;

                MessageBox.Show("已上传" + index.ToString() + "条记录！", "");
            }
            else
            {
                Button_Upload.Enabled = false;
                MessageBox.Show("", "错误");
            }
        }
    }
}
