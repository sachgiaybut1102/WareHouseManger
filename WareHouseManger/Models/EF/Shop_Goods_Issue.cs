using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_Issue
    {
        public Shop_Goods_Issue()
        {
            FinalSettlement_Customers = new HashSet<FinalSettlement_Customer>();
            Shop_Goods_Issues_Details = new HashSet<Shop_Goods_Issues_Detail>();
        }

        public string GoodsIssueID { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CustomerID { get; set; }
        public string Remark { get; set; }
        public decimal? Total { get; set; }
        public int? EmployeeID { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<FinalSettlement_Customer> FinalSettlement_Customers { get; set; }
        public virtual ICollection<Shop_Goods_Issues_Detail> Shop_Goods_Issues_Details { get; set; }
    }
}
