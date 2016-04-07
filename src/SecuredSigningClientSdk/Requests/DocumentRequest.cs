using SecuredSigningClientSdk.Models;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace SecuredSigningClientSdk.Requests
{

    [Route("/Document/Status/{DocumentReference}", Verbs = "GET", Summary = "Return current document status", Notes = "Returns a string containing the current status of a document")]
    public class StatusRequest : IReturn<Document>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }

    [Route("/Document/Log/{DocumentReference}", Verbs = "GET", Summary = "Return a document log", Notes = "Returns a collection of document logs")]
    public class LogRequest : IReturn<List<DocumentLog>>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }

    [Route("/Document/Extend", Verbs = "POST", Summary = "Extend the document due date", Notes = "Extend the document due date")]
    public class ExtendRequest : IReturn<Document>
    {
        [ApiMember(Description = "Document reference", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
        [ApiMember(Name = "DueDate", Description = "Due date that document is to be signed by.", DataType = SwaggerType.Date, IsRequired = true)]
        public DateTime DueDate { get; set; }
        [ApiMember(Description = "GMT Offset", DataType = SwaggerType.String, IsRequired = false)]
        public string GMT { get; set; }
    }

    [Route("/Document/UpdateSigner", Verbs = "POST", Summary = "Update signer profile", Notes = "Update signer profile")]
    public class SignerRequest : IReturn<Document>
    {
        [ApiMember(Description = "Document reference", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
        [ApiMember(Description = "New signer information", AllowMultiple = true, DataType = "Signer", IsRequired = true)]
        public Signer[] Signers { get; set; }
    }

    [Route("/Document/UploadByUrl", Verbs = "POST", Summary = "Uploads a file by url", Notes = "Uploads a file using a url")]
    public class UploadRequest : IReturn<Document>
    {
        [ApiMember(Name = "File", IsRequired = true, DataType = "FileInfo", Description = "The file infomation")]
        public FileInfo File { get; set; }
    }

    [Route("/Document/Uploader", Verbs = "POST", Summary = "Uploads a file by mulitpart form", Notes = "Uploads a file using multipart form type. Allowed FileTypes: .pdf, .doc, .docx, .odt, .rtf, .xls, .xlsx, .ods.")]
    public class UploaderRequest : IReturn<Document>
    {
        [ApiMember(Name = "body", DataType = "file", ParameterType = "body", IsRequired = true)]
        public object AnyThing { get; set; }
    }

    [Route("/Document/GetDocumentUrl/{DocumentReference}", Verbs = "GET", Summary = "Returns a url for downloading a document", Notes = "Returns the download URL for that document. The document may not be found due to it being removed from Secured Signing according to our data retention policy.")]
    public class DocumentRequest : IReturn<DocumentResponse>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }

    [Route("/Document/Validation/{DocumentReference}", Verbs = "GET", Summary = "validate document", Notes = "Verifies the signatures in document. The document may not be found due to it being removed from Secured Signing according to our data retention policy.")]
    public class DocumentValidationRequest : IReturn<DocumentValidationResponse>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }

    [Route("/Document/FileValidation", Verbs = "POST", Summary = "validate document", Notes = "Verifies the signatures in document. The document may not be found due to it being removed from Secured Signing according to our data retention policy.")]
    public class DocumentFileValidationRequest : IReturn<DocumentValidationResponse>
    {
        [ApiMember(Name = "body", DataType = "file", ParameterType = "body", IsRequired = true)]
        public object AnyThing { get; set; }
    }

    [Route("/Document/GetActiveDocuments/{Folder}", Verbs = "GET", Summary = "Returns account user's active documents", Notes = "Returns an array of all In progress and signed documents that haven't been removed.")]
    public class GetActiveDocumentsRequest : IReturn<List<Document>>
    {
        [ApiMember(DataType = "string", ParameterType = "path", IsRequired = true, Name = "Folder")]
        [ApiAllowableValues("Folder", typeof(Folder))]
        public string Folder { get; set; }
    }

    [Route("/Document/SendReminder", Verbs = "POST", Summary = "Send invitation reminder to invitee", Notes = "Send invitation reminder to invitee")]
    public class SendReminderRequest : IReturn
    {
        public string DocumentReference { get; set; }
        public string SignerReference { get; set; }
    }

}