using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class RoleGroup
    {
        public RoleGroup()
        {
            Roles = new HashSet<Role>();
        }

        public int RoleGroupID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
