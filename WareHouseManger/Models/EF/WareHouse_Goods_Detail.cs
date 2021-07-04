using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class WareHouse_Goods_Detail
    {
        public int WareHouseID { get; set; }
        public string TemplateID { get; set; }
        public int? Count { get; set; }
    }
}
