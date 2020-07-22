using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ
{
    #region 采购进货单_ERP
    class ERP_Create_Purtg
    {
        #region 私有变量
        private Mssql mssql = new Mssql();
        private string conn = null;
        #endregion

        #region 对象类
        private class HeadObject
        {
            private string flowId = null;
            private string company = null;
            private string uid = null;
            private string ugroup = null;
            private string time = null;

            private string tg001 = null; // 单别
            private string tg002 = null; // 单号
            private string tg003 = null; // 进货日期
            private string tg004 = "01"; // 工厂编号
            private string tg005 = null; // 供应商编号
            private string tg006 = null; // 送货单号
            private string tg007 = null; // 币种
            private string tg008 = null; // 汇率
            private string tg009 = null; // 发票种类
            private string tg010 = null; // 税种
            private string tg013 = "N"; // 审核码
            private string tg014 = null;  // 单据日期=进货日期
            private string tg015 = "N";  // 更新码
            private string tg021 = null;  // 供应商全称
            private string tg030 = null;  // 增值税率
            private string tg033 = null;  // 付款条件编号

            public string FlowId { get { return flowId; } set { flowId = value; } }
            public string Company { get { return company; } set { company = value; } }
            public string Uid { get { return uid; } set { uid = value; } }
            public string Ugroup { get { return ugroup; } set { ugroup = value; } }
            public string Time { get { return time; } set { time = value; } }

            public string TG001 { get { return tg001; } set { tg001 = value; } }
            public string TG002 { get { return tg002; } set { tg002 = value; } }
            public string TG003 { get { return tg003; } set { tg003 = value; } }
            public string TG004 { get { return tg004; } }
            public string TG005 { get { return tg005; } set { tg005 = value; } }
            public string TG006 { get { return tg006; } set { tg006 = value; } }
            public string TG007 { get { return tg007; } set { tg007 = value; } }
            public string TG008 { get { return tg008; } set { tg008 = value; } }
            public string TG009 { get { return tg009; } set { tg009 = value; } }
            public string TG010 { get { return tg010; } set { tg010 = value; } }
            public string TG013 { get { return tg013; } }
            public string TG014 { get { return tg014; } set { tg014 = value; } }
            public string TG015 { get { return tg015; } }
            public string TG021 { get { return tg021; } set { tg021 = value; } }
            public string TG030 { get { return tg030; } set { tg030 = value; } }
            public string TG033 { get { return tg033; } set { tg033 = value; } }
        }

        private class DetailObject
        {
            private int rowIndex = 0; //写入行序号

            private float total = 0; //需入库数量

            private DataTable lsDt = null;
            private DataTable slDt = null;

            private string th003 = null; //序号
            private string th004 = null; //品号
            private string th005 = null; //品名
            private string th006 = null; //规格
            private string th007 = null; //进货数量
            private string th008 = null; //单位
            private string th009 = null; //仓库
            private string th010 = null; //批号
            private string th011 = null; //采购单别
            private string th012 = null; //采购单号
            private string th013 = null; //采购序号
            private string th014 = null; //验收日期
            private string th015 = null; //验收数量
            private string th016 = null; //计价数量
            private string th018 = null; //原币单位今后价
            private string th019 = null; //原币进货金额
            private string th027 = "N"; //超期码
            private string th033 = null; //备注 订单信息
            private string th034 = null; //验收库存数量
            private string th035 = null; //小单位
            private string th042 = null; //项目编号
            private string th064 = null; //计价单温
            private string th065 = null; //库存单位
            private string thc02 = null; //类型

            public int RowIndex { get { return rowIndex; } set { rowIndex = value; } }
            public float Total { get { return total; } set { total = value; } }
            public DataTable LsDt { get { return lsDt; } set { lsDt = value; } }
            public DataTable SlDt { get { return slDt; } set { slDt = value; } }

            public string TH003 { get { return th003; } set { th003 = value; } }
            public string TH004 { get { return th004; } set { th004 = value; } }
            public string TH005 { get { return th005; } set { th005 = value; } }
            public string TH006 { get { return th006; } set { th006 = value; } }
            public string TH007 { get { return th007; } set { th007 = value; } }
            public string TH008 { get { return th008; } set { th008 = value; } }
            public string TH009 { get { return th009; } set { th009 = value; } }
            public string TH010 { get { return th010; } set { th010 = value; } }
            public string TH011 { get { return th011; } set { th011 = value; } }
            public string TH012 { get { return th012; } set { th012 = value; } }
            public string TH013 { get { return th013; } set { th013 = value; } }
            public string TH014 { get { return th014; } set { th014 = value; } }
            public string TH015 { get { return th015; } set { th015 = value; } }
            public string TH016 { get { return th016; } set { th016 = value; } }
            public string TH018 { get { return th018; } set { th018 = value; } }
            public string TH019 { get { return th019; } set { th019 = value; } }
            public string TH027 { get { return th027; } }
            public string TH033 { get { return th033; } set { th033 = value; } }
            public string TH034 { get { return th034; } set { th034 = value; } }
            public string TH035 { get { return th035; } set { th035 = value; } }
            public string TH042 { get { return th042; } set { th042 = value; } }
            public string TH064 { get { return th064; } set { th064 = value; } }
            public string TH065 { get { return th065; } set { th065 = value; } }
            public string THC02 { get { return thc02; } set { thc02 = value; } }

        }
        #endregion

        #region 主方法
        public ERP_Create_Purtg(string conn)
        {
            this.conn = conn;
        }
        
        public string HandelDef(string flowID)
        {
            HeadObject headObj = new HeadObject();
            headObj.FlowId = flowID;

            GetHeadInfo(headObj);
            GetHeadTG002(headObj);
            GetHeadTime(headObj);
            SetHeadInfo(headObj);
            UptHeadInfo2(headObj);

            SetDetailDef(headObj);
            UptDetailMoney(headObj);
            UptHeadMoney(headObj);
            UptJHXAInfo(headObj);
            return headObj.TG001 + '-' + headObj.TG002;
        }

        public string HandelDef(DataTable dt)
        {
            return null;
        }
        #endregion

        #region 业务逻辑
        private void GetHeadTime(HeadObject headObj)
        {
            string sqlstr = "select dbo.f_getTime(1)";
            DataTable dt = mssql.SQLselect(conn, sqlstr);
            if (dt != null)
            {
                headObj.Time = dt.Rows[0][0].ToString();
            }
        }

        private void GetHeadInfo(HeadObject headObj)
        {
            string sqlstr = @"SELECT DISTINCT RTRIM(JHXA.COMPANY) 公司别, RTRIM(JHXA.CREATOR) 创建人, RTRIM(JHXA.USR_GROUP) 用户组, 
                                RTRIM(JHXA001) 进货单别, RTRIM(JHXA004) 进货日期, RTRIM(JHXA002) 供应商编号, RTRIM(JHXA013) 送货单号, 
                                RTRIM(MA021) 交易币种, MG2.MG003 汇率, 
                                RTRIM(MA030) 发票种类, 
                                (CASE WHEN NOT (TC018 = '' OR TC018 IS NULL) THEN TC018 
                                ELSE (CASE WHEN MA044 ='' OR MA044 IS NULL THEN '1' ELSE MA044 END ) END) AS TC018C, 
                                RTRIM(MA003) 供应商全称, 
                                (CASE WHEN TC026 IS NULL THEN MA064 ELSE TC026 END) AS TC026C, 
                                (CASE WHEN TC027 = '' OR TC027 IS NULL THEN MA055 ELSE TC027 END) AS TC027C 
                                FROM dbo.JH_LYXA AS JHXA 
                                LEFT JOIN dbo.PURTC AS PURTC ON 1=2 
                                LEFT JOIN dbo.INVMB AS INVMB ON MB001=JHXA007 
                                LEFT JOIN dbo.PURMA AS PURMA ON MA001=JHXA002 
                                LEFT JOIN (SELECT CMSMG.MG003, CMSMG.MG001 FROM dbo.CMSMG 
                                INNER JOIN (SELECT MAX(MG002) MAXMG02, MG001 MAXMG01 FROM dbo.CMSMG GROUP BY MG001) AS MG 
                                ON MG.MAXMG01 = CMSMG.MG001 AND MG.MAXMG02 = CMSMG.MG002) AS MG2 ON MG2.MG001 = MA021 
                                WHERE JHXA005 IN ('{0}') AND JHXA011 = 'N' ";
            DataTable dt = mssql.SQLselect(conn, string.Format(sqlstr, headObj.FlowId));
            if (dt != null)
            {
                headObj.Company = dt.Rows[0][0].ToString();
                headObj.Uid = dt.Rows[0][1].ToString();
                headObj.Ugroup = dt.Rows[0][2].ToString();
                headObj.TG001 = dt.Rows[0][3].ToString();
                headObj.TG003 = dt.Rows[0][4].ToString();
                headObj.TG005 = dt.Rows[0][5].ToString();
                headObj.TG006 = dt.Rows[0][6].ToString();
                headObj.TG007 = dt.Rows[0][7].ToString();
                headObj.TG008 = dt.Rows[0][8].ToString();
                headObj.TG009 = dt.Rows[0][9].ToString();
                headObj.TG010 = dt.Rows[0][10].ToString();
                headObj.TG014 = headObj.TG003;
                headObj.TG021 = dt.Rows[0][11].ToString();
                headObj.TG030 = dt.Rows[0][12].ToString();
                headObj.TG033 = dt.Rows[0][13].ToString();
            }
        }

        private void GetHeadTG002(HeadObject headObj)
        {
            string sqlstr = "EXEC dbo.P_GETDH '{0}'";
            DataTable dt = mssql.SQLselect(conn, string.Format(sqlstr, headObj.TG001));
            if (dt != null)
            {
                headObj.TG002 = dt.Rows[0][0].ToString();
            }
        }

        // 写入单头
        private void SetHeadInfo(HeadObject headObj)
        {
            string sqlstr = @"INSERT INTO dbo.PURTG (COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, TG001, TG002, TG003, TG004, TG005, 
                                TG006, TG007, TG008, TG009, TG010, TG013, TG014, TG015, TG021, TG030, TG033, TG016, TG043, TG052) 
                                VALUES('{0}', '{1}', '{2}', '{3}', '1', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', 
                                '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '', '', '' )";
            mssql.SQLexcute(conn, string.Format(sqlstr, headObj.Company, headObj.Uid, headObj.Ugroup, headObj.Time, headObj.TG001,
                headObj.TG002, headObj.TG003, headObj.TG004, headObj.TG005, headObj.TG006, headObj.TG007, headObj.TG008, headObj.TG009,
                headObj.TG010, headObj.TG013, headObj.TG014, headObj.TG015, headObj.TG021, headObj.TG030, headObj.TG033));
        }

        private void SetDetailDef(HeadObject headObj)
        {
            SetDetailDefaultDef(headObj);
        }

        private void SetDetailDefaultDef(HeadObject headObj)
        {
            DetailObject detailObj = new DetailObject();
            GetLsDt(headObj, detailObj);
            if (detailObj.LsDt != null)
            {
                for (int lsRowIndex = 0; lsRowIndex < detailObj.LsDt.Rows.Count; lsRowIndex++)
                {
                    try
                    {
                        detailObj.TH004 = detailObj.LsDt.Rows[lsRowIndex][0].ToString();
                        detailObj.TH005 = detailObj.LsDt.Rows[lsRowIndex][1].ToString();
                        detailObj.TH006 = detailObj.LsDt.Rows[lsRowIndex][2].ToString();
                        detailObj.TH008 = detailObj.LsDt.Rows[lsRowIndex][3].ToString();
                        detailObj.TH009 = detailObj.LsDt.Rows[lsRowIndex][4].ToString();
                        detailObj.Total = float.Parse(detailObj.LsDt.Rows[lsRowIndex][5].ToString());

                        detailObj.TH010 = headObj.TG005;
                        detailObj.TH014 = headObj.TG003;
                        GetSlDt(headObj, detailObj);
                        GetNumber(headObj, detailObj);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
            }

        }

        //根据流水号，获获取临时表的明细
        private void GetLsDt(HeadObject headObj, DetailObject detailObj)
        {
            string sqlstr = @"SELECT RTRIM(MB001), RTRIM(MB002), RTRIM(MB003), RTRIM(MB004), 
                                RTRIM(JHXA003), RTRIM(JHXA009) FROM INVMB 
                                INNER JOIN dbo.JH_LYXA ON JHXA007 = MB001 WHERE 1=1 AND JHXA005 = '{0}' ORDER BY ID";
            detailObj.LsDt = mssql.SQLselect(conn, string.Format(sqlstr, headObj.FlowId));
        }

        //可进货采购单明细逻辑
        private void GetSlDt(HeadObject headObj, DetailObject detailObj)
        {
            string sqlstr = @"SELECT DISTINCT TOP 200 TD008 - TD015 - ( SELECT isnull( SUM ( TH007 ), 0 ) 
                                FROM dbo.PURTH(NOLOCK) AS PURTH 
                                INNER JOIN dbo.PURTG(NOLOCK) AS PURTG ON TG001 = TH001 AND TG002 = TH002
                                WHERE TH011 = TD001 AND TH012 = TD002 AND TH013 = TD003 
                                AND TG013 = 'N'
                                ) AS WJL, 
                                TD001 AS TH011, RTRIM(TD002) AS TH012, TD003 AS TH013, (CASE WHEN TD010 IS NULL THEN 0 ELSE TD010 END) AS TH018, TD014 AS TH033, RTRIM(TD020) AS TH035, 
                                RTRIM(TD022) AS TH042, RTRIM(TDC03) AS THC02, TC003, TD012 
                                FROM dbo.PURTD(NOLOCK) AS PURTD 
                                LEFT JOIN .dbo.PURTC(NOLOCK) AS PURTC ON TC001 = TD001 AND TC002 = TD002 
                                WHERE TC004 = '{0}' AND TD004 = '{1}' 
                                AND (TD008 - TD015 - ( SELECT isnull( SUM ( TH007 ), 0 ) FROM dbo.PURTH PURTH 
                                WHERE TH011 = TD001 AND TH012 = TD002 AND TH013 = TD003 AND TH030 = 'N' )) > 0 
                                AND TD016 = 'N' AND TC014 = 'Y' AND TC001 <> '3305' AND TC001 <> '3306' 
                                ORDER BY TC003 DESC, TD012, TD001 DESC, RTRIM(TD002), TD003";
            detailObj.SlDt = mssql.SQLselect(conn, string.Format(sqlstr, headObj.TG005, detailObj.TH004));
        }

        //数量
        private void GetNumber(HeadObject headObj, DetailObject detailObj)
        {
            if (detailObj.SlDt != null && detailObj.SlDt.Rows.Count != 0)
            {
                detailObj.RowIndex++;

                detailObj.TH011 = detailObj.SlDt.Rows[0][1].ToString();
                detailObj.TH012 = detailObj.SlDt.Rows[0][2].ToString();
                detailObj.TH013 = detailObj.SlDt.Rows[0][3].ToString();
                detailObj.TH018 = Math.Round(float.Parse(detailObj.SlDt.Rows[0][4].ToString()), 6, MidpointRounding.AwayFromZero).ToString();
                detailObj.TH033 = detailObj.SlDt.Rows[0][5].ToString();
                detailObj.TH035 = detailObj.SlDt.Rows[0][6].ToString();
                detailObj.TH042 = detailObj.SlDt.Rows[0][7].ToString();
                detailObj.THC02 = detailObj.SlDt.Rows[0][8].ToString();

                if (float.Parse(detailObj.SlDt.Rows[0][0].ToString()) >= detailObj.Total)
                {
                    detailObj.TH007 = Math.Round(detailObj.Total, 6, MidpointRounding.AwayFromZero).ToString();
                    SetDetailInfo(headObj, detailObj);
                }
                else
                {
                    float sl = float.Parse(detailObj.SlDt.Rows[0][0].ToString());
                    detailObj.TH007 = Math.Round(sl, 6, MidpointRounding.AwayFromZero).ToString();
                    detailObj.Total -= sl;
                    SetDetailInfo(headObj, detailObj);
                    detailObj.SlDt.Rows.RemoveAt(0);
                    GetNumber(headObj, detailObj);
                }
            }
        }

        //写入单身信息
        private void SetDetailInfo(HeadObject headObj, DetailObject detailObj)
        {
            detailObj.TH015 = detailObj.TH007;
            detailObj.TH016 = detailObj.TH007;
            detailObj.TH034 = detailObj.TH007;
            detailObj.TH064 = detailObj.TH008;
            detailObj.TH065 = detailObj.TH008;
            detailObj.TH019 = Math.Round(float.Parse(detailObj.TH007) * float.Parse(detailObj.TH018), 6, MidpointRounding.AwayFromZero).ToString();
            detailObj.TH003 = detailObj.RowIndex.ToString().PadLeft(4, '0');

            string sqlstr = @"INSERT INTO dbo.PURTH(COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG, 
                                TH001,TH002,TH003,TH004,TH005,TH006,TH007,TH008,TH009,TH010, 
                                TH011,TH012,TH013,TH014,TH015,TH016,TH018,TH019,TH026,TH027, 
                                TH029,TH030,TH031,TH032,TH033,TH034,TH035,TH042,TH043,TH044, 
                                TH060,TH064,TH065,TH071,TH072,THC02) 
                                VALUES('{0}','{1}','{2}','{3}',1,'{4}','{5}','{6}', 
                                '{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}', 
                                '{18}','{19}','{20}','{21}','N','{22}','N','N','N','N','{23}', 
                                '{24}','{25}','{26}','N','N','0','{27}','{28}','1','##########','{29}')";
            mssql.SQLexcute(conn, string.Format(sqlstr, headObj.Company, headObj.Uid, headObj.Ugroup, headObj.Time, headObj.TG001, headObj.TG002,
                detailObj.TH003, detailObj.TH004, detailObj.TH005, detailObj.TH006, detailObj.TH007, detailObj.TH008, detailObj.TH009, detailObj.TH010,
                detailObj.TH011, detailObj.TH012, detailObj.TH013, detailObj.TH014, detailObj.TH015, detailObj.TH016, detailObj.TH018, detailObj.TH019,
                detailObj.TH027, detailObj.TH033, detailObj.TH034, detailObj.TH035, detailObj.TH042, detailObj.TH064, detailObj.TH065, detailObj.THC02));
        }

        //更新单头税率，汇率 
        private void UptHeadInfo2(HeadObject headObj)
        {
            string sqlstr = @"UPDATE dbo.PURTG SET TG008 = ISNULL(MG003, 1) FROM dbo.PURTG 
                            LEFT JOIN( 
                                SELECT G.MG001, G.MG003 FROM(
                                    SELECT MG001, MAX(MG002) MG002 FROM CMSMG
                                    WHERE 1 = 1
                                    AND MG002 <= LEFT(dbo.f_getTime(1), 8)
                                    GROUP BY MG001
                                ) AS K LEFT JOIN dbo.CMSMG AS G ON K.MG001 = G.MG001 AND K.MG002 = G.MG002
                            ) AS A ON TG007 = A.MG001
                            WHERE TG001 = '{0}' AND TG002 = '{1}'

                            UPDATE dbo.PURTG SET TG030 = ISNULL(MA004, 0) FROM COMFORT.dbo.PURTG LEFT JOIN dbo.CMSMA ON CMSMA.COMPANY = 'COMFORT' 
                            WHERE TG001 = '{0}' AND TG002 ='{1}'  ";
            mssql.SQLexcute(conn, string.Format(sqlstr, headObj.TG001, headObj.TG002));
        }

        //更新单身金额信息
        private void UptDetailMoney(HeadObject headObj)
        {
            string sqlstr = @"UPDATE dbo.PURTH  SET 
                                TH045 = CAST(ROUND(TH019/(1+CONVERT(FLOAT, TG030)),2) AS  NUMERIC(10,2)), 
                                TH046 = CAST(ROUND(TH019 - (TH019/(1+CONVERT(FLOAT, TG030))),2) AS  NUMERIC(10,2)), 
                                TH047 = CAST(ROUND((TH019 * CONVERT(FLOAT, TG008)/(1+CONVERT(FLOAT, TG030))),2) 
                                AS  NUMERIC(10,2)), 
                                TH048 = CAST(ROUND((TH019 * CONVERT(FLOAT, TG008)) - 
                                (TH019 * CONVERT(FLOAT, TG008)/(1+CONVERT(FLOAT, TG030))),2) AS  NUMERIC(10,2)) 
                                FROM PURTH INNER JOIN dbo.PURTG AS PURTG ON TG001 = TH001 AND TG002 = TH002 
                                WHERE TG001= '{0}' AND TG002= '{1}' ";
            mssql.SQLexcute(conn, string.Format(sqlstr, headObj.TG001, headObj.TG002));
        }

        //更新单头金额信息
        private void UptHeadMoney(HeadObject headObj)
        {
            string sqlstr = @"UPDATE A SET TG017=SUMTH019,TG019=SUMTH046,TG026=SUMTH015,TG028=SUMTH045,TG031=SUMTH047, 
                                TG032=SUMTH048,TG040=SUMTH050,TG041=SUMTH052,TG053=SUMTH007,TG054=SUMTH049 
                                FROM COMFORT.dbo.PURTG A 
                                INNER JOIN (SELECT TH001,TH002,SUMTH019=SUM(TH019),SUMTH046=SUM(TH046), 
                                SUMTH007=SUM(CASE WHEN MA024='2' THEN FLOOR(TH007) ELSE TH007 END), 
                                SUMTH015=SUM(CASE WHEN MA024='2' THEN FLOOR(TH015) ELSE TH015 END),SUMTH045=SUM(TH045), 
                                SUMTH047=SUM(TH047),SUMTH048=SUM(TH048),SUMTH050=SUM(TH050),SUMTH052=SUM(TH052), 
                                SUMTH049=SUM(TH049) 
                                FROM dbo.PURTH 
                                INNER JOIN dbo.CMSMA ON 1=1 
                                GROUP BY TH001,TH002)  AS B ON A.TG001=B.TH001 AND A.TG002=B.TH002 
                                WHERE TG001= '{0}' AND TG002= '{1}' ";
            mssql.SQLexcute(conn, string.Format(sqlstr, headObj.TG001, headObj.TG002));
        }

        private void UptJHXAInfo(HeadObject headObj)
        {
            string time = mssql.SQLselect(conn, "select dbo.f_getTime(1)").Rows[0][0].ToString();
            string sqlstr = @"UPDATE dbo.JH_LYXA SET MODIFIER='{1}', MODI_DATE='{2}', 
                                FLAG=(convert(int,dbo.JH_LYXA.FLAG))%999+1, JHXA011 = 'Y', UDF01 = '{3}'WHERE  JHXA005 = '{0}' ";
            mssql.SQLexcute(conn, string.Format(sqlstr, headObj.FlowId, headObj.Uid, time, headObj.TG001 + '-' + headObj.TG002));
        }
        #endregion
    }
    #endregion

    #region 采购退货单_ERP
    class ERP_Create_Purti
    {
        #region 私有变量
        private Mssql mssql = new Mssql();
        private string conn = null;
        #endregion

        #region 对象类
        class HeadObject
        {

        }

        class DetailObject
        {

        }
        #endregion

        #region 主方法
        public ERP_Create_Purti(string conn)
        {
            this.conn = conn;
        }

        public string HandelDef(string flowId)
        {
            return "";
        }

        public string HandelDef(DataTable dt)
        {
            return "";
        }
        #endregion

        #region 业务逻辑

        #endregion
    }
    #endregion

    #region 生产入库单_码垛线
    class ERP_Create_Moctf_Md
    {
        #region 私有变量
        private Mssql mssql = new Mssql();
        private string connYF = null;
        private string connMD = null;
        private HeadObject headObj = null;
        private DetailObject detailObj = null;
        #endregion

        #region 对象类
        class HeadObject
        {
            private string _company = "COMFORT";
            private string _creator = "Robot";
            private string _usr_group = "";
            private string _printId = "";
            private string _tg001 = "";
            private string _tg002 = "";
            private string _tf001 = "5803";
            private string _tf002 = "";
            private string _tf004 = "01";
            private string _tf005 = "";
            private string _tf006 = "N";
            private string _tf007 = "N";
            private int _tf008 = 0;
            private string _tf009 = "N";
            private string _tf010 = "N";
            private string _tf011 = "6";
            private string _tf014 = "N";
            private int _tf015 = 0;
            private string _tf016 = "080B";
            private string _tf033 = "";

            public string company { get { return _company; } }
            public string creator { get { return _creator; } }
            public string usr_group { get { return _usr_group; } set { _usr_group = value; } }
            public string printId { get { return _printId; } set { _printId = value; } }
            public string tf001 { get { return _tf001; } set { _tf001 = value; } }
            public string tf002 { get { return _tf002; } set { _tf002 = value; } }
            public string tg001 { get { return _tg001; } set { _tg001 = value; } }
            public string tg002 { get { return _tg002; } set { _tg002 = value; } }
            public string tf004 { get { return _tf004; } set { _tf004 = value; } }
            public string tf005 { get { return _tf005; } set { _tf005 = value; } }
            public string tf006 { get { return _tf006; } set { _tf006 = value; } }
            public string tf007 { get { return _tf007; } set { _tf007 = value; } }
            public int tf008 { get { return _tf008; } set { _tf008 = value; } }
            public string tf009 { get { return _tf009; } set { _tf009 = value; } }
            public string tf010 { get { return _tf010; } set { _tf010 = value; } }
            public string tf011 { get { return _tf011; } set { _tf011 = value; } }
            public string tf014 { get { return _tf014; } set { _tf014 = value; } }
            public int tf015 { get { return _tf015; } set { _tf015 = value; } }
            public string tf016 { get { return _tf016; } set { _tf016 = value; } }
            public string tf033 { get { return _tf033; } set { _tf033 = value; } }
        }

        class DetailObject
        {

        }
        #endregion

        #region 主方法
        public ERP_Create_Moctf_Md(string connYF, string connMD)
        {
            this.connYF = connYF;
            this.connMD = connMD;
            headObj = new HeadObject();
            detailObj = new DetailObject();
        }

        public void HandelDef()
        {
            GetPrintId();

            if (headObj.printId != null)
            {
                GetUsrGroup();
                GetTf002();
                //注释了的待修改
                InsertHead();
                InsertDetail();
                UpdateHeadSl();
                UpdateXhOutFlag();
            }
        }
        #endregion

        #region 业务逻辑
        private void GetPrintId()
        {
            string sqlstr = @"SELECT TOP 1 PrintId, TG001, TG002 FROM ROBOT_TEST.dbo.PrintData WHERE STATUSS = 0 AND XhOutFlag = 1 AND ScrkOutFlag = 0 ORDER BY Create_Date ";
            DataTable dt = mssql.SQLselect(connMD, sqlstr);
            if (dt != null)
            {
                if (dt.Rows[0]["TG002"].ToString() != "")
                {
                    headObj.printId = dt.Rows[0]["PrintId"].ToString();
                    headObj.tg001 = dt.Rows[0]["TG001"].ToString();
                    headObj.tg002 = dt.Rows[0]["TG002"].ToString();
                }
                else
                {
                    headObj.printId = null;
                    headObj.tg001 = null;
                    headObj.tg002 = null;
                }
            }
            else
            {
                headObj.printId = null;
                headObj.tg001 = null;
                headObj.tg002 = null;
            }
        }

        private void GetUsrGroup()
        {
            string sqlstr = @"SELECT isnull(RTRIM(MF004), '') FROM dbo.ADMMF WHERE MF001 = '{0}' ";
            DataTable dt = mssql.SQLselect(connYF, string.Format(sqlstr, headObj.creator));
            if (dt != null)
            {
                headObj.usr_group = dt.Rows[0][0].ToString();
            }
            else
            {
                headObj.usr_group = "";
            }
        }

        private void GetTf002()
        {
            string sqlstr = @"exec P_GETDH '{0}'";
            headObj.tf002 = mssql.SQLselect(connYF, string.Format(sqlstr, headObj.tf001)).Rows[0][0].ToString();
        }

        private void InsertHead()
        {
            string sqlstr = @"INSERT INTO dbo.MOCTF (COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, TF001, TF002, TF003, TF012, 
                                        TF004, TF005, TF006, TF007, TF008, TF009, TF010, TF011, TF013, TF014, TF015, TF016) 
			                    VALUES('{0}', '{1}','{2}', dbo.f_getTime(1), 1, '{3}', '{4}', LEFT(dbo.f_getTime(1), 8), LEFT(dbo.f_getTime(1), 8), 
                                        '01', '', 'N', 'N', 0, 'N', 'N', '{5}', '', 'N', 0, '{6}')";
            mssql.SQLexcute(connYF, string.Format(sqlstr, headObj.company, headObj.creator, headObj.usr_group, headObj.tf001, headObj.tf002, headObj.tf011, headObj.tf016));
        }

        private void InsertDetail()
        {
            string sqlstr = @"INSERT INTO dbo.MOCTG (COMPANY, CREATOR, CREATE_DATE, USR_GROUP, FLAG, TG001, TG002, TG003, TG004, TG005, TG006, TG007, TG009, TG010, TG011, TG013, TG014, TG015, TG016, TG017, TG020, 
                                TG022, TG023, TG024, TG031, TG035, TG036, TG037, TG038, TGC01)
                                SELECT MOCTF.COMPANY, MOCTF.CREATOR, MOCTF.CREATE_DATE, MOCTF.USR_GROUP, MOCTF.FLAG, TF001, TF002, RIGHT('0000' + CONVERT(VARCHAR(20), ROW_NUMBER() Over (ORDER BY TH003)), 4) AS TG003, 
	                                TH004 AS TG004, TH005 AS TG005, TH006 AS TG006, TH009 AS TG007, 1 AS TG009, TH007 AS TG010, TH008 AS TG011, TH008 AS TG013, TA001 AS TG014, TA002 AS TG015, '0' AS TG016, TH015 AS TG017, 
	                                '' AS TG020, 'N' AS TG022, 0 AS TG023, 'N' AS TG024, TA058 AS TG031, 'N' AS TG035, '##########' AS TG036, TH009 AS TG037, TH008 AS TG038, '2' AS TGC01
                                FROM dbo.COPTG 
                                INNER JOIN dbo.COPTH ON TG001 = TH001 AND TG002 = TH002 
                                INNER JOIN dbo.MOCTF ON TF001 = '{2}' AND TF002 = '{3}'
                                INNER JOIN 
                                (SELECT TA1.TA001, MIN(TA1.TA002) TA002, TA1.TA058, TA1.TA026, TA1.TA027, TA1.TA028 
                                FROM dbo.MOCTA AS TA1
                                WHERE TA1.TA001 = '5101' AND TA1.TA011 NOT IN ('Y', 'y') GROUP BY TA1.TA001, TA1.TA058, TA1.TA026, TA1.TA027, TA1.TA028) AS MOCTA
                                ON TA026 = TH014 AND TA027 = TH015 AND TA028 = TH016 
                                WHERE 1=1
                                AND COPTG.TG001 = '{0}' AND COPTG.TG002 = '{1}'";

            mssql.SQLexcute(connYF, string.Format(sqlstr, headObj.tg001, headObj.tg002, headObj.tf001, headObj.tf002));
        }

        private void UpdateHeadSl()
        {
            string sqlstr = @"UPDATE dbo.MOCTF SET TF023 = TF023S, TF024 = TF024S
                            FROM 
                            (SELECT TG001, TG002, SUM(TG011) AS TF023S, SUM(TG013) AS TF024S FROM dbo.MOCTG 
                            WHERE TG001 = '{0}' AND TG002 = '{1}'
                            GROUP BY TG001, TG002
                            ) AS A 
                            WHERE TG001 = TF001 AND TG002 = TF002";

            mssql.SQLexcute(connYF, string.Format(sqlstr, headObj.tf001, headObj.tf002));
        }

        private void UpdateXhOutFlag()
        {
            string sqlstr = @"UPDATE dbo.PrintData SET TF001 = '{1}', TF002 = '{2}', ScrkOutFlag = 1, ScrkOutDate = getdate() WHERE PrintId = '{0}' ";
            mssql.SQLexcute(connMD, string.Format(sqlstr, headObj.printId, headObj.tf001, headObj.tf002));
        }
        #endregion
    }
    #endregion

    #region 生产入库单_ERP
    class ERP_Create_Moctf
    {
        #region 私有变量
        private Mssql mssql = new Mssql();
        private string conn = null;
        #endregion

        #region 对象类
        class HeadObject
        {

        }

        class DetailObject
        {

        }
        #endregion

        #region 主方法
        public ERP_Create_Moctf(string conn)
        {
            this.conn = conn;
        }

        public string HandelDef(string flowId)
        {
            return "";
        }

        public string HandelDef(DataTable dt)
        {
            return "";
        }
        #endregion

        #region 业务逻辑

        #endregion
    }
    #endregion

    #region 销货单_码垛线
    class ERP_Create_Coptg_Md
    {
        #region 私有变量
        private Mssql mssql = new Mssql();
        private string connMD = null;
        private string connYF = null;
        private HeadObject headObj = null;
        private DetailObject detailObj = null;
        #endregion

        #region 对象类
        class HeadObject
        {
            private string _creator = "Robot";
            private string _company = "COMFORT";
            private string _usr_group = "";
            private string _printId = "";
            private string _tg001 = "";
            private string _tg002 = "";
            private string _tg004 = "";

            public string creator { get { return _creator; } }
            public string company { get { return _company; } }
            public string usr_group { get { return _usr_group; } set { _usr_group = value; } }
            public string printId { get { return _printId; } set { _printId = value; } }
            public string tg001 { get { return _tg001; } set { _tg001 = value; } }
            public string tg002 { get { return _tg002; } set { _tg002 = value; } }
            public string tg004 { get { return _tg004; } set { _tg004 = value; } }

        }

        class DetailObject
        {

        }
        #endregion

        #region 主方法
        public ERP_Create_Coptg_Md(string connYF, string connMD)
        {
            this.connYF = connYF;
            this.connMD = connMD;
            headObj = new HeadObject();
            detailObj = new DetailObject();
        }

        public void HandelDef()
        {
            GetPrintId();

            if(headObj.printId != null)
            {
                GetUsrGroup();
                GetTg002();
                InsertHead();
                UpdateHead();
                InsertDetail();
                UpdateDetail();
                UpdateDetailMoney();
                UpdateHeadMoney();
                UpdateXhOutFlag();
            }
        }
        #endregion

        #region 业务逻辑
        private void GetPrintId()
        {
            string sqlstr = @"SELECT TOP 1 PrintId, TG001, TG004 FROM ROBOT_TEST.dbo.PrintData WHERE STATUSS = 0 AND XhOutFlag = 0 ORDER BY Create_Date ";
            DataTable dt = mssql.SQLselect(connMD, sqlstr);
            if (dt != null)
            {
                if (dt.Rows[0]["TG004"].ToString() != "")
                {
                    headObj.printId = dt.Rows[0]["PrintId"].ToString();
                    headObj.tg001 = dt.Rows[0]["TG001"].ToString();
                    headObj.tg004 = dt.Rows[0]["TG004"].ToString();
                }
                else
                {
                    headObj.printId = null;
                    headObj.tg001 = null;
                    headObj.tg004 = null;
                }
            }
            else
            {
                headObj.printId = null;
                headObj.tg001 = null;
                headObj.tg004 = null;
            }
        }

        private void GetUsrGroup()
        {
            string sqlstr = @"SELECT isnull(RTRIM(MF004), '') FROM dbo.ADMMF WHERE MF001 = '{0}' ";
            DataTable dt = mssql.SQLselect(connYF, string.Format(sqlstr, headObj.creator));
            if (dt != null)
            {
                headObj.usr_group = dt.Rows[0][0].ToString();
            }
            else
            {
                headObj.usr_group = "";
            }
        }

        private void GetTg002()
        {
            string sqlstr = @"exec P_GETDH '{0}'";
            headObj.tg002 = mssql.SQLselect(connYF, string.Format(sqlstr, headObj.tg001)).Rows[0][0].ToString();
        }

        private void InsertHead()
        {
            string sqlstr = @"INSERT INTO COMFORT.dbo.COPTG (COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, TG001, TG002, TG003, TG042) 
			                    VALUES('{0}', '{1}','{2}', dbo.f_getTime(1), 1, '{3}', '{4}', LEFT(dbo.f_getTime(1), 8), LEFT(dbo.f_getTime(1), 8))";
            mssql.SQLexcute(connYF, string.Format(sqlstr, headObj.company, headObj.creator, headObj.usr_group, headObj.tg001, headObj.tg002));
        }

        private void UpdateHead()
        {
            string sqlstr1 = @"UPDATE dbo.COPTG SET TG010 = '01', TG020 = '', TG022 = 0, TG023 = 'N', 
			                    TG024 = 'N', TG031 = 'N', TG036 = 'N', TG037 = 'N' 
			                    WHERE TG001 = '{0}' AND TG002 = '{1}'";
            string sqlstr2 = @"UPDATE dbo.COPTG SET TG004 = MA001, TG005 = MA015, TG006 = MA016, 
			                    TG008 = MA027, 
			                    TG009 = MA064, 
			                    TG011 = MA014, 
			                    TG012 = MG004, 
			                    TG016 = MA037, 
			                    TG017 = MA038, 
			                    TG026 = MA085, 
			                    TG044 = MA101, 
			                    TG047 = MA083, 
			                    TG068 = MA113 
			                    FROM (
				                    SELECT COPMAC.*, ISNULL(COPTGC.MG004, 0) AS MG004 FROM dbo.COPMA AS COPMAC
				                    LEFT JOIN(
					                    SELECT MA2.MA001, MA2.MA021, MG.MG002, MG.MG004 FROM dbo.COPMA AS MA2 
					                    LEFT JOIN dbo.CMSMG AS MG ON MG.MG001 = MA2.MA014 
						                    AND MG002 = (SELECT MAX(MG2.MG002) FROM CMSMG AS MG2 WHERE MG2.MG001 = MG.MG001 AND CONVERT(FLOAT, MG2.MG002) <= CONVERT(FLOAT, CONVERT(VARCHAR(8), GETDATE(), 112)))
				                    ) AS COPTGC ON COPTGC.MA001 = COPMAC.MA001
				                    WHERE COPMAC.MA001 = '{2}'
			                    ) AS COPMA
			                    WHERE TG001 = '{0}' AND TG002 ='{1}' ";
            string sqlstr3 = @"UPDATE dbo.COPTG SET TG044 =(CASE WHEN TG017 IN ('3','4','9') THEN 0 ELSE ISNULL(MA004, 0) END) FROM dbo.COPTG 
                                LEFT JOIN dbo.CMSMA ON CMSMA.COMPANY ='{2}' WHERE TG001 = '{0}' AND TG002 = '{1}' ";

            mssql.SQLexcute(connYF, string.Format(sqlstr1, headObj.tg001, headObj.tg002));
            mssql.SQLexcute(connYF, string.Format(sqlstr2, headObj.tg001, headObj.tg002, headObj.tg004));
            mssql.SQLexcute(connYF, string.Format(sqlstr3, headObj.tg001, headObj.tg002, headObj.company));
        }

        private void InsertDetail()
        {
            string sqlstr = @"INSERT INTO dbo.COPTH (COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, TH001, TH002, TH003, TH008, TH014, TH015, TH016)
			SELECT '{1}' AS COMPANY, '{2}' AS CREATOR, '{3}' AS USR_GROUP, dbo.f_getTime(1) AS CREATE_DATE, 1 AS FLAG, 
			'{4}' AS TH001, '{5}' AS TH002, RIGHT('00000' + CONVERT(VARCHAR(20), ROW_NUMBER() Over (ORDER BY SC001)), 4) AS TH003, TH008, TH014, TH015, TH016
			FROM (SELECT SC001, SUBSTRING(SC001, 1, 4) AS TH014, 
			SUBSTRING(SUBSTRING(SC001, 6, LEN(SC001)), 1, CHARINDEX('-', SUBSTRING(SC001, 6, LEN(SC001))) - 1) AS TH015, 
			SUBSTRING(SUBSTRING(SC001, 6, LEN(SC001)), (CHARINDEX('-', SUBSTRING(SC001, 6, LEN(SC001))) + 1), LEN(SUBSTRING(SC001, 6, LEN(SC001)))) AS TH016, 
			COUNT(SC001) AS TH008
			FROM [192.168.0.198].ROBOT_TEST.dbo.PdData AS PdData
			WHERE PrintId = '{0}'
			AND Pd_Sta = 'OK'
			GROUP BY SC001, SUBSTRING(SC001, 1, 4), 
			SUBSTRING(SUBSTRING(SC001, 6, LEN(SC001)), 1, CHARINDEX('-', SUBSTRING(SC001, 6, LEN(SC001))) - 1), 
			SUBSTRING(SUBSTRING(SC001, 6, LEN(SC001)), (CHARINDEX('-', SUBSTRING(SC001, 6, LEN(SC001))) + 1), LEN(SUBSTRING(SC001, 6, LEN(SC001)))) 
			) AS PdData2";

            mssql.SQLexcute(connYF, string.Format(sqlstr, headObj.printId, headObj.company, headObj.creator, headObj.usr_group, headObj.tg001, headObj.tg002));
        }

        private void UpdateDetail()
        {
            string sqlstr = @"UPDATE dbo.COPTH SET 
			                    COPTH.CREATE_DATE = COPTG.CREATE_DATE,
			                    TH004 = TD004, 
			                    TH005 = TD005, 
			                    TH006 = TD006, 
			                    TH007 = TD007,
			                    TH009 = TD010, 
			                    TH012 = TD011, 
			                    TH017 = TH015, 
			                    TH018 = TD020, 
			                    TH019 = TD014, 
			                    TH020 = 'N', 
			                    TH021 = 'N', 
			                    TH025 = TD026, 
			                    TH026 = 'N', 
			                    TH031 = '1', 
			                    TH048 = TD037, 
			                    TH049 = TD042, 
			                    TH050 = TD043, 
			                    TH055 = '', 
			                    TH056 = '##########', 
			                    TH063 = TD061, 
			                    TH064 = TD062, 
			                    COPTH.UDF01 = COPTD.UDF01, 
			                    COPTH.UDF03 = TQ003, 
			                    COPTH.UDF04 = COPTD.UDF08, 
			                    COPTH.UDF05 = TD053, 
			                    COPTH.UDF10 = COPTD.UDF10 
		
			                    FROM dbo.COPTH AS COPTH
			                    INNER JOIN dbo.COPTG ON TG001 = TH001 AND TG002 = TH002 
			                    LEFT JOIN dbo.COPTD AS COPTD ON TH014 = TD001 AND TH015 = TD002 AND TH016 = TD003
			                    LEFT JOIN dbo.COPTQ AS COPTQ ON TQ001 = TD004 AND TQ002 = TD053
			                    WHERE 1=1
			                    AND TH001 = '{0}' AND TH002 = '{1}' ";
            mssql.SQLexcute(connYF, string.Format(sqlstr, headObj.tg001, headObj.tg002));
        }

        private void UpdateDetailMoney()
        {
            string sqlstr = @"UPDATE dbo.COPTH SET 
			                    TH013 = TH013C, 
			                    TH035 = TH035C, 
			                    TH036 = TH036C, 
			                    TH037 = TH037C, 
			                    TH038 = TH038C 
			                    FROM (
				                    SELECT TG001 AS TG001C, TG002 AS TG002C, TH003 AS TH003C, 
				                    (CASE 
					                    WHEN TG017 = '1' THEN ROUND(TH008 * TH012 * TH025, 2) 
					                    WHEN TG017 = '2' THEN ROUND(TH008 * TH012 * TH025, 2) 
					                    WHEN TG017 IN ('3', '4', '9') THEN ROUND(TH008 * TH012 * TH025, 2) END) TH013C, 
				                    (CASE 
					                    WHEN TG017 = '1' THEN ROUND(TH008 * TH012 * TH025 / (1 + TG044), 2) 
					                    WHEN TG017 = '2' THEN ROUND(TH008 * TH012 * TH025, 2)  
					                    WHEN TG017 IN ('3', '4', '9') THEN ROUND(TH008 * TH012 * TH025, 2) END) TH035C, 
				                    (CASE 
					                    WHEN TG017 = '1' THEN ROUND(TH008 * TH012 * TH025, 2) - ROUND(TH008 * TH012 * TH025 / (1 + TG044), 2) 
					                    WHEN TG017 = '2' THEN ROUND(TH008 * TH012 * TH025 * TG044, 2) 
					                    WHEN TG017 IN ('3', '4', '9') THEN 0 END) TH036C, 
				                    (CASE 
					                    WHEN TG017 = '1' THEN ROUND(ROUND(TH008 * TH012 * TH025 / (1 + TG044), 2) * TG012, 2) 
					                    WHEN TG017 = '2' THEN ROUND(ROUND(TH008 * TH012 * TH025, 2) * TG012, 2) 
					                    WHEN TG017 IN ('3', '4', '9') THEN ROUND(TH008 * TH012 * TG012 * TH025, 2) END) TH037C, 
				                    (CASE 
					                    WHEN TG017 = '1' THEN ROUND((ROUND(TH008 * TH012 * TH025, 2) - ROUND(TH008 * TH012 * TH025 / (1 + TG044), 2)) * TG012, 2) 
					                    WHEN TG017 = '2' THEN ROUND(TH008 * TH012 * TH025 * TG044 * TG012, 2)  
					                    WHEN TG017 IN ('3', '4', '9') THEN 0 END) TH038C 
				                    FROM COPTH 
				                    INNER JOIN dbo.COPTG ON TG001 = TH001 AND TG002 = TH002
				                    WHERE TG001 = '{0}' AND TG002 = '{1}'
			                    ) AS A0
			                    WHERE TH001 = TG001C AND TH002 = TG002C AND TH003 = TH003C ";
            mssql.SQLexcute(connYF, string.Format(sqlstr, headObj.tg001, headObj.tg002));
        }

        private void UpdateHeadMoney()
        {
            string sqlstr = @"UPDATE dbo.COPTG SET 
			                    TG033 = TG033S, 
			                    TG045 = TG045S, 
			                    TG013 = TG013S, 
			                    TG046 = TG046S, 
			                    TG025 = TG025S 
			                    FROM 
			                    (SELECT TH001, TH002, SUM(TH008) AS TG033S, SUM(TH037) AS TG045S, SUM(TH035) AS TG013S, SUM(TH038) AS TG046S, SUM(TH036) AS TG025S FROM dbo.COPTH 
			                    WHERE TH001 = '{0}' AND TH002 = '{1}'
			                    GROUP BY TH001, TH002
			                    ) AS A 
			                    WHERE TG001 = TH001 AND TG002 = TH002";

            mssql.SQLexcute(connYF, string.Format(sqlstr, headObj.tg001, headObj.tg002));
        }

        private void UpdateXhOutFlag()
        {
            string sqlstr = @"UPDATE dbo.PrintData SET TG001 = '{1}', TG002 = '{2}', XhOutFlag = 1, XhOutDate = getdate() WHERE PrintId = '{0}' ";
            mssql.SQLexcute(connMD, string.Format(sqlstr, headObj.printId, headObj.tg001, headObj.tg002));
        }
        #endregion
    }
    #endregion

    #region 销货单_ERP
    class ERP_Create_Coptg
    {
        #region 私有变量
        private Mssql mssql = new Mssql();
        private string conn = null;
        #endregion

        #region 对象类
        class HeadObject
        {

        }

        class DetailObject
        {

        }
        #endregion

        #region 主方法
        public ERP_Create_Coptg(string conn)
        {
            this.conn = conn;
        }

        public string HandelDef(string flowId)
        {
            return "";
        }

        public string HandelDef(DataTable dt)
        {
            return "";
        }
        #endregion

        #region 业务逻辑

        #endregion
    }
    #endregion

    #region 客户订单_分公司
    class ERP_Create_Coptc_Fgs
    {
        #region 私有变量
        private Mssql mssql = new Mssql();
        private string conn = null;
        #endregion

        #region 对象类
        class HeadObject
        {

        }

        class DetailObject
        {

        }
        #endregion

        #region 主方法
        public ERP_Create_Coptc_Fgs(string conn)
        {
            this.conn = conn;
        }

        public string HandelDef(string flowId)
        {
            return "";
        }

        public string HandelDef(DataTable dt)
        {
            return "";
        }
        #endregion

        #region 业务逻辑

        #endregion
    }
    #endregion
}
