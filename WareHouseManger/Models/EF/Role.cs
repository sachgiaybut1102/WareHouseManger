using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Role
    {
        public Role()
        {
            Account_Role_Details = new HashSet<Account_Role_Detail>();
        }

        public int RoleID { get; set; }
        public string Name { get; set; }
        public int? RoleGroupID { get; set; }

        public virtual RoleGroup RoleGroup { get; set; }
        public virtual ICollection<Account_Role_Detail> Account_Role_Details { get; set; }
    }
}
