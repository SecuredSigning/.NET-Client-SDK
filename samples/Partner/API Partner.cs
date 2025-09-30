using System;
using System.Collections.Generic;
using System.Linq;
using PartnerSDK = SecuredSigningClientSdk.Partner;

namespace Test
{
    static partial class Sample
    {
        public static partial class Partner
        {
            public static void APIPartnerSample(PartnerSDK.ServiceClient client)
            {
                Console.WriteLine("Get token for invitee");
                var inviteeToken = client.OAuth2.GetAccessTokenForInvitee(SampleParameters.Invitee1_FirstName, SampleParameters.Invitee1_LastName, SampleParameters.Invitee1_Email, SecuredSigningClientSdk.OAuth2Client.OAuth2Scope.Basic | SecuredSigningClientSdk.OAuth2Client.OAuth2Scope.SmartTag);
                client.AccessToken = inviteeToken.Access_Token;
                Console.WriteLine("Upload smart tag document");
                var docRef = client.uploadDocumentFile(new System.IO.FileInfo(SampleParameters.Path2SmartTagDocument));
                Console.WriteLine("Send smart tag");
                var stResp = client.sendSmartTagDocument(new List<string> { docRef }, DateTime.Now.AddDays(3), new SecuredSigningClientSdk.Models.SmartTagOptions { Embedded = true });
                Console.WriteLine("Populate signing key to embedded html page");
                var signingKey = stResp.FirstOrDefault()?.Signers?.FirstOrDefault()?.SigningKey;
                Console.WriteLine(signingKey);
                var server = new Server.SampleServer()
                {
                    SDKClient = client
                };

                System.Diagnostics.Process.Start($"{SampleParameters.EmbeddedSigningUrl}?key={signingKey}");
                var runResult = server.StartOnce();
                Console.WriteLine("Go to browser to sign the document");
                bool signed = false;
                while (!signed)
                {
                    System.Threading.Thread.Sleep(5000);
                    Console.WriteLine("Check document status");
                    var status = client.getDocumentStatus(docRef);
                    if (status.Status == "Complete")
                    {
                        signed = true;
                    }
                }
                Console.WriteLine("Done");
                Console.Read();
            }

        }
    }
}
