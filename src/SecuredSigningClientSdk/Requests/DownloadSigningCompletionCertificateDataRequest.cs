using SecuredSigningClientSdk.Models;
using ServiceStack;
using System.Collections.Generic;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Document/SigningCompletionCertificate/{DocumentReference}", Verbs = "GET", Summary = "Returns array of all certificates for this document", Notes = "Returns metadata only.")]
    public class DownloadSigningCompletionCertificateDataRequest : IReturn<List<SigningCompletionCertificateResponse>>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }

    }
}