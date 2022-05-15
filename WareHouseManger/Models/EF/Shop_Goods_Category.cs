using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_Category
    {
        public Shop_Goods_Category()
        {
            Shop_Goods_SubCategories = new HashSet<Shop_Goods_SubCategory>();
        }

        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string SortName { get; set; }

        public virtual ICollection<Shop_Goods_SubCategory> Shop_Goods_SubCategories { get; set; }
    }
}
