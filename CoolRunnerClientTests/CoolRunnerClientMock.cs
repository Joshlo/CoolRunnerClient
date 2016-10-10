using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRClient;
using CRClient.HttpClient;

namespace CoolRunnerClientTests
{
    public class CoolRunnerClientMock : CoolRunnerClient
    {
        public CoolRunnerClientMock(IHttpCoolRunnerClient mockClient) : base(mockClient)
        {
            
        }
    }
}
