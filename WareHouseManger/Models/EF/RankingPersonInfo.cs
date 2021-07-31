using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WareHouseManger.Models.EF
{
    public class RankingPersonInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int TotalBill { get; set; }
    }
}
