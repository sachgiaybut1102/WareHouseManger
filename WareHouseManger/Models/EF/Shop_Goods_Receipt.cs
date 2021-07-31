using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_Receipt
    {
        public Shop_Goods_Receipt()
        {
            FinalSettlement_Supliers = new HashSet<FinalSettlement_Suplier>();
            Shop_Goods_Receipt_Details = new HashSet<Shop_Goods_Receipt_Detail>();
        }

        public string GoodsReceiptID { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? SupplierID { get; set; }
        public string Remark { get; set; }
        public decimal? Total { get; set; }
        public decimal? Prepay { get; set; }
        public decimal? TransferMoney { get; set; }
        public int? EmployeeID { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<FinalSettlement_Suplier> FinalSettlement_Supliers { get; set; }
        public virtual ICollection<Shop_Goods_Receipt_Detail> Shop_Goods_Receipt_Details { get; set; }
    }
}
