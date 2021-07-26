using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WareHouseManger.Models.EF
{
    public class StatisticsShopGoodsInfo
    {
        public string TemplateID { get; set; }
        public string Name { get; set; }
        public decimal Count { get; set; }
        public decimal Turnover { get; set; }
        public string Unit { get; set; }
    }
}
