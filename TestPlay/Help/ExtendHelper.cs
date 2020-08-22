using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestPlay.Help
{
    public static class ExtendHelper
    {
        public static string ToStr(this string obj)
        {
            obj = obj ?? string.Empty;
            return obj.ToString();
        }
    }
}