using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HarveyZ
{
    class Bom
    {
        private Mssql mssql = new Mssql();
        private string connYWGDB = Global_Const.strConnection_Y_WGDB;
        private string connYCOMFORT = Global_Const.strConnection_Y_COMFORT;

        BomObjectBase bomObj = null;

        private class BomObjectBase
        {
            private bool typeC = false;
            private bool craftwork = false;
            private DataTable resultDt = null;

            public bool TypeC { get { return typeC; } set { typeC = value; } }
            public bool Craftwork { get { return craftwork; } set { craftwork = value; } }
            public DataTable ResultDt { get { return resultDt; } set { resultDt = value; } }
        }

        public DataTable GetBom(string material = null, bool typeC = false, bool craftwork = false, bool getAll = false)
        {
            bomObj = new BomObjectBase();
            bomObj.TypeC = typeC;
            bomObj.Craftwork = craftwork;
            if (getAll)
            {
                InitResultDt(bomObj.ResultDt);
                GetBomDt(material: material);
            }
            return null;
        }

        private DataTable GetBomDtSelect(string material)
        {
            string sqlstr = @" SELECT RTRIM(CB005) 品号, CAST(CB008 AS FLOAT)/CAST(CB009 AS FLOAT) 用量, MB025 品号属性";
            if (bomObj.Craftwork) sqlstr += @" , CB011 工艺 ";
            sqlstr += @" FROM BOMCB INNER JOIN INVMB  ON MB001= CB005 WHERE 1=1 
            AND MB109 = 'Y' 
            AND (CB013 <= CONVERT(VARCHAR(20), GETDATE(), 112) OR CB013 IS NULL OR RTRIM(CB013) = '') 
            AND (CB014 > CONVERT(VARCHAR(20), GETDATE(), 112) OR CB014 IS NULL OR RTRIM(CB014) = '') 
            AND CB001 = '{0}' ";
            if (bomObj.TypeC) sqlstr += @" AND CB05 = 'Y' ";
            sqlstr += @"ORDER BY CB004 ";
            DataTable dt = mssql.SQLselect(connYCOMFORT, string.Format(sqlstr, material));
            return dt;
        }

        private void GetBomDt(string material = null, DataTable dtTmp =null, string coefficient = "1.0")
        {
            int coefficients = int.Parse(coefficient);
            if (dtTmp == null) InitResultDt(dtTmp);
            
        }

        private DataTable GetMaterialInfo(string material)
        {
            string sqlstr = @"SELECT RTRIM(MB004), RTRIM(MB002), RTRIM(MB003), RTRIM(MB032), RTRIM(MB200) FROM INVMB WHERE MB001 = '{0}' ";
            return mssql.SQLselect(connYCOMFORT, string.Format(sqlstr, material));
        }

        private void InitResultDt(DataTable dt)
        {
            if (dt == null) dt = new DataTable();
            if (dt != null && dt.Columns.Count == 0)
            {
                dt.Columns.Add("品号", Type.GetType("System.String"));
                dt.Columns.Add("用量", Type.GetType("System.Float"));
                dt.Columns.Add("品号属性", Type.GetType("System.String"));
                if (bomObj.Craftwork) dt.Columns.Add("工艺", Type.GetType("System.String"));
            }
        }
    }
}
