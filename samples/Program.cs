using SecuredSigningClientSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceClient(
                serviceUrl: "https://api.securedsigning.com/web",
                version: "v1.3",
                apiKey: "YOUR API KEY",
                secret: "YOUR API Secret",
                accessUrl: "YOUR Access URL");

            var forms = client.getFormList();
        }
    }
}
