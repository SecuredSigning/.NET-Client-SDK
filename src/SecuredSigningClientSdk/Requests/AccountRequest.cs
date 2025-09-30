using SecuredSigningClientSdk.Models;
using ServiceStack;
using System.Collections.Generic;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Account/Info", Verbs = "GET", Summary = "Get account information", Notes = "Get account information.")]
    public class AccountRequest : IReturn<AccountInfo>
    {
    }
    [Route("/Account/NotaryCheck", Verbs = "POST", Summary = "Check if user has notary setup in the account", Notes = "Check if user has notary setup in the account.")]
    public class NotaryCheckRequest : IReturn<NotaryCheck>
    {
        [ApiMember(Description = "First Name", DataType = SwaggerType.String, IsRequired = true)]
        public string FirstName { get; set; }
        [ApiMember(Description = "Middle Name", DataType = SwaggerType.String, IsRequired = false)]
        public string MiddleName { get; set; }
        [ApiMember(Description = "Last Name", DataType = SwaggerType.String,  IsRequired = true)]
        public string LastName { get; set; }
        [ApiMember(Description = "Email", DataType = SwaggerType.String, IsRequired = true)]
        public string Email { get; set; }
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