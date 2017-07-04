using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxyz
{
    public class SourceId360
    {
        public string sub1 { get; set; }
        public string sub2 { get; set; }
        public string sub3 { get; set; }
        public string sub4 { get; set; }
        public string sub5 { get; set; }
        public int click1 { get; set; }
        public int click2 { get; set; }
        public int sb1 { get; set; }
        public int sb2 { get; set; }
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
        public string remain7num { get; set; }
        public string remain7rate { get; set; }
        public string remainquality { get; set; }
        public float newusercost { get; set; }
        public float usercost { get; set; }
        public float activatecost { get; set; }
        public float rolecost { get; set; }
        public float validcost { get; set; }
        public float remaincost { get; set; }
        public float remain7cost { get; set; }
        public int newpaidusernum { get; set; }
        public int newpaidcashnum { get; set; }
        public int allpaidusernum { get; set; }
        public int allpaidcashnum { get; set; }
        public string firstdaypaidrate { get; set; }
        public string roi { get; set; }
        public float arppu { get; set; }
        public float paidcost { get; set; }
        public float ltv7 { get; set; }



    }

    public sealed class SourceId360Map : CsvClassMap<SourceId360>
    {
        public SourceId360Map()
        {
            Map(m => m.sub1).Name("sub_1"); 
            Map(m => m.sub2).Name("sub_2"); 
            Map(m => m.sub3).Name("sub_3"); 
            Map(m => m.sub4).Name("sub_4");
            Map(m => m.sub5).Name("sub_5");
            Map(m => m.click1).Name("点击量");
            Map(m => m.click2).Name("点击数");
            Map(m => m.sb1).Name("素材加载量");
            Map(m => m.sub2).Name("素材点击率");
            Map(m => m.sub3).Name("按钮点击率");
            Map(m => m.registernum).Name("新注册数");
            Map(m => m.registeripnum).Name("注册ip"); 
            Map(m => m.backnum).Name("回流数");
            Map(m => m.conversionrate).Name("注册转化率");
            Map(m => m.activatenum).Name("激活数");
            Map(m => m.rolenum).Name("角色数");
            Map(m => m.activateip).Name("激活ip"); 
            Map(m => m.activaterate).Name("激活率");
            Map(m => m.validnum).Name("有效数");
            Map(m => m.validrate).Name("有效率");
            Map(m => m.validrole).Name("有效角色");
            Map(m => m.remain).Name("次留数");
            Map(m => m.remainrate).Name("次留率");
            Map(m => m.remain7num).Name("7留数");
            Map(m => m.remain7rate).Name("7留率");
            Map(m => m.remainquality).Name("留存品质");
            Map(m => m.newusercost).Name("新用户成本");
            Map(m => m.usercost).Name("用户成本");
            Map(m => m.activatecost).Name("激活成本");
            Map(m => m.rolecost).Name("角色成本");
            Map(m => m.validcost).Name("有效成本");
            Map(m => m.remaincost).Name("次留成本");
            Map(m => m.remain7cost).Name("7留成本");
            Map(m => m.newpaidusernum).Name("新充值人数");
            Map(m => m.newpaidcashnum).Name("新充值金额");
            Map(m => m.allpaidusernum).Name("总充值人数");
            Map(m => m.allpaidcashnum).Name("总充值金额");
            Map(m => m.firstdaypaidrate).Name("首日付费率");
            Map(m => m.roi).Name("ROI");
            Map(m => m.arppu).Name("ARPPU");
            Map(m => m.paidcost).Name("付费成本");
            Map(m => m.ltv7).Name("7日LTV");

        }
    }

    public sealed class SubsYouzuMap : CsvClassMap<BaseClass>
    {

    }

    public sealed class Cost360Map : CsvClassMap<BaseClass>
    {

    }
}
