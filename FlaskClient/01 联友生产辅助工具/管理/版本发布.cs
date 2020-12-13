using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ
{
    public partial class 版本发布 : Form
    {
        private Mssql mssql = new Mssql();
        private string connWG = FormLogin.infObj.connWG;
        
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        public 版本发布(string text)
        {
            InitializeComponent(); 
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);

        }

        private void BtnSetNew_Click(object sender, EventArgs e)
        {
            string progName = FormLogin.infObj.progName;
            string progVer = FormLogin.infObj.progVer;

            string serverVer = GetServerProgVer(progName);

            if(GetNewer(serverVer, progVer))
            {
                SetNewProgVer(progName, progVer);
                Msg.Show("已更新版本为当前软件版本。");
            }
            else
            {
                Msg.ShowErr("操作已跳过，服务器上版本高于当前软件版本。");
            }
        }

        private string GetServerProgVer(string progName)
        {
            string slqStr = @"SELECT Version From WG_APP_INF Where ProgName = '{0}' ";
            DataTable dt = mssql.SQLselect(connWG, string.Format(slqStr, progName));
            if (dt != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        private bool GetNewer(string VerOld, string VerNew)
        {
            bool result = false;
            var NewVersionList = VerNew.Split('.');
            var NowVersionList = VerOld.Split('.');
            for (int index = 0; index < (Normal.GetSubstringCount(VerNew, ".") + 1); index++)
            {
                if (int.Parse(NewVersionList[index]) > int.Parse(NowVersionList[index]))
                {
                    result = true;
                    break;
                }
                if (int.Parse(NewVersionList[index]) < int.Parse(NowVersionList[index]))
                {
                    break;
                }
            }

            return result;
        }

        private void SetNewProgVer(string progName, string newVer)
        {
            string slqStr = @"UPDATE WG_APP_INF set Version = '{1}' where ProgName = '{0}' ";
            mssql.SQLexcute(connWG, string.Format(slqStr, progName, newVer));
        }
    }
}
