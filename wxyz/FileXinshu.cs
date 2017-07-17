using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace uvwxyz
{
    class Excel
    {
        string ExcelFile;
        public DataTable Data;

        public Excel(string ExcelFile)
        {
            this.ExcelFile = ExcelFile;
            this.Data = ReadFromExcel(this.ExcelFile);
        }

        public DataTable ReadFromExcel(string ExcelFile)
        {
            DataTable sheetData = new DataTable();

            //Create COM Objects. Create a COM object for everything that is referenced
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(ExcelFile);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            if (colCount != 12 | rowCount < 2)       // empty sheet is not equal to null, but its dimension is.
            {
                return sheetData;
            }
            else
            {
                //add header
                for (int i = 1; i <= colCount; i++)
                {
                    object cell = xlWorksheet.Cells[i, 1].Value;
                    string columnName = cell != null ? cell.ToString() : string.Empty;
                    DataColumn column = new DataColumn();

                    column.DataType = Type.GetType("System.String");
               
                    sheetData.Columns.Add(column);
                }

                //read content
                try
                {
                    for (int i = 1; i <= rowCount; i++)
                    {

                        DataRow row = sheetData.NewRow();
                        for (int j = 1; j <= colCount; j++)
                        {
                            if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            {
                                row[j - 1] = xlRange.Cells[i, j].Value2.ToString();
                            }
                        }
                        sheetData.Rows.Add(row);
                    }
                }
                catch (Exception)
                {
                    ReleaseObject(xlApp, xlWorkbook, xlWorksheet, xlRange);
                    return sheetData;
                }
            }

            ReleaseObject(xlApp, xlWorkbook, xlWorksheet, xlRange);
            return sheetData;
        }

        public void ReleaseObject(Microsoft.Office.Interop.Excel.Application xlApp, Microsoft.Office.Interop.Excel.Workbook xlWorkbook, Microsoft.Office.Interop.Excel._Worksheet xlWorksheet, Microsoft.Office.Interop.Excel.Range xlRange)
        {
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
    }
}
