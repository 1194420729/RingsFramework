using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rings.Models
{
    public static class LimitHelper
    {
        public static bool IsAllowed(this Account account, string limitname)
        {
            //todo:check limit
            if (account.UserName.ToLower() == "admin")
            {
                return true;
            }
            return false;
        }
    }
}
