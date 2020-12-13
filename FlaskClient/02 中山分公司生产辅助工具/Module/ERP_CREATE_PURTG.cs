using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ
{
    class ERP_Create_Purtg
    {
        Mssql mssql = new Mssql();
        string strConnection = FormLogin.infObj.connYF;

        #region 后端业务逻辑
        public string HandelDef(string flowID)
        {
            ERP_Create_Purtg_HeadObject headObj = new ERP_Create_Purtg_HeadObject();
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

        private void GetHeadTime(ERP_Create_Purtg_HeadObject headObj)
        {
            string sqlstr = "select dbo.f_getTime(1)";
            DataTable dt = mssql.SQLselect(strConnection, sqlstr);
            if (dt != null)
            {
                headObj.Time = dt.Rows[0][0].ToString();
            }
        }

        private void GetHeadInfo(ERP_Create_Purtg_HeadObject headObj)
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
                                FROM COMFORT.dbo.JH_LYXA AS JHXA 
                                LEFT JOIN COMFORT.dbo.PURTC AS PURTC ON 1=2 
                                LEFT JOIN COMFORT.dbo.INVMB AS INVMB ON MB001=JHXA007 
                                LEFT JOIN COMFORT.dbo.PURMA AS PURMA ON MA001=JHXA002 
                                LEFT JOIN (SELECT CMSMG.MG003, CMSMG.MG001 FROM COMFORT.dbo.CMSMG 
                                INNER JOIN (SELECT MAX(MG002) MAXMG02, MG001 MAXMG01 FROM CMSMG GROUP BY MG001) AS MG 
                                ON MG.MAXMG01 = CMSMG.MG001 AND MG.MAXMG02 = CMSMG.MG002) AS MG2 ON MG2.MG001 = MA021 
                                WHERE JHXA005 IN ('{0}') AND JHXA011 = 'N' ";
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sqlstr, headObj.FlowId));
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

        private void GetHeadTG002(ERP_Create_Purtg_HeadObject headObj)
        {
            //string sqlstr = @"SELECT (CASE WHEN A1 IS NULL THEN A2 + '0001' ELSE A1 END ) B FROM 
            //                    (SELECT MAX(TG002) + 1 A1, SUBSTRING(CONVERT(VARCHAR(10), GETDATE(), 112), 3, 4) A2 
            //                    FROM PURTG 
            //                    WHERE TG001 = '{0}' AND SUBSTRING(TG002, 1, 4) = 
            //                    SUBSTRING(CONVERT(VARCHAR(10), GETDATE(), 112), 3, 4)) A";
            string sqlstr = "EXEC dbo.P_GETDH '{0}'";
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sqlstr, headObj.TG001));
            if (dt != null)
            {
                headObj.TG002 = dt.Rows[0][0].ToString();
            }
        }

        // 写入单头
        private void SetHeadInfo(ERP_Create_Purtg_HeadObject headObj)
        {
            string sqlstr = @"INSERT INTO PURTG (COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, TG001, TG002, TG003, TG004, TG005, 
                                TG006, TG007, TG008, TG009, TG010, TG013, TG014, TG015, TG021, TG030, TG033, TG016, TG043, TG052) 
                                VALUES('{0}', '{1}', '{2}', '{3}', '1', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', 
                                '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '', '', '' )";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.Company, headObj.Uid, headObj.Ugroup, headObj.Time, headObj.TG001,
                headObj.TG002, headObj.TG003, headObj.TG004, headObj.TG005, headObj.TG006, headObj.TG007, headObj.TG008, headObj.TG009,
                headObj.TG010, headObj.TG013, headObj.TG014, headObj.TG015, headObj.TG021, headObj.TG030, headObj.TG033));
        }

        private void SetDetailDef(ERP_Create_Purtg_HeadObject headObj)
        {
            SetDetailDefaultDef(headObj);
        }

        private void SetDetailDefaultDef(ERP_Create_Purtg_HeadObject headObj)
        {
            ERP_Create_Purtg_DetailObject detailObj = new ERP_Create_Purtg_DetailObject();
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
        private void GetLsDt(ERP_Create_Purtg_HeadObject headObj, ERP_Create_Purtg_DetailObject detailObj)
        {
            string sqlstr = @"SELECT RTRIM(MB001), RTRIM(MB002), RTRIM(MB003), RTRIM(MB004), 
                                RTRIM(JHXA003), RTRIM(JHXA009) FROM INVMB 
                                INNER JOIN JH_LYXA ON JHXA007 = MB001 WHERE 1=1 AND JHXA005 = '{0}' ORDER BY ID";
            detailObj.LsDt = mssql.SQLselect(strConnection, string.Format(sqlstr, headObj.FlowId));
        }

        //可进货采购单明细逻辑
        private void GetSlDt(ERP_Create_Purtg_HeadObject headObj, ERP_Create_Purtg_DetailObject detailObj)
        {
            string sqlstr = @"SELECT DISTINCT TOP 200 TD008 - TD015 - ( SELECT isnull( SUM ( TH007 ), 0 ) 
                                FROM COMFORT.dbo.PURTH(NOLOCK) AS PURTH 
                                INNER JOIN COMFORT.dbo.PURTG(NOLOCK) AS PURTG ON TG001 = TH001 AND TG002 = TH002
                                WHERE TH011 = TD001 AND TH012 = TD002 AND TH013 = TD003 
                                AND TG013 = 'N'
                                ) AS WJL, 
                                TD001 AS TH011, RTRIM(TD002) AS TH012, TD003 AS TH013, (CASE WHEN TD010 IS NULL THEN 0 ELSE TD010 END) AS TH018, TD014 AS TH033, RTRIM(TD020) AS TH035, 
                                RTRIM(TD022) AS TH042, RTRIM(TDC03) AS THC02, TC003, TD012 
                                FROM COMFORT.dbo.PURTD(NOLOCK) AS PURTD 
                                LEFT JOIN COMFORT.dbo.PURTC(NOLOCK) AS PURTC ON TC001 = TD001 AND TC002 = TD002 
                                WHERE TC004 = '{0}' AND TD004 = '{1}' 
                                AND (TD008 - TD015 - ( SELECT isnull( SUM ( TH007 ), 0 ) FROM COMFORT.dbo.PURTH PURTH 
                                WHERE TH011 = TD001 AND TH012 = TD002 AND TH013 = TD003 AND TH030 = 'N' )) > 0 
                                AND TD016 = 'N' AND TC014 = 'Y' AND TC001 <> '3305' AND TC001 <> '3306' 
                                ORDER BY TC003 DESC, TD012, TD001 DESC, RTRIM(TD002), TD003";
            detailObj.SlDt = mssql.SQLselect(strConnection, string.Format(sqlstr, headObj.TG005, detailObj.TH004));
        }

        //数量
        private void GetNumber(ERP_Create_Purtg_HeadObject headObj, ERP_Create_Purtg_DetailObject detailObj)
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
        private void SetDetailInfo(ERP_Create_Purtg_HeadObject headObj, ERP_Create_Purtg_DetailObject detailObj)
        {
            detailObj.TH015 = detailObj.TH007;
            detailObj.TH016 = detailObj.TH007;
            detailObj.TH034 = detailObj.TH007;
            detailObj.TH064 = detailObj.TH008;
            detailObj.TH065 = detailObj.TH008;
            detailObj.TH019 = Math.Round(float.Parse(detailObj.TH007) * float.Parse(detailObj.TH018), 6, MidpointRounding.AwayFromZero).ToString();
            detailObj.TH003 = detailObj.RowIndex.ToString().PadLeft(4, '0');

            string sqlstr = @"INSERT INTO PURTH(COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG, 
                                TH001,TH002,TH003,TH004,TH005,TH006,TH007,TH008,TH009,TH010, 
                                TH011,TH012,TH013,TH014,TH015,TH016,TH018,TH019,TH026,TH027, 
                                TH029,TH030,TH031,TH032,TH033,TH034,TH035,TH042,TH043,TH044, 
                                TH060,TH064,TH065,TH071,TH072,THC02) 
                                VALUES('{0}','{1}','{2}','{3}',1,'{4}','{5}','{6}', 
                                '{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}', 
                                '{18}','{19}','{20}','{21}','N','{22}','N','N','N','N','{23}', 
                                '{24}','{25}','{26}','N','N','0','{27}','{28}','1','##########','{29}')";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.Company, headObj.Uid, headObj.Ugroup, headObj.Time, headObj.TG001, headObj.TG002,
                detailObj.TH003, detailObj.TH004, detailObj.TH005, detailObj.TH006, detailObj.TH007, detailObj.TH008, detailObj.TH009, detailObj.TH010,
                detailObj.TH011, detailObj.TH012, detailObj.TH013, detailObj.TH014, detailObj.TH015, detailObj.TH016, detailObj.TH018, detailObj.TH019,
                detailObj.TH027, detailObj.TH033, detailObj.TH034, detailObj.TH035, detailObj.TH042, detailObj.TH064, detailObj.TH065, detailObj.THC02));
        }

        //更新单头税率，汇率 
        private void UptHeadInfo2(ERP_Create_Purtg_HeadObject headObj)
        {
            string sqlstr = @"UPDATE COMFORT.dbo.PURTG SET TG008 = ISNULL(MG003, 1) FROM COMFORT.dbo.PURTG 
                            LEFT JOIN( 
                                SELECT G.MG001, G.MG003 FROM(
                                    SELECT MG001, MAX(MG002) MG002 FROM CMSMG
                                    WHERE 1 = 1
                                    AND MG002 <= LEFT(dbo.f_getTime(1), 8)
                                    GROUP BY MG001
                                ) AS K LEFT JOIN COMFORT.dbo.CMSMG AS G ON K.MG001 = G.MG001 AND K.MG002 = G.MG002
                            ) AS A ON TG007 = A.MG001
                            WHERE TG001 = '{0}' AND TG002 = '{1}'

                            UPDATE COMFORT.dbo.PURTG SET TG030 = ISNULL(MA004, 0) FROM COMFORT.dbo.PURTG LEFT JOIN COMFORT.dbo.CMSMA ON CMSMA.COMPANY = 'COMFORT' 
                            WHERE TG001 = '{0}' AND TG002 ='{1}'  ";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.TG001, headObj.TG002));
        }

        //更新单身金额信息
        private void UptDetailMoney(ERP_Create_Purtg_HeadObject headObj)
        {
            string sqlstr = @"UPDATE COMFORT.dbo.PURTH  SET 
                                TH045 = CAST(ROUND(TH019/(1+CONVERT(FLOAT, TG030)),2) AS  NUMERIC(10,2)), 
                                TH046 = CAST(ROUND(TH019 - (TH019/(1+CONVERT(FLOAT, TG030))),2) AS  NUMERIC(10,2)), 
                                TH047 = CAST(ROUND((TH019 * CONVERT(FLOAT, TG008)/(1+CONVERT(FLOAT, TG030))),2) 
                                AS  NUMERIC(10,2)), 
                                TH048 = CAST(ROUND((TH019 * CONVERT(FLOAT, TG008)) - 
                                (TH019 * CONVERT(FLOAT, TG008)/(1+CONVERT(FLOAT, TG030))),2) AS  NUMERIC(10,2)) 
                                FROM PURTH INNER JOIN COMFORT.dbo.PURTG AS PURTG ON TG001 = TH001 AND TG002 = TH002 
                                WHERE TG001= '{0}' AND TG002= '{1}' ";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.TG001, headObj.TG002));
        }

        //更新单头金额信息
        private void UptHeadMoney(ERP_Create_Purtg_HeadObject headObj)
        {
            string sqlstr = @"UPDATE A SET TG017=SUMTH019,TG019=SUMTH046,TG026=SUMTH015,TG028=SUMTH045,TG031=SUMTH047, 
                                TG032=SUMTH048,TG040=SUMTH050,TG041=SUMTH052,TG053=SUMTH007,TG054=SUMTH049 
                                FROM COMFORT.dbo.PURTG A 
                                INNER JOIN (SELECT TH001,TH002,SUMTH019=SUM(TH019),SUMTH046=SUM(TH046), 
                                SUMTH007=SUM(CASE WHEN MA024='2' THEN FLOOR(TH007) ELSE TH007 END), 
                                SUMTH015=SUM(CASE WHEN MA024='2' THEN FLOOR(TH015) ELSE TH015 END),SUMTH045=SUM(TH045), 
                                SUMTH047=SUM(TH047),SUMTH048=SUM(TH048),SUMTH050=SUM(TH050),SUMTH052=SUM(TH052), 
                                SUMTH049=SUM(TH049) 
                                FROM COMFORT.dbo.PURTH 
                                INNER JOIN COMFORT.dbo.CMSMA ON 1=1 
                                GROUP BY TH001,TH002)  AS B ON A.TG001=B.TH001 AND A.TG002=B.TH002 
                                WHERE TG001= '{0}' AND TG002= '{1}' ";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.TG001, headObj.TG002));
        }

        private void UptJHXAInfo(ERP_Create_Purtg_HeadObject headObj)
        {
            string time = mssql.SQLselect(strConnection, "select dbo.f_getTime(1)").Rows[0][0].ToString();
            string sqlstr = @"UPDATE COMFORT.dbo.JH_LYXA SET MODIFIER='{1}', MODI_DATE='{2}', 
                                FLAG=(convert(int,COMFORT.dbo.JH_LYXA.FLAG))%999+1, JHXA011 = 'Y', UDF01 = '{3}'WHERE  JHXA005 = '{0}' ";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.FlowId, headObj.Uid, time, headObj.TG001 + '-' + headObj.TG002));
        }
        #endregion
    }

    #region 数据对象基类
    class ERP_Create_Purtg_HeadObject
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

    class ERP_Create_Purtg_DetailObject
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
}
