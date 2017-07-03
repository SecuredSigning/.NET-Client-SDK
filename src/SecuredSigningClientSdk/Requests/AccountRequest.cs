using SecuredSigningClientSdk.Models;
using ServiceStack;
using System.Collections.Generic;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Account/Info", Verbs = "GET", Summary = "Get account information", Notes = "Get account information.")]
    public class AccountRequest : IReturn<AccountInfo>
    {
    }
    [Route("/Account/SaveSender", Verbs = "POST")]
    public class SaveSenderRequest : IReturn<UserReferenceResponse>
    {
        [ApiMember(Description = "User details", DataType = "UserDetails", IsRequired = true)]
        public UserDetails User { get; set; }
        [ApiMember(Name = "GMT", Description = "GMT Offset", DataType = SwaggerType.Int, IsRequired = false)]
        public string GMT { get; set; }
        [ApiMember(Name = "ClientReference", Description = "The reference/id in client's side", DataType = SwaggerType.String, IsRequired = false)]
        public string ClientReference { get; set; }
    }

    [Route("/Account/ShareUsers", Verbs = "GET")]
    public class ShareUsersRequest : IReturn<List<ShareUser>>
    {
    }
}