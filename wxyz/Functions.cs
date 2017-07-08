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
        public static string[] GetFileInfo(string path)
        {
            string[] FileInfo = new string[3];
            FileInfo file = new FileInfo(path);
            FileInfo[0] = "  " + Path.GetFileName(path);
            FileInfo[1] = "  " +  file.CreationTime.ToString();
            FileInfo[2] = "  " +  (file.Length / 1024).ToString() + " KB";
            return FileInfo;
        }


        public static void ButtonFunction(string channel, string mode, params string[] files)
        {
            DataTable CostDT = new DataTable();
            DataColumn column1 = new DataColumn("game");
            column1.DataType = Type.GetType("System.String");
            CostDT.Columns.Add(column1);
            DataColumn column2 = new DataColumn("campaign");
            column2.DataType = Type.GetType("System.String");
            CostDT.Columns.Add(column2);
            DataColumn column3 = new DataColumn("date");
            column3.DataType = Type.GetType("DateTime");
            CostDT.Columns.Add(column3);
            DataColumn column4 = new DataColumn("type");
            column4.DataType = Type.GetType("String");
            CostDT.Columns.Add(column4);
            DataColumn column5 = new DataColumn("cost");
            column5.DataType = Type.GetType("Double");
            CostDT.Columns.Add(column5);



            //file1 渠道，file2 游族
            if (channel == "360")
            {
                if (mode == "花费")
                {
                    List<Cost360> cost = ReadCost360(files[0]);
                    foreach(Cost360 record in cost)
                    {
                        DataRow r = CostDT.DataRow();
                        r["game"] = "360DSP-" + record.project;

                    }

                }
                else if (mode == "拼表")
                {
                    List<SourceID360> sourceid360 = ReadSourceId360(files[0]);
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
