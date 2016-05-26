using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecuredSigningClientSdk;
namespace SampleWebsite.Models
{
    public static class SDK
    {
        public static ServiceClient client = null;
        static SDK() {
            client = new ServiceClient(
                serviceUrl: "https://api.securedsigning.com/web",
                version: "v1.4",
                apiKey: "[Your API Key]",
                secret: "[Your API Secret]",
                accessUrl: "[Your Website URL]"
                );
            }
    }
}
