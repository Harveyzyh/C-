using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.仓储中心
{
    public partial class 生成领料单 : Form
    {
        public static Mssql mssql = new Mssql();
        public static string strConnection = Global_Const.strConnection_COMFORT;
        public static string Title = "";
        public static string Mode = "";

        private Dictionary<string, string> tradeModeDict = new Dictionary<string, string>();
        private Generate generate = new Generate(_mssql: mssql, _strConnection: strConnection);
        

        public 生成领料单()
        {
            InitializeComponent();
            textBoxDbSelect.Text = "5401";
            textBoxGdDptSelect.Text = "080A";

            tradeModeDict.Add("1.内销", "1");
            tradeModeDict.Add("2.一般贸易", "2");
            tradeModeDict.Add("3.合同", "3");

        }

        private void linkLabelGdSelect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string sqlstr = "SELECT RTRIM(TA001)+'-'+RTRIM(TA002) FROM MOCTA WHERE TA026 = '2215' AND TA027 = 'TEST01' AND TA064 = '{0}' AND MOCTA.UDF04 = '{1}'";
            string tradeMode = "";
            tradeModeDict.TryGetValue(comboBoxGdTradeMode.Text, out tradeMode);
            MessageBox.Show(string.Format(sqlstr, textBoxGdDptSelect.Text, tradeMode));
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sqlstr, textBoxGdDptSelect.Text, tradeMode));
            if (dt != null)
            {
                for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                {
                    labelGdSelect.Text += dt.Rows[rowIdx][0].ToString() + ",";
                }
            }
            else
            {
                MessageBox.Show("无数据");
            }
        }
    }

    class Generate
    {
        private static Mssql mssql = null;
        private static string strConnection = null;
        public Generate(Mssql _mssql, string _strConnection)
        {
            mssql = _mssql;
            strConnection = _strConnection;
        }

        private string GetDh(string db)
        {
            return "";
        }

        private int GetSl(string gd)
        {
            string sqlstr = "SELECT CAST(TA015) AS FLOAT FROM MOCTA WHERE RTRIM(TA001)+'-'+RTRIM(TA002) = '{0}' ";
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sqlstr, gd));
            if(dt != null)
            {
                return int.Parse(dt.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        private void MoctcIns()
        {
            string sqlstr = "";
        }

        private void MoctdIns(string db, string dh, string gdStr)
        {
            string sqlstr = "INSERT INTO MOCTD(COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, TD001, TD002, TD003, TD004, TD005, TD006, TD007, TD008, TD009, "
                          + "TD010, TD011, TD012, TD013, TD014, TD015, TD016, TD017, TD018, TD019, TD020, TD021, TD022, TD023, TD024, TD025, TDC01) "
                          + "VALUES ('COMFORT   ', 'Robot', '', dbo.f_getTime(1), 1, '{0}', '{1}', '{2}', '{3}', '3', {4}, '{5}', '1', "
                          + "'', '', '', '', 'N', '', '*', '', '2', '', 'N', ' ', '', '', .000000, .000000, .000000, '2');";
            foreach(string gd in gdStr.Split(','))
            {
                int sl = GetSl(gd);
                string gdb = gd.Split('-')[0];
                string gdh = gd.Split('-')[1];

                mssql.SQLexcute(strConnection, string.Format(sqlstr, db, dh, gdb, gdh, sl));
            }
        }

        private void MocteIns(string db, string dh)
        {
            string sqlstr = "";
        }

        private void MocteUdt(string db, string dh)
        {
            string sqlstr = "";
        }

        private void MoctcUdt(string db, string dh)
        {
            string sqlstr = "";
        }
    }
}
