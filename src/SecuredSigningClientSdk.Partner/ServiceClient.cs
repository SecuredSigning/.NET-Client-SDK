using SecuredSigningClientSdk.Partner.Models;
using SecuredSigningClientSdk.Partner.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SecuredSigningClientSdk.Partner
{
    public class ServiceClient : SecuredSigningClientSdk.ServiceClient
    {
        public ServiceClient(string serviceUrl, string version, string apiKey, string secret, string accessUrl)
            : base(serviceUrl, version, apiKey, secret, accessUrl)
        {
            _oauth2 = new OAuth2Client(new Uri(serviceUrl.Replace("api", "www")).GetLeftPart(UriPartial.Authority), apiKey, secret, accessUrl);
        }
        public new OAuth2Client OAuth2 {
            get
            {
                return _oauth2 as OAuth2Client;
            }
        }
        #region Membership (Type A)
        /// <summary>
        /// Create a new type A company account,
        /// </summary>
        /// <param name="details"></param>
        /// <param name="termsOfUse"></param>
        /// <returns></returns>
        [Obsolete("Use the method with options instead")]
        public MembershipResponse createMembership(Company details,bool termsOfUse)
        {
            return _client.Post(new MembershipRequest
            {
                Company=details,
                TermsOfUse=termsOfUse
            });
        }
        /// <summary>
        /// Create a new type A company account
        /// </summary>
        /// <param name="details"></param>
        /// <param name="termsOfUse"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public MembershipResponse createMembership(Company details, bool termsOfUse,MembershipOptions options)
        {
            return _client.Post(new MembershipRequest
            {
                Company = details,
                TermsOfUse = termsOfUse,
                UserAuthenticationOptions = options?.UserAuthenticationOptions,
                ClientReference=options?.ClientReference
            });
        }
        /// <summary>
        /// Link to a existing type A company account
        /// </summary>
        /// <param name="membershipCode"></param>
        /// <param name="termsOfUse"></param>
        /// <param name="membershipAuthenticationCode"></param>
        /// <returns></returns>
        public MembershipResponse linkMembership(string membershipCode, bool termsOfUse,string membershipAuthenticationCode="")
        {
            return _client.Post(new MembershipRequest
            {
                MembershipCode=membershipCode,
                MembershipAuthenticationCode=membershipAuthenticationCode,
                TermsOfUse = termsOfUse
            });
        }
        /// <summary>
        /// add a new user to type A company account
        /// </summary>
        /// <param name="userDetails"></param>
        /// <param name="clientReference">a unique and invariable key represents the user on partner side, such as UserID in partner system.</param>
        /// <returns>If partner wants users to active and connnect by themself, empty string returns and user will receive an email to active and connect to partner;</returns>
        public string addUserToMembership(UserDetails userDetails, string clientReference="")
        {
            return _client.Post(new AddMembershipUserRequest
            {
                User= userDetails,
                ClientReference=clientReference
            });
        }
        /// <summary>
        /// update user details in type A company account
        /// </summary>
        /// <param name="userDetails"></param>
        /// <param name="clientRefernece"></param>
        public void updateUserInMembership(UserDetails userDetails,string clientRefernece = "")
        {
            _client.Post(new UpdateMembershipUserDetailRequest
            {
                ClientReference=clientRefernece,
                Email=userDetails.Email,
                FirstName=userDetails.FirstName,
                LastName=userDetails.LastName,
                JobTitle=userDetails.JobTitle
            });
        }
        /// <summary>
        /// remove a user from type A company account
        /// </summary>
        /// <param name="userReference">the UserID in get account/info response</param>
        public void removeUserFromMembership(string userReference,string transferTo=null,bool forceRemove=false)
        {
            _client.Post(new RemoveMembershipUserRequest
            {
                UserID = userReference,
                TransferTo=transferTo,
                ForceRemove=forceRemove
            });
        }
        /// <summary>
        /// get the price plan for type A company account
        /// </summary>
        /// <returns></returns>
        public PlanResponse getMembershipPlan()
        {
            return _client.Get(new MembershipPlanRequest());
        }
        #endregion
        #region Account (Type B)
        /// <summary>
        /// Create a new type B company account
        /// </summary>
        /// <param name="accountPlan"></param>
        /// <param name="defaultAccountAdminUser"></param>
        /// <returns></returns>
        public AddAccountUserResponse createAccount(PlanDetails accountPlan, UserDetails defaultAccountAdminUser)
        {
            return _client.Post(new AddAccountRequest
            {
                Plan = accountPlan,
                User = defaultAccountAdminUser,
                GMT = this.GMT
            });
        }
        /// <summary>
        /// Add a new user to type B company account
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        public AddAccountUserResponse addUserToAccount(UserDetails userDetails)
        {
            return _client.Post(new AddUserRequest
            {
                User = userDetails,
                GMT = base.GMT
            });
        }
        /// <summary>
        /// remove a user from type B company account
        /// </summary>
        /// <param name="userReference"></param>
        public void removeUserFromAccount(string userReference)
        {
            _client.Post(new RemoveUserRequest
            {
                UserID = userReference
            });
        }
        #endregion
    }
}
