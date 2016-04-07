using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SecuredSigningClientSdk
{
    public class OAuth2Client
    {
        readonly string AuthorizeEndpoint = "/api/oauth2/authorize";
        readonly string TokenEndpoint = "/api/oauth2/token";
        readonly string RevokeEndpoint = "/api/oauth2/revoke";
        readonly string ConsumerKey;
        readonly string ConsumerSecret;
        readonly string CallbackUrl;
        internal OAuth2Client(string host, string apiKey, string apiSecret, string accessUrl)
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
            public override string ToString()
            {
                var type = this.GetType();
                var properties = type.GetProperties();
                List<string> result = new List<string>();
                foreach (var p in properties)
                {
                    if (p.PropertyType == typeof(string))
                    {
                        if (p.GetValue(this) == null)
                            continue;
                        result.Add(string.Format("{0}={1}", p.Name.ToLower(), p.GetValue(this)));
                        continue;
                    }
                }
                return string.Join("&", result);
            }
        }
        /// <summary>
        /// OAuth2 Token Response
        /// </summary>
        public class OAuth2TokenResponse
        {
            public string Access_Token { get; set; }
            public string Refresh_Token { get; set; }
            public int Expires_In { get; set; }
            public string Token_Type { get; set; }
            public string Scope { get; set; }
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
            return CreateAuthorizeRequest(state, scopes.ToString("F").Split(',').Select(t => t.Trim()).ToArray());
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
        /// <param name="httpContext"></param>
        /// <param name="state"></param>
        /// <returns>authorization code; if empty or null, means user refused authorization.</returns>
        public static string HandleAuthorizeCallback(System.Web.HttpContextBase httpContext,out string state)
        {
            var uri = httpContext.Request.Url;
            var queries = uri.Query.TrimStart('?').Split('&').Select(t => t.Split('='))
                .ToDictionary(t => t[0], t => t[1]);

            state = queries.ContainsKey("state") ? queries["state"] : string.Empty;

            if (queries.ContainsKey("error"))
            {                
                return string.Empty;
            }
            if (queries.ContainsKey("code"))
            {
                return queries["code"];
            }
            return string.Empty;
        }
    }
}
