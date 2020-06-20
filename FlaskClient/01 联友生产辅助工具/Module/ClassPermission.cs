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
            string sqlstrReset = @" UPDATE WG_PERM_BASE SET Valid = 0 WHERE Type = '{0}' ";
            string sqlstrFind = @" SELECT K_ID FROM WG_PERM_BASE WHERE Name = '{0}' AND Type = '{1}'";
            string sqlstrSet = @" UPDATE WG_PERM_BASE SET Valid = 1 WHERE Name = '{0}' AND Type = '{1}'";
            string sqlstrNew = @" INSERT INTO WG_PERM_BASE (Type, Name) VALUES ('{1}', '{0}') ";

            if (getList != null && getList.Count != 0)
            {
                mssql.SQLexcute(conn, string.Format(sqlstrReset, type));

                foreach (string MenuListTmp in getList)
                {
                    if (mssql.SQLexist(conn, string.Format(sqlstrFind, MenuListTmp, type)))
                    {
                        mssql.SQLexcute(conn, string.Format(sqlstrSet, MenuListTmp, type));
                    }
                    else
                    {
                        mssql.SQLexcute(conn, string.Format(sqlstrNew, MenuListTmp, type));
                    }
                }
            }
        }
        #endregion

        #region 用户权限操作
        public List<string> GetPermUser(string U_ID)
        {
            List<string> backList = new List<string> { };
            string sqlstr = @"SELECT Name FROM WG_PERM_BASE 
                                INNER JOIN WG_PERM_USER ON WG_PERM_USER.Perm_ID = WG_PERM_BASE.K_ID 
                                WHERE WG_PERM_USER.U_ID = '{0}' ";
            DataTable dt = mssql.SQLselect(conn, string.Format(sqlstr, U_ID));
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    backList.Add(dt.Rows[rowIndex][0].ToString());
                }
            }
            return backList;
        }

        public void SetPermUser(string U_ID, List<string> getList)
        {
            string sqlstrDel = @" DELETE FROM WG_PERM_USER WHERE U_ID = '{0}' AND Type = '{1}'";
            string sqlstrNew = @" INSERT INTO WG_PERM_USER (Type, U_ID, U_NAME, Perm_ID) 
                                    SELECT Type, WG_USER.U_ID, WG_USER.U_NAME, WG_PERM_BASE.K_ID 
                                    FROM WG_PERM_BASE 
                                    INNER JOIN WG_USER ON TYPE = Type 
                                    WHERE 1=1 
                                    AND WG_PERM_BASE.Name = '{1}' 
                                    AND WG_USER.U_ID = '{0}' 
                                    AND TYPE = '{2}'";

            if(getList != null && getList.Count != 0)
            {
                mssql.SQLexcute(conn, string.Format(sqlstrDel, U_ID, type));
                foreach (string getListTmp in getList)
                {
                    mssql.SQLexcute(conn, string.Format(sqlstrNew, U_ID, getListTmp, type));
                }
            }
        }
        #endregion

        #region 显示权限明细
        public DataTable ShowUserPerm(string U_ID)
        {
            string sqlstr = @"SELECT CONVERT(BIT, (CASE WHEN U.Perm_ID IS NULL THEN 0 ELSE 1 END)) AS 有效码 , B.Name 权限名 
                                FROM ( SELECT Type, K_ID, Name FROM WG_PERM_BASE WHERE Valid = 1) AS B 
                                LEFT JOIN WG_PERM_USER AS U ON B.K_ID = U.Perm_ID AND B.Type = U.Type AND U.U_ID = '{0}' 
                                WHERE 1 = 1 AND B.Type = '{1}' 
                                ORDER BY B.Name";
            DataTable dt = mssql.SQLselect(conn, string.Format(sqlstr, U_ID, type));
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
