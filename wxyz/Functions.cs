using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;

namespace wxyz
{
    public class Functions
    {
        public static string[] GetFileInfo(string path)
        {
            string[] FileInfo = new string[3];
            FileInfo file = new FileInfo(path);
            FileInfo[0] = "  " + Path.GetFileName(path);
            FileInfo[1] = "  " +  file.CreationTime.ToString();
            FileInfo[2] = "  " +  (file.Length / 1024).ToString() + " KB";
            return FileInfo;
        }


        public static void ReadSubsYouzu(string channel, string mode, params string[] files)
        {

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

        public static List<Cost360> ReadCost360(string file)
        {
            TextReader reader = new StreamReader(@file, Encoding.UTF8);
            List<Cost360> objs = new List<Cost360>();
            using (CsvReader csv = new CsvReader(reader))
            {
                CsvHelper.Configuration.CsvConfiguration configuration = new CsvHelper.Configuration.CsvConfiguration();
                configuration.Encoding = Encoding.UTF8;
                configuration.HasHeaderRecord = true;
                csv.Configuration.SkipEmptyRecords = true;
                csv.Configuration.RegisterClassMap<Cost360Map>();
                objs = csv.GetRecords<Cost360>().ToList();
                return objs;
            }
        }
    }
    
}
