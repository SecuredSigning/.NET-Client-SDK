using SecuredSigningClientSdk.Models;
using ServiceStack;
using System.Collections.Generic;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Recipient/GetRecipients", Verbs = "GET", Summary = "Returns account user's recipients", Notes = "Returns an array of all notification and completion recipients")]
    [Route("/Recipient/Recipients", Verbs = "GET", Summary = "Returns account user's recipients", Notes = "Returns an array of all notification and completion recipients")]
    public class GetRecipientsRequest : IReturn<List<RecipientsResponse>>
    {

    }
}