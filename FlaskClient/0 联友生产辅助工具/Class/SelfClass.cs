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
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Web.Script.Serialization;
using System.Collections;

namespace HarveyZ
{
    public class IPInfo
    {
        public static string GetIpAddress()
        {
            string addr = "";
            string ip = "192."; //用于过滤ip
            int index = 0;
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址

            foreach(IPAddress localaddr in localhost.AddressList)
            {
                index = localaddr.ToString().IndexOf(ip);
                if (index >= 0)
                {
                    addr = localaddr.ToString();
                }
            }

            return addr;
        }
    }

    public class CEncrypt
    {
        public static string GetMd5Str(string str)     //取得字符串str被MD5加密算法加密后的字符串
        {
            string resultstr = "";
            MD5 myMD5 = MD5.Create();
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
                    //MessageBox.Show("执行失败(" + ex.Message + ")，请退出后重新进入！", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
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
        public Form Form_Init(Form sender)
        {
            Form frm = sender;
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.WindowState = FormWindowState.Maximized;
            return frm;
        }
    }


    #region WebPost
    public class WebClientEx : WebClient
    {
        private int timeout = 60000;
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            request.Timeout = timeout;
            return request;
        }
    }

    public class WebNet
    {
        public static Dictionary<string, string> WebPost(string url, Dictionary<string, string> dict, int timeout = 60000)
        {
            try
            {
                var client = new WebClientEx();
                client.Timeout = timeout;
                string json = Json.Dict2Json(dict);
                string response = client.UploadString(url, json);
                dict = Json.Json2Dict(response);
                return dict;
            }
            catch
            {
                return null;
            }
        }

        public static string WebPost(string url, string strin, int timeout = 60000)
        {
            try
            {
                var client = new WebClientEx();
                client.Timeout = timeout;
                string strout = client.UploadString(url, strin);
                return strout;
            }
            catch
            {
                return null;
            }
        }
    }
    #endregion


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
            private string status = null;//打开返回状态

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

            public string Status
            {
                get
                {
                    return status;
                }
                set
                {
                    status = value;
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


            try 
            {
                FileStream fsRead = File.OpenRead(path);
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
                newExcel.Status = "Yes";
                newExcel.CellDt = cellDt;
            }
            catch(Exception IOException)
            {
                newExcel.Status = "Error";
                MessageBox.Show("Excel文件：" + fileName + "已被打开，请先将该文件关闭再执行导入操作！", "错误", MessageBoxButtons.OK);
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


    public class Json
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

            string jsonStr = JsonConvert.SerializeObject(dict, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
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

        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable Json2DT2(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                if (current != "data")
                                    dataTable.Columns.Add(current, dictionary[current].GetType());
                                else
                                {
                                    ArrayList list = dictionary[current] as ArrayList;
                                    foreach (Dictionary<string, object> dic in list)
                                    {
                                        foreach (string key in dic.Keys)
                                        {
                                            dataTable.Columns.Add(key, dic[key].GetType());
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        //Rows
                        string root = "";
                        foreach (string current in dictionary.Keys)
                        {
                            if (current != "data")
                                root = current;
                            else
                            {
                                ArrayList list = dictionary[current] as ArrayList;
                                foreach (Dictionary<string, object> dic in list)
                                {
                                    DataRow dataRow = dataTable.NewRow();
                                    dataRow[root] = dictionary[root];
                                    foreach (string key in dic.Keys)
                                    {
                                        dataRow[key] = dic[key];
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }


        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable Json2DT(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result = null;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        //Rows
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            result = dataTable;
            return result;
        }
    }


    public class ZipUnZip
    {
        #region 压缩  

        /// <summary>   
        /// 递归压缩文件夹的内部方法   
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹路径</param>   
        /// <param name="zipStream">压缩输出流</param>   
        /// <param name="parentFolderName">此文件夹的上级文件夹</param>   
        /// <returns></returns>   
        private bool ZipDirectory(string folderToZip, ZipOutputStream zipStream, string parentFolderName)
        {
            bool result = true;
            string[] folders, files;
            ZipEntry ent = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();

            try
            {
                ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/"));
                zipStream.PutNextEntry(ent);
                zipStream.Flush();

                files = Directory.GetFiles(folderToZip);
                foreach (string file in files)
                {
                    fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file)));
                    ent.DateTime = DateTime.Now;
                    ent.Size = fs.Length;

                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    ent.Crc = crc.Value;
                    zipStream.PutNextEntry(ent);
                    zipStream.Write(buffer, 0, buffer.Length);
                }

            }
            catch
            {
                result = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (ent != null)
                {
                    ent = null;
                }
                GC.Collect();
                GC.Collect(1);
            }

            folders = Directory.GetDirectories(folderToZip);
            foreach (string folder in folders)
                if (!ZipDirectory(folder, zipStream, folderToZip))
                    return false;

            return result;
        }

        /// <summary>   
        /// 压缩文件夹    
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹路径</param>   
        /// <param name="zipedFile">压缩文件完整路径</param>   
        /// <param name="password">密码</param>   
        /// <returns>是否压缩成功</returns>   
        public bool ZipDirectory(string folderToZip, string zipedFile, string password)
        {
            bool result = false;
            if (!Directory.Exists(folderToZip))
                return result;

            ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile));
            zipStream.SetLevel(6);
            if (!string.IsNullOrEmpty(password)) zipStream.Password = password;

            result = ZipDirectory(folderToZip, zipStream, "");

            zipStream.Finish();
            zipStream.Close();

            return result;
        }

        /// <summary>   
        /// 压缩文件夹   
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹路径</param>   
        /// <param name="zipedFile">压缩文件完整路径</param>   
        /// <returns>是否压缩成功</returns>   
        public bool ZipDirectory(string folderToZip, string zipedFile)
        {
            bool result = ZipDirectory(folderToZip, zipedFile, null);
            return result;
        }

        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="fileToZip">要压缩的文件全名</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <param name="password">密码</param>   
        /// <returns>压缩结果</returns>   
        public bool ZipFile(string fileToZip, string zipedFile, string password)
        {
            bool result = true;
            ZipOutputStream zipStream = null;
            FileStream fs = null;
            ZipEntry ent = null;

            if (!File.Exists(fileToZip))
                return false;

            try
            {
                fs = File.OpenRead(fileToZip);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                fs = File.Create(zipedFile);
                zipStream = new ZipOutputStream(fs);
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                ent = new ZipEntry(Path.GetFileName(fileToZip));
                zipStream.PutNextEntry(ent);
                zipStream.SetLevel(6);

                zipStream.Write(buffer, 0, buffer.Length);

            }
            catch
            {
                result = false;
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (ent != null)
                {
                    ent = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            GC.Collect();
            GC.Collect(1);

            return result;
        }

        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="fileToZip">要压缩的文件全名</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <returns>压缩结果</returns>   
        public bool ZipFile(string fileToZip, string zipedFile)
        {
            bool result = ZipFile(fileToZip, zipedFile, null);
            return result;
        }

        /// <summary>   
        /// 压缩文件或文件夹   
        /// </summary>   
        /// <param name="fileToZip">要压缩的路径</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <param name="password">密码</param>   
        /// <returns>压缩结果</returns>   
        public bool Zip(string fileToZip, string zipedFile, string password)
        {
            bool result = false;
            if (Directory.Exists(fileToZip))
                result = ZipDirectory(fileToZip, zipedFile, password);
            else if (File.Exists(fileToZip))
                result = ZipFile(fileToZip, zipedFile, password);

            return result;
        }

        /// <summary>   
        /// 压缩文件或文件夹   
        /// </summary>   
        /// <param name="fileToZip">要压缩的路径</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <returns>压缩结果</returns>   
        public bool Zip(string fileToZip, string zipedFile)
        {
            bool result = Zip(fileToZip, zipedFile, null);
            return result;

        }

        #endregion

        #region 解压  

        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <param name="password">密码</param>   
        /// <returns>解压结果</returns>   
        public bool UnZip(string fileToUnZip, string zipedFolder, string password)
        {
            bool result = true;
            FileStream fs = null;
            ZipInputStream zipStream = null;
            ZipEntry ent = null;
            string fileName;

            if (!File.Exists(fileToUnZip))
                return false;

            if (!Directory.Exists(zipedFolder))
                Directory.CreateDirectory(zipedFolder);

            try
            {
                zipStream = new ZipInputStream(File.OpenRead(fileToUnZip));
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                while ((ent = zipStream.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(ent.Name))
                    {
                        fileName = Path.Combine(zipedFolder, ent.Name);
                        fileName = fileName.Replace('/', '\\');//change by Mr.HopeGi   

                        if (fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        fs = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[size];
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                                fs.Write(data, 0, data.Length);
                            else
                                break;
                        }
                    }
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (zipStream != null)
                {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                if (ent != null)
                {
                    ent = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            return result;
        }

        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <returns>解压结果</returns>   
        public bool UnZip(string fileToUnZip, string zipedFolder)
        {
            bool result = UnZip(fileToUnZip, zipedFolder, null);
            return result;
        }

        #endregion
    }

}
