using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class FinalSettlement_Customer
    {
        public int ID { get; set; }
        public int? CustomerID { get; set; }
        public string GoodsIssuesID { get; set; }
        public DateTime? DateCreated { get; set; }
        public decimal? Payment { get; set; }
        public decimal? Remainder { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Shop_Goods_Issue GoodsIssues { get; set; }
    }
}
