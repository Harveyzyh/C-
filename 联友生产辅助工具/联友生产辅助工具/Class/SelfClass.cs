using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace HarveyZ
{
    public class CEncrypt
    {
        public string GetMd5Str(string str)     //取得字符串str被MD5加密算法加密后的字符串
        {
            string resultstr = "";
            System.Security.Cryptography.MD5 myMD5 = System.Security.Cryptography.MD5.Create();
            Byte[] mybyte;
            mybyte = myMD5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            for (int itmp = 0; itmp < mybyte.Length; itmp++)
            {
                resultstr += mybyte[itmp].ToString("X");
            }
            return resultstr;
        }
    }


    public class UpdateMe
    {
        string _conStr;
        public UpdateMe(string conStr)
        {
            _conStr = conStr;
        }

        public void Update()
        {
            string prodName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.Trim();
            string prodVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string prodPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            string pid = p.Id.ToString();
            bool _needUpdateFlag = false;

            using (SqlConnection conSqlSvr = new SqlConnection(_conStr))
            {
                conSqlSvr.Open();
                string sqlVersion = "select ProgVersion from daruenfa.dbo.ProgInf Where (ProgName='" + prodName + "' or ProgName='" + prodName + ".exe')";
                DataTable dt = null;// CConDB.GetDataFromSqlSvr(sqlVersion, conSqlSvr);
                if (dt == null)
                {
                    _needUpdateFlag = false;
                }
                else if (dt.Rows[0]["ProgVersion"].ToString().Trim() == prodVersion.Trim())
                {
                    _needUpdateFlag = false;
                }
                else
                {
                    _needUpdateFlag = true;
                }
                conSqlSvr.Close();
                conSqlSvr.Dispose();
            }
            if (_needUpdateFlag)
            {
                System.Diagnostics.Process.Start(prodPath + "\\" + "程序更新.exe", "\"" + prodName.Trim() + "\" \"" + prodVersion.Trim() + "\" \"" + prodPath + "\" \"" + _conStr + "\" \"" + pid + "\"");
            }
        }
    }


    public class Mssql
    {
        /// <summary>
        /// 数据库连接测试
        /// </summary>
        /// <param name="strConnection">数据库连接字</param>
        public bool SQLlinkTest(string strConnection) //数据库连接测试
        {
            bool CanConnectDB = false;
            using (SqlConnection testConnection = new SqlConnection(strConnection))
            {
                try
                {
                    testConnection.Open();
                    CanConnectDB = true;
                    testConnection.Close();
                    testConnection.Dispose();
                }
                catch { }
                if (CanConnectDB)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 数据库-增，改，删
        /// </summary>
        /// <param name="SQLstr">数据库连接字符串</param>
        /// <param name="CMDstr">本数据库中表示DeviceID</param>
        public int SQLexcute(string SQLstr, string CMDstr)
        {
            using (SqlConnection conn = new SqlConnection(SQLstr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(CMDstr, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    return 0;
                }
                catch (Exception es)
                {
                    MessageBox.Show("SQL Commit 出错了！\r\n" + SQLstr + "\r\n\r\n\r\n" + es.ToString(), "提示", MessageBoxButtons.OK);
                    return 1;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }


        /// <summary>
        /// 数据库-查
        /// </summary>
        /// <param name="SQLstr">数据库连接字符串</param>
        /// <param name="CMDstr">本数据库中表示DeviceID</param>
        public DataTable SQLselect(string SQLstr, string CMDstr)
        {
            using (SqlConnection conn = new SqlConnection(SQLstr))
            {
                try
                {
                    //conn.Open();
                    //SqlCommand cmd = new SqlCommand(CMDstr, conn);
                    DataTable dttmp = new DataTable();
                    SqlDataAdapter sdatmp = new SqlDataAdapter(CMDstr, conn);
                    sdatmp.Fill(dttmp);
                    sdatmp.Dispose();
                    if (dttmp.Rows.Count <= 0)
                    {
                        return null;
                    }
                    else
                    {
                        return dttmp;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("执行失败(" + ex.Message + ")，请退出后重新进入！", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return null;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
    }


    public class Form_Opt
    {
        public static Form Form_Init(Form sender)
        {
            Form frm = sender;
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.WindowState = FormWindowState.Maximized;
            return frm;
        }
    }


    public class WebNet
    {
        public Dictionary<string, string> WebPost(string url, Dictionary<string, string> dict)
        {
            try
            {
                var client = new WebClient();

                string json = DictJson.Dict2Json(dict);

                string response = client.UploadString(url, json);
                dict = DictJson.Json2Dict(response);
                return dict;
            }
            catch
            {
                return null;
            }
        }
    }


    public class Excel
    {
        private Excel_Base newExcel = null;
        
        #region 基类
        public class Excel_Base
        {
            private string filePath = null;//路径
            private string fileName = null;//文件名
            private int sheetIndex = 0;
            private bool isTitleRow = false;//是否有首行
            private bool isWrite = false;//是否为写入模式
            private DataTable titleDt = null;//表头内容
            private DataTable titleFormat = null;//表头格式
            private DataTable cellDt = null;//数据内容

            public string FilePath
            {
                get
                {
                    return filePath;
                }
                set
                {
                    filePath = value;
                }
            }

            public string FileName
            {
                get
                {
                    return fileName;
                }
                set
                {
                    fileName = value;
                }
            }

            public int SheetIndex
            {
                get
                {
                    return sheetIndex;
                }
                set
                {
                    if(value >= 0)
                    {
                        sheetIndex = value;
                    }
                }
            }

            public bool IsTitleRow
            {
                get
                {
                    return isTitleRow;
                }
                set
                {
                    isTitleRow = value;
                }
            }

            public bool IsWrite
            {
                get
                {
                    return isWrite;
                }
                set
                {
                    isWrite = value;
                }
            }

            public DataTable TitleDt
            {
                get
                {
                    return titleDt;
                }
                set
                {
                    titleDt = value;
                }
            }

            public DataTable TitleFormat
            {
                get
                {
                    return titleFormat;
                }
                set
                {
                    titleFormat = value;
                }
            }

            public DataTable CellDt
            {
                get
                {
                    return cellDt;
                }
                set
                {
                    cellDt = value;
                }
            }
        }
        #endregion

        #region Excel操作判断及分类处理
        public void ExcelOpt(object obj)
        {
            newExcel = (Excel_Base)obj;
            if (newExcel.IsWrite)
            {
                if(newExcel.CellDt == null)
                {
                    MessageBox.Show("保存的Excel内容为空！", "错误");
                }
                else
                {
                    OutportExcel();
                }
            }
            else
            {
                ImportExcel();
            }
        }
        #endregion

        #region 读Excel
        private void ImportExcel()
        {
            string fileName = newExcel.FileName;
            string filePath = newExcel.FilePath;
            int sheetIndex = newExcel.SheetIndex;
            DataTable cellDt = new DataTable();
            ICell cell = null;
            int rowIndex = 0;


            string path = Path.Combine(filePath, fileName);


            using (FileStream fsRead = File.OpenRead(path))
            {
                IWorkbook wk = null;
                //获取后缀名
                string extension = path.Substring(path.LastIndexOf(".")).ToString().ToLower();
                //判断是否是excel文件
                if (extension == ".xlsx" || extension == ".xls")
                {
                    //判断excel的版本
                    if (extension == ".xlsx")
                    {
                        wk = new XSSFWorkbook(fsRead);
                    }
                    else
                    {
                        wk = new HSSFWorkbook(fsRead);
                    }

                    //获取第一个sheet
                    ISheet sheet = wk.GetSheetAt(sheetIndex);
                    //获取第一行
                    IRow headrow = sheet.GetRow(0);


                    //创建列
                    for (int i = headrow.FirstCellNum; i < headrow.Cells.Count; i++)
                    {
                        if (newExcel.IsTitleRow)
                        {
                            cell = headrow.GetCell(i);
                            cellDt.Columns.Add(GetCellValue(cell).Replace("\n", ""));
                            rowIndex = 1;
                        }
                        else
                        {
                            cellDt.Columns.Add("Col" + (i + 1).ToString());
                            rowIndex = 0;
                        }
                        
                    }

                    
                    //读取每行,从第二行起
                    for (int r = rowIndex; r <= sheet.LastRowNum; r++)
                    {
                        bool result = false;
                        DataRow dr = cellDt.NewRow();
                        //获取当前行
                        IRow row = sheet.GetRow(r);
                        //读取每列
                        for (int j = 0; j < row.Cells.Count; j++)
                        {
                            cell = row.GetCell(j); //一个单元格
                            dr[j] = GetCellValue(cell); //获取单元格的值
                                                        //全为空则不取
                            if (dr[j].ToString() != "")
                            {
                                result = true;
                            }
                        }
                        if (result == true)
                        {
                            cellDt.Rows.Add(dr); //把每行追加到DataTable
                        }
                    }
                }
                newExcel.CellDt = cellDt;
            }
        }

        //对单元格进行判断取值
        private string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank: //空数据类型 这里类型注意一下，不同版本NPOI大小写可能不一样,有的版本是Blank（首字母大写)
                    return string.Empty;
                case CellType.Boolean: //bool类型
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric: //数字类型
                    if (HSSFDateUtil.IsCellDateFormatted(cell))//日期类型
                    {
                        return cell.DateCellValue.ToString("yyyy-MM-dd");
                    }
                    else //其它数字
                    {
                        return cell.NumericCellValue.ToString();
                    }
                case CellType.Unknown: //无法识别类型
                default: //默认类型
                    return cell.ToString();//
                case CellType.String: //string 类型
                    return cell.StringCellValue;
                case CellType.Formula: //带公式类型
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
        #endregion

        #region 写Excel
         private void OutportExcel()
        {
            string fileName = newExcel.FileName;
            string filePath = newExcel.FilePath;
            int sheetIndex = newExcel.SheetIndex;
            DataTable cellDt = newExcel.CellDt;
            DataTable formatDt = newExcel.TitleFormat;

            string path = Path.Combine(filePath, fileName);


            IWorkbook workbook;
            string fileExt = Path.GetExtension(path).ToLower();
            if (fileExt == ".xlsx") { workbook = new XSSFWorkbook(); } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(); } else { workbook = null; }
            if (workbook == null) { return; }
            ISheet sheet = string.IsNullOrEmpty(cellDt.TableName) ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet(cellDt.TableName);

            //表头  
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < cellDt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(cellDt.Columns[i].ColumnName);
                //if(formatDt != null) // 列宽
                //{

                //}
            }

            //数据  
            for (int i = 0; i < cellDt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < cellDt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(cellDt.Rows[i][j].ToString());
                }
            }

            //转为字节数组  
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件  
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }
        #endregion
    }


    public class AESEncryption
    {
        //默认密钥向量 
        private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };


        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <param name="strKey">密钥</param>
        /// <returns>返回加密后的密文字节数组</returns>
        public static byte[] AESEncrypt(string plainText, string strKey)
        {
            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组
            des.Key = Encoding.UTF8.GetBytes(strKey);//设置密钥及密钥向量
            des.IV = _key1;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组
            cs.Close();
            ms.Close();
            return cipherBytes;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字节数组</param>
        /// <param name="strKey">密钥</param>
        /// <returns>返回解密后的字符串</returns>
        public static byte[] AESDecrypt(byte[] cipherText, string strKey)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.IV = _key1;
            byte[] decryptBytes = new byte[cipherText.Length];
            MemoryStream ms = new MemoryStream(cipherText);
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
            cs.Read(decryptBytes, 0, decryptBytes.Length);
            cs.Close();
            ms.Close();
            return decryptBytes;
        }
    }


    public class DictJson
    {
        /// <summary>
        /// 将字典类型序列化为json字符串
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="dict">要序列化的字典数据</param>
        /// <returns>json字符串</returns>
        public static string Dict2JsonNull<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict.Count == 0)
                return "";

            string jsonStr = JsonConvert.SerializeObject(dict);
            return jsonStr;
        }

        public static string Dict2Json(Dictionary<string, string> dict)
        {
            if (dict.Count == 0)
                return "";

            string jsonStr = JsonConvert.SerializeObject(dict);
            return jsonStr;
        }

        /// <summary>
        /// 将json字符串反序列化为字典类型
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="jsonStr">json字符串</param>
        /// <returns>字典数据</returns>
        public static Dictionary<TKey, TValue> Json2DictNull<TKey, TValue>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new Dictionary<TKey, TValue>();

            Dictionary<TKey, TValue> dict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);

            return dict;

        }

        public static Dictionary<string, string> Json2Dict(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new Dictionary<string, string>();

            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);

            return dict;

        }
    }
}
