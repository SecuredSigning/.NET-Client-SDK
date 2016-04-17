using System;
using PartnerSDK = SecuredSigningClientSdk.Partner;

namespace Test
{
    static partial class Sample
    {
        public static partial class Partner
        {
            public static void TypeBSample(PartnerSDK.ServiceClient client)
            {
                var partnerToken = client.OAuth2.GetAccessTokenForPartner(PartnerSDK.OAuth2Client.PartnerApiFeature.AccountManagement);
                client.AccessToken = partnerToken.Access_Token;
                var createResp = client.createAccount(new PartnerSDK.Models.PlanDetails
                {
                    PlanUsers = 5,
                    PlanDocuments = 50
                }, new PartnerSDK.Models.UserDetails
                {
                    FirstName = SampleParameters.Partner_TypeB_Admin1_FirstName, //required
                    LastName = SampleParameters.Partner_TypeB_Admin1_LastName,  //required
                    Email = SampleParameters.Partner_TypeB_Admin1_Email,  //required
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
                if (createResp.Result != "OK")
                    return;
                //the account admin user need to setup credit card to continue
                Console.ReadLine();
                var companyToken = client.OAuth2.GetAccessTokenForAccountManagement(createResp.ConnectKey, SampleParameters.OAuth2State);
                client.AccessToken = companyToken.Access_Token;
                var addUserResp = client.addUserToAccount(new PartnerSDK.Models.UserDetails
                {
                    FirstName = SampleParameters.Partner_TypeB_User1_FirstName, //required
                    LastName = SampleParameters.Partner_TypeB_User1_LastName,  //required
                    Email = SampleParameters.Partner_TypeB_User1_Email,  //required
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
                var userToken = client.OAuth2.GetAccessTokenFromUserKey(addUserResp.ConnectKey, SampleParameters.OAuth2State, SecuredSigningClientSdk.OAuth2Client.OAuth2Scope.Basic | SecuredSigningClientSdk.OAuth2Client.OAuth2Scope.SmartTag);
                client.AccessToken = userToken.Access_Token;
                var user = client.getAccountInfo();
                userToken = client.OAuth2.RefreshToken(userToken.Refresh_Token);
                // connectKey for company can only be used once, so you can refesh it.
                companyToken = client.OAuth2.RefreshToken(companyToken.Refresh_Token);
                client.AccessToken = companyToken.Access_Token;
                client.removeUserFromAccount(user.UserID);
            }

        }
    }
}
