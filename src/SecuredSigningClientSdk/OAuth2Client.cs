using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SecuredSigningClientSdk.Helpers;

namespace SecuredSigningClientSdk
{
    /// <summary>
    /// Secured Signing OAuth 2.0 Client
    /// </summary>
    public class OAuth2Client
    {
        /// <summary>
        /// Authorize Endpoint
        /// </summary>
        protected readonly string AuthorizeEndpoint = "/api/oauth2/authorize";
        /// <summary>
        /// Token Endpoint
        /// </summary>
        protected readonly string TokenEndpoint = "/api/oauth2/token";
        /// <summary>
        /// Revoke Endpoint
        /// </summary>
        protected readonly string RevokeEndpoint = "/api/oauth2/revoke";
        /// <summary>
        /// Consumer Key / API Key
        /// </summary>
        protected readonly string ConsumerKey;
        /// <summary>
        /// Consumer Secret / API Secret
        /// </summary>
        protected readonly string ConsumerSecret;
        /// <summary>
        /// Callback URL / Access URL
        /// </summary>
        protected readonly string CallbackUrl;
        /// <summary>
        /// Secured Signing OAuth 2.0 Client
        /// </summary>
        /// <param name="host"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="accessUrl"></param>
        internal protected OAuth2Client(string host, string apiKey, string apiSecret, string accessUrl)
        {
            this.AuthorizeEndpoint = host + AuthorizeEndpoint;
            this.TokenEndpoint = host + TokenEndpoint;
            this.RevokeEndpoint = host + RevokeEndpoint;
            this.ConsumerKey = apiKey;
            this.ConsumerSecret = apiSecret;
            this.CallbackUrl = accessUrl;
        }
        public class OAuth2TokenRequest
        {
            public const string GrantTypeAuthorizationCode = "authorization_code";
            public const string GrantTypeRefreshToken = "refresh_token";
            public const string GrantTypeClientCredentials = "client_credentials";

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
            public Dictionary<string, string> Client_Credential_Extra { get; } = new Dictionary<string, string>();
            public override string ToString()
            {
                var type = this.GetType();
                var properties = type.GetProperties();
                List<string> result = new List<string>();
                foreach (var p in properties)
                {
                    if (p.PropertyType == typeof(string))
                    {
                        if (p.GetValue(this, null) == null)
                            continue;
                        result.Add(string.Format("{0}={1}", p.Name.ToLower(), p.GetValue(this, null)));
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
            /// <summary>
            /// Access Token
            /// </summary>
            [Newtonsoft.Json.JsonProperty(PropertyName = "access_token", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Access_Token { get; set; }
            /// <summary>
            /// Refresh Token
            /// </summary>
            [Newtonsoft.Json.JsonProperty(PropertyName = "refresh_token", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Refresh_Token { get; set; }
            /// <summary>
            /// Seconds Expires In 
            /// </summary>
            [Newtonsoft.Json.JsonProperty(PropertyName = "expires_in", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int Expires_In { get; set; }
            /// <summary>
            /// Access Token Type
            /// </summary>
            [Newtonsoft.Json.JsonProperty(PropertyName = "token_type", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Token_Type { get; set; }
            /// <summary>
            /// Scopes associatated with this token; It may be less than requested
            /// </summary>
            [Newtonsoft.Json.JsonProperty(PropertyName = "scope", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Scope { get; set; }
            /// <summary>
            /// Error if failed to issue a token.
            /// </summary>
            [Newtonsoft.Json.JsonProperty(PropertyName = "error", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Error { get; set; }
        }
        /// <summary>
        /// OAuth2 Scopes
        /// </summary>
        [Flags]
        public enum OAuth2Scope
        {
            /// <summary>
            /// Don't use this one
            /// </summary>
            None = 0,
            /// <summary>
            /// Basic Profile - Fetch information about your Secured Signing account, such as your price plan, account status etc.
            /// </summary>
            [Description("Basic Profile - Fetch information about your Secured Signing account, such as your price plan, account status etc.")]
            Basic = 1,
            /// <summary>
            /// I Sign - Sign documents as a sole Signatory.
            /// </summary>
            [Description("I Sign - Sign documents as a sole Signatory.")]
            ISign = 2,
            /// <summary>
            /// We Sign - Invite other people to sign documents.
            /// </summary>
            [Description("We Sign - Invite other people to sign documents.")]
            WeSign = 4,
            /// <summary>
            /// Smart Tag - Send documents with Smart Tags."
            /// </summary>
            [Description("Smart Tag - Send documents with Smart Tags.")]
            SmartTag = 8,
            /// <summary>
            /// Form Direct - Fetch and send your online Form Direct forms.
            /// </summary>
            [Description("Form Direct - Fetch and send your online Form Direct forms.")]
            FormDirect = 16,
            /// <summary>
            /// Form Filler - Fill in online forms and sign.
            /// </summary>
            [Description("Form Filler - Fill in online forms and sign.")]
            FormFiller = 32,
            /// <summary>
            /// Account Management - Manage account settings.
            /// </summary>
            [Description("Account Management - Manage account settings.")]
            Account = 64,
            /// <summary>
            /// Billing Management - Manage account invoices and details.
            /// </summary>
            [Description("Billing Management - Manage account invoices and details.")]
            Billing = 128,

        }
        /// <summary>
        /// Create authorize URL
        /// </summary>
        /// <param name="state"></param>
        /// <param name="scopes"></param>
        /// <returns>Authorize URL</returns>
        public string CreateAuthorizeRequest(string state, params string[] scopes)
        {
            return string.Format("{0}?response_type=code&client_id={1}&redirect_uri={2}&state={3}&scope={4}"
                , AuthorizeEndpoint, this.ConsumerKey, this.CallbackUrl, state,
                string.Join(" ", scopes));
        }
        /// <summary>
        /// Create authorize URL
        /// </summary>
        /// <param name="state"></param>
        /// <param name="scopes"></param>
        /// <returns>Authorize URL</returns>
        public string CreateAuthorizeRequest(string state, OAuth2Scope scopes)
        {
            return CreateAuthorizeRequest(state, scopes.ToStringArray());
        }
        /// <summary>
        /// Get access token in Authorization Code Flow
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
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<OAuth2TokenResponse>(result);
            if (response != null && string.IsNullOrEmpty(response?.Refresh_Token))
            {
                response.Refresh_Token = refreshToken;
            }
            return response;
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
        public static string HandleAuthorizeCallback(Uri callbackUrl, out string state)
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
        /// <summary>
        /// Get access token in Implicit Flow
        /// </summary>
        /// <param name="clientCredentialType"></param>
        /// <param name="extraData"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public OAuth2TokenResponse GetClientAccessToken(string clientCredentialType, Dictionary<string, string> extraData, params string[] scopes)
        {
            HttpWebRequest req = WebRequest.CreateHttp(TokenEndpoint);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            req.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            using (var rs = req.GetRequestStream())
            {
                var request = new OAuth2TokenRequest(ConsumerKey, ConsumerSecret, CallbackUrl, OAuth2TokenRequest.GrantTypeClientCredentials)
                {
                    Client_Credential_Type = clientCredentialType,
                    Scope = string.Join(" ", scopes)
                };
                foreach (var key in extraData.Keys)
                {
                    request.Client_Credential_Extra.Add(key, extraData[key]);
                }
                var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(request.ToString());
                rs.Write(bytes, 0, bytes.Length);
            }
            var resp = req.GetResponse();
            using (var respStream = new System.IO.StreamReader(resp.GetResponseStream()))
            {
                var result = respStream.ReadToEnd();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<OAuth2TokenResponse>(result);
            }

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore)
                {

                };
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
}