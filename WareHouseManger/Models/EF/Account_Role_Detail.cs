using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Account_Role_Detail
    {
        public int AccountID { get; set; }
        public int RoleID { get; set; }

        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }
    }
}
