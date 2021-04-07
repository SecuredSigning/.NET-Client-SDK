using ServiceStack;
using System;
using SecuredSigningClientSdk.Partner.Models;

namespace SecuredSigningClientSdk.Partner.Requests
{
    [Route("/Account/Membership", Verbs = "POST")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class MembershipRequest : IReturn<MembershipResponse>
    {
        public bool TermsOfUse { get; set; }
        public Company Company { get; set; }
        public string MembershipCode { get; set; }
        public string MembershipAuthenticationCode { get; set; }
        public MembershipUserAuthenticationOptions UserAuthenticationOptions { get; set; }
        public string ClientReference { get; set; }
    }

    [Route("/Account/AddMembershipUser", Verbs = "POST")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class AddMembershipUserRequest : IReturn<string>
    {
        public UserDetails User { get; set; }
        public string ClientReference { get; set; }
    }
    [Route("/Account/UpdateMembershipUser", Verbs = "POST")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class UpdateMembershipUserDetailRequest : IReturn
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string ClientReference { get; set; }
    }
    [Route("/Account/RemoveMembershipUser", Verbs = "POST")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class RemoveMembershipUserRequest : IReturn
    {
        public string UserID { get; set; }
        public bool ForceRemove { get; set; }
        public string TransferTo { get; set; }

    }
    [Route("/Account/MembershipPlan", Verbs = "GET")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class MembershipPlanRequest : IReturn<PlanResponse>
    {

    }

    [Route("/Account/AddAccount", Verbs = "POST")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class AddAccountRequest : IReturn<Models.AddAccountUserResponse>
    {
        public UserDetails User { get; set; }
        public PlanDetails Plan { get; set; }
        public string GMT { get; set; }
    }
    [Route("/Account/AddUser", Verbs = "POST")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class AddUserRequest : IReturn<Models.AddAccountUserResponse>
    {
        public UserDetails User { get; set; }
        public string GMT { get; set; }
    }
    [Route("/Account/RemoveUser", Verbs = "POST")]
    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class RemoveUserRequest : IReturn
    {
        public string UserID { get; set; }
    }
}
