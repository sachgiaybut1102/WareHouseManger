using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WareHouseManger.Models.EF
{
    public class StockCard
    {
        public string ID { get; set; }
        public DateTime DateCreated {  get; set; }
        public int Category { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
