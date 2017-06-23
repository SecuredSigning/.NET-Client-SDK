using SecuredSigningClientSdk.Models;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/SmartTag/Send/", Verbs = "POST", Summary = "Send smart tag document", Notes = "Send a smart tag document.")]
    public class SmartTagRequest : IReturn<List<Document>>
    {
        [ApiMember(Description = "Mail merge document reference", AllowMultiple = true, DataType = SwaggerType.String, IsRequired = false)]
        public List<string> DocumentReferences { get; set; }

        [ApiMember(Description = "Shows if embedded signing", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool Embedded { get; set; }

        [ApiMember(Name = "DueDate", Description = "Due date that document are to be signed by.", DataType = SwaggerType.Date, IsRequired = true)]
        public string DueDate { get; set; }
        public string GMT { get; set; }

        [ApiMember(Description = "Email template reference", DataType = SwaggerType.String, IsRequired = false)]
        public string EmailTemplateReference { get; set; }

        [ApiMember(Description = "Return Url", DataType = SwaggerType.String, IsRequired = false)]
        public string ReturnUrl { get; set; }
        [ApiMember(Description = "Signer details, overwrite details populated in document", DataType = SwaggerType.Array, IsRequired = false)]
        public List<SmartTagInvitee> Signers { get; set; }

        [ApiMember(Description = "The list options for drop down list field smart tag; only work with client field", DataType = SwaggerType.Array, IsRequired = false)]
        public List<DropDownListItem> ListItems { get; set; }

        [ApiMember(Description = "Whether all documents are in a package (by default) or sent separately", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool NoPackage { get; set; }

        [ApiMember(Description = "Notify Url.", DataType = SwaggerType.String, IsRequired = false)]
        public string NotifyUrl { get; set; }
    }

    [Route("/SmartTag/MailMerge/", Verbs = "POST", Summary = "Merge mail merge list with the document", Notes = "Send a smart tag document.")]
    public class MailMergeRequest : IReturn<ProcessDocument>
    {
        [ApiMember(Description = "Mail merge document reference", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }

        [ApiMember(Description = "File type of file", DataType = SwaggerType.String, IsRequired = true)]
        [ApiAllowableValues("FileType", typeof(MailMergeListFileType))]
        public string MailMergeListFileType { get; set; }

        [ApiMember(Description = "Base64 encoded mail merge list file for the document", DataType = SwaggerType.String, IsRequired = true)]
        public string MailMergeListFileData { get; set; }

        [ApiMember(Name = "DueDate", Description = "Due date that document are to be signed by.", DataType = SwaggerType.Date, IsRequired = true)]
        public DateTime DueDate { get; set; }

        [ApiMember(Description = "Email template reference", DataType = SwaggerType.String, IsRequired = false)]
        public string EmailTemplateReference { get; set; }
        [ApiMember(Description = "Shows if embedded signing", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool Embedded { get; set; }
        [ApiMember(Description = "Url to return after document signed", DataType = SwaggerType.String, IsRequired = false)]
        public string ReturnUrl { get; set; }
    }

    [Route("/SmartTag/MailMergeDocuments/{ProcessDocumentReference}", Verbs = "GET", Summary = "Returns status of mail merge processing along with documents.", Notes = "Returns status of mail merge processing along with documents.")]
    public class ProcessDocumentRequest : IReturn<ProcessDocument>
    {
        [ApiMember(Description = "Mail merge process document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string ProcessDocumentReference { get; set; }
    }
}