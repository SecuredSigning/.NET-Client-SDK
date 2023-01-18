using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Download/Snapshot/{DocumentReference}", Verbs = "GET", Summary = "Returns the specified captured images")]
    public class DownloadCapturedImagesRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }
}