using SecuredSigningClientSdk.Models;
using ServiceStack;
using System.Collections.Generic;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/EmailTemplate/GetInvitationTemplates", Verbs = "GET", Summary = "Returns account user's active email templates.", Notes = "Returns account user's active email templates.")]
    public class EmailTemplateRequest : IReturn<List<EmailTemplate>>
    {
    }

    [Route("/EmailTemplate/GetCompletionTemplates", Verbs = "GET", Summary = "Returns account user's active completion email templates.", Notes = "Returns account user's active completion email templates.")]
    public class CompletionEmailTemplateRequest : IReturn<List<EmailTemplate>>
    {
    }
}