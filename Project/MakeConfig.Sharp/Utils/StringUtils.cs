using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeConfig.Utils
{
    public static class StringUtils
    {

        public static string RemoveLast(this string self, string value)
        {
            if (!self.EndsWith(value))
            {
                return self;
            }

            return self.Substring(0, self.Length - value.Length);
        }
        
    }
}
