using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Position
    {
        public Position()
        {
            Employees = new HashSet<Employee>();
        }

        public int PositionID { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
