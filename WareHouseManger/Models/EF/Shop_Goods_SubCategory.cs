using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Shop_Goods_SubCategory
    {
        public Shop_Goods_SubCategory()
        {
            Shop_Goods = new HashSet<Shop_Good>();
        }

        public int SubCategoryID { get; set; }
        public int CategoryParentID { get; set; }
        public string SubCategoriName { get; set; }
        public string SubCategoriDescription { get; set; }
        public string SortName { get; set; }

        public virtual Shop_Goods_Category CategoryParent { get; set; }
        public virtual ICollection<Shop_Good> Shop_Goods { get; set; }
    }
}
