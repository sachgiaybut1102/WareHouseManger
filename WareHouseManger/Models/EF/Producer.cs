using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Producer
    {
        public Producer()
        {
            Shop_Goods = new HashSet<Shop_Good>();
        }

        public int ProducerID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }
        public string SortName { get; set; }

        public virtual ICollection<Shop_Good> Shop_Goods { get; set; }
    }
}
