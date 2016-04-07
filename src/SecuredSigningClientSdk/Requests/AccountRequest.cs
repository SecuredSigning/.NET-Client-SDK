using SecuredSigningClientSdk.Models;
using ServiceStack;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Account/Info", Verbs = "GET", Summary = "Get account information", Notes = "Get account information.")]
    public class AccountRequest : IReturn<AccountInfo>
    {

    }

}
