using ServiceStack;
using SecuredSigningClientSdk.Models;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Attachment/UploadByUrl", Verbs = "POST", Summary = "Uploads a file as attachment by url", Notes = "Uploads a file using a url")]
    public class UploadAttachmentRequest : IReturn<Document>
    {
        [ApiMember(Name = "File", IsRequired = true, DataType = "FileInfo", Description = "The file infomation")]
        public FileInfo File { get; set; }
    }

    [Route("/Attachment/Uploader", Verbs = "POST", Summary = "Uploads a file as attachment by mulitpart form", Notes = "Uploads a file using multipart form type. Allowed FileTypes: .pdf, .doc, .docx, .odt, .rtf, .xls, .xlsx, .ods.")]
    public class AttachmentUploaderRequest : IReturn<Document>
    {
        [ApiMember(Name = "body", DataType = "file", ParameterType = "body", IsRequired = true)]
        public object AnyThing { get; set; }
    }
}