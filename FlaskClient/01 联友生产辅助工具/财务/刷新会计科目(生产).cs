using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HarveyZ.财务
{
    public partial class 刷新会计科目生产 : Form
    {
        private string connYF = FormLogin.infObj.connYF;
        private Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        private string dtp = "";

        public 刷新会计科目生产(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init() // 窗体显示初始化
        {
            dateTimePicker1.Value = DateTime.Now.AddMonths(-1);
        }

        #region 窗口大小变化设置
        private void FormMain_Resized(object sender, EventArgs e)
        {
            FormMain_Resized_Work();
        }

        private void FormMain_Resized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
            PanelTitle.Size = new Size(FormWidth, PanelTitle.Height); 
        }
        #endregion

        #region 界面
        private void button1_Click(object sender, EventArgs e)
        {
            GetDtp();
            SetData1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetDtp();
            SetData2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetDtp();
            SetData3();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GetDtp();
            SetData4();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GetDtp();
            SetData5();
        }
        #endregion

        #region 逻辑
        private void GetDtp()
        {
            dtp = dateTimePicker1.Value.ToString("yyyyMM");
        }

        /// <summary>
        /// 采购发票(委外)
        /// </summary>
        private void SetData1()
        {
            string sqlStr = @"UPDATE ACPTB SET TB013='1408'
                                FROM ACPTB
                                INNER JOIN ACPTA ON TB001=TA001 AND TB002=TA002
                                WHERE SUBSTRING(TA003,1,6)='{0}' AND (TB004='3' OR TB004='4') 
                                AND EXISTS (SELECT 1 FROM ACMMJ WHERE TB037=MJ002 AND MJ003='3001' AND MJ004='Y' )
                                AND TB013 = '1407'";
            mssql.SQLexcute(connYF, string.Format(sqlStr, dtp));
        }

        /// <summary>
        /// 主营业务收入(内销)
        /// </summary>
        private void SetData2()
        {
            string sqlStr = @"UPDATE ACRTB SET TB013='600102' 
                                FROM ACRTB INNER JOIN ACRTA ON TB001=TA001 AND TB002=TA002 
                                INNER JOIN INVMB ON TB039=MB001
                                WHERE SUBSTRING(TA038,1,6)='{0}' AND TB001='6103' AND ((MB025='M' AND TB039 LIKE '1%') OR TB039 LIKE '6%' OR TB039 LIKE '7%')
                                AND TB013 != '600102'";
            mssql.SQLexcute(connYF, string.Format(sqlStr, dtp));
        }

        /// <summary>
        /// 主营业务收入(电商)
        /// </summary>
        private void SetData3()
        {
            string sqlStr = @"UPDATE ACRTB SET TB013='600103' 
                                FROM ACRTB INNER JOIN ACRTA ON TB001=TA001 AND TB002=TA002 
                                INNER JOIN INVMB ON TB039=MB001
                                WHERE SUBSTRING(TA038,1,6)='{0}' AND TB001='6104' AND ((MB025='M' AND TB039 LIKE '1%') OR TB039 LIKE '6%' OR TB039 LIKE '7%')
                                AND TB013 != '600103'";
            mssql.SQLexcute(connYF, string.Format(sqlStr, dtp));
        }

        /// <summary>
        /// 主营业务成本(内销)
        /// </summary>
        private void SetData4()
        {
            string sqlStr = @"UPDATE AJSTB SET TB005='640102' 
                                FROM AJSTB LEFT JOIN AJSTA ON TB001=TA001 AND TB002=TA002 WHERE (AJSTA.TA001 like '6103%')
                                AND (AJSTA.TA014 = 'N') AND (AJSTA.TA006 >= '{0}01') AND (AJSTB.TB005 = '640101') 
                                AND TB005 != '640102'";
            mssql.SQLexcute(connYF, string.Format(sqlStr, dtp));
        }

        /// <summary>
        /// 主营业务成本(电商)
        /// </summary>
        private void SetData5()
        {
            string sqlStr = @"UPDATE AJSTB SET TB005='640103' 
                                FROM AJSTB LEFT JOIN AJSTA ON TB001=TA001 AND TB002=TA002 WHERE (AJSTA.TA001 like '6104%')
                                AND (AJSTA.TA014 = 'N') AND (AJSTA.TA006 >= '{0}01') AND (AJSTB.TB005 = '640101')
                                AND TB005 != '640102'";
            mssql.SQLexcute(connYF, string.Format(sqlStr, dtp));
        }

        #endregion
    }
}
