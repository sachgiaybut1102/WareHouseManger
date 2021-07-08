using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Employee
    {
        public Employee()
        {
            Accounts = new HashSet<Account>();
            Shop_Goods_Issues = new HashSet<Shop_Goods_Issue>();
            Shop_Goods_Receipts = new HashSet<Shop_Goods_Receipt>();
            Shop_Goods_StockTakes = new HashSet<Shop_Goods_StockTake>();
        }

        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }
        public int? PositionID { get; set; }

        public virtual Position Position { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Shop_Goods_Issue> Shop_Goods_Issues { get; set; }
        public virtual ICollection<Shop_Goods_Receipt> Shop_Goods_Receipts { get; set; }
        public virtual ICollection<Shop_Goods_StockTake> Shop_Goods_StockTakes { get; set; }
    }
}
