public class Global_Const //静态量配置
{
    #region 数据库连接字
    public const string strConnection_CONFIG =  "Server=192.168.0.198;initial catalog=CONFIG;user id=sa;password=COMfort123456;Connect Timeout=5";

    public const string strConnection_ROBOT = "Server=192.168.0.198;initial catalog=ROBOT;user id=sa;password=COMfort123456;Connect Timeout=5";

    public const string strConnection_ROBOT_TEST = "Server=192.168.0.198;initial catalog=ROBOT_TEST;user id=sa;password=COMfort123456;Connect Timeout=5";

    public const string strConnection_WGDB = "Server=192.168.0.198;initial catalog=WG_DB;user id=sa;password=COMfort123456;Connect Timeout=5";
    
    public const string strConnection_COMFORT = "Server=192.168.0.99;initial catalog=COMFORT;user id=sa;password=comfortgroup2016{;Connect Timeout=5";


    public const string strConnection_Y_WGDB = "Server=40.73.246.171;initial catalog=WG_DB;user id=sa;password=DGlsdnkj168;Connect Timeout=5";

    public const string strConnection_Y_ROBOT = "Server=40.73.246.171;initial catalog=ROBOT_TEST;user id=sa;password=DGlsdnkj168;Connect Timeout=5";

    public const string strConnection_Y_COMFORT = "Server=40.73.246.171;initial catalog=WG_DB;user id=sa;password=DGlsdnkj168;Connect Timeout=5";

    public const string strConnection_JY = "Server=192.168.1.188;initial catalog=lserp-JY;user id=sa;password=lsdnkj;Connect Timeout=5";
    #endregion


    public static class ConnStrObject
    {
        private static string strConnection_CONFIG = "Server=192.168.0.198;initial catalog=CONFIG;user id=sa;password=COMfort123456;Connect Timeout=5";

        private static string strConnection_ROBOT = "Server=192.168.0.198;initial catalog=ROBOT;user id=sa;password=COMfort123456;Connect Timeout=5";

        private static string strConnection_ROBOT_TEST = "Server=192.168.0.198;initial catalog=ROBOT_TEST;user id=sa;password=COMfort123456;Connect Timeout=5";

        private static string strConnection_WGDB = "Server=192.168.0.198;initial catalog=WG_DB;user id=sa;password=COMfort123456;Connect Timeout=5";

        private static string strConnection_COMFORT = "Server=192.168.0.99;initial catalog=COMFORT;user id=sa;password=comfortgroup2016{;Connect Timeout=5";

        private static string strConnection_Y_WGDB = "Server=40.73.246.171;initial catalog=WG_DB;user id=sa;password=DGlsdnkj168;Connect Timeout=5";

        private static string strConnection_Y_ROBOT_TEST = "Server=40.73.246.171;initial catalog=ROBOT_TEST;user id=sa;password=DGlsdnkj168;Connect Timeout=5";

        private static string strConnection_Y_ROBOT = "Server=40.73.246.171;initial catalog=ROBOT;user id=sa;password=DGlsdnkj168;Connect Timeout=5";

        private static string strConnection_Y_COMFORT = "Server=40.73.246.171;initial catalog=WG_DB;user id=sa;password=DGlsdnkj168;Connect Timeout=5";

        private static string strConnection_JY = "Server=192.168.1.188;initial catalog=lserp-JY;user id=sa;password=lsdnkj;Connect Timeout=5";

        public static string Config { get { return strConnection_CONFIG; } }
        public static string Robot { get { return strConnection_ROBOT; } }
        public static string RobotTest { get { return strConnection_ROBOT_TEST; } }
        public static string WgDb { get { return strConnection_WGDB; } }
        public static string Comfort { get { return strConnection_WGDB; } }
        public static string YWgDb { get { return strConnection_Y_WGDB; } }
        public static string YRobotTest { get { return strConnection_Y_ROBOT_TEST; } }
        public static string YRobot { get { return strConnection_Y_ROBOT; } }
        public static string YComfort { get { return strConnection_Y_COMFORT; } }
        public static string Jy { get { return strConnection_JY; } }
    }

    public static string GetConnStr(string dbName)
    {
        return null;
    }
}