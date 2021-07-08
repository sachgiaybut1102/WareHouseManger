using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Role
    {
        public Role()
        {
            Roles_Details = new HashSet<Roles_Detail>();
        }

        public int RoleID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Roles_Detail> Roles_Details { get; set; }
    }
}
