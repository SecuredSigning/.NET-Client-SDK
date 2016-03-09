using SecuredSigningClientSdk.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Account/Info", Verbs = "GET", Summary = "Get account information", Notes = "Get account information.")]
    public class AccountRequest : IReturn<AccountInfo>
    {

    }

}
