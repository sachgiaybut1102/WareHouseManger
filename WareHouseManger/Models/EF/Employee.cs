using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Employee
    {
        public Employee()
        {
            Shop_Goods_Receipts = new HashSet<Shop_Goods_Receipt>();
            Shop_Goods_StockTakes = new HashSet<Shop_Goods_StockTake>();
        }

        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }

        public virtual ICollection<Shop_Goods_Receipt> Shop_Goods_Receipts { get; set; }
        public virtual ICollection<Shop_Goods_StockTake> Shop_Goods_StockTakes { get; set; }
    }
}
