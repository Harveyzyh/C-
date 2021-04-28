using System;
using HarveyZ;
using System.Data;
using System.Threading;

namespace ERP定时任务
{
    class AutoLrpPlan2
    {
        private Mssql mssql = null;
        private string connYF = null;
        private Logger logger = null;


        public AutoLrpPlan2(Mssql mssql, string connYF, Logger logger)
        {
            this.mssql = mssql;
            this.connYF = connYF;
            this.logger = logger;
        }

        public void MainWork(string planDd)
        {
            log("Work Start!");

            try
            {
                Work(planDd);
            }
            catch (Exception e)
            {
                log("Work Error: \n" + e.ToString());
            }
            finally
            {
                ResetInvmbInfo();
                log("Work Finished! \n");
            }
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("AutoLrpPlan-SingleUsages: " + text);
        }

        #region 共用逻辑
        /// <summary>
        /// 字符串转16进制字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>16进制字符串</returns>
        private string StrToHex(string str)//字符串转16进制
        {
            string returnStr = "";
            foreach (char c in str)
            {
                string hex = Convert.ToString(c, 16).Replace("0x", "").Replace("0X", "").PadLeft(2, '0').ToUpper();
                if (hex.Length == 2)
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
        #endregion

        private void Work(string planDd2)
        {
            string planId = "RobotPD";
            string planDd = "2299-PD         0001";

            //以下为处理主逻辑
            log(string.Format("Work With PlanDd:{0}  - PlanId:{1}", planDd2, planId));

            SetOrderInfo(planDd2);

            SetInvmbInfo();

            GeneratePlan(planDd, planId);

            LayoutPlan(planId);

            ResetInvmbInfo();

            SetSingleUsages(planDd2);

            DeleteMocta();
            
        }

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

        #region 发放计划
        /// <summary>
        /// 发放LRP计划
        /// </summary>
        /// <param name="planId">LRP计划ID</param>
        /// <param name="planVer">计划版本，默认0001</param>
        private void LayoutPlan(string planId, string planVer = "0001")
        {
            string jobId = LayoutScPlan(planId, planVer);
            CheckJobDone(jobId);
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

        #region 修改订单信息
        private void SetOrderInfo(string planDd)
        {
            string sqlStr1 = @"UPDATE COPTC SET 
	                                COPTC.CREATOR=A.CREATOR, COPTC.CREATE_DATE=A.CREATE_DATE, COPTC.USR_GROUP=A.USR_GROUP, 
	                                COPTC.TC003=A.TC003, COPTC.TC004=A.TC004, 
	                                COPTC.TC005=A.TC005, COPTC.TC006=A.TC006, COPTC.TC007=A.TC007, COPTC.TC008=A.TC008, COPTC.TC009=A.TC009, 
	                                COPTC.TC010=A.TC010, COPTC.TC011=A.TC011, COPTC.TC012=A.TC012, COPTC.TC013=A.TC013, COPTC.TC014=A.TC014, 
	                                COPTC.TC015=A.TC015, COPTC.TC016=A.TC016, COPTC.TC017=A.TC017, COPTC.TC018=A.TC018, COPTC.TC019=A.TC019, 
	                                COPTC.TC020=A.TC020, COPTC.TC021=A.TC021, COPTC.TC022=A.TC022, COPTC.TC023=A.TC023, COPTC.TC024=A.TC024, 
	                                COPTC.TC025=A.TC025, COPTC.TC026=A.TC026, COPTC.TC027=A.TC027, COPTC.TC028=A.TC028, COPTC.TC029=A.TC029, 
	                                COPTC.TC030=A.TC030, COPTC.TC031=A.TC031, COPTC.TC032=A.TC032, COPTC.TC033=A.TC033, COPTC.TC034=A.TC034, 
	                                COPTC.TC035=A.TC035, COPTC.TC036=A.TC036, COPTC.TC037=A.TC037, COPTC.TC038=A.TC038, COPTC.TC039=A.TC039, 
	                                COPTC.TC040=A.TC040, COPTC.TC041=A.TC041, COPTC.TC042=A.TC042, COPTC.TC043=A.TC043, COPTC.TC044=A.TC044, 
	                                COPTC.TC045=A.TC045, COPTC.TC046=A.TC046, COPTC.TC047=A.TC047, COPTC.TC048=A.TC048, COPTC.TC049=A.TC049, 
	                                COPTC.TC050=A.TC050, COPTC.TC051=A.TC051, COPTC.TC052=A.TC052, COPTC.TC053=A.TC053, COPTC.TC054=A.TC054, 
	                                COPTC.TC055=A.TC055, COPTC.TC056=A.TC056, COPTC.TC057=A.TC057, COPTC.TC058=A.TC058, COPTC.TC059=A.TC059, 
	                                COPTC.TC060=A.TC060, COPTC.TC061=A.TC061, COPTC.TC062=A.TC062, COPTC.TC063=A.TC063, COPTC.TC064=A.TC064, 
	                                COPTC.TC065=A.TC065, COPTC.TC066=A.TC066, COPTC.TC067=A.TC067, COPTC.TC068=A.TC068, COPTC.TC069=A.TC069, 
	                                COPTC.TC070=A.TC070, COPTC.TC071=A.TC071, COPTC.TC072=A.TC072, COPTC.TC073=A.TC073, COPTC.TC074=A.TC074, 
	                                COPTC.TC075=A.TC075, COPTC.TC076=A.TC076, COPTC.TC077=A.TC077, 
	                                COPTC.TCI01=A.TCI01, COPTC.TCI02=A.TCI02, COPTC.TCI03=A.TCI03, COPTC.TCI04=A.TCI04, 
	                                COPTC.UDF01=A.UDF01, COPTC.UDF02=A.UDF02, COPTC.UDF03=A.UDF03, COPTC.UDF04=A.UDF04, COPTC.UDF05=A.UDF05, 
	                                COPTC.UDF06=A.UDF06, COPTC.UDF07=A.UDF07, COPTC.UDF08=A.UDF08, COPTC.UDF09=A.UDF09, COPTC.UDF10=A.UDF10, 
	                                COPTC.UDF11=A.UDF11, COPTC.UDF12=A.UDF12, 
	                                COPTC.UDF51=A.UDF51, COPTC.UDF52=A.UDF52, COPTC.UDF53=A.UDF53, COPTC.UDF54=A.UDF54, COPTC.UDF55=A.UDF55, 
	                                COPTC.UDF56=A.UDF56, COPTC.UDF57=A.UDF57, COPTC.UDF58=A.UDF58, COPTC.UDF59=A.UDF59, COPTC.UDF60=A.UDF60, 
	                                COPTC.UDF61=A.UDF61, COPTC.UDF62=A.UDF62, 
	                                COPTC.TC200=A.TC200, COPTC.TC201=A.TC201, COPTC.TC202=A.TC202
                                FROM COPTC 
                                INNER JOIN COPTD AS B ON 1=1 AND RTRIM(B.TD001)+'-'+B.TD002+B.TD003 = '{0}'
                                INNER JOIN COPTC AS A ON 1=1 AND A.TC001 = B.TD001 AND A.TC002 = B.TD002
                                WHERE 1=1
                                AND COPTC.TC001 = '2299' AND COPTC.TC002 = 'PD'";

            string sqlStr2 = @"UPDATE COPTD SET 
	                                COPTD.CREATOR=A.CREATOR, COPTD.CREATE_DATE=A.CREATE_DATE, COPTD.USR_GROUP=A.USR_GROUP, 
	                                COPTD.TD004=A.TD004, 
	                                COPTD.TD005=A.TD005, COPTD.TD006=A.TD006, COPTD.TD007=A.TD007,  
	                                COPTD.TD010=A.TD010, COPTD.TD011=A.TD011, COPTD.TD012=A.TD012, COPTD.TD013=A.TD013, COPTD.TD014=A.TD014, 
	                                COPTD.TD015=A.TD015, COPTD.TD016=A.TD016, COPTD.TD017=A.TD017, COPTD.TD018=A.TD018, COPTD.TD019=A.TD019, 
	                                COPTD.TD020=A.TD020, COPTD.TD021=A.TD021, COPTD.TD022=A.TD022, COPTD.TD023=A.TD023, COPTD.TD024=A.TD024, 
	                                COPTD.TD025=A.TD025, COPTD.TD026=A.TD026, COPTD.TD027=A.TD027, COPTD.TD028=A.TD028, COPTD.TD029=A.TD029, 
	                                COPTD.TD030=A.TD030, COPTD.TD031=A.TD031, COPTD.TD032=A.TD032, COPTD.TD033=A.TD033, COPTD.TD034=A.TD034, 
	                                COPTD.TD035=A.TD035, COPTD.TD036=A.TD036, COPTD.TD037=A.TD037, COPTD.TD038=A.TD038, COPTD.TD039=A.TD039, 
	                                COPTD.TD040=A.TD040, COPTD.TD041=A.TD041, COPTD.TD042=A.TD042, COPTD.TD043=A.TD043, COPTD.TD044=A.TD044, 
	                                COPTD.TD045=A.TD045, COPTD.TD046=A.TD046, COPTD.TD047=A.TD047, COPTD.TD048=A.TD048, COPTD.TD049=A.TD049, 
	                                COPTD.TD050=A.TD050, COPTD.TD051=A.TD051, COPTD.TD052=A.TD052, COPTD.TD053=A.TD053, COPTD.TD054=A.TD054, 
	                                COPTD.TD055=A.TD055, COPTD.TD056=A.TD056, COPTD.TD057=A.TD057, COPTD.TD058=A.TD058, COPTD.TD059=A.TD059, 
	                                COPTD.TD060=A.TD060, COPTD.TD061=A.TD061, COPTD.TD062=A.TD062, 
	
	                                COPTD.UDF01=A.UDF01, COPTD.UDF02=A.UDF02, COPTD.UDF03=A.UDF03, COPTD.UDF04=A.UDF04, COPTD.UDF05=A.UDF05, 
	                                COPTD.UDF06=A.UDF06, COPTD.UDF07=A.UDF07, COPTD.UDF08=A.UDF08, COPTD.UDF09=A.UDF09, COPTD.UDF10=A.UDF10, 
	                                COPTD.UDF11=A.UDF11, COPTD.UDF12=A.UDF12, 
	                                COPTD.UDF51=A.UDF51, COPTD.UDF52=A.UDF52, COPTD.UDF53=A.UDF53, COPTD.UDF54=A.UDF54, COPTD.UDF55=A.UDF55, 
	                                COPTD.UDF56=A.UDF56, COPTD.UDF57=A.UDF57, COPTD.UDF58=A.UDF58, COPTD.UDF59=A.UDF59, COPTD.UDF60=A.UDF60, 
	                                COPTD.UDF61=A.UDF61, COPTD.UDF62=A.UDF62, 
	
	                                COPTD.TD200=A.TD200, COPTD.TD201=A.TD201, COPTD.TD202=A.TD202, COPTD.TD203=A.TD203, COPTD.TD204=A.TD204 
                                FROM COPTD 
                                INNER JOIN COPTD AS A ON 1=1 AND RTRIM(A.TD001)+'-'+A.TD002+A.TD003 = '{0}'
                                WHERE 1=1
                                AND COPTD.TD001 = '2299' AND COPTD.TD002 = 'PD' AND COPTD.TD003 = '0001'";


            mssql.SQLexcute(connYF, string.Format(sqlStr1, planDd));
            mssql.SQLexcute(connYF, string.Format(sqlStr2, planDd));
        }
        #endregion

        #region 写入单用量明细
        private void SetSingleUsages(string planDd)
        {
            string sqlStr1 = @"exec dbo.P_SetMOCTBList '{0}'";
            string sqlStr2 = @"exec dbo.P_SetMOCTBGroup '{0}'";

            mssql.SQLexcute(connYF, string.Format(sqlStr1, planDd));
            mssql.SQLexcute(connYF, string.Format(sqlStr2, planDd));
        }
        #endregion

        #region 修改品号信息领用倍量
        private void SetInvmbInfo()
        {
            string sqlStr1 = @" UPDATE INVMB SET UDF62 = MB041 ";
            string sqlStr2 = @"UPDATE INVMB SET MB041 = 0
                                FROM BOMMD 
                                INNER JOIN INVMB ON MD003 = MB001 AND MB041 = 1 AND MB025 = 'P' AND MB109 = 'Y'
                                WHERE 1=1
                                AND CAST(CAST(MD006/MD007 AS INT) AS FLOAT) != MD006/MD007";
            mssql.SQLexcute(connYF, sqlStr1);
            //mssql.SQLexcute(connYF, sqlStr2);
        }

        private void ResetInvmbInfo()
        {
            string sqlStr1 = @" UPDATE INVMB SET MB041 = UDF62 ";
            mssql.SQLexcute(connYF, sqlStr1);
        }
        #endregion

        #region 删除工单
        private void DeleteMocta()
        {
            string sqlStr = @"DELETE FROM MOCTB WHERE RTRIM(TB001)+'-'+RTRIM(TB002) IN (SELECT RTRIM(TA001)+'-'+RTRIM(TA002) FROM MOCTA WHERE TA026 = '2299' AND TA027 = 'PD' AND TA028 = '0001')
                            DELETE FROM MOCTA WHERE TA026 = '2299' AND TA027 = 'PD' AND TA028 = '0001'";
            mssql.SQLexcute(connYF, sqlStr);
        }
        #endregion
    }
}
