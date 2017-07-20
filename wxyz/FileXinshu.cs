using CsvHelper;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace uvwxyz
{
    class ExcelFile
    {
        string ExcelFilePath;
        public DataTable Data;

        public ExcelFile(string ExcelFile)
        {
            this.ExcelFilePath = ExcelFile;
            this.Data = ReadFromExcel(this.ExcelFilePath);
        }


        public static DataTable ReadFromExcel(string filePath)
        {
            IWorkbook wk = null;
            DataTable table = new DataTable();
            string extension = Path.GetExtension(filePath);
            Console.WriteLine(extension);
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                StreamReader sr = new StreamReader(fs);
                if (extension.Equals(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    wk = new XSSFWorkbook(fs);
                }
                fs.Close();
                //读取当前表数据
                ISheet sheet = wk.GetSheetAt(0);
                IRow headerRow = sheet.GetRow(0);  //读取当前行数据
                                                   //LastRowNum 是当前表的总行数-1（注意）

                for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
                {
                    string ColumnName = headerRow.GetCell(i).StringCellValue;
                    DataColumn column = new DataColumn(ColumnName);
                    if (ColumnName == "消耗（元）")
                    {
                        //修改列类型
                        column.DataType = typeof(double);
                    }
                    table.Columns.Add(column);
                }

                // int offset = 0;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();

                    for (int j = row.FirstCellNum; j < headerRow.LastCellNum; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            dataRow[j] = row.GetCell(j).ToString();
                        }

                    }
                    table.Rows.Add(dataRow);
                }
            }
            catch (Exception e)
            {
                //只在Debug模式下才输出
                Console.WriteLine(e.Message);
            }
            return table;
        }
    }

    class FunctionXinshu
    {
        string mode;
        string file1;
        string file2;
        string game;
        string date;
        string ExportName;
        public Message ResultMessage;
        
        public FunctionXinshu(string mode, string file1, string file2, string game, string date, string ExportName, Message ResultMessage)
        {
            this.mode = mode;
            this.file1 = file1;
            this.file2 = file2;
            this.game = game;
            this.date = date;
            this.ExportName = ExportName;
            this.ResultMessage = ResultMessage;

            if (this.mode == "花费" & this.file1 == string.Empty)
            {
                ResultMessage.times += 1;
                ResultMessage.text = "先选择一个文件" + new String('!', ResultMessage.times);

            }

            if (this.mode == "花费" & this.file1 != string.Empty)
            {
                ResultMessage = MultiCost();
            }
            
        }

        public Message MultiCost()
        {
            List<MultiCost> CostList = new List<MultiCost>();
            DataTable sheet = null;
            ExcelFile s = new ExcelFile(this.file1);
            sheet = s.Data;

            if (sheet.Rows.Count == 0)
            {
                ResultMessage.text = "空文件。";
                return ResultMessage;
            }
            else
            {
                List<string> headertest = new List<string> { "推广计划名称", "默认营销点", "预算", "展现次数", "点击数", "点击率", "平均点击价格（元）", "千次展现价格（元）", "消耗（元）", "转化量", "转化成本（元）", "报表日期" };
                List<string> headerread = new List<string>();
                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    headerread.Add(sheet.Columns[i].ColumnName);
                }
                if (!(headertest.All(headerread.Contains) && headertest.Count == headerread.Count))
                {
                    ResultMessage.text = "文件格式错误";
                    return ResultMessage;
                }
            }

            for (int i = 0; i < sheet.Rows.Count; i++)
            {
                MultiCost record = new MultiCost();
                DataRow row = sheet.Rows[i];
                if (row[0].ToString().StartsWith(this.game) & (dynamic)row[8] != 0)
                {
                    record.platform = "国内页游";
                    record.game = this.game;
                    string campaignname = (dynamic)row[0].ToString();
                    campaignname = campaignname.Replace("(", "（").Replace(")", "）");
                    if((dynamic)row[0].ToString().IndexOf("）") != -1)
                    {
                        campaignname = ((dynamic)row[0].ToString()).Split('）')[0] + "）";
                    }  
                    record.campaign = "新数DSP-" + campaignname;
                    record.date = this.date;
                    record.type = "点击";
                    record.cost = (dynamic)row[8];
                    CostList.Add(record);
                }
            }

            List<MultiCost> newList = SubsTotalMultiCost(CostList);

            using (var csv = new CsvWriter(new StreamWriter(ExportName, false, Encoding.GetEncoding("GB2312"))))
            {
                List<string> headerCost = new List<string>() { "平台", "游戏", "广告名", "时间", "计费方式", "消耗" };
                CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                foreach (var i in headerCost)
                {
                    csv.WriteField(i);
                }
                csv.NextRecord();
                foreach (var i in newList)
                {
                    csv.WriteRecord(i);
                }
            }
            this.ResultMessage.code = 1;
            this.ResultMessage.text = ExportName + " done.";
            return ResultMessage;
        }

        public List<MultiCost> SubsTotalMultiCost(List<MultiCost> list)
        {
            var newList = (from a in list
                           group a by new { a.campaign } into b
                           select new MultiCost
                           {
                               platform = b.First().platform,
                               game = b.First().game,
                               campaign = b.First().campaign,
                               date = b.First().date,
                               type = b.First().type,
                               cost = b.Sum(c => c.cost),
                           }).ToList();
            return newList;
        }
    }
}

