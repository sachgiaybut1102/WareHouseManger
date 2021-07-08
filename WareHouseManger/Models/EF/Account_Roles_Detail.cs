using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseManger.Models.EF
{
    public partial class Account_Roles_Detail
    {
        public int AccountID { get; set; }
        public int ID { get; set; }

        public virtual Account Account { get; set; }
        public virtual Roles_Detail IDNavigation { get; set; }
    }
}
