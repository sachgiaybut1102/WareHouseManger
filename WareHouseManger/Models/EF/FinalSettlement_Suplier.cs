using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class FinalSettlement_Suplier
    {
        public int ID { get; set; }
        public int? SupplierID { get; set; }
        public string GoodsReceiptID { get; set; }
        public DateTime? DateCreated { get; set; }
        public decimal? Payment { get; set; }
        public decimal? Remainder { get; set; }

        public virtual Shop_Goods_Receipt GoodsReceipt { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
