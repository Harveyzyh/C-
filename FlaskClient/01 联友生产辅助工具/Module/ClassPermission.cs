using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HarveyZ
{
    public class UserPermission
    {
        private Mssql mssql = new Mssql();
        private string conn = null;
        private string type = null;

        public UserPermission(string conn, string type)
        {
            this.conn = conn;
            this.type = type;
        }

        #region 更新基本权限表
        public void SetPermBase(List<string> getList)
        {
            string slqStrReset = @" UPDATE WG_PERM_BASE SET Valid = 0 WHERE Type = '{0}' ";
            string slqStrFind = @" SELECT K_ID FROM WG_PERM_BASE WHERE Name = '{0}' AND Type = '{1}'";
            string slqStrSet = @" UPDATE WG_PERM_BASE SET Valid = 1 WHERE Name = '{0}' AND Type = '{1}'";
            string slqStrNew = @" INSERT INTO WG_PERM_BASE (Type, Name) VALUES ('{1}', '{0}') ";

            if (getList != null && getList.Count != 0)
            {
                mssql.SQLexcute(conn, string.Format(slqStrReset, type));

                foreach (string MenuListTmp in getList)
                {
                    if (mssql.SQLexist(conn, string.Format(slqStrFind, MenuListTmp, type)))
                    {
                        mssql.SQLexcute(conn, string.Format(slqStrSet, MenuListTmp, type));
                    }
                    else
                    {
                        mssql.SQLexcute(conn, string.Format(slqStrNew, MenuListTmp, type));
                    }
                }
            }
        }
        #endregion

        #region 用户权限操作
        public List<string> GetPermUser(string uId)
        {
            List<string> backList = new List<string> { };
            string slqStr = @"SELECT Name FROM WG_PERM_BASE 
                                INNER JOIN WG_PERM_USER ON WG_PERM_USER.Perm_ID = WG_PERM_BASE.K_ID AND WG_PERM_USER.Type = WG_PERM_BASE.Type
                                WHERE WG_PERM_USER.U_ID = '{0}' AND WG_PERM_USER.Type = '{1}' ";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, uId, type));
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    backList.Add(dt.Rows[rowIndex][0].ToString());
                }
            }
            return backList;
        }

        public void GetPermUserDetail(string uId, string permName, out bool newFlag, out bool editFlag, out bool delFlag, out bool outFlag, out bool lockFlag, out bool printFlag)
        {
            newFlag = false;
            editFlag = false;
            delFlag = false;
            outFlag = false;
            lockFlag = false;
            printFlag = false;
            string slqStr = @"SELECT WG_PERM_USER.New, WG_PERM_USER.Edit, WG_PERM_USER.Del, WG_PERM_USER.Out, WG_PERM_USER.Lock, WG_PERM_USER.[Print] FROM WG_PERM_BASE 
                                INNER JOIN WG_PERM_USER ON WG_PERM_USER.Perm_ID = WG_PERM_BASE.K_ID AND WG_PERM_USER.Type = WG_PERM_BASE.Type
                                WHERE WG_PERM_USER.U_ID = '{0}' AND WG_PERM_USER.Type = '{1}' AND WG_PERM_BASE.Name = '{2}'";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, uId, type, permName));
            if (dt != null)
            {
                if (dt.Rows[0]["New"].Equals(true)) newFlag = true;
                if (dt.Rows[0]["Edit"].Equals(true)) editFlag = true;
                if (dt.Rows[0]["Del"].Equals(true)) delFlag = true;
                if (dt.Rows[0]["Out"].Equals(true)) outFlag = true;
                if (dt.Rows[0]["Lock"].Equals(true)) lockFlag = true;
                if (dt.Rows[0]["Print"].Equals(true)) printFlag = true;
            }
        }

        public void SetPermUser(string U_ID, List<string> getList)
        {
            string slqStrDel = @" DELETE FROM WG_PERM_USER WHERE U_ID = '{0}' AND Type = '{1}'";
            string slqStrNew = @" INSERT INTO WG_PERM_USER (Type, U_ID, U_NAME, Perm_ID) 
                                    SELECT Type, WG_USER.U_ID, WG_USER.U_NAME, WG_PERM_BASE.K_ID 
                                    FROM WG_PERM_BASE 
                                    INNER JOIN WG_USER ON TYPE = Type 
                                    WHERE 1=1 
                                    AND WG_PERM_BASE.Name = '{1}' 
                                    AND WG_USER.U_ID = '{0}' 
                                    AND TYPE = '{2}'";

            if(getList != null && getList.Count != 0)
            {
                mssql.SQLexcute(conn, string.Format(slqStrDel, U_ID, type));
                foreach (string getListTmp in getList)
                {
                    mssql.SQLexcute(conn, string.Format(slqStrNew, U_ID, getListTmp, type));
                }
            }
        }

        public void SetPermUser(string U_ID, DataTable dt)
        {
            string slqStrDel = @" DELETE FROM WG_PERM_USER WHERE U_ID = '{0}' AND Type = '{1}'";
            string slqStrNew = @" INSERT INTO WG_PERM_USER (Type, U_ID, U_NAME, Perm_ID, New, Edit, Del, Out, Lock, [Print]) 
                                    SELECT Type, WG_USER.U_ID, WG_USER.U_NAME, WG_PERM_BASE.K_ID, 
                                    '{3}' New, '{4}' Edit, '{5}' Del, '{6}' Out, '{7}' Lock, '{8}' [Print]  
                                    FROM WG_PERM_BASE 
                                    INNER JOIN WG_USER ON TYPE = Type 
                                    WHERE 1=1 
                                    AND WG_PERM_BASE.Name = '{1}' 
                                    AND WG_USER.U_ID = '{0}' 
                                    AND TYPE = '{2}'";

            if (dt != null && dt.Rows.Count != 0)
            {
                mssql.SQLexcute(conn, string.Format(slqStrDel, U_ID, type));
                for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex ++)
                {
                    mssql.SQLexcute(conn, string.Format(slqStrNew, U_ID, dt.Rows[rowIndex]["PermName"].ToString(), type,
                        dt.Rows[rowIndex]["New"].ToString(), dt.Rows[rowIndex]["Edit"].ToString(), dt.Rows[rowIndex]["Del"].ToString(),
                        dt.Rows[rowIndex]["Out"].ToString(), dt.Rows[rowIndex]["Lock"].ToString(), dt.Rows[rowIndex]["Print"].ToString()));
                }
            }
        }
        #endregion

        #region 显示权限明细
        public DataTable ShowUserPerm(string U_ID)
        {
            string slqStr = @"SELECT CONVERT(BIT, (CASE WHEN U.Perm_ID IS NULL THEN 0 ELSE 1 END)) AS 有效码, 
                                CAST(ISNULL(U.New, 0) AS BIT) 新增, 
                                CAST(ISNULL(U.Edit, 0) AS BIT) 编辑, 
                                CAST(ISNULL(U.Del, 0) AS BIT) 删除, 
                                CAST(ISNULL(U.Out, 0) AS BIT) 输出, 
                                CAST(ISNULL(U.[Print], 0) AS BIT) 打印, 
                                CAST(ISNULL(U.Lock, 0) AS BIT) 锁定, 
                                B.Name 权限名 
                                FROM ( SELECT Type, K_ID, Name FROM WG_PERM_BASE WHERE Valid = 1) AS B 
                                LEFT JOIN WG_PERM_USER AS U ON B.K_ID = U.Perm_ID AND B.Type = U.Type AND U.U_ID = '{0}' 
                                WHERE 1 = 1 AND B.Type = '{1}' 
                                ORDER BY B.Name";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, U_ID, type));
            if(dt != null)
            {
                return dt;
            }
            else
            {
                return null;
            }

        }
        #endregion
    }
}
