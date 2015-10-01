using System;
using System.Collections.Generic;
using System.Net;
using SecuredSigningClientSdk.Models;
using ServiceStack;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/FormDirect/GetFormList", Verbs = "GET", Summary = "Gets form list", Notes = "Gets a list of Forms available for an account. Returns a collection of signers required for signing the forms.")]
    public class FormDirectRequest : IReturn<List<FormDirect>>
    {
    }

    [Route("/FormDirect/GetSingleForm/{FormReference}", Verbs = "GET", Summary = "Gets a single form", Notes = "Gets a single form. Returns a collection of signers required for signing the forms.")]
    public class SingleFormDirectRequest : IReturn<FormDirect>
    {
        [ApiMember(Description = "Form reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string FormReference { get; set; }
    }

    [Route("/FormDirect/SendForms", Verbs = "POST", Summary = "Starts Form Direct Process", Notes = "Send a package of forms for filling and signing")]
    public class SendFormDirectRequest : IReturn<List<Document>>
    {
        [ApiMember(Name = "Forms", Description = "Collection of forms to be sent, if an account reference is not supplied for the forms, the forms will be associated with your api account.",
        DataType = "FormDirect", AllowMultiple = true, IsRequired = true)]
        public List<FormDirect> Forms { get; set; }

        [ApiMember(Name = "DueDate", Description = "Due date that forms are to be signed by. If not set, +14 days is the default", DataType = SwaggerType.Date, IsRequired = false)]
        public DateTime DueDate { get; set; }
    }

    [Route("/FormDirect/GetFormData/{DocumentReference}/{FormDataFileType}", Verbs = "GET", Summary = "Gets form data", Notes = "Returns the download URL for that document's formdata. Export file format can be XML, CSV, XLSX, XLS.")]
    public class FormDataRequest : IReturn<FormDataResponse>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }

        [ApiMember(Description = "Form data file return type", DataType = SwaggerType.String, ParameterType = "path", IsRequired = true)]
        [ApiAllowableValues("FormDataFileType", typeof(FormDataFileType))]
        public string FormDataFileType { get; set; }

        [ApiMember(Description = "Seperator of CSV file (Default - \\t), can pass any visible character, e.g. A,#,@,~..., for invisible character, please use '0x' as the prefix for hexidecimal, e.g. 0x09, 0x0a, 0x0d", DataType = "char", ParameterType = "query", IsRequired = false)]
        public string Separator { get; set; }
    }

    public class FormDataResponse
    {
        [ApiMember(Description = "Url which file content will be downloaded", DataType = SwaggerType.String)]
        public string Url { get; set; }
    }

    [Route("/FormDirect/GetSignerLink", Verbs = "POST", Summary = "Gets a signers link", Notes = "Returns a signer with the link required to access their form. Requires both a document reference and the signer (First name, Last name and email)")]
    public class LinkRequest : IReturn<Signer>
    {
        [ApiMember(Description = "Document reference", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }

        [ApiMember(Description = "Form signer", IsRequired = true, DataType = "Signer")]
        public Signer Signer { get; set; }
    }
}