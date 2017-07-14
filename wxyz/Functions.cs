using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Data;
using System.Windows.Documents;

namespace uvwxyz
{
    public class Functions
    {
        private string mode;
        private string date;
        private string channel;
        private string game;
        private string file1;
        private string file2;
        private Message ResultMessage;

        public static string[] GetFileInfo(string path)
        {
            string[] FileInfo = new string[3];
            FileInfo file = new FileInfo(path);
            FileInfo[0] = "  " + Path.GetFileName(path);
            FileInfo[1] = "  " +  file.CreationTime.ToString();
            FileInfo[2] = "  " + GetFileSize(file);
            return FileInfo;
        }

        public static string GetFileSize(FileInfo file)
        {
            double len = file.Length;
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            string filesize = String.Format("{0:0.##} {1}", len, sizes[order]);
            return filesize;
        }

        public Message  ButtonFunction(string mode, string date, string channel, string game, string file1, string file2)
        {
            this.mode = mode;
            this.date = date;
            this.channel = channel;
            this.game = game;
            this.file1 = file1;
            this.file2 = file2;
            ResultMessage = new Message { code = 0, text = "^o^", times = 0 };
            string ExportName = GetFileName(Environment.CurrentDirectory, this.channel + "-" + this.game + "-" + this.mode + "-" + this.date.Replace("/", ""), ".csv");
            List<string> headerCost = new List<string>() { "平台", "游戏", "广告名", "时间", "计费方式", "消耗" };
            List<string> headerSubs = new List<string>() { "平台", "游戏", "广告名", "时间", "计费方式", "消耗" };
            //file1 渠道，file2 游族
            if (this.channel == "360")
            {
                if (this.mode == "花费" & this.file1 == string.Empty)
                {
                    ResultMessage.times += 1;
                    ResultMessage.text = "先选择一个文件" + new String('!', ResultMessage.times);
                }

                if (this.mode == "花费" & this.file1 != string.Empty)
                {
                    List<MutilCost> costlist = ReadCost360(this.file1);
                    
                    if(costlist.Count != 0)
                    {
                        foreach (MutilCost record in costlist)
                        {
                            record.platform = "国内页游";
                            record.game = this.game;
                            record.campaign = "360DSP-" + record.campaign;
                            record.date = this.date;
                            record.type = "点击";
                            record.cost = Math.Round(record.cost / 1.42, 2);
                        }
                        costlist.RemoveAt(costlist.Count - 1);
                        costlist = costlist.Where(p => p.cost != 0).ToList();
                        using (var csv = new CsvWriter(new StreamWriter(ExportName, false, Encoding.GetEncoding("GB2312"))))
                        {
                            foreach (var i in headerCost)
                            {
                                csv.WriteField(i);
                            }
                            //UTF8 with bom
                            csv.NextRecord();
                            foreach (var i in costlist)
                            {
                                csv.WriteRecord(i);
                            }
                            // csv.WriteRecords(costlist);
                        }
                        this.ResultMessage.code = 1;
                        this.ResultMessage.text = ExportName + " done.";
                    }
                    else
                    {
                        if (ResultMessage.text == "^o^")
                        {
                            ResultMessage.text = "空文件。";
                        }
                        return ResultMessage;
                    }
                }
                if (mode == "拼表")
                {
                    if (this.file1 == string.Empty & this.file2 == string.Empty)
                    {
                        ResultMessage.times += 1;
                        ResultMessage.text = "先选择一个文件" + new String('!', ResultMessage.times);
                    }
                    else if(this.file1 == string.Empty | this.file2 == string.Empty)
                    {
                        ResultMessage.times += 1;
                        ResultMessage.text = "再选择一个文件" + new String('!', ResultMessage.times);
                    }
                    else
                    {
                        List<SourceID360> sourceid360 = ReadSourceID360(this.file1);
                        List<SubsYouzu> subsyouzu = ReadSubsYouzu(this.file2);
                        if(sourceid360.Count != 0 & subsyouzu.Count != 0)
                        {
                            List<SubsYouzu> newSubsList = Combine360Subs(sourceid360, subsyouzu);

                            using (var csv = new CsvWriter(new StreamWriter(ExportName, false, UTF8Encoding.UTF8)))
                            {
                                //UTF8 with bom 
                                csv.WriteRecords(newSubsList);
                            }
                            this.ResultMessage.code = 1;
                            this.ResultMessage.text = ExportName + " done.";
                        }
                        else
                        {
                            return ResultMessage;
                        }
                    }
                }
            }

            if (channel == "舜飞")
            {
                if (this.mode == "花费" & this.file1 == string.Empty)
                {
                    ResultMessage.times += 1;
                    ResultMessage.text = "先选择一个文件" + new String('!', ResultMessage.times);
                }

                if (this.mode == "花费" & this.file1 != string.Empty)
                {
                    List<MutilCost> costlist = ReadCostSF(this.file1);
                    if(costlist.Count != 0)
                    {
                        costlist.RemoveAt(0);
                        List<MutilCost> newList = (from r in costlist
                                                   where r.cost != 0
                                                   select r).ToList();

                        foreach (MutilCost record in newList)
                        {
                            record.platform = "国内页游";
                            record.game = this.game;
                            record.campaign = "舜飞DSP-" + record.campaign;
                            record.date = this.date;
                            record.type = "点击";
                            record.cost = Math.Round(record.cost, 2);
                        }
                        costlist = costlist.Where(p => p.cost != 0).ToList();
                    
                        using (var csv = new CsvWriter(new StreamWriter(ExportName, false, Encoding.GetEncoding("GB2312"))))
                        {
                            CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                            configuration.Encoding = Encoding.BigEndianUnicode;
                            foreach (var i in headerCost)
                            {
                                csv.WriteField(i);
                            }
                            //UTF8 with bom
                            csv.NextRecord();
                            foreach (var i in newList)
                            {
                                csv.WriteRecord(i);
                            }
                        }
                        this.ResultMessage.code = 1;
                        this.ResultMessage.text = ExportName + " done.";
                    }
                    else
                    {
                        if(ResultMessage.text == "^o^")
                        {
                            ResultMessage.text = "空文件。";
                        }
                        return ResultMessage;
                    }             
                }
                if (mode == "拼表")
                {
                    if (this.file1 == string.Empty & this.file2 == string.Empty)
                    {
                        ResultMessage.times += 1;
                        ResultMessage.text = "先选择一个文件" + new String('!', ResultMessage.times);
                    }
                    else if (this.file1 == string.Empty | this.file2 == string.Empty)
                    {
                        ResultMessage.times += 1;
                        ResultMessage.text = "再选择一个文件" + new String('!', ResultMessage.times);
                    }
                    else
                    {
                        List<SourceIDSF> SourceIDSFList = ReadSourceIDSF(this.file1);
                        List<SubsYouzu> SubsYouzuList = ReadSubsYouzu(this.file2);

                        if(SourceIDSFList.Count != 0 & SubsYouzuList.Count != 0)
                        {
                            List<SubsYouzu> newSubsList = CombineSFSubs(SourceIDSFList, SubsYouzuList);

                            using (var csv = new CsvWriter(new StreamWriter(ExportName, false, Encoding.GetEncoding("GB2312"))))
                            {
                                csv.WriteRecords(newSubsList);
                            }
                            this.ResultMessage.code = 1;
                            this.ResultMessage.text = ExportName + " done.";
                        }
                        else
                        {
                            return ResultMessage;
                        }
                    }
                }
            }
            if (channel == "新数")
            {
                if (this.mode == "花费" & this.file1 == string.Empty)
                {
                    ResultMessage.times += 1;
                    ResultMessage.text = "先选择一个文件" + new String('!', ResultMessage.times);
                }

                if (this.mode == "花费" & this.file1 != string.Empty)
                {
                    ResultMessage = XSMultiCost(this.file1);
                }
            }
            return ResultMessage;
        }

        public Message XSMultiCost(string ExportName )
        {
            List<MutilCost2> CostList = new List<MutilCost2>();
            Excel XsCostFile = new Excel(this.file1);
            DataTable sheet = XsCostFile.Data;


            if(sheet.Rows.Count == 0)
            {
                ResultMessage.text = "空文件。";
                return ResultMessage;
            }
            else
            {
                List<string> headertest = new List<string> { "推广计划名称", "默认营销点", "预算", "展现次数", "点击数", "点击率", "平均点击价格（元）", "千次展现价格（元）", "消耗（元）", "转化量", "转化成本（元）", "报表日期" };
                List<string> headerread = new List<string>();
                for (int i = 1; i <= sheet.Columns.Count; i++)
                {
                    headerread.Add(sheet.Columns[i].ColumnName);
                }
                if (headerread != headertest)
                {
                    ResultMessage.text = "文件格式错误";
                    return ResultMessage;
                }
            }

            //DataRow[] dr = sheet.Select("推广计划名称  like"'+this.game+ '"% and  消耗（元）!= '0'");
            MutilCost2 record = new MutilCost2();
            for (int i = 1; i <= sheet.Rows.Count; i++)
            {
                DataRow row = sheet.Rows[i];
                if (row[1].ToString().StartsWith(this.game) & row[9].ToString() != "0")
                {
                    record.platform = "国内页游";
                    record.game = this.game;
                    record.campaign = "新数DSP-" + (row[1].ToString().Split(')'))[0];
                    record.date = this.date;
                    record.type = "点击";
                    record.cost = row[9].ToString();
                    CostList.Add(record);
                }
            }

            using (var csv = new CsvWriter(new StreamWriter(ExportName, false, Encoding.GetEncoding("GB2312"))))
            {
                List<string> headerCost = new List<string>() { "平台", "游戏", "广告名", "时间", "计费方式", "消耗" };
                CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                foreach (var i in headerCost)
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
            this.ResultMessage.text = ExportName + " done.";
            return ResultMessage;
    }


        public List<SubsYouzu> ReadSubsYouzu(string file)
        {
            TextReader reader = new StreamReader(@file, Encoding.UTF8);
            List<SubsYouzu> SubsRecordList = new List<SubsYouzu>();
            try
            {
                using (CsvReader csv = new CsvReader(reader))
                {
                    CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                    configuration.Encoding = Encoding.UTF8;
                    configuration.HasHeaderRecord = true;
                    csv.Configuration.SkipEmptyRecords = true;
                    csv.Configuration.RegisterClassMap<SubsYouzuMap>();
                    SubsRecordList = csv.GetRecords<SubsYouzu>().ToList();
                }
            }
            catch
            {
                this.ResultMessage.code = -1;
                this.ResultMessage.text = "文件格式错误。";
            }
            return SubsRecordList;
        }

        public List<SourceID360> ReadSourceID360(string file)
        {
            TextReader reader = new StreamReader(@file, Encoding.UTF8);
            List<SourceID360> SourceIdList = new List<SourceID360>();
            try {
                using (CsvReader csv = new CsvReader(reader))
                {
                    CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                    configuration.Encoding = Encoding.UTF8;
                    configuration.HasHeaderRecord = true;
                    csv.Configuration.SkipEmptyRecords = true;
                    csv.Configuration.RegisterClassMap<SourceID360Map>();
                    SourceIdList = csv.GetRecords<SourceID360>().ToList();
                }
            }
            catch
            {
                this.ResultMessage.code = -1;
                this.ResultMessage.text = "文件格式错误。";
            }
            return SourceIdList;
        }

        public List<MutilCost> ReadCost360(string file)
        {
            List<MutilCost> MutiCostList = new List<MutilCost>();
            try
            {     
                TextReader reader = new StreamReader(@file, Encoding.BigEndianUnicode); //编码格式
                using (CsvReader csv = new CsvReader(reader))
                {
                    CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                    configuration.Encoding = Encoding.UTF8;
                    csv.Configuration.Delimiter = "	"; // 空格分隔
                    configuration.HasHeaderRecord = true;
                    csv.Configuration.SkipEmptyRecords = true;
                    csv.Configuration.RegisterClassMap<Cost360Map>();
                    MutiCostList = csv.GetRecords<MutilCost>().ToList();
                }
            }
            catch
            {
                this.ResultMessage.code = -1;
                this.ResultMessage.text = "文件格式错误。";
            }

            return MutiCostList;
        }

        public static List<SubsYouzu> Combine360Subs(List<SourceID360> SourceIdList, List<SubsYouzu> SubsYouzuList)
        {
            SourceIdList = SourceIdList.OrderBy(x => x.sourceid).Distinct().ToList();
            SubsYouzuList = SubsYouzuList.OrderBy(x => x.sub3).Distinct().ToList(); //sourceid 在sub3

            List<SubsYouzu> newSubsYouzu = new List<SubsYouzu>();

            var queryNotExist = SourceIdList.Where(p => !SubsYouzuList.Select(g => g.sub3).Contains(p.sourceid)).ToList();
            // var query=lista.Where(p=>!listb.Any(g=>p.id==g.id && p.no==g.no))

            List<SubsYouzu> extendList = ExtendSourceID360ToSubsYouzu(queryNotExist);


            var queryExist = from sub in SubsYouzuList
                             join id in SourceIdList on sub.sub3 equals id.sourceid into os
                             from o in os
                             select new { sub, o };

            foreach (var item in queryExist)
            {
                item.sub.sub3 = item.o.adposition;
                newSubsYouzu.Add(item.sub);
            }

            newSubsYouzu.AddRange(extendList);

            newSubsYouzu = SubsTotal360Subs(newSubsYouzu);

            return newSubsYouzu;

        }

        public static List<SubsYouzu> CombineSFSubs(List<SourceIDSF> SourceIDSFList, List<SubsYouzu> SubsYouzuList)
        {
            SourceIDSFList = SourceIDSFList.OrderBy(x => x.sourceid).Distinct().ToList();
            SubsYouzuList = SubsYouzuList.OrderBy(x => x.sub3).Distinct().ToList(); //sourceid 在sub3

            List<SubsYouzu> newSubsYouzu = new List<SubsYouzu>();

            var queryNotExist = SourceIDSFList.Where(p => !SubsYouzuList.Select(g => g.sub3).Contains(p.sourceid)).ToList();
            // var query=lista.Where(p=>!listb.Any(g=>p.id==g.id && p.no==g.no))

            List<SubsYouzu> extendList = ExtendSourceIDSFToSubsYouzu(queryNotExist);


            var queryExist = from recordyz in SubsYouzuList
                             join record360 in SourceIDSFList on recordyz.sub3 equals record360.sourceid into os
                             from o in os
                             select new { recordyz, o };

            foreach (var item in queryExist)
            {
                item.recordyz.sub3 = item.o.sourceid;
                newSubsYouzu.Add(item.recordyz);
            }

            newSubsYouzu.AddRange(extendList);

            newSubsYouzu = SubsTotalSFSubs(newSubsYouzu);

            return newSubsYouzu;
        }

        public static string GetFileName(string path, string name, string extension)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            int number = directory.EnumerateFiles().Where(f => f.Name.Contains(name) && f.Extension == extension).Count();
            if(number == 0)
            {
                return name + extension;
            }
            else
            {
                return name + "["+ number.ToString() + "]"+ extension;
            }

        }

        public static List<SubsYouzu> ExtendSourceID360ToSubsYouzu(List<SourceID360> RecordsList)
        {
            List<SubsYouzu> extendList = new List<SubsYouzu>();

            foreach (var item in RecordsList)
            {
                SubsYouzu sub = new SubsYouzu();
                sub.cost = item.cost;
                sub.sub3 = item.adposition;
                extendList.Add(sub);
            }
            return extendList;
        }

        public static List<SubsYouzu> ExtendSourceIDSFToSubsYouzu(List<SourceIDSF> RecordsList)
        {
            List<SubsYouzu> extendList = new List<SubsYouzu>();

            foreach (var item in RecordsList)
            {
                SubsYouzu sub = new SubsYouzu();
                sub.cost = item.cost;
                sub.sub3 = item.sourcename;
                sub.sub1 = item.channel;
                extendList.Add(sub);
            }
            return extendList;
        }


        public static List<SubsYouzu> SubsTotal360Subs(List<SubsYouzu> list)
        {
            var newList = (from a in list
                           group a by new { a.sub3 } into b
                           select new SubsYouzu
                           {
                               sub3 = b.Key.sub3,
                               cost = b.Sum(c => c.cost),
                               click1 = b.Sum(c => c.click1),
                               click2 = b.Sum(c => c.click2),
                               registernum = b.Sum(c=>c.registeripnum),
                               registeripnum = b.Sum(c => c.registeripnum),
                               backnum = b.Sum(c => c.backnum),
                               activatenum = b.Sum(c => c.activatenum),
                               validnum = b.Sum(c => c.validnum),
                               remain = b.Sum(c => c.remain),
                               remain7num = b.Sum(c => c.remain7num),
                               newpaidusernum = b.Sum(c => c.newpaidusernum),
                               newpaidcashnum = b.Sum(c => c.newpaidcashnum),
                               allpaidusernum = b.Sum(c => c.allpaidusernum),
                               allpaidcashnum = b.Sum(c => c.allpaidcashnum),

                           }).OrderBy(t => t.cost).ThenBy(t => t.sub3).ToList();

            return newList;
            
        }

        public static List<SubsYouzu> SubsTotalSFSubs(List<SubsYouzu> list)
        {
            var newList = (from a in list
                           group a by new { a.sub1, a.sub3 } into b
                           select new SubsYouzu
                           {
                               sub1 = b.Key.sub1,
                               sub3 = b.Key.sub3,
                               cost = b.Sum(c => c.cost),
                               click1 = b.Sum(c => c.click1),
                               click2 = b.Sum(c => c.click2),
                               registernum = b.Sum(c => c.registeripnum),
                               registeripnum = b.Sum(c => c.registeripnum),
                               backnum = b.Sum(c => c.backnum),
                               activatenum = b.Sum(c => c.activatenum),
                               validnum = b.Sum(c => c.validnum),
                               remain = b.Sum(c => c.remain),
                               remain7num = b.Sum(c => c.remain7num),
                               newpaidusernum = b.Sum(c => c.newpaidusernum),
                               newpaidcashnum = b.Sum(c => c.newpaidcashnum),
                               allpaidusernum = b.Sum(c => c.allpaidusernum),
                               allpaidcashnum = b.Sum(c => c.allpaidcashnum),

                           }).OrderBy(t => t.cost).ThenBy(t => t.sub3).ToList();

            return newList;

        }


        public List<MutilCost> ReadCostSF(string file)
        {
            List<MutilCost> MutiCostList = new List<MutilCost>();
            try
            {
                TextReader reader = new StreamReader(@file, Encoding.UTF8); //编码格式
                using (CsvReader csv = new CsvReader(reader))
                {
                    CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                    configuration.Encoding = Encoding.UTF8;
                    configuration.HasHeaderRecord = true;
                    csv.Configuration.SkipEmptyRecords = true;
                    csv.Configuration.RegisterClassMap<CostSFMap>();
                    MutiCostList = csv.GetRecords<MutilCost>().ToList();                
                }
            }
            catch
            {
                this.ResultMessage.code = -1;
                this.ResultMessage.text = "文件格式错误。";
            }
            return MutiCostList;
        }

        public List<SourceIDSF> ReadSourceIDSF(string file)
        {
            List<SourceIDSF> SourceIDSFList = new List<SourceIDSF>();
            try
            {
                TextReader reader = new StreamReader(@file, Encoding.UTF8); //编码格式
                using (CsvReader csv = new CsvReader(reader))
                {
                    CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                    configuration.Encoding = Encoding.UTF8;
                    configuration.HasHeaderRecord = true;
                    csv.Configuration.SkipEmptyRecords = true;
                    csv.Configuration.RegisterClassMap<SourceIDSFMap>();
                    SourceIDSFList = csv.GetRecords<SourceIDSF>().ToList();
                }
            }
            catch
            {
                this.ResultMessage.code = -1;
                this.ResultMessage.text = "文件格式错误。";
            }
            return SourceIDSFList;
        }
    }




}
