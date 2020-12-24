using System.Data;
using HarveyZ;
using System;

namespace ERP定时任务
{
    public class InvmbBoxSize
    {
        private Mssql mssql = null;
        private string connYF = null;
        private Logger logger = null;

        public bool workFlag = false;

        public InvmbBoxSize(Mssql mssql, string connYF, Logger logger)
        {
            this.mssql = mssql;
            this.connYF = connYF;
            this.logger = logger;
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("GetInvmbBoxSize: " + text);
        }

        public void MainWork()
        {
            if (ERP定时任务.GetConfig("INVMBBoxSize", "Work"))
            {
                workFlag = true;
                log("Work Start!");
                try
                {
                    BoxSizeSelect();
                }
                catch (Exception e)
                {
                    log("Work Error.\n\r\t" + e.ToString());
                }
                finally
                {

                    log("Work Finished!");
                    workFlag = false;
                }
            }
        }

        private void BoxSizeSelect()
        {
            string sqlStr = "SELECT RTRIM(MB001) 品号, RTRIM(MB003) 规格 FROM dbo.INVMB WHERE 1=1 AND MB025 = 'P' "
                          + "AND MB109 = 'Y' AND (MB002 LIKE '%纸箱%' OR MB002 LIKE '%彩盒%' OR MB002 LIKE '%天地盖%') ORDER BY MB001 ";

            DataTable dt = mssql.SQLselect(connYF, sqlStr);
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    string wlno = dt.Rows[rowIndex]["品号"].ToString();
                    string spec = dt.Rows[rowIndex]["规格"].ToString();
                    if (Normal.GetSubstringCount(spec, "*") < 2)
                    {
                        continue;
                    }
                    else
                    {
                        string sizeStr = spec.Split('/')[1];
                        if (Normal.GetSubstringCount(sizeStr, "*") == 2)
                        {
                            int L = 0;
                            int W = 0;
                            int H = 0;
                            try
                            {
                                L = int.Parse(sizeStr.Split('*')[0].Split('(')[0].Split('（')[0]);
                                W = int.Parse(sizeStr.Split('*')[1].Split('(')[0].Split('（')[0]);
                                H = int.Parse(sizeStr.Split('*')[2].Split('(')[0].Split('（')[0]);
                            }
                            catch
                            {
                                continue;
                            }
                            finally
                            {
                                if (L != 0 && H != 0 && W != 0)
                                {
                                    BoxSizeUpdate(wlno, L.ToString() + "*" + H.ToString() + "*" + W.ToString(), L * H * W);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BoxSizeUpdate(string mb001, string mbu01, int mbu02)
        {
            string sqlStr = "update dbo.INVMB SET MBU01='{1}', MBU02={2} WHERE MB001 = '{0}' ";
            mssql.SQLexcute(connYF, string.Format(sqlStr, mb001, mbu01, mbu02));
        }
    }
}
