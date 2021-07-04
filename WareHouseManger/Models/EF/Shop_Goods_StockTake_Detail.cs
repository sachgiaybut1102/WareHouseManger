using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_StockTake_Detail
    {
        public int StockTakeID { get; set; }
        public string TemplateID { get; set; }
        public int? AmountOfStock { get; set; }
        public int? ActualAmount { get; set; }

        public virtual Shop_Goods_StockTake StockTake { get; set; }
        public virtual Shop_Good Template { get; set; }
    }
}
