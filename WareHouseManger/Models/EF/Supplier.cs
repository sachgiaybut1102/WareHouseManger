using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Supplier
    {
        public Supplier()
        {
            FinalSettlement_Supliers = new HashSet<FinalSettlement_Suplier>();
            Shop_Goods_Receipts = new HashSet<Shop_Goods_Receipt>();
        }

        public int SupplierID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }

        public virtual ICollection<FinalSettlement_Suplier> FinalSettlement_Supliers { get; set; }
        public virtual ICollection<Shop_Goods_Receipt> Shop_Goods_Receipts { get; set; }
    }
}
