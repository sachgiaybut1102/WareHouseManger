using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Customer
    {
        public Customer()
        {
            FinalSettlement_Customers = new HashSet<FinalSettlement_Customer>();
            Shop_Goods_Issues = new HashSet<Shop_Goods_Issue>();
        }

        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }
        public int? CustomerCategoryID { get; set; }

        public virtual Customer_Category CustomerCategory { get; set; }
        public virtual ICollection<FinalSettlement_Customer> FinalSettlement_Customers { get; set; }
        public virtual ICollection<Shop_Goods_Issue> Shop_Goods_Issues { get; set; }
    }
}
