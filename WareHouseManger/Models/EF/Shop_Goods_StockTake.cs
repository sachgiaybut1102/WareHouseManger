using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_StockTake
    {
        public Shop_Goods_StockTake()
        {
            Shop_Goods_StockTake_Details = new HashSet<Shop_Goods_StockTake_Detail>();
        }

        public string StockTakeID { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Remark { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? DateUpdate { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<Shop_Goods_StockTake_Detail> Shop_Goods_StockTake_Details { get; set; }
    }
}
