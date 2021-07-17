using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Account_Status
    {
        public Account_Status()
        {
            Accounts = new HashSet<Account>();
        }

        public int StatusID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
