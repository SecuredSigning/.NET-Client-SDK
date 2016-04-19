using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SecuredSigningClientSdk.Helpers;

namespace SecuredSigningClientSdk
{
    public class OAuth2Client
    {
        protected readonly string AuthorizeEndpoint = "/api/oauth2/authorize";
        protected readonly string TokenEndpoint = "/api/oauth2/token";
        protected readonly string RevokeEndpoint = "/api/oauth2/revoke";
        protected readonly string ConsumerKey;
        protected readonly string ConsumerSecret;
        protected readonly string CallbackUrl;
        internal protected OAuth2Client(string host, string apiKey, string apiSecret, string accessUrl)
        {
            this.AuthorizeEndpoint = host + AuthorizeEndpoint;
            this.TokenEndpoint = host + TokenEndpoint;
            this.RevokeEndpoint = host + RevokeEndpoint;
            this.ConsumerKey = apiKey;
            this.ConsumerSecret = apiSecret;
            this.CallbackUrl = accessUrl;
        }
        internal class OAuth2TokenRequest
        {
            public const string GrantTypeAuthorizationCode = "authorization_code";
            public const string GrantTypeRefreshToken = "refresh_token";
            internal const string GrantTypeClientCredentials = "client_credentials";

            public OAuth2TokenRequest(string consumerKey, string consumerSecret, string callbackUrl, string grantType)
            {
                this.Grant_Type = grantType;
                this.Client_Id = consumerKey;
                this.Client_Secret = consumerSecret;
                this.Redirect_Uri = callbackUrl;
            }
            public string Grant_Type { get; set; }
            public string Client_Id { get; set; }
            public string Client_Secret { get; set; }
            public string Redirect_Uri { get; set; }
            public string Code { get; set; }
            public string Refresh_Token { get; set; }
            public string Client_Credential_Type { get; set; }
            public string Scope { get; set; }
            internal Dictionary<string, string> Client_Credential_Extra { get; } = new Dictionary<string, string>();
            public override string ToString()
            {
                var type = this.GetType();
                var properties = type.GetProperties();
                List<string> result = new List<string>();
                foreach (var p in properties)
                {
                    if (p.PropertyType == typeof(string))
                    {
                        if (p.GetValue(this,null) == null)
                            continue;
                        result.Add(string.Format("{0}={1}", p.Name.ToLower(), p.GetValue(this,null)));
                        continue;
                    }
                }
                var request = string.Join("&", result);
                if (Client_Credential_Extra.Any())
                {
                    request += $"&{string.Join("&", Client_Credential_Extra.Select(data => string.Format("{0}={1}", data.Key, data.Value)))}";
                }
                return request;

            }
        }
        /// <summary>
        /// OAuth2 Token Response
        /// </summary>
        public class OAuth2TokenResponse
        {
            [Newtonsoft.Json.JsonProperty(PropertyName = "access_token",NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Access_Token { get; set; }
            [Newtonsoft.Json.JsonProperty(PropertyName = "refresh_token", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Refresh_Token { get; set; }
            [Newtonsoft.Json.JsonProperty(PropertyName = "expires_in", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int Expires_In { get; set; }
            [Newtonsoft.Json.JsonProperty(PropertyName = "token_type", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Token_Type { get; set; }
            [Newtonsoft.Json.JsonProperty(PropertyName = "scope", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Scope { get; set; }
            [Newtonsoft.Json.JsonProperty(PropertyName = "error", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Error { get; set; }
        }
        /// <summary>
        /// OAuth2 Scopes
        /// </summary>
        [Flags]
        public enum OAuth2Scope
        {
            None = 0,
            [Description("Basic Profile - Fetch information about your Secured Signing account, such as your price plan, account status etc.")]
            Basic = 1,
            [Description("I Sign - Sign documents as a sole Signatory.")]
            ISign = 2,
            [Description("We Sign - Invite other people to sign documents.")]
            WeSign = 4,
            [Description("Smart Tag - Send documents with Smart Tags.")]
            SmartTag = 8,
            [Description("Form Direct - Fetch and send your online Form Direct forms.")]
            FormDirect = 16,
            [Description("Form Filler - Fill in online forms and sign.")]
            FormFiller = 32,
        }
        /// <summary>
        /// Create authorize URL
        /// </summary>
        /// <param name="state"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public string CreateAuthorizeRequest(string state, params string[] scopes)
        {
            return string.Format("{0}?response_type=code&client_id={1}&redirect_uri={2}&state={3}&scope={4}"
                , AuthorizeEndpoint, this.ConsumerKey, this.CallbackUrl, state,
                string.Join(" ", scopes));
        }
        public string CreateAuthorizeRequest(string state, OAuth2Scope scopes)
        {
            return CreateAuthorizeRequest(state, scopes.ToStringArray());
        }
        /// <summary>
        /// Get access token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OAuth2TokenResponse GetToken(string code)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var request = new OAuth2TokenRequest(ConsumerKey, ConsumerSecret, CallbackUrl, OAuth2TokenRequest.GrantTypeAuthorizationCode)
            {
                Code = code.Trim()
            }.ToString();
            var result = client.UploadString(TokenEndpoint, request);            
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OAuth2TokenResponse>(result);
        }
        /// <summary>
        /// Refresh access token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public OAuth2TokenResponse RefreshToken(string refreshToken)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var result = client.UploadString(TokenEndpoint, new OAuth2TokenRequest(ConsumerKey, ConsumerSecret, CallbackUrl, OAuth2TokenRequest.GrantTypeRefreshToken) { Refresh_Token = refreshToken }.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OAuth2TokenResponse>(result);
        }
        /// <summary>
        /// Revoke access token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public bool RevokeToken(string accessToken)
        {
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);
            var result = client.DownloadString(RevokeEndpoint);
            return true;
        }
        /// <summary>
        /// Handle authorization callback 
        /// </summary>
        /// <param name="callbackUrl"></param>
        /// <param name="state"></param>
        /// <returns>authorization code; if empty or null, means user refused authorization.</returns>
        public static string HandleAuthorizeCallback(Uri callbackUrl,out string state)
        {
            var queries = callbackUrl.Query.TrimStart('?').Split('&').Select(t => t.Split('='))
                .ToDictionary(t => t[0], t => t[1]);

            state = queries.ContainsKey("state") ? queries["state"] : string.Empty;
            state = WebUtility.UrlDecode(state);
            if (queries.ContainsKey("error"))
            {
                throw new Exception(queries["error"]);
            }
            if (queries.ContainsKey("code"))
            {
                return queries["code"];
            }
            return string.Empty;
        }
        public OAuth2TokenResponse GetClientAccessToken(string clientCredentialType, Dictionary<string,string> extraData, params string[] scopes)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var request = new OAuth2TokenRequest(ConsumerKey, ConsumerSecret, CallbackUrl, OAuth2TokenRequest.GrantTypeClientCredentials)
            {
                Client_Credential_Type = clientCredentialType,
                Scope = string.Join(" ", scopes)
            };
            foreach (var key in extraData.Keys)
            {
                request.Client_Credential_Extra.Add(key, extraData[key]);
            }
            var result = client.UploadString(TokenEndpoint, request.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OAuth2TokenResponse>(result);
        }
    }
}
