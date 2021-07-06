using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WareHouseManger.ViewModels
{
    public class Shop_GoodsViewModel
    {
        public string GoodsReceiptID { get; set; }
        public string TemplateID { get; set; }
        public int? Count { get; set; }
        public int? UnitPrice { get; set; }
    }
}
