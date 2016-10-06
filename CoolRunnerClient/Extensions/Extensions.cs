using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CRClient.Extensions
{
    public static class Extensions
    {
        private static readonly Regex ToUnderscoreRegex = new Regex(@"([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])");

        public static Dictionary<string, string> ToCoolRunnerNamingFormat(this object o)
        {
            var properties = o.GetType().GetProperties();

            return properties.ToDictionary(property => ToUnderscoreRegex.Replace(property.Name, "$1$3_$2$4").ToLower(), 
                                            property => property.GetValue(o) != null ? property.GetValue(o).ToString() : "");
        }
    }
}
