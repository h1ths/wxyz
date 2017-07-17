using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
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

    public sealed class CostSFMap : CsvClassMap<MutilCost>
    {
        public CostSFMap()
        {
            Map(m => m.campaign).Name("活动名称").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("活动名称")) ? string.Empty : Convert.ToString(row.GetField("活动名称")));
            Map(m => m.cost).Name("总消费(元)").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("总消费(元)")) ? 0 : Convert.ToDouble(row.GetField("总消费(元)")));
        }
    }
}
