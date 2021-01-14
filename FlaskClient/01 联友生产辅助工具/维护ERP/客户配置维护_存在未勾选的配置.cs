using System;
using System.Data;
using System.Windows.Forms;


namespace HarveyZ.维护ERP
{
    public partial class 客户配置维护_存在未勾选的配置 : Form
    {
        #region 静态变量
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        private Mssql mssql = new Mssql();
        private string connYF = FormLogin.infObj.connYF;
        #endregion

        #region 初始化
        public 客户配置维护_存在未勾选的配置(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            Init();
            UI();
        }

        private void Init()
        {

        }
        #endregion

        #region 界面
        private void UI()
        {
            if(DgvMain.DataSource == null)
            {
                BtnUpdate.Enabled = false;
            }
            else
            {
                BtnUpdate.Enabled = true;
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            DgvOpt.SetShow(DgvMain, GetShowDt());
            UI();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DataUpdate();
            DgvOpt.SetShow(DgvMain);
            Msg.Show("已更新！");
            UI();
        }
        #endregion

        #region 逻辑
        DataTable GetShowDt()
        {
            string sqlStr = @"SELECT RTRIM(TR001) TR001, RTRIM(TR002) TR002, RTRIM(TR003) TR003, TR004, TR009, TR017, CB015, CB004, MB002, TR1.TR200 
                                FROM COPTR AS TR1 
                                INNER JOIN INVMB ON MB001 = TR009
                                LEFT JOIN BOMCB ON CB001 = TR004 AND CB005 = TR009 
                                WHERE 1=1
                                AND NOT EXISTS (SELECT 1 FROM COPTR AS TR2 WHERE TR2.TR001 = TR1.TR001 AND TR2.TR002 = TR1.TR002 AND SUBSTRING(TR2.TR003, 1, LEN(TR2.TR003) - 3) = SUBSTRING(TR1.TR003, 1, LEN(TR1.TR003) - 3) AND TR017 = 'Y')
                                AND TR001 LIKE '1%'
                                AND TR004 LIKE '2%'
                                AND CB015 = 'Y'
                                ORDER BY RTRIM(TR001), RTRIM(TR002), TR004, RTRIM(TR003), CB004, MB002";
            DataTable dt = mssql.SQLselect(connYF, sqlStr);
            return dt;
        }

        private void DataUpdate()
        {
            string sqlStr = @"UPDATE COPTR SET TR017 = CB015
                                FROM COPTR AS TR1 
                                INNER JOIN INVMB ON MB001 = TR009
                                LEFT JOIN BOMCB ON CB001 = TR004 AND CB005 = TR009 
                                WHERE 1=1
                                AND NOT EXISTS (SELECT 1 FROM COPTR AS TR2 WHERE TR2.TR001 = TR1.TR001 AND TR2.TR002 = TR1.TR002 
	                                AND SUBSTRING(TR2.TR003, 1, LEN(TR2.TR003) - 3) = SUBSTRING(TR1.TR003, 1, LEN(TR1.TR003) - 3) AND TR017 = 'Y')
                                AND TR001 LIKE '1%'
                                AND TR004 LIKE '2%'
                                AND CB015 = 'Y'
                                ORDER BY RTRIM(TR001), RTRIM(TR002), TR004, RTRIM(TR003), CB004, MB002";
            mssql.SQLexcute(connYF, sqlStr);
        }
        #endregion
    }
}
