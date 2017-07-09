using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxyz
{
    public class SubsYouzu
    {
        public string sub1 { get; set; }
        public string sub2 { get; set; }
        public string sub3 { get; set; }
        public string sub4 { get; set; }
        public string sub5 { get; set; }

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

            Map(m => m.sb1).Name("素材加载量").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("素材加载量")) ? 0 : Convert.ToInt32(row.GetField("素材加载量")));
            Map(m => m.sb2).Name("素材点击率").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("素材点击率")) ? string.Empty : Convert.ToString(row.GetField("素材点击率")));
            Map(m => m.sb3).Name("按钮点击率").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("按钮点击率")) ? string.Empty : Convert.ToString(row.GetField("按钮点击率")));

            Map(m => m.registernum).Name("新注册数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("新注册数")) ? 0 : Convert.ToInt32(row.GetField("新注册数")));
            Map(m => m.registeripnum).Name("注册ip").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("注册ip")) ? 0 : Convert.ToInt32(row.GetField("注册ip")));
            Map(m => m.backnum).Name("回流数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("回流数")) ? 0 : Convert.ToInt32(row.GetField("回流数")));
            Map(m => m.conversionrate).Name("注册转化率").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("注册转化率")) ? string.Empty : Convert.ToString(row.GetField("注册转化率")));
            Map(m => m.activatenum).Name("激活数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("激活数")) ? 0 : Convert.ToInt32(row.GetField("激活数")));
            Map(m => m.rolenum).Name("角色数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("角色数")) ? 0 : Convert.ToInt32(row.GetField("角色数")));
            Map(m => m.activateip).Name("激活ip").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("激活ip")) ? 0 : Convert.ToInt32(row.GetField("激活ip")));
            Map(m => m.activaterate).Name("激活率").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("激活率")) ? string.Empty : Convert.ToString(row.GetField("激活率")));
            Map(m => m.validnum).Name("有效数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("有效数")) ? 0 : Convert.ToInt32(row.GetField("有效数")));
            Map(m => m.validrate).Name("有效率").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("有效率")) ? string.Empty : Convert.ToString(row.GetField("有效率")));
            Map(m => m.validrole).Name("有效角色").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("有效角色")) ? 0 : Convert.ToInt32(row.GetField("有效角色")));
            Map(m => m.remain).Name("次留数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("次留数")) ? 0 : Convert.ToInt32(row.GetField("次留数")));
            Map(m => m.remainrate).Name("次留率").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("次留率")) ? string.Empty : Convert.ToString(row.GetField("次留率")));
            Map(m => m.remain7num).Name("7留数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("7留数")) ? 0 : Convert.ToInt32(row.GetField("7留数")));
            Map(m => m.remain7rate).Name("7留率").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("7留率")) ? string.Empty : Convert.ToString(row.GetField("7留率")));
            Map(m => m.remainquality).Name("留存品质").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("留存品质")) ? 0 : Convert.ToDouble(row.GetField("留存品质")));
            Map(m => m.newusercost).Name("新用户成本").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("新用户成本")) ? 0 : Convert.ToDouble(row.GetField("新用户成本")));
            Map(m => m.usercost).Name("用户成本").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("用户成本")) ? 0 : Convert.ToDouble(row.GetField("用户成本")));
            Map(m => m.activatecost).Name("激活成本").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("激活成本")) ? 0 : Convert.ToDouble(row.GetField("激活成本")));
            Map(m => m.rolecost).Name("角色成本").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("角色成本")) ? 0 : Convert.ToDouble(row.GetField("角色成本")));
            Map(m => m.validcost).Name("有效成本").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("有效成本")) ? 0 : Convert.ToDouble(row.GetField("有效成本")));
            Map(m => m.remaincost).Name("次留成本").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("次留成本")) ? 0 : Convert.ToDouble(row.GetField("次留成本")));
            Map(m => m.remain7cost).Name("7留成本").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("7留成本")) ? 0 : Convert.ToDouble(row.GetField("7留成本")));
            Map(m => m.newpaidusernum).Name("新充值人数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("新充值人数")) ? 0 : Convert.ToInt32(row.GetField("新充值人数")));
            Map(m => m.newpaidcashnum).Name("新充值金额").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("新充值金额")) ? 0 : Convert.ToDouble(row.GetField("新充值金额")));
            Map(m => m.allpaidusernum).Name("总充值人数").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("总充值人数")) ? 0 : Convert.ToInt32(row.GetField("总充值人数")));
            Map(m => m.allpaidcashnum).Name("总充值金额").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("总充值金额")) ? 0 : Convert.ToDouble(row.GetField("总充值金额")));
            Map(m => m.firstdaypaidrate).Name("首日付费率").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("首日付费率")) ? string.Empty : Convert.ToString(row.GetField("首日付费率")));
            Map(m => m.roi).Name("ROI").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("ROI")) ? string.Empty : Convert.ToString(row.GetField("ROI")));
            Map(m => m.arppu).Name("ARPPU").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("ARPPU")) ? 0 : Convert.ToDouble(row.GetField("ARPPU")));
            Map(m => m.paidcost).Name("付费成本").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("付费成本")) ? 0 : Convert.ToDouble(row.GetField("付费成本")));
            Map(m => m.ltv7).Name("7日LTV").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("7日LTV")) ? 0 : Convert.ToDouble(row.GetField("7日LTV")));


            /*
            Map(m => m.sub1).Index(0);
            Map(m => m.sub2).Index(1);
            Map(m => m.sub3).Index(2);
            Map(m => m.sub4).Index(3);
            Map(m => m.sub5).Index(4);
            Map(m => m.click1).Index(5);
            Map(m => m.click2).Index(6);
            */
        }
    }

    
    public class SourceID360
    {

    }

    public sealed class SourceID360Map : CsvClassMap<SourceID360>
    {

    }
    
    public class Cost360
    {
        public string game { get; set; }
        public string campaign { get; set; }
        public double cost { get; set; }
        public string date { get; set; }
        public string type { get; set; }
    }

    public sealed class Cost360Map: CsvClassMap<Cost360>
    {
        public Cost360Map()
        {
            Map(m => m.campaign).Name("推广计划").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("推广计划")) ? string.Empty : Convert.ToString(row.GetField("推广计划")));
            Map(m => m.cost).Name("花费").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("花费")) ? 0 : Convert.ToDouble(row.GetField("花费")));
        }
    }
}
