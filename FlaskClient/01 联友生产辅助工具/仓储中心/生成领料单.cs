using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System;
using HarveyZ;
using System.Drawing;

namespace 联友生产辅助工具.仓储中心
{
    public partial class 生成领料单 : Form
    {   
        private static LldGenerate generate = new LldGenerate();

        private static InfoObject infObj = generate.infObj;


        public 生成领料单()
        {
            InitializeComponent();
            FormMain_Resized_Work();

            Init();
        }
        
        private void Init() //初始化所有信息
        {
            foreach(string tmp in generate.dbDict.Keys)
            {
                comboBoxDb.Items.Add(tmp);
            }

            foreach (string tmp in generate.dptDict.Keys)
            {
                comboBoxDpt.Items.Add(tmp);
            }

            foreach (string tmp in generate.centerDict.Keys)
            {
                comboBoxCenter.Items.Add(tmp);
            }

            foreach (string tmp in generate.tradeModeDict.Keys)
            {
                comboBoxTradeMode.Items.Add(tmp);
            }

            // 组别
            foreach (string tmp in generate.GetGroupList())
            {
                checkedListBoxGroup.Items.Add(tmp);
            }
            
            comboBoxCenter.SelectedIndex = 0;
            comboBoxDb.SelectedIndex = 0;
            comboBoxDpt.SelectedIndex = 0;
            comboBoxTradeMode.SelectedIndex = 0;

            DataGridView_List.DataSource = infObj.gdDt;
            DgvOpt.SetRowColor(DataGridView_List);

            uiShow();
        }

        #region 窗口大小变化设置
        private void FormMain_Resized(object sender, EventArgs e)
        {
            FormMain_Resized_Work();
        }

        private void FormMain_Resized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
            panel_Title.Location = new Point(1, 1);
            panel_Title.Size = new Size(FormWidth - 2, panel_Title.Height);

            DataGridView_List.Location = new Point(panel_Title.Location.X, panel_Title.Location.Y + panel_Title.Height + 2);
            DataGridView_List.Size = new Size(panel_Title.Width, FormHeight - panel_Title.Height - 3);
        }
        #endregion

        private void uiShow() //根据界面上的UI信息，对其他UI作配置
        {
            labelRowCount.ForeColor = Color.Black;
            if (infObj.gdDt.Rows.Count > 0)
            {
                comboBoxDb.Enabled = false;
                comboBoxDpt.Enabled = false;
                comboBoxCenter.Enabled = false;
                comboBoxTradeMode.Enabled = false;
                btnCheck.Enabled = true;
                btnClean.Enabled = true;
                btnSave.Enabled = false;
                if (infObj.wlRowCount > 0)
                {
                    if (infObj.wlRowCount < 9850)
                    {
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        labelRowCount.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                comboBoxDb.Enabled = true;
                comboBoxDpt.Enabled = true;
                comboBoxCenter.Enabled = true;
                comboBoxTradeMode.Enabled = true;
                btnCheck.Enabled = false;
                btnClean.Enabled = false;
                btnSave.Enabled = false;
            }

            labelGdCount.Text = "工单数：" + infObj.gdDt.Rows.Count.ToString();
            labelGdScCount.Text = "生产数量：" + infObj.gdScCount.ToString();
            labelRowCount.Text = "物料总行数：" + infObj.wlRowCount.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            生成领料单_获取单据信息 frm = new 生成领料单_获取单据信息(infObj, generate);
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                frm.Dispose();
            }
            infObj.wlRowCount = 0;
            generate.GetGdScSl(infObj.gdDt);
            DgvOpt.SelectLastRow(DataGridView_List);
            uiShow();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            infObj.GroupList = "";
            for (int rowIdx = 0; rowIdx < checkedListBoxGroup.CheckedItems.Count; rowIdx++)
            {
                if (rowIdx + 1 == checkedListBoxGroup.CheckedItems.Count)
                {
                    infObj.GroupList += "'" + checkedListBoxGroup.CheckedItems[rowIdx].ToString() + "' ";
                }
                else
                {
                    infObj.GroupList += "'" + checkedListBoxGroup.CheckedItems[rowIdx].ToString() + "', ";
                }
            }

            infObj.wlRowCount = int.Parse(generate.GdRowCount(infObj.gdDt));
            uiShow();
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            infObj.gdDt.Clear();
            infObj.wlRowCount = 0;
            infObj.gdScCount = 0;
            uiShow();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBoxOrder.Text == "") MessageBox.Show("请输入生产单号", "错误");
            else
            {
                infObj.order = textBoxOrder.Text;
                infObj.cpName = textBoxCpName.Text;
                infObj.orderNum = textBoxOrderNum.Text;
                //infObj.wlRowCount = int.Parse(generate.GdRowCount(infObj.gdDt));

                infObj.GroupList = "";
                for (int rowIdx = 0; rowIdx < checkedListBoxGroup.CheckedItems.Count; rowIdx++)
                {
                    if (rowIdx + 1 == checkedListBoxGroup.CheckedItems.Count)
                    {
                        infObj.GroupList += "'" + checkedListBoxGroup.CheckedItems[rowIdx].ToString() + "' ";
                    }
                    else
                    {
                        infObj.GroupList += "'" + checkedListBoxGroup.CheckedItems[rowIdx].ToString() + "', ";
                    }
                }

                if (infObj.wlRowCount > 9900)
                {
                    MessageBox.Show("物料行数过多，请删减一部分工单", "错误");
                }
                else
                {
                    // 生成领料单的主函数
                    generate.work();
                    if (infObj.gengerateSucc)
                    {
                        MessageBox.Show(string.Format("领料单 {0}-{1}", infObj.db, infObj.dh), "领料单生成成功", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("领料单 {0}-{1}", infObj.db, infObj.dh), "领料单生成失败", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            infObj.GroupList = "";
            for (int rowIdx = 0; rowIdx < checkedListBoxGroup.CheckedItems.Count; rowIdx++)
            {
                if(rowIdx + 1 == checkedListBoxGroup.CheckedItems.Count)
                {
                    infObj.GroupList += "'" + checkedListBoxGroup.CheckedItems[rowIdx].ToString() + "' ";
                }
                else
                {
                    infObj.GroupList += "'" + checkedListBoxGroup.CheckedItems[rowIdx].ToString() + "', ";
                }
            }
        }

        private void comboBoxDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            infObj.db = generate.GetDbStr(obj.Text);
        }

        private void comboBoxCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            infObj.center = generate.GetCenterStr(obj.Text);
        }

        private void comboBoxDpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            infObj.dpt = generate.GetDptStr(obj.Text);
        }

        private void comboBoxTradeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            infObj.tradeMode = generate.GetTradeModeStr(obj.Text);
        }

        private void DataGridView_List_DoubleClick(object sender, EventArgs e)
        {
            infObj.gdDt.Rows.RemoveAt(DataGridView_List.CurrentRow.Index);
            infObj.wlRowCount = 0;
            uiShow();
        }

        private void checkedListBoxGroup_SelectedValueChanged(object sender, EventArgs e)
        {
            infObj.wlRowCount = 0;
            uiShow();
        }
    }

    public class LldGenerate
    {
        public InfoObject infObj = new InfoObject();

        public Dictionary<string, string> tradeModeDict = new Dictionary<string, string>();
        public Dictionary<string, string> dbDict = new Dictionary<string, string>();
        public Dictionary<string, string> dptDict = new Dictionary<string, string>();
        public Dictionary<string, string> centerDict = new Dictionary<string, string>();

        public LldGenerate()
        {
            infObj.sql = new Mssql();
            infObj.connStr = Global_Const.strConnection_COMFORT;

            infObj.userId = 主界面.infObj.userId;
            infObj.userName = 主界面.infObj.userName;
            infObj.userDpt = 主界面.infObj.userDpt;

            InitDict();
            InitGdDt();
        }

        private void InitGdDt()
        {
            infObj.gdDt = infObj.sql.SQLselect(infObj.connStr, "SELECT TOP 1 *  FROM V_GetWscGd ");
            infObj.gdDt.Rows.RemoveAt(0);
        }

        public void InitDict()
        {
            dbDict.Add("5401-领料单", "5401");
            dbDict.Add("5402-测试领料单", "5402");
            dbDict.Add("5406-国内领料单", "5406");

            dptDict.Add("1.生产一部", "080A");
            dptDict.Add("2.生产二部", "080B");
            dptDict.Add("3.生产三部", "080D");

            centerDict.Add("1.一厂车间", "1");
            centerDict.Add("6.二厂车间", "6");
            centerDict.Add("7.三厂车间", "7");

            tradeModeDict.Add("1.内销", "1");
            tradeModeDict.Add("2.一般贸易", "2");
            tradeModeDict.Add("3.合同", "3");
        }

        public List<string> GetGroupList()
        {
            string sqlstr = "SELECT DISTINCT CAST(MW003 AS VARCHAR(100)) MW003 FROM CMSMW WHERE MW003 IS NOT NULL AND CAST(MW003 AS VARCHAR(100)) != '' ";
            DataTable dt = infObj.sql.SQLselect(infObj.connStr, sqlstr);
            if (dt != null)
            {
                List<string> groupList = new List<string>();
                for(int rowIdx =0; rowIdx < dt.Rows.Count; rowIdx++)
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

        public string GetDbStr(string db)
        {
            string dbStr = "";
            dbDict.TryGetValue(db, out dbStr);
            return dbStr;
        }

        public string GetDptStr(string dpt)
        {
            string dptStr = "";
            dptDict.TryGetValue(dpt, out dptStr);
            return dptStr;
        }

        public string GetCenterStr(string center)
        {
            string centerStr = "";
            centerDict.TryGetValue(center, out centerStr);
            return centerStr;
        }

        public string GetTradeModeStr(string tradeMode)
        {
            string tradeModeStr = "";
            tradeModeDict.TryGetValue(tradeMode, out tradeModeStr);
            return tradeModeStr;
        }

        public DataTable GetGdDt() //获取根据
        {
            string sqlstr = "";
            return null;
        }
        
        public void GetDh() //获取单号
        {
            if (infObj.db != null)
            {
                string sqlstr = "EXEC P_GETDH @MQ001='{0}' ";
                DataTable dt = infObj.sql.SQLselect(infObj.connStr, string.Format(sqlstr, infObj.db));
                if (dt != null)
                {
                    infObj.dh = dt.Rows[0][0].ToString();
                }
            }
            
        }

        private int GetSl(string gd) // 根据工单号，获取工单的预计产量
        {
            string sqlstr = "SELECT ISNULL(CAST(TA015 AS FLOAT), 0) FROM MOCTA WHERE RTRIM(TA001)+'-'+RTRIM(TA002) = '{0}' ";
            DataTable dt = infObj.sql.SQLselect(infObj.connStr, string.Format(sqlstr, gd));
            if(dt != null)
            {
                return int.Parse(dt.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public void GetGdScSl(DataTable dt)
        {
            infObj.gdScCount = 0;
            int colIdx = 0;
            for(; colIdx < dt.Columns.Count; colIdx ++)
            {
                if(dt.Columns[colIdx].ColumnName == "预计产量")
                {
                    break;
                }
            }
            for(int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
            {
                infObj.gdScCount += int.Parse(dt.Rows[rowIdx][colIdx].ToString());
            }
        }

        public string GdRowCount(DataTable dt)
        {
            string sqlstr = "SELECT COUNT(1) FROM MOCTB(NOLOCK) "
                          + "INNER JOIN CMSMW ON MW001 = TB006 "
                          + "WHERE RTRIM(TB001) + '-'+ RTRIM(TB002) IN ({0}) ";
            if(infObj.GroupList != "")
            {
                sqlstr += string.Format(" AND CAST(MW003 AS VARCHAR(100)) IN ({0}) ", infObj.GroupList);
            }
            string gdStr = "";
            if (dt.Rows.Count != 0)
            {
                for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                {
                    gdStr += "'" + dt.Rows[rowIdx][0].ToString() + "-" + dt.Rows[rowIdx][1].ToString() + "'";
                    if (rowIdx + 1 != dt.Rows.Count) gdStr += ", ";
                }
                return infObj.sql.SQLselect(infObj.connStr, string.Format(sqlstr, gdStr.ToString())).Rows[0][0].ToString();
            }
            else return 0.ToString();
        }

        public void work()
        {
            MoctcIns();
            MoctdIns();
            MoctdUdt();
            MocteIns();
            MoctcUdt();
            MocteUdt();
            infObj.gengerateSucc = true;
        }

        private void PreWork()
        {

        }


        private void MoctcIns()
        {
            string sqlstr = "INSERT INTO MOCTC(COMPANY, CREATOR, CREATE_DATE, FLAG, TC001, TC002, TC003, TC004, TC005, TC006, TC007, TC008, TC009, TC010, "
                          + "TC011, TC012, TC013, TC014, TC015, TC016, TC017, TC018, TC019, TC020, TC021, TC022, TC023, TC024, TC025, TC026, TC027, TC028, "
                          + "TC029, TC030, TC031, UDF01, UDF04, UDF02, UDF03, UDF09) "
                          + "VALUES('COMFORT', '{0}', dbo.f_getTime(1), 1, '{1}', '{2}', LEFT(dbo.f_getTime(1), 8), '01', '{3}', '', '', '54', 'N', 0, 'N', '1', "
                          + "'Y', LEFT(dbo.f_getTime(1), 8), '', 'N', 0, '0', '', '', '{4}', '', '', NULL, 0, 0, 0, '', 0, 0, '', '{5}', '{6}', '{7}', '{8}', '{9}')";
            GetDh();
            infObj.sql.SQLexcute(infObj.connStr, string.Format(sqlstr, FormLogin.infObj.userId, infObj.db, infObj.dh, infObj.center, infObj.dpt, infObj.order, infObj.tradeMode, infObj.orderNum, infObj.cpName, infObj.GroupList.Replace("\'", ""))); 
        }
        
        private void MoctdIns()
        {
            string sqlstr = "INSERT INTO MOCTD(COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, TD001, TD002, TD003, TD004, TD005, TD006, TD007, TD008, TD009, "
                          + "TD010, TD011, TD012, TD013, TD014, TD015, TD016, TD017, TD018, TD019, TD020, TD021, TD022, TD023, TD024, TD025, TDC01) "
                          + "VALUES ('COMFORT   ', '{6}', '', dbo.f_getTime(1), 1, '{0}', '{1}', '{2}', '{3}', '3', {4}, '{5}', '1', "
                          + "'', '', '', '', 'N', '', '*', '', '2', '', 'N', ' ', '', '', .000000, .000000, .000000, '2');";
            for(int rowIdx = 0; rowIdx < infObj.gdDt.Rows.Count; rowIdx ++ )
            {
                DataRow row = infObj.gdDt.Rows[rowIdx];

                string gdb = row[0].ToString();
                string gdh = row[1].ToString();

                int sl = GetSl(gdb + "-" + gdh);

                infObj.sql.SQLexcute(infObj.connStr, string.Format(sqlstr, infObj.db, infObj.dh, gdb, gdh, sl, "P013", FormLogin.infObj.userId));
            }
        }

        private void MoctdUdt()
        {
            string sqlstr = "UPDATE MOCTD SET MOCTD.USR_GROUP = MF004 FROM MOCTD INNER JOIN ADMMF ON MF001=MOCTD.CREATOR WHERE TD001='{0}' AND TD002='{1}' ";
            infObj.sql.SQLexcute(infObj.connStr, string.Format(sqlstr, infObj.db, infObj.dh));
        }
        
        private void MocteIns()
        {
            string sqlstr = "INSERT INTO MOCTE(COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, TE001, TE002, TE003, TE004, TE005, TE006, TE007, "
                          + "TE008, TE009, TE010, TE011, TE012, TE013, TE014, TE015, TE016, TE017, TE018, TE019, TE020, TE021, TE022, TE023, TE024, "
                          + "TE025, TE026, TE027, TE028, TE029, TE030, TE031, TE032, TE033, TEC01, UDF02, UDF03, UDF04) "
                          + " "
                          + "SELECT MOCTC.COMPANY, MOCTC.CREATOR, MOCTD.USR_GROUP, MOCTC.CREATE_DATE, MOCTC.FLAG, MOCTC.TC001, MOCTC.TC002, "
                          + "RIGHT('0000' + CAST(ROW_NUMBER() over(ORDER BY TB001, TB002, TB003) AS VARCHAR(4)), 4) TE003, "
                          + "TB003, TB004 - TB005 - ISNULL(TE005, 0) TE005, MB004, '' TE007, 'P013' TE008, TB006 TE009, MB032 TE010, TB001, TB002, "
                          + "'' TE013, '' TE014, '' TE015, '1' TE016, MB002 TE017, MB003 TE018, 'N' TE019, '' TE020, 0 TE021, '' TE022, "
                          + "ISNULL(MA002,'') TE023, 0 TE024, '##########' TE025, MB004 TE026, TB004-TB005 - ISNULL(TE005, 0) TE027, '' TE028, '' TE029, '' TE030, 0 TE031, "
                          + "0 TE032, 0 TE033, '2' TEC01, ISNULL(INVMB.UDF02, '') UDF02, ISNULL(MW003, '') UDF03, ISNULL(MOCTA.UDF12, '') UDF04 "
                          + "FROM MOCTC INNER JOIN MOCTD ON TC001 = TD001 AND TC002 = TD002 INNER JOIN MOCTB ON TD003 = TB001 AND TD004 = TB002 "
                          + "INNER JOIN INVMB ON MB001 = TB003 INNER JOIN CMSMW ON MW001 = TB006 LEFT JOIN INVME ON ME001 = TB003 AND ME002 = MB032 "
                          + "INNER JOIN MOCTA ON TD003 = TA001 AND TD004 = TA002 LEFT JOIN PURMA ON MA001 = MB032 "
                          + "LEFT JOIN ("
                          + "SELECT TE2.TE004, TE2.TE009, TE2.TE011, TE2.TE012, SUM(TE2.TE005) AS TE005 FROM MOCTC AS TC2 "
                          + "INNER JOIN MOCTE AS TE2 ON TC2.TC001 = TE2.TE001 AND TC2.TC002 = TE2.TE002 "
                          + "WHERE TC2.TC008 = '54' AND TC2.TC009 = 'N' "
                          + "GROUP BY TE004, TE009, TE011, TE012 ) AS MOCTE ON TE011 = TD003 AND TE012 = TD004 AND TE004 = TB003 AND TE009 = TB006 " 
                          + "WHERE TB004-TB005 - ISNULL(TE005, 0) > 0 AND TB011 NOT IN ('4') AND TC001 = '{0}' AND TC002 = '{1}' {2} ORDER BY TB001, TB002, TB003 ";
            string sqlstr2 = "";
            if (infObj.GroupList != "")
            {
                sqlstr2 = string.Format("AND CAST(MW003 AS VARCHAR(100)) IN ({0}) ", infObj.GroupList);
                
            }
            infObj.sql.SQLexcute(infObj.connStr, string.Format(sqlstr, infObj.db, infObj.dh, sqlstr2));
        }

        private void MocteUdt() //其他额外情况的更新
        {
            //订单的指定气压棒供应商更新
            string sqlstr1 = @"UPDATE MOCTE SET TE010 = COPTD.UDF09
                                FROM MOCTE
                                INNER JOIN MOCTA ON TE011 = TA001 AND TE012 = TA002
                                INNER JOIN COPTD ON TD001 = TA026 AND TD002 = TA027 AND TD003 = TA028
                                INNER JOIN PURMA ON MA001 = COPTD.UDF09
                                WHERE 1 = 1
                                AND TE004 IN('3020501004', '3020501014', '3020502014', '3020502023'
                                    , '3020501005', '3020501012', '3020501016', '3020502015', '3020502024', '3020502026')
                                AND TE001 = '{0}' AND TE002 = '{1}'";
            infObj.sql.SQLexcute(infObj.connStr, string.Format(sqlstr1, infObj.db, infObj.dh));

            //内销非电商，底盘供应商更新
        }

        private void MoctcUdt()
        {
            string sqlstr = "UPDATE MOCTC SET TC029 = ISNULL(STE005, 0), MOCTC.USR_GROUP= MOCTE.USR_GROUP "
                          + "FROM MOCTC "
                          + "LEFT JOIN (SELECT TE001, TE002, USR_GROUP, SUM(TE005) STE005 FROM MOCTE GROUP BY TE001, TE002, USR_GROUP) AS MOCTE ON TE001=TC001 AND TE002=TC002 "
                          + "WHERE TC001='{0}' AND TC002='{1}'";
            infObj.sql.SQLexcute(infObj.connStr, string.Format(sqlstr, infObj.db, infObj.dh));
        }
    }

    public class InfoObject: InfoObjectBase
    {
        private string _db = null;
        private string _dh = null;
        private string _center = null;
        private string _dpt = null;
        private string _tradeMode = null;
        private string _order = null;
        private string _orderNum = null;
        private string _cpName = null;
        private int _wlRowCount = 0;
        private int _gdScCount = 0;

        private int _rowCount = 0;
        private bool _generateSucc = false;

        private DataTable _gdDt = null;

        private string _groupList = "";

        public string getGdSqlStr { get { return "SELECT CAST(0 AS BIT) 选择, V_GetWscGd.* From V_GetWscGd "
                    + " WHERE 部门编号='{1}' AND 贸易方式='{2}' "
                    + " AND (RTRIM(工单单别) + '-' + RTRIM(工单单号) LIKE '%{0}%' OR 订单号 LIKE '%{0}%' " 
                    + " OR 椅型 LIKE '%{0}%' OR 规格 LIKE '%{0}%' OR 成品品号 LIKE '%{0}%' OR 客户编号 LIKE '%{0}%') "
                    + " ORDER BY RTRIM(工单单别) + '-' + RTRIM(工单单号)"; } }

        public string db { get { return _db; } set { _db = value; } }
        public string dh { get { return _dh; } set { _dh = value; } }
        public string center { get { return _center; } set { _center = value; } }
        public string dpt { get { return _dpt; } set { _dpt = value; } }
        public string tradeMode { get { return _tradeMode; } set { _tradeMode = value; } }
        public string order { get { return _order; } set { _order = value; } }
        public string orderNum { get { return _orderNum; } set { _orderNum = value; } }
        public string cpName { get { return _cpName; } set { _cpName = value; } }
        public int gdStr { get { return _wlRowCount; } set { _wlRowCount = value; } }
        public int wlRowCount { get { return _rowCount; } set { _rowCount = value; } }
        public int gdScCount { get { return _gdScCount; }set { _gdScCount = value; } }
        public bool gengerateSucc { get { return _generateSucc; } set { _generateSucc = value; } }
        public DataTable gdDt { get { return _gdDt; } set { _gdDt = value; } }
        public string GroupList { get { return _groupList; } set { _groupList = value; } }
    }
}
