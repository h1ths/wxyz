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
    public class ExcelFile
    {
        private CostConf config;
        public Message ResultMessage;

        public ExcelFile(CostConf conf)
        {
            this.config = conf;
            this.ResultMessage = new Message
            {
                code = 0,text="^o^"
            };
            DataTable sheet = ReadFromExcel(this.config);
            if(sheet != null)
            {
                ResultMessage = MultiCost(sheet);
            }            
        }

        public DataTable ReadFromExcel(CostConf config)
        {
            IWorkbook wk = null;
            ISheet sheet = null;
            DataTable table = new DataTable();
            string extension = Path.GetExtension(config.filePath);
            try
            {
                using (FileStream fs = File.Open(config.filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.WriteLine(fs.Length);
                    wk = WorkbookFactory.Create(fs);
                    fs.Close();
                }
                //读取当前表数据 
                sheet = wk.GetSheet(config.sheetName);
                if (sheet == null)
                {
                    this.ResultMessage.code = -1;
                    this.ResultMessage.text = "文件格式错误。";
                    return null;
                }
                IRow headerRow = sheet.GetRow(config.startRow);

                for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
                {
                    string ColumnName = headerRow.GetCell(i).StringCellValue;
                    DataColumn column = new DataColumn(ColumnName);
                    if (ColumnName == config.costColumnName)
                    {
                        //修改列类型
                        column.DataType = typeof(double);
                    }
                    table.Columns.Add(column);
                }

                // int offset = 0;
                for (int i = config.startRow+1; i <= sheet.LastRowNum; i++)
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
            catch
            {
                this.ResultMessage.code = -1;
                this.ResultMessage.text = "文件格式错误。";
                return null;
            }
            return table;
        }

        public Message MultiCost(DataTable sheet)
        {
            List<string> costFileHeader = new List<string>() { "平台", "游戏", "广告名", "时间", "计费方式", "消耗" };
            List<string> checkHeader = new List<string>();
            if (config.channel == "新数")
            {
                checkHeader = new List<string> { "推广计划名称", "默认营销点", "预算", "展现次数", "点击数", "点击率", "平均点击价格（元）", "千次展现价格（元）", "消耗（元）", "转化量", "转化成本（元）", "报表日期" };
            }
            else
            {
                checkHeader = new List<string> { "日期", "推广计划", "总花费", "展现数", "点击数", "平均点击率", "平均点击价格", "注册数", "CPA", "注册转化率" };
            }
            
            List<MultiCost> CostList = new List<MultiCost>();
            if (sheet == null)
            {
                ResultMessage.text = "文件格式错误。";
                ResultMessage.code = -1;
                return ResultMessage;
            }

            if (sheet.Rows.Count == 0)
            {
                ResultMessage.text = "空文件。";
                return ResultMessage;
            }
            else
            {
                
                List<string> headerread = new List<string>();
                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    headerread.Add(sheet.Columns[i].ColumnName);
                }
                if (!(checkHeader.All(headerread.Contains) && checkHeader.Count == headerread.Count))
                {
                    ResultMessage.text = "文件格式错误";
                    return ResultMessage;
                }
            }

            for (int i = 0; i < sheet.Rows.Count; i++)
            {
                MultiCost record = new MultiCost();
                DataRow row = sheet.Rows[i];
                if (row[config.startColumn].ToString().StartsWith(config.game) & (dynamic)row[config.costColumnName] != 0)
                {
                    record.platform = "国内页游";
                    record.game = config.game;
                    string campaignname = (dynamic)row[config.campaignColumnName].ToString();
                    campaignname = campaignname.Replace("(", "（").Replace(")", "）");
                    if ((dynamic)row[config.campaignColumnName].ToString().IndexOf("）") != -1)
                    {
                        campaignname = ((dynamic)row[config.campaignColumnName].ToString()).Split('）')[0] + "）";
                    }
                    record.campaign = config.channel + "-" + campaignname;
                    record.date = (dynamic)row[config.date];
                    record.type = "注册";
                    record.cost = (dynamic)row[config.costColumnName];
                    CostList.Add(record);
                }
            }
            CostList = SubsTotalMultiCost(CostList);
            CostList = CostList.Where(p => p.cost != 0).ToList();
            using (var csv = new CsvWriter(new StreamWriter(config.exportName, false, Encoding.GetEncoding("GB2312"))))
            {
                CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                foreach (var i in costFileHeader)
                {
                    csv.WriteField(i);
                }
                csv.NextRecord();
                foreach (var i in CostList)
                {
                    csv.WriteRecord(i);
                }
            }
            this.ResultMessage.code = 1;
            this.ResultMessage.text = config.exportName + " done.";
            return ResultMessage;
        }

        public List<MultiCost> SubsTotalMultiCost(List<MultiCost> list)
        {
            var newList = (from a in list
                           group a by new { a.campaign, a.date } into b
                           select new MultiCost
                           {
                               platform = b.First().platform,
                               game = b.First().game,
                               campaign = b.Key.campaign,
                               date = b.Key.date,
                               type = b.First().type,
                               cost = b.Sum(c => c.cost),
                           }).ToList();
            return newList;
        }
    }
}

