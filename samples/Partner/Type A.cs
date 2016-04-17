using PartnerSDK = SecuredSigningClientSdk.Partner;

namespace Test
{
    static partial class Sample
    {
        public static partial class Partner
        {
            public static PartnerSDK.ServiceClient InitializePartnerClient()
            {
                var client = new PartnerSDK.ServiceClient(
                    serviceUrl: SampleParameters.SecuredSigningServiceBase,
                    version: SampleParameters.SecuredSigningServiceVersion,
                    apiKey: SampleParameters.PartnerAPIKey,
                    secret: SampleParameters.PartnerAPISecret,
                    accessUrl: SampleParameters.PartnerAPICallback);
                return client;
            }
            public static void TypeASample(PartnerSDK.ServiceClient client)
            {
                var partnerToken = client.OAuth2.GetAccessTokenForPartner(PartnerSDK.OAuth2Client.PartnerApiFeature.MembershipManagement);
                client.AccessToken = partnerToken.Access_Token;
                var linkResp = client.linkMembership(SampleParameters.Partner_TypeA_Code, true, SampleParameters.Partner_TypeA_AuthenticateCode);
                var companyToken = client.OAuth2.GetAccessTokenForMembership(linkResp.MembershipCode, linkResp.Reference);
                client.AccessToken = companyToken.Access_Token;
                var addUserResp = client.addUserToMembership(new PartnerSDK.Models.UserDetails
                {
                    FirstName = SampleParameters.Partner_TypeA_User1_FirstName, //required
                    LastName = SampleParameters.Partner_TypeA_User1_LastName,  //required
                    Email = SampleParameters.Partner_TypeA_User1_Email,  //required
                    #region additional detail
                    JobTitle = "",
                    CompanyName = "",
                    LegalName = "",
                    Website = "",
                    Industry = "",
                    Employees = "",
                    Street = "",
                    Suburb = "",
                    City = "",
                    Postcode = "",
                    Country = "", //full country name, such as New Zealand, United States, not nz or us.
                    State = "",
                    PhoneCountry = "", //international phone code, such as 64 for New Zealand, 1 for United States.
                    PhoneArea = "",
                    PhoneNumber = "",
                    Title = ""
                    #endregion
                });
                var userToken = client.OAuth2.GetAccessTokenFromUserKey(addUserResp,SampleParameters.OAuth2State,SecuredSigningClientSdk.OAuth2Client.OAuth2Scope.Basic|SecuredSigningClientSdk.OAuth2Client.OAuth2Scope.SmartTag);
                client.AccessToken = userToken.Access_Token;
                var user = client.getAccountInfo();
                userToken = client.OAuth2.RefreshToken(userToken.Refresh_Token);
                // token for company,partner can only be used once, so get a new one.
                companyToken = client.OAuth2.GetAccessTokenForMembership(linkResp.MembershipCode, linkResp.Reference);
                client.AccessToken = companyToken.Access_Token;
                client.removeUserFromMembership(user.UserID);
            }
        }
    }
}
