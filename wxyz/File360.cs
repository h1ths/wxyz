using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace uvwxyz
{
    public class SourceID360
    {
        public string sourceid { get; set; }
        public string campaign { get; set; }
        public string group { get; set; }
        public string adposition { get; set; }
        public double cost { get; set; }
    }

    public sealed class SourceID360Map : CsvClassMap<SourceID360>
    {
        public SourceID360Map()
        {
            Map(m => m.sourceid).Name("sourceId").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("sourceId")) ? string.Empty : Convert.ToString(row.GetField("sourceId")));
            Map(m => m.campaign).Name("推广计划").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("推广计划")) ? string.Empty : Convert.ToString(row.GetField("推广计划")));
            Map(m => m.group).Name("推广组").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("推广组")) ? string.Empty : Convert.ToString(row.GetField("推广组")));
            Map(m => m.adposition).Name("广告位").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("广告位")) ? string.Empty : Convert.ToString(row.GetField("广告位")));
            Map(m => m.cost).Name("扣费(元)").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("扣费(元)")) ? 0 : Convert.ToDouble(row.GetField("扣费(元)")));
        }
    }

    public sealed class Cost360Map : CsvClassMap<MultiCost>
    {
        public Cost360Map()
        {
            Map(m => m.date).Name("日期").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("日期")) ? string.Empty : Convert.ToString(row.GetField("日期")));
            Map(m => m.campaign).Name("推广计划").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("推广计划")) ? string.Empty : Convert.ToString(row.GetField("推广计划")));
            Map(m => m.cost).Name("花费").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("花费")) ? 0 : Convert.ToDouble(row.GetField("花费")));
        }
    }

    class Function360
    {
        string mode;
        string file1;
        string file2;
        string game;
        string date;
        string ExportName;
        public Message ResultMessage;

        public Function360(string mode, string file1, string file2, string game, string date, string ExportName, Message ResultMessage)
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
            if (this.mode == "参数" & this.file1 == string.Empty & this.file2 == string.Empty)
            {
                ResultMessage.times += 1;
                ResultMessage.text = "先选择一个文件" + new String('!', ResultMessage.times);
            }
            if (this.mode == "参数" & (this.file1 == string.Empty | this.file2 == string.Empty))
            {
                ResultMessage.times += 1;
                ResultMessage.text = "再选择一个文件" + new String('!', ResultMessage.times);
            }
            if (this.mode == "参数" & this.file1 != string.Empty & this.file2 != string.Empty)
            {
                ResultMessage = VlookUp();
            }
        }

        private Message MultiCost()
        {
            List<MultiCost> CostList = ReadCost360(this.file1);

            if (CostList.Count != 0)
            {
                foreach (MultiCost record in CostList)
                {
                    record.platform = "国内页游";
                    record.game = this.game;
                    record.campaign = "360DSP-" + record.campaign.Split('#')[0];
                    record.date = string.IsNullOrWhiteSpace(record.date)?this.date:record.date;
                    record.type = "点击";
                    record.cost = Math.Round(record.cost / 1.42, 2);
                }
                CostList.RemoveAt(CostList.Count - 1);
                CostList = SubsTotalMultiCost(CostList);
                CostList = CostList.Where(p => p.cost != 0).ToList();
                using (var csv = new CsvWriter(new StreamWriter(ExportName, false, Encoding.GetEncoding("GB2312"))))
                {
                    List<string> headerCost = new List<string>() { "平台", "游戏", "广告名", "时间", "计费方式", "消耗" };
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
            }
            else
            {
                if (ResultMessage.text == "^o^")
                {
                    ResultMessage.text = "空文件。";
                }             
            }
            return ResultMessage;
        }

        private Message VlookUp()
        {
            List<SourceID360> sourceid360 = ReadSourceID360(this.file1);
            List<SubsYouzu> subsyouzu = ReadSubsYouzu(this.file2);
            if (sourceid360.Count != 0 & subsyouzu.Count != 0)
            {
                List<SubsYouzu> newSubsList = Combine360Subs(sourceid360, subsyouzu);

                using (var csv = new CsvWriter(new StreamWriter(ExportName, false, UTF8Encoding.UTF8)))
                {
                    csv.WriteRecords(newSubsList);
                }
                this.ResultMessage.code = 1;
                this.ResultMessage.text = ExportName + " done.";
            }
            return ResultMessage;
        }

        public List<SourceID360> ReadSourceID360(string file)
        {
            TextReader reader = new StreamReader(@file, Encoding.UTF8);
            List<SourceID360> SourceIdList = new List<SourceID360>();
            try
            {
                using (CsvReader csv = new CsvReader(reader))
                {
                    CsvConfiguration configuration = new CsvConfiguration();
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

        public List<MultiCost> ReadCost360(string file)
        {
            List<MultiCost> MutiCostList = new List<MultiCost>();
            try
            {
                TextReader reader = new StreamReader(@file, Encoding.BigEndianUnicode); //编码格式
                using (CsvReader csv = new CsvReader(reader))
                {
                    CsvConfiguration configuration = new CsvConfiguration();
                    configuration.Encoding = Encoding.UTF8;
                    csv.Configuration.Delimiter = "	"; // 空格分隔
                    configuration.HasHeaderRecord = true;
                    csv.Configuration.SkipEmptyRecords = true;
                    csv.Configuration.RegisterClassMap<Cost360Map>();
                    MutiCostList = csv.GetRecords<MultiCost>().ToList();
                }
            }
            catch
            {
                this.ResultMessage.code = -1;
                this.ResultMessage.text = "文件格式错误。";
            }
            return MutiCostList;
        }

        public List<MultiCost> ReadCost360FromExcel(string file)
        {
            List<MultiCost> MutiCostList = new List<MultiCost>();
            try
            {
                TextReader reader = new StreamReader(@file, Encoding.BigEndianUnicode); //编码格式
                using (CsvReader csv = new CsvReader(reader))
                {
                    CsvConfiguration configuration = new CsvConfiguration();
                    configuration.Encoding = Encoding.UTF8;
                    csv.Configuration.Delimiter = "	"; // 空格分隔
                    configuration.HasHeaderRecord = true;
                    csv.Configuration.SkipEmptyRecords = true;
                    csv.Configuration.RegisterClassMap<Cost360Map>();
                    MutiCostList = csv.GetRecords<MultiCost>().ToList();
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

        private List<MultiCost> SubsTotalMultiCost(List<MultiCost> list)
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
