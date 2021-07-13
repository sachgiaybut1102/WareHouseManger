using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManger.Common
{
    public static class MD5
    {
        public static string CreateHash(string unHashed)
        {
            var x = new System.Security.Cryptography.HMACMD5();
            var data = Encoding.ASCII.GetBytes(unHashed);
            data = x.ComputeHash(data);
            return Encoding.ASCII.GetString(data);
        }
    }
}
