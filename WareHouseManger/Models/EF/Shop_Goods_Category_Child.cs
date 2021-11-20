using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_Category_Child
    {
        public Shop_Goods_Category_Child()
        {
            Shop_Goods = new HashSet<Shop_Good>();
        }

        public int CategoryChildID { get; set; }
        public int CategoryParentID { get; set; }
        public string Name { get; set; }
        public string SortName { get; set; }

        public virtual Shop_Goods_Category_Parent CategoryParent { get; set; }
        public virtual ICollection<Shop_Good> Shop_Goods { get; set; }
    }
}
