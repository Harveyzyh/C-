using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HarveyZ
{
    public class ERP_UserLogin
    {
        #region 私有变量
        private UserObjectBase userObj = null;
        private UserObjectReturn userObjRtn = null;
        private Mssql mssql = FormLogin.infObj.sql;
        private string connWG = null;
        private string connYF = null;
        private string type = "ERP";
        #endregion

        #region 信息处理类
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
            private string erpGroup = null;
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
            public string ErpGroup { get { return erpGroup; } set { erpGroup = value; } }
            public bool ErpValid { get { return erpValid; } set { erpValid = value; } }
        }

        public class UserObjectReturn
        {
            private bool status = false;
            private string uid = null;
            private string pwd = null;
            private string name = null;
            private string dpt = null;
            private string group = null;
            private string msg = "";
            private List<string> permission = null;

            public string Uid { get { return uid; } set { uid = value; } }
            public string Pwd { get { return pwd; } set { pwd = value; } }
            public string Dpt { get { return dpt; } set { dpt = value; } }
            public string Name { get { return name; } set { name = value; } }
            public string Group { get { return group; } set { group = value; } }
            public string Msg { get { return msg; } set { msg = value; } }
            public bool Status { get { return status; } set { status = value; } }
            public List<string> Permission { get { return permission; } set { permission = value; } }
        }
        #endregion

        #region 类初始化
        /// <summary>
        /// 易飞ERP外挂用户登录
        /// </summary>
        /// <param name="connWG">外挂管理库</param>
        /// <param name="connYF">易飞ERP库</param>
        public ERP_UserLogin(string connWG, string connYF)
        {
            this.connWG = connWG;
            this.connYF = connYF;
        }
        #endregion

        #region 添加用户
        public bool AddUser(string U_ID, string U_PWD, string DPT, string TYPE)
        {
            string slqStr = @"INSERT INTO WG_DB.dbo.WG_USER (U_ID, U_NAME, U_PWD, DPT, FLAG, TYPE) VALUES('{0}', '{1}', '{2}', '{3}', 'Y', '{4}') ";
            mssql.SQLexcute(connWG, string.Format(slqStr, U_ID, U_PWD, DPT, TYPE));
            return false;
        }
        #endregion

        #region 修改用户密码
        public bool ChangePwd(string U_ID, string OldPwd, string NewPwd, out string Msg)
        {
            Msg = null;
            string slqStrFind = @" SELECT U_PWD FROM WG_DB.dbo.WG_USER WHERE U_ID = '{0}'";
            string slqStrUpt = @" UPDATE WG_DB.dbo.WG_USER SET U_PWD = '{}' WHERE U_ID = '{0}' ";
            DataTable dt = mssql.SQLselect(connWG, string.Format(slqStrFind, U_ID));
            if(dt != null)
            {
                if(dt.Rows[0][0].ToString() == OldPwd)
                {
                    mssql.SQLexcute(connWG, string.Format(slqStrUpt, U_ID, NewPwd));
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
            userObjRtn2.Pwd = CEncrypt.GetMd5Str(userObjRtn2.Pwd);
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
                            userObjRtn.Group = userObj.ErpGroup;
                            UpdateWgInf();
                        }
                        else
                        {
                            if (JudgeErpPwdSame()) 
                            {
                                if (JudgeWgPwdSame())
                                {
                                    userObjRtn.Status = true;
                                    userObjRtn.Name = userObj.ErpName;
                                    userObjRtn.Dpt = userObj.ErpDpt;
                                    userObjRtn.Group = userObj.ErpGroup;
                                    UpdateWgInf();
                                }
                                else
                                {
                                    userObjRtn.Status = false;
                                    userObjRtn.Msg = "密码错误，请重新输入";
                                }
                            }
                            else //erp密码不一样了，视为erp更改过密码，登录
                            {
                                userObjRtn.Status = true;
                                userObjRtn.Name = userObj.ErpName;
                                userObjRtn.Dpt = userObj.ErpDpt;
                                userObjRtn.Group = userObj.ErpGroup;
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
                else //当wg信息type不为erp，其他客户端用户
                {
                    if (JudgeWgTypeContainsClinet() || JudgeWgTypeContainsWeb())
                    {
                        if (!userObj.WgFlag) //重置密码标记
                        {
                            userObjRtn.Status = true;
                            userObjRtn.Name = userObj.WgName;
                            userObjRtn.Dpt = userObj.WgDpt;
                            UpdateWgInf();
                        }
                        else
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
                    }
                    else
                    {
                        userObjRtn.Status = false;
                        userObjRtn.Msg = "账号不存在，请重新输入";
                    }
                }
            }
            else //当wg信息不存在
            {
                if (userObj.ErpExist) //erp信息存在
                {
                    if (userObj.ErpValid) //erp账号有效，登录
                    {
                        userObjRtn.Status = true;
                        userObjRtn.Name = userObj.ErpName;
                        userObjRtn.Dpt = userObj.ErpDpt;
                        userObjRtn.Group = userObj.ErpGroup;
                        InsertWgInf();
                    }
                    else 
                    {
                        userObjRtn.Status = false;
                        userObjRtn.Msg = "该ERP账号已被停用，请重新输入";
                    }
                }
                else //不存在所有信息，登陆失败
                {
                    userObjRtn.Status = false;
                    userObjRtn.Msg = "账号不存在，请重新输入";
                }
            }
        }

        private void GetWgInf()
        {
            string slqStr = @"SELECT U_ID, U_NAME, U_PWD, ERP_PWD, DPT, FLAG, TYPE FROM WG_DB.dbo.WG_USER WHERE U_ID = '{0}' AND TYPE = '{1}'";
            DataTable dt = mssql.SQLselect(connWG, string.Format(slqStr, userObj.Uid, type));
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
            string slqStr = @" SELECT RTRIM(MA001) UID, ISNULL(RTRIM(MA002), '') NAME, ISNULL(RTRIM(ME002), '') DPT, 
                                RTRIM(MA003) ERPPWD, RTRIM(MA005) ERPVALID, ISNULL(RTRIM(MF004), '') USR_GROUP 
                                FROM DSCSYS.dbo.DSCMA AS DSCMA 
                                LEFT JOIN CMSMV ON MV001 = MA001 
                                LEFT JOIN CMSME ON ME001 = MV004 
                                LEFT JOIN ADMMF ON MF001 = MA001
                                WHERE MA001 = '{0}'";
            DataTable dt = mssql.SQLselect(connYF, string.Format(slqStr, userObj.Uid));
            if (dt != null)
            {
                userObj.ErpExist = true;
                userObj.ErpName = dt.Rows[0][1].ToString();
                userObj.ErpDpt = dt.Rows[0][2].ToString();
                userObj.ErpPwd = dt.Rows[0][3].ToString();
                userObj.ErpValid = dt.Rows[0][4].Equals("Y") ? true : false;
                userObj.ErpGroup = dt.Rows[0][5].ToString();
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
            string slqStr = @"INSERT INTO WG_DB.dbo.WG_USER (U_ID, U_NAME, U_PWD, ERP_PWD, DPT, FLAG, TYPE) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', 'Y', 'ERP') ";
            mssql.SQLexcute(connWG, string.Format(slqStr, userObjRtn.Uid, userObjRtn.Name, userObjRtn.Pwd, userObj.ErpPwd, userObjRtn.Dpt));
        }

        private void UpdateWgInf()
        {
            userObj.ErpPwd = userObj.ErpPwd.Replace("'", "''");
            string slqStr = @"UPDATE WG_DB.dbo.WG_USER SET U_NAME = '{1}', U_PWD = '{2}', ERP_PWD = '{3}', DPT = '{4}', FLAG = 'Y' WHERE U_ID = '{0}'";
            mssql.SQLexcute(connWG, string.Format(slqStr, userObjRtn.Uid, userObjRtn.Name, userObjRtn.Pwd, userObj.ErpPwd, userObjRtn.Dpt));
        }

        private void GetPermission()
        {
            if (userObjRtn.Status) userObjRtn.Permission = FormLogin.infObj.userPermission.GetPermUser(userObj.Uid);
        }
        #endregion

        #region 获取所有用户信息
        public DataTable GetUserInfo()
        {
            string slqStr = @"SELECT U_ID 账号, U_NAME 用户名, DPT 部门, FLAG 不允许重置密码, TYPE 账号来源类型 
                                FROM WG_USER WHERE TYPE = 'ERP' ORDER BY U_ID ";
            return mssql.SQLselect(connWG, slqStr);
        }
        #endregion

        #region 设置用户重置密码
        public bool SetPasswdReset(string U_id)
        {
            try
            {
                string slqStr = @"UPDATE WG_USER SET FLAG = 'N' WHERE U_ID = '{0}' AND TYPE = 'ERP'";
                mssql.SQLexcute(connWG, string.Format(slqStr, U_id));
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }


    public class Client_UserLogin
    {
        #region 私有变量
        private UserObjectBase userObj = null;
        private UserObjectReturn userObjRtn = null;
        private Mssql mssql = new Mssql();
        private string conn = null;
        private string type = null;
        #endregion

        #region 信息处理类
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

            public string Uid { get { return uid; } set { uid = value; } }
            public string Pwd { get { return pwd; } set { pwd = value; } }

            public bool WgExise { get { return wgExist; } set { wgExist = value; } }
            public string WgPwd { get { return wgPwd; } set { wgPwd = value; } }
            public string WgErpPwd { get { return wgErpPwd; } set { wgErpPwd = value; } }
            public string WgDpt { get { return wgDpt; } set { wgDpt = value; } }
            public string WgName { get { return wgName; } set { wgName = value; } }
            public bool WgFlag { get { return wgFlag; } set { wgFlag = value; } }
            public string WgType { get { return wgType; } set { wgType = value; } }
        }

        public class UserObjectReturn
        {
            private bool status = false;
            private string uid = null;
            private string pwd = null;
            private string name = null;
            private string dpt = null;
            private string group = null;
            private string msg = "";
            private List<string> permission = null;

            public string Uid { get { return uid; } set { uid = value; } }
            public string Pwd { get { return pwd; } set { pwd = value; } }
            public string Dpt { get { return dpt; } set { dpt = value; } }
            public string Name { get { return name; } set { name = value; } }
            public string Group { get { return group; } set { group = value; } }
            public string Msg { get { return msg; } set { msg = value; } }
            public bool Status { get { return status; } set { status = value; } }
            public List<string> Permission { get { return permission; } set { permission = value; } }
        }
        #endregion

        #region 类初始化
        /// <summary>
        /// 非ERP用户登录
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="type">用户类型</param>
        public Client_UserLogin(string conn, string type)
        {
            this.conn = conn;
            this.type = type;
        }
        #endregion

        #region 添加用户
        public bool AddUser(string U_ID, string U_NAME, string U_PWD, string DPT, out string msg)
        {
            msg = "";
            string slqStrSelect = @"SELECT U_ID FROM WG_DB.dbo.WG_USER WHERE U_ID = '{0}' AND TYPE = '{1}' ";
            string slqStrIns = @"INSERT INTO WG_DB.dbo.WG_USER (U_ID, U_NAME, U_PWD, DPT, FLAG, TYPE) VALUES('{0}', '{1}', '{2}', '{3}', 'Y', '{4}') ";

            if (!mssql.SQLexist(conn, string.Format(slqStrSelect, U_ID, type)))
            {
                mssql.SQLexcute(conn, string.Format(slqStrIns, U_ID, U_NAME, CEncrypt.GetMd5Str(U_PWD), DPT, type));
                return true;
            }
            else
            {
                msg = "该账号已存在，请重新输入";
                return false;
            }
            
        }
        #endregion

        #region 修改用户密码
        public bool ChangePwd(string U_ID, string OldPwd, string NewPwd, out string Msg)
        {
            Msg = null;
            string slqStrFind = @" SELECT U_PWD FROM WG_DB.dbo.WG_USER WHERE U_ID = '{0}' AND TYPE = '{1}'";
            string slqStrUpt = @" UPDATE WG_DB.dbo.WG_USER SET U_PWD = '{1}' WHERE U_ID = '{0}' AND TYPE = '{2}'";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStrFind, U_ID, type));
            if (dt != null)
            {
                if (dt.Rows[0][0].ToString() == CEncrypt.GetMd5Str(OldPwd))
                {
                    mssql.SQLexcute(conn, string.Format(slqStrUpt, U_ID, CEncrypt.GetMd5Str(NewPwd), type));
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

        #region 获取所有用户信息
        public DataTable GetUserInfo()
        {
            string slqStr = @"SELECT U_ID 账号, U_NAME 用户名, DPT 部门, FLAG 不允许重置密码, TYPE 账号来源类型 
                                FROM WG_USER WHERE TYPE = '{0}' ORDER BY U_ID ";
            return mssql.SQLselect(conn, string.Format(slqStr, type));
        }
        #endregion

        #region 设置用户重置密码
        public bool SetPasswdReset(string U_id)
        {
            try
            {
                string slqStr = @"UPDATE WG_USER SET FLAG = 'N' WHERE U_ID = '{0}' AND TYPE = '{1}'";
                mssql.SQLexcute(conn, string.Format(slqStr, U_id, type));
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 用户登录认证
        public void Login(UserObjectReturn userObjRtn2)
        {
            userObjRtn2.Pwd = CEncrypt.GetMd5Str(userObjRtn2.Pwd);
            userObjRtn = userObjRtn2;
            userObj = new UserObjectBase();
            userObj.Uid = userObjRtn.Uid;
            userObj.Pwd = userObjRtn.Pwd;

            GetWgInf();

            MainJudge();
            GetPermission();
        }

        private void MainJudge()
        {
            if (userObj.WgExise)
            {
                if (!userObj.WgFlag) //重置密码标记
                {
                    userObjRtn.Status = true;
                    userObjRtn.Name = userObj.WgName;
                    userObjRtn.Dpt = userObj.WgDpt;
                    UpdateWgInf();
                }
                else
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
            }
            else //当wg信息不存在
            {

                userObjRtn.Status = false;
                userObjRtn.Msg = "账号不存在，请重新输入";
            }
        }

        private void GetWgInf()
        {
            string slqStr = @"SELECT U_ID, U_NAME, U_PWD, ERP_PWD, DPT, FLAG, TYPE FROM WG_DB.dbo.WG_USER WHERE U_ID = '{0}' AND TYPE = '{1}' ";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, userObj.Uid, type));
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

        private bool JudgeWgPwdSame()
        {
            return (userObj.Pwd == userObj.WgPwd) ? true : false;
        }

        private void UpdateWgInf()
        {
            string slqStr = @"UPDATE WG_DB.dbo.WG_USER SET U_PWD = '{1}', FLAG = 'Y' WHERE U_ID = '{0}'";
            mssql.SQLexcute(conn, string.Format(slqStr, userObjRtn.Uid, userObjRtn.Pwd));
        }

        private void GetPermission()
        {
            if (userObjRtn.Status) userObjRtn.Permission = FormLogin.infObj.userPermission.GetPermUser(userObj.Uid);
        }
        #endregion
    }
}
