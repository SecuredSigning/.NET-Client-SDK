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
        [ApiMember(Description = "wheather return with documeng log", ParameterType = "query", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool DocumentLog { get; set; }
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
        [ApiMember(Name = "DueDate", Description = @"The ISO 8601 formats is the better way to pass the due date to API;
otherwise API will try to parse the date string according to user's settings.
If it can not be identified, an error will return", DataType = SwaggerType.Date, IsRequired = true)]
        public string DueDate { get; set; }
        [ApiMember(Description = @"The total minutes of the offset between your local time to UTC time.
If it's not specified, user's settings will be applied.
If the date is UTC time already, set it as 0.", DataType = SwaggerType.String, IsRequired = false)]
        public string GMT { get; set; }
    }
    [Route("/Package/Extend", Verbs = "POST", Summary = "Extend the package due date", Notes = "Extend the package due date")]

    public class PackageExtendRequest : IReturn<PackageResponse>
    {
        [ApiMember(Description = "Package reference", DataType = SwaggerType.String, IsRequired = true)]
        public string PackageReference { get; set; }
        [ApiMember(Name = "DueDate", Description = @"The ISO 8601 formats is the better way to pass the due date to API;
otherwise API will try to parse the date string according to user's settings.
If it can not be identified, an error will return", DataType = SwaggerType.Date, IsRequired = true)]
        public string DueDate { get; set; }
        [ApiMember(Name = "GMT", Description = @"The total minutes of the offset between your local time to UTC time.
If it's not specified, user's settings will be applied.
If the date is UTC time already, set it as 0.", DataType = SwaggerType.String, IsRequired = false)]
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

    [Route("/Package/Status/{PackageReference}", Verbs = "GET", Summary = "Return current package status", Notes = "Returns a object containing the current status of a package")]

    public class PackageStatusRequest : IReturn<PackageResponse>
    {
        [ApiMember(Description = "Package reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string PackageReference { get; set; }
        [ApiMember(Description = "wheather return with documeng log", ParameterType = "query", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool DocumentLog { get; set; }
    }
    [Route("/Package/Delete/{PackageReference}", Verbs = "POST", Summary = "Delete the package", Notes = "Delete the package")]
    [Route("/Package/{PackageReference}", Verbs = "DELETE", Summary = "Delete the package", Notes = "Delete the package")]

    public class PackageDeleteRequest : IReturn
    {
        [ApiMember(Description = "Package reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string PackageReference { get; set; }
    }

    [Route("/Document/Uploader", Verbs = "POST", Summary = "Uploads a file by mulitpart form", Notes = "Uploads a file using multipart form type. Allowed FileTypes: .pdf, .doc, .docx, .odt, .rtf, .xls, .xlsx, .ods.")]
    [Route("/Document/Uploader/{ClientReference}", Verbs = "POST", Summary = "Uploads a file with client reference by mulitpart form", Notes = "Uploads a file using multipart form type. Allowed FileTypes: .pdf, .doc, .docx, .odt, .rtf, .xls, .xlsx, .ods.")]
    public class UploaderRequest : IReturn<Document>
    {
        [ApiMember(Description = "Client reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = false)]
        public string ClientReference { get; set; }

        [ApiMember(Name = "body", DataType = "file", ParameterType = "body", IsRequired = true)]
        public object AnyThing { get; set; }
    }
    [Route("/Document/Uploader", Verbs = "POST", Summary = "Uploads a file by mulitpart form", Notes = "Uploads a file using multipart form type. Allowed FileTypes: .pdf, .doc, .docx, .odt, .rtf, .xls, .xlsx, .ods.")]
    public class Uploader2Request : IReturn<Document>
    {
        [ApiMember(Description = "Client reference", ParameterType = "body", DataType = SwaggerType.String, IsRequired = false)]
        public string ClientReference { get; set; }

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
        [ApiMember(Description = "wheather return with documeng log", ParameterType = "query", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool DocumentLog { get; set; }
    }

    [Route("/Document/SendReminder", Verbs = "POST", Summary = "Send invitation reminder to invitee", Notes = "Send invitation reminder to invitee")]
    public class SendReminderRequest : IReturn
    {
        public string DocumentReference { get; set; }
        public string SignerReference { get; set; }
    }
    [Route("/Document/FieldData/{DocumentReference}", Verbs = "GET", Summary = "Get field data in documents", Notes = "Get field data in documents")]
    public class FieldDataRequest : IReturn<List<FormFillerField>>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }

    [Route("/Document/Delete/{DocumentReference}", Verbs = "POST", Summary = "Delete the document", Notes = "Delete the document")]
    [Route("/Document/{DocumentReference}", Verbs = "DELETE", Summary = "Delete the document", Notes = "Delete the document")]
    public class DeleteRequest : IReturn<Document>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
        [ApiMember(Description = "If the document is part of package, delete this whole package or just remove this document.", ParameterType = "query", DataType = SwaggerType.Boolean, IsRequired = true)]
        public bool RemoveFromPackage { get; set; }

    }
    [Route("/Document/Combine", Verbs = "POST", Summary = "Returns a combined document", Notes = "Returns a document reference for the combined documents")]
    public class CombineRequest : IReturn<Document>
    {
        [ApiMember(Description = "Collection of document references to add to package", AllowMultiple = true, DataType = SwaggerType.String, IsRequired = true)]
        public string[] DocumentReferences { get; set; }

        [ApiMember(Description = "Document reference", DataType = SwaggerType.String, IsRequired = true)]
        public string CombinedDocumentName { get; set; }

        [ApiMember(Description = "Client reference", DataType = SwaggerType.String, IsRequired = false)]
        public string ClientReference { get; set; }
    }
}