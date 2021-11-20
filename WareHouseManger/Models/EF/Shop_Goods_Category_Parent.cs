using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_Category_Parent
    {
        public Shop_Goods_Category_Parent()
        {
            Shop_Goods_Category_Children = new HashSet<Shop_Goods_Category_Child>();
        }

        public int CategoryParentID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Shop_Goods_Category_Child> Shop_Goods_Category_Children { get; set; }
    }
}
