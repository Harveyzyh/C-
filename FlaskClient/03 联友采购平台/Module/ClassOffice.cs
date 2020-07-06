using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ
{

    public class Excel
    {
        private Excel_Base newExcel = null;

        #region 基类
        public class Excel_Base
        {
            private string _filePath = null;//路径
            private string _fileName = null;//文件名
            private int _sheetIndex = 0;
            private bool _isTitleRow = false;//是否有首行
            private bool _isWrite = false;//是否为写入模式
            private DataTable _titleDt = null;//表头内容
            private DataTable _titleFormat = null;//表头格式
            private DataTable _cellDt = null;//数据内容
            private string _status = null;//打开返回状态

            public string filePath { get { return _filePath; } set { _filePath = value; } }
            public string fileName { get { return _fileName; } set { _fileName = value; } }
            public int sheetIndex { get { return _sheetIndex; } set { if (value >= 0) _sheetIndex = value; } }
            public bool isTitleRow { get { return _isTitleRow; } set { _isTitleRow = value; } }
            public bool isWrite { get { return _isWrite; } set { _isWrite = value; } }
            public DataTable titleDt { get { return _titleDt; } set { _titleDt = value; } }
            public DataTable titleFormat { get { return _titleFormat; } set { _titleFormat = value; } }
            public DataTable cellDt { get { return _cellDt; } set { _cellDt = value; } }
            public string status { get { return _status; } set { _status = value; } }
        }
        #endregion

        #region Excel操作判断及分类处理
        public void ExcelOpt(object obj)
        {
            string filePath = null;
            string fileName = null;

            newExcel = (Excel_Base)obj;
            if (newExcel.isWrite)
            {
                if (newExcel.fileName == null)
                {
                    if (SaveFile(out filePath, out fileName))
                    {
                        newExcel.filePath = filePath;
                        newExcel.fileName = fileName;
                        if (newExcel.cellDt == null)
                        {
                            MessageBox.Show("保存的Excel内容为空！", "错误");
                        }
                        else
                        {
                            OutportExcel();
                        }
                    }
                }
            }
            else
            {
                if (OpenFile(out filePath, out fileName))
                {
                    newExcel.filePath = filePath;
                    newExcel.fileName = fileName;

                    ImportExcel();
                }
            }
        }
        #endregion

        #region 读Excel
        private void ImportExcel()
        {
            string fileName = newExcel.fileName;
            string filePath = newExcel.filePath;
            string fullPath = null;
            int sheetIndex = newExcel.sheetIndex;
            DataTable cellDt = new DataTable();
            ICell cell = null;
            int rowIndex = 0;

            fullPath = Path.Combine(filePath, fileName);

            try
            {
                FileStream fsRead = File.OpenRead(fullPath);
                IWorkbook wk = null;
                //获取后缀名
                string extension = fullPath.Substring(fullPath.LastIndexOf(".")).ToString().ToLower();
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
                        if (newExcel.isTitleRow)
                        {
                            cell = headrow.GetCell(i);
                            cellDt.Columns.Add(GetCellValue(cell).Replace("\n", ""));
                            rowIndex = 1;
                        }
                        else
                        {
                            cellDt.Columns.Add(new DataColumn("Col" + (i + 1).ToString(), typeof(string)));
                            //cellDt.Columns.Add("Col" + (i + 1).ToString());
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
                newExcel.status = "Yes";
                newExcel.cellDt = cellDt;
            }
            catch (IOException)
            {
                newExcel.status = "Error";
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
            string fileName = newExcel.fileName;
            string filePath = newExcel.filePath;
            int sheetIndex = newExcel.sheetIndex;
            DataTable cellDt = newExcel.cellDt;
            DataTable formatDt = newExcel.titleFormat;

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
            if (cellDt != null)
            {
                for (int i = 0; i < cellDt.Rows.Count; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < cellDt.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(cellDt.Rows[i][j].ToString());
                    }
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

        #region fileDialog
        public bool OpenFile(out string filePath, out string fileName )
        {
            filePath = null;
            fileName = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Excel文件|*.xlsx;*.xls";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = Path.GetDirectoryName(openFileDialog.FileName);
                fileName = Path.GetFileName(openFileDialog.FileName);
                return true;
            }
            else
            {
                return false;
            }
        } 

        public bool SaveFile(out string filePath, out string fileName)
        {
            filePath = null;
            fileName = null;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "Excel 2007|*.xlsx";
            //saveFileDialog.FileName = "生产排程导出_" + DateTime.Now.ToString("yyyy-MM-dd");
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = Path.GetDirectoryName(saveFileDialog.FileName);
                fileName = Path.GetFileName(saveFileDialog.FileName);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

}
