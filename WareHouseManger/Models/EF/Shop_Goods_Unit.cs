using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_Unit
    {
        public Shop_Goods_Unit()
        {
            Shop_Goods = new HashSet<Shop_Good>();
        }

        public int UnitID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Shop_Good> Shop_Goods { get; set; }
    }
}
