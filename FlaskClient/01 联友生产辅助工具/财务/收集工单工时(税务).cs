using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HarveyZ.财务
{
    public partial class 收集工单工时税务 : Form
    {
        private string connSW = FormLogin.infObj.connSW;
        private Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        private string dtp = "";

        public 收集工单工时税务(string text = "")
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
        #endregion

        #region 逻辑
        private void GetDtp()
        {
            dtp = dateTimePicker1.Value.ToString("yyyyMM");
        }

        /// <summary>
        /// 收集工单工时
        /// </summary>
        private void SetData1()
        {
            string sqlStr = @"DECLARE @NIANYUE VARCHAR(6)
                                SET @NIANYUE='{0}'

                                DELETE FROM Comfortseating.dbo.CSTMB WHERE SUBSTRING(MB002,1,6)=@NIANYUE
                                INSERT INTO 
                                Comfortseating.dbo.CSTMB(COMPANY,CREATOR,USR_GROUP,CREATE_DATE,MODIFIER,MODI_DATE,FLAG,
                                MB001,MB002,MB003,MB004,MB005,MB006,MB007,MB008,MB009,MB010,MB011,MB012,MB013,
                                UDF01,UDF02,UDF03,UDF04,UDF05,UDF06,UDF07,UDF08,UDF09,UDF10,UDF11,UDF12,UDF51,UDF52,
                                UDF53,UDF54,UDF55,UDF56,UDF57,UDF58,UDF59,UDF60,UDF61,UDF62 )

                                select 'comfortwx','DS','','','','',1,
                                MOCTA.TA021,CSTTA.TA002+'01',CSTTA.TA003,CSTTA.TA004,CSTTA.TA015,0,MOCTA.TA006,'','','',0,0,0,
                                '','','','','','','','','','','','',
                                0,0,0,0,0,0,0,0,0,0,0,0 
                                from Comfortseating.dbo.CSTTA CSTTA 
                                INNER JOIN Comfortseating.dbo.MOCTA MOCTA ON CSTTA.TA003=MOCTA.TA001 AND CSTTA.TA004=MOCTA.TA002 
                                WHERE CSTTA.TA002=@NIANYUE ";
            mssql.SQLexcute(connSW, string.Format(sqlStr, dtp));
        }
        #endregion
    }
}
