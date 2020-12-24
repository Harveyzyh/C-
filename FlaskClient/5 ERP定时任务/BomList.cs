using System;
using HarveyZ;
using System.Data;

namespace ERP定时任务
{
    class BomList
    {
        private Mssql mssql = null;
        private string connYF = null;
        private Logger logger = null;

        public bool workFlag = false;

        public BomList(Mssql mssql, string connYF, Logger logger)
        {
            this.mssql = mssql;
            this.connYF = connYF;
            this.logger = logger;
        }
        public void MainWork()
        {
            if (ERP定时任务.GetConfig("GetBomList", "Work"))
            {
                workFlag = true;
                log("Work Start!");

                try
                {
                    Work();
                }
                catch (Exception e)
                {
                    log("Work Error: \n" + e.ToString());
                }
                finally
                {

                    log("Work Finished!");
                    workFlag = false;
                }
            }
        }

        private void log(string text)
        {
            logger.Instance.WriteLog("GetBomList: " + text);
        }

        private void Work()
        {
            string sqlStr = "SELECT RTRIM(MB001) 品号 FROM INVMB(NOLOCK) INNER JOIN BOMCA(NOLOCK) ON CA003 = MB001 " 
                          + "WHERE MB025 = 'M' AND MB109 = 'Y' AND (MB001 LIKE '1%' OR MB001 LIKE '2%') "
                          + "ORDER BY MB001 ";

            DataTable wlnoDt = mssql.SQLselect(connYF, sqlStr);
            if (wlnoDt != null)
            {
                string count = wlnoDt.Rows.Count.ToString();
                log("Wlno Count: " + count);

                for (int rowIndex = 0; rowIndex < wlnoDt.Rows.Count; rowIndex ++)
                {
                    string wlno = wlnoDt.Rows[rowIndex]["品号"].ToString();

                    log((rowIndex + 1).ToString() + "/" + count + "  - Wlno: " + wlno);

                    DeleteList(wlno);
                    DataTable bomDt = GetBomListMain(wlno);
                    InsertList(bomDt);
                }
            }
        }

        private void DeleteList(string wlno)
        {
            string sqlStr = "delete from BOMCB_List where CB001 = '{0}'";
            mssql.SQLexcute(connYF, string.Format(sqlStr, wlno));
        }

        private void InsertList(DataTable dt)
        {
            string sqlStr = "INSERT INTO BOMCB_List VALUES('{0}', '{1}', '{2}', {3})";
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    mssql.SQLexcute(connYF, string.Format(sqlStr, dt.Rows[rowIndex]["成品号"].ToString(), dt.Rows[rowIndex]["材料品号"].ToString(),
                        dt.Rows[rowIndex]["工艺"].ToString(), dt.Rows[rowIndex]["用量"].ToString()));
                    //log(string.Format(sqlStr, dt.Rows[rowIndex]["成品号"].ToString(), dt.Rows[rowIndex]["材料品号"].ToString(),
                    //    dt.Rows[rowIndex]["工艺"].ToString(), dt.Rows[rowIndex]["用量"].ToString()));
                }
            }
        }

        private DataTable GetBomListMain(string wlno)
        {
            DataTable bomDt = new DataTable();
            bomDt.Columns.Add("成品号", Type.GetType("System.String"));
            bomDt.Columns.Add("材料品号", Type.GetType("System.String"));
            bomDt.Columns.Add("工艺", Type.GetType("System.String"));
            bomDt.Columns.Add("用量", Type.GetType("System.String"));
            bomDt.Columns.Add("品号属性", Type.GetType("System.String"));

            GetBomList(bomDt, wlno);
            GetListSum(bomDt);

            return bomDt;
        }

        private void GetBomList(DataTable bomDt, string wlno, string material = null, float useSl = 1, bool typeC = false)
        {
            DataTable listDt = new DataTable();
            string sqlStr = @"SELECT RTRIM(CB005) 材料品号, CAST(CB008 AS FLOAT)/CAST(CB009 AS FLOAT) 用量, "
                  + @"MB025 品号属性, CB011 工艺 "
                  + @"FROM BOMCB(NOLOCK) "
                  + @"INNER JOIN INVMB(NOLOCK)  ON MB001= CB005 "
                  + @"WHERE 1=1 "
                  + @"AND MB109 = 'Y' "
                  + @"AND (CB013 <= CONVERT(VARCHAR(20), GETDATE(), 112) OR CB013 IS NULL OR RTRIM(CB013) = '') "
                  + @"AND (CB014 > CONVERT(VARCHAR(20), GETDATE(), 112) OR CB014 IS NULL OR RTRIM(CB014) = '') "
                  + @"AND CB001 = '{0}' ";
		    if (typeC)
            {
                sqlStr += @"AND CB015 = 'Y' ";
            }
            sqlStr += @"ORDER BY CB004";

            if (material == null)
            {
                listDt = mssql.SQLselect(connYF, string.Format(sqlStr, wlno));
            }
            else
            {
                listDt = mssql.SQLselect(connYF, string.Format(sqlStr, material));
            }

            if (listDt != null)
            {
                for(int rowIndex = 0; rowIndex < listDt.Rows.Count; rowIndex++)
                {
                    string material2 = listDt.Rows[rowIndex]["材料品号"].ToString();
                    string gy = listDt.Rows[rowIndex]["工艺"].ToString();
                    float useSl2 = float.Parse(listDt.Rows[rowIndex]["用量"].ToString());
                    float useSlSum = useSl2 * useSl;
                    string typeC2 = listDt.Rows[rowIndex]["品号属性"].ToString();

                    if(typeC2 == "P" || typeC2 == "S")
                    {
                        DataRow dr = bomDt.NewRow();
                        dr["成品号"] = wlno;
                        dr["材料品号"] = material2;
                        dr["工艺"] = gy;
                        dr["用量"] = useSlSum.ToString();
                        bomDt.Rows.Add(dr);
                    }
                    else if ( typeC2 == "C")
                    {
                        GetBomList(bomDt, wlno, material2, useSlSum, true);
                    }
                    else
                    {
                        GetBomList(bomDt, wlno, material2, useSlSum, false);
                    }
                }
            }
        }

        private void GetListSum(DataTable bomDt)
        {
            if(bomDt != null)
            {
                bomDt.Columns.Remove("品号属性");
                DataTable bomDtBak = bomDt.Copy();
                bomDt.Clear();

                DataView sumDv = bomDtBak.DefaultView;
                sumDv.Sort = "成品号 ASC, 材料品号 ASC, 工艺 ASC";
                bomDtBak = sumDv.ToTable();

                string material = "";
                string gy = "";

                for(int rowIndex =0; rowIndex < bomDtBak.Rows.Count; rowIndex++)
                {
                    
                    DataRow dr = bomDt.NewRow();
                    dr["成品号"] = bomDtBak.Rows[rowIndex]["成品号"].ToString();

                    if (material == bomDtBak.Rows[rowIndex]["材料品号"].ToString() && gy == bomDtBak.Rows[rowIndex]["工艺"].ToString())
                    {
                        bomDt.Rows[bomDt.Rows.Count-1]["用量"] = 
                            (float.Parse(bomDtBak.Rows[rowIndex]["用量"].ToString()) + float.Parse(bomDt.Rows[bomDt.Rows.Count - 1]["用量"].ToString())).ToString();
                    }
                    else
                    {
                        material = bomDtBak.Rows[rowIndex]["材料品号"].ToString();
                        gy = bomDtBak.Rows[rowIndex]["工艺"].ToString();
                        dr["材料品号"] = bomDtBak.Rows[rowIndex]["材料品号"].ToString();
                        dr["工艺"] = bomDtBak.Rows[rowIndex]["工艺"].ToString();
                        dr["用量"] = bomDtBak.Rows[rowIndex]["用量"].ToString();
                        bomDt.Rows.Add(dr);
                    }
                }
            }
        }
    }
}
