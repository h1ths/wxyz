using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uvwxyz
{
    public class SourceIDSF
    {
        public string sourcename { get; set; }
        public string sourceid { get; set; }
        public string channel { get; set; }
        public double cost { get; set; }
    }

    public sealed class SourceIDSFMap : CsvClassMap<SourceIDSF>
    {
        public SourceIDSFMap()
        {
            Map(m => m.sourcename).Name("广告位名称").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("广告位名称")) ? string.Empty : Convert.ToString(row.GetField("广告位名称")));
            Map(m => m.sourceid).Name("广告位ID").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("广告位ID")) ? string.Empty : Convert.ToString(row.GetField("广告位ID")));
            Map(m => m.channel).Name("渠道").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("渠道")) ? string.Empty : Convert.ToString(row.GetField("渠道")));
            Map(m => m.cost).ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("总消费(元)")) ? 0 : Convert.ToDouble(row.GetField("总消费(元)")));
        }
    }

    public sealed class CostSFMap : CsvClassMap<MultiCost>
    {
        public CostSFMap()
        {
            Map(m => m.campaign).Name("活动名称").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("活动名称")) ? string.Empty : Convert.ToString(row.GetField("活动名称")));
            Map(m => m.cost).Name("总消费(元)").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("总消费(元)")) ? 0 : Convert.ToDouble(row.GetField("总消费(元)")));
        }
    }

    class FunctionShunfei
    {
        string mode;
        string file1;
        string file2;
        string game;
        string date;
        string ExportName;
        public Message ResultMessage;

        public FunctionShunfei(string mode, string file1, string file2, string game, string date, string ExportName, Message ResultMessage)
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
            List<MultiCost> CostList = ReadCostSF(this.file1);
            if (CostList.Count != 0)
            {
                CostList.RemoveAt(0);               

                foreach (MultiCost record in CostList)
                {
                    record.platform = "国内页游";
                    record.game = this.game;
                    record.campaign = "舜飞DSP-" + record.campaign.Split('#')[0];
                    record.date = this.date;
                    record.type = "点击";
                    record.cost = Math.Round(record.cost, 2);
                }
                CostList = SubsTotalMultiCost(CostList);
                CostList = CostList.Where(p => p.cost != 0).ToList();
                using (var csv = new CsvWriter(new StreamWriter(ExportName, false, Encoding.GetEncoding("GB2312"))))
                {
                    List<string> headerCost = new List<string>() { "平台", "游戏", "广告名", "时间", "计费方式", "消耗" };
                    CsvConfiguration configuration = new CsvConfiguration();
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
            else
            {
                if (ResultMessage.text == "^o^")
                {
                    ResultMessage.text = "空文件。";
                }
                return ResultMessage;
            }
        }

        private Message VlookUp()
        {
            List<SourceIDSF> SourceIDSFList = ReadSourceIDSF(this.file1);
            List<SubsYouzu> SubsYouzuList = ReadSubsYouzu(this.file2);

            if (SourceIDSFList.Count != 0 & SubsYouzuList.Count != 0)
            {
                List<SubsYouzu> newSubsList = CombineSFSubs(SourceIDSFList, SubsYouzuList);

                using (var csv = new CsvWriter(new StreamWriter(ExportName, false, Encoding.GetEncoding("GB2312"))))
                {
                    csv.WriteRecords(newSubsList);
                }
                this.ResultMessage.code = 1;
                this.ResultMessage.text = ExportName + " done.";
            }
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

        public List<MultiCost> ReadCostSF(string file)
        {
            List<MultiCost> MutiCostList = new List<MultiCost>();
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
    }
}

