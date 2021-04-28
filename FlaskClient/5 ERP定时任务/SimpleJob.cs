using System.Data;
using HarveyZ;
using System;

namespace ERP定时任务
{

    public class ERP_BOMB05
    {
        private Mssql mssql = null;
        private string connYF = null;
        private Logger logger = null;

        public bool workFlag = false;

        public ERP_BOMB05(Mssql mssql, string connYF, Logger logger)
        {
            this.mssql = mssql;
            this.connYF = connYF;
            this.logger = logger;
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("ERP Job BOMB05: " + text);
        }

        public void MainWork()
        {
            if (ERP定时任务.GetConfig("ERP_BOMB05", "Work"))
            {
                workFlag = true;
                log("Work Start!");
                try
                {
                    Work();
                }
                catch (Exception e)
                {
                    log("Work Error./n" + e.ToString());
                }
                finally
                {

                    log("Work Finished!");
                    workFlag = false;
                }
            }
        }

        private void Work()
        {
            string sqlStr = @"
                            DECLARE @NYR VARCHAR(20)
                            DECLARE @XH INT
                            DECLARE @JOBID VARCHAR(20)
                            DECLARE @SUBID VARCHAR(20)
                            SELECT @NYR = CONVERT(VARCHAR(20), GETDATE(), 112)

                            IF NOT EXISTS(SELECT * FROM [DSCSYS].[dbo].[JOBQUEUE] WHERE JOBNAME = 'BOMB05' AND STATUS IN ('P', 'N'))
                            BEGIN
	                            IF EXISTS(SELECT * FROM [DSCSYS].[dbo].[JOBQUEUE] WHERE SUBSTRING(JOBID, 1, 8) = @NYR )
	                            BEGIN
		                            SELECT  @XH = CONVERT(INT, SUBSTRING(MAX(JOBID), 9, 6)) 
			                            FROM [DSCSYS].[dbo].[JOBQUEUE] WHERE SUBSTRING(JOBID, 1, 8) = @NYR
	                            END
	                            ELSE 
	                            BEGIN
		                            SET @XH = 0
	                            END

	                            SELECT @JOBID = @NYR + RIGHT('000000' + CAST(@XH + 1 AS VARCHAR(6)), 6)
	                            SELECT @SUBID = @NYR + '0001'

	                            INSERT INTO [DSCSYS].[dbo].[JOBQUEUE] ([JOBID], [SUBID], [COMPANYID], [USERID], [USEDALIAS], [JOBNAME], [EXTNAME], [COMPROGID], [JOBOPTION], [GENTYPE], [GENSTATUS], 
	                            [PRIORITY], [STATUS], [PROGRESS], [DTREQUEST], [DTRECEIVE], [DTSCHEDULE], [DTSTART], [DTFINISH], [RESULT], [STYLE], [PROCESSER], [FLAG], [NOTIFY]) 
	                            VALUES (@JOBID, @SUBID, 'COMFORT', 'Robot', 'COMFORT', 'BOMB05', '', 'BOMB05S.Class1', 
	                            0x44532056415249414E54202030313030180100000C2000000100000000000000010000000C2000000100000001000000030000000C20000001000000000000000100000008000000060000000990E9623B4EF64EC154F753080000000400000073007000300031000C20000001000000000000000100000008000000070000001F7510624C006F006700876563680800000006000000630068006B004C006F0067000C20000001000000000000000100000008000000040000005C4F1A4EE5651F67080000000B000000650064005000720069006E00740044006100740065000C2000000100000001000000030000000C200000010000000000000002000000080000000100000004000800000000000000080000000000000008000000010000004E000800000000000000, 
	                            1, 1, 3, 'N', NULL, GETDATE(), GETDATE(), GETDATE(), NULL, NULL, NULL, 'B', '', 1, '')
                            END";
            mssql.SQLexcute(connYF, sqlStr);
        }
    }

    public class ERP_COPAB02
    {
        private Mssql mssql = null;
        private string connYF = null;
        private Logger logger = null;

        public bool workFlag = false;

        public ERP_COPAB02(Mssql mssql, string connYF, Logger logger)
        {
            this.mssql = mssql;
            this.connYF = connYF;
            this.logger = logger;
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("ERP Job COPAB02: " + text);
        }

        public void MainWork()
        {
            if (ERP定时任务.GetConfig("ERP_COPAB02", "Work"))
            {
                workFlag = true;
                log("Work Start!");
                try
                {
                    Work();
                }
                catch (Exception e)
                {
                    log("Work Error./n" + e.ToString());
                }
                finally
                {

                    log("Work Finished!");
                    workFlag = false;
                }
            }
        }

        private void Work()
        {
            string sqlStr = @"
                            DECLARE @NYR VARCHAR(20)
                            DECLARE @XH INT
                            DECLARE @JOBID VARCHAR(20)
                            DECLARE @SUBID VARCHAR(20)
                            SELECT @NYR = CONVERT(VARCHAR(20), GETDATE(), 112)

                            IF NOT EXISTS(SELECT * FROM [DSCSYS].[dbo].[JOBQUEUE] WHERE JOBNAME = 'COPAB02' AND STATUS IN ('P', 'N'))
                            BEGIN
	                            IF EXISTS(SELECT * FROM DSCSYS..JOBQUEUE WHERE SUBSTRING(JOBID, 1, 8) = @NYR )
	                            BEGIN 
		                            SELECT  @XH = CONVERT(INT, SUBSTRING(MAX(JOBID), 9, 6)) FROM DSCSYS..JOBQUEUE WHERE SUBSTRING(JOBID, 1, 8) = @NYR 
	                            END
	                            ELSE 
	                            BEGIN
		                            SET @XH = 0
	                            END

	                            SELECT @JOBID = @NYR + RIGHT('000000' + CAST(@XH + 1 AS VARCHAR(6)), 6)
	                            SELECT @SUBID = @NYR + '0001'

	                            INSERT INTO [DSCSYS]..[JOBQUEUE] ([JOBID], [SUBID], [COMPANYID], [USERID], [USEDALIAS], [JOBNAME], [EXTNAME], [COMPROGID], [JOBOPTION], [GENTYPE], [GENSTATUS], 
	                            [PRIORITY], [STATUS], [PROGRESS], [DTREQUEST], [DTRECEIVE], [DTSCHEDULE], [DTSTART], [DTFINISH], [RESULT], [STYLE], [PROCESSER], [FLAG], [NOTIFY]) 
	                            VALUES (@JOBID, @SUBID, 'COMFORT', 'Robot', 'COMFORT', 'COPAB02', '', 'COPAB02S.Class1', 
	                            0x44532056415249414E54202030313030640100000C2000000100000000000000010000000C2000000100000001000000040000000C20000001000000000000000100000008000000040000000990E962C154F753080000000400000073007000300031000C20000001000000000000000100000008000000070000000990E96242004F004D00E5651F67080000000400000073007000300032000C20000001000000000000000100000008000000070000001F7510624C006F006700876563680800000006000000630068006B004C006F0067000C20000001000000000000000100000008000000040000005C4F1A4EE5651F67080000000B000000650064005000720069006E00740044006100740065000C2000000100000001000000040000000C200000010000000000000000000000080000000100000005000C2000000100000000000000020000000800000001000000040008000000000000000800000000000000080000000100000059000800000000000000, 
	                            1, 1, 3, 'N', NULL, GETDATE(), GETDATE(), GETDATE(), NULL, NULL, NULL, 'B', '', 1, '');
                            END";
            mssql.SQLexcute(connYF, sqlStr);
        }
    }

    public class FixMocta
    {
        private Mssql mssql = null;
        private string connYF = null;
        private Logger logger = null;

        public bool workFlag = false;

        public FixMocta(Mssql mssql, string connYF, Logger logger)
        {
            this.mssql = mssql;
            this.connYF = connYF;
            this.logger = logger;
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("Fix Mocta Department Info: " + text);
        }

        public void MainWork()
        {
            if (ERP定时任务.GetConfig("Fix_Mocta", "Work"))
            {
                workFlag = true;
                log("Work Start!");
                try
                {
                    Work();
                }
                catch (Exception e)
                {
                    log("Work Error./n" + e.ToString());
                }
                finally
                {

                    log("Work Finished!");
                    workFlag = false;
                }
            }
        }

        private void Work()
        {
            string sqlStr = @"
                            UPDATE COMFORT.dbo.MOCTA 
                            SET TA064 = MV004, TA021 = MD001 

                            FROM COMFORT.dbo.MOCTA 

                            INNER JOIN COMFORT.dbo.CMSMV ON MV001 = TA041 

                            INNER JOIN COMFORT.dbo.CMSMD ON MD015 = MV004 AND MD001 IN ('1' ,'6' , '7') 

                            WHERE 1=1 
                            AND TA013 = 'Y'  

                            AND TA011 NOT IN ('Y', 'y') 

                            AND (TA064 != MV004 OR MD001 != TA021) 
                            AND (MD001 IS NOT NULL OR RTRIM(MD001) != '') 
                            AND (MV004 IS NOT NULL OR RTRIM(MV004) != '') ";
            mssql.SQLexcute(connYF, sqlStr);
        }
    }

    public class FixPurta
    {
        private Mssql mssql = null;
        private string connYF = null;
        private Logger logger = null;

        public bool workFlag = false;

        public FixPurta(Mssql mssql, string connYF, Logger logger)
        {
            this.mssql = mssql;
            this.connYF = connYF;
            this.logger = logger;
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("Fix Purta Info: " + text);
        }

        public void MainWork()
        {
            if (ERP定时任务.GetConfig("Fix_Purta", "Work"))
            {
                workFlag = true;
                log("Work Start!");
                try
                {
                    Work();
                }
                catch (Exception e)
                {
                    log("Work Error./n" + e.ToString());
                }
                finally
                {
                    log("Work Finished!");
                    workFlag = false;
                }
            }
        }

        private void Work()
        {
            Fix1();
            //Fix2();
            Fix3();
        }

        /// <summary>
        /// 请购单中单头单身审核码不对应的
        /// </summary>
        private void Fix1()
        {
            string sqlStr = @"IF EXISTS(SELECT 1 FROM dbo.PURTA INNER JOIN dbo.PURTB ON TA001 = TB001 AND TA002 = TB002 WHERE TA007 <> TB025 AND TA003 >= '20200101')
	                            BEGIN 
		                            UPDATE dbo.PURTB SET TB025 = TA007 FROM dbo.PURTA INNER JOIN dbo.PURTB ON TA001 = TB001 AND TA002 = TB002 WHERE TA007 <> TB025 AND TA003 >= '20200101'
	                            END ";
            mssql.SQLexcute(connYF, sqlStr);
        }

        /// <summary>
        /// 请购子单身PURTR采购发放码与请购单身PURTB结束码不对应的
        /// </summary>
        private void Fix2()
        {
            string sqlStr = @"IF EXISTS(SELECT 1 FROM dbo.PURTA INNER JOIN dbo.PURTB(NOLOCK) ON TA001 = TB001 AND TA002 = TB002 INNER JOIN dbo.PURTR ON TB001 = TR001 AND TB002 = TR002 AND TB003 = TR003 
						                            WHERE TA007 = 'Y' AND TB039 = 'N' AND TR017 = 'Y' AND TA003 >= '20200101')
	                            BEGIN 
		                            UPDATE dbo.PURTR SET TR017 = TB039 FROM dbo.PURTA INNER JOIN dbo.PURTB ON TA001 = TB001 AND TA002 = TB002 INNER JOIN dbo.PURTR ON TB001 = TR001 AND TB002 = TR002 AND TB003 = TR003 
			                            WHERE TA007 = 'Y' AND TB039 = 'N' AND TR017 = 'Y' AND TA003 >= '20200101'
	                            END ";
            mssql.SQLexcute(connYF, sqlStr);
        }

        /// <summary>
        /// 请购子单身PURTR表对比请购单身PURTB缺少项补全
        /// </summary>
        private void Fix3()
        {
            string sqlStr1 = @"SELECT DISTINCT RTRIM(TA001)+'-'+RTRIM(TA002) 
				                FROM dbo.PURTA INNER JOIN dbo.PURTB(NOLOCK)  ON TA001 = TB001 AND TA002 = TB002
				                LEFT JOIN dbo.PURTR ON TR001 = TB001 AND TR002 = TB002 AND TR003 = TB003
				                WHERE TA007 = 'Y'
				                AND TR004 IS NULL AND TA003 >= '20200101'
				                ORDER BY RTRIM(TA001)+'-'+RTRIM(TA002)";

            string sqlStr2 = @"INSERT INTO dbo.PURTR (COMPANY, CREATOR, CREATE_DATE, FLAG, TR001, TR002, TR003, TR004, TR005, TR006, TR007, TR008, TR009, TR010, TR011, TR012, TR013, TR014, TR015, TR016, TR017, TR018, TR019, TR020, TR021, TR022, TR023, TR024, TR025, TR026, TR027)
				                SELECT K.COMPANY, K.CREATOR, K.CREATE_DATE, K.FLAG, TB001 AS TR001, TB002 AS TR002, TB003 AS TR003, '0001' AS TR004, 
				                TB010 AS TR005, TB009 AS TR006, 1 AS TR007, TB007 AS TR008, MA021 AS TR009, TB017 AS TR010, TB018 AS TR011, TB026 AS TR012, 
				                TB019 AS TR013, TB008 AS TR014, TB032 AS TR015, TB024 AS TR016, 'N' AS TR017, 'N' AS TR018, '' AS TR019, TB035 AS TR020, 
				                TB038 AS TR021, TB040 AS TR022, TB041 AS TR023, TB028 AS TR024, TB042 AS TR025, TB043 AS TR026, TB044 AS TR027
				                FROM dbo.PURTB(NOLOCK)  
				                INNER JOIN dbo.PURTA(NOLOCK) ON TA001 = TB001 AND TA002 = TB002 
				                LEFT JOIN dbo.PURMA(NOLOCK) ON MA001 = TB010
				                LEFT JOIN (SELECT TOP 1 COMPANY, CREATOR, CREATE_DATE, FLAG FROM dbo.PURTR(NOLOCK) WHERE RTRIM(TR001)+'-'+RTRIM(TR002) = '{0}') AS K ON 1=1
				                WHERE 1=1
				                AND RTRIM(TB001)+'-'+RTRIM(TB002) = '{0}' 
				                AND NOT EXISTS(SELECT 1 FROM dbo.PURTR(NOLOCK) WHERE TB001 = TR001 AND TB002 = TR002 AND TB003 = TR003)";

            DataTable dt =  mssql.SQLselect(connYF, sqlStr1);
            if (dt != null)
            {
                for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    log("Insert Into Purtr: " + dt.Rows[rowIndex][0].ToString());
                    mssql.SQLexcute(connYF, string.Format(sqlStr2, dt.Rows[rowIndex][0].ToString()));
                }
            }
        }
    }

    public class FixPurtc
    {
        private Mssql mssql = null;
        private string connYF = null;
        private Logger logger = null;

        public bool workFlag = false;

        public FixPurtc(Mssql mssql, string connYF, Logger logger)
        {
            this.mssql = mssql;
            this.connYF = connYF;
            this.logger = logger;
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("Fix Purtc Info: " + text);
        }

        public void MainWork()
        {
            if (ERP定时任务.GetConfig("Fix_Purtc", "Work"))
            {
                workFlag = true;
                log("Work Start!");
                try
                {
                    Work();
                }
                catch (Exception e)
                {
                    log("Work Error./n" + e.ToString());
                }
                finally
                {
                    log("Work Finished!");
                    workFlag = false;
                }
            }
        }

        private void Work()
        {
            Fix1();
        }

        /// <summary>
        /// 请购单中单头单身审核码不对应的
        /// </summary>
        private void Fix1()
        {
            string sqlStr1 = @"SELECT TC001, TC002 FROM PURTC WHERE PURTC.UDF07 IS NULL AND PURTC.TC003 >= '20210101' AND PURTC.TC001 IN ('3301', '3308') ORDER BY TC004, TC001, TC002";
            string sqlStr2 = @"UPDATE PURTC SET PURTC.UDF07 = 
		                        ISNULL(STUFF((SELECT DISTINCT ',' +  INVMB.UDF12 FROM PURTD(NOLOCK)
		                        INNER JOIN PURTR(NOLOCK) ON PURTD.TD001+'-'+PURTD.TD002+'-'+PURTD.TD003 = PURTR.TR019 
		                        INNER JOIN PURTB(NOLOCK) ON PURTB.TB001 = PURTR.TR001 AND PURTB.TB002 = PURTR.TR002 AND PURTB.TB003 = PURTR.TR003 
		                        INNER JOIN COPTD(NOLOCK) ON COPTD.TD001 = PURTB.TB029 AND COPTD.TD002 = PURTB.TB030 AND COPTD.TD003 = PURTB.TB031
		                        INNER JOIN INVMB(NOLOCK) ON INVMB.MB001 = COPTD.TD004 
		                        WHERE PURTC.TC001 = PURTD.TD001 AND PURTC.TC002 = PURTD.TD002
		                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''), '')
		                        FROM PURTC(NOLOCK) 
		                        WHERE PURTC.TC001 = '{0}' AND PURTC.TC002 = '{1}' ";
            DataTable dt = mssql.SQLselect(connYF, sqlStr1);

            if(dt != null)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    string tc001 = dt.Rows[i]["TC001"].ToString();
                    string tc002 = dt.Rows[i]["TC002"].ToString();
                    log(string.Format("TC001: {0}, TC002: {1}!", tc001, tc002));
                    mssql.SQLexcute(connYF, string.Format(sqlStr2, tc001, tc002));
                }
                
            }
        }
    }

    public class DeleteCoptrError
    {
        private Mssql mssql = null;
        private string connYF = null;
        private Logger logger = null;

        public bool workFlag = false;

        public DeleteCoptrError(Mssql mssql, string connYF, Logger logger)
        {
            this.mssql = mssql;
            this.connYF = connYF;
            this.logger = logger;
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("Delete Coptr Error Info: " + text);
        }

        public void MainWork()
        {
            if (ERP定时任务.GetConfig("CleanCoptrX", "Work"))
            {
                workFlag = true;
                log("Work Start!");
                try
                {
                    Work();
                }
                catch (Exception e)
                {
                    log("Work Error./n" + e.ToString());
                }
                finally
                {

                    log("Work Finished!");
                    workFlag = false;
                }
            }
        }

        private void Work()
        {
            string sqlStr = @"IF EXISTS (SELECT* FROM DSCSYS.dbo.JOBQUEUE WHERE JOBNAME IN ('COPAB02', 'COPAB01', 'LRPMB01', 'LRPB01', 'LRPMB02', 'LRPB02') AND STATUS IN ('P', 'N'))
                                BEGIN
	                                DELETE FROM [COMFORT].[dbo].[COPTR] WHERE TR003 like 'X%' 
	                                AND TR001 NOT LIKE '6%'
	                                AND TR001 NOT LIKE '7%'
	                                AND TR001 NOT LIKE '8%'
	                                AND TR001 NOT LIKE '9%'


	                                UPDATE COPTR SET TR003 = SUBSTRING(TR003, 2, LEN(TR003)) 
	                                WHERE (TR001 LIKE '6%' 
	                                OR TR001 LIKE '7%'
	                                OR TR001 LIKE '8%'
	                                OR TR001 LIKE '9%')
	                                AND TR003 LIKE 'X%'

                                END ";
            mssql.SQLexcute(connYF, sqlStr);
        }
    }

    public class CreateScPlanSnapShot
    {
        private Mssql mssql = null;
        private string connWG = null;
        private Logger logger = null;

        public bool workFlag = false;

        public CreateScPlanSnapShot(Mssql mssql, string connWG, Logger logger)
        {
            this.mssql = mssql;
            this.connWG = connWG;
            this.logger = logger;
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("Create SC_PLAN Snapshot: " + text);
        }

        public void MainWork()
        {
            if (ERP定时任务.GetConfig("SCPLAN_Snapshot", "Work"))
            {
                workFlag = true;
                log("Work Start!");
                try
                {
                    Work();
                }
                catch (Exception e)
                {
                    log("Work Error./n" + e.ToString());
                }
                finally
                {

                    log("Work Finished!");
                    workFlag = false;
                }
            }
        }

        private void Work()
        {
            string sqlStr = @"
                                DELETE FROM SC_PLAN_Snapshot WHERE SC000 <= CONVERT(VARCHAR(8), DATEADD(DAY, -3, GETDATE()), 112)

                                INSERT INTO SC_PLAN_Snapshot(SC000, K_ID, SC001, SC003, SC013, SC023, SC028, SC029)
                                SELECT CONVERT(VARCHAR(8), GETDATE(), 112) SC000, K_ID, SC001, SC003, SC013, SC023, SC028, SC029 FROM SC_PLAN 
                                WHERE 1=1
                                AND NOT EXISTS(SELECT 1 FROM SC_PLAN_Snapshot AS A WHERE A.K_ID = SC_PLAN.K_ID AND A.SC001 = SC_PLAN.SC001 AND A.SC000 = CONVERT(VARCHAR(8), GETDATE(), 112))
                                ORDER BY K_ID";
            mssql.SQLexcute(connWG, sqlStr);
        }
    }
}
