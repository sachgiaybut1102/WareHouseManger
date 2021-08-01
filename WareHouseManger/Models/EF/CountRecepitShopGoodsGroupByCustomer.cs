﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WareHouseManger.Models.EF
{
    public class CountRecepitShopGoodsGroupByCustomer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int Count { get; set; }
        public decimal Turnover { get; set; }
    }
}
