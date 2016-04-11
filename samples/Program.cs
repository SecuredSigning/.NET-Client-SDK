using SecuredSigningClientSdk;
using System;
using System.IO;
using System.Text;
using static Test.Sample;
using static Test.Helper;
using System.Windows.Forms;

namespace Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //WithoutSDK.Sample.Run();
            //return;

            //initialise API Client
            var client = new ServiceClient(
                serviceUrl: "https://api.securedsigning.com/web",
                version: "v1.4",
                apiKey: "[APIKey]",
                secret: "[APISecret]",
                accessUrl: "[AccessUrl]");

            //see https://www.securedsigning.com/developer/api-documentation#auth for more details about OAuth 2, default enabled for API Key from March 2016. earlier API Key should ignore it.           
            #region OAuth2
            //get OAuth2 access token
            Application.EnableVisualStyles();
            var form = new OAuth2AuthoriseForm(client, DateTime.Now.Ticks.ToString(), OAuth2Client.OAuth2Scope.Basic | OAuth2Client.OAuth2Scope.FormDirect | OAuth2Client.OAuth2Scope.FormFiller | OAuth2Client.OAuth2Scope.SmartTag);
            form.OnAuthorized = tokenResp =>
            {
                client.AccessToken = tokenResp.Access_Token;
                form.Close();
            };
            Application.Run(form);

            AccountSample(client);

            #endregion

            //FormDirectSample(client);
            //FormFillerSample(client);
            //SmartTagSample(client);
            Console.Read();
        }
        static void AccountSample(ServiceClient client)
        {
            var account=client.getAccountInfo();
            Console.WriteLine(string.Format("Hello {0}!", account.Name));
            var documents = client.getActiveDocuments("InBox");
        }
    }
}
