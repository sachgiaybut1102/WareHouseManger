using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Good
    {
        public Shop_Good()
        {
            Shop_Goods_Issues_Details = new HashSet<Shop_Goods_Issues_Detail>();
            Shop_Goods_Receipt_Details = new HashSet<Shop_Goods_Receipt_Detail>();
            Shop_Goods_StockTake_Details = new HashSet<Shop_Goods_StockTake_Detail>();
        }

        public string TemplateID { get; set; }
        public string Name { get; set; }
        public int? CategoryID { get; set; }
        public int? UnitID { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public int? Count { get; set; }
        public int? CountMin { get; set; }
        public int? ProducerID { get; set; }

        public virtual Shop_Goods_Category Category { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual Shop_Goods_Unit Unit { get; set; }
        public virtual ICollection<Shop_Goods_Issues_Detail> Shop_Goods_Issues_Details { get; set; }
        public virtual ICollection<Shop_Goods_Receipt_Detail> Shop_Goods_Receipt_Details { get; set; }
        public virtual ICollection<Shop_Goods_StockTake_Detail> Shop_Goods_StockTake_Details { get; set; }
    }
}
