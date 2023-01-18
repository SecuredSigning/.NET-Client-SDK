using SecuredSigningClientSdk;
using System;
using static Test.Sample;
using System.Windows.Forms;

namespace Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //initialise API Client
            var client = new ServiceClient(
                serviceUrl: SampleParameters.SecuredSigningServiceBase,
                version: SampleParameters.SecuredSigningServiceVersion,
                apiKey: SampleParameters.APIKey,
                secret: SampleParameters.APISecret,
                accessUrl: SampleParameters.CallbackUrl
                );
            //see https://www.securedsigning.com/developer/api-documentation#auth for more details about OAuth 2, default enabled for API Key from March 2016. earlier API Key should ignore it.           
            #region OAuth2
            var state = SampleParameters.OAuth2State;

            //get OAuth2 access token in WinForm
            //Application.EnableVisualStyles();
            //var form = new OAuth2AuthoriseForm(client, state, OAuth2Client.OAuth2Scope.Basic | OAuth2Client.OAuth2Scope.FormDirect | OAuth2Client.OAuth2Scope.FormFiller | OAuth2Client.OAuth2Scope.SmartTag);
            //form.OnAuthorized = (_tokenResp, _state) =>
            //{
            //    System.Diagnostics.Debug.Assert(_state == state);
            //    client.AccessToken = _tokenResp.Access_Token;
            //    form.Close();
            //};
            //Application.Run(form);

            //AccountSample(client);


            //get OAuth2 access token in Server
            //var authorizeUrl = client.OAuth2.CreateAuthorizeRequest(state,
            //    OAuth2Client.OAuth2Scope.Basic.ToString(),
            //    OAuth2Client.OAuth2Scope.FormDirect.ToString(),
            //    OAuth2Client.OAuth2Scope.FormFiller.ToString(),
            //    OAuth2Client.OAuth2Scope.SmartTag.ToString());
            // //start oauthorize process in a webpage 
            // System.Diagnostics.Process.Start(authorizeUrl);
            //run an server implemented by HttpListener, you can implement your own server using ASP.Net, etc.
            //  Server.SampleServer server = new Server.SampleServer();
            //  var code = server.StartOnce();
            //var tokenResp = client.OAuth2.GetToken(code);
            //client.AccessToken = tokenResp.Access_Token;
            //tokenResp = client.OAuth2.RefreshToken(tokenResp.Refresh_Token);
            //client.AccessToken = tokenResp.Access_Token;

            //AccountSample(client);

            #endregion

            //FormDirectSample(client);
            //FormFillerSample(client);
            //FormFillerSample_EmbeddedSigning(client);
            //SmartTagSample(client);

            //DownloadSigningCompletionCertificateSample(client);
            //DownloadENotaryJounalSample(client);
            //DownloadSnapshotsSample(client);
            //DownloadIDVerificationample(client);
            DownloadVideoRecordingSample(client);

            Console.Read();
        }
        static void AccountSample(ServiceClient client)
        {
            var account=client.getAccountInfo();
            Console.WriteLine($"Hello {account.Name}!");
            var documents = client.getActiveDocuments("InBox");
        }
    }
}
