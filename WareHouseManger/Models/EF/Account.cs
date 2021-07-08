using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Account
    {
        public Account()
        {
            Account_Roles_Details = new HashSet<Account_Roles_Detail>();
        }

        public int AccountID { get; set; }
        public int? EmployeeID { get; set; }
        public string Password { get; set; }
        public int? StatusID { get; set; }
        public DateTime? DateCreated { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<Account_Roles_Detail> Account_Roles_Details { get; set; }
    }
}
