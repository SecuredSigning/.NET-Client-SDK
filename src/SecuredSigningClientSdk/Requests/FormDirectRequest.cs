using System;
using System.Collections.Generic;
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
    public class SendFormDirectRequest : SendFormDirectRequestBase, IReturn<List<Document>>
    {
    }
    [Route("/FormDirect/SendForms2", Verbs = "POST", Summary = "Starts Form Direct Process", Notes = "Send a package of forms for filling and signing")]
    public class SendFormDirectRequest2 : SendFormDirectRequestBase, IReturn<PackageResponse>
    {
    }
    public class SendFormDirectRequestBase
    {
        [ApiMember(Name = "Forms", Description = "Collection of forms to be sent, if an account reference is not supplied for the forms, the forms will be associated with your api account.",
        DataType = "FormDirect", AllowMultiple = true, IsRequired = true)]
        public List<FormDirect> Forms { get; set; }

        [ApiMember(Name = "DueDate", Description = "Due date that forms are to be signed by. If not set, +14 days is the default", DataType = SwaggerType.Date, IsRequired = false)]
        public string DueDate { get; set; }
        [ApiMember(Description = "GMT Offset", DataType = SwaggerType.String, IsRequired = false)]
        public string GMT { get; set; }
        [ApiMember(Description = "Invitation Email template reference", DataType = SwaggerType.String, IsRequired = false)]
        public string InvitationEmailTemplateReference { get; set; }
        public List<DropDownListItem> ListItems { get; set; }
    }

    [Route("/FormDirect/GetSignerLink", Verbs = "POST", Summary = "Gets a signers link", Notes = "Returns a signer with the link required to access their form. Requires both a document reference and the signer (First name, Last name and email)")]
    public class LinkRequest : IReturn<Signer>
    {
        [ApiMember(Description = "Document reference", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }

        [ApiMember(Description = "Form signer", IsRequired = true, DataType = "Signer")]
        public Signer Signer { get; set; }
    }

    [Route("/FormDirect/Export/{DocumentReference}/{FormDataFileType}", Verbs = "GET", Summary = "Get FormData for that specific Document", Notes = "choose different export options (csv, xls, xlsx, xml), if it has Xslt set for that Form, it will apply automatically.")]
    public class ExportRequest : IReturn<byte[]>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }

        [ApiMember(Description = "Form data file return type", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        [ApiAllowableValues("FormDataFileType", typeof(FormDataFileType))]
        public string FormDataFileType { get; set; }
    }

    [Route("/FormDirect/Employers", Verbs = "GET", Summary = "List employer details for some forms", Notes = "List employer details for some forms. return form type and employer details.")]
    public class EmployerListRequest : IReturn<Employers>
    {
    }
    [Route("/FormDirect/UpdateEmployers", Verbs = "POST", Summary = "Save employer details for some forms", Notes = "Save employer details for some forms")]
    public class UpdateEmployerRequest : IReturn<Employers>
    {
        public List<SuperFundInfo> SuperFund { get; set; }
        public List<TFNInfo> TFN { get; set; }
        public List<AccClaimsHistoryInfo> AccClaimsHistory { get; set; }
    }
    [Route("/FormDirect/GetPredefinedFormFields/{FormReference}", Verbs = "GET", Summary = "Gets predefined fields for a single form", Notes = "Gets a single form. Returns a collection of signers required for signing the forms.")]
    public class FormFieldsRequest : IReturn<FormFieldResponse>
    {
        [ApiMember(Description = "Form reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string FormReference { get; set; }
    }
}