using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WareHouseManger.Common
{
    public class ClaimsPrincipalCommon
    {
        private ClaimsPrincipal user;

        public ClaimsPrincipalCommon(ClaimsPrincipal user)
        {
            this.user = user;
        }


    }
}
