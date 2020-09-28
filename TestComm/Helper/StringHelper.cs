using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestComm.Helper
{
    public static class StringHelper
    {
        public static string ToStr(this object obj)
        {
            try
            {
                return obj == null ? string.Empty : obj.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
