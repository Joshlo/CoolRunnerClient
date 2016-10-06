using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CRClient.Exceptions
{
    public class CoolRunnerException : Exception
    {
        public CoolRunnerException(HttpStatusCode statusCode, string jsonResponse) : base(GetMessage(statusCode, jsonResponse))
        {
        }

        private static string GetMessage(HttpStatusCode statusCode, string jsonResponse)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"StatusCode was {statusCode}");
            builder.AppendLine();
            builder.AppendLine("Body:");
            builder.AppendLine(jsonResponse);
            return builder.ToString();
        }
    }
}
