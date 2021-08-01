using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class ShopGoods_Image
    {
        public ShopGoods_Image()
        {
            Shop_Goods = new HashSet<Shop_Good>();
        }

        public int ImageID { get; set; }
        public string TemplateID { get; set; }
        public string Name { get; set; }
        public DateTime? DateUploaded { get; set; }

        public virtual Shop_Good Template { get; set; }
        public virtual ICollection<Shop_Good> Shop_Goods { get; set; }
    }
}
