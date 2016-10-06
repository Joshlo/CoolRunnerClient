using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace CRClient
{
    internal class UnderscoreMappingResolver : DefaultContractResolver
    {
        private static readonly Regex R = new Regex(@"([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])");

        protected override string ResolvePropertyName(string propertyName)
        {
            return GetUnderscoredPropertyName(propertyName);
        }

        public static string GetUnderscoredPropertyName(string propertyName)
        {
            var name = R.Replace(propertyName, "$1$3_$2$4").ToLower();
            return name;
        }
    }
}
