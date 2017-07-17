using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace npoitest
{
    class Program
    {

        /*
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex, bool HaveHeader)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            workbook = new HSSFWorkbook(ExcelFileStream);
            InitializeWorkbook();
            HSSFSheet sheet = workbook.GetSheetAt(SheetIndex);

            DataTable table = new DataTable();

            HSSFRow headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                string ColumnName = (HaveHeader == true) ? headerRow.GetCell(i).StringCellValue : "f" + i.ToString();
                DataColumn column = new DataColumn(ColumnName);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;
            int RowStart = (HaveHeader == true) ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;
            for (int i = RowStart; i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                table.Rows.Add(dataRow);
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }
        */

        public static void ReadFromExcelFile(string filePath)
        {
            IWorkbook wk = null;
            string extension = System.IO.Path.GetExtension(filePath);
            Console.WriteLine(extension);
            try
            {
                // FileStream fs = File.OpenRead(filePath);
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if (extension.Equals(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    // wk = new HSSFWorkbook(fs);
                  wk = new HSSFWorkbook(new FileStream(filePath, FileMode.Open));

                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    wk = new XSSFWorkbook(fs);
                }
                fs.Close();
                //读取当前表数据
                ISheet sheet = wk.GetSheetAt(0);
                IRow row = sheet.GetRow(0);  //读取当前行数据
                                             //LastRowNum 是当前表的总行数-1（注意）
                // int offset = 0;
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);  //读取当前行数据
                    if (row != null)
                    {
                        //LastCellNum 是当前行的总列数
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            //读取该行的第j列数据
                            string value = row.GetCell(j).ToString();
                            Console.Write(value.ToString() + " ");
                        }
                        Console.WriteLine("\n");
                    }
                }
            }

            catch (Exception e)
            {
                //只在Debug模式下才输出
                Console.WriteLine(e.Message);
                
            }
        }

        static void Main(string[] args)
        {
            ReadFromExcelFile(@"上海游族3_推广计划 - 副本.xls");


            Console.ReadKey();
        }
    }
}
