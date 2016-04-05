using SecuredSigningClientSdk;
using System;
using System.IO;
using System.Text;
using static Test.Sample;
using static Test.Helper;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //initialise API Client
            var client = new ServiceClient(
                serviceUrl: "https://api.securedsigning.com/web",
                version: "v1.4",
                apiKey: "YOUR API KEY",
                secret: "YOUR API Secret",
                accessUrl: "YOUR Access URL");

            //see https://www.securedsigning.com/developer/api-documentation#auth for more details about OAuth 2, default enabled for API Key from March 2016. earlier API Key should ignore it.           
#region OAuth2
            //get OAuth2 access token
            var authorizeUrl = client.OAuth2.CreateAuthorizeRequest("some value",
                OAuth2Client.OAuth2Scope.Basic.ToString(),
                OAuth2Client.OAuth2Scope.FormDirect.ToString(),
                OAuth2Client.OAuth2Scope.FormFiller.ToString(),
                OAuth2Client.OAuth2Scope.SmartTag.ToString());
            //start oauthorize process in a webpage
            System.Diagnostics.Process.Start(authorizeUrl);
            //for real using, need a server to handle the response
            Console.WriteLine("finish OAuth2 authorize process, then input authorization code:");
            string code = ReadLine();
            //get access token
            var accessToken = client.OAuth2.GetToken(code);
            client.AccessToken = accessToken.Access_Token;
            //AccountSample(client);
            #endregion

            FormDirectSample(client);
            FormFillerSample(client);
            SmartTagSample(client);
        }
        static void AccountSample(ServiceClient client)
        {
            var account=client.getAccountInfo();
            Console.WriteLine(string.Format("Hello {0}!", account.Name));
            var documents = client.getActiveDocuments("InBox");
        }
    }
}
