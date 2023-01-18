using SecuredSigningClientSdk;
using SecuredSigningClientSdk.Models;
using SecuredSigningClientSdk.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    static partial class Sample
    {
        /// <summary>
        /// Basic Usage. For more details about API, see:
        /// <see cref="https://www.securedsigning.com/developer/api-documentation" />
        /// </summary>
        /// <param name="client"></param>
        public static void DownloadSigningCompletionCertificateSample(ServiceClient client)
        {
            var documents = client.getActiveDocuments("Signed");
            if (documents?.Any() ?? false)
            {
                var certificates = client.getSigningCompletionCertificate(documents[0].Reference);
                if (certificates?.Any() ?? false)
                {
                   var response = client.downloadSigningCompletionCertificate(certificates.ToList()[0].CertificateReference);
                }
            }
            
        }

        public static void DownloadENotaryJounalSample(ServiceClient client)
        {
            var documents = client.getActiveDocuments("Signed");
            if (documents?.Any() ?? false)
            {
                var document = documents.FirstOrDefault(d => d.Name.Equals("Notary"));
                if (document != null)
                {
                    var downloadResp = client.downloadENotaryJounal(new DownloadENotaryJournalRequest() { DocumentReference = document.Reference, ENotaryJournalDataType = ENotaryJournalDataType.pdf.ToString() });
                }
            }
        }

        public static void DownloadSnapshotsSample(ServiceClient client)
        {
            var documents = client.getActiveDocuments("Signed");
            if (documents?.Any() ?? false)
            {
                var document = documents.FirstOrDefault(d => d.Name.Equals("Notary2"));
                if (document != null)
                {
                    var downloadResp = client.getSnapshots(document.Reference);
                }
            }
        }

        public static void DownloadIDVerificationample(ServiceClient client)
        {
            var documents = client.getActiveDocuments("Signed");
            if (documents?.Any() ?? false)
            {
                var document = documents.FirstOrDefault(d => d.Name.Equals("Notary2"));
                if (document != null)
                {
                    var downloadResp = client.downloadIDVerification(new DownloadIDVerificationRequest { DocumentReference = document.Reference, IDVerificationDataType = IDVerificationDataType.pdf.ToString() });
                }
            }
        }

        public static void DownloadVideoRecordingSample(ServiceClient client)
        {
            var documents = client.getActiveDocuments("Signed");
            if (documents?.Any() ?? false)
            {
                var document = documents.FirstOrDefault(d => d.Name.Equals("Notary2"));
                if (document != null)
                {
                    var downloadResp = client.downloadVideoSigningRecording(document.Reference);
                }
            }
        }
    }
}
