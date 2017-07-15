using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uvwxyz
{
    public class SourceID360
    {
        public string sourceid { get; set; }
        public string campaign { get; set; }
        public string group { get; set; }
        public string adposition { get; set; }
        public double cost { get; set; }
    }

    public sealed class SourceID360Map : CsvClassMap<SourceID360>
    {
        public SourceID360Map()
        {
            Map(m => m.sourceid).Name("sourceId").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("sourceId")) ? string.Empty : Convert.ToString(row.GetField("sourceId")));
            Map(m => m.campaign).Name("推广计划").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("推广计划")) ? string.Empty : Convert.ToString(row.GetField("推广计划")));
            Map(m => m.group).Name("推广组").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("推广组")) ? string.Empty : Convert.ToString(row.GetField("推广组")));
            Map(m => m.adposition).Name("广告位").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("广告位")) ? string.Empty : Convert.ToString(row.GetField("广告位")));
            Map(m => m.cost).Name("扣费(元)").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("扣费(元)")) ? 0 : Convert.ToDouble(row.GetField("扣费(元)")));
        }
    }

    public sealed class Cost360Map : CsvClassMap<MutilCost>
    {
        public Cost360Map()
        {
            Map(m => m.campaign).Name("推广计划").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("推广计划")) ? string.Empty : Convert.ToString(row.GetField("推广计划")));
            Map(m => m.cost).Name("花费").ConvertUsing(row => string.IsNullOrWhiteSpace(row.GetField("花费")) ? 0 : Convert.ToDouble(row.GetField("花费")));
        }
    }
}
