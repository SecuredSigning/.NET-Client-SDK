using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SecuredSigningClientSdk.Helpers;

namespace SecuredSigningClientSdk.Partner
{
    public class OAuth2Client:SecuredSigningClientSdk.OAuth2Client
    {
        [Flags]
        public enum PartnerApiFeature
        {
            MembershipManagement,
            AccountManagement       
        }
        const string OAuth2Scope_AccountManagement = "Account";
        class CookieAwareWebClient : WebClient
        {
            OAuth2Client client;
            public CookieAwareWebClient(OAuth2Client client)
            {
                this.client = client;
            }
            private CookieContainer cc = new CookieContainer();
            public string LastPage { get; set; }

            protected override WebRequest GetWebRequest(System.Uri address)
            {
                WebRequest R = base.GetWebRequest(address);          
                if (R is HttpWebRequest)
                {
                    HttpWebRequest WR = R  as HttpWebRequest;
                    WR.CookieContainer = cc;
                    WR.AllowAutoRedirect = false;
                    if (LastPage != null)
                    {
                        WR.Referer = LastPage;
                    }
                }
                LastPage = address.ToString();
                return R;
            }
            protected override WebResponse GetWebResponse(WebRequest request)
            {
                LastPage = request.RequestUri.ToString();
                var resp = base.GetWebResponse(request) as HttpWebResponse;
                while(resp.StatusCode==HttpStatusCode.Found)
                {
                    LastPage = resp.Headers["Location"];
                    if (LastPage.StartsWith(client.CallbackUrl,StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                    resp = base.GetWebResponse(GetWebRequest(new Uri(LastPage))) as HttpWebResponse;
                }
                return resp;
            }
        }
        internal new class OAuth2TokenRequest: SecuredSigningClientSdk.OAuth2Client.OAuth2TokenRequest
        {
            public OAuth2TokenRequest(string consumerKey, string consumerSecret, string callbackUrl, string grantType)
            : base(consumerKey,  consumerSecret,  callbackUrl,  grantType) { }
            public const string GrantTypeClientCredentials = "client_credentials";
            public string Client_Credential_Type { get; set; }
            public string Scope { get; set; }
            public Dictionary<string, string> Client_Credential_Extra { get; } = new Dictionary<string, string>();
            public override string ToString()
            {
                var request= base.ToString();
                if(Client_Credential_Extra.Any())
                {
                    request += $"&{string.Join("&", Client_Credential_Extra.Select(data => string.Format("{0}={1}", data.Key, data.Value)))}";
                }
                return request;
            }
        }
        public OAuth2Client(string host, string apiKey, string apiSecret, string accessUrl)
            : base(host,  apiKey,  apiSecret,  accessUrl) {
        }
        public OAuth2TokenResponse GetAccessTokenForPartner(PartnerApiFeature feature)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var request = new OAuth2TokenRequest(ConsumerKey, ConsumerSecret, CallbackUrl, OAuth2TokenRequest.GrantTypeClientCredentials)
            {
                Client_Credential_Type= "special_feature"                
            };
            request.Client_Credential_Extra.Add("feature", feature.ToString());
            var result = client.UploadString(TokenEndpoint, request.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OAuth2TokenResponse>(result);
        }
        public OAuth2TokenResponse GetAccessTokenForMembership(string membershipCode,string membershipRefernece)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var request = new OAuth2TokenRequest(ConsumerKey, ConsumerSecret, CallbackUrl, OAuth2TokenRequest.GrantTypeClientCredentials)
            {
                Client_Credential_Type = "membership_authentication"
            };
            request.Client_Credential_Extra.Add("membership_code", membershipCode);
            request.Client_Credential_Extra.Add("membership_reference", membershipRefernece);
            var result = client.UploadString(TokenEndpoint, request.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OAuth2TokenResponse>(result);
        }
        public OAuth2TokenResponse GetAccessTokenForInvitee(string firstname,string lastname,string email,params string[] scopes)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var request = new OAuth2TokenRequest(ConsumerKey, ConsumerSecret, CallbackUrl, OAuth2TokenRequest.GrantTypeClientCredentials)
            {
                Client_Credential_Type = "user_profile",
                Scope = string.Join(" ", scopes)
            };
            request.Client_Credential_Extra.Add("firstname", firstname);
            request.Client_Credential_Extra.Add("lastname", lastname);
            request.Client_Credential_Extra.Add("email", email);
            var result = client.UploadString(TokenEndpoint, request.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OAuth2TokenResponse>(result);
        }
        public OAuth2TokenResponse GetAccessTokenForInvitee(string firstname, string lastname, string email, OAuth2Scope scopes)
        {
            return GetAccessTokenForInvitee(firstname, lastname, email, scopes.ToStringArray());
        }
        string CreateAuthorizeRequest(string userKey, string state, OAuth2Scope scopes)
        {
            return $"{base.CreateAuthorizeRequest(state, scopes)}&key={userKey}";
        }
        string CreateAuthorizeRequest(string userKey, string state, params string[] scopes)
        {
            return $"{base.CreateAuthorizeRequest(state, scopes)}&key={userKey}";
        }
        public string CreateAuthorizeRequest(string state,bool isAdmin, OAuth2Scope scopes)
        {
            return CreateAuthorizeRequest(state, isAdmin, scopes.ToStringArray());
        }
        public string CreateAuthorizeRequest(string state, bool isAdmin, params string[] scopes)
        {
            if(isAdmin&&!scopes.Any(t=>t.Equals(OAuth2Scope_AccountManagement, StringComparison.OrdinalIgnoreCase)))
            {
                var scopeList = scopes.ToList();
                scopeList.Add(OAuth2Scope_AccountManagement);
                scopes = scopeList.ToArray();
            }
            return base.CreateAuthorizeRequest(state, scopes);
        }

        public OAuth2TokenResponse GetAccessTokenFromUserKey(string userKey,string state,params string[] scopes)
        {
            var request = CreateAuthorizeRequest(userKey, state, scopes);
            CookieAwareWebClient client = new CookieAwareWebClient(this);
            var result = client.DownloadString(request);
            var callback = new Uri(client.LastPage);
            string stateInResponse = string.Empty;
            var code = HandleAuthorizeCallback(callback, out stateInResponse);
            System.Diagnostics.Debug.Assert(stateInResponse == state);
            return base.GetToken(code);
        }
        public OAuth2TokenResponse GetAccessTokenFromUserKey(string userKey, string state, OAuth2Scope scopes)
        {
            return GetAccessTokenFromUserKey(userKey, state, scopes.ToStringArray());
        }
        public OAuth2TokenResponse GetAccessTokenForAccountManagement(string accountKey,string state)
        {
            return GetAccessTokenFromUserKey(accountKey, state, OAuth2Scope_AccountManagement);
        }
    }
}
