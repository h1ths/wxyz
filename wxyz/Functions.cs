using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Data;

namespace wxyz
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
            string ExportName = GetFileName(Environment.CurrentDirectory, this.channel + "-" + this.game + "-花费-" + this.date.Replace("/", ""), ".csv");
            List<string> header = new List<string>() { "平台", "游戏", "层级", "时间", "计费方式", "是否是品牌", "消耗" };
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
                    foreach(MutilCost record in costlist)
                    {
                        record.platform = "国内页游";
                        record.game = this.game;
                        record.campaign = "360DSP-" + record.campaign;
                        record.date = this.date;                  
                        record.type = "点击";
                        record.brand = "否";
                        record.cost = Math.Round(record.cost / 1.42, 2);
                    }
                    costlist.RemoveAt(costlist.Count - 1);

                    using (var csv = new CsvWriter(new StreamWriter(ExportName, false, UTF8Encoding.UTF8)))
                    {
                        foreach(var i in header)
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
                        List<SourceID360> sourceid360 = ReadSourceId360(this.file1);
                        List<SubsYouzu> subsyouzu = ReadSubsYouzu(this.file2);

                        using (var csv = new CsvWriter(new StreamWriter(ExportName, false, UTF8Encoding.UTF8)))
                        {
                            //UTF8 with bom 
                            csv.WriteRecords(sourceid360);
                        }
                        this.ResultMessage.code = 1;
                        this.ResultMessage.text = ExportName + " done.";
                    }
                }
            }

            /*
            if (channel == "新数")
            {
                if (mode == "花费")
                {

                }
                else if (mode == "拼表")
                {

                }
            }
            */

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
                    }
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
                        record.brand = "否";
                        record.cost = Math.Round(record.cost, 2);
                    }

                    using (var csv = new CsvWriter(new StreamWriter(ExportName, false, Encoding.UTF8)))
                    {
                        foreach (var i in header)
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
            }
            
            return ResultMessage;
        }


        public static List<SubsYouzu> ReadSubsYouzu(string file)
        {
            TextReader reader = new StreamReader(@file, Encoding.UTF8);
            List<SubsYouzu> objs = new List<SubsYouzu>();
            using (CsvReader csv = new CsvReader(reader))
            {
                CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                configuration.Encoding = Encoding.UTF8;
                configuration.HasHeaderRecord = true;
                csv.Configuration.SkipEmptyRecords = true;
                csv.Configuration.RegisterClassMap<SubsYouzuMap>();            
                objs = csv.GetRecords<SubsYouzu>().ToList();
                /*
                Console.WriteLine(objs.Count());
                foreach (SubsYouzu s in objs)
                {
                    Console.WriteLine(s.sub1);
                }
                Console.ReadKey();
                */
                return objs;
            }
        }

        public static List<SourceID360> ReadSourceId360(string file)
        {
            TextReader reader = new StreamReader(@file, Encoding.UTF8);
            List<SourceID360> objs = new List<SourceID360>();
            using (CsvReader csv = new CsvReader(reader))
            {
                CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                configuration.Encoding = Encoding.UTF8;
                configuration.HasHeaderRecord = true;
                csv.Configuration.SkipEmptyRecords = true;
                csv.Configuration.RegisterClassMap<SourceID360Map>();
                objs = csv.GetRecords<SourceID360>().ToList();
                return objs;
            }
        }

        public List<MutilCost> ReadCost360(string file)
        {
            List<MutilCost> objs = new List<MutilCost>();
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
                    objs = csv.GetRecords<MutilCost>().ToList();
                    return objs;
                }
            }
            catch
            {
                this.ResultMessage.code = -1;
                FileInfo fileinfo = new FileInfo(file);
                this.ResultMessage.text = fileinfo.Name + "can not be parsed.";
                return objs;
            }
        }

        public static void Combine360Subs(List<SourceID360> SourceIdList, List<SubsYouzu> SubsYouzuList)
        {
            SourceIdList = SourceIdList.OrderBy(x => x.sourceid).Distinct().ToList();
            SubsYouzuList = SubsYouzuList.OrderBy(x => x.sub3).Distinct().ToList(); //sourceid 在sub3

            List<SubsYouzu> newSubsYouzu = new List<SubsYouzu>();

            var queryNotExist = SourceIdList.Where(p => !SubsYouzuList.Select(g => g.sub3).Contains(p.sourceid)).ToList();
            // var query=lista.Where(p=>!listb.Any(g=>p.id==g.id && p.no==g.no))

            List<SubsYouzu> extendList = ExtendToSubsYouzu(queryNotExist);


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

            foreach (var item in newSubsYouzu)
            {
                Console.Write(item.sub1 + ", " + item.sub3 + "\n");
            }
            Console.ReadKey();

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

        public static List<SubsYouzu> ExtendToSubsYouzu(List<SourceID360> restList)
        {
            List<SubsYouzu> extendList = new List<SubsYouzu>();

            foreach (var item in restList)
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


        public List<MutilCost> ReadCostSF(string file)
        {
            List<MutilCost> objs = new List<MutilCost>();
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
                    objs = csv.GetRecords<MutilCost>().ToList();
                    return objs;
                }
            }
            catch
            {
                this.ResultMessage.code = -1;
                FileInfo fileinfo = new FileInfo(file);
                this.ResultMessage.text = fileinfo.Name + "can not be parsed.";
                return objs;
            }
        }
    }




}
