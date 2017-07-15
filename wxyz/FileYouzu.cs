using CsvHelper.Configuration;
using System;

namespace uvwxyz
{
    public class SubsYouzu
    {
        public string sub1 { get; set; }
        public string sub2 { get; set; }
        public string sub3 { get; set; }
        public string sub4 { get; set; }
        public string sub5 { get; set; }
        public double cost { get; set; }
        public double cpa { get; set; }
        public int click1 { get; set; }
        public int click2 { get; set; }

        public int sb1 { get; set; }
        public string sb2 { get; set; }
        public string sb3 { get; set; }

        public int registernum { get; set; }
        public int registeripnum { get; set; }
        public int backnum { get; set; }
        public string conversionrate { get; set; }
        public int activatenum { get; set; }
        public int rolenum { get; set; }
        public int activateip { get; set; }
        public string activaterate { get; set; }
        public int validnum { get; set; }
        public string validrate { get; set; }
        public int validrole { get; set; }
        public int remain { get; set; }
        public string remainrate { get; set; }
        public int remain7num { get; set; }
        public string remain7rate { get; set; }
        public double remainquality { get; set; }
        public double newusercost { get; set; }
        public double usercost { get; set; }
        public double activatecost { get; set; }
        public double rolecost { get; set; }
        public double validcost { get; set; }
        public double remaincost { get; set; }
        public double remain7cost { get; set; }
        public int newpaidusernum { get; set; }
        public double newpaidcashnum { get; set; }
        public int allpaidusernum { get; set; }
        public double allpaidcashnum { get; set; }
        public string firstdaypaidrate { get; set; }
        public string roi { get; set; }
        public double arppu { get; set; }
        public double paidcost { get; set; }
        public double ltv7 { get; set; }
    }

    public sealed class SubsYouzuMap : CsvClassMap<SubsYouzu>
    {
        public SubsYouzuMap()
        {
            Map(m => m.sub1).Name("sub_1");
            Map(m => m.sub2).Name("sub_2");
            Map(m => m.sub3).Name("sub_3");
            Map(m => m.sub4).Name("sub_4");
            Map(m => m.sub5).Name("sub_5");
            Map(m => m.click1).Name("点击量");
            Map(m => m.click2).Name("点击数");
            Map(m => m.registernum).Name("新注册数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("新注册数")) ? 0 : Convert.ToInt32(row.GetField("新注册数")));
            Map(m => m.registeripnum).Name("注册ip").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("注册ip")) ? 0 : Convert.ToInt32(row.GetField("注册ip")));
            Map(m => m.backnum).Name("回流数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("回流数")) ? 0 : Convert.ToInt32(row.GetField("回流数")));       
            Map(m => m.activatenum).Name("激活数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("激活数")) ? 0 : Convert.ToInt32(row.GetField("激活数")));
            Map(m => m.validnum).Name("有效数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("有效数")) ? 0 : Convert.ToInt32(row.GetField("有效数")));
            Map(m => m.remain).Name("次留数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("次留数")) ? 0 : Convert.ToInt32(row.GetField("次留数")));
            Map(m => m.remain7num).Name("7留数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("7留数")) ? 0 : Convert.ToInt32(row.GetField("7留数")));
            Map(m => m.newpaidusernum).Name("新充值人数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("新充值人数")) ? 0 : Convert.ToInt32(row.GetField("新充值人数")));
            Map(m => m.newpaidcashnum).Name("新充值金额").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("新充值金额")) ? 0 : Convert.ToDouble(row.GetField("新充值金额")));
            Map(m => m.allpaidusernum).Name("总充值人数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("总充值人数")) ? 0 : Convert.ToInt32(row.GetField("总充值人数")));
            Map(m => m.allpaidcashnum).Name("总充值金额").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("总充值金额")) ? 0 : Convert.ToDouble(row.GetField("总充值金额")));
        }
    }

    public class MutilCost
    {
        public string platform { get; set; }
        public string game { get; set; }
        public string campaign { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public double cost { get; set; }    
    }

    public class MutilCost2
    {
        public string platform { get; set; }
        public string game { get; set; }
        public string campaign { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string cost { get; set; }
    }

    public class Message
    {
        public int code { get; set; }
        public string text { get; set; }
        public int times { get; set; }
    }
}
