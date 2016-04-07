using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test.WithoutSDK
{
    public class SecuredSigningRestAPI
    {
        private WebClient _client;
        private string accessToken;
        private OAuth2Client _oauth2;
        public OAuth2Client OAuth2 { get { return _oauth2; } }

        /// <summary>
        /// Set Access Token
        /// </summary>
        public string AccessToken
        {
            set { accessToken = value; }
        }
        /// <summary>
        /// OAuth 2 Client to deal with authentication.
        /// </summary>
        private string GMT
        {
            get
            {
                return TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes.ToString("F0");
            }
        }
        void Setup(WebClient client)
        {
            //create unix time stamp string
            var requestDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();

            //create nonce
            var nonce = KeyGenerator.GetUniqueKey(32);
            client.Headers.Clear();
            client.Headers.Add("X-CUSTOM-API-KEY", APIKey);
            client.Headers.Add("X-CUSTOM-DATE", requestDate);
            client.Headers.Add("X-CUSTOM-NONCE", nonce);
            client.Headers.Add("X-CUSTOM-SIGNATURE", AuthHelper.CreateSignature(APIKey, APISecret, requestDate, nonce));
            if (!string.IsNullOrEmpty(accessToken))
                client.Headers.Add(System.Net.HttpRequestHeader.Authorization, "Bearer " + accessToken);
            else
            {
                client.Headers.Add("Referer", AccessUrl);
            }
        }
        readonly string APIKey;
        readonly string APISecret;
        readonly string AccessUrl;
        public SecuredSigningRestAPI(string serviceUrl, string version, string apiKey, string secret, string accessUrl)
        {
            this.APIKey = apiKey;
            this.APISecret = secret;
            this.AccessUrl = accessToken;
            _client = new WebClient();
            _oauth2 = new OAuth2Client(new Uri(serviceUrl.Replace("api", "www")).GetLeftPart(UriPartial.Authority), apiKey, secret, accessUrl);
            _client.BaseAddress = serviceUrl + "/" + version;
        }

        #region Account
        /// <summary>
        /// Get account information
        /// </summary>
        /// <returns></returns>
        public AccountInfo getAccountInfo()
        {
            Setup(_client);
            var result= _client.DownloadString(_client.BaseAddress+"/Account/Info");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AccountInfo>(result);
        }
        public List<Document> getActiveDocuments(string folder)
        {
            Setup(_client);
            var result = _client.DownloadString(_client.BaseAddress + "/Document/GetActiveDocuments/" +folder);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Document>>(result);
        }    
        public Document uploadDocumentFile(string file)
        {
            Setup(_client);
            var result = _client.UploadFile(_client.BaseAddress + "/Document/Uploader ",file);
            var reference=System.Text.Encoding.UTF8.GetString(result);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(reference);
        }

        #endregion

    }
    #region Models
    public class AccountInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int DocumentRemain { get; set; }
        public string PlanName { get; set; }
        public int DocumentUsed { get; set; }
        public int DocumentLeft { get; set; }
        public string PlanType { get; set; }
        public int DefaultDueDate { get; set; }
        public int MaxDueDate { get; set; }
        public string DateFormat { get; set; }
        public string Upgrade { get; set; }
        public string UserID { get; set; }
        public long MaxUploadSize { get; set; }
        public bool Actived { get; set; }
        public bool Locked { get; set; }
        public string AccountStatus { get; set; }
    }
    public class Document
    {
        public string Name { get; set; }

        public string Reference { get; set; }

        public string FileType { get; set; }

        public string FormDirectReference { get; set; }

        public List<Signer> Signers { get; set; }

        public string Status { get; set; }

        public string ServiceType { get; set; }

        public string DocumentUrl { get; set; }

        public string DueDate { get; set; }

        public string GMT { get; set; }

        public string LastSignedDate { get; set; }
    }
    public class Signer : Invitee
    {
        public string SignerReference { get; set; }

        public string SigningKey { get; set; }

        public bool HasSigned { get; set; }
    }
    public class Invitee
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string MobileNumber { get; set; }

        public string MobileCountry { get; set; }
    }
    #endregion
}
