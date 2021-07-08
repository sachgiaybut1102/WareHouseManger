using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Customer_Category
    {
        public Customer_Category()
        {
            Customers = new HashSet<Customer>();
        }

        public int CustomerCategoryID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
