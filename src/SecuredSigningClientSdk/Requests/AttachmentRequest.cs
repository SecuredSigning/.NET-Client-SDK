using ServiceStack;
using SecuredSigningClientSdk.Models;
using System.Collections.Generic;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Attachment/UploadByUrl", Verbs = "POST", Summary = "Uploads a file as attachment by url", Notes = "Uploads a file using a url")]
    public class UploadAttachmentRequest : IReturn<AttachmentResponse>
    {
        [ApiMember(Name = "File", IsRequired = true, DataType = "FileInfo", Description = "The file infomation")]
        public AttachmentFileInfo File { get; set; }
    }

    [Route("/Attachment/Uploader", Verbs = "POST", Summary = "Uploads a file as attachment by mulitpart form", Notes = "Uploads a file using multipart form type. Allowed FileTypes: .pdf, .doc, .docx, .odt, .rtf, .xls, .xlsx, .ods.")]
    public class AttachmentUploaderRequest : IReturn<AttachmentResponse>
    {
        [ApiMember(Name = "body", DataType = "file", ParameterType = "body", IsRequired = true)]
        public object AnyThing { get; set; }
        [ApiMember(Description = "Attachment Number, can only be digitals", ParameterType = "query", DataType = SwaggerType.String, IsRequired = false)]
        public string Number { get; set; }
        [ApiMember(Description = "Attachment Category", ParameterType = "query", DataType = SwaggerType.String, IsRequired = false)]
        public string Category { get; set; }
    }

    [Route("/Attachment/GetAttachments", Verbs = "GET", Summary = "Returns account user's available attachments", Notes = "Returns account user's available attachments.")]
    public class GetAttachmentsRequest : IReturn<List<AttachmentResponse>>
    {
    }
    [Route("/Attachment/Delete/{AttachmentReference}", Verbs = "POST", Summary = "Delete the attachment", Notes = "Delete the attachment")]
    [Route("/Attachment/{AttachmentReference}", Verbs = "DELETE", Summary = "Delete the attachment", Notes = "Delete the attachment")]
    public class DeleteAttachmentRequest : IReturn
    {
        [ApiMember(Description = "Attachment reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string AttachmentReference { get; set; }
    }
}