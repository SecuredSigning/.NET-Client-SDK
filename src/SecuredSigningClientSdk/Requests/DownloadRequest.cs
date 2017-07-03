using SecuredSigningClientSdk.Models;
using ServiceStack;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Download/FormData/{DocumentReference}/{FormDataFileType}", Verbs = "GET", Summary = "Get FormData for that specific Document", Notes = "choose different export options (csv, xls, xlsx, xml), if it has Xslt set for that Form, it will apply automatically.")]
    public class DownloadFormDataRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }

        [ApiMember(Description = "Form data file return type", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        [ApiAllowableValues("FormDataFileType", typeof(FormDataFileType))]
        public string FormDataFileType { get; set; }
    }

    [Route("/Download/GetDocumentData/{DocumentReference}", Verbs = "GET", Summary = "Returns document data", Notes = "Returns the document data as a stream. The document may not be found due to it being removed from Secured Signing according to our data retention policy.")]
    public class DownloadDocumentRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }
    [Route("/Download/GetUploadedFileData/{DocumentReference}", Verbs = "GET", Summary = "Returns uploaded file data, always in zip", Notes = "Returns the document data as a stream. The document may not be found due to it being removed from Secured Signing according to our data retention policy.")]
    public class DownloadUploadedFileRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }
    [Route("/Download/Attachment/{AttachmentReference}", Verbs = "GET", Summary = "Returns attachment file data", Notes = "Returns attachment file data")]
    public class DownloadAttachmentRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string AttachmentReference { get; set; }
    }
}
