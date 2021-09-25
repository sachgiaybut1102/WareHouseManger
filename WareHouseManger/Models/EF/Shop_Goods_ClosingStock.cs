using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_ClosingStock
    {
        public Shop_Goods_ClosingStock()
        {
            Shop_Goods_ClosingStock_Details = new HashSet<Shop_Goods_ClosingStock_Detail>();
        }

        public string ClosingStockID { get; set; }
        public string Name { get; set; }
        public DateTime? DateClosing { get; set; }

        public virtual ICollection<Shop_Goods_ClosingStock_Detail> Shop_Goods_ClosingStock_Details { get; set; }
    }
}
