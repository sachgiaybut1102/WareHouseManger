using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Account
    {
        public Account()
        {
            Account_Role_Details = new HashSet<Account_Role_Detail>();
        }

        public int AccountID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? StatusID { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? EmployeeID { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Account_Status Status { get; set; }
        public virtual ICollection<Account_Role_Detail> Account_Role_Details { get; set; }
    }
}
