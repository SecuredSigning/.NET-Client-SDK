using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test.WithoutSDK
{
    class Sample
    {
        public static void Run()
        {
            var client = new WithoutSDK.SecuredSigningRestAPI(
                serviceUrl: SampleParameters.SecuredSigningServiceBase,
                version: SampleParameters.SecuredSigningServiceVersion,
                apiKey: SampleParameters.APIKey,
                secret: SampleParameters.APISecret,
                accessUrl: SampleParameters.CallbackUrl);
            var authorizeUrl = client.OAuth2.CreateAuthorizeRequest(SampleParameters.OAuth2State,
                 OAuth2Client.OAuth2Scope.Basic.ToString(),
                OAuth2Client.OAuth2Scope.FormDirect.ToString(),
                OAuth2Client.OAuth2Scope.FormFiller.ToString(),
                OAuth2Client.OAuth2Scope.SmartTag.ToString());
            //start oauthorize process in a webpage 
            System.Diagnostics.Process.Start(authorizeUrl);
            //for real using, need a server to handle the response 
            Console.WriteLine("finish OAuth2 authorize process, then input authorization code:");
            string code = Helper.ReadLine();
            //get access token 
            var accessToken = client.OAuth2.GetToken(code);
            client.AccessToken = accessToken.Access_Token;
            APISample(client);
            Console.Read();
        }
        static void APISample(SecuredSigningRestAPI client)
        {
            var account = client.getAccountInfo();
            Console.WriteLine(string.Format("Hello {0}!", account.Name));
            var documents = client.getActiveDocuments("InBox");
            Console.WriteLine(string.Format("Upload document"));
            var document=client.uploadDocumentFile(@"[Path to file]");
            Console.WriteLine(string.Format("Done: {0}", document.Reference));
        }

    }
}
