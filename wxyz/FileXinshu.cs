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
using uvwxyz;

namespace uvwxyz
{
    class FunctionXinshu
    {
        string mode;
        string file1;
        string file2;
        string game;
        string date;
        string ExportName;
        // CostConf config;
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
            CostConf costExcelConf = new CostConf
            {
                filePath = this.file1,
                channel = "新数",
                date = this.date,
                game = this.game,
                exportName = ExportName,
                sheetName = "Woeksheet",
                sheetIndex = 0,
                startRow = 0,
                startColumn = 0,
                campaignColumnName = "推广计划名称",
                dateColumnName = "报表日期",
                costColumnName = "转化成本（元）"
            };

            if (this.mode == "花费" & this.file1 == string.Empty)
            {
                ResultMessage.times += 1;
                ResultMessage.text = "先选择一个文件" + new String('!', ResultMessage.times);
            }
            if (this.mode == "花费" & this.file1 != string.Empty)
            {
                ExcelFile file = new ExcelFile(costExcelConf);
                ResultMessage = file.ResultMessage;
            }
            if (this.mode == "参数" & this.file1 == string.Empty & this.file2 == string.Empty)
            {
                ResultMessage.text = "没用。";
            }
            if (this.mode == "参数" & (this.file1 == string.Empty | this.file2 == string.Empty))
            {
                ResultMessage.text = "没用。";
            }
            if (this.mode == "参数" & this.file1 != string.Empty & this.file2 != string.Empty)
            {
                // ResultMessage = VlookUp();
                ResultMessage.text = "没用。";
            }
        }
    }
}

