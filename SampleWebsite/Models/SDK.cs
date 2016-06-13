using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecuredSigningClientSdk;
using Sample.Shared;

namespace SampleWebsite.Models
{
    public static class SDK
    {
        public static ServiceClient client = null;
        static SDK() {
            client = new ServiceClient(
                serviceUrl: SampleParameters.SecuredSigningServiceBase,
                version: SampleParameters.SecuredSigningServiceVersion,
                apiKey: SampleParameters.APIKey,
                secret: SampleParameters.APISecret,
                accessUrl: SampleParameters.CallbackUrl
                );
            }
    }
}
