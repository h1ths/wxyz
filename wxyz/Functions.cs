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

        public Functions()
        {
            DirectoryCheck();
        }

        private void DirectoryCheck()
        {
            if (Directory.Exists(@"新数/花费") == false)
            {
                Directory.CreateDirectory(@"新数/花费");
            }
            if (Directory.Exists(@"新数/参数") == false)
            {
                Directory.CreateDirectory(@"新数/参数");
            }
            if (Directory.Exists(@"舜飞/花费") == false)
            {
                Directory.CreateDirectory(@"舜飞/花费");
            }
            if (Directory.Exists(@"舜飞/参数") == false)
            {
                Directory.CreateDirectory(@"舜飞/参数");
            }
            if (Directory.Exists(@"360/花费") == false)
            {
                Directory.CreateDirectory(@"360/花费");
            }
            if (Directory.Exists(@"360/参数") == false)
            {
                Directory.CreateDirectory(@"360/参数");
            }
        }

        public static string[] GetFileInfo(string path)
        {
            string[] FileInfo = new string[3];
            FileInfo file = new FileInfo(path);
            FileInfo[0] = "  " + Path.GetFileName(path);
            FileInfo[1] = "  " + file.CreationTime.ToString();
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

        public Message ButtonFunction(string mode, string date, string channel, string game, string file1, string file2)
        {
            this.mode = mode;
            this.date = date;
            this.channel = channel;
            this.game = game;
            this.file1 = file1;
            this.file2 = file2;
            ResultMessage = new Message { code = 0, text = "^o^", times = 0 };
            string ExportName = GetFilePath(this.channel + "/" + this.mode, this.channel + "-" + this.game + "-" + this.mode + "-" + this.date.Replace("/", ""), ".csv");
            List<string> headerCost = new List<string>() { "平台", "游戏", "广告名", "时间", "计费方式", "消耗" };
            List<string> headerSubs = new List<string>() { "平台", "游戏", "广告名", "时间", "计费方式", "消耗" };
            //file1 渠道，file2 游族
            if (this.channel == "360")
            {
                Function360 a360 = new Function360(mode, file1, file2, game, date, ExportName, ResultMessage);
                ResultMessage = a360.ResultMessage;
            }

            if (channel == "舜飞")
            {
                FunctionShunfei shunfei = new FunctionShunfei(mode, file1, file2, game, date, ExportName, ResultMessage);
                ResultMessage = shunfei.ResultMessage;
            }
            if (channel == "新数")
            {
                FunctionXinshu xinshu = new FunctionXinshu(mode, file1, file2, game, date, ExportName, ResultMessage);
                ResultMessage = xinshu.ResultMessage;
            }
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


        public static string GetFilePath(string path, string name, string extension)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            int number = directory.EnumerateFiles().Where(f => f.Name.Contains(name) && f.Extension == extension).Count();
            if (number == 0)
            {
                return path + "/" + name + extension;
            }
            else
            {
                return path + "/" + name + "[" + number.ToString() + "]" + extension;
            }

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
