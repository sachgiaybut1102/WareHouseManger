using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_ClosingStock_Detail
    {
        public string ClosingStockID { get; set; }
        public string TemplateID { get; set; }
        public int? Count { get; set; }

        public virtual Shop_Goods_ClosingStock ClosingStock { get; set; }
        public virtual Shop_Good Template { get; set; }
    }
}
