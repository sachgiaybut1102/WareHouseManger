using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_Receipt_Detail
    {
        public string GoodsReceiptID { get; set; }
        public string TemplateID { get; set; }
        public int? Count { get; set; }
        public int? UnitPrice { get; set; }

        public virtual Shop_Goods_Receipt GoodsReceipt { get; set; }
        public virtual Shop_Good Template { get; set; }
    }
}
