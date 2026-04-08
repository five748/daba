using System.IO;
using System.Data;
using OfficeOpenXml;
using System.Linq;
using ExcelDataReader;

public class ExcelTool
{
    public static void Read(string path, System.Action<string> callback)
    {
        FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        DataSet result = excelDataReader.AsDataSet();
        for (int k = 0; k < result.Tables.Count; k++)
        {
            var table = result.Tables[k];
            // 获取表格有多少列 
            int columns = table.Columns.Count;
            // 获取表格有多少行 
            int rows = table.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                if (string.IsNullOrEmpty(table.Rows[i][1].ToString()))
                {
                    break;
                }
                string str = "";
                for (int j = 0; j < columns; j++)
                {
                    str += table.Rows[i][j].ToString().Trim() + "|";
                }
                str = str.CutLast();
                callback(str);
            }
        }
    }
    public static void Write(string path, string name, System.Action<ExcelWorksheet> callback)
    {
        FileInfo newFile = new FileInfo(path);
        //通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[name];
            if (worksheet == null)
            {
                UnityEngine.Debug.LogError($"表 {name} 不存在, 新建表 {name}");
                worksheet = package.Workbook.Worksheets.Add(name);
            }
            callback(worksheet);
            package.Save();
        }

    }
    public static void mWrite(string path, string name, System.Action<ExcelWorksheet> callback)
    {
        FileInfo newFile = new FileInfo(path);
        //通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[name];

            if (worksheet == null)
            {
                UnityEngine.Debug.LogError($"表 {name} 不存在, 新建表 {name}");
                worksheet = package.Workbook.Worksheets.Add(name);
            }
            worksheet.Cells.Clear();
            callback(worksheet);
            package.Save();
        }
    }


}
