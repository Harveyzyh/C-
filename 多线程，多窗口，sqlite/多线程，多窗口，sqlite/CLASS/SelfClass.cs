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

public class CEncrypt
{
    public static string GetMd5Str(string str)     //取得字符串str被MD5加密算法加密后的字符串
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
    public static void SQLlinkTest(string strConnection) //数据库连接测试
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
                MessageBox.Show("数据库连接成功！", "提示");
            }
            else
            {
                MessageBox.Show("数据库连接失败！", "提示");
            }
        }
    }


    /// <summary>
    /// 数据库-增，改，删
    /// </summary>
    /// <param name="SQLstr">数据库连接字符串</param>
    /// <param name="CMDstr">本数据库中表示DeviceID</param>
    public static int SQLexcute(string SQLstr, string CMDstr)
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
    public static DataTable SQLselect(string SQLstr, string CMDstr)
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
    public static Dictionary<string, string> WebPost(string url, Dictionary<string, string> dict)
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
    #region 读Excel
    /// <summary>
    /// 获取excel内容
    /// </summary>
    /// <param name="filePath">excel文件路径</param>
    /// <returns></returns>
    public static DataTable ImportExcel(string filePath)
    {
        DataTable dt = new DataTable();
        using (FileStream fsRead = File.OpenRead(filePath))
        {
            IWorkbook wk = null;
            //获取后缀名
            string extension = filePath.Substring(filePath.LastIndexOf(".")).ToString().ToLower();
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
                ISheet sheet = wk.GetSheetAt(0);
                //获取第一行
                IRow headrow = sheet.GetRow(0);
                //创建列
                for (int i = headrow.FirstCellNum; i < headrow.Cells.Count; i++)
                {
                    dt.Columns.Add((i + 1).ToString());
                }
                //读取每行,从第二行起
                for (int r = 0; r <= sheet.LastRowNum; r++)
                {
                    bool result = false;
                    DataRow dr = dt.NewRow();
                    //获取当前行
                    IRow row = sheet.GetRow(r);
                    //读取每列
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        ICell cell = row.GetCell(j); //一个单元格
                        dr[j] = GetCellValue(cell); //获取单元格的值
                                                    //全为空则不取
                        if (dr[j].ToString() != "")
                        {
                            result = true;
                        }
                    }
                    if (result == true)
                    {
                        dt.Rows.Add(dr); //把每行追加到DataTable
                    }
                }
            }

        }
        return dt;
    }

    //对单元格进行判断取值
    private static string GetCellValue(ICell cell)
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
    /// <summary>
    /// 写Excel文件
    /// </summary>
    /// <param name="filePath">传入文件路径</param>
    /// <param name="exceltable">传入数据表</param>
    /// <returns name="int">返回0，1判断成功与否</returns>
    public static int OutportExcel(string filePath, DataTable exceltable)
    {
        return 0;
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
