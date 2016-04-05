using SecuredSigningClientSdk.Models;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/FormFiller/GetTemplateList", Verbs = "GET", Summary = "Gets form filler template list", Notes = "Gets a list of templates available for an account. Returns a collection of templates required for sending.")]
    public class FormFillerRequest : IReturn<List<FormFillerTemplate>>
    {

    }
    [Route("/FormFiller/GetSignerTemplate/{TemplateReference}", Verbs = "GET", Summary = "Gets all signer details in a single template ", Notes = "Gets a single template. Returns a collection of signers required for signing the template.")]
    public class FormFillerSignerRequest : IReturn<FormFillerTemplate>
    {
        [ApiMember(Description = "Template reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string TemplateReference { get; set; }
    }

    [Route("/FormFiller/GetFieldTemplate/{TemplateReference}", Verbs = "GET", Summary = "Gets all field details in a single template", Notes = "Gets a single template. Returns a collection of fields required for filling the template.")]
    public class FormFillerFieldRequest : IReturn<FormFillerTemplate>
    {
        [ApiMember(Description = "Template reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string TemplateReference { get; set; }
    }

    [Route("/FormFiller/Send", Verbs = "POST", Summary = "Send the template to start a We Sign process", Notes = "Send a template for filling and signing")]
    public class SendFormFillerRequest : IReturn<List<Document>>
    {
        [ApiMember(Name = "Forms", Description = "Collection of forms to be sent, if an account reference is not supplied for the forms, the forms will be associated with your api account.",
        DataType = "FormDirect", AllowMultiple = true, IsRequired = true)]
        public List<FormFillerTemplate> Templates { get; set; }

        [ApiMember(Name = "DueDate", Description = "Due date that forms are to be signed by. If not set, +14 days is the default", DataType = SwaggerType.Date, IsRequired = false)]
        public DateTime DueDate { get; set; }
        [ApiMember(Description = "GMT Offset", DataType = SwaggerType.String, IsRequired = false)]
        public string GMT { get; set; }
    }
}
