using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HarveyZ
{
    class UserPermission
    {
        string Conn_WG_DB = Global_Const.strConnection_WG_DB;
        private Mssql mssql = new Mssql();

        #region 更新基本权限表
        public void SetPermBase(List<string> getList)
        {
            string sqlstrReset = @" UPDATE WG_PERM_BASE SET Valid = 'N' ";
            string sqlstrFind = @" SELECT K_ID FROM WG_PERM_BASE WHERE Name = '{0}' ";
            string sqlstrSet = @" UPDATE WG_PERM_BASE SET Valid = 'Y' WHERE Name = '{0}' ";
            string sqlstrNew = @" INSERT INTO WG_PERM_BASE (Name) VALUES ('{0}') ";

            if (getList != null && getList.Count != 0)
            {
                mssql.SQLexcute(Conn_WG_DB, sqlstrReset);

                foreach (string MenuListTmp in getList)
                {
                    if (mssql.SQLselect(Conn_WG_DB, sqlstrFind) != null)
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
        public List<string> GetPermUser(string U_ID)
        {
            List<string> backList = new List<string> { };
            string sqlstr = @"SELECT Name FROM WG_PERM_BASE 
                                INNER JOIN WG_PERM_USER ON WG_PERM_USER.Permission_ID = WG_PERM_BASE.K_ID 
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

        public void SetPermUser(string U_ID, List<string> getList)
        {
            string sqlstrDel = @" DELETE FROM WG_PERM_USER WHERE U_ID = '{0}' ";
            string sqlstrNew = @"INSERT INTO WG_PERM_USER (U_ID, U_NAME, Permission_ID) 
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
    }
}
