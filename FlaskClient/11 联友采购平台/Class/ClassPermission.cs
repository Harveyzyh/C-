using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HarveyZ
{
    class UserPermission
    {
        private static string Conn_WG_DB = Global_Const.strConnection_WGDB;
        private static Mssql mssql = new Mssql();

        #region 更新基本权限表
        public static void SetPermBase(List<string> getList)
        {
            string sqlstrReset = @" UPDATE WG_PERM_BASE SET Valid = 0 ";
            string sqlstrFind = @" SELECT K_ID FROM WG_PERM_BASE WHERE Name = '{0}' ";
            string sqlstrSet = @" UPDATE WG_PERM_BASE SET Valid = 1 WHERE Name = '{0}' ";
            string sqlstrNew = @" INSERT INTO WG_PERM_BASE (Name) VALUES ('{0}') ";

            if (getList != null && getList.Count != 0)
            {
                mssql.SQLexcute(Conn_WG_DB, sqlstrReset);

                foreach (string MenuListTmp in getList)
                {
                    if (mssql.SQLselect(Conn_WG_DB, sqlstrFind) == null)
                    {
                        mssql.SQLexcute(Conn_WG_DB, string.Format(sqlstrSet, MenuListTmp));
                    }
                    else
                    {
                        mssql.SQLexcute(Conn_WG_DB, string.Format(sqlstrNew, MenuListTmp));
                    }
                }
            }
        }
        #endregion

        #region 用户权限操作
        public static List<string> GetPermUser(string U_ID)
        {
            List<string> backList = new List<string> { };
            string sqlstr = @"SELECT Name FROM WG_PERM_BASE 
                                INNER JOIN WG_PERM_USER ON WG_PERM_USER.Perm_ID = WG_PERM_BASE.K_ID 
                                WHERE WG_PERM_USER.U_ID = '{0}' ";
            DataTable dt = mssql.SQLselect(Conn_WG_DB, string.Format(sqlstr, U_ID));
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    backList.Add(dt.Rows[rowIndex][0].ToString());
                }
            }
            else
            {
                backList = null;
            }
            return backList;
        }

        public static void SetPermUser(string U_ID, List<string> getList)
        {
            string sqlstrDel = @" DELETE FROM WG_PERM_USER WHERE U_ID = '{0}' ";
            string sqlstrNew = @" INSERT INTO WG_PERM_USER (U_ID, U_NAME, Perm_ID) 
                                    SELECT WG_USER.U_ID, WG_USER.U_NAME, WG_PERM_BASE.K_ID 
                                    FROM WG_PERM_BASE 
                                    INNER JOIN WG_USER ON 1=1 
                                    WHERE 1=1 
                                    AND WG_PERM_BASE.Name = '{1}' 
                                    AND WG_USER.U_ID = '{0}' ";

            if(getList != null && getList.Count != 0)
            {
                mssql.SQLexcute(Conn_WG_DB, string.Format(sqlstrDel, U_ID));
                foreach (string getListTmp in getList)
                {
                    mssql.SQLexcute(Conn_WG_DB, string.Format(sqlstrNew, U_ID, getListTmp));
                }
            }
        }
        #endregion

        #region 显示权限明细
        public static DataTable ShowUserPerm(string U_ID)
        {
            string sqlstr = @"SELECT CONVERT(BIT, (CASE WHEN U.Perm_ID IS NULL THEN 0 ELSE 1 END)) AS 有效码 , B.Name 权限名 
                                FROM ( SELECT K_ID, Name FROM WG_PERM_BASE WHERE Valid = 1) AS B
                                LEFT JOIN WG_PERM_USER AS U ON B.K_ID = U.Perm_ID AND U.U_ID = '{0}'
                                WHERE 1 = 1
                                ORDER BY B.Name";
            DataTable dt = mssql.SQLselect(Conn_WG_DB, string.Format(sqlstr, U_ID));
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
