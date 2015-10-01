using SecuredSigningClientSdk.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

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
        public DateTime DueDate { get; set; }

        [ApiMember(Description = "Email template reference", DataType = SwaggerType.String, IsRequired = false)]
        public string EmailTemplateReference { get; set; }

        [ApiMember(Description = "Workflow reference", DataType = SwaggerType.String, IsRequired = false)]
        public string WorkflowReference { get; set; }
        [ApiMember(Description = "Return Url", DataType = SwaggerType.String, IsRequired = false)]
        public string ReturnUrl { get; set; }
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