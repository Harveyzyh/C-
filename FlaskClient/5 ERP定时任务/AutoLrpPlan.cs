using System;
using HarveyZ;
using System.Data;
using System.Threading;

namespace ERP定时任务
{
    class AutoLrpPlan
    {
        private Mssql mssql = null;
        private string connYF = null;
        private string connWG = null;
        private Logger logger = null;

        public bool workFlag = false;
        private bool workingFlag = false;

        #region 运行配置参数变量
        private bool workEnableFlag = false;
        private bool bomCalculateEnableFlag = false;
        private bool pzCalculateEnableFlag = false;
        private bool lockCgPlanEnableFlag = false;
        private bool lockScPlanEnableFlag = false;
        private bool layoutCgPlanEnableFlag = false;
        private bool layoutScPlanEnableFlag = false;
        private int lrpCount = 3;

        private bool layoutCgPlanEnableTmpFlag = false;
        private bool hasKhpzFlag = false;
        #endregion

        public AutoLrpPlan(Mssql mssql, string connYF, string connWG, Logger logger)
        {
            this.mssql = mssql;
            this.connYF = connYF;
            this.connWG = connWG;
            this.logger = logger;
        }

        public void MainWork()
        {
            workFlag = true;
            log("Work Start!");

            try
            {
                Work();
            }
            catch(Exception e)
            {
                log("Work Error: \n" + e.ToString());
            }
            finally
            {
                log("Work Finished! \n");
                workFlag = false;
            }
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("AutoLrpPlan: " + text);
        }

        private void Work()
        {
            WorkPrepare();
            GetWorkConfig();

            if (workEnableFlag)
            {
                workingFlag = true;
                ResetWorkingFlag();
                ResetWorkOtherFlag();

                while (workingFlag)
                {
                    DataTable listDt = GetDdList();
                    if (listDt != null)
                    {
                        string planDd = listDt.Rows[0]["planDd"].ToString();
                        string planId = listDt.Rows[0]["planId"].ToString();
                        //当前已跑次数
                        int count = int.Parse(listDt.Rows[0]["LRPCOUNT"].ToString());
                        //设置单个订单对否要发放请购单
                        layoutCgPlanEnableTmpFlag = listDt.Rows[0]["CG"].ToString() == "N" ? true : false;

                        string wlno = listDt.Rows[0]["TD004"].ToString(); ;
                        string khpz = listDt.Rows[0]["TD053"].ToString(); ;
                        hasKhpzFlag = khpz == "" ? false : true;


                        //以下为处理主逻辑
                        log(string.Format("Work With PlanDd:{0}  - PlanId:{1}  - Wlno: {2}  - Pz: {3}  - LrpCount: {4} ", planDd, planId, wlno, khpz, count.ToString()));
                        
                        SetWorkingFlag(planDd);
                        
                        Calculate(wlno, khpz);
                        
                        GeneratePlan(planDd, planId);
                        
                        bool planBomError = CheckPlanBomError(planId);
                        bool planNoneError = CheckPlanNoneError(planId);


                        if (count + 1 != lrpCount)
                        {
                            if (planBomError)
                            {
                                if (hasKhpzFlag)
                                {
                                    SetWorkFalseFlag(planDd);
                                }
                                else
                                {
                                    log("当前订单无客户配置");
                                    LockPlan(planId);
                                    LayoutPlan(planId);
                                    SetWorkDoneFlag(planDd);
                                }
                            }
                            else if (planNoneError)
                            {
                                SetWorkFalseFlag(planDd);
                            }
                            else
                            {
                                LockPlan(planId);
                                LayoutPlan(planId);
                                SetWorkDoneFlag(planDd);
                            }
                        }
                        //已跑3次
                        else
                        {
                            if (planBomError)
                            {
                                if (hasKhpzFlag)
                                {
                                    log(string.Format("当前订单执行LRP次数将达到配置上限{0}次，视为订单使用标准BOM配置", lrpCount.ToString()));
                                    LockPlan(planId);
                                    LayoutPlan(planId);
                                    SetWorkDoneFlag(planDd);
                                }
                                else
                                {
                                    log("当前订单无客户配置");
                                    LockPlan(planId);
                                    LayoutPlan(planId);
                                    SetWorkDoneFlag(planDd);
                                }
                            }
                            else if (planNoneError)
                            {
                                log(string.Format("当前订单已跑3次计划，且当前计划单身为空，需人工检查。 PlanDd: {0}", planDd));
                            }
                            else
                            {
                                LockPlan(planId);
                                LayoutPlan(planId);
                                SetWorkDoneFlag(planDd);
                            }
                        }
                        CleanPlan(planId);
                    }
                    else
                    {
                        workingFlag = false;
                        log("Not Found Order List!");
                    }
                }
            }
        }

        #region 预处理逻辑
        /// <summary>
        /// 预处理，清理配置的标志位
        /// </summary>
        private void WorkPrepare()
        {
            workEnableFlag = false;
            bomCalculateEnableFlag = false;
            pzCalculateEnableFlag = false;
            lockCgPlanEnableFlag = false;
            lockScPlanEnableFlag = false;
            layoutCgPlanEnableFlag = false;
            layoutScPlanEnableFlag = false;
            lrpCount = 3;

            layoutCgPlanEnableTmpFlag = false;
            hasKhpzFlag = false;
        }

        /// <summary>
        /// 获取相关配置信息
        /// </summary>
        private void GetWorkConfig()
        {
            string sqlStr1 = "SELECT Valid FROM WG_DB.dbo.WG_CONFIG WHERE ConfigName = 'AutoErpPlan' AND Type = 'Work' AND Valid = 'Y'";
            string sqlStr2 = "SELECT Valid FROM WG_DB.dbo.WG_CONFIG WHERE ConfigName = 'AutoErpPlan' AND Type = 'BomCaculate' AND Valid = 'Y'";
            string sqlStr3 = "SELECT Valid FROM WG_DB.dbo.WG_CONFIG WHERE ConfigName = 'AutoErpPlan' AND Type = 'PzCaculate' AND Valid = 'Y'";
            string sqlStr4 = "SELECT Valid FROM WG_DB.dbo.WG_CONFIG WHERE ConfigName = 'AutoErpPlan' AND Type = 'LayoutScPlan' AND Valid = 'Y'";
            string sqlStr5 = "SELECT Valid FROM WG_DB.dbo.WG_CONFIG WHERE ConfigName = 'AutoErpPlan' AND Type = 'LayoutCgPlan' AND Valid = 'Y'";
            string sqlStr6 = "SELECT Valid FROM WG_DB.dbo.WG_CONFIG WHERE ConfigName = 'AutoErpPlan' AND Type = 'LockScPlan' AND Valid = 'Y'";
            string sqlStr7 = "SELECT Valid FROM WG_DB.dbo.WG_CONFIG WHERE ConfigName = 'AutoErpPlan' AND Type = 'LockCgPlan' AND Valid = 'Y'";
            string sqlStr8 = "SELECT Version FROM WG_DB.dbo.WG_CONFIG WHERE ConfigName = 'AutoErpPlan' AND Type = 'LrpCount' AND Valid = 'Y'";

            workEnableFlag = mssql.SQLexist(connWG, sqlStr1);
            bomCalculateEnableFlag = mssql.SQLexist(connWG, sqlStr2);
            pzCalculateEnableFlag = mssql.SQLexist(connWG, sqlStr3);
            layoutScPlanEnableFlag = mssql.SQLexist(connWG, sqlStr4);
            layoutCgPlanEnableFlag = mssql.SQLexist(connWG, sqlStr5);
            lockScPlanEnableFlag = mssql.SQLexist(connWG, sqlStr6);
            lockCgPlanEnableFlag = mssql.SQLexist(connWG, sqlStr7);

            DataTable lrpCountDt = mssql.SQLselect(connWG, sqlStr8);
            if (lrpCountDt != null) lrpCount = int.Parse(lrpCountDt.Rows[0][0].ToString());


            log(string.Format("Configuration：Global Enable：{0}, BomCalculation Enable：{1}, PzCalculation Enable：{2}, Layout Sc Plan Enable：{3}, Layout Cg Plan Enable：{4}, Max Lrp Count：{5}",
                   workEnableFlag, bomCalculateEnableFlag, pzCalculateEnableFlag, layoutScPlanEnableFlag, layoutCgPlanEnableFlag, lrpCount.ToString()));
        }

        /// <summary>
        /// 还原LRPFLAG为n,y的状态
        /// </summary>
        private void ResetWorkingFlag()
        {
            log("还原LRPFLAG为n,y的状态");
            string sqlStr = "UPDATE dbo.COPTD SET LRPFLAG = 'N' WHERE LRPFLAG IN ('n', 'y') ";
            mssql.SQLexcute(connYF, sqlStr);
        }

        /// <summary>
        /// 重置已变更但工单未审的订单，不存在工单但状态为Y的订单
        /// </summary>
        private void ResetWorkOtherFlag()
        {
            log("重置已变更但工单未审的订单，不存在工单但状态为Y的订单");
            string sqlStr = "DECLARE @WORKFLAG1 BIT, @TA001 VARCHAR(4), @TA002 VARCHAR(20), @TA026 VARCHAR(4), @TA027 VARCHAR(20), "
                       + "@TA028 VARCHAR(4), @LRPCOUNT INT "
                       + ""
                       + "UPDATE COMFORT.dbo.COPTD SET COPTD.LRPFLAG = 'N', LRPCOUNT = LRPCOUNT - 1 "
                       + "FROM COMFORT.dbo.COPTC "
                       + "INNER JOIN COMFORT.dbo.COPTD ON TD001 = TC001 AND TD002 = TC002 "
                       + "INNER JOIN COMFORT.dbo.INVMB ON MB001 = TD004 "
                       + "WHERE 1=1 "
                       + "AND COPTC.TC027 = 'Y' "
                       + "AND COPTD.UDF04 != 0 "
                       + "AND NOT EXISTS (SELECT 1 FROM COMFORT.dbo.MOCTA WHERE TA026 = TD001 AND TA027 = TD002 "
                       + "    AND TA028 = TD003 AND TA006 = TD004) "
                       + "AND NOT EXISTS (SELECT 1 FROM COMFORT.dbo.LRPLB WHERE LB002 = TD001 AND LB003 = TD002 "
                       + "    AND LB004 = TD003) "
                       + "AND COPTD.LRPFLAG = 'Y' "
                       + "AND COPTD.TD016 = 'N' "
                       + "AND INVMB.MB025 NOT IN ('P') "
                       + "AND COPTC.TC003 >= '20200301' "
                       + ""
                       + "SET @WORKFLAG1=1 "
                       + "WHILE(@WORKFLAG1=1) "
                       + "BEGIN "
                       + "    SET @TA001 = '' "
                       + "	SET @TA002 = '' "
                       + "	SET @TA026 = '' "
                       + "	SET @TA027 = '' "
                       + "	SET @TA028 = '' "
                       + "	IF EXISTS(SELECT TD001, TD002, TD003, TA001, TA002 "
                       + "        FROM COMFORT.dbo.COPTD "
                       + "        INNER JOIN COMFORT.dbo.MOCTA ON TA026 = TD001 AND TA027 = TD002 AND TA028 = TD003 "
                       + "        WHERE 1=1 "
                       + "        AND LRPDATE < COPTD.UDF12 "
                       + "        AND TA013 = 'N' "
                       + "        AND TA033 LIKE 'A%' "
                       + "        AND SUBSTRING(MOCTA.CREATE_DATE, 1, 8) >= '20200301' ) "
                       + "    BEGIN "
                       + "        SELECT TOP 1 @TA001=TA001, @TA002=TA002, @TA026=TA026, @TA027=TA027, "
                       + "            @TA028=TA028 FROM COMFORT.dbo.COPTD "
                       + "        INNER JOIN COMFORT.dbo.MOCTA ON TA026 = TD001 AND TA027 = TD002 AND TA028 = TD003 "
                       + "        WHERE 1=1 "
                       + "		AND LRPDATE < COPTD.UDF12 "
                       + "		AND TA013 = 'N' "
                       + "		AND TA033 LIKE 'A%' "
                       + "		AND SUBSTRING(MOCTA.CREATE_DATE, 1, 8) >= '20200301' "
                       + "		ORDER BY MOCTA.CREATE_DATE, TA033 "
                       + ""
                       + "        UPDATE COMFORT.dbo.COPTD SET LRPFLAG = 'N', LRPCOUNT = 0 WHERE TD001=@TA026 AND TD002=@TA027 "
                       + "            AND TD003=@TA028 "
                       + "        DELETE FROM COMFORT.dbo.MOCTB WHERE TB001=@TA001 AND TB002=@TA002 "
                       + "		DELETE FROM COMFORT.dbo.MOCTA WHERE TA001=@TA001 AND TA002=@TA002 "
                       + "	END "
                       + "	ELSE  "
                       + "	BEGIN  "
                       + "		SET @WORKFLAG1=0 "
                       + "	END  "
                       + "END ";
            mssql.SQLexcute(connYF, sqlStr);
        }
        #endregion

        #region 共用逻辑
        /// <summary>
        /// 字符串转16进制字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>16进制字符串</returns>
        private string StrToHex(string str)//字符串转16进制
        {
            string returnStr = "";
            foreach(char c in str)
            {
                string hex = Convert.ToString(c, 16).Replace("0x", "").Replace("0X", "").PadLeft(2, '0').ToUpper();
                if(hex.Length == 2)
                {
                    hex = hex + "00";
                }
                else
                {
                    hex = hex.Substring(2, 2) + hex.Substring(0, 2);
                }
                returnStr += hex;
            }
            return returnStr;
        }

        /// <summary>
        /// 数值转16进制字符串
        /// </summary>
        /// <param name="i">源数值</param>
        /// <returns>16进制字符串</returns>
        private string IntToHex(int i)//数字转16进制
        {
            string hex = Convert.ToString(i, 16).Replace("0x", "").Replace("0X", "").PadLeft(2, '0').ToUpper();
            if (hex.Length == 2)
            {
                hex = hex + "00";
            }
            else
            {
                hex = hex.Substring(2, 2) + hex.Substring(0, 2);
            }
            return hex;
        }

        /// <summary>
        /// 获取ERP派班任务ID信息
        /// </summary>
        /// <returns>派班任务ID</returns>
        private DataTable GetJobId()
        {
            string sqlStr = @"DECLARE @NYR VARCHAR(20), @XH INT "
                           + @"SELECT @NYR = CONVERT(VARCHAR(20), GETDATE(), 112) "
                           + @"IF EXISTS(SELECT * FROM DSCSYS.dbo.JOBQUEUE WHERE SUBSTRING(JOBID, 1, 8) = @NYR ) "
                           + @"BEGIN "
                           + @" SELECT  @XH = CONVERT(INT, SUBSTRING(MAX(JOBID), 9, 6)) FROM DSCSYS.dbo.JOBQUEUE "
                           + @"     WHERE SUBSTRING(JOBID, 1, 8) = @NYR "
                           + @"END "
                           + @"ELSE "
                           + @"BEGIN "
                           + @"	SET @XH = 0 "
                           + @"END "
                           + @"SELECT @NYR + RIGHT('000000' + CAST(@XH + 1 AS VARCHAR(6)), 6) AS JOBID, @NYR + '0001' AS SUBID  ";
            
            return mssql.SQLselect(connYF, sqlStr);
        }

        /// <summary>
        /// 把后台任务写入ERP派班中心
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobOption">条件的16进制字符串</param>
        /// <param name="progName">调用程序名称</param>
        /// <param name="creator">任务创建人</param>
        /// <returns></returns>
        private string InsertJob(string jobName, string jobOption, string progName = "", string creator = "Robot")
        {
            //string sqlStr = "INSERT INTO DSCSYS.dbo.JOBQUEUE ([JOBID], [SUBID], [COMPANYID], [USERID], [USEDALIAS], [JOBNAME], "
            //             + "[EXTNAME], [COMPROGID], [JOBOPTION], [GENTYPE], [GENSTATUS], [PRIORITY], [STATUS], [PROGRESS], "
            //             + "[DTREQUEST], [DTRECEIVE], [DTSCHEDULE], [DTSTART], [DTFINISH], [RESULT], [STYLE], [PROCESSER], "
            //             + "[FLAG], [NOTIFY]) "
            //             + "VALUES ('{jobId}', '{subId}', 'COMFORT', '{creator}', 'COMFORT', '{jobName}', '', "
            //             + "'{progName}', {hexStr}, 1, 1, 3, 'N', NULL, getdate(), getdate(), getdate(), "
            //             + "NULL, NULL, NULL, 'B', '', 1, '');";
            string sqlStr = "INSERT INTO DSCSYS.dbo.JOBQUEUE ([JOBID], [SUBID], [COMPANYID], [USERID], [USEDALIAS], [JOBNAME], "
                         + "[EXTNAME], [COMPROGID], [JOBOPTION], [GENTYPE], [GENSTATUS], [PRIORITY], [STATUS], [PROGRESS], "
                         + "[DTREQUEST], [DTRECEIVE], [DTSCHEDULE], [DTSTART], [DTFINISH], [RESULT], [STYLE], [PROCESSER], "
                         + "[FLAG], [NOTIFY]) "
                         + "VALUES ('{0}', '{1}', 'COMFORT', '{2}', 'COMFORT', '{3}', '', "
                         + "'{4}', {5}, 1, 1, 3, 'N', NULL, getdate(), getdate(), getdate(), "
                         + "NULL, NULL, NULL, 'B', '', 1, '');";

            if (progName == "") progName = jobName + "S.Class1";

            string jobId = "";
            string subId = "";

            try
            {
                DataTable jobIdDt = GetJobId();

                jobId = jobIdDt.Rows[0]["JOBID"].ToString();
                subId = jobIdDt.Rows[0]["SUBID"].ToString();

                mssql.SQLexcute(connYF, string.Format(sqlStr, jobId, subId, creator, jobName, progName, jobOption));
            }
            catch
            {
                DataTable jobIdDt = GetJobId();

                jobId = jobIdDt.Rows[0]["JOBID"].ToString();
                subId = jobIdDt.Rows[0]["SUBID"].ToString();

                mssql.SQLexcute(connYF, string.Format(sqlStr, jobId, subId, creator, jobName, progName, jobOption));
            }
            return jobId;
        }

        /// <summary>
        /// 检查ERP派班任务是否已完成
        /// </summary>
        /// <param name="jobId">派班任务ID</param>
        private void CheckJobDone(string jobId)
        {
            string sqlStr = "SELECT JOBID FROM DSCSYS.dbo.JOBQUEUE WHERE JOBID = '{0}' AND STATUS != 'D' ";

            while (mssql.SQLexist(connYF, string.Format(sqlStr, jobId)))
            {
                Thread.Sleep(500);
            }

            log(string.Format("Job Done: JobId: {0}'", jobId));
        }

        /// <summary>
        /// 设置COPTD订单正在工作y
        /// </summary>
        /// <param name="planDd">订单号</param>
        private void SetWorkingFlag(string planDd)
        {
            string sqlStr = "UPDATE dbo.COPTD SET LRPFLAG = 'y' WHERE TD001+'-'+TD002+TD003 = '{0}' ";
            mssql.SQLexcute(connYF, string.Format(sqlStr, planDd));
            log(string.Format("Set Working Flag Plan: PlanDd: {0}", planDd));
        }

        /// <summary>
        /// 设置COPTD订单完成工作Y
        /// </summary>
        /// <param name="planDd">订单号</param>
        private void SetWorkDoneFlag(string planDd)
        {
            string sqlStr = "UPDATE dbo.COPTD SET LRPFLAG='Y', LRPDATE=LEFT(dbo.f_getTime(1), 12), LRPCOUNT=LRPCOUNT+1 WHERE TD001+'-'+TD002+TD003 = '{0}' ";
            mssql.SQLexcute(connYF, string.Format(sqlStr, planDd));
            log(string.Format("Set Work Done Flag Plan: PlanDd: {0}", planDd));
        }

        /// <summary>
        /// 设置COPTD订单错误工作n
        /// </summary>
        /// <param name="planDd">订单号</param>
        private void SetWorkFalseFlag(string planDd)
        {
            string sqlStr = "UPDATE dbo.COPTD SET LRPFLAG='n', LRPCOUNT=LRPCOUNT+1 WHERE TD001+'-'+TD002+TD003 = '{0}' ";
            mssql.SQLexcute(connYF, string.Format(sqlStr, planDd));
            log(string.Format("Set Work False Flag Plan: PlanDd: {0}", planDd));
        }
        #endregion
        
        #region 获取信息
        /// <summary>
        /// 获取一条订单信息
        /// </summary>
        /// <returns></returns>
        private DataTable GetDdList()
        {
            string sqlStr = "SELECT TOP 1 planDd, planId, LRPCOUNT, CG, TD004, TD053 FROM V_GetAutoErpPlanOrderDetail ORDER BY UDF12, planDd ";

            return mssql.SQLselect(connYF, sqlStr);
        }

        /// <summary>
        /// 获取品号信息
        /// </summary>
        /// <param name="dd">订单号</param>
        /// <param name="wlno">品号</param>
        /// <param name="khpz">客户配置</param>
        private void GetKhpz(string dd, out string wlno, out string khpz)
        {
            string sqlStr = "SELECT DISTINCT RTRIM(TD004) 品号, ISNULL(RTRIM(TD053), '') 客户配置 FROM dbo.COPTD "
                      + "INNER JOIN dbo.INVMB ON TD004 = MB001 "
                      + "WHERE TD001+'-'+TD002+TD003 = '{0}' "
                      + "AND MB025 NOT IN ('P') ";
            DataTable dt = mssql.SQLselect(connYF, string.Format(sqlStr, dd));
            if (dt != null)
            {
                wlno = dt.Rows[0]["品号"].ToString();
                khpz = dt.Rows[0]["客户配置"].ToString();
            }
            else
            {
                wlno = "";
                khpz = "";
            }
        }
        #endregion

        #region 计算低阶码、层级码
        /// <summary>
        /// 计算低阶码，层级码
        /// </summary>
        /// <param name="wlno">品号</param>
        /// <param name="khpz">客户配置</param>
        private void Calculate(string wlno = "", string khpz = "")
        {
            if (wlno != "")
            {
                if (bomCalculateEnableFlag)
                {
                    string jobId = BomCalculate(wlno);
                    CheckJobDone(jobId);
                }
                if (pzCalculateEnableFlag)
                {
                    if (khpz != "")
                    {
                        string jobId = PzCalculate(wlno, khpz);
                        CheckJobDone(jobId);
                    }
                }
            }
            else
            {
                log("Calculate: Not Found Wlno!");
            }
    }

        private string BomCalculate(string wlno)
        {
            string hexStr = "0x44532056415249414E54202030313030380100000C2000000100000000000000010000000C2000000100000001000000"
                     + "030000000C20000001000000000000000100000008000000060000000990E9623B4EF64EC154F753080000000400000073"
                     + "007000300031000C20000001000000000000000100000008000000070000001F7510624C006F0067008765636808000000"
                     + "06000000630068006B004C006F0067000C20000001000000000000000100000008000000040000005C4F1A4EE5651F6708"
                     + "0000000B000000650064005000720069006E00740044006100740065000C2000000100000001000000030000000C200000"
                     + "0100000000000000020000000800000001000000040008000000{0}0000{1}08000000{0}0000{1}08000000"
                     + "010000004E000800000000000000";

            string wlnoHex = StrToHex(wlno);
            string wlnoLenHex = IntToHex(wlno.Length);

            string jobId = InsertJob("BOMB05", string.Format(hexStr, wlnoLenHex, wlnoHex));

            log(string.Format("Insert Job BomCalculate: Wlno: {0}  - JobId: {1}", wlno, jobId));
            return jobId;
        }

        private string PzCalculate(string wlno, string khpz)
        {
            string hexStr = "0x44532056415249414E54202030313030740100000C2000000100000000000000010000000C2000000100000001000000"
                        + "040000000C2000000100000000000000010000000800000002000000C154F753080000000400000065006400300031000C"
                        + "20000001000000000000000100000008000000060000000990E9624D916E7FB96548680800000004000000730070003000"
                        + "31000C20000001000000000000000100000008000000070000001F7510624C006F00670087656368080000000600000063"
                        + "0068006B004C006F0067000C20000001000000000000000100000008000000040000005C4F1A4EE5651F67080000000B00"
                        + "0000650064005000720069006E00740044006100740065000C20000001000000010000000400000008000000{0}000"
                        + "0{1}0C200000010000000000000001000000080000000100000005000C20000001000000000000000000000008000000"
                        + "{2}0000{3}08000000010000004E000800000000000000";

            string wlnoHex = StrToHex(wlno);
            string wlnoLenHex = IntToHex(wlno.Length);
            string khpzHex = StrToHex(khpz);
            string khpzLenHex = IntToHex(khpz.Length);

            string jobId = InsertJob("COPAB01", string.Format(hexStr, wlnoLenHex, wlnoHex, khpzLenHex, khpzHex));

            log(string.Format("Insert Job PzCalculate: Wlno: {0}  - Pz: {1}  - JobId: {2}", wlno, khpz, jobId));

            return jobId;
        }
        #endregion

        #region 生成LRP计划
        /// <summary>
        /// 生成LRP计划
        /// </summary>
        /// <param name="planDd">订单号</param>
        /// <param name="planId">LRP计划ID</param>
        private void GeneratePlan(string planDd, string planId)
        {
            string jobId = GeneratePlanDetail(planDd, planId);
            CheckJobDone(jobId);
        }

        private string GeneratePlanDetail(string planDd, string planId)
        {
            string hexStr = "0x44532056415249414E54202030313030C40700000C2000000100000000000000010000000C20000001000000010000"
                        + "00180000000C20000001000000000000000100000008000000060000000990E962A18B12529D4F6E6308000000050000"
                        + "00630062006F00300031000C20000001000000000000000100000008000000040000000990E962E55D82530800000004"
                        + "00000065006400300032000C20000001000000000000000100000008000000060000000990E9626567906E167FF75308"
                        + "0000000400000073007000300033000C2000000100000000000000010000000800000006000000938F6551A18B125279"
                        + "62F75308000000050000006D0065006400300034000C20000001000000000000000100000008000000040000000990E9"
                        + "62D34E935E080000000400000073007000300035000C20000001000000000000000100000008000000060000000990E9"
                        + "626588278D3F65567B0800000005000000630062006F00300036000C2000000100000000000000010000000800000006"
                        + "0000000097426CA18B977BB9650F5F0800000005000000720064006700300037000C2000000100000000000000010000"
                        + "00080000000600000003805186895B6851585BCF910800000005000000630068006B00300038000C2000000100000000"
                        + "00000001000000080000000C0000000097426CE5651F670E5484769B4FD97EB37E6551A18B977B080000000600000063"
                        + "0068006B003000380031000C200000010000000000000001000000080000000C0000000097426CE5651F670E54847600"
                        + "97426CB37E6551A18B977B0800000006000000630068006B003000380032000C20000001000000000000000100000008"
                        + "000000080000000990E96200971F7510628476A18B12520800000005000000630062006F00300039000C200000010000"
                        + "000000000001000000080000000C000000F95B8E4EF25DD1533E659965F64E847665884551B9650F5F08000000050000"
                        + "00630062006F00310030000C20000001000000000000000100000008000000090000001F7510620097426C3A4EF69684"
                        + "769965F64E0800000005000000630068006B00310031000C2000000100000000000000010000000800000008000000D6"
                        + "53FF66E34E9965B37E6551A18B977B0800000005000000630068006B00310032000C2000000100000000000000010000"
                        + "000800000008000000A25B37629B4F9965B37E6551A18B977B0800000007000000630068006B00300037005F0031000C"
                        + "200000010000000000000001000000080000000B0000000990E9624D0050005300A18B1252005FE55DE5651F67080000"
                        + "000400000073007000310036000C2000000100000000000000010000000800000005000000038051865F631780877308"
                        + "00000005000000630068006B00310033000C2000000100000000000000010000000800000005000000A18B977B5D4E27"
                        + "59CF910800000005000000630068006B00300039000C2000000100000000000000010000000800000008000000085476"
                        + "5EA18B977B00674E4F6588CF910800000005000000630068006B00310034000C20000001000000000000000100000008"
                        + "000000070000004151B88BF76D7962D653FF66E34E0800000005000000630068006B00310035000C2000000100000000"
                        + "00000001000000080000000200000048722C67080000000400000065006400310036000C200000010000000000000001"
                        + "00000008000000020000002760288D0800000005000000630062006F00310037000C2000000100000000000000010000"
                        + "0008000000070000001F7510624C006F006700876563680800000006000000630068006B004C006F0067000C20000001"
                        + "000000000000000100000008000000040000005C4F1A4EE5651F67080000000B000000650064005000720069006E0074"
                        + "0044006100740065000C2000000100000001000000180000000C20000001000000000000000100000003000000010000"
                        + "00080000000400000031002E00A28B55530800000002000000300031000C200000010000000000000001000000080000"
                        + "000100000005000C20000001000000000000000000000008000000{0}0000{1}08000000{2}"
                        + "0000{3}0C200000010000000000000000000000080000000100000005000C200000010000000000000001000000"
                        + "08000000010000004C0008000000080000004C002E0009634C00520050000097426C0C20000001000000000000000100"
                        + "000003000000020000000800000003000000DB6B0097426C0800000001000000590008000000010000004E0008000000"
                        + "010000004E000C2000000100000000000000010000000300000003000000080000000400000033002E006851E8900C20"
                        + "00000100000000000000010000000300000002000000080000000600000032002E00CD91B06565884551080000000100"
                        + "00004E0008000000010000004E0008000000010000004E000C2000000100000000000000020000000800000001000000"
                        + "04000800000000000000080000000000000008000000010000004E0008000000010000004E0008000000010000004E00"
                        + "08000000010000004E00080000000400000030003000300031000C200000010000000000000001000000030000000100"
                        + "0000080000000400000031002E0009674865080000000100000059000800000000000000";

            string planDdHex = StrToHex(planDd);
            string planDdLenHex = IntToHex(planDd.Length);
            string planIdHex = StrToHex(planId);
            string planIdLenHex = IntToHex(planId.Length);

            string jobId = InsertJob("LRPMB01", string.Format(hexStr, planDdLenHex, planDdHex, planIdLenHex, planIdHex));

            log(string.Format("Insert Job Generate Plan: PlanDd: {0}  - PlanId: {1}  - JobId: {2} ", planDd, planId, jobId));
            return jobId;
        }
        #endregion

        #region 计划检查
        /// <summary>
        /// 检查是否为标准BOM，是返回True
        /// </summary>
        /// <param name="planId">LRP计划ID</param>
        /// <returns></returns>
        private bool CheckPlanBomError(string planId)
        {
            string sqlStr = "SELECT LRP, CP, PH, GY FROM ("
                       + "   SELECT TA.TA001 AS LRP, TA.TA002 AS CP, TB.TB005 AS PH, TB.TB006 AS GY FROM dbo.LRPTA AS TA "
                       + "   INNER JOIN dbo.LRPTB AS TB ON TA.TA001 = TB.TB001 AND TA.TA028 = TB.TB013 AND TA.TA002 = TB.TB002 "
                       + "   INNER JOIN dbo.COPTD AS TD ON TD.TD001 = TA.TA023 AND TD.TD002 = TA.TA024 AND TD.TD003 = TA.TA025 "
                       + "AND TA.TA002 = TD.TD004 "
                       + "   WHERE TA010 = '5101' AND TA001 = '{0}' "
                       + "   UNION "
                       + "   SELECT TA.TA001 AS LRP, TA.TA002 AS CP, BL.CB005 AS PH, BL.CB011 AS GY FROM dbo.LRPTA AS TA "
                       + "   INNER JOIN dbo.COPTD AS TD ON TD.TD001 = TA.TA023 AND TD.TD002 = TA.TA024 AND TD.TD003 = TA.TA025 "
                       + "AND TA.TA002 = TD.TD004 "
                       + "   INNER JOIN dbo.BOMCB_List AS BL ON BL.CB001 = TA.TA002  "
                       + "   WHERE TA010 = '5101' AND TA001 = '{0}' "
                       + ") AS UN "
                       + "WHERE 1=1 "
                       + "AND ( "
                       + "	NOT EXISTS(SELECT 1 FROM dbo.BOMCB_List AS BL1 WHERE BL1.CB001=CP AND BL1.CB005=PH AND BL1.CB011=GY) "
                       + "   OR "
                       + "	NOT EXISTS(SELECT 1 FROM dbo.LRPTA AS TA "
                       + "		INNER JOIN dbo.LRPTB AS TB ON TA.TA001 = TB.TB001 AND TA.TA028 = TB.TB013 "
                       + "AND TA.TA002 = TB.TB002 AND TA010 = '5101' "
                       + "		INNER JOIN dbo.COPTD AS TD ON TD.TD001 = TA.TA023 AND TD.TD002 = TA.TA024 "
                       + "AND TD.TD003 = TA.TA025 AND TA.TA002 = TD.TD004 "
                       + "		WHERE TA.TA001=LRP)"
                       + ") ";
            DataTable dt = mssql.SQLselect(connYF, string.Format(sqlStr, planId));

            if (dt == null)
            {
                log(string.Format("Checked Bom Error: PlanId: {0}", planId));
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查是否计划为空，空返回True
        /// </summary>
        /// <param name="planId">LRP计划ID</param>
        /// <returns></returns>
        private bool CheckPlanNoneError(string planId)
        {
            string sqlStr = "SELECT TA.TA001 AS LRP, TA.TA002 AS CP, TB.TB005 AS PH, TB.TB006 AS GY FROM dbo.LRPTA AS TA "
                       + "INNER JOIN dbo.LRPTB AS TB ON TA.TA001 = TB.TB001 AND TA.TA028 = TB.TB013 AND TA.TA002 = TB.TB002 "
                       + "INNER JOIN dbo.COPTD AS TD ON TD.TD001 = TA.TA023 AND TD.TD002 = TA.TA024 AND TD.TD003 = TA.TA025 "
                       + "AND TA.TA002 = TD.TD004 "
                       + "WHERE TA010 = '5101' AND TA001 = '{0}' ";
            DataTable dt = mssql.SQLselect(connYF, string.Format(sqlStr, planId));
            if (dt == null)
            {
                log(string.Format("Checked Plan None Error: PlanId: {0}", planId));
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 锁定计划
        /// <summary>
        /// 锁定LRP计划
        /// </summary>
        /// <param name="planId">LRP计划ID</param>
        /// <param name="planVer">计划版本，默认0001</param>
        private void LockPlan(string planId, string planVer = "0001")
        {
            if (lockScPlanEnableFlag)
            {
                string jobId = LockScPlan(planId, planVer);
                CheckJobDone(jobId);
            }
            if (lockCgPlanEnableFlag)
            {
                string jobId = LockCgPlan(planId, planVer);
                CheckJobDone(jobId);
            }
        }

        private string LockScPlan(string planId, string planVer)
        {
            string hexStr = "0x44532056415249414E54202030313030AE0400000C2000000100000000000000010000000C20000001000000010000000E"
                        + "0000000C20000001000000000000000100000008000000040000000990E962C154F753080000000400000073007000300031"
                        + "000C20000001000000000000000100000008000000040000000990E962E55D8253080000000400000065006400300032000C"
                        + "20000001000000000000000100000008000000040000000990E962D34E935E080000000400000073007000300033000C2000"
                        + "0001000000000000000100000008000000050000000990E9628C5BE55DE565080000000400000073007000300034000C2000"
                        + "0001000000000000000100000008000000060000000990E962A18B12527962F753080000000400000073007000300035000C"
                        + "20000001000000000000000100000008000000060000000990E962E55D555355532B52080000000400000065006400300036"
                        + "000C20000001000000000000000100000008000000060000000990E962A18B1252BA4E585408000000040000006500640030"
                        + "0037000C20000001000000000000000100000008000000060000000990E96201959A5BB67201600800000005000000720064"
                        + "006700300038000C200000010000000000000001000000080000000B000000C54E8894F95BC6620652B08B555FDB8F4C8801"
                        + "959A5B0800000005000000630068006B00300039000C200000010000000000000001000000080000000B000000C54E8894F9"
                        + "5BC6620652B08B555FD653886D01959A5B0800000005000000630068006B00310030000C2000000100000000000000010000"
                        + "0008000000070000000C54656B01959A5BA74E1062C1540800000005000000630068006B00310031000C2000000100000000"
                        + "0000000100000008000000090000000C54656BD653886D01959A5BA74E1062C1540800000005000000630068006B00310032"
                        + "000C20000001000000000000000100000008000000070000001F7510624C006F006700876563680800000006000000630068"
                        + "006B004C006F0067000C20000001000000000000000100000008000000040000005C4F1A4EE5651F67080000000B00000065"
                        + "0064005000720069006E00740044006100740065000C20000001000000010000000E0000000C200000010000000000000002"
                        + "000000080000000100000004000800000000000000080000000000000008000000000000000C200000010000000000000002"
                        + "00000008000000010000000400080000000000000008000000000000000C2000000100000000000000020000000800000001"
                        + "0000000400080000000000000008000000000000000C20000001000000000000000200000008000000010000000400080000"
                        + "0018000000{0}{1}0800000018000000{0}{1}080000000000000008000000000000000C200000"
                        + "0100000000000000010000000300000001000000080000000200000001959A5B08000000010000004E000800000001000000"
                        + "4E0008000000010000004E0008000000010000004E0008000000010000004E000800000000000000";

            string planIdHex = StrToHex(planId.PadRight(20, ' '));
            string planVerHex = StrToHex(planVer);

            string jobId = InsertJob("LRPB02", string.Format(hexStr, planIdHex, planVerHex));

            log(string.Format("Insert Job Lock SC Plan: PlanId: {0}  - JobId: {1} ", planId, jobId));
            return jobId;
        }

        private string LockCgPlan(string planId, string planVer)
        {
            string hexStr = "0x44532056415249414E54202030313030B00300000C2000000100000000000000010000000C20000001000000010000000A"
                        + "0000000C20000001000000000000000100000008000000040000000990E962C154F753080000000400000073007000300031"
                        + "000C20000001000000000000000100000008000000040000000990E962E55D8253080000000400000065006400300032000C"
                        + "20000001000000000000000100000008000000040000000990E962D34E935E080000000400000073007000300033000C2000"
                        + "0001000000000000000100000008000000050000000990E962A44E278DE565080000000400000073007000300034000C2000"
                        + "0001000000000000000100000008000000050000000990E962C7912D8DE565080000000400000073007000300035000C2000"
                        + "0001000000000000000100000008000000060000000990E962A18B12527962F753080000000400000073007000300036000C"
                        + "20000001000000000000000100000008000000060000000990E962A18B1252BA4E5854080000000400000065006400300037"
                        + "000C20000001000000000000000100000008000000060000000990E96201959A5BB672016008000000040000007200670030"
                        + "0038000C20000001000000000000000100000008000000070000001F7510624C006F00670087656368080000000600000063"
                        + "0068006B004C006F0067000C20000001000000000000000100000008000000040000005C4F1A4EE5651F67080000000B0000"
                        + "00650064005000720069006E00740044006100740065000C20000001000000010000000A0000000C20000001000000000000"
                        + "0002000000080000000100000004000800000000000000080000000000000008000000000000000C20000001000000000000"
                        + "000200000008000000010000000400080000000000000008000000000000000C200000010000000000000002000000080000"
                        + "00010000000400080000000000000008000000000000000C2000000100000000000000020000000800000001000000040008"
                        + "0000000000000008000000000000000C200000010000000000000002000000080000000100000004000800000018000000"
                        + "{0}{1}0800000018000000{0}{1}08000000000000000C20000001000000000000000100000003"
                        + "00000001000000080000000200000001959A5B08000000010000004E000800000000000000";

            string planIdHex = StrToHex(planId.PadRight(20, ' '));
            string planVerHex = StrToHex(planVer);

            string jobId = InsertJob("LRPB04", string.Format(hexStr, planIdHex, planVerHex));

            log(string.Format("Insert Job Lock CG Plan: PlanId: {0}  - JobId: {1} ", planId, jobId));
            return jobId;
        }
        #endregion

        #region 发放计划
        /// <summary>
        /// 发放LRP计划
        /// </summary>
        /// <param name="planId">LRP计划ID</param>
        /// <param name="planVer">计划版本，默认0001</param>
        private void LayoutPlan(string planId, string planVer = "0001")
        {
            if (layoutScPlanEnableFlag)
            {
                string jobId = LayoutScPlan(planId, planVer);
                CheckJobDone(jobId);
            }
            if (layoutCgPlanEnableFlag && layoutCgPlanEnableTmpFlag)
            {
                string jobId = LayoutCgPlan(planId, planVer);
                CheckJobDone(jobId);
            }
        }

        private string LayoutScPlan(string planId, string planVer)
        {
            string hexStr = "0x44532056415249414E54202030313030C20500000C2000000100000000000000010000000C200000010000000100000011"
                        + "0000000C20000001000000000000000100000008000000040000000990E962C154F753080000000400000073007000300031"
                        + "000C20000001000000000000000100000008000000050000000990E9628C5BE55DE565080000000400000073007000300032"
                        + "000C20000001000000000000000100000008000000060000000990E962A18B12527962F75308000000040000007300700030"
                        + "0033000C20000001000000000000000100000008000000040000000990E962E55D8253080000000400000065006400300034"
                        + "000C20000001000000000000000100000008000000060000000990E9621F75A74ED34E935E08000000040000007300700030"
                        + "0035000C20000001000000000000000100000008000000040000000990E962B6720160080000000400000072006700300036"
                        + "000C20000001000000000000000100000008000000060000000990E962E55D555355532B5208000000040000006500640030"
                        + "0037000C20000001000000000000000100000008000000060000000990E962E55D55532760288D0800000005000000630062"
                        + "006F00300038000C20000001000000000000000100000008000000060000000990E962A18B1252BA4E585408000000040000"
                        + "0065006400300039000C20000001000000000000000100000008000000100000000C54A18B12527962F7532C00C154F75320"
                        + "000854765E10620C54004EE55D55530800000005000000630068006B00310030000C20000001000000000000000100000008"
                        + "0000000E000000D1533E65D4591659E55D55538476D45916595553F74E3A4EF69605800800000005000000630068006B0031"
                        + "0031000C200000010000000000000001000000080000001F00000042004F004D002C00005FE55D2C008498A18B86989965E5"
                        + "652C008C5BE55DE5650F5C8E4ED1533E65E5651F67F46639650E4ED1533E65E5651F67F8760C540800000005000000630068"
                        + "006B00310032000C2000000100000000000000010000000800000006000000938F6551D1533E65E5651F6708000000040000"
                        + "006D006500310033000C2000000100000000000000010000000800000006000000D1533E6592638F5E9D4F6E630800000007"
                        + "000000630062006F00300039005F0031000C2000000100000000000000010000000800000004000000CD91D6535553F74E08"
                        + "00000005000000630068006B00310034000C20000001000000000000000100000008000000070000001F7510624C006F0067"
                        + "00876563680800000006000000630068006B004C006F0067000C20000001000000000000000100000008000000040000005C"
                        + "4F1A4EE5651F67080000000B000000650064005000720069006E00740044006100740065000C200000010000000100000011"
                        + "0000000C20000001000000000000000200000008000000010000000400080000000000000008000000000000000C20000001"
                        + "000000000000000200000008000000010000000400080000000000000008000000000000000C200000010000000000000001"
                        + "000000080000000100000005000C2000000100000000000000000000000800000018000000{0}{1}080000000"
                        + "00000000C20000001000000000000000200000008000000010000000400080000000000000008000000000000000C2000000"
                        + "10000000000000001000000030000000200000008000000020000006851E89008000000000000000C2000000100000000000"
                        + "0000100000003000000010000000800000002000000EA813652080000000000000008000000010000004E000800000001000"
                        + "0004E00080000000100000059000800000008000000{2}0C20000001000000000000000"
                        + "100000003000000010000000800000004000000A18B12527962F75308000000010000004E0008000000010000004E0008000"
                        + "00000000000";

            string planIdHex = StrToHex(planId.PadRight(20, ' '));
            string planVerHex = StrToHex(planVer);
            string dateStrHex = StrToHex(DateTime.Now.ToString("yyyyMMdd"));

            string jobId = InsertJob("LRPMB03", string.Format(hexStr, planIdHex, planVerHex, dateStrHex));

            log(string.Format("Insert Job Layout SC Plan: PlanId: {0}  - JobId: {1} ", planId, jobId));
            return jobId;
        }

        private string LayoutCgPlan(string planId, string planVer)
        {
            string hexStr = "0x44532056415249414E54202030313030900800000C2000000100000000000000010000000C20000001000000010000001C00"
                    + "00000C20000001000000000000000100000008000000050000000990E9629B4F945E4655080000000400000073007000300031"
                    + "000C20000001000000000000000100000008000000040000000990E962C154F753080000000400000073007000300032000C20"
                    + "000001000000000000000100000008000000050000000990E962C7912D8DE565080000000400000073007000300033000C2000"
                    + "0001000000000000000100000008000000060000000990E962A18B12527962F753080000000400000073007000300034000C20"
                    + "000001000000000000000100000008000000040000000990E962E55D8253080000000400000065006400300035000C20000001"
                    + "000000000000000100000008000000060000000990E9621F75A74ED34E935E080000000400000073007000300036000C200000"
                    + "01000000000000000100000008000000040000000990E962015ECD79080000000400000065006400300037000C200000010000"
                    + "00000000000100000008000000040000000990E962B6720160080000000400000072006700300038000C200000010000000000"
                    + "00000100000008000000060000000990E962A18B1252BA4E5854080000000400000065006400300039000C2000000100000000"
                    + "0000000100000008000000060000000990E962D1533E65B9650F5F0800000005000000630062006F00310030000C2000000100"
                    + "00000000000001000000080000000B000000D1533E65C7912D8D5553F74E3A4EF6968476A18B12520800000005000000630068"
                    + "006B00310031000C20000001000000000000000100000008000000130000008498A44E278DE5650F5C8E4ED1533E65E5651F67"
                    + "F46639650E4ED1533E65E5651F67F8760C540800000005000000630068006B00310032000C2000000100000000000000010000"
                    + "000800000004000000938F655155532B52080000000400000065006400310033000C2000000100000000000000010000000800"
                    + "00000600000007639A5BD1533E65E5651F6708000000040000006D006500310034000C20000001000000000000000100000008"
                    + "00000008000000938F655107639A5BC7912D8DBA4E5854080000000400000065006400310035000C2000000100000000000000"
                    + "010000000800000008000000C7912D8DA18B1252D1533E659D4F6E63080000000400000065006400310036000C200000010000"
                    + "00000000000100000008000000060000000963C7912D8DE565D1533E650800000005000000630068006B00310037000C200000"
                    + "01000000000000000100000008000000080000000990E962C7912D8DBA4E5854B9650F5F0800000005000000630062006F0032"
                    + "0030000C20000001000000000000000100000008000000080000000990E962C154F753C7912D8DBA4E58540800000004000000"
                    + "73007000310038000C2000000100000000000000010000000800000006000000938F6551F78B2D8DBA4E585408000000040000"
                    + "0065006400310039000C2000000100000000000000010000000800000004000000CD91D6535553F74E08000000050000006300"
                    + "68006B00320031000C2000000100000000000000010000000800000006000000938F6551416D0B7A167FF75308000000040000"
                    + "0065006400320033000C2000000100000000000000010000000800000006000000938F6551416D0B7A167FF753080000000400"
                    + "000065006400320033000C2000000100000000000000010000000800000008000000C7912D8D74655553476C3B60D653F74E08"
                    + "00000005000000630068006B00320032000C20000001000000000000000100000008000000060000000990E962C7912D8DBA4E"
                    + "5854080000000400000065006400320034000C200000010000000000000001000000080000000600000007639A5BC7912D8DBA"
                    + "4E58540800000005000000630068006B00320035000C20000001000000000000000100000008000000070000001F7510624C00"
                    + "6F006700876563680800000006000000630068006B004C006F0067000C20000001000000000000000100000008000000040000"
                    + "005C4F1A4EE5651F67080000000B000000650064005000720069006E00740044006100740065000C2000000100000001000000"
                    + "1C0000000C20000001000000000000000200000008000000010000000400080000000000000008000000000000000C20000001"
                    + "000000000000000200000008000000010000000400080000000000000008000000000000000C20000001000000000000000200"
                    + "000008000000010000000400080000000000000008000000000000000C20000001000000000000000100000008000000010000"
                    + "0005000C2000000100000000000000000000000800000018000000{0}{1}08000000000000000C2000000100000"
                    + "00000000002000000080000000100000004000800000000000000080000000000000008000000000000000C200000010000000"
                    + "000000001000000030000000200000008000000020000006851E89008000000000000000C20000001000000000000000100000"
                    + "0030000000300000008000000040000005553EC72D1533E6508000000010000005900080000000100000059000800000004000"
                    + "00033003100300031000800000008000000{2}08000000000000000800000001000000320008000000010000004E000C"
                    + "20000001000000000000000100000003000000010000000800000008000000938F655107639A5BC7912D8DBA4E58540C200000"
                    + "01000000000000000000000008000000010000000500080000000A0000003000300030003000360033007C0030003600310008"
                    + "000000010000004E000800000000000000080000000000000008000000010000004E0008000000000000000800000001000000"
                    + "4E0008000000010000004E000800000000000000";

            string planIdHex = StrToHex(planId.PadRight(20, ' '));
            string planVerHex = StrToHex(planVer);
            string dateStrHex = StrToHex(DateTime.Now.ToString("yyyyMMdd"));

            string jobId = InsertJob("LRPB05", string.Format(hexStr, planIdHex, planVerHex, dateStrHex));

            log(string.Format("Insert Job Layout CG Plan: PlanId: {0}  - JobId: {1} ", planId, jobId));
            return jobId;
        }
        #endregion

        #region 清除计划
        /// <summary>
        /// 清除LRP计划
        /// </summary>
        /// <param name="planId">LRP计划ID</param>
        /// <param name="planVer">计划版本，默认0001</param>
        private void CleanPlan(string planId, string planVer = "0001")
        {
            string jobId = CleanAllPlan(planId, planVer);
            CheckJobDone(jobId);
        }

        private string CleanAllPlan(string planId, string planVer)
        {
            string hexStr = "0x44532056415249414E54202030313030980500000C2000000100000000000000010000000C200000010000000100000011"
                        + "0000000C20000001000000000000000100000008000000040000001F75A74EA18B1252080000000600000067006200300031"
                        + "005F0031000C2000000100000000000000010000000800000004000000C7912D8DA18B125208000000060000006700620030"
                        + "0031005F0032000C2000000100000000000000010000000800000004000000A18B1252B08B555F0800000006000000670062"
                        + "00300031005F0033000C20000001000000000000000100000008000000040000000990E962B6720160080000000400000072"
                        + "006700300032000C20000001000000000000000100000008000000060000000990E962A18B12527962F75308000000040000"
                        + "0073007000300033000C20000001000000000000000100000008000000060000000990E962A18B1252E5651F670800000004"
                        + "00000073007000300034000C20000001000000000000000100000008000000040000000990E962C154F75308000000040000"
                        + "0073007000300035000C20000001000000000000000100000008000000090000000990E962A44E278DE5652F008C5BE55DE5"
                        + "65080000000400000073007000300036000C20000001000000000000000100000008000000040000000990E962E55D825308"
                        + "0000000400000065006400300037000C20000001000000000000000100000008000000040000000990E962D34E935E080000"
                        + "000400000073007000300038000C2000000100000000000000010000000800000003000000E55D5C4FF75308000000060000"
                        + "0067006200300031005F0034000C20000001000000000000000100000008000000040000000990E9622760288D0800000005"
                        + "000000630062006F00310031000C200000010000000000000001000000080000000C000000205264960C54004EA18B125279"
                        + "62F7534062096748722C670800000005000000630068006B00310032000C200000010000000000000001000000080000000B"
                        + "000000C54E20526496F25D6851E890D1533E658476A18B12520800000005000000630068006B00310033000C200000010000"
                        + "00000000000100000008000000050000000990E962E55D5C4FF753080000000400000073007000310035000C200000010000"
                        + "00000000000100000008000000070000001F7510624C006F006700876563680800000006000000630068006B004C006F0067"
                        + "000C20000001000000000000000100000008000000040000005C4F1A4EE5651F67080000000B000000650064005000720069"
                        + "006E00740044006100740065000C200000010000000100000011000000080000000100000059000800000001000000590008"
                        + "0000000100000059000C200000010000000000000001000000030000000200000008000000020000006851E8900C20000001"
                        + "0000000000000001000000080000000100000005000C2000000100000000000000000000000800000018000000"
                        + "{0}{1}0C200000010000000000000002000000080000000100000004000800000000000000080000000000000"
                        + "00C20000001000000000000000200000008000000010000000400080000000000000008000000000000000C2000000100000"
                        + "0000000000200000008000000010000000400080000000000000008000000000000000800000002000000300031000C20000"
                        + "00100000000000000020000000800000001000000040008000000000000000800000000000000080000000100000059000C2"
                        + "000000100000000000000010000000300000004000000080000000400000034002E006851E89008000000010000004E00080"
                        + "00000010000004E000C200000010000000000000002000000080000000100000004000800000000000000080000000000000"
                        + "008000000010000004E000800000000000000";

            string planIdHex = StrToHex(planId.PadRight(20, ' '));
            string planVerHex = StrToHex(planVer);

            string jobId = InsertJob("LRPB06", string.Format(hexStr, planIdHex, planVerHex));
            log(string.Format("Insert Job Clean Plan: PlanId: {0}  - JobId: {1} ", planId, jobId));
            return jobId;
        }
        #endregion
    }
}
