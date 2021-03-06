﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HarveyZ
{
    class UserLogin
    {
        private UserObjectBase userObj = null;
        private UserObjectReturn userObjRtn = null;
        private Mssql mssql = new Mssql();
        private string connWGDB = Global_Const.strConnection_WGDB;
        private string connCOMFORT = Global_Const.strConnection_COMFORT;

        #region 添加用户
        public bool AddUser(string U_ID, string U_PWD, string DPT, string TYPE)
        {
            string sqlstr = @"INSERT INTO WG_DB.dbo.WG_USER (U_ID, U_NAME, U_PWD, DPT, FLAG, TYPE) VALUES('{0}', '{1}', '{2}', '{3}', 'Y', '{4}') ";
            mssql.SQLexcute(connWGDB, string.Format(sqlstr, U_ID, U_PWD, DPT, TYPE));
            return false;
        }
        #endregion

        #region 修改用户密码
        public bool ChangePwd(string U_ID, string OldPwd, string NewPwd, out string Msg)
        {
            Msg = null;
            string sqlstrFind = @" SELECT U_PWD FROM WG_DB.dbo.WG_USER WHERE U_ID = '{0}'";
            string sqlstrUpt = @" UPDATE WG_DB.dbo.WG_USER SET U_PWD = '{}' WHERE U_ID = '{0}' ";
            DataTable dt = mssql.SQLselect(connWGDB, string.Format(sqlstrFind, U_ID));
            if(dt != null)
            {
                if(dt.Rows[0][0].ToString() == OldPwd)
                {
                    mssql.SQLexcute(connWGDB, string.Format(sqlstrUpt, U_ID, NewPwd));
                    return true;
                }
                else
                {
                    Msg = "原密码输入错误，请重新输入";
                    return false;
                }
            }
            else
            {
                Msg = "找不到此账号 " + U_ID;
                return false;
            }
        }
        #endregion

        #region 用户登录认证
        public void Login(UserObjectReturn userObjRtn2)
        {
            userObjRtn = userObjRtn2;
            userObj = new UserObjectBase();
            userObj.Uid = userObjRtn.Uid;
            userObj.Pwd = userObjRtn.Pwd;

            GetWgInf();
            GetErpInf();

            MainJudge();
            GetPermission();
        }

        private void MainJudge()
        {
            if (userObj.WgExise)
            {
                if (JudgeWgTypeEqualsErp())
                {
                    if (userObj.ErpValid)
                    {
                        if (!userObj.WgFlag)//重置密码标记
                        {
                            userObjRtn.Status = true;
                            userObjRtn.Name = userObj.ErpName;
                            userObjRtn.Dpt = userObj.ErpDpt;
                            UpdateWgInf();
                        }
                        else
                        {
                            if (JudgeErpPwdSame())
                            {
                                if (JudgeWgPwdSame())
                                {
                                    userObjRtn.Status = true;
                                    userObjRtn.Name = userObj.WgName;
                                    userObjRtn.Dpt = userObj.WgDpt;
                                }
                                else
                                {
                                    userObjRtn.Status = false;
                                    userObjRtn.Msg = "密码错误，请重新输入";
                                }
                            }
                            else
                            {
                                userObjRtn.Status = true;
                                userObjRtn.Name = userObj.ErpName;
                                userObjRtn.Dpt = userObj.ErpDpt;
                                UpdateWgInf();
                            }
                        }
                    }
                    else
                    {
                        userObjRtn.Status = false;
                        userObjRtn.Msg = "该ERP账号已被停用，请重新输入";
                    }
                }
                else if (JudgeWgTypeContainsClinet() || JudgeWgTypeContainsWeb())
                {
                    if (JudgeWgPwdSame())
                    {
                        userObjRtn.Status = true;
                        userObjRtn.Name = userObj.WgName;
                        userObjRtn.Dpt = userObj.WgDpt;
                    }
                    else
                    {
                        userObjRtn.Status = false;
                        userObjRtn.Msg = "密码错误，请重新输入";
                    }
                }
                else
                {
                    userObjRtn.Status = false;
                    userObjRtn.Msg = "账号不存在，请重新输入";
                }
            }
            else if (userObj.ErpExist)
            {
                if (userObj.ErpValid)
                {
                    userObjRtn.Status = true;
                    userObjRtn.Name = userObj.ErpName;
                    userObjRtn.Dpt = userObj.ErpDpt;
                    InsertWgInf();
                }
                else
                {
                    userObjRtn.Status = false;
                    userObjRtn.Msg = "该ERP账号已被停用，请重新输入";
                }
            }
            else
            {
                userObjRtn.Status = false;
                userObjRtn.Msg = "账号不存在，请重新输入";
            }
        }

        private void GetWgInf()
        {
            string sqlstr = @"SELECT U_ID, U_NAME, U_PWD, ERP_PWD, DPT, FLAG, TYPE FROM WG_DB.dbo.WG_USER WHERE U_ID = '{0}' ";
            DataTable dt = mssql.SQLselect(connWGDB, string.Format(sqlstr, userObj.Uid));
            if (dt != null)
            {
                userObj.WgExise = true;
                userObj.WgName = dt.Rows[0][1].ToString();
                userObj.WgPwd = dt.Rows[0][2].ToString();
                userObj.WgErpPwd = dt.Rows[0][3].ToString();
                userObj.WgDpt = dt.Rows[0][4].ToString();
                userObj.WgFlag = (dt.Rows[0][5].ToString() == "Y") ? true : false;
                userObj.WgType = dt.Rows[0][6].ToString();
            }
            else
            {
                userObj.WgExise = false;
            }
        }
        
        private void GetErpInf()
        {
            string sqlstr = @" SELECT RTRIM(MA001) UID, RTRIM(MA002) NAME, RTRIM(ME002) DPT, RTRIM(MA003) ERPPWD, RTRIM(MA005) ERPVALID 
                                FROM DSCSYS.dbo.DSCMA AS DSCMA 
                                LEFT JOIN CMSMV ON MV001 = MA001 
                                LEFT JOIN CMSME ON ME001 = MV004 
                                WHERE MA001 = '{0}'";
            DataTable dt = mssql.SQLselect(connCOMFORT, string.Format(sqlstr, userObj.Uid));
            if (dt != null)
            {
                userObj.ErpExist = true;
                userObj.ErpName = dt.Rows[0][1].ToString();
                userObj.ErpDpt = dt.Rows[0][2].ToString();
                userObj.ErpPwd = dt.Rows[0][3].ToString();
                userObj.ErpValid = dt.Rows[0][4].Equals("Y") ? true : false;
            }
            else
            {
                userObj.ErpExist = false;
            }
        }

        private bool JudgeWgTypeContainsClinet()
        {
            return userObj.WgType.Contains("Client") ? true : false;
        }

        private bool JudgeWgTypeContainsWeb()
        {
            return userObj.WgType.Contains("Web") ? true : false;
        }

        private bool JudgeWgTypeEqualsErp()
        {
            return userObj.WgType.Equals("ERP") ? true : false;
        }

        private bool JudgeWgPwdSame()
        {
            return (userObj.Pwd == userObj.WgPwd) ? true : false;
        }

        private bool JudgeErpPwdSame()
        {
            return (userObj.WgErpPwd == userObj.ErpPwd) ? true : false;
        }

        private void InsertWgInf()
        {
            userObj.ErpPwd = userObj.ErpPwd.Replace("'", "''");
            string sqlstr = @"INSERT INTO WG_DB.dbo.WG_USER (U_ID, U_NAME, U_PWD, ERP_PWD, DPT, FLAG, TYPE) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', 'Y', 'ERP') ";
            mssql.SQLexcute(connWGDB, string.Format(sqlstr, userObjRtn.Uid, userObjRtn.Name, userObjRtn.Pwd, userObj.ErpPwd, userObjRtn.Dpt));
        }

        private void UpdateWgInf()
        {
            userObj.ErpPwd = userObj.ErpPwd.Replace("'", "''");
            string sqlstr = @"UPDATE WG_DB.dbo.WG_USER SET U_NAME = '{1}', U_PWD = '{2}', ERP_PWD = '{3}', DPT = '{4}', FLAG = 'Y' WHERE U_ID = '{0}'";
            mssql.SQLexcute(connWGDB, string.Format(sqlstr, userObjRtn.Uid, userObjRtn.Name, userObjRtn.Pwd, userObj.ErpPwd, userObjRtn.Dpt));
        }

        private void GetPermission()
        {
            if (userObjRtn.Status) userObjRtn.Permission = UserPermission.GetPermUser(userObj.Uid);
        }

        private class UserObjectBase
        {
            private string uid = null;
            private string pwd = null;

            private bool wgExist = false;
            private string wgPwd = null;
            private string wgErpPwd = null;
            private string wgDpt = null;
            private string wgName = null;
            private bool wgFlag = true;
            private string wgType = null;

            private bool erpExist = false;
            private string erpPwd = null;
            private string erpDpt = null;
            private string erpName = null;
            private bool erpValid = true;

            public string Uid { get { return uid; } set { uid = value; } }
            public string Pwd { get { return pwd; } set { pwd = value; } }

            public bool WgExise { get { return wgExist; } set { wgExist = value; } }
            public string WgPwd { get { return wgPwd; } set { wgPwd = value; } }
            public string WgErpPwd { get { return wgErpPwd; } set { wgErpPwd = value; } }
            public string WgDpt { get { return wgDpt; } set { wgDpt = value; } }
            public string WgName { get { return wgName; } set { wgName = value; } }
            public bool WgFlag { get { return wgFlag; } set { wgFlag = value; } }
            public string WgType { get { return wgType; } set { wgType = value; } }

            public bool ErpExist { get { return erpExist; } set { erpExist = value; } }
            public string ErpPwd { get { return erpPwd; } set { erpPwd = value; } }
            public string ErpDpt { get { return erpDpt; } set { erpDpt = value; } }
            public string ErpName { get { return erpName; } set { erpName = value; } }
            public bool ErpValid { get { return erpValid; } set { erpValid = value; } }
        }

        public class UserObjectReturn
        {
            private bool status = false;
            private string uid = null;
            private string pwd = null;
            private string name = null;
            private string dpt = null;
            private string msg = "";
            private List<string> permission = null;

            public string Uid { get { return uid; } set { uid = value; } }
            public string Pwd { get { return pwd; } set { pwd = value; } }
            public string Dpt { get { return dpt; } set { dpt = value; } }
            public string Name { get { return name; } set { name = value; } }
            public string Msg { get { return msg; } set { msg = value; } }
            public bool Status { get { return status; } set { status = value; } }
            public List<string> Permission { get { return permission; } set { permission = value; } }
        }
        #endregion
    }
}
