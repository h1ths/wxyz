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
        private Dictionary<string, string> ResultMessage;

        public Functions(string mode, string date, string channel, string game, string file1, string file2)
        {
            this.mode = mode;
            this.date = date;
            this.channel = channel;
            this.game = game;
            this.file1 = file1;
            this.file2 = file2;
            ResultMessage = new Dictionary<string, string>();
            ResultMessage.Add("code", "0");
            ResultMessage.Add("message", "Ready.");
        }

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

        public Dictionary<string,string>  ButtonFunction()
        {


            string ExportName = GetFileName(Environment.CurrentDirectory, this.channel + "-" + this.game + "-花费-" + this.date.Replace("/", ""), ".csv");

            //file1 渠道，file2 游族
            if (this.channel == "360")
            {
                if (this.mode == "花费")
                {
                    List<Cost360> costlist = ReadCost360(this.file1);
                    foreach(Cost360 record in costlist)
                    {
                        record.game = this.game;
                        record.date = this.date;
                        record.campaign = "360DSP-" + record.campaign;
                        record.type = "点击";
                        record.cost = Math.Round(record.cost / 1.42, 2);
                    }
                    costlist.RemoveAt(costlist.Count - 1);

                    using (var csv = new CsvWriter(new StreamWriter(ExportName, false, UTF8Encoding.UTF8)))
                    {
                        //UTF8 with bom 
                        csv.WriteRecords(costlist);
                    }
                    this.ResultMessage["code"] = "1";
                    this.ResultMessage["message"] = ExportName + " done.";
                    
                }
                else if (mode == "拼表")
                {
                    List<SourceID360> sourceid360 = ReadSourceId360(this.file1);
                    List<SubsYouzu> subsyouzu = ReadSubsYouzu(this.file2);

                    using (var csv = new CsvWriter(new StreamWriter(ExportName, false, UTF8Encoding.UTF8)))
                    {
                        //UTF8 with bom 
                        csv.WriteRecords(sourceid360);
                    }
                    this.ResultMessage["code"] = "1";
                    this.ResultMessage["message"] = ExportName + " done.";

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
            if (channel == "舜飞")
            {
                if (mode == "花费")
                {

                }
                else if (mode == "拼表")
                {

                }
            }
            */

            return this.ResultMessage;
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

        public List<Cost360> ReadCost360(string file)
        {
            List<Cost360> objs = new List<Cost360>();
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
                    objs = csv.GetRecords<Cost360>().ToList();
                    return objs;
                }
            }
            catch
            {
                this.ResultMessage["code"] = "-1";
                FileInfo fileinfo = new FileInfo(file);
                this.ResultMessage["message"] = fileinfo.Name + "can not be parsed.";
                return objs;
            }
        }

        public static List<SubsYouzu> Combine360Subs(List<SourceID360> SourceIdList, List<SubsYouzu> SubsYouzuList)
        {
            SourceIdList = SourceIdList.OrderBy(x => x.sourceid).Distinct().ToList();
            SubsYouzuList = SubsYouzuList.OrderBy(x => x.sub3).Distinct().ToList(); //sourceid 在sub3
            /*
            List<SubsYouzu> SubsList = from record in SubsYouzuList
                                    where SourceIdList.Count(t => t.sourceid == record.sub3)
                                    select record;
            */

            return SubsYouzuList;
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
    }
    
    
}
