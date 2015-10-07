using SecuredSigningClientSdk.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/EmailTemplate/GetInvitationTemplates", Verbs = "GET", Summary = "Returns account user's active email templates.", Notes = "Returns account user's active email templates.")]
    public class EmailTemplateRequest : IReturn<List<EmailTemplate>>
    {
    }
}