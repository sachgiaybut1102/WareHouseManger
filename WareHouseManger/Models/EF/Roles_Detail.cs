using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Roles_Detail
    {
        public Roles_Detail()
        {
            Account_Roles_Details = new HashSet<Account_Roles_Detail>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int? RoleID { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Account_Roles_Detail> Account_Roles_Details { get; set; }
    }
}
