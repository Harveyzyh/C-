using HarveyZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 码垛线生成出货清单
{
    public partial class 码垛线生成出货清单 : Form
    {
        #region 局域静态变量
        private Mssql mssql = new Mssql();
        private static string connRobot = null;
        private static string connWg = Global_Const.strConnection_WGDB;
        private static string connComfort = Global_Const.strConnection_COMFORT;
        #endregion

        public 码垛线生成出货清单()
        {
            InitializeComponent();
            GetRobotDbConn();
        }

        private void GetRobotDbConn()
        {
            string sqlstr = @"SELECT ServerURL FROM WG_CONFIG WHERE ConfigName = 'MdSysDb' AND Valid = 'Y' ";
            string dbName = mssql.SQLselect(connWg, sqlstr).Rows[0][0].ToString();
            if (dbName == "ROBOT_TEST") connRobot = Global_Const.ConnStrObject.RobotTest;
            if (dbName == "ROBOT") connRobot = Global_Const.ConnStrObject.Robot;
        }
    }
}
