﻿public class Global_Var //全局变量配置
{
    #region 程序版本信息
    public static string Software_Version = "";
    public static string File_Version  = "";
    #endregion
    
    public static string Login_UID = "";
    public static string Login_Role = "";
    public static string Login_Name = "";
    public static string Login_Dpt = "";


    //public static string XL_List = "";
    //public static bool XL_ChangeFlag = false;
}

public class Global_Const //静态量配置
{
    #region 数据库连接字
    public const string strConnection_CONFIG =      "Server=192.168.0.198;initial catalog=CONFIG;user id=sa;password=COMfort123456;Connect Timeout=5";
    public const string strConnection_ROBOT =       "Server=192.168.0.198;initial catalog=ROBOT;user id=sa;password=COMfort123456;Connect Timeout=5"; 
    public const string strConnection_ROBOT_TEST =  "Server=192.168.0.198;initial catalog=ROBOT_TEST;user id=sa;password=COMfort123456;Connect Timeout=5";

    public const string strConnection_COMFORT_TEST = "Server=192.168.0.198;initial catalog=COMFORT;user id=sa;password=COMfort123456;Connect Timeout=5";
    public const string strConnection_WG_DB =  "Server=192.168.0.198;initial catalog=WG_DB;user id=sa;password=COMfort123456;Connect Timeout=5";



    public const string strConnection_COMFORT =     "Server=192.168.0.99;initial catalog=COMFORT;user id=sa;password=comfortgroup2016{;Connect Timeout=5";
    #endregion

    #region 数据库年月日获取字
    public const string sqldatestrlong = "(DATENAME(YYYY,GETDATE())+DATENAME(MM,GETDATE())+DATENAME(DD,GETDATE())+DATENAME(HH,GETDATE())+DATENAME(mm,GETDATE())+DATENAME(SS,GETDATE()))"; //获取年月日时分秒
    public const string sqldatestrshort = "(DATENAME(YYYY,GETDATE())+DATENAME(MM,GETDATE())+DATENAME(DD,GETDATE()))"; //获取年月日
    #endregion
}