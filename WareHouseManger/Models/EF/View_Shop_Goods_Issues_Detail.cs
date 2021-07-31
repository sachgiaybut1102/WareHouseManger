using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class View_Shop_Goods_Issues_Detail
    {
        public string GoodsIssueID { get; set; }
        public string TemplateID { get; set; }
        public int? Count { get; set; }
        public int? UnitPrice { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string UnitName { get; set; }
    }
}
